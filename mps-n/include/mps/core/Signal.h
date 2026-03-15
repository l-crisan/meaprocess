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
#ifndef MPS_CORE_SIGNAL_H
#define MPS_CORE_SIGNAL_H

#include <Pt/Types.h>
#include <mps/core/Api.h>
#include <mps/core/Object.h>
#include <mps/core/SignalScaling.h>
#include <mps/core/SignalDataType.h>
#include <vector>
#include <string>

namespace mps{
namespace core{

/**@brief The signal parameters.
*
* A parameter has a name and a value. The first in the pair is the parameter name and the second the parameter value.
*/
typedef std::vector< std::pair<std::string, std::string> > Parameters;

/** @brief The base class for each signal.
*
* This class implemenets the signal representation.
* Define the signal data types and the default control signal types.
* Derivate this to create your own signal class.*/
class MPS_CORE_API Signal : public Object
{
public:
    friend class SynchSourcePS;

    /**@brief Default construtor.*/
    Signal(Pt::uint32_t id);

    /**@brief Destructor.*/
    virtual ~Signal(void);

    /** @brief Override this to setup the signal.
    *
    *  The properties of the signal are allready loaded.*/
    virtual void onInitInstance();

    /** @brief Override this to shutdown the signal.*/
    virtual void onExitInstance();

    /** @brief Gets the signal identifier.*/
    const std::string& name() const;

    /** @brief Sets the signal identifier.*/
    void setName(const std::string& name);

    /** @brief Gets the signal unit.*/
    const std::string& unit() const;

    /** @brief Sets the signal unit.*/
    void setUnit(const std::string& unit);

    /** @brief Gets the signal comment.*/
    const std::string& comment() const;

    /** @brief Sets the signal comment.*/
    void setComment(const std::string& comment);

    /** @brief Gets the signal value data type.*/
    Pt::uint8_t valueDataType() const;

    /** @brief Sets the signal value data type.*/
    void setValueDataType( Pt::uint8_t valueType);

    /** @brief Gets the signal value data type as string.*/
    virtual std::string valueDataTypeAsString() const;

    /** @brief Gets the signal physical minimum.*/
    double physMin() const;

    /** @brief Sets the signal physical minimum.*/
    void setPhysMin(double min);

    /** @brief Gets the signal physical maximum.*/
    double physMax() const;

    /** @brief Sets the signal physical maximum.*/
    void setPhysMax(double max);

    /** @brief Gets the signal value data type size.*/
    Pt::uint32_t valueSize() const;

    /** @brief Gets the signal data type size sign.*/
    bool  isValueSigned() const;

    /** @brief Gets the signal samplerate.*/
    double sampleRate() const;

    /** @brief Sets the signal samplerate.*/
    void setSampleRate(double sampleRate);	

    /** @brief Gets the signal scaling object.*/
    SignalScaling* scaling() const;

    /** @brief Gets the signal source id.*/
    Pt::uint32_t sourceNumber() const;	

    /** @brief Sets the signal source id.*/
    void setSourceNumber( Pt::uint32_t number );	

    /**@brief Returns the signal identifier
    * @return The signal identifier.*/
    Pt::uint32_t signalID() const;	

    inline const std::string& cat() const
    {
        return _cat;
    }

    inline void setCat(const std::string& c)
    {
        _cat = c;
    }

    /** @brief Scale a signal value.
    *   @param The byte buffer with the value to scale.
    *   @return The scaled value.*/
    double scaleValue( const Pt::uint8_t* value ) const;

    void addObject( Object* object, const std::string& type, const std::string& subType);


    /** @brief Gets the signal parameters
    *   @return The signal parameters.*/
    const Parameters& getParameters() const;

protected:

    inline void setSignalIndex(Pt::uint32_t index)
    {
        _index = index;
    }

    inline Pt::uint32_t signalIndex() const
    {
        return _index;
    }


    inline Pt::uint32_t objectSize() const
    {
        return _objSize;
    }

    inline void setObjectSize(Pt::uint32_t os)
    {
        _objSize = os;
    }


protected:
    Parameters     _parameters;

private:
    const std::string& parameters() const;

    void setParameters(const std::string& params);

private:
    std::string    _name;
    std::string    _unit;
    std::string    _comment;
    SignalDataType::Type  _dataType;
    double         _physMin;
    double         _physMax;
    SignalScaling* _scaling;
    Pt::uint32_t         _signalValueSize;
    bool           _signed;
    double         _sampleRate;
    Pt::uint32_t   _sourceNumber;
    Pt::uint32_t   _id;
    Pt::uint32_t         _signalIndex;
    std::string	   _cat;	
    Pt::uint32_t         _index;
    Pt::uint32_t	 _objSize;
};

}}

#endif
