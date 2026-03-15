using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Windows.Forms;

using Mp.Components;
using Mp.Scheme.Sdk;
using Manometers;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    internal class ManometerPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public ManometerPS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = "Manometer";
            base.Group = StringResource.Display;
            base.Symbol = Mp.Scheme.Win.Images.Manometer;
            base.Icon = Mp.Scheme.Win.Images.ManometerIcon;
            base.SubType = "Mp.Runtime.Sdk.ManometerPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Manometers.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.ManometerPsDescription;
            }
        }

        private void InitManometer(Manometers.Thermometer manometer)
        {
            manometer.Width = 200;
            manometer.Height = 200;
            manometer.Min = -10;
            manometer.Max = 10;
            manometer.Interval = 2;
            manometer.StartAngle = 240;

            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Decimals");
            ctrlData.PropertyFilter.Add("NumberSpacing");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("BorderWidth");
            ctrlData.PropertyFilter.Add("ArrowColor");
            ctrlData.PropertyFilter.Add("ClockWise");
            ctrlData.PropertyFilter.Add("BarsBetweenNumbers");
            ctrlData.PropertyFilter.Add("Max");
            ctrlData.PropertyFilter.Add("Min");
            ctrlData.PropertyFilter.Add("Interval");
            ctrlData.PropertyFilter.Add("StoreMax");
            ctrlData.PropertyFilter.Add("StartAngle");
            ctrlData.PropertyFilter.Add("Value");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Dock");
            manometer.Tag = ctrlData;
            manometer.Name = this.Text;
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            Manometers.Thermometer manometer = new Manometers.Thermometer();
            InitManometer(manometer);
            return manometer;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            Manometers.Thermometer manometer = (Manometers.Thermometer)control;

            manometer.TextUnit = XmlHelper.GetParam(xmlSignal, "unit");
            manometer.TextDescription = XmlHelper.GetParam(xmlSignal, "name");
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_INPUT", true, true);
            AddPort(port);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
        }        
    }
}
