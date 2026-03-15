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
#ifndef MPS_CAN_EVENTPS_H
#define MPS_CAN_EVENTPS_H

#include "Event.h"
#include <mps/core/ProcessStation.h>
#include <mps/core/ObjectVector.h>
#include <mps/can/drv/Driver.h>
#include <Pt/Types.h>
#include <map>

namespace mps{
namespace can{

class EventPS : public mps::core::ProcessStation
{
public:

    EventPS(void);

    virtual ~EventPS();

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

private:
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
        return _deviecNo;
    }

    inline void setDeviceNo(Pt::uint8_t n)
    {
        _deviecNo = n;
    }

    inline Pt::uint8_t port() const
    {
        return _port;
    }

    inline void setPort(Pt::uint8_t port)
    {
        _port = port;
    }

    inline Pt::uint32_t bitrate() const
    {
        return _bitrate;
    }

    inline void setBitrate(Pt::uint32_t r)
    {
        _bitrate = r;
    }

    inline Pt::uint8_t adrMode() const
    {
        return _adrMode;
    }

    inline void setAdrMode(Pt::uint8_t m)
    {
        _adrMode = m;
    }

    bool fireEvents(std::vector<Event*>& events, double value);

    void resetLastData();

    can::drv::Driver* createDriver();

private:
    typedef std::map<Pt::uint32_t, std::vector<Event*> >::iterator Sig2EventIt;

    mps::core::ObjectVector<Event*>* _events;
    std::map<Pt::uint32_t, std::vector<Event*> > _sig2Event;
    std::vector<std::vector<double> > _lastData;
    std::vector<std::vector<bool> > _firstSample;
    std::string _driverType;
    std::string _device;
    Pt::uint8_t  _port;
    Pt::uint32_t _bitrate;
    Pt::uint8_t _adrMode;
    can::drv::Driver* _driver;
    Pt::uint8_t _deviecNo;
    bool _errorState;
};

}}

#endif
