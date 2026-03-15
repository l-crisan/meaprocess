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
#include "WritePS.h"
#include "Signal.h"
#include <mps/can/drv/Factory.h>
#include <mps/core/Signal.h>
#include <mps/core/Message.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <Pt/System/Clock.h>
#include <limits>
#include <math.h>
#include <sstream>

namespace mps{
namespace can{

static Pt::uint32_t toUInt(const std::string& str)
{
    std::stringstream ss;
    Pt::uint32_t no;

    ss<<str;
    ss>>no;

    return no;
}


WritePS::WritePS(void)
: _errorState(false)
, _driverType("")
, _device("")
, _port(0)
, _bitrate(0)
, _adrMode(0)
, _driver(0)
{
    registerProperty("port", *this, &WritePS::port, &WritePS::setPort);
    registerProperty("bitrate", *this, &WritePS::bitrate, &WritePS::setBitrate);
    registerProperty("adrMode", *this, &WritePS::adrMode, &WritePS::setAdrMode);
    registerProperty("driver", *this, &WritePS::driver, &WritePS::setDriver);
    registerProperty("device", *this, &WritePS::device, &WritePS::setDevice);
    registerProperty("deviceNo", *this, &WritePS::deviceNo, &WritePS::setDeviceNo);
    registerProperty("signalTypeMap", *this, &WritePS::signalTypeMap, &WritePS::setSignalTypeMap);
}


WritePS::~WritePS(void)
{
}


void WritePS::onInitInstance()
{
    core::ProcessStation::onInitInstance();	
    _driver = can::drv::Factory::createDriver( driver() );

    //Create the signal mapping
    std::stringstream ss;
    ss<<_signalTypeMap;

    std::string item;

    const mps::core::Port* port = _inputPorts->at(0);

    _idSignal = 0;
    _timeSignal = 0;
    _dlcSignal = 0;
    _dataSignal = 0;

    while(ss)
    {
        std::getline(ss, item, '#');
        
        std::stringstream ss2;
        ss2<<item;

        std::string sigId;
        std::string sigType;

        std::getline(ss2, sigId, '/');
        std::getline(ss2, sigType);

        Pt::uint32_t sigIdNo = toUInt(sigId);
        Pt::uint32_t type = toUInt(sigType);
        
        mps::core::Signal * signal = port->getSignalByID(sigIdNo);

        switch(type)
        {
            case 0: //Timestamp
                _timeSignal = signal;
            break;
            
            case 1: //Identifier
                _idSignal = signal;
            break;

            case 2://DLC
                _dlcSignal = signal;
            break;

            case 3://CAN Daten
                _dataSignal = signal;
            break;
        }
    }
}


void WritePS::onExitInstance()
{
    core::ProcessStation::onExitInstance();

    if( _driver != 0)
        delete _driver;
}


void WritePS::onInitialize()
{
    core::ProcessStation::onInitialize();

    _errorState = false;
    std::stringstream ss;
    std::string strport;
    ss<<(int)port()+1;
    ss>>strport;

    try
    {
        //Initialize the CAN driver
        if(!_driver->open( device(), deviceNo(), port(), bitrate(), adrMode() > 0) )
        {
            mps::core::Message message( format(translate("CANLoggerPS_PortErr"), _driver->driverInfo(), strport),
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
        mps::core::Message message( format(translate("CANLoggerPS_PortInitErr"), strport, _driver->driverInfo()),
                         mps::core::Message::Output, mps::core::Message::Warning, Pt::System::Clock::getLocalTime());
        sendMessage( message );
    }
}


void WritePS::onStart()
{
    core::ProcessStation::onStart();
    _messageToSend.setIdentifier(0);
    _messageToSend.setTimeStamp(0);
}


void WritePS::onDeinitialize()
{
    if(!_errorState && _driver != 0)
        _driver->close();

    core::ProcessStation::onDeinitialize();
}


void WritePS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if(_errorState)
        return;

    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source  = sources[sourceIdx];
    const Pt::uint32_t recSize = port->sourceDataSize(sourceIdx);
    Pt::uint32_t offset = 0;

    for(Pt::uint32_t rec = 0; rec <  noOfRecords; ++rec)
    {
        for(Pt::uint32_t sig = 0; sig < source.size(); ++sig)
        {
            const mps::core::Signal* signal = source[sig];
            
            if( signal == _idSignal)
            {
                Pt::uint32_t offsetInRec = port->signalOffsetInSource(sourceIdx,sig);
                const double id = signal->scaleValue(&data[recSize*rec + offsetInRec]);
                _messageToSend.setIdentifier((Pt::uint32_t) id);
            }

            if( signal == _timeSignal)
            {
                Pt::uint32_t offsetInRec = port->signalOffsetInSource(sourceIdx,sig);
                const double timeStamp = signal->scaleValue(&data[recSize*rec + offsetInRec]);
                _messageToSend.setTimeStamp((Pt::uint64_t) timeStamp);
            }

            if( signal == _dlcSignal)
            {
                Pt::uint32_t offsetInRec = port->signalOffsetInSource(sourceIdx,sig);
                const double dlc = signal->scaleValue(&data[recSize*rec + offsetInRec]);
                _messageToSend.setDlc((Pt::uint32_t)dlc);
            }

            if( signal == _dataSignal)
            {
                Pt::uint32_t offsetInRec = port->signalOffsetInSource(sourceIdx,sig);
                memcpy(&_messageToSend.data()[0],&data[recSize*rec + offsetInRec], 8);
            }
        }

        if(_messageToSend.identifier() != 0)
        {
            if( _messageToSend.identifier() != _lastMessage.identifier())
            {
                _driver->send(_messageToSend);
            }
            else
            {
                if(_messageToSend.timeStamp() > _lastMessage.timeStamp())
                    _driver->send(_messageToSend);
            }
        }

        _lastMessage = _messageToSend;
    }

}


}}
