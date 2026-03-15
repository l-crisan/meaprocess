using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Windows.Forms;

using LBSoft.IndustrialCtrls.Knobs;
using Mp.Scheme.Sdk;
using Mp.Components;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    internal class KnobPS : VisualPS
    {
        private Assembly _myControlsAssembly;

        public KnobPS()
        {
            base.Group = StringResource.Control;
            base.Type = "PS_INPUT";
            base.Text = StringResource.Knob;
            base.Symbol = Mp.Scheme.Win.Images.Knob;
            base.Icon = Mp.Scheme.Win.Images.KnobIcon;
            base.SubType = "Mp.Runtime.Sdk.KnobPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\LBIndustrialCtrls.dll");
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
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");            
            ctrlData.PropertyFilter.Add("KnobColor");
            ctrlData.PropertyFilter.Add("ScaleColor");
            ctrlData.PropertyFilter.Add("IndicatorColor");
            ctrlData.PropertyFilter.Add("IndicatorOffset");
            ctrlData.PropertyFilter.Add("MinValue");
            ctrlData.PropertyFilter.Add("MaxValue");
            ctrlData.PropertyFilter.Add("Value");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("Dock");            
            ctrlData.PropertyFilter.Add("StepValue");
           

            LBKnob knob = new LBKnob();
            knob.MinValue = 0f;
            knob.MaxValue = 10.0f;
            knob.StepValue = 0.25f;
            knob.Name = this.Text;            
            knob.Tag = ctrlData;
            
            RegisterControl(knob);

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_OUTPUT", false, true);

            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);

            ctrlData.SignalListId = XmlHelper.GetObjectID(port.SignalList);
            AddPort(port);   
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
            
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document, 2);
            dlg.ShowDialog();
            UpdateControlData();
        }
        
        public override void OnSaveXml()
        {
            UpdateControlData();

            base.OnSaveXml();
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        private void UpdateControlData()
        {
            Port port = OutputPorts[0];

            XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Signal");            
            if (xmlSignal != null)
            {
                LBKnob knob = (LBKnob)Controls[0];
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(knob.MinValue));
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(knob.MaxValue));
            }
        }
    }
}
