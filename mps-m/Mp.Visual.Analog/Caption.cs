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
    public class Caption
    {
        private Color _color = Color.Black;
        private string _text = "";
        private PointF _position;

        public Caption()
        {
        }

        public Color CapColor
        {
            get { return _color; }
            set { _color = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public PointF Position
        {
            get { return _position; }
            set { _position = value; }
        }
    }
}
