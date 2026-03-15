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
#include "VectorCANXLDriverImpl.h"
#include <mps/drv/can/CANException.h>
#include <vector>

namespace mps{
namespace can{
namespace vector{

VectorCANXLDriverImpl::VectorCANXLDriverImpl()
: _logger("mps.candrv.VectorCANXLDriverImpl")
, _readThread(0)
,  _xlPortHandle(XL_INVALID_PORTHANDLE)
{

}

VectorCANXLDriverImpl::~VectorCANXLDriverImpl()
{	
      xlCloseDriver();
}

bool VectorCANXLDriverImpl::open(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedID)
{
    XLstatus  xlStatus;
    XLaccess  xlPermissionMask = 0;
    int       devID = 0;

    std::stringstream ss;

    ss<< deviceID;
    ss>>devID;

    _port = port;
    _extendedID = extendedID;
    _deviceID = deviceID;
    _bitRate = bitRate;
    _xlChannelMask = 0;
    _deviceNo = deviceNo;

    _event = CreateEvent(NULL, FALSE, FALSE, NULL);

    xlStatus = xlOpenDriver();

    if(XL_SUCCESS != xlStatus) 
    {
        logger_log_error(_logger, "xlOpenDriver() FAILED! code = " << xlStatus);
        return false;
    }

    xlStatus = xlGetDriverConfig(&_xlDrvConfig);
  
    if(XL_SUCCESS != xlStatus) 
    {
        logger_log_error(_logger, "xlGetDriverConfig() FAILED! code = " << xlStatus);
        return false;
    }

    //Search all channels from the given device type and channel nummber.
    std::vector<XL_CHANNEL_CONFIG*> channels;

    for( unsigned int i = 0; i < _xlDrvConfig.channelCount; ++i)
    {
        if ( (_xlDrvConfig.channel[i].channelBusCapabilities & XL_BUS_ACTIVE_CAP_CAN)
          && (_xlDrvConfig.channel[i].hwType == devID) &&  _xlDrvConfig.channel[i].hwChannel == port)  
        {
            channels.push_back(&_xlDrvConfig.channel[i]);
        }
    }

    if(deviceNo >= channels.size())
    {
        logger_log_error(_logger, "Unknow device nummber for "<<devID);
        return false;
    }

    _xlChannelMask |= channels[deviceNo]->channelMask;

    xlPermissionMask = _xlChannelMask;
    
    xlStatus = xlOpenPort(&_xlPortHandle, "MeaProcess", _xlChannelMask, &xlPermissionMask, 4096*10, XL_INTERFACE_VERSION, XL_BUS_TYPE_CAN);

    if(XL_SUCCESS != xlStatus) 
    {
        logger_log_error(_logger, "xlOpenPort() FAILED! code = " << xlStatus);
        return false;
    }

    xlStatus = xlCanSetChannelBitrate(_xlPortHandle, _xlChannelMask, _bitRate);

    if(XL_SUCCESS != xlStatus) 
    {
        logger_log_error(_logger, "xlCanSetChannelBitrate() FAILED! code = " << xlStatus);
        return false;
    }

    xlStatus = xlSetNotification (_xlPortHandle, &_hMsgEvent, 1);

    if(XL_SUCCESS != xlStatus) 
    {
        logger_log_error(_logger, "xlSetNotification() FAILED! code = " << xlStatus);
        return false;
    }

     xlStatus = xlActivateChannel(_xlPortHandle, _xlChannelMask, XL_BUS_TYPE_CAN, XL_ACTIVATE_RESET_CLOCK);
     
    if(XL_SUCCESS != xlStatus) 
    {
        logger_log_error(_logger, "xlActivateChannel() FAILED! code = " << xlStatus);
        return false;
    }

    reset();

    _running = true;
    _readThread = new Pt::System::AttachedThread(Pt::callable(*this, &VectorCANXLDriverImpl::read));
    _readThread->start();	
    return true;
}

bool VectorCANXLDriverImpl::wait(Pt::uint32_t timeout)
{
    return (WaitForSingleObject(_event, timeout) != WAIT_TIMEOUT);
}

void VectorCANXLDriverImpl::send(const mps::drv::can::Message& messageData)
{	
    Pt::System::MutexLock lock(_mutex);
    
    XLevent       xlEvent;
    XLstatus      xlStatus;
     unsigned int  messageCount = 1;

    xlEvent.tag               = XL_TRANSMIT_MSG;
    xlEvent.tagData.msg.flags = 0;
    xlEvent.tagData.msg.id    = messageData.identifier();
    xlEvent.tagData.msg.dlc   = messageData.dlc();

    memcpy(&xlEvent.tagData.msg.data[0], messageData.data(), 8);

    xlStatus = xlCanTransmit(_xlPortHandle, _xlChannelMask, &messageCount, &xlEvent);

    if( xlStatus != 0) 
    {
        logger_log_error(_logger,"send()  failed : code = "<<xlStatus);
        std::stringstream ss;
        ss<<"Send message failed internal error: "<<xlStatus;
        throw mps::drv::can::CANException(ss.str(),mps::drv::can::CANException::SendError);
    }
}

void VectorCANXLDriverImpl::read()
{
    XLevent         xlEvent; 
    XLstatus        xlStatus;

    mps::drv::can::Message messageData;

    while(_running)
    {
        unsigned int msgsrx = 1;
        xlStatus = xlReceive(_xlPortHandle, &msgsrx, &xlEvent);

        if( xlStatus == XL_ERR_QUEUE_IS_EMPTY)
        {            
            WaitForSingleObject(_hMsgEvent,100);
            continue;
        }

        messageData.setTimeStamp( xlEvent.timeStamp/10000 ); // 10 microseconds resolution
        messageData.setIdentifier( xlEvent.tagData.msg.id );
        messageData.setDlc( xlEvent.tagData.msg.dlc );
        memcpy( messageData.data(), xlEvent.tagData.msg.data, 8 );		
        onMessage(messageData);
    }
}

void VectorCANXLDriverImpl::reset()
{
    Pt::System::MutexLock lock(_mutex);
    
    //xlFlushTransmitQueue( g_xlPortHandle, xlChannelMask );
    xlFlushReceiveQueue( _xlPortHandle );
    xlResetClock (_xlPortHandle );
}
    
void VectorCANXLDriverImpl::close()
{
    if(_xlPortHandle != XL_INVALID_PORTHANDLE)
    {
        _running = false;
        SetEvent(_event);
        _readThread->join();
        delete _readThread;
        _readThread = 0;

        xlClosePort(_xlPortHandle);
         _xlPortHandle = XL_INVALID_PORTHANDLE;
        CloseHandle(_event);
    }
}

}}}
