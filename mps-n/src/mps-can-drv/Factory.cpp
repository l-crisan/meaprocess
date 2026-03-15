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
#include <mps/can/drv/Factory.h>
#include <mps/can/drv/IFactory.h>

namespace mps{
namespace can{
namespace drv{

typedef IFactory* (*GetFactoryType)();

std::vector<Factory::DriverInfo> Factory::_drivers;


Factory::Factory(void)
{
}


Factory::~Factory(void)
{
}


Driver* Factory::createDriver(const std::string& driver)
{
    Pt::System::Library* lib = 0;

    for( Pt::uint32_t i = 0; i < _drivers.size(); ++i)
    {
        if(_drivers[i].driver == driver)
        {
            lib = &_drivers[i].library;
            break;
        }
    }

    if( lib == 0)
    {
        try
        {
            DriverInfo drvInfo;
            drvInfo.driver = driver;
            drvInfo.library.open(Pt::System::Path(driver.c_str()));
            _drivers.push_back(drvInfo);
            lib = &_drivers[_drivers.size() -1].library;
        }
        catch(const std::exception& ex)
        {
            std::cerr<<ex.what()<<std::endl;
            return 0;
        }
    }

    GetFactoryType getFactory = (GetFactoryType) lib->resolve("mpsGetFactory");
    IFactory* canDrvFactory = getFactory();
    return canDrvFactory->createDriver();
}


}}}
