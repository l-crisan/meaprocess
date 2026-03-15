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
#ifndef MPS_CALCULATION_MULTIPLEXERPS_H
#define MPS_CALCULATION_MULTIPLEXERPS_H

#include <string>
#include <map>

#include <Pt/Types.h>
#include <Pt/Any.h>

#include <mps/core/FiFoSynchSourcePS.h>
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include "MovingMeanSignal.h"
#include <map>
#include <vector>
#include <mps/core/RecordBuilder.h>

namespace mps{
namespace calculation{

class MultiplexerPS : public mps::core::FiFoSynchSourcePS
{
public:
    MultiplexerPS();
    virtual ~MultiplexerPS();

    virtual void onInitInstance();

    virtual void onStart(); 

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);

    inline const std::string&  signals() const
    {
        return _signals;
    }

    void setSignals(const std::string& signals);

    virtual void onReadData();

private:
    const mps::core::Signal* getInputSignal(Pt::uint32_t sigId);

private:
    std::string _signals;
    std::vector<Pt::uint32_t> _signalList;
    mps::core::RecordBuilder _recordBuilder;
    std::map<Pt::uint32_t, std::vector<Pt::uint32_t> > _sigId2Idx;
    typedef std::map<Pt::uint32_t, std::vector<Pt::uint32_t> >::iterator SigId2IdxIt;
    const mps::core::Signal* _outSignal;
    const mps::core::Signal* _selSignal;
    void processRecords(const Pt::uint8_t* data, Pt::uint32_t count);
};

}}

#endif
