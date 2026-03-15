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

namespace Mp.Runtime.Sdk
{
    class TachometerPS : SignalBasedPS
    {
        public TachometerPS()
            :base(100)
        {
        }

        public override void OnStart()
        {
            foreach (Mp.Visual.Analog.Tachometer control in Controls)
            {
                control.Value = 0.0f;
                control.RequiresRedraw = true;
            }

            base.OnStart();
        }

        protected override void OnUpdateControlValue(Control control, double value, Signal signal)
        {
            Mp.Visual.Analog.Tachometer ctrl = (Mp.Visual.Analog.Tachometer)control;
            ctrl.Value = (float)value;
        }

        protected override void OnSetupTheControls()
        {
            base.OnSetupTheControls();

            Mp.Visual.Analog.Tachometer control;
            Signal signal;
            //Setup the unit and the label.
            for (int i = 0; i < Controls.Count; i++)
            {
                control = (Mp.Visual.Analog.Tachometer)Controls[i];
                signal = GetSignalForControl(control);

                if (signal.Unit != "" && signal.Unit != null)
                    control.DialText = signal.Name + " (" + signal.Unit + ")";
                else
                    control.DialText = signal.Name;
            }
        }
    }
}
