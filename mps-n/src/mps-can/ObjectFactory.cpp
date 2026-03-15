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
#include "mps-can.h"
#include "ObjectFactory.h"
#include "OutPort.h"
#include "Signal.h"
#include "SourcePS.h"
#include "LoggerSignal.h"
#include "LoggerPS.h"
#include "WritePS.h"
#include "OutChannel.h"
#include "OutputPS.h"
#include "Event.h"
#include "EventPS.h"


namespace mps{
namespace can{


ObjectFactory::ObjectFactory(void)
{
}


ObjectFactory::~ObjectFactory(void)
{
}


std::string ObjectFactory::resourceID() const
{
    return "mps-can";
}


mps::core::Object* ObjectFactory::createObject(const Pt::String& type, const Pt::String& subtype, Pt::uint32_t id)
{
    if(type == L"Mp.CAN.PS.In")
    {
        return new SourcePS();
    }
    else if( type == L"Mp.CAN.PS.Logger")
    {
        return new LoggerPS();
    }
    else if( type == L"Mp.CAN.PS.Write")
    {
        return new WritePS();
    }
    else if( type == L"Mp.CAN.PS.Out")
    {
        return new OutputPS();
    }
    else if( type == L"Mp.CAN.PS.Event")
    {
        return new EventPS();
    }
    else if( type == L"Mp.CAN.Events")
    {
        return new mps::core::ObjectVector<Event*>();
    }
    else if( type == L"Mp.CAN.Event")
    {
        return new Event();
    }
    else if( subtype == L"Mp.CAN.Sig")
    {
        return new Signal(id);
    }
    else if(  subtype == L"Mp.CAN.Port")
    {
        return new OutPort();
    }
    else if( subtype == L"Mp.CAN.Sig.Logger")
    {
        return new LoggerSignal(id);
    }
    
    else if( type ==L"Mp.CAN.OutChannels")
    {
        return new mps::core::ObjectVector<OutChannel*>();
    }
    else if(type == L"Mp.CAN.OutChannel")
    {
        return new OutChannel();
    }
    return 0;
}


extern "C"
{
    MPS_CAN_API mps::core::ObjectFactory* mpsGetFactory()
    {
        static mps::can::ObjectFactory factory;
        return &factory;
    }
}

}}
