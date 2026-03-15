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
    public class LedPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public LedPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = "LED";
            base.Group = StringResource.Display;
            base.Symbol = Images.Led;
            base.Icon = Images.LedIcon;
            base.SubType = "Mp.Runtime.Sdk.LedPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Digital.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = "LED";
            base.Group = StringResource.Display;
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
            Mp.Visual.Digital.LabelLed led = new Mp.Visual.Digital.LabelLed();
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
            Mp.Visual.Digital.LabelLed led = (Mp.Visual.Digital.LabelLed)control;
            led.LabelText = XmlHelper.GetParam(xmlSignal, "name");
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
            Document.ShowHelp(this.Site, 740);
        }
    }
}
