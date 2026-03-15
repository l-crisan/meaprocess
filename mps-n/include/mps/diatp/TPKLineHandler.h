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
#ifndef MPS_DIATP_TPKLINEHANDLER_H
#define MPS_DIATP_TPKLINEHANDLER_H

#include <mps/can/drv/Message.h>
#include <mps/can/drv/Driver.h>
#include <mps/diatp/CANHandler.h>
#include <mps/diatp/TPDUAddress.h>
#include <mps/diatp/TPHandler.h>
#include <mps/diatp/TPDU.h>
#include <mps/diatp/TPDUAddress.h>
#include <Pt/System/Thread.h>
#include <Pt/System/Condition.h>
#include <Pt/System/SerialDevice.h>
#include <Pt/System/Logger.h>
#include <Pt/System/MainLoop.h>
#include <vector>

namespace mps{
namespace diatp{

class MPS_DIA_API TPKLineHandler : public TPHandler
{
public:
    TPKLineHandler(Pt::System::SerialDevice& serDevice);

    virtual ~TPKLineHandler();

    virtual void send(const TPDU& requestPdu);

    virtual bool waitForSID(Pt::uint8_t sid, size_t timeout);

    virtual bool readSID(Pt::uint8_t sid, TPDU& pdu);

    virtual void reset(bool hardware = true);

    virtual void start();

    virtual void stop();

    bool startCommunication(const TPDUAddress& address);

    bool stopCommunication(const TPDUAddress& address);

private:
    void run();

    void signalDataAvailable(Pt::uint8_t sid);

    void onDataAvailable(Pt::System::IODevice& device);

private:
    bool _running;
    Pt::System::SerialDevice&   _serDevice;
    std::vector<TPDU>			_pdus;
    Pt::System::AttachedThread*	_thread;
    Pt::System::Mutex			_mutex;
    std::map<Pt::uint8_t,Pt::System::Condition*> _sidsToWait;
    Pt::uint8_t _key1;
    Pt::uint8_t _key2;
    Pt::System::MainLoop _loop;
    std::vector<Pt::uint8_t> _buffer;

    bool _al0;
    bool _al1;
    bool _hb0;
    bool _hb1;
};

}}
#endif
