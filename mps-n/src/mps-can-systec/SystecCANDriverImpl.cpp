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
#include "SystecCANDriverImpl.h"
#include <mps/drv/can/CANException.h>
#include "UCanLibrary.h"
#include <vector>

namespace mps{
namespace can{
namespace systec{

std::map<Pt::uint8_t, SystecCANDriverImpl::DeviceInfo> SystecCANDriverImpl::_deviceHandles;

SystecCANDriverImpl::SystecCANDriverImpl()
: _logger("mps.candrv.SystecCANDriverImpl")
, _extendedID(false)
, _running(false)
, _readThread(0)
{
}

SystecCANDriverImpl::~SystecCANDriverImpl()
{
}

bool SystecCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
    _port = port;
    _deviceID = deviceID;
    _bitRate = bitRate;
    _extendedID = extendedId;
    _deviceNo = deviceNo;
    WORD rate = 0;


    if(!UCanLibrary::loadDriver())
        return false;

    switch(_bitRate)
    {
        case 1000000:
            rate = USBCAN_BAUD_1MBit;
        break;
        case 800000:
            rate = USBCAN_BAUD_800kBit;
        break;

        case 500000:
            rate = USBCAN_BAUD_500kBit;
        break;

        case 250000:
            rate = USBCAN_BAUD_250kBit;
        break;

        case 100000:
            rate = USBCAN_BAUD_100kBit;
        break;
        case 10000:
            rate = USBCAN_BAUD_10kBit;
        break;

        default:
            logger_log_error(_logger,"open()  unsupported bit rate :"<<bitRate);
            return false;
        break;
    }

    DeviceHandleIt it = _deviceHandles.find(_deviceNo);
    
    if( it == _deviceHandles.end())
    {
        if(UCanLibrary::initHardwareEx(&_handle, _deviceNo, SystecCANDriverImpl::onEvent, this) != USBCAN_SUCCESSFUL)
        {
            logger_log_error(_logger,"initHardwareEx() FAILED!");
            return false;
        }
        
        DeviceInfo info;
        info.handle = _handle;
        info.refCounter = 1; 

        std::pair<Pt::uint8_t,DeviceInfo>  pair(_deviceNo, info);
        _deviceHandles.insert(pair);
    }
    
    it = _deviceHandles.find(_deviceNo);
    _handle = it->second.handle;
    it->second.refCounter++;
              
    memset (&_chnParam, 0, sizeof (_chnParam));
    _chnParam.m_dwSize                  = sizeof (_chnParam);
    _chnParam.m_bMode                   = kUcanModeTxEcho; 
    _chnParam.m_bBTR0                   = HIBYTE (rate);
    _chnParam.m_bBTR1                   = LOBYTE (rate);
    _chnParam.m_bOCR                    = USBCAN_OCR_DEFAULT;
    _chnParam.m_dwAMR                   = 0xFFFFFFFF;
    _chnParam.m_dwACR                   = 0x00000000;
    _chnParam.m_dwBaudrate              = USBCAN_BAUDEX_USE_BTR01;
    _chnParam.m_wNrOfRxBufferEntries    = USBCAN_DEFAULT_BUFFER_ENTRIES;
    _chnParam.m_wNrOfTxBufferEntries    = USBCAN_DEFAULT_BUFFER_ENTRIES;

   if(UCanLibrary::initCanEx2(_handle, _port, &_chnParam) != USBCAN_SUCCESSFUL)
   {
        _port = 0xff;
        close();
        logger_log_error(_logger,"initCanEx2() FAILED!");
        return false;
   }
   
   return true;
}

void SystecCANDriverImpl::onEvent (tUcanHandle UcanHandle_p, DWORD dwEvent_p, BYTE bChannel_p, void* pArg_p)
{
    SystecCANDriverImpl* impl = (SystecCANDriverImpl*) pArg_p;

    switch (dwEvent_p)
    {
        case USBCAN_EVENT_INITHW:
        break;

        
        case USBCAN_EVENT_INITCAN:
        break;
        
        case USBCAN_EVENT_RECEIVE:
        {
            tCanMsgStruct msg[10];
            DWORD count = 10;

            {
                Pt::System::MutexLock lock(impl->_mutex);

                BYTE bRet = UCanLibrary::readCanMsgEx (UcanHandle_p, &bChannel_p, &msg[0], &count);

                if (USBCAN_CHECK_ERROR (bRet))
                    return;
            }

            for( Pt::uint32_t  i = 0; i < count; ++i)
            {
                mps::drv::can::Message message;
                message.setTimeStamp(msg[i].m_dwTime*100);
                message.setIdentifier(msg[i].m_dwID);
                message.setDlc(msg[i].m_bDLC);   
                memcpy(message.data(),msg[i].m_bData,8);
                impl->onMessage(message);
            }
        }
        break;
        
        case USBCAN_EVENT_STATUS:
        break;
       
        case USBCAN_EVENT_DEINITCAN:
        break;
       
        case USBCAN_EVENT_DEINITHW:
        break;
    }

}

void SystecCANDriverImpl::send(const mps::drv::can::Message& messageData)
{
    Pt::System::MutexLock lock(_mutex);
    tCanMsgStruct msg;
    DWORD count = 1;

    msg.m_dwID = messageData.identifier();
    msg.m_bDLC = messageData.dlc();

    if( _extendedID)
        msg.m_bFF = 0x80;
    else
        msg.m_bFF = 0x00;

    memcpy(msg.m_bData , &messageData.data()[0], 8);

    BYTE bRet = UCanLibrary::writeCanMsgEx (_handle, _port, &msg, &count);
    
    if (USBCAN_CHECK_ERROR (bRet))
        throw mps::drv::can::CANException("Send data faild", mps::drv::can::CANException::SendError);
}

void SystecCANDriverImpl::reset()
{
    Pt::System::MutexLock lock(_mutex);
    DWORD flags =   USBCAN_RESET_NO_CANCTRL |  USBCAN_RESET_NO_TXBUFFER_ALL  |  USBCAN_RESET_NO_RXBUFFER_COMM | USBCAN_RESET_NO_TXCOUNTER;
    UCanLibrary::resetCanEx(_handle, _port, flags);
}

void SystecCANDriverImpl::close()
{
    DeviceHandleIt it = _deviceHandles.find(_deviceNo);
    
    if(it->second.refCounter == 0)
        return;
    
    if( _port != 0xff)
        UCanLibrary::deinitCanEx(_handle, _port);

    it->second.refCounter--;

    if( it->second.refCounter == 0)
    {
        UCanLibrary::deinitHardware(_handle);
        _deviceHandles.erase(_deviceNo);
    }
}

}}}
