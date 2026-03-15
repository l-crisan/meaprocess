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
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    public partial class SignalDelayPSDlg : Form
    {
        private SignalInputView _signals;
        private XmlElement _xmlPS;
        private Document _doc;
        private XmlElement _xmlInSignalList;
        private XmlElement _xmlOutSignalList;


        public SignalDelayPSDlg(XmlElement xmlPS, Document doc, XmlElement xmlInSignalList, XmlElement xmlOutSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlInSignalList = xmlInSignalList;
            _xmlOutSignalList = xmlOutSignalList;
            _signals = new SignalInputView(doc, xmlInSignalList);
            _signals.Dock = DockStyle.Fill;
            InitializeComponent();
            this.Icon = Document.AppIcon;
            splitContainer.Panel1.Controls.Add(_signals);
            LoadData();
            FormStateHandler.Restore(this, "Mp.Calculation.SignalDelayDlg");
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

            int index = sigDelayGrid.Rows.Add();
            DataGridViewRow row = sigDelayGrid.Rows[index];
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
            row.Cells[1].Value = XmlHelper.GetParam(xmlSignal, "name") + "_Delay";
            row.Cells[2].Value = 10.0;
            row.Cells[3].Value = XmlHelper.GetParam(xmlSignal, "comment"); ;
            row.Tag = xmlSignal;
        }

        private void LoadData()
        {
            name.Text = XmlHelper.GetParam(_xmlPS, "name");
            
            foreach (XmlElement xmlDelaySignal in _xmlOutSignalList.ChildNodes)
            {
                uint sigId = (uint)XmlHelper.GetParamNumber(xmlDelaySignal, "signal");
                XmlElement xmlSignal = _doc.GetXmlObjectById(sigId);
                
                if( xmlSignal == null)
                    continue;

                int index = sigDelayGrid.Rows.Add();
                DataGridViewRow row = sigDelayGrid.Rows[index];

                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[1].Value = XmlHelper.GetParam(xmlDelaySignal, "name");
                row.Cells[2].Value = XmlHelper.GetParamDouble(xmlDelaySignal,"delay");
                row.Cells[3].Value = XmlHelper.GetParam(xmlDelaySignal, "comment");
                row.Tag = xmlSignal;
                row.Cells[0].Tag = xmlDelaySignal;
            }
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sigDelayGrid.SelectedCells.Count == 0)
                return;

            int index = sigDelayGrid.SelectedCells[0].RowIndex;
            DataGridViewRow row = sigDelayGrid.Rows[index];
            sigDelayGrid.Rows.RemoveAt(index);
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (sigDelayGrid.Rows.Count == 0)
            {
                errorProvider.SetError(sigDelayGrid, StringResource.NoScalingDefinedErr);
                return;
            }
            
            _doc.UpdateSource(_xmlPS);

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            foreach (DataGridViewRow row in sigDelayGrid.Rows)
            {
                XmlElement xmlToDelaySignal = (XmlElement)row.Tag;
                uint sigId = XmlHelper.GetObjectID(xmlToDelaySignal);
                
                XmlElement xmlDelaySignal = (XmlElement) row.Cells[0].Tag;

                if (xmlDelaySignal== null)
                {
                    xmlDelaySignal = _doc.CreateXmlObject(_xmlOutSignalList, "Mp.Sig", "Mp.Calculation.Sig.Delay");
                    row.Cells[0].Tag = xmlDelaySignal;
                }

                XmlHelper.SetParam(xmlDelaySignal, "name", "string", row.Cells[1].Value.ToString());
                XmlHelper.SetParam(xmlDelaySignal, "comment", "string", row.Cells[3].Value.ToString());
                XmlHelper.SetParamDouble(xmlDelaySignal, "delay", "double", Convert.ToDouble(row.Cells[2].Value));
                uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
                XmlHelper.SetParamNumber(xmlDelaySignal, "sourceNumber", "uint32_t", (long)srcID);
                sigId = XmlHelper.GetObjectID(xmlToDelaySignal);
                XmlHelper.SetParamNumber(xmlDelaySignal, "signal", "uint32_t", sigId);

                XmlElement xmlHasScaling = XmlHelper.GetChildByType(xmlToDelaySignal, "Mp.Scaling");
                XmlElement xmlScaling = XmlHelper.GetChildByType(xmlDelaySignal, "Mp.Scaling");

                if (xmlHasScaling != null)
                {
                    if (xmlScaling == null)
                        xmlScaling = _doc.CreateXmlObject(xmlDelaySignal, "Mp.Scaling", "Mp.Scaling.FactorOffset");

                    XmlHelper.SetParam(xmlScaling, "factor", "double", XmlHelper.GetParam(xmlHasScaling, "factor"));
                    XmlHelper.SetParam(xmlScaling, "offset", "double", XmlHelper.GetParam(xmlHasScaling, "offset"));
                }
                else
                {
                    if( xmlScaling  != null)
                        _doc.RemoveXmlObject(xmlScaling);
                }

                
                _doc.CopySignalBaseParam(xmlToDelaySignal, xmlDelaySignal);
            }
             
            RemoveUnusedSignals();

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }


        private void RemoveUnusedSignals()
        {
            for(int i = 0; i < _xmlOutSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement) _xmlOutSignalList.ChildNodes[i];
                uint id = XmlHelper.GetObjectID(xmlSignal);
                bool found = false;
                foreach (DataGridViewRow row in sigDelayGrid.Rows)
                {
                    XmlElement delaySignal = (XmlElement)row.Cells[0].Tag;
                    if (delaySignal == xmlSignal)
                    {
                        found = true;
                        break;
                    }
                }

                if(!found)
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                }
            }
        }

        private void ScalingPSDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, "Mp.Calculation.SignalDelayDlg");
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1400);
        }

        private void sigDelayGrid_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if (e.ColumnIndex != 2)
                return;

            try
            {
                double value = Convert.ToDouble(e.FormattedValue);

                if (value < 0 || value > 10000)
                {
                    e.Cancel = true;
                    errorProvider.SetError(sigDelayGrid, StringResource.Max10SecDelay);
                }
            }
            catch(Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(sigDelayGrid, ex.Message);
            }

        }
    }
}
