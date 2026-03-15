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
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;

using Mpal.Model;

namespace Mpal.Debugger
{   
    public delegate void OnUpdateDebuggerStateDelegate(int line, string unit, string callStack);
    public delegate void OnMessageDelegate(string message);
    public delegate void OnTerminateDelegate();

    public class Debugger
    {
        private string _serverIP;
        private int _serverPort;
        private VariableManager _varMannager;
        private string _mppFile;
        private Process _debuggerProcess;
        private bool _running = false;
        private bool _buildInDebugger = true;
        private TcpClient _socket;
        private AutoResetEvent _onDataEvent = new AutoResetEvent(false);
        private byte[] _responceBuffer;
        private int _responceBufferSize = 0;
        private string _responceString;
        private byte[] _buffer = new byte[1];
        private static readonly int _timeout = 2000;
        private Mutex _mutex = new Mutex();

        public event OnUpdateDebuggerStateDelegate UpdateDebuggerStateEvent;
        public event OnTerminateDelegate TerminateEvent;
        public event OnMessageDelegate MessageEvent;
        private uint _vmMemSize = 10;

        private enum Command
        {
            InitInputStack = 1,
            ClearBreakPoints,
            InsertBreakPoint,
            RemoveBreakPoint,
            StartDebugger,
            StepOver,
            StepInto,
            GetCallStack,
            ContinueExecution,
            Terminate,
            ReadMemoryByRef,
            ReadMemoryFromInstance,
            ReadMemoryByOffset,
            SetVmMemSize,
            Responce,
            
            NotifyOnline = 100,
            NotifyOnTerminate,
            NotifyOnMessage,
            NotifyOK
        }

        public Debugger()
        {            
        }

        public void Setup(string serverIP, int serverPort, bool buildIn, uint memSize)
        {
            _vmMemSize = memSize;

            _buildInDebugger = buildIn;
            _serverIP = serverIP;
            _serverPort = serverPort;
        }

        public VariableManager Load(Unit unit, string mppFile)
        {
            _mppFile = mppFile;
            _varMannager = new VariableManager(this, unit);
            return _varMannager;
        }

        public void Start(DebuggerSettings settings)
        {            
            string debuggerExe;

            _running = true;

            if (_buildInDebugger)
            {
                        debuggerExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"mpal-debugger.exe");
                        ProcessStartInfo psi = new ProcessStartInfo();
                        psi.UseShellExecute = false;
                        psi.CreateNoWindow = true;
                        string command = AppDomain.CurrentDomain.BaseDirectory;
                        psi.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        psi.Arguments = "\"" + _mppFile + "\" " + _serverIP + " " + _serverPort.ToString();
                        psi.FileName = debuggerExe;
                        _debuggerProcess = Process.Start(psi);

                        Thread.Sleep(500);

                        _socket = new System.Net.Sockets.TcpClient(_serverIP, _serverPort);
                        _socket.GetStream().BeginRead(_buffer, 0, _buffer.Length, new AsyncCallback(OnReadEvent), this);
                        SetMemSize();
            }
            else
            {
                _socket = new System.Net.Sockets.TcpClient();
                _socket.Connect(_serverIP, _serverPort);
                _socket.GetStream().BeginRead(_buffer, 0, _buffer.Length, new AsyncCallback(OnReadEvent), this);
                Continue();
            }

            SetBreakPoints(settings);

            if(_buildInDebugger)
            {                
                InitInputStack(settings);
                StartDebugging();
            }

        }

        private static void OnReadEvent(IAsyncResult result)
        {            
            Debugger debugger = (Debugger)result.AsyncState;
                      
            if (debugger._socket == null)
                return;

            Stream s = null;
            try
            {
                s = debugger._socket.GetStream();
                s.EndRead(result);
            
                BinaryReader br = new BinaryReader(s);

                switch ((Command)debugger._buffer[0])
                {
                    case Command.NotifyOnline:
                    {
                        int line = br.ReadInt32();
                        string fname = ReadString(s);
                        s.BeginRead(debugger._buffer, 0, 1, new AsyncCallback(OnReadEvent), debugger);

                        debugger.OnLine(line, fname);
                    
                    }
                    break;
                
                    case Command.NotifyOnTerminate:
                    {                    
                        s.BeginRead(debugger._buffer, 0, 1, new AsyncCallback(OnReadEvent), debugger);
                        debugger.OnTerminate();
                    }
                    break;
                
                    case Command.NotifyOnMessage:
                    {
                        string message = ReadString(s);
                        s.BeginRead(debugger._buffer, 0, 1, new AsyncCallback(OnReadEvent), debugger);

                        debugger.OnMessage(message);
                    }
                    break;
                
                    case Command.Responce:
                    {
                        if (debugger._responceBufferSize != 0)
                            debugger._responceBuffer = br.ReadBytes(debugger._responceBufferSize);
                        else
                            debugger._responceString = ReadString(s);

                        s.BeginRead(debugger._buffer, 0, 1, new AsyncCallback(OnReadEvent), debugger);
                        debugger._onDataEvent.Set();
                    }
                    break;

                    case Command.NotifyOK:
                    {
                        s.BeginRead(debugger._buffer, 0, 1, new AsyncCallback(OnReadEvent), debugger);
                        debugger._onDataEvent.Set();
                    }
                    break;

                    default:
                    {
                    }
                    break; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

        }

        private static string ReadString(Stream sr)
        {
            try
            {
                List<byte> data = new List<byte>();
                byte bdata = (byte)sr.ReadByte();
                data.Add(bdata);

                while (bdata != 0)
                {
                    bdata = (byte)sr.ReadByte();
                    data.Add(bdata);
                }

                data.RemoveAt(data.Count - 1);

                UTF8Encoding encoding = new UTF8Encoding();
                return encoding.GetString(data.ToArray());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }

        private void ClearBreakPoints()
        {
            byte[] data = new byte[1];
            data[0] = (byte)Command.ClearBreakPoints;
            Send(data);

            if (!_onDataEvent.WaitOne(_timeout))
                throw new Exception("ClearBreakPoints() filed");
        }


        private string GetCallStack()
        {
            _responceBufferSize = 0;
            _responceString = "";

            byte[] data = new byte[1];
            data[0] = (byte)Command.GetCallStack;
            Send(data);

            if (!_onDataEvent.WaitOne(_timeout))
                throw new Exception("GetCallStack() failed");

            return _responceString;
        }

        private void SetBreakPoints(DebuggerSettings settings)
        {
            ClearBreakPoints();

            foreach (DebuggerBreakpoint breakPoint in settings.BreakPoints)
                InsertBreakPoint(breakPoint);
        }

        private void SetMemSize()
        {
            byte[] data = new byte[5];
            MemoryStream mm = new MemoryStream(data);
            BinaryWriter bw = new BinaryWriter(mm);

            bw.Write((byte)Command.SetVmMemSize);
            bw.Write((int)_vmMemSize);
            
            Send(data);
            if (!_onDataEvent.WaitOne(_timeout))
                throw new Exception("SetMemSize() filed");
        }


        public void InsertBreakPoint(DebuggerBreakpoint bp)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] stringData = encoding.GetBytes(bp.Unit);

            byte[] data = new byte[1 + sizeof(int) + stringData.Length + 1];
            
            BinaryWriter bw = new BinaryWriter(new MemoryStream(data));
            
            bw.Write((byte)Command.InsertBreakPoint);
            bw.Write(bp.Line);
            bw.Write(stringData);
            bw.Write((byte)0);
            bw.Flush();
            Send(data);

            if (!_onDataEvent.WaitOne(_timeout))
                throw new Exception("InsertBreakPoint() failed");
        }

        public void RemoveBreakPoint(DebuggerBreakpoint bp)
        {
            UTF8Encoding encoding = new UTF8Encoding();

            byte[] stringData = encoding.GetBytes(bp.Unit);
            byte[] data = new byte[1 + sizeof(int) + stringData.Length + 1];

            BinaryWriter bw = new BinaryWriter(new MemoryStream(data));
            
            bw.Write((byte)Command.RemoveBreakPoint);
            bw.Write(bp.Line);
            bw.Write(stringData);
            bw.Write((byte)0);
            bw.Flush();

            Send(data);

            if (!_onDataEvent.WaitOne(_timeout))
                throw new Exception("RemoveBreakPoint() failed");
        }

        private void InitInputStack(DebuggerSettings settings)
        {            
            StringBuilder sb = new StringBuilder();
            
            foreach (string varstr in settings.InputVarValues)
            {
                string[] array = varstr.Split(';');
                sb.Append(array[1]);
                sb.Append("\n");
            }

            string strdata = sb.ToString();
            
            if (strdata.Length == 0)
                return;


            UTF8Encoding encoding = new UTF8Encoding();
            byte[] stringData = encoding.GetBytes(strdata);

            byte[] data = new byte[1 + stringData.Length + 1];

            BinaryWriter bw = new BinaryWriter(new MemoryStream(data));
            
            bw.Write((byte)Command.InitInputStack);
            bw.Write(stringData);
            bw.Write((byte)0);
            bw.Flush();
            
            Send(data);

            if (!_onDataEvent.WaitOne(_timeout))
                throw new Exception("InitInputStack() failed");
        }

        private void StartDebugging()
        {
            byte[] data = new byte[1];
            data[0] = (byte) Command.StartDebugger;
            Send(data);

            if(!_onDataEvent.WaitOne(_timeout))
                throw new Exception("StartDebugging() failed");
        }

        public void StepOver()
        {
            _mutex.WaitOne();
            byte[] data = new byte[1];
            data[0] = (byte)Command.StepOver;
            Send(data);
            _mutex.ReleaseMutex();
        }

        public void StepInto()
        {
            _mutex.WaitOne();
            byte[] data = new byte[1];
            data[0] = (byte)Command.StepInto;
            Send(data);
            _mutex.ReleaseMutex();
        }

        public void Continue()
        {
            _mutex.WaitOne();
            byte[] data = new byte[1];
            data[0] = (byte)Command.ContinueExecution;
            Send(data);
            _mutex.ReleaseMutex();
        }

        public void Terminate()
        {
            if (!_running)
                return;

            _mutex.WaitOne();
            
            try
            {
                _running = false;

                if (!_buildInDebugger)
                {                 
                    ClearBreakPoints();
                    Continue();

                    if (TerminateEvent != null)
                        TerminateEvent();

                    if (_socket != null)
                    {                    
                        _socket.Close();
                        _socket = null;                    
                    }

                }
                else
                {
                    byte[] data = new byte[1];
                    data[0] = (byte)Command.Terminate;
                    Send(data);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _mutex.ReleaseMutex();
        }

        public bool OnTerminate()
        {
            if (_buildInDebugger)
            {
                _running = false;

                if (TerminateEvent != null)
                    TerminateEvent();

                if (_socket != null)
                {                    
                    _socket.Close();
                    _socket = null;                 
                }
            }

            return true;
        }

        public bool OnMessage(string msg)
        {
            if (MessageEvent != null)
                MessageEvent(msg);

            return true;
        }

        public bool OnLine(int line, string fname)
        {
            _mutex.WaitOne();

            Debug.WriteLine("OnLine()");
            _varMannager.UpdateVariables(fname);
            
            string callStack = GetCallStack();

            if (UpdateDebuggerStateEvent != null)
                UpdateDebuggerStateEvent(line, fname, callStack);

            _mutex.ReleaseMutex();

            return true;
        }

        public byte[] ReadMemoryByRef(Parameter variable, string funcName)
        {
            _responceBufferSize = (int) variable.Size;
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] stringData = encoding.GetBytes(funcName);

            byte[] data = new byte[9 + stringData.Length + 1];
            
            BinaryWriter bw = new BinaryWriter(new MemoryStream(data));
            
            bw.Write((byte)Command.ReadMemoryByRef);
            bw.Write((int)variable.Offset);
            bw.Write((int)variable.Size);
            bw.Write(stringData);
            bw.Write((byte)0);
            bw.Flush();

            Send(data);

            if (!_onDataEvent.WaitOne(_timeout))
                return null;

            _responceBufferSize = 0;
            return _responceBuffer;
        }

        public byte[] ReadInstanceData(Function function)
        {
            _responceBufferSize = (int) function.FuncBlockSize;
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] stringData = encoding.GetBytes(function.Name);

            byte[] data = new byte[9 + stringData.Length +1];
            BinaryWriter bw = new BinaryWriter(new MemoryStream(data));
            bw.Write((byte)Command.ReadMemoryFromInstance);
            bw.Write((int)0);
            bw.Write((int)function.FuncBlockSize);
            bw.Write(stringData);
            bw.Write((byte)0);
            bw.Flush();
            Send(data);

            if (!_onDataEvent.WaitOne(_timeout))
            {
                return null;
            }

            _responceBufferSize = 0;
            return _responceBuffer;
        }

        public byte[] ReadStackData(Function function)
        {            
            _responceBufferSize= (int) function.StackSize;

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] stringData = encoding.GetBytes(function.Name);
            
            byte[] data = new byte[9 + stringData.Length + 1];

            BinaryWriter bw = new BinaryWriter(new MemoryStream(data));

            bw.Write((byte)Command.ReadMemoryByOffset);
            bw.Write((int)0);
            bw.Write((int)function.StackSize);
            bw.Write(stringData);
            bw.Write((byte)0);
            bw.Flush();

            Send(data);
            Debug.WriteLine("ReadStackData() begin");

            if (!_onDataEvent.WaitOne(_timeout))
            {
                return null;
            }

            Debug.WriteLine("ReadStackData() end");
            _responceBufferSize = 0;            
            return _responceBuffer;
        }

        private void Send(byte[] data)
        {            
            try
            {                
                Stream sr = _socket.GetStream();
                sr.Write(data, 0, data.Length);
                sr.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }
    }
}
