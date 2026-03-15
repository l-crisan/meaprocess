using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using Mp.Scheme.Sdk;

using Mp.Utils;

namespace Mp.Mod.CAN
{
    public class CANEventPS : WorkPS
    {
        public CANEventPS()
        {
            base.Type = "Mp.CAN.PS.Event";
            base.Text = StringResource.CANEventPS;        
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

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

            XmlHelper.SetParam(XmlRep, "driver", "string", "mps-can-demo");
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.CANEventPS;
            base.Group = StringResource.CANBus;
        }

        public override string Description
        {
            get
            {
                return StringResource.CANEventPsDescription;
            }
        }
        protected override void OnProperties(object sender, EventArgs e)
        {
            CANEventPSDlg dlg = new CANEventPSDlg(InputPorts[0].SignalList, this.XmlRep, this.Document);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            XmlElement xmlEvents = XmlHelper.GetChildByType(XmlRep, "Mp.CAN.Events");

            if (xmlEvents == null)
            {
                string msg = String.Format(StringResource.NoEventErr, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
            else
            {
                if (xmlEvents.ChildNodes.Count == 0)
                {
                    string msg = String.Format(StringResource.NoEventErr, this.Text);
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                }
            }
        }

        protected override void OnDocumentChanged()
        {
            XmlElement xmlEvents = XmlHelper.GetChildByType(XmlRep, "Mp.CAN.Events");

            if (xmlEvents == null)
                return;

            if (InputPorts.Count == 0)
            {
                for (int i = 0; i < xmlEvents.ChildNodes.Count; ++i)
                {
                    xmlEvents.RemoveChild(xmlEvents.ChildNodes[i]);
                    --i;
                }
                return;
            }

            Port port = InputPorts[0];

            //Update the signals ids
            for (int i = 0; i < xmlEvents.ChildNodes.Count; ++i)
            {
                XmlElement xmlEvent = (XmlElement)xmlEvents.ChildNodes[i];

                if (port.SignalList == null)
                {
                    Document.RemoveXmlObject(xmlEvent);
                    --i;
                    continue;
                }

                uint sigId = (uint)XmlHelper.GetParamNumber(xmlEvent, "signal");

                XmlElement xmlSig = Document.GetXmlObjectById(sigId);

                if (xmlSig == null)
                {
                    Document.RemoveXmlObject(xmlEvent);
                    --i;
                    continue;
                }

                if (!IsSignalInPortsAvailable(sigId, true, null))
                {
                    Document.RemoveXmlObject(xmlSig);
                    --i;
                    continue;
                }
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1120);
        }
    }
}
