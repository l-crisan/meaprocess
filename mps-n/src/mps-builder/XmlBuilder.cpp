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
#include <mps/builder/XmlBuilder.h>
#include <mps/core/ObjectFactory.h>
#include <Pt/System/Logger.h>
#include <Pt/System/Clock.h>
#include <Pt/NonCopyable.h>
#include <Pt/Xml/XmlReader.h>
#include <fstream>
#include <sstream>
#include <algorithm>
#include <istream>

#include "XmlObjectBuilder.h"

PT_LOG_DEFINE("mps.builder.XmlBuilder");

namespace mps {
namespace builder {

using namespace std;

mps::core::RuntimeEngine* XmlBuilder::load( const char* file, mps::core::Message& message, mps::core::ObjectFactory& factory)
{
    fstream fs;

    fs.open( file, ios_base::in | ios_base::binary);

    if( !fs.is_open() )
    {
        std::string text = "File '";
        text += file;
        text += "' could not be opend!";	    
        PT_LOG_ERROR(text);
        message.setText(text);
        message.setType(mps::core::Message::Error);
        message.setTimeStamp(Pt::System::Clock::getLocalTime());
        message.setTarget(mps::core::Message::Output);	    		
        return 0;
    }
       
    return load( fs, message, factory);
}

mps::core::RuntimeEngine* XmlBuilder::loadXml( const char* xmlString, mps::core::Message& message, mps::core::ObjectFactory& factory)
{
    stringstream ss;
    ss<< xmlString;
    return  load( ss, message, factory);
}

mps::core::RuntimeEngine* XmlBuilder::load( std::istream& stream, mps::core::Message& message, mps::core::ObjectFactory& factory)
{
    try
    {
        mps::core::RuntimeEngine* runtime = (mps::core::RuntimeEngine*) XmlObjectBuilder::build( stream , factory);		
        runtime->onInitInstance();
        return runtime;
    }
    catch( const std::exception& e)
    {
        std::stringstream ss;		
        ss<<"Load scheme failed. May be one or more modules are missing. '"<< e.what()<<"'";
        std::string msg = ss.str();
        PT_LOG_ERROR(msg);
        message.setText(msg);
        message.setType(mps::core::Message::Error);
        message.setTimeStamp(Pt::System::Clock::getLocalTime());
        message.setTarget(mps::core::Message::Output);
    }

    return 0;
}

void XmlBuilder::destroy( mps::core::RuntimeEngine* runtime )
{
    runtime->onExitInstance();
    delete runtime;
}


}}
