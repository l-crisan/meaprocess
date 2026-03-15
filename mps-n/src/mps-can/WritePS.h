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
#ifndef MPS_CAN_WRITEPS_H
#define MPS_CAN_WRITEPS_H

#include "OutChannel.h"
#include <mps/can/drv/Driver.h>
#include <mps/core/ProcessStation.h>
#include <mps/core/CircularBuffer.h>
#include <mps/core/ObjectVector.h>
#include <mps/core/RecordBuilder.h>
#include <mps/core/Signal.h>
#include <Pt/Types.h>
#include <map>
#include <vector>

namespace mps {
namespace can{

class WritePS : public core::ProcessStation
{
public:
    WritePS(void);

    virtual ~WritePS(void);

    virtual void onInitInstance();

    virtual void onExitInstance();

    virtual void onInitialize();

    virtual void onStart();

    virtual void onDeinitialize();

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);
    
    virtual PSType psType() const
    { return ReceptorPS; }

    inline const std::string& driver() const
    {
        return _driverType;
    }

    inline void setDriver(const std::string& d)
    {
        _driverType = d;
    }
    
    inline const std::string& device() const
    {
        return _device;
    }

    inline void setDevice(const std::string& d)
    {
        _device = d;
    }

    inline Pt::uint8_t deviceNo() const
    {
        return _deviceNo;
    }

    inline void setDeviceNo(Pt::uint8_t n)
    {
        _deviceNo = n;
    }

    inline Pt::uint8_t port() const
    {
        return _port;
    }

    inline void setPort(Pt::uint8_t p)
    {
        _port = p;
    }

    inline Pt::uint32_t bitrate() const
    {
        return _bitrate;
    }

    inline void setBitrate(Pt::uint32_t b)
    {
        _bitrate = b;
    }

    inline Pt::uint8_t adrMode() const
    {
        return _adrMode;
    }
    
    inline void setAdrMode(Pt::uint8_t m)
    {
        _adrMode = m;
    }

    inline const std::string& signalTypeMap() const
    {
        return _signalTypeMap;
    }
    
    inline void setSignalTypeMap(const std::string& t)
    {
        _signalTypeMap = t;
    }

private:
    Pt::uint64_t getSignalValue(Pt::uint32_t sourceSize, Pt::uint32_t signalOffset, Pt::uint32_t record, const Pt::uint8_t* data);

private:
    struct ChannelInfo
    {
        OutChannel* chn;
        double		   factor;
        double		   offset;
    };

    bool _errorState;
    std::string _driverType;
    std::string _device;
    Pt::uint8_t _port;
    Pt::uint32_t _bitrate;
    Pt::uint8_t _adrMode;
    std::string _signalTypeMap;
    can::drv::Driver* _driver;
    Pt::uint8_t _deviceNo;	
    can::drv::Message _messageToSend;
    can::drv::Message _lastMessage;
    mps::core::Signal* _idSignal;
    mps::core::Signal* _timeSignal;
    mps::core::Signal* _dlcSignal;
    mps::core::Signal* _dataSignal;
};

}}
#endif
