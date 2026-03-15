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
#include "FuncGenSignal.h"
#include <Pt/Math.h>
#include <cmath>

namespace mps{
namespace fgen{

FuncGenSignal::FuncGenSignal(Pt::uint32_t id)
: Signal(id)
, _sigCounter(0)
, _sigCounterInit(0.0)
, _periode(0.0)
, _functionType(Sine)
, _displacementOfPhase(0.0)
, _displacementOfPhaseRad(0.0)
, _valueInc(0)
, _onPeriode(0)
, _parameter(0)
, _totalPoints(0)
, _onPoints(0)
{
    registerProperty( "periode", *this, &FuncGenSignal::periode, &FuncGenSignal::setPeriode );
    registerProperty( "functionType", *this, &FuncGenSignal::functionType, &FuncGenSignal::setFunctionType );
    registerProperty( "displacementOfPhase", *this, &FuncGenSignal::displacementOfPhase, &FuncGenSignal::setDisplacementOfPhase );
    registerProperty( "onPeriode", *this, &FuncGenSignal::onPeriode, &FuncGenSignal::setOnPeriode );	
    registerProperty( "parameter", *this, &FuncGenSignal::parameter, &FuncGenSignal::setParameter );	
}

FuncGenSignal::~FuncGenSignal(void)
{
}

void FuncGenSignal::onInitInstance()
{
    switch(_functionType)
    {
        case Sine:
        case SinePlus:
        case SineMinus:
            _displacementOfPhaseRad	= (_displacementOfPhase * Pt::piDouble<double>())/360.0;
            _valueInc = (Pt::piDouble<double>())/(sampleRate() * (periode()/1000.0));
            _sigCounter	= _displacementOfPhaseRad;
        break;

        case HalfRoundMinus:
        case HalfRoundPlus:
            _displacementOfPhaseRad	= (_displacementOfPhase * Pt::piDouble<double>())/360.0;
            _valueInc = (Pt::piDouble<double>())/(sampleRate() * (periode()/2000.0));
            _sigCounter	= _displacementOfPhaseRad;
        break;

        case ExpPlus:
        case ExpMinus:
            _displacementOfPhaseRad	= (_displacementOfPhase * Pt::piDouble<double>())/360.0;
            _valueInc = 5 / (sampleRate() * (periode() / 1000.0));
            _sigCounter	= -5;
        break;

        case Sinc:
        case SincMinus:
        {
            _sincPeriode = parameter()*Pt::pi<double>();

            if(parameter() == 0)
                _sincPeriode = 5*Pt::pi<double>();

            _displacementOfPhaseRad	= (_displacementOfPhase *_sincPeriode)/360.0;

            _periodeHalf = _sincPeriode/2.0;
            _sigCounter	= -_periodeHalf  + _displacementOfPhaseRad;
            _valueInc = _sincPeriode/(sampleRate() * (periode()/1000.0));
        }
        break;

        case RampUp:
        {
            const double periodeInSec = periode() / 1000.0;

            const double xValueInc = 1.0 / sampleRate();

            //Calculate the slope.
            const double slope = ( ( physMax() - physMin() ) / ( periodeInSec - xValueInc) );

            //Y increment.
            _valueInc = xValueInc * slope;

            const double phase = _displacementOfPhase / 1000.0;

            //Calculate the start y value.

            // y = x*slope + b
            // x = -phase, b = (((_data.Max - _data.Min) / 2) + _data.Min)
            _sigCounter = (slope * -phase) + (((physMax() - physMin()) / 2.0) + physMin());

            if (_sigCounter > physMax() )
                _sigCounter = physMin() + std::abs(physMax() - _sigCounter);

            if (_sigCounter < physMin())
                _sigCounter = physMax() - std::abs(physMin() - _sigCounter);
        }
        break;

        case RampDown:
        {
            const double periodeInSec = periode() / 1000.0;

            const double xValueInc = 1.0 / sampleRate(); 

            //Calculate the slope.
            const double slope = -( ( physMax() - physMin() ) / ( periodeInSec - xValueInc) );

            //Y increment.
            _valueInc = xValueInc * slope;

            const double phase = _displacementOfPhase / 1000.0;

            //Calculate the start y value.

            // y = x*slope + b
            // x = -phase, b = (((_data.Max - _data.Min) / 2) + _data.Min)
            _sigCounter = (slope * -phase) + physMax();//(((physMax() - physMin()) / 2.0) + physMin());

            if (_sigCounter > physMax() )
                _sigCounter = physMin() + std::abs(physMax() - _sigCounter);

            if (_sigCounter < physMin())
                _sigCounter = physMax() - std::abs(physMin() - _sigCounter);
        }
        break;

    case Random:
        if( parameter() == 0)
            setParameter(10);
    break;

        case RectangleMinus:	
        case RectanglePlus:	
        {
            _valueInc = 1;
            _sigCounter = -(_displacementOfPhase/1000)*sampleRate();
            const double periodeInSec = periode() / 1000.0;			
            _totalPoints = static_cast<int>(periodeInSec * sampleRate());
            _onPoints = static_cast<int>((_onPeriode / 1000) * sampleRate());
        }
        break;

        default:
        break;
    }

    _sigCounterInit = _sigCounter;
}

void FuncGenSignal::reset()
{
    _sigCounter = _sigCounterInit;
}

double FuncGenSignal::periode() const
{
    return _periode;
}

void FuncGenSignal::setPeriode(double value)
{
    _periode = value;
}

Pt::uint8_t FuncGenSignal::functionType() const
{
    return (Pt::uint8_t) _functionType;
}

void FuncGenSignal::setFunctionType(Pt::uint8_t type)
{
    _functionType = (FunctionType) type;
}

double FuncGenSignal::displacementOfPhase() const
{
    return _displacementOfPhaseRad;
}

void FuncGenSignal::setDisplacementOfPhase(double phase)
{
    _displacementOfPhase = phase;
}

double FuncGenSignal::counter() const
{ 
    return _sigCounter; 
}
    
void FuncGenSignal::setCounter( double value )
{
    _sigCounter = value;
}

void FuncGenSignal::incCounter()
{ 
    _sigCounter += _valueInc; 
}

void FuncGenSignal::subCounter( double value)
{
    _sigCounter -= value; 
}

double  FuncGenSignal::onPeriode() const
{
    return _onPeriode;
}

void FuncGenSignal::setOnPeriode(double periode)
{
    _onPeriode = periode;
}

void FuncGenSignal::setParameter(double value)
{
    _parameter = value;
}

double FuncGenSignal::parameter() const
{
    return _parameter;
}

int FuncGenSignal::totalPointsInPeriode() const
{
    return _totalPoints;
}

int FuncGenSignal::onPoints() const
{
    return _onPoints;
}

}}
