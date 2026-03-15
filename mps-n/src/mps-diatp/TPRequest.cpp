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
#include <mps/diatp/TPRequest.h>
#include <cmath>
#include <mps/diatp/TPDU.h>
#include "TPSingleFrame.h"
#include "TPFirstFrame.h"
#include "TPConsecutiveFrame.h"
#include "TPFlowControlFrame.h"
#include <iostream>
#include <fstream>

namespace mps{
namespace diatp{

TPRequest::TPRequest(TPHandler& udsHandler, const TPDUAddress& address, const Pt::uint8_t* data, size_t size)
: _udsHandler(udsHandler)
, _requestPdu(new TPDU(address, data, size))
, _responsePdu(new TPDU())
{

}


TPRequest::TPRequest(TPHandler& udsHandler, const TPDUAddress& address)
: _udsHandler(udsHandler)
, _requestPdu(new TPDU(address))
, _responsePdu(new TPDU())
{
}


void TPRequest::setData(const Pt::uint8_t* data, size_t size)
{
    _requestPdu->setData(data,size);
}


TPRequest::~TPRequest()
{
    delete _requestPdu;
    delete _responsePdu;
}


void TPRequest::request() const
{
    try
    {
        _udsHandler.send(*_requestPdu);
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }
}


bool TPRequest::waitResponse(size_t timeout) const
{
    return _udsHandler.waitForSID(_requestPdu->data()[0] + 0x40, timeout);
}


const std::vector<Pt::uint8_t>& TPRequest::response() const
{
    _data.clear();

    if(!_udsHandler.readSID(_requestPdu->data()[0] +0x40, *_responsePdu))
    {
        _data.clear();
        return _data;
    }

    _data.resize(_responsePdu->size());
    memcpy(&_data[0], _responsePdu->data(), _responsePdu->size());

    return _data;
}


const TPDUAddress& TPRequest::responseAddress() const
{
    return _responsePdu->address();
}

}}
