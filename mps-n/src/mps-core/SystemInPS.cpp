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
#include "SystemInPS.h"
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>

#include <Pt/Types.h>

namespace mps{
namespace core{

SystemInPS::SystemInPS()
: _objId(0)
, _callBack(0)
{ }

SystemInPS::~SystemInPS()
{ }

void SystemInPS::onStart()
{
    SynchSourcePS::onStart();
    if(_callBack != 0)
        _callBack(_objId, 1, 0, 0, 0 );
}

void SystemInPS::onStop()
{
    if(_callBack != 0)
        _callBack(_objId, 2, 0, 0, 0 );

    SynchSourcePS::onStop();
}

void SystemInPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    if(_callBack == 0)
        return;

    const Port* outPort = _outputPorts->at(portIdx);
    const Sources& sources = outPort->sources();
    const std::vector<Signal*>& source = sources[sourceIdx];
    Pt::uint8_t* outData = 0;
    const Pt::uint32_t recordSize = outPort->sourceDataSize(sourceIdx);

    for( Pt::uint32_t sigIdx = 0; sigIdx < source.size(); sigIdx++)
    {
        const Signal* signal = source[sigIdx];
        const Pt::uint32_t sigIndex = signalIndex(signal);
        const Pt::uint32_t sigOffset = outPort->signalOffsetInSource(sigIndex);
        
        for( Pt::uint32_t rec =0 ; rec < noOfRecords; rec++)
        {
            outData	   = &data[(rec * recordSize) + sigOffset];
            _callBack(_objId, 0, (Pt::uint32_t) sigIndex, (Pt::uint32_t) signal->valueSize(), outData );
        }
    }
}

}}
