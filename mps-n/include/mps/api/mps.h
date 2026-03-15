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

#ifndef MPS_API_H
#define MPS_API_H

#include <Pt/Api.h>
#include <Pt/Types.h>
#include <mps/core/ApiDef.h>

/** @namespace mps
    @brief MeaProcess runtime engine.

    This module is the base module for all other modules and has no dependency
    to any system specific libraries except the standard C++ library and Platinum portability framework. 
    It provides the basic functionality for creating an own MeaProcess application.
*/

#if defined(MPS_API_EXPORTS)
#  define MPS_API PT_EXPORT
#else
#  define MPS_API PT_IMPORT
#endif

#ifndef DOXYGEN_SHOULD_SKIP_THIS

extern "C"
{
    /** @brief Load the runtime from xml representation .*/	
    MPS_API Pt::uint64_t  mpsLoadFromXML(const char* xmlData, mpsMessage& msg);
    MPS_API void  mpsSetExecDirectory(Pt::uint64_t handle, const char* path);
    MPS_API void  mpsSetProperty(Pt::uint64_t handle, const char* name, const char* value);
    MPS_API void  mpsSetLanguage(Pt::uint64_t handle, const char* code);
    MPS_API void  mpsGetProperty(Pt::uint64_t handle, const char* name, char** value);
    MPS_API Pt::uint8_t  mpsLoadModule(const char* file);
    MPS_API void  mpsUnloadAllModules();
    MPS_API void  mpsInitialize(Pt::uint64_t handle);
    MPS_API void  mpsStart(Pt::uint64_t handle);
    MPS_API void  mpsStop(Pt::uint64_t handle);
    MPS_API void  mpsDeinitilize(Pt::uint64_t handle);
    MPS_API void  mpsAddDataListener(Pt::uint64_t handle, Pt::uint32_t objId, Pt::uint32_t psID, mpsOnData callBack);	
    MPS_API void  mpsAddDataSource(Pt::uint64_t handle, Pt::uint32_t objId, Pt::uint32_t psID, mpsOnGetSignalData callBack);
    MPS_API void  mpsSetMessageListener(Pt::uint64_t handle, Pt::uint64_t objId, mpsOnMessage callBack);
    MPS_API void  mpsReleaseRuntime(Pt::uint64_t handle);
    MPS_API void  mpsSetLogFile(const char* path, Pt::int32_t level);
}
#endif //DOXYGEN_SHOULD_SKIP_THIS

#endif

