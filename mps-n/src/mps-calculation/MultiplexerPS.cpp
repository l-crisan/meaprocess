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
#include "MultiplexerPS.h"
#include <sstream>
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <algorithm>

namespace mps{
namespace calculation{

MultiplexerPS::MultiplexerPS()
{
    registerProperty( "signals", *this, &MultiplexerPS::signals, &MultiplexerPS::setSignals );
}

MultiplexerPS::~MultiplexerPS()
{
}

void MultiplexerPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    std::vector<double> rates;

    _selSignal = _inputPorts->at(1)->signalList()->at(0);
    rates.push_back(_selSignal->sampleRate());
    Pt::uint32_t rateMax = static_cast<Pt::uint32_t>(_selSignal->sampleRate());

    for(Pt::uint32_t i = 0; i < _signalList.size(); ++i)
    {
        const mps::core::Signal* signal = getInputSignal(_signalList[i]);
        rates.push_back(signal->sampleRate());
        rateMax = std::max((Pt::uint32_t)signal->sampleRate(), rateMax);

        SigId2IdxIt it = _sigId2Idx.find(signal->signalID());

        if( it == _sigId2Idx.end())
        {
            std::vector<Pt::uint32_t> indexes;
            indexes.push_back(i+1);
            std::pair<Pt::uint32_t,std::vector<Pt::uint32_t> > pair(signal->signalID(),  indexes);
            _sigId2Idx.insert(pair);
        }
        else
        {
            it->second.push_back(i);
        }
    }

    std::vector<Pt::uint32_t> indexes;
    indexes.push_back(0);
    std::pair<Pt::uint32_t,std::vector<Pt::uint32_t> > pair(_selSignal->signalID(),  indexes);
    _sigId2Idx.insert(pair);

    _outSignal = _outputPorts->at(0)->signalList()->at(0);

    std::vector<Pt::uint32_t> itemSizes(rates.size(), sizeof(double));
    _recordBuilder.init(rateMax*2, itemSizes, rates, _outSignal->sampleRate());
}

void MultiplexerPS::onStart()
{
    FiFoSynchSourcePS::onStart();
}

const mps::core::Signal* MultiplexerPS::getInputSignal(Pt::uint32_t sigId)
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

void MultiplexerPS::onReadData()
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

void MultiplexerPS::processRecords(const Pt::uint8_t* data, Pt::uint32_t count)
{
    for( Pt::uint32_t rec = 0; rec < count; ++rec)
    {
        int selValue = (int)*((double*) &data[(_signalList.size() + 1) * sizeof(double) * rec]);
          
        if(selValue >= 0 && selValue < static_cast<int>(_signalList.size()))
            selValue++;
        else
            selValue = (int) _signalList.size();

        double* pdata = (double*) &data[(_signalList.size() +1) * sizeof(double) * rec + sizeof(double)*selValue];

        putValue(_outSignal,0, (Pt::uint8_t*) pdata);
    }
}

void MultiplexerPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
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

void MultiplexerPS::setSignals(const std::string& signals)
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
