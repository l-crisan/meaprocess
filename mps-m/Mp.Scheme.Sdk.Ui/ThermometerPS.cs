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
using System.IO;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using Mp.Visual.Analog;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class ThermometerPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public ThermometerPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = "Thermometer";
            base.Group = StringResource.Display;
            base.Symbol = Images.ThermometerImage;
            base.Icon = Images.ThermometerIcon;
            base.SubType = "Mp.Runtime.Sdk.ThermometerPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Analog.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = "Thermometer";
            base.Group = StringResource.Display;
        }

        public override string Description
        {
            get
            {
                return StringResource.ThermometerPsDescription;
            }
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            Thermometer therm = new Thermometer();
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");            
            ctrlData.PropertyFilter.Add("SmallTicFreq");
            ctrlData.PropertyFilter.Add("LargeTicFreq");
            ctrlData.PropertyFilter.Add("RangeMin");
            ctrlData.PropertyFilter.Add("RangeMax");
            ctrlData.PropertyFilter.Add("Value");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("ForeColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Display");
            ctrlData.PropertyFilter.Add("Dock");

            therm.Tag = ctrlData;
            therm.Text = XmlHelper.GetParam(xmlSignal, "name");
            therm.RangeMin = XmlHelper.GetParamDouble(xmlSignal, "physMin");
            therm.RangeMax = XmlHelper.GetParamDouble(xmlSignal, "physMax");
            therm.Name = this.Text;
            return therm;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            Thermometer therm = (Thermometer)control;
            therm.Text = XmlHelper.GetParam(xmlSignal, "name");
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 730);
        }
    }
}
