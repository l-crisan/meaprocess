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
#ifndef MPS_CORE_RECORDBUILDER_H
#define MPS_CORE_RECORDBUILDER_H

#include <mps/core/Api.h>
#include <Pt/Types.h>
#include <vector>

namespace mps{
namespace core{

/**@brief This class is used to create a data records.
*
* The elements are build as following:<p>
* Element2:  [item1, item2, item3]<p>
* Element1:  [item1, item2, item3]<p>
*/
class MPS_CORE_API RecordBuilder
{
public:
    /**@brief Default constructor.*/
    RecordBuilder();
    
    /**@brief Destructor.*/
    virtual ~RecordBuilder();
    
    /**@brief Initialize the output buffer.
    *
    * @param noOfElements Total size of the buffer (elements).
    * @param itemSize The size of each item handled by the record builder.
    * @param rates The sample rate of the items in the element.
    * @param outputRate The output rate.
    */
    void init(Pt::uint32_t noOfElements, const std::vector<Pt::uint32_t>& itemSizes, const std::vector<double>& rates, double outputRate);

    /**@brief Insert an item into the buffer.
    *
    * @param index The position of the item into the element.
    */
    bool insert(const Pt::uint8_t* item, Pt::uint32_t index);

    /**@brief Insert an item into the buffer.The item is bit based.
    *
    * @param index The position of the item into the element.
    * @param pivotBit The start bit into the item.
    * @param bitCount The number of bits to insert.
    * @param intel Byteorder
    */
    bool insert(const Pt::uint8_t* item, Pt::uint32_t index, Pt::uint32_t pivotBit, Pt::uint32_t bitCount, bool intel);

    /**@brief Read elements from the buffer. The buffer is circular organised.
    *
    * @param data1 Output data part 1.
    * @param data1count The number of elements readed in the part 1.
    * @param data1 Output data part 2.
    * @param data1count The number of elements readed in the part 2. */
    void get(const Pt::uint8_t** data1, Pt::uint32_t& data1count, const Pt::uint8_t** data2, Pt::uint32_t& data2count) const;

    /**@brief Clear the buffer*/
    void reset();

    /**@brief Gets the element size in bytes.
    *
    * @return Element size in bytes.
    */
    inline Pt::uint32_t elementSize() const
    {
        return _elementSize;
    }
    
 /**@brief Gets the item byte offset.
  *
  * @param[in] item The item index.
  * @return The byte offset.
  */
    inline Pt::uint32_t itemOffset(Pt::uint32_t item) const
    {
        return _itemOffsets[item];
    }

private: 
    bool isLooped() const;
    std::vector<Pt::uint8_t>  _buffer;
    std::vector<Pt::uint32_t> _writeIndex;
    mutable Pt::uint32_t	  _readIndex;
    Pt::uint32_t			  _elementSize;
    Pt::uint32_t			  _noOfElements;
    mutable std::vector<bool> _looped;
    mutable std::vector<bool> _firstSample;
    std::vector<double>		  _sampleIncrement;
    std::vector<double>       _rates;
    double                    _outputRate;
    std::vector<Pt::uint32_t>       _itemSizes; 
    std::vector<Pt::uint32_t>       _itemOffsets;
};

}}

#endif
