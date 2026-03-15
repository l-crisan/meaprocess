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
#ifndef MPS_TIMERIMPL_H
#define MPS_TIMERIMPL_H

#include <vector>
#include <Pt/Types.h>
#include <Pt/System/Thread.h>
#include <mps/timer/TimerListener.h>

namespace mps{
namespace timer{

class TimerImpl 
{
public:
    TimerImpl(Pt::uint64_t ns);

    virtual ~TimerImpl(void);

    void addTimerListener( TimerListener* listener);
    
    Pt::uint64_t getTimerResolution() const;

    Pt::uint64_t getStartTime() const;
    void setStartPoint(Pt::uint64_t startPointNanoSec);

    void start(bool synchron = true);
    void stop();

    Pt::uint64_t getCurrentTimeStamp() const;

private:
    void onTimeElapsed(std::vector<TimerListener*>& listenerArray);
    void run();
    void setRealtimeThreadPriority();
    void wait();


private:
    Pt::uint32_t                _timerCounter;
    Pt::uint64_t                _timerResolution;
    std::vector<TimerListener*> _timerListener;
    bool                        _running;
    Pt::System::AttachedThread* _thread;
    bool                        _synchron;
    Pt::uint64_t                _startPoint;
    Pt::uint64_t                _startedAt;
    void*                       _hTimer;
};

}}

#endif
