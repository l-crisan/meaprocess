using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Windows.Forms;

using Mp.Scheme.Sdk;
using Mp.Components;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    internal class SliderPS : VisualPS
    {
        public SliderPS()
        {
            base.Group = StringResource.Control;
            base.Type = "PS_INPUT";
            base.Text = StringResource.Slider;
            base.Symbol = Mp.Scheme.Win.Images.Slider;
            base.Icon = Mp.Scheme.Win.Images.SliderIcon;
            base.SubType = "Mp.Runtime.Sdk.SliderPS";
        }

        public override string Description
        {
            get
            {
                return StringResource.SliderPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            TrackBar slider = new TrackBar();

            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");            
            ctrlData.PropertyFilter.Add("Orientation");
            ctrlData.PropertyFilter.Add("Minimum");
            ctrlData.PropertyFilter.Add("Maximum");
            ctrlData.PropertyFilter.Add("Value");
            ctrlData.PropertyFilter.Add("LargeChange");
            ctrlData.PropertyFilter.Add("RightToLeft");
            ctrlData.PropertyFilter.Add("SmallChange");
            ctrlData.PropertyFilter.Add("TickFrequency");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("Dock");

            slider.Tag = ctrlData;
            slider.Name = this.Text;

            RegisterControl(slider);

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

        public override void OnSaveXml()
        {
            Port port = OutputPorts[0];

            XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Signal");
            TrackBar slider = (TrackBar)Controls[0];
            if (xmlSignal != null)
            {
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(slider.Minimum));
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(slider.Maximum));
            }

            base.OnSaveXml();
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);            
        }

        private void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document, 9);
            dlg.ShowDialog();
        }

        public override void OnPortDoubleClick(Port port)
        {
            OnPropertyDataPort(null, null);
        }
    }
}
