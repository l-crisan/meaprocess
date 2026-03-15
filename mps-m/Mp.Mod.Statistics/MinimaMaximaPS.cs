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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;
using System.Collections;

namespace Mp.Mod.Statistics
{
    internal class MinimaMaximaPS : WorkPS
    {
        public MinimaMaximaPS()
        {
            base.Type = "Mp.Stat.PS.MinMax";
            base.Text = StringResource.MinimaMaxima;
            base.Group = StringResource.Statistics;
            base.Symbol = Mp.Mod.Statistics.Resource.MinimaMaximaImg;
            base.Icon = Mp.Mod.Statistics.Resource.MinimaMaximaIcon;
            base.IsSingleton = false;
        }

        public override string RuntimeModule
        {
            get { return "mps-statistics"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.MinimaMaxima;
            base.Group = StringResource.Statistics;
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data out port.
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);
            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);
            AddPort(port);

            //Data port in port. 
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        public override string Description
        {
            get
            {
                return StringResource.MinimaMaximaDescription;
            }
        }

        private void InitMenuForPort(Port port)
        {
            //Create the context menu.
            port.ContextMenuStrip = new ContextMenuStrip();
            port.ContextMenuStrip.Tag = port;

            ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.MenuProperties);

            menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            menuItem.Click += new System.EventHandler(this.OnPropertyDataPort);
            port.ContextMenuStrip.Items.Add(menuItem);
        }

        protected override void OnDocumentChanged()
        {
            if (InputPorts.Count == 0)
                return;

            XmlElement xmlOutSignalList = OutputPorts[0].SignalList;

            if (xmlOutSignalList == null)
                return;

            Port port = InputPorts[0];
            //Update the signals ids
            for (int i = 0; i < xmlOutSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlOutSignal = (XmlElement)xmlOutSignalList.ChildNodes[i];


                uint sigId = (uint)XmlHelper.GetParamNumber(xmlOutSignal, "inSignal");

                XmlElement xmlSig = Document.GetXmlObjectById(sigId);

                if (xmlSig == null || port.SignalList == null)
                {
                    Document.RemoveXmlObject(xmlOutSignal);
                    i--;
                    continue;
                }

                bool found = false;
                foreach (XmlElement xmlPortSig in port.SignalList.ChildNodes)
                {
                    XmlElement xmlSignal = xmlPortSig;

                    if (XmlHelper.GetObjectID(xmlPortSig) == 0)
                        xmlSignal = Document.GetXmlObjectById(Convert.ToUInt32(xmlPortSig.InnerText));

                    if (xmlSignal == xmlSig)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Document.RemoveXmlObject(xmlOutSignal);
                    i--;
                    continue;
                }
                else
                {
                    //Update the output signal
                    double sampleRate = XmlHelper.GetParamDouble(xmlSig, "samplerate");
                    XmlHelper.SetParamDouble(xmlOutSignal, "samplerate", "double", sampleRate);

                    double min = XmlHelper.GetParamDouble(xmlSig, "physMin");
                    XmlHelper.SetParamDouble(xmlOutSignal, "physMin", "double", min);

                    double max = XmlHelper.GetParamDouble(xmlSig, "physMax");
                    XmlHelper.SetParamDouble(xmlOutSignal, "physMax", "double", max);

                }
            }
        }

        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);

            if (port.IsInput)
                return;

            OnPropertyDataPort(null, null);
        }

        protected void OnPropertyDataPort(object sender, EventArgs e)
        {
            SignalViewDlg dlg = new SignalViewDlg(OutputPorts[0].SignalList, Document);
            dlg.ShowDialog();
        }


        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1550);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            MinimaMaximaDlg dlg = new MinimaMaximaDlg(XmlRep, Document, InputPorts[0].SignalList, OutputPorts[0].SignalList);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            //Check for connected input ports.
            Port inPort = InputPorts[0];
            Int32 portNo = 1;

            if (!inPort.Connected)
            {
                string msg = String.Format(StringResource.InPortINotCon, portNo.ToString(), this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            //Check for signal in the output ports.
            Port outPort = OutputPorts[0];
            portNo = 1;

            if (!outPort.Connected)
            {
                string msg = String.Format(StringResource.OutPortINotCon, portNo.ToString(), this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            if (outPort.SignalList == null)
            {
                string msg = string.Format(StringResource.OutPortINoSigErr, portNo.ToString(), this.Text);

                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
            else if (outPort.SignalList.InnerText == "")
            {
                string msg = string.Format(StringResource.OutPortINoSigErr, portNo.ToString(), this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }


            //Check duplicated signals in output ports.
            Hashtable multipleIds = new Hashtable();

            foreach (Port port in InputPorts)
            {
                if (port.SignalList == null)
                    continue;

                foreach (XmlElement xmlSignal in port.SignalList.ChildNodes)
                {
                    uint id = XmlHelper.GetObjectID(xmlSignal);

                    if (id == 0)
                        id = Convert.ToUInt32(xmlSignal.InnerText);

                    if (!IsSignalInPortsAvailable(id, true, xmlSignal) || multipleIds.ContainsKey(id))
                        continue;

                    XmlElement signal = Document.GetXmlObjectById(id);

                    string msg = String.Format(StringResource.SigDupInInput, XmlHelper.GetParam(signal, "name"), base.Text);

                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    multipleIds.Add(id, true);
                }
            }
        }
    }
}
