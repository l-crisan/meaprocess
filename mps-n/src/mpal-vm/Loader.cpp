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
#include "Loader.h"
#include "Parameter.h"
#include "Function.h"
#include "Unit.h"
#include "Instruction.h"
#include <sstream>
#include <Pt/SourceInfo.h>
#include "InstructionTable.h"
#include "LineInfo.h"
#include "CallInst.h"
#include <Pt/Byteorder.h>
#include <stdexcept>

namespace mpal{
namespace vm{

Loader::Loader(void)
{
}

Loader::~Loader(void)
{
    clear();
}

void Loader::clear()
{
    _tempOperands.clear();
    _callInst.clear();
}

std::string Loader::readString(std::istream& ist)
{
    std::string str;
    char ch;
    while(true)
    {
        ist.read(&ch,1);
        
        if( ch == '\0')
            break;

        str +=  ch;
    }
    return str;
}

void Loader::loadDependencies(const std::vector<std::string>& dependencies)
{

}

Unit* Loader::loadUnit(std::istream& ist)
{
    clear();
    Pt::uint64_t version;
    std::string  name;
    
    ist.read((char*)&version,sizeof(version));
    version = Pt::leToHost(version);

    if( version != 3)
        throw  std::logic_error(std::string("error R1004: Wrong MPAL runtime version"));

    name = readString(ist);

    Unit* unit = new Unit(name.c_str(), version);
    
    //User unit version
    Pt::uint32_t* userVersion = unit->userVersion();

    ist.read((char*) &userVersion[0], sizeof(Pt::uint32_t) );
    ist.read((char*) &userVersion[1], sizeof(Pt::uint32_t) );
    ist.read((char*) &userVersion[2], sizeof(Pt::uint32_t) );
    
    userVersion[0] = Pt::leToHost(userVersion[0]);
    userVersion[1] = Pt::leToHost(userVersion[1]);
    userVersion[2] = Pt::leToHost(userVersion[2]);

    //Check machine type
    if( userVersion[0] == 0)
        userVersion[0] = 4; //4 bytes word size

    if( userVersion[0] != sizeof(void*))
        throw  std::logic_error(std::string("error R1004: Wrong MPAL runtime version"));

    //Description
    readString(ist);

    //Load type declarations.
    Pt::uint32_t typeDefCount;
    ist.read((char*)&typeDefCount, sizeof(typeDefCount));
    
    typeDefCount = Pt::leToHost(typeDefCount);
    
    for( Pt::uint32_t i = 0; i < typeDefCount; ++i)
        loadTypeDeclaration(ist, unit);

    //Load function declarations and code.
    Pt::uint32_t noOfFunctions;	
    ist.read((char*)&noOfFunctions, sizeof(noOfFunctions));

    noOfFunctions = Pt::leToHost(noOfFunctions);

    for( Pt::uint32_t i = 0; i< noOfFunctions; ++i )
        loadFunction(ist, unit);
    
    //Load default values.
    loadDefaultValues(unit);

    //Initialise the call instructions.
    for( Pt::uint32_t i = 0; i < _callInst.size(); ++i )
    {
        std::pair<std::string,CallInst*>& pair = _callInst[i];
        
        std::map<std::string, Function*>& functions = unit->functions();
        CallInst* inst = pair.second;

        std::map<std::string, Function*>::iterator funcIt = functions.find(pair.first);
        
        if( funcIt == functions.end())
        {
            int errLine = static_cast<int>(inst->uid() >> 32);
            std::stringstream ss;
            ss<<" Function '"<<pair.first;
            ss<<"' line "<<errLine; 
            ss<<" : error R1004: Function '"<< pair.first<<"' not found";
            throw std::runtime_error(ss.str());
        }

        inst->setInstructionList(funcIt->second);
        inst->setStackSize(funcIt->second->stackSize());
    }

    return unit;
}

void Loader::loadDefaultValues( Unit* unit )
{
    //Types
    std::map<std::string,Parameter*>::iterator it = unit->types().begin();
    
    for( ; it !=  unit->types().end(); ++it)
        loadParamDefaultValue(it->second, unit);

    //Variables
    std::map<std::string, Function*>::iterator funcIt = unit->functions().begin();

    for( ; funcIt != unit->functions().end(); ++funcIt )
    {
        Function* func = funcIt->second;

        for( Pt::uint32_t i = 0; i < func->parameter().size(); ++i )
        {
            Parameter* param = func->parameter()[i];
            loadParamDefaultValue(param, unit);
        }
    }
}

void Loader::loadFunction( std::istream& ist, Unit* unit )
{
    std::string  name;
    Pt::uint8_t  type;
    Pt::uint32_t number;

    name = readString(ist);
    ist.read((char*)&type,sizeof(type));
    
    Pt::uint32_t stackSize;
    ist.read((char*)&stackSize, sizeof(stackSize));
    stackSize = Pt::leToHost(stackSize);
    
    Pt::uint32_t fbSize;
    ist.read((char*)&fbSize, sizeof(fbSize));
    fbSize = Pt::leToHost(fbSize);
    Pt::int32_t lineBegin = 0;
    Pt::int32_t lineEnd = 0;

    if( unit->version() != 1)
    {
        ist.read((char*)&lineBegin, sizeof(lineBegin));
        ist.read((char*)&lineEnd, sizeof(lineEnd));
    }

    Function* func = new Function(name.c_str(), (Function::Type) type, stackSize, fbSize, lineBegin, lineEnd);

    //Load Parameter
    ist.read((char*)&number,sizeof(number));
    number = Pt::leToHost(number);

    for( Pt::uint32_t i = 0; i < number; ++i)
    {
        Parameter* param = loadParameter(ist);
        std::vector<Parameter*>& params = func->parameter();
        params.push_back(param);
    }

    //Load instructions
    _tempOperands.clear();

    ist.read((char*)&number, sizeof(number));
    number = Pt::leToHost(number);

    for( Pt::uint32_t i = 0; i < number; ++i)
    {
       Instruction* inst = loadInstruction(ist, unit);
       std::vector<Instruction*>&  instructions = func->instructions();
       instructions.push_back(inst);
    }
    
    std::map<std::string, Function*>& functions = unit->functions();

    std::stringstream ss;
    ss<<name;
    std::string fname;

    if( unit->version() == 3)
    {
            std::getline(ss, fname,'.');
            std::getline(ss, fname,'.');
    }
    else
    {
            fname = name;
    }

    std::pair<std::string, Function*> pr(fname, func);
    functions.insert(pr);	
    _tempOperands.clear();
}

Pt::SmartPtr<Operand> Loader::loadOperand( std::istream& ist, Unit* unit, bool isResultOp )
{
    Pt::SmartPtr<Operand> result;
    Pt::uint8_t  type;
    Pt::uint64_t op;

    ist.read( (char*)&type, sizeof(type) );
    
    if( isResultOp )
    {
        Pt::uint32_t resultOp;
        ist.read((char*)&resultOp,sizeof(resultOp));
        op = resultOp;
    }
    else
    {
        ist.read((char*)&op,sizeof(op));
    }

    if( type == Operand::Temporary  || type == Operand::TemporaryRef )
    {
        std::map<Pt::uint32_t,Pt::SmartPtr<Operand> >::iterator it = _tempOperands.find(static_cast<Pt::uint32_t>(op));

        if( it != _tempOperands.end())
        {
            result = it->second;
        }
        else
        {
            Pt::uint32_t offset = Pt::leToHost(static_cast<Pt::uint32_t>(op));
            result.reset(new Operand(offset, (Operand::Type) type));
            std::pair<Pt::uint32_t,Pt::SmartPtr<Operand> > pr(static_cast<Pt::uint32_t>(op),result);
            _tempOperands.insert(pr);
        }
    }
    else
    {
        if(type == Operand::Immediate)
            result.reset(new Operand(op));
        else
        {
            Pt::uint32_t offset = Pt::leToHost(static_cast<Pt::uint32_t>(op));
            result.reset(new Operand(offset, (Operand::Type) type));
        }
    }
    
    return result;
}

Instruction* Loader::loadInstruction(std::istream& ist, Unit* unit)
{
    Pt::uint32_t opCode;
    Pt::uint64_t uid;
    
    ist.read((char*)&opCode, sizeof(opCode));
    opCode = Pt::leToHost(opCode);
    ist.read((char*)&uid, sizeof(uid));
    uid = Pt::leToHost(uid);

    Instruction* inst = 0;

    if( opCode != Call)
    {
        InstructionTable& instTable = InstructionTable::instance();

        Pt::SmartPtr<Operand> result = loadOperand(ist, unit, true);
        Pt::SmartPtr<Operand> op1    = loadOperand(ist, unit);
        Pt::SmartPtr<Operand> op2    = loadOperand(ist, unit);

        inst = instTable.createInstruction((InstructionCode) opCode);

        if( inst == 0)
            throw std::runtime_error( LineInfo::lineInfo(uid) + ": error R1002: Unknow instruction");

        inst->setResult(result);
        inst->setOp1(op1);
        inst->setOp2(op2);
        inst->setUid(uid);
    }
    else
    {
        std::string call = readString(ist);
        inst = new CallInst();
        inst->setUid(uid);
        _callInst.push_back(std::pair<std::string,CallInst*>(call,(CallInst*)inst));
    }

    return inst;
}

void Loader::loadParamDefaultValue(Parameter* parameter, Unit* unit)
{
    if( parameter->type() == STRUCT)
    {
        for( Pt::uint32_t i = 0; i < parameter->structured().size(); ++i)
            loadParamDefaultValue(parameter->structured()[i], unit);
    }
    
    if( parameter->defValueBuffer().size() == 0)
    {
        if(parameter->type() == ARRAY)
        {
            Parameter* arrayType = parameter->structured()[0];
            loadParamDefaultValue(arrayType, unit);
        }
        return;
    }

    std::stringstream ss;
    ss.write((char*) &parameter->defValueBuffer()[0], parameter->defValueBuffer().size());

    Pt::Any value;
    switch(parameter->type())
    {
        case STRUCT:
            value = loadStructDefaultValue(parameter, ss, unit);
        break;
        
        case ARRAY:
            value = loadArrayDefaultValue(parameter, ss, unit);
        break;
        
        case UDT:
        case FB:
            value = loadUDTDefaultValue(parameter, ss, unit);
        break;
        
        default:
            value = loadDefaultValue(parameter->type(),ss);
        break;
    }
    parameter->setDefaultValue(value);
}

Pt::Any Loader::loadDefaultValue(Pt::uint8_t dataType, std::istream& ist)
{
    Pt::Any any;

    switch(dataType)
    {
        case BOOL:
        {
            Pt::uint8_t value;
            ist.read((char*)&value, 1);			
            any = value;
        }
        break; 
        case SINT:
        {
            Pt::int8_t value;
            ist.read((char*)&value,sizeof(value));
            any = value;
        }
        break;
        case INT:
        {
            Pt::int16_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case DINT:
        case ENUM:
        {
            Pt::int32_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case LINT:
        {
            Pt::int64_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case USINT:
        {
            Pt::uint8_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case UINT:
        {
            Pt::uint16_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case UDINT:
        {
            Pt::uint32_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case ULINT:
        {
            Pt::uint64_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case REAL:
        {
            float value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case LREAL:
        {
            double value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case STRING:
        {
            any = readString(ist);
        }
        break;
        case BYTE:
        {
            Pt::uint8_t value;
            ist.read((char*)&value,sizeof(value));
            any = value;
        }
        break;
        case WORD:
        {
            Pt::uint16_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case DWORD:
        {
            Pt::uint32_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case LWORD:
        {
            Pt::uint64_t value;
            ist.read((char*)&value,sizeof(value));
            any = Pt::leToHost(value);
        }
        break;
        case WSTRING:
        break;
        case STRUCT:
        break;
        case ARRAY:
        break;
        case UDT:
        break;
    }

    return any;
}

Parameter* Loader::findUDT(const char* name, Unit* unit)
{
    std::map<std::string,Parameter*>& types = unit->types();
    std::map<std::string,Parameter*>::iterator typeIt = types.find(name);

    if(typeIt != types.end())
        return typeIt->second;
    return 0;
}

Function* Loader::findFunction(const char* name, Unit* unit)
{
    std::map<std::string, Function*>::iterator fIt = unit->functions().begin();

    for(; fIt !=  unit->functions().end(); ++fIt)
    {
        if(fIt->first == name)
            return fIt->second;
    }

    return 0;
}

Parameter* Loader::loadParameter(std::istream& ist)
{
    std::string   name;
    Pt::uint64_t  uid;
    Pt::uint8_t	  access;
    Pt::uint8_t	  dataType;	
    std::string   typeId;
    
    //name
    name = readString(ist);
    //uid
    ist.read((char*)&uid, sizeof(uid));
    uid = Pt::leToHost(uid);
    //access
    ist.read((char*)&access,1);
    //typeid	
    typeId = readString(ist);
    //size
    Pt::uint32_t size;
    ist.read((char*)&size,sizeof(size));
    size = Pt::leToHost(size);
    //udt name
    std::string udtName = readString(ist);
    //offset
    Pt::uint32_t offset;
    ist.read((char*)&offset,sizeof(offset));
    offset = Pt::leToHost(offset);
    //FB offset
    Pt::uint32_t fbOffset;
    ist.read((char*)&fbOffset,sizeof(fbOffset));
    fbOffset = Pt::leToHost(fbOffset);

    //data type
    ist.read((char*)&dataType,1);
    
    Parameter*  parameter = new Parameter( name.c_str(), (DataType) dataType, uid, typeId.c_str(), (Access)access, offset, size);
    parameter->setUDTName( udtName.c_str());

    switch(dataType)
    {	
        case STRUCT:
        {
            Pt::uint32_t noOfParams;
            ist.read((char*)&noOfParams, sizeof(noOfParams));
            noOfParams = Pt::leToHost(noOfParams);
            
            for(Pt::uint32_t i = 0; i < noOfParams; ++i)
            {
                Parameter* structParam = loadParameter(ist);
                parameter->structured().push_back(structParam);
            }
        }
        break;
    
        case ARRAY:
        {
            Parameter* arrayParamType = loadParameter(ist);
            parameter->structured().push_back(arrayParamType);
            Pt::uint32_t noOfDim;

            ist.read((char*)&noOfDim, sizeof(noOfDim));
            noOfDim = Pt::leToHost(noOfDim);
            Dimensions& dim = parameter->dimensions();

            for(Pt::uint32_t i = 0; i < noOfDim; ++i)
            {
                Pt::int64_t  from;
                Pt::int64_t  to;
                ist.read((char*)&from,sizeof(from));
                from = Pt::leToHost(from);
                ist.read((char*)&to,sizeof(to));
                to = Pt::leToHost(to);
                dim.push_back( std::pair<Pt::int64_t,Pt::int64_t>(from,to));
            }
        }
        break;
        case ENUM:
            Pt::uint32_t noOfEnums;
            ist.read((char*)&noOfEnums, sizeof(noOfEnums));
            noOfEnums = Pt::leToHost(noOfEnums);
            
            std::vector<std::string>& enumList = parameter->enumList();

            for(Pt::uint32_t i = 0; i < noOfEnums; ++i)
            {
                std::string item = readString(ist);
                enumList.push_back(item);
            }
        break;
    }

    //Read the default value stream into a buffer => evaluet it later.
    Pt::uint32_t lenght;
    ist.read((char*)&lenght, sizeof(lenght));
    lenght = Pt::leToHost(lenght);
    
    if( lenght != 0)
    {
        std::vector<Pt::uint8_t>& buffer = parameter->defValueBuffer();
        buffer.resize(lenght);
        ist.read((char*)&buffer[0], lenght);
    }

    return parameter;
}

Pt::Any Loader::loadStructDefaultValue(Parameter* parameter, std::istream& ist, Unit* unit, Function* f)
{
    Pt::Any any;
    Pt::Any value;
    std::pair<Pt::uint32_t,Pt::Any> pairValue;
    std::vector<Pt::Any> resList;
    std::pair<Pt::uint32_t,Pt::Any> newEntry;

    //Number of members.
    Pt::uint32_t count;
    ist.read((char*)&count, sizeof(count));
    count = Pt::leToHost(count);
    
    if( count == 0)
        return any;

    for(Pt::uint32_t i = 0; i < count; ++i)
    {
        //Read the member index.
        Pt::int32_t index;
        ist.read((char*)&index, sizeof(index));
        index = Pt::leToHost(index);
        pairValue.first = index;

        Parameter* memberParam = 0;

        if( f == 0)
            memberParam = parameter->structured()[index];
        else
            memberParam = f->parameter()[index];
        
        switch(memberParam->type())
        {
            case ARRAY:
                value = loadArrayDefaultValue(memberParam, ist, unit);
            break;

            case STRUCT:
                value = loadStructDefaultValue(memberParam, ist, unit);
            break;

            case UDT:
            case FB:
                value = loadUDTDefaultValue(memberParam, ist, unit);
            break;

            default:
                value = loadDefaultValue(memberParam->type(),ist);
            break;
        }

        //Make a index/value pair
        pairValue.second = value;
        resList.push_back(pairValue);
    }
    
    any = resList;

    return any;
}

Pt::Any Loader::loadUDTDefaultValue(Parameter* parameter, std::istream& ist, Unit* unit)
{
    std::string udtName = parameter->UDTName();
    Parameter* udtParam = findUDT(udtName.c_str(), unit);
    
    if( udtParam == 0)
    {
        Function* f = findFunction(udtName.c_str(), unit);
        return loadStructDefaultValue(0, ist, unit, f);
    }

    switch(udtParam->type())
    {
        case ARRAY:
            return loadArrayDefaultValue(udtParam, ist, unit);
        
        case STRUCT:
            return loadStructDefaultValue(udtParam, ist, unit);
        
        case UDT:
        case FB:
            return loadUDTDefaultValue(udtParam, ist, unit);
        
        default:
            return loadDefaultValue(udtParam->type(), ist);
    }
}

Pt::Any Loader::loadArrayDefaultValue(Parameter* parameter, std::istream& ist, Unit* unit)
{
    Pt::uint32_t count;
    ist.read((char*)&count, sizeof(count));
    count = Pt::leToHost(count);

    if(count == 0)
        return Pt::Any();

    Pt::Any defaultValue;
    std::vector<Pt::Any> values;

    Parameter* arrayType = parameter->structured()[0];

    for (Pt::uint32_t i = 0; i < count; ++i)
    {      
        switch (arrayType->type())
        {
            case UDT:
            case FB:
                defaultValue = loadUDTDefaultValue(arrayType, ist, unit);	
            break;
            
            case STRUCT:
                defaultValue = loadStructDefaultValue(arrayType, ist, unit);	
            break;

            default:
                defaultValue = loadDefaultValue(arrayType->type(), ist);
            break;
        }
        values.push_back(defaultValue);
    }

    return Pt::Any(values);
}

void Loader::loadTypeDeclaration(std::istream& ist, Unit* unit)
{
    Parameter* parameter = loadParameter(ist);

    std::map<std::string,Parameter*>& types = unit->types();
    std::pair<std::string,Parameter*> pr(parameter->name(), parameter);
    types.insert(pr);
}

}}

