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
#ifndef MPS_DIATP_TPREQUEST_H
#define MPS_DIATP_TPREQUEST_H

#include <mps/diatp/mps-diatp.h>
#include <mps/diatp/TPException.h>
#include <mps/diatp/TPHandler.h>

#include <Pt/Types.h>
#include <vector>

namespace mps{
namespace diatp{

class TPDU;

class MPS_DIA_API TPRequest
{
public:

    TPRequest(TPHandler& udsHandler, const TPDUAddress& address, const Pt::uint8_t* data, size_t size);

    TPRequest(TPHandler& udsHandler, const TPDUAddress& address);

    virtual ~TPRequest();

    void setData(const Pt::uint8_t* data, size_t size);

    virtual void request() const;

    bool waitResponse(size_t timeout = 1000) const;
    
    const std::vector<Pt::uint8_t>& response() const;

    const TPDUAddress& responseAddress() const;

protected:
    TPHandler&   _udsHandler;

private:
    std::vector<Pt::uint8_t> _responce;
    
    mutable std::vector<Pt::uint8_t> _data;
    TPDU*_requestPdu;
    TPDU* _responsePdu;
};

}}

#endif
