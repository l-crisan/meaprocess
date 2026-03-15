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
using System.Drawing;

namespace Mp.Visual.XYChart
{
    [Serializable()]
    public class Curve
    {
        private double[] _x;
        private double[] _y;
        private string _xName;
        private string _yName;
        private Color _color = Color.White;
        private double _yRate;
        private string _name = "";
 
        public Curve()
        {
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public double[] X
        {
            set { _x = value; }
            get{ return _x; }
        }

        public double[] Y
        {
            set { _y = value; }
            get { return _y; }
        }

        public string XName
        {
            set { _xName = value; }
            get { return _xName; }
        }

        public string YName
        {
            set { _yName = value; }
            get { return _yName; }
        }

        public Color LineColor
        {
            get { return _color; }
            set { _color = value; }
        }

        public double YRate
        {
            get { return _yRate; }
            set { _yRate = value; }
        }
    }
}
