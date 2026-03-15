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
#ifndef MPS_DIATP_TPHANDLER_H
#define MPS_DIATP_TPHANDLER_H

#include <mps/diatp/mps-diatp.h>
#include <mps/diatp/TPDUAddress.h>
#include <mps/diatp/TPDU.h>
#include <Pt/System/Thread.h>
#include <Pt/System/Condition.h>
#include <Pt/Connectable.h>

namespace mps{
namespace diatp{

class MPS_DIA_API TPHandler : public Pt::Connectable
{
public:
    virtual ~TPHandler()
    { }

    virtual void send(const TPDU& requestPdu) = 0;

    virtual bool waitForSID(Pt::uint8_t sid, size_t timeout) = 0;

    virtual bool readSID(Pt::uint8_t sid, TPDU& pdu) = 0;

    virtual void reset(bool hardware) = 0;

    virtual void start() = 0;

    virtual void stop() = 0; 

protected:
    TPHandler()
    { }

};

}}

#endif
