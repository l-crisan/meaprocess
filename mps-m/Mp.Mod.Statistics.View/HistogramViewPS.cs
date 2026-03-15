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

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Mp.Scheme.Sdk.Ui;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Statistics.View
{
    public class HistogramViewPS : VisualPS
    {
        private Visual.XYChart.Chart _chart;

        public HistogramViewPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.HistogramView;
            base.Group = StringResource.Statistics;
            base.Symbol = Resource.ClassingImg;
            base.Icon = Resource.Classing; 
            base.SubType = "Mp.Runtime.Sdk.HistogramViewPS";
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.HistogramView;
            base.Group = StringResource.Statistics;
        }


        public override string Description
        {
            get
            {
                return StringResource.HistogramPsDescription;
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
            chartCtrl.PropertyFilter.Add("GridXDivision");
            chartCtrl.PropertyFilter.Add("GridYDivision");
            chartCtrl.PropertyFilter.Add("GridColor");
            chartCtrl.PropertyFilter.Add("DrawPoint");
            chartCtrl.PropertyFilter.Add("DrawLine");
            chartCtrl.PropertyFilter.Add("Dock");
            

            _chart = new Visual.XYChart.Chart();
            _chart.Tag = chartCtrl;
            _chart.Text = Text;
            _chart.XText = "Class Limits";
            _chart.YText = "%";

            _chart.Title = Text;

            _chart.ShowControlPanel = false;
            List<Control> controls = new List<Control>();
            controls.Add(_chart);
            base.RegisterControls(controls);
        }

        public override void AppendControl(Control control)
        {
            base.AppendControl(control);
            _chart = (Visual.XYChart.Chart)control;
            _chart.ShowControlPanel = false;
            _chart.Reset();
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1180);    
        }
    }
}
