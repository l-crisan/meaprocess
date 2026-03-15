using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.CAN.View
{
    public partial class CANViewDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSinalList;
        private XmlElement _xmlPS;
        private SignalInputView _signals;

        public CANViewDlg(Document doc, XmlElement xmlPS, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlSinalList = xmlSignalList;
            _xmlPS = xmlPS;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
           
            psName.Text = XmlHelper.GetParam(_xmlPS, "name");
            LoadSignals();
            SetupChannels(); 
            LoadMapping();
        }

        private void SetupChannels()
        {
            channels.Rows.Clear();
            
            int index = channels.Rows.Add();
            DataGridViewRow row = channels.Rows[index];
            row.Cells[0].Value = "";
            row.Cells[1].Value = StringResource.TypeTimeStamp;

            index = channels.Rows.Add();
            row = channels.Rows[index];
            row.Cells[0].Value = "";
            row.Cells[1].Value = "Identifier";

            index = channels.Rows.Add();
            row = channels.Rows[index];
            row.Cells[0].Value = "";
            row.Cells[1].Value = "DLC";

            index = channels.Rows.Add();
            row = channels.Rows[index];
            row.Cells[0].Value = "";
            row.Cells[1].Value = StringResource.Message;
        }

        private void LoadSignals()
        {
            this.Icon = Document.AppIcon;
            _signals = new SignalInputView(_doc, _xmlSinalList);
            _signals.Dock = DockStyle.Fill;
            splitContainer1.Panel1.Controls.Add(_signals);
        }


        private XmlElement GetSignalByID(uint id)
        {
            if(_xmlSinalList == null)
                return null;

            foreach(XmlElement xmlSignal in _xmlSinalList.ChildNodes)
            {
                if( XmlHelper.GetObjectID(xmlSignal) == id)
                    return xmlSignal;
            }

            return null;
        }

        private void LoadMapping()
        {
        
            foreach (XmlElement xmlElement in _xmlPS.ChildNodes)
            {
                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");
                
                if (name != "signalTypeMap")
                    continue;

                string[] array = xmlElement.InnerText.Split(new char[]{'/'});
                uint signalID = Convert.ToUInt32(array[0]);
                XmlElement xmlSignal = GetSignalByID(signalID);

                if( xmlSignal == null)
                    continue;

                DataGridViewRow row = channels.Rows[Convert.ToInt32(array[1])];

                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;                
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            XmlHelper.SetParam(_xmlPS, "name", "string", psName.Text);
            DialogResult = DialogResult.OK;

            //Remove the old mapping
            for (int i = 0; i < _xmlPS.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement) _xmlPS.ChildNodes[i];

                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");

                if (name != "signalTypeMap")
                    continue;

                _xmlPS.RemoveChild(xmlElement);
                --i;
            }

            //Create the new mapping            
            foreach (DataGridViewRow row in channels.Rows)
            {                
                XmlElement xmlSignal = (XmlElement)row.Tag;
                
                if(xmlSignal == null)
                    continue;

                string mapping = XmlHelper.GetObjectID(xmlSignal).ToString() + "/" + row.Index.ToString();
                XmlHelper.CreateElement(_xmlPS, "string", "signalTypeMap", mapping);
            }
            
            _doc.Modified = true;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 890);
        }

        private void CANViewDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }

        private void OnDragOverChannels(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
            {
                ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

                if (item.Tag is XmlElement)
                    e.Effect = DragDropEffects.Move;
            }
        }

        private void OnDragDropChannels(object sender, DragEventArgs e)
        {
            ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");
            Point p = channels.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo info = channels.HitTest(p.X, p.Y);
            if (info.RowIndex != -1)
            {
                DataGridViewRow row = channels.Rows[info.RowIndex];
                XmlElement xmlSignal = (XmlElement)item.Tag;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;
            }
        }

        private void OnRemoveSignalFromChannel(object sender, EventArgs e)
        {
            if (channels.SelectedCells.Count == 0)
                return;

            DataGridViewRow row = channels.Rows[channels.SelectedCells[0].RowIndex];
            row.Tag = null;
            row.Cells[0].Value = "";
        }
    }
}
