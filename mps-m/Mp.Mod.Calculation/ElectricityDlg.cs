//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2010-2016  Laurentiu-Gheorghe Crisan
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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    public partial class ElectricityDlg : Form
    {
        private SignalInputView _signals;
        private XmlElement _xmlPS;
        private Document _doc;
        private XmlElement _xmlInSignalList;

        public ElectricityDlg(XmlElement xmlPS, Document doc, XmlElement xmlInSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlInSignalList = xmlInSignalList;
            _signals = new SignalInputView(doc, xmlInSignalList);
            _signals.TabIndex = 2;
            _signals.Dock = DockStyle.Fill;
            
            InitializeComponent();

            this.Icon = Document.AppIcon;
            
            int index = channels.Rows.Add();
            DataGridViewRow voltageRow = channels.Rows[index];
            voltageRow.Cells[1].Value = StringResource.Voltage;
            
            index = channels.Rows.Add();
            DataGridViewRow currentRow = channels.Rows[index];
            currentRow.Cells[1].Value = StringResource.Current;

            splitContainer1.Panel1.Controls.Add(_signals);
            name.Text = XmlHelper.GetParam(_xmlPS, "name");

            XmlElement xmlSignal;
            uint id = (uint)XmlHelper.GetParamNumber(_xmlPS, "voltageSignal");
            if (id != 0)
            {
                xmlSignal = _doc.GetXmlObjectById(id);
                voltageRow.Tag = xmlSignal;
                voltageRow.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            }

            id = (uint)XmlHelper.GetParamNumber(_xmlPS, "currentSignal");
            if (id != 0)
            {
                xmlSignal = _doc.GetXmlObjectById(id);
                currentRow.Tag = xmlSignal;
                currentRow.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            }

            FormStateHandler.Restore(this, "Mp.Calculation.ElectricityDlg");
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            DataGridViewRow voltageRow = channels.Rows[0];
            DataGridViewRow currentRow = channels.Rows[1];

            if (voltageRow.Tag == null && currentRow.Tag == null)
            {
                errorProvider.SetError(channels, StringResource.SigChnMapErr);
                return;
            }

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            XmlElement xmlSignal = (XmlElement)voltageRow.Tag;
            if (xmlSignal != null)
            {
                uint id = XmlHelper.GetObjectID(xmlSignal);
                XmlHelper.SetParamNumber(_xmlPS, "voltageSignal", "uint32_t", id);
            }
            else
            {
                XmlHelper.SetParamNumber(_xmlPS, "voltageSignal", "uint32_t", 0);
            }

            xmlSignal = (XmlElement)currentRow.Tag;

            if (xmlSignal != null)
            {
                uint id = XmlHelper.GetObjectID(xmlSignal);
                XmlHelper.SetParamNumber(_xmlPS, "currentSignal", "uint32_t", id);
            }
            else
            {
                XmlHelper.SetParamNumber(_xmlPS, "currentSignal", "uint32_t", 0);
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            _doc.Modified = true;
            Close();
        }

        private void channels_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
            {
                ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

                if (item.Tag is XmlElement)
                    e.Effect = DragDropEffects.Move;
            }
        }

        private void channels_DragDrop(object sender, DragEventArgs e)
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

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (channels.SelectedCells.Count == 0)
                return;

            DataGridViewRow row = channels.Rows[channels.SelectedCells[0].RowIndex];
            row.Tag = null;
            row.Cells[0].Value = "";
        }

        private void ElectricityDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, "Mp.Calculation.ElectricityDlg");
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1430);
        }
    }
}
