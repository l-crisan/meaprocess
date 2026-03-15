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
    public class MamometerBlockPS : VisualBlockPS
    {
        private Assembly _myControlsAssembly;
        

        public MamometerBlockPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = "Manometer";
            base.Group = StringResource.DisplayBlock;
            base.Symbol = Images.Manometer;
            base.Icon = Images.ManometerIcon;
            base.SubType = "Mp.Runtime.Sdk.ManometerBlockPS";
            
            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Analog.dll");
            
        }

        public override void OnLoadResources()
        {
            base.Text = "Manometer";
            base.Group = StringResource.DisplayBlock;
        }


        public override string Description
        {
            get
            {
                return StringResource.ManometerPsDescription;
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
            base.BlockView = new Mp.Visual.Analog.ManometerBlock();

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
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Dock");

            base.BlockView.Tag = ctrlData;
            List<Control> controls = new List<Control>();
            controls.Add(base.BlockView);

            base.RegisterControls(controls);
        }

        protected override Control OnCreateControl()
        {
            return new Manometer();
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1380);
        }

    }
}

