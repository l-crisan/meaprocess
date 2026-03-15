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
#include "Operand.h"

namespace mpal{
namespace vm{

Operand::Operand(Pt::uint32_t offset,Type type)
: _offset(offset)
, _size(0)
, _const(0)
, _type(type)
{
}

Operand::Operand(Pt::uint64_t constant)
:  _offset(0)
, _size(0)
, _const(constant)
, _type(Immediate)

{
}

Operand::Operand()
: _offset(0)
, _size(0)
, _const(0)
, _type(NoneType)
{
}

Operand::~Operand(void)
{ 

}
void Operand::setOffset(Pt::uint32_t offset)
{
    _offset = offset;
}

void Operand::setSize( Pt::uint32_t size)
{
    _size = size;
}

Pt::uint32_t Operand::size() const
{
    return _size;
}

Pt::uint8_t* Operand::getOperandPtr(Pt::uint8_t* basePtr) const
{
    switch(_type)
    {

        case Direct:
        case Temporary:
            return (Pt::uint8_t*)(basePtr + _offset);

        case Reference:
        case TemporaryRef:
        {
            size_t* ref = (size_t*)(basePtr + _offset);
            return (Pt::uint8_t*) (*ref);
        }

        case Immediate:
        {
            return (Pt::uint8_t*) & _const;
        }
        default:
        throw std::logic_error("Wrong address");
    }

    throw std::logic_error("Wrong address");
    return 0;
}

Pt::uint8_t* Operand::calcAddress(Pt::uint8_t* basePtr, bool address) const
{
    switch(_type)
    {
        case Direct:
        case Temporary:
            return basePtr + _offset;

        case Reference:
        case TemporaryRef:
        {
            size_t* ref = (size_t*)(basePtr + _offset);
                
            if(address)
                return (Pt::uint8_t*) ref;
            else
                return (Pt::uint8_t*) (*ref);
        }

        case Immediate:
        {
            return (Pt::uint8_t*) & _const;
        }
        default:
            throw std::logic_error("Wrong address");
            return 0;
    }

    throw std::logic_error("Wrong address");
    return 0;
}

}}

