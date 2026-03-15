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
#ifndef MPS_BUILDER_BUILDER_H
#define MPS_BUILDER_BUILDER_H

#include <Pt/NonCopyable.h>
#include <mps/builder/Api.h>
#include <mps/core/Object.h>
#include <mps/core/RuntimeEngine.h>

namespace mps {
namespace builder {

class ObjectFactory;

/**@brief Build a runtime engine from a XML stream.*/
class MPS_BUILDER_API XmlBuilder : public Pt::NonCopyable
{
public:
    /**@brief Load a scheme from file.
    *
    *  Instance a scheme from file and return its runtime engine.
    *  @param file The scheme file.
    *  @message If the return value is null pointer the message is filled.
    *  @return The runtime engine for the schema or null pointer.*/
    static mps::core::RuntimeEngine* load( const char* file, mps::core::Message& message, mps::core::ObjectFactory& factory);

    /**@brief Load a scheme from string.
    *
    *  Instance a scheme from string and return its runtime engine.
    *  @param xmlString The scheme string.
    *  @message If the return value is null pointer the message is filled.
    *  @return The runtime engine for the schema or null pointer.*/
    static mps::core::RuntimeEngine* loadXml( const char* xmlString, mps::core::Message& message, mps::core::ObjectFactory& factory);

    /**@brief Load a scheme from an intput stream.
    *
    *  Instance a scheme from an input stream and return its runtime engine.
    *  @param stream The scheme input stream.
    *  @message If the return value is null pointer the message is filled.
    *  @return The runtime engine for the scheme.*/
    static mps::core::RuntimeEngine* load( std::istream& stream, mps::core::Message& message, mps::core::ObjectFactory& factory);

    /**@brief Destroy a runtime engine created by load.
    *  @param file The scheme file.
    *  @return The runtime engine for the scheme.*/    
    static void destroy( mps::core::RuntimeEngine* runtime ); 
};    

}}

#endif
