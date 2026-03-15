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
#include <string>
#include <stdio.h>
#include <stdlib.h>
#include <sstream>

#include <mps/core/FactorOffsetSignalScaling.h>

using namespace std;

namespace mps{
namespace core{

FactorOffsetSignalScaling::FactorOffsetSignalScaling(void)
{
    registerProperty("factor", *this, &FactorOffsetSignalScaling::getFactor, &FactorOffsetSignalScaling::setFactor);
    registerProperty("offset", *this, &FactorOffsetSignalScaling::getOffset, &FactorOffsetSignalScaling::setOffset);
}

FactorOffsetSignalScaling::~FactorOffsetSignalScaling(void)
{ }

double FactorOffsetSignalScaling::scaleValue(const Pt::uint8_t* value)
{ return _factor* (*value) + _offset; }

double FactorOffsetSignalScaling::scaleValue(const Pt::int8_t* value)
{ return _factor* (*value) + _offset; }

double FactorOffsetSignalScaling::scaleValue(const Pt::uint16_t* value)
{ return _factor* (*value) + _offset; }

double FactorOffsetSignalScaling::scaleValue(const Pt::int16_t* value)
{ return _factor* (*value) + _offset; }

double FactorOffsetSignalScaling::scaleValue(const Pt::int32_t* value)
{ return _factor* (*value) + _offset; }

double FactorOffsetSignalScaling::scaleValue(const Pt::uint32_t* value)
{ return _factor* (*value) + _offset; }

double FactorOffsetSignalScaling::scaleValue(const Pt::int64_t* value)
{ return _factor* (*value) + _offset; }

double FactorOffsetSignalScaling::scaleValue(const Pt::uint64_t* value)
{ return _factor* (*value) + _offset; }

double  FactorOffsetSignalScaling::getFactor() const
{ return _factor; }

void FactorOffsetSignalScaling::setFactor(double factor)
{ _factor = factor; }

double FactorOffsetSignalScaling::getOffset() const
{ return _offset; }

void FactorOffsetSignalScaling::setOffset(double offset)
{ _offset = offset; }

double FactorOffsetSignalScaling::scaleValue(const float* value)
{
    return (*value) * _factor + _offset;
}

double FactorOffsetSignalScaling::scaleValue(const double* value)
{
        return (*value) * _factor + _offset;
}

string FactorOffsetSignalScaling::getScalingObjectAsXMLString(Pt::uint32_t objectID)
{
    std::stringstream ss;
    ss<<objectID;
    std::string strid;
    ss>>strid;
    char buffer[100];
    string scalingObject = "<mp:Scaling id=\"_";
    scalingObject += strid;
    scalingObject += "\">";
    scalingObject += "		  <mp:type>FACTOR_OFFSET</mp:type>\n";
    scalingObject += "		  <mp:factor>";
    sprintf(buffer,"%f",_factor);
    scalingObject += buffer;
    scalingObject += "</mp:factor>\n";
    scalingObject += "		  <mp:offset>";
    sprintf(buffer,"%f",_offset);
    scalingObject += buffer;
    scalingObject += "</mp:offset>\n";
    scalingObject += "</mp:Scaling>\n";
    return scalingObject;
}

}}
