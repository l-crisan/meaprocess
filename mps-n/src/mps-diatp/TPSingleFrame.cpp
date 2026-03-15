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
#include "TPSingleFrame.h"
#include <stdexcept> 

namespace mps{
namespace diatp{

TPSingleFrame::TPSingleFrame(const TPDUAddress& address, const Pt::uint8_t* dataIn, size_t len)
{
    if(!fitInSingleFrame(address, len))
        throw std::logic_error("Data is to big for a single frame.");

    Pt::uint8_t* pdata = data();
    setIdentifier(address.identifier());
    setDlc(8);

    if(address.isExtAdr())
    {
        memset(data(),0,8);
        pdata[0] = address.adrExt();
        pdata[1] = len;
        Pt::uint8_t* pdata = data();
        memcpy(&pdata[2], &dataIn[0], len);
    }
    else
    {
        memset(data(),0,8);
        Pt::uint8_t* pdata = data();
        pdata[0] = len;
        memcpy(&pdata[1], &dataIn[0], len);
    }
}


bool TPSingleFrame::fitInSingleFrame(const TPDUAddress& address, size_t len)
{
    if(address.isExtAdr() && len > 5)
        return false;
    
    else if(!address.isExtAdr() && len > 6)
        return false;

    return true;
}


TPSingleFrame::TPSingleFrame()
{
}


TPSingleFrame::~TPSingleFrame()
{
}

}}
