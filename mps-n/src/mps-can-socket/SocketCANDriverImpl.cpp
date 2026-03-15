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
#include "SocketCANDriverImpl.h"
#include <mps/can/drv/Exception.h>
#include <Pt/System/Process.h>
#include <Pt/System/Logger.h>
#include <string.h>

namespace mps{
namespace can{
namespace socket{

PT_LOG_DEFINE("mps.can.socket");

SocketCANDriverImpl::SocketCANDriverImpl()
: _running(false)
, _extendedID(false)
, _port(0)
, _deviceID("")
, _bitRate(0)
, _skt(0)
, _readThread(0)
{
}


SocketCANDriverImpl::~SocketCANDriverImpl()
{
}


std::string SocketCANDriverImpl::replaceString(const std::string& searchString, const std::string& replaceWith, std::string stringToReplace) 
{ 
    std::string::size_type pos    = stringToReplace.find(searchString, 0); 
    const int intLengthSearch	  = searchString.length(); 
    const int intLengthReplacment = replaceWith.length(); 

    while(std::string::npos != pos) 
    { 
        stringToReplace.replace((Pt::uint32_t)pos, (Pt::uint32_t)intLengthSearch, replaceWith); 
        pos = stringToReplace.find(searchString, pos + intLengthReplacment); 
    } 

    return stringToReplace; 
} 


bool SocketCANDriverImpl::setBitrate(const std::string& dev, Pt::uint32_t bitRate)
{ //Workaround => use ifconfig and ip C-API
    //Set first the baud rate using "ip" tool
    std::stringstream ss;
    std::string bitRateStr;

    ss<<bitRate;
    ss>>bitRateStr;

    //CAN down
    std::string cmd = "ifconfig " + dev + " down";
    system(cmd.c_str());

    //Set bitrate
    cmd = "ip link set " + dev  + " type can bitrate " + bitRateStr;
    system(cmd.c_str());

    //Set restart

    cmd = "ip link set " + dev  + " type can restart-ms 10 ";
    system(cmd.c_str());

    //CAN dup
    cmd = "ifconfig " + dev  + " up";
    system(cmd.c_str());

    return true;
}


bool SocketCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
    _deviceID	= replaceString("/dev/","",deviceID);
    _extendedID   = extendedId;
    _port		= port;
    _bitRate	= bitRate;

    SocketCANDriverImpl::setBitrate(_deviceID, _bitRate);

    // Create the socket
    _skt = ::socket( PF_CAN, SOCK_RAW, CAN_RAW );

    if( _skt < 0)
    {
        PT_LOG_ERROR("open() : create socket failed");
        return false;
    }
    
    // Locate the interfa#include <termios.h>ce
    struct ifreq ifr;
    memset(&ifr, 0, sizeof(ifr));
    strcpy(ifr.ifr_name, _deviceID.c_str());
    
    int ret = ioctl(_skt, SIOCGIFINDEX, &ifr); // ifr.ifr_ifindex gets filled  with that device's index

    if( ret < 0) 
    {
        PT_LOG_ERROR("open()  :ioctl SIOCGIFINDEX failed");
        return false;
    }

    // Select that CAN interface, and bind the socket to it.
    struct sockaddr_can addr;
    addr.can_family = AF_CAN;
    addr.can_ifindex = ifr.ifr_ifindex;

    ret = bind(_skt, (struct sockaddr*)&addr, sizeof(addr) );

    if( ret < 0)
    {
        PT_LOG_ERROR("open() : bind failed");
        return false;
    }

    can_err_mask_t  errMask = CAN_ERR_MASK;
    
    ret = setsockopt(_skt, SOL_CAN_RAW, CAN_RAW_ERR_FILTER, &errMask, sizeof(errMask));

    if( ret < 0)
    {
        PT_LOG_ERROR("open() : setsockopt CAN_RAW_ERR_FILTER failed");
        return false;
    }

    int flags  = 0;

    if (-1 == (flags = fcntl(_skt, F_GETFL, 0)))
        flags = 0;

    fcntl(_skt, F_SETFL, flags | O_NONBLOCK);

    _running = true;
    _readThread = new Pt::System::AttachedThread(Pt::callable(*this, &SocketCANDriverImpl::read));
    _readThread->start();

   return true;
}


bool SocketCANDriverImpl::wait(Pt::uint32_t timeoutIn)
{
    timeval timeOut;

    FD_ZERO(&_rfds);
    FD_SET(_skt, &_rfds);

    timeOut.tv_sec = timeoutIn / 1000;
    timeOut.tv_usec = (timeoutIn % 1000) * 1000;

    int rc = select( FD_SETSIZE, &_rfds, NULL, NULL, &timeOut );

    if( rc == -1)
        return false;

    return FD_ISSET(_skt, &_rfds) > 0;
}


void SocketCANDriverImpl::send(const can::drv::Message& messageData)
{
    Pt::System::MutexLock lock(_mutex);

    struct can_frame frame;

    frame.can_id = messageData.identifier();
    frame.can_id |= (_extendedID ? CAN_EFF_FLAG : 0);

    memcpy( &frame.data[0], messageData.data(), 8 );
    frame.can_dlc = messageData.dlc();

    int bytesSent = write(_skt, &frame, sizeof(frame) );   

    if( bytesSent != sizeof(frame))
    {
        PT_LOG_ERROR("send()  failed");
        throw can::drv::Exception("send()  failed", can::drv::Exception::SendError);
    }
}


void SocketCANDriverImpl::read()
{
    can::drv::Message messageData;

    while(_running)
    {
        if(!wait(100))
        {
            if( !_running)
                break;

            continue;
        }

        if( !_running)
            break;

        _mutex.lock();

        struct can_frame frame;

        memset(&frame, 0, sizeof(frame));

        int bytesRead  = ::read( _skt, &frame, sizeof(frame) );

        _mutex.unlock();

        if( bytesRead == 0)
            continue;
        
        if((frame.can_id & CAN_ERR_FLAG) == CAN_ERR_FLAG)
            continue;

        timeval timeout;

        if( ioctl(_skt, SIOCGSTAMP, &timeout) > 0 )
        {
            timeout.tv_sec = 0;
            timeout.tv_usec = 0;
        }

        Pt::uint64_t timeStamp = (static_cast<Pt::uint64_t>(timeout.tv_sec * 1000000) + timeout.tv_usec)/10;
        messageData.setTimeStamp(timeStamp);

        messageData.setIdentifier(frame.can_id & CAN_ERR_MASK);
        messageData.setDlc(frame.can_dlc);
        memcpy(messageData.data(), frame.data, 8);
        
        onMessage(messageData);//Propagate the message to the listener
    }
}


void SocketCANDriverImpl::reset()
{
    //TODO: reset??
}


void SocketCANDriverImpl::close()
{
    if( _readThread != 0 )
    {
        _running = false;		//Set the stop flag

        FD_CLR( _skt, &_rfds );  //Wake up
        _readThread->join();	//Wait terminate

        delete _readThread;

        _readThread = 0;
        ::close(_skt);
    }
}

}}}
