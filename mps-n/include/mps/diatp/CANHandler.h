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
#ifndef MPS_DIATP_CANHANDLER_H
#define MPS_DIATP_CANHANDLER_H

#include <Pt/Connectable.h>
#include <mps/can/drv/Message.h>
#include <mps/can/drv/Driver.h>
#include <Pt/System/Thread.h>
#include <Pt/System/Condition.h>
#include <mps/diatp/TPDUAddress.h>

#include <vector>

namespace mps{
namespace diatp{

class MPS_DIA_API CANHandler : public Pt::Connectable
{
public:
    CANHandler(mps::can::drv::Driver& driver,  Pt::uint8_t blockSize = 0, Pt::uint32_t separationTime = 34, TimeUnit::Unit timeUnit = TimeUnit::MilliSec);
    
    virtual ~CANHandler();

    void send(mps::can::drv::Message& msg);
    
    bool readMessageByID(Pt::uint32_t id, mps::can::drv::Message& msg);

    bool readMessage(mps::can::drv::Message& msg);

    bool wait(size_t timeout);

    bool waitForID(Pt::uint32_t id, size_t timeout);

    void reset(bool hardware = true);

    mps::can::drv::Driver& driver();

    Pt::uint32_t separationTime() const;

    Pt::uint8_t blockSize() const ;

    TimeUnit::Unit timeUnit() const;

    void start();

    void stop();

    void restart();

    void wake();

private:
    void run();

private:
    bool _running;
    Pt::uint32_t    _separationTime;
    Pt::uint8_t     _blockSize;
    TimeUnit::Unit  _timeUnit;

    mps::can::drv::Driver& _driver;
    std::vector<mps::can::drv::Message> _messages;
    Pt::System::Condition  _dataAvailCondition;

    Pt::System::AttachedThread*	_thread;	
    Pt::System::Mutex _mutex;
    std::map<Pt::uint32_t,Pt::System::Condition*> _waitForIDMap;
};

}}
#endif
