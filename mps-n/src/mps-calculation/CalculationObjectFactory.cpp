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
#include "mps-calculation.h"
#include "CalculationObjectFactory.h"
#include "CalcScaling.h"
#include <mps/core/ObjectVector.h>
#include "ScalingPS.h"
#include "MovingMeanPS.h"
#include "MovingMeanSignal.h"
#include "MeanNPS.h"
#include "ElectricitySignal.h"
#include "ElectricityPS.h"
#include "SampleRateSignal.h"
#include "SampleRatePS.h"
#include "MultiplexerPS.h"
#include "DemuxPS.h"
#include "DemuxSignal.h"
#include "BitExtractionSignal.h"
#include "BitExtractionPS.h"
#include "BitGroupingSignal.h"
#include "BitGroupingPS.h"
#include "SignalDelayPS.h"
#include "DelaySignal.h"
#include "ConvertPS.h"
#include "ConvertSignal.h"


namespace mps{
namespace calculation{

CalculationObjectFactory::CalculationObjectFactory()
{
}

std::string CalculationObjectFactory::resourceID() const
{
    return "mps-calculation";
}

CalculationObjectFactory::~CalculationObjectFactory()
{

}

mps::core::Object* CalculationObjectFactory::createObject( const Pt::String& type, const Pt::String& subType, Pt::uint32_t id )
{
    if(type == L"Mp.Calculation.PS.Scaling")
    {
        return new ScalingPS();
    }
    else if( type == L"Mp.Calculation.PS.MovingMean")
    {
        return new MovingMeanPS();
    }
    else if( type == L"Mp.Calculation.PS.MeanN")
    {
        return new MeanNPS();
    }
    else if( type == L"Mp.Calculation.Scaling")
    {	
        return new CalcScaling();
    }
    else if( type == L"Mp.Calculation.Scalings")
    {
        return new mps::core::ObjectVector<CalcScaling*>();
    }
    else if( subType == L"Mp.Calculation.Sig.MovingMean")
    {
        return new MovingMeanSignal(id);
    }
    else if( subType == L"Mp.Calculation.Sig.Elec")
    {
        return new ElectricitySignal(id);
    }
    else if( type == L"Mp.Calculation.PS.Elec")
    {
        return new ElectricityPS();
    }
    else if( subType == L"Mp.Calculation.Sig.SampleRate")
    {
        return new SampleRateSignal(id);
    }
    else if( type == L"Mp.Calculation.PS.SampleRate")
    {
        return new SampleRatePS();
    }
    else if( type == L"Mp.Calculation.PS.Mux")
    {
        return new MultiplexerPS();
    }
    else if(subType == L"Mp.Calculation.Sig.Demux")
    {
        return new DemuxSignal(id);
    }
    else if( type == L"Mp.Calculation.PS.Demux")
    {
        return new DemuxPS();
    }
    else if( subType == L"Mp.Calculation.Sig.BitEx")
    {
        return new BitExtractionSignal(id);
    }
    else if( type == L"Mp.Calculation.PS.BitEx")
    {
        return new BitExtractionPS();
    }
    else if( subType == L"Mp.Calculation.Sig.BitGrp")
    {
        return new BitGroupingSignal(id);
    }
    else if( type == L"Mp.Calculation.PS.BitGrp")
    {
        return new BitGroupingPS();
    }
    else if( subType == L"Mp.Calculation.Sig.Delay")
    {
        return new DelaySignal(id);
    }
    else if( type == L"Mp.Calculation.PS.Delay")
    {
        return new SignalDelayPS();
    }
    else if (subType == "Mp.Calculation.Sig.Convert")
    {
        return new ConvertSignal(id);
    }
    else if (type == "Mp.Calculation.PS.SignalConvert")
    {
        return new ConvertPS();
    }

    return 0;
}

}}

extern "C"
{
    MPSCALCULATION_API mps::core::ObjectFactory* mpsGetFactory()
    {
        static mps::calculation::CalculationObjectFactory factory;
        return &factory;
    }
}
