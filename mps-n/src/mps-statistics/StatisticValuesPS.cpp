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
#include "StatisticValuesPS.h"
#include <mps/core/Port.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <Pt/Math.h>
#include <cmath>
#include <algorithm>
#include <limits>

namespace mps{
namespace statistics{

StatisticValuesPS::StatisticValuesPS()
: _samples(10)
{
    registerProperty( "samples", *this, &StatisticValuesPS::samples, &StatisticValuesPS::setSamples );
}

StatisticValuesPS::~StatisticValuesPS()
{
}

void StatisticValuesPS::onInitInstance()
{
    ProcessStation::onInitInstance();
    _data.resize(_samples);
    _sortedData.resize(_samples);
    const mps::core::Port*  inPort  = _inputPorts->at(0);
    _inSignal = inPort->signalList()->at(0);
    _sigSrcIdx  = inPort->sourceIndex(0);
    _sigOffsetInSource = inPort->signalOffsetInSource(0);
    _sourceSize = inPort->sourceDataSize(_sigSrcIdx);

    _calcMin = false;
    _calcMax = false;
    _calcEffect = false;
    _calcMean = false;
    _calcVariance = false;
    _calcRMS = false;
    _calcMedian = false;
    _calcPeakPeak = false;
    _calcGeoMean = false;
    _calcHarmMean = false;
    _calcQuadMean = false;
    _calcCubicMean = false;
    const mps::core::Port* outPort = _outputPorts->at(0);

    for( Pt::uint32_t i = 0; i < outPort->signalList()->size(); ++i)
    {
        const StatisticValueSignal* outSignal = (StatisticValueSignal*) outPort->signalList()->at(i);

        switch(outSignal->sigType())
        {
            case StatisticValueSignal::Minimum:
                _calcMin = true;
            break;

            case StatisticValueSignal::Maximum:
                _calcMax = true;
            break;

            case StatisticValueSignal::EffectiveValue:
                _calcEffect = true;
            break;

            case StatisticValueSignal::AritmeticMean:
                _calcMean = true;
            break;

            case StatisticValueSignal::GeometricMean:
                _calcGeoMean = true;
            break;

            case StatisticValueSignal::HarmonicMean:
                _calcHarmMean = true;
            break;

            case StatisticValueSignal::QuadraticMean:
                _calcQuadMean = true;
            break;

            case StatisticValueSignal::CubicMean:
                _calcCubicMean = true;
            break;

            case StatisticValueSignal::Variance:
                _calcVariance = true;
            break;

            case StatisticValueSignal::Median:
                _calcMedian = true;
            break;

            case StatisticValueSignal::PeakPeakValue:
            {
                _calcPeakPeak = true;
            }
            break;
        }
    }

    const mps::core::Sources& sources = outPort->sources();
    const std::vector<mps::core::Signal*>& source = sources[0];
    _outputData.resize(static_cast<Pt::uint32_t>(source.size() *source[0]->sampleRate() *2));
}

void StatisticValuesPS::onStart()
{
    memset(&_data[0],0, sizeof(double) * _data.size());
    _position = 0;

    ProcessStation::onStart();
}

void StatisticValuesPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if( sourceIdx != _sigSrcIdx)
        return;

    double min = 0;
    double max = 0;
    double effectiveValue = 0;
    double rmsDerivation = 0;
    double variance = 0;
    double mean = 0;
    double sumPow2 = 0;
    double sum = 0;
    double sumDiff = 0;
    double median = 0;
    double geoMean = 0;
    double harMean = 0;
    double quadMean = 0;
    double cubicMean = 0;

    mps::core::Port* outPort = _outputPorts->at(0);
    const mps::core::Sources& sources = outPort->sources();
    const std::vector<mps::core::Signal*>& source = sources[0];
    
    for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
    {
        const double value = _inSignal->scaleValue( &data[_sourceSize * rec + _sigOffsetInSource] );

        if(_position == _data.size())
            _position = 0;

        _data[_position] = value;
        _position++;

        //Mean, Effective, Min, Max , PeakPeak
        min = std::numeric_limits<Pt::int32_t>::max();
        max = std::numeric_limits<Pt::int32_t>::min();
        sum = 0;
        sumPow2 = 0;
        geoMean = 1;
        harMean = 0;
        quadMean = 0;
        cubicMean = 0;

        for(Pt::uint32_t i = 0; i < _data.size(); ++i)
        {
            min = std::min(min, _data[i]);
            max = std::max(max, _data[i]);
            sumPow2 += (_data[i] * _data[i]);
            sum  += _data[i];     
            
            if( _calcGeoMean)
                geoMean *= _data[i];

            if( _calcHarmMean)
                harMean += 1.0/_data[i];

            if( _calcCubicMean)
                cubicMean += (_data[i] * _data[i] * _data[i]);
        }

        if( _calcGeoMean)
        {
            double sign = 1;
            
            if( geoMean < 0)
                sign = -1;

            if(samples() % 2 != 0)
            {
                geoMean = sign * std::pow(std::abs(geoMean), 1.0/samples());
            }
            else
            {
                if(geoMean < 0)
                    geoMean = 0;

                geoMean = std::pow(geoMean, 1.0/samples());
            }
        }

        if( _calcHarmMean && harMean != 0)
            harMean = samples() / harMean;

        if( _calcQuadMean)
        {
            quadMean = sumPow2 / samples();
            
            if( quadMean < 0)
                quadMean = 0;

            quadMean = sqrt(quadMean);
        }

        if( _calcCubicMean)
        {
            cubicMean = cubicMean/samples();

            double sign = 1;
            
            if( cubicMean < 0)
                sign = -1;

            cubicMean = sign * std::pow(std::abs(cubicMean), 1.0/3.0);
        }

        effectiveValue = sumPow2/_data.size();
        mean = sum/_data.size();

        if( _calcVariance || _calcRMS)
        {
            //Variance, RMS
            sumDiff = 0;
            for(Pt::uint32_t i = 0; i < _data.size(); ++i)
            {
                const double diff = (mean - _data[i]);
                sumDiff += diff*diff;
            }

            variance = sumDiff / _data.size();
            rmsDerivation = sqrt(variance);
        }

        //Median
        if( _calcMedian)
        {
            _sortedData = _data;
            std::sort(_sortedData.begin(), _sortedData.begin() + _sortedData.size());

            if(_data.size() %2 == 0)
                median = 0.5*(_sortedData[_sortedData.size()/2] + _sortedData[_sortedData.size()/2 +1]);
            else
                median = _sortedData[_sortedData.size()/2 +1];
        }
        
        //Output
        for( Pt::uint32_t i = 0; i < source.size(); ++i)
        {
            const StatisticValueSignal* outSignal = (StatisticValueSignal*)source[i];

            switch(outSignal->sigType())
            {
                case StatisticValueSignal::Minimum:
                    _outputData[i + rec * source.size()] = min;
                break;

                case StatisticValueSignal::Maximum:
                    _outputData[i + rec * source.size()] = max;
                break;

                case StatisticValueSignal::EffectiveValue:
                    _outputData[i + rec * source.size()] = effectiveValue;
                break;

                case StatisticValueSignal::AritmeticMean:
                     _outputData[i + rec * source.size()] = mean;
                break;

                case StatisticValueSignal::GeometricMean:
                    _outputData[i + rec * source.size()] = geoMean;
                break;

                case StatisticValueSignal::HarmonicMean:
                    _outputData[i + rec * source.size()] = harMean;
                break;

                case StatisticValueSignal::QuadraticMean:
                    _outputData[i + rec * source.size()] = quadMean;
                break;
                    
                case StatisticValueSignal::CubicMean:
                    _outputData[i + rec * source.size()] = cubicMean;
                break;

                case StatisticValueSignal::Variance:
                    _outputData[i + rec * source.size()] = variance;
                break;

                case StatisticValueSignal::RMSDerivation:
                    _outputData[i + rec * source.size()] = rmsDerivation;
                break;

                case StatisticValueSignal::Median:
                    _outputData[i + rec * source.size()] = median;
                break;

                case StatisticValueSignal::PeakPeakValue:
                {
                    double peakPeak = max-min;
                    _outputData[i + rec * source.size()] = peakPeak;
                }
                break;
            }
        }
    }

    outPort->onUpdateDataValue(noOfRecords,0, (Pt::uint8_t*) &_outputData[0]);
}

}}
