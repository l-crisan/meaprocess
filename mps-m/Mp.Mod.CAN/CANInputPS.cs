using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;
using Mp.Utils;
using Mp.Scheme.Sdk;
using Mp.Visual.Diagram;
using Mp.Drv.CAN;

namespace Mp.Mod.CAN
{
    public class CANInputPS : ProcessStation
    {        
        public CANInputPS()
        {
            base.Type = "Mp.CAN.PS.In";
            base.Text = StringResource.CANInput;
            base.Group = StringResource.CANBus;
            base.Symbol = Mp.Mod.CAN.Resource.CANImgIN;
            base.Icon = Mp.Mod.CAN.Resource.CANIconIN;
            base.IsSingleton = false;        
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
            base.Text = StringResource.CANInput;
            base.Group = StringResource.CANBus;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            for (int i = 0; i < 2; ++i)            
                CreatePort();

            XmlHelper.SetParam(XmlRep, "driver", "string", "mps-can-demo");
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (OutputPorts.Count == 4)
            {
                this.ContextMenuStrip.Items[1].Enabled = false;
                this.ContextMenuStrip.Items[2].Enabled = false;
            }
            else
            {
                this.ContextMenuStrip.Items[1].Enabled = true;
                this.ContextMenuStrip.Items[2].Enabled = true;
            }            
        }

        public override string Description
        {
            get
            {
                return StringResource.CANInputPsDescription;
            }
        }

        private void CreatePort()
        {
            int offset = 0;

            if (OutputPorts.Count > 0)
            {
                Port lastPort = OutputPorts[OutputPorts.Count - 1];
                Point pos = Site.PointToClient(lastPort.Position);
                offset = (lastPort.Position.Y - _rectangle.Top - PortTopOffset) + DistanceBetweenPort;
            }

            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset + offset)), "Mp.CAN.Port", false, true);


            port.SignalList = Document.CreateSignalList();            

            InitMenuForPort(port);

            AddPort(port);
        }
        
        private void InitMenuForPort(Port port)
        {
            //Create the context menu.
            port.ContextMenuStrip = new ContextMenuStrip();
            port.ContextMenuStrip.Tag = port;

            ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.MenuProperty);

            menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            menuItem.Click += new System.EventHandler(this.OnPropertyDataPort);
            menuItem.Tag = port;

            port.ContextMenuStrip.Items.Add(menuItem);            
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();

            foreach (Port port in OutputPorts)
                InitMenuForPort(port);
        }

        public override void OnPortDoubleClick(Port port)
        {
            ShowPortDialog(port);
        }

        private void ShowPortDialog(Port port)
        {
            int i = 0;
            for (; i < OutputPorts.Count; ++i)
            {
                if (port == OutputPorts[i])
                    break;
            }

            CANPortDlg dlg = new CANPortDlg(port, i, Document);
            dlg.ShowDialog();
        }

        protected void OnPropertyDataPort(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            ShowPortDialog((Port)menuItem.Tag);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            bool connected  = false;
            bool hasSignals = false;

            //Check for signal in the output ports.
            for (int i = 0; i < OutputPorts.Count; ++i)
            {
                Port outPort = OutputPorts[i];            
                if (outPort.Connected)
                    connected = true;

                if (outPort.SignalList.ChildNodes.Count != 0 )
                {
                    hasSignals = true;
                    if(!outPort.Connected)
                    {
                        string msg = String.Format(StringResource.OutPortINotCon,(i+1).ToString(),base.Text);
                        valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    }
                }
            }

            if (!connected)
            {
                string msg = String.Format(StringResource.OutPortNotCon,base.Text);

                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            if (!hasSignals)
            {
                string msg = String.Format(StringResource.OutPortSigErr, base.Text);

                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            CANDrvPropDlg dlg = new CANDrvPropDlg(this.XmlRep, Document);
            
            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text =XmlHelper.GetParam(XmlRep,"name");
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 520);    
        }
    }
}
