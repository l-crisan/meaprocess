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
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using Mp.Visual.Oscilloscope;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class OscilloscopePS  : VisualPS
    {

        private Assembly _myControlsAssembly;
        private OscilloscopeView _chartCtrl;

        public OscilloscopePS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.Oscilloscope;
            base.Group = StringResource.DisplayBlock;
            base.Symbol = Images.Oscilloscope;
            base.Icon = Images.OscilloscopeIcon;
            base.SubType = "Mp.Runtime.Sdk.OscilloscopePS";
            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Oscilloscope.dll");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.Oscilloscope;
            base.Group = StringResource.DisplayBlock;
        }

        public override string Description
        {
            get
            {
                return StringResource.OscilloscopePsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            InitControl();

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

            //Reset port.
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset + DistanceBetweenPort)), "Mp.Port.In", true, false);
            port.ConnectorBrush = new SolidBrush(Color.DarkGreen);
            AddPort(port);
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
        }


        public override void OnSaveXml()
        {
            base.OnSaveXml();
        }


        private void InitControl()
        {
            //Set the Control. 
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("Dock");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("GridColor");
            ctrlData.PropertyFilter.Add("Ch1Color");
            ctrlData.PropertyFilter.Add("Ch2Color");
            ctrlData.PropertyFilter.Add("ChViewColor");
            _chartCtrl = new OscilloscopeView();
            _chartCtrl.Tag = ctrlData;
            List<Control> controls = new List<Control>();
            controls.Add(_chartCtrl);
            base.RegisterControls(controls);         
        }
        
        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            Port dataInPort = InputPorts[0];

            if (base.Controls.Count == 0)
            {
                InitControl();
                valInfoList.Add( new ValidationInfo("A new Oscilloscope ontrol was created. The old coudn't be loaded because a new version of this control is available.",ValidationInfo.InfoType.Warning)); 
            }

            if (dataInPort.SignalList == null)
            {
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.OsciSigErr, this.Text), ValidationInfo.InfoType.Error));
                return;
            }

            if (dataInPort.SignalList.ChildNodes.Count < 1)
            {
                valInfoList.Add(new ValidationInfo(String.Format(StringResource.OsciSigErr, this.Text), ValidationInfo.InfoType.Error));
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1230);
        }
    }
}          

