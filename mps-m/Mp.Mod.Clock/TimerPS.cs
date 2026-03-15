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
using System.Drawing;
using System.Windows.Forms;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Clock
{
    public class TimerPS : ProcessStation
    {
        public TimerPS()
        {
            base.Type   = "Mp.Clock.PS.Timer";
            base.Text   = StringResource.Timer;
            base.Group  = StringResource.Time;
            base.Symbol = Images.Timer;
            base.Icon   = Images.TimerIcon;
        }

        public override string RuntimeModule
        {
            get { return "mps-clock"; }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data out port.
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);
            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);
            AddPort(port);
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.Timer;
            base.Group = StringResource.Time;
        }

        public override string Description
        {
            get
            {
                return StringResource.TimerPsDescription;
            }
        }

        private void InitMenuForPort(Port port)
        {
            //Create the context menu.
            port.ContextMenuStrip = new ContextMenuStrip();
            port.ContextMenuStrip.Tag = port;

            ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.MenuProperties);

            menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            menuItem.Click += new System.EventHandler(this.OnPropertyDataPort);
            port.ContextMenuStrip.Items.Add(menuItem);
        }

        protected void OnPropertyDataPort(object sender, EventArgs e)
        {
            OnPortDoubleClick(OutputPorts[0]);
        }

        public override void OnPortDoubleClick(Port port)
        {
            TimerPortDlg dlg = new TimerPortDlg(Document,port.SignalList);
            dlg.ShowDialog();
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            Port dataOutPort = OutputPorts[0];

            if (!dataOutPort.Connected)
            {
                string msg = String.Format(StringResource.DataOutPortConError,this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            if (dataOutPort.SignalList.InnerText == "")
            {
                string msg = String.Format(StringResource.DataOutPortSigError, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 560);
        }
    }
}
