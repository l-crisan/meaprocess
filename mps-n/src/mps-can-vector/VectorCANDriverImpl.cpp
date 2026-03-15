/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#include "VectorCANDriverImpl.h"
#include <mps/candrv/CANException.h>

namespace mps{
namespace can{
namespace vector{

VectorCANDriverImpl::VectorCANDriverImpl()
: _baudRate(0)
, _btr0(0x00)
, _btr1(0x23)
, _channelMask(0)
, _outputMode(OUTPUT_MODE_NORMAL)
, _initMask(0)
, _permissionMask(0)
, _pDriverConfig(0)
, _debugLevel(0)
, _timerRate(0)
, _hwChannel(0)
, _hwType(0)
, _logger("mps.candrv.VectorCANDriver")
, _readThread(0)
{
	strcpy(_appName,"MeaProcess");
}

VectorCANDriverImpl::~VectorCANDriverImpl()
{	
	ncdCloseDriver();
}

bool VectorCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedID)
{		
	_port = port;
    _extendedID = extendedID;
	_deviceID = deviceID;
	_bitRate = bitRate;

	_event = CreateEvent(NULL, FALSE, FALSE, NULL);

	Vstatus			vErr;
	VchipParams		chipInit;
	VsetAcceptance	acc;

	_baudRate = bitRate;

	vErr = ncdOpenDriver();

	if (vErr) 
	{
		logger_log_error(_logger,"ncdOpenDriver() : code = "<<vErr);
		return false;
	}

	Pt::uint32_t devIdNo = 0;
	std::stringstream ss;
	ss<<_deviceID;
	ss>>devIdNo;

	if(!regApplication(devIdNo))
		return false;

	_channelMask = ncdGetChannelMask( _hwType, 0, _hwChannel);
    
	if(_channelMask == 0)
	{
		logger_log_error(_logger,"ncdGetChannelMask() : hardware is not present");
	   return false;  //Hardware is not present
	}

	//Open the communication port
	vErr = ncdOpenPort(&_portHandle, _appName, _channelMask, _initMask, &_permissionMask, 1024);

	if (vErr) 
	{
		logger_log_error(_logger,"ncdOpenPort() : code = "<<vErr);
		return false;
	}

	if (_portHandle == INVALID_PORTHANDLE) 
	{
		logger_log_error(_logger,"ncdOpenPort() : invalide port handle");
		return false;
	}

	//Initialize the channels
	chipInit.sjw   = 1;
	chipInit.tseg1 = 8;
	chipInit.tseg2 = 7;
	chipInit.sam   = 3;
	chipInit.bitRate = _baudRate;

	vErr = ncdSetChannelParams(_portHandle, _permissionMask, &chipInit);

	if (vErr) 
	{
		logger_log_error(_logger,"ncdSetChannelParams() failed : code = "<<vErr);
		return false;
	}

	vErr = ncdSetChannelOutput(_portHandle, _permissionMask, OUTPUT_MODE_NORMAL );

	if( vErr )
	{
		logger_log_error(_logger,"ncdSetChannelOutput() failed : code = "<<vErr);
		return false;
	}

	if(extendedID)
	{
		// extended
		acc.mask = 0x80000000; // Open for all
		acc.code = 0x80000000;
	}
	else
	{
		// set the acceptance filter relevant=1
		// standard
		acc.mask = 0x000; // Open all
		acc.code = 0x000;
	}
	
	vErr = ncdSetChannelAcceptance(_portHandle,_channelMask, &acc);
	if (vErr) 
	{
		logger_log_error(_logger,"ncdSetChannelAcceptance() failed : code = "<<vErr);
		return false;
	}

	// put all selected channels on bus
	vErr = ncdActivateChannel(_portHandle, _channelMask);
	
	if (vErr) 
	{
		logger_log_error(_logger,"ncdActivateChannel() failed : code = "<<vErr);
		return false;
	}	

	vErr = ncdSetNotification(_portHandle, (unsigned long*) &_event, 1);

	if (vErr) 
	{
		logger_log_error_logger,"ncdSetNotification() failed : code = "<<vErr);
		return false;
	}	

	reset();

    _running = true;
	_readThread = new Pt::System::AttachedThread(Pt::callable(*this, &VectorCANDriverImpl::read));
	_readThread->start();

	return true;
}

bool VectorCANDriverImpl::wait(Pt::uint32_t timeout)
{
    return (WaitForSingleObject(_event, timeout) != WAIT_TIMEOUT);
}

void VectorCANDriverImpl::send(const mps::candrv::Message& messageData)
{	
    Pt::System::MutexLock lock(_mutex);
    
    Vevent tevent;

    tevent.tag               = V_TRANSMIT_MSG;
    tevent.tagData.msg.flags = 0;
    tevent.tagData.msg.id    = messageData.identifier();
    tevent.tagData.msg.dlc   = messageData.dlc();

    memcpy(&tevent.tagData.msg.data[0], messageData.data(), 8);

    Vstatus vErr = ncdTransmit(_portHandle, _channelMask, &tevent);

    if( vErr != VERR_QUEUE_IS_FULL && vErr != VSUCCESS) 
    {
	    logger_log_error(_logger,"send()  failed : code = "<<vErr);
	    std::stringstream ss;
	    ss<<"Send message failed internal error: "<<vErr;
	    throw mps::candrv::CANException(ss.str(),mps::candrv::CANException::SendError);
    }   
}

void VectorCANDriverImpl::read()
{    
    mps::candrv::Message messageData;

    while(_running)
    {
        _mutex.lock();

		Vevent sEvent;
		memset( &sEvent, 0, sizeof( Vevent ));

		int			nCount = 1;
		Vstatus		vErr;
	        
		vErr = ncdReceive( _portHandle, VCAN_POLL, 0, &nCount, &sEvent );

		_mutex.unlock();

		if( vErr != VSUCCESS || nCount <= 0 ||sEvent.tag != V_RECEIVE_MSG )
		{            
			wait(100000);
			continue;
		}	            

		messageData.setTimeStamp( sEvent.timeStamp );
        messageData.setIdentifier( sEvent.tagData.msg.id );
		messageData.setDlc( sEvent.tagData.msg.dlc );
        memcpy( messageData.data(), sEvent.tagData.msg.data, 8 );		
        onMessage(messageData);
    }		
}

void VectorCANDriverImpl::reset()
{
    Pt::System::MutexLock lock(_mutex);

	ncdFlushTransmitQueue( _portHandle, _channelMask );
	ncdFlushReceiveQueue( _portHandle );
	ncdResetClock ( _portHandle );
}
	
void VectorCANDriverImpl::close()
{
	if(_portHandle != INVALID_PORTHANDLE)
	{
        _running = false;
        SetEvent(_event);
        _readThread->join();
        delete _readThread;
        _readThread = 0;

		ncdClosePort(_portHandle);
		_portHandle = INVALID_PORTHANDLE;
		CloseHandle(_event);
	}
}

bool VectorCANDriverImpl::regApplication(Pt::uint32_t nHWType)
{	
   int nHwIndex;
   
   _hwType = nHWType;

   Vstatus vErr = ncdGetApplConfig(_appName, _port, &_hwType, &nHwIndex, &_hwChannel);

   if( vErr || HWTYPE_NONE == _hwType)
   {
	   _hwType	  = nHWType;
	   nHwIndex	  = 0;
	   _hwChannel = _port;

	   vErr = ncdSetApplConfig( (char *)_appName, _port, _hwType, nHwIndex, _hwChannel );
   }

   if(vErr)
   {
        logger_log_error(_logger,"ncdSetApplConfig() failed : code = "<<vErr);
   }

   return (vErr == 0);
}

}}}
