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
#include "ObjectFactoryManager.h"
#include "DefaultObjectFactory.h"
#include <mps/core/RuntimeEngine.h>

#include <sstream>
#include <stdio.h>


namespace mps{
namespace core{

using namespace std;

ObjectFactoryManager::ObjectFactoryManager( void )
{
    _objectMap[0] = 0;
    addObjectFactory( new DefaultObjectFactory() );
}

ObjectFactoryManager::~ObjectFactoryManager(void)
{
    ObjectFactory* factory;

    //Delete only the default object factory.
    factory = _factories[0];
    delete factory;

    unregAll();
}

void ObjectFactoryManager::addObjectFactory( ObjectFactory* newFactory )
{
    _factories.push_back(newFactory);
}

Object* ObjectFactoryManager::createObject( const Pt::String& type, const Pt::String& name,Pt::uint32_t id )
{
    ObjectFactory*	factory;
    Object* newObject;

    for( Pt::uint32_t index = 0; index < _factories.size(); index++)
    {
        factory   = _factories[index];
        newObject = factory->createObject( type, name, id );		

        if( newObject != 0 )
        {
            regObject( id , newObject );
            return newObject;
        }
    }
    
    char buffer[200];
    std::string str  = name.narrow();

    if( str.size() == 0 )
        sprintf(buffer, "Unknown type '%s'. May be a module is not registered.", type.narrow().c_str());
    else
        sprintf(buffer, "Unknown type '%s' ( %s ). May be a module is not registered.", type.narrow().c_str(), str.c_str());

    throw std::runtime_error(buffer);

    return 0;
}

const Object* ObjectFactoryManager::getObjectByID( Pt::uint32_t id ) const
{
    if(id == 0)
        return 0;

    ObjectIdMap::const_iterator it = _objectMap.find(id);
    
    if( it == _objectMap.end())
        return 0;

    return  it->second;
}

void ObjectFactoryManager::regObject( Pt::uint32_t id, Object* object )
{
    _objectMap[id] = object;
}

void ObjectFactoryManager::unregObject( Pt::uint32_t objID )
{
    _objectMap.erase(objID);
}

void ObjectFactoryManager::unregAll()
{
    _objectMap.clear();
}

}}

