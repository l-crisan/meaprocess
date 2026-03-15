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
#ifndef MPS_STREAMING_OUTPS_H
#define MPS_STREAMING_OUTPS_H

#include <vector>

#include <mps/core/ProcessStation.h>
#include <mps/core/CircularBuffer.h>
#include "StreamingPSBase.h"
#include <Pt/System/Thread.h>
#include <Pt/System/Condition.h>
#include <mps/stream/prot/MeaSendProtocol.h>


namespace mps{
namespace streaming{

class OutPS : public mps::core::ProcessStation, public StreamingPSBase
{
public:
    OutPS(void);

    virtual ~OutPS(void);

    virtual void onInitInstance();

    virtual void onExitInstance();

    virtual void onInitialize();

    virtual void onStart();

    virtual void onStop();

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);

    virtual PSType psType() const
    { return ReceptorPS; }

    inline const std::string& connection() const
    {
        return _connection;
    }

    inline void setConnection(const std::string& con)
    {
        _connection = con;
    }

    inline void setSignalList(const std::string& list)
    {
        _signalList = list;
    }

    inline const std::string& signalList() const
    {
        return _signalList;
    }

protected:
    void sendErrorMsgOnCreateConn();

    const std::string& onGetSignalList(const std::string& connection) const;

private:
    struct DataPacketInfo
    {
         DataPacketInfo(Pt::uint32_t s)
         {
             data.resize(s);
         }

         std::vector<Pt::uint8_t> data;
         Pt::uint32_t     size;
    };

//    void buildStreamPacket(std::vector<Pt::uint8_t>& packet, Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Pt::uint8_t* data, Pt::uint32_t recordSize, Pt::uint32_t srcNo, Pt::uint32_t packetsPerSecond, Pt::uint32_t& offset);
    void sendThread();

private:
    bool _errorState;
    std::string _connection;
    mps::stream::prot::SendProtocol* _protocol;

    //Send data
    bool _running;
    Pt::System::Mutex            _sendBufferMutex;
    core::CircularBuffer		 _sendBuffer;	
    Pt::System::AttachedThread*  _sendThread;
    Pt::System::Condition        _sendSignal;
    std::vector<std::vector<DataPacketInfo*> > _memPckPool;
    std::vector<Pt::uint32_t> _memPckPoolIndex;
    std::string _signalList;
};

}}

#endif
