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
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class XYChartPS  : VisualPS
    {
        private Mp.Visual.XYChart.Chart _chart;
        private Assembly _myControlsAssembly;

        public XYChartPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.XYChart;
            base.Group = StringResource.DisplayBlock;
            base.Symbol = Images.XYChart;
            base.Icon = Images.XYChartIcon;
            base.SubType = "Mp.Runtime.Sdk.XYChartPS";
            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath+ "\\Mp.Visual.XYChart.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.XYChart;
            base.Group = StringResource.DisplayBlock;
        }

        public override string Description
        {
            get
            {
                return StringResource.XYChartPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            InitControl();

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

            //Reset port.
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset + DistanceBetweenPort)), "Mp.Port.In", true, false);
            port.ConnectorBrush = new SolidBrush(Color.DarkGreen);
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
            chartCtrl.PropertyFilter.Add("LineColor");
            chartCtrl.PropertyFilter.Add("PointColor");
            chartCtrl.PropertyFilter.Add("AxisColor");
            chartCtrl.PropertyFilter.Add("XMinimum");
            chartCtrl.PropertyFilter.Add("XMaximum");
            chartCtrl.PropertyFilter.Add("YMinimum");
            chartCtrl.PropertyFilter.Add("YMaximum");
            chartCtrl.PropertyFilter.Add("Title");
            chartCtrl.PropertyFilter.Add("XAxisDivision");
            chartCtrl.PropertyFilter.Add("XAxisPrecision");
            chartCtrl.PropertyFilter.Add("YAxisDivision");
            chartCtrl.PropertyFilter.Add("YAxisPrecision");
            chartCtrl.PropertyFilter.Add("GridLineStyle");
            chartCtrl.PropertyFilter.Add("GridXDevision");
            chartCtrl.PropertyFilter.Add("GridYDevision");
            chartCtrl.PropertyFilter.Add("GridColor");
            chartCtrl.PropertyFilter.Add("NoOfPoints");
            chartCtrl.PropertyFilter.Add("DrawPoint");
            chartCtrl.PropertyFilter.Add("DrawLine");
            chartCtrl.PropertyFilter.Add("ShowControlPanel");
            chartCtrl.PropertyFilter.Add("Dock");
            chartCtrl.PropertyFilter.Add("ReferenceCurves");
            chartCtrl.PropertyFilter.Add("Editable");

            _chart = new Visual.XYChart.Chart();
            _chart.Tag = chartCtrl;
            _chart.Text = Text;
            _chart.Name = Text;
            List<Control> controls = new List<Control>();
            controls.Add(_chart);
            base.RegisterControls(controls);
        }

        public override void AppendControl(Control control)
        {            
            _chart = (Visual.XYChart.Chart) control;
            _chart.Reset();
            
            ControlData ctrlData = (ControlData)_chart.Tag;

            UpdateProperty(ctrlData, "ReferenceCurves");
            UpdateProperty(ctrlData, "Editable");

            base.AppendControl(control);
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 650); 
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            Port dataInPort = InputPorts[0];

            if (base.Controls.Count == 0)
            {
                InitControl();
                valInfoList.Add( new ValidationInfo("A new XY Chart control was created. The old coudn't be loaded because a new version of this control is available.",ValidationInfo.InfoType.Warning)); 
            }

            if (dataInPort.SignalList == null)
            {
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.TwoSigErr,this.Text), ValidationInfo.InfoType.Error));
                return;
            }

            if (dataInPort.SignalList.ChildNodes.Count < 2)
            {
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.TwoSigErr, this.Text), ValidationInfo.InfoType.Error));
            }
        }
    }
}
