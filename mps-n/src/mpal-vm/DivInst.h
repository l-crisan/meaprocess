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
#ifndef MPAL_VM_DIVINST_H
#define MPAL_VM_DIVINST_H

#include "Instruction.h"

namespace mpal{
namespace vm{

template <typename T>
class DivInst : public Instruction
{
public:
    DivInst()
    { }

    ~DivInst()
    { }

    virtual Instruction* clone()
    {
        return new DivInst();
    }

    virtual int execute(VMContext& context)
    {
        const T op1 = _op1->getOperandValue<T>(context.basePtr);

        const T op2 = _op2->getOperandValue<T>(context.basePtr);

        if( op2 == 0 )
        {
            std::stringstream ss;
            ss<<" Function '"<<context.currentFunction;
            ss<<"' "<<LineInfo::lineInfo(uid());
            ss<<": error R1001: Division by zero";
            throw std::logic_error(ss.str());
        }

        const T result =  op1 / op2;

        _result->setOperandValue(context.basePtr, result);
        return -1;
    }
};

}}

#endif

