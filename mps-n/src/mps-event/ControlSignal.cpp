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
#include "ControlSignal.h"

namespace mps{
namespace eventps{

ControlSignal::ControlSignal(Pt::uint32_t id)
:Signal(id)
, _lastValue(0)
{
	registerProperty( "ctrlType", *this, &ControlSignal::ctrlType, &ControlSignal::setCtrlType );
	registerProperty( "inSignal", *this, &ControlSignal::inSignal, &ControlSignal::setInSignal );
	registerProperty( "windowType", *this, &ControlSignal::windowType, &ControlSignal::setWindowType );
	registerProperty( "lower", *this, &ControlSignal::lower, &ControlSignal::setLower );
	registerProperty( "upper", *this, &ControlSignal::upper, &ControlSignal::setUpper );
	registerProperty( "slopeType", *this, &ControlSignal::slopeType, &ControlSignal::setSlopeType );
	registerProperty( "slopeValue", *this, &ControlSignal::slopeValue, &ControlSignal::setSlopeValuer );
	registerProperty( "alteration", *this, &ControlSignal::alteration, &ControlSignal::setAlteration );
	registerProperty( "absolut", *this, &ControlSignal::absolut, &ControlSignal::setAbsolut );
	registerProperty( "signalIf", *this, &ControlSignal::signalIf, &ControlSignal::setSignalIf );
	registerProperty( "outValOnTrue", *this, &ControlSignal::outValOnTrue, &ControlSignal::setOutValOnTrue );
	registerProperty( "outValOnFalse", *this, &ControlSignal::outValOnFalse, &ControlSignal::setOutValOnFalse );	
}
 
ControlSignal::~ControlSignal()
{

}

}}
