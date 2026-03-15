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
#ifndef MPS_GPS_GPSPS_H
#define MPS_GPS_GPSPS_H

#include <mps/core/SynchSourcePS.h>
#include <Pt/System/IOStream.h>
#include <Pt/System/SerialDevice.h>
#include <Pt/System/Thread.h>
#include "NMEAParser.h"
#include <Pt/Connectable.h>
#include <Pt/System/Mutex.h>
#include <Pt/System/MainLoop.h>


namespace mps{
namespace gps{

class GpsPS : public mps::core::SynchSourcePS, public Pt::Connectable
{
public:
    GpsPS(void);

    virtual ~GpsPS(void);

    const std::string& comPort() const;

    void setComPort(const std::string& p);

    const std::string& rate() const;

    void setRate(const std::string& r);

    virtual void onInitialize();

    virtual void onDeinitialize();

    virtual void onStart();

    virtual void onStop();

protected:
    virtual void onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data);

private:
    void run();

    void onGpsData();

    void onDataAvailable(Pt::System::IODevice& device);

    std::string					_comPort;
    std::string					_rate;
    Pt::System::SerialDevice	_device;
    bool						_errorState;
    Pt::System::AttachedThread* _runThread;
    std::vector<char>			_nmeaData;
    NMEAParser					_nmeaParser;
    std::vector<std::vector<Pt::uint8_t> > _signalData;
    std::vector<Pt::uint8_t>	_recordData;
    Pt::System::MainLoop        _loop;
};

}}

#endif
