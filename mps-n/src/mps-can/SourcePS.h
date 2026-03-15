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
#ifndef MPS_CAN_SOURCEPS_H
#define MPS_CAN_SOURCEPS_H

#include <mps/can/drv/Message.h>
#include <mps/can/drv/Driver.h>
#include <mps/core/FiFoSynchSourcePS.h>
#include <Pt/System/Library.h>
#include <Pt/System/Thread.h>

namespace mps{
namespace can{

class Signal;

class SourcePS : public core::FiFoSynchSourcePS
{
public:
    SourcePS();

    virtual ~SourcePS();

    void onInitialize();

    void onStart();

    void onStop();

    void onInitInstance();

    void onExitInstance();

    void onDeinitialize();

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
    void onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data);

    void onReadData();

private:
    can::drv::Driver* createDriver();

    bool getSignalValue(Pt::uint8_t* data, const Signal* signal, can::drv::Message& msg);

    void scan();

private:
    typedef std::map<Pt::uint32_t,std::vector<const Signal*> >::iterator Id2SignalsIt;
    typedef std::vector<std::vector<std::vector<Pt::uint8_t> > > SourceData;

    std::vector<std::map<Pt::uint32_t,std::vector<const Signal*> > > _id2Signals;
    SourceData _sourceData;
    std::string _driver;
    std::string _device;
    bool        _running;
    Pt::System::AttachedThread* _scanThread;
    bool		_errorState;
    can::drv::Driver*  _drivers[4];  
    Pt::uint8_t _deviceNo;
};

}}

#endif
