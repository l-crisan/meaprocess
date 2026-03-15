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
#include "TPFlowControlFrame.h"
#include <memory.h>

namespace mps{
namespace diatp{

TPFlowControlFrame::TPFlowControlFrame(const TPDUAddress& address, FlowStatus flowStatus, Pt::uint8_t blockSize, TimeUnit::Unit unit, Pt::uint32_t separationTime)
{
    setIdentifier(address.identifier());
    setDlc(8);
    _extendedID = address.isExtendedID();

    if(!address.isExtAdr())
    {
        Pt::uint8_t* pdata = data();

        memset(&pdata[0],0,8);
        pdata[0] = 48;	
        pdata[0] |= ((Pt::uint8_t)flowStatus);
        pdata[1] = blockSize;
        
        if(unit == TimeUnit::MilliSec)
        {
            pdata[2] = separationTime;
        }
        else
        {
            pdata[2] = separationTime/100 + 241;
        }
    }
    else
    {
        Pt::uint8_t* pdata = data();

        memset(&pdata[0],0,8);
        pdata[0] = address.adrExt();
        pdata[1] = 48;
        pdata[1] |= ((Pt::uint8_t)flowStatus);
        pdata[2] = blockSize;
        
        if(unit == TimeUnit::MilliSec)
        {
            pdata[3] = separationTime;
        }
        else
        {
            pdata[3] = separationTime/100 + 241;
        }
    }
}


TPFlowControlFrame::TPFlowControlFrame(const Message& msg, bool extendedID)
{
    _extendedID = extendedID;
    setIdentifier(msg.identifier());
    setDlc(msg.dlc());
    Pt::uint8_t* pdata = data();
    memcpy(&pdata[0],msg.data(),8);
}


TPFlowControlFrame::TPFlowControlFrame()
{
}


TPFlowControlFrame::~TPFlowControlFrame()
{
}


TPFlowControlFrame::FlowStatus TPFlowControlFrame::flowStatus() const
{
    const Pt::uint8_t* pdata = data();
    TPDUAddress address(identifier(), _extendedID, pdata[0]);
 
    if( address.isExtAdr())
        return (FlowStatus)(pdata[1] & 0x0F);

    return (FlowStatus)(pdata[0] & 0x0F);
}


TimeUnit::Unit TPFlowControlFrame::timeUnit() const
{
    const Pt::uint8_t* pdata = data();
    TPDUAddress address(identifier(), _extendedID, pdata[0]);
    
    if( address.isExtAdr())	
    {
        if(  pdata[3] < 0x7F)
            return TimeUnit::MilliSec;
    }
    else
    {
        if(  pdata[2] < 0x7F)
            return TimeUnit::MilliSec;
    }

    return TimeUnit::MicroSec;
}


Pt::uint32_t TPFlowControlFrame::separationTime() const
{
    const Pt::uint8_t* pdata = data();
    TPDUAddress address(identifier(), _extendedID, pdata[0]);

    if( address.isExtAdr())	
    {
        if(timeUnit() == TimeUnit::MilliSec)
            return pdata[3];

        return (pdata[3] - 0xF1) * 100;
    }
    else
    {
        if(timeUnit() == TimeUnit::MilliSec)
            return pdata[2];

        return (pdata[2] - 0xF1) * 100;
    }
}


Pt::uint8_t TPFlowControlFrame::blockSize() const
{
    const Pt::uint8_t* pdata = data();
    TPDUAddress address(identifier(), _extendedID, pdata[0]);

    if( address.isExtAdr())	
        return pdata[2]; 

    return pdata[1]; 
}

bool TPFlowControlFrame::isFlowControlFrame(const TPDUAddress& address, const Pt::uint8_t* data)
{
    Pt::uint8_t pciByte;
    
    if( address.isExtAdr())
        pciByte = data[1] & 0xF0;
    else
        pciByte = data[0] & 0xF0;
    
    return pciByte == 48; //Flow control Frame
}

}}
