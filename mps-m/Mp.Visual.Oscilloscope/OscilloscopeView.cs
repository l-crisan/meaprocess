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
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;

namespace Mp.Visual.Oscilloscope
{
    public partial class OscilloscopeView : UserControl
    {
        #region DataTypeDefinition
        private enum TriggerType
        {
            None = 0,
            RisingEdge,
            FallingEdge,
            Extern
        }

        private enum OsciMode
        {
            Ch2Mode,
            ChXYMode
        }
        #endregion

        #region Member        
        private double _axisDivision = 10.0;
        private Rectangle _drawRect = new Rectangle(0,0,100,100);
        private System.Windows.Forms.Timer _updateTimer = new System.Windows.Forms.Timer();
        private Mutex _mutex = new Mutex();
        private double _xScaleValue = 0.0;
        private bool _showLines = true;
        private int _osciMode = 0;
        private bool[] _chnSynch = new bool[4];

        private SignalRecordBuilder _xyRecordBuilder;
        private SignalRecordBuilder _xyTriggerRecordBuilder;
        private SignalRecordBuilder _ch1TriggerRecordBuilder;
        private SignalRecordBuilder _ch2TriggerRecordBuilder;

        private Queue<PointF> _xyQueue = new Queue<PointF>();
        private PointF[] _xyValues;
        private int _meaUpdateCounter = 0;
        private bool _freezed = false;
        private Color _gridColor = Color.Gray;
        private double _xmax = 0;
        private bool _doSingleShot = false;

        private ChannelControl[] _channels = new ChannelControl[2];
        #endregion

        #region Properties

        public Mutex SynchMutex
        {
            get{ return _mutex;}
        }

        [SRCategory("Display"), SRDescription("GridColor")]
        public Color GridColor
        {
            set { _gridColor = value; }
            get { return _gridColor; }
        }

        [SRCategory("Display"), SRDescription("Ch1Color")]
        public Color Ch1Color
        {
            set 
            { 
                _channels[0].ChColor = value;
                legendCh1.ForeColor = value;
                InitMeasurementPanel();
            }
            get 
            { 
                return _channels[0].ChColor; 
            }
        }

        [SRCategory("Display"), SRDescription("Ch2Color")]
        public Color Ch2Color
        {
            set 
            { 
                _channels[1].ChColor = value;
                InitMeasurementPanel();
            }

            get 
            { 
                return _channels[1].ChColor; 
            }
        }

        [SRCategory("Display"), SRDescription("ChViewColor")]
        public Color ChViewColor
        {
            set 
            { 
                view.BackColor = value;
                InitMeasurementPanel();
            }
            get 
            { 
                return view.BackColor; 
            }
        }
        #endregion

        public OscilloscopeView()
        {
            InitializeComponent();
            
            for (int i = 0; i < _chnSynch.Length; ++i)
                _chnSynch[i] = true;

            _xyRecordBuilder = new SignalRecordBuilder(2);
            _xyRecordBuilder.OnNewValue += new NewValueDelegate(OnXYValue);

            _ch1TriggerRecordBuilder = new SignalRecordBuilder(2);
            _ch1TriggerRecordBuilder.OnNewValue += new NewValueDelegate(OnCh1AndTriggerValue);

            _ch2TriggerRecordBuilder = new SignalRecordBuilder(2);
            _ch2TriggerRecordBuilder.OnNewValue += new NewValueDelegate(OnCh2AndTriggerValue);

            _xyTriggerRecordBuilder = new SignalRecordBuilder(3);
            _xyTriggerRecordBuilder.OnNewValue += new NewValueDelegate(OnXYAndTriggerValue);


            for (int i = 0; i < _channels.Length; ++i)
            {
                _channels[i] = new ChannelControl();
                if( i == 0)
                    _channels[i].Dock = DockStyle.Top;
                else
                    _channels[i].Dock = DockStyle.Fill;            

                _channels[i].OnTriggerChanged += new TriggerChangedDelegate(OnChTriggerChanged);
                _channels[i].Index = i;
                _channels[i].SyncMutex = _mutex;
                panelChControls.Controls.Add(_channels[i]);

                _channels[i].BackBuffer = new Bitmap(_drawRect.Width * 3, _drawRect.Height * 3);
                _channels[i].DrawRect = _drawRect;
                _channels[i].SampleRate = 0;
            }
                        
            _channels[1].BringToFront();

            _channels[0].ChColor = Color.White;
            _channels[1].ChColor = Color.Yellow;

            scaleXUnit.SelectedIndex = 0;
            display.SelectedIndex = 0;
            osciModeCtrl.SelectedIndex = 0;
            scaleX.Text = Convert.ToString(0.1);
            InitSizes();
            _updateTimer.Interval = 100;
            _updateTimer.Tick += new EventHandler(OnUpdate);
            Invalidate();
        }


        private void InitMeasurementPanel()
        {
            meaPanel.BackColor = view.BackColor;
            legendCh1.ForeColor = _channels[0].ChColor;
            ch1FreqCtrl.ForeColor = _channels[0].ChColor;
            ch1FreqLabel.ForeColor = _channels[0].ChColor;
            ch1FreqCtrlUnit.ForeColor = _channels[0].ChColor;
            ch1Volt.ForeColor = _channels[0].ChColor;
            ch1VoltLabel.ForeColor = _channels[0].ChColor;
            ch1VoltUnit.ForeColor = _channels[0].ChColor;

            legendCh2.ForeColor = _channels[1].ChColor;
            ch2FreqCtrl.ForeColor = _channels[1].ChColor;
            ch2FreqLabel.ForeColor = _channels[1].ChColor;
            ch2FreqCtrlUnit.ForeColor = _channels[1].ChColor;
            ch2Volt.ForeColor = _channels[1].ChColor;
            ch2VoltLabel.ForeColor = _channels[1].ChColor;
            ch2VoltUnit.ForeColor = _channels[1].ChColor;
        }

        private void OnChTriggerChanged()
        {
            _mutex.WaitOne();
            SynchChannels();
            _mutex.ReleaseMutex();
        }

        public void Stop()
        {
            _updateTimer.Stop();
        }

        public void Start()
        {
            InitMeasurementPanel();

            double[] rates = new double[2];
            rates[0] = _channels[0].SampleRate;
            rates[1] = _channels[1].SampleRate;

            _xyRecordBuilder.Start(rates);
            rates[0] = _channels[0].SampleRate;
            rates[1] = _channels[0].TriggerSampleRate;
            legendCh1.Text = _channels[0].ChName;

            _ch1TriggerRecordBuilder.Start(rates);

            rates[0] = _channels[1].SampleRate;
            rates[1] = _channels[1].TriggerSampleRate;
            
            if(_channels[1].ChName != "")
                legendCh2.Text = _channels[1].ChName;

            _ch2TriggerRecordBuilder.Start(rates);

            rates = new double[3];
            rates[0] = _channels[0].SampleRate;
            rates[1] = _channels[1].SampleRate;
            rates[2] = _channels[0].TriggerSampleRate;

            _xyTriggerRecordBuilder.Start(rates);
    
            foreach (ChannelControl chn in _channels)
            {
                chn.MinValue = Double.MaxValue;
                chn.MaxValue = Double.MinValue;
                chn.CalcMinValue = Double.MaxValue;
                chn.CalcMaxValue = Double.MinValue;
                chn.PeriodeCounter = 0;
                chn.CopyRecordFlag = true;
            }

            if (_channels[1].SampleRate == 0.0)
            {
                osciModeCtrl.SelectedIndex = 0;
                osciModeCtrl.Enabled = false;
                _channels[1].ChnVisible = false;
            }
            else
            {
                _channels[1].ChnVisible = true;
            }            

            OnUpdateXScaleClick(null, null);
            
            _updateTimer.Start();
        }

     
        private void OnUpdate(Object sender, EventArgs e)
        {
            foreach (ChannelControl chn in _channels)
                chn.CopyRecordFlag = true;

            view.Invalidate();
        }

        private void InitSizes()
        {
            if (view.ClientRectangle.Width < 1 || view.ClientRectangle.Height < 1)
                return;

            _mutex.WaitOne();

            foreach (ChannelControl chn in _channels)
            {
                _drawRect = new Rectangle(view.ClientRectangle.X + 30, view.ClientRectangle.Y + 30, view.ClientRectangle.Width - 60, view.ClientRectangle.Height - 60);

                if (_drawRect.Height % 2 != 0)
                    _drawRect.Height++;

                if (_drawRect.Width % 2 != 0)
                    _drawRect.Width++;

                chn.DrawRect = _drawRect;
                chn.BackBuffer = new Bitmap(_drawRect.Width * 3, _drawRect.Height * 3);                
                chn.InitSizes();
            }

            legendCh2.ForeColor = _channels[1].ChColor;
            legendCh1.ForeColor = _channels[0].ChColor;

            _mutex.ReleaseMutex();
            Invalidate();
        }

        private int ValueXToPixel(double x, ChannelControl chn)
        {
            return (int)(x * (chn.BackBuffer.Width) / _xmax);
        }    

        private double PixelXToValue(int x, double resolution, ChannelControl chn)
        {
            double xmin = 0;
            double xmax = _axisDivision * resolution * 3;

            double factor = (xmax - xmin) / (double)(chn.BackBuffer.Width);

            double offset = ((double)(xmin * chn.BackBuffer.Width) - (double)(xmax * 0)) / (chn.BackBuffer.Width);

            return (double)(x * factor + offset);
        }

        public double Chn1TriggerSampleRate
        {
            set
            {
                _channels[0].TriggerSampleRate = value;
            }

            get { return _channels[0].TriggerSampleRate; }
        }


        public string Ch1Name
        {
            set { _channels[0].ChName = value; }
        }

        public string Ch2Name
        {
            set { _channels[1].ChName = value; }
        }

        public double Chn2TriggerSampleRate
        {
            set
            {
                _channels[1].TriggerSampleRate = value;
            }

            get { return _channels[1].TriggerSampleRate; }
        }

        public double Chn1SampleRate
        {
            set 
            {                 
                _channels[0].SampleRate = value;
                _channels[0].ChnVisible = true;
                UpdateValueArray(_channels[0]);
            }

            get { return _channels[0].SampleRate; }
        }
        
        public double Chn2SampleRate
        {
            set 
            {
                _channels[1].SampleRate = value;
                _channels[0].ChnVisible = true;
                UpdateValueArray(_channels[1]);
            }

            get
            {
                return _channels[1].SampleRate; 
            }
        }

        public void AddTriggerValue(int chnIdx, double value, bool lastSample)
        {
            ChannelControl chn = _channels[chnIdx];

            if (_osciMode == (int)OsciMode.ChXYMode)
            {
                if (chnIdx == 0)
                {
                    if (_chnSynch[0] && _chnSynch[1] && _chnSynch[2])
                       _xyTriggerRecordBuilder.AddValue(chnIdx + 2, value);
                }
            }
            else
            {
                if (chnIdx == 0)
                {
                    if (_chnSynch[0] && _chnSynch[2])
                        _ch1TriggerRecordBuilder.AddValue(1, value);
                }

                if (chnIdx == 1)
                {
                    if (_chnSynch[1] && _chnSynch[3])
                     _ch2TriggerRecordBuilder.AddValue(1, value);
                }
            }

            if( lastSample)
                _chnSynch[chnIdx + 2] = true;
        }
        
        private void OnCh1AndTriggerValue(double[] values)
        {            
            InsertChannelValue(values[0], _channels[0], values[1]);
        }

        private void OnCh2AndTriggerValue(double[] values)
        {
            InsertChannelValue(values[0], _channels[1], values[1]);
        }

        private void OnXYValue(double[] values)
        {
            InsertXYValue(values[0], values[1], 0);
        }

        private void InsertChannelValue(double value, ChannelControl chn, double extTriggerValue)
        {            
            chn.DataQueue.Enqueue(value);            
            
            chn.PeriodeCounter++;
            chn.SampleCounter++;
            chn.CalcMinValue = Math.Min(chn.CalcMinValue, value);
            chn.CalcMaxValue = Math.Max(chn.CalcMaxValue, value);
            
            if (chn.DataQueue.Count < chn.Data.Length)
                return;

            TriggerChannel(chn, value, extTriggerValue);

            chn.LastValue = value;
            chn.LastTriggerValue = extTriggerValue;

            if (chn.TriggerPos == (chn.Data.Length / 2 + 1))
            {
                if (!_freezed && (chn.CopyRecordFlag || chn.SingleShot))
                {
                    chn.DataQueue.CopyTo(chn.Data, 0);
                    if (chn.SingleShot)
                    {
                        _freezed = true;
                        chn.SingleShot = false;
                        _doSingleShot = true;
                    }
                    chn.CopyRecordFlag = false;
                }
                chn.SampleCounter = 0;
                chn.TriggerPos = -1;
            }
            else
            {
                if (chn.SampleCounter > (chn.Data.Length * 2) && chn.TriggerPos == -1 && !_freezed && chn.CopyRecordFlag)
                {
                    chn.DataQueue.CopyTo(chn.Data, 0);
                    chn.SampleCounter = 0;
                    chn.CopyRecordFlag = false;
                }
            }

            chn.DataQueue.Dequeue();

            if(chn.TriggerPos != -1)
                chn.TriggerPos--;
        }

        private void InsertXYValue(double xval, double yval, double extTriggerValue)
        {
            ChannelControl chn1 = _channels[0];
            ChannelControl chn2 = _channels[1];

            chn1.PeriodeCounter++;
            chn1.SampleCounter++;
            chn1.CalcMinValue = Math.Min(chn1.CalcMinValue, xval);
            chn1.CalcMaxValue = Math.Max(chn1.CalcMaxValue, xval);
            
            chn2.PeriodeCounter++;
            chn2.CalcMinValue = Math.Min(chn2.CalcMinValue, yval);
            chn2.CalcMaxValue = Math.Max(chn2.CalcMaxValue, yval);

            PointF p = new PointF((float)xval, (float)yval);
            chn1.DataQueue.Enqueue(xval);
            _xyQueue.Enqueue(p);

            if (_xyQueue.Count < _xyValues.Length)
                return;

            TriggerChannel(chn1, xval, extTriggerValue);
            TriggerChannel(chn2, yval, extTriggerValue);

            chn1.LastValue = xval;
            chn1.LastTriggerValue = extTriggerValue;

            if (chn1.TriggerPos == (_xyValues.Length / 2 + 1))
            {
                if (!_freezed && chn1.CopyRecordFlag)
                {
                    _xyQueue.CopyTo(_xyValues, 0);
                    if (chn1.SingleShot)
                    {
                        _freezed = true;
                        chn1.SingleShot = false;
                        _doSingleShot = true;
                    }
                    chn1.CopyRecordFlag = false;
                }
                chn1.TriggerPos = -1;
                chn1.SampleCounter = 0;
            }
            else
            {
                if (chn1.SampleCounter > (_xyValues.Length * 2) && chn1.TriggerPos == -1 && !_freezed && chn1.CopyRecordFlag)
                {
                    _xyQueue.CopyTo(_xyValues, 0);
                    chn1.SampleCounter = 0;
                    chn1.CopyRecordFlag = false;
                }
            }

            _xyQueue.Dequeue();
            chn1.DataQueue.Dequeue();

            if (chn1.TriggerPos != -1)
                chn1.TriggerPos--;
        }

        public void OnXYAndTriggerValue(double[] values)
        {
            InsertXYValue(values[0], values[1], values[2]);
        }

        public void AddValue(int chnIdx, double value, bool lastSample)
        {            
            ChannelControl chn = _channels[chnIdx];
            
            if (_osciMode == (int)OsciMode.Ch2Mode)
            {//2 channel mode                    

                if (chn.ChTriggerType == ChannelControl.TriggerType.ExternOnOff ||
                    chn.ChTriggerType == ChannelControl.TriggerType.ExternRisingEdge ||
                    chn.ChTriggerType == ChannelControl.TriggerType.ExternFallingEdge)
                {//External trigger
                    if (chnIdx == 0)
                    {
                        if (_chnSynch[0] && _chnSynch[2])
                           _ch1TriggerRecordBuilder.AddValue(0, value);
                    }
                    else
                    {
                        if (_chnSynch[1] && _chnSynch[3])
                            _ch2TriggerRecordBuilder.AddValue(0, value);
                    }
                }
                else
                {//Internal trigger
                    InsertChannelValue(value, chn, 0);
                }
            }
            else
            {//X/Y mode
                ChannelControl chn1 = _channels[0];

                if (chn1.ChTriggerType == ChannelControl.TriggerType.ExternOnOff||
                    chn1.ChTriggerType == ChannelControl.TriggerType.ExternRisingEdge ||
                    chn1.ChTriggerType == ChannelControl.TriggerType.ExternFallingEdge)
                {//External Trigger
                    if (_chnSynch[0] && _chnSynch[1] && _chnSynch[2])
                        _xyTriggerRecordBuilder.AddValue(chnIdx, value);
                }
                else
                {//Internal trigger
                    if (_chnSynch[0] && _chnSynch[1])
                       _xyRecordBuilder.AddValue(chnIdx, value);
                }
            }   
         
            if( lastSample)
                _chnSynch[chnIdx] = true;
        }    

        private void TriggerChannel(ChannelControl chn, double value, double extTriggerValue )
        {
            double triggerLevel = chn.TriggerLevelValue;

            if (chn.InvertChn)
                triggerLevel *= -1;

            if (chn.Coupling == 0)
                triggerLevel +=  chn.Average;
                
            switch (chn.ChTriggerType)
            {
                case ChannelControl.TriggerType.RisingEdge:
                {
                    if ((chn.LastValue < triggerLevel) && (value >= triggerLevel))
                        DoTrigger(chn);
                }
                break;

                case ChannelControl.TriggerType.FallingEdge:
                {
                    if ((chn.LastValue >= triggerLevel) && (value < triggerLevel))
                        DoTrigger(chn);
                }
                break;

                case ChannelControl.TriggerType.ExternOnOff:
                {
                    if (extTriggerValue != 0)
                        DoTrigger(chn);
                }
                break;
                case ChannelControl.TriggerType.ExternRisingEdge:
                {
                    if ((chn.LastTriggerValue < triggerLevel) && (extTriggerValue >= triggerLevel))
                        DoTrigger(chn);
                }
                break;

                case ChannelControl.TriggerType.ExternFallingEdge:
                {
                    if ((chn.LastTriggerValue > triggerLevel) && (extTriggerValue <= triggerLevel))
                        DoTrigger(chn);
                }
                break;
            }
        }

        private void DoTrigger(ChannelControl chn)
        {
            chn.MinValue = chn.CalcMinValue;
            chn.MaxValue = chn.CalcMaxValue;
            chn.CalcMinValue = double.MaxValue;
            chn.CalcMaxValue = double.MinValue;
            chn.Periode = chn.PeriodeCounter / chn.SampleRate;
            chn.PeriodeCounter = 0;
            
            if(chn.TriggerPos == -1)
                chn.TriggerPos = chn.DataQueue.Count - 1;
        }

        private void OnViewPaint(object sender, PaintEventArgs e)
        {            
            Graphics g = e.Graphics;
            bool localDoSingleShot = false;

            _mutex.WaitOne();
            if (_doSingleShot)
            {
                freeze.Checked = _freezed;
                foreach (ChannelControl chn in _channels)
                    chn.ResetSingleShot();

                _doSingleShot = false;
                localDoSingleShot = true;
            }
            _mutex.ReleaseMutex();

            DrawGrid(g);

            foreach (ChannelControl chnCtrl in _channels)
            {
                if (chnCtrl.ChnVisible)
                {
                    DrawTriggerLevel(g, chnCtrl);
                    DrawChannelMarker(g, chnCtrl);
                    DrawHorizontalMarker(g, chnCtrl);

                    if (_osciMode == 0 && chnCtrl.SampleRate != 0)
                        DrawChannelValues(g, chnCtrl);
                }

                if (_meaUpdateCounter % 5 == 0)
                {
                    if (chnCtrl.SampleRate != 0 && (!_freezed || localDoSingleShot))
                        UpdateMeasurement(chnCtrl);
                }
            }

            if (_osciMode == 1)
            {
                DrawXYChannels(g);
            }

            if (_meaUpdateCounter % 5 == 0)
                _meaUpdateCounter = 0;
            
            _meaUpdateCounter++;
                      
        }

        private void UpdateMeasurement(ChannelControl chn)
        {
            double freq = 0.0;
            string  unit = "Hz";
            if (chn.Periode != 0)
            {
                freq  = 1.0 / chn.Periode;
                unit = "(Hz)";

                if (freq > 999)
                {
                    unit = "(kHz)";
                    freq /= 1000;
                }

                if (freq > 999)
                {
                    unit = "(MHz)";
                    freq /= 1000;
                }

                if (freq > 999)
                {
                    unit = "(GHz)";
                    freq /= 1000;
                }

                if (chn.Index == 0)
                {
                    ch1FreqCtrl.Text = String.Format("{0:0.00}", freq);
                    ch1FreqCtrlUnit.Text = unit;
                }
                else
                {
                    ch2FreqCtrl.Text = String.Format("{0:0.00}", freq);
                    ch2FreqCtrlUnit.Text = unit;
                }
            }

            if (chn.MinValue < chn.MaxValue)
            {
                double volt = Math.Abs(chn.MaxValue - chn.MinValue);
                unit = "(V)";
                if (volt < 0.1)
                {
                    volt *= 1000;
                    unit = "(mV)";
                }
                else if (volt > 500)
                {
                    unit = "(kV)";
                    volt /=1000.0;
                }

                if (chn.Index == 0)
                {
                    ch1Volt.Text = String.Format("{0:0.000}", volt);
                    ch1VoltUnit.Text = unit;
                }
                else
                {
                    ch2Volt.Text = String.Format("{0:0.000}", volt);
                    ch2VoltUnit.Text = unit;
                }
            }
        }

        private void DrawXYChannels(Graphics g)
        {
            if (_xyValues.Length == 0)
                return;

            ChannelControl chn1 = _channels[0];

            if (!chn1.ChnVisible)
                return;                    

            ChannelControl chn2 = _channels[1];

            Graphics backBuffer = Graphics.FromImage(chn1.BackBuffer);
            backBuffer.Clear(view.BackColor);

            Pen pen = new Pen(chn1.ChColor);
            pen.Width = 1;

            chn1.Average = 0;
            chn2.Average = 0;

            if (chn1.Coupling == 0 || chn2.Coupling == 0)
            {                
                foreach (PointF p in _xyValues)
                {
                    if( chn1.Coupling == 0)
                        chn1.Average += p.X;

                    if (chn2.Coupling == 0)
                        chn2.Average += p.Y;
                }

                chn1.Average /= _xyValues.Length;
                chn2.Average /= _xyValues.Length;
            }

            Point[] points = new Point[_xyValues.Length];

            for (int i = 0; i < _xyValues.Length; ++i)
            {                    
                PointF vp1 = _xyValues[i];

                vp1.X -= (float)chn1.Average;
                vp1.Y -= (float)chn2.Average;

                if (chn1.Coupling == 2)
                    vp1.X = 0;

                if (chn1.InvertChn)
                    vp1.X *= -1;

                if (chn2.Coupling == 2)
                    vp1.Y = 0;

                if (chn2.InvertChn)
                    vp1.Y *= -1;

                points[i].X = chn1.ValueYToPixel(vp1.X) + _drawRect.Width / 2;
                points[i].Y = chn2.ValueYToPixel(vp1.Y);
            }

            try
            {
                if (_showLines)
                {
                    backBuffer.DrawLines(pen, points);
                }
                else
                {
                    foreach (Point p in points)
                        backBuffer.DrawRectangle(pen, new Rectangle(p.X, p.Y, 1, 1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            ImageAttributes attr = new ImageAttributes();
            attr.SetColorKey(view.BackColor, view.BackColor);

            g.DrawImage(chn1.BackBuffer, _drawRect, _drawRect.Width + chn1.HorizontalPosValue, _drawRect.Height + chn1.VerticalPosValue, _drawRect.Width, _drawRect.Height, GraphicsUnit.Pixel, attr);
        }

        private void DrawChannelValues(Graphics g, ChannelControl chn)
        {
            _mutex.WaitOne();

            if (chn.Data.Length == 0)
            {
                _mutex.ReleaseMutex();
                return;
            }

            Graphics bkGraphics = Graphics.FromImage(chn.BackBuffer);
            bkGraphics.Clear(view.BackColor);            

            Pen pen = new Pen(chn.ChColor);

            chn.Average = 0;
            
            if (chn.Coupling == 0)
            {                
                foreach (double value in chn.Data)
                    chn.Average += value;

                chn.Average /= chn.Data.Length;
            }

            Point[] points = new Point[chn.Data.Length];
            double yvalue = 0;

            for (int i = 0; i < chn.Data.Length; ++i)
            {
                points[i].X = ValueXToPixel(i * (1.0 / chn.SampleRate), chn);

                if (chn.Coupling == 2)
                    yvalue = 0;
                else
                    yvalue = chn.Data[i] - chn.Average;

                if (chn.InvertChn)
                    yvalue *= -1;

                points[i].Y = chn.ValueYToPixel(yvalue);                
            }
            _mutex.ReleaseMutex();

            try
            {
                if (_showLines)
                {
                    bkGraphics.DrawLines(pen, points);
                }
                else
                {
                    foreach (Point p in points)
                        bkGraphics.DrawRectangle(pen, new Rectangle(p.X, p.Y, 1, 1));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            ImageAttributes attr = new ImageAttributes();
            attr.SetColorKey(view.BackColor, view.BackColor);
            g.DrawImage(chn.BackBuffer, _drawRect, _drawRect.Width + chn.HorizontalPosValue, _drawRect.Height + chn.VerticalPosValue, _drawRect.Width, _drawRect.Height, GraphicsUnit.Pixel, attr);            
        }

        private void DrawHorizontalMarker(Graphics g, ChannelControl chn)
        {
            Point[] arrow = new Point[3];
            arrow[0] = new Point(_drawRect.Left + (int)chn.RawHorPosValue, _drawRect.Bottom + 2);
            arrow[1] = new Point(_drawRect.Left + (int)chn.RawHorPosValue- 5, _drawRect.Bottom + 8);
            arrow[2] = new Point(_drawRect.Left + (int)chn.RawHorPosValue+5, _drawRect.Bottom + 8);
            SolidBrush brush = new SolidBrush(chn.ChColor);
            g.FillPolygon(brush, arrow);

            Font font = new System.Drawing.Font("Microsoft Sans Serif", 7, FontStyle.Regular);

            SizeF size = g.MeasureString("X-pos" + (chn.Index + 1).ToString(), font);
            Point pos = new Point((_drawRect.Left + (int)chn.RawHorPosValue - (int)size.Width / 2), (int)_drawRect.Bottom + 10);

            g.DrawString("X-pos" + (chn.Index + 1).ToString(), font, new SolidBrush(chn.ChColor), pos);

        }

        private void DrawChannelMarker(Graphics g, ChannelControl chn)
        {
            Point[] arrow = new Point[3];
            arrow[0] = new Point(_drawRect.Left - 2, (int)(_drawRect.Bottom - chn.RawVerPos));
            arrow[1] = new Point(_drawRect.Left - 7, (int)(_drawRect.Bottom - chn.RawVerPos - 5));
            arrow[2] = new Point(_drawRect.Left - 7, (int)(_drawRect.Bottom - chn.RawVerPos + 5));
            SolidBrush brush = new SolidBrush(chn.ChColor);

            Font font = new System.Drawing.Font("Microsoft Sans Serif", 7, FontStyle.Regular);

            SizeF size = g.MeasureString("Ch" + (chn.Index +1), font);

            g.DrawString("Ch" + (chn.Index + 1), font, new SolidBrush(chn.ChColor), new Point((int)(_drawRect.Left - size.Width - 11), (int)(_drawRect.Bottom - chn.RawVerPos - size.Height / 2)));

            g.FillPolygon(brush, arrow);
        }

        private void DrawTriggerLevel(Graphics g, ChannelControl chn)
        {
            Point[] arrow = new Point[3];
            arrow[0] = new Point(_drawRect.Right + 2, (int)(_drawRect.Bottom - chn.RawTriggerLevelValue));
            arrow[1] = new Point(_drawRect.Right + 8, (int)(_drawRect.Bottom - chn.RawTriggerLevelValue - 5));
            arrow[2] = new Point(_drawRect.Right + 8, (int)(_drawRect.Bottom - chn.RawTriggerLevelValue + 5));

            Font font = new System.Drawing.Font("Microsoft Sans Serif", 7, FontStyle.Regular);

            SizeF size = g.MeasureString("Tr" + (chn.Index +1).ToString(), font);

            g.DrawString("Tr" + (chn.Index + 1).ToString(), font, new SolidBrush(chn.ChColor), new Point(_drawRect.Right + 11, (int)(_drawRect.Bottom - chn.RawTriggerLevelValue - size.Height / 2)));

            SolidBrush brush = new SolidBrush(chn.ChColor);
            g.FillPolygon(brush, arrow);
        }

        private void DrawGrid(Graphics g)
        {
            double xPixInc = _drawRect.Width / _axisDivision;
            double pos = _drawRect.Left;
            Pen pen = new Pen(_gridColor);

            pen.Width = 1;
            pen.DashStyle =  System.Drawing.Drawing2D.DashStyle.DashDot;

            for (int x = 0; x <= _axisDivision; x++)
            {
                if (x == _axisDivision / 2)
                {
                    pen.Width = 2;
                }
                else
                {
                    pen.Width = 1;
                }

                g.DrawLine(pen, new Point((int)pos, _drawRect.Top), new Point((int)pos, (int)_drawRect.Bottom));
                pos += xPixInc;
            }

            double yPixInc = _drawRect.Height / _axisDivision;
            pos = _drawRect.Top;

            for (int y = 0; y <= _axisDivision; y++)
            {
                if (y == _axisDivision / 2)
                {
                    pen.Width = 2;
                }
                else
                {
                    pen.Width = 1;
                }

                g.DrawLine(pen, new Point(_drawRect.Left, (int)(pos)), new Point(_drawRect.Right, (int)(pos)));
                pos += yPixInc;
            }
        }

        private void view_Resize(object sender, EventArgs e)
        {
            InitSizes();
        }

        private void OnAutoClick(object sender, EventArgs e)
        {
            double time = 0;
            
            _mutex.WaitOne();
            double periode = Math.Max(_channels[0].Periode, _channels[1].Periode);
            time = periode * 1000 / _axisDivision;

            if (time == 0.0)
                time = 1;

            _mutex.ReleaseMutex();

            int unitIdx = 0;
            int oldUnit = scaleXUnit.SelectedIndex;
            string oldTime = scaleX.Text;                
            
            if (time > 999 && time < 1000000)
            {
                time /= 1000;
                scaleX.Text = String.Format("{0:0.000}", time);
                unitIdx = 1;
            }
            else if (time > 999999)
            {
                time /= 1000000;
                scaleX.Text = String.Format("{0:0.000}", time);
                unitIdx = 2;
            }
            else
            {
                scaleX.Text = String.Format("{0:0.000}", time);
                unitIdx = 0;
            }

            scaleXUnit.SelectedIndex = unitIdx;

            if (!CheckXResolution())
            {
                errorProvider.Clear();
                scaleX.Text = oldTime;
                scaleXUnit.SelectedIndex = oldUnit;
                return;
            }

            OnUpdateXScaleClick(null, null);

            _channels[0].InvertChn = false;
            _channels[1].InvertChn = false;
            
            _mutex.WaitOne();
            if (_channels[0].MinValue < _channels[0].MaxValue)
                _channels[0].ScaleY = String.Format("{0:0.0000}",((_channels[0].MaxValue - _channels[0].MinValue) / _axisDivision));

            if (_channels[1].MinValue < _channels[1].MaxValue)
                _channels[1].ScaleY = String.Format("{0:0.0000}",((_channels[1].MaxValue - _channels[1].MinValue) / _axisDivision));

            _mutex.ReleaseMutex();        
        }     

        private void CalcScaleX()
        {
            _xScaleValue = Convert.ToDouble(scaleX.Text);
           
            switch(scaleXUnit.SelectedIndex)
            {
                case 0:
                    _xScaleValue = _xScaleValue / 1000.0;
                break;
                case 1:
                    _xScaleValue = _xScaleValue / 1000000.0;
                break;
                case 2:
                    _xScaleValue = _xScaleValue / 1000000000.0;
                break;
            }            
        }

        private void UpdateValueArray(ChannelControl chn)
        {
            _mutex.WaitOne();
            CalcScaleX();
            SynchChannels();

            double dt = _xScaleValue * _axisDivision;

            int samples = (int)Math.Ceiling(dt * chn.SampleRate) * 3;

            if (samples % 2 != 1)
                samples++;

            chn.Data = new double[samples];

            if (chn.Index == 0)
                _xyValues = new PointF[samples];

            _mutex.ReleaseMutex();
        }
   
        private bool CheckXResolution()
        {
            try
            {
                double value = Convert.ToDouble(scaleX.Text);
                if( value == 0.0)
                {
                    errorProvider.SetError(updateXScale, StringResource.PeriodeErr);
                    return false;
                }

                double maxFreq = Math.Max(_channels[0].SampleRate, _channels[1].SampleRate);

                switch( scaleXUnit.SelectedIndex)
                {
                    case 0://ms
                        value /= 1000;
                    break;
                    case 1://µs
                        value /= 1000000;
                    break;
                    case 2://ns
                        value /= 1000000000;
                    break;
                }

                double periode = _axisDivision * value;
                long samples = (long) (maxFreq * periode * 3);                
                if (samples > 10000)
                {
                    errorProvider.SetError(updateXScale, StringResource.PeriodeToBigErr);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(updateXScale, ex.Message);
                return false;
            }
            return true;
        }

        private void scaleXUnit_Validated(object sender, EventArgs e)
        {
            errorProvider.Clear();
        }

        private void scaleYChn_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            Control ctrl = (Control) sender;
            try
            {
                double scale = Convert.ToDouble(ctrl.Text);
                if (scale == 0.0)
                {
                    errorProvider.SetError(ctrl, StringResource.ZeroValueErr);
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                errorProvider.SetError(ctrl, ex.Message);
                e.Cancel = true;
            }
        }

        private void osciMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _mutex.WaitOne();

            SynchChannels();

            _osciMode = osciModeCtrl.SelectedIndex;
            
            if (_osciMode == 1)
                _channels[1].SetXYMode();                
            else
                _channels[1].ResetXYMode();

            _mutex.ReleaseMutex();
        }

        private void SynchChannels()
        {
            for (int i = 0; i < _chnSynch.Length; ++i)
                _chnSynch[i] = false;

            _xyTriggerRecordBuilder.Reset();
            _ch1TriggerRecordBuilder.Reset();
            _ch2TriggerRecordBuilder.Reset();
            _xyRecordBuilder.Reset();

            foreach (ChannelControl chn in _channels)
            {
                chn.DataQueue.Clear();
                chn.PeriodeCounter = 0;
                chn.SampleCounter = 0;
            }

            _xyQueue.Clear();
        }

        private void display_SelectedIndexChanged(object sender, EventArgs e)
        {
            _showLines = display.SelectedIndex == 0;
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertyDlg dlg = new PropertyDlg(this);
            dlg.ShowDialog();
        }

        private void freeze_CheckedChanged(object sender, EventArgs e)
        {
            _freezed = freeze.Checked;
        }

        private void copyImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bitmap = new Bitmap(view.ClientRectangle.Width, view.ClientRectangle.Height);
            view.DrawToBitmap(bitmap, new Rectangle(0, 0, view.ClientRectangle.Width, view.ClientRectangle.Height));
            Clipboard.SetData("System.Drawing.Bitmap", bitmap);
        }

        private void view_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            propertiesToolStripMenuItem_Click(null, null);
        }

        private void OnUpdateXScaleClick(object sender, EventArgs e)
        {
            if (!CheckXResolution())
            {
                scaleXUnit.SelectedIndex++;
                return;
            }

            foreach( ChannelControl chn in _channels)
                UpdateValueArray(chn);
            
            _xmax = _axisDivision * _xScaleValue * 3;
        }
    }
}
