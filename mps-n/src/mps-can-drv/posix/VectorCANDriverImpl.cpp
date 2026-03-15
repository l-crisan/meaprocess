/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#include "VectorCANDriverImpl.h"
#include <mps/candrv/CANException.h>

namespace mps{
namespace candrv{

VectorCANDriverImpl::VectorCANDriverImpl()
: _port(0)
, _extendedID(false)
, _deviceID("")
, _bitRate(0)
{
}

VectorCANDriverImpl::~VectorCANDriverImpl()
{	

}

bool VectorCANDriverImpl::open(const std::string&  deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedID)
{
	return false;
}

bool VectorCANDriverImpl::wait(Pt::uint32_t timeout)
{
    return false;
}

void VectorCANDriverImpl::send(const Message& messageData)
{	
}

void VectorCANDriverImpl::read()
{
}

void VectorCANDriverImpl::reset()
{
}
	
void VectorCANDriverImpl::close()
{
}

}}
