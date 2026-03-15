using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

using Mp.Utils;
using Mp.Scheme.Sdk;
//

namespace Mp.Mod.CAN
{
    public class CANOutputPS : WorkPS
    {
        public CANOutputPS()
        {
            base.Type = "Mp.CAN.PS.Out";
            base.Text = StringResource.CANOutput;
            base.Group = StringResource.CANBus;
            base.Symbol = Mp.Mod.CAN.Resource.CANImgOUT;
            base.Icon = Mp.Mod.CAN.Resource.CANIconOUT; 
        }


        public override string RuntimeModule
        {
            get
            {
                return "mps-can";
            }
        } 

        public override void OnLoadResources()
        {
            base.Text = StringResource.CANOutput;
            base.Group = StringResource.CANBus;
        }

        protected override void OnDocumentChanged()
        {            
            XmlElement xmlChannels = XmlHelper.GetChildByType(XmlRep, "Mp.CAN.OutChannels");

            if (xmlChannels == null)
                return;

            Port port = InputPorts[0];

            //Update the signals ids
            for (int i = 0; i < xmlChannels.ChildNodes.Count; ++i)
            {
                XmlElement xmlChannel = (XmlElement)xmlChannels.ChildNodes[i];

                if (!xmlChannel.HasAttribute("type"))
                    continue;

                if (xmlChannel.Attributes["type"].Value != "Mp.CAN.OutChannel")
                    continue;

                uint sigId = (uint)XmlHelper.GetParamNumber(xmlChannel, "signal");

                XmlElement xmlSig = Document.GetXmlObjectById(sigId);

                if (xmlSig == null || port.SignalList == null)
                {
                    XmlHelper.SetParamNumber(xmlChannel, "signal", "uint32_t", 0);
                    continue;
                }

                bool found = false;                

                foreach (XmlElement xmlPortSig in port.SignalList.ChildNodes)
                {
                    XmlElement xmlSignal = xmlPortSig;

                    if (XmlHelper.GetObjectID(xmlPortSig) == 0)
                        xmlSignal = Document.GetXmlObjectById(Convert.ToUInt32(xmlPortSig.InnerText));

                    if (xmlSignal == xmlSig)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    XmlHelper.SetParamNumber(xmlChannel, "signal", "uint32_t", 0);
                    continue;
                }               
            }
        }


        public override string Description
        {
            get
            {
                return StringResource.CANOutputPsDescription;
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 860);
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data in port.
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

            XmlHelper.SetParam(XmlRep, "driver", "string", "mps-can-demo");
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            OnMouseDoubleClick(new Point());
        }

        public override void OnMouseDoubleClick(Point p)
        {
            CANOutputPSDlg dlg = new CANOutputPSDlg(Document, InputPorts[0].SignalList, this.XmlRep);
            
            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep,"name");
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            
            //Check if any signal is mapped.
            XmlElement xmlDevices = XmlHelper.GetChildByType(XmlRep, "Mp.CAN.OutChannels");
            if (xmlDevices == null)
            {
                string msg = String.Format(StringResource.CANOutError, base.Text) ;
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                return;
            }

            if (xmlDevices.ChildNodes.Count == 0)
            {
                string msg = String.Format(StringResource.CANOutError, base.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }
    }
}
