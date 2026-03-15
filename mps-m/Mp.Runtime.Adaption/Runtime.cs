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
using System.Text;
using System.Runtime.InteropServices;

namespace Mp.Runtime.Adaption
{
    public delegate int OnNewMessageDelegate(Message message);


    public class Runtime
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct MpsMessage
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
            public string text;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
            public string comment;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 200)]
            public string fileName;

            public int target;
            public int type;
            public long timeStamp;
            public uint errorCode;
        }

        private delegate int MpsOnMessage(ulong objID, ref MpsMessage msg);
        public event OnNewMessageDelegate MessageEvent;
        private ulong _handle = 0;
        private static uint _objID = 0;
        private uint _myID = 0;
        private static Hashtable _id2obj = new Hashtable();
        private static MpsOnMessage _onMsg = new MpsOnMessage(OnMessage);

        public Runtime()
        {
            _objID++;
            _id2obj[_objID] = this;
            _myID = _objID;
        }	

        private static int OnMessage(ulong objID, ref MpsMessage msg)
        {
            Runtime rt = (Runtime)_id2obj[(uint)objID];
            return rt.OnMessage(ref msg);
        }

        private int OnMessage(ref MpsMessage msg)
        {
            if (MessageEvent == null)
                return 0;

            Message message = new Message();
            message.Comment = Utf7EncodeString(msg.comment);
            message.FileName = Utf7EncodeString(msg.fileName);
            DateTime year1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime dt = new DateTime((long)(msg.timeStamp* 10000) + year1970.Ticks);
            message.TargetType = (Message.Target) msg.target;

            message.Text = Utf7EncodeString(msg.text);
            message.TimeStamp = dt;
            message.Type = (Message.MessageType)msg.type;
            int res = MessageEvent(message);
            msg.fileName = message.FileName;
            return res;
        }

        private static string Utf7EncodeString(string text)
        {
            UTF7Encoding encode = new UTF7Encoding();

            char[] chars = text.ToCharArray();
            byte[] bytes = new byte[chars.Length];

            for (int i = 0; i < chars.Length; ++i)
                bytes[i] = (byte)chars[i];

            return encode.GetString(bytes);
        }

        private static string Utf8EncodeString(string text)
        {
            UTF8Encoding encode = new UTF8Encoding();

            char[] chars = text.ToCharArray();
            byte[] bytes = new byte[chars.Length];

            for (int i = 0; i < chars.Length; ++i)
                bytes[i] = (byte)chars[i];

            return  encode.GetString(bytes);
        }

        public void OnNewMessage(Message msg)
        {
            if (MessageEvent == null)
                MessageEvent(msg);
        }

        public bool LoadFromXml( string xmlRepRuntime )
        {
            MpsMessage message = new MpsMessage();

            UTF8Encoding encode = new UTF8Encoding();
            int size = encode.GetByteCount(xmlRepRuntime) + 1;
            byte[] bytestr = new byte[size];
            encode.GetBytes(xmlRepRuntime, 0, xmlRepRuntime.Length, bytestr, 0);

            _handle = mpsLoadFromXML(bytestr, ref message);

            if (_handle == 0)
            {
                OnMessage(ref message);
                return false;
            }
                         
            mpsSetExecDirectory(_handle, AppDomain.CurrentDomain.BaseDirectory);
            mpsSetMessageListener(_handle, _myID, _onMsg);
            return _handle != 0;
        }

	    public void Initialize()
        {
            mpsInitialize(_handle);
        }

	    public void Start()
        {
            mpsStart(_handle);
        }

	    public void Stop()
        {
            mpsStop(_handle);
        }

	    public void Deinitialize()
        {
            mpsDeinitilize(_handle);
        }
	    
        public void addDataOutListener( uint dataOutPSId, OutputPS ps )
        {
            mpsAddDataListener(_handle, ps.OutObjectID, dataOutPSId, ps.OutDataCallBack);
        }

	    public void addDataSource(uint dataOutPSId, InputPS ps )
        {
            mpsAddDataSource(_handle, ps.ObjectID, dataOutPSId, ps.DataCallBack);
        }
	    
        public void FreeHandle()
        {
            if(_handle != 0)
                mpsReleaseRuntime(_handle);
        }
	    
        public void UnloadAllModules()
        {
            try
            {
                mpsUnloadAllModules();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }	          

	    public void SetProperty(string name, string value)
        {
            if (value.Length == 0)
                return;

            UTF8Encoding encode = new UTF8Encoding();
            int size = encode.GetByteCount(value) + 1;

            byte[] byteValue = new byte[size];
            encode.GetBytes(value, 0, value.Length, byteValue, 0);

            size = encode.GetByteCount(name) + 1;
            byte[] byteName = new byte[size];
            encode.GetBytes(name, 0, name.Length, byteName, 0);

            mpsSetProperty(_handle, byteName, byteValue);
        }

	    public string GetPropertyValue(string name)
        {
            const int bufferSize = 1024; //Buffer for an elefant.

            //Alocate the memory for the return value
            IntPtr pstr = Marshal.AllocCoTaskMem(bufferSize);

            //Encode the name to utf8
            UTF8Encoding encode = new UTF8Encoding();
            int size = encode.GetByteCount(name) + 1;
            byte[] bytestr = new byte[size];
            encode.GetBytes(name, 0, name.Length, bytestr, 0);

            //Read the property
            mpsGetProperty(_handle, bytestr, out pstr);

            //Decode the return value from utf8            
            byte[] resdata = new byte[bufferSize];

            Marshal.Copy(pstr, resdata, 0, bufferSize);
            int count = 0;
            
            //How many character have we get.
            while(resdata[count] != 0)
                count++;

            string value = encode.GetString(resdata,0, count);

            //Free the allocated memory
            Marshal.FreeCoTaskMem(pstr);

            return value;            
        }
	
        public static bool LoadModule(string path)
        {
            UTF8Encoding encode = new UTF8Encoding();
            
            int size = encode.GetByteCount(path) + 1;
            byte[] bytestr = new byte[size];
            encode.GetBytes(path, 0, path.Length, bytestr, 0);

            return mpsLoadModule(bytestr) != 0;
        }
	
        public void SetLanguage(string languageCode)
        {
            if (_handle == 0)
                return;

            if (languageCode.Length == 0 )
                return;

            UTF8Encoding encode = new UTF8Encoding();
            int size = encode.GetByteCount(languageCode) + 1;
            byte[] lanCodeBytes = new byte[size];
            encode.GetBytes(languageCode, 0, languageCode.Length, lanCodeBytes, 0);

            mpsSetLanguage(_handle, lanCodeBytes);
        }

        public void SetLogFile(string file, int level)
        {
            UTF8Encoding encode = new UTF8Encoding();

            int size = encode.GetByteCount(file) + 1;
            byte[] bytestr = new byte[size];
            encode.GetBytes(file, 0, file.Length, bytestr, 0);

            mpsSetLogFile(bytestr, level);
        }

        [DllImport("mps-api", EntryPoint = "mpsLoadFromXML", CallingConvention = CallingConvention.Cdecl)]
        private static extern ulong mpsLoadFromXML(byte[] xmlString, ref MpsMessage msg);

        [DllImport("mps-api", EntryPoint = "mpsSetExecDirectory", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsSetExecDirectory(ulong handle, string path);

        [DllImport("mps-api", EntryPoint = "mpsStart", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsStart(ulong handle);

        [DllImport("mps-api", EntryPoint = "mpsInitialize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsInitialize(ulong handle);

        [DllImport("mps-api", EntryPoint = "mpsStop", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsStop(ulong handle);

        [DllImport("mps-api", EntryPoint = "mpsDeinitilize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsDeinitilize(ulong handle);

        [DllImport("mps-api", EntryPoint = "mpsAddDataSource", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsAddDataSource(ulong handle, uint objId, uint psID, MpsOnGetSignalData callBack);

        [DllImport("mps-api", EntryPoint = "mpsAddDataListener", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsAddDataListener(ulong handle, uint objId, uint psID, MpsOnData callBack);

        [DllImport("mps-api", EntryPoint = "mpsReleaseRuntime", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsReleaseRuntime(ulong handle);

        [DllImport("mps-api", EntryPoint = "mpsUnloadAllModules", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsUnloadAllModules();

        [DllImport("mps-api", EntryPoint = "mpsSetMessageListener", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsSetMessageListener(ulong handle, ulong objId, MpsOnMessage callBack);

        [DllImport("mps-api", EntryPoint = "mpsSetProperty", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsSetProperty(ulong handle, byte[] name, byte[] value);

        [DllImport("mps-api", EntryPoint = "mpsSetLanguage", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsSetLanguage(ulong handle, byte[] code);

        [DllImport("mps-api", EntryPoint = "mpsGetProperty", CallingConvention = CallingConvention.Cdecl)]
        private static extern void mpsGetProperty(ulong handle, byte[] name, out IntPtr value);

        [DllImport("mps-api", EntryPoint = "mpsLoadModule", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte mpsLoadModule(byte[] file);

        [DllImport("mps-api", EntryPoint = "mpsSetLogFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool mpsSetLogFile(byte[] file, int level);

    }
}
