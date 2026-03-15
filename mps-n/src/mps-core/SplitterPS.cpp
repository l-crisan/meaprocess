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
#include <mps/core/SignalList.h>
#include <mps/core/Port.h>
#include "SplitterPS.h"

namespace mps{
namespace core{

SplitterPS::SplitterPS(void)
{
}

SplitterPS::~SplitterPS(void)
{
}

void SplitterPS::onInitInstance()
{
    ProcessStation::onInitInstance();

    //Create the input/output signal mapping.

    _inOutPosMap.resize(_inputPorts->size());

    for( Pt::uint32_t portIdx = 0; portIdx < _inputPorts->size(); portIdx++ )
    {
        const Sources& sources = _inputPorts->at(portIdx)->sources();
        
        for( Pt::uint32_t src = 0; src < sources.size(); src++ )
        {
            const std::vector<Signal*>& source = sources[src];

            for( Pt::uint32_t sig = 0; sig < source.size(); sig++)
            {
                const Signal* inSignal = source[sig];
                std::vector<MapPortItem> mappingItems  = searchOutSignal(inSignal->signalID());
                
                if(mappingItems.size() != 0)
                    _inOutPosMap[portIdx][inSignal->signalID()] = mappingItems;
            }
        }
    }

    //Allocate the data memory for output sources.
    for( Pt::uint32_t outPortIdx = 0; outPortIdx < _outputPorts->size(); outPortIdx++ )
    {
        const Port* port = _outputPorts->at(outPortIdx);
        const Sources& sources = port->sources();		

        std::vector<std::vector<Pt::uint8_t> > outSourceData;
        std::vector<std::vector<bool> > updateSourceMask;

        for( Pt::uint32_t src = 0; src < sources.size(); src++)
        {
            const std::vector<Signal*>& source = sources[src];
            const Pt::uint32_t dataSize = port->sourceDataSize(src);

            std::vector<Pt::uint8_t> outData;
            outData.resize(dataSize);
            outSourceData.push_back(outData);

            std::vector<bool> updateMask;
            updateMask.resize(source.size(),false);
            updateSourceMask.push_back(updateMask);
        }
        _sourceUpdateArray.push_back(updateSourceMask);
        _outDataArray.push_back(outSourceData );
    }


    for( Pt::uint32_t outPortIdx = 0; outPortIdx < _outputPorts->size(); outPortIdx++ )
    {
        const Port* outPort = _outputPorts->at(outPortIdx);
        const Sources& sources = outPort->sources();	

        for( Pt::uint32_t srcIdx = 0; srcIdx < sources.size(); ++srcIdx)
        {
            //Get the output data.
            std::vector<Pt::uint8_t>& outDataArray = _outDataArray[outPortIdx][srcIdx];
            const std::vector<Signal*>& signals = sources.at(srcIdx);
            const Signal* signal = signals[0];
            const Pt::uint32_t outRecordSize = outPort->sourceDataSize(srcIdx);
            
            //Resize the outdata array.
            outDataArray.resize(static_cast<Pt::uint32_t>(5* signal->sampleRate() * outRecordSize)); // 5 seconds buffer
        }
    }
}

std::vector<SplitterPS::MapPortItem> SplitterPS::searchOutSignal(Pt::uint32_t sinSigID)
{
    std::vector<MapPortItem> mapPortItems;
    
    for( Pt::uint32_t outPortIdx = 0; outPortIdx < _outputPorts->size(); outPortIdx++ )
    {
        const Port* port = _outputPorts->at(outPortIdx);
        const Sources& outSources = port->sources();

        for( Pt::uint32_t outSrc = 0; outSrc < outSources.size(); outSrc++)
        {
            const std::vector<Signal*> outSource = outSources[outSrc];

            for( Pt::uint32_t outSigIdx = 0; outSigIdx < outSource.size(); outSigIdx++ )
            {
                const Signal* outSignal = outSource[outSigIdx];		
                
                if( outSignal->signalID() == sinSigID )
                {
                    MapPortItem mapPortItem;
                    mapPortItem.portIdx = outPortIdx;
                    mapPortItem.sourceIdx = outSrc;
                    mapPortItem.signalIdxInSrc = outSigIdx;
                    mapPortItem.offsetInSource = port->signalOffsetInSource(outSrc, outSigIdx);
                    mapPortItems.push_back(mapPortItem);
                }
            }
        }
    }

    return mapPortItems;
}

void SplitterPS::onExitInstance()
{
    _outDataArray.clear();
    _sourceUpdateArray.clear();
    _inOutPosMap.clear();
    ProcessStation::onExitInstance();
}

bool SplitterPS::isOutSourceFull( std::vector<bool>& updateArray)
{
    for( Pt::uint32_t i = 0; i < updateArray.size(); i++)
    {
        if( !updateArray[i])
            return false;
    }

    return true;
}
    
void SplitterPS::clearUpdateArray(std::vector<bool>& updateArray)
{
    for( Pt::uint32_t i = 0; i < updateArray.size(); ++i)
        updateArray[i] = false;
}


void SplitterPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Port* port, const Pt::uint8_t* data)
{
    const Pt::uint8_t* inData = 0;
    Pt::uint8_t* outData = 0;
    const std::vector<Signal*>& inSource = port->sources()[sourceIdx];

    std::map<Pt::uint32_t, std::vector<MapPortItem> >::iterator it;
    
    for( Pt::uint32_t sig = 0; sig < inSource.size(); ++sig )
    {
        //Get the signal.
        const Signal* signal = inSource[sig];

        it = _inOutPosMap[port->portNumber()].find(signal->signalID());

        //Sould we put out the signal data?
        if( it == _inOutPosMap[port->portNumber()].end())
            continue;

        //Get the in record size.
        const Pt::uint32_t inRecordSize = port->sourceDataSize(sourceIdx);

        for(Pt::uint32_t item =0 ; item <  it->second.size(); ++item )
        {
            MapPortItem& outDescr = it->second.at(item);

            //Get the output port.
            Port* outPort = _outputPorts->at(outDescr.portIdx);

            //Get the output data.
            Pt::uint8_t* outDataArray = &_outDataArray[outDescr.portIdx][outDescr.sourceIdx][0];
            const Pt::uint32_t outRecordSize = outPort->sourceDataSize(outDescr.sourceIdx);		
            const Pt::uint32_t inSignalOffset = port->signalOffsetInSource(sourceIdx, sig);

            //Copy the signal values.
            for( Pt::uint32_t rec = 0 ; rec < noOfRecords; ++rec)
            {
                inData = &data[(rec*inRecordSize) + inSignalOffset];
                outData = &outDataArray[(rec*outRecordSize)+ outDescr.offsetInSource];
                memcpy(outData,inData,signal->valueSize());
            }

            //Update the out update mask.
            _sourceUpdateArray[outDescr.portIdx][outDescr.sourceIdx][outDescr.signalIdxInSrc] = true;
        
            //Check the out update mask.
            if( isOutSourceFull(_sourceUpdateArray[outDescr.portIdx][outDescr.sourceIdx]) )
            {  //Send the data to the out port.
                outPort->onUpdateDataValue( noOfRecords,outDescr.sourceIdx, outDataArray);
                //Clear the update mask.
                clearUpdateArray(_sourceUpdateArray[outDescr.portIdx][outDescr.sourceIdx]);
            }
        }
    }
}

}}
