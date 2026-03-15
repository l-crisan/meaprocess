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
using System.Windows.Forms;

namespace Mp.Visual.Analog
{
    /// <summary>
    /// Base class for manometers
    /// </summary>
    [DefaultProperty("Value")]
    [DefaultEvent("ValueChanged")]
    [Description("Base class for manometers")]
    public abstract class ManometerBase : UserControl
    {
        #region -- Members --

        private float max;
        private float min;
        private float storedMax;
        private bool storeMax;
        private String textUnit;
        private String textDescription;
        private float value;
        private int startAngle = startAngleDefault;
        private float interval = defaultInterval;
        //Constants
        private const int defaultInterval = 10;
        private const int maxDefault = 100;
        private const int minDefault = 0;
        private const int startAngleDefault = 230;
        private const String textUnitDefault = "";
        private const String textDescriptionDefault = "";

        #endregion

        #region -- Properties --

        /// <summary>
        /// Gets or sets the max value.
        /// </summary>
        /// <value>The max value</value>
        [Browsable(true)]
        [SRDescription("MaxValue")]
        [Category("Layout")]
        [DefaultValue(maxDefault)]      
        public float Max
        {
            get { return max; }
            set
            {
                max = (max < min) ? min : value;
                if (MaxChanged != null)
                    MaxChanged(this, new EventArgs());
                Invalidate(); 
            }
        }

        /// <summary>
        /// Gets or sets the min value.
        /// </summary>
        /// <value>The min.</value>
        [Browsable(true)]
        [SRDescription("MinValue")]
        [Category("Layout")]
        [DefaultValue(minDefault)] 
        public float Min
        {
            get { return min; }
            set
            {
                min = (min > max) ? max : value;
                if (MinChanged != null)
                    MinChanged(this, new EventArgs());
                Invalidate();
            }
        }

        /// <summary>
        /// The intervals between Min and Max.
        /// </summary>
        /// <value>The min.</value>
        [Browsable(true)]
        [SRDescription("IntervalMinMax")]
        [Category("Layout")]
        [DefaultValue(defaultInterval)] 
        public float Interval
        {
            get { return interval; }
            set { interval = value;
            if (IntervalChanged != null)
                IntervalChanged(this, new EventArgs());
                Invalidate();
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
            get { return storeMax; }
            set { storeMax = value; 
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description</value>
        [Browsable(true)]
        [SRDescription("Description")]
        [Category("Layout")]
        [DefaultValue(textDescriptionDefault)]
        [Localizable(true)]
        public string TextDescription
        {
            get { return textDescription; }
            set { textDescription = value; Invalidate();}
        }

        /// <summary>
        /// Gets or sets the text unit.
        /// </summary>
        /// <value>The text unit.</value>
        [Browsable(true)]
        [SRDescription("TextUnit")]
        [Category("Layout")]
        [DefaultValue(textUnitDefault)]
        [Localizable(true)]
        public string TextUnit
        {
            get { return textUnit; }
            set { textUnit = value; Invalidate();}
        }

        /// <summary>
        /// Gets or sets the starting angle (degrees)
        /// </summary>
        /// <value>The starting angle</value>
        [Browsable(true)]
        [SRDescription("LayoutStartDegrees")]
        [Category("Layout")]
        [DefaultValue(startAngleDefault)]
        public int StartAngle
        {
            get { return startAngle; }
            set {
                if (value > 360)
                    value = 360;
                startAngle = value; 
                Invalidate(); }
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
            get { return storedMax; }
            set
            {
                if (value < min)
                    value = min;
                if (value > max)
                    value = max;
                if (value < Value)
                    value = Value;
                storedMax = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        [Browsable(true)]
        [SRDescription("Value")]
        [Category("Layout")]
        [DefaultValue(0)] 
        public float Value
        {
            get { return value; }
            set
            {
                if (value < min)
                    value = min;
                if (value > max)
                    value = max;
                this.value = value; 
                //Fire events
                if ((value > storedMax) && storeMax)
                {
                    storedMax = value;
                    if (StoredMaxChanged != null)
                        StoredMaxChanged(this, new EventArgs());
                }
                if (ValueChanged != null)
                    ValueChanged(this, new EventArgs());
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the text associated with this control. Not relevant for this control
        /// </summary>
        /// <value></value>
        /// <returns>The text associated with this control.</returns>
        [Browsable(false)]
        public override string Text
        {
            get{return base.Text;}
            set{base.Text = value;}
        }

        #endregion

        #region -- Events --

        /// <summary>
        ///    Occurs when the value of the ManometerBase.Interval property changes.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Property Changed")]
        public event EventHandler IntervalChanged;
        /// <summary>
        ///    Occurs when the value of the ManometerBase.Min property changes.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Property Changed")]
        public event EventHandler MinChanged;
        /// <summary>
        ///    Occurs when the value of the ManometerBase.Max property changes.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Property Changed")]
        public event EventHandler MaxChanged;
        /// <summary>
        /// Occurs when the value of the ManometerBase.StoredMax property changes.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Property Changed")]
        public event EventHandler StoredMaxChanged;
        /// <summary>
        ///    Occurs when the value of the ManometerBase.ValueChanged property changes.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("Property Changed")]
        public event EventHandler ValueChanged;


        #endregion

        #region -- Constructor --

        /// <summary>
        /// Create a new instance of ManometerBase
        /// </summary>
        /// <remarks></remarks>
        protected ManometerBase()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Name = "ManometerBase";
            max = 100;
            min = 0;
            StartAngle = startAngleDefault;
            TextDescription = "";
            TextUnit = "";
            StoreMax = true;
            StoredMax = min;
        }

        #endregion

    }
}
