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
#ifndef MPS_OBD2_OBD2SIGNAL_H
#define MPS_OBD2_OBD2SIGNAL_H

#include <mps/core/Signal.h>

namespace mps{
namespace obd2{

class OBD2Signal : public mps::core::Signal
{
public:
    OBD2Signal(Pt::uint32_t id);

    virtual ~OBD2Signal(void);

    inline  Pt::uint8_t sid() const
    {
        return _sid;
    }

    inline void setSid(Pt::uint8_t s)
    {
        _sid = s;
    }

    inline  Pt::uint8_t pid() const
    {
        return _pid;
    }

    inline void setPid(Pt::uint8_t p)
    {
        _pid = p;
    }

    inline  Pt::int8_t sensor() const
    {
        return _sensor;
    }

    inline void setSensor(Pt::int8_t s)
    {
        _sensor = s;
    }

    inline  Pt::uint8_t byteOffset() const
    {
        return _byteOffset;
    }

    inline void setByteOffset(Pt::uint8_t b)
    {
        _byteOffset = b;
    }

    inline void setValid(bool v)
    {
        _valid = v;
    }

    inline bool valid() const
    {
        return _valid;
    }

    inline Pt::uint8_t totalDataSize() const
    {
        return _totalDataSize;
    }

    inline void setTotalDataSize(Pt::uint8_t t)
    {
        _totalDataSize = t;
    }
private:
    Pt::uint8_t _sid;
    Pt::uint8_t _pid;
    Pt::int8_t _sensor;
    Pt::uint8_t _totalDataSize;
    Pt::uint8_t _byteOffset;
    bool        _valid;
};

}}

#endif
