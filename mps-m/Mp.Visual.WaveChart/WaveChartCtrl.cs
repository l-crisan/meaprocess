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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Drawing.Imaging;

namespace Mp.Visual.WaveChart
{
    internal delegate void DelegateOnPaintBackground(PaintEventArgs e);
    internal delegate void RefreshDelegate();

    /// <summary>
	/// A wave chart control.
	/// </summary>	
	public class WaveChartCtrl : System.Windows.Forms.UserControl
    {
      
        private IContainer components;
        private Graphics _graphics;
        private bool _sync;
        private List<InternalSignal> _signals = new List<InternalSignal>();
        private bool _showGrid = true;
        private DashStyle _gridLineStyle = DashStyle.Dash;
        private int _margin = 15;
        private Rectangle _drawRect = new Rectangle();
        private uint _gridXDivision = 10;
        private uint _gridYDivision = 5;
        private Color _gridLineColor = Color.Gray;
        private uint _timeSlot = 10;
        private int _gridLineWidth = 1;
        private TimeAxis _timeAxis;
        private SigLegend _legend;
        private YAxis _YAxis;
        private ToolStripMenuItem freezeToolStripMenuItem;
        private bool _freeze = false;
        private double _scroll = 1;
        private bool _autoScaleY = false;
        private ToolStripMenuItem scroll25ToolStripMenuItem;
        private ToolStripMenuItem scroll50ToolStripMenuItem;
        private ToolStripMenuItem scroll100ToolStripMenuItem;
        private ToolStripMenuItem autoScaleYToolStripMenuItem;
        private ulong _chartMaxSampleRate = 50000;
        private List<Bitmap> _backBuffers = new List<Bitmap>();
        private Bitmap _secondBuffer;
        private Mutex _mutex = new Mutex();
        private List<List<InternalSignal>> _signalsForBuffer = new List<List<InternalSignal>>();
        private List<int> _signalToUpdate = new List<int>();
        private int _bufferCount = 4;
        private int _eventCounter = 0;
        private int _usedBuffers = 0;

        private System.Windows.Forms.Timer _updateTimer = new System.Windows.Forms.Timer();        
        /// <summary>
        /// Default constructor.
        /// </summary>
		public WaveChartCtrl()
		{ 
            InitializeComponent();
            
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            Left = 157;                        

            scroll50ToolStripMenuItem.Checked = true;
            _scroll = 0.50;
            _autoScaleY = false;
            //Setup the update timer.
            _updateTimer.Interval = 50;
            _updateTimer.Tick += new EventHandler(OnUpdateTimer);
        }

        public uint TimeSlot
        {
            get 
            {
                if (_timeAxis != null)
                    return _timeAxis.TimeSlot;
                else
                    return _timeSlot; 
            }

            set
            {
                _timeSlot = value;
            }
        }

        public Mutex SynchMutex
        {
            get { return _mutex; }
            set { _mutex = value; }
        }

		/// <summary>
		/// Call this after all properties and signals are set to update the control. 
		/// </summary>
        public void InitDone()
		{
			UpdateSettings();

            //Create a default graphics.
            UpdateBuffers();

            List<Signal> userSignals = GetUserSignals();

            if (_legend != null)
            {
                _legend.Signals = userSignals;
                _legend.UpdateLegend();
            }

            if( YAxis != null)
            {
                YAxis.Signals = userSignals;
                YAxis.UpdateYAxis();
            }

            foreach (Bitmap backBuffer in _backBuffers)
            {
                Graphics g = Graphics.FromImage(backBuffer);
                g.Clear(Color.BlueViolet);
            }

            for (int i = 0; i < _bufferCount; ++i)
                _signalsForBuffer.Add(new List<InternalSignal>());

            _usedBuffers = 0;
            for (int i = 0; i < _signals.Count; ++i)
            {
                InternalSignal signal = _signals[i];

                signal.Index = i;
                _usedBuffers = Math.Max(_usedBuffers, (i % _bufferCount) + 1);
                _signalsForBuffer[i % _bufferCount].Add(_signals[i]);
            }

            //Update the Graphics perhaps the size has been changed.
            DrawGrid(_graphics);
		}

        private void UpdateBuffers()
        {
            if(_graphics == null)
                _graphics = CreateGraphics();

            int widthOffset = this.Width / 10;

            if (_secondBuffer == null)
            {
                int width = this.Width;
                int height = this.Height;
                if (width == 0)
                    width = 100;

                if (height == 0)
                    height = 10;

                _secondBuffer = new Bitmap(width + widthOffset, height);
                _backBuffers.Clear();

                for (int i = 0; i < _bufferCount; ++i)
                    _backBuffers.Add(new Bitmap(width + widthOffset, height));
                
                return;
            }

            if( (_secondBuffer.Width != (this.Width + widthOffset) || (_secondBuffer.Height != this.Height)) && this.Height != 0&& this.Width != 0)
            {
                _secondBuffer = new Bitmap(this.Width + widthOffset, this.Height);
                _backBuffers.Clear();
                for (int i = 0; i < _bufferCount; ++i)
                    _backBuffers.Add(new Bitmap(this.Width + widthOffset, this.Height));                
            }
        }

        /// <summary>
        /// Add a signal to the control.
        /// </summary>
        /// <param name="Signal">The signal to add</param>
		public void Add(Signal inSignal )
		{
            InternalSignal signal = new InternalSignal(inSignal);

            if( _timeAxis != null)
                signal.PixelIncrement = (double)(_drawRect.Width * (1.0 / signal.UserSignal.Samplerate)) / _timeAxis.TimeSlot;
            else
                signal.PixelIncrement = (double)(_drawRect.Width * (1.0 / signal.UserSignal.Samplerate)) / _timeSlot;
            _signals.Add(signal);

            inSignal.Index = _signals.Count -1;
		}

        /// <summary>
        /// Remove a signal from the control.
        /// </summary>
        /// <param name="index">The signal index</param>
        public void RemoveSignal(Signal signal)
		{            
            _signals.Remove((InternalSignal)signal.InternalHandle);
            
            for(int i = 0; i < _signals.Count; ++i)
            {
                InternalSignal sig = _signals[i];
                sig.Index = i;
                sig.UserSignal.Index = i;
            }           

            List<Signal> userSigList = GetUserSignals();

            if (Legend != null)
            {
                Legend.Signals = userSigList;
                Legend.UpdateLegend();  
            }
                                
            if(YAxis != null)
            {
                YAxis.Signals = userSigList;
                YAxis.UpdateYAxis();              
            }
		}
			
        /// <summary>
        /// Start the signal processing
        /// </summary>
		public void Start()
		{
            //_resizeBackBuffer = false;

			UpdateSettings();
			Clear();

            foreach (InternalSignal signal in _signals)
            {
                signal.VisibleMin = Double.MaxValue;
                signal.VisibleMax = Double.MinValue;
                signal.FilterMin  = Int32.MaxValue;
                signal.FilterMax  = Int32.MinValue;
                signal.NewX = _drawRect.Left;
                signal.Time = 0;
                signal.Counter = 0;
            }
            
            if( TimeAxis != null)
                TimeAxis.Start();

            _updateTimer.Start();
		}

        private void OnUpdateTimer(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <summary>
        /// Stop the signal processing
        /// </summary>
		public void Stop()
		{
            _updateTimer.Stop();
            
            if (TimeAxis != null)
                TimeAxis.Stop();

            Invalidate();
        }

        /// <summary>
        /// Clear the chart
        /// </summary>
		public void Clear()
		{
            foreach(InternalSignal signal in _signals)
			{
                signal.Points.Clear();
                signal.Points.Capacity = 0;
                signal.NewX = (int)_drawRect.Left;
                signal.Time = 0;
			}

            foreach (Bitmap backBuffer in _backBuffers)
            {
                Graphics g = Graphics.FromImage(backBuffer);
                g.Clear(Color.BlueViolet);
            }

            if (_timeAxis != null)
                _timeAxis.Reset();
            
            GC.Collect();

			Invalidate();
		}

        private int ValueToPixel(double value, Signal signal)
        {
            //Calculate the new point for the signal.
            double verticalFactor = 0;

            double difference = 0.00001;

            if (Math.Abs(signal.Maximum - signal.Minimum) > difference)
                verticalFactor = (double)_drawRect.Height / (signal.Maximum - signal.Minimum);

            return (int)(_drawRect.Bottom - (int)((value - signal.Minimum) * verticalFactor));
        }

        /// <summary>
        /// Set the signal value
        /// </summary>
        /// <param name="signalIndex">The signal index</param>
        /// <param name="value">The new value</param>
		public void SetValue( int signalIndex, double value )
		{            
            if( signalIndex >= _signals.Count)
                return;

            InternalSignal signal = _signals[signalIndex];
            signal.Counter++;


            PointData pData = new PointData(signal.NewX, value, signal.Time);
            signal.NewX += signal.PixelIncrement;
            signal.Time += 1.0 / signal.UserSignal.Samplerate;

            if (signal.UserSignal.Samplerate > _chartMaxSampleRate)
            {//Sample rate filter
                if (signal.Counter % signal.Reductor != 0 && Dock == DockStyle.None)
                    return;
            }

            pData.Y = ValueToPixel(value, signal.UserSignal);

            if (signal.Points.Count == 0)
            {//First sample
                signal.Points.Add(pData);
                signal.LastPoint = pData;
                return;
            }

            if (((int)signal.LastPoint.X == (int)pData.X) && (signal.LastPoint.Y == pData.Y) && Dock == DockStyle.None)
                return; //Same point skip
            
            if ((int)signal.LastPoint.X == (int)pData.X)
            {
                signal.FilterMin = Math.Min(signal.FilterMin, pData.Y);
                signal.FilterMax = Math.Max(signal.FilterMax, pData.Y);

                if (pData.Y > signal.FilterMin && pData.Y < signal.FilterMax && Dock == DockStyle.None)
                    return; //Point not visible skip.
            }
            else
            {
                signal.FilterMin = Int32.MaxValue;
                signal.FilterMax = Int32.MinValue;
            }

            UpdateYScale(value, signal);

            signal.Points.Add(pData);

            DrawLine(signalIndex, pData);

            if (pData.X > _drawRect.Right)
                UpdateSignalsWithScroll(signal, pData);

            signal.LastPoint = pData;

            if (_autoScaleY)
            {
                bool updateSignal = false;

                if (Math.Round(signal.UserSignal.Maximum, 4) != Math.Round(signal.VisibleMax, 4))
                {
                    signal.UserSignal.Maximum = signal.VisibleMax;
                    updateSignal = true;
                }

                if (Math.Round(signal.UserSignal.Minimum, 4) != Math.Round(signal.VisibleMin, 4))
                {
                    signal.UserSignal.Minimum = signal.VisibleMin;
                    updateSignal = true;
                }                
                
                if(updateSignal)
                    if(!_signalToUpdate.Contains(signalIndex))
                        _signalToUpdate.Add(signalIndex);                
            }
		}

        private void RepaintSignal(int index, bool recalcTime)
        {
            Bitmap backBuffer = _backBuffers[index % _bufferCount];
            Graphics g = Graphics.FromImage(backBuffer);

            g.Clear(Color.BlueViolet);

            List<InternalSignal> signals = _signalsForBuffer[index % _bufferCount];

            foreach (InternalSignal signal in signals)
            {
                if (signal.Points.Count == 0)
                    continue;

                PointData fromData = signal.Points[0];
                fromData.Y = ValueToPixel(fromData.Value, signal.UserSignal);

                if (recalcTime)
                {       
                    double deltaTime = _timeAxis.AxisTimeBegin - fromData.Time;
                    double deltaPixel = deltaTime * signal.PixelIncrement * signal.UserSignal.Samplerate;
                    signal.NewX = _drawRect.Left - deltaPixel;
                    fromData.X = signal.NewX;
                }

                if (signal.Points.Count < 1 || !signal.UserSignal.Visible || signal.UserSignal.LineWidth == 0)
                {
                    if (recalcTime)
                    {
                        for (int i = 1; i < signal.Points.Count; ++i)
                        {
                            PointData toData = signal.Points[i];
                            signal.NewX += signal.PixelIncrement;
                            toData.X = signal.NewX;
                        }
                    }
                    continue;
                }

                for (int i = 1; i < signal.Points.Count; ++i)
                {
                    PointData toData = signal.Points[i];
                    if (recalcTime)
                    {                       
                        signal.NewX += signal.PixelIncrement;
                        toData.X = signal.NewX;
                    }

                    Point from = new Point((int)fromData.X, fromData.Y);
                    toData.Y = ValueToPixel(toData.Value, signal.UserSignal);
                    
                    Point to = new Point((int)toData.X, toData.Y);
                    Point mittelPoint = new Point(to.X, from.Y);

                    if (from.X != to.X || from.Y != to.Y)
                    {
                        g.DrawLine(new Pen(signal.UserSignal.LineColor, signal.UserSignal.LineWidth), from, mittelPoint);
                        g.DrawLine(new Pen(signal.UserSignal.LineColor, signal.UserSignal.LineWidth), mittelPoint, to);
                    }
                    fromData = toData;

                    if (signal.UserSignal.PointsVisible && signal.UserSignal.PointSize != 0)
                        DrawPoint(g, to, signal.UserSignal.PointColor, signal.UserSignal.PointSize);
                }
            }

            if (YAxis != null)
                YAxis.UpdateYAxis();            
        }

        private void DrawLine(int index, PointData pData)
        {
            InternalSignal signal = _signals[index];

            if (_freeze || !signal.UserSignal.Visible || signal.UserSignal.LineWidth == 0)
                return;


            Bitmap backBuffer = _backBuffers[index % _bufferCount];

            Graphics g = Graphics.FromImage(backBuffer);

            Point from = new Point((int)signal.LastPoint.X, signal.LastPoint.Y);
            Point to = new Point((int)pData.X, pData.Y);
            Point mittelPoint = new Point(to.X, from.Y);
            Pen pen = new Pen(signal.UserSignal.LineColor, signal.UserSignal.LineWidth);

            Point[] points = new Point[3]{from, mittelPoint, to};
            try
            {
                g.DrawLines(pen, points);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (signal.UserSignal.PointsVisible && signal.UserSignal.PointSize != 0)
            {
                if (from.X != to.X || from.Y != to.Y)
                {
                    DrawPoint(g, from, signal.UserSignal.PointColor, signal.UserSignal.PointSize);
                    DrawPoint(g, to, signal.UserSignal.PointColor, signal.UserSignal.PointSize);
                }
            }            
        }

        private static void UpdateYScale(double value, InternalSignal signal)
        {
            signal.VisibleMax = Math.Max(value, signal.VisibleMax);
            signal.VisibleMin = Math.Min(value, signal.VisibleMin);
        }

        /// <summary>
        /// Call this to update the chart components like axis.
        /// </summary>
        /// <param name="sender">The sender of refresh</param>
		public void Refresh(object sender)
		{
            if (_freeze)
                return;

            if( _timeAxis != null && sender != _timeAxis )
                _timeAxis.Invalidate();

            if( _YAxis != null && sender != _YAxis )
                _YAxis.UpdateYAxis();

            if( _legend != null && sender != _legend )
                _legend.Invalidate();

               Invalidate();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Vom Komponenten-Designer generierter Code
		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaveChartCtrl));
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
            System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSignalProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.scroll25ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scroll50ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scroll100ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoScaleYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.freezeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemSyncSignals = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemShowGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemClearOK = new System.Windows.Forms.ToolStripMenuItem();
            toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenuItem3
            // 
            resources.ApplyResources(toolStripMenuItem3, "toolStripMenuItem3");
            toolStripMenuItem3.Name = "toolStripMenuItem3";
            // 
            // toolStripMenuItem1
            // 
            resources.ApplyResources(toolStripMenuItem1, "toolStripMenuItem1");
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // toolStripMenuItem5
            // 
            resources.ApplyResources(toolStripMenuItem5, "toolStripMenuItem5");
            toolStripMenuItem5.Name = "toolStripMenuItem5";
            // 
            // toolStripMenuItem2
            // 
            resources.ApplyResources(toolStripMenuItem2, "toolStripMenuItem2");
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            // 
            // contextMenuStrip
            // 
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemProperties,
            this.menuItemSignalProperties,
            toolStripMenuItem3,
            this.scroll25ToolStripMenuItem,
            this.scroll50ToolStripMenuItem,
            this.scroll100ToolStripMenuItem,
            toolStripMenuItem1,
            this.autoScaleYToolStripMenuItem,
            toolStripMenuItem5,
            this.freezeToolStripMenuItem,
            this.menuItemSyncSignals,
            this.menuItemShowGrid,
            toolStripMenuItem2,
            this.clearToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip2";
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // menuItemProperties
            // 
            resources.ApplyResources(this.menuItemProperties, "menuItemProperties");
            this.menuItemProperties.Name = "menuItemProperties";
            this.menuItemProperties.Click += new System.EventHandler(this.menuItemProperties_Click);
            // 
            // menuItemSignalProperties
            // 
            resources.ApplyResources(this.menuItemSignalProperties, "menuItemSignalProperties");
            this.menuItemSignalProperties.Name = "menuItemSignalProperties";
            this.menuItemSignalProperties.Click += new System.EventHandler(this.menuItemSignalProperties_Click);
            // 
            // scroll25ToolStripMenuItem
            // 
            resources.ApplyResources(this.scroll25ToolStripMenuItem, "scroll25ToolStripMenuItem");
            this.scroll25ToolStripMenuItem.Name = "scroll25ToolStripMenuItem";
            this.scroll25ToolStripMenuItem.Click += new System.EventHandler(this.scroll25ToolStripMenuItem_Click);
            // 
            // scroll50ToolStripMenuItem
            // 
            resources.ApplyResources(this.scroll50ToolStripMenuItem, "scroll50ToolStripMenuItem");
            this.scroll50ToolStripMenuItem.Name = "scroll50ToolStripMenuItem";
            this.scroll50ToolStripMenuItem.Click += new System.EventHandler(this.scroll50ToolStripMenuItem_Click);
            // 
            // scroll100ToolStripMenuItem
            // 
            resources.ApplyResources(this.scroll100ToolStripMenuItem, "scroll100ToolStripMenuItem");
            this.scroll100ToolStripMenuItem.Name = "scroll100ToolStripMenuItem";
            this.scroll100ToolStripMenuItem.Click += new System.EventHandler(this.scroll100ToolStripMenuItem_Click);
            // 
            // autoScaleYToolStripMenuItem
            // 
            resources.ApplyResources(this.autoScaleYToolStripMenuItem, "autoScaleYToolStripMenuItem");
            this.autoScaleYToolStripMenuItem.Name = "autoScaleYToolStripMenuItem";
            this.autoScaleYToolStripMenuItem.Click += new System.EventHandler(this.autoScaleYToolStripMenuItem_Click);
            // 
            // freezeToolStripMenuItem
            // 
            resources.ApplyResources(this.freezeToolStripMenuItem, "freezeToolStripMenuItem");
            this.freezeToolStripMenuItem.Name = "freezeToolStripMenuItem";
            this.freezeToolStripMenuItem.Click += new System.EventHandler(this.freezeToolStripMenuItem_Click);
            // 
            // menuItemSyncSignals
            // 
            resources.ApplyResources(this.menuItemSyncSignals, "menuItemSyncSignals");
            this.menuItemSyncSignals.Checked = true;
            this.menuItemSyncSignals.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemSyncSignals.Name = "menuItemSyncSignals";
            this.menuItemSyncSignals.Click += new System.EventHandler(this.menuItemSyncSignals_Click);
            // 
            // menuItemShowGrid
            // 
            resources.ApplyResources(this.menuItemShowGrid, "menuItemShowGrid");
            this.menuItemShowGrid.Checked = true;
            this.menuItemShowGrid.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemShowGrid.Name = "menuItemShowGrid";
            this.menuItemShowGrid.Click += new System.EventHandler(this.menuItemShowGrid_Click);
            // 
            // clearToolStripMenuItem
            // 
            resources.ApplyResources(this.clearToolStripMenuItem, "clearToolStripMenuItem");
            this.clearToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemClearOK});
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            // 
            // menuItemClearOK
            // 
            resources.ApplyResources(this.menuItemClearOK, "menuItemClearOK");
            this.menuItemClearOK.Name = "menuItemClearOK";
            this.menuItemClearOK.Click += new System.EventHandler(this.menuItemClearOK_Click);
            // 
            // WaveChartCtrl
            // 
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.Black;
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Name = "WaveChartCtrl";
            this.Load += new System.EventHandler(this.OnLoad);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnMouseDoubleClick);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        /// <summary>
        /// Sets or gest the time axis
        /// </summary>
		public TimeAxis	TimeAxis
		{
			set 
			{
                _timeAxis = value;

                if (_timeAxis != null)
                {
                    _timeAxis.WaveChart = this;
                    _timeSlot = _timeAxis.TimeSlot;
                }

				UpdateSettings();		
			}
            get { return _timeAxis; }
		}
        
        /// <summary>
        /// Scroll on percent
        /// </summary>
        public double ScrollChart
        {
            set
            {
                _scroll = value;
            }
            get { return _scroll; }
        }
        
        /// <summary>
        /// Gets or sets the auto scale Y flag
        /// </summary>
        public bool AutoScaleY
        {
            set
            {
                _autoScaleY = value;
            }

            get { return _autoScaleY; }

        }

        /// <summary>
        /// Sets or gets the Y axis
        /// </summary>
		public YAxis YAxis
		{
			get { return _YAxis;}
			set 
			{ 
				_YAxis = value;

                if (YAxis != null)
                {
                    YAxis.Signals = GetUserSignals();
                    _YAxis.Text = this.Text;
                }
			}
		}

        private List<Signal> GetUserSignals()
        {
            List<Signal> userSigs = new List<Signal>();
            
            foreach(InternalSignal sig in _signals)
                userSigs.Add(sig.UserSignal);
            
            return userSigs;
        }
        /// <summary>
        /// Gets or sets the maximum samplerate of the signals in Hz.
        /// </summary>
        public int MaxSampleRate
        {
            get
            {
                return (int)_chartMaxSampleRate;
            }

            set
            {
                _chartMaxSampleRate = (ulong) value;
            }
        }

        /// <summary>
        /// Set or gets the signal legend
        /// </summary>
        public SigLegend Legend
        {
            get { return _legend; }
            set
            {
                _legend = value;
                _legend.Signals = GetUserSignals();
                _legend.WaveChart = this;
            }
        }
        
        /// <summary>
        /// Define the distance between grid and chart.
        /// </summary>
        [SRCategory("Grid"), SRDescription("Margin")]
        public new uint Margin
		{
            get { return (uint) _margin; }
            set 
            { 
                _margin = (int)value;
                
                if (!_freeze)
                    Invalidate();
                
                if (_YAxis != null)
                    _YAxis.UpdateYAxis();
            }
		}

        /// <summary>
        /// Gets or sets the grid visible flag.
        /// </summary>
        [SRCategory("Grid"), SRDescriptionAttribute("ShowGrid")]
		public bool ShowGrid
		{
			get { return _showGrid; }
            set
            {
                _showGrid = value; 
                
                if (!_freeze)
                    Invalidate();
            }
		}

        /// <summary>
        /// Sets or gets the grid line color.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridLineColor")]
		public Color GridLineColor
		{
            set 
            { 
                _gridLineColor = value;

                if (!_freeze)
                    Invalidate();                
            }
            get { return _gridLineColor; }
		}

        /// <summary>
        /// Sets or gets the grid line Width.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridLineWidth")]
		public int  GridLineWidth
		{
            set 
            { 
                _gridLineWidth = value;
                if (!_freeze)
                    Invalidate();                
            }

            get { return _gridLineWidth; }
		}
        
        /// <summary>
        /// Sets or gets the grid line Style.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridLineStyle")]
		public DashStyle GridLineStyle
		{
			set 
            {
                _gridLineStyle = value;
                if (!_freeze)
                    Invalidate();                
            }
            get { return _gridLineStyle; }
		}
        
        /// <summary>
        /// Sets or gets the Grid X devision.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridXDivision")]
		public uint GridXDevision
		{
			set 
            {
                _gridXDivision = value; 
                if (!_freeze)
                    Invalidate();                

            }
            get { return _gridXDivision; }
		}
        
        /// <summary>
        /// Sets or gets the Y grid division.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridYDivision")]
		public uint GridYDevision
		{
			set 
            { 
                _gridYDivision = value;
                
                if (!_freeze)
                    Invalidate();                
            }

            get { return _gridYDivision; }
        }

        /// <summary>
        /// Sets or gets the Grid X devision.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridXDivision")]
        public uint GridXDivision
        {
            set
            {
                _gridXDivision = value;
                if (!_freeze)
                    Invalidate();

            }
            get { return _gridXDivision; }
        }

        /// <summary>
        /// Sets or gets the Y grid division.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridYDivision")]
        public uint GridYDivision
        {
            set
            {
                _gridYDivision = value;

                if (!_freeze)
                    Invalidate();
            }

            get { return _gridYDivision; }
        }
        /// <summary>
        /// Sets or gets the signal synchronisation flag.
        /// </summary>
        [SRCategoryAttribute("Chart"), SRDescriptionAttribute("SyncSignal")]
        public bool SyncSignal
		{
			set { _sync = value;}
            get { return _sync; }
		}

		private void DrawGrid(Graphics g)
		{
            if (!_showGrid)
                return;


            double xPixelInc = (_drawRect.Width / (double)_gridXDivision);
            double nPos = _drawRect.Left;
            Pen dPen = new Pen(_gridLineColor);
            dPen.Width = _gridLineWidth;
            dPen.DashStyle = _gridLineStyle;

            for (int x = 0; x < _gridXDivision; x++)
			{
                g.DrawLine(dPen, new Point((int)nPos, (int)_drawRect.Top), new Point((int)nPos, (int)_drawRect.Bottom));
				nPos += xPixelInc;
			}

            g.DrawLine(dPen, new Point((int)(nPos), (int)_drawRect.Top), new Point((int)(nPos), (int)_drawRect.Bottom));

            double yPixeInc = (_drawRect.Height / (double)_gridYDivision);
            nPos = _drawRect.Top;

            for (int y = 0; y < _gridYDivision; y++)
			{
                g.DrawLine(dPen, new Point((int)_drawRect.Left, (int)(nPos)), new Point((int)_drawRect.Right, (int)(nPos)));
				nPos += yPixeInc;
			}
            g.DrawLine(dPen, new Point((int)(_drawRect.Left), (int)(nPos + 1)), new Point((int)_drawRect.Right, (int)(nPos + 1)));

		}

        private void ClearMargin(Graphics g)
        {
            if (_margin == 0)
                return;

            //Margin rect
            Rectangle rect = ClientRectangle;
            rect.Width = (int)(ClientRectangle.Left + _margin);

            //Let's the base to paint the backgruound into a image.
            Bitmap bitmap = new Bitmap(rect.Width, rect.Height);
            Graphics gr = Graphics.FromImage(bitmap);
            PaintEventArgs pe = new PaintEventArgs(gr, rect);
            OnPaintBackground(pe);

            //Now draw the image
            g.DrawImage(bitmap, 0, 0);
        }

        private void UpdateSignalsWithScroll(InternalSignal inSignal, PointData curPoint)
        {            
            double diff = (double)(_scroll * _drawRect.Width);
            inSignal.NewX -= diff;

            foreach (InternalSignal signal in _signals)
            {
                int remove = (int)((double)signal.Points.Count * (_scroll - 0.05));

                if (signal != inSignal)
                {
                    signal.NewX -= diff;

                    if(_sync)
                        signal.NewX = inSignal.NewX;
                }

                if (remove > 1)
                    signal.Points.RemoveRange(0, remove - 1);

                signal.VisibleMin = double.MaxValue;
                signal.VisibleMax = double.MinValue;

                foreach (PointData pdata in signal.Points)
                {
                    pdata.X -= diff;
                    UpdateYScale(pdata.Value, signal);
                }
            }

            if (_timeAxis != null)
               _timeAxis.Overrun(_scroll);

            if (_freeze)
                return;

            _mutex.WaitOne();
            
            //Scroll the view by diff.
            Graphics g = Graphics.FromImage(_secondBuffer);            

            for ( int i = 0; i < _usedBuffers; ++i)                
            {
                Bitmap backBuffer = _backBuffers[i];
                g.Clear(Color.BlueViolet);
                g.DrawImage(backBuffer, new Point(-((int)diff), 0));

                Graphics gr = Graphics.FromImage(backBuffer);
                ImageAttributes attr = new ImageAttributes();                
                attr.SetColorKey(Color.BlueViolet, Color.BlueViolet);
                gr.Clear(Color.BlueViolet);
                gr.DrawImage(_secondBuffer, new Rectangle(0, 0, this.Width, this.Height), 0, 0, this.Width, this.Height, GraphicsUnit.Pixel, attr);
            }

            _mutex.ReleaseMutex();
        }
				
		private void DrawPoint(Graphics g, Point pt,Color color,uint nSize)
		{
			Pen dPen = new Pen(color);
			SolidBrush br = new SolidBrush(color);
			Point dPoint = pt;
			dPoint.X -= (int)(nSize/2);
			dPoint.Y -= (int)(nSize/2);
		
			g.DrawEllipse(dPen,dPoint.X,dPoint.Y,nSize,nSize);
		}

		private void UpdateSettings()
		{
            if (this.Width == 0 || this.Height == 0)
                return;

            int dx = (this.Width - 2*_margin);
            int dy = (this.Height - 2*_margin);

            _drawRect = new Rectangle(_margin, _margin, dx, dy);
            
            DrawGrid(_graphics);

            if (_timeAxis != null)
                _timeAxis.ChartRectangle = _drawRect;

			if(_YAxis != null)
                _YAxis.ChartRectangle = _drawRect;

            foreach (InternalSignal signal in _signals)
			{
                signal.Reductor = (ulong)signal.UserSignal.Samplerate / _chartMaxSampleRate;
                if (signal.Reductor == 0)
                    signal.Reductor = 1;

                if( _timeAxis != null)
                    signal.PixelIncrement = (double)((_drawRect.Width * (1.0 / signal.UserSignal.Samplerate))) / (double)_timeAxis.TimeSlot;
                else
                    signal.PixelIncrement = (double)((_drawRect.Width * (1.0 / signal.UserSignal.Samplerate))) / (double)_timeSlot;
			}
		}

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem menuItemProperties;
        private ToolStripMenuItem menuItemSignalProperties;
        private ToolStripMenuItem menuItemShowGrid;
        private ToolStripMenuItem clearToolStripMenuItem;
        private ToolStripMenuItem menuItemClearOK;
        private ToolStripMenuItem menuItemSyncSignals;


        protected override void OnPaint(PaintEventArgs e)
        {
            _mutex.WaitOne();

            if (_eventCounter % 4 == 0)
            {
                foreach (int index in _signalToUpdate)
                    RepaintSignal(index,false);

                _signalToUpdate.Clear();
                _eventCounter = 0;
            }

            ImageAttributes attr = new ImageAttributes();
         
            DrawGrid(e.Graphics);

            attr.SetColorKey(Color.BlueViolet, Color.BlueViolet);            

            for(int i = 0; i < _usedBuffers; ++i)
            {
                Bitmap backBuffer = _backBuffers[i];
                e.Graphics.DrawImage(backBuffer, new Rectangle(0, 0, this.Width, this.Height), 0, 0, this.Width, this.Height, GraphicsUnit.Pixel, attr);
            }

            ClearMargin(e.Graphics);
            _eventCounter++;
           _mutex.ReleaseMutex();
		}

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);

            _mutex.WaitOne();
            
            UpdateBuffers();
            UpdateSettings();

            _signalToUpdate.Clear();

            foreach (InternalSignal signal in _signals)
                RepaintSignal(signal.Index, true);

            _mutex.ReleaseMutex();
            
		}

		private void OnLoad(object sender, System.EventArgs e)
		{
            menuItemShowGrid.Checked = ShowGrid;
            menuItemSyncSignals.Checked = SyncSignal;
		}

        public void OnUpdateChartProperties(PropertyDlg.DialogData properties)
        {
            BackColor = properties.BackgroundColor;
            _margin = (int)properties.Margin;
            
            if (_YAxis != null)
                _YAxis.Invalidate();

            _gridXDivision = (uint)properties.XDevision;
            _gridYDivision = (uint)properties.YDevision;
            _gridLineWidth = properties.LineWidth;
            _gridLineStyle = properties.LineStyle;
            _gridLineColor = properties.LineColor;
            _chartMaxSampleRate = properties.ChartRate;

            UpdateSettings();            
            
            if(!_freeze)
                Invalidate();
        }

        private void menuItemProperties_Click(object sender, EventArgs e)
        {
            PropertyDlg waveChartPropDlg = new PropertyDlg();
            waveChartPropDlg.Data.BackgroundColor = BackColor;
            waveChartPropDlg.Data.Name = this.Text;
            waveChartPropDlg.Data.Margin = (int)_margin;
            waveChartPropDlg.Data.XDevision = (int)_gridXDivision;
            waveChartPropDlg.Data.YDevision = (int)_gridYDivision;
            waveChartPropDlg.Data.LineWidth = _gridLineWidth;
            waveChartPropDlg.Data.LineStyle = _gridLineStyle;
            waveChartPropDlg.Data.LineColor = _gridLineColor;
            waveChartPropDlg.Data.ChartRate = (uint) _chartMaxSampleRate;
            waveChartPropDlg.OnUpdateProperties = new PropertyDlg.UpdateProperties(OnUpdateChartProperties);
            waveChartPropDlg.ShowDialog();
        }

        private void OnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            menuItemProperties_Click(null, null);
        }

        private void menuItemSignalProperties_Click(object sender, EventArgs e)
        {
            if (_legend != null)
            {
                SignalPropertiesDlg Properties = new SignalPropertiesDlg();
                Properties.Legend = _legend;
                Properties.WaveChart = this;
                Properties.Signals = GetUserSignals();
                Properties.ShowDialog();
            }
        }

        private void menuItemShowGrid_Click(object sender, EventArgs e)
        {
            menuItemShowGrid.Checked = !menuItemShowGrid.Checked;
            ShowGrid = menuItemShowGrid.Checked;
        }

        private void menuItemClearOK_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void menuItemSyncSignals_Click(object sender, EventArgs e)
        {
            menuItemSyncSignals.Checked = !menuItemSyncSignals.Checked;
            SyncSignal = menuItemSyncSignals.Checked;
        }

        private void menuItemAutoArrange_Click(object sender, EventArgs e)
        { }

        private void contextMenu_Popup(object sender, EventArgs e)
        { }

        private void freezeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            freezeToolStripMenuItem.Checked = !freezeToolStripMenuItem.Checked;
            
            _freeze = freezeToolStripMenuItem.Checked;
            TimeAxis.Freeze = freezeToolStripMenuItem.Checked;

            if (!_freeze)
            {//not freze => repaint all buffers.
                _mutex.WaitOne();

                foreach (List<InternalSignal> signals in _signalsForBuffer)
                {
                    if( signals.Count > 0)
                        RepaintSignal(signals[0].Index,false);
                }

                _mutex.ReleaseMutex();
            }
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            menuItemSyncSignals.Checked = SyncSignal;
        }

        private void scroll25ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scroll25ToolStripMenuItem.Checked = true;
            scroll50ToolStripMenuItem.Checked = false;
            scroll100ToolStripMenuItem.Checked = false;
            _scroll = 0.25;
        }

        private void scroll50ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scroll25ToolStripMenuItem.Checked = false;
            scroll50ToolStripMenuItem.Checked = true;
            scroll100ToolStripMenuItem.Checked = false;
            _scroll = 0.50;
        }

        private void scroll100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scroll25ToolStripMenuItem.Checked = false;
            scroll50ToolStripMenuItem.Checked = false;
            scroll100ToolStripMenuItem.Checked = true;
            _scroll = 1;
        }

        private void autoScaleYToolStripMenuItem_Click(object sender, EventArgs e)
        {         
            autoScaleYToolStripMenuItem.Checked = !autoScaleYToolStripMenuItem.Checked;
            _autoScaleY = autoScaleYToolStripMenuItem.Checked;         
        }
	}
}


