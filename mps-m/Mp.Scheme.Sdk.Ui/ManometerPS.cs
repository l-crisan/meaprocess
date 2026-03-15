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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Mp.Visual.Analog;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class ManometerPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public ManometerPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = "Manometer";
            base.Group = StringResource.Display;
            base.Symbol = Images.Manometer;
            base.Icon = Images.ManometerIcon;
            base.SubType = "Mp.Runtime.Sdk.ManometerPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Analog.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = "Manometer";
            base.Group = StringResource.Display;
        }

        public override string Description
        {
            get
            {
                return StringResource.ManometerPsDescription;
            }
        }

        private void InitManometer(Manometer manometer)
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
            Manometer manometer = new Manometer();
            InitManometer(manometer);
            return manometer;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            Manometer manometer = (Manometer)control;

            manometer.TextUnit = XmlHelper.GetParam(xmlSignal, "unit");
            manometer.TextDescription = XmlHelper.GetParam(xmlSignal, "name");
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, true);
            AddPort(port);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 700);
        }
    }
}
