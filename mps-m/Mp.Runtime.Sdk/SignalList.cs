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
using System.Reflection;

namespace Mp.Runtime.Sdk
{
    /// <summary>
    /// This class implements a signal list.
    /// </summary>
    public class SignalList : List<Signal>
    {
        private List<uint> _signalRefList = new List<uint>();

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SignalList()
        { }


        /// <summary>
        /// Return the data size in the signal list.
        /// </summary>
        /// <returns>The data size.</returns>
        public uint GetDataSize()
        {
            uint size = 0;

            foreach (Signal signal in this)
                size += signal.DataTypeSize;

            return size;     
        }
               
        public void AddSignalReference( uint  sigRef )
        {
            _signalRefList.Add(sigRef);   
        }

        public void LoadSigRef(Hashtable signals)
        {
            Signal signalRef;

            foreach( uint sigRef in _signalRefList )
            {
                signalRef = (Signal)signals[sigRef];
                Signal copySignal = new Signal(signalRef);

                this.Add(copySignal);
                copySignal.SignalIndex = this.Count - 1;
            }

            _signalRefList.Clear();
        }
    }
}

