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
using System.Reflection;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class GaugePS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public GaugePS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.Gauge;
            base.Group = StringResource.Display;
            base.Symbol = Images.GaugeImage;
            base.Icon = Images.GaugeIcon;
            base.SubType = "Mp.Runtime.Sdk.GaugePS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Analog.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.Gauge;
            base.Group = StringResource.Display;
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
            Mp.Visual.Analog.Gauge gauge = new Mp.Visual.Analog.Gauge();
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
            Mp.Visual.Analog.Gauge gauge = (Mp.Visual.Analog.Gauge)control;

            gauge.CaptionDefinition[0].Text = XmlHelper.GetParam(xmlSignal, "name");
            gauge.CaptionDefinition[1].Text = XmlHelper.GetParam(xmlSignal, "unit");
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 710); 
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }
    }
}
