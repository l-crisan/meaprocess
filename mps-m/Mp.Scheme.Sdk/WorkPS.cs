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
using System.Xml;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// This class is the base class for a work process station.
    /// </summary>
    /// <remarks>
    /// A work process station is a process station which has input ports and output ports,
    /// is not a source and not a sink process station.
    /// </remarks>
    public class WorkPS : ProcessStation
    {

        private bool _noDocumentChangedEvent = false;
        private bool _acceptObject = true;

        /// <summary>
        ///   Default constructor.
        /// </summary>
        public WorkPS()
        {
            AcceptObjectSignal = false;
        }


        public bool AcceptObjectSignal
        {
            get { return _acceptObject; }
            set { _acceptObject = value; }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            if (base.Document != null)
                base.Document.OnDocumentStateChangedEvent += new DocumentStateDelegate(Document_OnDocumentStateChangedEvent);
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();

            if (base.Document != null)
                base.Document.OnDocumentStateChangedEvent += new DocumentStateDelegate(Document_OnDocumentStateChangedEvent);
        }

        private void Document_OnDocumentStateChangedEvent(bool modified)
        {
            if (_noDocumentChangedEvent || !modified)
                return;

            OnDocumentChanged();
        }

        /// <summary>
        /// Called by the framework to update the signals.
        /// </summary>
        protected virtual void OnDocumentChanged()
        {
        }

        public override void OnRemove()
        {
            _noDocumentChangedEvent = true;
            base.OnRemove();
        }

        public override void OnRestore()
        {
            base.OnRestore();
            _noDocumentChangedEvent = false;
        }

        /// <summary>
        /// This method check if a signal is into the input ports available
        /// </summary>
        /// <param name="sigId">The signal id to check.</param>
        /// <param name="xmlSignalToExclude">The signal to exclude from check</param>
        /// <returns>true If is available.</returns>
        protected bool IsSignalInPortsAvailable(uint sigId,bool inputPort, XmlElement xmlSignalToExclude)
        {
            List<Port> ports = OutputPorts;

            if (inputPort)
                ports = InputPorts;

            foreach (Port port in ports)
            {
                if (IsSignalInPortAvailable(port, sigId, xmlSignalToExclude))
                    return true;
            }

            return false;
        }


        protected bool IsSignalInPortAvailable(Port port, uint sigId, XmlElement xmlSignalToExclude)
        {
            if (port.SignalList == null)
                return false;

            foreach (XmlElement xmlElement in port.SignalList.ChildNodes)
            {
                XmlElement xmlSignal = Document.GetSignal(xmlElement);
                
                if( xmlSignal == null)
                    continue;

                uint id = XmlHelper.GetObjectID(xmlSignal);

                if ((id == sigId) && (xmlSignalToExclude != xmlSignal))
                    return true;
            }

            return false;
        }


        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);

            //Check for connected input ports.
            for (int i = 0; i < InputPorts.Count; ++i)
            {
                Port inPort = InputPorts[i];
                Int32 portNo = i + 1;

                if (!inPort.Connected)
                {
                    string msg = String.Format(StringResource.InPortINotCon, portNo.ToString(), this.Text); 
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }
            }

            //Check for signal in the output ports.
            for (int i = 0; i < OutputPorts.Count; ++i)
            {
                Port outPort = OutputPorts[i];
                Int32 portNo = i + 1;

                if (!outPort.Connected)
                {
                    string msg = String.Format(StringResource.OutPortINotCon, portNo.ToString(), this.Text);
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }

                if (outPort.SignalList == null)
                {
                    string msg = string.Format(StringResource.OutPortINoSigErr,portNo.ToString(), this.Text);

                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }
                else if (outPort.SignalList.InnerText == "")
                {
                    string msg = string.Format(StringResource.OutPortINoSigErr, portNo.ToString(), this.Text);
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }
            }

            //Check duplicated signals in input ports.
            Hashtable multipleIds = new Hashtable();

            foreach (Port inPort in InputPorts)
            {
                if (inPort.SignalList == null)
                    continue;

                foreach (XmlElement xmlSig in inPort.SignalList.ChildNodes)
                {
                    XmlElement xmlSignal = Document.GetSignal(xmlSig);
                    uint id = XmlHelper.GetObjectID(xmlSignal);

                    if (!IsSignalInPortsAvailable(id, true, xmlSignal) || multipleIds.ContainsKey(id))
                        continue;

                    XmlElement signal = Document.GetXmlObjectById(id);

                    string msg = String.Format(StringResource.SigDupInInput, XmlHelper.GetParam(signal, "name"), base.Text);                    

                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    multipleIds.Add(id, true);
                }
            }
            
            
            if(InputPorts.Count == 0)
                return;

            
            if (!_acceptObject && InputPorts[0].SignalList != null)
            {
                //Check signal type
                foreach (XmlElement xmlSigOrRef in InputPorts[0].SignalList)
                {
                    XmlElement xmlSignal = Document.GetSignal(xmlSigOrRef);

                    SignalDataType sigDataType = (SignalDataType)(int)XmlHelper.GetParamNumber(xmlSignal, "valueDataType");

                    if (sigDataType == SignalDataType.OBJECT)
                    {
                        string msg = String.Format(StringResource.SigObjetErr, this.Text);
                        valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    }
                }
            }
        }
    }
}
