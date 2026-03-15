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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class DigitalMeter1Block : Base.VisualBlock
    {
        private string _unit = "";
        private ControlLimits _limits = new ControlLimits();
        private double _value = 0;
        private int _precision = 1;
        private  string _label = "";
        private int _splitterDistance1;
        private int _splitterDistance2;
        private int _labelWidth = 10;
        private int _valueWidth = 10;
        private Color _valueBoxBackColor;
        private Color _valueForeColor;

        public DigitalMeter1Block()
        {
            InitializeComponent();
            DigitalMeter1 ctrl = new DigitalMeter1();
            _valueBoxBackColor = ctrl.ValueBoxBackColor;
            _valueForeColor = ctrl.ValueBoxForeColor;
            _labelWidth = ctrl.LabelWidth;
            _valueWidth= ctrl.ValueWidth;
            _splitterDistance1 = ctrl.SplitterDistance1;
            _splitterDistance2 = ctrl.SplitterDistance2;
            this.Font = ctrl.Font;
            this.Width = 400;
            this.Height = 400;
        }

        public override void UpdateProperties()
        {
            foreach (DigitalMeter1 ctrl in base.ControlsInContainer)
            {
                ctrl.Font = this.Font;
                ctrl.Value = _value;
                ctrl.Limits = _limits;
                ctrl.Precision = _precision;
                ctrl.LabelWidth = _labelWidth;
                ctrl.ValueWidth = _valueWidth;
                ctrl.ValueBoxBackColor = _valueBoxBackColor;
                ctrl.ValueBoxForeColor = _valueForeColor;
                ctrl.SplitterDistance1 = _splitterDistance1;
                ctrl.SplitterDistance2 = _splitterDistance2;
                ctrl.Invalidate();
            }

            base.UpdateProperties();
        }


        [System.ComponentModel.Browsable(true),
        SRCategory("View"),
        SRDescription("Limits"),
        System.ComponentModel.Editor(typeof(LimitsUIEditor), typeof(UITypeEditor))]
        public ControlLimits Limits
        {
            get { return _limits; }
            set { _limits = value; }
        }

        /// <summary>
        /// Gets or sets the text label.
        /// </summary>
        [SRCategory("View")]
        [Description("Sets the text label.")]
        public string LabelText
        {
            set
            {
                _label = value;
            }

            get { return _label; }
        }

        /// <summary>
        /// Gets or sets the unit label
        /// </summary>
        [SRCategory("View")]
        [Description("Sets the unit text.")]
        public string UnitText
        {
            set { _unit = value; }
            get { return _unit; }
        }

        [Browsable(false)]
        public int SplitterDistance1
        {
            set
            {
                _splitterDistance1 = value;
            }
            get 
            {
                return _splitterDistance1; 
            }
        }

        [Browsable(false)]
        public int SplitterDistance2
        {
            set
            {
                _splitterDistance2 = value;
            }
            get { return _splitterDistance2; }
        }

        [SRCategory("View")]
        [SRDescription("LabelWidth")]
        public int LabelWidth
        {
            set
            {
                _labelWidth = value;
            }
            get
            {
                return _labelWidth;
            }
        }

        [SRCategory("View")]
        [SRDescription("ValueWidthCol")]
        public int ValueWidth
        {
            set
            {
               _valueWidth = value;
            }
            get
            {
                return _valueWidth;
            }
        }

        /// <summary>
        /// Gets or sets the value precision.
        /// </summary>
        [SRCategory("View")]
        [SRDescription("Precision")]
        public int Precision
        {
            set
            {
                _precision = value;
            }

            get { return _precision; }
        }

        [SRCategory("View")]
        [SRDescription("ValueBoxBackColor")]
        public Color ValueBoxBackColor
        {
            get { return _valueBoxBackColor; }
            set { _valueBoxBackColor = value; }
        }

        [SRCategory("View")]
        [SRDescription("ValueBoxForeColor")]
        public Color ValueBoxForeColor
        {
            get { return _valueForeColor; }
            set
            {
                _valueForeColor = value;
            }
        }
    }
}
