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
#include "ClockObjectFactory.h"
#include "mps-clock.h"
#include "ClockPS.h"
#include "ClockSignal.h"
#include "StopWatchPS.h"
#include "StopWatchSignal.h"
#include "TactPS.h"
#include "TimerPS.h"
#include "TimerSignal.h"

namespace mps{
namespace clock{

ClockObjectFactory::ClockObjectFactory(void)
{
}

ClockObjectFactory::~ClockObjectFactory(void)
{
}

std::string ClockObjectFactory::resourceID() const
{
    return "mps-clock";
}

mps::core::Object* ClockObjectFactory::createObject( const Pt::String& type, const Pt::String& subtype, Pt::uint32_t id )
{
    if(type == L"Mp.Clock.PS.Clock")
    {
        return new ClockPS();
    }
    if(type == L"Mp.Clock.PS.StopWatch")
    {
        return new StopWatchPS();
    }
    if(type == L"Mp.Clock.PS.Tact")
    {
        return new TactPS();
    }
    if(type == L"Mp.Clock.PS.Timer")
    {
        return new TimerPS();
    }
    else if( subtype == L"Mp.Clock.Sig.Clock")
    {
        return new ClockSignal(id);
    }
    else if( subtype == L"Mp.Clock.Sig.StopWatch")
    {
        return new StopWatchSignal(id);
    }
    else if( subtype == L"Mp.Clock.Sig.Timer")
    {
        return new TimerSignal(id);
    }
    return 0;
}

extern "C"
{
    MPS_CLOCK_API mps::core::ObjectFactory* mpsGetFactory()
    {
        static ClockObjectFactory factory;
        return &factory;
    }
}


}}
