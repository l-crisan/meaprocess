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
using System.Drawing.Design;

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class DigitalMeter2Block : Mp.Visual.Base.VisualBlock
    {
        private ControlLimits _limits = new ControlLimits();
        private string _unit = "";
        private double _value;
        private int _precision;

        public DigitalMeter2Block()
        {
            InitializeComponent();
            DigitalMeter2 ctrl = new DigitalMeter2();
            this.Font = ctrl.Font;
            _precision = ctrl.Precision;
            _limits = ctrl.Limits;
            this.Width = 400;
            this.Height = 400;
        }

        public override void UpdateProperties()
        {
            foreach (DigitalMeter2 ctrl in base.ControlsInContainer)
            {
                ctrl.Font = this.Font;
                ctrl.Unit   = _unit;
                ctrl.Limits = _limits;
                ctrl.Value  = _value;
                ctrl.Precision = _precision;
                ctrl.Invalidate();
            }

            base.UpdateProperties();
        }


        [System.ComponentModel.Browsable(true)]
        [ SRCategory("View")]
        [SRDescription("Limits")]
        [System.ComponentModel.Editor(typeof(LimitsUIEditor), typeof(UITypeEditor))]
        public ControlLimits Limits
        {
            get { return _limits; }
            set { _limits = value; }
        }

        public double Value
        {
            set
            {
                _value = value;
            }
        }

        [SRDescription("Precision")]
        [SRCategory("View")]
        public int Precision
        {
            set
            {
                _precision = value;
            }

            get { return _precision; }
        }

    }
}
