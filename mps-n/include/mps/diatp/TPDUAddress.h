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

#ifndef MPS_DIATP_TPDUADDRESS_H
#define MPS_DIATP_TPDUADDRESS_H

#include <mps/can/drv/Driver.h>
#include <mps/diatp/TPException.h>
#include <Pt/Types.h>
#include <map>

namespace mps{
namespace diatp{

namespace TimeUnit
{
    enum Unit
    {
        MilliSec,
        MicroSec
    };
}

class MPS_DIA_API TPDUAddress
{
public:
    enum AddressType
    {
        NormalFixedPhysical = 0, ///< Physical source + target address
        NormalFixedFunctional,   ///< Functional source + target address
        Extended,                ///< Identifier + target address in extended byte(used by gateway)
        MixedPhysical29Bit,      ///< Physical source + target + extended (remote) address in extended byte (used by gateway)
        MixedFunctional29Bit,    ///< Functional source + target + extended (remote) address in extended byte (used by gateway)
        Mixed11Bit,              ///< Identifier + extended (remote) address in extended byte (used by gateway)
        Normal11Bit              ///< Identifier as defined in ISO
    };

    TPDUAddress(Pt::uint32_t sourceAdrOrUniqID, Pt::uint8_t targetAdr, AddressType type, Pt::uint8_t adrExt = 0);

    TPDUAddress(Pt::uint32_t identifier, bool isExtendedID, Pt::uint8_t adrExt = 0);

    Pt::uint32_t identifier() const;

    bool isExtAdr() const;

    Pt::uint8_t adrExt() const;
    
    Pt::uint32_t sourceAdrOrUniqID() const;

    Pt::uint8_t targetAdr()const;

    AddressType adrressType() const;

    bool isExtendedID() const;

    TPDUAddress getComplementAddress() const;

private:
    AddressType _type;
    Pt::uint32_t _sourceAdr;
    Pt::uint8_t _targetAdr;
    Pt::uint32_t _identifier;
    Pt::uint8_t _adrExt;
    bool _isExtAdr;
    bool _extendedID;

private:
    static void initAddressMap();

    static std::map<Pt::uint32_t,Pt::uint32_t> _addressMap;
};

}}

#endif
