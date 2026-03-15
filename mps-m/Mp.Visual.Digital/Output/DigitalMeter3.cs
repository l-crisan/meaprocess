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
using System.Windows.Forms;
using System.Globalization;

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class DigitalMeter3 : UserControl
    {
        private int _precision = 3;
        private string _format = "{0:0.000}";
        private Timer _updateTimer = new Timer();
        private double _value;

        public DigitalMeter3()
        {
            InitializeComponent();
            Elements = 5;
            _updateTimer.Interval = 100;
            _updateTimer.Tick += new EventHandler(OnUpdateValue);
            _updateTimer.Start();
        }


        public string Label
        {
            set { name.Text = value; }
            get { return name.Text; }
        }

        /// <summary>
        /// Background color of the LED array.
        /// </summary>
        [SRDescription("ColorBackground")]
        [SRCategory("LEDView")]
        public Color ColorBackground 
        { 
            get { return segments.ColorBackground; } 
            set {segments.ColorBackground = value; } 
        }

        /// <summary>
        /// Color of inactive LED segments.
        /// </summary>
        [SRDescription("ColorDark")]
        [SRCategory("LEDView")]
        public Color ColorDark 
        {
            get { return segments.ColorDark; }
            set { segments.ColorDark = value; } 
        }

        /// <summary>
        /// Color of active LED segments.
        /// </summary>
        [SRDescription("ColorLight")]
        [SRCategory("LEDView")]
        public Color ColorLight
        {
            get { return segments.ColorLight; }
            set { segments.ColorLight = value; }
        }

        /// <summary>
        /// Number of seven-segment elements in this array.
        /// </summary>
        [SRDescription("Elements")]
        [SRCategory("LEDView")]
        public int Elements
        {
            get { return segments.Elements; }
            set { segments.Elements = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [SRDescription("ElementWidth")]
        [SRCategory("LEDView")]
        public int ElementWidth
        {
            get { return segments.ElementWidth; }
            set { segments.ElementWidth = value; }
        }

        [SRDescription("Precision")]
        [SRCategory("LEDView")]
        public int Precision
        {
            set
            {
                _precision = value;

                _format = "{0:0.";

                for (int i = 0; i < _precision; ++i)
                {
                    _format += "0";
                }

                _format += "}";

                if (_precision == 0)
                    _format = "{0:0}";
            }

            get { return _precision; }
        }

        public double Value
        {
            set 
            {
               _value = value;
            }
        }



        private void OnUpdateValue(object sender, EventArgs e)
        {

            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";
            segments.Value = String.Format(info, _format, _value);
        }

        private void DigitalMeter7Seg_BackColorChanged(object sender, EventArgs e)
        {
            name.BackColor = this.BackColor;
        }

        private void DigitalMeter7Seg_ForeColorChanged(object sender, EventArgs e)
        {
            name.ForeColor = this.ForeColor;
        }

        private void DigitalMeter7Seg_FontChanged(object sender, EventArgs e)
        {
            name.Font = this.Font;
        }
    }
}
