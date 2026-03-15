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
#include "SignalControlPS.h"
#include <mps/core/Port.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <cmath>

namespace mps{
namespace eventps{

SignalControlPS::SignalControlPS()
{
}

SignalControlPS::~SignalControlPS()
{
}

void SignalControlPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    const mps::core::Port* port = _outputPorts->at(0);

    for(Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        ControlSignal* signal = (ControlSignal*) port->signalList()->at(i);
        Pt::uint32_t inSigId = signal->inSignal();

        InSig2OutSigIt it = _inSig2OutSig.find(inSigId);
        if( it == _inSig2OutSig.end())
        {
            std::vector<ControlSignal*> signals;
            signals.push_back(signal);
            std::pair<Pt::uint32_t, std::vector<ControlSignal*> > pair(inSigId, signals);
            _inSig2OutSig.insert(pair);
        }
        else
        {
            it->second.push_back(signal);
        }
    }        
}

void SignalControlPS::onStart()
{
    FiFoSynchSourcePS::onStart();

    const mps::core::Port* port = _outputPorts->at(0);

    for(Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        ControlSignal* signal = (ControlSignal*) port->signalList()->at(i);
        signal->setLastValue(0);
    }
}

void SignalControlPS::processWindow(double value, ControlSignal* outSignal)
{
    Pt::int32_t outValue = outSignal->outValOnFalse();

    if(outSignal->windowType() == 0)
    {
        if( value >= outSignal->lower() && value < outSignal->upper())
            outValue = outSignal->outValOnTrue();
    }
    else
    {
        if( value <= outSignal->lower() || value > outSignal->upper())
            outValue = outSignal->outValOnTrue();
    }

    putValue(outSignal, 0, (Pt::uint8_t*)  &outValue);
}

void SignalControlPS::processSlope(double value, ControlSignal* outSignal)
{
    Pt::int32_t outValue =  outSignal->outValOnFalse();

    if(outSignal->slopeType() == 0)
    {
        if(outSignal->lastValue()  <= outSignal->slopeValue() && value > outSignal->slopeValue())
            outValue = outSignal->outValOnTrue();
    }
    else
    {
        if(outSignal->lastValue()  > outSignal->slopeValue() && value <= outSignal->slopeValue())
            outValue = outSignal->outValOnTrue();
    }

    putValue(outSignal, 0, (Pt::uint8_t*) &outValue);
    
    outSignal->setLastValue(value);
}

void SignalControlPS::processAlteration(double value, ControlSignal* outSignal)
{
    Pt::int32_t outValue = outSignal->outValOnFalse();

    double delta = 0;
    double alt = 0;

    if( outSignal->absolut())
    {
        delta = std::abs(value - outSignal->lastValue());
        alt = std::abs(outSignal->alteration());
    }
    else
    {
        delta = value - outSignal->lastValue();
        alt = outSignal->alteration();
    }

    switch( outSignal->signalIf())
    {
        case 0:
            if( delta > alt)
                outValue = outSignal->outValOnTrue();
        break;

        case 1:
            if( delta >= alt)
                outValue = outSignal->outValOnTrue();
        break;

        case 2:
            if( delta < alt )
                outValue = outSignal->outValOnTrue();
        break;

        case 3:
            if( delta <= alt )
                outValue = outSignal->outValOnTrue();
        break;
    }

    putValue(outSignal, 0,  (Pt::uint8_t*) &outValue);
    
    outSignal->setLastValue(value);
}

void SignalControlPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];

    const Pt::uint32_t sourceSize =  port->sourceDataSize(sourceIdx);

    for( Pt::uint32_t i = 0; i < source.size(); ++i)
    {
        const mps::core::Signal* signal = source[i];
        InSig2OutSigIt it =  _inSig2OutSig.find(signal->signalID());

        if( it == _inSig2OutSig.end())
            continue;

        Pt::uint32_t offsetInSource = port->signalOffsetInSource(sourceIdx, i);

        for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            const Pt::uint8_t* pdata = &data[rec* sourceSize + offsetInSource];
            double value = signal->scaleValue(pdata);
            for( Pt::uint32_t outSig = 0; outSig < it->second.size(); ++outSig)
            {
                ControlSignal* outSignal = it->second[outSig];

                switch(outSignal->ctrlType())
                {
                    case ControlSignal::Window:
                        processWindow(value, outSignal);
                    break;

                    case ControlSignal::Slope:
                        processSlope(value, outSignal);
                    break;

                    case ControlSignal::Alteration:
                        processAlteration(value, outSignal);
                    break;
                }
            }
        }
    }
}

void SignalControlPS::onReadData()
{
}

}}

