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
#ifndef MPS_MPAL_VM_INSTRUCTION_H
#define MPS_MPAL_VM_INSTRUCTION_H

#include <Pt/Types.h>
#include <Pt/SmartPtr.h>
#include <Pt/System/Condition.h>
#include <Pt/Signal.h>
#include <mpal/vm/VirtualMachine.h>
#include <mpal/vm/BreakPoint.h>
#include <sstream>
#include <string>
#include "LineInfo.h"
#include "Operand.h"
#include "DebuggerContext.h"
#include "VMContext.h"

namespace mpal{
namespace vm {


class Instruction
{
public:
    Instruction();
    
    virtual ~Instruction(void);

    virtual Instruction* clone() = 0;	

    virtual int execute(VMContext& context) = 0;

    virtual int executeDebug(DebuggerContext& context);
    
    void setResult(Pt::SmartPtr<Operand> result);

    void setOp1(Pt::SmartPtr<Operand> op1);
    
    void setOp2(Pt::SmartPtr<Operand> op2);
    
    void setUid( Pt::uint64_t uid );

    Pt::uint64_t uid() const;

protected:
    Pt::SmartPtr<Operand> _result;
    Pt::SmartPtr<Operand> _op1;
    Pt::SmartPtr<Operand> _op2;
    Pt::uint64_t          _uid;
};

}}

#endif

