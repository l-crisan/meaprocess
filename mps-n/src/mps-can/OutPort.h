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
#ifndef MPS_CAN_OUTPORT_H
#define MPS_CAN_OUTPORT_H

#include <mps/core/Port.h>

namespace mps{
namespace can{

class OutPort : public mps::core::Port
{
public:
    OutPort();

    virtual ~OutPort();

    inline Pt::uint32_t bitRate() const
    {
        return _bitrate;
    }

    inline void setBitRate(Pt::uint32_t r)
    {
        _bitrate = r;
    }

    inline Pt::uint8_t extendedId() const
    {
        return _extendedId;
    }

    inline void setExtendedId(Pt::uint8_t e)
    {
        _extendedId = e;
    }

    inline Pt::uint8_t tact() const
    {
        return _tact;
    }

    inline void setTact(Pt::uint8_t t)
    {
        _tact = t;
    }


    inline Pt::uint32_t mask() const
    {
        return _mask;
    }

    inline void setMask(Pt::uint32_t m)
    {
        _mask = m;
    }

    inline Pt::uint32_t code() const
    {
        return _code;
    }

    inline void setCode(Pt::uint32_t c)
    {
        _code = c;
    }
private:
    Pt::uint32_t _bitrate;
    Pt::uint8_t  _extendedId;
    Pt::uint8_t  _tact;
    Pt::uint32_t _mask;
    Pt::uint32_t _code;

};

}}
#endif
