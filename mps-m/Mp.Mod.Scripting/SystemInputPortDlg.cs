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

namespace Mp.Mod.Scripting
{
    public partial class SystemInputPortDlg : Form
    {
        private XmlElement _xmlSigList;
        private Document _doc;


        public SystemInputPortDlg(Document doc, XmlElement xmlSigList)
        {
            _xmlSigList = xmlSigList;
            _doc = doc;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            foreach (XmlElement xmlSignal in _xmlSigList.ChildNodes)
            {
                int index = signals.Rows.Add();
                DataGridViewRow row = signals.Rows[index];
                row.Tag = xmlSignal;
                row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[1].Value = GetSampleRate(xmlSignal);
                row.Cells[2].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                row.Cells[3].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                row.Cells[4].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[5].Value = XmlHelper.GetParam(xmlSignal, "comment");
            }

            signals.Rows[signals.Rows.Count -1].Cells[1].Value = "1 Hz";
        }


        private string GetSampleRate(XmlElement xmlSignal)
        {
            int rate = (int)XmlHelper.GetParamDouble(xmlSignal, "samplerate");
            switch (rate)
            {
                case 1:
                    return "1 Hz";
                case 2:
                    return "2 Hz";
                case 5:
                    return "5 Hz";
                case 10:
                    return "10 Hz";
                case 20:
                    return "20 Hz";
                case 50:
                    return "50 Hz";
                case 100:
                    return "100 Hz";

            }

            return "1 Hz";
        }

        private double GetSampleRate(DataGridViewRow row)
        {
            string rate = (string) row.Cells[1].Value;
            switch (rate)
            {
                case "1 Hz":
                    return 1;
                case "2 Hz":
                    return 2;
                case "5 Hz":
                    return 5;
                case "10 Hz":
                    return 10;
                case "20 Hz":
                    return 20;
                case "50 Hz":
                    return 50;
                case "100 Hz":
                    return 100;
            }

            return 1;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            foreach (DataGridViewRow row in signals.Rows)
            {
                if (row.IsNewRow)
                    continue;

                string name = Convert.ToString(row.Cells[0].Value);
                if (name == null || name == "")
                {
                    errorProvider.SetError(signals, StringResource.SigNameErr);
                    return;
                }
            }

            foreach (DataGridViewRow row in signals.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                XmlElement xmlSignal = row.Tag as XmlElement;

                if (xmlSignal == null)
                {
                    xmlSignal = _doc.CreateXmlObject(_xmlSigList, "Mp.Sig", "Mp.Sig");
                    row.Tag = xmlSignal;
                }

                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[0].Value);
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int) SignalDataType.LREAL);
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(row.Cells[2].Value));
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(row.Cells[3].Value));
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", GetSampleRate(row));
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[4].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[5].Value);

            }

            RemoveUnusedSignals();

            _doc.Modified = true;
            Close();
        }


        private DataGridViewRow GetRowBySignal(XmlElement xmlSignal)
        {
            foreach (DataGridViewRow row in signals.Rows)
            {
                XmlElement xmlCurSig = (XmlElement) row.Tag;

                if (xmlCurSig == xmlSignal)
                    return row;
            }

            return null;
        }

        private void RemoveUnusedSignals()
        {
            for(int i = 0; i < _xmlSigList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal =(XmlElement) _xmlSigList.ChildNodes[i];
                DataGridViewRow row = GetRowBySignal(xmlSignal);
                if (row == null)
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                }
            }
        }

        private void signals_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            DataGridViewRow row = signals.Rows[e.RowIndex];
            errorProvider.Clear();
  
            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                try
                {
                    Convert.ToDouble(e.FormattedValue);
                }
                catch (Exception ex)
                {
                    errorProvider.SetError(signals, ex.Message);
                    e.Cancel = true;
                }
            }
        }

        private void signals_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewRow row = signals.Rows[e.RowIndex];

            double min = Convert.ToDouble(row.Cells[2].Value);
            double max = Convert.ToDouble(row.Cells[3].Value);
            errorProvider.Clear();
            if (min >= max)
            {
                errorProvider.SetError(signals,StringResource.MinMaxErr);
                e.Cancel = true;
            }
        }

        private void remove_Click(object sender, EventArgs e)
        {
            if (signals.SelectedCells.Count == 0)
                return;

            signals.Rows.RemoveAt(signals.SelectedCells[0].RowIndex);
        }

        private void signals_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            DataGridViewRow row = signals.Rows[e.RowIndex];
            row.Cells[1].Value = "1 Hz";
        }

    }
}
