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
#include "ScalingPS.h"
#include <mps/core/SignalList.h>

namespace mps{
namespace calculation{

ScalingPS::ScalingPS()
: _scalings(0)
{
}

ScalingPS::~ScalingPS()
{
    if(_scalings != 0)
    {
        for( Pt::uint32_t i = 0; i < _scalings->size(); ++i)
            delete _scalings->at(i);

        delete _scalings;
        _scalings = 0;
    }
}

void ScalingPS::addObject(Object* obj, const std::string& type, const std::string& subType)
{
    if( type == "Mp.Calculation.Scalings")
        _scalings = (mps::core::ObjectVector<CalcScaling*>*) obj;
    else
        FiFoSynchSourcePS::addObject(obj, type, subType);
}

const mps::core::Signal* ScalingPS::getOutputSignal(Pt::uint32_t sigId)
{
    const mps::core::Port* outPort = _outputPorts->at(0);

    for( Pt::uint32_t i = 0; i < outPort->signalList()->size(); ++i)
    {
        const mps::core::Signal* signal = outPort->signalList()->at(i);
        
        if( signal->signalID() == sigId)
            return signal;
    }

    return 0;
}

void ScalingPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    for( Pt::uint32_t i  = 0; i < _scalings->size(); ++i)
    {
        const CalcScaling* scaling = _scalings->at(i);
        Sig2ScalingIt it = _sig2scaling.find(scaling->signal());
        const mps::core::Signal* outSignal = getOutputSignal(scaling->outSignal());

        if( it == _sig2scaling.end())
        {
            std::vector<ScalingInfo> scalingInfos;
            ScalingInfo scalingInfo;
            scalingInfo.outSignal = outSignal;
            scalingInfo.scaling = scaling;
            scalingInfos.push_back(scalingInfo);

            std::pair< Pt::uint32_t, std::vector<ScalingInfo> > pair(scaling->signal(), scalingInfos);

            _sig2scaling.insert(pair);
        }
        else
        {
            ScalingInfo scalingInfo;
            scalingInfo.outSignal = outSignal;
            scalingInfo.scaling = scaling;
            it->second.push_back(scalingInfo);
        }
    }
}

void ScalingPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
    const Pt::uint32_t inRecordSize = port->sourceDataSize(sourceIdx);
    double scaledValue = 0;

    for( Pt::uint32_t i = 0; i < source.size(); ++i)
    {
        const mps::core::Signal* signal = source[i];
        Sig2ScalingIt it = _sig2scaling.find(signal->signalID());

        if( it == _sig2scaling.end())
            continue;

        const Pt::uint32_t sigOffset = port->signalOffsetInSource(sourceIdx, i);

        for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            const Pt::uint8_t* pvalue = &data[rec * inRecordSize + sigOffset];
            const double value = signal->scaleValue(pvalue);

            for( Pt::uint32_t k = 0; k < it->second.size(); ++k)
            {
                ScalingInfo& scaleInfo = it->second[k];

                switch(scaleInfo.scaling->type())
                {
                    case 0: //Linear scaling
                    {
                        scaledValue = scaleInfo.scaling->factor() * value + scaleInfo.scaling->offset();
                        putValue(scaleInfo.outSignal,0, (Pt::uint8_t*) &scaledValue);
                    }
                    break;

                    case 1: //Two point scaling
                    {
                        PointF p1(scaleInfo.scaling->p1x(), scaleInfo.scaling->p1y());
                        PointF p2(scaleInfo.scaling->p2x(), scaleInfo.scaling->p2y());
                        scaledValue = getFactor(p1,p2) * value + getOffset(p1,p2);

                        putValue(scaleInfo.outSignal,0, (Pt::uint8_t*) &scaledValue);
                    }
                    break;
                    
                    case 2: //Table scaling
                    {            
                        bool scaled = false;
                        const std::vector<PointF>& scalingTable = scaleInfo.scaling->tableData();

                        for (Pt::uint32_t j = 1; j < scalingTable.size(); ++j)
                        {
                            const PointF& p1 = scalingTable[j - 1];
                            const PointF& p2 = scalingTable[j];

                            if( j == scalingTable.size() - 1)
                            {
                                if (value >= p1.first && value <= p2.first)
                                {
                                    scaledValue = value * getFactor(p1, p2) + getOffset(p1,p2);
                                    putValue(scaleInfo.outSignal,0, (Pt::uint8_t*) &scaledValue);
                                    scaled = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (value >= p1.first && value < p2.first)
                                {
                                    scaledValue = value * getFactor(p1, p2) + getOffset(p1,p2);
                                    putValue(scaleInfo.outSignal,0, (Pt::uint8_t*) &scaledValue);
                                    scaled = true;
                                    break;
                                }
                            }
                        }

                        if( !scaled )
                            putValue(scaleInfo.outSignal,0, (Pt::uint8_t*) &value);
                    }
                    break;
                }
            }
        }
    }
}

}}
