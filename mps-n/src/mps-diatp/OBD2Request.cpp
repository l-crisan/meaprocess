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
#include <mps/diatp/OBD2Request.h>
#include <iostream>
#include <fstream>

namespace mps{
namespace diatp{

OBD2Request::OBD2Request(TPHandler& udsHandler, const TPDUAddress& address)
: TPRequest(udsHandler,address)
, _errorCode(0)
{
}


OBD2Request::~OBD2Request()
{
}


Pt::uint8_t OBD2Request::lastErrorCode() const
{
    return _errorCode;
}


void OBD2Request::readData()
{
    try
    {
        const std::vector<Pt::uint8_t>& d = response();
        if(d.size() == 0)
            return;

        if(d[0] == 0x7F)
        {//We have a error code received
            _errorCode = d[2];
            return;
        }

        for( size_t i = 1; i < d.size(); ++i)
            _data.push_back(d[i]);
    }
    catch(const std::exception& e)
    {
        std::cerr<<e.what()<<std::endl;
    }
}


const std::vector<Pt::uint8_t>& OBD2Request::getResponse()
{
    _data.clear();

    try
    {
        request();
        _errorCode = 0;

        if(!waitResponse(500))
            return _data;

        readData();
    }
    catch(const std::exception& e)
    {
        std::cerr<<e.what()<<std::endl;
    }
    return _data;
}


const std::vector<Pt::uint8_t>& OBD2Request::readNextResponse()
{
    try
    {
        _errorCode = 0;
        _data.clear();
        
        if(!waitResponse(500))
            return _data;

        readData();
    }
    catch(const std::exception& e)
    {
        std::cerr<<e.what()<<std::endl;
    }

    return _data;
}

}}
