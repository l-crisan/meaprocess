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
#include "LoggerPS.h"
#include "OutPort.h"
#include "LoggerSignal.h"
#include <mps/can/drv/Factory.h>
#include <mps/core/Message.h>
#include <mps/core/SignalList.h>
#include <Pt/System/Clock.h>
#include <Pt/Byteorder.h>
#include <sstream>

namespace mps{ 
namespace can{

LoggerPS::LoggerPS()
: _driver("")
, _device("")
, _errorState(false)
{
    registerProperty("driver", *this, &LoggerPS::driver, &LoggerPS::setDriver);
    registerProperty("device", *this, &LoggerPS::device, &LoggerPS::setDevice);
    registerProperty("deviceNo", *this, &LoggerPS::deviceNo, &LoggerPS::setDeviceNo);

    for( Pt::uint32_t i = 0; i < 4; ++i)
        _drivers[i] = 0;
}


LoggerPS::~LoggerPS()
{
}


void LoggerPS::onInitialize()
{
    SynchSourcePS::onInitialize();

    _errorState = false;

    for( Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); portIdx++)
    {
        OutPort* port = (OutPort*)_outputPorts->at(portIdx);

        if(port->signalList() == 0)
            continue;		

        if(port->signalList()->size() == 0)
            continue;

        std::stringstream ss;
        std::string strport;

        ss<<portIdx+1;
        ss>>strport;

        try
        {
            //Initialize the CAN driver
            if(!_drivers[portIdx]->open( _device, _deviceNo, portIdx, port->bitRate(), port->extendedId() == 1) )
            {			
                mps::core::Message message( format(translate("Mp.CAN.Err.Port"), _drivers[portIdx]->driverInfo(),strport),
                                 mps::core::Message::Output, mps::core::Message::Error, Pt::System::Clock::getLocalTime());
                sendMessage( message );
                _errorState = true;
                return;
            }
        }
        catch(const std::logic_error& e)
        {
            std::cerr<<e.what()<<std::endl;
            mps::core::Message message( format(translate("Mp.CAN.Err.PortInit"), strport, _drivers[portIdx]->driverInfo()),
                             mps::core::Message::Output, mps::core::Message::Warning, Pt::System::Clock::getLocalTime());
            
            sendMessage( message );
        }

        _drivers[portIdx]->setAcceptanceMask(port->mask(), port->code());
        _drivers[portIdx]->reset();
    }
}


void LoggerPS::onDeinitialize()
{
    for( Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); portIdx++)
    {
        OutPort* port = (OutPort*) _outputPorts->at(portIdx);

        if(port->signalList() == 0)
            continue;

        if(port->signalList()->size() == 0)
            continue;

        _drivers[portIdx]->close();
    }

    SynchSourcePS::onDeinitialize();
}


void LoggerPS::onStart()
{
    if( _errorState)
        return;

    SynchSourcePS::onStart();

    for( Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); portIdx++)
    {
        OutPort* port = (OutPort*)_outputPorts->at(portIdx);

        if(port->signalList() == 0)
            continue;

        if(port->signalList()->size() == 0)
            continue;

        _drivers[portIdx]->reset();
    }
}


void LoggerPS::onStop()
{
    if( _errorState)
        return;

    SynchSourcePS::onStop();
}


void LoggerPS::onInitInstance()
{
    SynchSourcePS::onInitInstance();

    for( Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); portIdx++)
    {
        mps::core::Port* port = _outputPorts->at(portIdx);

        if(port->signalList() == 0)
            continue;

        if(port->signalList()->size() == 0)
            continue;

        _drivers[portIdx] = createDriver();
    }
}


void LoggerPS::onExitInstance()
{
    SynchSourcePS::onExitInstance();

    for( Pt::uint32_t i = 0; i < 4; ++i)
    {
        if(_drivers[i] != 0)
            delete _drivers[i];
    }
}


void LoggerPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    if( _errorState)
    {
        noOfRecords = 0;
        return;
    }

    can::drv::Message message;

    const OutPort* port = (const OutPort*)_outputPorts->at(portIdx);
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
    const Pt::uint32_t recSize = port->sourceDataSize(sourceIdx);

    for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
    {
        if(!_drivers[portIdx]->receive(message))
        {
            message.setDlc(0);
            message.setIdentifier(0);
            message.setTimeStamp(0);
            memset(message.data(),0, 8);
        }

        for(Pt::uint32_t s = 0; s < source.size(); ++s)
        {
            const LoggerSignal* signal = (const LoggerSignal*)source[s];
            int offset = port->signalOffsetInSource(sourceIdx, s);

            switch(signal->channelType())
            {
                case 0: //time stamp
                {
                    Pt::uint64_t time = message.timeStamp();
                    memcpy(&data[rec* recSize + offset], &time, 8);
                }
                break;
                
                case 2: //dlc
                    data[rec* recSize + offset] = (Pt::uint8_t) message.dlc();
                break;
                
                case 1: //identifier
                {
                    Pt::uint32_t id = message.identifier();
                    memcpy(&data[rec* recSize + offset], &id, 4);
                }
                break;
                case 3: //message
                {
                    //Pt::uint64_t dvalue = *((Pt::uint64_t*)message.data());
                    //dvalue = Pt::beToHost(dvalue);
                    memcpy(&data[rec* recSize + offset], &message.data()[0], 8);
                }
                break;
            }
        }
    }
}


can::drv::Driver* LoggerPS::createDriver()
{
    return can::drv::Factory::createDriver(driver());
}

}}
