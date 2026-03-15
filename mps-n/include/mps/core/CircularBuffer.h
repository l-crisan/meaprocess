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
#ifndef MPS_CORE_CIRCULARBUFFER_H
#define MPS_CORE_CIRCULARBUFFER_H

#include <vector>
#include <Pt/Types.h>
#include <mps/core/Api.h>

namespace mps{
namespace core{

/**@brief A low level circular buffer class.
*
* Remark: This class is not thread safe.
*/
class MPS_CORE_API CircularBuffer
{
public:
    /**@brief Default Constructor */
    CircularBuffer(void);

    /**@brief Destructor*/
    virtual ~CircularBuffer(void);

    /**@brief Initialize the circular buffer 
    *
    *  @param noOfElements Number of elements in the circular buffer.
    *  @param elementSize The size of one element in bytes.
    */
    void init(Pt::uint32_t noOfElements, Pt::uint32_t elementSize );

    /**@brief Insert an elements in the circular buffer.
    *
    *  @param elements The stream of the elements.
    *  @param count The elements count in the stream. 
    *  @return The number of elements inserted.
    */
    Pt::uint32_t insert(const Pt::uint8_t* elements, Pt::uint32_t count);

    /**@brief Insert one element in the circular buffer 
    *
    * @param element The element Stream.
    * @return True if inserted.
    */
    bool insert(const Pt::uint8_t* element);

    /**@brief Gets one element from the circular buffer.
    *  @return The element.
    */
    const Pt::uint8_t* get() const;

    /**@brief Move the read pointer to the next element. */
    void next();

    /**@brief Gets an amount of elements from buffer.
    *
    *  @param count The number of elements to read.
    *  @param max The number of elements readed.
    *  @param return The elements stream.
    */
    const Pt::uint8_t* get(Pt::uint32_t count, Pt::uint32_t& max) const;

    /**@brief Gets an amout of elements from buffer.
    *
    *  This method don't make a copy of wrop'around data.
    *  Returns to pointer and to sizes.
    *
    *  @param count The number of elements to read.
    *  @param data1 The pointer of the first part
    *  @param data1count The number of elements in the first part.
    *  @param data2 The pointer of the second part
    *  @param data2count The number of elements in the second part.
    */
    void get(Pt::uint32_t count, const Pt::uint8_t** data1, Pt::uint32_t& data1count, const Pt::uint8_t** data2, Pt::uint32_t& data2count) const;

    /** @brief Move the read pointer on count elements.
    *
    *   @param count The number of elements to remove.
    */
    void next(Pt::uint32_t count);

    /** @brief Returns the number of elements in the buffer.
    *
    *  @return The number of elements in the buffer.
    */
    inline Pt::uint32_t noOfElements() const
    {
        return _count;
    }

    /** @brief Returns the capacity of the buffer in elements.
    *
    *  @return The capacity of the buffer.
    */
    inline Pt::uint32_t totalSize() const
    {
        return _bufferTotalSize;
    }


    /**@brief Return true if the buffer is empty.
    *
    *  @return True if the buffer is empty.
    */

    inline bool isEmpty() const
    {
        return _count == 0;
    }

    /**@brief Returns the element size in bytes.
    *
    *  @return The element size in bytes.
    */
    inline Pt::uint32_t elementSize() const
    {
        return _elementSize;
    }
    
    /**@brief Return true if the buffer is full.
    *
    *  @return Treu if the buffer is full.
    */
    inline bool isFull() const
    {
        return(_count == _bufferTotalSize);
    }

    /**@brief Resets the buffer. remove all elements.*/
    void reset();
    
private:
    void exit();

    Pt::uint32_t	_bufferTotalSize;
    Pt::uint32_t	_elementSize;
    Pt::uint32_t	_bufferSizeInByte;
    Pt::uint8_t*	_begin;
    Pt::uint8_t*	_writer;
    Pt::uint8_t*	_reader;
    Pt::uint8_t*	_end;
    Pt::uint8_t*	_current;
    Pt::uint32_t	_count;
    mutable std::vector<Pt::uint8_t> _tempBuffer;
};
}}
#endif
