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
#ifndef MPAL_VM_PARAMETER_H
#define MPAL_VM_PARAMETER_H

#include <vector>
#include <string>
#include <Pt/Types.h>
#include <Pt/Any.h>

#include <mpal/vm/VirtualMachine.h>

namespace mpal{
namespace vm{

class Parameter
{
public:
    Parameter( const char* name, DataType type, Pt::uint64_t uid, const char* typeId, Access access, Pt::uint32_t offset, Pt::uint32_t size = 0);

    ~Parameter(void);

    DataType type() const;
    
    Access access() const;

    void setAccess(Access ac);

    const std::string& name() const;

    std::vector<Parameter*>& structured();

    Dimensions& dimensions();

    std::string typeId() const;
    Pt::Any defaultValue() const;
    
    void setDefaultValue(Pt::Any& defValue);

    Pt::uint32_t size();

    void setSize(Pt::uint32_t size);
    
     const std::string& UDTName() const;

    void setUDTName(const char* name);

    Pt::uint32_t offset() const;	

    std::vector<Pt::uint8_t>& defValueBuffer() ;

    std::vector<std::string>& enumList();

private:
    DataType				 _type;
    Access					 _access;
    std::string				 _name;
    std::string				 _udtName;
    std::vector<Parameter*>	 _struct;
    Dimensions				 _dimensions;
    Pt::uint64_t			 _uid;
    std::string				 _typeId;
    Pt::Any					 _defaultValue;
    Pt::uint32_t			 _size;
    Pt::uint32_t			 _offset;	
    std::vector<Pt::uint8_t> _defValueBuffer;
    std::vector<std::string> _enumList;
};

}}

#endif

