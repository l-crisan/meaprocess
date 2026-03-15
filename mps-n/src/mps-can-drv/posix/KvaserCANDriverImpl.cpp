/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#include "KvaserCANDriverImpl.h"
#include <mps/candrv/CANException.h>
#include <Pt/System/Clock.h>

namespace mps{
namespace candrv{

KvaserCANDriverImpl::KvaserCANDriverImpl()
: _extendedID(false)
, _running(false)
, _bitRate(0)
, _deviceID("")
, _port(0)
{
}

KvaserCANDriverImpl::~KvaserCANDriverImpl()
{

}

bool KvaserCANDriverImpl::open(const std::string& deviceID, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
    return false;
}

void KvaserCANDriverImpl::send(const Message& messageData)
{	
}

void KvaserCANDriverImpl::read()
{
}

void KvaserCANDriverImpl::reset()
{
}

void KvaserCANDriverImpl::close()
{
}

}}
