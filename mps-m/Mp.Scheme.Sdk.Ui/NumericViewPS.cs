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
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using System.IO;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class NumericViewPS : VisualPS
    {
        private Assembly _myControlsAssembly;

        public NumericViewPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.TableView;
            base.Group = StringResource.DisplayBlock;
            base.Symbol = Images.TableViewImage;
            base.Icon = Images.TableIcon;
            base.SubType = "Mp.Runtime.Sdk.NumericViewPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Digital.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.TableView;
            base.Group = StringResource.DisplayBlock;
        }

        public override string Description
        {
            get
            {
                return StringResource.TableViewPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            CreateControl();

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

        }

        private void CreateControl()
        {
            //Control
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Text");
            ctrlData.PropertyFilter.Add("SignalWidth");
            ctrlData.PropertyFilter.Add("ValueWidth");
            ctrlData.PropertyFilter.Add("UnitWidth");
            ctrlData.PropertyFilter.Add("Precision");
            ctrlData.PropertyFilter.Add("HideSignalName");
            ctrlData.PropertyFilter.Add("HideSignalUnit");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Dock");


            Mp.Visual.Digital.TableView numericViewCtrl = new Mp.Visual.Digital.TableView();
            numericViewCtrl.Tag = ctrlData;
            numericViewCtrl.Text = this.Text;
            numericViewCtrl.Name = this.Text;
            
            RegisterControl(numericViewCtrl);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            Port dataInPort = InputPorts[0];

            if (base.Controls.Count == 0)
            {
                CreateControl();
                valInfoList.Add(new ValidationInfo("A new Table View was created, because a new version is available.", ValidationInfo.InfoType.Warning));
            }
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            base.OnProperties(sender, e);
            Mp.Visual.Digital.TableView nc = (Mp.Visual.Digital.TableView)Controls[0];
            nc.Text = this.Text;
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 660);
        }
    }
}
