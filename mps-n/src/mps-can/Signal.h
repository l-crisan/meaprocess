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
#ifndef MPS_CAN_SIGNAL_H
#define MPS_CAN_SIGNAL_H

#include <mps/core/Signal.h>

namespace mps{
namespace can{

class Signal : public core::Signal
{
public:
    enum ByteOrder
    {
        Motorola = 0,
        Intel = 1,
    };

    enum SignalType
    {
        Standard,
        ModeSignal,
        ModeDepended,
    };

    enum CANDataType
    {
        Signed,
        Unsigned,
        Float,
        Double
    };

    Signal(Pt::uint32_t id);

    virtual ~Signal();

    inline Pt::uint32_t msgId() const
    {
        return _msgId;
    }
    
    inline void setMsgId(Pt::uint32_t id)
    {
        _msgId = id;
    }

    inline Pt::uint32_t byteCount() const
    {
        return _byteCount;
    }

    inline void setByteCount(Pt::uint32_t bc)
    {
        _byteCount = bc;
    }

    inline Pt::uint8_t  byteOrder() const
    {
        return _byteOrder;
    }

    inline void setByteOrder(Pt::uint8_t o)
    {
        _byteOrder = o;
    }
    
    inline Pt::uint8_t  signalType() const
    {
        return _signalType;
    }

    inline void setSignalType(Pt::uint8_t t)
    {
        _signalType = t;
    }
    
    inline Pt::int32_t  modeValue() const
    {
        return _modeValue;
    }

    inline void setModeValue(Pt::int32_t v)
    {
        _modeValue = v;
    }

    inline Pt::uint8_t pivotBit() const
    {
        return _pivotBit;
    }

    inline void setPivotBit(Pt::uint8_t p)
    {
        _pivotBit = p;
    }
    
    inline Pt::uint8_t bitCount() const
    {
        return _bitCount;
    }

    inline void setBitCount(Pt::uint8_t c)
    {
        _bitCount = c;
    }

    inline Pt::uint8_t canDataType() const
    {
        return _canDataType;
    }

    inline void setCanDataType(Pt::uint8_t s)
    {
        _canDataType = s;
    }

    inline Pt::uint8_t modeBitCount() const
    {
        return _modeBitCount;
    }

    inline void setModeBitCount(Pt::uint8_t m)
    {
        _modeBitCount = m;
    }

    inline Pt::uint8_t modeByteOrder() const
    {
         return _modeByteOrder;
    }

    inline void setModeByteOrder(Pt::uint8_t o)
    {
        _modeByteOrder = o;
    }

    inline double modeFactor() const
    {
        return _modeFactor;
    }
    
    inline void setModeFactor(double f)
    {
        _modeFactor = f;
    }
    
    inline double modeOffset() const
    {
        return _modeOffset;
    }

    inline void setModeOffset(double o)
    {
        _modeOffset = o;
    }
    
    inline Pt::uint8_t modeCanDataType() const
    {
        return _modeCanDataType;
    }
    
    inline void setModeCanDataType(Pt::uint8_t s)
    {
        _modeCanDataType = s;
    }
    
    inline Pt::uint8_t modePivotBit() const
    {
        return _modePivotBit;
    }

    inline void setModePivotBit(Pt::uint8_t p)
    {
        _modePivotBit = p;
    }

private:
    Pt::uint32_t _msgId;
    Pt::uint32_t _byteCount;
    Pt::uint8_t  _byteOrder;
    Pt::uint8_t  _signalType;
    Pt::int32_t  _modeValue;
    Pt::uint8_t  _pivotBit;
    Pt::uint8_t  _bitCount;
    Pt::uint8_t  _canDataType;
    Pt::uint8_t  _modeBitCount;
    Pt::uint8_t  _modeByteOrder;
    double       _modeFactor;
    double       _modeOffset;
    Pt::uint8_t  _modeCanDataType;
    Pt::uint8_t  _modePivotBit;

};

}}

#endif
