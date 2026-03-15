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
using System.Text;
using System.Drawing;

namespace Mp.Visual.Digital
{
    [Serializable()]
    public class ControlLimits
    {
        private bool _useWarningLimit;
        private double _warningLower = -6;
        private double _warningUpper = 6;
        private Color _warningColor = Color.Yellow;
        
        private bool _useAlarmLimit;
        private double _alarmLower = -8;
        private double _alarmUpper = 8;
        private Color _alarmColor = Color.Red;

        public ControlLimits()
        {
        }

        public bool UseWarningLimit
        {
            get { return _useWarningLimit; }
            set { _useWarningLimit = value; }
        }

        public double WarningLower
        {
            get { return _warningLower; }
            set { _warningLower = value; }
        }

        public double WarningUpper
        {
            get{ return _warningUpper;}
            set{ _warningUpper = value;}
        }

        public Color WarningColor
        {
            get { return _warningColor; }
            set { _warningColor = value; }
        }

        public bool UseAlarmLimit
        {
            get { return _useAlarmLimit; }
            set { _useAlarmLimit = value; }
        }

        public double AlarmLower
        {
            get { return _alarmLower; }
            set { _alarmLower = value; }
        }

        public double AlarmUpper
        {
            get { return _alarmUpper; }
            set { _alarmUpper = value; }
        }

        public Color AlarmColor
        {
            get { return _alarmColor; }
            set { _alarmColor = value; }
        }

        public override string ToString()
        {
            return StringResource.LimitsText;
        }
    }
}
