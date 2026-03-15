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
#ifndef MPS_MPAL_VM_VIRTUALMACHINEIMPL_H
#define MPS_MPAL_VM_VIRTUALMACHINEIMPL_H

#include <istream>
#include <string>
#include "Loader.h"
#include <Pt/System/Mutex.h>
#include <Pt/System/Condition.h>
#include <Pt/Signal.h>
#include "Instruction.h"

namespace mpal{
namespace vm{

class Instruction;
class ProgramInfoImpl;

class VirtualMachineImpl
{
public:
    VirtualMachineImpl(Pt::Signal<BreakPoint>&  onBreakPoint, Pt::Signal<int,std::string>& onLine, 
        Pt::Signal<>& onTerminate, Pt::Signal<std::string>& onMessage, Pt::uint32_t memorySize);

    virtual ~VirtualMachineImpl(void);

    void setVmMemory(Pt::uint32_t sizeInByte);
    ProgramInfo& load(std::istream& ist);
    void unload( ProgramInfo& prgInfo);
    void execute( ProgramInfo& progInfo, bool debug = false, bool runInLoop = false);
    void clear();
    void clearBreakPoints();
    void insertBreakPoint(const BreakPoint& bp);
    void removeBreakPoint(const BreakPoint& bp);

    
    //Debuging
    void continueExecution();
    void stepOver();
    void stepInto();
    void terminate(); 

    std::string getCallStack();

    std::vector<Pt::uint8_t> readMemory(size_t address, Pt::uint32_t size,const std::string& func);
    std::vector<Pt::uint8_t> readMemoryByOffset(Pt::uint32_t offset, Pt::uint32_t size,const std::string& func);

private:
    void execute(Function* func, const std::string& unit, bool debug, bool runInLoop);

    DebuggerContext*                _context;
    std::vector<Pt::uint8_t>		_stack;	
    std::vector<ProgramInfoImpl*>	_infos;
    Loader					        _loader;
    std::vector<BreakPoint>         _breakPoints;
    Pt::System::Condition           _waitCondition;
    DebuggerCommand::Command        _command;
    std::vector<std::string>        _callStack;
    Pt::Signal<BreakPoint>&         _onBreakPoint;
    Pt::Signal<int, std::string>&   _onLine;
    Pt::Signal<>&                   _onTerminate;
    Pt::Signal<std::string>&        _onMessage;
    Pt::System::Mutex               _mutex;
    Pt::System::Mutex               _debuggerDataMutex;
    Pt::uint32_t                    _memSize;
};

}}
#endif

