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
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Controller
{
    internal class FuzzyControllerPS : WorkPS
    {
        private bool _invalidRules = false;

        public FuzzyControllerPS()
        {
            base.Type = "Mp.Controller.PS.FuzzyCtrl";
            base.Text = StringResource.FuzzyController;
            base.Group = StringResource.AutomaticControl;
            base.Symbol = Mp.Mod.Controller.Resource.FuzzyControllerImg;
            base.Icon = Mp.Mod.Controller.Resource.FuzzyControllerIcon;
            base.IsSingleton = false;
        }

        public override string RuntimeModule
        {
            get { return "mps-controller"; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.FuzzyController;
            base.Group = StringResource.AutomaticControl;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Create the data out port.
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);
            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);
            AddPort(port);

            //Data in port. 
            port = new Port(new Point(_rectangle.Left - PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.In", true, false);
            AddPort(port);
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        public override string Description
        {
            get
            {
                return StringResource.FuzzyControllerDescrp;
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

        protected override void OnDocumentChanged()
        {
            XmlElement xmlLinguisticVars = XmlHelper.GetChildByType(XmlRep, "Mp.Controller.LinguisticVars");
            
            if (xmlLinguisticVars == null)
                return;

            for (int i = 0; i < xmlLinguisticVars.ChildNodes.Count; ++i)
            {
                XmlElement xmlLigVar = (XmlElement) xmlLinguisticVars.ChildNodes[i];

                uint sigId = (uint) XmlHelper.GetParamNumber(xmlLigVar, "signal");

                if(!IsSignalInPortsAvailable(sigId,true, null))
                {
                    Document.RemoveXmlObject(xmlLigVar);
                    --i;
                    _invalidRules = true;
                }
            }
        }

        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);

            if (port.IsInput)
                return;

            OnPropertyDataPort(null, null);
        }

        protected void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            SignalViewDlg dlg = new SignalViewDlg(port.SignalList, Document);
            dlg.ShowDialog();
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1600);
        }

        protected override void OnValidate(List<ValidationInfo> valInfoList)
        {
            base.OnValidate(valInfoList);
        
            if (_invalidRules)
            {
                string message = String.Format(StringResource.InvalidRulesErr, this.Text);
                valInfoList.Add( new ValidationInfo(message,ValidationInfo.InfoType.Error));
            }
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            FuzzyControllerDlg dlg = new FuzzyControllerDlg(Document, XmlRep, InputPorts[0].SignalList, OutputPorts[0].SignalList);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                this.Text = XmlHelper.GetParam(XmlRep, "name");
                _invalidRules = false;
            }
        }
    }
}
