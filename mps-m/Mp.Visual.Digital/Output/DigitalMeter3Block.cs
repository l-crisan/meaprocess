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

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class DigitalMeter3Block : Mp.Visual.Base.VisualBlock
    {
        private ControlLimits _limits = new ControlLimits();
        private Color _backgroundColor;
        private Color _colorDark;
        private Color _colorLight;
        private int _elements;
        private int _elementWidth;
        private int _precision;
        private string _label;

        public DigitalMeter3Block()
        {
            InitializeComponent();        
            DigitalMeter3 ctrl = new DigitalMeter3();
            _backgroundColor = ctrl.ColorBackground;
            _colorDark = ctrl.ColorDark;
            _colorLight = ctrl.ColorLight;
            _elements = ctrl.Elements;
            _elementWidth = ctrl.ElementWidth;
            _precision = ctrl.Precision;
            _label = ctrl.Label;
            this.Width = 400;
            this.Height = 400;
        }

        public override void UpdateProperties()
        {
            foreach (DigitalMeter3 ctrl in base.ControlsInContainer)
            {
                ctrl.Font = this.Font;
                //ctrl.Limits = _limits;
                ctrl.ColorBackground = _backgroundColor;
                ctrl.ColorDark = _colorDark;
                ctrl.ColorLight = _colorLight;
                ctrl.Elements = _elements;
                ctrl.ElementWidth = _elementWidth;
                ctrl.Precision = _precision;                

                ctrl.Invalidate();
            }

            base.UpdateProperties();
        }

        /*
        [System.ComponentModel.Browsable(true),
        SRCategory("View"),
        SRDescription("Limits"),
        System.ComponentModel.Editor(typeof(LimitsUIEditor), typeof(UITypeEditor))]
        public ControlLimits Limits
        {
            get { return _limits; }
            set { _limits = value; }
        }
        */
        /// <summary>
        /// Background color of the LED array.
        /// </summary>
        [SRDescription("ColorBackground")]
        [SRCategory("LEDView")]
        public Color ColorBackground
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        /// <summary>
        /// Color of inactive LED segments.
        /// </summary>
        [SRDescription("ColorDark")]
        [SRCategory("LEDView")]
        public Color ColorDark
        {
            get { return _colorDark; }
            set { _colorDark = value; }
        }

        /// <summary>
        /// Color of active LED segments.
        /// </summary>
        [SRDescription("ColorLight")]
        [SRCategory("LEDView")]
        public Color ColorLight
        {
            get { return _colorLight; }
            set { _colorLight = value; }
        }

        /// <summary>
        /// Number of seven-segment elements in this array.
        /// </summary>
        [SRDescription("Elements")]
        [SRCategory("LEDView")]
        public int Elements
        {
            get { return _elements; }
            set { _elements = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SRDescription("ElementWidth")]
        [SRCategory("LEDView")]
        public int ElementWidth
        {
            get { return _elementWidth; }
            set { _elementWidth = value; }
        }

        [SRDescription("Precision")]
        [SRCategory("LEDView")]
        public int Precision
        {
            set
            {
                _precision = value;            
            }

            get { return _precision; }
        }


        public string Label
        {
            set { _label = value; }
            get { return _label;}
        }
    }
}
