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
using System.Drawing;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.LabJackU12
{
    internal class LabJackOutputPS : WorkPS
    {
        public LabJackOutputPS()
        {
            base.Type = "Mp.LabJackU12.PS.Out";
            base.Text = StringResource.LabJackOutput;
            base.Group = "I/O"; //StringResource.LabJack;
            base.Symbol = Mp.Mod.LabJackU12.Resource.LabJackIn;
            base.Icon = Mp.Mod.LabJackU12.Resource.LabJackInIcon;
            base.IsSingleton = true;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data in port.
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        public override string RuntimeModule
        {
            get
            {
                return "mps-labjack";
            }
        }
        public override void OnLoadResources()
        {
            base.Text = StringResource.LabJackOutput;
        }

        public override string Description
        {
            get
            {
                return StringResource.LabJackOutPsDescription;
            }
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            OnMouseDoubleClick(new Point());
        }

        protected override void OnDocumentChanged()
        {
            XmlElement xmlOutSignals = XmlHelper.GetChildByType(XmlRep, "Mp.LabJackU12.Output");

            if (xmlOutSignals == null)
                return;

            for (int i = 0; i < xmlOutSignals.ChildNodes.Count; ++i)
            {
                XmlElement xmlULOutSignal = (XmlElement) xmlOutSignals.ChildNodes[i];

                if (XmlHelper.GetObjectID(xmlULOutSignal) == 0)
                    continue;

                uint sigId = (uint) XmlHelper.GetParamNumber(xmlULOutSignal, "signalID");

                if (!IsSignalInPortsAvailable(sigId, true, null))
                {
                    Document.RemoveXmlObject(xmlULOutSignal);
                    --i;
                }              
            }
        }

        public override void OnMouseDoubleClick(Point p)
        {
            try
            {
                Port port = InputPorts[0];

                LabJackOutDlg dlg = new LabJackOutDlg(Document, XmlRep, port.SignalList);
                dlg.PsName = base.Text;

                if( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
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

            XmlElement xmlOutSignals = XmlHelper.GetChildByType(XmlRep, "Mp.LabJackU12.Output");
            if (xmlOutSignals == null)
            {
                string msg = String.Format(StringResource.LJOutSigErr, base.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                return;
            }

            if (xmlOutSignals.ChildNodes.Count == 0)
            {
                string msg = String.Format(StringResource.LJOutSigErr, base.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }

        public override void OnHelpRequested()
        {
            //Document.ShowHelp(this.Site, 960);
        }
    }
}
