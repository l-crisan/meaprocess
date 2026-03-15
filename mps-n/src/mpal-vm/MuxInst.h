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
#ifndef MPAL_VM_MUXINST_H
#define MPAL_VM_MUXINST_H

#include "Instruction.h"

namespace mpal{
namespace vm{

template<typename T>
class MuxInst : public Instruction
{
public:
    MuxInst()
    { }

    ~MuxInst()
    { }

    virtual Instruction* clone()
    {
        return new MuxInst();
    }

    virtual int execute(VMContext& context)
    {
        Pt::uint8_t*  result = (Pt::uint8_t*) _result->calcAddress(context.basePtr);
        const Pt::uint32_t size = _op1->getOperandValue<Pt::uint32_t>(context.basePtr);
        const Pt::uint32_t count = _op2->getOperandValue<Pt::uint32_t>(context.basePtr);

        context.stackPtr -= sizeof(std::size_t);
        
        T* k = (T*)  (*(std::size_t*) (context.stackPtr));
        const long sel = (long) *k;

        if( sel > static_cast<long>(count) || sel < 0)
        { //Out of range = >default the first.
            void* p = (void*) (*(std::size_t*) (context.stackPtr - sizeof(std::size_t)));
            memcpy(result, p, size);
        }
        else
        {
            void* p =  (void*) (*(std::size_t*) (context.stackPtr - ( (*k+1) * sizeof(std::size_t)) ));
            memcpy(result, p, size);
        }

        //Clear the stack
        context.stackPtr -= ( count * sizeof(std::size_t));

        return -1;
    }
};

}}

#endif

