/*
 * Copyright (C) 2015 by Laurentiu-Gheorghe Crisan
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * As a special exception, you may use this file as part of a free
 * software library without restriction. Specifically, if other files
 * instantiate templates or use macros or inline functions from this
 * file, or you compile this file and link it with other files to
 * produce an executable, this file does not by itself cause the
 * resulting executable to be covered by the GNU General Public
 * License. This exception does not however invalidate any other
 * reasons why the executable file might be covered by the GNU Library
 * General Public License.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
#include <mps/diatp/TPCANHandler.h>
#include "TPSingleFrame.h"
#include "TPConsecutiveFrame.h"
#include "TPFirstFrame.h"
#include "TPFlowControlFrame.h"
#include <cmath>

namespace mps{
namespace diatp{


TPCANHandler::TPCANHandler(CANHandler& canHandler)
: _running(false)
, _canHandler(canHandler)
, _pdus()
, _thread(0)
, _mutex()
, _sidsToWait()
{
}


TPCANHandler::~TPCANHandler()
{
    stop();
}


void TPCANHandler::send(const TPDU& requestPdu)
{
    Pt::System::MutexLock lock(_mutex);

    if(TPSingleFrame::fitInSingleFrame(requestPdu.address(), requestPdu.size()))
    {//Single frame		
        TPSingleFrame frame(requestPdu.address(), requestPdu.data(), requestPdu.size());
        _canHandler.send(frame);
        return;
    }
    
    //Multiple frames, calculate blocks to send.	
    size_t blocksToSend = 0;
    size_t dataSended = 0;	

    if(requestPdu.address().isExtAdr())
    {
        dataSended = 5;
        size_t blocksToSend = requestPdu.size() - 5;
        blocksToSend = static_cast<size_t>( std::ceil((double)blocksToSend/6.0) );
    }
    else
    {
        dataSended = 6;
        size_t blocksToSend = requestPdu.size() - 6;
        blocksToSend = static_cast<size_t>( std::ceil((double)blocksToSend/7.0) );
    }

    const Pt::uint8_t* data = requestPdu.data();
    
    //Send the first frame
    TPFirstFrame firstFrame(requestPdu.address(), requestPdu.data(), requestPdu.size());
    
    _canHandler.send(firstFrame);
        
    mps::can::drv::Message rsvMsg;
    TPDUAddress targetAddress = requestPdu.address().getComplementAddress();

    if(!_canHandler.waitForID(targetAddress.identifier(),500))
         throw TPException("Receive flow control timeout", TPException::ReceiveTimeoutError);

    if(!_canHandler.readMessageByID(targetAddress.identifier(),rsvMsg))
        throw TPException("Receive flow control timeout", TPException::ReceiveTimeoutError);

    TPDUAddress address(rsvMsg.identifier(), requestPdu.address().isExtendedID(), rsvMsg.data()[0]);

    if(!TPFlowControlFrame::isFlowControlFrame(address ,rsvMsg.data()))
        throw TPException("Flow control frame expected", TPException::WrongFrameTypeReceived);

    TPFlowControlFrame flowCtrl(rsvMsg, requestPdu.address().isExtendedID());
    
    //Send the consecutive frames
    for(size_t i = 0; i < blocksToSend; ++i)
    {
        if( flowCtrl.flowStatus() == TPFlowControlFrame::Overflow)
            throw TPException("The receiver indicate a buffer overflow", TPException::ReceiverBufferOverflow);

        if(flowCtrl.flowStatus() == TPFlowControlFrame::ContinueToSend)
        {//Ready we can send the blocks
            
            //Calculate the block size
            size_t blockSize = flowCtrl.blockSize();
            
            if( flowCtrl.blockSize() ==  0)
                blockSize = blocksToSend - i;
            else
                blockSize = std::min((size_t)flowCtrl.blockSize(),((size_t) blocksToSend-i));
            
            int sequenceNo = 1;

            //Send the block
            for( size_t block = 0; block < blockSize; ++ block)
            {
                size_t curDataSize =  std::min((size_t) 7,(size_t) (requestPdu.size() - dataSended));
                TPConsecutiveFrame consecutiveFrame(requestPdu.address(),&data[dataSended], curDataSize,sequenceNo%16); 
                _canHandler.send(consecutiveFrame);
                 
                if(flowCtrl.timeUnit() == TimeUnit::MilliSec)
                    Pt::System::Thread::sleep(flowCtrl.separationTime());
                else
                    Pt::System::Thread::sleep(flowCtrl.separationTime()/1000);

                dataSended += curDataSize;
                sequenceNo++;
            }

            i += blockSize;
        }

        //Read the next flow control
        if( i < blocksToSend)
        {
            if(!_canHandler.waitForID(targetAddress.identifier(),500))
                throw TPException("Receive flow control timeout", TPException::ReceiveTimeoutError);

            if(!_canHandler.readMessageByID(targetAddress.identifier(),rsvMsg))
                throw TPException("Receive flow control timeout", TPException::ReceiveTimeoutError);
            
            TPDUAddress address(rsvMsg.identifier(), requestPdu.address().isExtendedID(), rsvMsg.data()[0]);

            if(!TPFlowControlFrame::isFlowControlFrame(address,rsvMsg.data()))
                throw TPException("Flow control frame expected", TPException::WrongFrameTypeReceived);

            flowCtrl = TPFlowControlFrame(rsvMsg, requestPdu.address().isExtendedID());
        }
    }
}


bool TPCANHandler::waitForSID(Pt::uint8_t sid, size_t timeout)
{
    Pt::System::MutexLock lock(_mutex);

    for( size_t i = 0; i < _pdus.size(); ++i)
    {
        if(_pdus[i].data()[0] == sid)
            return true;
    }
        
    Pt::System::Condition  condition;
    
    std::pair<Pt::uint8_t, Pt::System::Condition*> pair(sid, &condition);
    
    _sidsToWait.insert(pair);	
    
    bool retVal = condition.wait(_mutex, timeout);
    
    _sidsToWait.erase(sid);

    return retVal;
}


void TPCANHandler::reset(bool hardware)
{
    Pt::System::MutexLock lock(_mutex);

    _pdus.clear();
    _canHandler.reset(hardware);
}


bool TPCANHandler::readSID(Pt::uint8_t sid, TPDU& pdu)
{
    Pt::System::MutexLock lock(_mutex);

    for( size_t i = 0; i < _pdus.size(); ++i)
    {
        const TPDU& p = _pdus[i];

        if( p.data()[0] == sid)
        {
            pdu = p;
            _pdus.erase(_pdus.begin() + i);
            return true;
        }
    }

    return false;
}


void TPCANHandler::start()
{
    _canHandler.start();
    _pdus.clear();
    _running = true;
    _thread = new Pt::System::AttachedThread(Pt::callable(*this, &TPCANHandler::run));
    _thread->start();
}


void TPCANHandler::stop()
{
    if(!_running)
        return;
    
    _running = false;
    _canHandler.wake();
    _thread->join();
    delete _thread;
    _thread = 0;
    _canHandler.stop();
}


void TPCANHandler::run()
{
    bool extendedID = _canHandler.driver().extendedID();
    while(_running)
    {
        mps::can::drv::Message rsvMsg;
        int					 blockCount = 0;
        TPDUAddress			 taAdr(0,false);
        TPDU				 responcePdu;

        if(!_canHandler.wait(1000000))
            continue;

        while(_running)
        {
            if(taAdr.identifier() == 0)
            {
                if(!_canHandler.readMessage(rsvMsg))
                    break;
            }
            else
            {
                TPDUAddress physicalAddress = taAdr.getComplementAddress();
                if(!_canHandler.readMessageByID(physicalAddress.identifier(), rsvMsg))
                     break;
            }

            TPDU::FrameType ftype = responcePdu.addDataMessage(rsvMsg, extendedID);

            if(ftype == TPDU::SingleFrame || ftype == TPDU::EndFrame)
                break;

            switch(ftype)
            {				
                case TPDU::FirstFrame:
                {
                    blockCount = 1;
                    
                    if(taAdr.identifier() == 0)
                    {
                        TPDUAddress adsress(rsvMsg.identifier(), extendedID, rsvMsg.data()[0]);
                        taAdr = adsress.getComplementAddress();
                    }

                    TPFlowControlFrame fcf(taAdr, TPFlowControlFrame::ContinueToSend, _canHandler.blockSize(), _canHandler.timeUnit(), _canHandler.separationTime());
                    _canHandler.send(fcf);
                }
                break;

                case TPDU::ConsecutiveFrame:							

                    if( blockCount == _canHandler.blockSize())
                    {		
                        blockCount = 0;	
                        TPFlowControlFrame fcf(taAdr, TPFlowControlFrame::ContinueToSend, _canHandler.blockSize(), _canHandler.timeUnit(), _canHandler.separationTime());
                        _canHandler.send(fcf);
                    }

                    blockCount++;
                break;
                default:
                break;
            }

            if(taAdr.identifier() == 0)
                throw TPException("A target address is expected", TPException::ReceiveTimeoutError);

            if(!_canHandler.waitForID(rsvMsg.identifier(),500))
                throw TPException("Receive timeout", TPException::ReceiveTimeoutError);
        }		

        if(responcePdu.size() != 0 && _running)
        {
            _mutex.lock();
            _pdus.push_back(responcePdu);
            _mutex.unlock();

            signalDataAvailable(responcePdu.data()[0]);
        }
    }
}


void TPCANHandler::signalDataAvailable(Pt::uint8_t sid)
{
    Pt::System::MutexLock lock(_mutex);

    std::map<Pt::uint8_t,Pt::System::Condition*>::iterator it = _sidsToWait.begin();
    for( ; it != _sidsToWait.end(); ++it)
    {
        if( it->first == sid)
            it->second->signal();
    }
}

}}
