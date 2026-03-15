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
#include "ConvertPS.h"
#include "ConvertSignal.h"
#include <mps/core/SignalList.h>
#include <cmath>

namespace mps{
namespace calculation{

enum ConvertType
{
    ConvertCast = 0,
    ConvertRound,
    ConvertFloor,
    ConvertCeiling,
    ConvertAbs,
};


ConvertPS::ConvertPS()
{
}

ConvertPS::~ConvertPS()
{
}

double ConvertPS::rounding(double r)
{//Todo; verify this
    return (int)(r + 0.5);
}


std::vector<Pt::uint8_t> ConvertPS::castValue(core::SignalDataType::Type toType, double value)
{
    std::vector<Pt::uint8_t> cv;

    switch (toType)
    {
        case core::SignalDataType::VT_bool:
            cv.resize(1);
            cv[0] = value != 0;
        break;

        case core::SignalDataType::VT_real64:
            cv.resize(8);
            memcpy(&cv[0], &value, 8);
        break;

        case core::SignalDataType::VT_real32:
        {
            float vv = (float)value;
            cv.resize(sizeof(vv));
            memcpy(&cv[0], &vv, sizeof(vv));
        }
        break;

        case core::SignalDataType::VT_uint8_t:
        {
            Pt::uint8_t vv = (Pt::uint8_t)value;
            cv.resize(sizeof(vv));
            cv[0] = vv;
        }
        break;

        case core::SignalDataType::VT_int8_t:
        {
            Pt::int8_t vv = (Pt::int8_t)value;
            cv.resize(sizeof(vv));
            memcpy(&cv[0], &vv, sizeof(vv));
        }
        break;

        case core::SignalDataType::VT_uint16_t:
        {
            Pt::uint16_t vv = (Pt::uint16_t)value;
            cv.resize(sizeof(vv));
            memcpy(&cv[0], &vv, sizeof(vv));
        }
        break;

        case core::SignalDataType::VT_int16_t:
        {
            Pt::int16_t vv = (Pt::int16_t)value;
            cv.resize(sizeof(vv));
            memcpy(&cv[0], &vv, sizeof(vv));
        }
        break;


        case core::SignalDataType::VT_uint32_t:
        {
            Pt::uint32_t vv = (Pt::uint32_t)value;
            cv.resize(sizeof(vv));
            memcpy(&cv[0], &vv, sizeof(vv));
        }
        break;

        case core::SignalDataType::VT_int32_t:
        {
            Pt::int32_t vv = (Pt::int32_t)value;
            cv.resize(sizeof(vv));
            memcpy(&cv[0], &vv, sizeof(vv));
        }
        break;

        case core::SignalDataType::VT_uint64_t:
        {
            Pt::uint64_t vv = (Pt::uint64_t)value;
            cv.resize(sizeof(vv));
            memcpy(&cv[0], &vv, sizeof(vv));
        }
        break;

        case core::SignalDataType::VT_int64_t:
        {
            Pt::int64_t vv = (Pt::int64_t)value;
            cv.resize(sizeof(vv));
            memcpy(&cv[0], &vv, sizeof(vv));
        }
        break;
    }

    return cv;
}


mps::core::Signal* ConvertPS::getInputSignal(Pt::uint32_t sigId)
{
    mps::core::Port* port = _inputPorts->at(0);

    for( Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        core::Signal* signal = port->signalList()->at(i);
        
        if( signal->signalID() == sigId)
            return signal;
    }

    return 0;
}

void ConvertPS::onInitialize()
{
    Base::onInitialize();

    _in2OutMap.clear();

    const mps::core::Port* outPort = _outputPorts->at(0);

    for (Pt::uint32_t i = 0; i < outPort->signalList()->size(); ++i)
    {
        const ConvertSignal* signal = (ConvertSignal*) outPort->signalList()->at(i);

        const core::Signal* inSignal = getInputSignal(signal->fromSignal());

        if (inSignal == 0)
            continue;

        std::pair<const core::Signal*, const core::Signal*> pair(inSignal, signal);
        _in2OutMap.insert(pair);
    }

}


void ConvertPS::onDeinitialize()
{
    Base::onDeinitialize();
    _in2OutMap.clear();
}


void ConvertPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
    const Pt::uint32_t inRecordSize = port->sourceDataSize(sourceIdx);
    
    double scaledValue = 0;
    double convertedValue = 0;

    for( Pt::uint32_t i = 0; i < source.size(); ++i)
    {
        const mps::core::Signal* signal = source[i];

        In2OutSigMap::iterator it = _in2OutMap.find(signal);

        if( it == _in2OutMap.end())
            continue;

        const Pt::uint32_t sigOffset = port->signalOffsetInSource(sourceIdx, i);

        for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        {
            const Pt::uint8_t* pvalue = &data[rec * inRecordSize + sigOffset];
            const double value = signal->scaleValue(pvalue);

            const ConvertSignal* outSignal = (const ConvertSignal*)it->second;

            switch (outSignal->convert())
            {
                case ConvertCast:
                    convertedValue = value;
                break;

                case ConvertRound:
                    convertedValue = rounding( value);
                break;

                case ConvertFloor:
                    convertedValue = std::floor(value);
                break;

                case ConvertCeiling:
                    convertedValue = std::ceil(value);
                break;

                case ConvertAbs:
                    convertedValue = std::abs(value);
                break;
            }

            std::vector<Pt::uint8_t> vv = castValue((core::SignalDataType::Type) outSignal->valueDataType(), convertedValue);

            if( vv.size() != 0)
                putValue(outSignal,0, (Pt::uint8_t*) &vv[0]);
        }
    }
}

}}
