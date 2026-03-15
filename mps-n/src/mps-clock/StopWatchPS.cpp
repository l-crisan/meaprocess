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
#include "StopWatchPS.h"
#include "StopWatchSignal.h"
#include <mps/core/TriggerPort.h>
#include <Pt/System/Clock.h>
#include <mps/core/SignalList.h>

namespace mps{
namespace clock{

StopWatchPS::StopWatchPS(void)
: _counting(false)
{
}

StopWatchPS::~StopWatchPS(void)
{
}

void StopWatchPS::onInitialize()
{
    SynchSourcePS::onInitialize();

    _triggerPort = (mps::core::TriggerPort*) _inputPorts->at(0);
    
    if(_inputPorts->size() > 1)
    {
        mps::core::TriggerPort* resetPort = (mps::core::TriggerPort*) _inputPorts->at(1);

        resetPort->onEventTrigger += Pt::slot(*this, &StopWatchPS::onResetTrigger);
    }

    _triggerPort->onStartTrigger += Pt::slot( *this, &StopWatchPS::onStartTrigger);
    _triggerPort->onStopTrigger += Pt::slot( *this, &StopWatchPS::onStopTrigger);
}

void StopWatchPS::onStart()
{
    _counting = false;
    _deltaTime.setNull();

    if(_triggerPort->triggerType() ==  mps::core::TriggerPort::StopTrigger ||
        _triggerPort->triggerType() == mps::core::TriggerPort::NoTrigger)
        onStartTrigger();

    _triggerPort->start();
    SynchSourcePS::onStart();
}

void StopWatchPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    double time = 0;

    if(_counting)
        time = getTimeSpan(Pt::System::Clock::getSystemTicks() - _startTime);
    else
        time = getTimeSpan(_deltaTime);

    for( Pt::uint32_t i = 0; i < noOfRecords; ++i)
        memcpy(&data[i*sizeof(double)],&time, sizeof(double));
}

double StopWatchPS::getTimeSpan(Pt::Timespan span)
{
    const mps::core::Port* port = _outputPorts->at(0);
    const StopWatchSignal* signal = (const StopWatchSignal*)port->signalList()->at(0);

    double time = 0.0;

    switch(signal->unitIdx())
    {
        case 0: //Milliseconds
            time = (double) span.toUSecs()/1000.0;
        break;
        
        case 1: //Second
            time  = (double) span.toUSecs() / 1000000.0;
        break;
        
        case 2: // Minute
            time = ((double) span.toUSecs() / 1000000.0)/60.0;
        break;

        case 3: //Hour
            time = (((double) span.toUSecs() / 1000000.0)/60.0)/60.0;
        break;
    }

    return time;
}

void StopWatchPS::onStartTrigger()
{
    _counting = true;
    _startTime = Pt::System::Clock::getSystemTicks();
    _deltaTime.setNull();
    _clock.start();
}

void StopWatchPS::onResetTrigger()
{
    _currentTime.setNull();
    _deltaTime.setNull();
    _clock.start();	
    _startTime = Pt::System::Clock::getSystemTicks();
}

void StopWatchPS::onStopTrigger()
{
    if(_counting)
    {
        _deltaTime = _clock.stop();
        _currentTime = Pt::System::Clock::getSystemTicks() - _startTime;
        _counting = false;
    }
}


void StopWatchPS::onUpdateDataValue( Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data )
{

}

}}
