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
using System.Xml;
using System.Windows.Forms;
using Mp.Scheme.Sdk;
using Mp.Utils;
using Mp.Drv.OpenCV;

namespace Mp.Mod.Video
{
    public partial class PortDlg : Form
    {
        private Mp.Scheme.Sdk.Document _doc;
        private XmlElement _xmlSignalList;

        public PortDlg(Document doc, XmlElement xmlSignalList)
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


        private void UpdateSource(int cardID)
        {
            string uid = "VIDEO_INPUT_SOURCE" + (cardID).ToString();

            uint sourceID = _doc.GetSourceIdByUID(uid);

            if (sourceID == 0)
                sourceID = _doc.RegisterSource("Video source " + cardID.ToString(), 3543, uid);
        }

        private void SaveSignals()
        {
            foreach(DataGridViewRow row in signals.Rows) 
            {
                if(row.IsNewRow)
                    continue;

                int cardID =  Convert.ToInt32(row.Cells[1].Value);
                UpdateSource(cardID);
       
                string uid = "VIDEO_INPUT_SOURCE" + (cardID).ToString();

                uint sourceID = _doc.GetSourceIdByUID(uid);

                XmlElement xmlSignal = (row.Tag as XmlElement);

                if(xmlSignal == null)
                      xmlSignal = _doc.CreateXmlObject(_xmlSignalList, "Mp.Sig", "Mp.Sig.Video.OpenCV");

                row.Tag = xmlSignal;
            
                XmlHelper.SetParam(xmlSignal, "name", "string", row.Cells[2].Value.ToString());
                XmlHelper.SetParamNumber(xmlSignal, "deviceID", "uint16_t", cardID);

                XmlHelper.SetParam(xmlSignal, "unit", "string", row.Cells[6].Value.ToString());
                XmlHelper.SetParam(xmlSignal, "comment", "string", row.Cells[7].Value.ToString() );
                XmlHelper.SetParam(xmlSignal, "cat", "string", "Mp.Sig.Video");            
                XmlHelper.SetParamDouble(xmlSignal, "samplerate", "double", Convert.ToDouble(row.Cells[5].Value));
                XmlHelper.SetParamNumber(xmlSignal, "valueDataType", "uint8_t", (int) SignalDataType.OBJECT);
                XmlHelper.SetParamDouble(xmlSignal, "physMin", "double", 0);
                XmlHelper.SetParamNumber(xmlSignal, "pixelFormat", "uint8_t", 3); //RGB888 = 3
            
           
                XmlHelper.SetParamNumber(xmlSignal, "sourceNumber", "uint32_t", sourceID);

                int width  = Convert.ToInt32(row.Cells[3].Value);
                int height = Convert.ToInt32(row.Cells[4].Value);
            
                XmlHelper.SetParamNumber(xmlSignal, "width", "uint32_t", width);
                XmlHelper.SetParamNumber(xmlSignal, "height", "uint32_t", height);

                string sigParams = "width= " + width.ToString() + ";";
                sigParams += "height=" + height.ToString() + ";";
                sigParams += "pixelFormat=" + "3" + ";";
                XmlHelper.SetParam(xmlSignal, "parameters", "string", sigParams);

                int objSize = width*height*3;
                XmlHelper.SetParamNumber(xmlSignal, "objSize", "uint32_t", objSize);
                XmlHelper.SetParamDouble(xmlSignal, "physMax", "double", objSize);                
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
                int cardID = (int) XmlHelper.GetParamNumber(xmlSignal, "deviceID");
                row.Cells[1].Value = cardID;
                row.Cells[2].Value  = XmlHelper.GetParam(xmlSignal, "name");
                row.Cells[3].Value = XmlHelper.GetParamNumber(xmlSignal, "width"); 
                row.Cells[4].Value = XmlHelper.GetParamNumber(xmlSignal, "height");
                row.Cells[5].Value = (int) XmlHelper.GetParamDouble(xmlSignal, "samplerate");
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
            row.Cells[2].Value = "";
            row.Cells[3].Value = 800;
            row.Cells[4].Value = 600;
            row.Cells[5].Value = 30;
            row.Cells[6].Value = "";
            row.Cells[7].Value = "";
            
        }

        private void DetectCard(DataGridViewRow row)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                int card = Convert.ToInt32(row.Cells[1].Value);
                DeviceInfo devInfo = VideoCapture.Detect(card);

                if(devInfo.Error != 0)
                {
                    MessageBox.Show(StringResource.DetectFailed, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Preview dlg = new Preview(devInfo);
                
                if(dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    Cursor = Cursors.Default;
                    return;
                }


                row.Cells[3].Value = devInfo.Width;
                row.Cells[4].Value = devInfo.Height;
                row.Cells[5].Value = devInfo.Rate;
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(StringResource.DetectFailed, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Cursor = Cursors.Default;
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

            switch(e.ColumnIndex)
            {
                case 1:
                {
                    try
                    {
                        Convert.ToUInt32(e.FormattedValue);
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(signals, ex.Message);
                        e.Cancel = true;
                    }        

                }
                break;

                case 3:
                case 4:
                case 5:
                {
                    try
                    {
                        
                        uint value = Convert.ToUInt32(e.FormattedValue);
                        if( value == 0)
                            throw new Exception(StringResource.ZeroValueError);
                    }
                    catch(Exception ex)
                    {
                        errorProvider.SetError(signals, ex.Message);
                        e.Cancel = true;
                    }        
                }
                break;

            }
        }
    }
}
