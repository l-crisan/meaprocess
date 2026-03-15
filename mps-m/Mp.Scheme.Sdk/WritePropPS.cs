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
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    public class WritePropPS : WorkPS
    {
        public WritePropPS()
        {
            base.Type   = "Mp.PS.WriteProp";
            base.Text   = StringResource.WriteProp;
            base.Group  = StringResource.General;
            base.Symbol = Mp.Scheme.Sdk.Images.PropertyImage;
            base.Icon = Mp.Scheme.Sdk.Images.Property;
            base.IsSingleton = true;
        }


        public override string Description
        {
            get
            {
                return StringResource.PropPsDescription;
            }
        }


        protected override void OnDocumentChanged()
        {
            base.OnDocumentChanged();

            StringBuilder sb = new StringBuilder();

            string mapping = XmlHelper.GetParam(XmlRep, "propMap");
            string[] array = mapping.Split('#');
            
            foreach(string strmap in array)
            {
                if (strmap == "")
                    continue;

                string[] ar = strmap.Split(';');
                uint sigId = Convert.ToUInt32(ar[0]);
                string propName = ar[1];
                
                if(!Document.IsPropertyAvailable(propName))
                    continue;

                if(!IsSignalInPortsAvailable(sigId, true, null))
                    continue;


                string map = sigId.ToString() + ";" + propName + "#";
                sb.Append(map);
            }

            mapping = sb.ToString();
            mapping = mapping.TrimEnd('#');

            XmlHelper.SetParam(XmlRep, "propMap", "string", mapping);
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.WriteProp;
            base.Group = StringResource.General;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            XmlHelper.SetParam(XmlRep, "propMap", "string", "");
            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            WritePropPSDlg dlg = new WritePropPSDlg(this.XmlRep, Document, InputPorts[0].SignalList);
            
            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = XmlHelper.GetParam(XmlRep, "name");

        }
        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);

            if (!InputPorts[0].Connected)
            {
                string msg = string.Format(StringResource.DataInPortNotConnectedErr, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            string mapping = XmlHelper.GetParam(XmlRep, "propMap");
            
            if( mapping == "")
            {
                string msg = string.Format(StringResource.NoPropMapped, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }

        public override void OnHelpRequested()
        {
            //Document.ShowHelp(this.Site, 530);
        }
    }
}
