/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#include "PeakCANDriverImpl.h"
#include "PeakCANDrvWrapper.h"
#include <mps/candrv/CANException.h>
#include <Pt/System/Clock.h>

namespace mps{
namespace candrv{

TCAN_Initialize     PeakCANDrvWrapper::_CAN_Initialize = 0;
TCAN_Uninitialize   PeakCANDrvWrapper::_CAN_Uninitialize = 0;
TCAN_Read           PeakCANDrvWrapper::_CAN_Read = 0;
TCAN_Write          PeakCANDrvWrapper::_CAN_Write = 0;
TCAN_SetValue       PeakCANDrvWrapper::_CAN_SetValue = 0;
TCAN_Reset          PeakCANDrvWrapper::_CAN_Reset = 0;
Pt::System::Library PeakCANDrvWrapper::_library;
bool                PeakCANDrvWrapper::_isopen = false;

PeakCANDriverImpl::PeakCANDriverImpl()
: _logger("mps::candrv::PeakCANDriver")
, _extendedID(false)
, _running(false)
, _readThread(0)
, _bitRate(0)
, _deviceID("")
, _port(0)
{
	_canEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
}

PeakCANDriverImpl::~PeakCANDriverImpl()
{
	close();
	CloseHandle(_canEvent);
}

bool PeakCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
    if( port > 1 )
    {
		PT_LOG_ERROR(_logger,"open()  port > 1 condition failed");
        return false;
    }

	if(!PeakCANDrvWrapper::loadDriver())
    {
        PT_LOG_ERROR(_logger,"open() PeakCANDrvWrapper::loadDriver() failed");
		return false;
    }

	TPCANBaudrate brate = 0;
	_extendedID = extendedId;
	_bitRate = bitRate;
	_deviceID = deviceID;
	_port = port;
	_startTime = Pt::System::Clock::getSystemTicks().toUSecs() /10;

	std::stringstream ss;
	ss <<_deviceID;
	ss >> _channelHandle;

	switch(bitRate)
	{
		case 1000000:
			brate = PCAN_BAUD_1M;
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
			brate = PCAN_BAUD_500K;
		break;
	}

	DWORD err = PeakCANDrvWrapper::CAN_Initialize(_channelHandle, brate, 0, 0, 0); 

	if( err != PCAN_ERROR_OK)
	{
        PT_LOG_ERROR(_logger,"open() PeakCANDrvWrapper::CAN_Initialize failed : code = " << err);
		return false;
	}

    err = PeakCANDrvWrapper::CAN_SetValue(_channelHandle, PCAN_RECEIVE_EVENT, &_canEvent, sizeof(_canEvent));

    if( err != PCAN_ERROR_OK)
	{
        PT_LOG_ERROR(_logger,"open() PeakCANDrvWrapper::CAN_SetValue failed : code = " << err);
		return false;
	}

   	_running = true;
	_readThread = new Pt::System::AttachedThread(Pt::callable(*this, &PeakCANDriverImpl::read));
	_readThread->start();

    return true;
}

bool PeakCANDriverImpl::wait(Pt::uint32_t timeout)
{
	return (WaitForSingleObject(_canEvent, timeout) != WAIT_TIMEOUT);
}

void PeakCANDriverImpl::send(const Message& messageData)
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

	DWORD error = PeakCANDrvWrapper::CAN_Write(_channelHandle, &msg);
	
	if(error != PCAN_ERROR_OK)
	{
		PT_LOG_ERROR(_logger,"send()  failed : code = "<<error);
		std::stringstream ss;
		ss<<"Send message failed internal error: " <<error;
		throw CANException(ss.str(), CANException::SendError);
	}
}

void PeakCANDriverImpl::read()
{
    Message messageData;

    while(_running)
    {
        _mutex.lock();

        TPCANMsg msg;
        TPCANTimestamp CANTimeStamp;
		TPCANStatus status;

		status = PeakCANDrvWrapper::CAN_Read(_channelHandle, &msg, &CANTimeStamp);
		
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

void PeakCANDriverImpl::reset()
{
    Pt::System::MutexLock lock(_mutex);
    PeakCANDrvWrapper::CAN_Reset(_channelHandle); 
	_startTime = Pt::System::Clock::getSystemTicks().toUSecs() /10;
}

void PeakCANDriverImpl::close()
{
    if( _readThread != 0 )
    {
        _running = false; // Set the stop flag
    
        SetEvent(_canEvent); // Wake up the thread
    
        _readThread->join(); //Wait terminate
    
        delete _readThread;

        _readThread = 0;
	    PeakCANDrvWrapper::CAN_Uninitialize(_channelHandle);
    }
}

}}
