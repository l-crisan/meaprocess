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
#ifndef MPS_CORE_PROPERTY_H
#define MPS_CORE_PROPERTY_H

#include <Pt/Types.h>
#include <mps/core/Object.h>
#include <sstream>

namespace mps{
namespace core{

class Property : public Object
{
public:
    Property();
    virtual ~Property();

    inline const std::string& name() const
    { return _name; }

    inline void setName(const std::string& name)
    { _name = name; }

    inline const std::string& value() const
    { return _value; }

    inline void setValue(const std::string& value)
    {
        _value = value;
    }

    inline void setNumericValue(double& value)
    { 
        std::stringstream ss;
        ss<<value;
        ss>>_value;
    }
    
    inline double numericValue() const
    {
        std::stringstream ss;
        double value;
        ss << _value;
        ss  >> value;

        return value; 
    }

    inline const std::string& type() const
    { return _type; }

    inline void setType(const std::string& type)
    { _type = type;}

    inline bool mandatory() const
    {  return _mandatory; }

    inline void setMandatory(bool mandatory)
    { _mandatory = mandatory; }

private:
    std::string _name;
    std::string _value;
    std::string _type;
    bool _mandatory;
};

}}

#endif
