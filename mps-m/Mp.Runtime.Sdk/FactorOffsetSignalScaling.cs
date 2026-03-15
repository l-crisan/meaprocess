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
    internal class FactorOffsetSignalScaling : ISignalScaling
    {
        private double _factor;
        private double _offset;

        public FactorOffsetSignalScaling(double factor, double offset)
        {
            _factor = factor;
            _offset = offset;
        }

        public double ScaleValue(byte value)
        {
            return _factor * value + _offset;
        }

        public double ScaleValue(sbyte value)
        {
            return _factor * value + _offset;
        }

        public double ScaleValue(short value)
        {
            return _factor * value + _offset;
        }

        public double ScaleValue(ushort value)
        {
            return _factor * value + _offset;
        }

        public double ScaleValue(int value)
        {
            return _factor * value + _offset;
        }

        public double ScaleValue(uint value)
        {
            return _factor * value + _offset;
        }

        public double ScaleValue(long value)
        {
            return _factor * value + _offset;
        }

        public double ScaleValue(ulong value)
        {
            return _factor * value + _offset;
        }

        public double ScaleValue(float value)
        {
            return _factor * value + _offset;
        }

        public double ScaleValue(double value)
        {
            return _factor * value + _offset;
        }

        public double Factor
        {
            set { _factor = value; }
            get { return _factor; }
        }

        public double Offset
        {
            set { _offset = value; }
            get { return _offset; }
        }

    }
}
