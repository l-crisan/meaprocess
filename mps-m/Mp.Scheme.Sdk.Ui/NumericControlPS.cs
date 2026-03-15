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
    public class NumericControlPS : SignalBasedControlPS
    {
        private Assembly _myControlsAssembly;

        public NumericControlPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.DigitalMeter1;
            base.Group = StringResource.Display;
            base.Symbol = Images.NumericView;
            base.Icon = Images.NumericIcon;
            base.SubType = "Mp.Runtime.Sdk.NumericControlPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Digital.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.DigitalMeter1;
            base.Group = StringResource.Display;
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
            DigitalMeter1 nc = new DigitalMeter1();            
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
            ctrlData.PropertyFilter.Add("Limits");
            nc.Tag = ctrlData;            
            nc.LabelText = XmlHelper.GetParam(xmlSignal, "name");
            nc.UnitText = XmlHelper.GetParam(xmlSignal, "unit");
            nc.Name = this.Text;
            return nc;
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 670);
        }

        protected override void OnUpdateControl(Control control, XmlElement xmlSignal)
        {
            DigitalMeter1 nc = (DigitalMeter1)control;
            nc.LabelText = XmlHelper.GetParam(xmlSignal, "name");
            nc.UnitText = XmlHelper.GetParam(xmlSignal, "unit");
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
