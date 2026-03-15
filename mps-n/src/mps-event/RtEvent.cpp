/*
 * Copyright (C) 2015 by Laurentiu-Gheorghe Crisan
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * As a special exception, you may use this file as part of a free
 * software library without restriction. Specifically, if other files
 * instantiate templates or use macros or inline functions from this
 * file, or you compile this file and link it with other files to
 * produce an executable, this file does not by itself cause the
 * resulting executable to be covered by the GNU General Public
 * License. This exception does not however invalidate any other
 * reasons why the executable file might be covered by the GNU Library
 * General Public License.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
#include "RtEvent.h"
#include <mps/core/ProcessStation.h>

namespace mps{
namespace eventps{

RtEvent::RtEvent(void)
:  _message("")
, _signal(0)
, _audioData(0)
, _command("")
, _commandParam("")
, _target(0)
, _limit(0)
, _operation(0)
, _priority(0)
{
	registerProperty("message", *this, &RtEvent::message, &RtEvent::setMessage);
	registerProperty("outputTarget", *this, &RtEvent::outputTarget, &RtEvent::setOutputTarget);
	registerProperty("signal", *this, &RtEvent::signal, &RtEvent::setSignal);
	registerProperty("audioData",*this,&RtEvent::buffer, &RtEvent::setBuffer);
	registerProperty("command",*this,&RtEvent::command, &RtEvent::setCommand);
	registerProperty("commandParam",*this,&RtEvent::commandParam, &RtEvent::setCommandParam);
	registerProperty("limit",*this,&RtEvent::limit, &RtEvent::setLimit);
	registerProperty("operation",*this,&RtEvent::operation, &RtEvent::setOperation);
	registerProperty("priority",*this,&RtEvent::priority, &RtEvent::setPriority);
}

RtEvent::~RtEvent(void)
{
}

void RtEvent::setBuffer(const std::string& b)
{
	mps::core::ProcessStation::base64Decode(b, _audioData);	
}

}}
