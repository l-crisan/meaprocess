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
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;

namespace Mp.Mod.LabJackU12
{
    public partial class LabJackInputPortDlg : Form
    {
        private Document _doc;
        private XmlElement _xmlSigList;

        public LabJackInputPortDlg(Document doc, XmlElement xmlSigList)
        {
            InitializeComponent();
            _doc = doc;
            _xmlSigList = xmlSigList;
            FormStateHandler.Restore(this, Document.RegistryKey + "LabJackInputPortDlg");
            DataGridViewComboBoxColumn samplingModeCol = (DataGridViewComboBoxColumn) analogChns.Columns[5];
            samplingModeCol.Items.Clear();
            samplingModeCol.Items.Add(StringResource.SingleValue);
            samplingModeCol.Items.Add(StringResource.Stream);



            LoadChannels();
            LoadData();

        }

        private DataGridViewRow GetRowByData(int serial, int chnNo, int chnType)
        {
            DataGridView view = null;

            switch (chnType)
            {
                case 0:
                    view = analogChns;
                break;
                
                case 1:
                    view = digitalChns;
                break;
                
                case 2:
                    view = counterChns;
                break;
            }

            foreach (DataGridViewRow row in view.Rows)
            {
                int curSer = Convert.ToInt32(row.Cells[0].Value);
                int curChn = Convert.ToInt32(row.Cells[1].Tag);
                
                if (curSer == serial && curChn == chnNo)
                    return row;
            }

            return null;
        }

        private void LoadData()
        {
            foreach (XmlElement xmlSignal in _xmlSigList.ChildNodes)
            {
                int chnType = (int)XmlHelper.GetParamNumber(xmlSignal, "channelType");
                int serial = (int) XmlHelper.GetParamNumber(xmlSignal, "board");
                int chnNo = (int) XmlHelper.GetParamNumber(xmlSignal, "channel");
                DataGridViewRow row = GetRowByData(serial, chnNo, chnType);
                
                if (row == null)
                    continue;

                row.Tag = xmlSignal;
                row.Cells[2].Value = XmlHelper.GetParam(xmlSignal, "name");

                switch (chnType)
                {
                    case 0: //Analog
                    {
                        int mode = (int)XmlHelper.GetParamNumber(xmlSignal, "channelMode");
                        
                        if (mode == 0)
                            row.Cells[3].Value = "Single-Ended";
                        else
                            row.Cells[3].Value = "Differential";

                        int gain = (int)XmlHelper.GetParamNumber(xmlSignal, "gain");
                        row.Cells[4].Value = GetGain(gain);
                        double rate =  XmlHelper.GetParamDouble(xmlSignal, "samplerate");

                        if (XmlHelper.GetParamNumber(xmlSignal, "scanMode") == 0)
                        {
                            row.Cells[5].Value = StringResource.SingleValue;
                        }
                        else
                        {
                            UpdateSampleRate(serial, rate);
                            row.Cells[5].Value = StringResource.Stream;
                        }
                        
                            
                        row.Cells[7].Value = XmlHelper.GetParamDouble(xmlSignal, "physMin");
                        row.Cells[8].Value = XmlHelper.GetParamDouble(xmlSignal, "physMax");

                        row.Cells[9].Value = XmlHelper.GetParam(xmlSignal, "unit");
                        row.Cells[10].Value = XmlHelper.GetParam(xmlSignal, "comment");
                    }                    
                    break;
                    
                    case 1: //Digital
                    case 2: //Counter
                    {
                        row.Cells[4].Value = XmlHelper.GetParam(xmlSignal, "unit");
                        row.Cells[5].Value = XmlHelper.GetParam(xmlSignal, "comment");

                        double samplerate = XmlHelper.GetParamDouble(xmlSignal, "samplerate");
                        row.Cells[3].Value = GetSampleRate(samplerate);                                                                        
                    }
                    break;
                }
            }
        }

        private void UpdateSampleRate(int serial, double rate)
        {
            foreach (DataGridViewRow row in analogChns.Rows)
            {
                int curSer = Convert.ToInt32(row.Cells[0].Value);
                
                if (curSer == serial)
                    row.Cells[6].Value = Convert.ToUInt32(rate);
            }
        }
        private void LoadChannels()
        {
            InsertAnalogChannels(-1);
            InsertDigitalChannels(-1);
            InsertCounterChannels(-1);

            int[] productIDList = new int[127];
            int[] serialnumList = new int[127];
            int[] localIDList = new int[127];
            int[] powerList = new int[127];
            int[] calMatrix = new int[127 * 20];

            int numberFound = 0;
            int reserved1 = 0;
            int reserved2 = 0;
            try
            {
                int ret = DriverWrapper.ListAll(productIDList, serialnumList, localIDList, powerList, calMatrix, ref numberFound, ref reserved1, ref reserved2);

                for (int i = 0; i < numberFound; ++i)
                {
                    InsertAnalogChannels(serialnumList[i]);
                    InsertDigitalChannels(serialnumList[i]);
                    InsertCounterChannels(serialnumList[i]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InsertCounterChannels(int serial)
        {
            int index = counterChns.Rows.Add();
            DataGridViewRow row = counterChns.Rows[index];
            row.Cells[0].Value = serial;
            row.Cells[1].Tag = 0;
            row.Cells[1].Value = "Cnt" + 0;
            row.Cells[3].Value = "50 Hz";
        }

        private void InsertDigitalChannels(int serial)
        {
            for(int i = 0; i < 4; ++i)
            {//IO
                int index = digitalChns.Rows.Add();
                DataGridViewRow row = digitalChns.Rows[index];
                row.Cells[0].Value = serial;
                row.Cells[1].Tag = i;
                row.Cells[1].Value = "IO" +  i;
                row.Cells[3].Value = "10 Hz";
            }

            for (int i = 0; i < 16; ++i)
            {//D-Sub
                int index = digitalChns.Rows.Add();
                DataGridViewRow row = digitalChns.Rows[index];
                row.Cells[0].Value = serial;
                row.Cells[1].Tag = i;
                row.Cells[1].Value = "D" + i;
                row.Cells[3].Value = "10 Hz";
            }

        }

        private void InsertAnalogChannels(int serial)
        {
            uint srcId = RegisterSourceForBoard(serial);
            for (int i = 0; i < 8; ++i)
            {
                int index = analogChns.Rows.Add();
                DataGridViewRow row = analogChns.Rows[index];
                row.Cells[0].Value = serial;
                row.Cells[0].Tag = srcId;
                row.Cells[1].Value = "AI" + i;
                row.Cells[1].Tag = i;
                row.Cells[3].Value = "Single-Ended";
                row.Cells[4].Value = "±10 Volts";
                row.Cells[5].Value = StringResource.SingleValue;
                row.Cells[6].Value = 100;
                row.Cells[7].Value = -10;
                row.Cells[8].Value = 10;
                row.Cells[9].Value = "V";

            }
        }

        private bool ignoreChangeEvent = false;

        private void analogChns_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (ignoreChangeEvent || e.RowIndex == -1)
                return;

            DataGridViewRow row = analogChns.Rows[e.RowIndex];

            switch (e.ColumnIndex)
            {
                case 3:
                {
                    string mode = (string)row.Cells[e.ColumnIndex].Value;
                    int serial = Convert.ToInt32(row.Cells[0].Value);
                    ChangeMode(mode, serial, row);
                }
                break;
                case 5:
                {
                    string sampleMode = (string)row.Cells[e.ColumnIndex].Value;
                    int serial = Convert.ToInt32(row.Cells[0].Value);

                    foreach (DataGridViewRow curRow in analogChns.Rows)
                    {
                        int curSerial = Convert.ToInt32(curRow.Cells[0].Value);
                        if (curSerial == serial && curRow != row)
                        {
                            ignoreChangeEvent = true;
                            curRow.Cells[5].Value = sampleMode;
                            ignoreChangeEvent = false;
                        }
                    }

                    if (sampleMode == StringResource.Stream)
                    {
                        foreach (DataGridViewRow curRow in digitalChns.Rows)
                        {
                            int curSerial = Convert.ToInt32(curRow.Cells[0].Value);
                            if (serial == curSerial)
                            {
                                curRow.Cells[2].Value = "";
                                curRow.Cells[2].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                                curRow.Cells[3].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                                curRow.Cells[4].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                                curRow.Cells[5].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                                curRow.ReadOnly = true;
                            }
                        }

                        foreach (DataGridViewRow curRow in counterChns.Rows)
                        {
                            int curSerial = Convert.ToInt32(curRow.Cells[0].Value);
                            if (serial == curSerial)
                            {
                                curRow.Cells[2].Value = "";
                                curRow.Cells[2].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                                curRow.Cells[3].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                                curRow.Cells[4].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                                curRow.Cells[5].Style.BackColor = Color.FromKnownColor(KnownColor.InactiveCaption);
                                curRow.ReadOnly = true;
                            }
                        }

                        UpdateSampleRate(serial, Convert.ToDouble(row.Cells[6].Value));
                    }
                    else
                    {
                        foreach (DataGridViewRow curRow in digitalChns.Rows)
                        {
                            int curSerial = Convert.ToInt32(curRow.Cells[0].Value);
                            if (serial == curSerial)
                            {
                                curRow.Cells[2].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                                curRow.Cells[3].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                                curRow.Cells[4].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                                curRow.Cells[5].Style.BackColor = Color.FromKnownColor(KnownColor.Window);

                                curRow.ReadOnly = false;
                            }
                        }

                        foreach (DataGridViewRow curRow in counterChns.Rows)
                        {
                            int curSerial = Convert.ToInt32(curRow.Cells[0].Value);
                            if (serial == curSerial)
                            {
                                curRow.Cells[2].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                                curRow.Cells[3].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                                curRow.Cells[4].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                                curRow.Cells[5].Style.BackColor = Color.FromKnownColor(KnownColor.Window);
                                curRow.ReadOnly = false;
                            }
                        }
                    }
                }
                break;
            }
        }

        private void ChangeMode(string mode, int serial, DataGridViewRow row)
        {
            bool change = false;
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            foreach (DataGridViewRow curRow in analogChns.Rows)
            {
                int curSer = Convert.ToInt32(curRow.Cells[0].Value);
                if (curSer == serial)
                {
                    string curMode = (string)curRow.Cells[3].Value;
                    if (curMode != mode)
                    {

                        ignoreChangeEvent = true;
                        curRow.Cells[3].Value = mode;
                        ignoreChangeEvent = false;
                        change = true;
                    }

                    rows.Add(curRow);
                }
            }

            if (!change)
                return;
            
            if (mode == "Single-Ended")
            {
                int startIndex = rows[3].Index + 1;

                analogChns.Rows.Insert(startIndex, 4);
                int pos = 4;
                for (int i = startIndex; i < startIndex + 4; ++i)
                {
                    DataGridViewRow curRow = analogChns.Rows[i];
                    curRow.Cells[0].Value = rows[0].Cells[0].Value;
                    curRow.Cells[1].Value = "AI" + pos;
                    ++pos;
                    ignoreChangeEvent = true;
                    curRow.Cells[3].Value = mode;
                    ignoreChangeEvent = false;
                    curRow.Cells[4].Value = "±10 Volts";
                    curRow.Cells[4].ReadOnly = true;
                    curRow.Cells[5].Value = rows[0].Cells[5].Value;
                    curRow.Cells[6].Value = rows[0].Cells[6].Value;
                    curRow.Cells[7].Value = -10.0;
                    curRow.Cells[8].Value = 10.0;
                    curRow.Cells[9].Value = "V";
                }
                pos = 0;
                foreach (DataGridViewRow curRow in rows)
                {
                    curRow.Cells[4].Value = "±10 Volts";
                    curRow.Cells[4].ReadOnly = true;
                    curRow.Cells[1].Value = "AI" + pos;
                    pos++;
                }

            }
            else
            {
                int pos = 0;
                foreach (DataGridViewRow curRow in rows)
                {
                    curRow.Cells[1].Value = "AI" + pos;
                    pos += 2;
                    curRow.Cells[4].ReadOnly = false;
                }

                for (int i = 4; i < rows.Count; ++i)
                    analogChns.Rows.Remove(rows[i]);
            }
        }

        private void analogChns_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                int rate = Convert.ToInt32(analogChns.Rows[e.RowIndex].Cells[6].Value);
                int serial = (int) Convert.ToInt64(analogChns.Rows[e.RowIndex].Cells[0].Value);

                if(((string)analogChns.Rows[e.RowIndex].Cells[5].Value) == StringResource.Stream)
                    UpdateSampleRate(serial, rate);
            }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (!CheckAnalogChannels())
                return;

            _doc.Modified = true;
            SaveAnalogChannels();
            SaveDigitalChannels();
            SaveCounterChannels();
            RemoveUnmappedChannels();
            Close();
        }

        private bool CheckAnalogChannels()
        {
            Hashtable boardsToCheck = new Hashtable();

            foreach (DataGridViewRow row in analogChns.Rows)
            {
                int serial = Convert.ToInt32(row.Cells[0].Value);

                if (!boardsToCheck.Contains(serial))
                    boardsToCheck[serial] = row;
            }

            foreach (DictionaryEntry entry in boardsToCheck)
            {
                int serial = (int)entry.Key;
                DataGridViewRow curRow = (DataGridViewRow) entry.Value;
                
                int avtiveSignals = 0;
                double sumRate = 0;

                foreach (DataGridViewRow row in analogChns.Rows)
                {
                    int curSerial = Convert.ToInt32(row.Cells[0].Value);

                    if (curSerial == serial)
                    {
                        string signalName = (string) row.Cells[2].Value;

                        if(signalName  == null)
                            signalName = "";

                        if( signalName == "")
                            continue;
                        
                        avtiveSignals++;

                        double rate = Convert.ToDouble(row.Cells[6].Value);
                        sumRate += rate;
                    }
                }

                if((string)(curRow.Cells[5].Value) != StringResource.SingleValue &&  avtiveSignals > 4)
                {
                    string msg = String.Format(StringResource.SigNoErr, serial);
                    errorProvider.SetError(analogChns, msg);
                    return false;
                }

                if (sumRate > 1200)
                {
                    string msg = String.Format(StringResource.RateErr, serial);
                    errorProvider.SetError(analogChns, msg);
                    return false;
                }
            }
            return true;
        }

        private void RemoveUnmappedChannels()
        {
            for (int i = 0; i < _xmlSigList.ChildNodes.Count; ++i)
            {
                XmlElement xmlSignal = (XmlElement)_xmlSigList.ChildNodes[i];

                int chnType = (int)XmlHelper.GetParamNumber(xmlSignal, "channelType");
                int serial = (int) XmlHelper.GetParamNumber(xmlSignal, "board");
                int chnNo = (int)XmlHelper.GetParamNumber(xmlSignal, "channel");
                
                DataGridViewRow row = GetRowByData(serial, chnNo, chnType);

                if (row == null)
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                    continue;
                }

                string sigName = (string) row.Cells[2].Value;

                if (sigName == null || sigName == "")
                {
                    _doc.RemoveXmlObject(xmlSignal);
                    --i;
                    continue;
                }
            }
        }

        private XmlElement GetSignalByRow(DataGridViewRow row, string type, string sigName)
        {
            XmlElement xmlSignal = null;

            if (sigName == null)
                sigName = "";

            if (sigName == "" && row.Tag != null)
            {//Remove signal from list
                _doc.RemoveXmlObject((XmlElement)row.Tag);
                return null;
            }
            else if (sigName != "" && row.Tag != null)
            {//Update signal in list
                xmlSignal = (XmlElement)row.Tag;
            }
            else if (sigName != "" && row.Tag == null)
            {//Create signal in list
                xmlSignal = _doc.CreateXmlObject(_xmlSigList, "Mp.Sig", type);
            }
            else
            {
                return null;
            }

            return xmlSignal;
        }


        private string GetGain(int gain)
        {
            switch (gain)
            {
                case 0:
                    return "±20 Volts";

                case 1:
                    return "±10 Volts";
                    
                case 2:
                    return "±5 Volts";
                    
                case 3:
                    return "±4 Volts";
                    
                case 4:
                    return "±2.5 Volts";

                case 5:
                    return "±2 Volts";
                    
                case 6:
                    return "±1.25 Volts";
                    
                case 7:
                    return "±1 Volts";
            }
            return "±1 Volts";
        }

        private int GetGain(string gain)
        {
            switch (gain)
            {
                case "±20 Volts":
                    return 0;
                case "±10 Volts":
                    return 1;
                case "±5 Volts":
                    return 2;
                case "±4 Volts":
                    return 3;
                case "±2.5 Volts":
                    return 4;
                case "±2 Volts":
                    return 5;
                case "±1.25 Volts":
                    return 6;
                case "±1 Volts":
                    return 7;
            }
            return 1;
        }

        private void GetDeviceMinMax(int gain, out double min, out double max)
        {
            min = -10;
            max = 10;

            switch (gain)
            {
                case 0:
                    min = -20;
                    max = 20;
                break;
                
                case 1:
                    min = -10;
                    max = 10;
                break;
                
                case 2:
                    min = -5;
                    max = 5;
                break;
                case 3:
                    min = -4;
                    max = 4;
                break;

                case 4:
                    min = -2.5;
                    max = 2.5;
                break;
                case 5:
                    min = -2;
                    max = 2;
                break;
                case 6:
                    min = -1.25;
                    max = 1.25;
                break;
                case 7:
                    min = -1;
                    max = 1;
                break;
            }
        }
        
        private uint RegisterSourceForBoard(int serial)
        {
            string srcKey = serial.ToString();

            uint srcId = _doc.GetSourceIdByUID(srcKey);

            if (srcId == 0)
                srcId = _doc.RegisterSource("LabJack_" + srcKey.ToString(), serial, srcKey);

            return srcId;
        }

        private double GetSampleRate(string rate)
        {
            switch(rate)
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

        private string GetSampleRate(double rate)
        {
            switch ((int)rate)
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

        private void SaveCounterChannels()
        {
            //Save the signals
            foreach (DataGridViewRow row in counterChns.Rows)
            {
                string sigName = (string)row.Cells[2].Value;

                XmlElement xmlSignal = GetSignalByRow(row, "Mp.Sig.LabJack.Counter", sigName);

                if (xmlSignal == null)
                    continue;

                row.Tag = xmlSignal;

                //Set the signal values
                XmlHelper.SetParam(xmlSignal, "name", "string", sigName);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[4].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[5].Value);

                double samplerate = GetSampleRate((string)row.Cells[3].Value);
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", samplerate);

                XmlHelper.SetParamNumber(xmlSignal, "board", "int32_t", Convert.ToInt64(row.Cells[0].Value));

                XmlHelper.SetParamNumber(xmlSignal, "channelType", "uint8_t", 2);
                int channelNo = Convert.ToInt32(row.Cells[1].Tag);
                XmlHelper.SetParamNumber(xmlSignal, "channel", "uint8_t", channelNo);

                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", UInt32.MaxValue);

                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", 0);

                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.UDINT);
            }

        }

        private void SaveDigitalChannels()
        {
            //Save the signals
            foreach (DataGridViewRow row in digitalChns.Rows)
            {
                string sigName = (string)row.Cells[2].Value;

                XmlElement xmlSignal = GetSignalByRow(row, "Mp.Sig.LabJack.Digital", sigName);

                if (xmlSignal == null)
                    continue;

                row.Tag = xmlSignal;

                //Set the signal values
                XmlHelper.SetParam(xmlSignal, "name", "string", sigName);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[4].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[5].Value);

                double samplerate = GetSampleRate((string)row.Cells[3].Value);
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", samplerate);

                XmlHelper.SetParamNumber(xmlSignal, "board", "int32_t", Convert.ToInt32(row.Cells[0].Value));

                XmlHelper.SetParamNumber(xmlSignal, "channelType", "uint8_t", 1);
                int channelNo = Convert.ToInt32(row.Cells[1].Tag);
                XmlHelper.SetParamNumber(xmlSignal, "channel", "uint8_t", channelNo);

                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", 1);

                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", 0);

                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.BOOL);
            }
        }



        private uint UpdateSource(uint srcNo)
        {
            string uid = "LabJack" + Convert.ToString(srcNo);

            uint sourceID = _doc.GetSourceIdByUID(uid);

            if (sourceID == 0)
                sourceID = _doc.RegisterSource("LabJack", srcNo, uid);

            return sourceID;
        }

        private void SaveAnalogChannels()
        {
            //Save the signals
            foreach (DataGridViewRow row in analogChns.Rows)
            {
                string sigName = (string)row.Cells[2].Value;

                XmlElement xmlSignal = GetSignalByRow(row, "Mp.Sig.LabJack.Analog", sigName);

                if (xmlSignal == null)
                    continue;

                row.Tag = xmlSignal;

                //Set the signal values
                XmlHelper.SetParam(xmlSignal, "name", "string", sigName);
                XmlHelper.SetParam(xmlSignal, "unit", "string", (string)row.Cells[9].Value);
                XmlHelper.SetParam(xmlSignal, "comment", "string", (string)row.Cells[10].Value);

                double samplerate = Convert.ToDouble(row.Cells[6].Value);
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", samplerate);

                int gain = GetGain((string)row.Cells[4].Value);
                XmlHelper.SetParamNumber(xmlSignal, "gain", "uint8_t", gain);

                XmlHelper.SetParamNumber(xmlSignal, "board", "int32_t", Convert.ToInt32(row.Cells[0].Value));

                string mode = (string)row.Cells[3].Value;

                if (mode == "Single-Ended")
                    XmlHelper.SetParamNumber(xmlSignal, "channelMode", "uint8_t", 0);
                else
                    XmlHelper.SetParamNumber(xmlSignal, "channelMode", "uint8_t", 1);

                XmlHelper.SetParamNumber(xmlSignal, "channelType", "uint8_t", 0);
                int channelNo = Convert.ToInt32(row.Cells[1].Tag);
                XmlHelper.SetParamNumber(xmlSignal, "channel", "uint8_t", channelNo);

                if ((string)row.Cells[5].Value == StringResource.SingleValue)
                    XmlHelper.SetParamNumber(xmlSignal, "scanMode", "uint8_t", 0);
                else
                    XmlHelper.SetParamNumber(xmlSignal, "scanMode", "uint8_t", 1);

                double min = Convert.ToDouble(row.Cells[7].Value);
                double max = Convert.ToDouble(row.Cells[8].Value);


                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", min);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", max);

                uint srcNo = Convert.ToUInt32(row.Cells[0].Tag);

                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", UpdateSource(srcNo));

                XmlElement xmlScaling = XmlHelper.GetChildByType(xmlSignal, "Scaling");

                if (xmlScaling == null)
                    xmlScaling = _doc.CreateXmlObject(xmlSignal, "Mp.Scaling", "Mp.Scaling.FactorOffset");

                double factor = GetFactor(min, max, gain);
                double offset = GetOffset(min, max, gain);

                XmlHelper.SetParamDouble(xmlScaling, "factor", "double", factor);
                XmlHelper.SetParamDouble(xmlScaling, "offset", "double", offset);
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int)SignalDataType.REAL);
            }
        }

        private double GetFactor(double min, double max, int gain)
        {
            double devMin,devMax;

            GetDeviceMinMax(gain, out devMin, out devMax);
            return ((max - min) / (devMax - devMin));
        }

        private double GetOffset(double min, double max, int gain)
        {
            double devMin, devMax;

            GetDeviceMinMax(gain, out devMin, out devMax);

            return (devMin * max - devMax * min) / (devMax - devMin);
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void analogChns_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            errorProvider.Clear();
            if( e.ColumnIndex == 6)
            {
                try
                {
                    Convert.ToUInt32(e.FormattedValue);
                }
                catch(Exception ex)
                {
                    errorProvider.SetError(analogChns,ex.Message);
                    e.Cancel = true;
                }
            }

            if (e.ColumnIndex == 7 || e.ColumnIndex == 8)
            {
                try
                {
                    Convert.ToDouble(e.FormattedValue);
                }
                catch (Exception ex)
                {
                    errorProvider.SetError(analogChns, ex.Message);
                    e.Cancel = true;
                }
            }
        }

        private void analogChns_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if( e.RowIndex == -1)
                return;

            errorProvider.Clear();

            DataGridViewRow row = analogChns.Rows[e.RowIndex];


            double min = Convert.ToDouble(row.Cells[7].Value);
            double max = Convert.ToDouble(row.Cells[8].Value);

            if (min >= max)
            {
                e.Cancel = true;
                errorProvider.SetError(analogChns, "The physical maximum must be greater as teh minimum.");
            }
        }

        private void LabJackInputPortDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this, Document.RegistryKey + "LabJackInputPortDlg");
        }
    }
}
