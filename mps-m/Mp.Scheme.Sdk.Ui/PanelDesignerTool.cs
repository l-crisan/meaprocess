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
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Mp.Visual.Diagram;
using Mp.Utils;
using Mp.Visual.Docking;

namespace Mp.Scheme.Sdk.Ui
{
    public class PanelDesignerTool : Mp.Scheme.Sdk.Tool
    {
        private Document _document;
        private Mp.Scheme.Designer.MainFrame _designer;
        private DockPanel _dockPanel;

        public PanelDesignerTool()
        {
            _designer = new Mp.Scheme.Designer.MainFrame();
            base.Name = "";
        }

        public override void OnCreate()
        {
            _designer = new Mp.Scheme.Designer.MainFrame();
        }
        
        public override void LoadResources()
        {
            if (_designer != null)
                _designer.LoadResources();
        }

        public override void OnLoadDocument(Document document, DockPanel dockPanel, Form mainFrame)
        {
            _dockPanel = dockPanel;

            DockContent active = (DockContent)_dockPanel.ActiveContent;

            Assembly[]  asm = AppDomain.CurrentDomain.GetAssemblies();

            base.OnLoadDocument(document,dockPanel,mainFrame);
            _document = document;
            UpdateRuntimeOptions();
            _designer.AppendControl += new Mp.Scheme.Designer.AppendControl(OnAppendControl);
            _designer.Show(dockPanel);
            _designer.DockState = DockState.Document;
            _designer.Visible = true;
            _designer.LoadPanels(_document);

            VisualPS.DesignerTool = this;

            if (active != null)
                active.Activate();
        }
        
        public override void OnClose()
        {
             _designer.Close();
            _designer = null;
        }

        private void UpdateRuntimeOptions()
        {
            XmlElement xmlGUI = XmlHelper.GetChildByType(_document.XmlDoc.DocumentElement, "GUI");

            if (XmlHelper.GetParamNumber(xmlGUI, "roptions") > 0)
                return;

            XmlHelper.SetParamNumber(xmlGUI, "showMenu", "uint8_t", 1);
            XmlHelper.SetParamNumber(xmlGUI, "showControlBar", "uint8_t", 1);
            XmlHelper.SetParamNumber(xmlGUI, "showStatusBar", "uint8_t", 1);
            XmlHelper.SetParamNumber(xmlGUI, "undockPanels", "uint8_t", 1);
            XmlHelper.SetParamNumber(xmlGUI, "starOnOpen", "uint8_t", 0);
            XmlHelper.SetParamNumber(xmlGUI, "fixedWinSize", "uint8_t", 0);
            XmlHelper.SetParamNumber(xmlGUI, "rwidth", "uint32_t", 800);
            XmlHelper.SetParamNumber(xmlGUI, "rheight", "uint32_t", 600);
            XmlHelper.SetParamNumber(xmlGUI, "roptions", "uint8_t", 1);
            XmlHelper.SetParamNumber(xmlGUI, "resetPropOnStart", "uint8_t", 0);
            XmlHelper.SetParamNumber(xmlGUI, "mandatoryFlagVisible","uint8_t",1);
            XmlHelper.SetParamNumber(xmlGUI, "closeOnStop", "uint8_t", 0);
            XmlHelper.SetParamNumber(xmlGUI, "editPropBt", "uint8_t", 1);
            XmlHelper.SetParamNumber(xmlGUI, "ctrlBarEditPropBt", "uint8_t", 1);
            XmlHelper.SetParamNumber(xmlGUI, "hideTabForPanal", "uint8_t", 0);
        }

        public override void OnExecute()
        {
            base.OnExecute();

            if (!_designer.Visible)
            {
                _designer.Show(_dockPanel);
                _designer.DockState = DockState.Document;
            }
            else
            {
                _designer.BringToFront();
            }
            _document.Modified = true;
        }
        
        public override void OnSaveDocument()
        {
            _designer.Save();
            base.OnSaveDocument();
        }
        
        public override void OnCloseDocument()
        {
            _designer.Clear();
            _designer.Visible = false;
            VisualPS.DesignerTool = null;     
            base.OnCloseDocument();
        }

        public void RemoveProcessStation(VisualPS station)
        {
            _designer.RemoveControls( station.Controls );
            station.Controls.Clear();
        }

        public void RemoveControl(Control control)
        {            
            List<Control> controls = new List<Control>();
            controls.Add(control);
            _designer.RemoveControls(controls);
        }

        public void AddControls(List<Control> controls)
        {
            DockContent active = (DockContent) _dockPanel.ActiveContent;

            _designer.AddControls(controls);

            if (active != null)
                active.Activate();
        }

        private void OnAppendControl(Control control)
        {
            ControlData ctrlData = (ControlData)control.Tag;
            foreach (DiagramWindow digram in _document.Diagrams)
            {
                foreach (RectangleShape station in digram.Diagram.Shapes)
                {
                    VisualPS visualPS = (station as VisualPS);

                    if (visualPS == null)
                        continue;

                    if (XmlHelper.GetObjectID(visualPS.XmlRep) == ctrlData.StationId)
                    {
                        visualPS.AppendControl(control);
                        break;
                    }
                }
            }
        }
    }
}
