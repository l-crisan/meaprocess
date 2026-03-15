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
    internal class GaugePS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public GaugePS()
        {
            base.Type = "PS_OUTPUT";
            base.Text = StringResource.Gauge;
            base.Group = StringResource.Display;
            base.Symbol = Mp.Scheme.Win.Images.GaugeImage;
            base.Icon = Mp.Scheme.Win.Images.GaugeIcon;
            base.SubType = "Mp.Runtime.Sdk.GaugePS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Gauge.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.GaugePsDescription;
            }
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            Mp.Gauge.GaugeCtrl gauge = new Mp.Gauge.GaugeCtrl();
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Center");
            ctrlData.PropertyFilter.Add("Value");
            ctrlData.PropertyFilter.Add("CaptionDefinition");
            ctrlData.PropertyFilter.Add("BaseArcColor");
            ctrlData.PropertyFilter.Add("BaseArcRadius");
            ctrlData.PropertyFilter.Add("BaseArcStart");
            ctrlData.PropertyFilter.Add("BaseArcSweep");
            ctrlData.PropertyFilter.Add("BaseArcWidth");
            ctrlData.PropertyFilter.Add("ScaleLinesInterColor");
            ctrlData.PropertyFilter.Add("ScaleLinesInterInnerRadius");
            ctrlData.PropertyFilter.Add("ScaleLinesInterOuterRadius");
            ctrlData.PropertyFilter.Add("ScaleLinesInterWidth");
            ctrlData.PropertyFilter.Add("ScaleLinesMajorColor");
            ctrlData.PropertyFilter.Add("ScaleLinesMajorInnerRadius");
            ctrlData.PropertyFilter.Add("ScaleLinesMajorOuterRadius");
            ctrlData.PropertyFilter.Add("ScaleLinesMajorWidth");
            ctrlData.PropertyFilter.Add("ScaleLinesMinorColor");
            ctrlData.PropertyFilter.Add("ScaleLinesMinorInnerRadius");
            ctrlData.PropertyFilter.Add("ScaleLinesMinorNumOf");
            ctrlData.PropertyFilter.Add("ScaleLinesMinorOuterRadius");
            ctrlData.PropertyFilter.Add("ScaleLinesMinorWidth");
            ctrlData.PropertyFilter.Add("ScaleNumbersColor");
            ctrlData.PropertyFilter.Add("ScaleNumbersFormat");
            ctrlData.PropertyFilter.Add("ScaleNumbersRadius");
            ctrlData.PropertyFilter.Add("ScaleNumbersRotation");
            ctrlData.PropertyFilter.Add("ScaleNumbersStartScaleLine");
            ctrlData.PropertyFilter.Add("ScaleNumbersStepScaleLines");
            ctrlData.PropertyFilter.Add("NeedleColor1");
            ctrlData.PropertyFilter.Add("NeedleColor2");
            ctrlData.PropertyFilter.Add("NeedleRadius");
            ctrlData.PropertyFilter.Add("NeedleType");
            ctrlData.PropertyFilter.Add("NeedleWidth");
            ctrlData.PropertyFilter.Add("RangeDefinition");
            ctrlData.PropertyFilter.Add("MaxValue");
            ctrlData.PropertyFilter.Add("MinValue");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("Font");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("ScaleLinesMajorStepValue");            
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Dock");

            gauge.Tag = ctrlData;

            gauge.MinValue = (float)XmlHelper.GetParamDouble(xmlSignal, "physMin");
            gauge.MaxValue = (float)XmlHelper.GetParamDouble(xmlSignal, "physMax");

            gauge.Name = this.Text;
            float step = (float)(gauge.MaxValue - gauge.MinValue) / 10;
            gauge.ScaleLinesMajorStepValue = step;
            gauge.CaptionDefinition[0].Text = XmlHelper.GetParam(xmlSignal, "name");
            gauge.CaptionDefinition[0].Position = new System.Drawing.PointF(gauge.Width/2 - 10, gauge.Height/2 + 10);

            gauge.CaptionDefinition[1].Text = XmlHelper.GetParam(xmlSignal, "unit");
            gauge.CaptionDefinition[1].Position = new System.Drawing.PointF(gauge.Width / 2 - 10, gauge.Height / 2 + 20);
            gauge.RangeDefinition[0].StartValue = gauge.MinValue;
            gauge.RangeDefinition[0].EndValue = gauge.MaxValue;

            gauge.RangeDefinition[1].StartValue = gauge.MaxValue* 0.7f;
            gauge.RangeDefinition[1].EndValue = gauge.MaxValue;
            gauge.Center = new Point(gauge.Width / 2, gauge.Height / 2);

            return gauge;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            Mp.Gauge.GaugeCtrl gauge = (Mp.Gauge.GaugeCtrl)control;

            gauge.CaptionDefinition[0].Text = XmlHelper.GetParam(xmlSignal, "name");
            gauge.CaptionDefinition[1].Text = XmlHelper.GetParam(xmlSignal, "unit");
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
