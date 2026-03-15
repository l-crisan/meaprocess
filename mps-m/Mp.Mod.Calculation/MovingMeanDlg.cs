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
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    public partial class MovingMeanDlg : Form
    {
        private SignalInputView _signals;
        private XmlElement _xmlPS;
        private Document _doc;
        private XmlElement _xmlInSignalList;
        private XmlElement _xmlOutSignalList;

        public MovingMeanDlg(XmlElement xmlPS, Document doc, XmlElement xmlInSignalList, XmlElement xmlOutSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlInSignalList = xmlInSignalList;
            _xmlOutSignalList = xmlOutSignalList;
            _signals = new SignalInputView(doc, xmlInSignalList);
            _signals.TabIndex = 3;
            _signals.Dock = DockStyle.Fill;
            
            InitializeComponent();
            this.Icon = Document.AppIcon;
            splitContainer1.Panel1.Controls.Add(_signals);

            DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn) outputGrid.Columns[2];
            col.Items.Clear();
            col.Items.Add(StringResource.Simple);
            col.Items.Add(StringResource.Weighted);

            LoadData();
            
            psName.Text = XmlHelper.GetParam(_xmlPS, "name");
            samples.Text = XmlHelper.GetParamNumber(_xmlPS, "samples").ToString();

            FormStateHandler.Restore(this, "Mp.Calculation.MovingMeanDlg");
        }

        private void LoadData()
        {
            foreach (XmlElement xmlOutSignal in _xmlOutSignalList.ChildNodes)
            {
                int index = outputGrid.Rows.Add();
                DataGridViewRow row = outputGrid.Rows[index];
                row.Cells[0].Tag = xmlOutSignal;
                uint inSignal = (uint) XmlHelper.GetParamNumber(xmlOutSignal, "inSignal");
                XmlElement xmlInSignal = _doc.GetXmlObjectById(inSignal);
                row.Tag = xmlInSignal;
                row.Cells[0].Value = XmlHelper.GetParam(xmlInSignal, "name");
                row.Cells[1].Value = XmlHelper.GetParam(xmlOutSignal, "name");
                row.Cells[2].Value = GetMeanType((int)XmlHelper.GetParamNumber(xmlOutSignal, "meanType"));
                row.Cells[3].Value = XmlHelper.GetParam(xmlOutSignal, "unit");
                row.Cells[4].Value = XmlHelper.GetParam(xmlOutSignal, "comment");
            }
        }


        private string GetMeanType(int type)
        {
            switch (type)
            {
                case 0:
                    return StringResource.Simple;
                case 1:
                    return StringResource.Weighted;
            }

            return StringResource.Simple;
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
            if (!e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
                return;

            System.Windows.Forms.ListViewItem item = (System.Windows.Forms.ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

            XmlElement xmlSignal = item.Tag as XmlElement;

            if (xmlSignal == null)
                return;

            int index = outputGrid.Rows.Add();
            DataGridViewRow row = outputGrid.Rows[index];
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            row.Cells[1].Value = "";
            row.Cells[2].Value = StringResource.Simple;
            row.Cells[3].Value = "";
            row.Cells[4].Value = "";
            row.Tag = xmlSignal;
        }

        private void smaples_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !ValidateSamples();
        }

        private bool ValidateSamples()
        {
            errorProvider.Clear();
            try
            {
                uint value = Convert.ToUInt32(samples.Text);

                if (value == 0)
                {
                    errorProvider.SetError(samples, StringResource.ZeroValueErr);
                    return false;
                }
            }
            catch (Exception ex)
            {                
                errorProvider.SetError(samples, ex.Message);
                return false;
            }

            return true;
        }

        private void MovingMeanDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, "Mp.Calculation.MovingMeanDlg");
        }

        private int GetMeanType(string type)
        {
            if (type == StringResource.Simple)
                return 0;

            return 1;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            if (!ValidateSamples())
                return;

            errorProvider.Clear();

            foreach (DataGridViewRow row in outputGrid.Rows)
            {
                string outSigName = row.Cells[1].Value.ToString();
                if (outSigName == "")
                {
                    string message = String.Format(StringResource.MeanOutSigNameErr, row.Cells[0].Value.ToString());
                    errorProvider.SetError(outputGrid, message);
                    return;
                }
            }

            _doc.UpdateSource(_xmlPS);

            XmlHelper.SetParam(_xmlPS, "name", "string", psName.Text);
            XmlHelper.SetParamNumber(_xmlPS, "samples", "uint32_t", Convert.ToUInt32(samples.Text));

            foreach (DataGridViewRow row in outputGrid.Rows)
            {
                XmlElement inSignal = (XmlElement) row.Tag;
                XmlElement outSignal = (XmlElement) row.Cells[0].Tag;

                if (outSignal == null)
                {
                    outSignal = _doc.CreateXmlObject(_xmlOutSignalList, "Mp.Sig", "Mp.Calculation.Sig.MovingMean");
                    row.Cells[0].Tag = outSignal;
                }

                XmlHelper.SetParam(outSignal, "name", "string", row.Cells[1].Value.ToString());
                XmlHelper.SetParamNumber(outSignal, "meanType", "uint8_t", GetMeanType(row.Cells[2].Value.ToString()));

                XmlHelper.SetParam(outSignal, "unit", "string", row.Cells[3].Value.ToString());
                XmlHelper.SetParam(outSignal, "comment", "string", row.Cells[4].Value.ToString());

                XmlHelper.SetParamNumber(outSignal, "valueDataType", "uint8_t", (int)SignalDataType.LREAL);

                uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
                XmlHelper.SetParamNumber(outSignal, "sourceNumber", "uint32_t", (long)srcID);
                XmlHelper.SetParamNumber(outSignal, "inSignal", "uint32_t", (long)XmlHelper.GetObjectID(inSignal));                
            }

            RemoveUnusedSignals();

            DialogResult = System.Windows.Forms.DialogResult.OK;
            _doc.Modified = true;
            Close();
        }


        private void RemoveUnusedSignals()
        {
            for (int i = 0; i < _xmlOutSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlOutSignal = (XmlElement)_xmlOutSignalList.ChildNodes[i];
                
                bool found = false;

                foreach (DataGridViewRow row in outputGrid.Rows)
                {
                    XmlElement xmlSignal = (XmlElement)row.Cells[0].Tag;

                    if (xmlOutSignal == xmlSignal)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _doc.RemoveXmlObject(xmlOutSignal);
                    --i;
                }
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (outputGrid.SelectedCells.Count == 0)
                return;

            int index = outputGrid.SelectedCells[0].RowIndex;
            DataGridViewRow row = outputGrid.Rows[index];
            outputGrid.Rows.RemoveAt(index);
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1410);
        }
    }
}
