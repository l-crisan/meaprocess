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
using System.Windows.Forms;
using System.Drawing;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// Stop runtime Process-Station.
    /// </summary>
    public class StopPS : ProcessStation
    {
       public StopPS()
        {
            base.Type   = "Mp.PS.Stop";
            base.Text   = StringResource.Stop;
            base.Group  = StringResource.General;
            base.Symbol = Images.Stop;
            base.Icon   = Images.StopIcon;
        }


        public override string Description
        {
            get
            {
                return StringResource.StopPsDescription;
            }
        }


        public override void OnLoadResources()
        {
            base.Text = StringResource.Stop;
            base.Group = StringResource.General;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            StopPSPropDlg dlg = new StopPSPropDlg(this.XmlRep);
            
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
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 530);
        }
    }
}
