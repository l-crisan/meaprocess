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
#ifndef MPS_CAN_DRV_DRIVER_H
#define MPS_CAN_DRV_DRIVER_H

#include <mps/can/drv/mps-candrv.h>
#include <mps/can/drv/Message.h>
#include <mps/can/drv/Exception.h>
#include <Pt/Connectable.h>
#include <Pt/Types.h>
#include <Pt/Signal.h>
#include <string>
#include <Pt/System/Mutex.h>
#include <Pt/System/Condition.h>
#include <Pt/SmartPtr.h>
#include <queue>

namespace mps{
namespace can{
namespace drv{

class DriverImpl;

class MPS_CAN_DRV_API Driver : public Pt::Connectable
{
public:
    virtual ~Driver();

    virtual bool open(const std::string& deviceFileOrID, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId = false) = 0;

    void addImpl(Pt::SmartPtr<DriverImpl> impl);

    Pt::SmartPtr<DriverImpl> getImpl(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port);

    virtual bool setAcceptanceMask(Pt::uint32_t mask, Pt::uint32_t code);

    virtual bool receive(Message& messageData);

    virtual void send(const Message& messageData);

    virtual bool wait(Pt::uint32_t timeout);

    virtual void reset(bool hardware = true);

    virtual void close();

    virtual std::string driverInfo() const = 0;

    virtual bool extendedID() const;

    virtual void wake();

    static bool extractDataFromCANMsg(Pt::uint8_t* extractedData, Message& messageData, Pt::uint8_t pivotBit, Pt::uint8_t bitCount, bool isSigned, bool isMotorola);

protected:
    Driver(const std::string& driverType);

private:
    int getDriverInfoIndex(const std::string& drvType, const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port);

    void onMessage(Message msg);

private:
    std::queue<Message>   _messages;
    Pt::System::Mutex     _mutex;
    Pt::System::Condition _condition;
    const Pt::uint32_t    _bufferSize; 
    Pt::uint32_t          _mask;
    Pt::uint32_t          _code;
    std::string           _driverType;

    struct DriverInfo
    {
        Pt::SmartPtr<DriverImpl> impl;
        std::string     driverType;
        Pt::uint32_t          refCounter;
    };

    Pt::SmartPtr<DriverImpl> _impl;

    static std::vector<DriverInfo>  _drivers;
    static void swapBytes(Pt::uint64_t& ddw);
};

}}}

#endif
