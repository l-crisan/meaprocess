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
#include "EventPS.h"
#include <mps/can/drv/Factory.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <mps/core/Port.h>
#include <mps/core/Message.h>
#include <Pt/System/Clock.h>
#include <Pt/System/Process.h>
#include <Pt/Byteorder.h>

namespace mps{
namespace can{

EventPS::EventPS(void)
: _events(0)
, _sig2Event()
, _lastData()
, _firstSample()
, _driverType("")
, _device("")
, _port(0)
, _bitrate(0)
, _adrMode(0)
, _driver(0)
, _errorState(false)
{
    registerProperty("driver", *this, &EventPS::driver, &EventPS::setDriver);
    registerProperty("device", *this, &EventPS::device, &EventPS::setDevice);
    registerProperty("deviceNo", *this, &EventPS::deviceNo, &EventPS::setDeviceNo);
    registerProperty("port", *this, &EventPS::port, &EventPS::setPort);
    registerProperty("bitrate", *this, &EventPS::bitrate, &EventPS::setBitrate);
    registerProperty("adrMode", *this, &EventPS::adrMode, &EventPS::setAdrMode);
}


EventPS::~EventPS()
{
    for( Pt::uint32_t i = 0; i < _events->size(); ++i)
    {
        delete _events->at(i);
    }

    _events->clear();
    delete _events;
}


void EventPS::onInitInstance()
{
    ProcessStation::onInitInstance();

    for( Pt::uint32_t i = 0; i < _events->size(); ++i)
    {
        Event* ev = _events->at(i);
        Sig2EventIt it = _sig2Event.find(ev->signal());
        
        if( it == _sig2Event.end())
        {
            std::vector<Event*> events;
            events.push_back(ev);
            std::pair<Pt::uint32_t, std::vector<Event*> > pair(ev->signal(), events);
            _sig2Event.insert(pair);
        }
        else
        {
            it->second.push_back(ev);
        }
    }

    _driver = createDriver();
}


void EventPS::onExitInstance()
{
    ProcessStation::onExitInstance();

    if( _driver != 0)
        delete _driver;
}


can::drv::Driver* EventPS::createDriver()
{
    return can::drv::Factory::createDriver(driver());
}


void EventPS::onInitialize()
{
    ProcessStation::onInitialize();

    const mps::core::Port* inPort = _inputPorts->at(0);
    const mps::core::Sources& sources = inPort->sources();
   
    _lastData.resize(sources.size());
    _firstSample.resize(sources.size());

    for( Pt::uint32_t src = 0; src < sources.size(); ++src)
    {
        const std::vector<mps::core::Signal*>& source = sources[src];
        _lastData[src].resize(source.size());
        _firstSample[src].resize(source.size());
    }

    _errorState = false;

    std::stringstream ss;
    std::string strport;
    ss <<(int)(port() + 1);	
    ss>>strport;

    try
    {
        //Initialize the CAN driver
        if(!_driver->open( device(), deviceNo(), port(), bitrate(), adrMode() > 0) )
        {
            mps::core::Message message(format(translate("Mp.CAN.Err.Port"), _driver->driverInfo(), strport),
                            mps::core::Message::Output, mps::core::Message::Error, Pt::System::Clock::getLocalTime());

            sendMessage( message );
            _errorState = true;
            return;
        }
    }
    catch(const std::logic_error& e)
    {
        std::cerr<<e.what()<<std::endl;
        std::stringstream ss;

        mps::core::Message message(format(translate("Mp.CAN.Err.PortInit"), strport, _driver->driverInfo()), mps::core::Message::Output,
                                   mps::core::Message::Warning, Pt::System::Clock::getLocalTime());

        sendMessage( message );
    }
}


void EventPS::onStart()
{
    ProcessStation::onStart();
    resetLastData();
}


void EventPS::resetLastData()
{
    for( Pt::uint32_t i = 0; i < _lastData.size(); ++i)
    {
        std::vector<double>& data = _lastData[i];
    
        for( Pt::uint32_t j = 0; j < data.size(); ++j)
        {
            _firstSample[i][j] = true;
            data[j] = 0.0;
        }
    }
}


void EventPS::onStop()
{
    ProcessStation::onStop();
}


void EventPS::onDeinitialize()
{
    if(!_errorState && _driver != 0)
        _driver->close();

    ProcessStation::onDeinitialize();
}


void EventPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
    
    mps::core::Signal* signal = source[0];

    const Pt::uint32_t recSize = port->sourceDataSize(sourceIdx);

    Pt::uint32_t offset = 0;
    for( Pt::uint32_t i = 0; i < source.size(); ++i)
    {
        signal = source[i];
        Sig2EventIt it = _sig2Event.find(signal->signalID());

        if( it != _sig2Event.end())
        {
            for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
            {
                const Pt::uint8_t* datap = &data[rec* recSize + offset];
                double value = signal->scaleValue(datap);
                
                if( _firstSample[sourceIdx][i])
                {
                    fireEvents(it->second, value);
                    _firstSample[sourceIdx][i] = false;
                }
                else
                {
                    if(_lastData[sourceIdx][i] != value)
                        fireEvents(it->second, value);
                }
               _lastData[sourceIdx][i] = value;
            }
        }

        offset += signal->valueSize();
    }
}


bool EventPS::fireEvents(std::vector<Event*>& events, double value)
{
    if(_errorState)
        return false;

    bool fired = false;

    for(Pt::uint32_t i = 0; i < events.size(); ++i)
    {
        Event* ev = events[i];
        
        switch(ev->operation())
        {
            case Event::NotEq:
                if( value == ev->limit())
                    continue;
            break;
            
            case Event::Eq:
                if( value != ev->limit())
                    continue;
            break;

            case Event::Ls:
                if( value >= ev->limit())
                    continue;
            break;

            case Event::Le:
                if( value > ev->limit())
                    continue;
            break;

            case Event::Gr:
                if( value <= ev->limit())
                    continue;
            break;

            case Event::Ge:
                if( value < ev->limit())
                    continue;
            break;
        }

        fired = true;	
        can::drv::Message msg;

        msg.setIdentifier(ev->id());
        memcpy(msg.data(),&ev->byteData()[0],8);
        msg.setDlc(8);
        _driver->send(msg);
    }

    return fired;
}


void EventPS::addObject(Object* object, const std::string& type, const std::string& name )
{
    if(type == "Mp.CAN.Events")
        _events = (mps::core::ObjectVector<Event*>*) object;
    else
        ProcessStation::addObject(object, type, name);
}

}}
