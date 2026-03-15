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

namespace Mp.Mod.GPS
{
    public partial class GpsPortDlg : Form
    {
        private XmlElement _xmlSignalList;
        private Document _doc;

        public GpsPortDlg(XmlElement xmlSigalList, Document doc)
        {
            _xmlSignalList = xmlSigalList;
            _doc = doc;
            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;

            sampleRate.SelectedIndex = 0;

            InitElements();
            LoadSignals();
        }

        private void LoadSignals()
        {
            double srate = 0.0;

            foreach(XmlElement xmlSignal in _xmlSignalList.ChildNodes)
            {
                int index = (int)XmlHelper.GetParamNumber(xmlSignal,"element");
                DataGridViewRow row = signals.Rows[index];
                row.Tag = xmlSignal;
                row.Cells[1].Value = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[3].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                row.Cells[4].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");
                row.Cells[5].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[6].Value = XmlHelper.GetParam(xmlSignal, "comment");
                srate = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
            }

            sampleRate.SelectedIndex = GetRate(srate);

        }

        private int GetRate(double r)
        {
            switch ((int)r)
            {
                case 1:
                    return 0;

                case 2:
                    return 1;

                case 5:
                    return 2;

                case 10:
                    return 3;

                case 20:
                    return 4;

                case 50:
                    return 5;

                case 100:
                    return 6;
            }

            return 0;
        }

        private double GetRate(int index)
        {
            switch (index)
            {
                case 0:
                    return 1.0;
                case 1:
                    return 2.0;
                case 2:
                    return 5.0;
                case 3:
                    return 10.0;
                case 4:
                    return 20.0;
                case 5:
                    return 50.0;
                case 6:
                    return 100.0;
            }

            return 1.0;
        }
        private void InitElements()
        {
            int index = signals.Rows.Add();
            DataGridViewRow row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Latitude;
            row.Cells[2].Value = "LREAL";
            row.Cells[2].Tag = SignalDataType.LREAL;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 90;
            row.Cells[5].Value = "rad";

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Longitude;
            row.Cells[2].Value = "LREAL";
            row.Cells[2].Tag = SignalDataType.LREAL;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 90;
            row.Cells[5].Value = "rad";
            
            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Altitude;
            row.Cells[2].Value = "LREAL";
            row.Cells[2].Tag = SignalDataType.LREAL;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 30000;
            row.Cells[5].Value = "m";

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Satellites;
            row.Cells[2].Value = "UDINT";
            row.Cells[2].Tag = SignalDataType.UDINT;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 20;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Day;
            row.Cells[2].Value = "USINT";
            row.Cells[2].Tag = SignalDataType.USINT;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 31;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Month;
            row.Cells[2].Value = "USINT";
            row.Cells[2].Tag = SignalDataType.USINT;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 12;
            

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Year;
            row.Cells[2].Value = "UINT";
            row.Cells[2].Tag = SignalDataType.UINT;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 3000;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Hour;
            row.Cells[2].Value = "USINT";
            row.Cells[2].Tag = SignalDataType.USINT;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 24;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Minute;
            row.Cells[2].Value = "USINT";
            row.Cells[2].Tag = SignalDataType.USINT;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 60;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Second;
            row.Cells[2].Value = "USINT";
            row.Cells[2].Tag = SignalDataType.USINT;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 60;


            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Status;
            row.Cells[2].Value = "BOOL";
            row.Cells[2].Tag = SignalDataType.BOOL;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 1;


            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Speed;
            row.Cells[2].Tag = SignalDataType.LREAL;
            row.Cells[2].Value = "LREAL";
            row.Cells[3].Value = "kn";
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 6000;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.TrackAngle;
            row.Cells[2].Value = "LREAL";
            row.Cells[2].Tag = SignalDataType.LREAL;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 360;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = StringResource.Quality;
            row.Cells[2].Value = "USINT";
            row.Cells[2].Tag = SignalDataType.USINT;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 3000;

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = "PDOP";
            row.Cells[2].Value = "LREAL";
            row.Cells[2].Tag = SignalDataType.LREAL;            
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 3000;
            row.Cells[5].Value = "m";

            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = "HDOP";
            row.Cells[2].Value = "LREAL";
            row.Cells[2].Tag = SignalDataType.LREAL;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 3000;            
            row.Cells[5].Value = "m";



            index = signals.Rows.Add();
            row = signals.Rows[index];
            row.Cells[0].Value = "VDOP";
            row.Cells[2].Value = "LREAL";
            row.Cells[2].Tag = SignalDataType.LREAL;
            row.Cells[3].Value = 0;
            row.Cells[4].Value = 3000;            
            row.Cells[5].Value = "m";

        }

        private void OK_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in signals.Rows)
            {
                string name = (string)row.Cells[1].Value;
                XmlElement xmlSignal = null;

                if (name == null)
                    name = "";

                if (name == "" && row.Tag != null)
                { //Remove 
                    xmlSignal = (XmlElement)row.Tag;
                    _doc.RemoveXmlObject(xmlSignal);
                    continue;
                }
                else if (name != "" && row.Tag != null)
                {//Update
                    xmlSignal = (XmlElement)row.Tag;
                }
                else if (name != "" && row.Tag == null)
                {//Create                    
                    xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Gps.Sig");
                }
                else
                {
                    continue;
                }

                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[1].Value);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[5].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[6].Value);

                XmlHelper.SetParamNumber(xmlSignal, "element", "uint8_t", row.Index);
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int) row.Cells[2].Tag);
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", Convert.ToDouble(row.Cells[3].Value));
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", Convert.ToDouble(row.Cells[4].Value));
                double srate = GetRate(sampleRate.SelectedIndex);
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", srate);
            }

            DialogResult = DialogResult.OK;
            _doc.Modified = true;
            Close();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void help_Click(object sender, EventArgs e)
        {
            Document.ShowHelp(this, 830);
        }

        private void GpsPortDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            help_Click(null, null);
        }

        private void signals_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if (e.RowIndex == -1)
                return;

            if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                try
                {
                    Convert.ToDouble(e.FormattedValue);
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    errorProvider.SetError(signals, ex.Message);
                    return;

                }
            }

        }

        private void signals_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            errorProvider.Clear();

            DataGridViewRow row = signals.Rows[e.RowIndex];

            double min = Convert.ToDouble(row.Cells[3].Value);
            double max = Convert.ToDouble(row.Cells[4].Value);

            if (min >= max)
            {
                e.Cancel = true;
                errorProvider.SetError(signals, StringResource.SigMinMaxErr);
            }
        }

        private void enableSignalsWithDefaultNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in signals.Rows)
                row.Cells[1].Value = row.Cells[0].Value;
        }
    }
}
