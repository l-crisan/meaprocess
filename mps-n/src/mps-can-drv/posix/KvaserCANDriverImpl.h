/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#ifndef MPS_CANDRV_KVASERCANDRIVERIMPL_H
#define MPS_CANDRV_KVASERCANDRIVERIMPL_H

#include <Pt/System/Library.h>
#include <Pt/System/Mutex.h>
#include <Pt/Signal.h>

#include <mps/candrv/Message.h>
#include <mps/candrv/CANDriverImpl.h>


namespace mps{
namespace candrv{

class KvaserCANDriverImpl : public CANDriverImpl
{
public:
	KvaserCANDriverImpl();
	~KvaserCANDriverImpl();

    bool open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId);

	void send(const Message& messageData);

	void reset();

	void close();

	Pt::uint32_t bitRate() const
	{
		return _bitRate;
	}

    bool extendedID() const
    {
        return _extendedID;
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
    void read();

	bool _extendedID;
    bool _running;
	Pt::uint32_t _bitRate;
	const std::string _deviceID;
	Pt::uint8_t _port;
};

}}
#endif
