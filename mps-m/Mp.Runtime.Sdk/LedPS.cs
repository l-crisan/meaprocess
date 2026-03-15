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
    internal class LedPS : SignalBasedPS
    {
        public LedPS()
        :base(50)
        {
        }

        public override void OnStart()
        {
            foreach (Mp.Visual.Digital.LabelLed control in Controls)
                control.On = false;

            base.OnStart();
        }

        protected override void OnUpdateControlValue(Control control, double value, Signal signal)
        {
            Mp.Visual.Digital.LabelLed ctrl = (Mp.Visual.Digital.LabelLed)control;
            ctrl.On = (value != 0);
        }

        protected override void OnSetupTheControls()
        {
            base.OnSetupTheControls();

            Mp.Visual.Digital.LabelLed control;
            Signal signal;
            //Setup the unit and the label.
            for (int i = 0; i < Controls.Count; i++)
            {
                control = (Mp.Visual.Digital.LabelLed)Controls[i];
                signal = GetSignalForControl(control);
                control.LabelText = signal.Name;
            }
        }
    }
}