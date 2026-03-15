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

using Mp.XmlRpc;
using Mp.Runtime.Sdk;

namespace Mp.Runtime.App
{

    internal class MeaProcessRPC
    {
        private XmlRpcServer _xmlRpcServer;
        private MainFrame _mf;
        private delegate void OnCallDelegate();
        private delegate void OnCallVisibleDelegate(bool v);

        public MeaProcessRPC(MainFrame mf, string ip, int port)
        {
            _mf = mf;

            _mf.CreateStartUpMutex(ip + ":" + port.ToString());

            _xmlRpcServer = new XmlRpcServer(ip, port);
            _xmlRpcServer.Add("Start", this);
            _xmlRpcServer.Add("Close", this);
            _xmlRpcServer.Add("Reinitialize", this);
            _xmlRpcServer.Add("SetVisible", this);
            _xmlRpcServer.Add("GetVisible", this);
            _xmlRpcServer.Add("Stop", this);
            _xmlRpcServer.Add("GetNumberOfOutputSignals", this);
            _xmlRpcServer.Add("GetNumberOfInputSignals", this);
            _xmlRpcServer.Add("GetSignalValue", this);
            _xmlRpcServer.Add("GetSignalName", this);
            _xmlRpcServer.Add("GetSignalUnit", this);
            _xmlRpcServer.Add("GetSignalComment", this);
            _xmlRpcServer.Add("GetSignalMin", this);
            _xmlRpcServer.Add("GetSignalMax", this);
            _xmlRpcServer.Add("SetSignalValue", this);
            _xmlRpcServer.Add("ReadMessages", this); 

            _xmlRpcServer.Start();
        }


        public void StopServer()
        {
            _xmlRpcServer.Stop();
        }

        public void Close()
        {
            _mf.Close();
        }

        public string ReadMessages()
        {
            return _mf.Messages;
        }

        public void Reinitialize()
        {
            _mf.Invoke(new OnCallDelegate(_mf.Reinitialize), null);
        }

        public void Start()
        {
            _mf.Invoke(new OnCallDelegate(_mf.Start), null);
        }

        public void SetVisible(bool v)
        {
            object[] array = new object[1];
            array[0] = v;
            _mf.Invoke(new OnCallVisibleDelegate(OnSetVisible), array);
        }


        public bool GetVisible()
        {
            return _mf.Visible;
        }

        private void OnSetVisible(bool v)
        {
            _mf.Visible = v;
        }

        public void Stop()
        {
            _mf.Invoke(new OnCallDelegate(_mf.Stop), null);
        }


        public string GetMessages()
        {
            return "";
        }


        public int GetNumberOfInputSignals()
        {
            if(_mf.SysInPS == null)
              return 0;

            return _mf.SysInPS.GetPortSignals(0).Count;
        }


        public int GetNumberOfOutputSignals()
        {
            if (_mf.SysOutPS == null)
                return 0;

            return _mf.SysOutPS.GetPortSignals(0).Count;
        }


        public string GetSignalName(int index, bool input)
        {
            SignalList sigList = null;

            if (input)
            {
                if (_mf.SysInPS == null)
                    return "";

                sigList = _mf.SysInPS.GetPortSignals(0);
            }
            else
            {
                if (_mf.SysOutPS == null)
                    return "";

                sigList = _mf.SysOutPS.GetPortSignals(0);
            }

            return sigList[index].Name;
        }


        public string GetSignalComment(int index, bool input)
        {
            SignalList sigList = null;

            if (input)
            {
                if (_mf.SysInPS == null)
                    return "";

                sigList = _mf.SysInPS.GetPortSignals(0);
            }
            else
            {
                if (_mf.SysOutPS == null)
                    return "";

                sigList = _mf.SysOutPS.GetPortSignals(0);
            }

            return sigList[index].Comment;
        }


        public string GetSignalUnit(int index, bool input)
        {
            SignalList sigList = null;

            if (input)
            {
                if (_mf.SysInPS == null)
                    return "";

                sigList = _mf.SysInPS.GetPortSignals(0);
            }
            else
            {
                if (_mf.SysOutPS == null)
                    return "";

                sigList = _mf.SysOutPS.GetPortSignals(0);
            }

            return sigList[index].Unit;
        }


        public double GetSignalMin(int index, bool input)
        {
            SignalList sigList = null;

            if (input)
            {
                if (_mf.SysInPS == null)
                    return 0;

                sigList = _mf.SysInPS.GetPortSignals(0);
            }
            else
            {
                if (_mf.SysOutPS == null)
                    return 0;

                sigList = _mf.SysOutPS.GetPortSignals(0);
            }

            return sigList[index].Minimum;
        }


        public double GetSignalMax(int index, bool input)
        {
            SignalList sigList = null;

            if (input)
            {
                if (_mf.SysInPS == null)
                    return 0;

                sigList = _mf.SysInPS.GetPortSignals(0);
            }
            else
            {
                if (_mf.SysOutPS == null)
                    return 0;

                sigList = _mf.SysOutPS.GetPortSignals(0);
            }

            return sigList[index].Maximum;
        }


        public double GetSignalValue(int index)
        {
    
            if (_mf.SysOutPS == null)
                return 0;

            return _mf.SysOutPS.GetSignalValue(index); 
        }


        public void SetSignalValue(int index, double value)
        {
            if (_mf.SysInPS == null)
                return;

            _mf.SysInPS.SetSignalValue(index, value); 
        }
    }
}
