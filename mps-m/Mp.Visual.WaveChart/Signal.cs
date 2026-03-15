
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
using System.Drawing;
using System.Collections;
using System.Runtime.Serialization;

namespace Mp.Visual.WaveChart
{
    /// <summary>
	///  The chart signal representation
	/// </summary>
    [Serializable]
    public class Signal
	{
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Signal()
        { }

        /// <summary>
        /// The signal minimum.
        /// </summary>
        public double InitialMinimum = 1;
        
        /// <summary>
        /// The signal maximum.
        /// </summary>
        public double InitialMaximum = -1;

        /// <summary>
        /// The signal samplerate in Hz.
        /// </summary>        
        public double Samplerate = 100;
        
        /// <summary>
        /// The visible flag.
        /// </summary>        
        public bool Visible = true;
        
        /// <summary>
        /// The signal name.
        /// </summary>        
        public string Name;
        
        /// <summary>
        /// The signal comment.
        /// </summary>        
        public string Comment;
        
        /// <summary>
        /// The signal unit.
        /// </summary>        
        public string Unit;
        
        /// <summary>
        /// The signal color.
        /// </summary>        
        public Color LineColor = Color.White;
        
        /// <summary>
        /// The signal data point color.
        /// </summary>                        
        public Color PointColor = Color.Yellow;
        
        /// <summary>
        /// The signal line width.
        /// </summary>
        public uint LineWidth = 1;
        
        /// <summary>
        /// The signal point size.
        /// </summary>
        public uint PointSize = 3;

        /// <summary>
        /// The point visible flag.
        /// </summary>
        public bool PointsVisible = false;
        
        /// <summary>
        /// The Signal Y axis division.
        /// </summary>
        public uint YAxisDivision = 5;
        

        public int Index = 0;

        /// <summary>
        /// The Signal Y axis precision.
        /// </summary>
        /// <remarks>
        /// 12.23  => precision = 2
        /// 12.234 => precision = 3
        /// </remarks>
        public uint YAxisPrecision = 0;
        public double Minimum = -10;
        public double Maximum = 10;

        public object InternalHandle;
	}
}
