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
#include "DriverImpl.h"
#include "Wrapper.h"
#include <mps/can/drv/Exception.h>
#include <Pt/System/Clock.h>
#include <algorithm>
#include <Windows.h>
#include <pcan/PCANBasic.h>

namespace mps{
namespace can{
namespace peak{


DriverImpl::DriverImpl()
: _logger("mps.can.drv.DriverImpl")
, _extendedID(false)
, _running(false)
, _readThread(0)
, _bitRate(0)
, _deviceID("")
, _port(0)
{
    _canEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
}


DriverImpl::~DriverImpl()
{
    close();
    CloseHandle(_canEvent);
}


bool DriverImpl::open(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
    _deviceNo = deviceNo;

    if( port > 1 )
        return false;

    if(!Wrapper::loadDriver())
        return false;

    TPCANBaudrate brate = 0;
    _extendedID = extendedId;
    _bitRate = bitRate;
    _deviceID = deviceID;
    _port = port;
    _startTime = Pt::System::Clock::getSystemTicks().toUSecs() /10;

   _channelHandle = atoi(_deviceID.c_str());

    switch(bitRate)
    {
        case 1000000:
            brate = PCAN_BAUD_1M;
        break;

        case 666666:
            brate = 0x8027; //BTR0 = 0x80 BTR1 = 0x27;
        break;

        case 500000:
            brate = PCAN_BAUD_500K;
        break;

        case 250000:
            brate = PCAN_BAUD_250K;
        break;

        case 125000:
            brate = PCAN_BAUD_125K;
        break;

        case 100000:
            brate = PCAN_BAUD_100K;
        break;

        case 50000:
            brate = PCAN_BAUD_50K;
        break;

        case 20000:
            brate = PCAN_BAUD_20K;
        break;

        case 10000:
            brate = PCAN_BAUD_10K;
        break;

        case 5000:
            brate = PCAN_BAUD_5K;
        break;

        default:
        {
            return false;
        }
        break;
    }

    DWORD err = Wrapper::CAN_Initialize(_channelHandle, brate, 0, 0, 0); 

    if( err != PCAN_ERROR_OK)
    {
        return false;
    }

    err = Wrapper::CAN_SetValue(_channelHandle, PCAN_RECEIVE_EVENT, &_canEvent, sizeof(_canEvent));

    if( err != PCAN_ERROR_OK)
    {
        return false;
    }

    _running = true;
    _readThread = new Pt::System::AttachedThread(Pt::callable(*this, &DriverImpl::read));
    _readThread->start();

    return true;
}


bool DriverImpl::wait(Pt::uint32_t timeout)
{
    return (WaitForSingleObject(_canEvent, timeout) != WAIT_TIMEOUT);
}


void DriverImpl::send(const can::drv::Message& messageData)
{
    Pt::System::MutexLock lock(_mutex);

    TPCANMsg msg;
    msg.ID = messageData.identifier();

    if(_extendedID)
        msg.MSGTYPE = PCAN_MESSAGE_EXTENDED;
    else
        msg.MSGTYPE = PCAN_MESSAGE_STANDARD;

    msg.LEN = messageData.dlc();

    memcpy(&msg.DATA[0], messageData.data(), 8);

    DWORD error = Wrapper::CAN_Write(_channelHandle, &msg);

    if(error != PCAN_ERROR_OK)
    {
        std::stringstream ss;
        ss<<"Send message failed internal error: " <<error;
        throw can::drv::Exception(ss.str(), can::drv::Exception::SendError);
    }
}


void DriverImpl::read()
{
    can::drv::Message messageData;

    while(_running)
    {
        _mutex.lock();

        TPCANMsg msg;
        TPCANTimestamp CANTimeStamp;
        TPCANStatus status;

        status = Wrapper::CAN_Read(_channelHandle, &msg, &CANTimeStamp);

        _mutex.unlock();

        if( status != PCAN_ERROR_OK)
        {
            wait(100000);
            continue;
        }

        if( msg.MSGTYPE == PCAN_MESSAGE_STATUS )
            continue;

        //Total Microseconds = micros + 1000 * millis + 0xFFFFFFFF * 1000 * millis_overflow
        Pt::uint64_t micros10 = CANTimeStamp.micros;
        micros10 += (CANTimeStamp.millis * 1000);
        Pt::uint64_t overflow  = CANTimeStamp.millis_overflow;
        micros10 += (overflow <<32)* 1000;
        micros10 /= 10; // 10 microseconds resolution
        micros10 -= _startTime;//relative to start the driver or reset

        messageData.setTimeStamp(micros10);
        messageData.setIdentifier(msg.ID);
        messageData.setDlc(msg.LEN);
        memcpy(messageData.data(), msg.DATA, 8);
        onMessage(messageData);
    }
}


void DriverImpl::reset()
{
    Pt::System::MutexLock lock(_mutex);
    Wrapper::CAN_Reset(_channelHandle); 
    _startTime = Pt::System::Clock::getSystemTicks().toUSecs() /10;
}


void DriverImpl::close()
{
    if( _readThread != 0 )
    {
        _running = false; // Set the stop flag
    
        SetEvent(_canEvent); // Wake up the thread
    
        _readThread->join(); //Wait terminate
    
        delete _readThread;

        _readThread = 0;
        Wrapper::CAN_Uninitialize(_channelHandle);
    }
}


}}}
