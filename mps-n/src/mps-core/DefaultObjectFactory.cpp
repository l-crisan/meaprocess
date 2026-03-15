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
#include <mps/core/Object.h>
#include <mps/core/ObjectFactory.h>
#include <mps/core/ProcessStation.h>
#include <mps/core/TriggerPort.h>
#include <mps/core/Port.h>
#include <mps/core/FactorOffsetSignalScaling.h>
#include <mps/core/ObjectVector.h>
#include <mps/core/SignalList.h>
#include <mps/core/RuntimeEngine.h>
#include <mps/core/SourceDescription.h>
#include "StopPS.h"
#include "DefaultObjectFactory.h"
#include "SplitterPS.h"
#include "SystemOutPS.h"
#include "SystemInPS.h"
#include "Property.h"
#include "OutputSubPort.h"
#include "SubSchemePS.h"
#include "WritePropertyPS.h"
#include "ReadPropertyPS.h"
#include "PropertySignal.h"

namespace mps{
namespace core{

DefaultObjectFactory::DefaultObjectFactory(void)
{ }

DefaultObjectFactory::~DefaultObjectFactory(void)
{ }

Object* DefaultObjectFactory::createObject( const Pt::String& type, const Pt::String& subType, Pt::uint32_t id )
{
    if(type == L"Mp.MeaProcess")
    { 
        return new RuntimeEngine(); 
    }
    else if( type == L"Mp.Properties")
    {
        return new ObjectVector<Property*>();
    }
    else if( type == L"Mp.Property")
    {
        return new Property();
    }
    else if( type ==L"Mp.Sources")
    {
        return new ObjectVector<SourceDescription*>();
    }
    else if(type == L"Mp.Source")
    {
        return new SourceDescription(id);
    }
    else if( type == L"Mp.InputSubPorts")
    {
        return new ObjectVector<Port*>();
    }
    else if( type == L"Mp.OutputSubPorts")
    {
        return new ObjectVector<OutputSubPort*>();
    }
    else if( subType == L"Mp.Port.Sub")
    {
        return new OutputSubPort();
    }
    else if( type == L"Mp.PS.SubScheme")
    {
        return new SubSchemePS();
    }
    else if(type == L"Mp.PS.List")
    {
        return new ObjectVector<ProcessStation*>();
    }
    else if((subType == L"Mp.Port.Out") || 
            (subType == L"Mp.Port.In"))
    {
        return new Port();
    }
    else if(subType == L"Mp.Scaling.FactorOffset")
    {
        return new FactorOffsetSignalScaling();
    }
    else if(subType == L"Mp.Sig")
    {
        return new Signal(id);
    }
    else if(type == L"Mp.PS.Splitter")
    {
        return new SplitterPS();
    }
    else if( type == L"Mp.PS.Stop")
    {
        return new StopPS();
    }
    else if( type == L"Mp.PS.SysOut")
    {
        SystemOutPS* ps =  new SystemOutPS();
        ps->setSubType(subType.narrow());
        ps->setPSID(id);
        return ps;
    }
    else if( type == L"Mp.PS.WriteProp")
    {
        return new WritePropertyPS();
    }
    else if( type == L"Mp.PS.ReadProp")
    {
        return new ReadPropertyPS();
    }
    else if( type == L"Mp.PS.SysIn")
    {
        SystemInPS* ps = new  SystemInPS();
        ps->setSubType(subType.narrow());
        ps->setPSID(id);
        return ps;
    }
    else if(subType == L"Mp.Port.Trigger")
    {
        return new TriggerPort();
    }
    else if(type == L"Mp.OutputPorts" ||
            type == L"Mp.InputPorts")
    {
        return new ObjectVector<Port*>();
    }
    else if(type == L"Mp.Signals")
    {
        return new ObjectVector<SignalList*>();
    }
    else if(type == L"Mp.SignalList")
    {
        return new SignalList();
    }
    else if(subType == L"Mp.Sig.Prop")
    {
        return new PropertySignal(id);
    }
    return 0;
}

}}
