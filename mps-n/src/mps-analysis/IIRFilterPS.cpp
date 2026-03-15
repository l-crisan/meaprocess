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
#include "IIRFilterPS.h"
#include <Pt/System/Clock.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <mps/core/Message.h>

namespace mps{
namespace analysis{

IIRFilterPS::IIRFilterPS()
{
    registerProperty( "ftype", *this, &IIRFilterPS::ftype, &IIRFilterPS::setFType );
    registerProperty( "forder", *this, &IIRFilterPS::order, &IIRFilterPS::setOrder );
    registerProperty( "lowerPass", *this, &IIRFilterPS::lowerPass, &IIRFilterPS::setLowerPass );
    registerProperty( "upperPass", *this, &IIRFilterPS::upperPass, &IIRFilterPS::setUpperPass );
    registerProperty( "transitionBW", *this, &IIRFilterPS::transitionBW, &IIRFilterPS::setTransitionBW );
    registerProperty( "stopBand", *this, &IIRFilterPS::stopBand, &IIRFilterPS::setStopBand );
}

IIRFilterPS::~IIRFilterPS()
{
}

void IIRFilterPS::onInitInstance()
{
    ProcessStation::onInitInstance();
}

void IIRFilterPS::onInitialize()
{
    ProcessStation::onInitialize();

    const mps::core::Port* port = _inputPorts->at(0);
    _signal = port->signalList()->at(0);
    _inSourceIndex = port->sourceIndex(0);
    _sigOffsetInSource = port->signalOffsetInSource(0);
    _recSize = port->sourceDataSize(_inSourceIndex);

    double rate = _signal->sampleRate();
    _errorState = false;

    if(!_filter.calcCoef((mps::filter::Filter::FilterType) ftype(), order(), lowerPass(), upperPass(), transitionBW(), stopBand(), (Pt::uint32_t)rate))
    {
        mps::core::Message message(format(translate("Mp.Analysis.Err.Filer"),this->getName()), mps::core::Message::Output, mps::core::Message::Error,
                         Pt::System::Clock::getLocalTime());

        sendMessage( message );
        _errorState = true;
    }
}

void IIRFilterPS::onStart()
{
    if(_errorState)
        return;

    _filter.start();

    ProcessStation::onStart();
}

void IIRFilterPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if(_inSourceIndex != sourceIdx || _errorState)
        return;

    if( _outData.size() < noOfRecords)
        _outData.resize(noOfRecords);

    mps::core::Port* outPort = _outputPorts->at(0);	

    for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
    {
        const Pt::uint8_t* pdata = &data[rec * _recSize + _sigOffsetInSource];
        const double value = _signal->scaleValue(pdata);
        _outData[rec] =_filter.filter(value);		
    }

    outPort->onUpdateDataValue(noOfRecords,0, (Pt::uint8_t*)&_outData[0]);
}

void IIRFilterPS::onDeinitialize()
{

    ProcessStation::onDeinitialize();
}

void IIRFilterPS::onExitInstance()
{
    ProcessStation::onExitInstance();
}

}}
