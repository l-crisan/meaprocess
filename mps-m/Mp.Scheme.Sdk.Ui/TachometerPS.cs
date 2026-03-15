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
    public class TachometerPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public TachometerPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = "Tachometer";
            base.Group = StringResource.Display;
            base.Symbol = Images.Tachometer;
            base.Icon = Images.TachometerIcon;
            base.SubType = "Mp.Runtime.Sdk.TachometerPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Analog.dll");
        }


        public override void OnLoadResources()
        {
            base.Text = "Tachometer";
            base.Group = StringResource.Display;
        }

        public override string Description
        {
            get
            {
                return StringResource.TachometerPsDescription;
            }
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            Mp.Visual.Analog.Tachometer gauge = new Mp.Visual.Analog.Tachometer();
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Dock");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("MinValue");
            ctrlData.PropertyFilter.Add("MaxValue");
            ctrlData.PropertyFilter.Add("Value");
            ctrlData.PropertyFilter.Add("DialColor");
            ctrlData.PropertyFilter.Add("NoOfDivisions");
            ctrlData.PropertyFilter.Add("NoOfSubDivisions");
            

            gauge.Tag = ctrlData;
            gauge.MaxValue = (float)XmlHelper.GetParamDouble(xmlSignal, "physMax");
            gauge.MinValue = (float)XmlHelper.GetParamDouble(xmlSignal, "physMin");
            gauge.Name = this.Text;
            return gauge;
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            Mp.Visual.Analog.Tachometer gauge = (Mp.Visual.Analog.Tachometer)control;
            string unit = XmlHelper.GetParam(xmlSignal, "unit");

            if( unit != "")
                gauge.DialText = XmlHelper.GetParam(xmlSignal, "name") + " (" + unit + ")";
            else
                gauge.DialText = XmlHelper.GetParam(xmlSignal, "name");
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
            Document.ShowHelp(this.Site, 720);
        }
    }
}
