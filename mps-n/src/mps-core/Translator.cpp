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
#include <mps/core/Translator.h>
#include <string.h>
#include <sstream>

using namespace std;

namespace mps{
namespace core{

Translator::Translator(void)
: _languageCode("")
{ }

Translator::~Translator(void)
{ }

const std::string& Translator::translate( const char* key )
{
    static std::string empty;

    if( _translationMap.find(key) == _translationMap.end())
        return empty;

    return _translationMap[key];
}

void Translator::addToTranslationMap(const char* key, const char* value)
{
    _translationMap[key] = value;
}

void Translator::clear()
{
    _translationMap.clear();
}

void Translator::addToTranslationMapFromStr(const char* aString)
{
    std::stringstream ss;
    enum { C_STRING_SIZE = 255 };
    char lineBuffer[C_STRING_SIZE];

    ss<<aString;

    if(!ss.good())
        return;

    while(ss.getline(lineBuffer, C_STRING_SIZE, '\n'))
    {
        std::stringstream sl;
        
        sl <<lineBuffer;

        sl.getline(lineBuffer, C_STRING_SIZE, '#');

        std::string key = lineBuffer;
        
        sl.getline(lineBuffer, C_STRING_SIZE, '\n');
        
        std::string value = lineBuffer;

        _translationMap[key] = value;
    }
}

void Translator::setLanguageCode(const char* languageCode)
{
    _languageCode = languageCode;
}

const std::string& Translator::languageCode() const
{
    return _languageCode;
}

}}
