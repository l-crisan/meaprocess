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
using System.IO;
using System.Windows.Forms;
using Mp.Runtime.Adaption;
using Mp.Visual.Digital;

namespace Mp.Runtime.Sdk
{
    internal class RadioButtonPS : ProcessStation
    {
        private Visual.Digital.RadioButton _button;

        public RadioButtonPS()
        {
        }

        public override void OnGetSignalData(int sigInx, byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(_button.Value);
        }

        public override bool IsOutputPS
        {
            get { return false; }
        }

        protected override void OnSetupTheControls()
        {
            _button = (Visual.Digital.RadioButton)Controls[0];
            _button.UpdateView();
        }
    }
}
