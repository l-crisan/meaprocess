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
#include "XmlObjectBuilder.h" 
#include <mps/core/ObjectFactory.h>
#include <Pt/Convert.h>
#include <Pt/NonCopyable.h>
#include <Pt/Xml/XmlReader.h>
#include <Pt/System/Logger.h>
#include <Pt/Xml/StartElement.h>
#include <Pt/Xml/Characters.h>
#include <Pt/Xml/EndElement.h>
#include "Pt/Xml/InputSource.h"
#include <exception>
#include <sstream>
#include <fstream>
#include <stdio.h>
#include <stdlib.h>


namespace mps{ 
namespace builder{

struct ElementData
{
    std::string name;
    std::string atrName;
};

Pt::System::Logger XmlObjectBuilder::_logger("mps.XmlObjectBuilder");


Pt::uint32_t toSize(const Pt::String& s)
{
    static Pt::DecimalFormat<Pt::Char> fmt;
    int n = 0;
    Pt::parseInt(s.begin(), s.end(), n, fmt);
    return n;
}

mps::core::Object* XmlObjectBuilder::build( std::istream& is, mps::core::ObjectFactory& factory)
{
    Pt::Xml::BinaryInputSource	source(is);
    Pt::Xml::XmlReader			reader(source);
    Pt::Xml::InputIterator		iterator = reader.current();

    return loadObject( 0, iterator, reader, factory );
}

mps::core::Object* XmlObjectBuilder::loadObject( mps::core::Object* parent, Pt::Xml::InputIterator& iterator, Pt::Xml::XmlReader& stream, mps::core::ObjectFactory& factory  )
{
    ElementData         lastElementData;
    mps::core::Object*  object = 0;
    Pt::Any             aValue;

    for( ; iterator != stream.end(); ++iterator)
    {
        const Pt::Xml::Node& node = *iterator;

        if( const Pt::Xml::StartElement* e = Pt::Xml::toStartElement( &node ) )
        { //Start element
            if( e->name().local() == L"object" )
            {//Object node
                
                const Pt::Xml::AttributeList& attributes = e->attributes();
                const Pt::uint32_t  objId	= (Pt::uint32_t) toSize(attributes.get(L"id"));
                const Pt::String& type	  = attributes.get(L"type");

                Pt::String subType = L"";
                
                if(attributes.has(L"subType"))
                    subType = attributes.get(L"subType");

                object = factory.createObject( type, subType, objId );

                if(object == 0)
                    throw std::logic_error( "Invalid scheme." + PT_SOURCEINFO );

                //Give the object to the parent.
                if( parent != 0 )
                    parent->addObject(object, type.narrow(), subType.narrow());
                
                ++iterator;

                const Pt::Xml::Node& end = *iterator;
                
                if( end.type() != Pt::Xml::Node::EndElement)
                    loadObject( object, iterator, stream, factory );
            }
            else
            {   
                //Save last element data
                const Pt::Xml::AttributeList& attributes = e->attributes();

                lastElementData.name	= e->name().local().narrow();
                lastElementData.atrName = attributes.get(L"name").narrow();
            }
        }
        else if( const Pt::Xml::Characters* e = Pt::Xml::toCharacters(&node) )
        { // Text node
            
            if( lastElementData.name != "" && parent != 0 )
            {
                aValue = getValue( lastElementData.name ,  e->content().narrow() );
                parent->setProperty( lastElementData.atrName, aValue );
            }
        }
        else if( const Pt::Xml::EndElement* e = Pt::Xml::toEndElement(&node) )
        {//End Element
            if(e->name().local() == L"object")
            {
                if(parent == 0)
                    throw  std::logic_error("Invalid scheme." + PT_SOURCEINFO );

                return parent;
            }
            else
            {
                if(lastElementData.name == "")
                    throw  std::logic_error("Invalid scheme." + PT_SOURCEINFO );

                lastElementData.name = "";
                lastElementData.atrName = "";
            }
        }
    }

    return object;
}

Pt::Any XmlObjectBuilder::getValue(const std::string& type, const std::string& value)
{
    Pt::Any anyValue;
    std::stringstream ss;
    ss << value;

    if( type == "string" )
    {
        anyValue = value;
    }
    else if( type == "uint8_t" ) 
    {
        int v;
        ss>>v;
        anyValue = (Pt::uint8_t)v;
    }
    else if( type == "int8_t")
    {
        int v;
        ss>>v;
        anyValue = (Pt::int8_t)v;
    }
    else if( type == "uint16_t")
    {
        Pt::uint16_t v;
        ss>>v;
        anyValue = v;
    }
    else if( type == "int16_t")
    {
        Pt::int16_t v;
        ss>>v;
        anyValue = v;
    }
    else if( type == "uint32_t")
    {
        Pt::uint32_t v;
        ss>>v;
        anyValue = v;
    }
    else if( type == "int32_t")
    {
        Pt::int32_t v;
        ss>>v;
        anyValue = v;
    }
    else if( type == "uint64_t")
    {
        Pt::uint64_t v;
        ss>>v;
        anyValue = v;
    }
    else if( type == "int64_t")
    {
        Pt::int64_t v;
        ss>>v;
        anyValue = v;
    }
    else if( type == "double")
    {
        double v;
        ss>>v;
        anyValue = v;
    }
    else if( type == "bool")
    {
        anyValue = atol(value.c_str()) > 0;
    }

    return anyValue;
}

}}
