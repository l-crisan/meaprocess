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
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Calculation
{
    public partial class BitGroupingDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPS;
        private XmlElement _xmlInSignalList;
        private XmlElement _xmlOutSignalList;
        private SignalInputView _signals;

        public BitGroupingDlg(Document doc, XmlElement xmlPS, XmlElement xmlInSigList, XmlElement xmlOutSigList )
        {
            _doc = doc;
            _xmlPS = xmlPS;
            _xmlInSignalList = xmlInSigList;
            _xmlOutSignalList = xmlOutSigList;
            InitializeComponent();
            this.Icon = Document.AppIcon;
            _signals = new SignalInputView(doc, _xmlInSignalList);
            _signals.Dock = DockStyle.Fill;
            _signals.TabIndex = 9;
            splitContainer.Panel1.Controls.Add(_signals);
            LoadData();
            Utils.FormStateHandler.Restore(this, "Mp.Calculation.BitGroupingDlg");
            name.Text = XmlHelper.GetParam(_xmlPS, "name");
        }

        private void LoadData()
        {
            int size = 0;
            if (_xmlOutSignalList.ChildNodes.Count != 0)
            {
                XmlElement xmlOutSignal = (XmlElement)_xmlOutSignalList.ChildNodes[0];
                sigName.Text = XmlHelper.GetParam(xmlOutSignal, "name");
                sampleRate.Text = XmlHelper.GetParamDouble(xmlOutSignal, "samplerate").ToString();
                dataType.SelectedIndex = GetDataTypeIndex((int)XmlHelper.GetParamNumber(xmlOutSignal, "valueDataType"), out size);
                unit.Text = XmlHelper.GetParam(xmlOutSignal, "unit");
                min.Text = XmlHelper.GetParamDouble(xmlOutSignal, "physMin").ToString();
                max.Text = XmlHelper.GetParamDouble(xmlOutSignal, "physMax").ToString();
                comment.Text = XmlHelper.GetParam(xmlOutSignal, "comment");

                string sigMapping  = XmlHelper.GetParam(xmlOutSignal, "sigMapping");

                string[] array = sigMapping.Split(';');

                foreach (string str in array)
                {
                    if (str == "")
                        continue;

                    string[] id2Idx = str.Split(',');
                    uint id = Convert.ToUInt32(id2Idx[0]);
                    int index = Convert.ToInt32(id2Idx[1]);
                    DataGridViewRow row = outputGrid.Rows[index];
                    XmlElement xmlInSignal = _doc.GetXmlObjectById(id);
                    row.Tag = xmlInSignal;
                    row.Cells[0].Value = XmlHelper.GetParam(xmlInSignal, "name");
                }
            }
            else
            {
                size = 8;
                sigName.Text = "";
                sampleRate.Text = "100";
                dataType.SelectedIndex = 0;
                unit.Text = "";
                min.Text = "0";
                max.Text = "255";
                comment.Text = "";
            }
            
        }

        private int GetDataTypeIndex(int type, out int size)
        {
            switch ((SignalDataType) type)
            {
                case SignalDataType.USINT:
                {
                    size = 8;
                    return 0;
                }

                case SignalDataType.SINT:
                {
                    size = 8;
                    return 1;
                }

                case SignalDataType.UINT:
                {
                    size = 16;
                    return 2;
                }

                case SignalDataType.INT:
                {
                    size = 16;
                    return 3;
                }

                case SignalDataType.UDINT:
                {
                    size = 32;
                    return 4;
                }

                case SignalDataType.DINT:
                {
                    size = 32;
                    return 5;
                }

                case SignalDataType.ULINT:
                {
                    size = 64;
                    return 6;
                }

                case SignalDataType.LINT:
                {
                    size = 64;
                    return 7;
                }
            }

            size = 8;
            return 0;
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
            Point p = outputGrid.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo info = outputGrid.HitTest(p.X, p.Y);
            if (info.RowIndex != -1)
            {
                DataGridViewRow row = outputGrid.Rows[info.RowIndex];
                XmlElement xmlSignal = (XmlElement)item.Tag;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;
            }
        }

        private void removeSignalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (outputGrid.SelectedCells.Count == 0)
                return;

            DataGridViewRow row = outputGrid.Rows[outputGrid.SelectedCells[0].RowIndex];
            row.Tag = null;
            row.Cells[0].Value = "";

        }

        private int GetDataType(int dataTypeIndex)
        {
            switch (dataTypeIndex)
            {
                case 0:
                    return (int)SignalDataType.USINT;

                case 1:
                    return (int)SignalDataType.SINT;

                case 2:
                    return (int)SignalDataType.UINT;

                case 3:
                    return (int)SignalDataType.INT;

                case 4:
                    return (int)SignalDataType.UDINT;

                case 5:
                    return (int)SignalDataType.DINT;

                case 6:
                    return (int)SignalDataType.ULINT;

                case 7:
                    return (int)SignalDataType.LINT;
            }

            return 0;

        }
        private int GetDataTypeSize(int dataTypeIndex)
        {
            switch (dataTypeIndex)
            {
                case 0:
                case 1:
                    return 8;

                case 2:
                case 3:
                    return 16;

                case 4:
                case 5:
                    return 32;

                case 6:
                case 7:
                    return 64;
            }

            return 8;
        }
        private void dataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int size = GetDataTypeSize(dataType.SelectedIndex);
            
            if (outputGrid.Rows.Count == size)
                return;

            if (outputGrid.Rows.Count < size)
            {
                int count = outputGrid.Rows.Count;
                for (int i = count; i < size; ++i)
                {
                    int index = outputGrid.Rows.Add();
                    DataGridViewRow row = outputGrid.Rows[index];
                    row.Cells[1].Value = i + 1;
                }
            }
            else if (size < outputGrid.Rows.Count)
            {
                int count = outputGrid.Rows.Count;

                for (int i = size; i < count; ++i)
                    outputGrid.Rows.RemoveAt(size);                    
            }
        }

        private void value_Validating(object sender, CancelEventArgs e)
        {
            Control ctrl = (Control)sender;
            errorProvider.Clear();

            try
            {
                Convert.ToDouble(ctrl.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ctrl, ex.Message);
                e.Cancel = true;
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();
            if (sigName.Text == null || sigName.Text == "")
            {
                errorProvider.SetError(sigName, StringResource.SigNameErr);
                return;
            }

            double minValue = Convert.ToDouble(min.Text);
            double maxValue = Convert.ToDouble(max.Text);

            if (minValue >= maxValue)
            {
                errorProvider.SetError(max, StringResource.MinMaxErr);
                return;
            }

            _doc.UpdateSource(_xmlPS);

            XmlElement xmlOutSignal = null;

            if (_xmlOutSignalList.ChildNodes.Count == 0)
                xmlOutSignal = _doc.CreateXmlObject(_xmlOutSignalList, "Mp.Sig", "Mp.Calculation.Sig.BitGrp");
            else
                xmlOutSignal = (XmlElement)_xmlOutSignalList.ChildNodes[0];

            XmlHelper.SetParam(xmlOutSignal, "name","string", sigName.Text);
            XmlHelper.SetParam(xmlOutSignal, "unit", "string", unit.Text);
            XmlHelper.SetParam(xmlOutSignal, "comment", "string", comment.Text);
            XmlHelper.SetParamDouble(xmlOutSignal, "physMin", "double", minValue);
            XmlHelper.SetParamDouble(xmlOutSignal, "physMax", "double", maxValue);
            XmlHelper.SetParamNumber(xmlOutSignal, "valueDataType", "uint8_t", GetDataType(dataType.SelectedIndex));
            XmlHelper.SetParamDouble(xmlOutSignal, "samplerate", "double", Convert.ToDouble(sampleRate.Text));
            uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
            XmlHelper.SetParamNumber(xmlOutSignal, "sourceNumber", "uint32_t", (long)srcID);

            StringBuilder sb = new StringBuilder();

            foreach (DataGridViewRow row in outputGrid.Rows)
            {
                if (row.Tag == null)
                    continue;

                XmlElement xmlInSignal = (XmlElement)row.Tag;
                uint inObjId = XmlHelper.GetObjectID(xmlInSignal);
                sb.Append(inObjId.ToString());
                sb.Append(",");
                sb.Append(row.Index.ToString());
                sb.Append(";");
            }

            string sigMapping = sb.ToString().TrimEnd(';');
            XmlHelper.SetParam(xmlOutSignal, "sigMapping", "string", sigMapping);

            DialogResult = System.Windows.Forms.DialogResult.OK;
            _doc.Modified = true;
            Close();
        }

        private void sampleRate_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();

            try
            {
                double sr = Convert.ToDouble(sampleRate.Text);

                if (sr == 0)
                {
                    errorProvider.SetError(rateUnit, StringResource.ZeroValueErr);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(rateUnit, ex.Message);
                e.Cancel = true;
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1480);
        }
    }
}
