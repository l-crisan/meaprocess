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
#include "Function.h"

#include "Parameter.h"
#include "Instruction.h"


namespace mpal{
namespace vm{

Function::Function(const char* name, Type type, Pt::uint32_t stackSize, Pt::uint32_t fbSize, Pt::int32_t lineBegin, Pt::int32_t lineEnd)
:  _type(type)
, _stackSize(stackSize)
, _fbSize(fbSize)
, _name(name)
, _lineBegin(lineBegin)
, _lineEnd(lineEnd)
{
}

Function::~Function(void)
{
    for(Pt::uint32_t i = 0; i< _parameter.size(); ++i)
        delete _parameter[i];

    for(Pt::uint32_t j = 0; j < _instructions.size(); ++j)
        delete _instructions[j];
}


std::vector<Parameter*>& Function::parameter()
{
    return _parameter;
}

std::vector<Instruction*>& Function::instructions()
{
    return _instructions;
}

std::string Function::name() const
{
    return _name;
}

Function::Type Function::type() const
{
    return _type;
}

Pt::uint32_t Function::stackSize() const
{
    return _stackSize;
}

Pt::uint32_t Function::fbSize() const
{
    return _fbSize;
}

Pt::int32_t Function::lineBegin() const
{
    return _lineBegin;
}

Pt::int32_t Function::lineEnd() const
{
    return _lineEnd;
}

}}

