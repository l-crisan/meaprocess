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
#ifndef MPS_CORE_SIGNALDATATYPE_H
#define MPS_CORE_SIGNALDATATYPE_H

#include <Pt/Types.h>
#include <mps/core/Api.h>
#include <mps/core/Object.h>
#include <mps/core/SignalScaling.h>
#include <vector>
#include <string>

namespace mps{
namespace core{

class SignalDataType
{
    public:
        /** @brief The signal data Type  */
        enum Type
        {
            VT_bool     = 1,  ///< Boolean 8 bit type.
            VT_real64   = 2,  ///< Real 64 bit type.
            VT_real32   = 3,  ///< Real 32 bit type.
            VT_uint8_t  = 4,  ///< Unsigned integer 8 bit type.
            VT_int8_t   = 5,  ///< Signed integer 8 bit type.
            VT_uint16_t = 6,  ///< Unsigned integer 16 bit type.
            VT_int16_t  = 7,  ///< Signed integer 16 bit type.
            VT_uint32_t = 8,  ///< Unsigned integer 32 bit type.
            VT_int32_t  = 9,  ///< Signed integer 32 bit type.
            VT_uint64_t = 10, ///< Unsigned integer 64 bit type.
            VT_int64_t  = 11, ///< Signed integer 64 bit type.
            VT_string   = 12, ///< String type.
            VT_object   = 14, ///< Object type.
        };

        static SignalDataType::Type fromString(const std::string& t)
        {
            if( t == "BOOL")
                return VT_bool;

            if( t == "REAL")
                return VT_real32;

            if( t == "LREAL")
                return VT_real64;

            if( t == "USINT")
                return VT_uint8_t;

            if( t == "SINT")
                return VT_int8_t;

            if( t == "UINT")
                return VT_uint16_t;

            if( t == "INT")
                return VT_int16_t;

            if( t == "UDINT")
                return VT_uint32_t;

            if( t == "DINT")
                return VT_int32_t;

            if( t == "ULINT")
                return VT_uint64_t;

            if( t == "LINT")
                return VT_int64_t;

            if( t == "STRING")
                return VT_string;

            if( t == "object")
                return VT_object;
        }

        static std::string toString(SignalDataType::Type t)
        {
            switch( t )
            {
                case VT_bool:
                    return "BOOL";

                case VT_real32:
                    return "REAL";

                case VT_real64:
                    return "LREAL";

                case VT_uint8_t:
                    return "USINT";

                case VT_int8_t:
                    return "SINT";

                case VT_uint16_t:
                    return "UINT";

                case VT_int16_t:
                    return "INT";

                case VT_uint32_t:
                    return "UDINT";

                case VT_int32_t:
                    return "DINT";

                case VT_uint64_t:
                    return "ULINT";

                case VT_int64_t:
                    return "LINT";

                case VT_string:
                    return "STRING";

                case VT_object:
                    return "object";

                default:
                    return "";
            }
            return "";
        }
};
}}

#endif
