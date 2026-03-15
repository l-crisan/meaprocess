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
using System.Drawing;
using System.Windows.Forms;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Streaming
{
    internal class InputPS : ProcessStation
    {
        public InputPS()
        {
            base.Type = "Mp.Streaming.PS.In";
            base.Text = StringResource.NetInput;
            base.Group = StringResource.Net;
            base.Symbol = Mp.Mod.Streaming.Resource.Net;
            base.Icon = Mp.Mod.Streaming.Resource.NetIcon;
            base.IsSingleton = true;
        }

        public override string RuntimeModule
        {
            get
            {
                return "mps-streaming";
            }
        } 

        public override void OnLoadResources()
        {
            base.Text = StringResource.NetInput;
            base.Group = StringResource.Net;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data out port.
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);
            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);
            AddPort(port);
            InitContextMenu();
        }

        public override string Description
        {
            get
            {
                return StringResource.NetInPsDescription;
            }
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
            InitContextMenu();
        }

        private void InitContextMenu()
        {
            //Create the context menu.
            this.ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.MenuProperties);

            menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            menuItem.Click += new System.EventHandler(this.OnProperties);
            this.ContextMenuStrip.Items.Add(menuItem);

            ToolStripItem item = new ToolStripSeparator();
            this.ContextMenuStrip.Items.Add(item);

            menuItem = new ToolStripMenuItem(StringResource.MenuInportSignals);
            menuItem.Click += new System.EventHandler(this.OnInportSignalList);
            this.ContextMenuStrip.Items.Add(menuItem);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            StreamingPSDlg dlg = new StreamingPSDlg(Document, XmlRep, false);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
        }

        protected void OnInportSignalList(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "*.mpsl|*.mpsl";

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(dlg.FileName);
            Port port = OutputPorts[0];

            for (int i = 0; i < port.SignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement) port.SignalList.ChildNodes[i];

                Document.UnregisterSource((uint)XmlHelper.GetParamNumber(xmlSignal, "sourceNumber"));
                Document.RemoveXmlObject(xmlSignal);
                --i;
            }

            Hashtable sources = new Hashtable();

            foreach (XmlElement xmlSignal in xmlDoc.DocumentElement.ChildNodes)
            {
                if (XmlHelper.GetObjectID(xmlSignal) == 0)
                    continue;

                XmlElement newXmlSignal = Document.CreateXmlObject(port.SignalList, "Mp.Sig", "Mp.Streaming.Sig");
                Document.CopySignalParam(xmlSignal, newXmlSignal);
                
                uint sourceID = (uint) XmlHelper.GetParamNumber(xmlSignal, "sourceNumber");
                uint newSourceID = 0;

                if (!sources.ContainsKey(sourceID))
                {                    
                    newSourceID = Document.RegisterSource(XmlHelper.GetParam(this.XmlRep,"name"), sourceID, "STREAM_IN_" + sourceID.ToString());
                    sources.Add(sourceID, newSourceID);
                }
                else
                {
                    newSourceID = (uint) sources[sourceID];
                }

                XmlHelper.SetParamNumber(newXmlSignal, "sourceNumber", "uint32_t", (long)newSourceID);
                XmlHelper.SetParamNumber(newXmlSignal, "orgSourceNumber", "uint32_t", (long)sourceID);

                XmlElement xmlScaling = XmlHelper.GetChildByType(xmlSignal, "Mp.Scaling");

                if (xmlScaling != null)
                {
                    XmlElement newXmlScaling = Document.CreateXmlObject(newXmlSignal, "Mp.Scaling", "Mp.Scaling.FactorOffset");

                    XmlHelper.SetParamDouble(newXmlScaling, "factor", "double", XmlHelper.GetParamDouble(xmlScaling, "factor"));
                    XmlHelper.SetParamDouble(newXmlScaling, "offset", "double", XmlHelper.GetParamDouble(xmlScaling, "offset"));
                }               
            }

            Document.Modified = true;
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

            if (port.IsInput)
                return;

            OnPropertyDataPort(null, null);
        }

        protected void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            SignalViewDlg dlg = new SignalViewDlg(port.SignalList, Document);
            dlg.ShowDialog();
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1630);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            Port outPort = OutputPorts[0];

            if (!outPort.Connected)
            {
                string msg = String.Format(StringResource.OutPortNotCon, base.Text);

                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            if (outPort.SignalList.ChildNodes.Count == 0)
            {
                string msg = String.Format(StringResource.OutPortSigErr, base.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            string ip = XmlHelper.GetParam(XmlRep, "connection");
            if (ip == "")
            {
                string msg = String.Format(StringResource.InputIPErr, base.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

        }
    }
}
