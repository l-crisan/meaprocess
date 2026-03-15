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
using Mp.Scheme.Sdk;
using Mp.Scheme.Sdk.Ui;
using Mp.Utils;
using Mp.Visual.XYChart;

namespace Mp.Mod.FFT.View
{
    internal class FFTViewPS : VisualPS
    {
        private Chart _view;
        Assembly _asm;

        public FFTViewPS()
        {
            base.Type = "Mp.PS.SysOut";
            this.Text = StringResource.FFTViewPS;
            base.Group = "GPS";
            base.Symbol = Resource.FFT;
            base.Icon = Resource.FFTIcon;
            base.SubType = "Mp.Runtime.Sdk.FFTViewPS";
            _asm = Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Mp.Visual.XYChart.dll"));

        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.FFTViewPS;
            base.Group = StringResource.Analysis;
        }

        public override string Description
        {
            get
            {
                return StringResource.FFTViewPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            InitControl();

            base.OnDefaultInit();

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
            chartCtrl.PropertyFilter.Add("NoOfPoints");


            _view = new Chart();

            _view.Tag = chartCtrl;

            _view.NoOfPoints = 5000;
            _view.DrawPoint = false;

            //X-Axis
            _view.XAxisDivision = 10;
            _view.XMinimum = 0;
            _view.XMaximum = 5000;
            _view.XAxisPrecision = 0;

            //Y-Axis
            _view.XText = "Frequency (Hz)";
            _view.YMinimum = 0;
            _view.YMaximum = 2;
            _view.YText = "Voltage (V)";

            List<Control> controls = new List<Control>();
            controls.Add(_view);
            base.RegisterControls(controls);
        }

        public override void AppendControl(Control control)
        {
            base.AppendControl(control);
            _view = (Chart)control;
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            Port dataInPort = InputPorts[0];

            if (dataInPort.SignalList == null)
            {
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.TwoSigErr, this.Text), ValidationInfo.InfoType.Error));
                return;
            }

            if (dataInPort.SignalList.ChildNodes.Count < 2)
            {
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.TwoSigErr, this.Text), ValidationInfo.InfoType.Error));
                return;
            }
        }
    }
}
