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
#include <mps/core/TriggerPort.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <mps/core/Signal.h>

namespace mps{
namespace core{

TriggerPort::TriggerPort(void)
: Port()
, _triggerType(TriggerPort::NoTrigger)
, _preEventTime(0.0)
, _postEventTime(0.0)
, _oneStartStopSignal(false)
, _storing(false)
{
    registerProperty( "triggerType", *this, &TriggerPort::triggerType, &TriggerPort::setTriggerType );	
    registerProperty( "preEvenTime", *this, &TriggerPort::preEventTime, &TriggerPort::setPreEventType );	
    registerProperty( "postEvenTime", *this, &TriggerPort::postEventTime, &TriggerPort::setPostEventType );	
    registerProperty( "oneStartStopSignal", *this, &TriggerPort::isOneStartStopSignal, &TriggerPort::setOneStartStopSignal );	
    
    for(Pt::uint32_t i =0 ; i < 8; ++i)
        _compBuffer[i] = 0;
}

TriggerPort::~TriggerPort(void)
{
}
    
Pt::uint8_t TriggerPort::triggerType() const
{
    return _triggerType;
}

void TriggerPort::setTriggerType( Pt::uint8_t type)
{
    _triggerType = (TriggerType) type;
}

double TriggerPort::preEventTime() const
{
    return _preEventTime;
}

void TriggerPort::setPreEventType(double time )
{
    _preEventTime = time;
}

double TriggerPort::postEventTime() const
{
    return _postEventTime;
}

void TriggerPort::setPostEventType(double time )
{
    _postEventTime = time;
}
    
bool TriggerPort::isOneStartStopSignal() const
{
    return _oneStartStopSignal;
}
void TriggerPort::setOneStartStopSignal(bool one )
{
    _oneStartStopSignal = one;
}

void TriggerPort::onInitInstance()
{
    Port::onInitInstance();

    const SignalList* signals = signalList();

    if(signals == 0)
    {
        _triggerSource = (Pt::uint32_t) -1;
        _stopSource = (Pt::uint32_t) -1;
        return;
    }

    _stopSource = (Pt::uint32_t) -1;

    _triggerSignal = signals->at(0);
    _triggerSource	= sourceIndex(0);
    _triggerSignalPos = signalIndexInSource(0);
    _triggerOffsetInSrc = signalOffsetInSource(0);
    
    if( triggerType() == TriggerPort::StartStopTrigger)
    {
        if( !isOneStartStopSignal())
        {
            if( signals->size()  > 1 )
            {
                _stopSignal = signals->at(1);
                _stopSource	= sourceIndex(1);
                _stopSignalPos = signalIndexInSource(1);
                _stopOffsetInSrc = signalOffsetInSource(1);
            }
        }
    }
}

void TriggerPort::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Pt::uint8_t* data)
{   
    if(_triggerSource != sourceIdx && sourceIdx != _stopSource)
    {
        Port::onUpdateDataValue(noOfRecords, sourceIdx, data);
        return;
    }

        Pt::uint32_t triggerSourceRecSize = 0;
        Pt::uint32_t stopSourceRecSize  = 0;
    
    if(_stopSource != (Pt::uint32_t)-1)
        sourceDataSize(_stopSource);

    if(_triggerSource != (Pt::uint32_t) -1)
        triggerSourceRecSize = sourceDataSize(_triggerSource);

    switch(triggerType())
    {
        case TriggerPort::StartTrigger:
        {
            if(_triggerSource == sourceIdx)
            {
                for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
                {
                    const Pt::uint8_t* pdata = &data[triggerSourceRecSize* rec + _triggerOffsetInSrc];

                    if( memcmp(pdata,_compBuffer,_triggerSignal->valueSize()) !=  0)
                        onStartTrigger.send();
                }
            }
        }
        break;
        case TriggerPort::StopTrigger:
        {
            if(_triggerSource == sourceIdx)
            {
                for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
                {
                    const Pt::uint8_t* pdata = &data[triggerSourceRecSize* rec + _triggerOffsetInSrc];

                    if( memcmp(pdata,_compBuffer,_triggerSignal->valueSize()) !=  0)
                        onStopTrigger.send();
                }
            }
        }
        break;
        case TriggerPort::EventTrigger:
        {
            if(_triggerSource == sourceIdx)
            {
                for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
                {
                    const Pt::uint8_t* pdata = &data[triggerSourceRecSize* rec + _triggerOffsetInSrc];
                    
                    if( memcmp(pdata,_compBuffer,_triggerSignal->valueSize()) !=  0)
                         onEventTrigger.send();
                }
            }
        }
        break;
        case TriggerPort::StartStopTrigger:
        {
            if( isOneStartStopSignal())
            {
                if(_triggerSource == sourceIdx)
                {
                    for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
                    {
                        const Pt::uint8_t* pdata = &data[triggerSourceRecSize* rec + _triggerOffsetInSrc];

                        if( memcmp(pdata,_compBuffer,_triggerSignal->valueSize()) !=  0)
                        {
                            if(!_storing )
                            {
                                onStartTrigger.send();
                                _storing = true;
                            }
                        }
                        else if( _storing )
                        {
                            onStopTrigger.send();
                            _storing = false;
                        }
                    }
                }
             }
             else
             {
                if( sourceIdx == _triggerSource)
                {
                    for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
                    {
                        const Pt::uint8_t* pdata = &data[triggerSourceRecSize* rec + _triggerOffsetInSrc];
    
                        if( memcmp(pdata,_compBuffer,_triggerSignal->valueSize()) !=  0 )
                            onStartTrigger.send();
                    }
                }

                if( sourceIdx == _stopSource)
                {
                    for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
                    {
                        const Pt::uint8_t* pdata = &data[stopSourceRecSize* rec + _stopOffsetInSrc];

                        if( memcmp(pdata,_compBuffer,_stopSignal->valueSize()) !=  0 )
                            onStopTrigger.send();
                    }
                }
             }
        }
        break;
    }

    Port::onUpdateDataValue(noOfRecords, sourceIdx, data);
}

}}
