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

namespace Mp.Runtime.Sdk
{
    /// <summary>
    /// The base class for the signal scaling.
    /// </summary>
    public interface ISignalScaling
    {
        /// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>
        double ScaleValue(byte value);
        
        /// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>
        double ScaleValue(sbyte value);

        /// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>
        double ScaleValue(short value);

        /// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>
        double ScaleValue(ushort value);

    	/// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>    	
        double ScaleValue(int value);

    	/// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>    	
        double ScaleValue(uint value);

    	/// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>    	
        double ScaleValue(long value);

    	/// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>    	
        double ScaleValue(ulong value);

        /// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>    	
        double ScaleValue(float value);

        /// <summary>
        /// Scale a signal value.
        /// </summary>
        /// <param name="value">The numeric value to scale streamt in a byte array.</param>
        /// <returns>The scaled value.</returns>    	
        double ScaleValue(double value);
    }
}
