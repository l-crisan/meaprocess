using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;

using Mp.Gauge;
using Mp.Scheme.Sdk;
using Mp.Components;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    class TachometerPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public TachometerPS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = "Tachometer";
            base.Group = StringResource.Display;
            base.Symbol = Mp.Scheme.Win.Images.Tachometer;
            base.Icon = Mp.Scheme.Win.Images.TachometerIcon;
            base.SubType = "Mp.Runtime.Sdk.TachometerPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Gauge.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.TachometerPsDescription;
            }
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            Mp.Gauge.AquaGauge gauge = new Mp.Gauge.AquaGauge();
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Dock");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("MinValue");
            ctrlData.PropertyFilter.Add("MaxValue");
            ctrlData.PropertyFilter.Add("Value");
            ctrlData.PropertyFilter.Add("DialColor");
            ctrlData.PropertyFilter.Add("NoOfDivisions");
            ctrlData.PropertyFilter.Add("NoOfSubDivisions");
            

            gauge.Tag = ctrlData;
            gauge.MaxValue = (float)XmlHelper.GetParamDouble(xmlSignal, "physMax");
            gauge.MinValue = (float)XmlHelper.GetParamDouble(xmlSignal, "physMin");
            gauge.Name = this.Text;
            return gauge;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            Mp.Gauge.AquaGauge gauge = (Mp.Gauge.AquaGauge)control;
            string unit = XmlHelper.GetParam(xmlSignal, "unit");

            if( unit != "")
                gauge.DialText = XmlHelper.GetParam(xmlSignal, "name") + " (" + unit + ")";
            else
                gauge.DialText = XmlHelper.GetParam(xmlSignal, "name");
        }


        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_INPUT", true, false);
            AddPort(port);
        }
    }
}
