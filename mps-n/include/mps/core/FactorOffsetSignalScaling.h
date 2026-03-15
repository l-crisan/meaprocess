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
#ifndef MPS_CORE_FACTOROFFSETSIGNALSCALING_H
#define MPS_CORE_FACTOROFFSETSIGNALSCALING_H

#include <mps/core/Api.h>

#include <mps/core/SignalScaling.h>
#include <string>

namespace mps {
namespace core{

/**@brief Implements the factor offset signal scaling.*/
class MPS_CORE_API FactorOffsetSignalScaling : public SignalScaling
{
public:
    /**@brief Default constructor*/
    FactorOffsetSignalScaling(void);
    
    /**@brief Destructor*/
    virtual ~FactorOffsetSignalScaling(void);

    /**@brief Gets the scaling representation as xml string.
    *  @param objectID The xml object id.
    *  @return The xml string of the signal scaling.*/
    virtual std::string getScalingObjectAsXMLString(Pt::uint32_t objectID);

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
    /**@brief Gets the scaling factor.*/
    double getFactor() const;
    
    /**@brief Sets the scaling factor.*/
    void setFactor(double factor);

    /**@brief Gets the scaling offset.*/
    double getOffset() const;

    /**@brief Sets the scaling offset.*/
    void setOffset(double dbOffset);

private:
    /**@brief The scaling factor*/
    double _factor;
    
    /**@brief The scaling offset*/
    double _offset;
};
}}
#endif
