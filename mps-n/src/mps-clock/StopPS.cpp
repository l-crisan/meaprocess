//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
#include "StopPS.h"

#include <mps/Signal.h>
#include <mps/Port.h>
#include <mps/SignalList.h>

namespace mps
{

StopPS::StopPS()
:_stoping(false)
, _delay(0)
{
	registerProperty( "delay", *this, &StopPS::delay, &StopPS::setDelay );
}

StopPS::~StopPS()
{
}

void StopPS::onInitInstance()
{
	ProcessStation::onInitInstance();

	Port* port =_inputPorts->at(0);

	const SignalList* list = port->signalList();

	_stopSignal = list->at(0);

	size_t sigIdx = list->getSignalIndex(_stopSignal);
	_srcIdx = port->sourceIndex(sigIdx);

	_offsetInSrc = port->signalOffsetInSource(sigIdx);
}

void StopPS::onStart()
{
	_stoping = false;
	ProcessStation::onStart();
}

void StopPS::onUpdateDataValue(size_t noOfRecords, size_t sourceIdx, const Port* port, const Pt::uint8_t* data)
{	
	if( _stoping)
		return;

	if(_srcIdx != sourceIdx)
		return;

	const Pt::uint8_t* pdata = &data[_offsetInSrc];
	
	if(_stopSignal->scaleValue(pdata) != 0.0)
	{
		stopRuntimeEngine(_delay);	
		_stoping = true;
	}
}

}
