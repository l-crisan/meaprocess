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
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Mp.Visual.Analog;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class GaugeBlockPS : VisualBlockPS
    {
        private Assembly _myControlsAssembly;

        public GaugeBlockPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.Gauge;
            base.Group = StringResource.DisplayBlock;
            base.Symbol = Images.GaugeImage;
            base.Icon = Images.GaugeIcon;
            base.SubType = "Mp.Runtime.Sdk.GaugeBlockPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Analog.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.Gauge;
            base.Group = StringResource.DisplayBlock;
        }

        protected override Control OnCreateControl()
        {
            return new Gauge();
        }

        public override string Description
        {
            get
            {
                return StringResource.GaugePsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        protected override void OnInitControl()
        {
            base.BlockView = new GaugeBlock();
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Center");
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
            ctrlData.PropertyFilter.Add("ColumnCount");

            base.BlockView.Tag = ctrlData;

            List<Control> controls = new List<Control>();
            controls.Add(base.BlockView);

            base.RegisterControls(controls);
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1380);
        }     
    }
}

