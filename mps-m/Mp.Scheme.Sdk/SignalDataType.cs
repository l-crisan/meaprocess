//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// The signal data types.
    /// </summary>
    public enum SignalDataType
    {
        /// <summary>
        ///Boolean 1 byte
        /// </summary>
        BOOL = 1, 
        /// <summary>
        ///Real 64 bit type.
        /// </summary>
        LREAL = 2,
        /// <summary>
        ///Real 32 bit type.
        /// </summary>
        REAL = 3,  

        /// <summary>
        ///Unsigned integer 8 bit type.
        /// </summary>
        USINT = 4,
        /// <summary>
        ///Signed integer 8 bit type.
        /// </summary>
        SINT = 5,
        /// <summary>
        ///Unsigned integer 16 bit type.
        ///</summary>
        UINT = 6,
        /// <summary>
        ///Signed integer 16 bit type.
        /// </summary>
        INT = 7, 
        /// <summary>
        ///Unsigned integer 32 bit type.
        /// </summary>
        UDINT = 8,  
        /// <summary>
        ///Signed integer 32 bit type.
        /// </summary>
        DINT = 9,
        /// <summary>
        ///Unsigned integer 64 bit type.
        /// </summary>
        ULINT = 10,
        /// <summary>
        ///Signed integer 64 bit type.
        /// </summary>
        LINT = 11,

        /// <summary>
        ///Object type.
        /// </summary>
        OBJECT = 14,

    }
}
