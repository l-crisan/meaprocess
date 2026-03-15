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
using System.Text;
using System.Drawing;
using System.Xml;
using System.Windows.Forms;
using Mp.Visual.Diagram;
using Mp.Utils;

namespace Mp
{
    namespace Scheme.Sdk
    {
        ///<summary>
        /// The base class for each Process Station.
        /// </summary>
        /// <remarks>
        /// This class represents a process station in the diagram.
        /// To create your own Process Station derive this.
        /// </remarks>
        public class ProcessStation : RectangleShape
        {
            private List<XmlElement> _outputPortsSigList = new List<XmlElement>();
            private Hashtable _sources = new Hashtable();

            /// <summary>
            /// If set to true this process station can have only one instance.
            /// </summary>
            private bool _singleton = false;

            /// <summary>
            ///  The type identifier of the process station.
            /// </summary>
            private string _type;

            /// <summary>
            ///  The classification group of the process station.
            /// </summary>
            private string _group = "General";
            
            /// <summary>
            ///  The image represenation of the process station.
            /// </summary>
            private Image _symbol;            

            private Icon _icon;

            private string _runtimeEngine;

            /// <summary>
            /// Gets or sets the XML representation of the process station.
            /// </summary>
            public XmlElement XmlRep
            {
                get { return _xmlRep; }
                set { _xmlRep = value; }
            }

            public virtual  string RuntimeModule
            {
                get { return ""; }
            }

            private XmlElement _xmlRep;

            /// <summary>
            ///  The XML representation of the process station input ports.
            /// </summary>
            private XmlElement _xmlInputPorts;
            
            /// <summary>
            ///  The XML representation of the process station output ports.
            /// </summary>
            private XmlElement _xmlOutputPorts;


            private Document _document;
            
            /// <summary>
            ///  Gets the input ports of the process station.
            /// </summary>
            public List<Port> InputPorts
            {
                get { return _inputPorts; }
            }

            private List<Port> _inputPorts = new List<Port>();
            
            /// <summary>
            /// Gets the output ports.
            /// </summary>
            public List<Port> OutputPorts
            {
                get { return _outputPorts; }
            }

            private List<Port> _outputPorts = new List<Port>();
            
            /// <summary>
            ///  Graphic representation: 
            ///  The distance in pixel of the port from top of process station.
            /// </summary>
            public byte PortTopOffset
            {
                get { return 16; }
            }

            /// <summary>
            ///  Graphic representation:
            ///  The port width in pixel.
            /// </summary>
            public byte PortWidth
            {
                get { return 8; }
            }

            /// <summary>
            ///  Graphic representation:
            ///  The distance between two ports in pixel.
            /// </summary>
            public byte DistanceBetweenPort
            {
                get { return 16; }
            }

            private string _subType = "";

            /// <summary>
            /// Gets or sets the process station sub type.
            /// </summary>
            public string SubType
            {
                get { return _subType;}
                set { _subType = value; }
            }

            /// <summary>
            /// Gets the process station document.
            /// </summary>
            public Document Document
            {
                get { return _document; }
            }

            /// <summary>
            ///  Gets or sets the context menu strip.
            /// </summary>
            protected ContextMenuStrip ContextMenuStrip
            {
                set { _contextMenuStrip = value; }
                get { return _contextMenuStrip; }
            }

            /// <summary>
            /// Gets or sets the singleton flag.
            /// </summary>
            public bool IsSingleton
            {
                get { return _singleton; }
                set { _singleton = value; }
            }

            /// <summary>
            ///  Gets or sets the process station identifier. 
            /// </summary>
            public string Type
            {
                get{ return _type; }
                set{ _type = value; }
            }

            public virtual string Description
            {
                get { return ""; }
            }

            /// <summary>
            ///  Gets or sets the process station runtime engine identifier. 
            /// </summary>
            public string RuntimeEngine
            {
                get { return _runtimeEngine; }
                set { _runtimeEngine = value; }
            }

            /// <summary>
            ///  Gets or sets the process station classification group identifier.
            /// </summary>
            public string Group
            {
                get { return _group; }
                set { _group = value; }
            }

            /// <summary>
            ///  Gets or sets the process station symbol.
            /// </summary>
            protected Image Symbol
            {
                get { return _symbol; }
                set { _symbol = value; }
            }

            /// <summary>
            ///  Gets or sets the process station icon.
            /// </summary>
            public Icon Icon
            {
                get { return _icon; }
                set { _icon = value; }
            }

            /// <summary>
            ///   Default constructor. 
            /// </summary>
            public ProcessStation() 
            {
                CreateMenuStrip();
            }

            public virtual string CopyToXml()
            {
                OnSaveXml();

                StringBuilder sb = new StringBuilder();
                sb.Append("<Station>");
                sb.Append(XmlRep.OuterXml);
                sb.Append("</Station>");
                return sb.ToString();
            }

            public virtual void LoadVisualData(XmlElement  data)
            {
            }

            /// <summary> 
            /// Init the process station process station.
            /// </summary>           
            /// <remarks>
            ///  Create a copy of the process station in the given document and
            ///  attach the process station to a XML representation.
            /// </remarks>
            /// <param name="document">
            ///  The document in which to create the copy.
            /// </param>
            /// <param name="XmlRep">
            ///  The xml representation which should be attached to the process station. 
            /// </param>      
            /// <param name="pos">
            /// The position in diagram.
            /// </param>
            public virtual void Init(Document document, DiagramCtrl diagram, XmlElement xmlRep , Point pos)
            {
                //Document.
                _document = document;

                base.Site = diagram;

                //XML representation.
                XmlRep = xmlRep;
                XmlHelper.SetParamNumber(xmlRep, "diagram", "uint32_t", (int) diagram.ID);
                //Graph data.                 
                X = pos.X;
                Y = pos.Y;
                _rectangle.Height = 96;
                _rectangle.Width = 80;
                ShapeColor = Color.Gray;

                //Add the shape to the visual control.
                diagram.AddShape(this);
            }
            
            /// <summary>
            ///  Called by the first creation of the process station. 
            /// </summary>           
            /// <remarks>
            ///  Overwrite this to initialize your process station with defualt values.
            ///  The base.OnDefaultInit() must be called.
            /// </remarks>
            public virtual void OnDefaultInit()
            {
                XmlHelper.SetParam( XmlRep, "name", "string", Text );
            }

            /// <summary>
            ///  Called to initialize the process station with xml parameter.
            /// </summary>           
            /// <remarks>
            ///  Overwrite this method to initialize your process station with persistent values from XML.
            ///  Use <see cref="XmlRep"/> to obtain the persistent values.
            ///  The base.OnLoadXml() must be called.
            /// </remarks>
            public virtual void OnLoadXml()
            {
                Text = XmlHelper.GetParam(XmlRep, "name");

                base.X = (int)XmlHelper.GetParamNumber(this.XmlRep, "xPos");
                base.Y = (int)XmlHelper.GetParamNumber(this.XmlRep, "yPos");
                XmlElement element;

                foreach (XmlNode child in this.XmlRep.ChildNodes)
                {
                    element = (child as XmlElement);

                    if (element == null)
                        continue;

                    if (element.GetAttribute("type") == "Mp.InputPorts")
                    {
                        _xmlInputPorts = element;
                        LoadXmlPorts(element.ChildNodes, true);
                    }
                    else if (element.GetAttribute("type") == "Mp.OutputPorts")
                    {
                        _xmlOutputPorts = element;
                        LoadXmlPorts(element.ChildNodes, false);
                    }
                }
            }

            /// <summary>
            ///  Called when the process station is saved to an XML document.
            /// </summary>           
            /// <remarks>
            ///  Overwrite this method to make your process station persistent in an XML document.
            ///  Use <see cref="XmlRep"/> to write your persistent values.
            ///  The base.OnSaveXml() must be called.
            /// </remarks>
            public virtual void OnSaveXml()
            {
                XmlHelper.SetParam(this.XmlRep, "name", "string", Text);

                XmlHelper.SetParamNumber(this.XmlRep, "xPos", "int32_t", base.X);
                XmlHelper.SetParamNumber(this.XmlRep, "yPos", "int32_t", base.Y);

                foreach (Port port in _inputPorts)
                    port.OnSaveXml();

                foreach (Port port in _outputPorts)
                    port.OnSaveXml();
            }

            /// <summary>
            ///  Called before the process station is removed.
            /// </summary>           
            /// <remarks>
            ///  Overwrite this method when you have to make clears before removing of the process station.
            ///  </remarks>
            public override void OnRemove()
            {   //Remove the Xml representation of the PS
                OnSaveXml();                

                //Save the sources
                _sources.Clear();

                foreach (Port port in OutputPorts)
                {
                    if (port.SignalList == null)
                        continue;

                    foreach (XmlElement xmlSignal in port.SignalList)
                    {
                        uint id = (uint)XmlHelper.GetParamNumber(xmlSignal, "sourceNumber");


                        if (!_sources.ContainsKey(id) && id != 0)
                        {
                            XmlElement xmlSource = Document.GetXmlObjectById(id);
                            _sources.Add(id, xmlSource);
                            Document.UnregisterSource(id);
                        }
                    }
                }

                XmlElement xmlClonedRep = (XmlElement)this.XmlRep.CloneNode(true);

                _outputPortsSigList.Clear();              
                //Remove the output-signal-list From-Signals 
                for(int j = 0; j < _outputPorts.Count; ++j)
                    _outputPortsSigList.Add(null);

                int i = 0;
                foreach (Port port in _outputPorts)
                {
                    if (port.SignalList == null)
                    {
                        ++i;
                        continue;
                    }

                    _outputPortsSigList[i] = (XmlElement)port.SignalList.CloneNode(true);
                    ++i;                   

                    _document.RemoveXmlObject(port.SignalList);
                }

                //Remove the xml connections for all input ports
                Port attachedFromPort;
                ulong portId;

                foreach (Port port in _inputPorts)
                {
                    foreach (Connector connector in port.AttachedConnectors)
                    {
                        attachedFromPort = (connector.Connection.From.AttachedTo as Port);
                        portId = XmlHelper.GetObjectID(port.XmlRep);

                        foreach (XmlElement refLinkToPort in attachedFromPort.XmlRep.ChildNodes)
                        {
                            if (refLinkToPort.GetAttribute("name") == "refLinkToPort" &&
                              refLinkToPort.InnerText == (portId.ToString()))
                            {
                                attachedFromPort.XmlRep.RemoveChild(refLinkToPort);
                            }
                        }
                    }
                }
                

                //Remove the xml representation of the PS selft                
                _document.RemoveXmlObject(this.XmlRep);                
                base.OnRemove();

                this.XmlRep = xmlClonedRep;

                XmlElement xmlInputPorts = XmlHelper.GetChildByType(this.XmlRep, "Mp.InputPorts");
                
                if( xmlInputPorts != null)
                    RemovePortLinks(xmlInputPorts);

                XmlElement xmlOutputPorts = XmlHelper.GetChildByType(this.XmlRep, "Mp.OutputPorts");
                
                if( xmlOutputPorts != null)
                    RemovePortLinks(xmlOutputPorts);                
            }

            private void RemovePortLinks(XmlElement xmlPorts)
            {
                foreach (XmlElement xmlPort in xmlPorts.ChildNodes)
                {
                    for (int i = 0; i < xmlPort.ChildNodes.Count; ++i)
                    {
                        XmlElement xmlElement = (XmlElement) xmlPort.ChildNodes[i];
                        
                        if (xmlElement.Attributes["name"] == null)
                            continue;

                        if (xmlElement.Attributes["name"].Value != "refLinkToPort")
                            continue;

                        xmlPort.RemoveChild(xmlElement);
                        --i;
                    }
                }
            }

            public override void OnRestore()
            {                
                XmlElement xmlPSList = XmlHelper.GetChildByType(_document.XmlDoc.DocumentElement, "Mp.PS.List");
                
                //Restore the process station self
                _document.AppendXmlObject(xmlPSList, this.XmlRep);

                //Restore the xml representation of the port.
                //Input ports
                XmlElement xmlPorts = XmlHelper.GetChildByType(XmlRep, "Mp.InputPorts");


                int i = 0;
                if (xmlPorts != null)
                {
                    foreach (XmlElement xmlElement in xmlPorts.ChildNodes)
                    {
                        InputPorts[i].XmlRep = xmlElement;
                        ++i;
                    }
                }
                

                //Output ports
                xmlPorts = XmlHelper.GetChildByType(XmlRep, "Mp.OutputPorts");

                if (xmlPorts != null)
                {
                    i = 0;
                    foreach (XmlElement xmlElement in xmlPorts.ChildNodes)
                    {
                        OutputPorts[i].XmlRep = xmlElement;
                        ++i;
                    }
                }
                
                //Restore the ports signal list
                i = 0;
                foreach (Port port in _outputPorts)
                {
                    port.SignalList = (XmlElement)_outputPortsSigList[i];
                    ++i;

                    if (port.SignalList == null)
                        continue;

                    XmlElement xmlSignalListList = XmlHelper.GetChildByType(_document.XmlDoc.DocumentElement, "Mp.Signals");
                    _document.AppendXmlObject(xmlSignalListList, port.SignalList);
                }
                
                //Restore the sources                                
                XmlElement xmlSources = XmlHelper.GetChildByType(_document.XmlDoc.DocumentElement, "Mp.Sources");
                foreach(DictionaryEntry entry in _sources)
                {
                    uint id = (uint) entry.Key;
                    XmlElement xmlSource = (XmlElement) entry.Value;
                    _document.AppendXmlObject(xmlSources, xmlSource);
                }

                //Now lets restore the rest
                base.OnRestore();
            }

            /// <summary>
            ///  Called by mouse double clicked on the process station.
            /// </summary>           
            /// <remarks>
            ///  The default implementation call <see cref="OnProperties"/>.
            ///  </remarks>
            public override void OnMouseDoubleClick( Point p )
            {
                base.OnMouseDoubleClick(p);
                OnProperties(this, null);
            }

            /// <summary>
            ///  Called before open the process station context menu.
            /// </summary>           
            /// <param name="p">
            /// Position to open the context menu.
            /// </param>
            public override void OnContextMenu(Point p)
            {
                base.OnContextMenu(p);

                if (_contextMenuStrip == null)
                    return;

                _contextMenuStrip.Show(Site.PointToScreen(p));
            }

            /// <summary>
            ///  Called before show the process station properties.
            /// </summary>           
            /// <remarks>
            /// The default implementation open the default process station dialog.
            /// <see cref="PSDefaultDlg"/>
            /// </remarks>
            protected virtual void OnProperties(object sender, EventArgs e)
            {
                PSDefaultDlg dlg = new PSDefaultDlg(XmlRep, Document);
                if (dlg.ShowDialog() == DialogResult.OK)
                    this.Text = XmlHelper.GetParam(_xmlRep, "name");

                Invalidate();
            }

            /// <summary>
            ///  <see cref="Paint"/>
            /// </summary>           
            /// <remarks>
            ///  The default implementation draws the process station. 
            ///  </remarks>
            public override void Paint(Graphics g)
            {
                base.Paint(g);

                Rectangle rect = _drawRectangle;
                
                rect.X += 1;
                rect.Y += 26;
                rect.Width -= 1;                
                rect.Height  = _symbol.Height;                

                g.DrawImage(_symbol,rect);
                
            }

            /// <summary>
            ///  Add a specified port to the process station.
            /// </summary>           
            /// <param name="port">
            ///  The port to add.
            /// </param>
            public void AddPort(Port port)
            {
                bool created = false;

                if (port.XmlRep == null)
                {
                    CreateXmlPort(port);
                    created = true;
                }


                if (port.IsInput)
                    _inputPorts.Add(port);
                else
                    _outputPorts.Add(port);

                port.Document   = _document;
                port.Station    = this;
                _document.RegPort(port);
                AddConnnector(port);

                if (created)
                    port.OnSaveXml();
            }

            /// <summary>
            ///  Remove the specified port from the process station.
            /// </summary>           
            /// <param name="port">
            ///  The port to remove.
            /// </param>
            public void RemovePort(Port port)
            {                                
                base.RemoveConnector(port);
                _document.UnregPort(port);

                if (port.IsInput)
                {
                    _document.RemoveXmlObject(port.XmlRep);
                    _inputPorts.Remove(port);
                }
                else
                {
                    _document.RemoveXmlObject(port.XmlRep);
                    _outputPorts.Remove(port);
                }

                Invalidate();
            }

            /// <summary>
            ///  Is called before connecting two ports.
            ///  Override this and return true if the ports can be connected.
            /// </summary>           
            /// <param name="from">
            ///  The port from.
            /// </param>
            /// <param name="to">
            ///  The port to.
            /// </param>
            public override bool CanConnectToPort( Connector from, Connector to )
            {
                if (!base.CanConnectToPort(from, to))
                    return false;

                Port fromPort = (from as Port);
                Port toPort = (to as Port);

                if (fromPort.Type == "Mp.Port.Out" && toPort.Type == "Mp.Port.Out")
                    return false;

                foreach (Port outPort in _outputPorts)
                {
                    if (outPort == from)
                        return false;
                }

                return toPort.AttachedConnectors.Count == 0;
            }

            protected static bool ExistConnectionInXML(Port pfrom, Port pto)
            {
                string linkid = XmlHelper.GetObjectID(pto.XmlRep).ToString();

                foreach (XmlElement xmlElement in pfrom.XmlRep.ChildNodes)
                {
                    if (!xmlElement.HasAttribute("name"))
                        continue;

                    if (xmlElement.Attributes["name"].Value != "refLinkToPort")
                        continue;

                    if (linkid == xmlElement.InnerText)
                    {
                        return true;

                    }
                }

                return false;
            }

            /// <summary>
            ///  Called after the connection of an input port.
            /// </summary>           
            /// <param name="from">
            ///  The port from.
            /// </param>
            /// <param name="to">
            ///  The port to. The process station input port witch is connected.
            /// </param>
            public override void OnPostConnectedConnector(Connector from, Connector to)
            {
                Port portFrom = from as Port;
                Port portTo = to as Port;

                //Generate the xml refLinkToPort tag connect port in xml.
                if(!ExistConnectionInXML(portFrom,portTo))
                    XmlHelper.CreateElement(portFrom.XmlRep, "uint32_t", "refLinkToPort", XmlHelper.GetObjectID(portTo.XmlRep).ToString());

                //Set the signal list to the to port.
                portTo.SignalList = portFrom.SignalList;
            }

            /// <summary>
            ///  Called after the disconnection of an input port.
            /// </summary>           
            /// <param name="from">
            ///  The port from.
            /// </param>
            /// <param name="to">
            ///  The port to. The process station input port witch is disconnected.
            /// </param>
            public override void OnPostDisconnectedConnector(Connector from, Connector to)
            {
                Port      portFrom = from as Port;
                Port      portTo   = to as Port;
                XmlElement  refLinkToPort;

                //Remove the refLinkToPort from the from Port
                //Search all refLinks 
                for(int i = 0; i < portFrom.XmlRep.ChildNodes.Count; i++)
                {
                    refLinkToPort = (portFrom.XmlRep.ChildNodes[i] as XmlElement);
                    
                    if(refLinkToPort == null)
                        continue;

                    //Check for the correct ref link
                    if (refLinkToPort.GetAttribute("name") == "refLinkToPort" && 
                        refLinkToPort.InnerText == (XmlHelper.GetObjectID(portTo.XmlRep).ToString()) )
                    {
                        portFrom.XmlRep.RemoveChild(refLinkToPort);
                    }
                }

                portTo.SignalList = null;
            }

            /// <summary>
            ///  Called before open the port context menu.
            /// </summary>           
            /// <param name="p">
            ///  Position to open the context menu.
            /// </param>
            /// <param name="connector">
            ///  The port for which to open the context menu.
            ///  Can be casted to PoPort.
            /// </param>      
            public override void OnContextMenuConnector(Point p, Connector connector )
            {
                Port port = connector as Port;
                
                if(port.ContextMenuStrip != null)                
                    port.ContextMenuStrip.Show(Site.PointToScreen(p));
            }

            /// <summary>
            ///  Called before open the port properties.
            /// </summary>           
            /// <param name="port">
            ///  The port for which to open to show the properties..
            /// </param>      
            protected virtual void OnPortProperties(Port port)
            { }

            /// <summary>
            ///  Called on port double click.
            /// </summary>           
            /// <param name="port">
            ///  The double clicked port.
            /// </param>      
            public virtual void OnPortDoubleClick(Port port)
            { }

            
            /// <summary>
            /// Called by the framework to validate the process station.
            /// </summary>
            /// <param name="valInfoList">A list to be filled with validation info objects.</param>
            public void Validate(List<ValidationInfo> valInfoList)
            {
                OnValidate(valInfoList);
            }

            /// <summary>
            /// Override this to validate your process station.
            /// </summary>
            /// <param name="valInfoList">A list to be filled with validation info object.</param>
            protected virtual void OnValidate(List<ValidationInfo> valInfoList)
            {
            }

            public virtual void OnLoadResources()
            {
            }

            private void LoadXmlPorts(XmlNodeList ports, bool input)
            {
                Port  port;
                Point   pos;

                if( input )
                    _inputPorts.Clear();
                else
                    _outputPorts.Clear();

                XmlElement xmlPort;

                foreach (XmlNode child in ports)
                {
                    xmlPort = (child as XmlElement);

                    pos = new Point();

                    pos.X = (int)XmlHelper.GetParamNumber(xmlPort,"xPos");
                    pos.Y = (int)XmlHelper.GetParamNumber(xmlPort, "yPos");

                    port = new Port( pos, xmlPort.GetAttribute("subType") , input, !input);
                    port.XmlRep = xmlPort;
                    AddPort(port);
                    port.OnLoadXml();
                }
            }

            private void CreateMenuStrip()
            {
                //Initialize the default context menu.
                ToolStripMenuItem propertyMenuItem = new ToolStripMenuItem(StringResource.MenuProperties);

                propertyMenuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                propertyMenuItem.Click += new System.EventHandler(this.OnProperties);

                _contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { propertyMenuItem });
            }

            private void CreateXmlPort(Port port)
            {
                XmlElement      xmlNewPort;

                //Get the XML representation of the port list id doesn't exist.
                if( port.IsInput )
                {
                    _xmlInputPorts = XmlHelper.GetChildByType(this.XmlRep, "Mp.InputPorts");

                    if (_xmlInputPorts == null)
                        _xmlInputPorts = _document.CreateXmlObject(this.XmlRep, "Mp.InputPorts", "");
                }
                else
                {
                    _xmlOutputPorts = XmlHelper.GetChildByType(this.XmlRep, "Mp.OutputPorts");

                    if (_xmlOutputPorts == null)
                        _xmlOutputPorts = _document.CreateXmlObject(this.XmlRep, "Mp.OutputPorts", "");
                }               

                if (port.IsInput)
                    xmlNewPort = _document.CreateXmlObject(_xmlInputPorts, "Port", port.Type);
                else
                    xmlNewPort = _document.CreateXmlObject(_xmlOutputPorts, "Port", port.Type);

                port.XmlRep = xmlNewPort;
            }

            protected void RecalcStationRect()
            {
                int inputPortPosLen = 0;
                int outputPortPosLen = 0;

                if (OutputPorts.Count > 0)
                {
                    Port lastPort = OutputPorts[OutputPorts.Count - 1];
                    outputPortPosLen = lastPort.Position.Y + DistanceBetweenPort;
                }
                else
                {
                    outputPortPosLen = _rectangle.Top + PortTopOffset;
                }

                if (InputPorts.Count > 0)
                {
                    Port lastPort = InputPorts[InputPorts.Count - 1];
                    inputPortPosLen = lastPort.Position.Y + DistanceBetweenPort;
                }
                else
                {
                    inputPortPosLen = _rectangle.Top + PortTopOffset;
                }

                int newMax = Math.Max(inputPortPosLen, outputPortPosLen);
                _rectangle.Height = Math.Max(newMax - _rectangle.Top, 96);
            }

            private ContextMenuStrip _contextMenuStrip = new ContextMenuStrip();
        }

    }
}
