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
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Mp.Visual.Analog;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class BarCtrlPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public BarCtrlPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.BarView;
            base.Group = StringResource.Display;
            base.Symbol = Images.BarViewImg;
            base.Icon = Images.BarIcon;
            base.SubType = "Mp.Runtime.Sdk.BarCtrlPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Digital.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.BarView;
            base.Group = StringResource.Display;
        }

        public override string Description
        {
            get
            {
                return StringResource.BarViewPsDescription;
            }
        }

        public override void OnDefaultInit()
        {            

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1380);
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            Bar bar = new Bar();
            ControlData ctrlData = new ControlData();

            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("AxisDivision");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("ForeColor");
            ctrlData.PropertyFilter.Add("Dock");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("AxisPrecision");
            ctrlData.PropertyFilter.Add("Orientation");
            ctrlData.PropertyFilter.Add("BarColor");
            ctrlData.PropertyFilter.Add("Minimum");
            ctrlData.PropertyFilter.Add("Maximum");
            bar.Tag = ctrlData;
            UpdateBarCtrl(xmlSignal, bar);
            return bar;
        }

        private static void UpdateBarCtrl(XmlElement xmlSignal, Bar bar)
        {
            string sigName = XmlHelper.GetParam(xmlSignal, "name");
            string unit = XmlHelper.GetParam(xmlSignal, "unit");
            
            if (unit != null && unit != "")
                sigName += (" (" + unit + ")");

            bar.SigName = sigName;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            Bar bar = (Bar)control;
            UpdateBarCtrl(xmlSignal, bar);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
        }
    }
}

