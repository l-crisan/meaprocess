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
using Mp.Scheme.Sdk;
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Mod.Scripting
{
    internal class SystemInputPS : ProcessStation
    {
        public SystemInputPS()
        {
            base.Type = "Mp.PS.SysIn";
            base.Text = StringResource.SystemInput;
            base.Group = "I/O";
            base.Symbol = Images.SystemInputImg;
            base.Icon = Images.SystemInput;
            base.SubType = "Mp.Runtime.Win.SystemInputPS";
            base.IsSingleton = true;
        }

        public override string RuntimeModule
        {
            get { return ""; }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.SystemInput;
            base.Group = "I/O";
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);

            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);

            AddPort(port);
        }

        private void InitMenuForPort(Port port)
        {
            port.ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem menuItem = new ToolStripMenuItem(StringResource.PropertiesMenu);
            menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            menuItem.Click += new System.EventHandler(this.OnPropertyDataPort);
            port.ContextMenuStrip.Items.Add(menuItem);
        }

        private void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            SystemInputPortDlg dlg = new SystemInputPortDlg(this.Document, port.SignalList);
            dlg.ShowDialog();
         
        }
        public override string Description
        {
            get
            {
                return StringResource.SystemInputPsDescription;
            }
        }
        public override void OnHelpRequested()
        {
            //Document.ShowHelp(this.Site, 360);
        }


        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);
            OnPropertyDataPort(null, null);
        }
    }
}
