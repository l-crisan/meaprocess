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
#include <mps/core/RecordBuilder.h>
#include <algorithm>
#include <cmath>
#include <Pt/Byteorder.h>
#include <Pt/SourceInfo.h>
#include <memory.h>
#include <stdexcept>
#include <iostream>

namespace mps{
namespace core{

RecordBuilder::RecordBuilder()
: _readIndex(0)
, _elementSize(0)
, _noOfElements(0)
, _outputRate(0)
{
}

RecordBuilder::~RecordBuilder()
{
}
    
void RecordBuilder::init(Pt::uint32_t noOfElements, const std::vector<Pt::uint32_t>& itemSizes, const std::vector<double>& rates, double outputRate)
{
    _noOfElements = noOfElements;
    
    _itemSizes = itemSizes;

    _itemOffsets.resize(itemSizes.size());

    _elementSize = 0;
    
    for(Pt::uint32_t i = 0; i < itemSizes.size(); ++i)
    {
        _itemOffsets[i] = _elementSize;
        _elementSize += _itemSizes[i];
    }

    
    _buffer.resize(noOfElements * _elementSize);
    _writeIndex.resize( rates.size(), 0 );
    _looped.resize( rates.size(), false);	
    _outputRate = outputRate;
    _rates = rates;
    _sampleIncrement.resize(rates.size(), 0.0);
    reset();
}

void RecordBuilder::reset()
{
    _readIndex = 0;

    for( Pt::uint32_t i = 0; i < _writeIndex.size(); ++i)
    {
        _looped[i] = false;
        _sampleIncrement[i] = 0;
        _writeIndex[i] = 0;
    }
}

bool RecordBuilder::insert(const Pt::uint8_t* item, Pt::uint32_t index)
{
    _sampleIncrement[index] += (_outputRate /_rates[index] );
        
    int i = 0;
                
    for( ; i < (int)_sampleIncrement[index]; ++i)
    {
        memcpy(&_buffer[_writeIndex[index] * _elementSize + _itemOffsets[index]], item, _itemSizes[index]);
        _writeIndex[index]++;

        if( _writeIndex[index] == _noOfElements)
        {
            _writeIndex[index] = 0;
            _looped[index] = true;
        }

        if( _writeIndex[index] == _readIndex)
        {
            _readIndex++;
            if(  _readIndex == _noOfElements)
                _readIndex = 0;
        }
    }

    _sampleIncrement[index] -= i;

    return true;
}

bool RecordBuilder::insert(const Pt::uint8_t* item, Pt::uint32_t index, Pt::uint32_t pivotBit, Pt::uint32_t bitCount, bool intel)
{
    _sampleIncrement[index] += (_outputRate / _rates[index] );
        
    int i = 0;
                
    for( ; i < (int)_sampleIncrement[index]; ++i)
    {
        if( Pt::isLittleEndian())
        {
            if( intel)
            {
                Pt::uint64_t* data = (Pt::uint64_t*) &_buffer[_writeIndex[index] * _elementSize];
                Pt::uint64_t inData = 0;
                memcpy(&inData, &item[0], static_cast<Pt::uint32_t>(std::ceil( bitCount/8.0)));
                inData = inData<< pivotBit;

                const Pt::uint64_t mask1 = (0xFFFFFFFFFFFFFFFFULL<<(bitCount + pivotBit));
                const Pt::uint64_t mask2 = (0xFFFFFFFFFFFFFFFFULL>>(64 - pivotBit));

                *data &= (mask1 | mask2);
                *data |= inData;
            }
            else
            {
                Pt::uint8_t physStartByte   = (Pt::uint8_t) (pivotBit/8);
                Pt::uint8_t shift           = physStartByte * 8;	

                Pt::uint64_t* data = (Pt::uint64_t*) &_buffer[_writeIndex[index] * _elementSize];
                Pt::uint64_t inData = 0;

                Pt::uint32_t bytes = static_cast<Pt::uint32_t>(std::ceil( bitCount/8.0));
            
                Pt::uint64_t mask = 0;

                switch(bytes)
                {
                    case 1:
                    
                        memcpy(&inData, &item[0], bytes);
                        mask = static_cast<Pt::uint64_t>(pow(2.0,(double)bitCount) - 1);
                    break;

                    case 2:
                    {
                        Pt::uint16_t* pdata = (Pt::uint16_t*) &item[0];
                        Pt::uint16_t d = Pt::swab(*pdata);
                        memcpy(&inData, &d, bytes);

                        Pt::uint16_t mask16 = static_cast<Pt::uint16_t>(pow(2.0,(double)bitCount) - 1);
                        mask = Pt::swab(mask16);
                    }
                    break;
                    case 3:
                    {
                        Pt::uint32_t* pdata = (Pt::uint32_t*) &item[0];
                        Pt::uint32_t d = Pt::swab(*pdata);
                        d = d>>8;
                        memcpy(&inData, &d, bytes);
                        Pt::uint32_t mask32 = static_cast<Pt::uint32_t>(pow(2.0,(double)bitCount) - 1);
                        mask = Pt::swab(mask32);
                        mask = mask >>8;
                    }
                    break;
                    case 4:
                    {
                        Pt::uint32_t* pdata = (Pt::uint32_t*) &item[0];
                        Pt::uint32_t d = Pt::swab(*pdata);
                        memcpy(&inData, &d, bytes);
                        Pt::uint32_t mask32 = static_cast<Pt::uint32_t>(pow(2.0,(double)bitCount) - 1);
                        mask = Pt::swab(mask32);
                    }
                    break;
                    case 5:
                    {
                        Pt::uint64_t* pdata = (Pt::uint64_t*) &item[0];
                        Pt::uint32_t d = static_cast<Pt::uint32_t>(Pt::swab(*pdata));
                        d = d>>(3*8);
                        memcpy(&inData, &d, bytes);
                        Pt::uint64_t mask64 = static_cast<Pt::uint64_t>(pow(2.0,(double)bitCount) - 1);
                        mask = Pt::swab(mask64);
                        mask = mask>>(3*8);
                    }
                    break;
                    case 6:
                    {
                        Pt::uint64_t* pdata = (Pt::uint64_t*) &item[0];
                        Pt::uint32_t d =  static_cast<Pt::uint32_t>(Pt::swab(*pdata));
                        d = d>>(2*8);
                        memcpy(&inData, &d, bytes);
                        Pt::uint64_t mask64 = static_cast<Pt::uint64_t>(pow(2.0,(double)bitCount) - 1);
                        mask = Pt::swab(mask64);
                        mask = mask >>(2*8);
                    }
                    break;
                    case 7:
                    {
                        Pt::uint64_t* pdata = (Pt::uint64_t*) &item[0];
                        Pt::uint32_t d =  static_cast<Pt::uint32_t>(Pt::swab(*pdata));
                        d = d>>8;
                        memcpy(&inData, &d, bytes);
                        Pt::uint64_t mask64 = static_cast<Pt::uint64_t>(pow(2.0,(double)bitCount) - 1);
                        mask = Pt::swab(mask64);
                        mask = mask >>8;
                    }
                    break;
                    case 8:
                    {
                        Pt::uint64_t* pdata = (Pt::uint64_t*) &item[0];
                        Pt::uint32_t d =  static_cast<Pt::uint32_t>(Pt::swab(*pdata));
                        memcpy(&inData, &d, bytes);
                        Pt::uint64_t mask64 = static_cast<Pt::uint64_t>(pow(2.0,(double)bitCount) - 1);
                        mask = Pt::swab(mask64);
                    }
                    break;
                }

                inData = inData<< shift;
                mask   = mask <<shift;
                mask = ~mask;
                    
                *data &= mask;
                *data |= inData;
            }
        }
        else
        {//TODO: on big endian machines
            throw std::runtime_error(PT_SOURCEINFO + " for BigEndian CPU not implemented");
        }

        _writeIndex[index]++;

        if( _writeIndex[index] == _noOfElements)
        {
            _writeIndex[index] = 0;
            _looped[index] = true;
        }

        if( _writeIndex[index] == _readIndex)
        {
            _readIndex++;
            if(  _readIndex == _noOfElements)
                _readIndex = 0;
        }
    }

    _sampleIncrement[index] -= i;

    return true;
}

bool RecordBuilder::isLooped() const
{
    for(Pt::uint32_t i = 0; i <_looped.size(); ++i)
    {
        if(!_looped[i])
            return false;
    }

    return true;
}

void RecordBuilder::get(const Pt::uint8_t** data1, Pt::uint32_t& data1count, const Pt::uint8_t** data2, Pt::uint32_t& data2count) const
{
    Pt::uint32_t writeIndex = _noOfElements + 1;

    data1count = 0;
    data2count = 0;
    *data1     = 0;
    *data2     = 0;
    
    for(Pt::uint32_t i = 0; i < _writeIndex.size(); ++i)
    {
        if( _looped[i])
            writeIndex = std::min(writeIndex, _noOfElements);	
        else
            writeIndex = std::min(writeIndex, _writeIndex[i]);	
    }
    
    if(isLooped())
    {//all lopped

        //Extract the first part
        if( writeIndex != _readIndex)
        {
            data1count = _noOfElements - _readIndex;
            *data1 = &_buffer[_readIndex * _elementSize];
            _readIndex = writeIndex;
        }

        //We have a second part => extract the second part.
        writeIndex = _noOfElements + 1;

        //Determinate the minimum
        for(Pt::uint32_t i = 0; i < _writeIndex.size(); ++i)
            writeIndex = std::min(writeIndex, _writeIndex[i]);
                

        if( writeIndex == 0)
            return;

        _readIndex = 0;	

        //Clear the looped flags.
        for(Pt::uint32_t i = 0; i < _looped.size(); ++i)
            _looped[i] = false;

        *data2 = &_buffer[0];
        data2count = writeIndex;
        _readIndex = writeIndex;
    }
    else
    {
        if( writeIndex > _readIndex)
        {
            data1count = writeIndex - _readIndex;

            *data1 = &_buffer[_readIndex * _elementSize];
            _readIndex = writeIndex;
        }
    }
}

}}
