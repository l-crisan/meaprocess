using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Windows.Forms;

using Mp.Switch;
using Mp.Scheme.Sdk;
using Mp.Components;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    internal class SwitchPS : VisualPS
    {
       private Assembly _myControlsAssembly;

        public SwitchPS()
        {
            base.Group = StringResource.Control;
            base.Type = "PS_INPUT";
            base.Text = StringResource.Switch;
            base.Symbol = Mp.Scheme.Win.Images.Switch;
            base.Icon = Mp.Scheme.Win.Images.SwitchIcon;
            base.SubType = "Mp.Runtime.Sdk.SwitchPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Switch.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.SwitchPsDescription;
            }
        }

        public override void OnDefaultInit()
        {

            Mp.Switch.Switch s = new Mp.Switch.Switch();

            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Font");
            ctrlData.PropertyFilter.Add("OnText");
            ctrlData.PropertyFilter.Add("OffText");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Dock");
            s.Tag = ctrlData;
            s.Name = this.Text;
            RegisterControl(s);

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "PORT_OUTPUT", false, true);

            port.SignalList = Document.CreateSignalList();
            InitMenuForPort(port);

            ctrlData.SignalListId = XmlHelper.GetObjectID(port.SignalList);
            AddPort(port);   

        }

        private void InitMenuForPort(Port port)
        {
            port.ContextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem menuItem = new ToolStripMenuItem("Properties...");
            menuItem.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            menuItem.Click += new System.EventHandler(this.OnPropertyDataPort);
            port.ContextMenuStrip.Items.Add(menuItem);
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        private void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document,1);
            dlg.ShowDialog();
        }

        public override void OnPortDoubleClick(Port port)
        {
            OnPropertyDataPort(null, null);
        }
    }
}
