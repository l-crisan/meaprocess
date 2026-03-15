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
using System.Reflection;
using System.Windows.Forms;
using Mp.Runtime.Adaption;

namespace Mp.Runtime.Sdk
{
    internal class CheckBoxPS : ProcessStation
    {
        private CheckBox _button;

        public CheckBoxPS()
        {
        }

        public override void OnGetSignalData(int sigInx, byte[] data)
        {
            if (_button.Checked)
                data[0] = 1;
            else
                data[0] = 0;
        }

        public override bool IsOutputPS
        {
            get { return false; }
        }

        protected override void OnSetupTheControls()
        {
            _button = (CheckBox)Controls[0];
        }
    }
}
