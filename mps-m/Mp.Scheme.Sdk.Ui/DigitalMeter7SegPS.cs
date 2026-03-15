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
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Mp.Visual.Digital;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class DigitalMeter7SegPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public DigitalMeter7SegPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.DigitalMeter3;
            base.Group = StringResource.Display;
            base.Symbol = Images.NumericView;
            base.Icon = Images.NumericIcon;
            base.SubType = "Mp.Runtime.Sdk.DigitalMeter7SegPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Digital.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.DigitalMeter3;
            base.Group = StringResource.Display;
        }

        public override string Description
        {
            get
            {
                return StringResource.DigitalMeter7PsDescription;
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 690);
        }

        protected override Control OnCreateNewControl(XmlElement xmlSignal)
        {
            DigitalMeter3 nc = new DigitalMeter3();
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Font");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("ForeColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("ColorBackground");
            ctrlData.PropertyFilter.Add("ColorLight");
            ctrlData.PropertyFilter.Add("ColorDark");
            ctrlData.PropertyFilter.Add("Elements");
            ctrlData.PropertyFilter.Add("Precision");
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Dock");
            ctrlData.PropertyFilter.Add("ElementWidth");
            nc.Tag = ctrlData;
            nc.Name = this.Text;
            return nc;
        }

        public override void AppendControl(Control ctrl)
        {            
            ControlData ctrlData = (ControlData)ctrl.Tag;

            bool setProp = false;

            foreach (string prop in ctrlData.PropertyFilter)
            {
                if (prop == "ElementWidth")
                {
                    setProp = true;
                    break;
                }
            }

            if (!setProp)
                ctrlData.PropertyFilter.Add("ElementWidth");

            base.AppendControl(ctrl);
        }


        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            DigitalMeter3 nc = (DigitalMeter3)control;
            string unit = XmlHelper.GetParam(xmlSignal, "unit");

            if( unit != "")
                nc.Label = XmlHelper.GetParam(xmlSignal, "name") + " (" + unit + ")";
            else
                nc.Label = XmlHelper.GetParam(xmlSignal, "name");
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
    }
}
