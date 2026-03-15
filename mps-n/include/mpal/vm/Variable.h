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

#ifndef MPAL_VM_VARIABLE_H
#define MPAL_VM_VARIABLE_H

#include <Pt/Types.h>
#include <Pt/Any.h>
#include <mpal/vm/mpal-vm.h>
#include <vector>
#include <string>

namespace mpal{
namespace vm{

/** @brief The variable data type */
enum DataType
{
    BOOL = 1,   ///< Boolean 8 bit type.
    SINT,       ///< Short integer 8 bit type.
    INT,        ///< Integer 16 bit type.
    DINT,       ///< Integer 32 bit type.
    LINT,       ///< Integer 64 bit type.
    USINT,      ///< Unsigned short integer 8 bit type.
    UINT,       ///< Unsigned integer 16 bit type.
    UDINT,      ///< Unsigned integer 32 bit type.
    ULINT,      ///< Unsigned integer 64 bit type.
    REAL,       ///< Real 32 bit type.
    LREAL,      ///< Long real 64 bit type.
    STRING,     ///< String type.
    BYTE,       ///< Bit string 8 bit type.
    WORD,       ///< Bit string 16 bit type.
    DWORD,      ///< Bit string 32 bit type.
    LWORD,      ///< Bit string 64 bit type.
    WSTRING,    ///< Wide character string type.
    STRUCT,     ///< Struct type.
    ARRAY,      ///< Array type.
    UDT,        ///< User defined type.
    FB,         ///< Function block type.
    TYPEID,     ///< Reserved
    ENUM        ///< Enumeration
};

/** @brief The variable access */
enum Access
{
  Input = 1,        ///< Input variable
  Output,           ///< Output variable
  InOut,            ///< Input/Output variable
  Var,              ///< Local variable
  VarConst,         ///< Local constant variable
  VarTemp,          ///< Local temporary variable
  VarTempConst,     ///< Local temporary constant variable
  Const,            ///< Global constant variable
  None              ///< Reserved
};

/**@brief This class implements an array of from/to Dimension.*/
typedef std::vector<std::pair<Pt::int64_t,Pt::int64_t> > Dimensions;

/**@brief This class represents a program variable*/
class MPAL_VM_API Variable
{
    public:
    /**@brief Constructs a new variable object.
       @param type The data type of the variable.
       @param access The access of the variable.
       @param name The name of the variable.
       @param offset The offset of the variable.
       @param size The size in byte of the variable.
       @param defVal The default value of the variable.*/
    Variable(DataType type, Access access, const char* name, Pt::uint32_t offset, Pt::uint32_t size, Pt::Any defVal);

    /**@brief Destructor*/
    virtual ~Variable();

    /**@brief Gets the data type of the variable.
       @return The data type.*/
    DataType type() const;

    /**@brief Gets the access of the variable.
       @return The access.*/
    Access access() const;

    /**@brief The name of the variable.
       @return The name.*/
    const std::string& name() const;

    /**@brief By a structured variable gets the member variables.
       @return The member variables.*/
    std::vector<Variable*>& variables();

    /**@brief By an array variable gets the array dimensions.
       @return The array dimensions.*/	
    Dimensions& dimensions();

    /**@brief The default value of the variable.
       @return The default value.*/
    Pt::Any defaultValue() const;

    /**@brief The size of the variable.
       @return The size .*/
    Pt::uint32_t size() const;

    /**@brief The offset of the variable.
       @return The offset .*/
    Pt::uint32_t offset() const;

private:
   std::vector<Variable*>   _variables;
   std::string              _name;
   DataType                 _dataType;
   Access                   _access;
   Dimensions               _dimensions;
   Pt::Any                  _defVal;
   Pt::uint32_t             _size;
   Pt::uint32_t             _offset;
};

}}
#endif

