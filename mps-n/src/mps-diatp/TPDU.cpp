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
#include <mps/diatp/TPDU.h>
#include <mps/diatp/TPException.h>

namespace mps{
namespace diatp{


TPDU::TPDU(const TPDUAddress& address, const Pt::uint8_t* pdata, size_t size)
: _address(address)
{
    setData(pdata, size);
}


TPDU::TPDU(const TPDUAddress& address)
: _address(address)
{
}


TPDU::TPDU()
:_address(0, false, 0)
{
}


TPDU::~TPDU()
{
}


void TPDU::clear()
{
    _dataLength = 0;
    _data.clear();
}


void TPDU::setData(const Pt::uint8_t* data, size_t size)
{
    _data.resize(size);
    memcpy(&_data[0], data, size);
}


const Pt::uint8_t* TPDU::data() const
{
    if( _data.size() == 0)
        return 0;

    return &_data[0];
}


size_t TPDU::size() const
{
    return _data.size();
}


void TPDU::setAddress(const TPDUAddress& adr)
{
    _address = adr;
}


const TPDUAddress& TPDU::address() const
{
    return _address;
}


TPDU::FrameType TPDU::addDataMessage(const mps::can::drv::Message& msg, bool extendedID)
{
    _address = TPDUAddress(msg.identifier(), extendedID, msg.data()[0]);

    const Pt::uint8_t* pdata;
    size_t headerLen = 0;

    if( _address.isExtAdr())
    {
        pdata =  &msg.data()[1];
        headerLen = 1;
    }
    else
    {
        pdata = &msg.data()[0];
        headerLen = 0;
    }

    Pt::uint8_t pciByte = pdata[0] & 0xF0;
    
    if( pciByte == 0)
    {//Single frame
        size_t len = pdata[0];
        
        _data.clear();
        _data.resize(len);
        memcpy(&_data[0], &pdata[1], len);

        return SingleFrame;
    }

    if(pciByte == 16)
    {//First frame

        Pt::uint16_t dataLen = (pdata[0] & 0x0F);
        dataLen <<=8;
        dataLen |= pdata[1];
        pdata+= 2;
        headerLen += 2;

        _dataLength = dataLen;
        _data.clear();
        _dataLength -= (msg.dlc() - headerLen);

        for( size_t i = 0; i < msg.dlc() - headerLen; ++i)
            _data.push_back(pdata[i]);

        _sequenceNo = 0;
        return FirstFrame;
    }

    if( pciByte == 32)
    {//Consecutive frame
        
        headerLen++;

        _sequenceNo++;
        size_t sequence = (pdata[0] & 0x0F);

        if( sequence != _sequenceNo)
            throw TPException("Wrong sequence number recieved", TPException::WrongSequenceNumber);

        size_t msgSize = std::min((size_t)_dataLength, (size_t)msg.dlc() - headerLen);

        for( size_t i = 0; i < msgSize; ++i)
            _data.push_back(pdata[i+1]);

        _dataLength -= msgSize;
        
        if( _dataLength <= 0)
            return EndFrame;

        return ConsecutiveFrame;
    }

    return ConsecutiveFrame;
}
}}
