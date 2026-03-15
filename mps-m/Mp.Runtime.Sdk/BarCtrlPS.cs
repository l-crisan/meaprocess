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

using System.Windows.Forms;
using Mp.Visual.Analog;

namespace Mp.Runtime.Sdk
{
    internal class BarCtrlPS : SignalBasedPS
    {
        public BarCtrlPS()
        : base(70)
        {
        }

        public override void OnStart()
        {
            foreach (Bar control in Controls)
                control.Value = 0.0;

            base.OnStart();
        }

        protected override void OnUpdateControlValue(Control control, double value, Signal signal)
        {
            Bar numCtrl = (Bar)control;
            numCtrl.Value = value;
        }

        protected override void OnSetupTheControls()
        {
            base.OnSetupTheControls();

            Bar control;
            Signal signal;

            for (int i = 0; i < Controls.Count; i++)
            {
                control = (Bar)Controls[i];
                signal = GetSignalForControl(control);

                string unit = "";
                if(signal.Unit != null && signal.Unit != "")
                    unit = (" (" + signal.Unit + ")");

                control.SigName = signal.Name + unit;
            }
        }
    }
}
