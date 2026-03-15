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
using System.Xml;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class KnobPS : VisualPS
    {
        private Assembly _myControlsAssembly;

        public KnobPS()
        {
            base.Group = StringResource.Control;
            base.Type = "Mp.PS.SysIn";
            base.Text = StringResource.Knob;
            base.Symbol = Images.Knob;
            base.Icon = Images.KnobIcon;
            base.SubType = "Mp.Runtime.Sdk.KnobPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Analog.dll");
        }
        
        public override void OnLoadResources()
        {
            base.Text = StringResource.Knob;
            base.Group = StringResource.Control;
        }

        public override string Description
        {
            get
            {
                return StringResource.KnobPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            //Conntrol
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter = GetPropertyFilter();

            Mp.Visual.Analog.Knob knob = new Mp.Visual.Analog.Knob();
            knob.MinValue = 0f;
            knob.MaxValue = 10.0f;
            knob.StepValue = 0.25f;
            knob.Name = this.Text;            
            knob.Tag = ctrlData;
            
            RegisterControl(knob);

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);

            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);

            ctrlData.SignalListId = XmlHelper.GetObjectID(port.SignalList);
            AddPort(port);   
        }


        public override void OnSaveXml()
        {
            Port port = OutputPorts[0];

            Mp.Visual.Analog.Knob ctrl = Controls[0] as Mp.Visual.Analog.Knob;
            XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Mp.Sig");

            if (xmlSignal != null)
            {
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", ctrl.MinValue);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", ctrl.MaxValue);
            }
            base.OnSaveXml();
        }
        
        private static List<string> GetPropertyFilter()
        {
            List<string>  propertyFilter = new List<string>();
            propertyFilter.Add("Left");
            propertyFilter.Add("Top");
            propertyFilter.Add("Width");
            propertyFilter.Add("Height");
            propertyFilter.Add("KnobColor");
            propertyFilter.Add("ScaleColor");
            propertyFilter.Add("IndicatorColor");
            propertyFilter.Add("IndicatorOffset");
            propertyFilter.Add("MinValue");
            propertyFilter.Add("MaxValue");
            propertyFilter.Add("Value");
            propertyFilter.Add("BorderStyle");
            propertyFilter.Add("BackColor");
            propertyFilter.Add("Dock");
            propertyFilter.Add("StepValue");
            return propertyFilter;
        }
   
        private void InitMenuForPort(Port port)
        {
            port.ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem menuItem = new ToolStripMenuItem("Properties...");
            menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            menuItem.Click += new System.EventHandler(this.OnPropertyDataPort);
            port.ContextMenuStrip.Items.Add(menuItem);
        }

        private void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            OnPortDoubleClick(port);
        }

        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);
            
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document, SignalDataType.LREAL);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Mp.Sig");
                if (xmlSignal != null)
                {
                    Mp.Visual.Analog.Knob knob = (Mp.Visual.Analog.Knob)Controls[0];
                    knob.MinValue = (float) XmlHelper.GetParamDouble(xmlSignal, "physMin");
                    knob.MaxValue = (float) XmlHelper.GetParamDouble(xmlSignal, "physMax");
                }

            }
        }

        public override void AppendControl(Control control)
        {
            base.AppendControl(control);
            UpdateSignalData();
        }

        private void UpdateSignalData()
        {
            if (OutputPorts.Count == 0)
                return;

            Port port = OutputPorts[0];
            XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Mp.Sig");
            Mp.Visual.Analog.Knob knob = (Mp.Visual.Analog.Knob)Controls[0];
            knob.MinValue = (float)XmlHelper.GetParamDouble(xmlSignal, "physMin");
            knob.MaxValue = (float)XmlHelper.GetParamDouble(xmlSignal, "physMax");
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 770);
        }
    }
}
