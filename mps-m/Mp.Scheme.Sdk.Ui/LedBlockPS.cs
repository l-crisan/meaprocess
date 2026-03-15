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
    public class LedBlockPS : VisualBlockPS
    {
        private Assembly _myControlsAssembly;

        public LedBlockPS()
        {
            Type = "Mp.PS.SysOut";
            Text = "LED";
            Group = StringResource.DisplayBlock;
            Symbol = Images.Led;
            Icon = Images.LedIcon;
            SubType = "Mp.Runtime.Sdk.LedBlockPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);
            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Digital.dll");
        }

        public override void OnLoadResources()
        {
            Text = "LED"; ;
            Group = StringResource.DisplayBlock;
        }


        public override string Description
        {
            get
            {
                return StringResource.LEDPsDescription;
            }
        }


        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }


        protected override  void OnInitControl()
        {
            this.BlockView = new LedBlock();
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
            ctrlData.PropertyFilter.Add("ColumnCount");

            this.BlockView.Tag = ctrlData;
            List<Control> controls = new List<Control>();
            controls.Add(this.BlockView);

            RegisterControls(controls);
        }


        public override void OnHelpRequested()
        {
            //Document.ShowHelp(this.Site, 1380);
        }


        protected override Control OnCreateControl()
        {
            return new LabelLed();
        }

    }
}

