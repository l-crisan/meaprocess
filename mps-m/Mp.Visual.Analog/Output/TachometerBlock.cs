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
    public partial class TachometerBlock : Mp.Visual.Base.VisualBlock
    {
        private float _minValue;
        private float _maxValue;
        private float _threshold;
        private float _recommendedValue;
        private float _currentValue;
        private Color _dialColor;
        private float _glossinessAlpha;
        private int _noOfDivisions;
        private int _noOfSubDivisions;
        private string _dialText;
        private bool _enableTransparentBackground;

        public TachometerBlock()
        {
            InitializeComponent();
            Tachometer ctrl = new Tachometer();
            _minValue = -10;
            _maxValue = 10;
            _threshold = ctrl.ThresholdPercent;
            _recommendedValue = ctrl.RecommendedValue;
            _currentValue = ctrl.Value;
            _dialColor = ctrl.DialColor;
            _glossinessAlpha = ctrl.Glossiness;
            _noOfDivisions = ctrl.NoOfDivisions;
            _noOfSubDivisions = ctrl.NoOfSubDivisions;
            _dialText = ctrl.DialText;
            _enableTransparentBackground = ctrl.EnableTransparentBackground;
        }

        public override void UpdateProperties()
        {
            foreach (Tachometer ctrl in base.ControlsInContainer)
            {
                ctrl.MinValue = _minValue;
                ctrl.MaxValue =_maxValue;
                ctrl.ThresholdPercent = _threshold;
                ctrl.RecommendedValue = _recommendedValue;
                ctrl.DialColor = _dialColor;
                ctrl.Glossiness = _glossinessAlpha;
                ctrl.NoOfDivisions = _noOfDivisions;
                ctrl.NoOfSubDivisions = _noOfSubDivisions;                
                ctrl.EnableTransparentBackground = _enableTransparentBackground;
                ctrl.Invalidate();
            }

            base.UpdateProperties();
        }

        /// <summary>
        /// Mininum value on the scale
        /// </summary>
        [DefaultValue(0)]
        [SRDescription("MinValue")]
        [SRCategory("View")]
        public float MinValue
        {
            get { return _minValue; }

            set
            {
                _minValue = value;
            }
        }

        /// <summary>
        /// Maximum value on the scale
        /// </summary>
        [DefaultValue(100)]
        [SRDescription("MaxValue")]
        [SRCategory("View")]
        public float MaxValue
        {
            get { return _minValue; }
            set
            {
                _minValue = value;
            }
        }

        /// <summary>
        /// Gets or Sets the Threshold area from the Recommended Value. (1-99%)
        /// </summary>
        [DefaultValue(25)]
        [Description("Gets or Sets the Threshold area from the Recommended Value. (1-99%)")]
        public float ThresholdPercent
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
            }
        }

        /// <summary>
        /// Threshold value from which green area will be marked.
        /// </summary>
        [DefaultValue(25)]
        [Description("Threshold value from which green area will be marked.")]
        public float RecommendedValue
        {
            get { return _recommendedValue; }
            set
            {
                _recommendedValue = value;
            }
        }

        /// <summary>
        /// Value where the pointer will point to.
        /// </summary>
        [DefaultValue(0)]
        [SRDescription("AQValue")]
        [SRCategory("View")]
        public float Value
        {
            get { return _currentValue; }
            set
            {
                _currentValue = value;
            }
        }

        /// <summary>
        /// Background color of the dial
        /// </summary>
        [SRCategory("View")]
        [SRDescription("DialColor")]
        public Color DialColor
        {
            get { return _dialColor; }
            set
            {
                _dialColor = value;
            }
        }

        /// <summary>
        /// Glossiness strength. Range: 0-100
        /// </summary>
        [DefaultValue(72)]
        [Description("Glossiness strength. Range: 0-100")]
        public float Glossiness
        {
            get
            {
                return _glossinessAlpha;
            }

            set
            {
                _glossinessAlpha = value;
            }
        }

        /// <summary>
        /// Get or Sets the number of Divisions in the dial scale.
        /// </summary>
        [DefaultValue(10)]
        [SRDescription("NoOfDivisions")]
        [SRCategory("View")]
        public int NoOfDivisions
        {
            get { return _noOfDivisions; }
            set
            {
                _noOfDivisions = value;
            }
        }

        /// <summary>
        /// Gets or Sets the number of Sub Divisions in the scale per Division.
        /// </summary>
        [DefaultValue(3)]
        [SRDescription("NoOfSubDivisions")]
        [SRCategory("View")]
        public int NoOfSubDivisions
        {
            get { return _noOfSubDivisions; }
            set
            {
               _noOfSubDivisions = value;
            }
        }
        /// <summary>
        /// Gets or Sets the Text to be displayed in the dial
        /// </summary>
        [Description("Gets or Sets the Text to be displayed in the dial")]
        public string DialText
        {
            get { return _dialText;}
            set
            {
                _dialText = value;
            }
        }

        /// <summary>
        /// Enables or Disables Transparent Background color.
        /// Note: Enabling this will reduce the performance and may make the control flicker.
        /// </summary>
        [DefaultValue(false)]
        [Description("Enables or Disables Transparent Background color. Note: Enabling this will reduce the performance and may make the control flicker.")]
        public bool EnableTransparentBackground
        {
            get { return _enableTransparentBackground; }
            set
            {
                _enableTransparentBackground = value;
            }
        }
    }
}
