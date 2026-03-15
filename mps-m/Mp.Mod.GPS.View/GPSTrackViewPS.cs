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
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Scheme.Sdk.Ui;
using Mp.Visual.GPS;
using Mp.Utils;

namespace Mp.Mod.GPS.View
{
    class GPSTrackViewPS : VisualPS
    {
        private TrackViewCtrl _view;
        Assembly _asm;

        public GPSTrackViewPS()
        {
            base.Type = "Mp.PS.SysOut";
            this.Text = StringResource.TrackView;
            base.Group = "GPS";
            base.Symbol = Resource.Map;
            base.Icon = Resource.MapIcon;
            base.SubType = "Mp.Runtime.Sdk.GPSTrackViewPS";

        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.TrackView;
            base.Group = "GPS";
        }

        public override string Description
        {
            get
            {
                return StringResource.TrackViewPsDescription;
            }
        }
        public override void OnDefaultInit()
        {
            _asm = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Mp.Visual.Digital.dll"));
            InitControl();

            base.OnDefaultInit();

            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }
        
        protected override void OnUpdateSignalList()
        {
            base.OnUpdateSignalList();

            for (int i = 0; i < XmlRep.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement)XmlRep.ChildNodes[i];

                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");

                if (name != "sigMaping")
                    continue;

                string[] mapping = xmlElement.InnerText.Split('/');
                uint id = Convert.ToUInt32(mapping[0]);
                if (!IsSignalInPortsAvailable(id, true, null))
                {
                    XmlRep.RemoveChild(xmlElement);
                    --i;
                }
            }
        }

        public override void OnLoadXml()
        {
            _asm = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Mp.Visual.Digital.dll"));
            base.OnLoadXml();
        }

        public override void OnSaveXml()
        {
            base.OnSaveXml();
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            GPSTrackViewDlg dlg = new GPSTrackViewDlg(Document, this.XmlRep, InputPorts[0].SignalList);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(this.XmlRep, "name");
        }

        private void InitControl()
        {
            //Set the Control. 
            ControlData chartCtrl = new ControlData();
            chartCtrl.PropertyFilter = new List<string>();
            chartCtrl.PropertyFilter.Add("Left");
            chartCtrl.PropertyFilter.Add("Top");
            chartCtrl.PropertyFilter.Add("Width");
            chartCtrl.PropertyFilter.Add("Height");
            chartCtrl.PropertyFilter.Add("BackColor");
            chartCtrl.PropertyFilter.Add("BorderStyle");
            chartCtrl.PropertyFilter.Add("AddPosOnlyStatus");
            chartCtrl.PropertyFilter.Add("Dock");

            _view = new TrackViewCtrl();

            _view.Tag = chartCtrl;
            List<Control> controls = new List<Control>();
            controls.Add(_view);
            base.RegisterControls(controls);
        }

        public override void AppendControl(Control control)
        {
            base.AppendControl(control);
            _view = (TrackViewCtrl)control;
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            Port dataInPort = InputPorts[0];

            if (dataInPort.SignalList == null)
            {
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.TwoSigErr,this.Text), ValidationInfo.InfoType.Error));
                return;
            }

            bool lo = false;
            bool la = false;

            for (int i = 0; i < XmlRep.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement)XmlRep.ChildNodes[i];

                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");

                if (name != "sigMaping")
                    continue;

                string[] mapping = xmlElement.InnerText.Split('/');
                int index = Convert.ToInt32(mapping[1]);

                if (index == 0)
                    lo = true;

                if (index == 1)
                    la = true;
            }

            if (!la || !lo)
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.LonLatMapErr,this.Text), ValidationInfo.InfoType.Error));
        }
    }
}
