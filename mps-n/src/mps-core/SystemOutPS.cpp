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
#include "SystemOutPS.h"
#include <mps/core/SignalScaling.h>
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>

namespace mps{
namespace core{

SystemOutPS::SystemOutPS()
{ }

SystemOutPS::~SystemOutPS()
{ }

void SystemOutPS::onStart()
{
    for( Pt::uint32_t i = 0; i < _dataListeners.size(); ++i )
        _dataListeners[i].callBack(_dataListeners[i].objId, 1, 0, 0, 0, 0, 0);

    ProcessStation::onStart();
}

void SystemOutPS::onStop()
{
    ProcessStation::onStop();

    for( Pt::uint32_t i = 0; i < _dataListeners.size(); ++i )
        _dataListeners[i].callBack(_dataListeners[i].objId, 2, 0, 0, 0, 0, 0);
}

void SystemOutPS::addDataListener(Pt::uint32_t objId, mpsOnData callBack)
{
    Listener listener;
    listener.objId = objId;
    listener.callBack = callBack;
    _dataListeners.push_back( listener );
}

void SystemOutPS::onUpdateDataValue( Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Port* port, const Pt::uint8_t* data )
{
    const Sources& sources = port->sources();
    const std::vector<Signal*>& source = sources[sourceIdx];

    if( 0 == source.size() )
        return;

    const Pt::uint32_t recordSize =  port->sourceDataSize(sourceIdx);
    const int dataSize = (int)(recordSize* noOfRecords);

    for( Pt::uint32_t i = 0; i < _dataListeners.size(); ++i )
        _dataListeners[i].callBack(_dataListeners[i].objId, 0, (Pt::uint32_t)noOfRecords, (Pt::uint32_t)sourceIdx, (Pt::uint32_t)port->portNumber(), (Pt::uint32_t)dataSize, data);
}

}}

