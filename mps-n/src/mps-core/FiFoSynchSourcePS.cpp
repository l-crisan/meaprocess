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
#include <mps/core/FiFoSynchSourcePS.h>
#include <mps/core/Message.h>
#include <mps/core/Port.h>
#include <mps/core/CircularBuffer.h>
#include <mps/core/SignalList.h>
#include <Pt/Types.h>
#include <Pt/System/Mutex.h>
#include <Pt/System/Logger.h>
#include <Pt/System/Clock.h>
#include <stdexcept>
#include <algorithm>

namespace mps{
namespace core{

FiFoSynchSourcePS::FiFoSynchSourcePS()
{
}

FiFoSynchSourcePS::~FiFoSynchSourcePS(void)
{
}

void FiFoSynchSourcePS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    //First we read the data from real general fifo to source fifo's.
    onReadData();

    Pt::System::MutexLock lock(_dataSyncMutex);

    //Now we copy the data to the given buffer, determinate the requested FiFo.
    CircularBuffer* fiFo =  _sourceFifos[portIdx][sourceIdx];

    if(fiFo->isEmpty())
    {
        noOfRecords = 0;
        return;
    }

    
    const Pt::uint8_t* records = fiFo->get( maxNoOfSamples, noOfRecords );
    memcpy( data, records, fiFo->elementSize() * noOfRecords );
    fiFo->next(noOfRecords);
}

void FiFoSynchSourcePS::onReadData()
{

}

void FiFoSynchSourcePS::onInitInstance()
{
    SynchSourcePS::onInitInstance();
    
    //Create the source fifos and the source records
    CircularBuffer*	fiFo		= 0;

    for( Pt::uint32_t portIdx = 0 ; portIdx  < _outputPorts->size(); portIdx++)
    {
        const Port* port = _outputPorts->at(portIdx);
        const Sources& sources = port->sources();	

        std::vector<CircularBuffer*> bufferForSources;
        std::vector<RecordBuilder> records;

        for(Pt::uint32_t srcIdx = 0; srcIdx < sources.size(); srcIdx++)
        {
            const std::vector<Signal*>& source = sources[srcIdx];
            const Signal* signal = source[0];
                    
            //Allocate the FiFo.
            fiFo = new CircularBuffer();
            const Pt::uint32_t dataRecordSize = port->sourceDataSize(srcIdx);

            Pt::uint32_t noOfRecords = static_cast<Pt::uint32_t>(signal->sampleRate() *4); //4 seconds buffer
            fiFo->init( noOfRecords, dataRecordSize );
            bufferForSources.push_back(fiFo);

            //Allocate the record.
            RecordBuilder record;
            std::vector<Pt::uint32_t> itemSizes;
            std::vector<double> rates;
            Pt::uint32_t rateMax = 0;
            Pt::uint32_t sizeMax = 0;

            for( Pt::uint32_t  sig = 0; sig < source.size(); ++sig )
            {
                itemSizes.push_back(source[sig]->valueSize());
                rates.push_back(source[sig]->sampleRate());

                sizeMax = std::max(sizeMax,source[sig]->valueSize()); 
                rateMax = std::max(rateMax,(Pt::uint32_t)source[sig]->sampleRate()); 
            }			
            
            if( sizeMax <= sizeof(Pt::uint64_t))
                rateMax = std::max(rateMax,Pt::uint32_t(100));

    if( rateMax > 2000)
        rateMax /= 3;

            record.init(rateMax, itemSizes, rates, rates[0]);
            records.push_back(record);
        }

        _sourceRecords.push_back( records );
        _sourceFifos.push_back( bufferForSources );

        records.clear();
        bufferForSources.clear();
    }	
}

void FiFoSynchSourcePS::onStart()
{		
    CircularBuffer*	fiFo = 0;
    
    for(Pt::uint32_t portIdx = 0; portIdx < _sourceFifos.size(); portIdx++)
    {
        std::vector<CircularBuffer*>& buffer = _sourceFifos[portIdx];
        for( Pt::uint32_t src =0 ; src < buffer.size(); src++)
        {
            fiFo = buffer[src];
            fiFo->reset();

            _sourceRecords[portIdx][src].reset();
        }
    }
    return SynchSourcePS::onStart();
}


void FiFoSynchSourcePS::onExitInstance()
{  
    //Destroy the source fifos and the source records.	
    CircularBuffer* fiFo;

    for(Pt::uint32_t portIdx = 0; portIdx < _sourceFifos.size(); portIdx++)
    {
        std::vector<CircularBuffer*>& buffer = _sourceFifos[portIdx];
        for( Pt::uint32_t src =0 ; src < buffer.size(); src++)
        {
            fiFo = buffer[src];
            delete fiFo;
        }

        _sourceRecords[portIdx].clear();
        _sourceFifos[portIdx].clear();
    }

    _sourceFifos.clear();
    _sourceRecords.clear();
    SynchSourcePS::onExitInstance();
}

void FiFoSynchSourcePS::lock()
{
    _dataSyncMutex.lock();
}

void FiFoSynchSourcePS::unlock()
{
    _dataSyncMutex.unlock();
}

bool FiFoSynchSourcePS::putRecords( Pt::uint32_t sourceIndex, Pt::uint32_t portIndex, Pt::uint32_t recordCount, const Pt::uint8_t* records)
{
    CircularBuffer* fiFo = _sourceFifos[portIndex][sourceIndex];
    
    if( fiFo->insert( records, recordCount ) != recordCount)
    {
        Message message(format(translate("Mp.Core.Err.LostData"),getName()),Message::Output,Message::Warning,
                        Pt::System::Clock::getLocalTime());

        sendMessage(message);
        fiFo->reset();
        return false;
    }
    return true;
}

bool FiFoSynchSourcePS::putValue(Pt::uint32_t sigIndex, Pt::uint32_t portIndex, const Pt::uint8_t* value)
{
    const Port* port         = _outputPorts->at(portIndex);
    const Pt::uint32_t sourceIdx   =  port->sourceIndex(sigIndex);
    RecordBuilder& record	 = _sourceRecords[portIndex][sourceIdx];
    const Pt::uint32_t sigIdxInSrc =  port->signalIndexInSource(sigIndex);
    
    record.insert(value, sigIdxInSrc);

    //Copy the record in the FiFo	
    const Pt::uint8_t* data1;
    Pt::uint32_t count1 = 0;
    const Pt::uint8_t* data2;
    Pt::uint32_t count2 = 0;

    record.get(&data1, count1, &data2, count2);

    if(count1 != 0)
    {
        CircularBuffer* fiFo = _sourceFifos[portIndex][sourceIdx];

        if(!fiFo->insert(data1, count1))
        {
            Message message(format(translate("Mp.Core.Err.LostData"),getName()), Message::Output, Message::Warning,
                            Pt::System::Clock::getLocalTime());

            sendMessage(message);
            fiFo->reset();
            return false;
        }
    }

    if( count2 != 0 )
    {
        CircularBuffer* fiFo = _sourceFifos[portIndex][sourceIdx];

        if(!fiFo->insert(data2, count2))
        {
            Message message(format(translate("Mp.Core.Err.LostData"),getName()), Message::Output, Message::Warning,
                            Pt::System::Clock::getLocalTime());

            sendMessage(message);
            fiFo->reset();
            return false;
        }
    }

    return true;
}

bool FiFoSynchSourcePS::putValue(const Signal* signal, Pt::uint32_t portIndex, const Pt::uint8_t* value )
{
    //Put the value first to the record, if record full than to the fifo.    	
    return putValue(signalIndex(signal), portIndex, value);
}

}}
