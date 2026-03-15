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
using System.Collections;
using System.IO;
using System.Windows.Forms;
using Mp.Visual.Docking;
using Mp.Visual.Diagram;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{  

    public delegate void DocumentStateDelegate(bool modified);
    public delegate string GetPasswordDelegate(string file);
    public delegate bool MessageDelegate(ValidationInfo info);

    public class Document
    {
        private XmlDocument _xmlDoc;
        private Module _runtimeEngine;
        private List<DiagramWindow> _diagrams = new List<DiagramWindow>();        
        private uint _maxId = 100;
        private int _maxSource = 0;
        private static string _emptyDoc = "EmptyDoc.xcr";
        private Hashtable _xmlObjectIDMapping = new Hashtable();
        private Hashtable _portIdMapping = new Hashtable();
        private bool _isModify = false;
        private ModuleManager _moduleManager;
        private string _fileName;
        private DockPanel _dockPanel;
        private uint _maxSchemeID;
        private string _runtimeConfig = "";
        private static Icon _appIcon;
        private bool _isValid;
        public event MessageDelegate OnOutputMessage;
        public event DocumentStateDelegate OnDocumentStateChangedEvent;
        public event GetPasswordDelegate OnGetPasswordDelegate;
        private Hashtable _sharedObject = new Hashtable();
        private Dictionary<string,List<string>> _resources =  new Dictionary<string,List<string>>();
        readonly string _version = "1.0";

        public Document(DockPanel dockPanel, ModuleManager manager)
        {
            _dockPanel = dockPanel;
            InsertDiagram(new DiagramWindow());

            _moduleManager = manager;
            
            _xmlDoc = new XmlDocument();            
        }

        public Document(DockPanel dockPanel, Module engine, ModuleManager manager)
        {
            _dockPanel = dockPanel;
            _runtimeEngine = engine;
            _moduleManager = manager;            
            _xmlDoc = new XmlDocument();
            InsertDiagram(new DiagramWindow());
        }


        public static Icon AppIcon
        {
            get { return _appIcon; }
            set { _appIcon = value; }
        }


        public ModuleManager ModManager
        {
            get { return _moduleManager; }
        }


        public void AddResource(string identifier, List<string> resources)
        {
            if (_resources.ContainsKey(identifier))
                _resources[identifier] = resources;
            else
                _resources.Add(identifier, resources);
        }


        public List<string> GetResource(string identifier)
        {
            if(_resources.ContainsKey(identifier))
                return _resources[identifier];

            return null;
        }


        public XmlDocument GetRuntimeDocument()
        {
            XmlDocument newDoc = XmlDoc.Clone() as XmlDocument;

            //Remove the gui parts from the clone.
            XmlNode guiNode = XmlHelper.GetChildByType(newDoc.DocumentElement, "GUI");
            newDoc.DocumentElement.RemoveChild(guiNode);

            XmlNode conNode = XmlHelper.GetChildByType(newDoc.DocumentElement, "Connections");
            newDoc.DocumentElement.RemoveChild(conNode);
            XmlHelper.SetParam(newDoc.DocumentElement, "schemeState", "string", "");

            XmlElement xmlResources = XmlHelper.GetChildByType(newDoc.DocumentElement, "Resources");
            if (xmlResources != null)
                newDoc.DocumentElement.RemoveChild(xmlResources);

            return newDoc;
        }


        public void AddSharedObject(string key, object obj)
        {
           _sharedObject[key] = obj;
        }


        public object GetSharedObject(string key)
        {
            if (_sharedObject.ContainsKey(key))
                return _sharedObject[key];

            return null;
        }


        public DockPanel DockPanel
        {
            set{ _dockPanel = value; }
        }


        public string RuntimeConfiguration
        {
            get { return _runtimeConfig; }
            set 
            {
                Modified = true;
                _runtimeConfig = value; 
            }
        }


        public int GridInterval
        {
            get { return _diagrams[0].Diagram.GridInterval; }
            set
            {
                foreach (DiagramWindow w in _diagrams)
                {
                    w.Diagram.GridInterval = value;
                }
            }
        }


        public XmlElement GetSignalElement(XmlElement xmlSignal)
        {
            XmlElement xmlSigList = (XmlElement)xmlSignal.ParentNode;
            uint listObjID = XmlHelper.GetObjectID(xmlSigList);
            XmlElement xmlPSList = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Mp.PS.List");

            foreach (XmlElement xmlPS in xmlPSList.ChildNodes)
            {
                if (xmlPS.GetAttribute("type") == "Mp.PS.Splitter")
                    continue;

                XmlElement xmlOutPorts = XmlHelper.GetChildByType(xmlPS, "Mp.OutputPorts");

                if (xmlOutPorts == null)
                    continue;

                foreach (XmlElement xmlPort in xmlOutPorts.ChildNodes)
                {
                    uint curListID = (uint)XmlHelper.GetParamNumber(xmlPort, "refSignalList");

                    if (curListID == listObjID)
                        return xmlPS;
                }
            }

            return (XmlElement)xmlPSList.ChildNodes[0];
        }


        public bool SnapToGrid
        {
            get { return _diagrams[0].Diagram.SnapToGrid; }
            set
            {
                foreach (DiagramWindow w in _diagrams)
                {
                    w.Diagram.SnapToGrid = value;
                }
            }
        }


        public void Close()
        {
            _portIdMapping.Clear();
            ClearDiagrams();
        }


        private void OnDiagramDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(String)))
            {
                e.Effect = DragDropEffects.None;
                DiagramCtrl digram = sender as DiagramCtrl;

                if (sender == null)
                    return;                

                string classType = (string)e.Data.GetData(typeof(String));

                Point pos = digram.PointToClient(new Point(e.X, e.Y));

                try
                {
                    CreateStationByClassType(classType, pos, digram);
                }
                catch (Exception ex)
                {

                    ValidationInfo vi = new ValidationInfo(ex.Message, ValidationInfo.InfoType.Warning);
                    OutputMessage(vi);
                    return;
                }
                
                foreach(DiagramWindow d in _diagrams)
                {
                    if (d.Diagram == digram)
                    {
                        d.Activate();
                        break;
                    }
                }
            }
        }


        public XmlElement GetSignal(XmlElement xmlSignalOrRef)
        {
            XmlAttribute attr = xmlSignalOrRef.Attributes["name"];

            if (attr != null)
            {
                if (attr.Value == "signalRef")
                {
                    uint id = Convert.ToUInt32(xmlSignalOrRef.InnerText);
                    xmlSignalOrRef = GetXmlObjectById(id);
                }
            }

            return xmlSignalOrRef;
        }


        public bool IsVideoSignal(XmlElement xmlSigOrRef)
        {
            xmlSigOrRef = GetSignal(xmlSigOrRef);

            if(!xmlSigOrRef.HasAttribute("subType"))
                return false;

            if(xmlSigOrRef.Attributes["subType"].InnerText.Contains("Mp.Sig.Video"))
                return true;

            return XmlHelper.GetParam(xmlSigOrRef, "cat") == "Mp.Sig.Video";

        }


        private void OnDiagramDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(String)))
                e.Effect = DragDropEffects.Move;
        }


        public static string RegistryKey
        {
            get { return "Atesion\\MeaProcess\\Scheme\\"; }
        }


        public static string HelpFile
        {
            //StringResource.HelpFileName; 
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, StringResource.HelpFileName); }
        }


        public void New()
        {
            string strPath = System.Windows.Forms.Application.StartupPath;
            strPath += "\\" + _emptyDoc;
            _xmlDoc.Load(strPath);
            TimerResolution = 1;
            _xmlObjectIDMapping.Clear();
            UpdateXmlObjectIdMapping(_xmlDoc.DocumentElement);
            InstanceDocument();

            XmlHelper.SetParam(_xmlDoc.DocumentElement, "type", "string", _runtimeEngine.Type);
            XmlHelper.SetParam(_xmlDoc.DocumentElement, "name", "string", _runtimeEngine.Identifier);
            XmlHelper.SetParam(_xmlDoc.DocumentElement, "version", "string", _version);            
            ClearUndoStack();
            _fileName = "";
            Modified = true;
        }


        public uint TimerResolution
        {
            get { return (uint) (XmlHelper.GetParamNumber(_xmlDoc.DocumentElement, "timerResolution")/1000000); }
            set { XmlHelper.SetParamNumber(_xmlDoc.DocumentElement, "timerResolution", "uint64_t", value * 1000000); }
        }


        private void ClearDiagrams()
        {
            _maxSchemeID = 0;

            for (int i = 0; i < _diagrams.Count; ++i)
            {
                _diagrams[i].Diagram.Clear();
                _diagrams[i].Diagram.ClearUndoStack();
                _diagrams[i].CloseForced();
            }
        }


        public bool Load(Stream data, string file, bool checkPassword)
        {
            _fileName = file;

            //Load the new document
            try
            {
                _xmlDoc.RemoveAll();
                _xmlDoc.Load(data);
                _xmlDoc = ToCurrentVersion(_xmlDoc);
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(StringResource.UnknownDocType);
            }

            _maxId = (uint)XmlHelper.GetParamNumber(_xmlDoc.DocumentElement, "docMaxID");
            _maxSource = (int) XmlHelper.GetParamNumber(_xmlDoc.DocumentElement, "dataSourceID");
            _runtimeEngine = _moduleManager.GetEngineByType(XmlHelper.GetParam(_xmlDoc.DocumentElement, "type"));
            _runtimeConfig = XmlHelper.GetParam(_xmlDoc.DocumentElement, "runtimeConfig");

            if (_runtimeEngine == null)
                throw new Exception(StringResource.RtTypeUnknown);

            if (XmlHelper.GetParamNumber(_xmlDoc.DocumentElement, "useSchemePassword") != 0 && checkPassword)
            {
                if (OnGetPasswordDelegate != null)
                {
                    string userPassword = OnGetPasswordDelegate(_fileName);
                    if(XmlHelper.GetParam(_xmlDoc.DocumentElement, "schemePassword") != userPassword)
                        throw new Exception(StringResource.InvalidPassword);

                }
                else
                {
                    throw new Exception(StringResource.InvalidPassword);
                }
            }

            _portIdMapping.Clear();
            _xmlObjectIDMapping.Clear();
            UpdateXmlObjectIdMapping(_xmlDoc.DocumentElement);

            //Instancing the new document
            bool ret =  InstanceDocument();            
            _maxSchemeID = 0;

            foreach (DiagramWindow d in _diagrams)
                _maxSchemeID = Math.Max(d.Diagram.ID, _maxSchemeID);

            CleanSources();
            ClearUndoStack();

            LoadDocResources();
            Modified = false;
            return ret;
        }


        private XmlDocument ToCurrentVersion(XmlDocument doc)
        {
            return doc;
        }


        public void OutputMessage(ValidationInfo info)
        {
            if (OnOutputMessage != null)
                OnOutputMessage(info);
        }


        public bool Save(Stream data, string file)
        {
            _fileName = file;
            ProcessStation station;
            Hashtable runtimeModules = new Hashtable();

            foreach(DiagramWindow diagram in _diagrams)
            {
                foreach (Shape shape in diagram.Diagram.Shapes)
                {
                    station = (shape as ProcessStation);
                    
                    if (station == null)
                        continue;

                    string rmodule = station.RuntimeModule;
                    if (rmodule != "")
                        if (!runtimeModules.Contains(rmodule))
                            runtimeModules[rmodule] = true;

                    XmlHelper.SetParamNumber(station.XmlRep, "diagram", "int32_t", diagram.Diagram.ID);
                    station.OnSaveXml();
                }
            }

            SaveConnections();

            StringBuilder sb = new StringBuilder();
            foreach (DictionaryEntry entry in runtimeModules)
            {
                string rmodule = (string) entry.Key;
                sb.Append(rmodule);
                sb.Append(";");
            }

            string modules = sb.ToString().TrimEnd(';');
            XmlHelper.SetParamNumber(_xmlDoc.DocumentElement, "valid", "bool", _isValid ? 1 : 0);
            XmlHelper.SetParam(_xmlDoc.DocumentElement, "runtimeModules", "string", modules);

            XmlHelper.SetParamNumber(_xmlDoc.DocumentElement, "docMaxID", "uint32_t", (int)GetNextFreeID());
            XmlHelper.SetParam(_xmlDoc.DocumentElement, "runtimeConfig", "string", _runtimeConfig);

            long resolution = XmlHelper.GetParamNumber(_xmlDoc.DocumentElement, "timerResolution");

            if( resolution == 0)
                XmlHelper.SetParamNumber(_xmlDoc.DocumentElement, "timerResolution","uint64_t", 1000000);

            //Save resources
            SaveDocResources();

            try
            {
                _xmlDoc.Save(data);
            }
            catch (XmlException e)
            {
                e.ToString();
                return false;
            }
            Modified = false;

            return true;
        }


        private void LoadDocResources()
        {
            _resources.Clear();
            XmlElement xmlResources = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Resources");

            if (xmlResources == null)
                return;

            foreach(XmlElement xmlResource in xmlResources.ChildNodes)
            {
                string key =  XmlHelper.GetParam(xmlResource, "name");
                string value = XmlHelper.GetParam(xmlResource,"value");
                string[] valuesArray = value.Split(';');

                List<string> valueList = new List<string>(valuesArray);
                try
                {
                    _resources.Add(key, valueList);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        private void SaveDocResources()
        {
            XmlElement xmlResources = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Resources");

            if (xmlResources != null)
                this.RemoveXmlObject(xmlResources);

            if (_resources.Count != 0)
            {
                xmlResources = CreateXmlObject(_xmlDoc.DocumentElement, "Resources", "");

                foreach (KeyValuePair<string, List<string>> entry in _resources)
                {
                    XmlElement xmlResource = CreateXmlObject(xmlResources, "Resource", "");
                    XmlHelper.SetParam(xmlResource, "name", "string", entry.Key);
                    string value = "";

                    foreach (string v in entry.Value)
                        value += v + ";";

                    value.TrimEnd(';');

                    XmlHelper.SetParam(xmlResource, "value", "string", value);
                }
            }
        }


        public bool Modified
        {
            set 
            {
                _isModify = value;

                if (OnDocumentStateChangedEvent != null)
                     OnDocumentStateChangedEvent(value);                
            }

            get  { return _isModify; }
        }


        public ProcessStation CreateStationByClassType(string classType, Point pos, DiagramCtrl diagram)
        {
            ProcessStation station = _runtimeEngine.GetStationByClassType(classType);

            if( station == null )
                throw new Exception(StringResource.PsUndefined);

            if( station.IsSingleton && IsPsTypeAvailable(station.Type, station.SubType) )
            {
                string str = String.Format(StringResource.PsInstanceIsAviable,station.Text);
                throw new Exception(str);
            }

            XmlElement      xmlPSList;
            XmlElement      xmlRepOfPS;
            ProcessStation  theNewPS;

            xmlPSList = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Mp.PS.List");
            
            //Create the XML-PS node.
            xmlRepOfPS = CreateXmlObject(xmlPSList, station.Type, station.SubType);

            //Create the object for this PS.
            theNewPS = (ProcessStation) Activator.CreateInstance(station.GetType(), null);
            theNewPS.Init(this,diagram,xmlRepOfPS,pos);

            //Initialize the PS-object.
            theNewPS.OnDefaultInit();

            //The document is modified.
            Modified = true;
            diagram.Invalidate();
            return theNewPS;
        }


        public XmlElement CreateSignalList()
        {
            XmlElement xmlSignalListList = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Mp.Signals");
            XmlElement xmlSignalList = CreateXmlObject(xmlSignalListList, "Mp.SignalList", "");
            Modified = true;
            return xmlSignalList;
        }


        public XmlElement CreateSignalList(uint id)
        {
            XmlElement xmlSignalListList = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Mp.Signals");
            XmlElement xmlSignalList = XmlHelper.CreateObject(xmlSignalListList, "Mp.SignalList", "", id);
            _xmlObjectIDMapping[id] = xmlSignalList;
            Modified = true;
            return xmlSignalList;

        }


        public void DeleteObject()
        {
            DiagramWindow diagram = (DiagramWindow)_diagrams[0].DockPanel.ActiveContent;
            
            if (diagram == null)
                return;

            diagram.Diagram.DeleteObject();

            Modified = true;
        }


        private uint GetNextFreeID()
        {
            _maxId++;
            return _maxId;
        }


        public string File
        {
            get { return _fileName; }
            set { _fileName = value; }
        }


        public uint GetSourceIdByUID(string uid)
        {
            XmlElement xmlSources = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Mp.Sources");

            if (xmlSources == null)
                return 0;

            foreach (XmlElement xmlSource in xmlSources.ChildNodes)
            {
                if (XmlHelper.GetParam(xmlSource, "uid") == uid)
                    return XmlHelper.GetObjectID(xmlSource);
            }
            
            return 0;
        }


        private bool IsSourceReferenced(uint srcId)
        {
            XmlElement xmlSignals = XmlHelper.GetChildByType(XmlDoc.DocumentElement, "Mp.Signals");
            foreach (XmlElement xmlSignalList in xmlSignals.ChildNodes)
            {
                foreach (XmlElement xmlSignal in xmlSignalList.ChildNodes)
                {
                    uint curSrcId = (uint)XmlHelper.GetParamNumber(xmlSignal, "sourceNumber");

                    if (curSrcId == srcId)
                        return true;
                }
            }
            return false;
        }


        public void CleanSources()
        {
            XmlElement xmlSources = XmlHelper.GetChildByType(XmlDoc.DocumentElement, "Mp.Sources");

            if (xmlSources == null)
                return;

            for (int i = 0; i < xmlSources.ChildNodes.Count; ++i)
            {
                XmlElement xmlSource = xmlSources.ChildNodes[i] as XmlElement;

                if (xmlSource == null)
                    continue;

                uint srcId = XmlHelper.GetObjectID(xmlSource);

                if (IsSourceReferenced(srcId))
                    continue;

                RemoveXmlObject(xmlSource);
                --i;
            }
        }


        public uint RegisterSource(string sourceName, long sourceKey, string uid)
        {
            XmlElement xmlSources = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Mp.Sources");
            
            if (xmlSources == null)
                xmlSources = this.CreateXmlObject(_xmlDoc.DocumentElement, "Mp.Sources", "");

            XmlElement xmlSource = this.CreateXmlObject(xmlSources, "Mp.Source", "");

            XmlHelper.SetParam(xmlSource, "name", "string", sourceName);
            XmlHelper.SetParamNumber(xmlSource, "key", "uint64_t", sourceKey);
            XmlHelper.SetParam(xmlSource, "uid", "string", uid);            

            return (uint) XmlHelper.GetObjectID(xmlSource);
        }


        public void UnregisterSource(uint sourceId)
        {
            this.RemoveXmlObject(sourceId);
        }


        public XmlElement CreateXmlObject(XmlElement parentObject, string type, string subType)
        {
            uint id = GetNextFreeID();
            XmlElement xmlObject =  XmlHelper.CreateObject(parentObject, type, subType, id);
            _xmlObjectIDMapping[id] = xmlObject;
            return xmlObject;
        }


        public void RemoveXmlObject(XmlElement xmlObject)
        {
            RemoveXmlObjRecursiv(xmlObject);
        }
        

        public void RegPort(Port port)
        {
            ulong id = XmlHelper.GetObjectID(port.XmlRep);
            _portIdMapping[id] = port;
        }

        
        public void UnregPort(Port port)
        {
            ulong id = XmlHelper.GetObjectID(port.XmlRep);
            _portIdMapping.Remove(id);
        }


        private void RemoveXmlObjRecursiv(XmlElement parent)
        {
            for( int i = 0; i < parent.ChildNodes.Count; ++i)
            {
                XmlElement child = parent.ChildNodes[i] as XmlElement;
                
                if (child == null)
                    continue;
                
                if (child.Name != "object")
                    continue;

                RemoveXmlObjRecursiv(child);
                i--;
            }

            if (parent.ParentNode == null)
                return;

            XmlElement el = (XmlElement) parent.ParentNode;
            uint id = XmlHelper.GetObjectID(parent);
            el.RemoveChild(parent);
            _xmlObjectIDMapping.Remove(id);
        }


        public void RemoveXmlObject(uint objectId)
        {
            if (!_xmlObjectIDMapping.ContainsKey(objectId))
                return;

            XmlElement xmlObject = (XmlElement) _xmlObjectIDMapping[objectId];            
            RemoveXmlObject(xmlObject);
        }


        public void AppendXmlObject(XmlElement xmlParentObj, XmlElement xmlObject)
        {
            uint id = XmlHelper.GetObjectID(xmlObject);            
            xmlParentObj.AppendChild(xmlObject);
            UpdateXmlObjectIdMapping(xmlObject);
        }


        private bool InstanceDocument()
        {
            bool bRetVal = true;
            XmlElement element;
            string atrType;
            foreach (XmlNode child in _xmlDoc.DocumentElement.ChildNodes)
            {
                element = (child as XmlElement);
                
                if (element == null)
                    continue;

                if (element.Name != "object")
                    continue;

                atrType = element.GetAttribute("type");
                if (atrType == "Mp.PS.List")
                {
                    bRetVal = InstancePSList(element);
                }
                else if (atrType == "Connections")
                {
                    bRetVal = InstanceConnectios(element);
                }
                if (!bRetVal)
                    return false;
            }

            foreach (DiagramWindow diagram in _diagrams)
            {
                diagram.Diagram.RecalcScrollSize();
                diagram.Diagram.Invalidate();
            }

            _diagrams[0].Activate();
            _diagrams[0].MainScheme = true;

            return true;
        }

        private bool InstancePSList(XmlElement list)
        {
            XmlElement          xmlStation;
            ProcessStation    station;

            foreach (XmlNode child in list.ChildNodes)
            {
                xmlStation = (child as XmlElement);

                station = _runtimeEngine.CreateStation(this, xmlStation.GetAttribute("type"), xmlStation);

                if (station == null)
                {
                    string statioName = XmlHelper.GetParam(xmlStation, "name");

                    ClearDiagrams();
                    _xmlDoc.RemoveAll();
                    _portIdMapping.Clear();
                    
                    throw new Exception(String.Format(StringResource.PsLoadErr, statioName));
                }
                
                station.OnLoadXml();                
            }
            return true;
        }


        private DiagramCtrl GetDiagramByID(int id)
        {
            foreach (DiagramWindow diagram in _diagrams)
            {
                if (diagram.Diagram.ID == id)
                    return diagram.Diagram;
            }

            return null;
        }


        private bool InstanceConnectios(XmlElement xmlConnections)
        {
            Port fromPort;
            Port toPort;
            string strId;
            ulong id;
            Connection connection;
            XmlElement xmlConnection;
            XmlElement xmlPos;
            Point pos = new Point(0, 0);

            foreach (XmlNode child in xmlConnections.ChildNodes)
            {
                xmlConnection = (child as XmlElement);

                int diagramID  = (int) XmlHelper.GetParamNumber(xmlConnection, "diagram");
                DiagramCtrl diagram = null;
                
                if (diagramID != 0)
                    diagram = GetDiagramByID(diagramID);
                else
                    diagram = Diagrams[0].Diagram;

                //From port.
                strId = XmlHelper.GetParam(xmlConnection, "idFrom");
                id = Convert.ToUInt64(strId);

                fromPort = (_portIdMapping[id] as Port);

                //To port.
                strId = XmlHelper.GetParam(xmlConnection, "idTo");
                id = Convert.ToUInt64(strId);

                toPort = (_portIdMapping[id] as Port);

                //Add the connection in the graph.
                connection = diagram.AddConnection(fromPort, toPort);
                connection.Points.Clear();

                foreach( XmlNode xmlPosNode in xmlConnection.ChildNodes)
                {
                    xmlPos = (xmlPosNode as XmlElement);

                    if (xmlPos.GetAttribute("name") == "pX")
                    {
                        pos = new Point();
                        pos.X = Convert.ToInt32(xmlPos.InnerText);
                    }
                    else if (xmlPos.GetAttribute("name") == "pY")
                    {
                        pos.Y = Convert.ToInt32(xmlPos.InnerText);
                        connection.Points.Add(pos);
                    }
                }
            }
            _diagrams[0].Invalidate();
            return true;
        }


        public List<DiagramWindow> Diagrams
        {
            get 
            { 
                return _diagrams; 
            }
        }


        public DiagramWindow MainDiagram
        {
            get
            {
                if (_diagrams.Count > 0)
                    return _diagrams[0];

                return null;
            }
        }


        public DiagramWindow ActiveDiagram
        {
            get
            {
                return (DiagramWindow)_dockPanel.ActiveDocument;
            }
        }


        private void SaveConnections()
        {
            Port port;
            XmlElement xmlConnection;
            XmlElement xmlConnections;
            xmlConnections = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Connections");

            //Remove all child.
            ulong id = XmlHelper.GetObjectID(xmlConnections);
            for (int i = 0; i < xmlConnections.ChildNodes.Count; i++)
            {
                XmlElement child = xmlConnections.ChildNodes[i] as XmlElement;
                
                if (child == null)
                    continue;

                RemoveXmlObject(child);
                i--;
            }

            //Append the connections.
            foreach(DiagramWindow diagramWindow in _diagrams)
            {
                DiagramCtrl diagram = diagramWindow.Diagram;

                foreach (Connection connection in diagram.Connections)
                {
                    //Create the Xml-Connection object.
                    xmlConnection = CreateXmlObject(xmlConnections, "Connection", "");

                    XmlHelper.SetParamNumber(xmlConnection, "diagram", "uint32_t", diagram.ID);

                    //Set the from/to reference.
                    port = (connection.From.AttachedTo as Port);
                    XmlHelper.SetParamNumber(xmlConnection, "idFrom", "uint32_t", (int)XmlHelper.GetObjectID(port.XmlRep));

                    port = (connection.To.AttachedTo as Port);
                    XmlHelper.SetParamNumber(xmlConnection, "idTo", "uint32_t", (int)XmlHelper.GetObjectID(port.XmlRep));

                    //Save the split points. 
                    foreach (Point point in connection.Points)
                    {
                        XmlHelper.CreateElement(xmlConnection, "int32_t", "pX", point.X.ToString());
                        XmlHelper.CreateElement(xmlConnection, "int32_t", "pY", point.Y.ToString());
                    }
                }
            }
        }


        public List<ValidationInfo> Validate()
        {
            List<ValidationInfo> ls = new List<ValidationInfo>();

            foreach (DiagramWindow diagram in _diagrams)
            {
                for( int i = 0; i < diagram.Diagram.Shapes.Count; ++i)
                {
                    ProcessStation ps = diagram.Diagram.Shapes[i] as ProcessStation;
                    if( ps != null)
                        ps.Validate(ls);
                }
            }

            if (IsEmpty)
                ls.Add(new ValidationInfo(StringResource.EmptySchemeErr, ValidationInfo.InfoType.Error));

            _isValid = ls.Count == 0;

            return ls;
        }


        public bool IsValid
        {
            get
            {
                return _isValid;
            }
        }


        public bool IsEmpty
        {
            get
            {
                DiagramCtrl diagram = _diagrams[0].Diagram;
                return diagram.Shapes.Count == 0; 
            }
        }


        public XmlElement GetXmlObjectById(uint id)
        {
            return (XmlElement) _xmlObjectIDMapping[id];
        }


        private bool IsPsTypeAvailable(string type, string subType)
        {
            ProcessStation station;
            DiagramCtrl diagram = _diagrams[0].Diagram;

            foreach (Shape shape in diagram.Shapes)
            {
                station = (shape as ProcessStation);

                if (station.Type == type && station.SubType == subType)
                    return true;
            }

            return false;
        }


        public Module RuntimeEngine
        {
            get { return _runtimeEngine; }
        }


        public XmlDocument XmlDoc
        {
            get { return _xmlDoc; }
        }

        
        public void InsertDiagram(DiagramWindow diagram)
        {
            diagram.Diagram.DragOver += new DragEventHandler(OnDiagramDragOver);
            diagram.Diagram.DragDrop += new DragEventHandler(OnDiagramDragDrop);
            diagram.Diagram.ModifiedEvent += new ModifiedDelegate(OnDiagramModifiedEvent);
            diagram.Diagram.OnCopy += new CopyDelegate(OnDiagramCopy);
            diagram.Diagram.OnPaste += new PasteDelegate(OnDiagramPaste);

            if (_diagrams.Count > 0)
            {
                diagram.Diagram.GridInterval = MainDiagram.Diagram.GridInterval;
                diagram.Diagram.SnapToGrid = MainDiagram.Diagram.SnapToGrid;
            }
            diagram.Diagram.ClearUndoStack();
            diagram.Show(_dockPanel);                
            diagram.DockState = DockState.Document;
            _diagrams.Add(diagram);
            _maxSchemeID++;
            diagram.Diagram.ID = _maxSchemeID;
        }


        private void OnDiagramPaste(DiagramCtrl diagram, Point pos)
        {
            try
            {
                string xmlData = (string)Clipboard.GetData("MeaProcess.ProcessStations");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlData);

                foreach (XmlElement xmlStation in xmlDoc.DocumentElement)
                {
                    string stationData = "";
                    XmlElement xmlVisual = null;

                    if (xmlStation.Name == "Station")
                    {
                        stationData = xmlStation.InnerXml;
                    }
                    else if( xmlStation.Name == "VisualStation")
                    {
                        stationData = xmlStation["Station"].InnerXml;
                        xmlVisual = xmlStation["Visual"];                        
                    }

                    XmlElement xmlPSList = XmlHelper.GetChildByType(_xmlDoc.DocumentElement,"Mp.PS.List");

                    XmlDocument xmlStationDoc = new XmlDocument();
                    xmlStationDoc.LoadXml(stationData);

                    UpdateXmlObjIds(xmlStationDoc.DocumentElement);

                    uint id = XmlHelper.GetObjectID(xmlStationDoc.DocumentElement);
                    XmlNode xmlNewNode  = _xmlDoc.ImportNode(xmlStationDoc.DocumentElement, true);
                    xmlPSList.AppendChild(xmlNewNode);
                    UpdateXmlObjectIdMapping(xmlPSList);

                    XmlElement xmlNewPS = GetXmlObjectById(id);

                    XmlElement xmlOutPorts = XmlHelper.GetChildByType(xmlNewPS, "Mp.OutputPorts");

                    if (xmlOutPorts != null)
                    {
                        foreach (XmlElement xmlPort in xmlOutPorts.ChildNodes)
                        {
                            XmlElement xmlNewSigList = CreateSignalList();
                            uint sigListId = XmlHelper.GetObjectID(xmlNewSigList);
                            XmlHelper.SetParamNumber(xmlPort, "refSignalList", "uint32_t", sigListId);
                            XmlHelper.SetParamNumber(xmlPort, "refLinkToPort", "uint32_t", 0);
                        }
                    }

                    XmlElement xmlInPorts = XmlHelper.GetChildByType(xmlNewPS, "Mp.InputPorts");

                    if (xmlInPorts != null)
                    {
                        foreach (XmlElement xmlPort in xmlInPorts.ChildNodes)
                        {
                            XmlHelper.SetParamNumber(xmlPort, "refSignalList", "uint32_t", 0);
                            XmlHelper.SetParamNumber(xmlPort, "refLinkToPort", "uint32_t", 0);
                        }
                    }

                    ProcessStation ps = _runtimeEngine.CreateStation(this, diagram, xmlNewPS.GetAttribute("type"), xmlNewPS);

                    ps.OnLoadXml();

                    if (diagram.ClientRectangle.Contains(pos))
                        ps.Move(new Point(50,50));

                    ps.Site.Invalidate();                                        
                    
                    if( xmlVisual != null)
                        ps.LoadVisualData(xmlVisual);                    
                }

                Modified = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void UpdateXmlObjIds(XmlElement xmlElement)
        {
            xmlElement.SetAttribute("id", GetNextFreeID().ToString());

            foreach (XmlElement xmlChild in xmlElement.ChildNodes)
            {
                if (XmlHelper.GetObjectID(xmlChild) == 0)
                    continue;

                UpdateXmlObjIds(xmlChild);
            }
        }


        public static void CopySignalParam(XmlElement xmlFromSignal, XmlElement xmlToSignal)
        {
            foreach (XmlNode xmlNode in xmlFromSignal.ChildNodes)
            {
                if( !(xmlNode is XmlElement))
                    continue;

                XmlElement sigElement = (XmlElement)xmlNode;

                XmlAttribute nameAtr =  sigElement.Attributes["name"];
                
                if (nameAtr == null)
                    continue;

                string type = sigElement.Name;
                XmlHelper.SetParam(xmlToSignal, nameAtr.Value.ToString(), type, sigElement.InnerText);                
            }
        }


        public void CopySignalBaseParam(XmlElement xmlFromSignal, XmlElement xmlToSignal)
        {
            XmlHelper.SetParam(xmlToSignal, "unit", "string", XmlHelper.GetParam(xmlFromSignal, "unit"));
            XmlHelper.SetParamNumber(xmlToSignal, "valueDataType", "uint8_t", XmlHelper.GetParamNumber(xmlFromSignal, "valueDataType"));
            XmlHelper.SetParamDouble(xmlToSignal, "physMin", "double", XmlHelper.GetParamDouble(xmlFromSignal, "physMin"));
            XmlHelper.SetParamDouble(xmlToSignal, "physMax", "double", XmlHelper.GetParamDouble(xmlFromSignal, "physMax"));
            XmlHelper.SetParamDouble(xmlToSignal, "samplerate", "double", XmlHelper.GetParamDouble(xmlFromSignal, "samplerate"));

            XmlElement xmlScaling = XmlHelper.GetChildByName(xmlFromSignal, "Mp.Scaling");

            if (xmlScaling != null)
            {//Copy scaling
                XmlElement xmlDelaySigScaling = XmlHelper.GetChildByName(xmlToSignal, "Mp.Scaling");

                if (xmlDelaySigScaling == null)
                    xmlDelaySigScaling = CreateXmlObject(xmlFromSignal, "Mp.Scaling", "Mp.Scaling.FactorOffset");

                XmlHelper.SetParamDouble(xmlDelaySigScaling, "factor", "double", XmlHelper.GetParamDouble(xmlScaling, "factor"));
                XmlHelper.SetParamDouble(xmlDelaySigScaling, "offset", "double", XmlHelper.GetParamDouble(xmlScaling, "offset"));
            }
        }


        private void OnDiagramCopy(List<Shape> shapes)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Stations>");

            foreach(Shape shape  in shapes)
            {
                ProcessStation station = shape as ProcessStation;

                if (station == null)
                    continue;
                
                sb.Append(station.CopyToXml());
            }

            sb.Append("</Stations>");

            Clipboard.SetData("MeaProcess.ProcessStations", sb.ToString());
        }
        

        private void OnDiagramModifiedEvent(DiagramCtrl me)
        {
            this.Modified = me.Modified;
        }


        public void ClearUndoStack()
        {
            foreach (DiagramWindow w in _diagrams)
                w.Diagram.ClearUndoStack();
        }


        public void ShowDiagram(DiagramWindow diagram)
        {
            DiagramWindow w = _dockPanel.ActiveContent as DiagramWindow;

            diagram.Show(_dockPanel);
            diagram.DockState = DockState.Document;
            _diagrams.Add(diagram);

            if (w != null)
                w.Activate();
        }


        public void CloseDiagram(DiagramWindow diagram)
        {
            diagram.Diagram.Clear();
            _diagrams.Remove(diagram);
            diagram.CloseForced();
        }

        
        public void RemoveDiagram(DiagramWindow diagram)
        {
            _diagrams.Remove(diagram);
            diagram.Diagram.Clear();
        }


        private void UpdateXmlObjectIdMapping(XmlElement xmlStartObj)
        {            
            uint id = XmlHelper.GetObjectID(xmlStartObj);
            _xmlObjectIDMapping[id] = xmlStartObj;
            NextXmlObjectID(xmlStartObj);
        }


        public void UpdateSource(XmlElement xmlStorage)
        {
            uint id = (uint)XmlHelper.GetParamNumber(xmlStorage, "sourceId");
            XmlElement xmlSource = GetXmlObjectById(id);
            string name = XmlHelper.GetParam(xmlStorage, "name");

            if (xmlSource == null)
            {
                uint sourceId = RegisterSource(name, 123, name);
                XmlHelper.SetParamNumber(xmlStorage, "sourceId", "uint32_t", (long)sourceId);
            }
            else
            {
                XmlHelper.SetParam(xmlSource, "name", "string", name);
                XmlHelper.SetParam(xmlSource, "uid", "string", name);
            }
        }


        private void NextXmlObjectID(XmlElement parent)
        {
            foreach (XmlElement child in parent.ChildNodes)
            {
                if (child.Name != "object")
                    continue;

                uint id = XmlHelper.GetObjectID(child);
                _xmlObjectIDMapping[id] = child;
                NextXmlObjectID(child);
            }
        }


        public bool IsPropertyAvailable(string prop)
        {
            string propertyName = prop;

            if (prop.Length > 0)
            {
                if (prop[0] == '$')
                {
                    propertyName = propertyName.Remove(0, 2);
                    propertyName = propertyName.Remove(propertyName.Length - 1, 1);
                }
            }

            XmlElement xmlDocument = _xmlDoc.DocumentElement;
            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDocument, "Mp.Properties");

            if (xmlProperties == null)
                return false;

            foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
            {
                if (XmlHelper.GetParam(xmlProperty, "name") == propertyName)
                    return true;
            }

            return false;
        }


        public string GetPropertyValue(string prop)
        {
            string propertyName = prop;

            if (prop.Length > 0)
            {
                if (prop[0] == '$')
                {
                    propertyName = propertyName.Remove(0, 2);
                    propertyName = propertyName.Remove(propertyName.Length - 1, 1);
                }
            }

            XmlElement xmlDocument = _xmlDoc.DocumentElement;
            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDocument, "Mp.Properties");

            if (xmlProperties == null)
                return "";

            foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
            {
                if (XmlHelper.GetParam(xmlProperty, "name") == propertyName)
                {
                    return XmlHelper.GetParam(xmlProperty, "value");
                }
            }

            return "";
        }


        public string GetPropertyType(string prop)
        {
            string propertyName = prop;

            if (prop.Length > 0)
            {
                if (prop[0] == '$')
                {
                    propertyName = propertyName.Remove(0, 2);
                    propertyName = propertyName.Remove(propertyName.Length - 1, 1);
                }
            }

            XmlElement xmlDocument = _xmlDoc.DocumentElement;
            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDocument, "Mp.Properties");

            if (xmlProperties == null)
                return "";

            foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
            {
                if (XmlHelper.GetParam(xmlProperty, "name") == propertyName)
                    return XmlHelper.GetParam(xmlProperty, "type");
            }

            return "";
        }


        public static void ShowHelp(Control parent, int id)
        {
            Help.ShowHelp(parent, HelpFile, HelpNavigator.TopicId, id.ToString());
        }
    }
}
