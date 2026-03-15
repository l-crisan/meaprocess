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

public delegate void MpsOnGetSignalData(uint objID, uint status, uint sigInx, uint dataSize, IntPtr data);

public class InputPS
{
    private static uint _objID = 0;
    private uint _myID = 0;
    private static Hashtable _id2obj = new Hashtable();
    private byte[] _dataBuffer;
    private MpsOnGetSignalData _onGetData = new MpsOnGetSignalData(OnGetSignalData);

	public InputPS()
    {
        _objID++;
        _id2obj[_objID] = this;
        _myID = _objID;
	}

    static private void OnGetSignalData(uint objID, uint status, uint sigInx, uint dataSize, IntPtr data)
    {
        InputPS ps = (InputPS) _id2obj[objID];

        switch(status)
        {
            case 0:
            {
                if (ps._dataBuffer == null)
                {
                    ps._dataBuffer = new byte[dataSize];
                }
                else
                {
                    if (ps._dataBuffer.Length < dataSize)
                        ps._dataBuffer = new byte[dataSize];
                }

                ps.OnGetSignalData((int)sigInx, ps._dataBuffer);
                Marshal.Copy(ps._dataBuffer, 0, data, (int)dataSize);
            }
            break;

            case 1:
                ps.OnStartInputPS();
            break;

            case 2:
                ps.OnStopInputPS();
            break;
        }
    }

    public MpsOnGetSignalData DataCallBack
    {
        get { return _onGetData; }
    }


    public uint ObjectID
    {
        get { return _myID; }
    }

	public virtual void OnGetSignalData(int sigInx, byte[] data)
    {
    }

    public virtual void OnStartInputPS()
    {
    }
    public 	virtual void OnStopInputPS()
    {
    }
}

}
