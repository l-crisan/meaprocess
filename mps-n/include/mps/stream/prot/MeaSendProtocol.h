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
#ifndef MPS_STREAM_PROT_MEA_PROTOCOL_H
#define MPS_STREAM_PROT_MEA_PROTOCOL_H

#include <vector>
#include <Pt/Types.h>
#include <mps/stream/prot/StreamProtocol.h>
#include <mps/stream/prot/SendProtocol.h>

namespace mps{
namespace stream{
namespace prot{

class MPS_STREAMPROT_API MeaSendProtocol : public SendProtocol
{
public:

    MeaSendProtocol();

    virtual ~MeaSendProtocol();

    virtual Pt::uint32_t formatData(const SendProtocol::DataInfo& info, const Pt::uint8_t* data, std::vector<Pt::uint8_t>& outDataPacket);

    virtual void reset(Pt::uint32_t sources);

private:
    void buildStreamPacket(const SendProtocol::DataInfo& info, const Pt::uint8_t* data, Pt::uint32_t& offset, std::vector<Pt::uint8_t>& outDataPacket);

private:
    enum { HEADER_SIZE = 22 };

    std::vector<bool>         _cycleBeginAll;
    std::vector<Pt::uint32_t> _packetCounter;
    bool                      _sequenceBegin;
};

}}}

#endif

