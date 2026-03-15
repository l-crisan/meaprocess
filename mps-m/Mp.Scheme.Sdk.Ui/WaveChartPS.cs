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
using System.Reflection;
using System.IO;
using System.Windows.Forms;
using Mp.Utils;
using Mp.Visual.WaveChart;

namespace Mp.Scheme.Sdk.Ui
{
    public class WaveChartPS : VisualPS
    {
        public WaveChartPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.WaveChart;
            base.Group = StringResource.DisplayBlock;
            base.Symbol = Images.WaveChart;
            base.Icon = Images.YTChartIcon;
            base.SubType = "Mp.Runtime.Sdk.WaveChartPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(Path.Combine(currentAssemblyPath , "Mp.Visual.WaveChart.dll"));
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.WaveChart;
            base.Group = StringResource.DisplayBlock;
        }

        public override string Description
        {
            get
            {
                return StringResource.WaveChartPsDescription;
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

        protected override void OnProperties(object sender, EventArgs e)
        {
            base.OnProperties(sender, e);

            foreach (Control control in Controls)
                control.Text = Text;
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
            chartCtrl.PropertyFilter.Add("Margin");
            chartCtrl.PropertyFilter.Add("ShowGrid");
            chartCtrl.PropertyFilter.Add("GridLineColor");
            chartCtrl.PropertyFilter.Add("GridLineWidth");
            chartCtrl.PropertyFilter.Add("GridLineStyle");
            chartCtrl.PropertyFilter.Add("GridXDivision");
            chartCtrl.PropertyFilter.Add("GridYDivision");
            chartCtrl.PropertyFilter.Add("Text");
            chartCtrl.PropertyFilter.Add("BorderStyle");
            chartCtrl.PropertyFilter.Add("Dock");

            _waveChart = new WaveChartCtrl();
            _waveChart.Tag = chartCtrl;
            _waveChart.Text = Text;
            _waveChart.Name = this.Text;

            ControlData xAxisProperties = new ControlData();
            xAxisProperties.PropertyFilter = new List<string>();
            xAxisProperties.PropertyFilter.Add("Left");
            xAxisProperties.PropertyFilter.Add("Top");
            xAxisProperties.PropertyFilter.Add("Width");
            xAxisProperties.PropertyFilter.Add("Height");
            xAxisProperties.PropertyFilter.Add("BackColor");
            xAxisProperties.PropertyFilter.Add("TimeSlot");
            xAxisProperties.PropertyFilter.Add("AxisDivision");
            xAxisProperties.PropertyFilter.Add("TextColor");
            xAxisProperties.PropertyFilter.Add("LineColor");
            xAxisProperties.PropertyFilter.Add("DegreeTextColor");
            xAxisProperties.PropertyFilter.Add("Precision");
            xAxisProperties.PropertyFilter.Add("Representation");
            xAxisProperties.PropertyFilter.Add("AxisText");
            xAxisProperties.PropertyFilter.Add("Visible");
            xAxisProperties.PropertyFilter.Add("Text");
            xAxisProperties.PropertyFilter.Add("BorderStyle");
            xAxisProperties.PropertyFilter.Add("Dock");

            _waveChart.TimeAxis = new TimeAxis();
            _waveChart.TimeAxis.Tag = xAxisProperties;
            _waveChart.TimeAxis.Text = Text;
            _waveChart.TimeAxis.Name = this.Text;
            ControlData yAxisProperties = new ControlData();
            yAxisProperties.PropertyFilter = new List<string>();
            yAxisProperties.PropertyFilter.Add("Left");
            yAxisProperties.PropertyFilter.Add("Top");
            yAxisProperties.PropertyFilter.Add("Width");
            yAxisProperties.PropertyFilter.Add("Height");
            yAxisProperties.PropertyFilter.Add("BackColor");
            yAxisProperties.PropertyFilter.Add("Visible");
            yAxisProperties.PropertyFilter.Add("Text");
            yAxisProperties.PropertyFilter.Add("BorderStyle");
            yAxisProperties.PropertyFilter.Add("Dock");

            _waveChart.YAxis = new YAxis();
            _waveChart.YAxis.Text = Text;
            _waveChart.YAxis.Tag = yAxisProperties;
            _waveChart.YAxis.Name = this.Text;

            ControlData legendProperties = new ControlData();
            legendProperties.PropertyFilter = new List<string>();
            legendProperties.PropertyFilter.Add("Left");
            legendProperties.PropertyFilter.Add("Top");
            legendProperties.PropertyFilter.Add("Width");
            legendProperties.PropertyFilter.Add("Height");
            legendProperties.PropertyFilter.Add("Visible");
            legendProperties.PropertyFilter.Add("Text");
            legendProperties.PropertyFilter.Add("BorderStyle");
            legendProperties.PropertyFilter.Add("Dock");

            _waveChart.Legend = new SigLegend();
            _waveChart.Legend.Text = Text;
            _waveChart.Legend.Tag = legendProperties;
            _waveChart.Legend.Name = this.Text;

            List<Control> controls = new List<Control>();
            controls.Add(_waveChart);
            controls.Add(_waveChart.TimeAxis);
            controls.Add(_waveChart.YAxis);
            controls.Add(_waveChart.Legend);

            base.RegisterControls(controls);
            _waveChart.InitDone();
        }

        public override void AppendControl(Control control)
        {
            base.AppendControl(control);

            if (control is WaveChartCtrl)
            {
                _waveChart = (WaveChartCtrl) control;
            }

            if (control is TimeAxis)
            {
                _timeAxis = (TimeAxis) control;
            }

            if (control is YAxis)
            {
                _yAxis = (YAxis) control;
            }

            if (control is SigLegend)
            {
                _legend = (SigLegend) control;
            }

            AssemblyControls();
        }

        private void AssemblyControls()
        {
            if (_legend != null && _waveChart != null && _timeAxis != null && _legend != null && _yAxis != null)
            {
                _waveChart.Legend = _legend;
                _waveChart.YAxis = _yAxis;
                _waveChart.TimeAxis = _timeAxis;
            }
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            Port dataInPort = InputPorts[0];

            if (base.Controls.Count == 0)
            {
                InitControl();
                valInfoList.Add( new ValidationInfo("A new Wave Chart control was created. The old coudn't be loaded because a new version of this control is available.",ValidationInfo.InfoType.Warning)); 
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 630);
        }

        private Mp.Visual.WaveChart.WaveChartCtrl _waveChart;
        private Mp.Visual.WaveChart.YAxis _yAxis;
        private Mp.Visual.WaveChart.TimeAxis _timeAxis;
        private Mp.Visual.WaveChart.SigLegend _legend;


        private Assembly _myControlsAssembly;
    }
}
