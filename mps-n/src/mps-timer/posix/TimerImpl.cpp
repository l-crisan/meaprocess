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
#include <Pt/System/Thread.h>
#include <Pt/System/Clock.h>
#include <Pt/System/Logger.h>
#include <Pt/Timespan.h>
#include "StopThread.h"

#include <stdio.h>
#include <stdlib.h>
#include <sys/time.h>
#include <sys/mman.h>
#include <pthread.h>
#include <memory.h>
#include <iostream>

PT_LOG_DEFINE("mps.timer.TimerImpl");

namespace mps{
namespace timer{


TimerImpl::TimerImpl(Pt::uint64_t ns)
: _timerCounter(0)
, _timerResolution(ns)
, _timerListener()
, _running(false)
, _thread(0)
, _synchron(true)
, _startPoint(0)
, _startedAt(0)
{
}


TimerImpl::~TimerImpl(void)
{
}


void TimerImpl::stackPrefault()
{
    enum{ MAX_SAFE_STACK = 1*1024*1024 };

    unsigned char dummy[MAX_SAFE_STACK];
    memset(dummy, 0, MAX_SAFE_STACK); 
}


void TimerImpl::setRealtimeThreadPriority()
{
    pthread_t thId = pthread_self();

    //Set the CPU affinity
    {
        cpu_set_t cpuset;
        CPU_ZERO(&cpuset);
        CPU_SET(0, &cpuset);

        pthread_setaffinity_np(thId, sizeof(cpu_set_t), &cpuset);
    }

    //Set the thread priority
    {
        struct sched_param param;

        param.sched_priority = 46;

        if (sched_setscheduler(0, SCHED_FIFO, &param) == -1)
        {
            PT_LOG_ERROR("sched_setscheduler failed");
        }
    }
}


void TimerImpl::addTimerListener( TimerListener* listener)
{
    _timerListener.push_back(listener);
}


Pt::uint64_t TimerImpl::getTimerResolution() const
{
    return _timerResolution;
}


void TimerImpl::setStartPoint(Pt::uint64_t startPointInNanoSec)
{
    _startPoint  = startPointInNanoSec;
}


void TimerImpl::start(bool synchron)
{
    _synchron = synchron;
    _timerCounter = 0;
    _running = true;

    _thread = new Pt::System::AttachedThread(Pt::callable(*this, &TimerImpl::run) );
    _thread->start();
}


void TimerImpl::stop()
{
    if(_thread == 0)
        return;

    _running = false;
    _thread->join();
    delete  _thread;
    _thread = 0;
}


Pt::uint64_t TimerImpl::getStartTime() const
{
    return _startedAt; 
}


Pt::uint64_t TimerImpl::getCurrentTimeStamp() const
{
    struct timespec timeSpec;
    clock_gettime(CLOCK_REALTIME, &timeSpec);
    return ((Pt::uint64_t)timeSpec.tv_sec) * 1000000000 + timeSpec.tv_nsec;
}


bool TimerImpl::wait(struct timespec& t)
{    
    return clock_nanosleep(CLOCK_REALTIME, TIMER_ABSTIME, &t, NULL) == 0;
}


void TimerImpl::nextEvent(struct timespec& t)
{
    t.tv_nsec += _timerResolution;

    while (t.tv_nsec >= 1000000000) 
    {
        t.tv_nsec -= 1000000000;
        t.tv_sec++;
    }
}


void TimerImpl::run()
{
    struct timespec	timeSpec;

    //Set sthread priority to realtine
    setRealtimeThreadPriority();

    //Lock memory
    mlockall(MCL_CURRENT|MCL_FUTURE);

    //Pre-fault our stack
    stackPrefault();
    
    if( _startPoint != 0)
    {//Wait to start at the given time
        timeSpec.tv_sec  = _startPoint / 1000000000;
        timeSpec.tv_nsec = (_startPoint - (timeSpec.tv_sec * 1000000000));
        
        wait(timeSpec);
        _startedAt = _startPoint;
    }
    else
    {//Start now
        clock_gettime(CLOCK_REALTIME, &timeSpec);	
        _startedAt = timeSpec.tv_nsec + (timeSpec.tv_sec * 1000000000);
    }

    while(_running)
    {
        onTimeElapsed(_timerListener);

        nextEvent(timeSpec);

        if(!wait(timeSpec))
        {
            PT_LOG_ERROR("System is overloaded! Runtime stopped!");
            std::cout<<"System is overloaded! Runtime stopped!"<<std::endl;
            
            if(_timerListener.size() != 0)
                _timerListener[0]->onOverload();
            
            _running = false;
        }
    }

    //Unlock memory
    munlockall();
}


void TimerImpl::onTimeElapsed(std::vector<TimerListener*>& listenerArray)
{
    TimerListener* listener;

    for(Pt::uint32_t index = 0; index < listenerArray.size(); index++)
    {
        listener = listenerArray.at(index);

        if(!listener->isActive())
            continue;

        listener->onTimer( _timerCounter );
    }

    _timerCounter++;
}

}}
