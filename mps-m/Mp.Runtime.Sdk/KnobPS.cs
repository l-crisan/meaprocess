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
using System.Collections;
using System.Text;
using System.IO;
using Mp.Visual.Analog;

namespace Mp.Runtime.Sdk
{
    internal class KnobPS :ProcessStation
    {
        private Knob _knob;
        
        public KnobPS()
        {             
        }

        public override void OnGetSignalData(int sigInx, byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryWriter sw = new BinaryWriter(ms);
            sw.Write((double) _knob.Value);
            sw.Flush();
            ms.Close();
        }

        public override void OnStart()
        {
            base.OnStart();
            _knob.Value = _knob.Value;
        }

        public override bool IsOutputPS
        {
            get { return false; }
        }

        protected override void OnSetupTheControls()
        {
            _knob = (Knob)Controls[0];
            Signal signal = GetSource(0,0)[0];
            _knob.MinValue = (float)signal.Minimum;
            _knob.MaxValue = (float)signal.Maximum;
            _knob.Invalidate();
        }     
    }
}
