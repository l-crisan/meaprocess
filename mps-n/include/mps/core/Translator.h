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
#ifndef MPS_CORE_TRANSLATOR_H
#define MPS_CORE_TRANSLATOR_H

#include <string>
#include <vector>
#include <map>
#include <mps/core/Api.h>

namespace mps {
namespace core {


 /** @brief Implements a translation map.*/
class MPS_CORE_API Translator
{
public:
    /** @brief Default contructor.*/
    Translator(void);

    /** @brief Destructor.*/
    virtual ~Translator(void);

    /** @brief Clear the translation map. */
    void clear();

    /** @brief Sets the language code for the loadaded translation map.
    *
    *   The language code is a string of the form "de-DE", "en-US".
    *   @param code The language code.*/
    void setLanguageCode( const char* code );

    /** @brief Return the current language code.
    *   @return The language code.*/
    const std::string& languageCode() const;

    /** @brief Translate a string.
    *   @param key The key to translate.
    *   @return The translated string.*/
    const std::string& translate( const char* key ) ;

    /** @brief Adds a Key/Text pair to map.
    *   @param key The key.
    *   @param value The text.*/
    void addToTranslationMap( const char* key, const char* value );

    /** @brief Adds Key/Text pairs to the map.
    *
    *   Teh key/text pairs are separeted by: "Key # Value\n Key # Value".
    *   @param key The Key/text pairs.*/
    void addToTranslationMapFromStr( const char* aString ); // Key # Value\n Key # Value ...

private:
    std::map<std::string, std::string>  _translationMap;
    std::string                         _languageCode;
};

}}

#endif
