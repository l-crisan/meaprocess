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
#include <mps/core/Signal.h>
#include <mps/core/RuntimeEngine.h>
#include <mps/core/SignalScaling.h>
#include <sstream>

namespace mps{
namespace core{

using namespace std;

Signal::Signal(Pt::uint32_t id)
: _name("")
, _unit("")
, _comment("")
, _dataType(SignalDataType::VT_bool)
, _physMin(0.0)
, _physMax(0.0)
, _scaling(0)
, _signalValueSize(0)
, _signed(false)
, _sampleRate(0.0)
, _sourceNumber(0)
, _id(id)
, _parameters()
, _objSize(0)
{
    registerProperty( "name", *this, &Signal::name, &Signal::setName );
    registerProperty( "unit", *this, &Signal::unit, &Signal::setUnit );
    registerProperty( "comment", *this, &Signal::comment, &Signal::setComment );
    registerProperty( "samplerate", *this, &Signal::sampleRate, &Signal::setSampleRate );
    registerProperty( "valueDataType", *this, &Signal::valueDataType, &Signal::setValueDataType );
    registerProperty( "sourceNumber", *this, &Signal::sourceNumber, &Signal::setSourceNumber ); 
    registerProperty( "physMin", *this, &Signal::physMin, &Signal::setPhysMin ); 
    registerProperty( "physMax", *this, &Signal::physMax, &Signal::setPhysMax ); 
    registerProperty( "parameters", *this, &Signal::parameters, &Signal::setParameters ); 
    registerProperty( "objSize", *this, &Signal::objectSize, &Signal::setObjectSize ); 
    registerProperty( "cat", *this, &Signal::cat, &Signal::setCat ); 
}

Signal::~Signal(void)
{ 
    if(_scaling != 0)
    {
        delete _scaling;
        _scaling = 0;
    }
}

Pt::uint8_t Signal::valueDataType() const
{
    return (Pt::uint8_t) _dataType;
}

void Signal::addObject(Object* object, const std::string& type, const std::string& subType)
{
    if( type == "Mp.Scaling")
        _scaling = (SignalScaling*) object;
}

void Signal::setValueDataType( Pt::uint8_t valueType )
{	
    _dataType  = (SignalDataType::Type) valueType;
    
    switch( _dataType )
    {
        case SignalDataType::VT_bool:
            _signalValueSize    = 1;
            _signed             = false;
        break;

        case SignalDataType::VT_real32:
            _signalValueSize    = sizeof(float);
            _signed             = true;
        break;
        case SignalDataType::VT_real64:
            _signalValueSize    = sizeof(double);
            _signed             = true;
        break;	    
    
        case SignalDataType::VT_uint8_t:
            _signalValueSize    = sizeof(Pt::uint8_t);
            _signed             = false;
        break;
    
        case SignalDataType::VT_int8_t:
            _signalValueSize    = sizeof(Pt::int8_t);
            _signed             = true;
        break;
    
        case SignalDataType::VT_uint16_t:
            _signalValueSize    = sizeof(Pt::uint16_t);
            _signed             = false;
        break;	    
    
        case SignalDataType::VT_int16_t:
            _signalValueSize    = sizeof(Pt::int16_t);
            _signed             = true;
        break;
    
        case SignalDataType::VT_uint32_t:
            _signalValueSize    = sizeof(Pt::uint32_t);
            _signed             = false;
        break;                       
    
        case SignalDataType::VT_int32_t:
            _signalValueSize    = sizeof(Pt::int32_t);
            _signed             = true;
        break;
    
        case SignalDataType::VT_uint64_t:
            _signalValueSize    = sizeof(Pt::uint64_t);
            _signed             = false; 
        break;
    
        case SignalDataType::VT_int64_t:
            _signalValueSize    = sizeof(Pt::int64_t);
            _signed             = true;
        break;
            
        case SignalDataType::VT_string:
        case SignalDataType::VT_object:
            _signalValueSize     = _objSize;
            _signed              = false;
        break;
    }
}

void Signal::onInitInstance()
{ }

void Signal::onExitInstance()
{
    if(_scaling)
        delete _scaling;

    _scaling = 0;
}

string Signal::valueDataTypeAsString() const
{
    return SignalDataType::toString(_dataType);	
}

double Signal::scaleValue( const Pt::uint8_t* value) const
{
    switch( _dataType )
    {
        case SignalDataType::VT_bool:
            if( _scaling != 0)
            {
                return _scaling->scaleValue((Pt::uint8_t*)value);
            }
            else
            {
                Pt::uint8_t* data = (Pt::uint8_t*)value;
                return (double) *data;
            }

        case SignalDataType::VT_int8_t:
            if( _scaling != 0)
            {
                return _scaling->scaleValue((Pt::int8_t*)value);
            }
            else
            {
                Pt::int8_t* data = (Pt::int8_t*)value;
                return(double) *data;
            }

    case SignalDataType::VT_uint8_t:
            if( _scaling != 0)
            {
                return _scaling->scaleValue((Pt::uint8_t*)value);
            }
            else
            {
                Pt::uint8_t* data = (Pt::uint8_t*)value;
                return (double) *data;
            }

        case SignalDataType::VT_int16_t:
            if( _scaling != 0)
            {
                return _scaling->scaleValue((Pt::int16_t*) value);
            }
            else
            {
                Pt::int16_t* data = (Pt::int16_t*)value;
                return (double) *data;
            }

        case SignalDataType::VT_uint16_t:
            if( _scaling != 0)
            {
                return _scaling->scaleValue((Pt::uint16_t*)value);
            }
            else
            {
                Pt::uint16_t* data = (Pt::uint16_t*)value;
                return (double) *data;
            }

    case SignalDataType::VT_int32_t:
            if( _scaling != 0)
            {
                return _scaling->scaleValue((Pt::int32_t*)value);
            }
            else
            {
                Pt::int32_t* data = (Pt::int32_t*)value;
                return (double) *data;
            }

        case SignalDataType::VT_uint32_t:
            if( _scaling != 0)
            {
                return _scaling->scaleValue((Pt::uint32_t*)value);
            }
            else
            {
                Pt::uint32_t* data = (Pt::uint32_t*)value;
                return (double) *data;
            }

    case SignalDataType::VT_int64_t:
            if( _scaling != 0)
            {
                return _scaling->scaleValue((Pt::int64_t*)value);
            }
            else
            {
                Pt::int64_t* data = (Pt::int64_t*)value;
                return (double) *data;
            }

    case SignalDataType::VT_uint64_t:
            if( _scaling != 0 )
            {
                return _scaling->scaleValue((Pt::uint64_t*)value);
            }
            else
            {
                Pt::uint64_t* data = (Pt::uint64_t*)value;
                return (double) *data;
            }
        case    SignalDataType::VT_real32:
            if( _scaling != 0 )
            {
                return _scaling->scaleValue((float*)value);
            }
            else
            {
                float* data = (float*)value;
                return (double) *data;
            }

        case SignalDataType::VT_real64:
            if( _scaling != 0 )
                return _scaling->scaleValue((double*)value);
            else
                return *((double*)value);
        default:
            return 0;
    }

    return 0;
}

void Signal::setParameters(const std::string& params)
{
    stringstream ss;

    ss<<params;

    char buffer[255];
    while(ss.getline( buffer,255,';'))
    {
        stringstream ps;
        ps << buffer;
        ps.getline(buffer,255,'=');
        string name = buffer;
        ps.getline(buffer,255,'=');
        string value = buffer;
        pair<string,string> pair(name,value);
        _parameters.push_back(pair);
    }
}

const std::string& Signal::name() const
{ 
    return _name;
}

void Signal::setName(const std::string& name)
{ 
    _name = name;
}

const std::string& Signal::unit() const
{ 
    return _unit;
}

void Signal::setUnit(const std::string& unit)
{  
    _unit = unit;
}


const std::string& Signal::comment() const
{ 
    return _comment; 
}

void Signal::setComment(const std::string& comment)
{ 
    _comment = comment ;
}

double Signal::physMin() const
{ 
    return _physMin;
}

void Signal::setPhysMin(double min)
{ 
    _physMin = min;
}

double Signal::physMax() const
{ 
    return _physMax;
}

void Signal::setPhysMax(double max)
{ 
    _physMax = max;
}

Pt::uint32_t Signal::valueSize() const
{ 
    if(	 _dataType ==  SignalDataType::VT_object)
        return _objSize;

    return _signalValueSize; 
}

bool Signal::isValueSigned() const
{ 
    return _signed; 
}

double Signal::sampleRate() const
{ 
    return _sampleRate;
}

void Signal::setSampleRate(double sampleRate)
{
    _sampleRate = sampleRate;
}

SignalScaling* Signal::scaling() const
{ 
    return _scaling;
}

Pt::uint32_t Signal::sourceNumber() const
{ 
    return _sourceNumber;
}

void Signal::setSourceNumber( Pt::uint32_t number )
{ 
    _sourceNumber = number;
}

Pt::uint32_t Signal::signalID() const
{ 
    return _id;
}

const Parameters& Signal::getParameters() const
{ 
    return _parameters;
}

const std::string& Signal::parameters() const
{
    return _name;
}

}}
