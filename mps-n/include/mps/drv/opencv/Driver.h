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
#ifndef MPS_DRV_OPENCV_API_H
#define MPS_DRV_OPENCV_API_H

#include <Pt/Api.h>
#include <Pt/Types.h>

#if defined(MPS_OPENCV_EXPORTS)
#  define MPS_OPEN_API PT_EXPORT
#else
#  define MPS_OPEN_API PT_IMPORT
#endif


extern "C"
{

struct mpsopencv_DeviceInfo
{
    int CardID;
    int Width;
    int Height;
    int Rate;
    int Error;
};

MPS_OPEN_API Pt::uint64_t mpsopencv_open(int card);

MPS_OPEN_API void mpsopencv_setFrameRate(Pt::uint64_t handle, int rate);

MPS_OPEN_API int mpsopencv_readFrame(Pt::uint64_t handle, char* buffer);

MPS_OPEN_API void mpsopencv_close(Pt::uint64_t handle);

MPS_OPEN_API mpsopencv_DeviceInfo mpsopencv_detect(int card);

}
#endif
