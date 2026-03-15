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
#include "InPS.h"
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <mps/core/RuntimeEngine.h>
#include <Pt/System/Clock.h>
#include <Pt/System/Logger.h>
#include <sstream>
#include <mps/core/Message.h>
#include <algorithm>


namespace mps{
namespace streaming{

InPS::InPS(void)
: _readThread(0)
, _errorState(false) 
{
    registerProperty( "connection", *this, &InPS::connection, &InPS::setConnection);
    registerProperty( "signalList", *this, &InPS::signalList, &InPS::setSignalList);
}

InPS::~InPS(void)
{

}

const std::string& InPS::onGetSignalList(const std::string& connection) const
{
    return  _connection == connection ? _signalList : ProcessStation::onGetSignalList(connection);
}

void InPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    const mps::core::Port* port = _outputPorts->at(0);

    const mps::core::Sources& srcs = port->sources();

    _protocol = new mps::stream::prot::MeaReceiveProtocol();
    _protocol->onError += Pt::slot(*this, &InPS::sendWrongSigListError);
    _protocol->onDataAvailable += Pt::slot(*this, &InPS::onProtocolParsedData);

    _protocol->clearSourceMapping();

    for(Pt::uint32_t src = 0; src < srcs.size(); ++src)
    {
        const std::vector<mps::core::Signal*>& source = srcs[src];
        const StreamingSignal* signal = (const StreamingSignal*) source[0];
                _protocol->mapSource(signal->orgSourceNumber(), static_cast<Pt::uint32_t>(signal->sampleRate()), port->sourceDataSize(src), src);
    }        
}


void InPS::onExitInstance()
{	
    delete _protocol;
    _protocol = 0;
    FiFoSynchSourcePS::onExitInstance();
}

void InPS::onInitialize()
{
    FiFoSynchSourcePS::onInitialize();

    const mps::core::Port* port = _outputPorts->at(0);
    Pt::uint32_t maxRecordSize  = 1024;
    
    for(Pt::uint32_t src = 0; src < port->sources().size(); ++src)
    {
        const Pt::uint32_t srcSize = port->sourceDataSize(src);
        maxRecordSize = std::max(maxRecordSize, srcSize);
    }

   _buffer.resize(maxRecordSize*2);

    _errorState = false;
    _readThread = 0;
}

void InPS::sendErrorMsgOnCreateConn()
{
    _errorState = true;
    
    mps::core::Message message( format(translate("Mp.Streaming.Err.Conn"),this->connection()), mps::core::Message::Output, mps::core::Message::Error,
                      Pt::System::Clock::getLocalTime());
    sendMessage( message );
}

void InPS::onStart()
{
    FiFoSynchSourcePS::onStart();

    _readThread = 0;

    _protocol->reset();

    createConnection(connection());

    if(_errorState)
        return;

    if(StreamingPSBase::devices().size() != 0)
    {
        _readThread = new Pt::System::AttachedThread( Pt::callable(*this, &InPS::readData));
        _readThread->start();
    }
}

void InPS::onProtocolParsedData(const mps::stream::prot::ReceiveProtocol::DataInfo* dataInfo, bool& ready)
{
    _dataReady = true;
    ready = putRecords(dataInfo->sourceIndex, 0, dataInfo->records, &dataInfo->data[0]);
}

void InPS::onStop()
{
    if( _readThread != 0)
    {
        _loop.exit();
        _readThread->join();
        delete _readThread;
        _readThread = 0;
    }

    _protocol->reset();

    StreamingPSBase::destroyConnection();
    
    FiFoSynchSourcePS::onStop();

}

void InPS::onDeinitialize()
{
    FiFoSynchSourcePS::onDeinitialize();
}

void InPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    if(_dataReady)
    {
        FiFoSynchSourcePS::onSourceEvent(noOfRecords, maxNoOfSamples, sourceIdx, portIdx, data);
    }
    else
    {
        const mps::core::Port* port = _outputPorts->at(0);
        memset(data, 0, noOfRecords* port->sourceDataSize(sourceIdx));
    }
}


void InPS::onAddNewDevice(Pt::System::IODevice* device)
{
    if(_readThread != 0)
    {
        _loop.exit();
        _readThread->join();
        delete _readThread; 
    }

    deviceMutex().lock();

    while(devices().size() != 1)
    {
        Pt::System::IODevice* curDevice = devices()[0];
        if( device != curDevice)
            removeDevice(curDevice);
    }
    deviceMutex().unlock();

    _readThread = new Pt::System::AttachedThread( Pt::callable(*this, &InPS::readData));
    _readThread->start();
}

void InPS::onDataAvailable(Pt::System::IODevice& device)
{

    Pt::System::MutexLock logging(deviceMutex());

    try
    {
        const Pt::uint32_t count = device.endRead();

        if(_errorState)
            return;
        
        if(device.isEof())
            return;

        if( count == 0)
        {
            _protocol->reset();
            _dataReady = false;
            device.beginRead((char*)&_buffer[0], _buffer.size());
            return;
        }

        FiFoSynchSourcePS::lock();
        _protocol->propagateData(&_buffer[0], count);
        FiFoSynchSourcePS::unlock();
        
        device.beginRead((char*)&_buffer[0], _buffer.size());
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
        removeDevice(&device);
    }
}

void InPS::readData()
{
    try
    {
        if( !_errorState)
        {
            Pt::System::MutexLock logging(deviceMutex());

            if(StreamingPSBase::devices().size() != 0)
            {

                Pt::System::IODevice* device = devices().at(0);

                //Try to clear the device buffer
                try
                {
                    device->cancel();

                    while(true)
                    {
                        device->setTimeout(10);

                        Pt::uint32_t size = device->read((char*) &_buffer[0], (int) _buffer.size());

                        if( size == 0)
                            break;
                    }
                }
                catch(std::exception e)
                {

                }

                device->inputReady() += Pt::slot(*this, &InPS::onDataAvailable);
                device->setActive(_loop);
                device->sync();
                device->cancel();
                device->beginRead((char*) &_buffer[0], _buffer.size());
            }
        }
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }

    _dataReady = false;
    _protocol->reset();

    _loop.run();	
}

void InPS::sendWrongSigListError(const mps::stream::prot::ReceiveProtocol* prot)
{
    _errorState = true;
    _dataReady = false;
    mps::core::Message message( format(translate("Mp.Streaming.Err.WrongSigList"),this->getName()), mps::core::Message::Output, 
                     mps::core::Message::Error, Pt::System::Clock::getLocalTime());

    sendMessage( message );
}

}}
