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
#ifndef MPS_CORE_SIGNALLIST_H
#define MPS_CORE_SIGNALLIST_H

#include <Pt/Types.h>
#include <mps/core/ObjectVector.h>
#include <mps/core/Signal.h>

namespace mps{
namespace core{


/** @brief Implements a signal list 
* 
* The base class is std::vector, you can use all the methods of the std::vector
*/
class MPS_CORE_API SignalList : public ObjectVector<Signal*>
{
public:
    /** @brief Default constructor */
    SignalList(void);
    
    /** @brief Destructor */
    virtual ~SignalList(void);
    
    /** @brief Called by the fremework to initialise the signal list */
    virtual void onInitInstance();

    /** @brief Called by the fremework to deinitialise the signal list */
    virtual void onExitInstance();

    /** @brief Return the data size of the signal list
    * 
    *  Example: sizeof(DataTypeOfSignal1) + sizeof(DataTypeOfSignal2)+ ...
    * @return The data size of the signal list.*/
    inline Pt::uint32_t dataSize() const 
    { return _dataSize; }

    /** @brief Returns teh signal index in the list.
    *
    * @param[in] signal The signal.
    * @return The signal index in the list.*/
    Pt::uint32_t getSignalIndex(const Signal* signal) const;

    /** @brief Return the signal data offset in the signal list*/
    inline Pt::uint32_t sigDataOffset(Pt::uint32_t index) const
    {
        return _sigDataOffsetArr[index];
    }

private:
    Pt::uint32_t signalRef() const;
    void addSignalRef(Pt::uint32_t ref);

    std::vector<Pt::uint32_t> _signalRefIDList;
    Pt::uint32_t              _dataSize;
    std::vector<Pt::uint32_t> _sigDataOffsetArr;
};

}}

#endif
