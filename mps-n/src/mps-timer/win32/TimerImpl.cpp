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
#include <Windows.h>

PT_LOG_DEFINE("mps.timer.TimerImpl");

namespace mps{
namespace timer{

LARGE_INTEGER getFILETIMEoffset()
{
    SYSTEMTIME s;
    FILETIME f;
    LARGE_INTEGER t;

    s.wYear = 1970;
    s.wMonth = 1;
    s.wDay = 1;
    s.wHour = 0;
    s.wMinute = 0;
    s.wSecond = 0;
    s.wMilliseconds = 0;
    SystemTimeToFileTime(&s, &f);
    t.QuadPart = f.dwHighDateTime;
    t.QuadPart <<= 32;
    t.QuadPart |= f.dwLowDateTime;
    return (t);
}


TimerImpl::TimerImpl(Pt::uint64_t ns)
: _timerCounter(0)
, _timerResolution(ns)
, _timerListener()
, _running(false)
, _thread(0)
, _startPoint(0)
{
    _hTimer = CreateWaitableTimer(NULL, TRUE, NULL); 	
}

TimerImpl::~TimerImpl(void)
{
    CloseHandle(_hTimer);
}

void TimerImpl::wait()
{    
    LARGE_INTEGER liDueTime; 
    
    liDueTime.QuadPart =  (_timerResolution/100) * -1; 
     
    SetWaitableTimer(_hTimer, &liDueTime, 0, NULL, NULL, 0);
        
    WaitForSingleObject(_hTimer, INFINITE);
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

void TimerImpl::setRealtimeThreadPriority()
{
    DWORD mask = 1;
    SetPriorityClass(GetCurrentProcess(), HIGH_PRIORITY_CLASS);
    SetThreadAffinityMask(GetCurrentThread(), mask);
    SetThreadPriority(GetCurrentThread(),THREAD_PRIORITY_HIGHEST);	
}

Pt::uint64_t TimerImpl::getCurrentTimeStamp() const
{
    LARGE_INTEGER    t;
    FILETIME         f;
    double           microseconds;
    LARGE_INTEGER    offset;
    double           frequencyToMicroseconds;
    int              initialized = 0;
    BOOL             usePerformanceCounter = 0;

    if (!initialized) {
        LARGE_INTEGER performanceFrequency;
        initialized = 1;
        usePerformanceCounter = QueryPerformanceFrequency(&performanceFrequency);
        if (usePerformanceCounter) {
            QueryPerformanceCounter(&offset);
            frequencyToMicroseconds = (double)performanceFrequency.QuadPart / 1000000.;
        } else {
            offset = getFILETIMEoffset();
            frequencyToMicroseconds = 10.;
        }
    }
    if (usePerformanceCounter) QueryPerformanceCounter(&t);
    else {
        GetSystemTimeAsFileTime(&f);
        t.QuadPart = f.dwHighDateTime;
        t.QuadPart <<= 32;
        t.QuadPart |= f.dwLowDateTime;
    }

    t.QuadPart -= offset.QuadPart;
    microseconds = (double)t.QuadPart / frequencyToMicroseconds;
    t.QuadPart = (long long) microseconds;

    return  t.QuadPart * 1000 +  t.QuadPart % 1000;
}

void TimerImpl::run()
{
    Pt::System::Clock	clock;
    Pt::Timespan		timeSpan;
    Pt::int64_t			events		= 0;
    Pt::uint64_t		resolution  = _timerResolution / 1000;
    Pt::int64_t			delta		= resolution;	

    setRealtimeThreadPriority();

    if( _startPoint != 0)
    {
        while(true)
        {
            if( _startPoint >= getCurrentTimeStamp())
                break;

            Pt::System::Thread::yield();
        }
    }

    _startedAt = getCurrentTimeStamp();

    clock.start();
#if _DEBUG
    _synchron = false;
#endif

    while(_running)
    {
        delta += timeSpan.toUSecs();
    
        events = (delta / resolution);
        delta  = (delta % resolution);
        
        //Generate the events
        for(Pt::int32_t ev = 0; (ev < events) && _running; ev++)
            onTimeElapsed( _timerListener );

        //wait time
        wait();
        
        timeSpan = clock.stop();
        
        if(timeSpan.toMSecs() > 5000 && _timerListener.size() != 0 && _synchron)
        {
            PT_LOG_ERROR("System is overloaded: dT = "<< timeSpan.toMSecs()<<"(ms)");
            _timerListener[0]->onOverload();
            return;
        }

        clock.start();
    }
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
