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

namespace Mp.Visual.Analog
{
    [Serializable()]
    public class Range
    {
        private bool _enabled = false;
        private Color _color = Color.LightGray;
        private float _startValue = -10.0f;
        private float _endValue = 10.0f;
        private int _innerRadius = 70;
        private int _outerRadius = 80;

        public Range()
        {
        }


        public void Copy(Range r)
        {
            _enabled = r._enabled;
            _color = r._color;
            _startValue = r._startValue;
            _endValue = r._endValue;
            _innerRadius =r._innerRadius;
            _outerRadius = r._outerRadius;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public Color RangeColor
        {
            get { return _color; }
            set { _color = value; }
        }

        public float StartValue
        {
            get { return _startValue; }
            set { _startValue = value; }
        }

        public float EndValue
        {
            get { return _endValue; }
            set { _endValue = value; }
        }

        public int InnerRadius
        {
            get { return _innerRadius; }
            set { _innerRadius = value; }
        }

        public int OuterRadius
        {
            get { return _outerRadius; }
            set { _outerRadius = value; }
        }
    }
}
