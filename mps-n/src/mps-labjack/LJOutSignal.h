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
#ifndef MPS_LABJACK_LJOUTCHANNEL_H
#define MPS_LABJACK_LJOUTCHANNEL_H

#include <Pt/Types.h>
#include <mps/core/Object.h>

namespace mps{
namespace labjack{

class LJOutSignal : public mps::core::Object
{
public:
    LJOutSignal(void);

    virtual ~LJOutSignal(void);

    inline Pt::int32_t board() const
    {
        return _board;
    }

    inline void setBoard(Pt::int32_t b)
    {
        _board = b;
    }

    inline Pt::uint8_t channel() const
    {
        return _channel;
    }

    inline void setChannel(Pt::uint8_t ch)
    {
        _channel = ch;
    }

    inline Pt::uint8_t channelType() const
    {
        return _channelType;
    }
    
    inline void setChannelType(Pt::uint8_t ct)
    {
        _channelType = ct;
    }

    inline double scaleMin() const
    {
        return _min;
    }

    inline void setScaleMin(double min)
    {
        _min = min;
    }

    inline double scaleMax() const
    {
        return _max;
    }

    inline void setScaleMax(double max)
    {
        _max = max;
    }
    
    inline Pt::uint32_t signalID() const
    {
        return _signalID;
    }

    inline void setSignalID(Pt::uint32_t id)
    {
        _signalID = id;
    }

    inline Pt::uint32_t rate() const
    {
        return _rate;
    }

    inline void setRate(Pt::uint32_t r)
    {
        _rate = r;
    }

private:
    Pt::uint8_t _channel;
    Pt::uint8_t _channelType;
    double _min;
    double _max;
    Pt::uint32_t _signalID;
    Pt::uint32_t _rate;
    Pt::int32_t _board;
};


}}

#endif