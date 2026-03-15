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
    public partial class ThermometerBlock : Mp.Visual.Base.VisualBlock
    {
        private Thermometer.Unit _display;
        private int _smallTicFreq = 1;
        private int _largeTicFreq;
        private double _rangeMin;
        private double _rangeMax;

        public ThermometerBlock()
        {
            InitializeComponent();
            Thermometer ctrl = new Thermometer();
            _display = ctrl.Display;
            _smallTicFreq = ctrl.SmallTicFreq;
            _largeTicFreq = ctrl.LargeTicFreq;
            _rangeMin = ctrl.RangeMin;
            _rangeMax = ctrl.RangeMax;        
        }

        public override void UpdateProperties()
        {
            
            foreach (Thermometer ctrl in base.ControlsInContainer)
            {                              
                ctrl.Display = _display;
                ctrl.SmallTicFreq = _smallTicFreq;
                ctrl.LargeTicFreq = _largeTicFreq;
                ctrl.RangeMin = _rangeMin;
                ctrl.RangeMax = _rangeMax;        
                ctrl.Invalidate();
            }

            base.UpdateProperties();
        }
        /// <summary>
        /// Gets or sets the display mode.
        /// </summary>
        [SRDescription("Display")]
        [SRCategory("Thermometer")]
        public Thermometer.Unit Display
        {
            get { return _display; }

            set
            {
                _display = value;               
            }
        }
        /// <summary>
        /// Gets or sets the small tic frequency.
        /// </summary>
        [SRDescription("SmallTicFreq")]
        [SRCategory("Thermometer")]
        public int SmallTicFreq
        {
            get { return _smallTicFreq; }
            set
            {
                _smallTicFreq = value;             
            }
        }

        /// <summary>
        /// Gets or sets the large tic frequency.
        /// </summary>
        [SRDescription("LargeTicFreq")]
        [SRCategory("Thermometer")]
        public int LargeTicFreq
        {
            get { return _largeTicFreq; }
            set
            {
                _largeTicFreq = value;
            }
        }

        /// <summary>
        /// Gets otr sets the range minimum.
        /// </summary>
        [System.ComponentModel.Browsable(true),
        SRCategory("Thermometer"),
        SRDescription("RangeMin")]
        public double RangeMin
        {
            get
            {
                return _rangeMin;
            }
            set
            {
                _rangeMin = value;
            }
        }

        /// <summary>
        /// Gets or sets the range maximum.
        /// </summary>
        [System.ComponentModel.Browsable(true),
        SRCategory("Thermometer"),
        SRDescription("RangeMax")]
        public double RangeMax
        {
            get
            {
                return _rangeMax;
            }

            set
            {
                _rangeMax = value;
            }
        }    

    }
}
