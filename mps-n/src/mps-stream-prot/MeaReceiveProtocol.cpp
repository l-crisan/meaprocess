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
#include <mps/stream/prot/MeaReceiveProtocol.h>
#include <memory.h>
#include <assert.h>
#include <algorithm>

namespace mps{
namespace stream{
namespace prot{

MeaReceiveProtocol::MeaReceiveProtocol()
{
}

MeaReceiveProtocol::~MeaReceiveProtocol()
{ 
}

void MeaReceiveProtocol::generateSourceKey()
{
    Pt::uint32_t srcId = 0;
    Pt::uint32_t offset = 0;

    //Packet begin
    _cycleBegin  = _headerData[offset] != 0;
    offset++;
    
    //Source 
    memcpy(&srcId, &_headerData[offset],sizeof(Pt::uint32_t));
    offset += sizeof(Pt::uint32_t);
            
    //Rate
    Pt::uint32_t rate = 0; 
    memcpy(&rate, &_headerData[offset],sizeof(Pt::uint32_t));
    offset += sizeof(Pt::uint32_t);

    //Records
    memcpy(&_dataInfo.records, &_headerData[offset], sizeof(Pt::uint32_t));
    offset += sizeof(Pt::uint32_t);

    //Record size
    memcpy(&_receivedRecordSize, &_headerData[offset], sizeof(Pt::uint32_t));
    offset += sizeof(Pt::uint32_t);

    _packetDataSize = _receivedRecordSize * _dataInfo.records;
    assert(_packetDataSize < (1024*1024*40));


    //Create the source mapping key 
    _key  = static_cast<Pt::uint32_t>(rate) ;
    _key |= ((Pt::uint64_t)srcId<<32);
}


void MeaReceiveProtocol::mapSource(Pt::uint32_t clientSrcID, Pt::uint32_t sampleRate, Pt::uint32_t recordSize, Pt::uint32_t sourceIndex)
{
    Pt::uint64_t key = ((Pt::uint64_t)clientSrcID<<32);
    key |= sampleRate;
    SourceInfo srcInfo;

    srcInfo.recordSize = recordSize;
    srcInfo.sourceIndex = sourceIndex;

    std::pair<Pt::uint64_t, SourceInfo> pair(key, srcInfo);
    _sourceMap.insert(pair);
}

void MeaReceiveProtocol::clearSourceMapping()
{
    _sourceMap.clear();
}

void MeaReceiveProtocol::reset()
{
    _key = 0;
    _cycleBegin = false;
    _synchron = false;
    _packetDataSize = 0;
    _synchPos = 0;
    _state = StatePacketSynch;
    _receivedRecordSize = 0;
    _dataInfo.data.reserve(1024*10);
    _dataInfo.data.clear();
    _headerData.reserve(25);
    _headerData.clear();
}

void MeaReceiveProtocol::propagateData(const Pt::uint8_t* data, Pt::uint32_t count)
{
    if(count == 0)
        return;

    switch(_state)
    {
        case StatePacketSynch:
        {
            _dataInfo.data.clear();

            for( Pt::uint32_t i = 0; i < count; ++i)
            {
                if(data[i] == 0xAA)
                {
                    if( _synchPos == 4)
                    {
                        _state = StateHeader;
                        _synchPos= 0;

                        if( (i+1) < count)
                            propagateData(&data[i+1], count -(i+1));

                        return;
                    }
                    else
                    {
                        _synchPos++;
                    }
                }
                else
                {
                    _synchPos= 0;
                }
            }

            _synchPos = 0;
        }
        break;

        case StateHeader:
        {
            Pt::uint32_t rest = HeaderSize - _headerData.size();
            Pt::uint32_t toRead = std::min(rest, count);
            _dataInfo.data.clear();

            for( Pt::uint32_t i = 0; i < toRead; ++i)
                _headerData.push_back(data[i]);

            if( _headerData.size() == (Pt::uint32_t) HeaderSize)
            {
                generateSourceKey();
                _headerData.clear();

                _state = StateData;

                if((count - toRead) > 0)
                    propagateData(&data[toRead],count -toRead);
            }
        }
        break;

        case StateData:
        {
            Pt::uint32_t rest   = _packetDataSize - _dataInfo.data.size();
            Pt::uint32_t toRead = std::min(count, rest);
            
            for( Pt::uint32_t i = 0; i < toRead; ++i)
                _dataInfo.data.push_back(data[i]);

            if( _dataInfo.data.size() == _packetDataSize && _packetDataSize != 0)
            {
                _packetDataSize = 0;

                _state = StatePacketSynch;

                std::map<Pt::uint64_t,SourceInfo>::iterator it = _sourceMap.find(_key);

                if( it == _sourceMap.end() )
                {
                    _cycleBegin = false;
                    _synchron = false;
                    onError(this);
                    return;
                }


                if(_receivedRecordSize != it->second.recordSize)
                {
                    _cycleBegin = false;	
                    _synchron = false;
                    onError(this);
                    return;
                }

                if(_cycleBegin || _synchron)
                {
                    _cycleBegin = false;
                    _synchron = true;

                    _dataInfo.sourceIndex = (Pt::uint32_t) it->second.sourceIndex;
                    _dataInfo.inst = this;

                    onDataAvailable(&_dataInfo, _synchron);
                }

                _dataInfo.data.clear();

                if( (count - toRead) > 0)
                    propagateData(&data[toRead], (count - toRead));
            }
        }
        break;
    }
}
    
}}}

