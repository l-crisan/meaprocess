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
using Mp.Runtime.Sdk;
using System.IO;

namespace Mp.Runtime.App
{
    internal class SystemInputPS : ProcessStation
    {
        public double[] _data;

        public SystemInputPS()
        {
        }

        public override void OnStart()
        {
            SignalList sigList = GetPortSignals(0);
            _data = new double[sigList.Count];

            base.OnStart();
        }

        public override void OnGetSignalData(int sigInx, byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryWriter br = new BinaryWriter(ms);
            br.Write(_data[sigInx]);
        }


        public override bool IsOutputPS
        {
            get { return false; }
        }

        public void SetSignalValue(int index, double value)
        {
            _data[index] = value;
        }
    }
}
