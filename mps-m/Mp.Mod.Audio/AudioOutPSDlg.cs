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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Mp.Scheme.Sdk;
using Mp.Utils;


namespace Mp.Mod.Audio
{
    public partial class AudioOutPSDlg : Form
    {
        private XmlElement _xmlSignalList;
        private XmlElement _xmlPs;
        private Document _doc;
        private SignalInputView _signalView;
        private bool _ignoreValidate;

        public AudioOutPSDlg(Document doc, XmlElement xmlPs, XmlElement xmlSignalList)
        {
            _doc = doc;
            _xmlPs = xmlPs;
            _xmlSignalList = xmlSignalList;

            InitializeComponent();
            this.Icon = Document.AppIcon;

            _signalView = new SignalInputView(_doc, _xmlSignalList);
            _signalView.Dock = DockStyle.Fill;
            splitContainer1.Panel1.Controls.Add(_signalView);

            FormStateHandler.Restore(this,Document.RegistryKey + "AudioOutPSDlg");          

            LoadChannels();            
        }

        public string PsName
        {
            get { return psName.Text; }
            set { psName.Text = value; }
        }

        private int GetNextFreeDeviceNo()
        {
            int no = 0;

            foreach (DataGridViewRow row in outputDevices.Rows)
            {
                if (row.IsNewRow)
                    continue;

                int curNo = Convert.ToInt32(row.Cells[2].Value);

                no = Math.Max(curNo, no) + 1;
            }

            return no;
        }

        private void InitEmptyRow(int index)
        {

            DataGridViewRow row = (DataGridViewRow)outputDevices.Rows[index];
            row.Cells[0].Value = "";
            row.Cells[1].Value = "...";
            row.Cells[2].Value = GetNextFreeDeviceNo();
            row.Cells[3].Value = 1;
            row.Cells[4].Value = "8 Bit";
            row.Cells[5].Value = "11.025 (kHz)";
        }

        private void LoadChannels()
        {
            _ignoreValidate = true;
            int pos = 0;
            
            XmlElement xmlDevices = XmlHelper.GetChildByType(_xmlPs, "Mp.Audio.Channels");
            
            if (xmlDevices != null)
            {                
                foreach (XmlElement xmlChannel in xmlDevices.ChildNodes)                
                {
                    int index = outputDevices.Rows.Add();
                    DataGridViewRow row = outputDevices.Rows[index];

                    row.Cells[1].Value = "...";

                    int deviceID = (int) XmlHelper.GetParamNumber(xmlChannel,"deviceID");
                    row.Cells[2].Value = deviceID;

                    int channelID = (int) XmlHelper.GetParamNumber(xmlChannel,"channelID");       
                    row.Cells[3].Value = channelID;
                       
                    int dtype = (int) XmlHelper.GetParamNumber(xmlChannel, "dataType");
                    row.Cells[4].Value =AudioInPsPortDlg.GetFormat(dtype);
                
                    int rate = (int) XmlHelper.GetParamNumber(xmlChannel, "sampleRate");
                    row.Cells[5].Value =AudioInPsPortDlg.GetRate(rate);

                    uint sigID = (uint)XmlHelper.GetParamNumber(xmlChannel, "signal");
                    XmlElement xmlSignal =  _doc.GetXmlObjectById(sigID);

                    if( xmlSignal != null)
                    {
                        row.Tag = xmlSignal;
                        row.Cells[0].Value =  XmlHelper.GetParam(xmlSignal,"name");
                    }
                    pos++;
                }
            }
            
            InitEmptyRow(pos);
            _ignoreValidate = false;
        }

        private bool IsChannelAvail(DataGridViewRow row )
        {
            int device = Convert.ToUInt16(row.Cells[2].Value);
            int channel = Convert.ToUInt16(row.Cells[3].Value);

            foreach(DataGridViewRow curRow in outputDevices.Rows)
            {
                if(row == curRow)
                    continue;

                int curDevice = Convert.ToUInt16(curRow.Cells[2].Value);             
                int curChannel = Convert.ToUInt16(curRow.Cells[3].Value);
                
                if (curDevice == device && curChannel == channel)
                    return true;
            }

            return false;
        }

        private void OnOKClick(object sender, EventArgs e)
        {           
            if(!CheckChannelAssignemnt())
                return;

            if (!CheckChannel(4, StringResource.WrongDeviceFormat))
                return;

            if (!CheckChannel(5, StringResource.WrongDeviceRate))
                return;

            //Remove the old devices
            XmlElement xmlDevices = XmlHelper.GetChildByType(_xmlPs, "Mp.Audio.Channels");
            
            if (xmlDevices != null)
                _doc.RemoveXmlObject(xmlDevices);

            //Create the new devices.
            xmlDevices = _doc.CreateXmlObject(_xmlPs, "Mp.Audio.Channels", "");

            foreach (DataGridViewRow row in outputDevices.Rows)
            {
                if(row.IsNewRow)
                    continue;

                XmlElement xmlDevice = _doc.CreateXmlObject(xmlDevices, "Mp.Audio.Channel", "");
                
                int deviceID  = Convert.ToUInt16(row.Cells[2].Value);
                XmlHelper.SetParamNumber(xmlDevice, "deviceID", "uint16_t", deviceID);

                int channelID  = Convert.ToUInt16(row.Cells[3].Value);                
                XmlHelper.SetParamNumber(xmlDevice, "channelID", "uint16_t", channelID);
                
                int dtype = (int) AudioInPsPortDlg.GetDataType(row.Cells[4].Value.ToString());                                
                XmlHelper.SetParamNumber(xmlDevice, "dataType", "uint8_t", dtype);

                long sampleRate = (long) AudioInPsPortDlg.GetSampleRate(row.Cells[5].Value.ToString());
                XmlHelper.SetParamNumber(xmlDevice, "sampleRate", "uint32_t", sampleRate);

                if( row.Tag != null)
                {
                    XmlElement xmlSignal = (XmlElement) row.Tag;
                    uint id = XmlHelper.GetObjectID(xmlSignal);
                    XmlHelper.SetParamNumber(xmlDevice, "signal", "uint32_t", id);
                }
                else
                {
                    XmlHelper.SetParamNumber(xmlDevice, "signal", "uint32_t", 0);
                }
            }
            
            _doc.Modified = true;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private bool CheckRow(DataGridViewRow row, int col)
        {
            int deviceID = Convert.ToInt32(row.Cells[2].Value);

            foreach (DataGridViewRow curRow in outputDevices.Rows)
            {
                if (curRow == row)
                    continue;

                int curDeviceID = Convert.ToInt32(curRow.Cells[2].Value);

                if (deviceID != curDeviceID)
                    continue;

                if (curRow.Cells[col].Value.ToString() != row.Cells[col].Value.ToString())
                    return false;
            }

            return true;
        }

        private bool CheckChannel(int col, string msgFormat)
        {
            errorProvider.Clear();

           foreach(DataGridViewRow row in outputDevices.Rows)
           {
                if(!CheckRow(row, col))
                {
                    int device = Convert.ToUInt16(row.Cells[2].Value);                    
                    string msg = String.Format(msgFormat, device);
                    errorProvider.SetError(outputDevices, msg);
                    return false;
                }
           }

           return true;
        }

        private bool CheckChannelAssignemnt()
        {
            errorProvider.Clear();

            foreach (DataGridViewRow row in outputDevices.Rows)
            {
                if (IsChannelAvail(row))
                {
                    int device = Convert.ToUInt16(row.Cells[2].Value);
                    int channel = Convert.ToUInt16(row.Cells[3].Value);

                    string msg = String.Format(StringResource.ChannelAllReadySetErr, channel, device);

                    errorProvider.SetError(outputDevices, msg);
                    return false;

                }
            }
            return true;
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }   

        private void OnAudioDevicesDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
            {
                ListViewItem item = (ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");

                if (item.Tag is XmlElement)
                    e.Effect = DragDropEffects.Move;
            }

        }

        private void OnAudioDeviceDragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("System.Windows.Forms.ListViewItem"))
                return;

            System.Windows.Forms.ListViewItem item = (System.Windows.Forms.ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");


            XmlElement xmlSignal = item.Tag as XmlElement;

            if (xmlSignal == null)
                return;

            Point p = outputDevices.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo info = outputDevices.HitTest(p.X, p.Y);
            
            if(info.RowIndex  < 0)
                return;

            DataGridViewRow row = outputDevices.Rows[info.RowIndex];
            
            if(row.IsNewRow)
                return;

            row.Tag = xmlSignal;
            row.Cells[0].Value = XmlHelper.GetParam(xmlSignal, "name");
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            FormStateHandler.Save(this,Document.RegistryKey + "AudioOutPSDlg");
        }

     
        private void OnHelpRequested(object sender, HelpEventArgs hlpevent)
        {
            OnHelpClick(null, null);
        }

        private void OnHelpClick(object sender, EventArgs e)
        {
            Document.ShowHelp(this,430);
        }

        private void OnOutputDevicesCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex != 1 || e.RowIndex < 0)
                return;

            DataGridViewRow row = outputDevices.Rows[e.RowIndex];
            
            if(row.IsNewRow)
            {
                _ignoreValidate = true;
                int index = outputDevices.Rows.Add();
                row = outputDevices.Rows[index];
                InitEmptyRow(index+ 1);
                _ignoreValidate = false;
            }

            int device = Convert.ToInt32(row.Cells[2].Value);
            int channel = Convert.ToInt32(row.Cells[3].Value);

            AudioDeviceSelectDlg dlg = new AudioDeviceSelectDlg(device, channel, false);

            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                Cursor = Cursors.Default;
                return;
            }

            row.Cells[0].Value = "";
            row.Cells[1].Value = "...";
            row.Cells[2].Value = dlg.Device;
            row.Cells[3].Value = dlg.Channel;
            row.Cells[4].Value = "8 Bit";
            row.Cells[5].Value = "11.025 (kHz)";
        }

        private void OnDeviceCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if(_ignoreValidate)
                return;

            errorProvider.Clear();

            if (!(e.ColumnIndex == 2 || e.ColumnIndex == 3))
                return;

            try
            {
                Convert.ToUInt16(e.FormattedValue);
            }
            catch (Exception ex)
            {
                e.Cancel = true;
                errorProvider.SetError(outputDevices, ex.Message);
            }
        }
    }
}
