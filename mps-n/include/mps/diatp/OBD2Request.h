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
#ifndef MPS_DIATP_OBD2REQUEST_H
#define MPS_DIATP_OBD2REQUEST_H

#include <mps/diatp/mps-diatp.h>
#include <mps/diatp/TPCANHandler.h>
#include <mps/diatp/TPDUAddress.h>
#include <mps/diatp/TPRequest.h>

namespace mps{
namespace diatp{

/*@brief Implements the ODB2 request.*/
class MPS_DIA_API OBD2Request : public TPRequest
{
public:
    
    /*@brief Constructs a new OBD2 request object.
    *
    * @param canHandler  The can bus handler.
    * @param address  The request address.
    */
    OBD2Request(TPHandler& udsHandler, const TPDUAddress& address);

    /*@brief Destructor.*/
    virtual ~OBD2Request();

    /*@brief Read the next responce data.
    *
    * @return The responce data.
    */
    virtual const std::vector<Pt::uint8_t>& readNextResponse();

    /*@brief Sends the request and read the first responce.
    *
    * @return The responce data
    */
    virtual const std::vector<Pt::uint8_t>& getResponse();
    

    /*@brief If the response size is 0, with this can obtain the received error code.
    *
    * @return The received error code.
    */
    Pt::uint8_t lastErrorCode() const;

private:
    void readData();
    std::vector<Pt::uint8_t> _data;
    Pt::uint8_t				 _errorCode;
};

}}

#endif
