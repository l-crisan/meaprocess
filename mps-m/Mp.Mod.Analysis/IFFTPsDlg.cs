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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Analysis
{
    public partial class IFFTPsDlg : Form
    {
        private SignalInputView _signalView;
        private Document _doc;
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;

        public IFFTPsDlg(Document doc, XmlElement xmlSignalList, XmlElement xmlPS)
        {
            _doc = doc;
            _xmlSignalList = xmlSignalList;
            _xmlPS = xmlPS;
            _signalView = new SignalInputView(_doc, _xmlSignalList);
            
            InitializeComponent();
            this.Icon = Document.AppIcon;
            _signalView.Dock = DockStyle.Fill;

            splitContainer1.Panel1.Controls.Add(_signalView);
            name.Text = XmlHelper.GetParam(_xmlPS, "name");
            
            InitChannels();
            channelType.SelectedIndex = (int)XmlHelper.GetParamNumber(_xmlPS, "channelType");

            DataGridViewRow rrow = channels.Rows[0];
            DataGridViewRow irow = channels.Rows[1];

            XmlElement xmlSignal;
            uint id = (uint) XmlHelper.GetParamNumber(_xmlPS, "chn1");
            if (id != 0)
            {
                xmlSignal = _doc.GetXmlObjectById(id);
                irow.Tag = xmlSignal;
                irow.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            }

            id = (uint)XmlHelper.GetParamNumber(_xmlPS, "chn2");
            if (id != 0)
            {
                xmlSignal = _doc.GetXmlObjectById(id);
                rrow.Tag = xmlSignal;
                rrow.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            }

            uint count = (uint) XmlHelper.GetParamNumber(_xmlPS, "nifft");

            if (count != 0)
                nifft.Text = count.ToString();
            else
                nifft.Text = "1000";

        }

        private void InitChannels()
        {
            int index = channels.Rows.Add();
            DataGridViewRow row = channels.Rows[index];
            row.Cells[1].Value = StringResource.ComplexRealPart;
            
            index = channels.Rows.Add();
            row = channels.Rows[index];
            row.Cells[1].Value = StringResource.ComplexImagPart;
        }

        private void OK_Click(object sender, EventArgs e)
        {

            errorProvider.Clear();

            DataGridViewRow rrow = channels.Rows[0];
            DataGridViewRow irow = channels.Rows[1];

            if (rrow.Tag == null || irow.Tag == null)
            {
                errorProvider.SetError(channels, StringResource.SigChnMapErr);
                return;
            }

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);
            XmlHelper.SetParamNumber(_xmlPS, "nifft", "uint32_t", Convert.ToUInt32(nifft.Text));
            
            XmlHelper.SetParamNumber(_xmlPS, "channelType", "uint8_t", channelType.SelectedIndex);

            XmlElement xmlSignal = (XmlElement)irow.Tag;
            uint id = XmlHelper.GetObjectID(xmlSignal);

            XmlHelper.SetParamNumber(_xmlPS, "chn1", "uint32_t", id);

            xmlSignal = (XmlElement)rrow.Tag;
            id = XmlHelper.GetObjectID(xmlSignal);

            XmlHelper.SetParamNumber(_xmlPS, "chn2", "uint32_t", id);

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                if (Convert.ToUInt32(nifft.Text) % 2 != 0)
                {
                    errorProvider.SetError(nifft, StringResource.EvenPointsErr);
                    e.Cancel = true;
                }

            }
            catch (Exception ex)
            {
                errorProvider.SetError(nifft, ex.Message);
            }
        }

        private void channelType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (channelType.SelectedIndex)
            {
                case 0:
                {
                    DataGridViewRow row = channels.Rows[0];
                    row.Cells[1].Value = StringResource.ComplexRealPart;
                    
                    row = channels.Rows[1];
                    row.Cells[1].Value = StringResource.ComplexImagPart;
                }
                break;
                case 1:
                {
                    DataGridViewRow row = channels.Rows[0];
                    row.Cells[1].Value = StringResource.PhaseAngle;

                    row = channels.Rows[1];
                    row.Cells[1].Value = StringResource.Magnitude;
                }
                break;
                case 2:
                {
                    DataGridViewRow row = channels.Rows[0];
                    row.Cells[1].Value = StringResource.PhaseAngle;

                    row = channels.Rows[1];
                    row.Cells[1].Value = StringResource.PSD;
                }
                break;

            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 980);
        }

        private void IFFTPsDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
