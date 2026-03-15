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
#include "LinguisticVar.h"

namespace mps {
namespace controller{

LinguisticVar::LinguisticVar()
{
    registerProperty("name", *this, &LinguisticVar::varName, &LinguisticVar::setVarName);
    registerProperty("x1", *this, &LinguisticVar::x1, &LinguisticVar::setX1);
    registerProperty("x2", *this, &LinguisticVar::x2, &LinguisticVar::setX2);
    registerProperty("x3", *this, &LinguisticVar::x3, &LinguisticVar::setX3);
    registerProperty("x4", *this, &LinguisticVar::x4, &LinguisticVar::setX4);
    registerProperty("signal", *this, &LinguisticVar::signal, &LinguisticVar::setSignal);
}

LinguisticVar::~LinguisticVar()
{
}

void LinguisticVar::calcFuzzyValue(double value)
{
    _fuzzyValue = 0;

    if( _pos < 0)
    {
        if(value < _x3)
        {
            _fuzzyValue = 1;
        }
        else if( value >= _x3 && value < _x4)
        {
            _fuzzyValue = (_x4 -value)/(_x4 -_x3);
        }
        else
        {
            _fuzzyValue = 0;
        }
    }
    else if( _pos == 0)
    {
        if( value >= _x1 && value < _x2)
        {
            _fuzzyValue = (value - _x1)/(_x2 - _x1);
        }
        else if( value >= _x2 && value < _x3)
        {
            _fuzzyValue = 1;
        }
        else if( value >= _x3 && value < _x4)
        {
            _fuzzyValue = (_x4 -value)/(_x4 -_x3);
        }
        else
        {
            _fuzzyValue = 0;
        }
    }
    else
    {
        if( value >= _x1 && value < _x2)
        {
            _fuzzyValue = (value - _x1)/(_x2 - _x1);
        }
        else if( value >= _x2)
        {
            _fuzzyValue = 1;
        }
        else
        {
            _fuzzyValue = 0;
        }
    }
}

}}
