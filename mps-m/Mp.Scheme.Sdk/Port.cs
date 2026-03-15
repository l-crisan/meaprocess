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
using System.Xml;
using System.Windows.Forms;
using Mp.Visual.Diagram;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// This class implements the process station port.
    /// </summary>
    /// <remarks>
    /// This class is derivated from <see cref="Connector"/> which represent the port in a diagram.
    /// </remarks>
    public class Port : Connector
    {
        private XmlElement       _signalList;
        private ulong            _signalListID = 0;
        private ContextMenuStrip _contextMenuStrip;
        private string           _type;
        private XmlElement       _xmlRep;
        private Document         _document;
        private ProcessStation   _station;

        /// <summary>
        /// Gets or sets the type identifier.
        /// </summary>
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// Gets or sets the xml representation.
        /// </summary>            
        public XmlElement XmlRep
        {
            get { return _xmlRep; }
            set { _xmlRep = value; }
        }
        
        /// <summary>
        /// Gets or sets the document.
        /// </summary>                        
        public Document Document
        {
            get { return _document; }
            set { _document = value; }
        }
       
        /// <summary>
        /// Gets or sets the station.
        /// </summary>                                    
        public ProcessStation Station
        {  
            get { return _station; }
            set { _station = value; }
        }
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        public Port() 
        { }

        /// <summary>
        /// Construct a port with the given properties.
        /// </summary>
        /// <param name="pos">The port position in diagram.</param>
        /// <param name="type">The port type identifier.</param>
        /// <param name="isInput">Define an input/output port.</param>
        public Port(Point pos, string type, bool isInput, bool toolTipAvail)
        : base(pos)
        {
            base.IsInput = isInput;
            _type = type;

            if (toolTipAvail)
                this.OnGetConnectionToolTipText += new GetToolTipText(GetConnectionToolTipText);
        }

        /// <summary>
        /// Called when the port is saved to an xml document.
        /// </summary>
        public virtual void OnSaveXml()
        {             
            //Create the signal list reference.
            if( _signalList != null )
            {// signal list is set => save the link.
                _signalListID = XmlHelper.GetObjectID(_signalList);
                XmlHelper.SetParamNumber(XmlRep, "refSignalList", "uint32_t", (int)_signalListID);
            }
            else if( _signalListID == 0 )
            {// signal list is removed => remove the link.
                XmlElement element;
                XmlAttribute attr;
                foreach (XmlNode node in XmlRep.ChildNodes)
                {
                    element = (node as XmlElement);
                    
                    if(element == null)
                        continue;

                    attr = element.Attributes["name"];
                    if (attr == null)
                        continue;

                    if (attr.Value != "refSignalList")
                        continue;

                    XmlRep.RemoveChild(element);
                    break;                    
                }

            }
          
            XmlHelper.SetParamNumber(XmlRep, "xPos", "int32_t", base.Position.X);
            XmlHelper.SetParamNumber(XmlRep, "yPos", "int32_t", base.Position.Y);
            XmlHelper.SetParamNumber(XmlRep, "rColor", "uint8_t", base.ConnectorBrush.Color.R);
            XmlHelper.SetParamNumber(XmlRep, "gColor", "uint8_t", base.ConnectorBrush.Color.G);
            XmlHelper.SetParamNumber(XmlRep, "bColor", "uint8_t", base.ConnectorBrush.Color.B);
        }

        ///<summary>
        ///  Called to initialize the port with xml parameter.
        /// </summary>
        public virtual void OnLoadXml()
        {
            byte R =    (byte) XmlHelper.GetParamNumber(XmlRep, "rColor");
            byte G =    (byte) XmlHelper.GetParamNumber(XmlRep, "gColor");
            byte B =    (byte) XmlHelper.GetParamNumber(XmlRep, "bColor");
 
            _signalListID = (ulong)XmlHelper.GetParamNumber(XmlRep, "refSignalList");

            base.ConnectorBrush = new SolidBrush(Color.FromArgb(R, G, B));
        }

        /// <summary>
        ///  Called by mouse double clicked on the port.
        /// </summary>       
        public override void OnMouseDoubleClick(Point p)
        { Station.OnPortDoubleClick(this); }

        /// <summary>
        ///  Gets or sets the signal list.
        /// </summary>                   
        public XmlElement SignalList
        {
            get
            {
                if (_signalList == null && XmlRep != null)
                {
                    uint id = (uint)XmlHelper.GetParamNumber(XmlRep, "refSignalList");
                    _signalList = _document.GetXmlObjectById(id);
                }
                
                return _signalList;                 
            }

            set
            { 
                _signalList = value;

                if (_signalList == null && XmlRep != null)
                {
                    XmlElement xmlRefSigList = XmlHelper.GetChildByName(XmlRep, "refSignalList");

                    if (xmlRefSigList != null)
                        XmlRep.RemoveChild(xmlRefSigList);

                    _signalListID = 0;
                }
            }
        }

        public string[] GetConnectionToolTipText()
        {
            if (SignalList == null)
                return null;

            string[] text = new string[2];

            switch (SignalList.ChildNodes.Count)
            {
                case 0:
                    text[0] = SignalList.ChildNodes.Count.ToString() + " " + StringResource.Signals;
                    break;

                case 1:
                    text[0] = SignalList.ChildNodes.Count.ToString() + " " + StringResource.Signal;
                    break;

                default:
                    text[0] = SignalList.ChildNodes.Count.ToString() + " " + StringResource.Signals;
                    break;
            }

            string toolTipText ="";
            int i = 0;
            foreach (XmlElement xmlElement in SignalList)
            {
                XmlElement xmlSignal = xmlElement;
                
                uint sigId = XmlHelper.GetObjectID(xmlSignal);

                if (sigId == 0)
                {
                    sigId = Convert.ToUInt32(xmlElement.InnerText);
                    xmlSignal = _document.GetXmlObjectById(sigId);
                }

                toolTipText += XmlHelper.GetParam(xmlSignal, "name");
                toolTipText += "\r\n";
                ++i;

                if (i == 20)
                {
                    toolTipText += "...";
                    break;
                }
            }

            if (toolTipText == "")
                toolTipText = "no signals";

            text[1] = toolTipText;
            return text;
        }

        /// <summary>
        ///  Gets or sets the context menu strip.
        /// </summary>     
        public ContextMenuStrip ContextMenuStrip
        {
            get { return _contextMenuStrip; }
            set { _contextMenuStrip = value; }
        }
        
    }
}