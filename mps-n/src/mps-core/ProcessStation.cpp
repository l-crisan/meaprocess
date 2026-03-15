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
#include <mps/core/ProcessStation.h>
#include <mps/core/RuntimeEngine.h>
#include <mps/core/Port.h>
#include <mps/core/Translator.h>
#include <mps/core/SignalList.h>
#include <mps/core/Signal.h>
#include <mps/core/SourceDescription.h>

#include <Pt/System/Clock.h>

#include <stdio.h>

namespace mps{
namespace core{

using namespace std;

ProcessStation::ProcessStation(void)
: _inputPorts(0)
, _outputPorts(0)
, _name("")
, _tanslator(0)
, _runtime(0)
, _propertyMap(0)
{
    registerProperty("name", *this, &ProcessStation::getName, &ProcessStation::setName);
}

ProcessStation::~ProcessStation(void)
{
}


ProcessStation::PSType ProcessStation::psType() const	
{ 
    return WorkPS; 
}

bool ProcessStation::isBase64(unsigned char c) 
{
  return (isalnum(c) || (c == '+') || (c == '/'));
}

const std::string& ProcessStation::signalList(const std::string& con) const
{
    return onGetSignalList(con);
}

const std::string& ProcessStation::onGetSignalList(const std::string& connection) const
{
    static std::string signalList("");
    return signalList;
}

void ProcessStation::base64Decode(std::string const& encoded_string, std::vector<Pt::uint8_t>& ret) 
{
    static const std::string base64_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                                     "abcdefghijklmnopqrstuvwxyz"
                                     "0123456789+/";

    int in_len = (int) encoded_string.size();
    int i = 0;
    int j = 0;
    int in_ = 0;
    unsigned char char_array_4[4], char_array_3[3];

    while (in_len-- && ( encoded_string[in_] != '=') && isBase64(encoded_string[in_])) 
    {
        char_array_4[i++] = encoded_string[in_]; 
        in_++;

        if (i==4) 
        {
        
            for (i = 0; i <4; i++)
                char_array_4[i] =(unsigned char)  base64_chars.find(char_array_4[i]);

            char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
            char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
            char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

            for (i = 0; (i < 3); i++)
                ret.push_back(char_array_3[i]);
            
            i = 0;
        }
    }

    if (i) 
    {
        for (j = i; j <4; j++)
            char_array_4[j] = 0;

        for (j = 0; j <4; j++)
            char_array_4[j] = (unsigned char) base64_chars.find(char_array_4[j]);

        char_array_3[0] = (char_array_4[0] << 2) + ((char_array_4[1] & 0x30) >> 4);
        char_array_3[1] = ((char_array_4[1] & 0xf) << 4) + ((char_array_4[2] & 0x3c) >> 2);
        char_array_3[2] = ((char_array_4[2] & 0x3) << 6) + char_array_4[3];

        for (j = 0; (j < i - 1); j++) 
            ret.push_back(char_array_3[j]);
    }
}

void ProcessStation::addObject(Object* object, const std::string& type, const std::string& subType)
{
    if( type == "Mp.InputPorts")
        _inputPorts = (ObjectVector<Port*>*) object;

    if( type == "Mp.OutputPorts")
        _outputPorts = (ObjectVector<Port*>*) object;
}

void ProcessStation::setSynchronTimer(bool synchron)
{
    _runtime->setSynchronTimer(synchron);
}

const std::string& ProcessStation::translate( const char* key )
{ 
    return _runtime->translate(key); 
}

const std::string& ProcessStation::languageCode() const
{
    return _runtime->languageCode();
}

std::string ProcessStation::format(const std::string& format, const std::string& par1)
{
    char* buffer = new char[format.size()+1 + par1.size() + 1];
    sprintf(buffer, format.c_str(), par1.c_str());
    
    std::string retval = buffer;
    
    delete buffer;

    return retval;
}

std::string ProcessStation::format(const std::string& format, const std::string& par1, const std::string& par2)
{
    char* buffer = new char[format.size()+1 + par1.size() + 1 + par2.size() + 1];
    sprintf(buffer,format.c_str(), par1.c_str(), par2.c_str());

    std::string retval = buffer;
    delete buffer;

    return retval;
}

void ProcessStation::onInitInstance()
{
    Pt::uint32_t index;

    if( _inputPorts != 0)
    {
        for( index = 0; index < _inputPorts->size(); index++ )
        {
            _inputPorts->at(index)->setParentPS( this );
            _inputPorts->at(index)->setInputPort(true);
            _inputPorts->at(index)->setPortNumber(index);
            _inputPorts->at(index)->onInitInstance();
        }
    }

    if( _outputPorts != 0)
    {
        for( index = 0; index < _outputPorts->size(); index++ )
        {
            _outputPorts->at(index)->setParentPS( this );
            _outputPorts->at(index)->setInputPort(false);
            _outputPorts->at(index)->setPortNumber(index);
            _outputPorts->at(index)->onInitInstance();		
        }
    }
}

void ProcessStation::onExitInstance()
{
    Port*	port;
    Pt::uint32_t	index;

    if( _inputPorts != 0)
    {
        for(index = 0; index < _inputPorts->size(); index++)
        {
             port = _inputPorts->at(index);
             port->onExitInstance();
             delete port;
        }

        _inputPorts->clear();
        delete _inputPorts;
    }

    if( _outputPorts != 0 )
    {
        for(index = 0; index < _outputPorts->size(); index++)
        {
             port = _outputPorts->at( index );
             port->onExitInstance();
             delete port;
        }

        _outputPorts->clear();
        delete _outputPorts;
    }		
}

void ProcessStation::onUpdateDataValue( Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Port* port, const Pt::uint8_t* data )
{
}

void ProcessStation::onInitialize()
{ }

void ProcessStation::onStart()
{  }

void ProcessStation::onStop()
{  }

void ProcessStation::onDeinitialize()
{  }

bool ProcessStation::isSynchronizedPS() const 
{ 
    return false; 
}

MessageResult::MsgResult ProcessStation::sendMessage( Message& message )
{
    if(_runtime != 0)
        return _runtime->sendMessage(message);

    return MessageResult::No;
}

void ProcessStation::setRuntime(RuntimeEngine* runtime)
{
    _runtime = runtime;
}

void ProcessStation::stopRuntimeEngine(Pt::uint32_t delayMs)
{
    _runtime->stop(true, delayMs);
}

const std::string& ProcessStation::getName() const
{ 
    return _name; 
}

void ProcessStation::setName( const string& name )
{ 
    _name = name; 
}

std::string ProcessStation::getPropertyValue(const char* propName) const
{
    return _runtime->getPropertyValue(propName);
}

std::string ProcessStation::replaceProperties(std::string args) const
{
    return _runtime->replaceProperties(args);
}

void ProcessStation::setPropertyValue(const char* propName, double value)
{
    _runtime->setPropertyValue(propName, value);
}

double ProcessStation::getPropertyNumericValue(const char* propName) const
{
    return _runtime->getPropertyNumericValue(propName);
}

void ProcessStation::setPropertyValue(const char* propName, const char* propValue)
{
    _runtime->setPropertyValue(propName, propValue);
}

std::string ProcessStation::getPropertyValueFromKey(const char* propKey) const
{
    return _runtime->getPropertyValueFromKey(propKey);
}

std::string ProcessStation::getRawPropertyValueFromKey(const char* propKey) const
{
    return _runtime->getRawPropertyValue(propertyName(propKey).c_str());
}

bool ProcessStation::isProperty(const char* propKey) const
{
    return _runtime->isProperty(propKey);
}

std::string ProcessStation::propertyName(const char* propKey) const
{
    return _runtime->propertyName(propKey);
}

const SourceDescription* ProcessStation::getSourceDescription(Pt::uint32_t id) const
{
    return _runtime->getSourceDescription(id);
}

Pt::uint32_t ProcessStation::timerResolution() const
{ 
    return (Pt::uint32_t)_runtime->timerResolution(); 
}

Pt::uint64_t ProcessStation::currentTime() const
{
    return _runtime->currentTime();
}

Pt::uint64_t ProcessStation::startTime() const
{
    return _runtime->startTime();
}

const std::string& ProcessStation::workingDirectory() const
{
    return _runtime->workingDirectory();
}

}}
