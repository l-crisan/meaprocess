//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2010-2016  Laurentiu-Gheorghe Crisan
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

namespace Mp.Mod.Calculation
{
    internal class ElectricityPS : WorkPS
    {
        public ElectricityPS()
        {
            base.Type = "Mp.Calculation.PS.Elec";
            base.Text = StringResource.Electricity;
            base.Group = StringResource.Calculation;
            base.Symbol = Resource.ElectricityImg;
            base.Icon = Resource.ElectricityIcon;
            base.IsSingleton = false;
        }

        public override string RuntimeModule
        {
            get { return "mps-calculation"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.Electricity;
            base.Group = StringResource.Calculation;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data out port.
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);
            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);
            AddPort(port);

            //Data in port. 
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        public override string Description
        {
            get
            {
                return StringResource.ElectricityDescription;
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
            uint iSigId = (uint)XmlHelper.GetParamNumber(XmlRep, "currentSignal");

            if (iSigId != 0)
            {
                if (!IsSignalInPortsAvailable(iSigId, true, null))
                    XmlHelper.SetParamNumber(XmlRep, "currentSignal", "uint32_t", 0);
                else
                    UpdateSampleRate(iSigId);
            }

            uint vSigId = (uint)XmlHelper.GetParamNumber(XmlRep, "voltageSignal");

            if( vSigId != 0)
            {
                if (!IsSignalInPortsAvailable(vSigId, true, null))
                    XmlHelper.SetParamNumber(XmlRep, "voltageSignal", "uint32_t", 0);
                else
                    UpdateSampleRate(vSigId);            
            }
        }

        private void UpdateSampleRate(uint sigId)
        {
            Port outPort = OutputPorts[0];
            
            if (outPort.SignalList == null)
                return;

            if (outPort.SignalList.ChildNodes.Count == 0)
                return;
            
            XmlElement xmlSignal = Document.GetXmlObjectById(sigId);
            
            if (xmlSignal == null)
                return;

            double rate = XmlHelper.GetParamDouble(xmlSignal, "samplerate");

            foreach (XmlElement xmlOutSignal in outPort.SignalList.ChildNodes)
                XmlHelper.SetParamDouble(xmlOutSignal, "samplerate", "double", rate);            
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
            Port port = OutputPorts[0];

            ElectricityPortDlg dlg = new ElectricityPortDlg(Document, XmlRep, port.SignalList);
            dlg.ShowDialog();
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1430);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);

            uint currentSig = (uint)XmlHelper.GetParamNumber(XmlRep, "currentSignal");
            uint voltageSig = (uint)XmlHelper.GetParamNumber(XmlRep, "voltageSignal");

            if (currentSig == 0 && voltageSig == 0)
            {
                string msg = String.Format(StringResource.SigMappedErr, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            ElectricityDlg dlg = new ElectricityDlg(XmlRep, Document, InputPorts[0].SignalList);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
            
        }
    }
}
