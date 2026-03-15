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
#include "ThreeStepControllerPS.h"
#include <mps/core/SignalList.h>

namespace mps{
namespace controller{

ThreeStepControllerPS::ThreeStepControllerPS()
: _lowerOnLimit(0)
, _lowerOffLimit(0)
, _upperOnLimit(0)
, _upperOffLimit(0)
, _setPoint(0)
, _lastValue(0)
{
    registerProperty( "upperOnLimit", *this, &ThreeStepControllerPS::upperOnLimit, &ThreeStepControllerPS::setUpperOnLimit );
    registerProperty( "upperOffLimit", *this, &ThreeStepControllerPS::upperOffLimit, &ThreeStepControllerPS::setUpperOffLimit );
    registerProperty( "lowerOnLimit", *this, &ThreeStepControllerPS::lowerOnLimit, &ThreeStepControllerPS::setLowerOnLimit );
    registerProperty( "lowerOffLimit", *this, &ThreeStepControllerPS::lowerOffLimit, &ThreeStepControllerPS::setLowerOffLimit );
    registerProperty( "setPoint", *this, &ThreeStepControllerPS::setPoint, &ThreeStepControllerPS::setSetPoint );
}

ThreeStepControllerPS::~ThreeStepControllerPS()
{
}

void ThreeStepControllerPS::onInitInstance()
{
    mps::core::ProcessStation::onInitInstance();

    const mps::core::Port* setPointPort = _inputPorts->at(1);

    if( setPointPort->signalList() != 0)
    {
        _setPointSignal = setPointPort->signalList()->at(0);
        _setPointSrcIdx = (int) setPointPort->sourceIndex(0);
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
}

void ThreeStepControllerPS::onStart()
{
    _lastValue = 0;
    ProcessStation::onStart();
}

void ThreeStepControllerPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
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
            double value = _controlledSignal->scaleValue(pvalue);
            double diff = _setPoint - value;
        
            if( diff < _lowerOnLimit)
            {
                _outData[rec] = _outSignal->outOn2Value();
                _lastValue = _outSignal->outOn2Value();
            }
            else if(diff > _lowerOffLimit && diff < _upperOffLimit)
            {
                _outData[rec] = _outSignal->outOffValue();
                _lastValue = _outSignal->outOffValue();
            }
            else if(diff > _upperOnLimit)
            {
                _outData[rec] = _outSignal->outOn1Value();
                _lastValue = _outSignal->outOn1Value();
            }
            else
            {   _outData[rec] = _lastValue;
            }
        }

        outPort->onUpdateDataValue(noOfRecords,_outSignalSrcIdx,(Pt::uint8_t*) &_outData[0]);
    }
}

}}
