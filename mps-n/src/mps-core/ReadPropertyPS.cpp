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
#include "ReadPropertyPS.h"
#include "PropertySignal.h"
#include <sstream>
#include <Pt/Convert.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <mps/core/Port.h>

namespace mps{
namespace core{

ReadPropertyPS::ReadPropertyPS()
{
}

ReadPropertyPS::~ReadPropertyPS()
{
}

void ReadPropertyPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    const Port* port = ProcessStation::outputPorts()[portIdx];
    const Sources& sources = port->sources();
    const std::vector<Signal*>& source = sources[sourceIdx];
    const Pt::uint32_t sourceSize = port->sourceDataSize(sourceIdx);

    for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
    {
        const Pt::uint32_t recOffset = sourceSize*rec;

        for( Pt::uint32_t srcIdx = 0; srcIdx < source.size(); ++srcIdx)
        {
            const PropertySignal* signal = (const PropertySignal*) source[srcIdx];
            const double value = ProcessStation::getPropertyNumericValue(signal->propName().c_str());
            const Pt::uint32_t offset = port->signalOffsetInSource(sourceIdx, srcIdx);
            memcpy(&data[recOffset + offset], &value, sizeof(double));
        }
    }
}

}}
