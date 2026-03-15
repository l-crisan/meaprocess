/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#include "SoftingCANDriverImpl.h"
#include <mps/candrv/CANException.h>

namespace mps{
namespace candrv{

SoftingCANDriverImpl::SoftingCANDriverImpl()
: _logger("mps::candrv::SoftingCANDriver")
, _extendedID(false)
, _running(false)
, _readThread(0)
{
}

SoftingCANDriverImpl::~SoftingCANDriverImpl()
{	
}

bool SoftingCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
	if(port > 4)
	{
		PT_LOG_ERROR(_logger,"open()  port > 4 condition failed");
		return false;
	}

	_port = port;
	_deviceID = deviceID;
	_bitRate = bitRate;
	_extendedID = extendedId;
	
	int ret;
	unsigned long u32NeededBufferSize, u32NumOfChannels;

	_pbuffer = 0;

	ret = CANL2_get_all_CAN_channels(0, &u32NeededBufferSize, &u32NumOfChannels, NULL);
	
	if(u32NumOfChannels == 0 || ret != 0)
	{
		PT_LOG_ERROR(_logger,"open()  CANL2_get_all_CAN_channels(1) failed : code = "<<ret);
		return false;
	}

	_pbuffer = (PCHDSNAPSHOT)malloc(u32NeededBufferSize);  

	ret = CANL2_get_all_CAN_channels(u32NeededBufferSize, &u32NeededBufferSize, &u32NumOfChannels, _pbuffer);

	if(u32NumOfChannels == 0 || ret != 0)
	{
		PT_LOG_ERROR(_logger,"open()  CANL2_get_all_CAN_channels(2) failed : code = "<<ret);
		free(_pbuffer);
		_pbuffer = 0;
		return false;
	}

	if( port > u32NumOfChannels)
	{
		PT_LOG_ERROR(_logger,"open()  port > u32NumOfChannels condition failed");

		free(_pbuffer);
		_pbuffer = 0;
		return false;
	}

	PCHDSNAPSHOT pCh = (PCHDSNAPSHOT) &_pbuffer[port];

	_l2config.bEnableAck = GET_FROM_SCIM;
	_l2config.bEnableErrorframe = GET_FROM_SCIM;
	_l2config.s32AccCodeStd = GET_FROM_SCIM;
	_l2config.s32AccCodeXtd = GET_FROM_SCIM;
	_l2config.s32AccMaskStd = GET_FROM_SCIM;
	_l2config.s32AccMaskXtd = GET_FROM_SCIM;
	_l2config.s32OutputCtrl = GET_FROM_SCIM;
	_l2config.s32Sam = GET_FROM_SCIM;	
	_l2config.s32Prescaler = GET_FROM_SCIM;
	_l2config.s32Sjw = GET_FROM_SCIM;
	_l2config.s32Tseg1 = GET_FROM_SCIM;
	_l2config.s32Tseg2 = GET_FROM_SCIM;

	_l2config.hEvent = CreateEvent(NULL, FALSE, FALSE, NULL);
	_event = _l2config.hEvent;
	
	strcpy((char*) &_ch.sChannelName[0],pCh->ChannelName);

	ret = INIL2_initialize_channel(&_ch.ulChannelHandle, pCh->ChannelName);

	if( ret != 0)
	{
		PT_LOG_ERROR(_logger,"open()  INIL2_initialize_channel failed code ="<<ret);
		free(_pbuffer);
		_pbuffer = 0;
		return false;
	}


	ret = CANL2_initialize_fifo_mode(_ch.ulChannelHandle, &_l2config);

	if(ret != 0)
	{
		PT_LOG_ERROR(_logger,"open()  CANL2_initialize_fifo_mode failed code ="<<ret);
		free(_pbuffer);
		_pbuffer = 0;
		return false;
	}

	_portHandle = _ch.ulChannelHandle;

	int presc;
	int sjw;
	int tseg1;
	int tseg2;
	int sam = 1;

	switch(bitRate)
	{
		case 1000000:
			presc = 1;
			sjw = 1;
			tseg1 = 4;
			tseg2 = 3;
		break;
		case 800000:
			presc = 1;
			sjw = 1;
			tseg1 = 6;
			tseg2 = 3;
		break;

		case 500000:
			presc = 1;
			sjw = 1;
			tseg1 = 8;
			tseg2 = 7;
		break;
		case 250000:
			presc = 2;
			sjw = 1;
			tseg1 = 8;
			tseg2 = 7;
		break;
		case 150000:
			presc = 4;
			sjw = 1;
			tseg1 = 8;
			tseg2 = 7;
		break;
		case 100000:
			presc = 4;
			sjw = 4;
			tseg1 = 11;
			tseg2 = 8;
		break;
		case 10000:
			presc = 32;
			sjw = 4;
			tseg1 = 16;
			tseg2 = 8;
		break;
		default:
			PT_LOG_ERROR(_logger,"open()  unsupported bit rate :"<<bitRate);
			free(_pbuffer);
			_pbuffer = 0;
			return false;
		break;
	}

	ret = CANL2_initialize_chip(_portHandle, presc, sjw, tseg1, tseg2, sam);
	if( ret != 0)
	{
		PT_LOG_ERROR(_logger,"open()  CANL2_initialize_chip failed code=:"<<ret);
		free(_pbuffer);
		_pbuffer = 0;
		return false;
	}

	ret = CANL2_start_chip(_portHandle);

	if( ret != 0)
	{
		PT_LOG_ERROR(_logger,"open()  CANL2_start_chip failed code=:"<<ret);
		free(_pbuffer);
		_pbuffer = 0;
		return false;
	}
	
    _running = true;
	_readThread = new Pt::System::AttachedThread(Pt::callable(*this, &SoftingCANDriverImpl::read));
	_readThread->start();

	return true;
}

bool SoftingCANDriverImpl::wait(Pt::uint32_t timeout)
{
	return (WaitForSingleObject(_event,timeout) != WAIT_TIMEOUT);
}

void SoftingCANDriverImpl::send(const Message& messageData)
{	
    Pt::System::MutexLock lock(_mutex);

	unsigned char Xtd = 0;

	int err = CANL2_send_data(_portHandle, messageData.identifier(), Xtd, messageData.dlc(),(unsigned char*) messageData.data());
	
	if( err != 0)
		throw CANException("Send data faild", CANException::SendError);
}

void SoftingCANDriverImpl::read()
{
    Message message;

    while(_running)
    {
        _mutex.lock();

	    PARAM_STRUCT 	param;
	    int frc     = CANL2_RA_NO_DATA;
	    int lostMsg = 0;

	    frc = CANL2_read_ac(_portHandle, &param );

        _mutex.unlock();

	    if(frc != CANL2_RA_DATAFRAME)
	    {
			wait(100000);
			continue;
		}

	    message.setTimeStamp(param.Time/10);
        message.setIdentifier(param.Ident);
	    message.setDlc(param.DataLength);		    
	    memcpy(message.data(),param.RCV_data,8);
	    onMessage(message);
    }
}

void SoftingCANDriverImpl::reset()
{
    Pt::System::MutexLock lock(_mutex);

    CANL2_reset_rcv_fifo(_portHandle);
    CANL2_reset_xmt_fifo(_portHandle);
	CANL2_start_chip(_portHandle);
}

void SoftingCANDriverImpl::close()
{
	if(_pbuffer == 0)
		return;

    if( _readThread != 0)
    {
        _running = false;
        SetEvent(_event);
        _readThread->join();
        delete _readThread;
        _readThread = 0;

	    INIL2_close_channel(_portHandle);
    	
	    free(_pbuffer);
	    _pbuffer = 0;
	    CloseHandle(_event);
    }
}

}}
