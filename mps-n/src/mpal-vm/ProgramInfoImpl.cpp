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
#include "ProgramInfoImpl.h"
#include "Unit.h"
#include "Loader.h"
#include "Function.h"
#include "Parameter.h"
#include <sstream>

namespace mpal{
namespace vm{

ProgramInfoImpl::ProgramInfoImpl(Unit* unit)
: _unit(unit)
, _main(0)
{
     std::map<std::string, Function*>&  functions = _unit->functions();
     std::map<std::string, Function*>::iterator it = functions.begin();
     
     Function* func = 0;

     for( ; it != functions.end(); ++it)
     {
        func = it->second;
        if( func->type() == Function::PG )
        {
            _main = func;
            break;
        }
     }

    if(_main != 0)
        initStacks();
}

ProgramInfoImpl::~ProgramInfoImpl()
{
    for( Pt::uint32_t i = 0; i < _variables.size(); ++i)
        delete _variables[i];

    delete _unit;
}

Pt::uint32_t ProgramInfoImpl::stackSize() const
{
    std::map<std::string, Function*>::iterator it = _unit->functions().begin();
    Function* func = it->second;
    return func->stackSize();
}

void ProgramInfoImpl::init()
{
    if(_main == 0)
        return;

    initInput();
    initOutputStack(variables());
}

void ProgramInfoImpl::initInput()
{
    initInputStack(variables());
}

void ProgramInfoImpl::initStacks() 
{
    if(_main->stackSize() == 0)
        return;

    //Init the call stack.
    _inStack.resize( _main->stackSize(), 0 );
    
    //Init the input stack.
    initInputStack(variables());

    //Calc output stack size.
    Pt::uint32_t outputStackSize = 0;
    for( Pt::uint32_t j = 0; j < variables().size(); ++j)
    {
        Variable* var = variables()[j];
        
        if( !isStackParameter(var->access()))
            outputStackSize += var->size();
    }

    //Init the output stack.
    if( outputStackSize != 0)
    {
        _outStack.resize(outputStackSize, 0);
        initOutputStack(variables());
    }
}

void ProgramInfoImpl::copyDefValue(Pt::uint8_t* dest, mpal::vm::DataType type, Pt::Any anyValue)
{
    if(anyValue.empty())
        return;

    switch(type)
    {
        case mpal::vm::BOOL:
        case mpal::vm::BYTE:
        case mpal::vm::USINT:
        {
            Pt::uint8_t value = Pt::any_cast<Pt::uint8_t>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;
        case mpal::vm::UINT:
        case mpal::vm::WORD:
        {
            Pt::uint16_t value = Pt::any_cast<Pt::uint16_t>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;

        case mpal::vm::UDINT:
        case mpal::vm::DWORD:
        {
            Pt::uint32_t value = Pt::any_cast<Pt::uint32_t>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;

        case mpal::vm::ULINT:
        case mpal::vm::LWORD:
        {
            Pt::uint64_t value = Pt::any_cast<Pt::uint64_t>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;

        case mpal::vm::SINT:
        {
            Pt::int8_t value = Pt::any_cast<Pt::int8_t>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;

        case mpal::vm::INT:
        {
            Pt::int16_t value = Pt::any_cast<Pt::int16_t>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;

        case mpal::vm::DINT:
        case mpal::vm::ENUM:
        {
            Pt::int32_t value = Pt::any_cast<Pt::int32_t>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;
        case mpal::vm::LINT:
        {
            Pt::int64_t value = Pt::any_cast<Pt::int64_t>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;
                
        case mpal::vm::REAL:
        {
            float value = Pt::any_cast<float>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;
        
        case mpal::vm::LREAL:
        {
            double value = Pt::any_cast<double>(anyValue);
            memcpy(dest,&value,sizeof(value));
        }
        break;

        case mpal::vm::STRING:
        {
            std::string value = Pt::any_cast<std::string>(anyValue);
            memcpy(dest, &value[0], value.length() - 1);
        }
        break;

        case mpal::vm::WSTRING:
        break;

        default:
        break;
    }
}

bool ProgramInfoImpl::isStackParameter(mpal::vm::Access access)
{
    return ((access == mpal::vm::Input) || 
            (access == mpal::vm::VarTemp) ||
            (access == mpal::vm::VarTempConst) ||
            (access == mpal::vm::VarConst));
}

void ProgramInfoImpl::initInputStack(const std::vector<Variable*>& variables) 
{
    for( Pt::uint32_t i = 0; i < variables.size(); ++i)
    {
        mpal::vm::Variable* variable = variables[i];
        mpal::vm::Access access = variable->access();
        
        if( !isStackParameter(access) )
             continue;

        Pt::uint8_t* newPos = &_inStack[variable->offset()];

        switch( variable->type())
        {
            case mpal::vm::STRUCT:
                initStructMember( newPos, variable->variables(), variable->defaultValue());
            break;

            case mpal::vm::UDT:
                initUDT(newPos, variable->variables(), variable->defaultValue());
            break;

            case mpal::vm::FB:
                initFB(newPos, variable, variable->defaultValue());
            break;

            case mpal::vm::ARRAY:
                initArray(newPos, variable, variable->defaultValue());
            break;

            default:
                copyDefValue(newPos, variable->type(), variable->defaultValue());
            break;
        }
    }
}

void ProgramInfoImpl::initUDT(Pt::uint8_t* stack, const std::vector<Variable*>& variables, Pt::Any value) 
{
    for(Pt::uint32_t i = 0; i < variables.size(); ++i)
    {
        Variable* udtVar = variables[i];
        Pt::uint8_t* newPos = (stack + udtVar->offset());

        switch(udtVar->type())
        {
            case mpal::vm::STRUCT:
                initStructMember(newPos, udtVar->variables(), value);
            break;

            case mpal::vm::UDT:
                initUDT(newPos, udtVar->variables(), value);
            break;

            case mpal::vm::FB:
                initFB(newPos, udtVar, value);
            break;

            case mpal::vm::ARRAY:
                initArray(newPos, udtVar, value);
            break;
            
            default:
                copyDefValue(newPos, udtVar->type(), value);
            break;
        }
    }
}

bool ProgramInfoImpl::getFBMemberValue(int index, std::vector<Pt::Any>& data, Pt::Any& output)
{
    for( Pt::uint32_t i = 0; i < data.size(); ++i)
    {
        std::pair<Pt::uint32_t,Pt::Any> val = Pt::any_cast<std::pair<Pt::uint32_t,Pt::Any> >(data[i]);
        if( val.first == static_cast<Pt::uint32_t>(index))
        {
            output = val.second;
            return true;
        }
    }

    return false;
}

void ProgramInfoImpl::initFB(Pt::uint8_t* stack, Variable* variable, Pt::Any value) 
{
    Pt::uint8_t* newPos;
    const std::vector<Variable*>& variables = variable->variables();

    //Init with the implace values.
    for( Pt::uint32_t i = 0; i < variables.size(); ++i)
    {
        mpal::vm::Variable* variable = variables[i];

        if( variable->access() == VarTemp ||  variable->access() == mpal::vm::VarTempConst)
            continue;

        newPos = stack;
        newPos += variable->offset();

        switch( variable->type())
        {
            case mpal::vm::STRUCT:
                initStructMember(newPos, variable->variables(), variable->defaultValue());
            break;
            
            case mpal::vm::UDT:
                initUDT(newPos, variable->variables(), variable->defaultValue());
            break;

            case mpal::vm::FB:
                initFB(newPos, variable, variable->defaultValue());
            break;

            case mpal::vm::ARRAY:
                initArray(newPos, variable, variable->defaultValue());
            break;

            default:
                copyDefValue(newPos, variable->type(), variable->defaultValue());
            break;
        }
    }

    //Second init the outplace values
    if(value.empty())
        return;

    std::vector<Pt::Any> data = Pt::any_cast<std::vector<Pt::Any> >(value);

    for(Pt::uint32_t i = 0; i < variables.size(); ++i)
    {
        Variable* udtVar = variables[i];
        Pt::uint8_t* newPos = (stack + udtVar->offset());

        Pt::Any val;

        if( udtVar->access() == VarTemp || udtVar->access() == VarTempConst)
            continue;

        if(!getFBMemberValue((int) i, data, val))
            continue;

        switch(udtVar->type())
        {
            case mpal::vm::STRUCT:
                initStructMember(newPos, udtVar->variables(), val);
            break;

            case mpal::vm::UDT:
                initUDT(newPos, udtVar->variables(), val);
            break;

            case mpal::vm::FB:
                initFB(newPos, udtVar, val);
            break;

            case mpal::vm::ARRAY:
                initArray(newPos, udtVar, val);
            break;
            
            default:
                copyDefValue(newPos, udtVar->type(), val);
            break;
        }
    }
}

void ProgramInfoImpl::initStructMember(Pt::uint8_t* stackPos, const std::vector<Variable*>& variables, Pt::Any value) 
{
    Pt::uint8_t* newPos;
    //Init with the implace values.
    for( Pt::uint32_t i = 0; i < variables.size(); ++i)
    {
        mpal::vm::Variable* variable = variables[i];
        newPos = stackPos;
        newPos += variable->offset();

        switch( variable->type())
        {
            case mpal::vm::STRUCT:
                initStructMember(newPos, variable->variables(), variable->defaultValue());
            break;
            
            case mpal::vm::UDT:
                initUDT(newPos, variable->variables(), variable->defaultValue());
            break;

            case mpal::vm::FB:
                initFB(newPos, variable, variable->defaultValue());
            break;

            case mpal::vm::ARRAY:
                initArray(newPos, variable, variable->defaultValue());
            break;

            default:
                copyDefValue(newPos, variable->type(), variable->defaultValue());
            break;
        }
    }

    if( value.empty())
        return;

    //Init with the outplace values. 
    std::vector<Pt::Any> memberList = Pt::any_cast<std::vector<Pt::Any> >(value);
    for( Pt::uint32_t i = 0; i < memberList.size(); ++i)
    {
        std::pair<Pt::uint32_t,Pt::Any> member = Pt::any_cast<std::pair<Pt::uint32_t,Pt::Any> >(memberList[i]);

        mpal::vm::Variable* variable = variables[member.first];

        newPos = stackPos;
        newPos += variable->offset();

        switch( variable->type())
        {
            case mpal::vm::STRUCT:			
                initStructMember(newPos, variable->variables(), member.second);
            break;

            case mpal::vm::UDT:
                initUDT(newPos, variable->variables(), member.second);
            break;

            case mpal::vm::FB:
                initFB(newPos, variable, member.second);
            break;

            case mpal::vm::ARRAY:
                initArray(newPos, variable, member.second);
            break;

            default:
                copyDefValue(newPos, variable->type(), member.second);
            break;
        }
    }
}

void ProgramInfoImpl::initOutputStack(const std::vector<Variable*>& variables) 
{
    Pt::uint32_t curPos = 0;
    for( Pt::uint32_t i = 0; i < variables.size(); ++i)
    {
        mpal::vm::Variable* variable = variables[i];
        mpal::vm::Access access = variable->access();

        if( isStackParameter(access) )
            continue;

        Pt::uint8_t* newPos = &_outStack[curPos];

        switch( variable->type())
        {
            case mpal::vm::STRUCT:
                initStructMember(newPos, variable->variables(),variable->defaultValue());
            break;

            case mpal::vm::UDT:
                initUDT(newPos, variable->variables(), variable->defaultValue());
            break;

            case mpal::vm::FB:
                initFB(newPos, variable, variable->defaultValue());
            break;

            case mpal::vm::ARRAY:
                initArray(newPos, variable, variable->defaultValue());
            break;

            default:
                copyDefValue(newPos, variable->type(), variable->defaultValue());
            break;
        }

        size_t addr = (size_t) &_outStack[curPos];
        memcpy(&_inStack[variable->offset()], &addr, sizeof(size_t));

        curPos += variable->size();
    }
}

void ProgramInfoImpl::initArray(Pt::uint8_t* stack, Variable* variable, Pt::Any defValue) 
{
   Variable* arrayType = variable->variables()[0];

    if( defValue.empty() )
    {
        Pt::uint8_t* newPos = stack;
        Pt::uint32_t sizeOfElement = arrayType->size();

        for( Pt::uint32_t i = 0; i < variable->size(); i += sizeOfElement)
        {
            switch(arrayType->type())
            {
                case mpal::vm::STRUCT:			
                    initStructMember(newPos, arrayType->variables(), defValue);
                break;

                case mpal::vm::UDT:
                    initUDT(newPos, arrayType->variables(), defValue);
                break;

                case mpal::vm::FB:
                    initFB(newPos, arrayType, defValue);
                break;
                default:
                break;
            }

            newPos += sizeOfElement;
        }
        return;
    }

    std::vector<Pt::Any> values = Pt::any_cast<std::vector<Pt::Any> >(defValue);
    for( Pt::uint32_t i = 0; i < values.size(); ++i)
    {
        Pt::Any value = values[i];
        
        Pt::uint8_t* newPos = (stack + ( arrayType->size() * i));

        switch(arrayType->type())
        {
            case mpal::vm::STRUCT:			
                initStructMember(newPos, arrayType->variables(), value);
            break;

            case mpal::vm::UDT:
                initUDT(newPos, arrayType->variables(), value);
            break;

            case mpal::vm::FB:
                initFB(newPos, arrayType, value);
            break;

            default:
                copyDefValue(newPos, arrayType->type(), value);
            break;
        }
    }
}

void ProgramInfoImpl::setVariableValue(const std::vector<Pt::uint32_t>& path, const void* value)
{
    Variable* var = 0;
    Pt::uint8_t* pos = GetVariablePosition(path, &var);
    memcpy(pos, value, var->size());
}

Pt::uint8_t* ProgramInfoImpl::GetVariablePosition(const std::vector<Pt::uint32_t>& path, Variable** outvar)
{
    Variable* var = variables()[path[0]];

    Pt::uint8_t* pos;

    if( isStackParameter(var->access()))
        pos = &_inStack[var->offset()];
    else
        pos = (Pt::uint8_t*) *((size_t*) &_inStack[var->offset()]);

    Pt::uint32_t relOffset = 0;

    for( Pt::uint32_t i = 1; i < path.size(); ++i)
    {
        Pt::uint32_t index = path[i];
        var = var->variables()[index];
        relOffset += var->offset();
    }

    pos += relOffset;

    *outvar = var;

    return pos;
}

Variable* ProgramInfoImpl::getVariable(const std::vector<Pt::uint32_t>& path)
{
    Variable* var;
    GetVariablePosition(path, &var);
    return var;
}

void* ProgramInfoImpl::variableValue(const std::vector<Pt::uint32_t>& path)
{
    Variable* var = 0;
    return GetVariablePosition(path, &var);
}

bool ProgramInfoImpl::isExecutable() const
{
    return _main != 0;
}

std::string ProgramInfoImpl::name() const
{
    return _unit->name();
}

const std::vector<Variable*>& ProgramInfoImpl::variables() const
{
    if( _main == 0)
    {
        _variables.clear();
        return _variables;
    }

    if( _variables.size() != 0)
        return _variables;

     std::vector<Parameter*>& params = _main->parameter();

     for(Pt::uint32_t i = 0; i < params.size(); ++i)
        copyParams(params[i],_variables, params[i]->offset()); 

     return _variables;
}

void ProgramInfoImpl::copyParams(Parameter* param, std::vector<Variable*>& variables, Pt::uint32_t offset) const
{
    Variable* var = new Variable( param->type(),  param->access(), param->name().c_str(), offset, param->size(), param->defaultValue() );
    
    variables.push_back(var);

    switch( param->type())
    {
        case STRUCT:
        {
            for(Pt::uint32_t i = 0; i < param->structured().size(); ++i)
            {
                Parameter* parStruct = param->structured()[i];
                copyParams(parStruct, var->variables(), parStruct->offset());
            }
        }
        break;

        case ARRAY:
        {
            //OF type
            copyParams(param->structured()[0], var->variables(), param->offset());
            //Dimension
            var->dimensions() = param->dimensions();
        }
        break;
        
        case UDT:
        {
            std::string udtName = param->UDTName();
            std::map<std::string,Parameter*>& types = _unit->types();
            Parameter* udt = types[udtName];
            copyParams(udt, var->variables(), udt->offset());
        }
        break;
        
        case FB:
        {
            std::string fbName = param->UDTName();
            Function* func = searchFunction(fbName.c_str());
            if(func == 0)
            {
                std::string msg = "error R1005: Function block '";
                msg += fbName;
                msg += "not found";
                throw std::runtime_error(msg);
            }

            std::vector<Parameter*>& params =  func->parameter();

            for( Pt::uint32_t i = 0; i < params.size(); ++i)
            {
                Parameter* fbParam = params[i];				
                copyParams(fbParam, var->variables(), fbParam->offset());
            }
        }
        break;
        default:
        break;
    }
}

Function* ProgramInfoImpl::searchFunction(const char* fname) const
{
    std::string funcName = fname;
    std::map<std::string, Function*>& functions = _unit->functions();
    std::map<std::string, Function*>::iterator fit = functions.find(funcName);

    if( fit == functions.end())
        return 0; 

    return fit->second;
}

Unit* ProgramInfoImpl::unit() const
{ 
    return _unit; 
}

Function* ProgramInfoImpl::main() const
{
    return _main;
}

Pt::uint8_t* ProgramInfoImpl::inStack()
{ 
    if(_inStack.size() ==  0)
        return 0;

    return &_inStack[0];
}

}}

