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
#ifndef MPS_AUDIO_OUTCHANNEL_H
#define MPS_AUDIO_OUTCHANNEL_H

#include <mps/core/Object.h>
#include <Pt/Types.h>

namespace mps {
namespace audio{

class OutChannel : public mps::core::Object
{
public:
    OutChannel();
    virtual ~OutChannel();

    inline Pt::uint32_t signalID() const
    {
        return _signal;
    }

    inline void setSignalID(Pt::uint32_t id)
    {
        _signal = id;
    }

    inline Pt::uint16_t deviceID() const
    {
        return _deviceID;
    }

    inline void setDeviceID(Pt::uint16_t id)
    {
        _deviceID = id;
    }

    inline Pt::uint16_t channelID() const
    {
        return _channelID;
    }

    inline void setChannelID(Pt::uint16_t id)
    {
        _channelID = id;
    }

    inline Pt::uint32_t sampleRate() const
    {
        return _sampleRate;
    }

    inline void setSampleRate(Pt::uint32_t id)
    {
        _sampleRate = id;
    }

    inline Pt::uint8_t dataType() const
    {
        return _dataType;
    }
    
    inline void setDataType(Pt::uint8_t d)
    {
        _dataType = d;
    }

private:
    Pt::uint32_t _signal;
    Pt::uint16_t _deviceID;
    Pt::uint16_t _channelID;
    Pt::uint8_t  _dataType;
    Pt::uint32_t _sampleRate;
};

}}

#endif
