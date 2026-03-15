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
    public partial class TimerPortDlg : Form
    {
        private enum SignalCols
        {
            Name,
            GenRate,
            Interval,
            Unit,
            Comment,
            DataType,
        }

        private XmlElement _xmlSignalList;
        private Document _doc;

        public TimerPortDlg(Document doc, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlSignalList = xmlSignalList;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            LoadData();
            FormStateHandler.Restore(this, Document.RegistryKey + "TimerPortDlg"); 
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
                row.Cells[(int)SignalCols.GenRate].Value = rate;
                row.Cells[(int)SignalCols.Interval].Value = XmlHelper.GetParam(xmlSignal, "interval");
                row.Tag = xmlSignal;
                index++;
            }
            InitNewRow(index);
        }

        private void InitNewRow(int index)
        {
            DataGridViewRow row = signals.Rows[index];
            row.Cells[(int)SignalCols.GenRate].Value = 100;
            row.Cells[(int)SignalCols.Interval].Value = 100;
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
                    xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Clock.Sig.Timer");
                    row.Tag = xmlSignal;
                }

                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[(int)SignalCols.Name].Value);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[(int)SignalCols.Unit].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[(int)SignalCols.Comment].Value);

                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 1);
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.BOOL);
                XmlHelper.SetParamNumber(xmlSignal, "interval", "uint32_t",Convert.ToUInt32(row.Cells[(int)SignalCols.Interval].Value));
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", Convert.ToDouble(row.Cells[(int)SignalCols.GenRate].Value));
            }

            //Remove unused signals
            for (int i = 0; i < _xmlSignalList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement)_xmlSignalList.ChildNodes[i];
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

            if (e.ColumnIndex == (int)SignalCols.GenRate)
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
                }
                catch (Exception ex)
                {
                    errorProvider.SetError(signals, ex.Message);
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == (int)SignalCols.Name)
            {
                string name = (string)e.FormattedValue;

                if (name == null || name == "")
                {
                    errorProvider.SetError(signals, StringResource.SigNameErr);
                    e.Cancel = true;
                }
            }
            else if (e.ColumnIndex == (int)SignalCols.Interval)
            {
                try
                {
                    uint interval = Convert.ToUInt32(e.FormattedValue);

                    if (interval == 0)
                    {
                        e.Cancel = true;
                        errorProvider.SetError(signals, StringResource.PeriodeErr);
                        return;
                    }

                    if (interval % 10 != 0)
                    {
                        e.Cancel = true;
                        errorProvider.SetError(signals, StringResource.TimerRes10Err);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    errorProvider.SetError(signals, ex.Message);
                    e.Cancel = true;
                }
            }

        }

        private void signals_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            InitNewRow(e.RowIndex);
        }

        private void remove_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in signals.SelectedRows)
                {
                    if (!row.IsNewRow)
                        signals.Rows.Remove(row);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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

        private void TimerPortDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, Document.RegistryKey + "TimerPortDlg");
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 560);
        }

        private void TimerPortDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }
    }
}
