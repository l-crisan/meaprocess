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
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Mp.Utils;
using Mp.Scheme.Sdk;


namespace Mp.Mod.Streaming
{
    internal class OutputPS : ProcessStation
    {
        public OutputPS()
        {
            base.Type = "Mp.Streaming.PS.Out";
            base.Text = StringResource.NetOutput;
            base.Group = StringResource.Net;
            base.Symbol = Resource.Net;
            base.Icon = Resource.NetIcon;
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1640);
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
            base.Text = StringResource.NetOutput;
            base.Group = StringResource.Net;
        }

        public override string Description
        {
            get
            {
                return StringResource.NetOutPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data in port.
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
            InitContextMenu();
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
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

            menuItem = new ToolStripMenuItem(StringResource.MenuExportSignals);
            menuItem.Click += new System.EventHandler(this.OnExportSignalList);
            this.ContextMenuStrip.Items.Add(menuItem);
        }


        protected void OnExportSignalList(object sender, EventArgs e)
        {
            Port port = InputPorts[0];

            if (port.SignalList == null)
            {
                MessageBox.Show(StringResource.SigListErr, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "*.mpsl|*.mpsl";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;


            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n");
            sb.Append("<object type=\"SignalList\" id=\"24352\">\n");

            for (int i = 0; i < port.SignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement)port.SignalList.ChildNodes[i];
                uint id = XmlHelper.GetObjectID(xmlElement);
                if (id == 0)
                {
                    id = Convert.ToUInt32(xmlElement.InnerText);
                    string xmlSig = Document.GetXmlObjectById(id).OuterXml;
                    sb.Append(xmlSig);
                }
                else
                {
                    sb.Append(xmlElement.OuterXml);
                }
            }
            sb.Append("</object>\n");
            StreamWriter sw = new StreamWriter(dlg.FileName);
            sw.Write(sb.ToString());
            sw.Flush();
            sw.Close();
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            StreamingPSDlg dlg = new StreamingPSDlg(Document, XmlRep, false);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);

            string connectionName = XmlHelper.GetParam(XmlRep, "connection");
            if (connectionName == "")
            {
                string msg = String.Format(StringResource.InputIPErr, base.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }
    }
}
