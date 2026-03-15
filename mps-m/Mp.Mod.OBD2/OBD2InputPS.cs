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

namespace Mp.Mod.OBD2
{
    public class OBD2InputPS : ProcessStation
    {
        public OBD2InputPS()
        {
            base.Type = "Mp.OBD2.PS.In";
            base.Text = StringResource.OBD2Input;
            base.Group = "OBD2";
            base.Symbol = Mp.Mod.OBD2.Resource.OBD2Img;
            base.Icon = Mp.Mod.OBD2.Resource.OBD2Icon;
            base.IsSingleton = false;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();
            XmlHelper.SetParamNumber(XmlRep, "canRate", "uint32_t", 500000);
            XmlHelper.SetParamNumber(XmlRep, "driverType", "uint32_t", 1);
            XmlHelper.SetParamNumber(XmlRep, "port", "uint32_t", 0);
            XmlHelper.SetParam(XmlRep, "driver", "string", "mps-can-vector");
            CreatePort();
        }

        public override string RuntimeModule
        {
            get
            {
                return "mps-obd2";
            }
        } 

        public override void OnLoadResources()
        {
            base.Text = StringResource.OBD2Input;
            base.Group = "OBD2";
        }

        public override string Description
        {
            get
            {
                return StringResource.InputPSDescription;
            }
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

        private void CreatePort()
        {
            int offset = 0;

            if (OutputPorts.Count > 0)
            {
                Port lastPort = OutputPorts[OutputPorts.Count - 1];
                Point pos = Site.PointToClient(lastPort.Position);
                offset = (lastPort.Position.Y - _rectangle.Top - PortTopOffset) + DistanceBetweenPort;
            }

            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset + offset)), "Mp.Port.Out", false, true);


            port.SignalList = Document.CreateSignalList();

            InitMenuForPort(port);

            AddPort(port);
        }

        private void InitMenuForPort(Port port)
        {
            //Create the context menu.
            port.ContextMenuStrip = new ContextMenuStrip();
            port.ContextMenuStrip.Tag = port;

            ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.PropertyMenu);

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
            PortPropDlg dlg = new PortPropDlg(this.XmlRep, port.SignalList, Document);
            dlg.ShowDialog();
        }

        protected void OnPropertyDataPort(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            ShowPortDialog((Port)menuItem.Tag);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            bool connected = false;
            bool hasSignals = false;

            //Check for signal in the output ports.
            for (int i = 0; i < OutputPorts.Count; ++i)
            {
                Port outPort = OutputPorts[i];
                if (outPort.Connected)
                    connected = true;

                if (outPort.SignalList.ChildNodes.Count != 0)
                {
                    hasSignals = true;
                    if (!outPort.Connected)
                    {
                        string msg = String.Format(StringResource.OutPortINotCon,(i + 1).ToString(), base.Text);
                        valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    }
                }
            }

            if (!connected)
            {
                string msg = String.Format(StringResource.OutPortsNotCon,this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            if (!hasSignals)
            {
                string msg = String.Format(StringResource.OutPortsNoSig,this.Text);
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            //Check properties
            XmlElement xmlVehicleInfos = XmlHelper.GetChildByType(XmlRep, "Mp.OBD2.VehicleInfos");

            if (xmlVehicleInfos != null)
            {
                foreach (XmlElement xmlVehicleInfo in xmlVehicleInfos.ChildNodes)
                {
                    string prop = XmlHelper.GetParam(xmlVehicleInfo, "property");

                    if (!Document.IsPropertyAvailable(prop))
                    {
                        string msg = String.Format(StringResource.PropNotAvailErr,prop, base.Text);
                        valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    }
                    else
                    {
                        string propType = Document.GetPropertyType(prop);
                        if (propType != "STRING")
                        {
                            string msg = String.Format(StringResource.PropWrongTypeErr, prop, base.Text);
                            valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                        }
                    }
                }
            }
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            PsPropDlg dlg = new PsPropDlg(this.XmlRep, Document);

            if (dlg.ShowDialog() == DialogResult.OK)
                this.Text = dlg.PsName;            
        }

        public override void OnHelpRequested()
        {            
            Document.ShowHelp(this.Site, 1000);
        }
    }
}
