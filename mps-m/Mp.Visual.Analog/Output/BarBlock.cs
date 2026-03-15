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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Mp.Visual.Analog
{
    [Serializable]
    public partial class BarBlock : Mp.Visual.Base.VisualBlock
    {
        private Bar.CtrlOrientation _orientation;
        private double _min = 0;
        private double _max = 10;
        private int _division = 10;
        private int _precision = 0;
        private Color _barColor = Color.Blue;
        private Color _barBackColor;
        private double _value;
        private BorderStyle _barBorderStyle = BorderStyle.FixedSingle;

        public BarBlock()
        {
            InitializeComponent();
            _barBackColor = Color.White;
        }


        [SRDescription("BarColor")]
        [SRCategory("View")]
        public double Value
        {
            set
            {
                _value = value;
            }

            get { return _value; }
        }

        public override void UpdateProperties()
        {
            foreach (Bar ctrl in base.ControlsInContainer)
            {
                ctrl.BarColor = _barColor;
                ctrl.Minimum = _min;
                ctrl.Maximum = _max;
                ctrl.AxisDevision = _division;
                ctrl.AxisPrecision = _precision;
                ctrl.Orientation = _orientation;
                ctrl.BackColor = _barBackColor;
                ctrl.Value = _value;
                ctrl.BorderStyle = _barBorderStyle;
                ctrl.Invalidate();
            }

            base.UpdateProperties();
        }


        [SRDescription("BarBorderStyle")]
        [SRCategory("View")]
        public BorderStyle BarBorderStyle
        {
            get { return _barBorderStyle; }
            
            set
            {
                _barBorderStyle = value;
            }
        }

        [SRDescription("BarColor")]
        [SRCategory("View")]
        public Color BarColor
        {
            get { return _barColor; }
            set
            {
                _barColor = value;
            }
        }

        [SRDescription("Minimum")]
        [SRCategory("View")]
        public double Minimum
        {
            set
            {
                _min = value;
            }
            get { return _min; }
        }

        [SRDescription("Maximum")]
        [SRCategory("View")]
        public double Maximum
        {
            set
            {
                _max = value;
            }

            get { return _max; }
        }


        [SRDescription("BarBackColor")]
        [SRCategory("View")]
        public Color BarBackColor
        {
            get { return _barBackColor; }
            set
            {
                _barBackColor = value;
            }
        }

        [SRDescription("AxisDivision")]
        [SRCategory("View")]
        public int AxisDevision
        {
            get { return _division; }
            set
            {
                _division = value;
            }
        }


        [SRDescription("AxisPrecision")]
        [SRCategory("View")]
        public int AxisPrecision
        {
            get { return _precision; }
            set
            {
                _precision = value;
            }
        }

        [SRDescription("Orientation")]
        [SRCategory("View")]
        public Bar.CtrlOrientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
            }
        }
    }
}
