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
#include "Parameter.h"

namespace mpal{
namespace vm{

Parameter::Parameter( const char* name, DataType type, Pt::uint64_t uid, const char* typeId, Access access, Pt::uint32_t offset, Pt::uint32_t size)
: _type(type)
, _access(access)
, _name(name)
, _udtName("")
, _struct()
, _dimensions()
, _uid(uid)
, _typeId(typeId)
, _defaultValue()
, _size(size)
, _offset(offset)
, _defValueBuffer()
, _enumList()
{
}

Parameter::~Parameter(void)
{
    for(Pt::uint32_t i = 0; i < _struct.size(); ++i)
        delete  _struct[i];
}


DataType Parameter::type() const
{
    return _type;
}

Access Parameter::access() const
{
    return _access;
}

void Parameter::setAccess(Access ac)
{
    _access = ac;
}

const std::string& Parameter::name() const
{
    return _name;
}

std::vector<Parameter*>& Parameter::structured()
{
    return _struct;
}

Dimensions& Parameter::dimensions()
{
    return _dimensions;
}

std::string Parameter::typeId() const
{
    return _typeId;
}

Pt::Any Parameter::defaultValue() const
{
    return _defaultValue;
}

void Parameter::setDefaultValue(Pt::Any& defValue)
{
    _defaultValue = defValue;
}

Pt::uint32_t Parameter::size()
{
    return _size;
}

void Parameter::setSize(Pt::uint32_t size)
{
    _size = size;
}

const std::string& Parameter::UDTName() const
{
    return _udtName;
}

void Parameter::setUDTName(const char* name)
{
    _udtName = name;
}

Pt::uint32_t Parameter::offset() const
{
    return _offset;
}


std::vector<Pt::uint8_t>& Parameter::defValueBuffer() 
{ 
    return  _defValueBuffer;
}

std::vector<std::string>& Parameter::enumList()
{
    return _enumList;
}
}}

