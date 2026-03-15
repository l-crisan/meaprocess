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
#ifndef MPS_TIMER_ITIMER_H
#define MPS_TIMER_ITIMER_H

#include <mps/timer/TimerListener.h>
#include <Pt/Signal.h>

namespace mps{
namespace timer{

/** @namespace mps::timer
    @brief The timer module for MeaProcess

    If you want to implement your own timer create a shared/dynamic library with the name “mps-timer” implement the interface “ITimer” and export the following functions:
    <br>
    <code>	<br>
&nbsp;&nbsp;&nbsp;&nbsp;extern “C” <br>
&nbsp;&nbsp;&nbsp;&nbsp;{<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;mps::timer::ITimer* createTimer();<br>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;void freeTimer(mps::timer::ITimer* timer);<br>
&nbsp;&nbsp;&nbsp;&nbsp;}<br>
    </code><br>
    as “cdecl” call.

    Replace the original mps-timer library with your library.
*/

 /** @brief Interface for implementing a timer used by the MeaProcess runtime.*/
class ITimer
{
public:
    /** @brief Destructor.*/
    virtual ~ITimer()
    { }

    /** @brief Implement this to add a timer listener.
    *
    *   @param listener The timer listener.*/
    virtual void addTimerListener(TimerListener* listener) = 0;	

    /** @brief Implement this to return the timer resolution in ms.
    *
    *   @return The timer resolution in ns.*/
    virtual Pt::uint64_t getTimerResolution() const = 0;

    /** @brief Implement this to return the timer start time stamp in nano sec.
    *
    *   @return The timer rstart time .*/	
    virtual Pt::uint64_t getStartTime() const = 0;

    /** @brief Implement this to return the current time stamp in nano sec.
    *
    *   @return The time stamp .*/	
    virtual Pt::uint64_t getCurrentTimeStamp() const = 0;

    /** @brief Implement this to start the timer.
    *
    */
    virtual void start(bool synchron = true) = 0;

    /** @brief Implement this to stop the timer.
    *
    */
    virtual void stop(bool internalStop, Pt::uint32_t delayMs) = 0;

    /** @brief Sets the time start point.
    *
    * @param startPointNanoSec Start point in nano seconds.
    */
    virtual void setStartPoint(Pt::uint64_t startPointNanoSec) = 0;

    Pt::Signal<> stopped;

protected:
    /** @brief Default constructor.*/
    ITimer()
    { }

};

}}

#endif
