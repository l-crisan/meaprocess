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
#ifndef MPS_CORE_SOURCEDESCRIPTION_H
#define MPS_CORE_SOURCEDESCRIPTION_H

#include <Pt/Types.h>
#include <mps/core/Object.h>
#include <mps/core/Api.h>

namespace mps {
namespace core {

/**@brief This class describe a data source.*/
class SourceDescription : public Object
{
    public:

        /**@brief Construct a new source description object.*/
        SourceDescription(Pt::uint32_t sourceID);

        /**@brief Gets the source name		
        *
        *  @return The source name
        */
        inline const std::string& name() const
        { return _name; }

        /**@brief Sets the source name.		
        *
        *  @param name The source name.
        */
        inline void setName(const std::string& name)
        { _name = name; }

        /**@brief Gets the source key.
        *
        *  @return The source key.
        */
        inline Pt::uint64_t sourceKey() const
        { return _sourceKey; }

        /** @brief Sets the source key.
        *
        *  @param sourceKey The source key.
        */
        inline void setSourceKey(Pt::uint64_t sourceKey)
        {_sourceKey = sourceKey; }

        /** @brief Gets the source id.
        *
        *  @return The source id.
        */
        inline Pt::uint32_t sourceID() const
        { return _sourceID; }
    private:

        std::string _name;
        Pt::uint64_t _sourceKey;
        Pt::uint32_t _sourceID;
};

}}

#endif
