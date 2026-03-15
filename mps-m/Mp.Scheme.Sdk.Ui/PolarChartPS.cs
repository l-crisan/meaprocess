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
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class PoolarChartPS : VisualPS
    {
        private Mp.Visual.PolarChart.PolarChart _chart;

        private Assembly _myControlsAssembly;

        public PoolarChartPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.PolarChart;
            base.Group = StringResource.DisplayBlock;
            base.Symbol = Images.PolarChartImg;
            base.Icon = Images.PolarChartIcon;
            base.SubType = "Mp.Runtime.Sdk.PolarChartPS";
            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.PolarChart.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.PolarChart;
            base.Group = StringResource.DisplayBlock;
        }

        public override string Description
        {
            get
            {
                return StringResource.PolarChartPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            InitControl();

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
        }


        public override void OnSaveXml()
        {
            base.OnSaveXml();
        }

        protected override void OnDocumentChanged()
        {
            base.OnDocumentChanged();

            if (_chart == null)
                return;

            if (InputPorts.Count == 0)
                return;

            Port port = InputPorts[0];

            if (port.SignalList == null)
                return;

            XmlElement xmlSignal = (XmlElement) port.SignalList.ChildNodes[0];
            
            if (xmlSignal == null)
                return;

            uint id = XmlHelper.GetObjectID(xmlSignal);

            if (id == 0)
                xmlSignal = Document.GetXmlObjectById(Convert.ToUInt32(xmlSignal.InnerText));

            if (xmlSignal == null)
                return;

            _chart.RadiusMinimum = XmlHelper.GetParamDouble(xmlSignal, "physMin");
            _chart.RadiusMaximum = XmlHelper.GetParamDouble(xmlSignal, "physMax");
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
            chartCtrl.PropertyFilter.Add("Dock");
            chartCtrl.PropertyFilter.Add("AxisColor");
            chartCtrl.PropertyFilter.Add("Angle");
            chartCtrl.PropertyFilter.Add("AngleDivision");
            chartCtrl.PropertyFilter.Add("RadiusDivision");
            chartCtrl.PropertyFilter.Add("RadiusPrecision");
            chartCtrl.PropertyFilter.Add("IndicatorColor");
            chartCtrl.PropertyFilter.Add("ForeColor");
            chartCtrl.PropertyFilter.Add("Titel");

            _chart = new Mp.Visual.PolarChart.PolarChart();
            _chart.Tag = chartCtrl;
            _chart.Text = Text;
            _chart.Name = Text;
            List<Control> controls = new List<Control>();
            controls.Add(_chart);
            base.RegisterControls(controls);
        }

        public override void AppendControl(Control control)
        {
            _chart = (Mp.Visual.PolarChart.PolarChart)control;
//            _chart.Reset();

            ControlData ctrlData = (ControlData)_chart.Tag;

            base.AppendControl(control);
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1360);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            Port dataInPort = InputPorts[0];

            if (base.Controls.Count == 0)
            {
                InitControl();
                valInfoList.Add(new ValidationInfo("A new Polar-Chart control was created. The old coudn't be loaded because a new version of this control is available.", ValidationInfo.InfoType.Warning));
            }

            if (dataInPort.SignalList == null)
            {
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.TwoSigErr, this.Text), ValidationInfo.InfoType.Error));
                return;
            }

            if (dataInPort.SignalList.ChildNodes.Count < 2)
            {
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.TwoSigErr, this.Text), ValidationInfo.InfoType.Error));
            }
        }
    }
}
