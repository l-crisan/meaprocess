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
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    public partial class BitExtractionDlg : Form
    {
        private SignalInputView _signals;
        private XmlElement _xmlPS;
        private Document _doc;
        private XmlElement _xmlInSignalList;
        private XmlElement _xmlOutSignalList;

        public class OutputItem
        {
            public OutputItem()
            {
            }

            public string Name;
            public string Unit;
            public string Comment;
            public int BitNumber;
            public XmlElement xmlOutSignal;
        }


        public BitExtractionDlg(XmlElement xmlPS, Document doc, XmlElement xmlInSignalList, XmlElement xmlOutSignalList)
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
            FormStateHandler.Restore(this, "Mp.Calculation.BitExtractionDlg");
            name.Text = XmlHelper.GetParam(_xmlPS, "name");
        }

        private void LoadData()
        {
            Hashtable outputSignals = new Hashtable();

            foreach (XmlElement xmlOutSignal in _xmlOutSignalList.ChildNodes)
            {
                uint inSigId = (uint)XmlHelper.GetParamNumber(xmlOutSignal, "inSignal");
                XmlElement xmlInSignal = _doc.GetXmlObjectById(inSigId);

                List<OutputItem> items;

                if (!outputSignals.ContainsKey(inSigId))
                {
                    items = new List<OutputItem>();
                    outputSignals[inSigId] = items;
                    int index = outputGrid.Rows.Add();
                    DataGridViewRow row = outputGrid.Rows[index];
                    row.Tag = xmlInSignal;
                    row.Cells[0].Value = XmlHelper.GetParam(xmlInSignal,"name");
                    row.Cells[0].Tag = items;
                    row.Cells[1].Value = "...";
                }
                else
                {
                    items = (List<OutputItem>)outputSignals[inSigId];
                }

                OutputItem item = new OutputItem();
                item.Name = XmlHelper.GetParam(xmlOutSignal, "name");
                item.xmlOutSignal = xmlOutSignal;
                item.BitNumber = (int) XmlHelper.GetParamNumber(xmlOutSignal, "bit");
                item.Unit = XmlHelper.GetParam(xmlOutSignal, "unit");
                item.Comment = XmlHelper.GetParam(xmlOutSignal, "comment");
                items.Add(item);
            }
        }

        private void RemoveUnusedSignals()
        {
            for (int i = 0; i < _xmlOutSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement)_xmlOutSignalList.ChildNodes[i];
                uint id = XmlHelper.GetObjectID(xmlSignal);
                bool found = false;
                foreach (DataGridViewRow row in outputGrid.Rows)
                {
                    List<OutputItem> items = (List<OutputItem>)row.Cells[0].Tag;
                    foreach (OutputItem item in items)
                    {
                        if (item.xmlOutSignal == xmlSignal)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                }
            }
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

            foreach (DataGridViewRow inRow in outputGrid.Rows)
            {
                if (inRow.Tag == xmlSignal)
                    return;
            }

            int index = outputGrid.Rows.Add();
            DataGridViewRow row = outputGrid.Rows[index];
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");            
            row.Cells[1].Value = "...";
            List<OutputItem> items = new List<OutputItem>();
            row.Cells[0].Tag = items;
            row.Tag = xmlSignal;
            
        }

        private void scalingGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 1)
            {
                DataGridViewRow row = outputGrid.Rows[e.RowIndex];
                XmlElement xmlInSignal = (XmlElement) row.Tag;
                List<OutputItem> items = (List<OutputItem>) row.Cells[0].Tag;
                
                BitSelectDialog dlg = new BitSelectDialog(xmlInSignal, items);
                dlg.ShowDialog();
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            _doc.UpdateSource(_xmlPS);

            foreach (DataGridViewRow row in outputGrid.Rows)
            {
                XmlElement xmlInSignal = (XmlElement)row.Tag;
                List<OutputItem> items = (List<OutputItem>)row.Cells[0].Tag;

                foreach (OutputItem item in items)
                {
                    XmlElement xmlOutSignal = item.xmlOutSignal;

                    if (xmlOutSignal == null)
                        xmlOutSignal = _doc.CreateXmlObject(_xmlOutSignalList, "Mp.Sig", "Mp.Calculation.Sig.BitEx");

                    item.xmlOutSignal = xmlOutSignal;

                    XmlHelper.SetParam(xmlOutSignal, "name", "string", item.Name);
                    XmlHelper.SetParam(xmlOutSignal, "unit", "string", item.Unit);
                    XmlHelper.SetParam(xmlOutSignal, "comment", "string", item.Comment);

                    XmlHelper.SetParamNumber(xmlOutSignal, "valueDataType", "uint8_t", (int)SignalDataType.BOOL);
                    XmlHelper.SetParamDouble(xmlOutSignal, "physMin", "double", 0);
                    XmlHelper.SetParamDouble(xmlOutSignal, "physMax", "double", 1);

                    uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
                    XmlHelper.SetParamNumber(xmlOutSignal, "sourceNumber", "uint32_t", (long)srcID);
                    uint sigId = XmlHelper.GetObjectID(xmlInSignal);
                    XmlHelper.SetParamNumber(xmlOutSignal, "inSignal", "uint32_t", sigId);
                    XmlHelper.SetParamNumber(xmlOutSignal, "bit", "uint8_t", item.BitNumber);
                        
                }
            }

            XmlHelper.SetParam(_xmlPS, "name", "string", name.Text);

            RemoveUnusedSignals();
            _doc.Modified = true;
            DialogResult = System.Windows.Forms.DialogResult.OK;
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

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1470);
        }
    }
}
