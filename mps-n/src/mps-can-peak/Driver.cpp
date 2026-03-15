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
#include "Driver.h"
#include "DriverImpl.h"

namespace mps{
namespace can{
namespace peak{


Driver::Driver()
:can::drv::Driver("mps-can-peak")
{
}


Driver::~Driver()
{
}


bool Driver::open(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedID)
{
    const can::drv::DriverImpl*  null = 0;

    Pt::SmartPtr<can::drv::DriverImpl> impl = getImpl(deviceID, deviceNo, port);
    
    if( impl == null )
    {
        impl.reset(new DriverImpl());

        if(!impl->open(deviceID, deviceNo, port, bitRate, extendedID))
            return false;

        addImpl(impl);
    }
    else
    {
        if( impl->bitRate() != bitRate || impl->extendedID() != extendedID || deviceNo != impl->deviceNo())
            return false;

        addImpl(impl);
    }

    return true;
}


}}}
