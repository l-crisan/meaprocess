/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#include "SocketCANDriverImpl.h"

namespace mps{
namespace candrv{

SocketCANDriverImpl::SocketCANDriverImpl()
: _running(false)
, _extendedID(false)
, _port(0)
, _deviceID("")
, _bitRate(0)
{
}

SocketCANDriverImpl::~SocketCANDriverImpl()
{	
}

bool SocketCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
   return false;
}

bool SocketCANDriverImpl::wait(Pt::uint32_t timeout)
{
	return false;
}

void SocketCANDriverImpl::send(const Message& messageData)
{	
}

void SocketCANDriverImpl::read()
{
}

void SocketCANDriverImpl::reset()
{
}

void SocketCANDriverImpl::close()
{
}

}}
