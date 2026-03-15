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
#ifndef MPS_EVENT_CONTROLSIGNAL_H
#define MPS_EVENT_CONTROLSIGNAL_H

#include <mps/core/Signal.h>

namespace mps{
namespace eventps{

class ControlSignal : public mps::core::Signal
{
public:
    enum ControlType
    {
        Window,
        Slope,
        Alteration
    };

    ControlSignal(Pt::uint32_t id);
    virtual ~ControlSignal();

    inline Pt::uint8_t ctrlType() const
    {
        return _ctrlType;
    }

    inline void setCtrlType(Pt::uint8_t c)
    {
        _ctrlType = c;
    }

    inline Pt::uint32_t inSignal() const
    {
        return _inSignal;
    }

    inline void setInSignal(Pt::uint32_t inSig)
    {
        _inSignal = inSig;
    }

    inline Pt::uint8_t windowType() const
    {
        return _windowType;
    }

    inline void setWindowType(Pt::uint8_t w)
    {
        _windowType = w;
    }
    
    inline double lower() const
    {
        return _lower;
    }

    inline void setLower(double l)
    {
        _lower = l;
    }

    inline double upper() const
    {
        return _upper;
    }

    inline void setUpper(double l)
    {
        _upper = l;
    }

    inline Pt::uint8_t slopeType() const
    {
        return _slopeType;
    }

    inline void setSlopeType(Pt::uint8_t w)
    {
        _slopeType = w;
    }
    
    inline double slopeValue() const
    {
        return _slopeValue;
    }

    inline void setSlopeValuer(double l)
    {
        _slopeValue = l;
    }

    inline double alteration() const
    {
        return _alteration;
    }

    inline void setAlteration(double l)
    {
        _alteration = l;
    }

    inline double lastValue() const
    {
        return _lastValue;
    }

    inline void setLastValue(double l)
    {
        _lastValue = l;
    }

    inline Pt::uint8_t absolut() const
    {
        return _absolut;
    }

    inline void setAbsolut(Pt::uint8_t a)
    {
        _absolut = a;
    }

    inline Pt::uint8_t signalIf() const
    {
        return _signalIf;
    }

    inline void setSignalIf(Pt::uint8_t a)
    {
        _signalIf = a;
    }

    inline Pt::int32_t outValOnTrue() const
    {
        return _outValOnTrue;
    }

    inline void setOutValOnTrue(Pt::int32_t v)
    {
        _outValOnTrue = v;
    }

    inline Pt::int32_t outValOnFalse() const
    {
        return _outValOnFalse;
    }

    inline void setOutValOnFalse(Pt::int32_t v)
    {
        _outValOnFalse = v;
    }


private:
    Pt::uint8_t _ctrlType;
    Pt::uint32_t _inSignal;
    Pt::uint8_t _windowType;
    double _lower;
    double _upper;
    Pt::uint8_t _slopeType;
    double _slopeValue;
    double _alteration;
    double _lastValue; 
    Pt::uint8_t _absolut;
    Pt::uint8_t _signalIf;
    Pt::int32_t _outValOnTrue;
    Pt::int32_t _outValOnFalse;
};

}}

#endif
