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

using Mp.Utils;

namespace Mp.Mod.CAN
{
    public class CANWritePS : WorkPS
    {
        public CANWritePS()
        {
            base.Type = "Mp.CAN.PS.Write";
            base.Text = StringResource.CANWrite;
            base.Group = StringResource.CANBus;
            base.Symbol = Mp.Mod.CAN.Resource.CANImg;
            base.Icon = Mp.Mod.CAN.Resource.CANIcon;
            base.IsSingleton = false; 
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.CANWrite;
            base.Group = StringResource.CANBus;
        }

        public override void OnDefaultInit()
        {            

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        public override string Description
        {
            get
            {
                return StringResource.CANWritePsDescription;
            }
        }


        protected override void OnDocumentChanged()
        {
            base.OnDocumentChanged();

            //Remove the old mapping
            string newMapping = "";
            string mapping = XmlHelper.GetParam(XmlRep, "signalTypeMap");            
            string[] arrayMapping = mapping.Split('#');
               
            foreach(string map in arrayMapping)
            {
                if (map == "")
                    continue;

                string[] array = map.Split(new char[] { '/' });
                uint sigId = Convert.ToUInt32(array[0]);

                if (IsSignalInPortsAvailable(sigId, true, null))
                    newMapping += (map + "#");
            }

            XmlHelper.SetParam(XmlRep, "signalTypeMap", "string", newMapping); 

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
            //Document.ShowHelp(this.Site, 890);
        }
    }
}