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
#include "Signal.h"

namespace mps {
namespace can {

 Signal::Signal(Pt::uint32_t id)
: core::Signal(id)
, _byteCount(0)
, _byteOrder(0)
, _signalType(0)
, _modeValue(0)
, _pivotBit(0)
, _bitCount(0)
, _canDataType(0)
, _modeBitCount(0)
, _modeByteOrder(0)
, _modeFactor(0)
, _modeOffset(0)
, _modeCanDataType(0)
, _modePivotBit(0)
{
    registerProperty("id", *this, &Signal::msgId, &Signal::setMsgId);
    registerProperty("byteCount", *this, &Signal::byteCount, &Signal::setByteCount);
    registerProperty("byteOrder", *this, &Signal::byteOrder, &Signal::setByteOrder);
    registerProperty("signalType", *this, &Signal::signalType, &Signal::setSignalType);
    registerProperty("modeValue", *this, &Signal::modeValue, &Signal::setModeValue);
    registerProperty("pivotBit", *this, &Signal::pivotBit, &Signal::setPivotBit);
    registerProperty("bitCount", *this, &Signal::bitCount, &Signal::setBitCount);
    registerProperty("canDataType", *this, &Signal::canDataType, &Signal::setCanDataType);
    registerProperty("modeBitCount", *this, &Signal::modeBitCount, &Signal::setModeBitCount);
    registerProperty("modeByteOrder", *this, &Signal::modeByteOrder, &Signal::setModeByteOrder);
    registerProperty("modeFactor", *this, &Signal::modeFactor, &Signal::setModeFactor);
    registerProperty("modeOffset", *this, &Signal::modeOffset, &Signal::setModeOffset);
    registerProperty("modeDataType", *this, &Signal::modeCanDataType, &Signal::setModeCanDataType);
    registerProperty("modePivotBit", *this, &Signal::modePivotBit, &Signal::setModePivotBit);
}


Signal::~Signal(void)
{
}

}}
