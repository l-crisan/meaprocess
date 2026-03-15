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
#include "DemuxPS.h"
#include <mps/core/Port.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include "DemuxSignal.h"
#include <Pt/Math.h>
#include <algorithm>

namespace mps{
namespace calculation{

DemuxPS::DemuxPS()
{
}

DemuxPS::~DemuxPS()
{
}

void DemuxPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    const mps::core::Port* dataInPort = _inputPorts->at(0);
    _inSignal = dataInPort->signalList()->at(0);
    _inSource = dataInPort->sourceIndex(0);
    _inSigOffset = dataInPort->signalOffsetInSource(0);


    const mps::core::Port* selPort = _inputPorts->at(1);
    _selSignal = selPort->signalList()->at(0);
    _selSource = selPort->sourceIndex(0);
    _selSigOffset = selPort->signalOffsetInSource(0);


    std::vector<Pt::uint32_t> itemSizes(2, sizeof( double));
    std::vector<double> rates;
    rates.push_back(_selSignal->sampleRate());
    rates.push_back(_inSignal->sampleRate());

    _sampleIncrement.resize(_outputPorts->at(0)->signalList()->size());

    _inputRecord.init((Pt::uint32_t) std::max(_inSignal->sampleRate(), _selSignal->sampleRate()), itemSizes, rates, _inSignal->sampleRate()); 
}


void DemuxPS::onStart()
{
    for( Pt::uint32_t i = 0; i < _sampleIncrement.size(); ++i)
        _sampleIncrement[i] = 0;

    FiFoSynchSourcePS::onStart();
}

void DemuxPS::onReadData()
{
    const Pt::uint8_t* data1;
    Pt::uint32_t count1;
    const Pt::uint8_t* data2;
    Pt::uint32_t count2;

    _inputRecord.get(&data1, count1, &data2, count2);

    if( count1 != 0)
        processRecords(data1,count1);

    if( count2 != 0)
        processRecords(data2,count2);
}

void DemuxPS::processRecords(const Pt::uint8_t* data, Pt::uint32_t count)
{
    const mps::core::Port* outPort = _outputPorts->at(0);
    const mps::core::SignalList* signalList = outPort->signalList();

    for( Pt::uint32_t rec = 0; rec < count; ++rec)
    {
        const Pt::uint32_t recOffset = rec* sizeof(double)*2;

        double* sel = (double*) &data[recOffset];
        double* value =  (double*) &data[recOffset + sizeof(double)];

        for(Pt::uint32_t i = 0; i < signalList->size(); ++i)
        {
            const DemuxSignal* outSignal = (const DemuxSignal*) signalList->at(i);
            _sampleIncrement[i] += (outSignal->sampleRate() /_inSignal->sampleRate() );

            const double outValue = *sel == i ? *value :outSignal->noValue();

            int z = 0;

            for( ; z < (int)_sampleIncrement[i]; ++z)
                putValue(i,0,(Pt::uint8_t*) &outValue);

            _sampleIncrement[i] -= z;
        }
    }
}

void DemuxPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if( port->portNumber() == 1)
    {
        if(sourceIdx == _selSource)
        {
            const Pt::uint32_t sourceSize = port->sourceDataSize(sourceIdx);

            for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
            {
                 const Pt::uint8_t* pdata = &data[ rec * sourceSize + _selSigOffset]; 
                 const double value = _selSignal->scaleValue(pdata);
                 _inputRecord.insert((const Pt::uint8_t*) &value, 0);
            }
        }
    }
    else 
    {
        if( sourceIdx == _inSource)
        {
            const Pt::uint32_t sourceSize = port->sourceDataSize(sourceIdx);

            for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
            {
                 const Pt::uint8_t* pdata = &data[ rec * sourceSize + _inSigOffset]; 
                 const double value = _inSignal->scaleValue(pdata);
                 _inputRecord.insert((const Pt::uint8_t*) &value, 1);
            }
        }
    }
}

}}
