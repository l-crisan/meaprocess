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
#include "MpsSettings.h"
#include <Pt/SourceInfo.h>
#include <Pt/SerializationSurrogate.h>

namespace mps{
namespace rt{

MpsSettings::MpsSettings()
: _runtime("")
, _schemeFileLoc("")
, _logChannel("file:////opt/atesion/mea/data/mea.log?size=102400&files=3")
, _logLevel("Trace")
, _listenOnHttp(true)
, _httpPort(5040)
, _listenOnNetwork(true)
, _port(5000)
, _listenOnSerialDevice(false)
, _serDev("")
, _serBaudrate(0)
{
}

MpsSettings::~MpsSettings()
{
}

void MpsSettings::load(const Pt::Settings& si)
{
    si["MeaProcess"]["runtime"].get(_runtime);
    si["MeaProcess"]["schemeFileLocation"].get(_schemeFileLoc);
    si["MeaProcess"]["systemConfiguration"].get(_systemConfiguration);
    si["MeaProcess.Logging"]["channel"].get(_logChannel);
    si["MeaProcess.Logging"]["level"].get(_logLevel);
    si["MeaProcess.Network"]["port"].get(_port);
    si["MeaProcess.Network"]["on"].get( _listenOnNetwork);
    si["MeaProcess.XmlRpc"]["port"].get( _httpPort);
    si["MeaProcess.XmlRpc"]["on"].get( _listenOnHttp);
    si["MeaProcess.SerialDevice"]["on"] .get( _listenOnSerialDevice);
    si["MeaProcess.SerialDevice"]["device"].get( _serDev);
    si["MeaProcess.SerialDevice"]["baudrate"] .get( _serBaudrate);
}

void operator<<=(Pt::SerializationInfo& si, const MpsSettings& settings)
{
    throw std::runtime_error(PT_SOURCEINFO + " Not implemented!");
}

}}

