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
#ifndef MPS_CAN_LOGGERPS_H
#define MPS_CAN_LOGGERPS_H

#include <mps/core/FiFoSynchSourcePS.h>
#include <mps/can/drv/Message.h>
#include <mps/can/drv/Driver.h>
#include <Pt/System/Thread.h>

namespace mps{
namespace can{

class LoggerPS : public mps::core::SynchSourcePS
{
public:
    LoggerPS(void);

    virtual ~LoggerPS(void);

    virtual void onInitialize();

    virtual void onDeinitialize();

    virtual void onStart();

    virtual void onStop();

    virtual void onInitInstance();

    virtual void onExitInstance();

    inline const std::string& driver() const
    {
        return _driver;
    }

    inline void setDriver(const std::string& d)
    {
        _driver = d;
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

protected:
    virtual void onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data);

private:
    can::drv::Driver* createDriver();
    std::string _driver;
    std::string _device;
    can::drv::Driver*  _drivers[4];
    bool _errorState;
    Pt::uint8_t _deviceNo;
};

}}

#endif
