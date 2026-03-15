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
#include "GpsPS.h"
#include "GpsSignal.h"
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <mps/core/Port.h>
#include <mps/core/Message.h>
#include <Pt/System/Clock.h>
#include <sstream>

namespace mps{
namespace gps{

GpsPS::GpsPS(void)
: _device()
{	
    _nmeaData.resize(1024*2);
    registerProperty( "comPort", *this, &GpsPS::comPort, &GpsPS::setComPort );
    registerProperty( "rate", *this, &GpsPS::rate, &GpsPS::setRate );
    
    _signalData.resize(GpsSignal::GpsSignals);

    _signalData[GpsSignal::Latitude].resize(sizeof(double));
    _signalData[GpsSignal::Longitude].resize(sizeof(double));
    _signalData[GpsSignal::Altitude].resize(sizeof(double));
    _signalData[GpsSignal::Satellites].resize(sizeof(Pt::uint32_t));
    _signalData[GpsSignal::Day].resize(sizeof(Pt::uint8_t));
    _signalData[GpsSignal::Month].resize(sizeof(Pt::uint8_t));
    _signalData[GpsSignal::Year].resize(sizeof(Pt::uint16_t));
    _signalData[GpsSignal::Hour].resize(sizeof(Pt::uint8_t));
    _signalData[GpsSignal::Minute].resize(sizeof(Pt::uint8_t)); 
    _signalData[GpsSignal::Second].resize(sizeof(Pt::uint8_t)); 
    _signalData[GpsSignal::Status].resize(sizeof(Pt::uint8_t)); 
    _signalData[GpsSignal::Speed].resize(sizeof(double)); 
    _signalData[GpsSignal::TrackAngle].resize(sizeof(double)); 
    _signalData[GpsSignal::Quality].resize(sizeof(Pt::uint8_t)); 
    _signalData[GpsSignal::PDOP].resize(sizeof(double)); 
    _signalData[GpsSignal::HDOP].resize(sizeof(double)); 
    _signalData[GpsSignal::VDOP].resize(sizeof(double));

    _nmeaParser.dataAvailable += Pt::slot(*this,&GpsPS::onGpsData);
}


GpsPS::~GpsPS(void)
{
}


void GpsPS::onInitialize()
{
    SynchSourcePS::onInitialize();
    

    std::string portStr;

    try
    {
        _errorState = false;		

        portStr = comPort();

        if(isProperty(portStr.c_str()))
            portStr = getPropertyValueFromKey(portStr.c_str());

        std::string rateStr = rate();

        if(isProperty(rateStr.c_str()))
            rateStr = getPropertyValueFromKey(rateStr.c_str());

        Pt::int32_t irate = 0;
        std::stringstream ss;
        ss<<rateStr;
        ss>>irate;
        
        Pt::System::SerialDevice::BaudRate brate = (Pt::System::SerialDevice::BaudRate)irate;

        _device.open(portStr, std::ios::in);
        _device.setBaudRate(brate);		
        _device.setActive(_loop);
        _device.inputReady() += Pt::slot(*this, &GpsPS::onDataAvailable);

        const mps::core::Port* port = _outputPorts->at(0);
        const mps::core::Sources& sources = port->sources();
        Pt::uint32_t recSize = 0;

        for(Pt::uint32_t i = 0; i < sources.size(); ++i)
            recSize = std::max(recSize, port->sourceDataSize(i));
    
        _recordData.resize(recSize);

    }
    catch(const std::exception& e)
    {
        std::cerr<<e.what()<<std::endl;
        _errorState = true;
        mps::core::Message message( format(translate("Mp.Gps.Err.PortOpen"),portStr), mps::core::Message::Output, mps::core::Message::Error,
                         Pt::System::Clock::getLocalTime());

        sendMessage(message);
    }
}


void GpsPS::onDeinitialize()
{
    SynchSourcePS::onDeinitialize();
    
    if(_errorState)
        return;

    _device.close();
}


void GpsPS::onGpsData()
{
    double dbvalue = _nmeaParser.latitude();
    memcpy(&_signalData[GpsSignal::Latitude][0],&dbvalue,sizeof(double));
    
    dbvalue = _nmeaParser.longitude();
    memcpy(&_signalData[GpsSignal::Longitude][0],&dbvalue,sizeof(double));

    dbvalue = _nmeaParser.altitude();
    memcpy(&_signalData[GpsSignal::Altitude][0],&dbvalue,sizeof(double));

    Pt::uint32_t u32value = _nmeaParser.satellitesInUse();
    memcpy(&_signalData[GpsSignal::Satellites][0],&u32value,sizeof(Pt::uint32_t));

    Pt::uint8_t u8value = _nmeaParser.day();
    memcpy(&_signalData[GpsSignal::Day][0],&u8value,sizeof(Pt::uint8_t));

    u8value = _nmeaParser.month();
    memcpy(&_signalData[GpsSignal::Month][0],&u8value,sizeof(Pt::uint8_t));

    Pt::uint16_t u16value = _nmeaParser.year();
    memcpy(&_signalData[GpsSignal::Year][0],&u16value,sizeof(Pt::uint16_t));
    
    u8value = _nmeaParser.hour();
    memcpy(&_signalData[GpsSignal::Hour][0],&u8value,sizeof(Pt::uint8_t));

    u8value = _nmeaParser.minute();
    memcpy(&_signalData[GpsSignal::Minute][0],&u8value,sizeof(Pt::uint8_t));

    u8value = _nmeaParser.second();
    memcpy(&(_signalData[GpsSignal::Second][0]),&u8value,sizeof(Pt::uint8_t));

    if(_nmeaParser.status())
        u8value = 1;
    else
        u8value = 0;

    memcpy(&_signalData[GpsSignal::Status][0],&u8value,sizeof(Pt::uint8_t));

    dbvalue = _nmeaParser.groundSpeed();
    memcpy(&_signalData[GpsSignal::Speed][0],&dbvalue,sizeof(double));

    dbvalue = _nmeaParser.trackAngle();
    memcpy(&_signalData[GpsSignal::TrackAngle][0],&dbvalue,sizeof(double));

    u8value = (Pt::uint8_t) _nmeaParser.qualityOfMea();
    memcpy(&_signalData[GpsSignal::Quality][0],&u8value,sizeof(Pt::uint8_t));

    dbvalue = _nmeaParser.pdop();
    memcpy(&_signalData[GpsSignal::PDOP][0],&dbvalue,sizeof(double));

    dbvalue = _nmeaParser.hdop();
    memcpy(&_signalData[GpsSignal::HDOP][0],&dbvalue,sizeof(double));

    dbvalue = _nmeaParser.vdop();
    memcpy(&_signalData[GpsSignal::VDOP][0],&dbvalue,sizeof(double));
}


void GpsPS::onStart()
{
    if(_errorState)
        return;

    _nmeaParser.reset();

    
    _runThread = new Pt::System::AttachedThread( Pt::callable(*this, &GpsPS::run));
    _runThread->start();
    _device.beginRead(&_nmeaData[0],_nmeaData.size());

    SynchSourcePS::onStart();
}


void GpsPS::onStop()
{
    if(_errorState)
        return;
        
    _loop.exit();
    _device.cancel();

    _runThread->join();
    delete _runThread;
    _runThread = 0;
    

    SynchSourcePS::onStop();
}


void GpsPS::run()
{
    _loop.run();
}


void GpsPS::onDataAvailable(Pt::System::IODevice& device)
{
    try
    {
        size_t size = _device.endRead();
        
        for(size_t i = 0; i < size; ++i)
            _nmeaParser.parse(_nmeaData[i]); 
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }

    _device.beginRead(&_nmeaData[0],_nmeaData.size());
}


void GpsPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    const mps::core::Port* port = _outputPorts->at(portIdx);
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
    
    Pt::uint32_t recSize = port->sourceDataSize(sourceIdx);
    
    Pt::uint32_t offset = 0;
    for( Pt::uint32_t i = 0; i < source.size(); ++i)
    {
        GpsSignal* gpsSig = (GpsSignal*) source[i];
        memcpy(&_recordData[offset],&(_signalData[gpsSig->element()][0]),gpsSig->valueSize());
        offset += gpsSig->valueSize();
    }

    for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        memcpy(&data[rec* recSize], &_recordData[0],  recSize);	
}


const std::string& GpsPS::comPort() const
{
    return _comPort;
}


void GpsPS::setComPort(const std::string& p)
{
    _comPort = p;
}


const std::string& GpsPS::rate() const
{
    return _rate;
}


void GpsPS::setRate(const std::string& r)
{
    _rate = r;
}

}}
