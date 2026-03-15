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
using System.Windows.Forms;
using System.Drawing;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Event
{
    public class EventPS : WorkPS
    {
        public EventPS()
        {
            base.Type = "Mp.Event.PS";
            base.Text = StringResource.Event;
            base.Group = StringResource.SignalControl;
            base.Symbol = Mp.Mod.Event.Images.EventImage;
            base.Icon = Mp.Mod.Event.Images.EventIcon;
        }


        public override string RuntimeModule
        {
            get
            {
                return "mps-event";
            }
        } 

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.Event;
            base.Group = StringResource.SignalControl;
        }

        public override string Description
        {
            get
            {
                return StringResource.EventPsDescription;
            }
        } 

        protected override void OnProperties(object sender, EventArgs e)
        {
            EventPSDlg dlg = new EventPSDlg(InputPorts[0].SignalList, this.XmlRep,this.Document);
            
            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            XmlElement xmlEvents = XmlHelper.GetChildByType(XmlRep, "Mp.Event.RtEvents");

            if (xmlEvents == null)
            {
                string msg = String.Format(StringResource.NoEventErr,this.Text);
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
            XmlElement xmlEvents = XmlHelper.GetChildByType(XmlRep, "Mp.Event.RtEvents");

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

                if(!IsSignalInPortsAvailable(sigId,true,null))
                {
                    Document.RemoveXmlObject(xmlEvent);
                    --i;
                    continue;
                }
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1100);
        }
    }
}
