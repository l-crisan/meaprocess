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
using System.Collections;
using System.Runtime.InteropServices;

namespace Mp.Runtime.Adaption
{
    public delegate void MpsOnData(uint objID, uint status, uint noOfRecords, uint sourceIdx, uint portNo, uint dataSize, IntPtr data);

    public class OutputPS : InputPS
    {
        private static uint _objID = 0;
        private uint _myID = 0;
        private static Hashtable _id2obj = new Hashtable();
        private MpsOnData _onData = new MpsOnData(OnData);
        private byte[] _dataBuffer;

	    public OutputPS()
        {
            _objID++;
            _id2obj[_objID] = this;
            _myID = _objID;
        }

        private static void OnData(uint objID, uint status, uint noOfRecords, uint sourceIdx, uint portNo, uint dataSize, IntPtr data)
        {
            OutputPS ps = (OutputPS)_id2obj[objID];

            switch (status)
            {
                case 0:
                {
                    if (ps._dataBuffer == null)
                    {
                        ps._dataBuffer = new byte[dataSize];
                    }
                    else
                    {
                        if(ps._dataBuffer.Length  < dataSize)
                            ps._dataBuffer = new byte[dataSize];
                    }

                    Marshal.Copy(data,  ps._dataBuffer, 0, (int)dataSize);
                    ps.OnUpdateDataValue(ps._dataBuffer, (int)sourceIdx, (int)portNo, (int)noOfRecords);
                }
                break;

                case 1:
                    ps.OnStartOutputPS();
                break;

                case 2:
                    ps.OnStopOutputPS();
                break;
            }
        }

        public MpsOnData OutDataCallBack
        {
            get { return _onData; }
        }


        public uint OutObjectID
        {
            get { return _myID; }
        }

        protected virtual void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
        }

        protected virtual void OnStartOutputPS()
        {
        }

        protected virtual void OnStopOutputPS()
        {
        
        }
    }    
}
