//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2010-2016  Laurentiu-Gheorghe Crisan
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
using System.Windows.Forms;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Audio
{
    public class AudioOutPS : WorkPS
    {
        public AudioOutPS()
        {
            base.Type = "Mp.PS.Audio.Out";
            base.Text = StringResource.AudioOutput;
            base.Group = StringResource.Audio;
            base.Symbol = Audio.AudioOut;
            base.Icon = Audio.AudioOutIcon;
            base.IsSingleton = true;
        }

        public override string RuntimeModule
        {
            get
            {
                return "mps-audio";
            }
        }
        public override void OnLoadResources()
        {
            base.Text = StringResource.AudioOutput;
            base.Group = StringResource.Audio;
        }

        protected override void OnDocumentChanged()
        {
            XmlElement xmlDevices = XmlHelper.GetChildByType(XmlRep,"Mp.Audio.Channels");
            
            if( xmlDevices == null)
                return;

            if (InputPorts.Count == 0)
            {
                for(int i = 0; i < xmlDevices.ChildNodes.Count; ++i)
                {
                    xmlDevices.RemoveChild(xmlDevices.ChildNodes[i]);
                    --i;
                }
                return;
            }

            Port port = InputPorts[0];

            //Update the signals ids
            for (int i = 0; i < xmlDevices.ChildNodes.Count; ++i)
            {
                XmlElement xmlChannel = (XmlElement) xmlDevices.ChildNodes[i];

                if (!xmlChannel.HasAttribute("type"))
                    continue;

                if (xmlChannel.Attributes["type"].Value != "Mp.Audio.Channel")
                    continue;

                if (port.SignalList == null)
                {
                    Document.RemoveXmlObject(xmlChannel);
                    --i;
                    continue;
                }

                uint sigId = (uint)XmlHelper.GetParamNumber(xmlChannel, "signal");

                XmlElement xmlSig = Document.GetXmlObjectById(sigId);

                if (xmlSig == null)
                {
                    Document.RemoveXmlObject(xmlChannel);
                    --i;
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
                    Document.RemoveXmlObject(xmlChannel);
                    --i;
                    continue;
                }                
            }
        }

        public override string Description
        {
            get
            {
                return StringResource.AudioOutPsDescription;
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 430);
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data in port.
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            OnMouseDoubleClick(new Point());
        }

        public override void OnMouseDoubleClick(Point p)
        {
            try
            {
                Port port = InputPorts[0];
                AudioOutPSDlg dlg = new AudioOutPSDlg(Document, XmlRep, port.SignalList);
                dlg.PsName = base.Text;

                if (dlg.ShowDialog() == DialogResult.OK)
                    base.Text = dlg.PsName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);

            //Check if any signal is mapped.
            XmlElement xmlDevices = XmlHelper.GetChildByType(XmlRep, "Mp.Audio.Channels");

            if (xmlDevices == null)
            {
                string msg = String.Format(StringResource.OutPSSignalMapErr,this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                return;
            }

            if (xmlDevices.ChildNodes.Count == 0)
            {
                string msg = String.Format(StringResource.OutPSSignalMapErr, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }
    }
}
