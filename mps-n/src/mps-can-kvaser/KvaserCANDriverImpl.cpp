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
#include "KvaserCANDriverImpl.h"
#include <mps/drv/can/CANException.h>
#include <Pt/System/Clock.h>
#include <vector>

namespace mps{
namespace can{
namespace kvaser{

KvaserCANDriverImpl::KvaserCANDriverImpl()
: _logger("mps.candrv.KvaserCANDriverImpl")
, _extendedID(false)
, _running(false)
, _readThread(0)
, _bitRate(0)
, _deviceID("")
, _port(0)
{
    canInitializeLibrary();
}

KvaserCANDriverImpl::~KvaserCANDriverImpl()
{
    close();
}

bool KvaserCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
    _extendedID = extendedId;
    _bitRate = bitRate;
    _deviceID = deviceID;
    _port = port;
    _deviceNo = deviceNo;

    std::stringstream ss;
    DWORD cardTypeNo;
    ss<< deviceID;
    ss>>cardTypeNo;

    unsigned int channelCount = 0;

    int stat = canGetNumberOfChannels((int*)&channelCount);

    DWORD cardType;
    DWORD chnNoOnCard;

    std::vector<Pt::uint32_t> channels;

    for(Pt::uint32_t chn = 0; chn < channelCount; ++chn)
    {
        canGetChannelData(chn, canCHANNELDATA_CARD_TYPE, &cardType, sizeof(cardType));
        canGetChannelData(chn, canCHANNELDATA_CHAN_NO_ON_CARD, &chnNoOnCard, sizeof(chnNoOnCard));

        if( cardType == cardTypeNo && chnNoOnCard == port)
            channels.push_back(chn);
    }

    if( deviceNo >= channels.size())
    {
        logger_log_error(_logger, "Unknow device nummber for "<<deviceNo);
        return false;
    }

    _chnHandle = canOpenChannel(channels[deviceNo], canWANT_VIRTUAL);

    if (_chnHandle < 0) 
    {
        logger_log_error(_logger,"open()  canOpenChannel failed");
        return false;
    } 

    if(canIoCtl(_chnHandle, canIOCTL_FLUSH_TX_BUFFER, NULL, NULL) != canOK)
    {
        logger_log_error(_logger,"open()  canIoCtl failed");
        return false;
    }

    unsigned int usedBaudRate = 0;
    switch(_bitRate) 
    {
        case 1000000:
            usedBaudRate = BAUD_1M;
        break;
        
        case 666666:
            usedBaudRate = 666666;
        break;

        case 500000:
            usedBaudRate = BAUD_500K;
        break;
        
        case 250000:
            usedBaudRate = BAUD_250K;
        break;
        
        case 125000:
            usedBaudRate = BAUD_125K;
        break;
        
        case 100000:
            usedBaudRate = BAUD_100K;
        break;
        
        case 62500:
            usedBaudRate = BAUD_62K;
        break;
        
        case 50000:
            usedBaudRate = BAUD_50K;
        break;

        default:
            logger_log_error(_logger,"open()  unsupported bit rate :"<<bitRate);
            return false;
        break;
    }
    
    if(usedBaudRate == 666666)
        stat = canSetBusParams(_chnHandle, usedBaudRate, 8, 3, 1, 1, 0);
    else
        stat = canSetBusParams(_chnHandle, usedBaudRate, 0, 0, 0, 0, 0);

    if (stat < 0) 
    {
        logger_log_error(_logger,"open()  canSetBusParams failed");
        return false;
    }

    long mask = 0;
    long code = 0;
    if(_extendedID)
    {
        // extended
        mask = 0x80000000; // Open for all
        code = 0x80000000;
        canAccept(_chnHandle, mask, canFILTER_SET_MASK_EXT);
        canAccept(_chnHandle, code, canFILTER_SET_CODE_EXT);
    }
    else
    {
        // set the acceptance filter relevant=1
        // standard
        mask = 0x000; // Open all
        code = 0x000;
        canAccept(_chnHandle, mask, canFILTER_SET_MASK_STD);
        canAccept(_chnHandle, code, canFILTER_SET_CODE_STD);
    }

    reset();

    _running = true;
    _readThread = new Pt::System::AttachedThread(Pt::callable(*this, &KvaserCANDriverImpl::read));
    _readThread->start();

    return true;
}

void KvaserCANDriverImpl::send(const mps::drv::can::Message& messageData)
{	
    Pt::System::MutexLock lock(_mutex);

    long id = messageData.identifier();
    unsigned int dlc = messageData.dlc();
    unsigned int flag = canMSG_STD;

    if( _extendedID)
        flag = canMSG_EXT;

    if(canWriteWait(_chnHandle, id, (void*) messageData.data(), dlc, flag, 500) != 0)
    {
        logger_log_error(_logger,"send()  failed");
        std::stringstream ss;
        ss<<"Send message failed";
        throw mps::drv::can::CANException(ss.str(), mps::drv::can::CANException::SendError);
    }
}

void KvaserCANDriverImpl::read()
{
    mps::drv::can::Message messageData;
    int canStatus = 0;
    long id = 0;
    unsigned int dlc = 0;
    unsigned int flags = 0;
    DWORD time = 0;

    while(_running)
    {
        _mutex.lock();

        canStatus = canRead(_chnHandle, &id, messageData.data(), &dlc, &flags, &time);
        
        _mutex.unlock();

        if( canStatus != canOK )
        {
            canWaitForEvent(_chnHandle, 1000);
            continue;
        }

        Pt::uint64_t micros10 = time * 100 - _startTime;
        messageData.setTimeStamp(micros10);
        messageData.setIdentifier(id);
        messageData.setDlc(dlc);
        onMessage(messageData);
    }
}

void KvaserCANDriverImpl::reset()
{
    Pt::System::MutexLock lock(_mutex);

    canResetBus(_chnHandle);
    canFlushReceiveQueue(_chnHandle);
    canFlushTransmitQueue(_chnHandle);

    _startTime = canReadTimer(_chnHandle) * 100;
}

void KvaserCANDriverImpl::close()
{
    if( _readThread != 0 )
    {
        _running = false; // Set the stop flag

        //Wake up
        long id = 7411;
        unsigned int dlc = 0;
        Pt::uint8_t data[8];
        unsigned int flag = canMSG_WAKEUP;
        canWrite(_chnHandle, id, data, dlc, flag);

        _readThread->join(); //Wait terminate

        delete _readThread;

        _readThread = 0;
        canClose(_chnHandle);
    }
}

}}}
