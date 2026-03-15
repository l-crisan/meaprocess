/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#ifndef MPS_CANDRV_SOFTINGCANDRIVERIMPL_H
#define MPS_CANDRV_SOFTINGCANDRIVERIMPL_H

#include <windows.h>
#include <Can_def.h>
#include <CANL2.H>
#include <Pt/System/Logger.h>
#include <Pt/System/Thread.h>
#include <Pt/System/Mutex.h>
#include <Pt/Signal.h>
#include <mps/candrv/Message.h>
#include <mps/candrv/CANDriverImpl.h>

namespace mps{
namespace candrv{

class SoftingCANDriverImpl : public CANDriverImpl
{
public:
	SoftingCANDriverImpl();
	~SoftingCANDriverImpl();

	bool open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId);

	void send(const Message& messageData);

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

	Pt::System::Logger _logger;
    bool _running;
	bool _extendedID;
	HANDLE     _event; 
	L2CONFIG _l2config;
	PCHDSNAPSHOT _pbuffer;
	CANL2_CH_STRUCT _ch;
	Pt::uint8_t _port;
	std::string _deviceID;
	Pt::uint32_t _bitRate;
	CAN_HANDLE _portHandle;
    Pt::System::AttachedThread* _readThread;
    Pt::System::Mutex _mutex;
};

}}
#endif
