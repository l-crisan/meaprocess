using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using System.IO;
using Mp.Utils;
using Mp.Visual.CAN;
using Mp.Runtime.Sdk;

namespace Mp.Rt.Mod.CAN.View
{
    internal class PS : ProcessStation
    {
        private CANLoggerView _view;
        private Hashtable _sigId2TypeMap = new Hashtable();

        public PS()
        {
            Assembly.LoadFrom("Mp.Visual.CAN.dll");
        }

        public override void OnStart()
        {
            _view.Clear();
            _view.Start();
            base.OnStart();
        }

        public override void OnStop()
        {
            _view.Stop();
            base.OnStop();
        }
        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            Signal signal;

            List<Signal> source = GetSource(0, sourceIdx);
            int srcSize = GetSourceSize(0, sourceIdx);
            int lastRecord = srcSize * (records - 1);

            for (int rec = 0; rec < records; ++rec)
            {
                ulong timeStamp = 0;
                int id = 0;
                int dlc = 0;
                byte[] msgData = new byte[8];

                for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; sigIdxInSrc++)
                {
                    signal = source[sigIdxInSrc];

                    if (!_sigId2TypeMap.ContainsKey(signal.SignalID))
                        continue;

                    int type = (int)_sigId2TypeMap[signal.SignalID];

                    MemoryStream ms = new MemoryStream(data);
                    BinaryReader rs = new BinaryReader(ms);
                    int sigOffset = base.GetSignalOffsetInTheSource(0, sigIdxInSrc, sourceIdx);
                    rs.BaseStream.Seek(rec * srcSize + sigOffset, SeekOrigin.Begin);

                    switch (type)
                    {
                        case 0:// time stamp
                            if (signal.DataType == Signal.DataTypes.ULINT)
                                timeStamp = rs.ReadUInt64();
                            else
                                timeStamp = 0;
                        break;

                        case 1://identifier
                            if (signal.DataType == Signal.DataTypes.UDINT)
                                id = (int)rs.ReadUInt32();
                            else
                                id = 0;
                        break;

                        case 2:
                            if (signal.DataType == Signal.DataTypes.USINT)
                                dlc = (int)rs.ReadByte();
                            else
                                dlc = 0;
                        break; //dlc

                        case 3: //data
                            if( signal.DataType == Signal.DataTypes.ULINT)
                                msgData = rs.ReadBytes(8);
                        break;
                    }
                }
                
                if(id != 0)
                    _view.AddMessage(timeStamp, id, dlc, msgData);
            }
        }

        protected override void OnSetupTheControls()
        {
            _view = (CANLoggerView)Controls[0];

            for (int i = 0; i < XmlRep.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement)XmlRep.ChildNodes[i];

                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");

                if (name != "signalTypeMap")
                    continue;

                string[] array = xmlElement.InnerText.Split(new char[] { '/' });
                uint sigId = Convert.ToUInt32(array[0]);
                _sigId2TypeMap[sigId] = Convert.ToInt32(array[1]);
            }
        }
    }
}
