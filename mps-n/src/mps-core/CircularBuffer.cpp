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
#include <mps/core/CircularBuffer.h>
#include <Pt/SourceInfo.h>
#include <memory.h>
#include <exception>
#include <algorithm>

namespace mps{
namespace core{

CircularBuffer::CircularBuffer(void)
: _bufferTotalSize(0)
, _elementSize(0)
, _bufferSizeInByte(0)
, _begin(0)
, _writer(0)
, _reader(0)
, _end(0)
, _current(0)
, _count(0)
, _tempBuffer()
{}

CircularBuffer::~CircularBuffer(void)
{ 
    exit();
}

void CircularBuffer::init( Pt::uint32_t noOfElements, Pt::uint32_t elementSize )
{
   exit();

    _bufferTotalSize  = noOfElements;
    _elementSize      = elementSize;	
    _bufferSizeInByte = _bufferTotalSize * _elementSize;

    _begin  = new Pt::uint8_t[_bufferSizeInByte + elementSize];
    _writer = _begin;
    _reader = _begin;
    _end    = _begin + _bufferSizeInByte;	
    _count  = 0;
}

Pt::uint32_t CircularBuffer::insert( const Pt::uint8_t* elements, Pt::uint32_t count )
{	
    if ( (_count + count)  > _bufferTotalSize)
        count = (_bufferTotalSize - _count);

    if( count == 0)
        return 0;

    _count += count;
    
    const Pt::uint32_t bytesToInsert = _elementSize * count;

    if( (_writer + bytesToInsert) >= _end )
    { // We need to chunk.

        //Copy the first part to the end.
        const Pt::uint32_t pdiff = (Pt::uint32_t)(_end - _writer);

        if( pdiff != 0 )
            memcpy( _writer, elements, pdiff );
            
        //Copy the second part to the begin.
        elements += pdiff;

        const Pt::uint32_t newOffset = bytesToInsert - pdiff;
        memcpy( _begin, elements, newOffset ); 

        _writer  = _begin + newOffset ;		
        return count;
    }
    
    //Copy the bytes to buffer
    memcpy( _writer, elements, bytesToInsert );
    _writer += bytesToInsert;	
    return count;
}

bool CircularBuffer::insert(const Pt::uint8_t* element)
{
    if( isFull())
        return false;

    memcpy( _writer, element, _elementSize );

    if( _writer + _elementSize >= _end )
        _writer = _begin;
    else
        _writer += _elementSize;

    _count++;

    return true;
}

const Pt::uint8_t* CircularBuffer::get() const
{		
    return _reader;
}

void CircularBuffer::next()
{
    if ((_reader + _elementSize) >= _end)
        _reader = _begin;
    else
        _reader += _elementSize;

    _count--;
}

const Pt::uint8_t* CircularBuffer::get(Pt::uint32_t count, Pt::uint32_t& max) const
{
    if( isEmpty())
    {
        max = 0;
        return 0;
    }

    const Pt::uint32_t bytesToGet = count * _elementSize;

    if( _writer > _reader)
    {
        if( (_reader + bytesToGet)  > _writer)
            max = (Pt::uint32_t)( (_writer - _reader) / _elementSize );
        else
            max = count;

        return  _reader;
    }

    // The writer is looped
    const Pt::uint32_t firstPart = (Pt::uint32_t) (_end - _reader);

    if( count  < ( firstPart / _elementSize) )
    {
        max = count;
        return _reader;
    }

    //We need to create a new buffer.
  const Pt::uint32_t secondPart = (Pt::uint32_t) (_writer - _begin);
  const Pt::uint32_t dataSize = firstPart + secondPart;
    
  if(_tempBuffer.size() < dataSize)
      _tempBuffer.resize( dataSize);

    //Copy first part.
    memcpy(&_tempBuffer[0], _reader, firstPart);

    //Copy the second part.			
    if(secondPart != 0)
        memcpy( &_tempBuffer[firstPart], _begin, secondPart);

    //Calc max returned.
    max = (firstPart + std::min((count-firstPart), secondPart)) / _elementSize;

    if( max > count )
        max = count;

    return &_tempBuffer[0];
}

void CircularBuffer::get(Pt::uint32_t count, const Pt::uint8_t** data1, Pt::uint32_t& data1count, const Pt::uint8_t** data2, Pt::uint32_t& data2count) const
{
    *data1 = 0;
    *data2 = 0;
    data1count = 0;
    data2count = 0;

    if( isEmpty())
        return;

    const Pt::uint32_t bytesToGet = count * _elementSize;

    if( _writer > _reader)
    {
        if( (_reader + bytesToGet)  > _writer)
            data1count = (Pt::uint32_t)(_writer - _reader) / _elementSize;
        else
            data1count = count;
        
        *data1 =  _reader;
        return;
    }

    // The writer is looped
    const Pt::uint32_t firstPart = (Pt::uint32_t)(_end - _reader);
    *data1 = _reader;

    const Pt::uint32_t firstPartElements = firstPart / _elementSize;

    if( count < firstPartElements )
    {
        data1count = count;
        return;
    }

    data1count = firstPartElements;

    //Copy the second part.	
    const Pt::uint32_t secondPart = (Pt::uint32_t)(_writer - _begin);
    const Pt::uint32_t secondPartElements = secondPart/ _elementSize;

    if( count < (firstPartElements + secondPartElements))	
        data2count  = count - firstPartElements;
    else
        data2count = secondPartElements;

    *data2 = _begin;
}

void CircularBuffer::next(Pt::uint32_t count)
{
    Pt::uint32_t nextInBytes = count * _elementSize;
    
    _count -= count;

    if( (_reader + nextInBytes) < _end)
    {
        _reader += nextInBytes;
        return;
    }

    const Pt::uint32_t secondPart = nextInBytes - (Pt::uint32_t)(_end - _reader);
    _reader = _begin + secondPart;
}

void CircularBuffer::reset()
{
    _count  = 0;
    _writer = _begin;
    _reader = _begin;
}

void CircularBuffer::exit()
{ 
    if( _begin == 0)
        return;

    delete []_begin; 	
    _begin = 0;
}

}}

