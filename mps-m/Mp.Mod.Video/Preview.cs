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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Mp.Scheme.Sdk;

using Mp.Drv.OpenCV;

namespace Mp.Mod.Video
{
    public partial class Preview : Form
    {
        private ulong _handle;
        private Timer _timer = new Timer();
        private byte[] _frameBuffer;
        private DeviceInfo _info;

        public Preview(DeviceInfo info)
        {
            Icon = Document.AppIcon;
            InitializeComponent();

            _info = info;
            _frameBuffer = new byte[info.Width * info.Height * 3];

            this.Text += StringResource.CardNo + info.CardID.ToString();

            _handle = VideoCapture.Open(info.CardID);
            
            pictureView.Width = _info.Width;
            pictureView.Height =  _info.Height;
            _timer.Interval = 10;
            _timer.Tick += new EventHandler(OnTimerEvent);            

            if(_handle  != 0)
            {
                 width.Text = info.Width.ToString();
                 height.Text = info.Height.ToString();
                 rate.Text = info.Rate.ToString();
                _timer.Start();
            }
        }

        private static int GetStride(int width, System.Drawing.Imaging.PixelFormat pxFormat)
        {
            //float bitsPerPixel = System.Drawing.Image.GetPixelFormatSize(format);
            int bitsPerPixel = ((int)pxFormat >> 8) & 0xFF;
            //Number of bits used to store the image data per line (only the valid data)
            int validBitsPerLine = width * bitsPerPixel;
            //4 bytes for every int32 (32 bits)
            int stride = ((validBitsPerLine + 31) / 32) * 4;
            return stride;
        }


        private void OnTimerEvent(object sender, EventArgs e)
        {
            if( _handle == 0)
                return;

            int size = VideoCapture.ReadFrame(_handle, _frameBuffer);
            
            if( size == 0)
                return;

            System.Drawing.Imaging.PixelFormat pxFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            int stride = GetStride(_info.Width, pxFormat);
            GCHandle handle = GCHandle.Alloc(_frameBuffer, GCHandleType.Pinned);
            Bitmap bmp = new Bitmap(_info.Width, _info.Height, stride, pxFormat, handle.AddrOfPinnedObject());
            pictureView.Image = bmp;                        
            handle.Free();
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            _timer.Stop();
            Close();
        }

        private void OnOKClick(object sender, EventArgs e)
        {
            _timer.Stop();
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }


        protected override void  OnClosing(CancelEventArgs e)
        {            
             Cursor = Cursors.WaitCursor;
            
            if (_handle != 0)
                VideoCapture.Close(_handle);

            Cursor = Cursors.Default;
            base.OnClosing(e);
        }
    }
}
