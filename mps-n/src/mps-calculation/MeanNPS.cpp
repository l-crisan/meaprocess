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
#include "MeanNPS.h"
#include <sstream>
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <algorithm>

namespace mps{
namespace calculation{

MeanNPS::MeanNPS()
{
    registerProperty( "signals", *this, &MeanNPS::signals, &MeanNPS::setSignals );
}

MeanNPS::~MeanNPS()
{
}

void MeanNPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    std::vector<double> rates;

    Pt::uint32_t rateMax = 0;

    for(Pt::uint32_t i = 0; i < _signalList.size(); ++i)
    {
        const mps::core::Signal* signal = getInputSignal(_signalList[i]);
        rates.push_back(signal->sampleRate());
        rateMax = std::max((Pt::uint32_t)signal->sampleRate(), rateMax);
        SigId2IdxIt it = _sigId2Idx.find(signal->signalID());

        if( it == _sigId2Idx.end())
        {
            std::vector<Pt::uint32_t> indexes;
            indexes.push_back(i);
            std::pair<Pt::uint32_t,std::vector<Pt::uint32_t> > pair(signal->signalID(),  indexes);
            _sigId2Idx.insert(pair);
        }
        else
        {
            it->second.push_back(i);
        }
    }

    _outSignal = _outputPorts->at(0)->signalList()->at(0);

    std::vector<Pt::uint32_t> itemSizes(rates.size(), sizeof(double));
    _recordBuilder.init(rateMax*2, itemSizes, rates, _outSignal->sampleRate());
}

void MeanNPS::onStart()
{
    FiFoSynchSourcePS::onStart();
}

const mps::core::Signal* MeanNPS::getInputSignal(Pt::uint32_t sigId)
{
    const mps::core::Port* port = _inputPorts->at(0);

    for( Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        const mps::core::Signal* signal = port->signalList()->at(i);
        
        if( signal->signalID() == sigId)
            return signal;
    }

    return 0;
}

void MeanNPS::onReadData()
{
    const Pt::uint8_t* data1;
    Pt::uint32_t count1;
    const Pt::uint8_t* data2;
    Pt::uint32_t count2;

    _recordBuilder.get(&data1, count1, &data2, count2);

    if( count1 != 0)
        processRecords(data1,count1);

    if( count2 != 0)
        processRecords(data2,count2);
}

void MeanNPS::processRecords(const Pt::uint8_t* data, Pt::uint32_t count)
{
    for( Pt::uint32_t rec = 0; rec < count; ++rec)
    {
        double sum = 0;
        for(Pt::uint32_t i = 0; i < _signalList.size(); ++i)
        {
            double* pdata = (double*) &data[_signalList.size() * sizeof(double) * rec + sizeof(double)*i];
            sum += *pdata;
        }
        sum /= _signalList.size();
        putRecords(0,0,1, (Pt::uint8_t*) &sum);
    }
}

void MeanNPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];    
    const Pt::uint32_t inRecordSize = port->sourceDataSize(sourceIdx);

    for( Pt::uint32_t i = 0; i < source.size(); ++i)
    {
        const mps::core::Signal* signal = source[i];
        SigId2IdxIt it = _sigId2Idx.find(signal->signalID());

        if( it == _sigId2Idx.end())
            continue;

        const Pt::uint32_t sigOffset = port->signalOffsetInSource(sourceIdx, i);

        for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            const Pt::uint8_t* pvalue = &data[rec * inRecordSize + sigOffset];
            const double value = signal->scaleValue(pvalue);
            for( Pt::uint32_t j = 0; j < it->second.size(); ++j)
            {
                Pt::uint32_t index = it->second[j];
                _recordBuilder.insert((const Pt::uint8_t*) &value, index);
            }
        }
    }
}

void MeanNPS::setSignals(const std::string& signals)
{
    std::stringstream ss;
    char buffer[100];

    ss << signals;

    while(ss.getline(buffer,100,';'))
    {
        std::stringstream ss1;
        ss1 << buffer;
        Pt::uint32_t id;
        ss1>>id;
        _signalList.push_back(id);
    }
}

}}
