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
#ifndef MPS_FUZZY_OPERATION_H
#define MPS_FUZZY_OPERATION_H

#include <Pt/Types.h>
#include <mps/core/Object.h>
#include "LinguisticVar.h"

namespace mps {
namespace controller{

class FuzzyOperation : public mps::core::Object
{
public:
    FuzzyOperation();
    virtual ~FuzzyOperation();
        
    enum OperationType
    {
        LingVar,
        AND,
        OR,
    };

    inline Pt::uint8_t type() const
    {
        return _type;
    }

    inline void setType(Pt::uint8_t t)
    {
        _type = t;
    }

    inline const std::string& varName() const
    {
        return _varName;
    }

    inline void setVarName(const std::string& n)
    {
        _varName = n;
    }

    void setLinguisticVar(const LinguisticVar* var)
    {
        _linguisticVar = var;
    }

    const LinguisticVar* linguisticVar() const
    {
        return _linguisticVar;
    }
    
    void addObject(mps::core::Object* obj, const std::string& type, const std::string& subType);

    FuzzyOperation* left() const
    {
        return _leftOp;
    }

    FuzzyOperation* right() const
    {
        return _rightOp;
    }

    double eval() const;

private:
    Pt::uint8_t _type;
    std::string _varName;
    FuzzyOperation* _leftOp;
    FuzzyOperation* _rightOp;
    const LinguisticVar*  _linguisticVar;
};

}}

#endif
