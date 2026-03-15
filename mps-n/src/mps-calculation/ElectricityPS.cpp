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
#include "ElectricityPS.h"
#include <mps/core/Port.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include "ElectricitySignal.h"
#include <Pt/Math.h>

namespace mps{
namespace calculation{

ElectricityPS::ElectricityPS()
: _voltageSignal(0)
, _currentSignal(0)
, _vSource(0)
, _iSource(0)
, _outputRate(0)
, _effectiveVoltage(0)
, _voltageInt(0)
, _voltageCounter(0)
, _lastVoltage(0)
, _firstVoltagePriode(0)
, _voltagePeriode(0)
, _effectiveCurrent(0)
, _currentInt(0)
, _currentCounter(0)
, _lastCurrent(0)
, _currentPeriode(0)
, _firstCurrentPriode(0)
, _activePower(0)
, _apparentPower(0)
, _idelPower(0)
, _powerInt(0)
, _phi(0)
{
    registerProperty( "voltageSignal", *this, &ElectricityPS::voltageSignal, &ElectricityPS::setVoltageSignal );
    registerProperty( "currentSignal", *this, &ElectricityPS::currentSignal, &ElectricityPS::setCurrentSignal );
}

ElectricityPS::~ElectricityPS()
{
}

void ElectricityPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();


    if( _voltageSignal != 0 && _currentSignal != 0)
    {
        _vSignal = getInputSignal(_voltageSignal);
        Pt::uint32_t sigIdx = _inputPorts->at(0)->signalList()->getSignalIndex(_vSignal);
        _vSource = _inputPorts->at(0)->sourceIndex(sigIdx);
        _vSourceSize = _inputPorts->at(0)->sourceDataSize(_vSource);
        _vSignalOffset = _inputPorts->at(0)->signalOffsetInSource(sigIdx);
        _outputRate = _vSignal->sampleRate();

        _iSignal = getInputSignal(_currentSignal);
        sigIdx = _inputPorts->at(0)->signalList()->getSignalIndex(_iSignal);
        _iSource = _inputPorts->at(0)->sourceIndex(sigIdx);
        _iSignalOffset = _inputPorts->at(0)->signalOffsetInSource(sigIdx);
                
        _iSourceSize = _inputPorts->at(0)->sourceDataSize(_iSource);

        std::vector<double> rates;
        std::vector<Pt::uint32_t> itemSizes(2,sizeof(double));

        rates.push_back(_vSignal->sampleRate());
        rates.push_back(_iSignal->sampleRate());    

        Pt::uint32_t records = static_cast<Pt::uint32_t>(_vSignal->sampleRate()/2);
            
        if(_vSignal->sampleRate() < 100)
             records = 100;

        _record.init(records, itemSizes, rates, _vSignal->sampleRate());
    }
    else
    {
        if(  _voltageSignal != 0 )
        {
            _vSignal = getInputSignal(_voltageSignal);
            Pt::uint32_t sigIdx = _inputPorts->at(0)->signalList()->getSignalIndex(_vSignal);
            _vSource = _inputPorts->at(0)->sourceIndex(sigIdx);
            _outputRate = _vSignal->sampleRate();
            _vSignalOffset = _inputPorts->at(0)->signalOffsetInSource(sigIdx);
            _vSourceSize = _inputPorts->at(0)->sourceDataSize(_vSource);


            std::vector<double> rates(1,_vSignal->sampleRate());
            std::vector<Pt::uint32_t> itemSizes(1,sizeof(double));

            Pt::uint32_t records = static_cast<Pt::uint32_t>(_vSignal->sampleRate()/2);
            
            if(_vSignal->sampleRate() < 100)
                records = 100;

            _record.init(records, itemSizes, rates, _vSignal->sampleRate());
        }

        if( _currentSignal != 0 )
        {
            _iSignal = getInputSignal(_currentSignal);
            Pt::uint32_t sigIdx = _inputPorts->at(0)->signalList()->getSignalIndex(_iSignal);
            _iSource = _inputPorts->at(0)->sourceIndex(sigIdx);
            _iSignalOffset = _inputPorts->at(0)->signalOffsetInSource(sigIdx);  
            _iSourceSize = _inputPorts->at(0)->sourceDataSize(_iSource);
            _outputRate = _iSignal->sampleRate();

            std::vector<double> rates(1,_iSignal->sampleRate());
            std::vector<Pt::uint32_t> itemSizes(1,sizeof(double));

            Pt::uint32_t records = static_cast<Pt::uint32_t>(_iSignal->sampleRate()/2);
            
            if(_iSignal->sampleRate() < 100)
                records = 100;

            _record.init(records, itemSizes, rates, _iSignal->sampleRate());
        }
    }


    _outDataRecord.resize(_outputPorts->at(0)->signalList()->size());
}

const mps::core::Signal* ElectricityPS::getInputSignal(Pt::uint32_t sigId)
{
    const mps::core::Port* port = _inputPorts->at(0);

    for( Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        const mps::core::Signal* signal = port->signalList()->at(i);

        if( signal->signalID() == sigId)
            return signal;
    }

    return 0;
}

void ElectricityPS::onStart()
{
    _record.reset();

    _voltagePeriode = 0;
    _currentPeriode = 0;
    _lastVoltage = 0;
    _lastCurrent = 0;
    _voltageCounter = 0;
    _currentCounter = 0;
    _effectiveVoltage = 0;
    _voltageInt = 0;
    _currentInt = 0;
    _effectiveCurrent = 0;
    _activePower = 0;
    _apparentPower = 0;
    _firstVoltagePriode = true;
    _firstCurrentPriode = true;
    _idelPower = 0;
    _powerInt = 0;
    _phi = 0;

    memset(&_outDataRecord[0],0,sizeof( double) * _outDataRecord.size());

    FiFoSynchSourcePS::onStart();
}

void ElectricityPS::onReadData()
{
  
    const Pt::uint8_t* data1;
    Pt::uint32_t count1;
    const Pt::uint8_t* data2;
    Pt::uint32_t count2;

    _record.get(&data1, count1, &data2, count2);
    
    if( count1 != 0)
        processRecords(data1,count1);

    if( count2 != 0)
        processRecords(data2,count2);
}


void ElectricityPS::processRecords(const Pt::uint8_t* data, Pt::uint32_t count)
{
    const mps::core::Port* port = _outputPorts->at(0);

    for( Pt::uint32_t rec = 0; rec < count; ++rec)
    {        
        double voltage = 0;
        double current = 0;

        if( _voltageSignal != 0 &&  _currentSignal != 0)
        {
            const Pt::uint32_t position = rec *sizeof(double) * 2;
            voltage = *((double*) &data[position]);
            current = *((double*) &data[position + sizeof(double)]);
        }
        else
        {
            const Pt::uint32_t position = rec *sizeof(double);

            if( _voltageSignal != 0)
                voltage = *((double*) &data[position]);
                
            if( _currentSignal != 0)
                current = *((double*) &data[position]);
        }

        _voltageInt += (voltage * voltage);
        _currentInt += (current * current);
        _currentPower = voltage * current;
        _powerInt += _currentPower;
        
        if(_lastVoltage < 0 && voltage >= 0 && !_firstVoltagePriode)
        {
            _voltagePeriode = 1/_outputRate * _voltageCounter;

            if( _voltagePeriode != 0)
            {
                _effectiveVoltage = sqrt((_voltageInt * 1/_outputRate)/_voltagePeriode);
                _activePower = (_powerInt * 1/_outputRate)/_voltagePeriode;
                _apparentPower = _effectiveVoltage * _effectiveCurrent;
                
                double diff = (_apparentPower*_apparentPower) - (_activePower* _activePower);
                if( diff >= 0)
                    _idelPower = sqrt(diff);

                if(_apparentPower != 0)
                {
                    double cosPhi = _activePower/_apparentPower;
                    if( cosPhi >= -1 && cosPhi <= 1)
                    {
                        double radPhi = acos(cosPhi);
                        _phi = 360* radPhi/Pt::piDouble<double>();
                    }
                }
            }

            _voltageInt = 0;
            _voltageCounter = 0;
            _powerInt = 0;
        }

        if(_lastVoltage < 0 && voltage >= 0 && _firstVoltagePriode)
        {
            _voltageInt = 0;
            _voltageCounter = 0;
            _firstVoltagePriode = false;
            _powerInt = 0;
        }

        if(_lastCurrent < 0 && current >= 0 && !_firstCurrentPriode)
        {
            _currentPeriode = 1/_outputRate * _currentCounter;
  
            if( _currentPeriode != 0)
            {
                _effectiveCurrent = sqrt((_currentInt * 1/_outputRate)/_currentPeriode);
                _apparentPower = _effectiveVoltage * _effectiveCurrent;

                double diff = (_apparentPower*_apparentPower) - (_activePower* _activePower);
                if( diff >= 0)
                    _idelPower = sqrt(diff);
            }

            _currentInt = 0;
            _currentCounter = 0;
        }

        if(_lastCurrent < 0 && current >= 0 && _firstCurrentPriode)
        {
            _currentInt = 0;
            _currentCounter = 0;   
            _firstCurrentPriode = false;
        }
        
        const mps::core::Sources& sources = port->sources();
        const std::vector<mps::core::Signal*>& source = sources[0];

        for( Pt::uint32_t i = 0; i < source.size(); ++i)
        {
            const ElectricitySignal* signal = (const ElectricitySignal*) source[i];
            
            switch(signal->sigType())
            {
                case ElectricitySignal::EffectiveVoltage:
                    _outDataRecord[i] = _effectiveVoltage;
                break;
                        
                case ElectricitySignal::EffectiveCurrent:
                    _outDataRecord[i] = _effectiveCurrent;
                break;

                case ElectricitySignal::ActivePower:
                    _outDataRecord[i] = _activePower;
                break;

                case ElectricitySignal::ApparentPower:
                    _outDataRecord[i] = _apparentPower;
                break;

                case ElectricitySignal::IdlePower:
                    _outDataRecord[i] = _idelPower;
                break;

                case ElectricitySignal::Phi:
                    _outDataRecord[i] = _phi;
                break;

                case ElectricitySignal::SinPhi:
                    _outDataRecord[i] = sin((_phi* Pt::piDouble<double>())/360);
                break;

                case ElectricitySignal::CosPhi:
                    _outDataRecord[i] = cos((_phi* Pt::piDouble<double>())/360);
                break;

                case ElectricitySignal::VoltageFrequency:
                {
                    _outDataRecord[i] =  0;

                    if( _voltagePeriode != 0)
                    {
                        _outDataRecord[i] = 1.0/ _voltagePeriode;
                    }
                    else
                    {
                        if( _currentPeriode != 0)
                            _outDataRecord[i] = 1.0/ _currentPeriode;
                    }
                }
                break;
            }
        }
        
        putRecords(0, 0, 1, (Pt::uint8_t*)&_outDataRecord[0]);

        _voltageCounter++;
        _currentCounter++;

        _lastVoltage = voltage;
        _lastCurrent = current;
    }
}

void ElectricityPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if( sourceIdx == _vSource || sourceIdx == _iSource)
    {
        for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            if(_currentSignal != 0 && _voltageSignal != 0)
            {
                if( sourceIdx == _vSource) 
                {
                    const Pt::uint8_t* pdata = &data[ rec * _vSourceSize + _vSignalOffset]; 
                    const double value = _vSignal->scaleValue(pdata);
                    _record.insert((Pt::uint8_t*) &value,0);
                }
                    
                if(sourceIdx == _iSource)
                {
                    const Pt::uint8_t* pdata = &data[ rec * _iSourceSize + _iSignalOffset]; 
                    const double value = _iSignal->scaleValue(pdata);
                    _record.insert((const Pt::uint8_t*) &value,1);
                }
            }
            else
            {
                if( sourceIdx == _vSource && _voltageSignal != 0) 
                {
                    const Pt::uint8_t* pdata = &data[ rec * _vSourceSize + _vSignalOffset]; 
                    const double value = _vSignal->scaleValue(pdata);
                    _record.insert((Pt::uint8_t*) &value,0);
                }
                    
                if(sourceIdx == _iSource && _currentSignal != 0)
                {
                    const Pt::uint8_t* pdata = &data[ rec * _vSourceSize + _iSignalOffset]; 
                    const double value = _iSignal->scaleValue(pdata);
                    _record.insert((const Pt::uint8_t*) &value,0);
                }
            }
        }
    }
}

}}
