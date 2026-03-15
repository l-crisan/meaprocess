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
#include "SubSchemePS.h"

namespace mps{
namespace core{

SubSchemePS::SubSchemePS()
: _inputSubPorts(0)
, _outputSubPorts(0)
{
}

SubSchemePS::~SubSchemePS()
{
    if( _inputSubPorts != 0)
        delete _inputSubPorts;

    if( _outputSubPorts != 0)
        delete _outputSubPorts;
}

void SubSchemePS::onInitInstance()
{
    ProcessStation::onInitInstance();

    if( _outputPorts != 0)
    {
        for( Pt::uint32_t i = 0; i < _outputPorts->size(); ++i)
        {
            OutputSubPort* outSubPort = _outputSubPorts->at(i);
            outSubPort->setOutPort(_outputPorts->at(i));
            outSubPort->setInputPort(true);
            outSubPort->setPortNumber(i);
            outSubPort->onInitInstance();
        }
    }

    if( _inputSubPorts != 0)
    {
        for( Pt::uint32_t i = 0; i < _inputSubPorts->size(); ++i)
        {
            Port* port = _inputSubPorts->at(i);
            port->setInputPort(false);
            port->setPortNumber(i);
            port->onInitInstance();
        }
    }
}

void SubSchemePS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Port* port, const Pt::uint8_t* data)
{
    //Delegate the input port data
    if( _inputSubPorts != 0)
    {
        Port* inputSubPort = _inputSubPorts->at(port->portNumber());
        inputSubPort->onUpdateDataValue(noOfRecords, sourceIdx, data);
    }
}

void SubSchemePS::addObject(Object* obj, const std::string& type, const std::string& subType)
{
    if(type == "Mp.InputSubPorts")
        _inputSubPorts = (ObjectVector<Port*>*) obj;
    else if(type == "Mp.OutputSubPorts")
        _outputSubPorts = (ObjectVector<OutputSubPort*>*) obj;
    else
        ProcessStation::addObject(obj,type,subType);
}

}}
