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
#include "SignalDelayPS.h"
#include <mps/core/SignalList.h>
#include "DelaySignal.h"

namespace mps{
namespace calculation{

SignalDelayPS::SignalDelayPS()
{
}

SignalDelayPS::~SignalDelayPS()
{

}

void SignalDelayPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    //Build the signal mapping
    const mps::core::Port* outPort = _outputPorts->at(0);

    for( Pt::uint32_t i = 0; i < outPort->signalList()->size(); ++i)
    {
        const DelaySignal* signal = (DelaySignal*) outPort->signalList()->at(i);
        Pt::uint32_t inSigId = signal->sigToDelay();

        
        //Create a delay ifor structure
        SignalDelayInfo delaySigInfo;

        delaySigInfo.outputSignal = signal;

        SignalDelayMapIt it = _signalDelayMapping.find(inSigId);

        //Insert the delay info structure into the map
        if( it == _signalDelayMapping.end())
        {
            std::vector<SignalDelayInfo> delayInfos;
            delayInfos.push_back(delaySigInfo);
            std::pair<Pt::uint32_t, std::vector<SignalDelayInfo> > pair(inSigId, delayInfos);
            _signalDelayMapping.insert(pair);
        }
        else
        {
            it->second.push_back(delaySigInfo);
        }
    }
}

void SignalDelayPS::onStart()
{
    SignalDelayMapIt it = _signalDelayMapping.begin();
    
    for(; it != _signalDelayMapping.end(); ++it)
    {
        for( Pt::uint32_t i = 0; i < it->second.size(); ++i)
        {
            //Clear the data queue
            while(!it->second[i].dataDelayQueue.empty())
                it->second[i].dataDelayQueue.pop();

            //Re fill the queue with 0
            const DelaySignal* signal = it->second[i].outputSignal;
            Pt::uint32_t noOfSamplesToDelay = static_cast<Pt::uint32_t>(signal->delayMs() / (1000/signal->sampleRate()));

            noOfSamplesToDelay = noOfSamplesToDelay == 0 ? 1 : noOfSamplesToDelay;

            for(Pt::uint32_t j = 0; j < noOfSamplesToDelay; ++j)
            {
                std::vector<Pt::uint8_t> sample(signal->valueSize(), 0);
                it->second[i].dataDelayQueue.push(sample);
            }

        }
    }

    FiFoSynchSourcePS::onStart();
}

void SignalDelayPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
    const Pt::uint32_t inRecordSize = port->sourceDataSize(sourceIdx);
 
    FiFoSynchSourcePS::lock();

    for( Pt::uint32_t i = 0; i < source.size(); ++i)
    {
        const mps::core::Signal* signal = source[i];
        SignalDelayMapIt it = _signalDelayMapping.find(signal->signalID());

        if( it == _signalDelayMapping.end())
            continue;

        const Pt::uint32_t sigOffset = port->signalOffsetInSource(sourceIdx, i);

        for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            const Pt::uint8_t* value = &data[rec * inRecordSize + sigOffset];

            for( Pt::uint32_t k = 0; k < it->second.size(); ++k)
            {
                SignalDelayInfo& delayInfo = it->second[k];
                std::vector<Pt::uint8_t> dataValue(signal->valueSize());

                memcpy(&dataValue[0], value, signal->valueSize());

                delayInfo.dataDelayQueue.push(dataValue);
                const std::vector<Pt::uint8_t>& lastValue = delayInfo.dataDelayQueue.front();

                putValue(delayInfo.outputSignal,0, (Pt::uint8_t*) &lastValue[0]);
                delayInfo.dataDelayQueue.pop();
            }
        }
    }

    FiFoSynchSourcePS::unlock();
}

}}
