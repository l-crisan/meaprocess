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
#ifndef MPS_CLOCK_STOPWATCHPS_H
#define MPS_CLOCK_STOPWATCHPS_H

#include <mps/core/SynchSourcePS.h>
#include <mps/core/TriggerPort.h>

#include <Pt/Timespan.h>
#include <Pt/Connectable.h>
#include <Pt/System/Clock.h>

namespace mps{
namespace clock{

class StopWatchSignal;

class StopWatchPS : public mps::core::SynchSourcePS, public Pt::Connectable
{
public:
    StopWatchPS(void);

    virtual ~StopWatchPS(void);

protected:
    virtual void onInitialize();

    virtual void onStart();

    virtual void onUpdateDataValue( Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data );

    virtual void onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data);

private:
    void onStartTrigger();

    void onStopTrigger();

    void onResetTrigger();

    double getTimeSpan(Pt::Timespan span);

private:
    bool        _counting; 

    Pt::uint32_t _startSource;
    Pt::uint32_t _startSignalPos;
    Pt::uint32_t _startOffsetInSrc;

    Pt::uint32_t _stopSource;
    Pt::uint32_t _stopSignalPos;
    Pt::uint32_t _stopOffsetInSrc;
    Pt::uint8_t	_compBuffer[8];	
    Pt::Timespan _startTime;
    Pt::Timespan _currentTime;
    Pt::Timespan _deltaTime;
    mps::core::TriggerPort* _triggerPort;
    Pt::System::Clock _clock;
};

}}

#endif
