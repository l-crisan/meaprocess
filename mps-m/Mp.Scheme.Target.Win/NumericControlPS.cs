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
    internal class NumericControlPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public NumericControlPS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = "Digital Meter 1";
            base.Group = StringResource.Display;
            base.Symbol = Mp.Scheme.Win.Images.NumericView;
            base.Icon = Mp.Scheme.Win.Images.NumericViewIcon;
            base.SubType = "Mp.Runtime.Sdk.NumericControlPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.NumericView.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.DigitalMeter1PsDescription;
            }
        }


        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            NumericControl nc = new NumericControl();            
            ControlData ctrlData = new ControlData();

            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Precision");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("ForeColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("ValueBoxForeColor");
            ctrlData.PropertyFilter.Add("ValueBoxBackColor");
            ctrlData.PropertyFilter.Add("Font");
            ctrlData.PropertyFilter.Add("LabelWidth");            
            ctrlData.PropertyFilter.Add("ValueWidth");
            nc.Tag = ctrlData;            
            nc.LabelText = XmlHelper.GetParam(xmlSignal, "name");
            nc.UnitText = XmlHelper.GetParam(xmlSignal, "unit");
            nc.Name = this.Text;
            return nc;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            NumericControl nc = (NumericControl)control;
            nc.LabelText = XmlHelper.GetParam(xmlSignal, "name");
            nc.UnitText = XmlHelper.GetParam(xmlSignal, "unit");
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
