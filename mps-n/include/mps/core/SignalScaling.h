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
#ifndef MPS_CORE_SIGNALSCALING_H
#define MPS_CORE_SIGNALSCALING_H

#include <Pt/Types.h>
#include <mps/core/Api.h>
#include <mps/core/Object.h>

namespace mps{
namespace core{

/** @brief Base class for signal scaling
*
* Derive this to implement your own signal scaling. */
class MPS_CORE_API SignalScaling : public Object
{
public:
    /** @brief Destructor.*/
    virtual ~SignalScaling(void);

    /** @brief Return the signal scaling as xml representation 
    *
    * @return The signal scaling*/
    virtual std::string getScalingObjectAsXMLString(Pt::uint32_t startObjectID);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const Pt::uint8_t* value);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const Pt::int8_t* value);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const Pt::int16_t* value);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const Pt::uint16_t* value);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const Pt::int32_t* value);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const Pt::uint32_t* value);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const Pt::int64_t* value);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const Pt::uint64_t* value);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const float* value);

    /**@brief Scale a signal value
    *  @param value The numeric value to scale.
    *  @return The scaled value. */
    virtual double scaleValue(const double* value);

protected:
    /** @brief Default constructor */
    SignalScaling(void);
};

}}

#endif
