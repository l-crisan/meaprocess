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
#ifndef MPS_CALCULATION_SCALINGPS_H
#define MPS_CALCULATION_SCALINGPS_H

#include <string>
#include <map>

#include <Pt/Types.h>
#include <Pt/Any.h>

#include <mps/core/FiFoSynchSourcePS.h>
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include "CalcScaling.h"
#include <mps/core/ObjectVector.h>
#include <map>

namespace mps{
namespace calculation{

class ScalingPS : public mps::core::FiFoSynchSourcePS
{
public:
    ScalingPS();
    virtual ~ScalingPS();

    virtual void onInitInstance();

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);

    virtual void addObject(mps::core::Object* obj, const std::string& type, const std::string& subType);

private:
    struct ScalingInfo
    {
        const CalcScaling*  scaling;
        const mps::core::Signal* outSignal;
    };

    const mps::core::Signal* getOutputSignal(Pt::uint32_t sigId);

    static inline double getFactor(PointF p1, PointF p2)
    {
        return ((double)p2.second - (double)p1.second) /(p2.first - p1.first);
    }

    static inline double getOffset(PointF p1, PointF p2)
    {
        return ((double)(p1.second * p2.first) - (double)(p2.second* p1.first)) /(p2.first - p1.first);
    }

private:
    mps::core::ObjectVector<CalcScaling*>* _scalings;
    std::map<Pt::uint32_t, std::vector<ScalingInfo> > _sig2scaling;
    typedef std::map<Pt::uint32_t, std::vector<ScalingInfo> >::iterator Sig2ScalingIt;
};

}}

#endif
