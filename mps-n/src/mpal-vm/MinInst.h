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
#ifndef MPAL_VM_MININST_H
#define MPAL_VM_MININST_H

#include "Instruction.h"
#include <algorithm>

namespace mpal{
namespace vm{

template<typename T>
class MinInst : public Instruction
{
public:
    MinInst()
    { }

    ~MinInst()
    { }

    virtual Instruction* clone()
    {
        return new MinInst();
    }

    virtual int execute(VMContext& context)
    {
        const Pt::uint32_t count = _op1->getOperandValue<Pt::uint32_t>(context.basePtr);
            
        context.stackPtr -= sizeof(std::size_t);

        T* min = (T*) (*(std::size_t*) (context.stackPtr));

        for( int i = 1; i < static_cast<int>(count); ++i)
        {
                context.stackPtr -= sizeof(std::size_t);
            T* next = (T*) (*(std::size_t*) (context.stackPtr));
            *min = std::min(*min, *next);
        }

        _result->setOperandValue(context.basePtr, *min);

        return -1;
    }
};

}}

#endif

