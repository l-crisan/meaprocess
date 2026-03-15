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
#ifndef MPAL_VM_CALLINST_H
#define MPAL_VM_CALLINST_H

#include "Instruction.h"
#include "Function.h"
#include "VirtualMachineImpl.h"
#include "DebuggerContext.h"

namespace mpal{
namespace vm{

class CallInst : public Instruction
{
public:
    CallInst(bool main = false);

    virtual ~CallInst();

    std::string name() const;

    void setInstructionList(Function* func);

    void setStackSize(Pt::uint32_t size);

    virtual Instruction* clone();

    virtual int execute(VMContext& context);

    virtual int executeDebug(DebuggerContext& context);

private:
    void waitCommand(DebuggerContext& context);

    int isBreackPointAktive(const std::vector<BreakPoint>& breakPoints, const Instruction* inst, Pt::System::Mutex& debuggerDataMutex);

    int isBreackPointAktive(const std::vector<BreakPoint>& breakPoints, int line, Pt::System::Mutex& debuggerDataMutex);

private:
    std::vector<Instruction*>* _instList;
    Pt::uint32_t               _stackSize;
    bool                       _main; 
    Function*                  _function; 
    std::string                _ftype;
};

}}

#endif

