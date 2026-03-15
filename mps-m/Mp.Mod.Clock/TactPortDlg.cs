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
using System.Windows.Forms;
using System.Xml;
using Mp.Utils;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Clock
{
    public partial class TactPortDlg : Form
    {
        private enum SignalCols
        {
            Name,
            GenrationRate,
            Frequency,
            Unit,
            Comment,
            DataType,
        }
        private Document _doc;
        private XmlElement _xmlSignalList;

        public TactPortDlg(Document doc, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlSignalList = xmlSignalList;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            LoadData();
            FormStateHandler.Restore(this, Document.RegistryKey + "TactPortDlg"); 
        }

        private void LoadData()
        {
            int index = 0;
            foreach (XmlElement xmlSignal in _xmlSignalList.ChildNodes)
            {
                index = signals.Rows.Add();
                DataGridViewRow row = signals.Rows[index];

                row.Cells[(int)SignalCols.Name].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[(int)SignalCols.Unit].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[(int)SignalCols.Comment].Value = XmlHelper.GetParam(xmlSignal, "comment");
                row.Cells[(int)SignalCols.DataType].Value = "BOOL";
                uint rate = (uint)XmlHelper.GetParamNumber(xmlSignal, "samplerate");
                row.Cells[(int)SignalCols.GenrationRate].Value = rate;
                row.Cells[(int)SignalCols.Frequency].Value = (rate / 2).ToString() + " Hz";
                row.Tag = xmlSignal;
                index++;
            }            
            InitNewRow(index);
        }

        private void InitNewRow(int index)
        {
            DataGridViewRow row = signals.Rows[index];
            row.Cells[(int)SignalCols.GenrationRate].Value = 100;
            row.Cells[(int)SignalCols.Frequency].Value = "50 Hz";
            row.Cells[(int)SignalCols.DataType].Value = "BOOL";
        }

        private void OK_Click(object sender, EventArgs e)
        {
            //Save the signals
            foreach (DataGridViewRow row in signals.Rows)
            {
                if (row.IsNewRow)
                    continue;

                XmlElement xmlSignal = null;

                if (row.Tag != null)
                {
                    xmlSignal = (XmlElement)row.Tag;
                }
                else
                {
                    xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Sig");
                    row.Tag = xmlSignal;
                }

                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[(int)SignalCols.Name].Value);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string) row.Cells[(int)SignalCols.Unit].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string) row.Cells[(int)SignalCols.Comment].Value);
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 1);
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.BOOL);
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", Convert.ToDouble(row.Cells[(int)SignalCols.GenrationRate].Value));
            }
            
            //Remove unused signals
            for (int i = 0; i < _xmlSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement) _xmlSignalList.ChildNodes[i];
                if (!IsSignalInGrid(xmlSignal))
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                }
            }

            _doc.Modified = true;
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool IsSignalInGrid(XmlElement xmlSignal)
        {
            foreach (DataGridViewRow row in signals.Rows)
            {
                XmlElement curSignal = (XmlElement)row.Tag;
                
                if (curSignal == xmlSignal)
                    return true;
            }
            return false;
        }

        protected XmlElement GetSignalFromRow(DataGridViewRow row)
        {
            if (row.Tag != null)
                return(XmlElement)row.Tag;

            return _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Sig");
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void signals_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == (int)SignalCols.GenrationRate)
            {
                try
                {
                    uint rate = Convert.ToUInt32(e.FormattedValue);

                    if (rate == 0)
                    {
                        e.Cancel = true;
                        errorProvider.SetError(signals, StringResource.ZeroGenRateErr);
                        return;
                    }

                    if (rate % 2 != 0)
                    {
                        errorProvider.SetError(signals, StringResource.EvenFreqErr);
                        e.Cancel = true;
                        return;
                    }

                    DataGridViewRow row = signals.Rows[e.RowIndex];
                    row.Cells[(int)SignalCols.Frequency].Value = (rate/2).ToString() + " Hz";
                }
                catch (Exception ex)
                {
                    errorProvider.SetError(signals, ex.Message);
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == (int)SignalCols.Name)
            {
                string name = (string) e.FormattedValue;

                if (name == null || name == "")
                {
                    errorProvider.SetError(signals, StringResource.SigNameErr);
                    e.Cancel = true;
                }
            }

        }

        private void remove_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in signals.SelectedRows)
                {
                    if(!row.IsNewRow)
                        signals.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void signals_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            InitNewRow(e.RowIndex);
        }

        private void TactPortDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, Document.RegistryKey + "TactPortDlg");
        }

        private void signals_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            errorProvider.Clear();

            if (e.RowIndex == -1)
                return;

            DataGridViewRow row = signals.Rows[e.RowIndex];

            string name = (string)row.Cells[(int)SignalCols.Name].Value;

            if (name == null || name == "")
            {
                errorProvider.SetError(signals, StringResource.SigNameErr);
                e.Cancel = true;
            }     
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 550);
        }

        private void TactPortDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
