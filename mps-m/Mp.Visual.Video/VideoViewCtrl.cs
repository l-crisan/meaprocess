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
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;

namespace Mp.Visual.Video
{
    public partial class VideoViewCtrl : UserControl
    {
        private System.Windows.Forms.Timer _updateTimer = new System.Windows.Forms.Timer();
        private int _width;
        private int _height;
        private HwPixelFormat _pixelFormat;
        private ImageLayout _imageLayout;
        private Bitmap _bitmap;
        private Mutex _mutex = new Mutex();
        private byte[] _frame;
        private byte[] _rgb888Buffer;

        public enum HwPixelFormat
        {            
            RGB565_16Bit,
            YCbCr422_16Bit,
            YOnly_8bit,
            RGB888_24Bit,
            YOnly_16bit
        }        

        public VideoViewCtrl()
        {
            InitializeComponent();
            DoubleBuffered = true;

            _updateTimer.Interval = 60;
            _updateTimer.Tick += new EventHandler(OnUpdateView);
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            _mutex.WaitOne();

            try
            {
                if (_frame != null)
                {
                    Frame2Bitmap(_frame);
                    this.BackgroundImage =  _bitmap;
                    Invalidate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            _mutex.ReleaseMutex();
        }
        
        private void YCbCr2RGB888(byte[] src, int width, int height,  byte[] dst)
        {
            int byteCount = width * height * 2;
            int destPos = 0;

            for (int pos = 0; pos < byteCount; pos += 4, destPos += 6)
            {                
                int cb1 = src[pos] - 128;
                int y1  = src[pos + 1] - 16;
                int cr1 = src[pos + 2] - 128;
                                
                int cb2 = cb1;
                int y2  = src[pos + 3] - 16;
                int cr2 = cr1;

                dst[destPos + 5] = (byte)(y2 + 1.402 * cr2);                     //R
                dst[destPos + 4] = (byte)(y2 - 0.344136 * cb2 - 0.714136 * cr2); //G
                dst[destPos + 3] = (byte)(y2 + 1.772 * cb2);                     //B

                dst[destPos + 2] = (byte)(y1 + 1.402 * cr1);                     //R
                dst[destPos + 1] = (byte)(y1 - 0.343 * cb1 - 0.714 * cr1);       //G
                dst[destPos]     = (byte)(y1 + 1.772 * cb1);                     //B
            }
        }
 
        private void Frame2Bitmap(byte[] frame)
        {            
            switch (_pixelFormat)
            {
                case HwPixelFormat.RGB888_24Bit:
                {
                    BitmapData data = _bitmap.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    System.Runtime.InteropServices.Marshal.Copy(frame, 0, data.Scan0, _width * _height * 3);
                    _bitmap.UnlockBits(data);
                }
                break;

                case HwPixelFormat.RGB565_16Bit:
                {
                    BitmapData data = _bitmap.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadWrite, PixelFormat.Format16bppRgb565);
                    System.Runtime.InteropServices.Marshal.Copy(frame, 0, data.Scan0, _width * _height * 2);
                    _bitmap.UnlockBits(data);
                }
                break;

                case HwPixelFormat.YCbCr422_16Bit:
                {
                    BitmapData data = _bitmap.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    YCbCr2RGB888(_frame, _width, _height, _rgb888Buffer);
                    System.Runtime.InteropServices.Marshal.Copy(_rgb888Buffer, 0, data.Scan0, _rgb888Buffer.Length);
                    _bitmap.UnlockBits(data);
                }
                break;

                case HwPixelFormat.YOnly_8bit:
                {
                    BitmapData data = _bitmap.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                    System.Runtime.InteropServices.Marshal.Copy(frame, 0, data.Scan0, _width * _height);
                    _bitmap.UnlockBits(data);
                }
                break;
            }
            
        }
        
        private ColorPalette GetGrayScalePalette()
        {
            Bitmap bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);

            ColorPalette monoPalette = bmp.Palette;

            Color[] entries = monoPalette.Entries;

            for (int i = 0; i < 256; i++)
                entries[i] = Color.FromArgb(i, i, i);

            return monoPalette;
        }

        public HwPixelFormat ImagePixelFormat
        {
            get { return _pixelFormat; }
            set { _pixelFormat = value; }
        }

        public int VideoWidth
        {
            set { _width = value; }
            get { return _width; }
        }

        public int VideoHeight
        {
            set { _height = value; }
            get { return _height; }
        }

        public void Start()
        {
            _bitmap = new Bitmap(_width, _height);
            int pixelSize = 1;

            switch (_pixelFormat)
            {
                case HwPixelFormat.RGB565_16Bit:
                    pixelSize = 2;
                break;

                case HwPixelFormat.YCbCr422_16Bit:
                    pixelSize = 2;
                    _rgb888Buffer = new byte[_width * _height * 3];   
                break;                

                case HwPixelFormat.RGB888_24Bit:
                    pixelSize = 3;
                break;
            }

            _frame = new byte[_width * _height * pixelSize];

            this.BackgroundImageLayout = _imageLayout;
            this.BackgroundImage = _bitmap;

            if (_pixelFormat == HwPixelFormat.YOnly_8bit)
                _bitmap.Palette = GetGrayScalePalette();

            _updateTimer.Start();
        }

        public void UpdateStream(byte[] data)
        {
            _mutex.WaitOne();

            try
            {
                Array.Copy(data, _frame, data.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _mutex.ReleaseMutex();
        }

        public void Stop()
        {
            _updateTimer.Stop();
        }

        public ImageLayout Stretch
        {
            get { return _imageLayout; }
            set { _imageLayout = value; }
        }

        private void OnStrech_Click(object sender, EventArgs e)
        {
            if (strechToolStripMenuItem.Checked)
                this.BackgroundImageLayout = ImageLayout.Stretch;
            else
                this.BackgroundImageLayout = ImageLayout.None;
        }

    }
}
