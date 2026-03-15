using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;

using Mp.Scheme.Sdk;
using Mp.Scheme.Sdk.Ui;

using Mp.Visual.OBD2;
using Mp.Utils;

namespace Mp.Mod.OBD2.View
{
    public class OBD2DTCViewPS : VisualPS
    {
        public OBD2DTCViewPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.OBD2DTCView;
            base.Group = "OBD2";
            base.Symbol =Resource.OBD2Img;
            base.Icon = Resource.OBD2Icon;
            base.SubType = "Mp.Runtime.Sdk.OBD2DTCViewPS";
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.OBD2DTCView;
            base.Group = "OBD2";
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
                return StringResource.DTCViewDescription;
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1020);
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

            OBD2DTCView view = new OBD2DTCView();
            view.Tag = ctrlData;

            RegisterControl(view);
        }
    }
}