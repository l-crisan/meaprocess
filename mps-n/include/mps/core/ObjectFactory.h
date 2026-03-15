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
#ifndef MPS_CORE_OBJECTFACTORY_H
#define MPS_CORE_OBJECTFACTORY_H

#include <Pt/String.h>
#include <mps/core/Api.h>
#include <mps/core/Object.h>

namespace mps {
namespace core{

/**@brief The object factory interface. */
class MPS_CORE_API ObjectFactory 
{
    public:
        /**@brief The factory methode.
        * 
        * Overwrite this to create your own objects.
        *
        * @param type The object type identifier.
        * @param sybType The object sub type.
        * @param id The identifier of the object.
        * @return The new object of the requested type or 0 be unknown type. */
        virtual Object* createObject( const Pt::String& type, const Pt::String& sybType, Pt::uint32_t id ) = 0;

        /**@brief returns the resource id.
        * 
        * Overwrite this and return your resource id. The resource id is used to locate 
        * the resource file. The resource file is composed by "<resource id>.<lang code>.mres" where
        * <lang code> is formated as following: for german resource "de-DE" for english resource "en-US".
        *
        * @return The resource id. */
        virtual std::string resourceID() const = 0;


        virtual ~ObjectFactory()
        {
        }	
};

}}

#endif
