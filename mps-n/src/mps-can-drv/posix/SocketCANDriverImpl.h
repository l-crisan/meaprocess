/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#ifndef MPS_CANDRV_SOCKETCANDRIVERIMPL_H
#define MPS_CANDRV_SOCKETCANDRIVERIMPL_H

#include <Pt/System/Logger.h>
#include <Pt/System/Thread.h>
#include <Pt/System/Mutex.h>
#include <Pt/Signal.h>
#include <mps/candrv/Message.h>
#include <mps/candrv/CANDriverImpl.h>
#include <string>

namespace mps{
namespace candrv{

class SocketCANDriverImpl : public CANDriverImpl
{
public:
	SocketCANDriverImpl();
	~SocketCANDriverImpl();

	bool open(const std::string&  deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId);

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
	static std::string replaceString(const std::string& searchString, const std::string& replaceWith, std::string stringToReplace);

	Pt::System::Logger _logger;
    bool _running;
	bool _extendedID;
	Pt::uint8_t _port;
	std::string _deviceID;
	Pt::uint32_t _bitRate;
	int _skt;
	fd_set _rfds;
	Pt::System::AttachedThread* _readThread;
	Pt::System::Mutex _mutex;

};

}}
#endif
