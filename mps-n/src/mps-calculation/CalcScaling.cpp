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
#include "CalcScaling.h"
#include <sstream>

namespace mps{
namespace calculation{

CalcScaling::CalcScaling()
{
    registerProperty( "outSignal", *this, &CalcScaling::outSignal, &CalcScaling::setOutSignal );
    registerProperty( "type", *this, &CalcScaling::type, &CalcScaling::setType );
    registerProperty( "factor", *this, &CalcScaling::factor, &CalcScaling::setFactor );
    registerProperty( "offset", *this, &CalcScaling::offset, &CalcScaling::setOffset );
    registerProperty( "signal", *this, &CalcScaling::signal, &CalcScaling::setSignal );
    registerProperty( "p1x", *this, &CalcScaling::p1x, &CalcScaling::setP1x );
    registerProperty( "p1y", *this, &CalcScaling::p1y, &CalcScaling::setP1y );
    registerProperty( "p2x", *this, &CalcScaling::p2x, &CalcScaling::setP2x );
    registerProperty( "p2y", *this, &CalcScaling::p2y, &CalcScaling::setP2y );
    registerProperty( "table", *this, &CalcScaling::table, &CalcScaling::setTable );
}

CalcScaling::~CalcScaling()
{

}

void CalcScaling::setTable(const std::string& tableStr)
{
    std::stringstream ss;

    ss<<tableStr;
    char buffer[100];
    while(ss.getline(buffer,100,';'))
    {
        std::stringstream ss2;
        ss2 << buffer;

        char buffer2[100];
        ss2.getline(buffer2,100,',');
        std::stringstream convx;

        convx << buffer2;
        double x;
        convx >> x;

        ss2.getline(buffer2,100,',');

        std::stringstream convy;

        double y;
        convy << buffer2;
        convy >> y;

        PointF point(x,y);
        _table.push_back(point);
    }
}

}}
