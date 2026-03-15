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
using System.Windows.Forms;
using System.Text;
using Mp.Visual.Analog;

namespace Mp.Runtime.Sdk
{
    internal class ManometerPS : SignalBasedPS
    {
        private List<Manometer> _gauges = new List<Manometer>();
 
        public ManometerPS() 
        : base(100)
        {
        }

        protected override void OnUpdateControlValue(Control control, double value, Signal signal)
        {
            Manometer guage = (Manometer)control;
            guage.Value = (float)value;
        }

        public override void OnStart()
        {
            foreach (Manometer control in Controls)
                control.StoredMax = 0.0f;
            
            base.OnStart();
        }

        protected override void OnSetupTheControls()
        {
            base.OnSetupTheControls();

            Manometer control;
            Signal signal;
            for (int i = 0; i < Controls.Count; i++)
            {
                control = (Manometer)Controls[i];
                signal = GetSignalForControl(control);
                control.TextUnit = signal.Unit;
                control.TextDescription = signal.Name;

                _gauges.Add(control);
            }
        }
    }
}
