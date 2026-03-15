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
#include "Wrapper.h"

#include <Pt/System/Library.h>

namespace mps{
namespace can{
namespace peak{

TCAN_Initialize     Wrapper::_CAN_Initialize = 0;
TCAN_Uninitialize   Wrapper::_CAN_Uninitialize = 0;
TCAN_Read           Wrapper::_CAN_Read = 0;
TCAN_Write          Wrapper::_CAN_Write = 0;
TCAN_SetValue       Wrapper::_CAN_SetValue = 0;
TCAN_Reset          Wrapper::_CAN_Reset = 0;
Pt::System::Library Wrapper::_library;
bool                Wrapper::_isopen = false;


bool Wrapper::loadDriver()
{
    if(_isopen)
        return true;

    try
    {
        _library.open(Pt::System::Path("PCANBasic.dll"));

        _isopen = true;

        _CAN_Initialize     = (TCAN_Initialize)   _library.resolve("CAN_Initialize");
        _CAN_Uninitialize   = (TCAN_Uninitialize) _library.resolve("CAN_Uninitialize");
        _CAN_Read           = (TCAN_Read)         _library.resolve("CAN_Read");
        _CAN_Write          = (TCAN_Write)        _library.resolve("CAN_Write");
        _CAN_SetValue       = (TCAN_SetValue)     _library.resolve("CAN_SetValue");
        _CAN_Reset          = (TCAN_Reset)        _library.resolve("CAN_Reset");
    }
    catch(...)
    {
        return false;
    }

    return true;
}


Pt::uint32_t Wrapper::CAN_Initialize(Pt::uint8_t Channel, Pt::uint16_t Btr0Btr1, Pt::uint8_t HwType, Pt::uint32_t IOPort, Pt::uint16_t Interrupt)
{
    return _CAN_Initialize(Channel, Btr0Btr1, HwType, IOPort, Interrupt);
}


Pt::uint32_t Wrapper::CAN_Uninitialize(Pt::uint8_t Channel)
{
    return _CAN_Uninitialize(Channel);
}


Pt::uint32_t Wrapper::CAN_Read(Pt::uint8_t Channel, TPCANMsg* MessageBuffer, TPCANTimestamp* TimestampBuffer)
{
    return _CAN_Read(Channel, MessageBuffer, TimestampBuffer);
}


Pt::uint32_t Wrapper::CAN_Write(Pt::uint8_t Channel, TPCANMsg* MessageBuffer)
{
    return _CAN_Write(Channel, MessageBuffer);
}


Pt::uint32_t Wrapper::CAN_SetValue(Pt::uint8_t Channel, Pt::uint8_t Parameter, void* Buffer, Pt::uint32_t BufferLength)
{
    return _CAN_SetValue( Channel, Parameter, Buffer, BufferLength);
}


Pt::uint32_t Wrapper::CAN_Reset(Pt::uint8_t Channel)
{
    return _CAN_Reset(Channel);
}

}}}
