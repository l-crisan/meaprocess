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
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Globalization;

using Mp.Visual.Analog;
using Mp.Visual.WaveChart;


namespace Mp.Runtime.Sdk
{
    public class GaugeBlockPS : VisualBlockPS
    {

        public GaugeBlockPS()
        {

        }
        protected override Control OnCreateControl(Signal signal)
        {
            Gauge ctrl = new Gauge();

            ctrl.CaptionDefinition[0].Text = signal.Name;
            ctrl.CaptionDefinition[1].Text = signal.Unit;

            ctrl.MinValue = (float)signal.Minimum;
            ctrl.MaxValue = (float)signal.Maximum;
            return ctrl;
        }

        protected override void OnSetControlValue(Control control, double value)
        {
            Gauge ctrl = (Gauge)control;
            ctrl.Value = (float)value;
        }      
    }
}
