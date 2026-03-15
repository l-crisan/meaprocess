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
#ifndef MPS_LINGUISTICVAR_H
#define MPS_LINGUISTICVAR_H

#include <Pt/Types.h>
#include <mps/core/Object.h>

namespace mps {
namespace controller{

class LinguisticVar : public mps::core::Object
{
public:
    LinguisticVar();

    virtual ~LinguisticVar();
        
    inline const std::string& varName() const
    {
        return _varName;
    }

    inline void setVarName( const std::string& n)
    {
        _varName = n;
    }

    inline double x1() const
    {
        return _x1;
    }

    inline void setX1(double x)
    {
        _x1 = x;
    }
    
    inline double x2() const
    {
        return _x2;
    }

    inline void setX2(double x)
    {
        _x2 = x;
    }

    inline double x3() const
    {
        return _x3;
    }

    inline void setX3(double x)
    {
        _x3 = x;
    }


    inline double x4() const
    {
        return _x4;
    }

    inline void setX4(double x)
    {
        _x4 = x;
    }


    inline Pt::uint32_t signal() const
    {
        return _signal;
    }

    inline void setSignal(Pt::uint32_t s)
    {
        _signal = s;
    }

    void setPosition(int pos)
    {
        _pos = pos;
    }
    void calcFuzzyValue(double value);

    inline double fuzzyValue() const 
    {
        return _fuzzyValue;
    }

private:
    std::string _varName;
    double _x1;
    double _x2;
    double _x3;
    double _x4;
    Pt::uint32_t _signal;
    double _fuzzyValue;
    int _pos;
};

}}

#endif
