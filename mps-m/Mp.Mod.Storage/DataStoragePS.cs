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
using System.Xml;
using System.Windows.Forms;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Storage
{
    /// <summary>
    /// Implements a data storage Process-Station.
    /// </summary>
    public class DataStoragePS : ProcessStation
    {
        public DataStoragePS()
        {
            base.Type = "Mp.Storage.PS.DataStorage";
            base.Text   = StringResource.WriteData;
            base.Group  = StringResource.Storage;
            base.Symbol = Images.DataStorage;
            base.Icon   = Images.DataStorageIcon;
        }
        
        public override string RuntimeModule
        {
            get { return "mps-storage"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.WriteData;
            base.Group = StringResource.Storage;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In",true, false);
            AddPort(port);

            //Trigger port.
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset + DistanceBetweenPort)), "Mp.Port.Trigger", true, false);
            port.ConnectorBrush = new SolidBrush(Color.Black);
            AddPort(port);
            InitTriggerPortMenu();
        }

        public override string Description
        {
            get
            {
                return StringResource.WriteDataPsDescription;
            }
        }
        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 360);
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            DataStoragePSDlg dlg = new DataStoragePSDlg(Document);
            dlg.PSName = this.Text;
            dlg.XmlRep = XmlRep;
            
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            Text = dlg.PSName;
        }
        protected void OnTriggerPortProperties(object sender, EventArgs e)
        {
            TriggerPortDlg dlg = new TriggerPortDlg(Document, _triggerPort.XmlRep);
            dlg.ShowDialog();
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitTriggerPortMenu();
        }
        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);

            if (port.ContextMenuStrip != null)
                OnTriggerPortProperties(port.ContextMenuStrip.Items[0], null);
        }

        private void AddASignalIsNeededMsg(List<ValidationInfo> valInfoList)
        {
            string msg = String.Format(StringResource.TriggerPortSigError,this.Text); 
            valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
        }
        
        private void Add2SignalAreNeededMsg(List<ValidationInfo> valInfoList)
        {
            string msg = String.Format(StringResource.TriggerPortNeed2SigErr,this.Text);
            valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
            Port port = InputPorts[0];
            Port trigerPort = InputPorts[1];

            //Check connection
            if (!port.Connected)
            {
                string msg = String.Format(StringResource.DataInPortNotConnectedErr,this.Text);                
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }

            //Check file
            string file = XmlHelper.GetParam(XmlRep, "fileName");
            if (file == "")
            {
                string msg = String.Format(StringResource.StorageFileNotDef,this.Text);                
                valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
            }
            else
            {
                if (file.Length > 0)
                {//is a property is this property availanle
                    if (file[0] == '$')
                    {
                        if (!Document.IsPropertyAvailable(file))
                        {
                            string msg = String.Format(StringResource.StorageFileNotDef, this.Text);
                            msg += " " + StringResource.PropNotAvail;
                            valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                        }
                    }
                }
            }
            //Check measurement properties
            string meaComment  = XmlHelper.GetParam(XmlRep, "meaComment");
            if (meaComment.Length > 0)
            {
                if (meaComment[0] == '$')
                {
                    if (!Document.IsPropertyAvailable(meaComment))
                    {
                        string msg = String.Format(StringResource.MeaCommentNotDef,this.Text);
                        msg += " " + StringResource.PropNotAvail;
                        valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    }
                }
            }

            //Check Command
            string cmd = XmlHelper.GetParam(XmlRep, "command");
            if (cmd.Length > 0)
            {
                if (cmd[0] == '$')
                {
                    if (!Document.IsPropertyAvailable(cmd))
                    {
                        string msg = String.Format(StringResource.CommandPropNotDef, this.Text);
                        msg += " " + StringResource.PropNotAvail;
                        valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));
                    }
                }
            }
            //Check storage properties
            for (int i = 0; i < XmlRep.ChildNodes.Count; ++i)
            {
                XmlElement xmlProp = (XmlElement)XmlRep.ChildNodes[i];

                if (!xmlProp.HasAttributes)
                    continue;

                if (xmlProp.Attributes["name"] == null)
                    continue;

                if (xmlProp.Attributes["name"].Value != "property")
                    continue;

                string[] item = new string[1];

                string property = xmlProp.InnerText;
                if (!Document.IsPropertyAvailable(property))
                {
                    string msg = String.Format(StringResource.StorageFileNotDef, this.Text);
                    valInfoList.Add(new ValidationInfo(msg, ValidationInfo.InfoType.Error));                    
                }
            }
            
            //Check trigger
            long type = XmlHelper.GetParamNumber(trigerPort.XmlRep, "triggerType");
            switch (type)
            {
                case 0: //	NoTrigger = 0,
                break;
		        case 1: //StartTrigger,                
		        case 2: //StopTrigger,                
                case 4: //EventTrigger
                    if (trigerPort.SignalList == null)
                        AddASignalIsNeededMsg(valInfoList);
                    else if( trigerPort.SignalList.InnerText == "")
                        AddASignalIsNeededMsg(valInfoList);

                break;
		        case 3://StartStopTrigger,
                    long startStopType = XmlHelper.GetParamNumber(trigerPort.XmlRep, "oneStartStopSignal");
   
                    if (startStopType == 1)
                    {
                        if (trigerPort.SignalList == null)
                            AddASignalIsNeededMsg(valInfoList);
                        else if (trigerPort.SignalList.InnerText == "")
                            AddASignalIsNeededMsg(valInfoList);
                    }
                    else
                    {
                        if (trigerPort.SignalList == null)
                            Add2SignalAreNeededMsg(valInfoList);
                        else if( trigerPort.SignalList.ChildNodes.Count < 2)
                            Add2SignalAreNeededMsg(valInfoList);   
                    }
                break;
		
            }       
        }

        private void InitTriggerPortMenu()
        {
            foreach (Port port in InputPorts)
            {
                if (port.Type != "Mp.Port.Trigger")
                    continue;

                //Create the context menu.
                port.ContextMenuStrip = new ContextMenuStrip();
                port.ContextMenuStrip.Tag = port;

                ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.MenuProperties);

                menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
                menuItem.Click += new System.EventHandler(this.OnTriggerPortProperties);
                port.ContextMenuStrip.Items.Add(menuItem);
                _triggerPort = port;
                return;
            }
        }

        private Port _triggerPort;

    }
}