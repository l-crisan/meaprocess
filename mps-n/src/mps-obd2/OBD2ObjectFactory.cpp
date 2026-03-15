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
#include "OBD2ObjectFactory.h"
#include "OBD2Signal.h"
#include "OBD2SourcePS.h"
#include "OBD2VehicleInfo.h"
#include "mps-obd2.h"

namespace mps{
namespace obd2{

using namespace mps::core;

OBD2ObjectFactory::OBD2ObjectFactory(void)
{
}

OBD2ObjectFactory::~OBD2ObjectFactory(void)
{
}
    
std::string OBD2ObjectFactory::resourceID() const
{
    return "mps-obd2";
}

mps::core::Object* OBD2ObjectFactory::createObject( const Pt::String& type, const Pt::String& sybtype, Pt::uint32_t id )
{
    if(type == L"Mp.OBD2.PS.In")
    {
        return new OBD2SourcePS();
    }
    else if( sybtype == L"Mp.OBD2.Sig")
    {
        return new OBD2Signal(id);
    }
    else if( type == L"Mp.OBD2.VehicleInfos")
    {
        return new mps::core::ObjectVector<OBD2VehicleInfo*>();
    }
    else if( type == L"Mp.OBD2.VehicleInfo")
    {
        return new OBD2VehicleInfo();
    }
    return 0;
}

extern "C"
{
    MPSOBD2_API ObjectFactory* mpsGetFactory()
    {
            static OBD2ObjectFactory factory;
            return &factory;
    }
}

}}
