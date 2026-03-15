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
using System.Windows.Forms;
using Mp.Drv.Audio;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Audio
{
    public partial class AudioDeviceSelectDlg : Form
    {
        private int _device;
        private int _channel;
        private bool _input;
    
        public AudioDeviceSelectDlg(int devNo, int chnNo, bool input)
        {            
            InitializeComponent();
            _device = devNo;
            _channel = chnNo;
            this.Icon = Document.AppIcon;
            _input = input;

            Cursor = Cursors.WaitCursor;
            DeviceInfo[] deviceInfos = AudioDeviceInfo.Detect();
            Cursor = Cursors.Default;
            bool selected = false;

            foreach(DeviceInfo devInfo in deviceInfos)
            {
                int channels =  devInfo.maxOutputChannels;

                if( input)
                    channels = devInfo.maxInputChannels;

                for( int i = 0; i < channels; ++i)
                {                    
                    int index = devices.Rows.Add();
                    DataGridViewRow row = devices.Rows[index];
                
                    row.Cells[0].Value = false;

                    if( _device == devInfo.hostApi && _channel == (i+1))
                    {
                        selected = true;
                        row.Cells[0].Value = true;
                    }

                    row.Cells[1].Value = devInfo.name;
                    row.Cells[2].Value = devInfo.hostApi;
                    row.Cells[3].Value = (i+1);
                }
            }

            if (!selected  && devices.Rows.Count != 0)
            {
                DataGridViewRow row = devices.Rows[0];
                row.Cells[0].Value = true;
            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in devices.Rows)
            {
               if( Convert.ToBoolean(row.Cells[0].Value))
               {
                    _device  = Convert.ToInt32(row.Cells[2].Value);
                    _channel = Convert.ToInt32(row.Cells[3].Value);
                    break;
               } 
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        public int Device
        {
            get{ return _device;}
        }

        public int Channel
        {
            get { return _channel; }
        }

        private void OnDevicesCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex != 0)
                return;
            
            DataGridViewRow row  = devices.Rows[e.RowIndex];
            bool selected = Convert.ToBoolean(row.Cells[0].Value);

            foreach(DataGridViewRow curRow in  devices.Rows)
                curRow.Cells[0].Value = false;

            row.Cells[0].Value = selected;
        }
    }
}