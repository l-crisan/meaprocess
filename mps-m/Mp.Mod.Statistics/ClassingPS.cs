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
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Statistics
{
    internal class ClassingPS : WorkPS
    {
         public ClassingPS()
        {
            base.Type = "Mp.Stat.PS.Class";
            base.Text = StringResource.Classing;
            base.Group = StringResource.Statistics;
            base.Symbol = Resource.ClassingImg;
            base.Icon = Resource.Classing; 
        }

        public override string RuntimeModule
        {
            get
            {
                return "mps-statistics";
            }
        } 

        public override void OnLoadResources()
        {
            base.Text = StringResource.Classing;
            base.Group = StringResource.Statistics;
        }


        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1170);
        }

        public override string Description
        {
            get
            {
                return StringResource.ClassingPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data in port.
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

            //Create the data out port.
            port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);
            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);
            AddPort(port);

            //Trigger port.
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset + DistanceBetweenPort)), "Mp.Port.Trigger", true, false);
            port.ConnectorBrush = new SolidBrush(Color.DarkGreen);
            AddPort(port);
            InitTriggerPortMenu();

            //Reset port.
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset + 2*DistanceBetweenPort)), "Mp.Port.In", true, false);
            port.ConnectorBrush = new SolidBrush(Color.DarkGreen);
            AddPort(port);
        }

        private void InitTriggerPortMenu()
        {
            foreach (Port port in InputPorts)
            {
                if (port.Type != "Mp.Port.Trigger")
                    continue;

                //Create the context menu.
                port.ContextMenuStrip = new ContextMenuStrip();
                port.ContextMenuStrip.Tag = port;

                ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.MenuProperties);

                menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                menuItem.Click += new System.EventHandler(this.OnTriggerPortProperties);
                port.ContextMenuStrip.Items.Add(menuItem);
                return;
            }
        }


        protected void OnTriggerPortProperties(object sender, EventArgs e)
        {
            TriggerPortDlg dlg = new TriggerPortDlg(Document, InputPorts[1].XmlRep);
            dlg.ShowDialog();
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

        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);

            if (port == InputPorts[1])
                OnTriggerPortProperties(null, null);

            if (!port.IsInput)
                OnPropertyDataPort(null, null);   
        }

        protected void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            SignalViewDlg dlg = new SignalViewDlg(port.SignalList, Document);
            dlg.ShowDialog();
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            OnMouseDoubleClick(new Point());
        }

        public override void OnMouseDoubleClick(Point p)
        {
            ClassingPSDlg dlg = new ClassingPSDlg(XmlRep, Document, InputPorts[0].SignalList, OutputPorts[0].SignalList);
            
            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
        }

        protected override void OnDocumentChanged()
        {

            XmlElement xmlClassings = XmlHelper.GetChildByType(XmlRep, "Mp.Stat.Classings");

            if (xmlClassings == null)
                return;

            if (InputPorts.Count == 0)
            {
                Document.RemoveXmlObject(xmlClassings);
                return;
            }

            Port port = InputPorts[0];

            //Update the signals ids
            for (int i = 0; i < xmlClassings.ChildNodes.Count; ++i)
            {
                XmlElement xmlClassing = (XmlElement)xmlClassings.ChildNodes[i];

                if (!xmlClassing.HasAttribute("type"))
                    continue;

                if (xmlClassing.Attributes["type"].Value != "Mp.Stat.Classing")
                    continue;

                uint sigId = (uint)XmlHelper.GetParamNumber(xmlClassing, "signal");

                XmlElement xmlSig = Document.GetXmlObjectById(sigId);

                if (xmlSig == null || port.SignalList == null)
                {
                    uint id = (uint) XmlHelper.GetParamNumber(xmlClassing, "outSignal");
                    Document.RemoveXmlObject(id);
                    xmlClassings.RemoveChild(xmlClassing);
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
                    uint id = (uint)XmlHelper.GetParamNumber(xmlClassing, "outSignal");
                    Document.RemoveXmlObject(id);
                    xmlClassings.RemoveChild(xmlClassing);
                    i--;
                    continue;
                }
            }
        }
        
        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitTriggerPortMenu();
            InitMenuForPort(OutputPorts[0]);
        }

        private void AddASignalIsNeededMsg(List<ValidationInfo> valInfoList)
        {
            string msg = String.Format(StringResource.TriggerPortSigError, this.Text);
            valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
        }

        private void Add2SignalAreNeededMsg(List<ValidationInfo> valInfoList)
        {
            string msg = String.Format(StringResource.TriggerPortNeed2SigErr, this.Text);
            valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            //Check if any signal is mapped.
            XmlElement xmlDevices = XmlHelper.GetChildByType(XmlRep, "Mp.Stat.Classings");
            if (xmlDevices == null)
            {
                string msg = String.Format(StringResource.ClassingOutError, base.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                return;
            }

            if (xmlDevices.ChildNodes.Count == 0)
            {
                string msg = String.Format(StringResource.ClassingOutError, base.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }


            Port port = InputPorts[0];
            Port trigerPort = InputPorts[1];

            //Check connection
            if (!port.Connected)
            {
                string msg = String.Format(StringResource.DataInPortNotConnectedErr, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            //Check trigger
            long type = XmlHelper.GetParamNumber(trigerPort.XmlRep, "triggerType");
            switch (type)
            {
                case 0: //	NoTrigger = 0,
                    break;
                case 1: //StartTrigger,                
                case 2: //StopTrigger,                
                case 4: //EventTrigger
                    if (trigerPort.SignalList == null)
                        AddASignalIsNeededMsg(valInfoList);
                    else if (trigerPort.SignalList.InnerText == "")
                        AddASignalIsNeededMsg(valInfoList);

                    break;
                case 3://StartStopTrigger,
                    long startStopType = XmlHelper.GetParamNumber(trigerPort.XmlRep, "oneStartStopSignal");

                    if (startStopType == 1)
                    {
                        if (trigerPort.SignalList == null)
                            AddASignalIsNeededMsg(valInfoList);
                        else if (trigerPort.SignalList.InnerText == "")
                            AddASignalIsNeededMsg(valInfoList);
                    }
                    else
                    {
                        if (trigerPort.SignalList == null)
                            Add2SignalAreNeededMsg(valInfoList);
                        else if (trigerPort.SignalList.ChildNodes.Count < 2)
                            Add2SignalAreNeededMsg(valInfoList);
                    }
                    break;
            }
        }
    }
}
