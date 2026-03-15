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
#ifndef MPS_CLOCK_TIMERSIGNAL_H
#define MPS_CLOCK_TIMERSIGNAL_H

#include <Pt/Api.h>
#include <mps/core/Signal.h>

namespace mps{
namespace clock{

class TimerSignal : public mps::core::Signal
{
public:
    TimerSignal(Pt::uint32_t id);

    virtual ~TimerSignal(void);


    inline Pt::uint32_t interval() const
    {
        return _interval * 1000000;
    }

    inline void setInterval(Pt::uint32_t i)
    {
        _interval = i;
    }

    inline Pt::uint32_t counter() const
    {
        return _counter;
    }

    inline void incCounter() 
    {
        _counter++;
    }

    inline void setCounter(Pt::uint32_t c)
    {
        _counter = c;
    }

private:
    Pt::uint32_t _interval;
    Pt::uint32_t _counter;	
};

}}
#endif
