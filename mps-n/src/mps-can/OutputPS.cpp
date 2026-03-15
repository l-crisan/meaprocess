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
#include "OutputPS.h"
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


OutputPS::OutputPS(void)
: _errorState(false)
, _driverType("")
, _device("")
, _port(0)
, _bitrate(0)
, _adrMode(0)
, _channels(0)
, _messageOutBuffer()
, _sig2chn()
, _msg2chn()
, _driver(0)
{
    registerProperty("port", *this, &OutputPS::port, &OutputPS::setPort);
    registerProperty("bitrate", *this, &OutputPS::bitrate, &OutputPS::setBitrate);
    registerProperty("adrMode", *this, &OutputPS::adrMode, &OutputPS::setAdrMode);
    registerProperty("driver", *this, &OutputPS::driver, &OutputPS::setDriver);
    registerProperty("device", *this, &OutputPS::device, &OutputPS::setDevice);
    registerProperty("deviceNo", *this, &OutputPS::deviceNo, &OutputPS::setDeviceNo);
}


OutputPS::~OutputPS(void)
{
}


can::drv::Driver* OutputPS::createDriver()
{
    return can::drv::Factory::createDriver(driver());
}


double OutputPS::getOffset(OutChannel* chn)
{
    double  rmax = 0;
    double  rmin = 0;
    
    if( chn->min() == chn->max())
        return 0;

    switch(chn->canDataType())
    {
        case Signal::Signed:
            rmin = -pow(2.0,(double)chn->bitCount()-1);
            rmax = pow(2.0, (double)chn->bitCount()-1) - 1;

        break;
        
        case Signal::Unsigned:
            rmin = 0;
            rmax = pow(2.0, (double)chn->bitCount()) - 1;
        break;
        
        case Signal::Float:
        case Signal::Double:
            return 0;
/*
            rmin = std::numeric_limits<float>::min();
            rmax = std::numeric_limits<float>::max();
        break;

        case CANSignal::Double:
            rmin = std::numeric_limits<double>::min();
            rmax = std::numeric_limits<double>::max();
*/
        break;
    }
    
    return (rmin * chn->max() - rmax* chn->min()) / (chn->max() - chn->min());
}


double OutputPS::getFactor(OutChannel* chn)
{
    double  rmax = 0;
    double  rmin = 0;

    if( chn->max() == chn->min())
        return 1;

    switch(chn->canDataType())
    {
        case Signal::Signed:
            rmin = -pow(2.0,(double)chn->bitCount()-1);
            rmax = pow(2.0, (double)chn->bitCount()-1) - 1;

        break;
        
        case Signal::Unsigned:
            rmin = 0;
            rmax = pow(2.0, (double)chn->bitCount()) - 1;
        break;
        
        case Signal::Float:
        case Signal::Double:
            return 1;
            /*
            rmin = std::numeric_limits<float>::min();
            rmax = std::numeric_limits<float>::max();
        break;

        case CANSignal::Double:
            rmin = std::numeric_limits<double>::min();
            rmax = std::numeric_limits<double>::max();
        break;*/
    }

    return (rmax - rmin) / (chn->max() - chn->min());
}


double OutputPS::getSignalRate(Pt::uint32_t id)
{
    const core::Port* port = _inputPorts->at(0);

    for( Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        const mps::core::Signal* signal = port->signalList()->at(i);

        if( signal->signalID() == id)
            return signal->sampleRate();
    }

    return 0.0;
}


void OutputPS::onInitInstance()
{
    core::ProcessStation::onInitInstance();

    std::map<Pt::uint32_t, std::vector<ChannelInfo> >::iterator msg2chnIt;

    for( Pt::uint32_t i = 0; i < _channels->size(); ++i)
    {
        OutChannel* chn = _channels->at(i);
        
        //Setup the signal to channel map
        const Pt::uint32_t sigID = chn->signal();
        
        std::map<Pt::uint32_t, std::vector<OutChannel*> >::iterator it;
        
        it = _sig2chn.find(sigID);
            
        if( it == _sig2chn.end())
        {
            std::vector<OutChannel*> channels;
            channels.push_back(chn);
            std::pair<Pt::uint32_t, std::vector<OutChannel*> > pair(sigID, channels);
            _sig2chn.insert(pair);
        }
        else
        {
            it->second.push_back(chn);
        }

        //Setup the message id to channel map
        ChannelInfo info;
        info.chn = chn;
        info.factor = getFactor(chn);
        info.offset = getOffset(chn);

        msg2chnIt  = _msg2chn.find(chn->msgId());

        if( msg2chnIt == _msg2chn.end())
        {
            std::vector<ChannelInfo> channels;
            channels.push_back(info);
            std::pair<Pt::uint32_t,std::vector<ChannelInfo> > pair(chn->msgId(), channels);
            _msg2chn.insert(pair);
        }
        else
        {
            msg2chnIt->second.push_back(info);
        }
    }

    //Setup the message output buffers 
    for( msg2chnIt = _msg2chn.begin(); msg2chnIt != _msg2chn.end(); ++msg2chnIt)
    {
        mps::core::RecordBuilder outBuffer;

        std::vector<double> rates;

        for( Pt::uint32_t i = 0; i < msg2chnIt->second.size(); ++i)
        {
            ChannelInfo& chnInfo = msg2chnIt->second[i];
            rates.push_back(getSignalRate(chnInfo.chn->signal()));
        }        

        std::vector<Pt::uint32_t> itemSizes(rates.size());
        itemSizes[0] = 8;
        outBuffer.init(512, itemSizes, rates, msg2chnIt->second[0].chn->rate());

        std::pair<Pt::uint32_t, mps::core::RecordBuilder> msg2Buf(msg2chnIt->first, outBuffer);

        _messageOutBuffer.insert(msg2Buf);
    }

    _driver = createDriver();
}


void OutputPS::onExitInstance()
{
    ProcessStation::onExitInstance();

    if( _driver != 0)
        delete _driver;
}


void OutputPS::onInitialize()
{
    ProcessStation::onInitialize();

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


void OutputPS::onStart()
{
    std::map<Pt::uint32_t,mps::core::RecordBuilder>::iterator it =  _messageOutBuffer.begin();
    
    for( ; it != _messageOutBuffer.end(); ++it)
        it->second.reset();

    ProcessStation::onStart();
}


void OutputPS::onStop()
{
    ProcessStation::onStop();
}


void OutputPS::onDeinitialize()
{
    if(!_errorState && _driver != 0)
        _driver->close();

    ProcessStation::onDeinitialize();
}


void OutputPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if(_errorState)
        return;

    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source  = sources[sourceIdx];
    const Pt::uint32_t recSize = port->sourceDataSize(sourceIdx);

    for(Pt::uint32_t rec = 0; rec <  noOfRecords; ++rec)
    {
        for(Pt::uint32_t sig = 0; sig < source.size(); ++sig)
        {
            const mps::core::Signal* signal = source[sig];

            std::map<Pt::uint32_t, std::vector<OutChannel*> >::iterator it;
            it = _sig2chn.find(signal->signalID());
            
            if( it == _sig2chn.end())
                continue;

            const Pt::uint32_t offsetInRec = port->signalOffsetInSource(sourceIdx, sig); 
            const double value = signal->scaleValue(&data[recSize*rec + offsetInRec]);

            for(Pt::uint32_t i = 0; i < it->second.size(); ++i)
            {
                const OutChannel* chn = it->second[i];

                std::map<Pt::uint32_t, mps::core::RecordBuilder>::iterator it2 = _messageOutBuffer.find(chn->msgId());
                
                if( it2 == _messageOutBuffer.end())
                    continue;
            
                writeValueToBuffer(chn, it2->second, value);
            }
        }
    }

    //Send the data.
    std::map<Pt::uint32_t,mps::core::RecordBuilder>::iterator it = _messageOutBuffer.begin();
    
    can::drv::Message messageData;
    const Pt::uint8_t* data1 = 0;
    const Pt::uint8_t* data2 = 0;
    
    Pt::uint32_t data1count = 0;	
    Pt::uint32_t data2count = 0;

    for( ; it != _messageOutBuffer.end(); ++it)
    {
        mps::core::RecordBuilder& outBuffer = it->second;

        try
        {
            outBuffer.get(&data1, data1count, &data2, data2count);
            messageData.setIdentifier(it->first);
            messageData.setDlc(8);

            for(Pt::uint32_t i = 0; i < data1count; ++i)
            {
                memcpy(messageData.data(),&data1[i*8],8);
                _driver->send(messageData);
            }

            for(Pt::uint32_t i = 0; i < data2count; ++i)
            {
                memcpy(messageData.data(),&data2[i*8],8);
                _driver->send(messageData);
            }
        }
        catch( const std::exception& e)
        {
            std::cerr<<e.what();
        }
    }
}
    

void OutputPS::writeValueToBuffer(const OutChannel* chn, mps::core::RecordBuilder& outBuffer, double value)
{
    std::map<Pt::uint32_t, std::vector<ChannelInfo> >::iterator it;
    
    it = _msg2chn.find(chn->msgId());

    for(Pt::uint32_t index  = 0; index < it->second.size(); ++index)
    {
        ChannelInfo& info = it->second[index];

        if( chn == info.chn)
        {
            std::vector<Pt::uint8_t> data;
            scaleDataToOutput(info, value, data);
            outBuffer.insert(&data[0], index, chn->pivotBit(), chn->bitCount(), chn->byteOrder() == Signal::Intel);
        }
    }
}


void OutputPS::scaleDataToOutput(ChannelInfo& info, double value, std::vector<Pt::uint8_t>& data)
{
    double scaledValue = value * info.factor + info.offset;

    Pt::uint32_t bytes = static_cast<Pt::uint32_t>(ceil(info.chn->bitCount()/8.0));

    switch( info.chn->canDataType())
    {
        case Signal::Signed:
            switch(bytes)
            {
                case 1:
                {
                    Pt::int8_t v = (Pt::int8_t) scaledValue;
                    data.resize(1);
                    memcpy(&data[0], &v, 1);
                }
                break;
                
                case 2:
                {
                    Pt::int16_t v = (Pt::int16_t) scaledValue;
                    data.resize(2);
                    memcpy(&data[0], &v, 2);
                }
                break;
                
                case 3:
                case 4:
                {
                    Pt::int32_t v = (Pt::int32_t) scaledValue;
                    data.resize(4);
                    memcpy(&data[0], &v, 4);
                }
                break;

                case 5:
                case 6:
                case 7:
                case 8:
                {
                    Pt::int64_t v = (Pt::int64_t) scaledValue;
                    data.resize(8);
                    memcpy(&data[0], &v, 8);
                }
                break;
            }
        break;

        case Signal::Unsigned:
            switch(bytes)
            {
                case 1:
                {
                    Pt::uint8_t v = (Pt::uint8_t) scaledValue;
                    data.resize(1);
                    memcpy(&data[0], &v, 1);
                }
                break;
                
                case 2:
                {
                    Pt::uint16_t v = (Pt::uint16_t) scaledValue;
                    data.resize(2);
                    memcpy(&data[0], &v, 2);
                }
                break;
                
                case 3:
                case 4:
                {
                    Pt::uint32_t v = (Pt::uint32_t) scaledValue;
                    data.resize(4);
                    memcpy(&data[0], &v, 4);
                }
                break;

                case 5:
                case 6:
                case 7:
                case 8:
                {
                    Pt::uint64_t v = (Pt::uint64_t) scaledValue;
                    data.resize(8);
                    memcpy(&data[0], &v, 8);
                }
                break;
            }
        break;

        case Signal::Float:
        {
            data.resize(sizeof(float));
            float v = static_cast<float>(scaledValue);
            memcpy(&data[0],&v, sizeof(float));
        }
        break;

        case Signal::Double:
        {
            data.resize(sizeof(double));
            memcpy(&data[0],&scaledValue, sizeof(double));
        }
        break;
    }
}


void OutputPS::addObject(Object* object, const std::string& type, const std::string& name )
{
    if(type == "Mp.CAN.OutChannels")
        _channels = (mps::core::ObjectVector<OutChannel*>*) object;
    else
        ProcessStation::addObject(object, type, name);
}


}}
