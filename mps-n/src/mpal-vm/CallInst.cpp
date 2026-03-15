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
#include "CallInst.h"

namespace mpal{
namespace vm{

CallInst::CallInst(bool main)
: _instList(0)
, _stackSize(0)
, _main(main)
, _function(0)
, _ftype("")
{ 
}

CallInst::~CallInst()
{ 
}

Instruction* CallInst::clone()
{
    return new CallInst();
}

std::string CallInst::name() const
{
    if( _function != 0)
        return _function->name();

    return "";
}

void CallInst::setInstructionList(Function* func)
{
    _instList = &func->instructions();
    _function = func;

    switch(_function->type())
    {
        case Function::PG:
            _ftype = "PGR";
        break;
        case Function::FB:
            _ftype = "FB";
        break;
        case Function::FC:
            _ftype = "FC";
        break;
    }
}

void CallInst::setStackSize(Pt::uint32_t size)
{
    _stackSize = size;
}

int CallInst::execute(VMContext& context)
{
    int nextInst;
    Pt::uint32_t i = 0;
    Pt::uint8_t* parentBasePtr = context.basePtr;
    std::string parentFunction = context.currentFunction;
    context.currentFunction = name();
            
    if(!_main)
        context.basePtr = context.stackPtr - _stackSize;

    while( i < _instList->size())
    {
        nextInst = _instList->at(i)->execute(context);
                
        if( nextInst == -1)
        {//next instruction
            ++i;
        }
        else if( nextInst == -2)
        {//Return
            break;
        }
        else
        {//jump
            i = nextInst;
        }
    }	
            
    //Clear the stack.
    context.stackPtr = context.basePtr;
            
    if(!_main)
        context.basePtr  = parentBasePtr;

    context.currentFunction = parentFunction;

    return -1;
}

int CallInst::executeDebug(DebuggerContext& context)
{ 
    int     nextInst;
    Pt::uint32_t  i = 0;		
    bool    endFunctionEvent = true;

    DebuggerCommand::Command stepOverCmd = DebuggerCommand::Continue;
    
    Pt::uint8_t* parentBasePtr = context.basePtr;

    //Setup the stack pointer
    if(!_main)
        context.basePtr = context.stackPtr - _stackSize;

    //Update the call stack
    context.debuggerDataMutex.lock();

    std::string str = name() + ":" + _ftype;
    context.callStack.push_back(str);
    std::string parentFunction = context.currentFunction;
    context.currentFunction = name();
    
    context.debuggerDataMutex.unlock();

    if(context.cmd == DebuggerCommand::StepInto || context.cmd == DebuggerCommand::StepOver)
    {//Event of first instruction
        Instruction* is = _instList->at(0);
        int line = static_cast<int>(is->uid() >> 32);
        context.onLine.send(line, _function->name());
    }
    
    while( i < _instList->size())
    {               
        //Check break point
        context.debuggerDataMutex.lock();
        int breakPointIdx = isBreackPointAktive(context.breakPoints, _instList->at(i), context.debuggerDataMutex);

        if(breakPointIdx != -1 || context.cmd == DebuggerCommand::StepInto || context.cmd == DebuggerCommand::StepOver)
        {

            if(breakPointIdx != -1)
            {
                BreakPoint bp = context.breakPoints[breakPointIdx];
                context.debuggerDataMutex.unlock();

                context.onBreakPoint.send(bp);
            }
            else
            {
                context.debuggerDataMutex.unlock();
            }

            waitCommand(context);
        }
        else
        {
            context.debuggerDataMutex.unlock();
        }

        //If terminate return
        if( context.cmd == DebuggerCommand::Terminate)
        {
            context.debuggerDataMutex.lock();
            Pt::uint32_t endPos =  context.callStack.size() - 1;
            context.callStack.erase(context.callStack.begin() + endPos); 
            context.currentFunction = "";
            context.debuggerDataMutex.unlock();
            return -3;
        }

        //Check execute by step
        bool executeNextLine = false;
        int startLine = 0;

        Instruction* is = _instList->at(i);
        startLine = static_cast<int>(is->uid() >> 32);

        if( breakPointIdx != -1 || context.cmd == DebuggerCommand::StepOver || context.cmd == DebuggerCommand::StepInto)
        {//If step or break point execute until next line
            
            executeNextLine = true;
           
            if(startLine == _function->lineEnd())
                endFunctionEvent = false;
        }

        bool breaked = false;
        //If step execute until next line if not step execute one instruction
        while(i < _instList->size())
        {   
            Instruction* inst = _instList->at(i);

            if( executeNextLine)
            {
                int curLine = static_cast<int>(inst->uid() >> 32);

                if( curLine != startLine)
                {
                    if(context.cmd != DebuggerCommand::Continue)
                        context.onLine.send(curLine, _function->name());

                    break;
                }
            }

            if(context.cmd == DebuggerCommand::StepOver)
            {
                DebuggerCommand::Command oldCmd = context.cmd;
                context.cmd = stepOverCmd;
                nextInst = inst->executeDebug(context);
                context.cmd = oldCmd;
            }
            else
            {
                nextInst = inst->executeDebug(context);
            }
            
            if( context.cmd == DebuggerCommand::StepInto  || 
                context.cmd == DebuggerCommand::StepOver)
                executeNextLine = true;

            if( nextInst == -1)
            {//next instruction
                ++i;
            }
            else if( nextInst == -2)
            {//Return
                breaked = true;
                break;
            }
            else if( nextInst == -3)
            {
                context.currentFunction = "";
                return -3;
            }
            else
            {//jump
                i = nextInst;
            }

            if( !executeNextLine )
                break;
        }

        if( breaked )
            break;
    }

    if(endFunctionEvent)
    {
        if(context.cmd == DebuggerCommand::StepInto || context.cmd == DebuggerCommand::StepOver)
        {
            context.onLine.send(_function->lineEnd(), _function->name());
            waitCommand(context);

            if( context.cmd == DebuggerCommand::Terminate)
            {
                context.currentFunction = "";
                return -3;
            }
        }
        else
        {
            context.debuggerDataMutex.lock();
            int breakPointIndex = isBreackPointAktive(context.breakPoints, _function->lineEnd(), context.debuggerDataMutex);

            if(breakPointIndex != -1)
            {
                BreakPoint bp = context.breakPoints[breakPointIndex];

                context.debuggerDataMutex.unlock();

                context.onBreakPoint.send(bp);

                waitCommand(context);
            }
            else
            {
                context.debuggerDataMutex.unlock();
            }

            if( context.cmd == DebuggerCommand::Terminate)
            {
                context.currentFunction = "";
                return -3;   
            }
        }
    }
    else
    {
        if( context.cmd == DebuggerCommand::Terminate)
        {
            context.currentFunction = "";
            return -3;
        }
    }

    context.debuggerDataMutex.lock();

    Pt::uint32_t endPos =  context.callStack.size() - 1;
    context.callStack.erase(context.callStack.begin() + endPos);
    context.currentFunction = parentFunction;
    
    context.debuggerDataMutex.unlock();

    context.stackPtr = context.basePtr;
    
    if(!_main)
        context.basePtr = parentBasePtr;

    return -1;
}

void CallInst::waitCommand(DebuggerContext& context)
{
    Pt::System::Mutex mutex;

    mutex.lock();
    context.condition.wait(mutex,-1);
    mutex.unlock();
}

int CallInst::isBreackPointAktive(const std::vector<BreakPoint>& breakPoints, const Instruction* inst, Pt::System::Mutex& debuggerDataMutex)
{
    int line = static_cast<int>(inst->uid() >> 32);

    for( Pt::uint32_t i = 0; i < breakPoints.size(); ++i)
    {
        const BreakPoint& breakPoint = breakPoints[i];
        
        if(breakPoint.line() == line && _function->name() == breakPoint.funcName())
            return (int) i;
    }

    return -1;
}

int CallInst::isBreackPointAktive(const std::vector<BreakPoint>& breakPoints, int line,  Pt::System::Mutex& debuggerDataMutex)
{
    for( Pt::uint32_t i = 0; i < breakPoints.size(); ++i)
    {
        const BreakPoint& breakPoint = breakPoints[i];
        
        if((breakPoint.line() + 1) == line && name() == breakPoint.funcName())
            return (int) i;
    }

    return -1;
}

}}

