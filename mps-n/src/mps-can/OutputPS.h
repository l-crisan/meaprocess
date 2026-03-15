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
#ifndef MPS_CAN_OUTPUTPS_H
#define MPS_CAN_OUTPUTPS_H

#include "OutChannel.h"
#include <mps/core/ProcessStation.h>
#include <mps/core/CircularBuffer.h>
#include <mps/core/ObjectVector.h>
#include <mps/core/RecordBuilder.h>
#include <mps/core/Signal.h>
#include <mps/can/drv/Driver.h>
#include <Pt/System/Mutex.h>
#include <map>
#include <vector>

namespace mps {
namespace can{

class OutputPS : public mps::core::ProcessStation
{
public:
    OutputPS(void);

    virtual ~OutputPS(void);

    void onInitInstance();

    void onExitInstance();

    void onInitialize();

    void onStart();

    void onStop();

    void onDeinitialize();

    void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);
    
    virtual PSType psType() const
    { return ReceptorPS; }

    void addObject(Object* object, const std::string& type, const std::string& name );

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
private:
    void writeValueToBuffer(const OutChannel* chn, mps::core::RecordBuilder& outBuffer, double value);

    can::drv::Driver* createDriver();

    struct ChannelInfo
    {
        OutChannel* chn;
        double		   factor;
        double		   offset;
    };

    static double getFactor(OutChannel* chn);
    static double getOffset(OutChannel* chn);
    static void scaleDataToOutput(ChannelInfo& info, double value, std::vector<Pt::uint8_t>& data);    
    static Pt::uint32_t getChannelOffsetInMsg(const OutChannel* chn);
    double getSignalRate(Pt::uint32_t id);

private:
    bool _errorState;
    std::string _driverType;
    std::string _device;
    Pt::uint8_t _port;
    Pt::uint32_t _bitrate;
    Pt::uint8_t _adrMode;
    mps::core::ObjectVector<OutChannel*>* _channels;
    std::map<Pt::uint32_t,mps::core::RecordBuilder> _messageOutBuffer;
    std::map<Pt::uint32_t, std::vector<OutChannel*> >  _sig2chn;
    std::map<Pt::uint32_t, std::vector<ChannelInfo> > _msg2chn;
    can::drv::Driver* _driver;
    Pt::uint8_t _deviceNo;
};

}}
#endif
