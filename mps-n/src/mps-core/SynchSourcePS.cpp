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
#include <mps/core/Signal.h>
#include <mps/core/SynchSourcePS.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <map>
#include <Pt/System/Clock.h>
#include <cmath>
#include <algorithm>

namespace mps{
namespace core{

const Pt::uint8_t SynchSourcePS::MAX_BUFFER_FACTOR = 4;

SynchSourcePS::SynchSourcePS(void)
: _run(0)
, _dataBuffer()
{
}

SynchSourcePS::~SynchSourcePS(void)
{

}

bool SynchSourcePS::isSynchronizedPS() const
{
    return true;
}

void SynchSourcePS::onInitInstance()
{
    ProcessStation::onInitInstance();

    if( !isSynchronizedPS() )
        return;

    //Calculate the maximum number of samples per timer event and 
    //the biggest source size.

    Pt::uint32_t maxNoOfSamples = 0;
    double sourcePeriode  = 0;
    Pt::uint32_t maxSourceSize  = 0;
    const Port* port = 0;

    _sourceEventInfo.clear();

    for(Pt::uint32_t portIdx = 0 ; portIdx < _outputPorts->size(); portIdx++)
    {
        port = _outputPorts->at(portIdx);
        
        for(Pt::uint32_t index = 0; index < port->signalList()->size(); ++index)
            port->signalList()->at(index)->setSignalIndex(index);

        const Sources& sources = port->sources();
        
        std::vector<SourceEventInfo> portSourceEventInfo;

        for(Pt::uint32_t sourceIdx = 0 ; sourceIdx < sources.size(); sourceIdx++)
        {
            const std::vector<Signal*>& source = sources[sourceIdx];
            
            if( source.size() == 0)
                continue;

            SourceEventInfo sourceEventInfo;

            const Signal* signal = source[0];

            sourcePeriode  = 1000000000.0 / signal->sampleRate();

            //Calculate the event generation
            if(sourcePeriode >= timerResolution())
            {
                sourceEventInfo.curretTime	= sourcePeriode; 
                sourceEventInfo.eventOn		= sourcePeriode;
                sourceEventInfo.noOfSamples = 1;
            }
            else
            {
                sourceEventInfo.curretTime	= (double) timerResolution();
                sourceEventInfo.eventOn		  = (double) timerResolution();
                sourceEventInfo.noOfSamples = (sourcePeriode / timerResolution());
            }

            portSourceEventInfo.push_back(sourceEventInfo);


            //Calculate the memory size
            const Pt::uint32_t& noOfSamplesProEvent = static_cast<Pt::uint32_t>(std::ceil( timerResolution() / sourcePeriode)) ;

            maxNoOfSamples = std::max( noOfSamplesProEvent, maxNoOfSamples );

            const Pt::uint32_t& sourceSize = port->sourceDataSize(sourceIdx);

            maxSourceSize = std::max( sourceSize, maxSourceSize );
        }

        _sourceEventInfo.push_back(portSourceEventInfo);
    }

    if(maxNoOfSamples == 0)
        maxNoOfSamples = MAX_BUFFER_FACTOR;

    const Pt::uint32_t maxSamples	=  maxNoOfSamples * MAX_BUFFER_FACTOR;

    //Allocate the memory
    _dataBuffer.resize( maxSamples * maxSourceSize );
}

void SynchSourcePS::onOverload()
{
    Message message( translate("Mp.Core.Err.Overload"), Message::Output, Message::Error, Pt::System::Clock::getLocalTime());
    
    sendMessage(message);

    stopRuntimeEngine(0);	
}

void SynchSourcePS::onTimer(Pt::uint32_t timerCounter)
{
    if(!_run)
        return;

    for(Pt::uint32_t portIdx = 0 ; portIdx < _outputPorts->size(); portIdx++)
    {
        Port* port = _outputPorts->at(portIdx);

        std::vector<SourceEventInfo>&  sourcesEventInfo = _sourceEventInfo[portIdx];

        for(Pt::uint32_t sourceIdx = 0 ; sourceIdx < sourcesEventInfo.size(); sourceIdx++)
        {
            SourceEventInfo* eventInfo   = &sourcesEventInfo[sourceIdx];
            
            if(eventInfo->curretTime >= eventInfo->eventOn)
            {
                Pt::uint32_t readedSamples = 0;

                //Get the number of samples to get per event
                const Pt::uint32_t noOfSamples = (Pt::uint32_t) eventInfo->samplesToGet;

                readedSamples = noOfSamples;

                //Propaget the vent to aquire data
                onSourceEvent( readedSamples, noOfSamples * MAX_BUFFER_FACTOR, sourceIdx, portIdx, &_dataBuffer[0]);

                //Push data forward
                if( readedSamples != 0)
                    port->onUpdateDataValue(readedSamples, sourceIdx, &_dataBuffer[0]);

                //Reset the time
                eventInfo->curretTime = 0;

                //Calculate the number of samples to get for the next event
                eventInfo->samplesToGet -= noOfSamples;
                eventInfo->samplesToGet += eventInfo->noOfSamples;
            }

            eventInfo->curretTime  += timerResolution();
        }
    }
}

bool SynchSourcePS::isActive() const
{
    return _run;
}

void SynchSourcePS::onStart()
{
    for(Pt::uint32_t portIdx = 0 ; portIdx < _outputPorts->size(); portIdx++)
    {
        const Port* port = _outputPorts->at(portIdx);

        const Sources& sources = port->sources();
        
        std::vector<SourceEventInfo>& portSourceEventInfo = _sourceEventInfo[portIdx];

        for(Pt::uint32_t sourceIdx = 0 ; sourceIdx < sources.size(); sourceIdx++)
        {
            const std::vector<Signal*>& source = sources[sourceIdx];

            if( source.size() == 0)
                continue;

            const Signal* signal = source[0];

            double sourcePeriode  = 1000000000.0 / signal->sampleRate();

            SourceEventInfo* sourceEventInfo = &portSourceEventInfo[sourceIdx];

            if(sourcePeriode >= timerResolution())
            {
                sourceEventInfo->curretTime   = sourcePeriode; 
                sourceEventInfo->eventOn      = sourcePeriode;
                sourceEventInfo->noOfSamples  = 1;
                sourceEventInfo->samplesToGet = 1;
            }
            else
            {
                sourceEventInfo->curretTime    = (double) timerResolution();
                sourceEventInfo->eventOn       = (double)  timerResolution();
                sourceEventInfo->noOfSamples   = timerResolution()/sourcePeriode;
                sourceEventInfo->samplesToGet  = sourceEventInfo->noOfSamples;
            }
        }
    }

    _run = true;
    ProcessStation::onStart();
}

void SynchSourcePS::onStop()
{
    ProcessStation::onStop();
    _run = false;
}

}}
