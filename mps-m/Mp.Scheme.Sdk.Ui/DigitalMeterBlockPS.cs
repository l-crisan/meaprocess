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
using Mp.Visual.Digital;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class DigitalMeterBlockPS : VisualBlockPS
    {
        private Assembly _myControlsAssembly;

        public DigitalMeterBlockPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.DigitalMeter2;
            base.Group = StringResource.DisplayBlock;
            base.Symbol = Images.NumericView;
            base.Icon = Images.NumericIcon;
            base.SubType = "Mp.Runtime.Sdk.DigitalMeterBlockPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Digital.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.DigitalMeter2;
            base.Group = StringResource.DisplayBlock;
        }

     

        public override string Description
        {
            get
            {
                return StringResource.DigitalMeter2PsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        protected override Control OnCreateControl()
        {
            return   new DigitalMeter2();
        }

        protected override void OnInitControl()
        {
            base.BlockView = new DigitalMeter2Block();
            
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter.Add("Font");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("ForeColor");
            ctrlData.PropertyFilter.Add("Precision");
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Dock");
            ctrlData.PropertyFilter.Add("Limits");
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

