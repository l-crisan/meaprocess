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
using Mp.Utils;

namespace Mp.Mod.Clock
{
    public class StopWatchPS : ProcessStation
    {
        public StopWatchPS()
        {
            base.Type = "Mp.Clock.PS.StopWatch";
            base.Text = StringResource.StopWatch;
            base.Group = StringResource.Time;
            base.Symbol = Images.Timer;
            base.Icon = Images.TimerIcon;
        }

        public override string RuntimeModule
        {
            get { return "mps-clock"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.StopWatch;
            base.Group = StringResource.Time;
        }

        public override string Description
        {
            get
            {
                return StringResource.StopWatchPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data out port.
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);
            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);
            AddPort(port);

            //Create the trigger in port
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Trigger", true, false);
            port.ConnectorBrush = new SolidBrush(Color.DarkGreen);
            InitMenuForPort(port);
            AddPort(port);

            //Create the reset in port
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset + DistanceBetweenPort)), "Mp.Port.Trigger", true, false);
            port.ConnectorBrush = new SolidBrush(Color.DarkGreen);
            AddPort(port);

            XmlHelper.SetParamNumber(port.XmlRep, "triggerType", "uint8_t", 4);
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

        public override void OnLoadXml()
        {
            base.OnLoadXml();

            InitMenuForPort(InputPorts[0]);
            InitMenuForPort(OutputPorts[0]);
        }

        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);                        
            ShowPortDlg(port);
        }

        protected void OnPropertyDataPort(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (sender as ToolStripMenuItem);

            Port port = (item.Owner.Tag as Port);

            ShowPortDlg(port);     
        }

        private void ShowPortDlg(Port port)
        {
            if (port.Type == "Mp.Port.Out")
            {
                StopWatchOutPortDlg dlg = new StopWatchOutPortDlg(Document, port.SignalList);
                dlg.ShowDialog();
            }
            else
            {
                if (InputPorts[0] == port)
                {
                    SimpleTriggerPortDlg dlg = new SimpleTriggerPortDlg(Document, port.XmlRep);
                    dlg.ShowDialog();
                }
            }
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);

            Port dataOutPort = OutputPorts[0];

            if (!dataOutPort.Connected)
            {
                string msg = String.Format(StringResource.DataOutPortConError, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            if (dataOutPort.SignalList.InnerText == "")
            {
                string msg = String.Format(StringResource.DataOutPortSigError, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            Port triggerPort = InputPorts[0];

            int triggerType = (int)XmlHelper.GetParamNumber(triggerPort.XmlRep, "triggerType");

            if (triggerType == 0)
                return;

            if (!triggerPort.Connected)
            {
                string msg = String.Format(StringResource.TriggerPortNotConErr, this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                return;
            }

            switch (triggerType)
            {
                case 1:
                case 2:
                    if (triggerPort.SignalList.ChildNodes.Count < 1)
                    {
                        string msg = String.Format(StringResource.TriggerOneSigErr, this.Text);
                        valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    }
                break;

                case 3:
                {
                    bool oneStartStop = XmlHelper.GetParamNumber(triggerPort.XmlRep, "oneStartStopSignal") > 0;

                    if (triggerPort.SignalList.ChildNodes.Count < 2 && !oneStartStop)
                    {
                        string msg = String.Format(StringResource.TriggerTwoSigErr,this.Text);
                        valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    }

                    if (triggerPort.SignalList.ChildNodes.Count < 1 && oneStartStop)
                    {
                        string msg = String.Format(StringResource.TriggerOneSigErr, this.Text);
                        valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    }
                }
                break;
            }
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 570);
        }
    }
}
