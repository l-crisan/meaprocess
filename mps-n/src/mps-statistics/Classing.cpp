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
#include "Classing.h"
#include <mps/core/RuntimeEngine.h>
#include <math.h>
#include <limits>

namespace mps{
namespace statistics{

using namespace std;

bool doubleEquals(double left, double right, double epsilon) 
{
    return (fabs(left - right) < epsilon);
}

bool doubleLess(double left, double right, double epsilon, bool orequal = false) 
{
  if (fabs(left - right) < epsilon) 
    return (orequal);

  return (left < right);
}

bool doubleGreater(double left, double right, double epsilon, bool orequal = false) 
{
  if (fabs(left - right) < epsilon) 
    return (orequal);

  return (left > right);
}

Classing::Classing()
: _lastValue1(0)
, _lastValue2(0)
, _calc(true)
, _firstSampleGet(false)
, _resetNValues(0)
{
    registerProperty( "signal", *this, &Classing::signal, &Classing::setSignal );
    registerProperty( "type", *this, &Classing::type, &Classing::setType );
    registerProperty( "hysteresis", *this, &Classing::hysteresis, &Classing::setHysteresis );
    registerProperty( "lowerLimit", *this, &Classing::lowerLimit, &Classing::setLowerLimit );
    registerProperty( "upperLimit", *this, &Classing::upperLimit, &Classing::setUpperLimit );
    registerProperty( "count", *this, &Classing::count, &Classing::setCount );
    registerProperty( "refValue", *this, &Classing::refValue, &Classing::setRefValue );
    registerProperty( "outSignal", *this, &Classing::outSignal, &Classing::setOutSignal );
    registerProperty( "resetNValues", *this, &Classing::resetNValues, &Classing::setResetNValues );
}

Classing::~Classing()
{
}

bool Classing::init()
{
    _classes.clear();
    _completeData.clear();
    _currentData.clear();
    _classes.resize(_count,0);
    _completeData.resize(_count,0);
    _currentData.resize(_count,0);
    _classesRange.clear();
    
    if( type() == LevelCrossingCounting)
        _classesRange.resize(_count,0.0);	
    else
        _classesRange.resize((_count + 1),0.0);	

    double classRange = lowerLimit();
    double classInc = 0;

    if( type() == LevelCrossingCounting)
        classInc = (upperLimit() - lowerLimit())/(_classes.size() - 1);
    else
        classInc = (upperLimit() - lowerLimit())/(_classes.size());

    for(Pt::uint32_t i = 0; i < _classes.size(); ++i)
    {
        _classesRange[i] = classRange;
        classRange += classInc;
    }

    if( type() != LevelCrossingCounting)
        _classesRange[_count] = classRange;

    if(_hysteresis != 0)
        _hysteresisValue = (classInc * 1.0/_hysteresis) /2.0;
    else
        _hysteresisValue = 0;

    return true;
}

std::string Classing::getClassingName()
{
    switch((int)type())
    {
        case Sampling:
            return "Sampling";
        
        case ZeroCrossPeakCounting:
            return "Zero-crossing peak counting";
        
        case PeakCounting1:
            return "Peak counting 1";		
        
        case PeakCounting2:
            return "Peak counting 2";
        
        case LevelCrossingCounting:
            return "Level crossing counting";
    }

    return "";
}

void Classing::start()
{
    _clock.start();
    _position = 0;
    _sequence = 0;
    _beginPosCrossing = false;
    _endPosCrossing = false;
    _beginNegCrossing = false;
    _endNegCrossing = false;
    _valueCount = 0;
    _posClass = -1;
    _minPosClass = -1;
    _maxPosClass = -1;
    _lastMaxima = _refValue;
    _lastMinima = _refValue;
    _lastValue1 = _refValue;
    _lastValue2 = _refValue;

    for( Pt::uint32_t i = 0; i < _classes.size(); ++i)
    {
        _completeData[i] = 0;
        _currentData[i] = 0;
        _classes[i] = 0;
    }
    _firstSampleGet = true;
}

void Classing::stop()
{
    _time = _clock.stop();
}

void Classing::reset()
{
    start();
}

void Classing::classify(double value)
{
    if( !_calc )
        return;

    _valueCount++;

    for( Pt::uint32_t i = 0; i < _classes.size(); ++i)
    {	
        switch((int)type())
        {
            case Sampling:
            {
                if( (value >= (_classesRange[i] + _hysteresisValue)) && (value < (_classesRange[i+1] - _hysteresisValue)))
                    _classes[i]++;
            }
            break;
            
            case ZeroCrossPeakCounting:
            {
                if( value >= _classesRange[i] && value < _classesRange[i+1])
                {
                    //Calculate maxima over refValue
                    if(_lastValue1 <= (_refValue - _hysteresisValue) &&  value > (_refValue + _hysteresis))
                        _beginPosCrossing = true;
                    
                    if( _beginPosCrossing && _lastValue1 > (_refValue + _hysteresisValue) &&  value < (_refValue - _hysteresisValue))
                    {
                        _beginPosCrossing = false;
                        _endPosCrossing = true;
                    }

                    if( _lastValue1 > _lastValue2 && _lastValue1 > value && _lastValue1 > _lastMaxima)
                    {
                        _lastMaxima = _lastValue1;
                        _maxPosClass = _posClass;
                    }

                    if( _endPosCrossing)
                    {
                        _endPosCrossing = false;
                        if( _maxPosClass > -1)
                            _classes[_maxPosClass]++;
                    }

                    //Calculate minima over refValue
                    if(_lastValue1 >= (_refValue + _hysteresisValue) &&  value < (_refValue - _hysteresisValue))
                        _beginNegCrossing = true;
                    
                    if( _beginNegCrossing && _lastValue1 < (_refValue - _hysteresisValue) &&  value > (_refValue + _hysteresisValue))
                    {
                        _beginNegCrossing = false;
                        _endNegCrossing = true;
                    }

                    if( _lastValue1 < _lastValue2 && _lastValue1 < value && _lastValue1 < _lastMinima)
                    {
                        _lastMinima = _lastValue1;
                        _minPosClass = _posClass;
                    }

                    if( _endNegCrossing)
                    {
                        _endNegCrossing = false;
                        if( _posClass > -1)
                            _classes[_minPosClass]++;
                    }

                    _lastValue2 = _lastValue1;					
                   _lastValue1 = value;
                   _posClass = (int) i;
                }				
            }
            break;
            
            case PeakCounting1:
                if( value >= _classesRange[i] && value < _classesRange[i+1])
                {
                    //Calculate maximas over refValue
                    if(_lastValue1 <= (_refValue - _hysteresisValue) &&  value > (_refValue + _hysteresisValue))
                        _beginPosCrossing = true;
                    
                    if( _beginPosCrossing && _lastValue1 > (_refValue + _hysteresisValue)  &&  value < (_refValue - _hysteresisValue))
                    {
                        _beginPosCrossing = false;
                        _endPosCrossing = true;
                    }

                    if( _lastValue1 > _lastValue2 && _lastValue1 > value && _beginPosCrossing)
                        _classes[_posClass]++;

                    if( _endPosCrossing)
                        _endPosCrossing = false;

                    //Calculate minimas over refValue
                    if(_lastValue1 >= (_refValue + _hysteresisValue) &&  value < (_refValue - _hysteresisValue))
                        _beginNegCrossing = true;
                    
                    if( _beginNegCrossing && _lastValue1 < (_refValue - _hysteresisValue) &&  value > (_refValue + _hysteresisValue))
                    {
                        _beginNegCrossing = false;
                        _endNegCrossing = true;
                    }

                    if( _lastValue1 < _lastValue2 && _lastValue1 < value && _beginNegCrossing)
                        _classes[_posClass]++;

                    if( _endNegCrossing)
                        _endNegCrossing = false;

                    _lastValue2 = _lastValue1;					
                   _lastValue1 = value;
                   _posClass = (int)i;
                }	
            break;
            
            case PeakCounting2:
                if( value >= (_classesRange[i] + _hysteresisValue) && value < (_classesRange[i+1] - _hysteresisValue))
                {
                    if( _lastValue1 > _lastValue2 && _lastValue1 > value)
                        _classes[_posClass]++;

                    if( _lastValue1 < _lastValue2 && _lastValue1 < value)
                        _classes[_posClass]++;

                    _lastValue2 = _lastValue1;					
                   _lastValue1 = value;
                   _posClass = (int)i;
                }	
            break;
            
            case LevelCrossingCounting:
            {
                if(_lastValue1 <= (_refValue + _hysteresisValue) &&  value > (_refValue - _hysteresisValue) )
                    _beginPosCrossing = true;
                
                if( _beginPosCrossing && _lastValue1 > (_refValue + _hysteresisValue)  &&  value < (_refValue - _hysteresisValue))
                {
                    _beginPosCrossing = false;
                    _endPosCrossing = true;
                }

                if( doubleLess(_lastValue1, _classesRange[i], 0.0000001, true) && doubleGreater(value,_classesRange[i],0.0000001) && _beginPosCrossing)
                    _classes[i]++;

                if( _endPosCrossing)
                    _endPosCrossing = false;

                if(_lastValue1 >= (_refValue + _hysteresisValue)  &&  value < (_refValue - _hysteresisValue))
                    _beginNegCrossing = true;
                
                if( _beginNegCrossing && _lastValue1 < (_refValue - _hysteresisValue) &&  value > (_refValue + _hysteresisValue))
                {
                    _beginNegCrossing = false;
                    _endNegCrossing = true;
                }

                if( doubleGreater(_lastValue1 ,_classesRange[i],0.0000001)  && doubleLess(value,_classesRange[i],0.0000001) && _beginNegCrossing)
                    _classes[i]++;

                if( _endNegCrossing)
                    _endNegCrossing = false;
            }   
            break;
        }
    }

    if(type() == LevelCrossingCounting)
        _lastValue1 = value;
    
    _sequence++;

    if(_sequence == _classes.size())
    {
        memcpy(&_completeData[0], &_classes[0], sizeof(Pt::uint32_t) *_classes.size());            
        _sequence = 0;
    }

    if( _valueCount == _resetNValues)
        reset();
}

Pt::uint32_t Classing::getNextData()
{
    if(_firstSampleGet)
    {
        _firstSampleGet = false;
        return std::numeric_limits<Pt::uint32_t>::max();
    }

    if( _position == _currentData.size())
    {
        memcpy(&_currentData[0], &_completeData[0], sizeof(Pt::uint32_t) *_completeData.size());            
        _position = 0;
        
        return std::numeric_limits<Pt::uint32_t>::max();
    }

    Pt::uint32_t sample = _currentData[_position];
    _position++;

    return sample;
}

void Classing::close()
{	
}

}}
