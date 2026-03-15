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
#ifndef MPAL_VM_PROGRAMINFOIMPL_H
#define MPAL_VM_PROGRAMINFOIMPL_H

#include <mpal/vm/VirtualMachine.h>

namespace mpal{
namespace vm{


class Unit;
class Loader;
class Parameter;
class Function;

class ProgramInfoImpl : public ProgramInfo
{
    public:
        ProgramInfoImpl(Unit* unit);

        virtual ~ProgramInfoImpl();

        bool isExecutable() const;
        
        void init() ;

        void initInput();

        std::string name() const;

        const std::vector<Variable*>& variables() const;

        void setVariableValue(const std::vector<Pt::uint32_t>& path, const void* value);
        
        void* variableValue(const std::vector<Pt::uint32_t>& path);

        Unit* unit() const;

        Function* main() const;

        Pt::uint8_t* inStack();

        virtual Variable* getVariable(const std::vector<Pt::uint32_t>& path);

    private:
        Pt::uint32_t stackSize() const;

        void copyParams(Parameter* param, std::vector<Variable*>& variables, Pt::uint32_t offset) const;

        void initStacks() ;

        void initInputStack(const std::vector<Variable*>& variables) ;

        void initOutputStack(const std::vector<Variable*>& variables) ;

        void initUDT(Pt::uint8_t* stackPos, const std::vector<Variable*>& variables, Pt::Any value) ;

        void initFB(Pt::uint8_t* stack, Variable* variable, Pt::Any value) ;

        void initStructMember(Pt::uint8_t* stackPos, const std::vector<Variable*>& variables, Pt::Any value) ;

        void initArray( Pt::uint8_t* stackPos, Variable* arrayType, Pt::Any defValue) ;

        void copyDefValue(Pt::uint8_t* dest, mpal::vm::DataType type, Pt::Any value);		

        Pt::uint8_t* GetVariablePosition(const std::vector<Pt::uint32_t>& path, Variable** outvar);

        Function* searchFunction(const char* fname) const;

        bool isStackParameter(mpal::vm::Access access);

private:
        static bool getFBMemberValue(int index, std::vector<Pt::Any>& data, Pt::Any& output);

private:
        Unit* _unit;
        Function* _main;
        mutable std::vector<Variable*> _variables;
        std::vector<Pt::uint8_t>       _inStack;
        std::vector<Pt::uint8_t>       _outStack;
};

}}

#endif

