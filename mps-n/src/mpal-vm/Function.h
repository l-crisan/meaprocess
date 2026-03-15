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
#ifndef MPS_MPAL_VM_FUNCTION_H
#define MPS_MPAL_VM_FUNCTION_H

#include <vector>
#include <string>
#include <Pt/Types.h>

namespace mpal{
namespace vm{

class Instruction;
class Parameter;

class Function
{
public:
    enum Type
    {
        FC = 0,
        FB,
        PG
    };

    Function(const char* name, Function::Type type, Pt::uint32_t stackSize, Pt::uint32_t fbSize, Pt::int32_t lineBegin, Pt::int32_t lineEnd);

    virtual ~Function(void);
    
    std::vector<Parameter*>& parameter();

    std::vector<Instruction*>& instructions();
    std::string name() const;
    
    Function::Type type() const;
    Pt::uint32_t stackSize() const;
    
    Pt::uint32_t fbSize() const;
    Pt::int32_t lineBegin() const;
    Pt::int32_t lineEnd() const;

private:
    Type			_type;
    Pt::uint32_t	_stackSize;
    Pt::uint32_t	_fbSize;
    std::string		_name;
    Pt::int32_t     _lineBegin;
    Pt::int32_t		_lineEnd;
    std::vector<Parameter*>	  _parameter;	
    std::vector<Instruction*> _instructions;
};

}}

#endif

