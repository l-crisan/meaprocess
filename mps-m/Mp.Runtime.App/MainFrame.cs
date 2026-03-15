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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using ICSharpCode.SharpZipLib.Zip;
using Mp.Visual.Docking; 
using Mp.Runtime.Sdk;
using Mp.Scheme.Info;
//using Mp.Conv.Data;
using Mp.Utils;

namespace Mp.Runtime.App
{
    internal partial class MainFrame : Form
    {
        private delegate void OnStopReceived();

        private string _runtimeFile;
        private XmlDocument _xmlDoc;        
        private bool _errorState = false;
        private OutputWindow _output;
        private List<Mp.Runtime.Sdk.Module> _modules = new List<Mp.Runtime.Sdk.Module>();
        private List<Form> _panelsDockContent = new List<Form>();
        private System.Windows.Forms.StatusBar _statusBar = new StatusBar();
        private StatusBarPanel _statusText = new StatusBarPanel();
        private Hashtable _stations = new Hashtable();
        private Hashtable _xmlObjects = new Hashtable();
        private string _pw = "52gr2541g4211";
        private string _language = "en-US";
        private static Hashtable _typeMappingTable = new Hashtable();
        private SystemInputPS _systemInputPS = null;
        private SystemOutputPS _systemOutputPS = null;
        private EventWaitHandle _startUpEvent = null;
        private delegate void CallOutputMessageDelegate(DateTime timeStamp, uint msec, string message, int type);

        public MainFrame(string runtimeFile)
        {
            _xmlDoc = LoadDocument(runtimeFile);

            SetLanguage(runtimeFile, _xmlDoc);

            InitializeComponent();

            _output = new OutputWindow(dockPanel);

            RuntimeEngine.AppIcon = this.Icon;

            InitSatusBar();

            string fname = Path.GetFileName(runtimeFile);

            if (fname != "Mp.Rtf.dll")
                this.Text += " - " + runtimeFile;

            _runtimeFile = runtimeFile;
        }


        public void CreateStartUpMutex(string name)
        {
            try
            {
                _startUpEvent = NamedEvent.OpenOrWait(name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public SystemOutputPS SysOutPS
        {
            get { return _systemOutputPS; }
        }


        public SystemInputPS SysInPS
        {
            get { return _systemInputPS; }
        }


        private void SetLanguage(string runtimeFile, XmlDocument xmlDoc)
        {
            int language = -1;

            if (xmlDoc != null)
            {
                XmlElement xmlGUI = XmlHelper.GetChildByType(xmlDoc.DocumentElement, "GUI");
                language = (int)XmlHelper.GetParamNumber(xmlGUI, "language");
            }

            switch(language)
            {
                case 1: //English
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                    _language = "en-US";
                break;
                
                case 2: //Deutsch
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
                    _language = "de-DE";
                break;

                default:
                    _language = Thread.CurrentThread.CurrentCulture.IetfLanguageTag;
                break;
            }
        }


        private void InitSatusBar()
        {
            this.Controls.Add(_statusBar);
            _statusBar.ShowPanels = true;
            _statusText.BorderStyle = StatusBarPanelBorderStyle.None;
            _statusText.Text = StringResource.Ready;
            _statusText.Width = 200;
            _statusBar.Panels.Add(_statusText);
            _statusBar.Panels.Add(new StatusBarPanel());
        }

        
        private bool UpdateProperties(XmlElement xmlDoc)
        {
            if (_errorState)
                return false;

            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDoc, "Mp.Properties");

            if (xmlProperties == null)
                return true;

            foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
            {
                string value = XmlHelper.GetParam(xmlProperty, "value");
                bool mandatory = XmlHelper.GetParamNumber(xmlProperty, "mandatory") > 0;
                
                if (mandatory && value == "")
                {
                    PropertyDlg dlg = new PropertyDlg(xmlDoc);

                    if (dlg.ShowDialog() != DialogResult.OK)
                    {
                        OutputMessage(DateTime.Now, 0, StringResource.MandatoryPropErr, 1);
                        return false;
                    }
                    break;
                }
            }            

            WritePropertiesToRuntime();

            return true;
        }


        private void WritePropertiesToRuntime()
        {
            XmlElement xmlDoc = _xmlDoc.DocumentElement;

            XmlElement xmlProperties = XmlHelper.GetChildByType(xmlDoc, "Mp.Properties");

            if (xmlProperties == null)
                return;

            RuntimeEngine runtime = RuntimeEngine.Instance();

            foreach (XmlElement xmlProperty in xmlProperties.ChildNodes)
            {
                string value = XmlHelper.GetParam(xmlProperty, "value");
                string name = XmlHelper.GetParam(xmlProperty, "name");
                runtime.SetProperty(name, value);
            }
        }


        private void InitDockPane(XmlElement xmlGui)
        {
            string dockState = XmlHelper.GetParam(xmlGui,"dockManager");

            if (dockState == null || dockState == "")
            {
                _output.Show(dockPanel);
                _output.DockState = DockState.DockBottom;

                if (XmlHelper.GetParamNumber(xmlGui, "hideTabForPanal") == 0)
                {
                    if (_panelsDockContent.Count == 1)
                        dockPanel.DocumentStyle = DocumentStyle.DockingSdi;
                    else
                        dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
                }
                else
                {
                    dockPanel.DocumentStyle = DocumentStyle.DockingSdi;
                }
                
                foreach (PanelContainer panelContainer in _panelsDockContent)
                {
                    panelContainer.Show(dockPanel);
                    panelContainer.DockState = DockState.Document;
                }
            }
            else
            {
                int panels = _panelsDockContent.Count;


                if (XmlHelper.GetParamNumber(xmlGui, "hideTabForPanal") == 0)
                {
                    if (panels == 1)
                        dockPanel.DocumentStyle = DocumentStyle.DockingSdi;
                    else
                        dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
                }
                else
                {
                    dockPanel.DocumentStyle = DocumentStyle.DockingSdi;
                }
                
                dockPanel.DocumentStyle = DocumentStyle.DockingSdi;

                using (MemoryStream mm = new MemoryStream())
                {
                    StreamWriter sw = new StreamWriter(mm);
                    sw.Write(dockState.ToCharArray());
                    sw.Write(0);

                    sw.Flush();
                    mm.Flush();

                    mm.Seek(0, SeekOrigin.Begin);
                    dockPanel.LoadFromXml(mm, new DeserializeDockContent(GetDockContent), true);

                    foreach (PanelContainer panelContainer in _panelsDockContent)
                    {
                        panelContainer.Show(dockPanel);
                        panelContainer.DockState = DockState.Document;
                        panelContainer.Activate();
                    }
                }

                if (XmlHelper.GetParamNumber(xmlGui, "hideTabForPanal") == 0)
                {
                    if (panels == 1)
                        dockPanel.DocumentStyle = DocumentStyle.DockingSdi;
                    else
                        dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
                }
                else
                {
                    dockPanel.DocumentStyle = DocumentStyle.DockingSdi;
                }


                if (XmlHelper.GetParamNumber(xmlGui, "fullScreen") == 1)
                {
                    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                }
            }
        }


        private DockContent GetDockContent(string padTypeName)
        {
            try
            {
                string[] array = padTypeName.Split('\n');


                switch (array[0])
                {
                    case "Mp.Visual.DockWindows.OutputWindow":
                        return _output;
                    
                    case "Mp.Runtime.App.PanelContainer":
                    {
                        XmlElement xmlGUI = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "GUI");

                        if (array.Length < 2)
                            return null;

                        for (int i = 0; i < _panelsDockContent.Count; ++i)
                        {
                            PanelContainer panel = (PanelContainer)_panelsDockContent[i];

                            if (panel.ID == Convert.ToInt32(array[1]))
                            {
                                _panelsDockContent.Remove(panel);
                                return panel;
                            }
                        }
                    }
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }


        private Hashtable LoadSignalList( XmlElement xmlSignals )
        {
            Hashtable signalListMap = new Hashtable();
            Hashtable signalMap     = new Hashtable();

            foreach (XmlNode xmlNode in xmlSignals)
            {
                XmlElement xmlSignalList = xmlNode as XmlElement;

                if (xmlSignalList == null)
                    continue;

                SignalList signalList = new SignalList();

                foreach (XmlNode xmlSignalNode in xmlSignalList.ChildNodes)
                {
                    XmlElement xmlSignal = xmlSignalNode as XmlElement;

                    if (xmlSignal == null)
                        continue;

                    if (xmlSignal.GetAttribute("type") == "Mp.Sig")
                    { // Signal
                        Mp.Runtime.Sdk.Signal signal = new Mp.Runtime.Sdk.Signal(xmlSignal);
                        signal.SignalID = XmlHelper.GetObjectID(xmlSignal);
                        signal.Name = XmlHelper.GetParam(xmlSignal, "name");
                        signal.Comment = XmlHelper.GetParam(xmlSignal, "comment");
                        signal.Unit = XmlHelper.GetParam(xmlSignal, "unit");
                        signal.SampleRate = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                        signal.Minimum = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                        signal.Maximum = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                        signal.PhysSourceId = (ushort)XmlHelper.GetParamNumber(xmlSignal, "sourceNumber");
                        signal.DataType = (Mp.Runtime.Sdk.Signal.DataTypes)XmlHelper.GetParamNumber(xmlSignal, "valueDataType");

                        if( signal.DataType == Signal.DataTypes.ObjectType)
                            signal.DataTypeSize = (uint) XmlHelper.GetParamNumber(xmlSignal, "objSize");

                        XmlElement xmlScaling = XmlHelper.GetChildByType(xmlSignal, "Mp.Scaling");
                        signal.OnLoadScalingObject(xmlScaling);

                        signalList.Add(signal);
                        signal.SignalIndex = signalList.Count - 1;
                        signalMap.Add(XmlHelper.GetObjectID(xmlSignal), signal);
                    }
                    else
                    { //Signal reference
                        signalList.AddSignalReference(Convert.ToUInt32(xmlSignal.InnerText));
                    }
                }

                signalListMap[XmlHelper.GetObjectID(xmlSignalList)] = signalList;
            }

            //Load signal references 
            foreach (DictionaryEntry entry in signalListMap)
            {
                SignalList sigList = (SignalList)entry.Value;
                sigList.LoadSigRef(signalMap);
            }

            signalMap = null;
            return signalListMap;
        }


        private XmlElement GetXmlObjectByID(uint id)
        {
            if(_xmlObjects.ContainsKey(id))
                return (XmlElement) _xmlObjects[id];
            
            LoadXmlMapObjects(_xmlDoc.DocumentElement);

            return (XmlElement)_xmlObjects[id];
        }


        private void LoadXmlMapObjects(XmlElement rootObject)
        {
            foreach (XmlElement xmlElement in rootObject.ChildNodes)
            {
                uint id = XmlHelper.GetObjectID(xmlElement);
                if (id == 0)
                    continue;

                if (_xmlObjects.ContainsKey(id))
                    continue;

                _xmlObjects[id] = xmlElement;

                LoadXmlMapObjects(xmlElement);
            }
        }


        private string GetNewTypeName(string oldTypeName)
        {
            if (_typeMappingTable.Contains(oldTypeName))
                return GetNewTypeName((string)_typeMappingTable[oldTypeName]);

            return oldTypeName;
        }


        private void LoadThePanels(XmlElement xmlGui, Hashtable signalListMap )
        {
            _xmlObjects.Clear();

            viewToolStripMenuItem.DropDown.Items.Add( new ToolStripSeparator());

            foreach (XmlNode xmlNode in xmlGui.ChildNodes)
            {
                XmlElement xmlPanel = xmlNode as XmlElement;

                if (xmlPanel == null)
                    continue;

                if (XmlHelper.GetObjectID(xmlPanel) == 0)
                    continue;

                string panelData = XmlHelper.GetParam(xmlPanel, "panelData");
                Form panel = (Form)ControlSurrogate.DeserializeFromString(panelData, typeof(Form), _typeMappingTable);
                
                PanelContainer panelContainer = new PanelContainer();
                panelContainer.ID = (int) XmlHelper.GetParamNumber(xmlPanel, "ID");

                ToolStripMenuItem item = new ToolStripMenuItem(panel.Text);
                item.Tag = panelContainer;
                item.Image = panel.Icon.ToBitmap();
                item.Click += new EventHandler(OnPanelActivate);

                viewToolStripMenuItem.DropDown.Items.Add(item);

                if (panel.Icon == null)
                    panel.Icon = Resource.Document;

                panel.TopLevel = false;
                panelContainer.Panel = panel;
                panel.FormBorderStyle = FormBorderStyle.None;
                panel.Show();
                panelContainer.FormClosing += new FormClosingEventHandler(OnPanelClosing);
                panelContainer.ShowIcon = true;                
                _panelsDockContent.Add(panelContainer);

                //Load Controls
                foreach (XmlNode xmlCtrlNode in xmlPanel.ChildNodes)
                {
                    XmlElement xmlCtrl = xmlCtrlNode as XmlElement;

                    if (xmlCtrl == null)
                        continue;

                    if (xmlCtrl.Attributes["name"].Value != "ctrlData")
                        continue;

                    //Create the control.
                    string typeName = GetNewTypeName(xmlCtrl.Attributes["ctrlType"].Value);
                    Type ctrlType = ControlSurrogate.GetLoadedTypeByName(typeName);
                    
                     if( ctrlType == null)
                          throw new Exception(String.Format(StringResource.CtrlLoadErr, (string) xmlCtrl.Attributes["ctrlType"].Value));

                    Control ctrl = (Control)ControlSurrogate.DeserializeFromString(xmlCtrl.InnerText, ctrlType, _typeMappingTable);

                    //Get the process station.
                    ControlData data = (ControlData)ctrl.Tag;
                    ProcessStation ps = null;

                    if (data.StationId != 0)
                    {// It's a PS control
                        if (_stations.ContainsKey(data.StationId))
                        {
                            ps = (ProcessStation)_stations[data.StationId];
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(data.ProcessStationLibrary))
                                Assembly.LoadFrom(data.ProcessStationLibrary);

                            Type psType = ControlSurrogate.GetLoadedTypeByName(data.ProcessStationType);
                            
                            if (psType == null)
                                throw new Exception(String.Format(StringResource.PSLoadErr,data.ProcessStationType)); 

                            ps = (ProcessStation)Activator.CreateInstance(psType);
                            ps.ID = data.StationId;
                            ps.XmlRep = GetXmlObjectByID(data.StationId);
                            _stations[data.StationId] = ps;
                        }

                        //Attache the signal list to the process station
                        ps.Signals = signalListMap;

                        //Add the control to the process station.
                        ps.AddControl(ctrl);
                    }

                    //Add the control to panel.
                    panel.Controls.Add(ctrl);

                }
            }
        }


        void OnPanelActivate(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            PanelContainer panelContainer = (PanelContainer)item.Tag;
            panelContainer.BringToFront();
            panelContainer.Focus();
            panelContainer.Activate();
        }


        private void OnPanelClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (e.CloseReason == CloseReason.UserClosing);
        }


        private void SaveThePanels()
        {
            //Obtain the GUI xml tag.
            XmlElement xmlGUI = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "GUI");

            //Remove the old panels
            RemoveXMLPanels(xmlGUI);
            
            //Initialize the panel filter
            List<string> panelFilter = new List<string>();
            panelFilter.Add("BackColor");
            panelFilter.Add("Icon");
            panelFilter.Add("Left");
            panelFilter.Add("Top");
            panelFilter.Add("Width");
            panelFilter.Add("Height");
            panelFilter.Add("Text");
            panelFilter.Add("BackgroundImage");
            panelFilter.Add("BackgroundImageLayout");
            panelFilter.Add("FormBorderStyle");
            panelFilter.Add("Tag");

            //Determinate the document max id.
            ulong maxId = (ulong)XmlHelper.GetParamNumber(_xmlDoc.DocumentElement, "docMaxID");

            if(dockPanel.ActiveDocument != null)
                XmlHelper.SetParam(xmlGUI, "activePanel", "string", ((PanelContainer) dockPanel.ActiveDocument).TabText);
            else
                XmlHelper.SetParam(xmlGUI, "activePanel", "string","" );

            //Save the panels.
            foreach(Form form in dockPanel.Contents)
            {
                PanelContainer panelContainer = form as PanelContainer;
                
                if (panelContainer == null)
                    continue;

                panelContainer.Activate();
                Form panel = panelContainer.RestoreOrignalPanelState();

                XmlElement xmlPanel = XmlHelper.CreateObject(xmlGUI, "Panel", "Panel", maxId++);
                XmlHelper.SetParam(xmlPanel, "panelData", "string", ControlSurrogate.SerializeToString(panel, panelFilter));
                XmlHelper.SetParamNumber(xmlPanel, "ID", "uint32_t", panelContainer.ID);

                //Save the controls
                foreach (Control ctrl in panel.Controls)
                {
                    ControlData ctrlData = (ControlData)ctrl.Tag;
                    XmlElement xmlCtrl = XmlHelper.CreateElement(xmlPanel, "string", "ctrlData", ControlSurrogate.SerializeToString(ctrl, ctrlData.PropertyFilter));
                    XmlAttribute ctrlType = _xmlDoc.CreateAttribute("ctrlType");
                    ctrlType.Value = ctrl.GetType().FullName;
                    xmlCtrl.Attributes.Append(ctrlType);
                }
            }

            //Save the new max id back.
            XmlHelper.SetParamNumber(_xmlDoc.DocumentElement, "docMaxID","uint32_t", (int)maxId);         
        }

        
        private void RemoveXMLPanels(XmlElement xmlGUI)
        {
            XmlElement xmlPanel;
            XmlNode node;

            for (int i = 0; i < xmlGUI.ChildNodes.Count; i++)
            {
                node = xmlGUI.ChildNodes[i];

                xmlPanel = (node as XmlElement);

                if (xmlPanel == null)
                    continue;

                if (XmlHelper.GetObjectID(xmlPanel) == 0)
                    continue;

                xmlGUI.RemoveChild(xmlPanel);
                i--;
            }
        }


        private void ScanModules()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(dir,"*.dll");
            string modules = "";

            foreach (string file in files)
            {
                try
                {
                    string fileName = Path.GetFileName(file);
                    
                    if (fileName == "Mp.Runtime.Sdk.dll")
                        continue; //Contain the base Module class

                    //Load Assembly modules
                    Mp.Runtime.Sdk.Module module = TryLoadAssembly(file);

                    if (module != null)
                    {
                        _modules.Add(module);
                        modules += file;
                        modules += ";";
                        continue;
                    }

                    try
                    {   //Is a .Net Assembly => continue
                        Assembly ass = Assembly.LoadFile(file);

                        if (ass != null)
                            continue;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    
                    //Is a c++ runtime module => load it.
                    if (fileName.Length < 4)
                        continue;

                    if (fileName[0] != 'm' || fileName[1] != 'p' || fileName[2] != 's' ||
                        fileName[3] != '-')
                        continue;

                    if (RuntimeEngine.LoadModule(file))
                    {
                        modules += file;
                        modules += ";";
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }


        public static Mp.Runtime.Sdk.Module TryLoadAssembly(string file)
        {
            Assembly assembly;
            Type[] types;

            if (file[0] == '.')
            {
                file = file.Substring(2);
                file = AppDomain.CurrentDomain.BaseDirectory + file;
            }

            try
            {
                assembly = Assembly.LoadFrom(file);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }

            types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (type.BaseType == null)
                    continue;

                if (type.BaseType.FullName == "Mp.Runtime.Sdk.Module")
                {
                    return (Activator.CreateInstance(type) as Mp.Runtime.Sdk.Module);
                }

                if (type.BaseType.BaseType == null)
                    continue;

                if (type.BaseType.BaseType.FullName == "Mp.Schema.Sdk.Module")
                {
                    return (Activator.CreateInstance(type) as Mp.Runtime.Sdk.Module);
                }
            }

            return null;
        }


        private void LoadModules(XmlDocument xmlDoc)
        {
            if(xmlDoc == null)
                return;

            try
            {
                _modules.Add(new DefaultModule());

                XmlElement xmlRoot = xmlDoc.DocumentElement;

                string moduleFiles = XmlHelper.GetParam(xmlRoot, "runtimeModules");
                
                if (moduleFiles == "")
                    return;

                string[] modules = moduleFiles.Split(';');

                foreach (string f in modules)
                {
                    string file = f;

                    if (file == "")
                        continue;

                    file += ".dll";

                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
                    
                    Mp.Runtime.Sdk.Module module = TryLoadAssembly(path);

                    if (module != null)
                    {
                        _modules.Add(module);
                        continue;
                    }

                    if (RuntimeEngine.LoadModule(path))
                        continue;

                    OutputMessage(DateTime.Now,0,String.Format(StringResource.ModuleLoadErr, file),2);
                }
            }
            catch(Exception ex)
            {
                OutputMessage(DateTime.Now, 0, ex.Message, 2);
            }
        }


        private MessageBoxIcon GetIconFromType(MessageType type)
        {
            switch (type)
            {
                case MessageType.Error:
                    return MessageBoxIcon.Error;
                case MessageType.Info:
                    return MessageBoxIcon.Information;
                case MessageType.Question:
                    return MessageBoxIcon.Question;
                case MessageType.Stop:
                    return MessageBoxIcon.Stop;
                case MessageType.Warning:
                    return MessageBoxIcon.Warning;
                case MessageType.EventMsg:
                    return MessageBoxIcon.Information;
            }

            return MessageBoxIcon.Information;
        }


        private int OnRuntimeMessageEvent(Adaption.Message message)
        {
            switch (message.TargetType)
            {
                case Adaption.Message.Target.Output:
                case Adaption.Message.Target.Event:
                    OutputMessage((DateTime)message.TimeStamp, (uint) message.TimeStamp.Millisecond, message.Text, (int)message.Type);
                break;
                case Adaption.Message.Target.Status:
                break;
                case Adaption.Message.Target.Modal:
                {
                    message.FileName = "";

                    if (message.Type == Mp.Runtime.Adaption.Message.MessageType.Question)
                    {
                        DialogResult result = MessageBox.Show(message.Text, this.Text, MessageBoxButtons.YesNo, GetIconFromType((MessageType)message.Type));

                        if (result == DialogResult.Yes)
                            return 0;
                        else
                            return 1;
                    }
                    else if (message.Type == Mp.Runtime.Adaption.Message.MessageType.QuestionFile)
                    {
                        DialogResult result = MessageBox.Show(message.Text, this.Text, MessageBoxButtons.YesNo, GetIconFromType((MessageType)message.Type));
                        
                        if (result == DialogResult.Yes)
                        {
                            return 0;
                        }
                        else
                        {
                            OpenFileDialog dlg = new OpenFileDialog();
                            dlg.CheckFileExists = false;
                            dlg.Filter = "*.mmf|*.mmf|*.tdm|*.tdm|*.*|*.*";
                            
                            if (dlg.ShowDialog() != DialogResult.OK)
                                return 0;

                            message.FileName = dlg.FileName;
                            return 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show(message.Text, this.Text, MessageBoxButtons.OK, GetIconFromType((MessageType)message.Type));
                    }
                }
                break;
                case Adaption.Message.Target.LogFile:
                break;
                case Adaption.Message.Target.Trace:
                break;
                case Adaption.Message.Target.File:
                break;
                case Adaption.Message.Target.System:
                    if (message.Type == Adaption.Message.MessageType.Stop)
                        this.Invoke(new OnStopReceived(OnStopMessageReceived), null);
                break;
            }

            return 1;
        }


        private void OutputMessage(DateTime timeStamp, uint msec, string message, int type)
        {
            object[] param = new object[4];
            param[0] = timeStamp;
            param[1] = msec;
            param[2] = message;
            param[3] = type;

            this.BeginInvoke(new CallOutputMessageDelegate(CallOutputMessage), param); 
        }


        private void CallOutputMessage(DateTime timeStamp, uint msec, string message, int type)
        {
            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(_language);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(_language);

            string msg = "[" + timeStamp.ToString() + ":" + msec.ToString("000") + "]  " + message;

            if (_output.Handle == null)
                _output.Show(dockPanel);

            _output.WriteLine(msg, (MessageType)type);
        }


        private ProcessStation LoadSystemPS(XmlDocument xmlDoc, bool input)
        {
            XmlElement xmlPsList = XmlHelper.GetChildByType(xmlDoc.DocumentElement, "Mp.PS.List");
            foreach (XmlElement xmlPs in xmlPsList.ChildNodes)
            {
                XmlAttribute xmlSubType = xmlPs.Attributes["subType"];

                if (xmlSubType == null)
                    continue;

                XmlAttribute xmlID = xmlPs.Attributes["id"];
                uint id = Convert.ToUInt32(xmlID.Value);
    
                if (xmlSubType.Value == "Mp.Runtime.App.SystemInputPS" && input)
                {
                    SystemInputPS ps = new SystemInputPS();
                    ps.XmlRep = xmlPs;
                    ps.ID = id;
                    _stations.Add(id, ps);
                    return ps;
                }
                else if (xmlSubType.Value == "Mp.Runtime.App.SystemOutputPS" && !input)
                {
                    SystemOutputPS ps = new SystemOutputPS();
                    ps.XmlRep = xmlPs;
                    ps.ID = id;
                    _stations.Add(id, ps);
                    return ps;
                }
            }

            return null;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _statusText.Text = StringResource.StatusInitHw;
            Cursor cursor = Cursor;
            Cursor = Cursors.WaitCursor;

            //Get a instance to the runtime engine.
            RuntimeEngine runtime = RuntimeEngine.Instance();


            //Intialize the runtime language
            string code = Thread.CurrentThread.CurrentCulture.IetfLanguageTag;
            

            if (_runtimeFile == "")
            {
                runToolStripButton.Checked = false;
                runToolStripButton.Enabled = false;
                toolStripMenuItemRun.Checked = false;
                reinitializeToolStripMenuItem.Enabled = false;
                toolStripMenuItemRun.Enabled = false;
                infoToolStripMenuItem.Enabled = false;
                propertiesToolStripMenuItem.Enabled = false;
                toolStripButtonProperties.Enabled = false;
                _errorState = true;

                Cursor = cursor;
                _statusText.Text = StringResource.Ready;
                return;
            }

            LoadModules(_xmlDoc);
            //Open the xml document.
            if (_xmlDoc == null)
            {
                runToolStripButton.Checked = false;
                runToolStripButton.Enabled = false;
                toolStripMenuItemRun.Checked = false;
                reinitializeToolStripMenuItem.Enabled = false;
                toolStripMenuItemRun.Enabled = false;
                infoToolStripMenuItem.Enabled = false;
                propertiesToolStripMenuItem.Enabled = false;
                toolStripButtonProperties.Enabled = false;
                _errorState = true;
                Cursor = cursor;
                _statusText.Text = StringResource.Ready;
                OutputMessage(DateTime.Now, 0, StringResource.InvalidFile, 2);
                return;
            }

            _systemInputPS = (SystemInputPS) LoadSystemPS(_xmlDoc, true);
            _systemOutputPS = (SystemOutputPS)LoadSystemPS(_xmlDoc, false);

            try
            {
                XmlElement xmlGui = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "GUI");

                //Load the signals.
                XmlElement xmlSignals = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Mp.Signals");
                Hashtable signalListMap = LoadSignalList(xmlSignals);

                if (_systemInputPS != null)
                    _systemInputPS.Signals = signalListMap;


                if (_systemOutputPS != null)
                    _systemOutputPS.Signals = signalListMap;

                //Load the panels
                LoadThePanels(xmlGui, signalListMap);

                //Set the log file
                string logFile = XmlHelper.GetParam(xmlGui, "logFile");
                logFile = logFile.TrimEnd(' ');
                logFile = logFile.TrimStart(' ');

                int level = (int) XmlHelper.GetParamNumber(xmlGui, "logLevel");
                if (logFile != "")
                    runtime.SetLogFile(logFile, level);


                //Initialize the dockPane
                InitDockPane(xmlGui);

                //Clone the document.
                XmlDocument runtimeDoc = ProcessStation.GetRuntimeDocument(_xmlDoc);

                runtime.MessageEvent += new Mp.Runtime.Adaption.OnNewMessageDelegate(OnRuntimeMessageEvent);

                //Load the runtime engine.
                if (!runtime.LoadFromXml(runtimeDoc.OuterXml))
                    throw new Exception(StringResource.RuntimeLoadErr);

                runtime.SetLanguage(code);

                runtimeDoc = null;
                propertiesToolStripMenuItem.Enabled = true;
                toolStripButtonProperties.Enabled = true;

                GC.Collect();
            }
            catch (Exception ex)
            {
                OutputMessage(DateTime.Now, 0, ex.Message, 2);
                runToolStripButton.Checked = false;
                runToolStripButton.Enabled = false;
                toolStripMenuItemRun.Checked = false;
                reinitializeToolStripMenuItem.Enabled = false;
                toolStripMenuItemRun.Enabled = false;
                _errorState = true;
                _output.Show(dockPanel);
                _output.Focus();
            }
            Cursor = cursor;
            _statusText.Text = StringResource.Ready;

            if (_errorState)
            {
                return;
            }

            foreach(ProcessStation ps in _stations.Values)
            {
                ps.Setup();
                
                if( ps.IsOutputPS)
                    runtime.addDataOutListener( (uint) ps.ID, ps );
                else
                    runtime.addDataSource( (uint)ps.ID, ps);
            }
            
            stopToolStripButton.Checked = true;
            runToolStripButton.Checked = false;
            toolStripMenuItemRun.Checked = false;
        }


        private XmlDocument LoadDocument(string runtimeFile)
        {
            XmlDocument xmlDoc = new XmlDocument();

            try
            {
                using (FileStream fs = new FileStream(runtimeFile, FileMode.Open))
                {
                    ZipInputStream istream = new ZipInputStream(fs);
                    istream.Password = _pw;
                    ZipEntry entry = istream.GetNextEntry();
                    byte[] bytes = new byte[istream.Length];

                    istream.Read(bytes, 0, (int)istream.Length);
                    MemoryStream mm = new MemoryStream(bytes);
                    xmlDoc.Load(mm);

                    bool isValid = XmlHelper.GetParamNumber(xmlDoc.DocumentElement, "valid") != 0;
                    
                    if(!isValid)
                        throw new Exception(StringResource.InvalidDoc);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Console.WriteLine(ex.Message);
                    xmlDoc.Load(runtimeFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return null;
                }
            }

            return Runtime.Sdk.RuntimeEngine.DocToCurrentVersion(xmlDoc);
        }


        private void OnExitClick(object sender, EventArgs e)
        {
            Close();
        }


        public void Start()
        {
            XmlElement xmlGUI = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "GUI");

            if (XmlHelper.GetParamNumber(xmlGUI, "resetPropOnStart") > 0)
            {
                XmlElement _xmlProperties = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Mp.Properties");

                if (_xmlProperties != null)
                {
                    if (_xmlProperties.ChildNodes.Count != 0)
                    {
                        RuntimeEngine.Instance().Deinitialize();
                        PropertyDlg dlg = new PropertyDlg(_xmlDoc.DocumentElement);

                        if (dlg.ShowDialog() != DialogResult.OK)
                        {
                            RuntimeEngine.Instance().Initialize();
                            Cursor = Cursors.Default;
                            return;
                        }

                        WritePropertiesToRuntime();

                        RuntimeEngine.Instance().Initialize();
                        ReadPropertiesFromRuntime();
                    }
                }
            }
            else
            {
                if (!UpdateProperties(_xmlDoc.DocumentElement))
                    return;
            }

            RuntimeEngine.Instance().Start();
            stopToolStripButton.Checked = false;
            stopToolStripButton.Enabled = true;

            toolStripMenuItemStop.Checked = false;
            toolStripMenuItemStop.Enabled = true;

            runToolStripButton.Checked = true;
            runToolStripButton.Enabled = false;

            toolStripMenuItemRun.Checked = true;
            reinitializeToolStripMenuItem.Enabled = false;
            toolStripMenuItemRun.Enabled = false;
            propertiesToolStripMenuItem.Enabled = false;
            toolStripButtonProperties.Enabled = false;
            _statusText.Text = StringResource.Running;
        }


        private void OnStartClick(object sender, EventArgs e)
        {
            Start();
        }


        private void OnStopClick(object sender, EventArgs e)
        {
            Stop();
        }


        public void Stop()
        {
            RuntimeEngine.Instance().Stop();
            GC.Collect();
        }
        

        private void OnStopMessageReceived()
        {
            stopToolStripButton.Checked = true;
            stopToolStripButton.Enabled = false;

            toolStripMenuItemStop.Checked = true;
            toolStripMenuItemStop.Enabled = false;

            runToolStripButton.Checked = false;
            runToolStripButton.Enabled = true;

            toolStripMenuItemRun.Checked = false;
            toolStripMenuItemRun.Enabled = true;
            reinitializeToolStripMenuItem.Enabled = true;
            propertiesToolStripMenuItem.Enabled = true;
            toolStripButtonProperties.Enabled = true;
            _statusText.Text = StringResource.Ready;
            ReadPropertiesFromRuntime();

            XmlElement xmlGUI = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "GUI");

            if (XmlHelper.GetParamNumber(xmlGUI, "closeOnStop") != 0)
                Close();
        }


        private void ReadPropertiesFromRuntime()
        {

            XmlElement xmlProperties = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "Mp.Properties");

            if (xmlProperties != null)
            {
                RuntimeEngine runtime = RuntimeEngine.Instance();
                foreach (XmlElement xmlProp in xmlProperties.ChildNodes)
                {
                    string prop = XmlHelper.GetParam(xmlProp, "name");
                    string propValue = runtime.GetPropertyValue(prop);
                    XmlHelper.SetParam(xmlProp, "value", "string", propValue);
                }
            }
        }


        private void OnAboutClick(object sender, EventArgs e)
        {
            AboutBox AboutMP = new AboutBox();
            AboutMP.ShowDialog();
        }


        private void OnShowControlBarClick(object sender, EventArgs e)
        {
            _controlBar.Visible = !_controlBar.Visible;
        }


        private void OnShowOutputWindowClick(object sender, EventArgs e)
        {
            _output.Show(dockPanel);
            _output.Focus();
        }


        private void OnClearClick(object sender, EventArgs e)
        {
            _output.Clear();
        }      


        private void OnShowInfoClick(object sender, EventArgs e)
        {
            SchemeInfoDlg dlg = new SchemeInfoDlg(_xmlDoc.DocumentElement);
            dlg.ShowDialog();
        }


        private void OnPropertiesClick(object sender, EventArgs e)
        {
            PropertyDlg dlg = new PropertyDlg(_xmlDoc.DocumentElement);
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            RuntimeEngine runtime = RuntimeEngine.Instance();
            runtime.Deinitialize();

            WritePropertiesToRuntime();
            if (!_errorState)
            {
                toolStripMenuItemRun.Enabled = true;
                reinitializeToolStripMenuItem.Enabled = true;
                runToolStripButton.Enabled = true;
            }

            runtime.Initialize();
            ReadPropertiesFromRuntime();
        }


        private void OnMainFrameClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;

            if (runToolStripButton.Checked)
                e.Cancel = MessageBox.Show(StringResource.RunCloseApp, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;

            if (e.Cancel)
                return;

            if (_errorState)
                return;

            Cursor cursor = Cursor;
            Cursor = Cursors.WaitCursor;

            _statusText.Text = StringResource.StatusDeinitHw;

            //Send stop signal.
            if (runToolStripButton.Checked)
                RuntimeEngine.Instance().Stop();

            //Send deinitilize signal.
            RuntimeEngine.Instance().Deinitialize();

            //Let the procces station to save the control state.
            ProcessStation ps;
            foreach (DictionaryEntry entry in _stations)
            {
                ps = entry.Value as ProcessStation;
                ps.OnSaveControlsStates();
            }

            //Save the panals.
            SaveThePanels();

            //Save the docking state
            using (MemoryStream mm = new MemoryStream())
            {
                dockPanel.SaveAsXml(mm, Encoding.UTF8, true);

                mm.Flush();
                mm.Seek(0, SeekOrigin.Begin);

                StreamReader sr = new StreamReader(mm);
                string ss = sr.ReadToEnd();
                XmlElement xmlGui = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "GUI");
                XmlHelper.SetParam(xmlGui, "dockManager", "string", ss);

                XmlHelper.SetParamNumber(xmlGui, "rwidth", "uint32_t", this.Width);
                XmlHelper.SetParamNumber(xmlGui, "rheight", "uint32_t", this.Height);
                
            }

            //Save to file.
            string file = Path.GetFileNameWithoutExtension(_runtimeFile);
            file += ".tp";
            file = Path.Combine(Path.GetDirectoryName(_runtimeFile), file);
            _xmlDoc.Save(file);

            //Zip the file
            using (FileStream fs = File.Create(_runtimeFile))
            {
                ZipFile zfile = ZipFile.Create(fs);
                zfile.Password = _pw;

                string path = Path.GetDirectoryName(Path.GetFullPath(_runtimeFile));
                zfile.NameTransform = new ZipNameTransform(path);
                zfile.BeginUpdate();
                zfile.Add(file);
                zfile.CommitUpdate();
                zfile.Close();
                fs.Flush();
                fs.Close();
            }

            File.Delete(file);
            
            Cursor = cursor;
        }


        private void OnShowStatusBarClick(object sender, EventArgs e)
        {
            statusBarToolStripMenuItem.Checked = !statusBarToolStripMenuItem.Checked;
            _statusBar.Visible = statusBarToolStripMenuItem.Checked;
        }


        private void OnShowDataConverterClick(object sender, EventArgs e)
        {
/*
            DataConverterDlg dlg = new DataConverterDlg();
            dlg.StartPosition = FormStartPosition.CenterParent;
            dlg.ShowDialog();*/
        }


        private void ActivatePanel(string activePanal)
        {
            foreach (PanelContainer pc in dockPanel.Documents)
            {
                if (pc.TabText == activePanal)
                {
                    pc.Activate();
                    return;
                }
            }
        }


        public string Messages
        {
            get
            {
                string msg = _output.Messages;

                if( msg == null)
                    msg = "";
                return msg;
            }
        }


        protected override void OnShown(EventArgs e)
        {
 	        base.OnShown(e);

            if (_xmlDoc == null)
            {
                if (_startUpEvent != null)
                    _startUpEvent.Set();

                return;
            }

            if (_xmlDoc.DocumentElement == null)
            {
                if (_startUpEvent != null)
                    _startUpEvent.Set();

                return;
            }


            XmlElement xmlGUI = XmlHelper.GetChildByType(_xmlDoc.DocumentElement, "GUI");


            if (XmlHelper.GetParamNumber(_xmlDoc.DocumentElement, "useRuntimePassword") != 0)
            {
                string title = XmlHelper.GetParam(xmlGUI, "title");
                PasswordDlg dlg = new PasswordDlg(title);
                dlg.ShowDialog();

                if (XmlHelper.GetParam(_xmlDoc.DocumentElement, "runtimePassword") != dlg.Password)
                {
                    OutputMessage(DateTime.Now, 0, StringResource.WrongPassword, 2);
                    runToolStripButton.Checked = false;
                    runToolStripButton.Enabled = false;
                    toolStripMenuItemRun.Checked = false;
                    reinitializeToolStripMenuItem.Enabled = false;
                    toolStripMenuItemRun.Enabled = false;
                    infoToolStripMenuItem.Enabled = false;
                    propertiesToolStripMenuItem.Enabled = false;
                    toolStripButtonProperties.Enabled = false;
                    _errorState = true;
                    
                    _statusText.Text = StringResource.Ready;

                    if (_startUpEvent != null)
                        _startUpEvent.Set();

                    return;
                }
            }


            if (XmlHelper.GetParamNumber(xmlGUI, "roptions") > 0)
            {

                string title = XmlHelper.GetParam(xmlGUI, "title");

                if (title != "")
                    this.Text = title;

                string iconstr = XmlHelper.GetParam(xmlGUI, "icon");
                if (iconstr != "")
                {
                    byte[] buffer = Convert.FromBase64String(iconstr);

                    MemoryStream stream = new MemoryStream(buffer);
                    BinaryFormatter formater = new BinaryFormatter();
                    Bitmap img = (Bitmap)formater.Deserialize(stream);
                    this.Icon = Icon.FromHandle(img.GetHicon());
                    stream.Flush();
                    stream.Seek(0, 0);
                    stream.Close();
                }

                menuStrip.Visible = XmlHelper.GetParamNumber(xmlGUI, "showMenu") > 0;
                _controlBar.Visible = XmlHelper.GetParamNumber(xmlGUI, "showControlBar") > 0;
                controlBarToolStripMenuItem.Checked = _controlBar.Visible;
                _statusBar.Visible = XmlHelper.GetParamNumber(xmlGUI, "showStatusBar") > 0;
                statusBarToolStripMenuItem.Checked = _statusBar.Visible;
                
                string activePanal = XmlHelper.GetParam(xmlGUI, "activePanel");
                ActivatePanel(activePanal);

                if (XmlHelper.GetParamNumber(xmlGUI, "ctrlBarEditPropBt") == 1)
                {
                    ctrlBarSep.Visible = true;
                    toolStripButtonProperties.Visible = true;
                }
                else
                {
                    ctrlBarSep.Visible = false;
                    toolStripButtonProperties.Visible = false;
                }

                if (XmlHelper.GetParamNumber(xmlGUI, "fixedWinSize") > 0)
                {
                    _statusBar.SizingGrip = false;
                    this.FormBorderStyle = FormBorderStyle.FixedDialog;
                    this.MinimizeBox = false;
                    this.MaximizeBox = false;
                }

                this.Width = (int)XmlHelper.GetParamNumber(xmlGUI, "rwidth");
                this.Height = (int)XmlHelper.GetParamNumber(xmlGUI, "rheight");
            }

            if (_errorState)
            {
                if (_startUpEvent != null)
                    _startUpEvent.Set();

                return;
            }
            
            Cursor = Cursors.WaitCursor;
    
            Application.DoEvents();
            
            runToolStripButton.Enabled = false;
            toolStripMenuItemRun.Enabled = false;

            RuntimeEngine runtime = RuntimeEngine.Instance();
            runtime.Initialize();

            runToolStripButton.Enabled = true;
            toolStripMenuItemRun.Enabled = true;

            ReadPropertiesFromRuntime();

            if (XmlHelper.GetParamNumber(xmlGUI, "roptions") > 0)
                if (XmlHelper.GetParamNumber(xmlGUI, "startOnOpen") > 0)
                    OnStartClick(null, null);

            dockPanel.Invalidate();

            if (_startUpEvent != null)            
                _startUpEvent.Set();
                        
            Cursor = Cursors.Default;
        }


        public void Reinitialize()
        {
            if (_errorState)
                return;

            Cursor = Cursors.WaitCursor;
            runToolStripButton.Enabled = false;
            toolStripMenuItemRun.Enabled = false;


            _output.Clear();
            
            Application.DoEvents();

            RuntimeEngine runtime = RuntimeEngine.Instance();

            runtime.Deinitialize();

            WritePropertiesToRuntime();
            runtime.Initialize();
            ReadPropertiesFromRuntime();
            runToolStripButton.Enabled = true;
            toolStripMenuItemRun.Enabled = true;

            Cursor = Cursors.Default;
        }


        private void OnReinitializeClick(object sender, EventArgs e)
        {
            Reinitialize();
        }
    }
}