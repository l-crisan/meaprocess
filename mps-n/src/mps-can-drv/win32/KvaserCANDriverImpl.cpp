/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#include "KvaserCANDriverImpl.h"
#include <mps/candrv/CANException.h>
#include <Pt/System/Clock.h>

namespace mps{
namespace candrv{

KvaserCANDriverImpl::KvaserCANDriverImpl()
: _logger("mps::candrv::PeakCANDriver")
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

bool KvaserCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
    _extendedID = extendedId;
	_bitRate = bitRate;
	_deviceID = deviceID;
	_port = port;
    
    unsigned int channels = 0;

    int stat = canGetNumberOfChannels((int*)&channels);

    if( port >= channels)
    {
        PT_LOG_ERROR(_logger,"open()  port >= channels condition failed");
        return false;
    }    
  
    _chnHandle = canOpenChannel(_port, canWANT_VIRTUAL);

    if (_chnHandle < 0) 
    {
        PT_LOG_ERROR(_logger,"open()  canOpenChannel failed");
        return false;
    } 

    if(canIoCtl(_chnHandle, canIOCTL_FLUSH_TX_BUFFER, NULL, NULL) != canOK)
    {
        PT_LOG_ERROR(_logger,"open()  canIoCtl failed");
        return false;
    }

    unsigned int usedBaudRate = 0;
    switch(_bitRate) 
    {
        case 1000000:
            usedBaudRate = BAUD_1M;
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
    }
    
    stat = canSetBusParams(_chnHandle, usedBaudRate, 0, 0, 0, 0, 0);
    if (stat < 0) 
    {
        PT_LOG_ERROR(_logger,"open()  canSetBusParams failed");
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

void KvaserCANDriverImpl::send(const Message& messageData)
{	
    Pt::System::MutexLock lock(_mutex);

    long id = messageData.identifier();
    unsigned int dlc = messageData.dlc();
    unsigned int flag = canMSG_STD;

    if( _extendedID)
        flag = canMSG_EXT;

	if(canWriteWait(_chnHandle, id, (void*) messageData.data(), dlc, flag, 500) != 0)
    {
        PT_LOG_ERROR(_logger,"send()  failed");
		std::stringstream ss;
		ss<<"Send message failed";
		throw CANException(ss.str(), CANException::SendError);
    }
}

void KvaserCANDriverImpl::read()
{
    Message messageData;
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

}}
