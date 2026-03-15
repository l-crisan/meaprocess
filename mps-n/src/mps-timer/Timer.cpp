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
#include "TimerImpl.h"
#include "Timer.h"
#include "mps-timer.h"
#include "StopThread.h"

extern "C"
{

MPS_TIMER_API mps::timer::ITimer* createTimer(Pt::uint64_t resolutionMSec)
{
    return new mps::timer::Timer(resolutionMSec);
}

MPS_TIMER_API void freeTimer(mps::timer::ITimer* t)
{
    delete t;
}

}

namespace mps {
namespace timer{

Timer::Timer(Pt::uint64_t msec)
: _impl( new TimerImpl(msec))
{
}

Timer::~Timer(void)
{
    delete _impl;
}

Pt::uint64_t Timer::getStartTime() const
{
    return _impl->getStartTime();
}

Pt::uint64_t Timer::getCurrentTimeStamp() const
{
    return _impl->getCurrentTimeStamp();
}

void Timer::setStartPoint(Pt::uint64_t startPointNanoSec)
{
    _impl->setStartPoint(startPointNanoSec);
}

void Timer::addTimerListener( TimerListener* listener)
{
    _impl->addTimerListener(listener);
}

Pt::uint64_t Timer::getTimerResolution() const
{
    return _impl->getTimerResolution();
}

void Timer::start(bool synchron)
{
    _impl->start(synchron);
}

void Timer::stop(bool internalStop, Pt::uint32_t delayMs)
{
    if(internalStop)
    {
        StopThread* stopThread = new StopThread(stopped, _impl, delayMs);
        stopThread->start();
    }
    else
    {
        _impl->stop();
        stopped.send();
    }
}

}}
