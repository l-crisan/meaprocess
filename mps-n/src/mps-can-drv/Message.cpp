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
#include <mps/can/drv/Message.h>
#include <memory.h>

namespace mps{
namespace can{
namespace drv{

Message::Message(Pt::uint32_t identifier, const Pt::uint8_t* data, Pt::uint32_t dlc, Pt::uint64_t timeStamp)
: _dlc(dlc)
, _identifier(identifier)
, _timeStamp(timeStamp)
{
    memset(&_data[0], 0, 8);
    memcpy(&_data[0], data, dlc);
}

Message::Message()
:  _dlc(8)
, _identifier(0)
, _timeStamp(0)
{
    memset(&_data[0], 0, 8);
}

Message::~Message()
{

}

Pt::uint8_t* Message::data()
{
    return &_data[0];
}

const Pt::uint8_t* Message::data() const
{
    return &_data[0];
}

void Message::setData(const Pt::uint8_t* data, Pt::uint32_t dlc)
{
    _dlc = dlc;
    memcpy(&_data[0], data, dlc);
}

Pt::uint32_t Message::dlc() const
{
    return _dlc;
}

void Message::setDlc(Pt::uint32_t s)
{
    _dlc = s;
}

void Message::setIdentifier(Pt::uint32_t id)
{
    _identifier = id;
}

Pt::uint32_t Message::identifier() const
{
    return _identifier;
}

void Message::setTimeStamp(Pt::uint64_t t)
{
    _timeStamp = t;
}

Pt::uint64_t Message::timeStamp() const
{
    return _timeStamp;
}

}}}
