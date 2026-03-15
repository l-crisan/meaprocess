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
using Mp.Visual.Diagram;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// Signal splitter Process-Station.
    /// </summary>
    public class SplitterPS : WorkPS
    {
        private Dictionary<uint,List<int>> _cache = new Dictionary<uint,List<int>>();


        public SplitterPS()
        {
            base.Type   = "Mp.PS.Splitter";
            base.Text   = StringResource.Splitter;
            base.Group  = StringResource.General;
            base.Symbol = Mp.Scheme.Sdk.Images.Splitter;
            base.Icon = Mp.Scheme.Sdk.Images.SplitterIcon;
            base.AcceptObjectSignal = true;
        }


        public override void OnLoadResources()
        {
            base.Text = StringResource.Splitter;
            base.Group = StringResource.General;
        }


        public override string RuntimeModule
        {
            get { return ""; }
        }


        public override string Description
        {
            get
            {
                return StringResource.SplitterPsDescription;
            }
        }


        protected override void OnDocumentChanged()
        {
            //Iterate the output ports and check if we have the signals in the input ports.
            // If not remove the signal from output port.
            foreach (Port outPort in OutputPorts)
            {
                if( outPort.SignalList == null)
                    continue;

                for (int i = 0; i < outPort.SignalList.ChildNodes.Count; i++)
                {
                    XmlElement xmlSignalRef = (XmlElement) outPort.SignalList.ChildNodes[i];
                    uint sigId = Convert.ToUInt32(xmlSignalRef.InnerText);

                    if(!IsSignalInPortsAvailable( sigId, true,null ) )
                    {
                        outPort.SignalList.RemoveChild(xmlSignalRef);
                        i--;
                    }
                }
            }

            LoadCache();
        }


        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site,330);
        }


        public override void OnDefaultInit()
        {
            base.OnDefaultInit();
            InitContextMenu();
        }


        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitContextMenu();
            RecalcStationRect();
        }


        protected override void OnProperties(object sender, EventArgs e)
        {
            SplitterPSDlg dlg = new SplitterPSDlg(Document);
        
            dlg.InputPorts  = InputPorts;
            dlg.OutputPorts = OutputPorts;
            dlg.Document    = Document;
            dlg.PsName      = Text;

            if(dlg.ShowDialog() == DialogResult.OK)
            {
                SaveCache();
                Text = dlg.PsName;
                Document.Modified = true;
            }
        }


        private void SaveCache()
        {
            _cache.Clear();

            for(int i = 0;  i < OutputPorts.Count; ++i)
            {
                Port outp = OutputPorts[i];

                foreach(XmlElement xmlElement in outp.SignalList)
                {
                    XmlElement xmlSignal = Document.GetSignal(xmlElement);
                    uint sigId = XmlHelper.GetObjectID(xmlSignal);
                    if(!_cache.ContainsKey(sigId))
                    {
                        List<int> list = new List<int>();
                        list.Add(i);
                        _cache[sigId] = list;
                    }
                    else
                    {
                        List<int> list = _cache[sigId];
                        list.Add(i);
                    }
                }
            }
        }


        private void LoadCache()
        {
            foreach(KeyValuePair<uint,List<int>> entry in _cache)
            {
                uint sigId = entry.Key;
                
                if(!IsSignalInPortsAvailable(sigId, true, null))
                    continue;


                foreach(int portIndex in entry.Value)
                {
                    if( portIndex >= OutputPorts.Count)
                        continue;

                    Port port = OutputPorts[portIndex];

                    if(!IsSignalInPortAvailable(port, sigId, null))
                        XmlHelper.CreateElement(port.SignalList, "uint32_t", "signalRef", sigId.ToString());
                }
            }
        }


        public override void OnPostDisconnectedConnector(Connector from, Connector to)
        {
            base.OnPostDisconnectedConnector(from, to);
            Port port = (Port) to;

            if (port.SignalList == null)
                return;

            XmlElement  xmlSignal;
            XmlElement  xmlSignalRef;
            ulong       sigId;

            for (int sigIdx = 0; sigIdx < port.SignalList.ChildNodes.Count; sigIdx++)
            {
                xmlSignal = port.SignalList.ChildNodes[sigIdx] as XmlElement;
                
                if (xmlSignal == null)
                    continue;

                sigId = XmlHelper.GetObjectID(xmlSignal);

                foreach (Port outPort in OutputPorts)
                {
                    for (int i = 0; i < outPort.SignalList.ChildNodes.Count; i++)
                    {
                        xmlSignalRef = (outPort.SignalList.ChildNodes[i] as XmlElement);
                        
                        if (xmlSignalRef == null)
                            continue;

                        if (Convert.ToUInt64(xmlSignalRef.InnerText) == sigId)
                        {
                            outPort.SignalList.RemoveChild(xmlSignalRef);
                            i--;
                        }
                    }
                }
            }
            port.SignalList = null;
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

            if( portRemoved )
                Document.Modified = portRemoved;

            base.Site.Invalidate();
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


        private void OnAddOutputPort(object sender, EventArgs e)
        {
            int newPortPos = 0;

            if (OutputPorts.Count > 0)
            {
                Port lastPort = OutputPorts[OutputPorts.Count - 1];
                newPortPos = lastPort.Position.Y + DistanceBetweenPort;
            }
            else
            {
                newPortPos = _rectangle.Top + PortTopOffset;
            }

            if (_rectangle.Bottom < PortTopOffset + newPortPos)
                _rectangle.Height = PortTopOffset + newPortPos - _rectangle.Top;

            Port port = new Port(new Point(_rectangle.Right + PortWidth, newPortPos), "Mp.Port.Out", false, true);
            AddPort(port);
            port.SignalList = Document.CreateSignalList();
            Document.Modified = true;
            base.Site.Invalidate();
        }


        private void InitContextMenu()
        {
            ToolStripMenuItem menuItem;

            ToolStripSeparator menuSeparator = new ToolStripSeparator();
            this.ContextMenuStrip.Items.Add(menuSeparator);

            menuItem = new ToolStripMenuItem(StringResource.AddInputPortMenu);
            menuItem.Click += new System.EventHandler(this.OnAddInputPort);
            this.ContextMenuStrip.Items.Add(menuItem);

            menuItem = new ToolStripMenuItem(StringResource.AddOutputPortMenu);
            menuItem.Click += new System.EventHandler(this.OnAddOutputPort);
            this.ContextMenuStrip.Items.Add(menuItem);

            menuSeparator = new ToolStripSeparator();
            this.ContextMenuStrip.Items.Add(menuSeparator);

            menuItem = new ToolStripMenuItem(StringResource.RemoveUnusedPortsMenu);
            menuItem.Click += new System.EventHandler(this.OnRemoveUnusedPorts);
            this.ContextMenuStrip.Items.Add(menuItem);
        }


        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            if (InputPorts.Count == 0 || OutputPorts.Count == 0)
            {
                string msg = String.Format(StringResource.MinOnePortErr,this.Text);

                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            base.OnValidate(valInfoList);
        }
    }
}
