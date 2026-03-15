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
#include "MovingMeanPS.h"
#include <mps/core/SignalList.h>
#include <mps/core/Port.h>
#include "MovingMeanSignal.h"

namespace mps{
namespace calculation{

MovingMeanPS::MovingMeanPS()
{
    registerProperty( "samples", *this, &MovingMeanPS::samples, &MovingMeanPS::setSamples );
}

MovingMeanPS::~MovingMeanPS()
{
}

void MovingMeanPS::onInitInstance()
{
    mps::core::FiFoSynchSourcePS::onInitInstance();

    const mps::core::Port* outPort = _outputPorts->at(0);

    for( Pt::uint32_t i = 0; i < outPort->signalList()->size(); ++i)
    {
        const MovingMeanSignal* signal = (const MovingMeanSignal*) outPort->signalList()->at(i);
        InSig2OutSigIt it = _inSig2OutSig.find(signal->inSignal());

        if( it == _inSig2OutSig.end())
        {
            std::vector<OutSigInfo> outSignals;
            OutSigInfo outInfo;            
            outInfo.pos = 0;
            outInfo.values.resize(samples());
            outInfo.outSignal = signal;

            outSignals.push_back(outInfo);

            std::pair<Pt::uint32_t, std::vector<OutSigInfo> > pair(signal->inSignal(), outSignals);
            _inSig2OutSig.insert(pair);
        }
        else
        {
            OutSigInfo outInfo;
            outInfo.pos = 0;
            outInfo.values.resize(samples());
            outInfo.outSignal = signal;
            it->second.push_back(outInfo);
        }
    }

    _weight = 0;

    for(Pt::uint32_t i = 1; i <= samples(); ++i)
        _weight += i;
}

void MovingMeanPS::onStart()
{
    InSig2OutSigIt it = _inSig2OutSig.begin();

    for( ; it != _inSig2OutSig.end(); ++it)
    {
        for( Pt::uint32_t j = 0; j < it->second.size(); ++j)
        {
            OutSigInfo& info = it->second[j];
            info.pos = 0;
            memset(&info.values[0], 0, sizeof(double) * info.values.size());
        }
    }

    mps::core::FiFoSynchSourcePS::onStart();
}

void MovingMeanPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];    
    const Pt::uint32_t inRecordSize = port->sourceDataSize(sourceIdx);
    double mean = 0;

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

                if(outInfo.pos == outInfo.values.size())
                    outInfo.pos = 0;
                
                outInfo.values[outInfo.pos] = value;
                outInfo.pos++;

                mean = 0;

                if(outInfo.outSignal->meanType() == 0)
                {
                    for(Pt::uint32_t j = 0; j < outInfo.values.size(); ++j)
                        mean += outInfo.values[j];

                    mean /= outInfo.values.size();
                }
                else
                {
                    Pt::uint32_t pos = outInfo.pos;

                    for(Pt::uint32_t j = 1; j <= outInfo.values.size(); ++j)
                    {
                        if( pos == outInfo.values.size())
                            pos = 0;

                        mean += (outInfo.values[pos] * j);
                        pos++;
                    }

                    mean /= _weight;
                }

                putValue(outInfo.outSignal,0, (Pt::uint8_t*) &mean);
            }
        }
    }
}

}}
