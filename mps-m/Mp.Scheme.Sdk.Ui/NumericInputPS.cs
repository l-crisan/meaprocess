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
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class NumericInputPS : VisualPS
    {
        public NumericInputPS()
        {
            base.Group = StringResource.Control;
            base.Type = "Mp.PS.SysIn";
            base.Text = StringResource.NumericInput;
            base.Symbol = Images.NumericInput;
            base.Icon = Images.NumericInputIcon;
            base.SubType = "Mp.Runtime.Sdk.NumericInputPS";
        }

        public override string Description
        {
            get
            {
                return StringResource.NumericInputPsDescription;
            }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.NumericInput;
            base.Group = StringResource.Control;
        }

        public override void AppendControl(Control control)
        {
            if (control is Mp.Visual.Digital.NumericInput)
            {
                Mp.Visual.Digital.NumericInput inputBox = (Mp.Visual.Digital.NumericInput)control;
                ControlData ctrlData = (ControlData)inputBox.Tag;
                ctrlData.PropertyFilter = GetPropertyFilter();  
            }
            base.AppendControl(control);
        }

        public override void OnDefaultInit()
        {
            //Conntrol
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter = GetPropertyFilter();

            Mp.Visual.Digital.NumericInput ni = new Mp.Visual.Digital.NumericInput();
            ni.Value = 10;
            ni.Tag = ctrlData;
            ni.Left = 120;
            ni.Name = this.Text;            

            List<Control> controls = new List<Control>();
            controls.Add(ni);

            ctrlData = new ControlData();
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Text");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Visible");
            ctrlData.PropertyFilter.Add("Dock");
            Label nl = new Label();
            nl.Text = this.Text;
            nl.Tag = ctrlData;
            nl.Name = this.Text;
            controls.Add(nl);
            RegisterControls(controls);

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);

            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);

            ctrlData.SignalListId = XmlHelper.GetObjectID(port.SignalList);
            AddPort(port);   
        }

        private static List<string> GetPropertyFilter()
        {
            List<string> propertyFilter = new List<string>();
            propertyFilter.Add("Left");
            propertyFilter.Add("Top");
            propertyFilter.Add("Width");
            propertyFilter.Add("Height");
            propertyFilter.Add("NumberType");
            propertyFilter.Add("Minimum");
            propertyFilter.Add("Maximum");
            propertyFilter.Add("Value");
            propertyFilter.Add("BorderStyle");
            propertyFilter.Add("TextAlign");
            propertyFilter.Add("Dock");
            return propertyFilter;
        }

        private void InitMenuForPort(Port port)
        {
            port.ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.PropertiesMenu);
            menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            menuItem.Click += new System.EventHandler(this.OnPropertyDataPort);
            port.ContextMenuStrip.Items.Add(menuItem);
        }
        
        private void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document,SignalDataType.LREAL);

            if (dlg.ShowDialog() == DialogResult.OK)
                UpdateSignalData(port);
        }

        private void UpdateSignalData(Port port)
        {
            XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Mp.Sig");

            foreach (Control control in Controls)
            {
                if (control is Mp.Visual.Digital.NumericInput && xmlSignal != null)
                {
                    Mp.Visual.Digital.NumericInput ctrl = control as Mp.Visual.Digital.NumericInput;
                    ctrl.Minimum = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                    ctrl.Maximum = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                }
            }
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
            UpdateSignalData(OutputPorts[0]);
        }

        public override void OnSaveXml()
        {            
            Port port = OutputPorts[0];
            foreach (Control control in Controls)
            {
                if (control is Mp.Visual.Digital.NumericInput)
                {
                    Mp.Visual.Digital.NumericInput ctrl = control as Mp.Visual.Digital.NumericInput;
                    XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Mp.Sig");

                    if (xmlSignal != null)
                    {
                        XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", ctrl.Minimum);
                        XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", ctrl.Maximum);
                    }
                }
            }
            base.OnSaveXml();
        }

        public override void OnPortDoubleClick(Port port)
        {
            OnPropertyDataPort(null, null);
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 800);
        }
    }
}
