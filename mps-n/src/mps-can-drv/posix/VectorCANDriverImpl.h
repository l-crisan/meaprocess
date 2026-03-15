/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#ifndef MPS_CANDRV_VECTORCANDRIVERIMPL_H
#define MPS_CANDRV_VECTORCANDRIVERIMPL_H

#include <Pt/System/Thread.h>
#include <Pt/System/Mutex.h>
#include <Pt/Signal.h>
#include <mps/candrv/Message.h>
#include <mps/candrv/CANDriverImpl.h>

namespace mps{
namespace candrv{

class VectorCANDriverImpl : public CANDriverImpl
{
public:
	VectorCANDriverImpl();
	virtual ~VectorCANDriverImpl();

	bool open(const std::string&  deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedID);	

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

	const std::string&  deviceID() const
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
	
	Pt::uint8_t		_port;
	bool			_extendedID;
	std::string   _deviceID;
	Pt::uint32_t    _bitRate;
};

}}
#endif
