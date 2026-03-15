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
#include "SampleRatePS.h"
#include <mps/core/SignalList.h>
#include "SampleRateSignal.h"

namespace mps{
namespace calculation{

SampleRatePS::SampleRatePS()
{
}

SampleRatePS::~SampleRatePS()
{
}

void SampleRatePS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    const mps::core::Port* outPort = _outputPorts->at(0);

    for( Pt::uint32_t i = 0; i < outPort->signalList()->size(); ++i)
    {
        SampleRateSignal* signal = (SampleRateSignal*) outPort->signalList()->at(i);
        signal->setIndex(i);
        InSig2OutSigIt it = _inSig2OutSig.find(signal->inSignal());

        if( it == _inSig2OutSig.end())
        {
            std::vector<SampleRateSignal*> outSignals;

            outSignals.push_back(signal);

            std::pair<Pt::uint32_t, std::vector<SampleRateSignal*> > pair(signal->inSignal(), outSignals);
            _inSig2OutSig.insert(pair);
        }
        else
        {
            it->second.push_back(signal);
        }
    }

    _sampleIncrement.resize(outPort->signalList()->size());
}

void SampleRatePS::onStart()
{
    for( Pt::uint32_t i = 0; i < _sampleIncrement.size(); ++i)
        _sampleIncrement[i] = 0;

    FiFoSynchSourcePS::onStart();
}

void SampleRatePS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
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

            for( Pt::uint32_t k = 0; k < it->second.size(); ++k)
            {
                const SampleRateSignal* outSignal = it->second[k];
                _sampleIncrement[outSignal->index()] += (outSignal->sampleRate() /signal->sampleRate() );

                int z = 0;

                for( ; z < (int)_sampleIncrement[outSignal->index()]; ++z)
                    putValue(outSignal->index(),0,pvalue);
                    
                _sampleIncrement[outSignal->index()] -= z;
            }
        }
    }
}

}}
