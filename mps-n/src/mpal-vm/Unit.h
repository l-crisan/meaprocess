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
#ifndef MPS_MPAL_VM_UNIT_H
#define MPS_MPAL_VM_UNIT_H

#include <vector>
#include <map>
#include <string>
#include <Pt/Types.h>


namespace mpal{
namespace vm{

class Function;
class Parameter;

class Unit
{
public:
    enum Type
    {
        UnitType,
        ProgramType
    };

    Unit(const char* name,Pt::uint64_t version);

    ~Unit(void);

    inline std::map<std::string, Function*>& functions()
    {
        return _functions;
    }

    inline std::string name() const
    {
        return _name;
    }

    inline std::map<std::string,Parameter*>& types()
    {
        return _types;
    }

    inline Pt::uint64_t version() const
    {
        return _version;
    }

    inline Pt::uint32_t* userVersion()
    {
        return &_userVersion[0];
    }
    
private:
    std::map<std::string, Function*>  _functions;
    std::map<std::string, Parameter*> _types;
    std::string						  _name;
    Pt::uint64_t					  _version;
    Pt::uint32_t					  _userVersion[3];
};

}}

#endif

