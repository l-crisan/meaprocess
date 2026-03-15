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
#ifndef MPS_RT_MPSSETTINGS_H
#define MPS_RT_MPSSETTINGS_H

#include <Pt/SerializationInfo.h>
#include <Pt/Types.h>
#include <Pt/Settings.h>
#include <string>
#include "System.h"

namespace mps{
namespace rt{

class MpsSettings
{
public: 
    MpsSettings();
    virtual ~MpsSettings();

    inline Pt::uint32_t port() const
    {
        return _port;
    }

    inline void setPort(Pt::uint32_t p)
    {
        _port = p;
    }

    inline const std::string& runtime() const
    {
        return _runtime; 
    }
    
    inline void setRuntime(const std::string& r)
    {
        _runtime = r;
    }

    inline const std::string& schemeFileLocation() const
    {
        return _schemeFileLoc;
    }

    inline void setSchemeFileLoc(const std::string& loc)
    {
        _schemeFileLoc = loc;
    }
    
    inline void setListenOnNetwork(bool listen)
    {
        _listenOnNetwork = listen;
    }

    inline bool listenOnNetwork() const
    {
        return _listenOnNetwork;
    }

    inline void setListenOnSerialDevice(bool listen)
    {
        _listenOnSerialDevice = listen;
    }

    inline bool listenOnSerialDevice() const
    {
        return _listenOnSerialDevice;
    }

    inline void setSerDev(const std::string& serDev)
    {
        _serDev = serDev;
    }

    inline const std::string& serDev() const
    {
        return _serDev;
    }

    inline void setSerBaudrate(int r)
    {
        _serBaudrate = r;
    }

    inline int serBaudrate() const
    {
        return _serBaudrate;
    }

    inline void setLogChannel(const std::string& c)
    {
        _logChannel = c;
    }

    inline const std::string& logChannel() const
    {
        return _logChannel;
    }

    inline void setLogLevel(const std::string& l)
    {
        _logLevel = l;
    }

    inline const std::string& logLevel() const
    {
        return _logLevel;
    }

    inline const std::vector<std::string>& systemConfiguration() const
    {
        return _systemConfiguration;
    }

    inline void setSystemConfiguration(const std::vector<std::string>& conf)
    {
        _systemConfiguration = conf;
    }

    inline void setHttpPort(Pt::uint32_t port)
    {
        _httpPort = port;
    }

    inline void setListenOnHttp(bool on)
    {
        _listenOnHttp = on;
    }

    inline Pt::uint32_t httpPort() const
    {
        return _httpPort;
    }

    inline bool listenOnHttp() const
    {
        return _listenOnHttp;
    }

    void load(const Pt::Settings& si);

private:
    //runtime
    std::string  _runtime; 
    std::string  _schemeFileLoc;
    std::vector<std::string> _systemConfiguration;

    //logging
    std::string _logChannel;
    std::string _logLevel;
    
    //XmlRpc
    bool _listenOnHttp;
    Pt::uint32_t _httpPort;

    //Network
    bool		 _listenOnNetwork;
    Pt::uint32_t _port;

    //SerialDevice
    bool        _listenOnSerialDevice;  
    std::string _serDev;
    int         _serBaudrate;  
};

void operator>>=(const Pt::SerializationInfo& si_, MpsSettings& settings);
void operator<<=(Pt::SerializationInfo& si, const MpsSettings& settings);

}}
#endif

