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
#include "BitGroupingPS.h"
#include <mps/core/SignalList.h>
#include <sstream>
#include <algorithm>


namespace mps{
namespace calculation{

BitGroupingPS::BitGroupingPS()
{

}

BitGroupingPS::~BitGroupingPS()
{
}

void BitGroupingPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    _outSignal = (const BitGroupingSignal*) _outputPorts->at(0)->signalList()->at(0);


    std::stringstream ss;

    ss << _outSignal->sigMapping();

    char buffer[50];
    Pt::uint32_t sigIdx = 0;
    while(ss.getline(buffer,50,';'))
    {
        std::stringstream ss2;
        ss2 << buffer;
        char buffer2[30];
        ss2.getline(buffer2,30,',');
        
        std::stringstream ss3;
        ss3 << buffer2;
        Pt::uint32_t id;
        ss3>>id;

        ss2.getline(buffer2,30,',');
        std::stringstream ss4;
        ss4 << buffer2;
        Pt::uint32_t bit;
        ss4>>bit;
        Id2OutIdxIt it = _id2OutIdx.find(id);

        if( it == _id2OutIdx.end())
        {
            OutItem outItem;
            outItem.bits.push_back(bit);
            outItem.sigIdx = sigIdx;
            sigIdx++;
            std::pair<Pt::uint32_t, OutItem > pair(id, outItem);
            _id2OutIdx.insert(pair);
        }
        else
        {
            it->second.bits.push_back(bit);
        }
    }
    
    Id2OutIdxIt it = _id2OutIdx.begin();
    
    std::vector<double> rates;
    std::vector<Pt::uint32_t> sizes;
    double maxRate = 0;

    for( Pt::uint32_t i = 0; it != _id2OutIdx.end(); ++it, ++i)
    {
        const mps::core::Signal* signal = getInputSignal(it->first);
        rates.push_back(signal->sampleRate());
        sizes.push_back(sizeof(double));
        maxRate = std::max(maxRate, signal->sampleRate());
    }

    maxRate = maxRate/3;
    maxRate = std::max(maxRate,100.0);

    _record.init(static_cast<Pt::uint32_t>(maxRate),sizes,rates,_outSignal->sampleRate());
}

const mps::core::Signal* BitGroupingPS::getInputSignal(Pt::uint32_t id)
{
    const mps::core::Port* port = _inputPorts->at(0);

    for( Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        const mps::core::Signal* signal = port->signalList()->at(i);
    
        if( signal->signalID() == id)
            return signal;
    }

    return 0;
}

void BitGroupingPS::onReadData()
{
    const Pt::uint8_t* data1;
    Pt::uint32_t count1;
    const Pt::uint8_t* data2;
    Pt::uint32_t count2;

    _record.get(&data1, count1, &data2, count2);

    if( count1 != 0)
        processData( data1, count1);

    if( count2 != 0)
        processData( data2, count2);
}

void BitGroupingPS::processData(const Pt::uint8_t* data, Pt::uint32_t records)
{
    double* pvalue = (double*) data;

    Pt::uint64_t outData = 0;

    Pt::uint32_t items = (Pt::uint32_t)_id2OutIdx.size();
    for( Pt::uint32_t rec = 0; rec < records; ++rec)
    {
        Id2OutIdxIt it = _id2OutIdx.begin();

        for( ;it != _id2OutIdx.end(); ++it)
        {
            const double& value  = pvalue[it->second.sigIdx + rec * items];

            for(Pt::uint32_t i = 0; i < it->second.bits.size(); ++i)
            {
                Pt::uint64_t mask = value != 0 ? 1 : 0;

                Pt::uint8_t bit = it->second.bits[i];
                mask = mask << bit;
                outData |= mask;
            }
        }
    }

    putValue(_outSignal,0, (Pt::uint8_t*)&outData);
}

void BitGroupingPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];    
    const Pt::uint32_t inRecordSize = port->sourceDataSize(sourceIdx);
    
    for( Pt::uint32_t i = 0; i < source.size(); ++i)
    {
        const mps::core::Signal* signal = source[i];
        Id2OutIdxIt it = _id2OutIdx.find(signal->signalID());

        if( it == _id2OutIdx.end())
            continue;

        const Pt::uint32_t sigOffset = port->signalOffsetInSource(sourceIdx, i);

        for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            const Pt::uint8_t* pvalue = &data[rec * inRecordSize + sigOffset];
            const double value = signal->scaleValue(pvalue);
            _record.insert((Pt::uint8_t*) &value, it->second.sigIdx);
        }
    }
}

}}
