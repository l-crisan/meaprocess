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
    public partial class DemultiplexerPortDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlPS;
        private XmlElement _xmlSignalList;

        public DemultiplexerPortDlg(Document doc, XmlElement xmlPS, XmlElement xmlSignalList)
        {
            _xmlPS = xmlPS;
            _doc = doc;
            _xmlSignalList = xmlSignalList;

            InitializeComponent();
            this.Icon = Document.AppIcon;
            LoadData();
        }

        private void LoadData()
        {
            foreach (XmlElement xmlSignal in _xmlSignalList.ChildNodes)
            {

                int index = channels.Rows.Add();
                DataGridViewRow row = channels.Rows[index];
                row.Tag = xmlSignal;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[1].Value = XmlHelper.GetParamDouble(xmlSignal, "noValue");
                row.Cells[2].Value = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                row.Cells[3].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                row.Cells[4].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                row.Cells[5].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[6].Value = XmlHelper.GetParam(xmlSignal, "comment");
            }

            DataGridViewRow lastRow = channels.Rows[channels.Rows.Count - 1];
            lastRow.Cells[1].Value = 0;
            lastRow.Cells[2].Value = 100;
            lastRow.Cells[3].Value = -10;
            lastRow.Cells[4].Value = 10;
        }



        private void RemoveUnusedSignal()
        {
            for(int i = 0; i < _xmlSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement) _xmlSignalList.ChildNodes[i];
                bool found = false;
                foreach (DataGridViewRow row in channels.Rows)
                {
                    if (row.Tag == xmlSignal)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                }
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in channels.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string sigName = row.Cells[0].Value.ToString();

                if( sigName == null || sigName == "")
                {
                    errorProvider.SetError(channels, StringResource.SigNameErr);
                    return;
                }
            }

            _doc.UpdateSource(_xmlPS);

            foreach (DataGridViewRow row in channels.Rows)
            {
                if (row.IsNewRow)
                    continue;

                XmlElement xmlSignal = (XmlElement) row.Tag;

                if (xmlSignal == null)
                {
                    xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Calculation.Sig.Demux");
                    row.Tag = xmlSignal;
                }

                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[0].Value);
                XmlHelper.SetParamDouble(xmlSignal, "noValue", "double", Convert.ToDouble(row.Cells[1].Value));
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", Convert.ToDouble(row.Cells[2].Value));
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(row.Cells[3].Value));
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(row.Cells[4].Value));

                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[5].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[6].Value);
                
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.LREAL);
                uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", (long)srcID);
            }

            RemoveUnusedSignal();
            _doc.Modified = true;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void channels_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = channels.Rows[e.RowIndex];
            row.Cells[1].Value = 0;
            row.Cells[2].Value = 100;
            row.Cells[3].Value = -10;
            row.Cells[4].Value = 10;
        }

        private void channels_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if (e.ColumnIndex == 2)
            {
                try
                {
                    double rate = Convert.ToDouble(e.FormattedValue);

                    if (rate == 0)
                    {
                        e.Cancel = false;
                        errorProvider.SetError(channels, StringResource.ZeroValueErr);
                    }
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    errorProvider.SetError(channels, ex.Message);
                }
            }

            if (e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                try
                {
                    Convert.ToDouble(e.FormattedValue);
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    errorProvider.SetError(channels, ex.Message);
                }
            }
        }

        private void channels_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            DataGridViewRow row = channels.Rows[e.RowIndex];

            double min = Convert.ToDouble(row.Cells[3].Value);
            double max = Convert.ToDouble(row.Cells[4].Value);

            errorProvider.Clear();

            if (min >= max)
            {
                errorProvider.SetError(channels, StringResource.MinMaxErr);
                e.Cancel = true;
            }
        }

        private void remove_Click(object sender, EventArgs e)
        {
            if (channels.SelectedCells.Count == 1)
            {
                if (channels.SelectedCells[0].RowIndex == -1)
                    return;

                DataGridViewRow row = channels.Rows[channels.SelectedCells[0].RowIndex];

                if (row.IsNewRow)
                    return;
            }


            foreach (DataGridViewCell cell in channels.SelectedCells)
            {
                if (cell.RowIndex == -1)
                    continue;

                DataGridViewRow row = channels.Rows[cell.RowIndex];

                if (row.IsNewRow)
                    continue;

                channels.Rows.RemoveAt(row.Index);
            }
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1460);
        }
    }
}
