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
#ifndef MPS_MODLD_MODULELOADER_H
#define MPS_MODLD_MODULELOADER_H

#include <Pt/NonCopyable.h>
#include <Pt/System/Library.h>
#include <mps/modld/Api.h>
#include <mps/core/ObjectFactory.h>

namespace mps {
namespace modld {

/**@brief Build a runtime engine from a XML stream.*/
class MPS_MODLD_API ModuleLoader : public Pt::NonCopyable
{
public:           
    /**@brief Load a module that conatain process stations or plugins.
    *  @param modulePath The path of the module.
    *  @return True if successful.*/
    static mps::core::ObjectFactory* loadModule( const char* modulePath);

    /**@brief Unload all loaded modules.
    *
    *  Call this before leave the main function to unload all modules.
    */
    static void unloadAllModules();

private:
    static std::vector<Pt::System::Library*> _loadedModules;
};    

}}

#endif
