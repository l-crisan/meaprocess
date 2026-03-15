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
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Mp.Scheme.Sdk;
using Mp.Scheme.Sdk.Ui;
using Mp.Visual.HTML;
using Mp.Utils;

namespace Mp.Mod.HTML.View
{
    public class TextViewPS : VisualPS
    {
        public TextViewPS()
        {
            base.Type = "Mp.PS.SysOut";
            base.Text = StringResource.TextView;
            base.Group = StringResource.BlockDisplay;
            base.Symbol = Mp.Mod.HTML.View.Resource.TextView;
            base.Icon = Mp.Mod.HTML.View.Resource.TextViewIcon;
            base.SubType = "Mp.Runtime.Sdk.TextViewPS";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Mp.Visual.HTML.dll");
            Assembly.LoadFile(path);
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.TextView;
            base.Group = StringResource.BlockDisplay;
        }


        public override string Description
        {
            get
            {
                return StringResource.TextViewPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            CreateControl();

            base.OnDefaultInit();

            //Data port
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);

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

            TextView view = new TextView();
            view.Tag = ctrlData;
            view.Name = this.Text;

            RegisterControl(view);
        }

        protected override void OnUpdateSignalList()
        {
            base.OnUpdateSignalList();
            string data = XmlHelper.GetParam(XmlRep, "events");

            if (data == "")
                return;

            Port port = InputPorts[0];

            XmlDocument xmlEventDoc = new XmlDocument();
            xmlEventDoc.LoadXml(data);

            //Update the signals ids
            for (int i = 0; i < xmlEventDoc.DocumentElement.ChildNodes.Count; ++i)
            {
                XmlElement xmlEvent = xmlEventDoc.DocumentElement.ChildNodes[i] as XmlElement;
                
                if (xmlEvent == null)
                    continue;

                uint sigId = Convert.ToUInt32(xmlEvent["signal"].InnerText);

                if (sigId == 0)
                    continue;

                if (port.SignalList == null)
                {
                    xmlEventDoc.DocumentElement.RemoveChild(xmlEvent);
                    --i;
                    continue;
                }

                XmlElement xmlSig = Document.GetXmlObjectById(sigId);

                if (xmlSig == null)
                {
                    xmlEventDoc.DocumentElement.RemoveChild(xmlEvent);
                    --i;
                    continue;
                }

                if (!IsSignalInPortsAvailable(sigId, true, null))
                {
                    xmlEventDoc.DocumentElement.RemoveChild(xmlEvent);
                    --i;
                    continue;
                }
            }

            XmlHelper.SetParam(XmlRep, "events", "string", xmlEventDoc.InnerXml);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            TextPSDlg dlg = new TextPSDlg(InputPorts[0].SignalList, XmlRep, Document);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            string data = XmlHelper.GetParam(XmlRep, "events");
            
            if (data == "")
            {
                string msg = String.Format(StringResource.NoEventErr, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1210);
        }
    }
}