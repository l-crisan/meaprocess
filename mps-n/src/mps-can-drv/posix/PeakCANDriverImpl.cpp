/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#include "PeakCANDriverImpl.h"
#include <mps/candrv/CANException.h>
#include <Pt/System/Clock.h>

namespace mps{
namespace candrv{

PeakCANDriverImpl::PeakCANDriverImpl()
: _extendedID(false)
, _running(false)
, _bitRate(0)
, _deviceID("")
, _port(0)
{

}

PeakCANDriverImpl::~PeakCANDriverImpl()
{
}

bool PeakCANDriverImpl::open(const std::string&  deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
    return false;
}

bool PeakCANDriverImpl::wait(Pt::uint32_t timeout)
{
	return false;
}

void PeakCANDriverImpl::send(const Message& messageData)
{	
}

void PeakCANDriverImpl::read()
{
}

void PeakCANDriverImpl::reset()
{
}

void PeakCANDriverImpl::close()
{
}

}}
