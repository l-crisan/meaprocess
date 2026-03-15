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
#include "PIDControllerPS.h"
#include <mps/core/SignalList.h>

namespace mps{
namespace controller{

PIDControllerPS::PIDControllerPS()
: _pParam(0)
, _iParam(0)
, _dParam(0)
, _setPoint(0)
{
	registerProperty( "pParam", *this, &PIDControllerPS::pParam, &PIDControllerPS::setPParam );
	registerProperty( "iParam", *this, &PIDControllerPS::iParam, &PIDControllerPS::setIParam );
	registerProperty( "dParam", *this, &PIDControllerPS::dParam, &PIDControllerPS::setDParam );
    registerProperty( "setPoint", *this, &PIDControllerPS::setPoint, &PIDControllerPS::setSetPoint );
}

PIDControllerPS::~PIDControllerPS()
{
}

void PIDControllerPS::onInitInstance()
{
    ProcessStation::onInitInstance();

    const mps::core::Port* setPointPort = _inputPorts->at(1);

    if( setPointPort->signalList() != 0)
    {
        _setPointSignal = setPointPort->signalList()->at(0);
        _setPointSrcIdx = (int)setPointPort->sourceIndex(0);
        _setPointSignalOffset = setPointPort->signalOffsetInSource(0);
    }
    else
    {
        _setPointSignal = 0;
        _setPointSrcIdx = -1;
    }

    const mps::core::Port* controlledPort = _inputPorts->at(0);
    _controlledSignal = controlledPort->signalList()->at(0);
    _controlledSrcIdx = (int)controlledPort->sourceIndex(0);
    _controlledSignalOffset = controlledPort->signalOffsetInSource(0);
    _controlledRecordSize = controlledPort->sourceDataSize(_controlledSrcIdx);

    const mps::core::Port* outPort = _outputPorts->at(0);

    _outSignal = (const ControllerSignal*) outPort->signalList()->at(0);
    _outSignalSrcIdx = outPort->sourceIndex(0);

    if(_iParam != 0)
        _tn = _pParam/_iParam;
    else
        _tn = 0;

    if( _pParam != 0)
        _tv = _dParam / _pParam;
    else
        _tv = 0;
}

void PIDControllerPS::onStart()
{
    _integE = 0;
    _lastE = 0;
    _diffE = 0;
    _diffCounter = 1;
    ProcessStation::onStart();
}

void PIDControllerPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if(static_cast<Pt::uint32_t>(_setPointSrcIdx) == sourceIdx && port->portNumber() == 1)
        _setPoint = _setPointSignal->scaleValue((Pt::uint8_t*)&data[_setPointSignalOffset]);

    if(static_cast<Pt::uint32_t>(_controlledSrcIdx) == sourceIdx && port->portNumber() == 0)
    {
        if(_outData.size() < noOfRecords)
            _outData.resize(noOfRecords);
            
        mps::core::Port* outPort =  _outputPorts->at(0);

        for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            const Pt::uint8_t* pvalue = &data[rec * _controlledRecordSize + _controlledSignalOffset];
            const double e = _setPoint - _controlledSignal->scaleValue(pvalue);

            _integE += (e * 1.0 / _controlledSignal->sampleRate());

            double deltaE = e - _lastE;
            if( deltaE == 0 )
            {
                _diffCounter++;  
                _diffE = 0;
            }
            else
            {
                _diffE = deltaE * (_controlledSignal->sampleRate()/_diffCounter);
                _diffCounter = 1;
            }

            _lastE = e;

            double outputValue = 0;

            if( _tn != 0 )
                outputValue = _pParam * (e + 1.0/_tn *_integE + _tv * _diffE);
            else
                outputValue = _pParam * (e + _tv * _diffE);

            if( outputValue <  _outSignal->physMin())
                _outData[rec] = _outSignal->physMin();
            else if( outputValue > _outSignal->physMax())
                _outData[rec] = _outSignal->physMax();
            else
                _outData[rec] = outputValue;
        }

        outPort->onUpdateDataValue(noOfRecords, _outSignalSrcIdx, (Pt::uint8_t*) &_outData[0]);
    }        
}

}}
