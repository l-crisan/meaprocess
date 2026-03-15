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
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Statistics
{
    public partial class StatisticValuesPortDlg : Form
    {
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPS;
        private Document _doc;

        public StatisticValuesPortDlg(Document doc, XmlElement xmlPS, XmlElement xmlSignalList)
        {
            _xmlSignalList = xmlSignalList;
            _xmlPS = xmlPS;
            _doc = doc;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            int index = signals.Rows.Add();
            DataGridViewRow row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Minimum;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Maximum;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.EffectiveValue;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.MeanValue;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.GeometricMean;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.HarmonicMean;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.QuadraticMean;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.CubicMean;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;   

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Variance;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.RootMeanSquareDeviation;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Median;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.PeakPeak;
            row.Cells[2].Value = 0;
            row.Cells[3].Value = 100;

            LoadData();
        }


        private void LoadData()
        {
            foreach (XmlElement xmlSignal in _xmlSignalList.ChildNodes)
            {
                int sigType = (int)XmlHelper.GetParamNumber(xmlSignal, "sigType");
                DataGridViewRow row = signals.Rows[sigType];
                row.Cells[1].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Tag = xmlSignal;

                row.Cells[2].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                row.Cells[3].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                row.Cells[4].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[5].Value = XmlHelper.GetParam(xmlSignal, "comment");
            }
        }


        private void signals_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            errorProvider.Clear();

            if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
            {
                try
                {
                    Convert.ToDouble(e.FormattedValue);
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    errorProvider.SetError(signals, ex.Message);
                }
            }
        }

        private void signals_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex == -1)
                return;

            errorProvider.Clear();

            DataGridViewRow row = signals.Rows[e.RowIndex];
            double min = Convert.ToDouble(row.Cells[2].Value);
            double max = Convert.ToDouble(row.Cells[3].Value);

            if ( min >= max)
            {
                e.Cancel = true;
                errorProvider.SetError(signals, StringResource.MinMaxErr);
            }
        }   

        private void OK_Click(object sender, EventArgs e)
        {
            _doc.UpdateSource(_xmlPS);

            foreach (DataGridViewRow row in signals.Rows)
            {
                string signalName = (string)row.Cells[1].Value;

                if (signalName == null)
                    signalName = "";

                XmlElement xmlSignal = (XmlElement)row.Tag;

                if (xmlSignal == null && signalName != "")
                {
                    xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Stat.Sig.StatValue");
                    row.Tag = xmlSignal;
                }
                else if (xmlSignal == null && signalName == "")
                {
                    continue;
                }
                if (xmlSignal != null && signalName == "")
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    continue;
                }

                XmlHelper.SetParamNumber(xmlSignal, "sigType", "uint8_t", row.Index);
                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[1].Value);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[4].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[5].Value);

                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", 10.0);
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.LREAL);
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(row.Cells[2].Value));
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(row.Cells[3].Value));

                uint srcID = (uint)XmlHelper.GetParamNumber(_xmlPS, "sourceId");
                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", (long)srcID);
            }

            _doc.Modified = true;
            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 1530);
        }
    }
}
