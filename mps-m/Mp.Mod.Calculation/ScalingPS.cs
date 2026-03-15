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
    internal class ScalingPS : WorkPS
    {
        public ScalingPS()
        {
            base.Type = "Mp.Calculation.PS.Scaling";
            base.Text = StringResource.Scaling;
            base.Group = StringResource.Calculation;
            base.Symbol = Resource.ScalingImg;
            base.Icon = Resource.ScalingIcon;
            base.IsSingleton = false;
        }

        public override string RuntimeModule
        {
            get { return "mps-calculation"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.Scaling;
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
                return StringResource.ScalingDescription;
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
            XmlElement xmlScalings = XmlHelper.GetChildByType(XmlRep, "Mp.Calculation.Scalings");

            if (xmlScalings == null)
                return;

            if (InputPorts.Count == 0)
            {
                Document.RemoveXmlObject(xmlScalings);
                return;
            }                

            Port port = InputPorts[0];

            //Update the signals ids
            for (int i = 0; i < xmlScalings.ChildNodes.Count; ++i)
            {
                XmlElement xmlScaling = (XmlElement)xmlScalings.ChildNodes[i];

                if (!xmlScaling.HasAttribute("type"))
                    continue;

                if (xmlScaling.Attributes["type"].Value != "Mp.Calculation.Scaling")
                    continue;

                uint sigId = (uint)XmlHelper.GetParamNumber(xmlScaling, "signal");

                XmlElement xmlSig = Document.GetXmlObjectById(sigId);

                if (xmlSig == null || port.SignalList == null)
                {
                    uint id = (uint)XmlHelper.GetParamNumber(xmlScaling, "outSignal");
                    Document.RemoveXmlObject(id);
                    xmlScalings.RemoveChild(xmlScaling);
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
                    uint id = (uint)XmlHelper.GetParamNumber(xmlScaling, "outSignal");
                    Document.RemoveXmlObject(id);
                    xmlScalings.RemoveChild(xmlScaling);
                    i--;
                    continue;
                }
                else
                {
                    uint id = (uint)XmlHelper.GetParamNumber(xmlScaling, "outSignal");
                    XmlElement xmlScaledSignal = Document.GetXmlObjectById(id);

                    //Set the output signal sample rate.
                    double sampleRate = XmlHelper.GetParamDouble(xmlSig, "samplerate");
                    XmlHelper.SetParamDouble(xmlScaledSignal, "samplerate", "double", sampleRate);

                    //Calculate the output signal min max.
                    double min = XmlHelper.GetParamDouble(xmlSig, "physMin");
                    double max = XmlHelper.GetParamDouble(xmlSig, "physMax");


                    int scalingType = (int)XmlHelper.GetParamNumber(xmlScaling, "type");
                    switch (scalingType)
                    {
                        case 0:
                        {
                            double factor = XmlHelper.GetParamDouble(xmlScaling, "factor");
                            double offset = XmlHelper.GetParamDouble(xmlScaling, "offset");
                            min = min * factor + offset;
                            max = max * factor + offset;
                        }
                        break;
                        case 1:
                        {
                            PointF p1 = new PointF();
                            PointF p2 = new PointF();

                            p1.X = (float) XmlHelper.GetParamDouble(xmlScaling, "p1x");
                            p1.Y = (float) XmlHelper.GetParamDouble(xmlScaling, "p1y");
                            p2.X = (float) XmlHelper.GetParamDouble(xmlScaling, "p2x");
                            p2.Y = (float) XmlHelper.GetParamDouble(xmlScaling, "p2y");

                            double factor = GetFactor(p1,p2);
                            double offset = GetOffset(p1,p2);
                            min = min * factor + offset;
                            max = max * factor + offset;
                        }
                        break;

                        case 2:
                        {
                            //Load the point table
                            List<PointF> table = new List<PointF>();
                            string tableStr = XmlHelper.GetParam(xmlScaling, "table");

                            if (tableStr == "")
                                break;

                            string[] pairs = tableStr.Split(';');

                            foreach (string pair in pairs)
                            {
                                string[] points = pair.Split(',');
                                PointF point = new PointF();
                                point.X = Convert.ToSingle(points[0]);
                                point.Y = Convert.ToSingle(points[1]);
                                table.Add(point);
                            }
                           
                            //Calculate the output signal min max.
                            for (int j = 1; j < table.Count; ++j)
                            {
                                PointF p1 = table[j - 1];
                                PointF p2 = table[j];

                                if (j == table.Count - 1)
                                {
                                    if (min >= p1.X && min <= p2.X)
                                        min = min * GetFactor(p1, p2) + GetOffset(p1, p2);

                                    if (max >= p1.X && max <= p2.X)
                                        max = max * GetFactor(p1, p2) + GetOffset(p1, p2);
                                }
                                else
                                {
                                    if (min >= p1.X && min < p2.X)
                                        min = min * GetFactor(p1, p2) + GetOffset(p1, p2);

                                    if (max >= p1.X && max < p2.X)
                                        max = max * GetFactor(p1, p2) + GetOffset(p1, p2);
                                }
                            }

                            min = Math.Min(min,XmlHelper.GetParamDouble(xmlSig, "physMin"));
                            max = Math.Max(max,XmlHelper.GetParamDouble(xmlSig, "physMax"));
                        }
                        break;
                    }

                    XmlHelper.SetParamDouble(xmlScaledSignal, "physMin", "double", min);
                    XmlHelper.SetParamDouble(xmlScaledSignal, "physMax", "double", max);
                }
            }
        }

        
        private double GetFactor(PointF p1, PointF p2)
        {
	        return ((double)p2.Y - (double)p1.Y) /(p2.X - p1.X);
        }

        private double GetOffset(PointF p1, PointF p2)
        {
	        return ((double)(p1.Y * p2.X) - (double)(p2.Y* p1.X)) /(p2.X - p1.X);
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
            Document.ShowHelp(this.Site, 1400);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            ScalingPSDlg dlg = new ScalingPSDlg(XmlRep, Document, InputPorts[0].SignalList, OutputPorts[0].SignalList);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
        }
    }
}
