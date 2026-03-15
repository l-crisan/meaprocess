using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Windows.Forms;

using LBSoft.IndustrialCtrls.Knobs;
using Mp.Scheme.Sdk;
using Mp.Components;
using Mp.NumericView;

using Atesion.Utils;

namespace Mp.Scheme.Win
{
    internal class NumericInputPS : VisualPS
    {
        public NumericInputPS()
        {
            base.Group = StringResource.Control;
            base.Type = "PS_INPUT";
            base.Text = StringResource.NumericInput;
            base.Symbol = Mp.Scheme.Win.Images.NumericInput;
            base.Icon = Mp.Scheme.Win.Images.NumericInputIcon;
            base.SubType = "Mp.Runtime.Sdk.NumericInputPS";
        }

        public override string Description
        {
            get
            {
                return StringResource.NumericInputPsDescription;
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
            ctrlData.PropertyFilter.Add("Value");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("TextAlign");
            ctrlData.PropertyFilter.Add("NumberType");
            ctrlData.PropertyFilter.Add("Minimum");
            ctrlData.PropertyFilter.Add("Maximum");
            ctrlData.PropertyFilter.Add("Dock");   

            Mp.NumericView.NumericInputBox ni = new Mp.NumericView.NumericInputBox();
            ni.Value = 10;
            ni.Tag = ctrlData;
            ni.Left = 120;
            ni.Name = this.Text;

            List<Control> controls = new List<Control>();
            controls.Add(ni);

            ctrlData = new ControlData();
            ctrlData.PropertyFilter = new List<string>();
            ctrlData.PropertyFilter.Add("Left");
            ctrlData.PropertyFilter.Add("Top");
            ctrlData.PropertyFilter.Add("Width");
            ctrlData.PropertyFilter.Add("Height");
            ctrlData.PropertyFilter.Add("Text");
            ctrlData.PropertyFilter.Add("BorderStyle");
            ctrlData.PropertyFilter.Add("Visible");
            ctrlData.PropertyFilter.Add("Dock");
            Label nl = new Label();
            nl.Text = this.Text;
            nl.Tag = ctrlData;
            nl.Name = this.Text;
            controls.Add(nl);
            RegisterControls(controls);

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
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document,2);
            dlg.ShowDialog();
        }

        public override void OnLoadXml()
        {
            base.OnLoadXml();
            InitMenuForPort(OutputPorts[0]);
        }

        public override void OnPortDoubleClick(Port port)
        {
            base.OnPortDoubleClick(port);
            
            OneSigControlDlg dlg = new OneSigControlDlg(port.SignalList, Document,2);
            dlg.ShowDialog();
        }

        public override void OnSaveXml()
        {
            Port port = OutputPorts[0];

            XmlElement xmlSignal = XmlHelper.GetChildByType(port.SignalList, "Signal");
            foreach (Control control in Controls)
            {
                if (control is NumericInputBox && xmlSignal != null)
                {
                    NumericInputBox ctrl = (NumericInputBox) control;
                    XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(ctrl.Minimum));
                    XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(ctrl.Maximum));
                }
            }

            base.OnSaveXml();

        }
    }
}
