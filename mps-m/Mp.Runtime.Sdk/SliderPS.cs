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
using System.IO;
using System.Windows.Forms;

namespace Mp.Runtime.Sdk
{
    internal class SliderPS :ProcessStation
    {
        private TrackBar _slider;

        private int _value ;
        
        public SliderPS()
        {             
        }

        public override void OnGetSignalData(int sigInx, byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryWriter sw = new BinaryWriter(ms);

            sw.Write(_value);
            sw.Flush();
            ms.Close();
        }

        public override bool IsOutputPS
        {
            get { return false; }
        }

        protected override void OnSetupTheControls()
        {
            _slider = (TrackBar)Controls[0];
            _value = _slider.Value;
            Signal signal = GetSource(0, 0)[0];
            _slider.Minimum = (int) signal.Minimum;
            _slider.Maximum = (int) signal.Maximum;

            _slider.ValueChanged += new EventHandler(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(object sender, EventArgs e)
        {
            _value = _slider.Value;
        }     
    }    
}
