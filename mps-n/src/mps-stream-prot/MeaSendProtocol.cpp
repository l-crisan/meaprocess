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
#include <mps/stream/prot/MeaSendProtocol.h>
#include <memory.h>
#include <algorithm>

namespace mps{
namespace stream{
namespace prot{

MeaSendProtocol::MeaSendProtocol()
{
}

MeaSendProtocol::~MeaSendProtocol()
{
}

Pt::uint32_t MeaSendProtocol::formatData(const SendProtocol::DataInfo& info, const Pt::uint8_t* data, std::vector<Pt::uint8_t>& outDataPacket)
{
    SendProtocol::DataInfo  localDataInfo = info;
    Pt::uint32_t            offset = 0;
    Pt::uint32_t            dataSize = 0;	
    const Pt::uint32_t&	packetsSended = _packetCounter[info.sourceIdx];

    //Build the data packet to send
    if( (packetsSended + info.noOfRecords) <= info.packetsPerSecond)
    {
         dataSize = info.recordSize * info.noOfRecords + HEADER_SIZE;

        if(outDataPacket.capacity() < dataSize)
            outDataPacket.reserve(dataSize);

        if(outDataPacket.size() < dataSize)
            outDataPacket.resize(dataSize);

        buildStreamPacket(info, data, offset, outDataPacket);
    }
    else
    {
        const Pt::uint32_t& diff = info.packetsPerSecond - packetsSended;

        //Calculate the total data size
        dataSize = info.recordSize * diff + HEADER_SIZE;

        for( Pt::uint32_t index = diff; index < info.noOfRecords; index += info.packetsPerSecond)
            dataSize += info.recordSize * std::min(info.packetsPerSecond,info.noOfRecords-index) + HEADER_SIZE;

        if(outDataPacket.capacity() < dataSize)
            outDataPacket.reserve(dataSize);

        if(outDataPacket.size() < dataSize)
            outDataPacket.resize(dataSize);

        //Build the data to send packet
        localDataInfo.noOfRecords = diff;

        buildStreamPacket(localDataInfo, data, offset, outDataPacket);

        for( Pt::uint32_t index = diff; index < info.noOfRecords; index += info.packetsPerSecond)
        {
            localDataInfo.noOfRecords = std::min(info.packetsPerSecond,info.noOfRecords-index);
            buildStreamPacket(localDataInfo, &data[index*info.recordSize], offset, outDataPacket);
        }
    }

    return dataSize;
}

void MeaSendProtocol::reset(Pt::uint32_t sources)
{
    _packetCounter.clear();
    _cycleBeginAll.clear();

    for(Pt::uint32_t srcIdx = 0; srcIdx < sources; ++srcIdx)
    {
        _packetCounter.push_back(0);
        _cycleBeginAll.push_back(false);
    }

    _sequenceBegin = true;
}

void MeaSendProtocol::buildStreamPacket(const SendProtocol::DataInfo& info, const Pt::uint8_t* data, Pt::uint32_t& offset, std::vector<Pt::uint8_t>& outDataPacket)
{
    const Pt::uint32_t& dataSize = info.recordSize * info.noOfRecords;

    //Sync packet bits
    memset(&outDataPacket[offset], 0xAA, 5);
    offset += 5;

    outDataPacket[offset] = _sequenceBegin ? 1 : 0;

    offset++;

    //Source
    memcpy(&outDataPacket[offset], &info.srcNo, sizeof(Pt::uint32_t));
    offset += sizeof(Pt::uint32_t);

    //Rate
    memcpy(&outDataPacket[offset], &info.packetsPerSecond, sizeof(Pt::uint32_t));
    offset += sizeof(Pt::uint32_t);

    //Records
    memcpy(&outDataPacket[offset], &info.noOfRecords, sizeof(Pt::uint32_t));
    offset += sizeof(Pt::uint32_t);

    //Record size
    memcpy(&outDataPacket[offset], &info.recordSize, sizeof(Pt::uint32_t));
    offset += sizeof(Pt::uint32_t);	

    memcpy(&outDataPacket[offset], data, dataSize);
    offset += dataSize;

    //Calculate the 
    _sequenceBegin = true;
    _packetCounter[info.sourceIdx] += info.noOfRecords;
        
    if(_packetCounter[info.sourceIdx] == info.packetsPerSecond)
    {
        _packetCounter[info.sourceIdx] = 0;
        _cycleBeginAll[info.sourceIdx] = true;	
    }

    for( Pt::uint32_t n = 0; n < _cycleBeginAll.size(); ++n)
    {
        if(!_cycleBeginAll[n])
        {
            _sequenceBegin = false;
            break;
        }
    }

    if( _sequenceBegin )
    {
        for(Pt::uint32_t p = 0; p < _cycleBeginAll.size(); ++p)
            _cycleBeginAll[p] = false;
    }
}

}}}
