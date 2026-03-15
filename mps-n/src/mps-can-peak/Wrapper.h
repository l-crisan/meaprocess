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
#ifndef MPS_CAN_PEAKCANDRVWRAPPER_H
#define MPS_CAN_PEAKCANDRVWRAPPER_H

#include <Pt/Types.h>
#include <Pt/System/Library.h>
#include <Windows.h>
#include <pcan/PCANBasic.h>

namespace mps{
namespace can{
namespace peak{


typedef Pt::uint32_t (__stdcall *TCAN_Initialize)(Pt::uint8_t Channel, Pt::uint16_t Btr0Btr1, Pt::uint8_t HwType, Pt::uint16_t IOPort, Pt::uint16_t Interrupt);
typedef Pt::uint32_t (__stdcall *TCAN_Uninitialize)(Pt::uint8_t Channel);
typedef Pt::uint32_t(__stdcall *TCAN_Read)(Pt::uint8_t Channel, TPCANMsg* MessageBuffer, TPCANTimestamp* TimestampBuffer);
typedef Pt::uint32_t(__stdcall *TCAN_Write)(Pt::uint8_t Channel, TPCANMsg* MessageBuffer);
typedef Pt::uint32_t (__stdcall *TCAN_SetValue)(Pt::uint8_t Channel, Pt::uint8_t Parameter, void* Buffer, Pt::uint32_t BufferLength);
typedef Pt::uint32_t (__stdcall *TCAN_Reset)(Pt::uint8_t Channel);

class Wrapper
{
public:
    static bool loadDriver();

    static Pt::uint32_t CAN_Initialize( Pt::uint8_t Channel, Pt::uint16_t Btr0Btr1, Pt::uint8_t HwType, Pt::uint32_t IOPort, Pt::uint16_t Interrupt);

    static Pt::uint32_t CAN_Uninitialize(Pt::uint8_t Channel);

    static Pt::uint32_t CAN_Read(Pt::uint8_t Channel, TPCANMsg* MessageBuffer, TPCANTimestamp* TimestampBuffer);

    static Pt::uint32_t CAN_Write(Pt::uint8_t Channel, TPCANMsg* MessageBuffer);

    static Pt::uint32_t CAN_SetValue(Pt::uint8_t Channel, Pt::uint8_t Parameter, void* Buffer, Pt::uint32_t BufferLength);
        
    static Pt::uint32_t CAN_Reset(Pt::uint8_t Channel);

private:
    static TCAN_Initialize _CAN_Initialize;
    static TCAN_Uninitialize _CAN_Uninitialize;
    static TCAN_Read _CAN_Read;
    static TCAN_Write _CAN_Write;
    static TCAN_SetValue _CAN_SetValue;
    static TCAN_Reset _CAN_Reset;
    static Pt::System::Library _library;
    static bool _isopen;
};

}}}

#endif
