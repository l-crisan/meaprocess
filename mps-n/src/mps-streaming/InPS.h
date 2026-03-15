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
#ifndef MPS_STREAMING_INPS_H
#define MPS_STREAMING_INPS_H

#include "StreamingSignal.h"
#include "StreamingPSBase.h"
#include <mps/core/Signal.h>
#include <mps/core/FiFoSynchSourcePS.h>
#include <mps/stream/prot/ReceiveProtocol.h>
#include <mps/stream/prot/MeaReceiveProtocol.h>
#include <map>


namespace mps{
namespace streaming{

class InPS : public mps::core::FiFoSynchSourcePS, public StreamingPSBase
{
public:
    InPS(void);

    virtual ~InPS(void);

    virtual void onInitInstance();

    virtual void onInitialize();

    virtual void onStart();

    virtual void onStop();

    virtual void onDeinitialize();
    
    virtual void onExitInstance();

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

    void onAddNewDevice(Pt::System::IODevice* device);

    void onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data);

    const std::string& onGetSignalList(const std::string& connection) const;

private:
    void readData();

    void onDataAvailable(Pt::System::IODevice& device);

    void sendWrongSigListError(const mps::stream::prot::ReceiveProtocol* prot);

    void onProtocolParsedData(const mps::stream::prot::ReceiveProtocol::DataInfo* dataInfo, bool& ready);
    
private:
    std::string                          _connection;
    std::vector<Pt::uint8_t>             _buffer;
    std::string                          _ip;
    Pt::uint32_t                         _port;
    Pt::Net::TcpSocket*                  _socket;
    Pt::System::AttachedThread*          _readThread;
    Pt::System::MainLoop                 _loop;
    bool                                 _errorState;
    bool                                 _dataReady;
    mps::stream::prot::ReceiveProtocol*  _protocol;
    std::string                          _signalList;
};


}}

#endif
