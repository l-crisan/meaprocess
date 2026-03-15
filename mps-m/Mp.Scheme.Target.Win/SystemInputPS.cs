using System;
using System.Collections.Generic;
using System.Text;
using Mp.Scheme.Sdk;
using Atesion.Utils;
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Scheme.Win
{
    internal class SystemInputPS : ProcessStation
    {
        public SystemInputPS()
        {
            base.Type = "PS_INPUT";
            base.Text = StringResource.SystemInput;
            base.Group = StringResource.General;
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
            base.Group = StringResource.General;
        }

        public override void OnDefaultInit()
        {
            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_OUTPUT", false, true);

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
