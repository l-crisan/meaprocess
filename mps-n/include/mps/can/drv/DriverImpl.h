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
#ifndef MPS_CAN_DRV_RIVERIMPL_H
#define MPS_CAN_DRV_RIVERIMPL_H

#include <mps/can/drv/mps-candrv.h>
#include <mps/can/drv/Message.h>
#include <Pt/Types.h>
#include <Pt/Signal.h>
#include <string>

namespace mps{
namespace can{
namespace drv{

class MPS_CAN_DRV_API DriverImpl
{

public:
    virtual bool open(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId) = 0;

    virtual void send(const Message& messageData) = 0;

    virtual void reset() = 0;

    virtual void close() = 0;

    virtual bool extendedID() const = 0;

    virtual Pt::uint32_t bitRate() const = 0;

    virtual const std::string& deviceID() const = 0;
    
    virtual Pt::uint8_t port() const = 0;

    virtual Pt::uint8_t deviceNo() const = 0;

    Pt::Signal<Message> onMessage;

    virtual ~DriverImpl();

protected:
    DriverImpl();

};

}}}

#endif
