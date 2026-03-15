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
#include "VirtualMachineImpl.h"
#include "ProgramInfoImpl.h"
#include "Instruction.h"
#include "Function.h"
#include "CallInst.h"
#include "Unit.h"
#include "DebuggerContext.h"
#include "VMContext.h"

#include <iostream>
#include <fstream>

#include <Pt/System/FileInfo.h> 
#include <Pt/System/Directory.h> 

namespace mpal{
namespace vm{

VirtualMachineImpl::VirtualMachineImpl(Pt::Signal<BreakPoint>&  onBreakPoint, Pt::Signal<int, std::string>& onLine,
                                       Pt::Signal<>& onTerminate,Pt::Signal<std::string>& onMessage,  Pt::uint32_t memorySize)
: _context(0)
, _stack()
, _infos()
, _loader()
, _breakPoints()
, _waitCondition()
, _command(DebuggerCommand::Continue)
, _callStack()
, _onBreakPoint(onBreakPoint)
, _onLine(onLine)
, _onTerminate(onTerminate)
, _onMessage(onMessage)
, _mutex()
, _debuggerDataMutex()
, _memSize(memorySize)
{
    _stack.resize(_memSize, 0);
    _context = new DebuggerContext(&_stack[0], &_stack[0], &_stack[0], _stack.size(), _debuggerDataMutex, _breakPoints, _waitCondition, _onBreakPoint, _command, _onLine, _callStack);  
}

VirtualMachineImpl::~VirtualMachineImpl(void)
{ 
    clear();
    delete _context;
}

void VirtualMachineImpl::setVmMemory(Pt::uint32_t sizeInByte)
{
    _memSize = sizeInByte;
    _stack.resize(_memSize, 0);
    
    delete _context;
    _context = new DebuggerContext(&_stack[0], &_stack[0], &_stack[0], _stack.size(), _debuggerDataMutex, _breakPoints, _waitCondition, _onBreakPoint, _command, _onLine, _callStack);  
}

void VirtualMachineImpl::clear()
{
    if(_stack.size() != 0 && _context != 0)
    {
        _context->stackPtr = &_stack[0];
        _context->basePtr =  &_stack[0];
        _context->baseAdr =  &_stack[0];
    }

    for(Pt::uint32_t i = 0; i < _infos.size(); ++i)
    {
        ProgramInfoImpl* info = _infos[i];
        delete info;
    }

    _infos.clear();
}

ProgramInfo& VirtualMachineImpl::load(std::istream& ist)
{
    Unit* unit = _loader.loadUnit(ist);
    
    if( unit == 0)
        throw std::logic_error("error R1003: Unit coudn't be loaded");

    //Create the program info.
    ProgramInfoImpl* progInfo = new ProgramInfoImpl(unit);
    
    _infos.push_back(progInfo);

    return *progInfo;
}

void VirtualMachineImpl::unload(ProgramInfo& prgInfo)
{
    for(Pt::uint32_t i = 0; i < _infos.size(); ++i)
    {
        ProgramInfoImpl* info = _infos[i];

        if( info == &prgInfo)
        {
            _infos.erase(_infos.begin() + i);
            delete info;
            return;
        }
    }
}

void VirtualMachineImpl::execute(ProgramInfo& progInfo, bool debug, bool runInLoop)
{
    Pt::System::MutexLock lock(_mutex);

    ProgramInfoImpl* pInfo = (ProgramInfoImpl*) &progInfo;    

    Unit* unit = pInfo->unit();
    
    //Execute the unit
    Function* main = pInfo->main();

    if( main == 0)
    {
        std::string msg = "error R1001: Program not found in this unit";
        _onMessage.send(msg);
        _onTerminate.send();
        throw std::logic_error(msg);
    }

    if( _memSize <=  main->stackSize())
    {
        std::string msg = "error R1007: Not enough memory to run this program";
        _onMessage.send(msg);
        _onTerminate.send();
        throw std::runtime_error(msg);
    }

    _context->stackPtr = &_stack[0];
    _context->basePtr =  &_stack[0];
    _context->baseAdr =  &_stack[0];

    //Set the stack pointer to the end of the current unit stack.
    memcpy(_context->stackPtr, pInfo->inStack(), main->stackSize());	

    _context->stackPtr += main->stackSize();

    try
    {
        execute(main, unit->name(), debug, runInLoop);
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
        throw;
    }
}

std::vector<Pt::uint8_t> VirtualMachineImpl::readMemory(size_t address, Pt::uint32_t size,const std::string& func)
{
    Pt::System::MutexLock lock(_debuggerDataMutex);

    std::vector<Pt::uint8_t> returnVal(size,0);

  if(_context == 0)
        return returnVal;

    if( func != _context->currentFunction)
        return returnVal;

  Pt::uint32_t* ptr = (Pt::uint32_t*) address;
  memcpy(&returnVal[0], ptr, size);
  return returnVal;
}

std::vector<Pt::uint8_t> VirtualMachineImpl::readMemoryByOffset(Pt::uint32_t offset, Pt::uint32_t size, const std::string& func)
{
    Pt::System::MutexLock lock(_debuggerDataMutex);
    
    std::vector<Pt::uint8_t> returnVal(size,0);

    if(_context == 0)
        return returnVal;

    if( func != _context->currentFunction)
        return returnVal;

  Pt::uint8_t* ptr = (Pt::uint8_t*) (_context->basePtr + offset);
  memcpy(&returnVal[0], ptr, size);
  return returnVal;
}

std::string VirtualMachineImpl::getCallStack()
{
    Pt::System::MutexLock lock(_debuggerDataMutex);
  std::stringstream ss;

  for(Pt::uint32_t i = 0; i < _callStack.size(); ++i)
      ss<<_callStack[i]<<std::endl;

  return ss.str();
}

void VirtualMachineImpl::execute(Function* function, const std::string& unit, bool debug, bool runInLoop)
{ 
    CallInst inst(true);
    
    inst.setInstructionList(function);

    if(debug)
    {
        try
        {            
            _command = DebuggerCommand::Continue;
            _callStack.clear();
            inst.executeDebug(*_context);
            _onTerminate.send();
        }
        catch(const std::exception& ex)
        {
            std::string str = ex.what();
            _onMessage.send(str);
            _onTerminate.send();
        }
    }
    else
    {
        inst.execute(*_context);
    }
}

void VirtualMachineImpl::clearBreakPoints()
{
    Pt::System::MutexLock lock(_debuggerDataMutex);
    _breakPoints.clear();
}

void VirtualMachineImpl::insertBreakPoint(const BreakPoint& bp)
{
    Pt::System::MutexLock lock(_debuggerDataMutex);
    _breakPoints.push_back(bp);
}

void VirtualMachineImpl::removeBreakPoint(const BreakPoint& bp)
{
    Pt::System::MutexLock lock(_debuggerDataMutex);

    for(Pt::uint32_t i = 0; i < _breakPoints.size(); ++i)
    {
        BreakPoint& curBp = _breakPoints[i];
        if( curBp.line() == bp.line() && curBp.funcName() == bp.funcName())
        {
            _breakPoints.erase(_breakPoints.begin() + i);
            return;
        }
    }
}

void VirtualMachineImpl::continueExecution()
{
    _command = DebuggerCommand::Continue;
    _waitCondition.signal();
}

void VirtualMachineImpl::stepOver()
{
    _command = DebuggerCommand::StepOver;
    _waitCondition.signal();   
}

void VirtualMachineImpl::stepInto()
{   
    _command = DebuggerCommand::StepInto;
    _waitCondition.signal();
}

void VirtualMachineImpl::terminate()
{
    _command = DebuggerCommand::Terminate;
    _waitCondition.signal();
}

}}

