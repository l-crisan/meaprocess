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
#include "FuzzyOperation.h"
#include <algorithm>

namespace mps {
namespace controller{
    
FuzzyOperation::FuzzyOperation()
: _leftOp(0)
, _rightOp(0)
{
    registerProperty( "type", *this, &FuzzyOperation::type, &FuzzyOperation::setType );
    registerProperty( "name", *this, &FuzzyOperation::varName, &FuzzyOperation::setVarName );
}

FuzzyOperation::~FuzzyOperation()
{
    if(_rightOp != 0)
        delete _rightOp;

    if( _leftOp != 0)
        delete _leftOp;
}

void FuzzyOperation::addObject(Object* obj, const std::string& type, const std::string& subType)
{
    if(_leftOp == 0)
    {
        _leftOp = (FuzzyOperation*) obj;
    }
    else
    {
        if(_rightOp == 0)
            _rightOp = (FuzzyOperation*)obj;
    }
}


double FuzzyOperation::eval() const
{
    switch(_type)
    {
        case FuzzyOperation::LingVar:
            return _linguisticVar->fuzzyValue();
        break;

        case FuzzyOperation::AND:
        {
            double val1 = _leftOp->eval();
            double val2 = _rightOp->eval();
            return std::min(val1,val2);
        }
        break;

        case FuzzyOperation::OR:
        {
            double val1 = _leftOp->eval();
            double val2 = _rightOp->eval();
            return std::max(val1,val2);
        }
        break;
    }

    return 0;
}

}}
