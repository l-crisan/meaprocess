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
using System.Xml;
using System.Drawing;
using Mp.Scheme.Sdk;
using Mp.Scheme.Sdk.Ui;
using Mp.Visual.Video;
using Mp.Utils;


namespace Mp.Mod.VideoView
{
    public class VideoViewPS : VisualPS
    {
        public VideoViewPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.VideoView;
            base.Group = "Video";
            base.Symbol = Resource.VideoIn;
            base.Icon = Resource.video;
            base.SubType = "Mp.Runtime.Sdk.VideoViewPS";
            base.AcceptObjectSignal = true;
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.VideoView;
            base.Group = "Video";
        }

        public override void OnDefaultInit()
        {
            CreateControl();

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

        }

        public override string Description
        {
            get
            {
                return StringResource.VideoViewDescription;
            }
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
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Dock");
            ctrlData.PropertyFilter.Add("Stretch");
            ctrlData.PropertyFilter.Add("BackColor");

            VideoViewCtrl view = new VideoViewCtrl();
            view.Tag = ctrlData;

            RegisterControl(view);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);

            if (InputPorts[0].SignalList.ChildNodes.Count != 0)
            {                
                XmlElement xmlSignal = (XmlElement) InputPorts[0].SignalList.ChildNodes[0];
                if(!Document.IsVideoSignal(xmlSignal))
                {
                    string msg = String.Format(StringResource.InvalidInputSignal, this.Text);
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }
            }
        }

        public override void OnHelpRequested()
        {
            //Document.ShowHelp(this.Site, 1010);
        }

    }
}
