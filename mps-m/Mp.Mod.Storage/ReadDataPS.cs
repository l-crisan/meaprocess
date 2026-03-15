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
using System.Xml;
using System.Windows.Forms;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Storage
{
    /// <summary>
    /// Implements a data storage Process-Station.
    /// </summary>
    public class ReadDataPS : ProcessStation
    {
        public ReadDataPS()
        {
            base.Type = "Mp.Storage.PS.ReadData";
            base.Text = StringResource.ReadData;
            base.Group = StringResource.Storage;
            base.Symbol = Images.DataStorage;
            base.Icon = Images.DataStorageIcon;
        }

        public override string RuntimeModule
        {
            get { return "mps-storage"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.ReadData;
            base.Group = StringResource.Storage;
        }

        public override string Description
        {
            get
            {
                return StringResource.ReadDataPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            XmlHelper.SetParamNumber(this.XmlRep, "loopBack", "uint8_t", 1);

            //Create the data out port.
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);
            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port, new EventHandler(OnDataPort));
            AddPort(port);

            //Create the EOF signal port
            port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset + PortTopOffset)), "Mp.Port.Out", false, true);
            port.ConnectorBrush = new SolidBrush(Color.Black);
            InitMenuForPort(port, new EventHandler(OnEOFPort));
            port.SignalList = Document.CreateSignalList();

            XmlElement xmlSignal = this.Document.CreateXmlObject(port.SignalList,"Mp.Sig","Mp.Sig");
            XmlHelper.SetParam(xmlSignal, "name", "string", "EOF");
            XmlHelper.SetParam(xmlSignal, "unit", "string", "BOOL");
            XmlHelper.SetParam(xmlSignal, "comment", "string", "End of file signal");
            XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
            XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 1);
            XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int) SignalDataType.BOOL);
            XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", 0);
            XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", 100);
            AddPort(port);            
        }

        private void InitMenuForPort(Port port, System.EventHandler handler)
        {
            //Create the context menu.
            port.ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.MenuProperties);

            menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            menuItem.Click += handler;
            port.ContextMenuStrip.Items.Add(menuItem);
        }

        private void OnDataPort(object sender, EventArgs e)
        {
            Port port = (Port)OutputPorts[0];

            SignalViewDlg dlg = new SignalViewDlg(port.SignalList, Document);
            dlg.ShowDialog();
        }

        private void OnEOFPort(object sender, EventArgs e)
        {
            Port port = (Port) OutputPorts[1];

            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document, SignalDataType.BOOL);
            dlg.ShowDialog();
        }

        public override void OnPortDoubleClick(Port port)
        {
            if (port == OutputPorts[1])
            {
                OnEOFPort(null, null);
            }
            else if (port == OutputPorts[0])
            {
                OnDataPort(null, null);
            }
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            ReadDataDlg dlg = new ReadDataDlg(XmlRep, OutputPorts[0].SignalList, Document);
            
            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(this.XmlRep, "name");
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {            
            Port dataOutPort = OutputPorts[0];

            if (!dataOutPort.Connected)
            {
                string msg = string.Format(StringResource.DataOutPortConError, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            if (dataOutPort.SignalList.InnerText == "")
            {
                string msg = string.Format(StringResource.DataOutPortSigError, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            string rfile = XmlHelper.GetParam(XmlRep, "rfile");

            if (rfile == "" || rfile == null)
            {
                string msg = String.Format(StringResource.DataFileForPsExpected, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 610);
        }
    }
}