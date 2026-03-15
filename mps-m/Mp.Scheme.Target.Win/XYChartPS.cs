using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using Mp.Components;
using Mp.Scheme.Sdk;
using Mp.XYChart;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    class XYChartPS  : VisualPS
    {
        private XYChart.Chart _chart;
        private Assembly _myControlsAssembly;

        public XYChartPS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = StringResource.XYChart;
            base.Group = StringResource.Display;
            base.Symbol = Mp.Scheme.Win.Images.XYChart;
            base.Icon = Mp.Scheme.Win.Images.XYChartIcon;
            base.SubType = "Mp.Runtime.Sdk.XYChartPS";
            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.XYChart.dll");
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
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_INPUT", true, false);
            AddPort(port);

            //Reset port.
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset + DistanceBetweenPort)), "PORT_INPUT", true, false);
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
            chartCtrl.PropertyFilter.Add("XAxisDevision");
            chartCtrl.PropertyFilter.Add("XAxisPrecision");
            chartCtrl.PropertyFilter.Add("YAxisDevision");
            chartCtrl.PropertyFilter.Add("YAxisPrecision");
            chartCtrl.PropertyFilter.Add("GridLineStyle");
            chartCtrl.PropertyFilter.Add("GridXDevision");
            chartCtrl.PropertyFilter.Add("GridYDevision");
            chartCtrl.PropertyFilter.Add("GridColor");
            chartCtrl.PropertyFilter.Add("NoOfPoints");
            chartCtrl.PropertyFilter.Add("DrawPoint");
            chartCtrl.PropertyFilter.Add("DrawLine");
            chartCtrl.PropertyFilter.Add("ShowControlPanel");
            chartCtrl.PropertyFilter.Add("SpectrumDiagram");
            chartCtrl.PropertyFilter.Add("Dock");
            chartCtrl.PropertyFilter.Add("ReferenceCurves");

            _chart = new XYChart.Chart();
            _chart.Tag = chartCtrl;
            _chart.Text = Text;
            _chart.Name = Text;
            List<Control> controls = new List<Control>();
            controls.Add(_chart);
            base.RegisterControls(controls);
        }

        public override void AppendControl(Control control)
        {            
            _chart = (XYChart.Chart) control;
            _chart.Reset();
            
            ControlData ctrlData = (ControlData)_chart.Tag;

            bool found = false;
            foreach (string ft in ctrlData.PropertyFilter)
            {
                if (ft == "ReferenceCurves")
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                ctrlData.PropertyFilter.Add("ReferenceCurves");

            base.AppendControl(control);
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
