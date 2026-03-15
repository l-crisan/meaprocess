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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Collections;
using System.IO;
using Mp.Visual.Docking;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Scheme.Designer
{    
    public delegate void AppendControl(Control control);

    public partial class MainFrame : Mp.Visual.Docking.DockContent
    {
        private Document        _document;
        private int             _panelCount = 0;
        private List<string>    _panelFilter = new List<string>();
        private ControlData     _panelData = new ControlData();
        private PropertieWindow _propertyWindow = new PropertieWindow();
        private Timer           _updateTimer = new Timer();
        public AppendControl    AppendControl;
        private List<PanelContainer> _panels = new List<PanelContainer>();
        private string          _activePanelText;
        private int             _maxPanelID = 0;
        private Hashtable       _typeMappingTable = new Hashtable(); 
        
        public MainFrame()
        {
            InitializeComponent();
            Properties.Settings.Default.Reload();
   
            _panelFilter.Add("BackColor");
            _panelFilter.Add("Icon");
            _panelFilter.Add("Left");
            _panelFilter.Add("Top");
            _panelFilter.Add("Width");
            _panelFilter.Add("Height");
            _panelFilter.Add("Text");
            _panelFilter.Add("BackgroundImage");
            _panelFilter.Add("BackgroundImageLayout");
            _panelFilter.Add("Tag");            
            _panelData.PropertyFilter = _panelFilter;
            _updateTimer.Tick += new EventHandler(OnUpdateTimerTick);
            _updateTimer.Interval = 300;
            _updateTimer.Start();
            _propertyWindow.PropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(OnPropertyValueChanged);
            dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocChanged);
            dockPanel.ShowDocumentIcon = true;
        }


        ~MainFrame()
        {
            Properties.Settings.Default.Save();
        }


        public void LoadResources()
        {
            ResourceLoader.LoadResources(this);
            _propertyWindow.LoadResources();
            
            foreach (PanelContainer panel in _panels)
                panel.LoadResources();
            
            _propertyWindow.TabText = _propertyWindow.Text;
        }


        private void OnActiveDocChanged(object sender, EventArgs e)
        {
            if (dockPanel.ActiveDocument == null)
                return;

            PanelContainer container = dockPanel.ActiveDocument as PanelContainer;
            if(container !=  null)
            container.OnSelectionChanged(this, null);
        }


        private void OnPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            _document.Modified = true;

            CustomTypeDescriptor descp = (CustomTypeDescriptor)_propertyWindow.SelectedObject;
            PanelContainer panelContainer = null;
            if (descp.HostControl is Form && e.ChangedItem.Label == "Icon")
            {
                Form panel = (Form)descp.HostControl;
                panelContainer = panel.ParentForm as PanelContainer;

                if (panelContainer != null)
                {
                    panelContainer.Icon = (Icon)e.ChangedItem.Value;
                    dockPanel.ShowDocumentIcon = true;
                    dockPanel.Invalidate();
                    dockPanel.Refresh();
                }
            }

            panelContainer = dockPanel.ActiveDocument as PanelContainer;

            if( panelContainer != null)
                panelContainer.PropertyChanged(descp.HostControl, e);
        }


        private void OnUpdateTimerTick(object sender, EventArgs e)
        {
            if (_panels.Count == 0)
            {
                DisableToolBar();
            }
            else
            {
                PanelContainer p = dockPanel.ActiveDocument as PanelContainer;
                if (p != null)
                {
                    toolStripButtonInsertImage.Enabled = true;
                    toolStripButtonInsertLabel.Enabled = true;
                    toolStripButtonInsertImage.Enabled = true;
                    toolStripButtonFrame.Enabled = true;

                    if (p.HasControlsSelected())
                    {
                        toolStripButtonAlignLefts.Enabled = true;
                        toolStripButtonAlignRights.Enabled = true;
                        toolStripButtonAlignsTop.Enabled = true;
                        toolStripButtonAlignBottoms.Enabled = true;
                        toolStripButtonSameWidth.Enabled = true;
                        toolStripButtonSameHeight.Enabled = true;
                        toolStripButtonBringToFront.Enabled = true;
                        toolStripButtonSendToBack.Enabled = true;
                        toolStripButtonSameDistanceH.Enabled = true;
                        toolStripButtonSameDistanceV.Enabled = true;
                    }
                    else
                    {
                        toolStripButtonAlignLefts.Enabled = false;
                        toolStripButtonAlignRights.Enabled = false;
                        toolStripButtonAlignsTop.Enabled = false;
                        toolStripButtonAlignBottoms.Enabled = false;
                        toolStripButtonSameWidth.Enabled = false;
                        toolStripButtonSameHeight.Enabled = false;
                        toolStripButtonBringToFront.Enabled = false;
                        toolStripButtonSendToBack.Enabled = false;
                        toolStripButtonSameDistanceH.Enabled = false;
                        toolStripButtonSameDistanceV.Enabled = false;
                    }
                }
                else
                {
                    DisableToolBar();
                }
            }

            foreach (PanelContainer panelContainer in _panels)
            {
                if (!panelContainer.IsActivated)
                    continue;

                toolStripDelete.Enabled = (panelContainer.Panel.Controls.Count == 0);
            }
        }


        private void DisableToolBar()
        {
            toolStripButtonFrame.Enabled = false;
            toolStripDelete.Enabled = false;
            toolStripButtonInsertImage.Enabled = false;
            toolStripButtonInsertLabel.Enabled = false;
            toolStripButtonInsertImage.Enabled = false;
            toolStripButtonAlignLefts.Enabled = false;
            toolStripButtonAlignRights.Enabled = false;
            toolStripButtonAlignsTop.Enabled = false;
            toolStripButtonAlignBottoms.Enabled = false;
            toolStripButtonSameWidth.Enabled = false;
            toolStripButtonSameHeight.Enabled = false;
            toolStripButtonBringToFront.Enabled = false;
            toolStripButtonSendToBack.Enabled = false;
            toolStripButtonSameDistanceH.Enabled = false;
            toolStripButtonSameDistanceV.Enabled = false;
        }


        public void AddControls( List<Control> controls )
        {            
            if (controls.Count == 0)
                return;

            PanelContainer panelContainer = NewPanel();

            foreach (Control ctrl in controls)
                panelContainer.AddControl(ctrl);
        }


        public void RemoveControls( List<Control> controls )
        {
            Form panel;

            foreach (Control ctrl in controls)
            {
                for (int index = 0; index < dockPanel.Contents.Count; ++index)
                {
                    PanelContainer panelContainer = dockPanel.Contents[index] as PanelContainer;
                    
                    if (panelContainer == null)
                        continue;

                    panel = panelContainer.Panel;

                    for (int i = 0; i < panel.Controls.Count; i++)
                    {
                        if (panel.Controls[i] == ctrl)
                        {
                            panelContainer.RemoveControl(ctrl);
                            i--;
                        }
                    }

                    
                    if (panel.Controls.Count == 0)
                    {
                        OnSelectionChanged(null);

                        panelContainer.Close();
                        index--;
                    }                    
                }
            }
        }


        private PanelContainer CreatePanelContainer(Form panel)
        {
            PanelContainer panelContainer = new PanelContainer(panel);
            panelContainer.Panel.TextChanged += new EventHandler(OnPanelTextChanged);
            panelContainer.OnModify += new PanelContainer.ModifyDelegate(OnPanelContainerModify);
            _panels.Add(panelContainer);
            _maxPanelID++;
            panelContainer.ID = _maxPanelID;
            panelContainer.Icon = panel.Icon;

            ShowPanelContainer(panelContainer);
            return panelContainer;
        }


        private void OnPanelContainerModify(object sender, EventArgs e)
        {
            _document.Modified = true;
        }


        private PanelContainer NewPanel()
        {
            _panelCount++;
            
            Form panel = new Form();
            panel.Text = "Panel " + _panelCount.ToString();
            panel.Tag = _panelData;
            panel.Icon = Resource.Document;
            PanelContainer panelContainer = CreatePanelContainer(panel);
            _document.Modified = true;
            return panelContainer;
        }


        private void OnPanelTextChanged(object sender, EventArgs e)
        {
            Form panel = (Form)sender;
            PanelContainer container = panel.ParentForm as PanelContainer;
            
            if (container == null)
                return;

            panel.Name = panel.Text;
            container.TabText = panel.Text;
        }


        private void ShowPanelContainer(PanelContainer panelContainer)
        {
            panelContainer.SelectionChanged += new ObjectSelectionChanged(OnSelectionChanged);
            panelContainer.WindowState = FormWindowState.Normal;
            panelContainer.AutoScroll = true;
            panelContainer.TopLevel = false;
            panelContainer.Show(dockPanel);
            panelContainer.DockState = DockState.Document;
            panelContainer.Init();
            panelContainer.Activate();
        }


        private void OnSelectionChanged(CustomTypeDescriptor descr)
        {
            _propertyWindow.SelectedObject = descr;
        }


        private void DeletePanel()
        {
            PanelContainer panelContainer = (dockPanel.ActiveContent as PanelContainer);

            if (panelContainer == null)
                return;            

            if (panelContainer.Panel.Controls.Count != 0)
                return;

            _panels.Remove(panelContainer);
            panelContainer.Close();
            _document.Modified = true;
        }


        private void RemoveXMLPanels(XmlElement xmlGUI)
        {
            XmlElement xmlPanel;
            XmlNode node;

            for (int i = 0; i < xmlGUI.ChildNodes.Count; i++ )
            {
                node = xmlGUI.ChildNodes[i];

                xmlPanel = (node as XmlElement);

                if (xmlPanel == null)
                    continue;

                if (XmlHelper.GetObjectID(xmlPanel) == 0)
                    continue;

                _document.RemoveXmlObject(xmlPanel);
                i--;
            }
        }


        public void Save()
        {
            XmlElement xmlGUI = XmlHelper.GetChildByType(_document.XmlDoc.DocumentElement, "GUI");

            if (dockPanel.ActiveDocument != null)
                _activePanelText = ((DockContent)dockPanel.ActiveDocument).TabText;


            //Remove the old panels.
            RemoveXMLPanels(xmlGUI);

            List<Control> controls = new List<Control>();
            //Save the panels.
            
            for(int i = 0; i< dockPanel.Contents.Count; ++i)
            {
                PanelContainer panelContainer  =  dockPanel.Contents[i] as PanelContainer;
                
                if( panelContainer == null)
                    continue;

                Form panel = panelContainer.Panel;

                XmlElement xmlPanel = _document.CreateXmlObject(xmlGUI, "Panel", "Panel");
                XmlHelper.SetParam(xmlPanel, "panelData", "string", ControlSurrogate.SerializeToString(panel, _panelFilter));
                XmlHelper.SetParamNumber(xmlPanel, "ID", "uint32_t", panelContainer.ID);
                
                controls.Clear();
                //Save the controls 
                
                //First remove add the controls to a list
                foreach( Control ctrl in panel.Controls)
                    controls.Add(ctrl);

                foreach (Control ctrl in controls)
                {
                    //Remove the control from the panel => reason the visible flag need the original state
                    if( !this.Visible)
                        panel.Controls.Remove(ctrl);

                    Point scrollPos = panel.AutoScrollPosition;
                    Point ctrlPos = new Point(ctrl.Left, ctrl.Top);
                    Point curCtrlPos = ctrlPos;

                    ctrlPos.X -= scrollPos.X;
                    ctrlPos.Y -= scrollPos.Y;

                    ctrl.Left = ctrlPos.X;
                    ctrl.Top = ctrlPos.Y;

                    //Save the control
                    ControlData ctrlData  = (ControlData) ctrl.Tag;
                    string data = ControlSurrogate.SerializeToString(ctrl, ctrlData.PropertyFilter);
                    XmlElement xmlCtrl      = XmlHelper.CreateElement(xmlPanel, "string", "ctrlData", data);
                    XmlAttribute ctrlType   = _document.XmlDoc.CreateAttribute("ctrlType");
                    ctrlType.Value          = ctrl.GetType().FullName;
                    xmlCtrl.Attributes.Append(ctrlType);

                    ctrl.Left = curCtrlPos.X;
                    ctrl.Top = curCtrlPos.Y;

                    //Add the control back to the panel
                    if(!this.Visible)
                        panelContainer.AddControl(ctrl);
                }
            }

            SaveState();
        }


        private string GetNewTypeName(string oldTypeName)
        {
            if (_typeMappingTable.Contains(oldTypeName))
                return GetNewTypeName((string)_typeMappingTable[oldTypeName]);

            return oldTypeName;
        }


        public void LoadPanels(Document document)
        {
            _panels.Clear();
            _maxPanelID = 0;
            _document   = document;
            XmlElement xmlGUI     = XmlHelper.GetChildByType(_document.XmlDoc.DocumentElement, "GUI");
            dockPanel.Visible = false;
            
            PanelContainer active = null;

            //Load the panels. 
            foreach( XmlNode node in xmlGUI.ChildNodes )
            {
                XmlElement xmlPanel = (node as XmlElement);
                
                if( xmlPanel.GetAttribute("type") != "Panel" )
                    continue;
                
                string base64Data = XmlHelper.GetParam(xmlPanel, "panelData");
                Form panel= (Form)ControlSurrogate.DeserializeFromString(base64Data, typeof(Form), _typeMappingTable);

                if (panel == null)
                    continue;

                if (panel.Tag == null)
                    panel.Tag = _panelData;

                PanelContainer panelContainer = CreatePanelContainer(panel);
                panelContainer.ID = (int) XmlHelper.GetParamNumber(xmlPanel, "ID");

                if (_activePanelText == panelContainer.TabText)
                    active = panelContainer;
                
                //Load the controls
                foreach (XmlNode xmlCtrlNode in xmlPanel.ChildNodes)
                {
                    XmlElement xmlCtrl = (xmlCtrlNode as XmlElement);
                    
                    if (xmlCtrl == null)
                        return;
                    
                    if (xmlCtrl.Attributes["name"].Value != "ctrlData")
                        continue;

                    string typeName = GetNewTypeName(xmlCtrl.GetAttribute("ctrlType"));
                    Type ctrlType = ControlSurrogate.GetLoadedTypeByName(typeName);
                    if (typeName == "Mp.Visual.Digital.DigitalMeter1Block")
                    {
                        int iu = 0;
                        iu++;
                    }

                    Control control = (Control)ControlSurrogate.DeserializeFromString(xmlCtrl.InnerText, ctrlType, _typeMappingTable);

                    if( control == null)
                        continue;

                    ControlData ctrlData = (ControlData)control.Tag;

                    if (ctrlData.StationId != 0)
                        AppendControl(control);

                    panelContainer.AddControl(control);
                }
            }

            _maxPanelID = 0;

            foreach (PanelContainer p in _panels)
                _maxPanelID = Math.Max(p.ID, _maxPanelID);

            dockPanel.Visible = true;

            if (active != null)
                active.Activate();
             
            LoadState();
            dockPanel.ShowDocumentIcon = true;
        }


        public void Clear()
        {
            _panels.Clear();

            for(int  i = 0; i < dockPanel.Contents.Count; ++i)
            {
                PanelContainer panelContainer = dockPanel.Contents[i] as PanelContainer;
                
                if (panelContainer == null)
                    continue;

                panelContainer.Close();
                i--;
            }
        }


        private void OnNewClick(object sender, EventArgs e)
        {
            NewPanel();
        }
       

        private void OnDeleteClick(object sender, EventArgs e)
        {
            DeletePanel();
        }


        private void OnInsertLabelClick(object sender, EventArgs e)
        {
            PanelContainer panelContanier = dockPanel.ActiveContent as PanelContainer;
            
            if (panelContanier == null)
                return;            

            Label label = new Label();
            label.Text = "Label";
            ControlData ctrlData = new ControlData();
            ctrlData.StationId = 0;
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Text");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("ForeColor");
            ctrlData.PropertyFilter.Add("Font");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Tag");
            label.Tag = ctrlData;
            
            //Append the label to the form.
            panelContanier.AddControl(label);
            label.BringToFront();
        }


        private void OnInsertImageClick(object sender, EventArgs e)
        {
            if (dockPanel.ActiveContent == null)
                return;

            OpenFileDialog dlg = new OpenFileDialog();

            dlg.CheckFileExists = true;
            dlg.Filter = "*.bmp|*.bmp|*.jpg|*.jpg|*.*|*.*";
            
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            Image image;
            try
            {
                image = Image.FromFile(dlg.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            PictureBox imageBox = new PictureBox();
            imageBox.Image = image;
            
            //Initilize the control data
            ControlData ctrlData = new ControlData();
            ctrlData.StationId = 0;
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Tag");
            ctrlData.PropertyFilter.Add("Image");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("SizeMode");
           
            imageBox.Tag = ctrlData;
            PanelContainer panelContainer = dockPanel.ActiveContent as PanelContainer;
            
            panelContainer.AddControl(imageBox);
            imageBox.BringToFront();

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);        
            _propertyWindow.Show(dockPanel);
            _propertyWindow.DockState = DockState.DockRight;
        }


        private void LoadState()
        {
            XmlElement xmlGUI = XmlHelper.GetChildByType(_document.XmlDoc.DocumentElement, "GUI");
            string state = XmlHelper.GetParam(xmlGUI, "designerState");
                
            if (state == "")
                return;

            this.Controls.Remove(dockPanel);
            dockPanel = new DockPanel();
            dockPanel.ActivateOnDragOver = false;
            dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            dockPanel.Dock = DockStyle.Fill;
            this.Controls.Add(dockPanel);
            dockPanel.BringToFront();
            dockPanel.ActiveDocumentChanged += new EventHandler(OnActiveDocChanged);
   
            this.Activate();
            try
            {
                using (MemoryStream mm = new MemoryStream())
                {
                    StreamWriter sw = new StreamWriter(mm);
                    sw.Write(state);
                    sw.Write(0);

                    sw.Flush();
                    mm.Flush();

                    mm.Seek(0, SeekOrigin.Begin);

                    dockPanel.LoadFromXml(mm, new DeserializeDockContent(GetDockContent), true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            foreach (PanelContainer p in _panels)
                if(p.Visible)
                    p.Show(dockPanel);

        }
        

        private void SaveState()
        {
            XmlElement xmlGUI = XmlHelper.GetChildByType(_document.XmlDoc.DocumentElement, "GUI");

            //Save the docking state
            using (MemoryStream mm = new MemoryStream())
            {
                dockPanel.SaveAsXml(mm, Encoding.UTF8, true);

                mm.Flush();
                mm.Seek(0, SeekOrigin.Begin);

                StreamReader sr = new StreamReader(mm);
                string state = sr.ReadToEnd();
                XmlHelper.SetParam(xmlGUI, "designerState", "string",state);
            }

            Properties.Settings.Default.Save();
        }
    

        private DockContent GetDockContent(string padTypeName)
        {
            string[] array = padTypeName.Split('\n');
            try
            {
                switch (array[0])
                {
                    case "Mp.Scheme.Designer.PropertieWindow":
                        return _propertyWindow;

                    case "Mp.Scheme.Designer.PanelContainer":
                        return GetPanelByID(Convert.ToInt32(array[1]));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }


        private PanelContainer GetPanelByID(int ID)
        {
            foreach (PanelContainer p in _panels)
            {
                if (p.ID == ID)
                    return p;
            }

            return null;
        }


        private void OnAlignLeftsClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if( p == null)
                return;

            p.AlignLefts();
        }


        private void OnAlignRightsClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if (p == null)
                return;

            p.AlignRights();
        }


        private void OnAlignsTopClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if (p == null)
                return;

            p.AlignTops();
        }


        private void OnAlignBottomsClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if (p == null)
                return;

            p.AlignBottoms();
        }


        private void OnSameWidthClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if (p == null)
                return;

            p.SameWidth();
        }


        private void OnSameHeightClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if (p == null)
                return;

            p.SameHeight();
        }


        private void OnBringToFrontClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if (p == null)
                return;

            p.BringSelectionToFront();
        }


        private void OnSendToBackClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if (p == null)
                return;

            p.SendSelectionToBack();
        }


        private void OnAddFrameClick(object sender, EventArgs e)
        {
            UserControl frame = new UserControl();

            //Initilize the control data
            ControlData ctrlData = new ControlData();
            ctrlData.StationId = 0;
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Tag");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("BackColor");

            frame.Tag = ctrlData;
            frame.BorderStyle = BorderStyle.FixedSingle;

            PanelContainer panelContainer = dockPanel.ActiveContent as PanelContainer;
            
            if (panelContainer == null)
                return;

            panelContainer.AddControl(frame);
            frame.BringToFront();
        }


        private void OnSameDistanceHClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if (p == null)
                return;

            p.MakeHSpacingEgual();
        }


        private void OnSameDistanceVClick(object sender, EventArgs e)
        {
            PanelContainer p = dockPanel.ActiveDocument as PanelContainer;

            if (p == null)
                return;

            p.MakeVSpacingEgual();
        }


        private void OnOptionsClick(object sender, EventArgs e)
        {
            DesignerOptionDlg dlg = new DesignerOptionDlg();

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            
            foreach (PanelContainer panel in _panels)                
                panel.UpdateOptions();
        }


        protected override void OnClosed(EventArgs e)
        {
            Properties.Settings.Default.Save();
            base.OnClosed(e);        
        }
    }
}

