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
#ifndef MPS_CANDRV_SOFTINGCANDRIVERIMPL_H
#define MPS_CANDRV_SOFTINGCANDRIVERIMPL_H

#include <windows.h>
#include <Can_def.h>
#include <CANL2.H>
#include <Pt/System/Logger.h>
#include <Pt/System/Thread.h>
#include <Pt/System/Mutex.h>
#include <Pt/Signal.h>
#include <mps/drv/can/Message.h>
#include <mps/drv/can/CANDriverImpl.h>

namespace mps{
namespace can{
namespace softing{

class SoftingCANDriverImpl : public mps::drv::can::CANDriverImpl
{
public:
    SoftingCANDriverImpl();
    ~SoftingCANDriverImpl();

    bool open(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId);

    void send(const mps::drv::can::Message& messageData);

    void reset();
    void close();

    bool extendedID() const
    {
        return _extendedID;
    }

    Pt::uint32_t bitRate() const
    {
        return _bitRate;
    }

    const std::string& deviceID() const
    {
        return _deviceID;
    }

    Pt::uint8_t port() const
    {
        return _port;
    }

    Pt::uint8_t deviceNo() const
    {
        return _deviceNo;
    }

private:
    bool wait(Pt::uint32_t timeout);
    void read();

private:
    Pt::System::Logger _logger;
    bool _running;
    bool _extendedID;
    HANDLE     _event; 
    L2CONFIG _l2config;
    PCHDSNAPSHOT _pbuffer;
    CANL2_CH_STRUCT _ch;
    Pt::uint8_t _port;
    std::string _deviceID;
    Pt::uint32_t _bitRate;
    CAN_HANDLE _portHandle;
    Pt::System::AttachedThread* _readThread;
    Pt::System::Mutex _mutex;
    Pt::uint8_t _deviceNo;
};

}}}
#endif
