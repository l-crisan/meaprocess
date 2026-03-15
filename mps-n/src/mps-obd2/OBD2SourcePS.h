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
#ifndef MPS_OBD2_OBD2SOURCEPS_H
#define MPS_OBD2_OBD2SOURCEPS_H

#include <Pt/System/Library.h>
#include <mps/core/SynchSourcePS.h>
#include <Pt/System/Thread.h>
#include <mps/can/drv/Driver.h>
#include <mps/can/drv/Factory.h>
#include <mps/diatp/CANHandler.h>
#include <mps/diatp/TPCANHandler.h>
#include <Pt/System/SerialDevice.h>
#include "OBD2Signal.h"
#include "OBD2VehicleInfo.h"
#include <mps/core/ObjectVector.h>
#include <Pt/System/Library.h>

#include <vector>

namespace mps{
namespace obd2{

class OBD2SourcePS : public mps::core::SynchSourcePS
{
public:
    OBD2SourcePS(void);

    virtual ~OBD2SourcePS(void);

    virtual void onInitialize();

    virtual void onDeinitialize();

    virtual void onStart();

    virtual void onStop();

    virtual void onInitInstance();

    virtual void onExitInstance();

    inline const std::string& driver() const
    {
        return _driverType;
    }

    inline void setDriver(const std::string&  d)
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

    inline Pt::uint32_t port() const
    {
        return _port;
    }

    inline void setPort(Pt::uint32_t p)
    {
        _port = p;
    }

    inline Pt::uint8_t addressMode() const
    {
        return _addressMode;
    }

    inline void setAddressMode(Pt::uint8_t m)
    {
        _addressMode = m;
    }

    inline Pt::uint32_t canRate() const
    {
        return _canRate;
    }

    inline void setCanRate(Pt::uint32_t r)
    {
        _canRate = r;
    }

    inline Pt::uint32_t serRate() const
    {
        return _serRate;
    }

    inline void setSerRate(Pt::uint32_t r)
    {
        _serRate = r;
    }

    void addObject(Object* object, const std::string& type, const std::string& subType);

protected:
    virtual void onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data);

private:
    enum AddressMode
    {
        Normal11Bit,
        Normal29Bit,
        Normal
    };

    enum DriverType
    {
        Vector = 0,
        Peak,
        Softing,
        ELM327,
        KLine,
        Kvaser,
        SocketCAN
    };

    class LibraryInfo
    {
        public:
            LibraryInfo()
            : opened(false)
            { }

            Pt::System::Library lib;
            bool opened;
    };

private:
    void run();

    void readDTC();

    void checkSupportedPids();

    void readVehicleInfos();

    mps::can::drv::Driver* createCANDriver();

    std::string getStringFromResponse(const std::vector<Pt::uint8_t>& res);

    bool isSignalInSequence(Pt::uint8_t sid, Pt::uint8_t pid, Pt::int8_t sensor, const std::vector<std::vector<Pt::uint8_t> >& sequense, const std::vector<Pt::uint8_t>& currentRequest);

    std::vector<OBD2Signal*> getSignalsFromRequest(Pt::uint32_t sid, Pt::uint32_t pid, Pt::int8_t sensor = -1);

    const mps::diatp::TPDUAddress& getAddress();

private:    
    std::vector<std::vector<Pt::uint8_t> > _dataRecords;
    bool _running;
    std::string _driverType;
    Pt::uint32_t _port;
    Pt::uint8_t  _addressMode;
    Pt::uint32_t _canRate;
    Pt::uint32_t _serRate;
    Pt::System::AttachedThread* _runThread;
    mps::can::drv::Driver*  _canDriver;
    mps::diatp::CANHandler*  _canHandler;
    Pt::System::SerialDevice*  _serDevice;
    mps::diatp::TPHandler*   _tpHandler;
    bool _errorState;
    mps::core::ObjectVector<OBD2VehicleInfo*>* _vehicleInfos;
    
    //DTC handling
    Pt::uint32_t  _noOfEcus;

    //Actual DTCs
    Pt::uint32_t  _dtcSrcIdx;	
    std::vector<Pt::uint16_t> _dtcs;	
    const OBD2Signal* _dtcSignal;
    
    //DTC detected during current or last driving cycle
    Pt::uint32_t  _dtcDDCSrcIdx;
    std::vector<Pt::uint16_t> _dtcsDDC;
    const OBD2Signal* _dtcDDCSignal;
    
    Pt::System::Mutex _dtcMutex;
    mps::diatp::TPDUAddress _normalFixedFunctional;
    std::string _device;
    
    Pt::System::Library      _canDriverLib;
    mps::can::drv::Factory*  _canDrvFactory;
    Pt::uint8_t _deviceNo;
};

}}

#endif
