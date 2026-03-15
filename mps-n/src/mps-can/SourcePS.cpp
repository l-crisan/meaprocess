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
#include "SourcePS.h"
#include "OutPort.h"
#include "Signal.h"
#include <mps/can/drv/Driver.h>
#include <mps/can/drv/Factory.h>
#include <mps/core/Message.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <Pt/System/Clock.h>
#include <sstream>
#include <string>


namespace mps{
namespace can{

using namespace std;

SourcePS::SourcePS(void)
: _id2Signals()
, _sourceData()
, _driver("")
, _device("")
, _running(false)
, _scanThread(0)
, _errorState(false)
{
    registerProperty("driver", *this, &SourcePS::driver, &SourcePS::setDriver);
    registerProperty("device", *this, &SourcePS::device, &SourcePS::setDevice);
    registerProperty("deviceNo", *this, &SourcePS::deviceNo, &SourcePS::setDeviceNo);

    for( Pt::uint32_t i = 0; i < 4; ++i)
        _drivers[i] = 0;
}


SourcePS::~SourcePS(void)
{
}


void SourcePS::onInitInstance()
{
    core::FiFoSynchSourcePS::onInitInstance();

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


void SourcePS::onExitInstance()
{
    core::FiFoSynchSourcePS::onExitInstance();

    for( Pt::uint32_t i = 0; i < 4; ++i)
    {
        if(_drivers[i] != 0)
            delete _drivers[i];
    }
}


void SourcePS::onStart()
{
    if(_errorState)
        return;

    bool startScanThread = false;

    for (Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); ++portIdx)
    {
        OutPort* port = (OutPort*)_outputPorts->at(portIdx);
        
        if(port->signalList() == 0)
            continue;

        if(port->signalList()->size() == 0)
            continue;

        _drivers[portIdx]->reset();

        if( port->tact() == 0)
        {
            startScanThread = true;

            const mps::core::Sources& sources = port->sources();

            for(Pt::uint32_t src = 0; src < sources.size(); ++src)
            {
                Pt::uint32_t size = port->sourceDataSize(src);

                memset(&_sourceData[portIdx][src][0], 0, size);
            }
        }
    }

    if( startScanThread)
    {
        _running = true;
        _scanThread = new Pt::System::AttachedThread(Pt::callable(*this, &SourcePS::scan));
        _scanThread->start();
    }

    FiFoSynchSourcePS::onStart();
}


void SourcePS::onStop()
{
    core::FiFoSynchSourcePS::onStop();

    if(_scanThread != 0)
    {
        _running = false;
        _scanThread->join();
        delete _scanThread;
        _scanThread = 0;
    }
}


can::drv::Driver* SourcePS::createDriver()
{
    return can::drv::Factory::createDriver( driver() );
}


void SourcePS::onInitialize()
{
    core::FiFoSynchSourcePS::onInitialize();
        
    _errorState = false;

    _sourceData.clear();
    _sourceData.resize(_outputPorts->size());
    
    _id2Signals.clear();
    _id2Signals.resize(_outputPorts->size());

    for( Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); portIdx++)
    {	
        OutPort* port = (OutPort*)_outputPorts->at(portIdx);

        if(port->signalList() == 0)
            continue;

        if(port->signalList()->size() == 0)
        continue;

        std::stringstream ss;
        std::string strport;
        ss<< portIdx+1;
        ss>>strport;

        try
        {
            //Initialize the CAN driver
            if(!_drivers[portIdx]->open(_device,  _deviceNo,  portIdx, port->bitRate(), port->extendedId() != 0) )
            {
                mps::core::Message message( format( translate("Mp.CAN.Err.Port"), _drivers[portIdx]->driverInfo(), strport),
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
        
        _drivers[portIdx]->reset();
        
        //Create the CAN-ID map to signals.
        std::map<Pt::uint32_t,std::vector<const Signal*> >::iterator it;

        for( Pt::uint32_t sig = 0 ; sig < port->signalList()->size(); ++sig)
        {
            Signal* signal = (Signal*)port->signalList()->at(sig);
            
            it = _id2Signals[portIdx].find(signal->msgId());
            
            if(it == _id2Signals[portIdx].end())
            {
                std::pair<Pt::uint32_t, std::vector<const Signal*> > pair(signal->msgId(), std::vector<const Signal*>());
                pair.second.push_back(signal);
                _id2Signals[portIdx].insert(pair);
            }
            else
            {
                it->second.push_back(signal);
            }
        }
        
        //Create the source data array if MeaProcess is the sampler.
        if( port->tact() == 0)
        {
            const mps::core::Sources& sources = port->sources();
            _sourceData[portIdx].resize(sources.size());

            for(Pt::uint32_t src = 0; src < sources.size(); ++src)
            {
                Pt::uint32_t size = port->sourceDataSize(src);
                _sourceData[portIdx][src].resize(size);
            }
        }
    }
}


void SourcePS::onDeinitialize()
{
    for( Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); portIdx++)
    {
        OutPort* port = (OutPort*)_outputPorts->at(portIdx);

        if(port->signalList() == 0)
            continue;

        if(port->signalList()->size() == 0)
            continue;

        _drivers[portIdx]->close();
    }

    core::FiFoSynchSourcePS::onDeinitialize();
}


void SourcePS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    OutPort* port = (OutPort*)_outputPorts->at(portIdx);

    if( port->tact() == 1)
    {//Hardware has its own sampling rate
        core::FiFoSynchSourcePS::onSourceEvent(noOfRecords, maxNoOfSamples, sourceIdx, portIdx, data);
        return;
    }

    const Pt::uint32_t recordSize = port->sourceDataSize(sourceIdx);

    for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        memcpy(&data[rec*recordSize],&_sourceData[portIdx][sourceIdx][0],recordSize);
}


void SourcePS::scan()
{
    can::drv::Message msg;
    Pt::uint8_t extractedData[8];

    while(_running)
    {
        for( Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); portIdx++ )
        {
            OutPort* port = (OutPort*)_outputPorts->at(portIdx);
            
            if(port->signalList()== 0)
                continue;
        
            if(port->signalList()->size() == 0)
                continue;

            while( _drivers[portIdx]->receive(msg) )
            {
                Id2SignalsIt it = _id2Signals[portIdx].find(msg.identifier());
                
                if(it == _id2Signals[portIdx].end())
                    continue;

                for( Pt::uint32_t sig = 0;  sig < it->second.size(); ++sig)
                {
                    const Signal* signal = it->second[sig];
                    
                    const Pt::uint32_t sigIdx = signalIndex(signal);
                    const Pt::uint32_t srcIdx = port->sourceIndex(sigIdx);
                    const Pt::uint32_t offset = port->signalOffsetInSource(sigIdx);

                    if(getSignalValue(&extractedData[0], signal, msg))
                        memcpy(&_sourceData[portIdx][srcIdx][offset], &extractedData[0], signal->valueSize());
                }
            }
        }

        Pt::System::Thread::sleep(5);
    }
}


void SourcePS::onReadData()
{
    can::drv::Message msg;
    Pt::uint8_t	extractedData[8];

    for( Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); portIdx++ )
    {
        OutPort* port = (OutPort*)_outputPorts->at(portIdx);
        
        if( port->signalList()== 0  || port->tact() == 0 )
            continue;

        if(port->signalList()->size()== 0)
            continue;
    
        while( _drivers[portIdx]->receive(msg) )
        {
            Id2SignalsIt it = _id2Signals[portIdx].find(msg.identifier());
            
            if(it == _id2Signals[portIdx].end())
                continue;

            for( Pt::uint32_t sig = 0;  sig < it->second.size(); ++sig)
            {
                const Signal* signal = it->second[sig];

                if(getSignalValue(&extractedData[0], signal, msg))
                    putValue(signal, portIdx, &extractedData[0] );
            }
        }
    }
}


bool SourcePS::getSignalValue(Pt::uint8_t* data, const Signal* signal, can::drv::Message& msg)
{
    if( signal->signalType() == Signal::ModeDepended)
    {
        if( !can::drv::Driver::extractDataFromCANMsg( data, msg, signal->modePivotBit(), signal->modeBitCount(),
            signal->modeCanDataType() == Signal::Signed, signal->modeByteOrder() != Signal::Intel))
            return false;

        Pt::int32_t modeValue = 0;

        if (signal->modeCanDataType() == Signal::Unsigned)
        {
            if( signal->modeBitCount() <= 32)
                modeValue = *((Pt::int32_t*)&data[0]);

            if( signal->modeBitCount() <= 16)
                modeValue = (Pt::int32_t) *((Pt::int16_t*)&data[0]);

            if( signal->modeBitCount() <= 8)
                modeValue = (Pt::int32_t) *((Pt::int8_t*)&data[0]);
        }
        else if (signal->modeCanDataType() == Signal::Signed)
        {
            if( signal->modeBitCount() <= 32)
                modeValue = (Pt::int32_t)*((Pt::uint32_t*)&data[0]);

            if( signal->modeBitCount() <= 16)
                modeValue = (Pt::int32_t) *((Pt::uint16_t*)&data[0]);

            if( signal->modeBitCount() <= 8)
                modeValue = (Pt::int32_t) *((Pt::uint8_t*)&data[0]);
        }
        
        modeValue = static_cast<Pt::int32_t>( static_cast<double>(modeValue) * signal->modeFactor() + signal->modeOffset() );

        if( modeValue != signal->modeValue() )
            return false;	
    }
    
    if( !can::drv::Driver::extractDataFromCANMsg( data, msg, signal->pivotBit(),   signal->bitCount(), signal->canDataType() == Signal::Signed,
                                          signal->byteOrder() != Signal::Intel))
        return false;

    return true;
}


}}
