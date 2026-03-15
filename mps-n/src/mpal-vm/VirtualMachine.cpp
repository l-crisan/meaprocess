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
#include <mpal/vm/VirtualMachine.h>
#include "VirtualMachineImpl.h"

namespace mpal{
namespace vm{

VirtualMachine::VirtualMachine(Pt::uint32_t memorySize)
:_impl(new VirtualMachineImpl(onBreakPoint, onLine, onTerminate, onMessage, memorySize))
{

}

VirtualMachine::~VirtualMachine()
{
    delete _impl;
}

void VirtualMachine::setVmMemory(Pt::uint32_t sizeInByte)
{
    _impl->setVmMemory(sizeInByte);
}

ProgramInfo& VirtualMachine::load(std::istream& ist)
{
    return _impl->load(ist);
}

void VirtualMachine::unload(ProgramInfo& progInfo)
{
    _impl->unload(progInfo);
}

void VirtualMachine::execute(ProgramInfo& progInfo, bool debug, bool runInLoop)
{
    _impl->execute(progInfo, debug, runInLoop);
}

void VirtualMachine::clear()
{
    _impl->clear();
}

void VirtualMachine::clearBreakPoints()
{
    _impl->clearBreakPoints();
}

void VirtualMachine::insertBreakPoint(const BreakPoint& bp)
{
    _impl->insertBreakPoint(bp);
}

void VirtualMachine::removeBreakPoint(const BreakPoint& bp)
{
    _impl->removeBreakPoint(bp);
}

void VirtualMachine::continueExecution()
{
    _impl->continueExecution();
}

void VirtualMachine::stepOver()
{
    _impl->stepOver();
}

void VirtualMachine::stepInto()
{
    _impl->stepInto();
}

void VirtualMachine::terminate()
{
    _impl->terminate();
}

std::vector<Pt::uint8_t> VirtualMachine::readMemory(size_t address, Pt::uint32_t size, const std::string& func)
{
    return _impl->readMemory(address, size, func);
}

std::vector<Pt::uint8_t> VirtualMachine::readMemoryByOffset(Pt::uint32_t offset, Pt::uint32_t size, const std::string& func)
{
   return _impl->readMemoryByOffset(offset, size, func);
}

std::string VirtualMachine::getCallStack()
{
    return _impl->getCallStack();
}

}}

