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
#include <mps/diatp/TPDUAddress.h>

namespace mps{
namespace diatp{

std::map<Pt::uint32_t,Pt::uint32_t> TPDUAddress::_addressMap;

void TPDUAddress::initAddressMap()
{
    if(TPDUAddress::_addressMap.size() != 0)
        return;

    std::pair<Pt::uint32_t, Pt::uint32_t> pair;

    //ECU1
    pair.first = 0x7E0;
    pair.second = 0x7E8;
    TPDUAddress::_addressMap.insert(pair);

    pair.first = 0x7E8;
    pair.second = 0x7E0;

    TPDUAddress::_addressMap.insert(pair);

    //ECU2
    pair.first = 0x7E1;
    pair.second = 0x7E9;
    TPDUAddress::_addressMap.insert(pair);

    pair.first = 0x7E9;
    pair.second = 0x7E1;
    
    TPDUAddress::_addressMap.insert(pair);

    //ECU3
    pair.first = 0x7E2;
    pair.second = 0x7EA;
    TPDUAddress::_addressMap.insert(pair);

    pair.first = 0x7EA;
    pair.second = 0x7E2;
    
    TPDUAddress::_addressMap.insert(pair);

    //ECU4
    pair.first = 0x7E3;
    pair.second = 0x7EB;
    TPDUAddress::_addressMap.insert(pair);

    pair.first = 0x7EB;
    pair.second = 0x7E3;
    
    TPDUAddress::_addressMap.insert(pair);

    //ECU5
    pair.first = 0x7E4;
    pair.second = 0x7EC;
    _addressMap.insert(pair);

    pair.first = 0x7EC;
    pair.second = 0x7E4;
    
    TPDUAddress::_addressMap.insert(pair);

    //ECU6
    pair.first = 0x7E5;
    pair.second = 0x7ED;
    TPDUAddress::_addressMap.insert(pair);

    pair.first = 0x7ED;
    pair.second = 0x7E5;
    
    TPDUAddress::_addressMap.insert(pair);

    //ECU7
    pair.first = 0x7E6;
    pair.second = 0x7EE;
    TPDUAddress::_addressMap.insert(pair);

    pair.first = 0x7EE;
    pair.second = 0x7E6;
    
    TPDUAddress::_addressMap.insert(pair);


    //ECU8
    pair.first = 0x7E7;
    pair.second = 0x7EF;
    TPDUAddress::_addressMap.insert(pair);

    pair.first = 0x7EF;
    pair.second = 0x7E7;
    
    TPDUAddress::_addressMap.insert(pair);
}


TPDUAddress::TPDUAddress(Pt::uint32_t sourceAdrOrUniqID, Pt::uint8_t targetAdr, AddressType type, Pt::uint8_t adrExt)
: _type(type)
, _sourceAdr(sourceAdrOrUniqID)
, _targetAdr(targetAdr)
, _adrExt(adrExt)
{
    initAddressMap();

    switch(_type)
    {
        case NormalFixedPhysical:
            _identifier = ((Pt::uint32_t)6)<<26;
            _identifier |= (((Pt::uint32_t)218)<<16);
            _identifier |=  ((Pt::uint16_t)targetAdr) <<8;
            _identifier |=  sourceAdrOrUniqID;
            _isExtAdr = false;
            _extendedID = true;
        break;
        
        case NormalFixedFunctional:
            _identifier = ((Pt::uint32_t)6)<<26;
            _identifier |= (((Pt::uint32_t)219)<<16);
            _identifier |=  ((Pt::uint16_t)targetAdr) <<8;
            _identifier |=  sourceAdrOrUniqID;
            _isExtAdr = false;
            _extendedID = true;
        break;
        
        case Extended:
            _isExtAdr = true;
            _adrExt = targetAdr;
            _identifier = sourceAdrOrUniqID;
            _extendedID = true; 
        break;
        
        case MixedPhysical29Bit:
            _identifier = ((Pt::uint32_t)6)<<26;
            _identifier |= (((Pt::uint32_t)206)<<16);
            _identifier |=  ((Pt::uint16_t)targetAdr) <<8;
            _identifier |=  sourceAdrOrUniqID;
            _isExtAdr = true;
            _adrExt = adrExt;
        break;
        
        case MixedFunctional29Bit:
            _identifier = ((Pt::uint32_t)6)<<26;
            _identifier |= (((Pt::uint32_t)205)<<16);
            _identifier |=  ((Pt::uint16_t)targetAdr) <<8;
            _identifier |=  sourceAdrOrUniqID;
            _isExtAdr = true;
            _adrExt = adrExt;
            _extendedID = true;
        break;
        
        case Mixed11Bit:
            _isExtAdr = true;
            _adrExt = adrExt;
            _identifier = sourceAdrOrUniqID;
            _extendedID = false;
        break;
        case Normal11Bit:
            _isExtAdr = false;
            _identifier = sourceAdrOrUniqID;
            _extendedID = false;
        break;
    }
}


TPDUAddress::TPDUAddress(Pt::uint32_t identifier, bool isExtendedID, Pt::uint8_t adrExt)
{
    initAddressMap();

    _identifier = identifier;
    _adrExt = adrExt;
    _extendedID = isExtendedID;

    if( isExtendedID)
    {
        Pt::uint32_t type = _identifier & 0x00FF0000;
        type >>= 16;
        switch(type)
        {
            case  218:
                _type = NormalFixedPhysical;
                _isExtAdr = false;
                _targetAdr = (_identifier & 0x0000FF00)>>8;
                _sourceAdr = (_identifier & 0x000000FF);
            break;
            case 219:
                _type = NormalFixedFunctional;
                _isExtAdr = false;
                _targetAdr = (_identifier & 0x0000FF00)>>8;
                _sourceAdr = (_identifier & 0x000000FF);
            break;
            case 206:
                _type = MixedPhysical29Bit;
                _isExtAdr = true;
                _targetAdr = (_identifier & 0x0000FF00)>>8;
                _sourceAdr = (_identifier & 0x000000FF);
            break;
            case 205:
                _type = MixedFunctional29Bit;
                _isExtAdr = true;
                _targetAdr = (_identifier & 0x0000FF00)>>8;
                _sourceAdr = (_identifier & 0x000000FF);
            break;
            default:
                _type = Extended;
                _isExtAdr = true;
                _sourceAdr = _identifier;
                _adrExt =_targetAdr;
            break;
        }
    }
    else
    {
        _type = Normal11Bit;
        _isExtAdr = false;
        _sourceAdr = _identifier;
    }
}

TPDUAddress TPDUAddress::getComplementAddress() const
{
    switch(adrressType())
    {
        case NormalFixedPhysical:
            return TPDUAddress(targetAdr(), sourceAdrOrUniqID(),NormalFixedPhysical, adrExt()); 
                
        case NormalFixedFunctional:
            throw TPException("The address doesn't has a complement.", TPException::WrongAddress);
        
        case Extended:
            throw TPException("The address doesn't has a complement.", TPException::WrongAddress);
        
        case MixedPhysical29Bit:
            return TPDUAddress(targetAdr(), sourceAdrOrUniqID(),MixedPhysical29Bit, adrExt()); 
        
        case MixedFunctional29Bit:
            throw TPException("The address doesn't has a complement.", TPException::WrongAddress);

        case Mixed11Bit:
            throw TPException("The address doesn't has a complement.", TPException::WrongAddress);

        case Normal11Bit:
        {
            std::map<Pt::uint32_t,Pt::uint32_t>::iterator it = _addressMap.find(identifier());
            
            if( it == _addressMap.end())
                throw TPException("The address doesn't has a complement.", TPException::WrongAddress);

            return TPDUAddress(it->second, isExtendedID());
        }
    }

    throw TPException("The address doesn't has a complement.", TPException::WrongAddress);
    return TPDUAddress(0,false);
}

Pt::uint32_t TPDUAddress::sourceAdrOrUniqID() const
{
    return _sourceAdr;
}


Pt::uint8_t TPDUAddress::targetAdr()const
{
    return _targetAdr;
}

TPDUAddress::AddressType TPDUAddress::adrressType() const
{
    return _type;
}

Pt::uint32_t TPDUAddress::identifier() const
{
    return _identifier;
}

bool TPDUAddress::isExtAdr() const
{
    return _isExtAdr;
}

Pt::uint8_t TPDUAddress::adrExt() const
{
    return _adrExt;
}

bool TPDUAddress::isExtendedID() const
{
    return _extendedID;
}

}}
