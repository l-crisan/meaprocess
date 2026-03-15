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
#ifndef MPAL_VM_LOADER_H
#define MPAL_VM_LOADER_H

#include <string>
#include <map>
#include <istream>
#include <Pt/SmartPtr.h>
#include <Pt/Any.h>
#include <vector>
#include "Operand.h"

namespace mpal{
namespace vm{

class Instruction;
class Unit;
class Parameter;
class Function;
class CallInst;

class Loader
{
public:
    Loader(void);

    virtual ~Loader(void);

    Unit* loadUnit(std::istream& ist);
private:
    void clear();

    void loadTypeDeclaration(std::istream& ist, Unit* unit);

    void loadFunction(std::istream& ist, Unit* unit);

    void loadDependencies(const std::vector<std::string>& dependencies);

    std::string readString(std::istream& ist);

    Parameter* loadParameter(std::istream& ist);

    Parameter* findUDT(const char* name, Unit* unit);

    Function* findFunction(const char* name, Unit* unit);

    void loadDefaultValues(Unit* unit);

    void loadParamDefaultValue(Parameter* parameter, Unit* unit);

    Pt::Any loadDefaultValue(Pt::uint8_t dataType, std::istream& ist);

    Pt::Any loadUDTDefaultValue(Parameter* parameter, std::istream& ist, Unit* unit);

    Pt::Any loadStructDefaultValue(Parameter* parameter, std::istream& ist, Unit* unit, Function* f = 0);

    Pt::Any loadArrayDefaultValue(Parameter* parameter, std::istream& ist, Unit* unit);

    Instruction* loadInstruction(std::istream& ist, Unit* unit);

    Pt::SmartPtr<Operand> loadOperand(std::istream& ist, Unit* unit, bool isResultOp = false);

private:
    std::map<Pt::uint32_t,Pt::SmartPtr<Operand> >  _tempOperands;

    std::vector<std::pair<std::string,CallInst*> > _callInst;
};

}}

#endif

