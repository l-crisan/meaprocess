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
using System.Runtime.InteropServices;
using System.Drawing.Design;

 namespace Mp.Visual.Analog
{
    public partial class ManometerBlock : Mp.Visual.Base.VisualBlock
    {
        private int _decimals;
        private int _numberSpacing;        
        private int _borderWidth;
        private Color _colorArrow;
        private Color _backColor;
        private bool _clockWise;
        private int _barsBetweenNumbers;
        private float _storedMax;
        private string _textUnit;
        private bool _storeMax;
        private string _textDescription;

        private float _min = -10;
        private float _max = 10;
        private float _interval = 2;
        private int _startAngle = 240;

        public ManometerBlock()
        {
            InitializeComponent();
            Manometer ctrl = new Manometer();

            _decimals = ctrl.Decimals;
            _numberSpacing = ctrl.NumberSpacing;
            _backColor = ctrl.BackColor;
            _borderWidth = ctrl.BorderWidth;
            _colorArrow = ctrl.ArrowColor;
            _clockWise = ctrl.ClockWise;
            _barsBetweenNumbers = ctrl.BarsBetweenNumbers;
            _storedMax = ctrl.StoredMax;            
            _textUnit = ctrl.TextUnit;      
            _storeMax = ctrl.StoreMax;      
            _textDescription = ctrl.TextDescription;
        }

        public override void UpdateProperties()
        {
            foreach (Manometer ctrl in base.ControlsInContainer)
            {
                ctrl.Decimals = _decimals;
                ctrl.NumberSpacing = _numberSpacing;
                ctrl.BackColor = _backColor;
                ctrl.BorderWidth = _borderWidth;
                ctrl.ArrowColor = _colorArrow;
                ctrl.ClockWise = _clockWise;
                ctrl.BarsBetweenNumbers = _barsBetweenNumbers;
                ctrl.Min = _min;
                ctrl.Max = _max;
                ctrl.Interval = _interval;
                ctrl.StoredMax = _storedMax;
                ctrl.StartAngle = _startAngle;
                ctrl.StoreMax = _storeMax;
                ctrl.Invalidate();
            }

            base.UpdateProperties();
        }

        /// <summary>
        /// Gets or sets the decimals used for the numbers
        /// </summary>
        /// <value>The decimals.</value>
        [Browsable(true)]
        [SRDescription("Decimals")]
        [Category("Appearance")]
        public int Decimals
        {
            get { return _decimals; }
            set { _decimals = value;}
        }

        /// <summary>
        /// Gets or sets the space between numbers in degrees.
        /// </summary>
        /// <value>The number spacing.</value>
        [Browsable(true)]
        [SRDescription("NumberSpacing")]
        [Category("Layout")]
        [Localizable(true)]
        public int NumberSpacing
        {
            get { return _numberSpacing; }
            set
            {
                _numberSpacing = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color for the control, this property is not relevant for this control.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
        [Browsable(true)]
        [SRDescription("BackColor")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Peru")]
        public new Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value;}
        }

        /// <summary>
        /// Gets or sets the width of the border.
        /// </summary>
        /// <value>The width of the border.</value>
        [Browsable(true)]
        [SRDescription("BorderWidth")]
        [Category("Appearance")]
        public int BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                _borderWidth = value;
            }
        }

        /// <summary>
        /// Gets or sets the color of the arrow.
        /// </summary>
        /// <value>The color of the arrow.</value>
        [Browsable(true)]
        [SRDescription("ArrowColor")]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [Localizable(true)]
        public Color ArrowColor
        {
            get { return _colorArrow; }
            set { _colorArrow = value;}
        }

        /// <summary>
        /// Set to true if the layout should be clockwise.
        /// </summary>
        /// <value>true if Clockwise</value>
        [Browsable(true)]
        [SRDescription("ClockWise")]
        [Category("Layout")]
        [DefaultValue(true)]
        [Localizable(true)]
        public bool ClockWise
        {
            get { return _clockWise; }
            set { _clockWise = value;  }
        }

        /// <summary>
        /// Number of bars between the numbers.
        /// </summary>
        /// <value>true if Clockwise</value>
        [Browsable(true)]
        [SRDescription("BarNumber")]
        [Category("Layout")]
        public int BarsBetweenNumbers
        {
            get { return _barsBetweenNumbers; }
            set
            {
               _barsBetweenNumbers = value;
            }
        }

        /// <summary>
        /// Gets or sets the max value.
        /// </summary>
        /// <value>The max value</value>
        [Browsable(true)]
        [SRDescription("MaxValue")]
        [Category("Layout")]        
        public float Max
        {
            get { return _max; }
            set
            {
                _max = value;
            }
        }

        /// <summary>
        /// Gets or sets the min value.
        /// </summary>
        /// <value>The min.</value>
        [Browsable(true)]
        [SRDescription("MinValue")]
        [Category("Layout")]
        public float Min
        {
            get { return _min; }
            set
            {
                _min = value;
            }
        }

        /// <summary>
        /// The intervals between Min and Max.
        /// </summary>
        /// <value>The min.</value>
        [Browsable(true)]
        [SRDescription("IntervalMinMax")]
        [Category("Layout")]
        public float Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to store max.
        /// </summary>
        /// <value><c>true</c> if [store max]; otherwise, <c>false</c>.</value>
        [Browsable(true)]
        [SRDescription("MaxStore")]
        [Category("Layout")]
        [DefaultValue(true)]
        public bool StoreMax
        {
            get { return _storeMax; }
            set
            {
                _storeMax = value;
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description</value>
        [Browsable(true)]
        [SRDescription("Description")]
        [Category("Layout")]
        [Localizable(true)]
        public string TextDescription
        {
            get { return _textDescription; }
            set { _textDescription = value; }
        }

        /// <summary>
        /// Gets or sets the text unit.
        /// </summary>
        /// <value>The text unit.</value>
        [Browsable(true)]
        [SRDescription("TextUnit")]
        [Category("Layout")]
        [Localizable(true)]
        public string TextUnit
        {
            get { return _textUnit; }
            set { _textUnit = value; }
        }

        /// <summary>
        /// Gets or sets the starting angle (degrees)
        /// </summary>
        /// <value>The starting angle</value>
        [Browsable(true)]
        [SRDescription("LayoutStartDegrees")]
        [Category("Layout")]
        public int StartAngle
        {
            get { return _startAngle; }
            set
            {
                _startAngle = value;
            }
        }

        /// <summary>
        /// Gets or sets the stored max.
        /// </summary>
        /// <value>The stored max.</value>
        [Browsable(true)]
        [SRDescription("StoredMaximum")]
        [Category("Layout")]
        [DefaultValue(0)]
        public float StoredMax
        {
            get { return _storedMax; }
            set
            {
                _storedMax = value;
            }
        }
        
    }
}
