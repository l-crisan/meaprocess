using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using Mp.NumericView;
using Mp.Scheme.Sdk;
using Mp.Components;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    internal class DigitalMeterPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public DigitalMeterPS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = "Digital Meter 2";
            base.Group = StringResource.Display;
            base.Symbol = Mp.Scheme.Win.Images.NumericView;
            base.Icon = Mp.Scheme.Win.Images.NumericViewIcon;
            base.SubType = "Mp.Runtime.Sdk.DigitalMeterPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.NumericView.dll");
        }


        public override string Description
        {
            get
            {
                return StringResource.DigitalMeter2PsDescription;
            }
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            DigitalMeter nc = new DigitalMeter();
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Font");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("ForeColor");
            ctrlData.PropertyFilter.Add("Precision");
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Dock");
            nc.Tag = ctrlData;
            nc.Name = this.Text;
            nc.Label = XmlHelper.GetParam(xmlSignal, "name");
            nc.Unit= XmlHelper.GetParam(xmlSignal, "unit");

            return nc;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            DigitalMeter nc = (DigitalMeter)control;
            nc.Label = XmlHelper.GetParam(xmlSignal, "name");
            nc.Unit = XmlHelper.GetParam(xmlSignal, "unit");
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_INPUT", true, false);
            AddPort(port);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
        }
    }
}
