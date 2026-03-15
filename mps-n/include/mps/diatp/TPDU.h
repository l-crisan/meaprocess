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
 #ifndef MPS_DIATP_TPDUFRAME_H
#define MPS_DIATP_TPDUFRAME_H

#include <mps/can/drv/mps-candrv.h>
#include <mps/can/drv/Driver.h>
#include <mps/diatp/TPDUAddress.h>
#include <vector>
#include <Pt/Types.h>

namespace mps{
namespace diatp{

class TPDU
{
public:
    enum FrameType
    {
        SingleFrame,
        FirstFrame,
        ConsecutiveFrame,
        EndFrame
    };

    TPDU(const TPDUAddress& address, const Pt::uint8_t* data, size_t size);

    TPDU(const TPDUAddress& address);

    TPDU();

    virtual ~TPDU();

    void setData(const Pt::uint8_t* data, size_t size);

    const Pt::uint8_t* data() const;

    size_t size() const;

    void setAddress(const TPDUAddress& adr);

    const TPDUAddress& address() const;

    void clear();

    FrameType addDataMessage(const mps::can::drv::Message& msg, bool extendedID);

private:
    std::vector<Pt::uint8_t> _data;
    TPDUAddress _address;
    Pt::int32_t _dataLength;
    size_t _sequenceNo;
};

}}

#endif
