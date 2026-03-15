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
#include <mps/filter/Filter.h>
#include "BesselCoef.h"
#include "ButterworthCoef.h"
#include "ChebyshevCoef.h"
#include "EllipticCoef.h"
 
namespace mps{
namespace filter{
 
bool Filter::calcCoef(FilterType type, Pt::uint32_t order, Pt::uint32_t lowerFrequency, Pt::uint32_t upperFrequency, 
                      Pt::uint32_t transitionBandWidth, Pt::uint32_t stopBandAttenuation, Pt::uint32_t sampleRate)
{
    switch(type)
    {
        case Butterworth:
        {
            ButterworthCoef coef(order, sampleRate, lowerFrequency, upperFrequency, transitionBandWidth, stopBandAttenuation);
            if(!coef.calcCoef(_a, _b))
                return false;
        }
        break;

        case Chebyshev:
        {
            ChebyshevCoef coef(order, sampleRate, lowerFrequency, upperFrequency, transitionBandWidth, stopBandAttenuation);
            if(!coef.calcCoef(_a, _b))
                return false;

        }
        break;

        case Bessel:
        {
            BesselCoef coef(order, sampleRate, lowerFrequency, upperFrequency, transitionBandWidth, stopBandAttenuation);
            
            if(!coef.calcCoef(_a, _b))
                return false;
        }
        break;

        case Elliptic:
        {
            EllipticCoef coef(order, sampleRate, lowerFrequency, upperFrequency, transitionBandWidth, stopBandAttenuation);
            
            if(!coef.calcCoef(_a, _b))
                return false;
        }
        break;
    }

    _zx.resize(_b.size(),0.0);
    _zy.resize(_a.size(),0.0);

    return true;
}

void Filter::start()
{
    for(Pt::uint32_t i = 0; i < _zx.size(); ++i)
        _zx[i] = 0.0;

    for(Pt::uint32_t i = 0; i < _zy.size(); ++i)
        _zy[i] = 0.0;
}

double Filter::filter(double value)
{
    //Shift zx
    for(Pt::uint32_t i = _zx.size() - 2; i > -1; --i)
        _zx[i+1] = _zx[i];

    if(_zx.size() > 0)
        _zx[0] = value;

    //Calculate the new y;
    double y = 0.0;

    for(Pt::uint32_t i = 0; i < _b.size(); ++i)
        y += _zx[i] * _b[i];

    for(Pt::uint32_t i = 1; i < _a.size(); ++i)
        y -= _zy[i-1] * _a[i];

    //Shift y
    for(Pt::uint32_t i = _zy.size() - 2; i >-1; --i)
        _zy[i+1] = _zy[i];

    if(_zy.size() > 0)
        _zy[0] = y;

    return y;
}

}}
