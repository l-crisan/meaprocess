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
using System.Drawing.Design;

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class DigitalMeter1 : UserControl
    {
        private int _precision = 3;
        private string _format;
        private ControlLimits _limits = new ControlLimits();
        private Color _valueForeColor = Color.Black;
        private double _value = 0;
        private Timer _timer = new Timer();
            
        public DigitalMeter1()
        {
            InitializeComponent();
            valueCtrl.ContextMenu = null;
            
            valueCtrl.ContextMenuStrip = new ContextMenuStrip();
            _timer.Tick +=new EventHandler(OnUpdate);
            _timer.Interval = 100;
            _timer.Start();
        }

        private void OnUpdate(object sender, EventArgs e)
        {
 	        valueCtrl.ForeColor = _valueForeColor;

            if (_limits.UseWarningLimit)
            {
                if (_value < _limits.WarningLower || _value > _limits.WarningUpper)
                    valueCtrl.ForeColor = _limits.WarningColor;
            }

            if (_limits.UseAlarmLimit)
            {
                if (_value < _limits.AlarmLower || _value > _limits.AlarmUpper)
                    valueCtrl.ForeColor = _limits.AlarmColor;
            }

            valueCtrl.Text = _value.ToString(_format); 
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
               label.Text = value;
            }

            get { return label.Text; }
        }

        /// <summary>
        /// Gets or sets the unit label
        /// </summary>
        [SRCategory("View")]
        [Description("Sets the unit text.")]
        public string UnitText
        {
            set { unit.Text = value; }
            get { return unit.Text; }
        }

        [Browsable(false)]
        public int SplitterDistance1
        {
            set 
            {
                try
                {
                    splitContainer1.SplitterDistance = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            get { return splitContainer1.SplitterDistance; }
        }

        [Browsable(false)]
        public int SplitterDistance2
        {
            set 
            { 
                try
                {
                    splitContainer2.SplitterDistance = value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            get { return splitContainer2.SplitterDistance; }
        }

        protected override void OnResize(EventArgs e)
        {
            int sp1 = splitContainer1.SplitterDistance;
            int sp2 = splitContainer2.SplitterDistance;

            base.OnResize(e);

            try
            {
                splitContainer1.SplitterDistance = sp1;
                splitContainer2.SplitterDistance = sp2;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        [SRCategory("View")]
        [SRDescription("LabelWidth")]
        public int LabelWidth
        {
            set 
            { 
                splitContainer1.SplitterDistance = value; 
            }
            get 
            { 
                return splitContainer1.SplitterDistance; 
            }
        }
        
        [SRCategory("View")]
        [SRDescription("ValueWidthCol")]
        public int ValueWidth
        {
            set 
            { 
                splitContainer2.SplitterDistance = value; 
            }
            get 
            { 
                return splitContainer2.SplitterDistance; 
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
                _format = "0.";
                
                for (int i = 0; i < _precision; i++)
                    _format += "0";

            }
            get { return _precision; }
        }
        
        [Browsable(false)]
        public double Value
        {
            get { return Convert.ToDouble(valueCtrl.Text); }
            set 
            {
            _value = value;
            }
        }

        private void NumericControl_FontChanged(object sender, EventArgs e)
        {
            label.Font = this.Font;
            unit.Font = this.Font;
            valueCtrl.Font = this.Font;

        }

        [SRCategory("View")]
        [SRDescription("ValueBoxBackColor")]
        public Color ValueBoxBackColor
        {
            get { return valueCtrl.BackColor;}
            set { valueCtrl.BackColor = value; }
        }

        [SRCategory("View")]
        [SRDescription("ValueBoxForeColor")]
        public Color ValueBoxForeColor
        {
            get { return _valueForeColor; }
            set 
            {
                _valueForeColor = value;
                valueCtrl.ForeColor = value; 
            }
        }

        private void NumericControl_BackColorChanged(object sender, EventArgs e)
        {
            this.splitContainer1.BackColor = this.BackColor;
            this.splitContainer1.Panel1.BackColor = this.BackColor;
            this.splitContainer1.Panel2.BackColor = this.BackColor;

            this.splitContainer2.BackColor = this.BackColor;
            this.splitContainer2.Panel1.BackColor = this.BackColor;
            this.splitContainer2.Panel2.BackColor = this.BackColor;

            label.BackColor = this.BackColor;
            unit.BackColor = this.BackColor;
            label.Invalidate();
            unit.Invalidate();
            Invalidate();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            label.Invalidate();
            unit.Invalidate();
        }

        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {
            label.Invalidate();
            unit.Invalidate();
        }

        private void NumericControl_ForeColorChanged(object sender, EventArgs e)
        {
            label.ForeColor = this.ForeColor;
            unit.ForeColor = this.ForeColor;
            label.Invalidate();
            unit.Invalidate();
            Invalidate();
        }
    }
}
