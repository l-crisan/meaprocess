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
using System.Xml;
using System.Windows.Forms;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Clock
{
    internal partial class ClockPortDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSigList;
        private XmlElement _xmlPs;

        public ClockPortDlg(Document doc, XmlElement xmlSigList, XmlElement xmlPs)
        {
            _doc = doc;
            _xmlSigList = xmlSigList;
            _xmlPs = xmlPs;

            InitializeComponent();
            this.Icon = Mp.Scheme.Sdk.Document.AppIcon;
            LoadDefaultData();
            LoadSignals();
        }

        private void LoadSignals()
        {
            if (XmlHelper.GetParamNumber(_xmlPs, "time") == 0)
                localTime.Checked = true;
            else
                utcTime.Checked = true;

            int sample = 10;
            foreach (XmlElement xmlSignal in _xmlSigList)
            {
                int type = (int) XmlHelper.GetParamNumber(xmlSignal,"type");
                DataGridViewRow row = _signalGrid.Rows[type-1];
                row.Tag = xmlSignal;
                row.Cells[1].Value = XmlHelper.GetParam(xmlSignal,"name");
                row.Cells[3].Value = XmlHelper.GetParam(xmlSignal,"unit");
                row.Cells[4].Value = XmlHelper.GetParam(xmlSignal,"comment");
                double dbSampleRate = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                sample = (int) dbSampleRate;
            }

            switch (sample)
            {
                case 1:
                    sampleRate.SelectedIndex = 0;
                break;

                case 2:
                    sampleRate.SelectedIndex = 1;
                break;

                case 5:
                    sampleRate.SelectedIndex = 2;
                break;

                case 10:
                    sampleRate.SelectedIndex = 3;
                break;

                case 20:
                    sampleRate.SelectedIndex = 4;
                break;

                case 50:
                    sampleRate.SelectedIndex = 5;
                break;

                case 100:
                    sampleRate.SelectedIndex = 6;
                break;

            }            
        }

        private void LoadDefaultData()
        {
            int i = _signalGrid.Rows.Add();            
            DataGridViewRow row = _signalGrid.Rows[i];
            row.Cells[0].Value = StringResource.Year;
            row.Cells[1].Value = "";
            row.Cells[2].Value = "UINT";
            row.Cells[3].Value = "";
            row.Cells[4].Value = "";


            i = _signalGrid.Rows.Add();
            row = _signalGrid.Rows[i];
            row.Cells[0].Value = StringResource.Month;
            row.Cells[1].Value = "";
            row.Cells[2].Value = "USINT";
            row.Cells[3].Value = "";
            row.Cells[4].Value = "";

            i = _signalGrid.Rows.Add();
            row = _signalGrid.Rows[i];
            row.Cells[0].Value = StringResource.Day;
            row.Cells[1].Value = "";
            row.Cells[2].Value = "USINT";
            row.Cells[3].Value = "";
            row.Cells[4].Value = "";

            i = _signalGrid.Rows.Add();
            row = _signalGrid.Rows[i];
            row.Cells[0].Value = StringResource.Hour;
            row.Cells[1].Value = "";
            row.Cells[2].Value = "USINT";
            row.Cells[3].Value = "";
            row.Cells[4].Value = "";

            i = _signalGrid.Rows.Add();
            row = _signalGrid.Rows[i];
            row.Cells[0].Value = StringResource.Minute;
            row.Cells[1].Value = "";
            row.Cells[2].Value = "USINT";
            row.Cells[3].Value = "";
            row.Cells[4].Value = "";

            i = _signalGrid.Rows.Add();
            row = _signalGrid.Rows[i];
            row.Cells[0].Value = StringResource.Second;
            row.Cells[1].Value = "";
            row.Cells[2].Value = "USINT";
            row.Cells[3].Value = "";
            row.Cells[4].Value = "";

            i = _signalGrid.Rows.Add();
            row = _signalGrid.Rows[i];
            row.Cells[0].Value = StringResource.Millisecond;
            row.Cells[1].Value = "";
            row.Cells[2].Value = "UINT";
            row.Cells[3].Value = "";
            row.Cells[4].Value = "";
            Invalidate();
        }

        private double GetSampleRate()
        {
            switch (sampleRate.SelectedIndex)
            {
                case 0: //1Hz
                    return 1.0;
                
                case 1: //2Hz
                    return 2.0;
                
                case 2: //5Hz
                    return 5.0;
                
                case 3: //10Hz
                    return 10.0;
                
                case 4: //20Hz
                    return 20.0;
                
                case 5: //50 Hz
                    return 50.0;
                
                case 6: //100 Hz
                    return 100.0;                
            }

            return 0.0;
        }

        private void OK_Click(object sender, EventArgs e)
        {
            double samplerate = GetSampleRate();
            
            if (localTime.Checked)
                XmlHelper.SetParamNumber(_xmlPs, "time", "uint8_t", 0);
            else
                XmlHelper.SetParamNumber(_xmlPs, "time", "uint8_t", 1);


            foreach (DataGridViewRow row in _signalGrid.Rows)
            {
                string name = (string)row.Cells[1].Value;
                XmlElement xmlSignal = null;

                if (name == null)
                    name = "";

                if ( name == "" && row.Tag != null)
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
                    xmlSignal = _doc.CreateXmlObject(_xmlSigList, "Mp.Sig", "Mp.Clock.Sig.Clock");
                }
                else
                {
                    continue;
                }

                XmlHelper.SetParam(xmlSignal, "name", "string", (string)row.Cells[1].Value);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string) row.Cells[3].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string) row.Cells[4].Value);
                
                XmlHelper.SetParamNumber(xmlSignal, "type", "uint8_t", row.Index +1);

                switch (row.Index + 1)
                {
                    case 1://Year
                        XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 2008);
                        XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 2020);
                        XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", 6);
                    break;
                    case 2://Month
                        XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                        XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 12);
                        XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", 4);
                    break;
                    case 3://Day
                        XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 1);
                        XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 31);
                        XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", 4);
                    break;
                    case 4://Hour
                        XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                        XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 24);
                        XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", 4);
                    break;
                    case 5://Minute
                    case 6://Seconds
                        XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                        XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 60);
                        XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", 4);
                    break;
                    case 7://Milliseconds
                        XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                        XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 1000);
                        XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", 6);
                    break;
                }
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", samplerate);
                XmlHelper.SetParam(xmlSignal, "sourceNumber", "uint32_t", "0");
            }

            Close();
        }

        private void ClockPortDlg_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Document.ShowHelp(this, 340);
        }

        private void help_Click(object sender, EventArgs e)
        {
            ClockPortDlg_HelpRequested(null, null);
        }

        private void activateWithDefaultNamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in _signalGrid.SelectedCells)
            {
                if (cell.RowIndex == -1)
                    continue;

                DataGridViewRow row = _signalGrid.Rows[cell.RowIndex];
                row.Cells[1].Value = row.Cells[0].Value;
            }
        }

        private void desctivateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in _signalGrid.SelectedCells)
            {
                if (cell.RowIndex == -1)
                    continue;

                DataGridViewRow row = _signalGrid.Rows[cell.RowIndex];
                row.Cells[1].Value = "";
            }
        }
    }
}
