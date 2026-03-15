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
using Mp.Visual.CAN;
using Mp.Utils;

namespace Mp.Mod.CAN.View
{
    public class CANViewPS : VisualPS
    {
        public CANViewPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.CANMessageView;
            base.Group = StringResource.CANBus;
            base.Symbol = Resource.CANImg;
            base.Icon = Resource.CANIcon;
            base.SubType = "Mp.Rt.Mod.CAN.View.PS";
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.CANMessageView;
            base.Group = StringResource.CANBus;
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
                return StringResource.CANViewPsDescription;
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
            ctrlData.PropertyFilter.Add("HexDataWidth");
            ctrlData.PropertyFilter.Add("DezDataWidth");
            ctrlData.PropertyFilter.Add("AsciiDataWidth");
            ctrlData.PropertyFilter.Add("HighLight");
            ctrlData.PropertyFilter.Add("NoOfMessages");
            ctrlData.PropertyFilter.Add("ViewMode");
            ctrlData.PropertyFilter.Add("ShowIDHexadecimal");
            ctrlData.ProcessStationLibrary = "Mp.Rt.Mod.CAN.View.dll";

            CANLoggerView view = new CANLoggerView();
            view.Tag = ctrlData;

            RegisterControl(view);
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
            CANViewDlg dlg = new CANViewDlg(Document, XmlRep, InputPorts[0].SignalList);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
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
                string msg = String.Format(StringResource.PSSigMap, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 890);
        }
    }
}