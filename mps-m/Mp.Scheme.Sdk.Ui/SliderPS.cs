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
    public class SliderPS : VisualPS
    {
        private bool _ignoreEvent = false;

        public SliderPS()
        {
            base.Group = StringResource.Control;
            base.Type = "Mp.PS.SysIn";
            base.Text = StringResource.Slider;
            base.Symbol = Images.Slider;
            base.Icon = Images.SliderIcon;
            base.SubType = "Mp.Runtime.Sdk.SliderPS";
        }

        public override string Description
        {
            get
            {
                return StringResource.SliderPsDescription;
            }
        }


        public override void OnLoadResources()
        {
            base.Text = StringResource.Slider;
            base.Group = StringResource.Control;
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
            slider.Resize += new EventHandler(OnControlResize);
            RegisterControl(slider);

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);

            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);

            ctrlData.SignalListId = XmlHelper.GetObjectID(port.SignalList);
            AddPort(port);

            UpdateSignalData(OutputPorts[0]);
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
            TrackBar slider = (TrackBar)Controls[0];
            
            XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Mp.Sig");
            if (xmlSignal != null)
            {
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", slider.Minimum);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", slider.Maximum);
            }
            base.OnSaveXml();
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        public override void AppendControl(Control control)
        {
            base.AppendControl(control);

            control.Resize += new EventHandler(OnControlResize);
            if(OutputPorts.Count != 0)
                UpdateSignalData(OutputPorts[0]);
        }

        private void OnControlResize(object sender, EventArgs e)
        {
            if (_ignoreEvent)
                return;
            try
            {
                if (!(sender is TrackBar))
                    return;

                TrackBar ctrl = (TrackBar)sender;

                if (!ctrl.IsHandleCreated)
                    return;

                _ignoreEvent = true;
                RightToLeft old = ctrl.RightToLeft;

                if (old == RightToLeft.No)
                    ctrl.RightToLeft = RightToLeft.Yes;
                else
                    ctrl.RightToLeft = RightToLeft.No;

                ctrl.RightToLeft = old;
            }
            catch (Exception)
            {
            }

            _ignoreEvent = false;
        }

        private void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document, SignalDataType.DINT);
            if(dlg.ShowDialog() == DialogResult.OK)
                UpdateSignalData(port);
        }

        private void UpdateSignalData(Port port)
        {
            XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Mp.Sig");
            TrackBar slider = (TrackBar)Controls[0];

            if (xmlSignal != null)
            {
                slider.Minimum = (int)XmlHelper.GetParamDouble(xmlSignal, "physMin");
                slider.Maximum = (int)XmlHelper.GetParamDouble(xmlSignal, "physMax");
            }
        }

        public override void OnPortDoubleClick(Port port)
        {
            OnPropertyDataPort(null, null);
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 790);
        }
    }
}
