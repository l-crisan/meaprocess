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
#include "ClockPS.h"
#include <map>
#include <vector>
#include "ClockSignal.h"

#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include <Pt/System/Clock.h>

namespace mps{
namespace clock{

ClockPS::ClockPS(void)
: _time(0)
{
    registerProperty( "time", *this, &ClockPS::time, &ClockPS::setTime );
}

ClockPS::~ClockPS(void)
{
}

void ClockPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    ClockSignal*    signal;
    const mps::core::Port*  port = _outputPorts->at(portIdx);
    const mps::core::Sources&   sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];	

    for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
    {
        for( Pt::uint32_t i = 0; i < source.size(); ++i)
        {
            signal = (ClockSignal*) source[i];

            const Pt::uint32_t recordSize = port->sourceDataSize(sourceIdx);
            
            const Pt::uint32_t offsetInSource =  port->signalOffsetInSource(sourceIdx, i);
            Pt::uint8_t* dataPosInRecord = (data  +( (rec * recordSize) + offsetInSource));
            
            Pt::DateTime dateTime;
            
            if(_time == 0)
                dateTime = Pt::System::Clock::getLocalTime();
            else
                dateTime = Pt::System::Clock::getSystemTime();
            
            switch(signal->type())
            {
                case ClockSignal::Year:
                {
                    Pt::uint16_t* year = (Pt::uint16_t*)dataPosInRecord;
                    *year = dateTime.year();
                }
                break;

                case ClockSignal::Month:
                    *dataPosInRecord = dateTime.month();
                break;

                case ClockSignal::Day:
                    *dataPosInRecord = dateTime.day();
                break;

                case ClockSignal::Hour:
                    *dataPosInRecord = dateTime.hour();
                break;

                case ClockSignal::Minute:
                    *dataPosInRecord = dateTime.minute();
                break;

                case ClockSignal::Seconds:
                    *dataPosInRecord = dateTime.second();
                break;

                case ClockSignal::Miliseconds:
                {
                    Pt::uint16_t* msec = (Pt::uint16_t*)dataPosInRecord;
                    *msec = dateTime.msec();
                }
                break;
            }
        }
    }
}

}}

