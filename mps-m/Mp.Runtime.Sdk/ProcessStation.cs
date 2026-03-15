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
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using Mp.Runtime.Adaption;
using Mp.Utils;

namespace Mp.Runtime.Sdk
{
    /// <summary>
    /// This class is the base class for the visual representation of a process station.
    /// </summary>
    public class ProcessStation : OutputPS
    {
        private XmlElement _xmlRep;
        private List<Control> _controls = new List<Control>();
        private List<List<int>> _sourceSize = new List<List<int>>();
        private Hashtable _signalLists = null;
        private ulong _psid;
        private List<SortedList<uint, List<Signal>>> _sources = new List<SortedList<uint, List<Signal>>>();
        private List<SortedList<int, List<int>>> _signalDataOffset = new List<SortedList<int, List<int>>>();
        private List<SignalList> _portSignals = new List<SignalList>();


        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProcessStation()
        {
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~ProcessStation()
        {
        }

        public XmlElement XmlRep
        {
            get { return _xmlRep; }
            set { _xmlRep = value; }
        }

        public Signal GetSignal(int port, int id)
        {
            SignalList list = _portSignals[port];
            foreach(Signal sig in list)
            {
                if( sig.SignalID == id)
                    return sig;
            }

            return null;
        }

        public static XmlDocument GetRuntimeDocument(XmlDocument xmlDoc)
        {
            XmlDocument newDoc = xmlDoc.Clone() as XmlDocument;

            //Remove the gui parts from the clone.
            XmlNode guiNode = XmlHelper.GetChildByType(newDoc.DocumentElement, "GUI");
            newDoc.DocumentElement.RemoveChild(guiNode);

            XmlNode conNode = XmlHelper.GetChildByType(newDoc.DocumentElement, "Connections");
            newDoc.DocumentElement.RemoveChild(conNode);
            XmlHelper.SetParam(newDoc.DocumentElement, "schemeState", "string", "");

            XmlElement xmlResources = XmlHelper.GetChildByType(newDoc.DocumentElement, "Resources");
            if (xmlResources != null)
                newDoc.DocumentElement.RemoveChild(xmlResources);

            return newDoc;
        }

        /// <summary>
        /// Called by the framwork to start the process station.
        /// </summary>
        public virtual void OnStart()
        {}

        /// <summary>
        ///  Called by the framework to stop the process station.
        /// </summary>
        public virtual void OnStop()
        { }

        /// <summary>
        /// For internaly use.
        /// </summary>
        public override void OnStartInputPS()
        {
            base.OnStartInputPS();
            OnStart();
        }

        /// <summary>
        /// Used internal by the framework.
        /// </summary>
        protected override void OnStartOutputPS()
        {
            base.OnStartOutputPS();
            OnStart();
        }

        /// <summary>
        /// Used internal by the framework.
        /// </summary>
        public override void OnStopInputPS()
        {
            base.OnStopInputPS();
            OnStop();
        }

        /// <summary>
        /// Used internal by the framework.
        /// </summary>
        protected override void OnStopOutputPS()
        {
            base.OnStopOutputPS();
            OnStop();
        }


        /// <summary>
        /// Called by the framework to setup the process station.
        /// </summary>
        public void Setup()
        {
            CreateSignalSources();
            OnSetupTheControls();            
        }

        public List<SortedList<uint, List<Signal>>> Sources
        {
            get { return _sources; }
        }

        private void CreateSignalSources()
        {
            XmlElement xmlInPorts = XmlHelper.GetChildByType(_xmlRep, "Mp.InputPorts");

            if (xmlInPorts != null)
            {
                CreatePortsSources(xmlInPorts);
            }
            else
            {
                XmlElement xmlOutPorts = XmlHelper.GetChildByType(_xmlRep, "Mp.OutputPorts");
                CreatePortsSources(xmlOutPorts);
            }
        }

        private void CreatePortsSources(XmlElement xmlInPorts)
        {
            int port = 0;

            foreach (XmlElement xmlPort in xmlInPorts.ChildNodes)
            {
                uint id = (uint)XmlHelper.GetParamNumber(xmlPort, "refSignalList");

                SignalList signalList = (SignalList)_signalLists[id];
                if (signalList == null)
                    continue;

                _portSignals.Add(signalList);                
                _sources.Add(new SortedList<uint, List<Signal>>());

                for (int i = 0; i < signalList.Count; ++i)
                {
                    Signal signal = (Signal)signalList[i];
                    signal.SignalIndex =  i;

                    List<Signal> list;
                    uint sourceID = ((uint)signal.PhysSourceId) << 16;
                    sourceID |= (uint)signal.SampleRate;

                    if (!_sources[port].ContainsKey(sourceID))
                    {
                        list = new List<Signal>();
                        _sources[port][sourceID] = list;
                    }
                    else
                    {
                        list = _sources[port][sourceID];
                    }

                    list.Add(signal);
                }


                _signalDataOffset.Add(new SortedList<int, List<int>>());
                _sourceSize.Add(new List<int>());

                //Calculate the signal offsets in source.(data)
                for (int srcIdx = 0; srcIdx < _sources[port].Values.Count; ++srcIdx)
                {
                    List<Signal> source = _sources[port].Values[srcIdx];

                    int sigOffset = 0;

                    List<int> signalOffsets = new List<int>();
                    _signalDataOffset[port].Add(srcIdx, signalOffsets);

                    foreach (Signal signal in source)
                    {
                        signalOffsets.Add(sigOffset);
                        sigOffset += (int)signal.DataTypeSize;
                    }

                    _sourceSize[port].Add(sigOffset);
                }
                ++port;
            }
        }

        /// <summary>
        /// Called be the framework to add a control to the process station.
        /// </summary>
        /// <param name="ctrl"></param>
        public void AddControl( Control ctrl )
        { 
            _controls.Add( ctrl ); 
        }

        
        /// <summary>
        /// Gets or sets the process station signal list.
        /// </summary>
        public Hashtable Signals
        { 
            set { _signalLists = value; } 
            get { return _signalLists; }
        }

        /// <summary>
        /// Called by the framwork to initialize the control.
        /// </summary>
        /// <remarks>
        /// Override this to setup the control runtime data.
        /// The control state can be restored from the ControlData
        /// structure from the control Tag. This structure contains a
        /// string member ControlState.
        /// </remarks>
        protected virtual void OnSetupTheControls()
        {
        }

        /// <summary>
        /// Called by the framework to save the control states.
        /// </summary>
        /// <remarks>
        /// The control Tag contains the ControlData structure which has a string member
        /// ControlState. This member should be used to save the control state.
        /// </remarks>
        public virtual void OnSaveControlsStates()
        { }

        /// <summary>
        /// Sets the process station identifier.
        /// </summary>
        public ulong ID
        { 
            set { _psid = value; }
            get { return _psid; }
        }

        /// <summary>
        /// Return true if the process station is a output process station.
        /// </summary>
        virtual public bool IsOutputPS
        {
            get { return true; }
        }

        /// <summary>
        /// Return the signal source by index.
        /// </summary>
        /// <param name="index">The source index</param>
        /// <returns>The signal source</returns>
        protected List<Signal> GetSource(int portIdx, int index)
        {
                return _sources[portIdx].Values[index];
        }

        /// <summary>
        /// Returns the source size in byte.
        /// </summary>
        /// <param name="index">The source index.</param>
        /// <returns>The source size in byte.</returns>
        protected int GetSourceSize(int portIdx, int index)
        {
            return _sourceSize[portIdx][index];
        }

        /// <summary>
        /// Gets the number of sources.
        /// </summary>
        protected int GetSourceCount(int portIdx)
        {
           return _sources[portIdx].Values.Count;
        }
        

        protected int GetSignalOffsetInTheSource(int portIdx, int signalIdxInSource, int sourceIdx)
        {
            return _signalDataOffset[portIdx].Values[sourceIdx][signalIdxInSource];
        }

        /// <summary>
        /// Extract the signal value from the given stream.
        /// </summary>
        /// <param name="data">The data stream.</param>
        /// <param name="signalIdxInSource">The index of the signal in the source.</param>
        /// <param name="sourceIdx">The source index.</param>
        /// <param name="isScaled">Flag that specifie if the signal is scaled.</param>
        /// <returns>The signal value.</returns>
        protected double ExtractValue(byte[] data, int portIdx,  int signalIdxInSource,int sourceIdx, int offset, bool scale)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryReader rs = new BinaryReader(ms);

            rs.BaseStream.Seek(offset + _signalDataOffset[portIdx].Values[sourceIdx][signalIdxInSource], 0);

            List<Signal> source = GetSource(portIdx, sourceIdx);
            Signal signal = source[signalIdxInSource];

            double value = 0.0;

            switch (signal.DataType)
            {
                case Signal.DataTypes.BOOL:
                    value = Convert.ToDouble(rs.ReadByte()); 
                break;
                case Signal.DataTypes.REAL:
                   value =  Convert.ToDouble(rs.ReadSingle());
                break;
                case Signal.DataTypes.LREAL:
                   value = rs.ReadDouble();
                   break;
                case Signal.DataTypes.SINT:
                   value = Convert.ToDouble(rs.ReadSByte());
                   break;
                case Signal.DataTypes.USINT:
                   value = Convert.ToDouble(rs.ReadByte());
                   break;
                case Signal.DataTypes.INT:
                   value = Convert.ToDouble(rs.ReadInt16());
                   break;
                case Signal.DataTypes.UINT:
                   value = Convert.ToDouble(rs.ReadUInt16());
                   break;
                case Signal.DataTypes.DINT:
                   value = Convert.ToDouble(rs.ReadInt32());
                   break;
                case Signal.DataTypes.UDINT:
                   value = Convert.ToDouble(rs.ReadUInt32());
                   break;
                case Signal.DataTypes.ULINT:
                   value = Convert.ToDouble(rs.ReadUInt64());
                   break;
                case Signal.DataTypes.LINT:
                   value = Convert.ToDouble(rs.ReadInt64());
                   break;
                case Signal.DataTypes.StringType:
                    return 0.0;

                case Signal.DataTypes.ObjectType:
                    return 0.0;
            }

            if (scale && signal.Scaling != null)
                return signal.Scaling.ScaleValue(value);

            return value;
        }

        protected byte[] ExtractObject(byte[] data, int portIdx, int signalIdxInSource, int sourceIdx, int offset, int objSize)
        {
            MemoryStream ms = new MemoryStream(data);
            BinaryReader rs = new BinaryReader(ms);

            rs.BaseStream.Seek(offset + _signalDataOffset[portIdx].Values[sourceIdx][signalIdxInSource], 0);

            List<Signal> source = GetSource(portIdx, sourceIdx);
            
            Signal signal = source[signalIdxInSource];

            if(signal.DataType != Signal.DataTypes.ObjectType)
                return null;

            return rs.ReadBytes((int)signal.DataTypeSize);
        }

        public SignalList GetPortSignals(int portIdx)
        {
            if(portIdx < _portSignals.Count)
                return _portSignals[portIdx];

            return null;
        }

        /// <summary>
        /// Gets the process station controls.
        /// </summary>
        public List<Control> Controls
        {
            get
            {
                return _controls;
            }
        }
    }
}
