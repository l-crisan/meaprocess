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
#include "CounterPS.h"
#include <mps/core/Port.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <Pt/Math.h>

namespace mps{
namespace statistics{

CounterPS::CounterPS()
{
}

CounterPS::~CounterPS()
{
}

void CounterPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    const mps::core::Port* outPort = _outputPorts->at(0);

    for( Pt::uint32_t i = 0; i < outPort->signalList()->size(); ++i)
    {
        const CounterSignal* signal = (const CounterSignal*) outPort->signalList()->at(i);
        InSig2OutSigIt it = _inSig2OutSig.find(signal->inSignal());

        if( it == _inSig2OutSig.end())
        {
            std::vector<OutSigInfo> outSignals;
            OutSigInfo outInfo;
            outInfo.outSignal = signal;

            outSignals.push_back(outInfo);

            std::pair<Pt::uint32_t, std::vector<OutSigInfo> > pair(signal->inSignal(), outSignals);
            _inSig2OutSig.insert(pair);
        }
        else
        {
            OutSigInfo outInfo;
            outInfo.outSignal = signal;
            it->second.push_back(outInfo);
        }
    }

    _resetSourceIdx = (Pt::uint32_t) -1;

    const mps::core::Port* resetPort = _inputPorts->at(1);
    _resetSignal = 0;

    if( resetPort->signalList() != 0)
    {
         _resetSignal = resetPort->signalList()->at(0);
         _resetSourceIdx = resetPort->sourceIndex(0);
    }
}

void CounterPS::onStart()
{
    InSig2OutSigIt it = _inSig2OutSig.begin();

    for( ; it != _inSig2OutSig.end(); ++it)
    {
        for( Pt::uint32_t j = 0; j < it->second.size(); ++j)
        {
            OutSigInfo& info = it->second[j];
            info.lastValues1 = 0;
            info.lastValues2 = 0;
            info.outValue = static_cast<Pt::uint32_t>(info.outSignal->physMin());
        }
    }

    _lastResetValue = 0;
    FiFoSynchSourcePS::onStart();
}

void CounterPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if( port->portNumber() == 0)
    {
        const mps::core::Sources& sources = port->sources();
        const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
        const Pt::uint32_t inRecordSize = port->sourceDataSize(sourceIdx);

        for( Pt::uint32_t i = 0; i < source.size(); ++i)
        {
            const mps::core::Signal* signal = source[i];
            InSig2OutSigIt it = _inSig2OutSig.find(signal->signalID());

            if( it == _inSig2OutSig.end())
                continue;

            const Pt::uint32_t sigOffset = port->signalOffsetInSource(sourceIdx, i);

            for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
            {
                const Pt::uint8_t* pvalue = &data[rec * inRecordSize + sigOffset];
                const double value = signal->scaleValue(pvalue);

                for( Pt::uint32_t k = 0; k < it->second.size(); ++k)
                {
                    OutSigInfo& outInfo = it->second[k];

                    switch(outInfo.outSignal->sigType())
                    {
                        case CounterSignal::Event:
                        {
                            if(outInfo.lastValues1 == 0 && value != 0)
                                outInfo.outValue++;

                            outInfo.lastValues1 = value;
                        }
                        break;

                        case CounterSignal::Maxima:
                        {
                            if(outInfo.lastValues2 < outInfo.lastValues1 && outInfo.lastValues1 > value)
                                outInfo.outValue++;

                            outInfo.lastValues2 = outInfo.lastValues1;
                            outInfo.lastValues1 = value;
                        }
                        break;

                        case CounterSignal::Minima:
                        {
                            if(outInfo.lastValues2 > outInfo.lastValues1 && outInfo.lastValues1 < value)
                                outInfo.outValue++;

                            outInfo.lastValues2 = outInfo.lastValues1;
                            outInfo.lastValues1 = value;
                        }
                        break;

                        case CounterSignal::ZeroCrossing:
                        {
                            if((outInfo.lastValues1 < 0 && value >= 0) || (outInfo.lastValues1 >= 0 && value < 0))
                                outInfo.outValue++;

                            outInfo.lastValues1 = value;
                        }
                        break;

                        case CounterSignal::ZeroCrossingRising:
                        {
                            if(outInfo.lastValues1 < 0 && value >= 0)
                                outInfo.outValue++;

                            outInfo.lastValues1 = value;
                        }
                        break;

                        case CounterSignal::ZeroCrossingFalling:
                        {
                            if(outInfo.lastValues1 >= 0 && value < 0)
                                outInfo.outValue++;

                            outInfo.lastValues1 = value;
                        }
                        break;
                    }

                    if( outInfo.outValue > outInfo.outSignal->physMax())
                        outInfo.outValue = static_cast<Pt::uint32_t>(outInfo.outSignal->physMin());

                    putValue(outInfo.outSignal,0, (Pt::uint8_t*) &outInfo.outValue);
                }
            }
        }
    }
    else if( port->portNumber() == 1)
    {
        if(_resetSourceIdx != sourceIdx || _resetSignal == 0)
            return;

        const Pt::uint32_t resetDataRecSize = port->sourceDataSize(sourceIdx);
        const Pt::uint32_t sigIdxInSrc = port->signalIndexInSource(0);
        const Pt::uint32_t sigOffset = port->signalOffsetInSource(sourceIdx,sigIdxInSrc);

        for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            const Pt::uint8_t* pvalue = &data[rec * resetDataRecSize + sigOffset];
            const double value = _resetSignal->scaleValue(pvalue);

            if( _lastResetValue == 0 && value != 0)
            {
                InSig2OutSigIt it = _inSig2OutSig.begin();

                for( ; it != _inSig2OutSig.end(); ++it)
                {
                    for( Pt::uint32_t j = 0; j < it->second.size(); ++j)
                    {
                        OutSigInfo& info = it->second[j];
                        info.outValue = static_cast<Pt::uint32_t>(info.outSignal->physMin());
                    }
                }
            }

            _lastResetValue = value;
        }   
    }
}

}}
