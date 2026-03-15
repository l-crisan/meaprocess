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
    public class OBD2StatusViewPS : VisualPS
    {
        public OBD2StatusViewPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.OBD2StatusView;
            base.Group = "OBD2";
            base.Symbol = Resource.OBD2Img;
            base.Icon = Resource.OBD2Icon;
            base.SubType = "Mp.Runtime.Sdk.OBD2StatusViewPS";

        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.OBD2StatusView;
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
                return StringResource.StatusViewDescription;
            }
        }

        protected override void OnUpdateSignalList()
        {
            base.OnUpdateSignalList();

            //Remove the old mapping
            for (int i = 0; i < XmlRep.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement)XmlRep.ChildNodes[i];

                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");

                if (name != "signalTypeMap")
                    continue;

                string[] array = xmlElement.InnerText.Split(new char[] { '/' });
                uint sigId = Convert.ToUInt32(array[0]);

                if (!IsSignalInPortsAvailable(sigId, true, null))
                {
                    XmlRep.RemoveChild(xmlElement);
                    --i;
                }
            }
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            OBD2StatusViewDlg dlg = new OBD2StatusViewDlg(Document, XmlRep, InputPorts[0].SignalList);
            
            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
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

            OBD2StatusView view = new OBD2StatusView();
            view.Tag = ctrlData;

            RegisterControl(view);
        }
    
        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);

            bool mapped = false;

            for (int i = 0; i < XmlRep.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement)XmlRep.ChildNodes[i];

                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");

                if (name != "signalTypeMap")
                    continue;

                mapped = true;
                break;
            }

            if (!mapped)
            {
                string msg = String.Format(StringResource.StatusSigMapErr,this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1010);
        }

    }
}
