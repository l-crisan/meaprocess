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
#include <mps/modld/ModuleLoader.h>
#include <Pt/System/Logger.h>

PT_LOG_DEFINE("mps.modld.ModuleLoader");

namespace mps {
namespace modld {

typedef mps::core::ObjectFactory* (MpsGetFactory)(void);

std::vector<Pt::System::Library*> ModuleLoader::_loadedModules;

mps::core::ObjectFactory* ModuleLoader::loadModule( const char* modulePath)
{
    try
    {
    
        Pt::System::Path libPath(modulePath);
        Pt::System::Library* library = new Pt::System::Library(libPath);

    
        MpsGetFactory* getFactory = (MpsGetFactory*) library->resolve("mpsGetFactory");


        if( getFactory  == 0)
        {
            PT_LOG_ERROR("The module: " << modulePath << " is not a MeaProcess module ");

            library->close();
            delete library;
            return 0;
        }

        mps::core::ObjectFactory* factory = getFactory();

        _loadedModules.push_back(library);
        return factory;

    }
    catch(const std::exception& ex)
    {
        PT_LOG_ERROR("The module: " << modulePath << " couldn't be loaded.");
        PT_LOG_ERROR("What: " << ex.what());
    }

    return 0;
}

void ModuleLoader::unloadAllModules()
{
    for( Pt::uint32_t i = 0; i< _loadedModules.size(); ++i )
    {
        Pt::System::Library* library = _loadedModules[i];
        library->close();
        delete library;
    }

    _loadedModules.clear();
}
 
}}

