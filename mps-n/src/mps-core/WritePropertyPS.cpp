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
#include "WritePropertyPS.h"
#include <sstream>
#include <Pt/Convert.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <mps/core/Port.h>

namespace mps{
namespace core{


Pt::uint32_t toSize(const std::string& s)
{
    static Pt::DecimalFormat<char> fmt;
    int n = 0;
    Pt::parseInt(s.begin(), s.end(), n, fmt);
    return n;
}

WritePropertyPS::WritePropertyPS()
{
    registerProperty("propMap", *this, &WritePropertyPS::propMap, &WritePropertyPS::setPropMap);
}

WritePropertyPS::~WritePropertyPS()
{
}

void WritePropertyPS::onInitInstance()
{
    ProcessStation::onInitInstance();

    std::stringstream ss;
    std::string		  item;
    std::string		  sigIdStr;
    std::string		  propName;
    Pt::uint32_t	  sigId;

    ss<<_propMap;
    _sig2PropMap.clear();

    try
    {
        while(ss)
        {
            std::stringstream	itemStream;

            std::getline(ss, item, '#');

            if( item.empty())
                continue;

            itemStream<<item;

            std::getline(itemStream, sigIdStr, ';');
            std::getline(itemStream, propName);

            sigId = (Pt::uint32_t) toSize(sigIdStr);

            Sig2PropIt it = _sig2PropMap.find(sigId);

            if( it == _sig2PropMap.end() )
            {
                std::vector<std::string> properties;
                properties.push_back(propName);
                std::pair<Pt::uint32_t, std::vector<std::string> > pair(sigId, properties);
                _sig2PropMap.insert(pair);
            }
            else
            {
                it->second.push_back(propName);
            }
        }
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }

    _propMap.clear();
}

void WritePropertyPS::onUpdateDataValue( Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Port* port, const Pt::uint8_t* data )
{
    Sources sources = port->sources();
    const std::vector<Signal*>& source = sources[sourceIdx];
    Pt::uint32_t sourceSize = port->sourceDataSize(sourceIdx);
    const Pt::uint8_t* lastRecord = &data[sourceSize * (noOfRecords -1)];

    for( Pt::uint32_t srcIdx = 0; srcIdx < source.size(); ++srcIdx)
    {
        const Signal* signal = source[srcIdx];

        Sig2PropIt it = _sig2PropMap.find(signal->signalID());
        
        if( it == _sig2PropMap.end())
            continue;

        const Pt::uint32_t offset = port->signalOffsetInSource(sourceIdx, srcIdx);
        double value = signal->scaleValue(&lastRecord[offset]);	
        
        for(Pt::uint32_t i = 0; i < it->second.size(); ++i)
            this->setPropertyValue(it->second[i].c_str(), value);
    }
}

}}
