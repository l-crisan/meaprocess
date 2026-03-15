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
#include <mps/diatp/TPKLineHandler.h>
#include "TPSingleFrame.h"
#include "TPConsecutiveFrame.h"
#include "TPFirstFrame.h"
#include "TPFlowControlFrame.h"
#include <Pt/System/Thread.h>
#include <cmath>

namespace mps{
namespace diatp{

TPKLineHandler::TPKLineHandler(Pt::System::SerialDevice& serDevice)
: _running(false)
, _serDevice(serDevice)
, _pdus()
, _thread(0)
, _mutex()
, _sidsToWait()
, _key1(0)
, _key2(0)
, _loop()
, _buffer(1024)
, _al0(false)
, _al1(false)
, _hb0(false)
, _hb1(false)
{
    _serDevice.setActive(_loop);
}


TPKLineHandler::~TPKLineHandler()
{
    stop();
}


void TPKLineHandler::send(const TPDU& requestPdu)
{
    Pt::System::MutexLock lock(_mutex);
    const TPDUAddress& address = requestPdu.address();

    std::vector<Pt::uint8_t> pduData;

    if(requestPdu.size() < 64 && !_al0)
    {//length in format byte

        pduData.resize(requestPdu.size() + 4);
        
        Pt::uint8_t format = pduData.size() - 1;

        if(address.adrressType() == TPDUAddress::NormalFixedFunctional)
            format |= (((Pt::uint8_t)3) <<6);
        else
            format |= (((Pt::uint8_t)2) <<6);
        
        pduData[0] = format;
        pduData[1] = address.targetAdr();
        pduData[2] = (Pt::uint8_t) address.sourceAdrOrUniqID();
        memcpy(&pduData[3], requestPdu.data(), requestPdu.size());
    }
    else if( requestPdu.size() >= 64 && !_al1)
    {//add length byte
        pduData.resize(requestPdu.size() + 5);
        
        Pt::uint8_t format = 0;

        if(address.adrressType() == TPDUAddress::NormalFixedFunctional)
            format |= (((Pt::uint8_t)3) <<6);
        else
            format |= (((Pt::uint8_t)2) <<6);
        
        pduData[0] = format;
        pduData[1] = address.targetAdr();
        pduData[2] = (Pt::uint8_t) address.sourceAdrOrUniqID();
        pduData[3] = requestPdu.size();
        memcpy(&pduData[4], requestPdu.data(), requestPdu.size());
    }
    else
    {
        throw TPException("length coding error", TPException::WrongSequenceNumber);
    }

    //Calc check sum
    Pt::uint32_t sum = 0;
    for(size_t i = 0; i < (pduData.size() -1); ++i)
        sum += pduData[i];

    pduData[pduData.size()-1] = sum;

    size_t sended = _serDevice.write((const char*)&pduData[0], pduData.size());

    if(sended != pduData.size())
    {
        throw TPException("Serial device write error", TPException::SendTimeoutError);
    }
}


bool TPKLineHandler::waitForSID(Pt::uint8_t sid, size_t timeout)
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


bool TPKLineHandler::readSID(Pt::uint8_t sid, TPDU& pdu)
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


void TPKLineHandler::reset(bool hardware)
{
    Pt::System::MutexLock lock(_mutex);
    _pdus.clear();
}


void TPKLineHandler::start()
{
    _pdus.clear();
    _running = true;
    _thread = new Pt::System::AttachedThread(Pt::callable(*this, &TPKLineHandler::run));
    _thread->start();
    _serDevice.inputReady() += Pt::slot(*this, &TPKLineHandler::onDataAvailable); 	

    //_loop.add(_serDevice);
    _serDevice.beginRead((char*) &_buffer[0], _buffer.size());
}


bool TPKLineHandler::startCommunication(const TPDUAddress& address)
{
    Pt::System::MutexLock lock(_mutex);

    //Wake up over L-Line
    _serDevice.setRts(true);
    Pt::System::Thread::sleep(25);

    _serDevice.setRts(false);

    Pt::System::Thread::sleep(25);

    //Send the StartCommunication SID
    std::vector<Pt::uint8_t> startComPDU(5);
    
    if(address.adrressType() == TPDUAddress::NormalFixedFunctional)
        startComPDU[0] = 0xC1;
    else
        startComPDU[0] = 0x81;

    startComPDU[1] = address.targetAdr();
    startComPDU[2] = (Pt::uint8_t) address.sourceAdrOrUniqID();
    startComPDU[3] = 0x81; //SID for StartCommunication service 

    Pt::uint32_t checkSum = 0;

    for(size_t i = 0; i < (startComPDU.size() - 1); ++i)
        checkSum += startComPDU[i];
        
    startComPDU[4] = checkSum % 256;

    size_t written = _serDevice.write((const char*) &startComPDU[0], startComPDU.size());

    if( written != startComPDU.size())
    {
        return false;
    }
    
    if(!waitForSID(0xC1,500))
    {
        return false;
    }

    TPDU resPdu;

    if(!readSID(0xC1,resPdu))
    {
        return false;
    }

    const Pt::uint8_t* resData = resPdu.data();

    _key1 = resData[1];
    _key2 = resData[2];


    _al0 = (_key1 & 1) == 1;
    _al1 = (_key1 & 2) == 2;
    _hb0 = (_key1 & 4) == 4;
    _hb1 = (_key1 & 8) == 8;

    return true;
}


bool TPKLineHandler::stopCommunication(const TPDUAddress& address)
{
    Pt::System::MutexLock lock(_mutex);

    //Send the StopCommunication SID
    std::vector<Pt::uint8_t> stopComPDU(6);
    
    if(address.adrressType() == TPDUAddress::NormalFixedFunctional)
        stopComPDU[0] = 0xC0;
    else
        stopComPDU[0] = 0x80;

    stopComPDU[1] = address.targetAdr();
    stopComPDU[2] = (Pt::uint8_t) address.sourceAdrOrUniqID();
    stopComPDU[3] = 0x01; //length
    stopComPDU[4] = 0x82; //SID for StopCommunication service 

    Pt::uint32_t checkSum = 0;

    for(size_t i = 0; i < (stopComPDU.size() - 1); ++i)
        checkSum += stopComPDU[i];
        
    stopComPDU[5] = checkSum % 256;

    size_t written = _serDevice.write((const char*) &stopComPDU[0], stopComPDU.size());

    if( written != stopComPDU.size())
    {
        return false;
    }

    if(!waitForSID(0xC2,500))
    {
        return false;
    }

    TPDU resPdu;

    if(!readSID(0xC2,resPdu))
    {
        return false;
    }

    return true;
}


void TPKLineHandler::run()
{
    _loop.run();
}


void TPKLineHandler::onDataAvailable(Pt::System::IODevice& device)
{
    TPDU responcePdu;
    size_t count = _serDevice.endRead();
        
    if(count == 0)
    {
        _serDevice.beginRead((char*) &_buffer[0], _buffer.size());
        return;
    }

    Pt::uint8_t format = _buffer[0]; //Format
    Pt::uint8_t length = format & 0x3F;
    Pt::uint8_t adrType = (format & 0xC0)>>6;

    if( length == 0 )
    {
        length = _buffer[1]; //length

        TPDUAddress::AddressType pduAdrType = TPDUAddress::NormalFixedFunctional;
            
        if(adrType == 2)
            pduAdrType = TPDUAddress::NormalFixedPhysical;
        else if( adrType == 3)
            pduAdrType = TPDUAddress::NormalFixedFunctional;

        TPDUAddress  taAdr(_buffer[3], _buffer[2], pduAdrType); //source, target
        responcePdu.setAddress(taAdr);
        responcePdu.setData(&_buffer[4], length); //data
    }
    else
    {
        TPDUAddress::AddressType pduAdrType = TPDUAddress::NormalFixedFunctional;
            
        if(adrType == 2)
            pduAdrType = TPDUAddress::NormalFixedPhysical;
        else if( adrType == 3)
            pduAdrType = TPDUAddress::NormalFixedFunctional;

        TPDUAddress  taAdr(_buffer[2], _buffer[1],pduAdrType); //source, target
        responcePdu.setAddress(taAdr);
        responcePdu.setData(&_buffer[3], length); //data
    }

    if(responcePdu.size() != 0)
    {
        _mutex.lock();
        _pdus.push_back(responcePdu);
        _mutex.unlock();

        signalDataAvailable(responcePdu.data()[0]);
    }
    _serDevice.beginRead((char*) &_buffer[0], _buffer.size());
}


void TPKLineHandler::signalDataAvailable(Pt::uint8_t sid)
{
    Pt::System::MutexLock lock(_mutex);

    std::map<Pt::uint8_t,Pt::System::Condition*>::iterator it = _sidsToWait.begin();
    for( ; it != _sidsToWait.end(); ++it)
    {
        if( it->first == sid)
            it->second->signal();
    }
}


void TPKLineHandler::stop()
{
    if(!_running)
        return;

    _running = false;
    _loop.exit();

    _thread->join();
    delete _thread;
}

}}
