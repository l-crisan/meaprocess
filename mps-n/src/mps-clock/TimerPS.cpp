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
#include "TimerPS.h"
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Port.h>
#include "TimerSignal.h"

namespace mps{
namespace clock{

TimerPS::TimerPS()
{
}

TimerPS::~TimerPS()
{
}
    
void TimerPS::onInitialize()
{
    SynchSourcePS::onInitialize();
}

void TimerPS::onStart()
{
    SynchSourcePS::onStart();

    const mps::core::Port* port = _outputPorts->at(0);
    const mps::core::SignalList* signalList = port->signalList();

    for(Pt::uint32_t i = 0; i < signalList->size(); ++i)
    {
        TimerSignal* signal = (TimerSignal*) signalList->at(i);
        signal->setCounter(0);
    }
}

void TimerPS::onStop()
{
    SynchSourcePS::onStop();
}

void TimerPS::onDeinitialize()
{
    SynchSourcePS::onDeinitialize();
}

void TimerPS::onTimer( Pt::uint32_t timerCounter )
{
    SynchSourcePS::onTimer(timerCounter);

    const mps::core::Port* port = _outputPorts->at(0);
    const mps::core::SignalList* signalList = port->signalList();

    for(Pt::uint32_t i = 0; i < signalList->size(); ++i)
    {
        TimerSignal* signal = (TimerSignal*) signalList->at(i);
        signal->incCounter();
    }
}

void TimerPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    Pt::uint8_t value = 0;
    
    const mps::core::Port* port = _outputPorts->at(portIdx);
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
    const Pt::uint32_t recordSize = port->sourceDataSize(sourceIdx);

    for(Pt::uint32_t sig = 0; sig < source.size(); ++sig)
    {
        TimerSignal* signal = (TimerSignal*)source[sig];

        if((signal->counter() % (signal->interval()/ProcessStation::timerResolution()) == 0) && signal->counter() != 0)
        {
            value = 1;
            signal->setCounter(0);
        }
        else
        {
            value = 0;
        }

        const Pt::uint32_t offset = port->signalOffsetInSource(sourceIdx, sig);

        for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            data[rec*recordSize+ offset] = value;
            value = 0;
        }
    }
}

}}
