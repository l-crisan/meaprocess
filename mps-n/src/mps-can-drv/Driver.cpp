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
#include <mps/can/drv/Driver.h>
#include <mps/can/drv/DriverImpl.h>
#include <memory.h>
#include <Pt/Byteorder.h>
#include <Pt/SourceInfo.h>

using namespace std;

namespace mps{
namespace can{
namespace drv{

std::vector<Driver::DriverInfo>  Driver::_drivers;

Driver::Driver(const std::string& drvType)
: _messages()
, _mutex()
, _condition()
, _bufferSize(1024*10)
, _mask(0)
, _code(0)
, _driverType(drvType)
, _impl(0)
{
}


Driver::~Driver(void)
{
    close();
}


bool Driver::extractDataFromCANMsg(Pt::uint8_t* extractedData, Message& messageData, Pt::uint8_t pivotBit, Pt::uint8_t bitCount, bool isSigned, bool isMotorola)
{
    Pt::uint64_t ddwMsgData = *((Pt::uint64_t*)messageData.data());

    if(Pt::isLittleEndian())
    {
        if(isMotorola)
        {//Motorola	-> Big endian.
             Pt::uint64_t signalBitMask = 0xFFFFFFFFFFFFFFFFULL >> (64 - bitCount);
             Pt::uint64_t ddw 		    = ddwMsgData;

             //Swap the bytes in the int64 type.
             swapBytes(ddw);
             
             Pt::uint8_t physStartBit   = (pivotBit/8)*8 + (7-pivotBit%8);
             Pt::uint8_t shift          = 64 - ( physStartBit + bitCount);

             //Shift on the begin if the 64 byte.
             ddw >>= shift;//byStartBit;

             //Overload the mask.
             ddw &= signalBitMask;
             memcpy(extractedData,&ddw,8);
        }
        else 
        {//Intel -> Little endian.
            Pt::uint64_t ddw            = ddwMsgData >> pivotBit;
            Pt::uint64_t signalBitMask  = 0xFFFFFFFFFFFFFFFFULL >> (64-bitCount);
            
            ddw &= signalBitMask;

            if(isSigned)
            {
                signalBitMask = 0xFFFFFFFFFFFFFFFFULL << bitCount;
                ddw |= signalBitMask;	
            }

            memcpy(extractedData,&ddw,8);
        }
    }
    else
    {
        throw std::runtime_error(PT_SOURCEINFO + "CANDriver::extractDataFromCANMsg() for BigEndian CPU not implemented");
    }

    return true;
}


int Driver::getDriverInfoIndex(const std::string& drvType, const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port)
{
    for( Pt::uint32_t i = 0; i < _drivers.size(); ++i)
    {
        DriverInfo& info = _drivers[i];

        Pt::SmartPtr<DriverImpl> impl = info.impl;

        if( info.driverType == drvType && impl->port() == port && impl->deviceID() == deviceID && impl->deviceNo() == deviceNo)
            return i;
    }

    return -1;
}


void Driver::swapBytes(Pt::uint64_t& ddw)
{
    Pt::uint8_t* source  = (Pt::uint8_t*)&ddw;
    Pt::uint8_t* target  = (Pt::uint8_t*)&ddw;
    Pt::uint8_t  buffer  = 0;

    source	+= 7;

    for(int index = 0; index < 4; index++)
    {
        buffer  = *target;
        *target = *source;
        *source = buffer;
        target++;
        source--;
    }
}


void Driver::reset(bool hardware)
{
    Pt::System::MutexLock lock(_mutex);

    if(hardware)
        _impl->reset();

    while (!_messages.empty()) 
        _messages.pop(); 
}


bool Driver::receive(Message& messageData)
{
    Pt::System::MutexLock lock(_mutex);

    if(_messages.empty())
        return false;

    messageData = _messages.front();
    _messages.pop();
    return true;
}


bool Driver::wait(Pt::uint32_t timeout)
{
    Pt::System::MutexLock lock(_mutex);

    if(!_messages.empty())
        return true;

    return _condition.wait(_mutex, timeout);
}


void Driver::close()
{
    if(_impl == (const DriverImpl*) 0)
        return;

    wake();

    int index = getDriverInfoIndex(_driverType, _impl->deviceID(), _impl->deviceNo(), _impl->port());
    DriverInfo& info = _drivers[index];

    info.refCounter--;

    if( info.refCounter == 0)
    {
        _impl->close();
        _drivers.erase(_drivers.begin() + index);
    }

    _impl.reset(0);
}


void Driver::onMessage(Message msg)
{
    Pt::System::MutexLock lock(_mutex);

    if(((msg.identifier() ^ _code) & _mask) != 0)
        return; //Mask code filter

    _messages.push(msg);
    
    if( _messages.size() == _bufferSize)
        _messages.pop();

    _condition.signal();
}


void Driver::wake()
{    
    _condition.signal();
}


void Driver::addImpl(Pt::SmartPtr<DriverImpl> impl)
{
    _impl = impl;

    int index = getDriverInfoIndex(_driverType, impl->deviceID(), impl->deviceNo(), impl->port());

    if( index == -1)
    {
        DriverInfo info;
        info.impl = impl;
        info.driverType = _driverType;
        info.refCounter = 1;
        _drivers.push_back(info);
    }
    else
    {
        DriverInfo& info = _drivers[index];
        info.refCounter++;
    }

    _impl->onMessage += Pt::slot(*this, &Driver::onMessage);
}


Pt::SmartPtr<DriverImpl> Driver::getImpl(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port)
{
    Pt::SmartPtr<DriverImpl> empty;

    int index = getDriverInfoIndex(_driverType, deviceID, deviceNo, port);

    if( index == -1)
        return empty;

    DriverInfo& info = _drivers[index];
    return info.impl;
}


bool Driver::setAcceptanceMask(Pt::uint32_t mask, Pt::uint32_t code)
{
    Pt::System::MutexLock lock(_mutex);

    _mask = mask;
    _code = code;
    return true;
}


void Driver::send(const Message& messageData)
{
    _impl->send(messageData);
}


bool Driver::extendedID() const
{
    return _impl->extendedID();
}

}}}
