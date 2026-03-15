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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    internal class SampleRatePS : WorkPS
    {
        public SampleRatePS()
        {
            base.Type = "Mp.Calculation.PS.SampleRate";
            base.Text = StringResource.SampleRate;
            base.Group = StringResource.Calculation;
            base.Symbol = Resource.SampleRateImg;
            base.Icon = Resource.SampleRateIcon;
            base.IsSingleton = false;
        }

        public override string RuntimeModule
        {
            get { return "mps-calculation"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.SampleRate;
            base.Group = StringResource.Calculation;
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
                return StringResource.SampleRateDescription;
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
                    xmlOutSignalList.RemoveChild(xmlOutSignal);
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
                    xmlOutSignalList.RemoveChild(xmlOutSignal);
                    i--;
                    continue;
                }
                else
                {
                    //Update the output signal
                    double min = XmlHelper.GetParamDouble(xmlSig, "physMin");
                    double max = XmlHelper.GetParamDouble(xmlSig, "physMax");
                    XmlHelper.SetParamDouble(xmlOutSignal, "physMin", "double", min);
                    XmlHelper.SetParamDouble(xmlOutSignal, "physMax", "double", max);
                    XmlHelper.SetParam(xmlOutSignal, "parameters", "string", XmlHelper.GetParam(xmlSig, "parameters"));
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
            Port port = OutputPorts[0];
            SignalViewDlg dlg = new SignalViewDlg(port.SignalList, Document);
            dlg.ShowDialog();
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1440);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            SampleRateDlg dlg = new SampleRateDlg(XmlRep, Document, InputPorts[0].SignalList, OutputPorts[0].SignalList);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");             
        }
    }
}
