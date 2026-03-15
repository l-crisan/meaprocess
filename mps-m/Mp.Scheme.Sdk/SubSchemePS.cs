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
    public class SubSchemePS : WorkPS
    {
        private DiagramWindow _diagramWindow;
        private RectangleShape _diagramInput;
        private RectangleShape _diagramOutput;
        private int _undoPos = 0;
        private List<uint> _savedSignalListID;


        public SubSchemePS()
        {
            base.Type = "Mp.PS.SubScheme";
            base.Text = StringResource.SubScheme;
            base.Group = StringResource.General;
            base.Symbol = Mp.Scheme.Sdk.Images.SubScheme;
            base.Icon = Mp.Scheme.Sdk.Images.SchemeIcon;
            base.AcceptObjectSignal = true;
        }

        public override string RuntimeModule
        {
            get { return ""; }
        }
        public override void OnLoadResources()
        {
            base.Text = StringResource.SubScheme;
            base.Group = StringResource.General;
        }

        public override string Description
        {
            get
            {
                return StringResource.SubschemePsDescription;
            }
        }

        protected override void OnDocumentChanged()
        {
            for (int i = 0; i < _diagramInput.Connectors.Count; ++i)
            {
                Port port = InputPorts[i];
                Port subPort = (Port)_diagramInput.Connectors[i];

                if (port.SignalList != null)
                    CopyPortSignalList(port, subPort);
                else
                    RemovePortSignals(subPort);
            }

            for (int i = 0; i < _diagramOutput.Connectors.Count; ++i)
            {
                Port subPort = (Port)_diagramOutput.Connectors[i];
                Port port = OutputPorts[i];

                CopyPortSignalList(subPort, port);
            }
        }


        public override void OnDefaultInit()
        {
            CreateDiagram();
            base.OnDefaultInit();
            InitContextMenu();
        }


        private void CreateDiagram()
        {
            _diagramWindow = new DiagramWindow();
            _diagramWindow.Text = StringResource.SubScheme;
            _diagramWindow.TabText = StringResource.SubScheme;
            Document.InsertDiagram(_diagramWindow);

            _diagramInput = new RectangleShape(_diagramWindow.Diagram);
            _diagramOutput  = new RectangleShape(_diagramWindow.Diagram);

            _diagramInput.CanDelete = false;
            _diagramInput.X = 16;
            _diagramInput.Y = 16;
            _diagramInput.Width = 16;
            _diagramInput.Height = 32;
            _diagramInput.ShapeColor = Color.LightGray;

            _diagramOutput.CanDelete = false;
            _diagramOutput.X = 600;
            _diagramOutput.Y = 16;
            _diagramOutput.Width = 16;
            _diagramOutput.Height = 32;
            _diagramOutput.ShapeColor = Color.LightGray;

            _diagramWindow.Diagram.AddShape(_diagramInput);
            _diagramWindow.Diagram.AddShape(_diagramOutput);


            _diagramWindow.Diagram.ClearUndoStack();
            _diagramWindow.Diagram.Invalidate();
        }


        public override void OnLoadXml()
        {
            if (_diagramWindow == null)
            {
                CreateDiagram();
                _diagramWindow.Diagram.ID = (uint)XmlHelper.GetParamNumber(XmlRep, "myDiagramID");
                _diagramInput.X = (int)XmlHelper.GetParamNumber(XmlRep, "inputBarX");
                _diagramInput.Y = (int)XmlHelper.GetParamNumber(XmlRep, "inputBarY");
                _diagramInput.Width = (int)XmlHelper.GetParamNumber(XmlRep, "inputBarWidth");
                _diagramInput.Height = (int)XmlHelper.GetParamNumber(XmlRep, "inputBarHeight");

                _diagramOutput.X = (int)XmlHelper.GetParamNumber(XmlRep, "outputBarX");
                _diagramOutput.Y = (int)XmlHelper.GetParamNumber(XmlRep, "outputBarY");
                _diagramOutput.Width = (int)XmlHelper.GetParamNumber(XmlRep, "outputBarWidth");
                _diagramOutput.Height = (int)XmlHelper.GetParamNumber(XmlRep, "outputBarHeight");
            }

            XmlElement xmlSubPorts = XmlHelper.GetChildByType(XmlRep, "Mp.InputSubPorts");
            LoadSubPorts(xmlSubPorts, true, true);

            xmlSubPorts = XmlHelper.GetChildByType(XmlRep, "Mp.OutputSubPorts");
            LoadSubPorts(xmlSubPorts, false, true);
            
            foreach (Port port in _diagramInput.Connectors)
                port.OnLoadXml();

            foreach (Port port in _diagramOutput.Connectors)
                port.OnLoadXml();

            base.OnLoadXml();
            InitContextMenu();
            RecalcStationRect();
            SetDiagramText();
        }


        private void LoadSubPorts(XmlElement xmlPorts, bool input, bool createPort)
        {
            if (xmlPorts == null)
                return;

            int i = 0;
            foreach (XmlElement xmlPort in xmlPorts.ChildNodes)
            {
                Point pos = new Point();

                pos.X = (int)XmlHelper.GetParamNumber(xmlPort, "xPos");
                pos.Y = (int)XmlHelper.GetParamNumber(xmlPort, "yPos");

                Port port;

                if (createPort)
                {
                    if (input)
                    {
                        port = new Port(pos, xmlPort.GetAttribute("subType"), !input, true);
                        _diagramInput.AddConnnector(port);
                    }
                    else
                    {
                        port = new Port(pos, xmlPort.GetAttribute("subType"), !input, true);
                        port.OnPostConnect += new PostConnectConnectorDelegate(OnPostConnectOutputSubPort);
                        port.OnPostDisconnect += new PostConnectConnectorDelegate(OnPostDisconnectOutputSubPort);
                        _diagramOutput.AddConnnector(port);
                    }

                }
                else
                {
                    if (input)
                        port = (Port) _diagramInput.Connectors[i];
                    else
                        port = (Port)_diagramOutput.Connectors[i];
                }

                port.XmlRep = xmlPort;
                port.Document = Document;
                Document.RegPort(port);

                port.OnLoadXml();


                ++i;
            }

            int yoffset = _diagramInput.Y;

            if (_diagramInput.Connectors.Count > 0)
                yoffset = _diagramInput.Connectors[_diagramInput.Connectors.Count - 1].Position.Y;

            if ((_diagramInput.Y + _diagramInput.Height) >= yoffset + DistanceBetweenPort)
                _diagramInput.Height = (yoffset + 2 * DistanceBetweenPort) - _diagramInput.Y;

            yoffset = _diagramOutput.Y;

            if (_diagramOutput.Connectors.Count > 0)
                yoffset = _diagramOutput.Connectors[_diagramOutput.Connectors.Count - 1].Position.Y;

            if ((_diagramOutput.Y + _diagramOutput.Height) >= yoffset + DistanceBetweenPort)
                _diagramOutput.Height = (yoffset + 2 * DistanceBetweenPort) - _diagramOutput.Y;
        }


        public override void OnSaveXml()
        {
            base.OnSaveXml();
            
            XmlHelper.SetParamNumber(XmlRep,"inputBarX","int32_t",_diagramInput.X);
            XmlHelper.SetParamNumber(XmlRep, "inputBarY", "int32_t", _diagramInput.Y);
            XmlHelper.SetParamNumber(XmlRep, "inputBarWidth", "int32_t", _diagramInput.Width);
            XmlHelper.SetParamNumber(XmlRep, "inputBarHeight", "int32_t", _diagramInput.Height);

            XmlHelper.SetParamNumber(XmlRep, "outputBarX", "int32_t", _diagramOutput.X);
            XmlHelper.SetParamNumber(XmlRep, "outputBarY", "int32_t", _diagramOutput.Y);
            XmlHelper.SetParamNumber(XmlRep, "outputBarWidth", "int32_t", _diagramOutput.Width);
            XmlHelper.SetParamNumber(XmlRep, "outputBarHeight", "int32_t", _diagramOutput.Height);
            XmlHelper.SetParamNumber(XmlRep, "myDiagramID", "int32_t", _diagramWindow.Diagram.ID);

            foreach (Port port in _diagramInput.Connectors)
                port.OnSaveXml();

            foreach (Port port in _diagramOutput.Connectors)
                port.OnSaveXml();
        }


        public override void OnRestore()
        {
            foreach (uint id in _savedSignalListID)
                Document.CreateSignalList(id);           
     
            XmlElement xmlSubPorts = XmlHelper.GetChildByType(XmlRep, "Mp.InputSubPorts");
            LoadSubPorts(xmlSubPorts, true, false);

            xmlSubPorts = XmlHelper.GetChildByType(XmlRep, "Mp.OutputSubPorts");
            LoadSubPorts(xmlSubPorts, false, false);     

            Document.ShowDiagram(_diagramWindow);
                
            base.OnRestore();

            for( int i = 0; i < _undoPos; ++i)
                _diagramWindow.Diagram.Undo(); // Retore the content of the diagram
        }


        public override void OnRemove()
        {
            _savedSignalListID = new List<uint>();

            foreach (Port port in OutputPorts)
                _savedSignalListID.Add(XmlHelper.GetObjectID(port.SignalList));

            base.OnRemove();

            _undoPos = 0;

            if (_diagramWindow.Diagram.Connections.Count > 0)
                _undoPos++;

            if(_diagramWindow.Diagram.Shapes.Count > 2)
                _undoPos++;


            foreach( Port port in _diagramInput.Connectors)
            {
                _savedSignalListID.Add(XmlHelper.GetObjectID(port.SignalList));
                Document.RemoveXmlObject(port.SignalList);
                port.SignalList = null;
            }
            

            if (_diagramWindow != null)
            {
                _diagramWindow.Hide();
                Document.RemoveDiagram(_diagramWindow);
            }
            
        }


        protected override void OnProperties(object sender, EventArgs e)
        {
            base.OnProperties(sender, e);
            SetDiagramText();
        }


        private void SetDiagramText()
        {
            _diagramWindow.Text = this.Text;
            _diagramWindow.Diagram.Text = this.Text;
            _diagramWindow.TabText = this.Text;
        }


        private void OnRemoveUnusedPorts(object sender, EventArgs e)
        {
            Port port;
            int index;
            bool portRemoved = false;

            OnDocumentChanged();
            //Input ports
            for (index = 0; index < InputPorts.Count; index++)
            {
                port = InputPorts[index];
                Port subPort = (Port)_diagramInput.Connectors[index];

                if (!port.Connected && port.SignalList == null && !subPort.Connected)
                {
                    portRemoved = true;
                    base.RemovePort(port);
                    Port inputSubPort =  (Port) _diagramInput.Connectors[index];
                    _diagramInput.Connectors.Remove(inputSubPort);
                    Document.RemoveXmlObject(inputSubPort.XmlRep);                    
                    index--;
                }
            }
            
            int yOffset = 2*DistanceBetweenPort;
            
            if (_diagramInput.Connectors.Count > 0)
                yOffset = DistanceBetweenPort + _diagramInput.Connectors[_diagramInput.Connectors.Count - 1].Position.Y;

            _diagramInput.Height = yOffset;

            //Output Ports
            for (index = 0; index < OutputPorts.Count; index++)
            {
                port = OutputPorts[index];
                Port subPort = (Port)_diagramOutput.Connectors[index];

                if (!port.Connected && port.SignalList.ChildNodes.Count == 0 && !subPort.Connected)
                {
                    portRemoved = true;
                    base.RemovePort(port);
                    Port outputSubPort = (Port)_diagramOutput.Connectors[index];
                    _diagramOutput.Connectors.Remove(outputSubPort);
                    Document.RemoveXmlObject(outputSubPort.XmlRep);                    
                    index--;
                }
            }

            yOffset = 2 * DistanceBetweenPort;

            if (_diagramOutput.Connectors.Count > 0)
                yOffset = DistanceBetweenPort + _diagramOutput.Connectors[_diagramOutput.Connectors.Count - 1].Position.Y;

            _diagramOutput.Height = yOffset;

            RecalcStationRect();

            if (portRemoved)
                Document.Modified = portRemoved;


            _diagramWindow.Diagram.Invalidate();
            this.Site.Invalidate();
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
                newPortPos = _rectangle.Top + DistanceBetweenPort;
            }
            
            if (_rectangle.Bottom < PortTopOffset + newPortPos)
                _rectangle.Height = PortTopOffset + newPortPos - _rectangle.Top;

            Port port = new Port(new Point(_rectangle.Left - PortWidth, newPortPos), "Mp.Port.In", true, false);
            AddPort(port);

            int yoffset = _diagramInput.Y;// +DistanceBetweenPort;

            if (_diagramInput.Connectors.Count > 0)
                yoffset = _diagramInput.Connectors[_diagramInput.Connectors.Count - 1].Position.Y;

            Port diagramInputPort = new Port(new Point(_diagramInput.X + 24, yoffset + DistanceBetweenPort), "Mp.Port.In", false, true);
            
            diagramInputPort.SignalList = Document.CreateSignalList();
            diagramInputPort.Document = Document;
            XmlElement xmlInputSubPorts = XmlHelper.GetChildByType(XmlRep, "Mp.InputSubPorts");
            
            if (xmlInputSubPorts == null)
                xmlInputSubPorts = Document.CreateXmlObject(XmlRep, "Mp.InputSubPorts", "");

            diagramInputPort.XmlRep = Document.CreateXmlObject(xmlInputSubPorts, "Port", "Mp.Port.In");
            Document.RegPort(diagramInputPort);

            if ((_diagramInput.Y + _diagramInput.Height) >= yoffset + DistanceBetweenPort)
                _diagramInput.Height = (yoffset + 2 * DistanceBetweenPort) - _diagramInput.Y;

            _diagramInput.AddConnnector(diagramInputPort);
            _diagramWindow.Diagram.Invalidate();
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
                newPortPos = _rectangle.Top +PortTopOffset;
            }

            if (_rectangle.Bottom < PortTopOffset + newPortPos)
                _rectangle.Height = PortTopOffset + newPortPos - _rectangle.Top;

            Port port = new Port(new Point(_rectangle.Right + PortWidth, newPortPos), "Mp.Port.Out", false, true);
            AddPort(port);
            port.SignalList = Document.CreateSignalList();

            int yoffset = _diagramOutput.Y;// +DistanceBetweenPort;

            if (_diagramOutput.Connectors.Count > 0)
                yoffset = _diagramOutput.Connectors[_diagramOutput.Connectors.Count - 1].Position.Y;

            Port diagramPort = new Port(new Point(_diagramOutput.X - 10, yoffset + DistanceBetweenPort), "Mp.Port.Sub", true, true);
            diagramPort.SignalList = Document.CreateSignalList();
            diagramPort.Document = Document;

            if ((_diagramOutput.Y + _diagramOutput.Height) >= yoffset + DistanceBetweenPort)
                _diagramOutput.Height = (yoffset + 2 * DistanceBetweenPort) - _diagramOutput.Y;

            XmlElement xmlOutputSubPorts = XmlHelper.GetChildByType(XmlRep, "Mp.OutputSubPorts");

            if (xmlOutputSubPorts == null)
                xmlOutputSubPorts = Document.CreateXmlObject(XmlRep, "Mp.OutputSubPorts", "");

            diagramPort.XmlRep = Document.CreateXmlObject(xmlOutputSubPorts, "Mp.Port", "Mp.Port.Sub");
            diagramPort.OnPostConnect += new PostConnectConnectorDelegate(OnPostConnectOutputSubPort);
            diagramPort.OnPostDisconnect += new PostConnectConnectorDelegate(OnPostDisconnectOutputSubPort);
            Document.RegPort(diagramPort);
            _diagramOutput.AddConnnector(diagramPort);
            _diagramWindow.Diagram.Invalidate();

            Document.Modified = true;
            base.Site.Invalidate();
        }


        private void OnPostDisconnectOutputSubPort(Connector from, Connector to)
        {
            Port portFrom = from as Port;
            Port portTo = to as Port;
            RemoveRefLinkToPort(portFrom, portTo);

            portTo.SignalList = null;
        }


        private static void RemoveRefLinkToPort(Port portFrom, Port portTo)
        {
            XmlElement refLinkToPort;

            //Remove the refLinkToPort from the from Port
            //Search all refLinks 
            for (int i = 0; i < portFrom.XmlRep.ChildNodes.Count; i++)
            {
                refLinkToPort = (portFrom.XmlRep.ChildNodes[i] as XmlElement);

                if (refLinkToPort == null)
                    continue;

                //Check for the correct ref link
                if (refLinkToPort.GetAttribute("name") == "refLinkToPort" &&
                    refLinkToPort.InnerText == (XmlHelper.GetObjectID(portTo.XmlRep).ToString()))
                {
                    portFrom.XmlRep.RemoveChild(refLinkToPort);
                }
            }
        }


        private static void RemovePortSignals(Port port)
        {
            if (port.SignalList == null)
                return;

            for (int i = 0; i < port.SignalList.ChildNodes.Count; ++i)
            {
                XmlNode node = port.SignalList.ChildNodes[i];
                port.SignalList.RemoveChild(node);
                --i;
            }
        }


        private void OnPostConnectOutputSubPort(Connector from, Connector to)
        {
            Port pfrom = (Port) from;
            Port pto = (Port) to;

            if (!ExistConnectionInXML(pfrom, pto))
            {
                string linkid = XmlHelper.GetObjectID(pto.XmlRep).ToString();
                XmlHelper.CreateElement(pfrom.XmlRep, "uint32_t", "refLinkToPort", linkid);
            }

            pto.SignalList = pfrom.SignalList;
        }


        private static void CopyPortSignalList(Port pfrom, Port pto)
        {
            RemovePortSignals(pto);

            if (pfrom.SignalList == null)
                return;

            foreach (XmlElement xmlSignal in pfrom.SignalList)
            {
                uint signalID = XmlHelper.GetObjectID(xmlSignal);

                if (signalID == 0)
                    signalID = Convert.ToUInt32(xmlSignal.InnerText);

                XmlHelper.CreateElement(pto.SignalList, "uint32_t", "signalRef", signalID.ToString());
            }
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


            this.ContextMenuStrip.Items.Add(menuItem);
            this.ContextMenuStrip.Items[0].Font = this.ContextMenuStrip.Items[1].Font;
        }


        public override void OnMouseDoubleClick(Point p)
        {
            if (_diagramWindow != null)
                _diagramWindow.Activate();
        }


        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
        }


        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 600);
        }
    }
}
