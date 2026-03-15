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


namespace Mp.Mod.Controller
{
    internal class PIDControllerPS : WorkPS
    {
        public PIDControllerPS()
        {
            base.Type = "Mp.Controller.PS.PIDCtrl";
            base.Text = StringResource.PIDController;
            base.Group = StringResource.AutomaticControl;
            base.Symbol =Resource.PIDControllerImg;
            base.Icon =Resource.PIDController;
            base.IsSingleton = false;
        }

        public override string RuntimeModule
        {
            get { return "mps-controller"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.PIDController;
            base.Group = StringResource.AutomaticControl;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data out port.
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);
            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);
            AddPort(port);

            //Data in port. 
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

            //Set point port. 
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset + DistanceBetweenPort)), "Mp.Port.In", true, false);
            port.ConnectorBrush = new SolidBrush(Color.DarkGreen);
            AddPort(port);

        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            if (!InputPorts[0].Connected)
            {
                string msg = String.Format(StringResource.InPortINotCon, "1", this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
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
                    string msg = string.Format(StringResource.OutPortINoSigErr, portNo.ToString(), this.Text);

                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }
                else if (outPort.SignalList.InnerText == "")
                {
                    string msg = string.Format(StringResource.OutPortINoSigErr, portNo.ToString(), this.Text);
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }
            }
        }

        public override string Description
        {
            get
            {
                return StringResource.PIDControllerDescrp;
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

            Port inPort = InputPorts[0];

            if (inPort.SignalList == null)
                return;

            if (inPort.SignalList.ChildNodes.Count == 0)
                return;

            XmlElement xmlInSignal = (XmlElement)inPort.SignalList.ChildNodes[0];

            uint id = XmlHelper.GetObjectID(xmlInSignal);

            if (id == 0)
                xmlInSignal = Document.GetXmlObjectById(Convert.ToUInt32(xmlInSignal.InnerText));

            double rate = XmlHelper.GetParamDouble(xmlInSignal, "samplerate");

            XmlElement xmlSignalList = OutputPorts[0].SignalList;

            foreach (XmlElement xmlSignal in xmlSignalList.ChildNodes)
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", rate);
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
            PIDPortDlg dlg = new PIDPortDlg(Document, XmlRep, port.SignalList);
            dlg.ShowDialog();
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1590);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            PIDControllerDlg dlg = new PIDControllerDlg(Document, XmlRep, OutputPorts[0].SignalList);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
        }
    }
}
