/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#ifndef MPS_CAN_VECTORCANDRIVERIMPL_H
#define MPS_CAN_VECTORCANDRIVERIMPL_H

#include <VCanD.h>
#include <windows.h>
#include <Pt/System/Logger.h>
#include <Pt/System/Thread.h>
#include <Pt/System/Mutex.h>
#include <Pt/Signal.h>
#include <mps/candrv/Message.h>
#include <mps/candrv/CANDriverImpl.h>

namespace mps{
namespace can{
namespace vector{

class VectorCANDriverImpl : public mps::candrv::CANDriverImpl
{
public:
	VectorCANDriverImpl();
	virtual ~VectorCANDriverImpl();

	bool open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedID);	

	void send(const mps::candrv::Message& messageData);

	void reset();

	void close();

    bool extendedID() const
    {
        return _extendedID;
    }

	Pt::uint32_t bitRate() const
	{
		return _bitRate;
	}

	const std::string& deviceID() const
	{
		return _deviceID;
	}
	
	Pt::uint8_t port() const
	{
		return _port;
	}

private:
    bool wait(Pt::uint32_t timeout);
    void read();
	bool regApplication(Pt::uint32_t nHWType);

	VportHandle		_portHandle;
	unsigned long	_baudRate;
	unsigned char	_btr0;
	unsigned char	_btr1;
	char			_appName[50];
	Vaccess			_channelMask;
	int				_outputMode;
	Vaccess			_initMask;
	Vaccess			_permissionMask;
	VDriverConfig*	_pDriverConfig;
	unsigned char	_debugLevel;
	unsigned int	_timerRate;
	int				_hwChannel;
	int				_hwType;
	Pt::uint8_t		_port;
	bool			_extendedID;
	std::string    _deviceID;
	Pt::System::Logger _logger;
	HANDLE			_event;    
    bool            _running;
	Pt::uint32_t    _bitRate;

    Pt::System::AttachedThread* _readThread;
    Pt::System::Mutex _mutex;
};

}}}
#endif
