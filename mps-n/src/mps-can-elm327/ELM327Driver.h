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
#ifndef MPS_CANDRV_ELM327DRIVER_H
#define MPS_CANDRV_ELM327DRIVER_H

#include <Pt/System/Logger.h>
#include <Pt/System/SerialDevice.h>
#include <mps/drv/can/CANDriver.h>
#include <mps/drv/can/Message.h>
#include <queue>

namespace mps{
namespace can{
namespace elm327{

class ELM327Driver : public mps::drv::can::CANDriver
{
public:
    ELM327Driver();
    ~ELM327Driver();

    virtual bool open(const std::string& device, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId = false);

    virtual void send(const mps::drv::can::Message& MessageData);

    virtual bool receive(mps::drv::can::Message& MessageData);

    virtual bool wait(Pt::uint32_t timeout);

    virtual bool setAcceptanceMask( Pt::uint32_t mask, Pt::uint32_t code);

    virtual bool extendedID() const;

    virtual void reset();

    virtual void close();

    virtual void wake();

    virtual std::string driverInfo() const;

private:
    void readDirect(Pt::uint32_t timeout);
    
    bool writeCmd(const std::string& cmd);

    Pt::uint32_t hex2no(std::string str);

    std::string no2hex(Pt::uint32_t no);

    std::string byte2hex(Pt::uint8_t b);

private:
    Pt::System::SerialDevice _device;
    Pt::System::Logger _logger;
    std::queue<mps::drv::can::Message>  _receiveQueue;
    bool _extendedID;
    Pt::uint32_t _lastID;
    char _buffer[1024];

};

}}}
#endif
