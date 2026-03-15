/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.							       info@atesion.de *
 **************************************************************************/
#include "SocketCANDriverImpl.h"
#include <mps/candrv/CANException.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <sys/ioctl.h>
#include <sys/time.h>
#include <net/if.h>
 
#include <linux/can.h>
#include <linux/can/raw.h>
//#include <linux/can/netlink.h>

#include <string.h>


// At time of writing, these constants are not defined in the headers
#ifndef PF_CAN
#define PF_CAN 29
#endif
 
#ifndef AF_CAN
#define AF_CAN PF_CAN
#endif


namespace mps{
namespace candrv{

SocketCANDriverImpl::SocketCANDriverImpl()
: _logger("mps::candrv::SocketCANDriverImpl")
, _running(false)
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

std::string SocketCANDriverImpl::replaceString(const std::string &searchString, const std::string &replaceWith, std::string stringToReplace) 
{ 
    std::string::size_type pos    = stringToReplace.find(searchString, 0); 
   size_t intLengthSearch	  = searchString.length(); 
	 size_t intLengthReplacment = replaceWith.length(); 

    while(std::string::npos != pos) 
    { 
	stringToReplace.replace( pos, intLengthSearch, replaceWith.c_str()); 
        pos = stringToReplace.find(searchString, pos + intLengthReplacment); 
    } 

    return stringToReplace; 
} 

bool SocketCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{			
	_deviceID	= replaceString("/dev/","",deviceID);
	_extendedID = extendedId;
	_port		= port; //TODO: port??
	_bitRate	= bitRate;

	// Create the socket
	_skt = socket( PF_CAN, SOCK_RAW, CAN_RAW );
 
	if( _skt < 0)
	{
		std::cout<<"SocketCANDriverImpl::open() : create socket failed"<<std::endl;
		PT_LOG_ERROR(_logger,"open() : create socket failed");
		return false;
	}

	// Locate the interface
	struct ifreq ifr;
	memset(&ifr, 0, sizeof(ifr));
	strcpy(ifr.ifr_name, _deviceID.c_str());
	
	int ret = ioctl(_skt, SIOCGIFINDEX, &ifr); // ifr.ifr_ifindex gets filled  with that device's index

	if( ret < 0)
	{
		std::cout<<"SocketCANDriverImpl::open() : ioctl SIOCGIFINDEX failed"<<std::endl;
		PT_LOG_ERROR(_logger,"open()  :ioctl SIOCGIFINDEX failed");
		return false;
	}

/* TODO: baudrate set
    struct ifreq ifr_baudrate;   // Use different structure for setting the baud rate as the ioctls overwrite the structure
	
	memset(&ifr_baudrate, 0, sizeof(ifr_baudrate));

	strcpy(ifr_baudrate.ifr_name, _deviceID.c_str());

	ifr_baudrate.ifr_ifru = bitRate;

    ret = ioctl(_skt, SIOCSCANBAUDRATE, &ifr_baudrate);

	if( ret < 0)
	{
		std::cerr<<"SocketCANDriverImpl::open() : ioctl SIOCSCANBAUDRATE failed"<<std::endl;
		PT_LOG_ERROR(_logger,"open() : ioctl SIOCSCANBAUDRATE failed");
		return false;
	}
*/

	// Select that CAN interface, and bind the socket to it.
	struct sockaddr_can addr;
	addr.can_family = AF_CAN;
	addr.can_ifindex = ifr.ifr_ifindex;
	
	ret = bind( _skt, (struct sockaddr*)&addr, sizeof(addr) );
   
	if( ret < 0)
	{
		std::cout<<"SocketCANDriverImpl::open() : bind failed"<<std::endl;
		PT_LOG_ERROR(_logger,"open() : bind failed");
		return false;
	}

	can_err_mask_t  errMask = CAN_ERR_MASK;
	
	ret = setsockopt(_skt, SOL_CAN_RAW, CAN_RAW_ERR_FILTER, &errMask, sizeof(errMask));

	if( ret < 0)
	{
		std::cout<<"SocketCANDriverImpl::open() : setsockopt CAN_RAW_ERR_FILTER failed"<<std::endl;
		PT_LOG_ERROR(_logger,"open() : setsockopt CAN_RAW_ERR_FILTER failed");
		return false;
	}

	_running = true;
	_readThread = new Pt::System::AttachedThread(Pt::callable(*this, &SocketCANDriverImpl::read));
	_readThread->start();

   return true;
}

bool SocketCANDriverImpl::wait(Pt::uint32_t time)
{
	struct timeval timeout;	

	FD_ZERO(&_rfds);
        FD_SET(_skt, &_rfds);

	timeout.tv_sec = time / 1000;
	timeout.tv_usec = (time % 1000) * 1000;

	int rc = select( FD_SETSIZE, &_rfds, NULL, NULL, &timeout );

	if( rc == -1)
		return false;

	return FD_ISSET(_skt, &_rfds) > 0;
}

void SocketCANDriverImpl::send(const Message& messageData)
{
	Pt::System::MutexLock lock(_mutex);

	struct can_frame frame;

	frame.can_id = messageData.identifier();
	frame.can_id |= _extendedID ? CAN_EFF_FLAG : 0;

	memcpy( &frame.data, messageData.data(), 8 );
	frame.can_dlc = messageData.dlc();
	
	int bytesSent = write(_skt, &frame, sizeof(frame) );   

	if( bytesSent != sizeof(frame))
	{
		std::cout<<"SocketCANDriverImpl::send() : write failed"<<std::endl;
		throw CANException("SocketCANDriverImpl::send() : write failed", CANException::SendError);
	}
}

void SocketCANDriverImpl::read()
{
	Message messageData;

    while(_running)
    {
		if(!wait(10000))
			continue;
		
		if( !_running)
			break;

        _mutex.lock();

        struct can_frame frame;
		
		memset(&frame, 0, sizeof(frame));

		int bytesRead  = ::read(_skt, &frame, sizeof(frame) );   
		
		_mutex.unlock();

        if( bytesRead == 0)
            continue;
		
		if((frame.can_id & CAN_ERR_FLAG) == CAN_ERR_FLAG)
			continue;

		struct timeval timeout;

		if( ioctl(_skt, SIOCGSTAMP, &timeout) > 0 )
		{
			timeout.tv_sec = 0;
			timeout.tv_usec = 0;
		}

		Pt::uint64_t timestamp = (static_cast<Pt::uint64_t>(timeout.tv_sec * 1000000) + timeout.tv_usec)/10;
		messageData.setTimeStamp(timestamp);

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

}}
