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
: _running(false)
, _extendedID(false)
, _port(0)
, _deviceID("")
, _bitRate(0)
{
}

SoftingCANDriverImpl::~SoftingCANDriverImpl()
{	
}

bool SoftingCANDriverImpl::open(const std::string&  deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
	return false;
}

bool SoftingCANDriverImpl::wait(Pt::uint32_t timeout)
{
	return false;
}

void SoftingCANDriverImpl::send(const Message& messageData)
{	
}

void SoftingCANDriverImpl::read()
{
}

void SoftingCANDriverImpl::reset()
{
}

void SoftingCANDriverImpl::close()
{
}

}}
