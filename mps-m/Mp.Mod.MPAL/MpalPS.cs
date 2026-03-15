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
using System.Windows.Forms;
using System.Xml;
using System.Drawing;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.MPAL
{
    /// <summary>
    /// Implements the MPAL program Process-Station.
    /// </summary>
    public class MpalPS : WorkPS
    {
        public MpalPS()
        {
            base.Type   = "Mp.MPAL.PS";
            base.Text   = StringResource.Program;
            base.Group  = StringResource.Calculation;
            base.Symbol = Images.Mpal;
            base.Icon   = Images.MPALIcon;
        }

        public override string RuntimeModule
        {
            get { return "mps-mpal"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.Program;
            base.Group = StringResource.Calculation;
        }

        public override string Description
        {
            get
            {
                return StringResource.ProgramPsDescription;
            }
        }

        protected override void OnDocumentChanged()
        {
            for (int p = 0; p < XmlRep.ChildNodes.Count; ++p)
            {
                XmlElement xmlSigMap = (XmlElement)XmlRep.ChildNodes[p];
                XmlAttribute nameAttr = xmlSigMap.Attributes["name"];

                if (nameAttr == null)
                    continue;

                if (nameAttr.Value != "sigVarMap")
                    continue;

                string[] strArray = xmlSigMap.InnerText.Split('/');

                uint id = Convert.ToUInt32(strArray[0]);
                XmlElement xmlSignal = Document.GetXmlObjectById(id);

                if (xmlSignal == null)
                {
                    XmlRep.RemoveChild(xmlSigMap);
                    --p;
                    continue;
                }

                bool isInInputPort  = IsSignalInPortsAvailable(id, true, null);
                bool isInOutputPort = IsSignalInPortsAvailable(id, false, null);

                if (!isInInputPort && !isInOutputPort)
                {//Signal doesn't exist => remove the mapping.
                    XmlRep.RemoveChild(xmlSigMap);
                    --p;
                    continue;
                }
            }

            UpdateOutPortSigSampleRate();
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site,  350);
        }

        private void UpdateOutPortSigSampleRate()
        {
            uint triggerSigId = (uint) XmlHelper.GetParamNumber(XmlRep, "triggerSigID");
            XmlElement xmlTriggerSig = Document.GetXmlObjectById(triggerSigId);

            if (xmlTriggerSig == null)
                return;

            double samplerate = XmlHelper.GetParamDouble(xmlTriggerSig, "samplerate");
            foreach (Port port in OutputPorts)
            {
                foreach (XmlElement xmlSignal in port.SignalList)

                    XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", samplerate);
            }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            InitContextMenu();

            XmlHelper.SetParam(XmlRep, "debuggerIP","string", "127.0.0.1");
            XmlHelper.SetParamNumber(XmlRep, "debuggerPort","uint32_t", 8000);
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitContextMenu();
            RecalcStationRect();
        }

        private void InitContextMenu()
        {
            ToolStripMenuItem menuItem;

            ToolStripSeparator menuSeparator = new ToolStripSeparator();
            this.ContextMenuStrip.Items.Add(menuSeparator);

            menuItem = new ToolStripMenuItem(StringResource.AddInputPortMenu);
            menuItem.Click += new System.EventHandler(this.OnAddInputPort);
            this.ContextMenuStrip.Items.Add(menuItem);

            menuSeparator = new ToolStripSeparator();
            this.ContextMenuStrip.Items.Add(menuSeparator);

            menuItem = new ToolStripMenuItem(StringResource.RemoveUnusedPortsMenu);
            menuItem.Click += new System.EventHandler(this.OnRemoveUnusedPorts);
            this.ContextMenuStrip.Items.Add(menuItem);
        }

        private void OnAddInputPort(object sender, EventArgs e)
        {
            int newPortPos = 0;

            if (InputPorts.Count > 0)
            {
                Port lastPort = InputPorts[InputPorts.Count - 1];
                newPortPos = lastPort.Position.Y + DistanceBetweenPort;
            }
            else
            {
                newPortPos = _rectangle.Top + PortTopOffset;
            }

            if (_rectangle.Bottom < PortTopOffset + newPortPos)
                _rectangle.Height = PortTopOffset + newPortPos - _rectangle.Top;

            Port port = new Port(new Point(_rectangle.Left - PortWidth, newPortPos), "Mp.Port.In", true, false);
            AddPort(port);
            Document.Modified = true;
            base.Site.Invalidate();
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            MpalPSDlg mpalDlg = new MpalPSDlg(this);
            mpalDlg.ShowDialog();
        }

        private void OnRemoveUnusedPorts(object sender, EventArgs e)
        {
            Port port;
            int index;
            bool portRemoved = false;

            OnDocumentChanged();

            for (index = 0; index < InputPorts.Count; index++)
            {
                port = InputPorts[index];

                if (!port.Connected && port.SignalList == null)
                {
                    portRemoved = true;
                    base.RemovePort(port);
                    index--;
                }
            }

            for (index = 0; index < OutputPorts.Count; index++)
            {
                port = OutputPorts[index];

                if (!port.Connected && port.SignalList.ChildNodes.Count == 0)
                {
                    portRemoved = true;
                    base.RemovePort(port);
                    index--;
                }
            }
            RecalcStationRect();

            if (portRemoved)
                Document.Modified = portRemoved;

            base.Site.Invalidate();
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);

            string program = XmlHelper.GetParam(XmlRep, "program");

            if (program == null || program == "")
            {
                string msg = String.Format(StringResource.NoProgLoadErr, base.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
            else
            {
                long id = XmlHelper.GetParamNumber(XmlRep, "triggerSigID");

                if (id == 0)
                {
                    string msg = String.Format(StringResource.TriggerForProgErr, base.Text);
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }
            }

            //Check property mapping     
            for (int p = 0; p < XmlRep.ChildNodes.Count; ++p)
            {
                XmlElement xmlSigMap = (XmlElement)XmlRep.ChildNodes[p];
                XmlAttribute nameAttr = xmlSigMap.Attributes["name"];

                if (nameAttr == null)
                    continue;

                if (nameAttr.Value != "sigDefValue")
                    continue;

                string[] strArray = xmlSigMap.InnerText.Split('/');

                string property = strArray[0];
                if (property.Length > 1)
                {
                    if (property[0] == '$')
                    {
                        if (!Document.IsPropertyAvailable(property))
                        {
                            string msg = String.Format(StringResource.PropNotAvailErr, property, base.Text);
                            valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                        }
                    }
                }
            }
           
        }
    }
}
