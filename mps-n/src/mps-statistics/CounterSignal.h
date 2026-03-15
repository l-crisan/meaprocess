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
#ifndef MPS_STATISTICS_COUNTERSIGNAL_H
#define MPS_STATISTICS_COUNTERSIGNAL_H

#include <mps/core/Signal.h>

namespace mps {
namespace statistics{

class CounterSignal : public mps::core::Signal
{
public:
    CounterSignal(Pt::uint32_t id);

    virtual ~CounterSignal();

    enum SignalType
    {
        Event,
        Maxima,
        Minima,
        ZeroCrossing,
        ZeroCrossingRising,
        ZeroCrossingFalling
    };

    inline Pt::uint8_t sigType() const
    {
        return _sigType;
    }

    inline void setSigType(Pt::uint8_t t)
    {
        _sigType = t;
    }

    inline Pt::uint32_t inSignal() const
    {
        return _inSignal;
    }

    inline void setInSignal(Pt::uint32_t s)
    {
        _inSignal = s;
    }

private:
    Pt::uint8_t _sigType;
    Pt::uint32_t _inSignal;
};

}}

#endif
