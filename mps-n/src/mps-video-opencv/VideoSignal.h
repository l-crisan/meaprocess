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
#ifndef MPS_VIDEO_OPENCV_VIDEOSIGNAL_H
#define MPS_VIDEO_OPENCV_VIDEOSIGNAL_H

#include <mps/core/Signal.h>

namespace mps {
namespace video{
namespace opencv{

class VideoSignal : public mps::core::Signal
{
public:
    VideoSignal(Pt::uint32_t id);

    virtual ~VideoSignal();

    inline Pt::uint16_t deviceID() const
    {
        return _deviceID;
    }

    inline void setDeviceID(Pt::uint16_t id )
    {
        _deviceID = id;
    }	

    inline Pt::uint32_t width() const
    {
        return _width;
    }

    inline void setWidth(Pt::uint32_t w )
    {
        _width = w;
    }

    inline Pt::uint32_t height() const
    {
        return _height;
    }

    inline void setHeight(Pt::uint32_t h )
    {
        _height = h;
    }

private:
    Pt::uint16_t _deviceID;
    Pt::uint32_t _width;
    Pt::uint32_t _height;
};

}}}


#endif