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
#ifndef MPS_MPAL_VM_OPERAND_H
#define MPS_MPAL_VM_OPERAND_H

#include <Pt/Types.h>
#include <Pt/Byteorder.h>
#include <iostream>
#include <stdexcept>

namespace mpal{
namespace vm {

class Operand
{
public:
    
    enum Type
    {
        NoneType = 0,
        Direct,
        Immediate,
        Reference,
        Temporary,
        TemporaryRef
    };

    Operand(Pt::uint32_t offset, Type type);
    Operand(Pt::uint64_t constant);
    Operand();

    ~Operand(void);

    void setOffset(Pt::uint32_t offset);

    void setSize( Pt::uint32_t size);

    Pt::uint32_t size() const;


    template< typename T> 
    T getOperandValue(Pt::uint8_t* basePtr) const
    {
        T val;
        memcpy(&val, getOperandPtr(basePtr), sizeof(T));
        return val;
    }

    template< typename T>
    void setOperandValue(Pt::uint8_t* basePtr, const T& value) const
    {
        memcpy(getOperandPtr(basePtr), &value,  sizeof(T));
    }

    Pt::uint8_t* calcAddress(Pt::uint8_t* basePtr, bool address = false) const;

    Pt::uint8_t* getOperandPtr(Pt::uint8_t* basePtr) const;

private:
    Pt::uint32_t		 _offset;
    Pt::uint32_t		 _size;
    mutable Pt::uint64_t _const;
    Type		 _type;

};


}}

#endif

