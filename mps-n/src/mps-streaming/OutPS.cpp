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
#include "OutPS.h"
#include <mps/core/SignalList.h>
#include <mps/core/Port.h>
#include <mps/core/Signal.h>
#include <Pt/System/Clock.h>
#include <Pt/Convert.h>
#include <sstream>
#include <string.h>

namespace mps{
namespace streaming{
    
enum { DEFAULT_SEND_QUEUE_SIZE = 20 };      // 20 Elements
enum { DEFAULT_SEND_BUFFER_SIZE = 1024*100 }; // 100 (KB)

OutPS::OutPS(void)
: _errorState(false)
, _protocol(new mps::stream::prot::MeaSendProtocol())
{
    registerProperty( "connection", *this, &OutPS::connection, &OutPS::setConnection);	
    registerProperty( "signalList", *this, &OutPS::signalList,  &OutPS::setSignalList);
}

OutPS::~OutPS(void)
{
    delete _protocol;
}

const std::string& OutPS::onGetSignalList(const std::string& connection) const
{
    return  _connection == connection ? _signalList : ProcessStation::onGetSignalList(connection);
}

void OutPS::onInitInstance()
{
    ProcessStation::onInitInstance();

    const mps::core::Port* port = _inputPorts->at(0);

    _memPckPool.resize(port->sources().size());
    _memPckPoolIndex.resize(port->sources().size());

    for( Pt::uint32_t src = 0; src < port->sources().size(); ++src)
        for( Pt::uint32_t i = 0; i < DEFAULT_SEND_QUEUE_SIZE; ++i)
            _memPckPool[src].push_back( new DataPacketInfo(512));
}

void OutPS::onExitInstance()
{
    const mps::core::Port* port = _inputPorts->at(0);   

    for( Pt::uint32_t src = 0; src < port->sources().size(); ++src)
        for( Pt::uint32_t i = 0; i < DEFAULT_SEND_QUEUE_SIZE; ++i)
            delete _memPckPool[src][i];

    _memPckPool.clear();

    ProcessStation::onExitInstance();
}

void OutPS::onInitialize()
{
    ProcessStation::onInitialize();
    _sendBuffer.init(_memPckPool.size()* DEFAULT_SEND_QUEUE_SIZE, sizeof(DataPacketInfo*));
}

void OutPS::sendErrorMsgOnCreateConn()
{
    _errorState = true;   
    
    mps::core::Message message( format(translate("Mp.Streaming.Err.Conn"),this->connection()), mps::core::Message::Output, mps::core::Message::Error,
                      Pt::System::Clock::getLocalTime());
    sendMessage( message );
}

void OutPS::onStart()
{
    const mps::core::Port* port = _inputPorts->at(0);

    //Create the connections
    createConnection(connection());

    //Start the protocol
    _protocol->reset(port->sources().size());

    memset(&_memPckPoolIndex[0], 0, _memPckPoolIndex.size() * sizeof(Pt::uint32_t));

    _sendBuffer.reset();

    //Start the send thread
    _running = true;
    _sendThread = new Pt::System::AttachedThread(Pt::callable(*this, &OutPS::sendThread));
    _sendThread->start();
    
    ProcessStation::onStart(); 
}

void OutPS::sendThread()
{
    Pt::uint32_t i = 0;
    std::vector<std::vector<Pt::uint8_t> > outputData;
    
    //Alocate the local send buffer
    outputData.resize(DEFAULT_SEND_QUEUE_SIZE);

    for( i = 0; i < outputData.size(); ++i)
        outputData[i].reserve(DEFAULT_SEND_BUFFER_SIZE);	

    //Send thread
    while(_running)
    {
        Pt::uint32_t elements = 0;
        
        {
            Pt::System::MutexLock lock(_sendBufferMutex);

            if(!_sendSignal.wait(_sendBufferMutex,100))
                continue;
                
            //Copy the data from the queue 
            elements = _sendBuffer.noOfElements();

            if(outputData.size() < elements)
                outputData.resize(elements);

            for( Pt::uint32_t j = 0; (j < elements) && _running; ++j)
            {
                const size_t* addr  = (size_t*) _sendBuffer.get();
            
                const DataPacketInfo* packtInfo = (DataPacketInfo*) *addr;
            
                if(outputData[j].capacity() < packtInfo->size)
                    outputData[j].reserve(packtInfo->size);

                outputData[j].resize(packtInfo->size);

                memcpy(&outputData[j][0], &packtInfo->data[0], packtInfo->size);
            
                _sendBuffer.next();
            }
        }
            
        if(!_running)
            break;

        //Send the data
        {
            Pt::System::MutexLock lock(deviceMutex());


            for(i = 0; i < devices().size() && _running; ++i)
            {
                Pt::System::IODevice* device = devices()[i];

                for( Pt::uint32_t elem = 0; elem < elements; ++elem)
                {
                    std::vector<Pt::uint8_t>& packet = outputData[elem];

                    try
                    {
                        Pt::uint32_t offset = 0;

                        while(offset != packet.size())
                            offset +=  device->write((const char*) &packet[offset], (packet.size() - offset) );						
                    }
                    catch(std::exception& ex)
                    {
                        std::cerr<<"Send error "<<ex.what()<<std::endl;
                        removeDevice(device);
                        --i;
                        break;
                    }
                }
            }
        }
    }
}

void OutPS::onStop()
{
    destroyConnection();

    if(_sendThread != 0)
    {
        
        _running = false;
        _sendSignal.signal();
        _sendThread->join();
        delete _sendThread;
        _sendThread = 0;
    }

    ProcessStation::onStop();
}

void OutPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if(_errorState)
        return;
    
    {
        Pt::System::MutexLock mutexLock(_sendBufferMutex);

        if(_sendBuffer.isFull())
        {
            _sendSignal.signal();
            return;
        }

        const mps::core::Signal*    signal   = port->sources()[sourceIdx][0];
        mps::stream::prot::SendProtocol::DataInfo dataInfo = {0};	

        if( _memPckPoolIndex[sourceIdx] == _memPckPool[sourceIdx].size())
            _memPckPoolIndex[sourceIdx] = 0;

        DataPacketInfo* packetInfo = _memPckPool[sourceIdx][_memPckPoolIndex[sourceIdx]];
        _memPckPoolIndex[sourceIdx]++;

        dataInfo.noOfRecords		= noOfRecords;
        dataInfo.packetsPerSecond	= (Pt::uint32_t) signal->sampleRate();	
        dataInfo.recordSize			= port->sourceDataSize(sourceIdx);
        dataInfo.sourceIdx			= sourceIdx;
        dataInfo.srcNo				= signal->sourceNumber();

        packetInfo->size = _protocol->formatData(dataInfo, data, packetInfo->data);
        size_t addr = (size_t) packetInfo;
        _sendBuffer.insert((const Pt::uint8_t*) &addr);
    }

    _sendSignal.signal();
}

}}

