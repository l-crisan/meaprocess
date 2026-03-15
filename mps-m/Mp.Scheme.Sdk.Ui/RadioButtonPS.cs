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
using System.Reflection;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using Mp.Utils;

namespace Mp.Scheme.Sdk.Ui
{
    public class RadioButtonPS : VisualPS
    {
        private Assembly _myControlsAssembly;

        public RadioButtonPS()
        {
            base.Group = StringResource.Control;
            base.Type = "Mp.PS.SysIn";
            base.Text = StringResource.RadioButtons;
            base.Symbol = Images.RadioButtonImg;
            base.Icon = Images.RadioButtonIcon;
            base.SubType = "Mp.Runtime.Sdk.RadioButtonPS";

            string currentAssemblyPath = Path.GetDirectoryName(this.GetType().Assembly.Location);

            _myControlsAssembly = Assembly.LoadFrom(currentAssemblyPath + "\\Mp.Visual.Digital.dll");
        }

        public override string Description
        {
            get
            {
                return StringResource.RadionButtonsPsDescription;
            }
        }

        public override void OnLoadResources()
        {
            base.Text = StringResource.RadioButtons;
            base.Group = StringResource.Control;
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
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Titel");
            ctrlData.PropertyFilter.Add("Items");
            ctrlData.PropertyFilter.Add("Dock");

            Mp.Visual.Digital.RadioButton button = new Mp.Visual.Digital.RadioButton();
            button.Tag = ctrlData;
            button.Titel = this.Text;
            button.Name = this.Text;
            RegisterControl(button);

            base.OnDefaultInit();

            //Data port. 
            Port port = new Port(new Point(_rectangle.Right + PortWidth, (int)(_rectangle.Top + PortTopOffset)), "Mp.Port.Out", false, true);

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
            OnPortDoubleClick(port);
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

        public override void AppendControl(Control control)
        {
            base.AppendControl(control);
            Mp.Visual.Digital.RadioButton button = (Mp.Visual.Digital.RadioButton)control;
            button.UpdateView();
        }

        public override void OnPortDoubleClick(Port port)
        {
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document, SignalDataType.LREAL);
            dlg.ShowDialog();
        }

        public override void OnHelpRequested()
        {
            Document.ShowHelp(this.Site, 1490);
        }
    }
}
