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
using System.Xml;
using System.Windows.Forms;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.Audio
{
    public partial class AudioInPsPortDlg : Form
    {
        private Mp.Scheme.Sdk.Document _doc;
        private XmlElement _xmlSignalList;

        public AudioInPsPortDlg(Document doc, XmlElement xmlSignalList)
        {
            InitializeComponent();
            
            Icon = Document.AppIcon;
            _doc = doc;
            _xmlSignalList = xmlSignalList;            

            LoadSignals();            
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            SaveSignals();

            _doc.Modified = true;
            Close();
        }


        private void UpdateSource(int deviceID)
        {
            string uid = "AUDIO_INPUT_SOURCE" + (deviceID).ToString();

            uint sourceID = _doc.GetSourceIdByUID(uid);

            if (sourceID == 0)
                sourceID = _doc.RegisterSource("Audio source " + deviceID.ToString(), 3244, uid);
        }

        public static double GetSampleRate(string rate)
        {
            switch(rate)
            {
                case "11.025 (kHz)":
                    return 11025;
                
                case "22.050 (kHz)":
                    return 22050;
                
                case "44.100 (kHz)":
                    return 44100 ;
                
                case "48.000 (KHz)":
                    return 48000;
                
                case "96.000 (kHz)":
                    return 96000;
                
                case "192.000 (kHz)":
                    return 192000;
            }

            throw new Exception("Unknow sample rate");
        }

        public static string GetRate(int rate)
        {
            switch (rate)
            {
                case 11025:
                    return "11.025 (kHz)";

                case 22050:
                    return "22.050 (kHz)";

                case 44100:
                    return "44.100 (kHz)";

                case 48000:
                    return "48.000 (KHz)";

                case 96000:
                    return "96.000 (kHz)";

                case 192000:
                    return "192.000 (kHz)";
            }
            throw new Exception("Unknow rate");
        }

        public static string GetFormat(int dtype)
        {
            SignalDataType    dataType = (SignalDataType) dtype;
            
            switch(dataType)
            {
                case SignalDataType.INT:
                    return "16 Bit";
                
                case SignalDataType.USINT:
                    return "8 Bit";
            }
            throw new Exception("Unknow format");
        }

        public static SignalDataType GetDataType(string format)
        {
            switch(format)
            {
                case "8 Bit":
                    return SignalDataType.USINT;
                
                case "16 Bit":
                    return SignalDataType.INT;
            }

            throw new Exception("Unknow format");
        }

        private void SaveSignals()
        {        
            foreach(DataGridViewRow row in signals.Rows) 
            {
                if(row.IsNewRow)
                    continue;

                int deviceID =  Convert.ToInt32(row.Cells[1].Value);
                int channelID =  Convert.ToInt32(row.Cells[2].Value);
                UpdateSource(deviceID);
       
                string uid = "AUDIO_INPUT_SOURCE" + (deviceID).ToString();

                uint sourceID = _doc.GetSourceIdByUID(uid);

                XmlElement xmlSignal = (row.Tag as XmlElement);

                if(xmlSignal == null)
                      xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Sig.Audio");

                row.Tag = xmlSignal;
            
                XmlHelper.SetParam(xmlSignal, "name", "string", row.Cells[3].Value.ToString());
                XmlHelper.SetParamNumber(xmlSignal, "deviceID", "uint16_t", deviceID);
                XmlHelper.SetParamNumber(xmlSignal, "channelID", "uint16_t", channelID);

                XmlHelper.SetParam(xmlSignal, "unit", "string", row.Cells[6].Value.ToString());
                XmlHelper.SetParam(xmlSignal, "comment", "string", row.Cells[7].Value.ToString() );
                XmlHelper.SetParam(xmlSignal, "cat", "string", "Mp.Sig.Audio");            
                
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", GetSampleRate(row.Cells[5].Value.ToString()));

                SignalDataType valueType = GetDataType(row.Cells[4].Value.ToString());
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)valueType);

                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 5);           
                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", sourceID);

                XmlElement xmlScaling = XmlHelper.GetChildByType(xmlSignal, "Mp.Scaling");
                if (xmlScaling == null)
                    xmlScaling = _doc.CreateXmlObject(xmlSignal, "Mp.Scaling", "Mp.Scaling.FactorOffset");

                double factor = 1;
                double offset = 0;
                double max = 5;
                double min = 0;

                if (valueType == SignalDataType.USINT)
                {
                    factor = (max - min) / ((double)Byte.MaxValue - (double)Byte.MinValue);
                    offset = ((double)(min * Byte.MaxValue) - (double)(max * Byte.MinValue)) / (Byte.MaxValue - Byte.MinValue);
                }
                else if (valueType == SignalDataType.INT)
                {
                    factor = (max - min) / ((double)Int16.MaxValue - (double)Int16.MinValue);
                    offset = ((double)(min * Int16.MaxValue) - (double)(max * Int16.MinValue)) / (Int16.MaxValue - Int16.MinValue);
                }

                XmlHelper.SetParamDouble(xmlScaling, "factor", "double", factor);
                XmlHelper.SetParamDouble(xmlScaling, "offset", "double", offset);
            }

            RemoveUnusedSignals();

            _doc.Modified = true;        
        }

        private bool IsSignalInGrid(XmlElement xmlSignal)
        {
            foreach(DataGridViewRow row in signals.Rows)
            {
                if( row.Tag == xmlSignal)
                    return true;
            }

            return false;
        }

        private void RemoveUnusedSignals()
        {
            List<XmlElement> toRemove = new List<XmlElement>();

            foreach(XmlElement xmlSignal in _xmlSignalList.ChildNodes)
            {
                if(!IsSignalInGrid(xmlSignal))
                    toRemove.Add(xmlSignal);                    
            }

            foreach(XmlElement xmlSignal in toRemove)
                _doc.RemoveXmlObject(xmlSignal);
        }

        private void LoadSignals()
        {
            int rowCount = 0;

            foreach(XmlElement xmlSignal in _xmlSignalList.ChildNodes)
            {
                int index  = signals.Rows.Add();
                DataGridViewRow row  = signals.Rows[index];
                int deviceID = (int) XmlHelper.GetParamNumber(xmlSignal, "deviceID");
                int channelID = (int) XmlHelper.GetParamNumber(xmlSignal, "channelID");
                row.Cells[1].Value = deviceID;
                row.Cells[2].Value = channelID;
                row.Cells[3].Value  = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[4].Value  =  GetFormat((int) XmlHelper.GetParamNumber(xmlSignal, "valueDataType"));
                row.Cells[5].Value = GetRate((int) XmlHelper.GetParamDouble(xmlSignal, "samplerate"));
                row.Cells[6].Value = XmlHelper.GetParam(xmlSignal, "unit");
                row.Cells[7].Value = XmlHelper.GetParam(xmlSignal, "comment");
                row.Tag = xmlSignal;
                rowCount++;

            }
            
            InitEmptyRow(rowCount);
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void OnSignalsRowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            InitEmptyRow(e.RowIndex);
        }

        private int GetNextFreeDeviceNo()
        {
            int no = 0;
            
            foreach(DataGridViewRow row in signals.Rows)
            {
                if( row.IsNewRow)
                    continue;

                 int curNo = Convert.ToInt32(row.Cells[1].Value);

                 no = Math.Max(curNo, no) + 1;
            }

            return no;
        }

        private void InitEmptyRow(int index)
        {
        
            DataGridViewRow row = (DataGridViewRow)signals.Rows[index];
            row.Cells[0].Value = "...";
            row.Cells[1].Value = GetNextFreeDeviceNo();
            row.Cells[2].Value = 1;
            row.Cells[3].Value = "";
            row.Cells[4].Value = "8 Bit";
            row.Cells[5].Value = "11.025 (kHz)";
            row.Cells[6].Value = "V";
            row.Cells[7].Value = "";            
        }

        private void DetectCard(DataGridViewRow row)
        {
            int device = Convert.ToInt32(row.Cells[1].Value);
            int channel = Convert.ToInt32(row.Cells[2].Value);

            AudioDeviceSelectDlg dlg = new AudioDeviceSelectDlg(device, channel, true);

            if(dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.Default;
                return;
            }


            row.Cells[1].Value = dlg.Device;
            row.Cells[2].Value = dlg.Channel;                                
        }


        private void OnSignalsCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex != 0)
                return;

            DataGridViewRow row = signals.Rows[e.RowIndex];


            DetectCard(row);
        }

        private void OnSignalsCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();

            if(!(e.ColumnIndex == 1 || e.ColumnIndex == 2))
                return;

            try
            {
                
                Convert.ToUInt16(e.FormattedValue);
            }
            catch(Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(signals,ex.Message);
            }
        }

    }
}
