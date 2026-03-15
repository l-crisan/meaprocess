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
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Mp.Visual.FFT
{
    public partial class FFTViewCtrl : UserControl
    {
        private Plot3D _plot3D;
        private Size _zmapSize = new Size(1, 0);
        private double _zoom = 0.2;
        private List<double[]> _data = new List<double[]>();
        private int _timeWindowNumber = 100;
        private int _numberOfSamples = 1000;
        private int _samples = 0;
        private Mutex _mutex = new Mutex();

        private System.Windows.Forms.Timer _updateTimer = new System.Windows.Forms.Timer();


        public FFTViewCtrl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            toolStripComboBoxColor.SelectedIndex = 0;
            toolStripComboBoxResolution.SelectedIndex = 0;
            _updateTimer.Interval = 300;
            _updateTimer.Tick += new EventHandler(OnUpdateView);
            _updateTimer.Start();
        }

        /// <summary>
        /// Init the FFT view control
        /// </summary>
        /// <param name="noOfSamples">The FFT samples</param>
        /// <param name="timeWindow">The number of FFTs to show</param>
        public void Init()
        {
            _data = new List<double[]>();

            _plot3D = new Plot3D(trackBarObsX.Value, 0, trackBarObsZ.Value, 0, 0, ClientRectangle.Width, ClientRectangle.Height / 2, _zoom, 0, 0);
            _plot3D.PenColor = Color.FromArgb(123, 0, 0);
            _plot3D.DensityX = 5;
            _plot3D.DensityY = 5;
            _plot3D.StartPoint = new PointF(0, 0);
            _plot3D.EndPoint = new PointF(_timeWindowNumber, 300);
            _plot3D.ColorSchema = Color.Red;
            _plot3D.OnGetZValue += new GetZValueDelagate(GetZValue);
        }


        public uint NumberOfSamples
        {
            get { return (uint) _numberOfSamples; }
            set 
            { 
                _numberOfSamples = (int) value;
                _samples = _numberOfSamples / 2 + 1;
            }
        }

        public uint TimeWindowNumber
        {
            get { return (uint)_timeWindowNumber; }
            set { _timeWindowNumber = (int)value; }
        }

        private void OnUpdateView(object sender, EventArgs e)
        {
            if (_plot3D != null)
            {
                _plot3D.SetupTransCoeff(trackBarObsX.Value, 0, trackBarObsZ.Value, -400 + hScrollBar.Value, -400 + vScrollBar.Value, 800, 600, _zoom,0, 0);

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            if (_plot3D != null)
            {
                try
                {
                    _mutex.WaitOne();
                    _plot3D.RenderSurface(e.Graphics);
                    _mutex.ReleaseMutex();
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }

        public void Reset()
        {
            _data = new List<double[]>();
        }

        public void AddValues(double[] values)
        {
            _mutex.WaitOne();

            double[] data = new double[values.Length];
            
            Array.Copy(values, data, data.Length);

            _data.Add(data);

            if (_data.Count > _timeWindowNumber)
                _data.RemoveAt(0);

            _mutex.ReleaseMutex();
        }

        private double GetZValue(double y, double x)
        {
            int xPos = (int) (_samples / 300 * x/5);
            int yPos = (int)y;

            if (yPos < _data.Count && xPos < _samples)
                return _data[(int)yPos][(int)xPos] * 500;
            else
                return 0.0;
        }
    }
}
