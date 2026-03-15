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
    class LedPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public LedPS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = "LED";
            base.Group = StringResource.Display;
            base.Symbol = Mp.Scheme.Win.Images.Led;
            base.Icon = Mp.Scheme.Win.Images.LedIcon;
            base.SubType = "Mp.Runtime.Sdk.LedPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Led.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.LEDPsDescription;
            }
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            Mp.Led.LabelLed led = new Mp.Led.LabelLed();
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("ColorOff");
            ctrlData.PropertyFilter.Add("ColorOn");
            ctrlData.PropertyFilter.Add("On");
            ctrlData.PropertyFilter.Add("ShowLabel");
            ctrlData.PropertyFilter.Add("Dock");
            led.ColorOff = Color.YellowGreen;
            led.ColorOn = Color.YellowGreen;
            led.On = false;
            led.Tag = ctrlData;
            led.Name = this.Text;
            return led;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            Mp.Led.LabelLed led = (Mp.Led.LabelLed)control;
            led.LabelText = XmlHelper.GetParam(xmlSignal, "name");
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
