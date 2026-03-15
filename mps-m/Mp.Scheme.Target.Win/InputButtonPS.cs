using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;

using LBSoft.IndustrialCtrls.Buttons;
using Mp.Scheme.Sdk;
using Mp.Components;
using Atesion.Utils;

namespace Mp.Scheme.Win
{
    internal class InputButtonPS : VisualPS
    {
        private Assembly _myControlsAssembly;

        public InputButtonPS()
        {
            base.Group = StringResource.Control;
            base.Type = "PS_INPUT";
            base.Text = StringResource.Button;
            base.Symbol = Mp.Scheme.Win.Images.InputButton;
            base.Icon = Mp.Scheme.Win.Images.ButtonIcon;
            base.SubType = "Mp.Runtime.Sdk.InputButtonPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\LBIndustrialCtrls.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.ButtonPsDescription;
            }
        }

        public override void OnDefaultInit()
        {
            //Conntrol
            ControlData ctrlData = new ControlData();
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Text");
            ctrlData.PropertyFilter.Add("BackColor");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("ButtonColor");
            ctrlData.PropertyFilter.Add("State");
            ctrlData.PropertyFilter.Add("Label");
            ctrlData.PropertyFilter.Add("Font");
            ctrlData.PropertyFilter.Add("Type");
            ctrlData.PropertyFilter.Add("Dock");
            
            LBButton button = new LBButton();
            button.Tag = ctrlData;
            button.Text = this.Text;
            button.Name = this.Text;

            RegisterControl(button);

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

        private void OnPropertyDataPort(object sender, EventArgs e)
        {
            Port port = OutputPorts[0];
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document,1);
            dlg.ShowDialog();
        }

        protected override void OnProperties(object sender, EventArgs e)
        {
            base.OnProperties(sender, e);
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }
        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);

            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document,1);
            dlg.ShowDialog();
        }
    }
}
