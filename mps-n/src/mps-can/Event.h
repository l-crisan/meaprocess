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
#ifndef MPS_CAN_EVENT_H
#define MPS_CAN_EVENT_H

#include <mps/core/Object.h>
#include <Pt/Byteorder.h>
#include <string>
#include <sstream>

namespace mps{
namespace can{

class Event : public core::Object
{
public:
    enum Operation
    {
        NotEq,
        Eq,
        Ls,
        Le,
        Gr,
        Ge
    };

    Event(void);

    virtual ~Event(void);

    inline Pt::uint32_t signal() const
    {
        return _signal;
    }

    inline void setSignal(Pt::uint32_t sig)
    {
        _signal = sig;
    }

    inline Pt::uint8_t operation() const
    {
        return _operation;
    }

    inline void setOperation(Pt::uint8_t o)
    {
        _operation = o;
    }

    inline double limit() const
    {
        return _limit;
    }

    inline void setLimit(double l)
    {
        l = _limit;
    }

    inline Pt::uint32_t id() const
    {
        return _id;
    }

    inline void setID(Pt::uint32_t id)
    {
        _id = id;
    }

    inline std::string data() const
    {
        return "";
    }

    inline void setData(std::string d)
    {
        Pt::uint64_t binData = 0;
        std::stringstream ss;
        ss<<std::hex;
        ss<<d;
        ss>>binData;
        binData = Pt::hostToBe(binData);
        memcpy(&_byteData[0],&binData,8);
    }

    inline const std::vector<Pt::uint8_t> byteData() const
    {
        return _byteData;
    }

private:
    Pt::uint32_t _signal;
    double       _limit;
    Pt::uint8_t  _operation;
    Pt::uint32_t _id;
    std::vector<Pt::uint8_t> _byteData;
};


}}

#endif
