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
#ifndef MPS_CORE_APIDEF_H
#define MPS_CORE_APIDEF_H

#include <Pt/Types.h>


extern "C"
{

#pragma pack(push) 
#pragma pack(1)
 /** @brief Message info structure.*/
struct mpsMessage
{
    char text[200];
    char comment[200];
    char fileName[200];
    Pt::int32_t	 target;
    Pt::int32_t  type;
    Pt::int64_t  timeStamp;	
    Pt::uint32_t  errorCode;
};

#pragma pack(pop) 

#if defined(_WIN32)


/** @brief On data call back.*/
typedef void (__stdcall *mpsOnData)(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portNo, Pt::uint32_t dataSize,  const Pt::uint8_t* data );

/** @brief On data read signal data callback.*/
typedef void (__stdcall *mpsOnGetSignalData)(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t sigInx, Pt::uint32_t dataSize, Pt::uint8_t* data);

/** @brief On message callback.*/
typedef int (__stdcall *mpsOnMessage)(Pt::uint64_t objID, mpsMessage& message);



#else 

typedef void (*mpsOnData)(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portNo, Pt::uint32_t dataSize,  const Pt::uint8_t* data );
typedef void (*mpsOnGetSignalData)(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t sigInx, Pt::uint32_t dataSize, Pt::uint8_t* data);
typedef int (*mpsOnMessage)(Pt::uint64_t objID, mpsMessage message);


#endif


}
#endif

