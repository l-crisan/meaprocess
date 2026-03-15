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

#ifndef MPAL_VM_VIRTUALMACHINE_H
#define MPAL_VM_VIRTUALMACHINE_H

#include <mpal/vm/ProgramInfo.h>
#include <mpal/vm/BreakPoint.h>
#include <Pt/Signal.h>

namespace mpal{
namespace vm{

class VirtualMachineImpl;

/**@brief This class implements the MPAL (MeaProcess Automation Language) virtual machine.*/
class MPAL_VM_API VirtualMachine
{
    friend class Debugger;

    public:

    /** @brief Constructor*/
    VirtualMachine(Pt::uint32_t memorySize = (1024*1024*2));

        /** @brief Destructor*/
        virtual ~VirtualMachine();
    
        /** @brief Sets the memory size in byte used by the vm.
        *   @param sizeInByte The memory size in byte.*/
        void setVmMemory(Pt::uint32_t sizeInByte);

        /**@brief Load a unit which contain a program and its dependency.
        *  @param ist The stream which contain the unit.*/
        ProgramInfo& load(std::istream& ist);

        /**@brief Execute a loaded program
        *  @param progInfo The program to execute.*/
        void execute(ProgramInfo& progInfo, bool debug = false, bool runInLoop = false);

        /**@brief Unload a program
        *  @param progInfo The program to unload.*/
        void unload(ProgramInfo& progInfo);

        /**@brief Unload all loaded units.*/
        void clear();

protected:
    //Debugging
    void clearBreakPoints();
    void insertBreakPoint(const BreakPoint& bp);
    void removeBreakPoint(const BreakPoint& bp);
    void continueExecution();
    void stepOver();
    void stepInto();
    void terminate();
    std::vector<Pt::uint8_t> readMemory(size_t address, Pt::uint32_t size, const std::string& func);
    std::vector<Pt::uint8_t> readMemoryByOffset(Pt::uint32_t offset, Pt::uint32_t size,const std::string& func);
    std::string getCallStack();

protected:
    //Debugger events
    Pt::Signal<BreakPoint> onBreakPoint;
    Pt::Signal<int, std::string> onLine;
    Pt::Signal<>           onTerminate;
    Pt::Signal<std::string> onMessage;

private:
    VirtualMachineImpl* _impl;
};

}}

#endif

