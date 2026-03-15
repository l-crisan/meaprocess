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
#ifndef MPS_STATISTICS_CLASSING_H
#define MPS_STATISTICS_CLASSING_H

#include <Pt/Types.h>
#include <Pt/System/Clock.h>
#include <mps/core/Signal.h>
#include <mps/core/Object.h>

#include <fstream>

namespace mps{
namespace statistics{

class Classing : public mps::core::Object
{
public:
    Classing();
    virtual ~Classing();

    enum ClassingType
    {
        Sampling,
        ZeroCrossPeakCounting,
        PeakCounting1,
        PeakCounting2,
        LevelCrossingCounting
    };

    inline Pt::uint32_t signal() const
    {
        return _signal;
    }

    inline void setSignal(Pt::uint32_t s)
    {
        _signal = s;
    }

    inline Pt::uint8_t type() const
    {
        return _type;
    }

    inline void setType(Pt::uint8_t t)
    {
        _type = t;
    }

    inline std::string file() const
    {
        return _file;
    }

    inline void setFile(const std::string& f)
    {
        _file = f;
    }

    inline double hysteresis() const
    {
        return _hysteresis;
    }

    inline void setHysteresis(double h)
    {
        _hysteresis = h;
    }

    inline double lowerLimit() const
    {
        return _lowerLimit;
    }

    inline void setLowerLimit(double h)
    {
        _lowerLimit = h;
    }

    inline double upperLimit() const
    {
        return _upperLimit;
    }

    inline void setUpperLimit(double h)
    {
        _upperLimit = h;
    }

    inline Pt::uint32_t count() const
    {
        return _count;
    }

    inline void setCount(Pt::uint32_t count)
    {
        _count = count;
    }

    inline double refValue() const
    {
        return _refValue;
    }

    inline void setRefValue(double h)
    {
        _refValue = h;
    }

   
    inline Pt::uint32_t outSignal() const
    {
        return _outSignal;
    }

    inline void setOutSignal(Pt::uint32_t o)
    {
        _outSignal = o;
    }

    inline void calc(bool calc)
    {
        _calc = calc;
    }

    inline mps::core::Signal* outputSignal() const 
    {
        return _outSignalInst;
    }

    void setOutputSignal(mps::core::Signal* sig)
    {
        _outSignalInst = sig;
    }

    bool init();

    void start();

    void reset();

    void classify(double value);

    void stop();

    void close();
 

    std::string getClassingName();

    inline const std::vector<Pt::uint32_t>& data() const
    {
        return _classes;
    }

    inline const std::vector<Pt::uint32_t>& completeData() const
    {
        return _completeData;
    }

    inline Pt::uint32_t resetNValues() const
    {
        return _resetNValues;
    }

    inline void setResetNValues(Pt::uint32_t values)
    {
        _resetNValues = values;
    }

    Pt::uint32_t getNextData();

private:	

    double       _lastValue1;
    double       _lastValue2;
    bool _beginPosCrossing;
    bool _endPosCrossing;
    Pt::uint32_t _outSignal;

    bool _beginNegCrossing;
    bool _endNegCrossing;
    bool _calc;

    int  _posClass;
    int _maxPosClass;
    int _minPosClass;
    double _lastMaxima;
    double _lastMinima;

    Pt::uint32_t _signal;
    Pt::uint8_t _type;
    std::string _file;
    double _hysteresis;
    double _lowerLimit;
    double _upperLimit;
    Pt::uint32_t _count;
    double _refValue;
    std::vector<Pt::uint32_t> _classes;
    std::vector<Pt::uint32_t> _completeData;
    std::vector<Pt::uint32_t> _currentData;
    std::vector<double> _classesRange;
    Pt::System::Clock _clock;
    Pt::Timespan _time;
    mps::core::Signal* _outSignalInst;
    Pt::uint32_t _position;
    Pt::uint32_t _sequence;
    bool _firstSampleGet;
    Pt::uint32_t _resetNValues;
    Pt::uint32_t _valueCount;
    double _hysteresisValue;
    double _hystLastValue;
};

}}

#endif
