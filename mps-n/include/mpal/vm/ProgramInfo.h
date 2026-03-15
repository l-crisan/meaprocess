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

#ifndef MPAL_VM_PROGRAMINFO_H
#define MPAL_VM_PROGRAMINFO_H

#include <mpal/vm/Variable.h>

namespace mpal{
namespace vm{

/**@brief This class represents a loaded unit.*/
class MPAL_VM_API ProgramInfo
{
    public:	
        /** @brief Destructor */
        virtual ~ProgramInfo()
        {};

        /** @brief Return true if the unit has a program.
        *   @return True if the unit can be executed.*/
        virtual bool isExecutable() const = 0;

        /**@brief Gets the unit name.
        *  @return The unit name */
        virtual std::string name()  const = 0;

        /**@brief Init the executable program in this unit.*/
        virtual void init() = 0;

        /**@brief Init the input part of the executable program in this unit.*/
        virtual void initInput()  = 0;

        /**@brief Gets the program variables.
        *  @return The program variables.*/
        virtual const std::vector<Variable*>& variables() const = 0;

        /**@brief Gets a program variable by the path.
        *  @param path A index path that identify a variable.
        *  @return The program variable.*/
        virtual Variable* getVariable(const std::vector<Pt::uint32_t>& path) = 0;

        /**@brief Sets a program variable value by the path.
        *  @param path A index path that identify a variable.
        *  @param value The value of the variable.*/
        virtual void setVariableValue(const std::vector<Pt::uint32_t>& path, const void* value) = 0;

        /**@brief Gets the memory associated with a program variable.
        *  @param path A index path that identify a variable.
        *  @return The memory associated with a program variable.*/
        virtual void* variableValue(const std::vector<Pt::uint32_t>& path) = 0;

    protected:
        ProgramInfo()
        {};
};

}}
#endif

