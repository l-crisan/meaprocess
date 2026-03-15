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
using System.Drawing.Design;

namespace Mp.Visual.Digital
{
    [Serializable]
    public partial class DigitalMeter2 : UserControl
    {
        private string _unit = "";
        private int _precision = 2;
        private string _format = "{0:0.00}";
        private double _value;
        private ControlLimits _limits = new ControlLimits();        
        private Timer _updateTimer = new Timer();
        
        public DigitalMeter2()
        {
            InitializeComponent();
            Value = 0.0;
            dataValue.ContextMenuStrip = null;            
            dataValue.ContextMenu = null;
            dataValue.GotFocus += new EventHandler(Dummy_GotFocus);
            name.GotFocus += new EventHandler(Dummy_GotFocus);
            name.ContextMenuStrip = null;
            name.ContextMenu = null;     
            _updateTimer.Interval = 100;
            _updateTimer.Tick += new EventHandler(OnUpdateValue);       
            _updateTimer.Start();
        }

        void OnUpdateValue(object sender, EventArgs e)
        {

            dataValue.ForeColor = this.ForeColor;

            if (_limits.UseWarningLimit)
            {
                if (_value < _limits.WarningLower || _value > _limits.WarningUpper)
                    dataValue.ForeColor = _limits.WarningColor;
            }

            if (_limits.UseAlarmLimit)
            {
                if (_value < _limits.AlarmLower || _value > _limits.AlarmUpper)
                    dataValue.ForeColor = _limits.AlarmColor;
            }

            dataValue.Text = String.Format(_format, _value) + " " + _unit; 
        }

        private void Dummy_GotFocus(object sender, EventArgs e)
        {//Set the focus to a dummy control=> to disable focus.
            dummy.Focus();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        private void DigitalMeter_FontChanged(object sender, EventArgs e)
        {
            dataValue.Font = this.Font;
            name.Font = new Font(this.Font.Name, this.Font.Size / 3);
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

        public string Unit
        {
            set             
            { 
                _unit = value;
                Value = _value;
            }
        }

        public string Label
        {
            set { name.Text = value; }
        }

        [SRDescription("Precision")]
        [SRCategory("View")]
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

                if(_precision == 0)
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

        private void DigitalMeter_BackColorChanged(object sender, EventArgs e)
        {
            dataValue.BackColor = this.BackColor;
            name.BackColor = this.BackColor;
        }

        private void DigitalMeter_ForeColorChanged(object sender, EventArgs e)
        {
            dataValue.ForeColor = this.ForeColor;
            name.ForeColor = this.ForeColor;
        }
    }
}
