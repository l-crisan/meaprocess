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
#ifndef MPAL_VM_SHLINST_H
#define MPAL_VM_SHLINST_H

#include "Instruction.h"
#include <stdio.h>
#include <math.h>

namespace mpal{
namespace vm{

template <typename T>
class ShlInst : public Instruction
{
public:
    ShlInst()
    { }

    ~ShlInst()
    { }

    virtual Instruction* clone()
    {
        return new ShlInst();
    }

    virtual int execute(VMContext& context)
    {
        const T src = _op1->getOperandValue<T>(context.basePtr);

        const Pt::int16_t count = _op2->getOperandValue<Pt::int16_t>(context.basePtr);

        const T res = src << count;

        _result->setOperandValue(context.basePtr, res);
        return -1;
    }
};

}}

#endif

