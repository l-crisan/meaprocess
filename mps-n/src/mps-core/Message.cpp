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
#include <mps/core/Message.h>

namespace mps{
namespace core{

Message::Message(void)
: _text("")
, _comment("")
, _fileName("")
, _target(Output)
, _type(Info)
, _timeStamp(0)
, _errorCode(0)
{
}

Message::Message(const std::string& text, MessageTarget target, MessageType type, const Pt::DateTime& timeStamp)
: _text(text)
, _comment("")
, _fileName("")
, _target(target)
, _type(type)
, _timeStamp(0)
, _errorCode(0)
{
    setTimeStamp(timeStamp);
}

Message::~Message(void)
{
}

bool Message::operator==(const Message& msg) const
{
    if( _text != msg._text )
        return false;

    if( _comment != msg._comment )
        return false;

    if( _fileName != msg._fileName )
        return false;
        
    if( _target != msg._target)
        return false;

    if( _type != msg._type )
        return false;
        
    if( _timeStamp != msg._timeStamp )
        return false;

    return true;
}

bool Message::operator!=(const Message& msg)
{
    return !(*this == msg);
}

}}
