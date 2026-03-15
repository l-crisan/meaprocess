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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Mp.Visual.WaveChart
{
    /// <summary>
    /// The time axis unit represenation.
    /// </summary>
	public enum TimeAxisRep
	{
		Seconds = 0,
        MinuteSeconds,
		HourMinuteSeconds
	}

	/// <summary>
	/// This calss implements the wave chart time axis.
	/// </summary>
	public class TimeAxis : System.Windows.Forms.UserControl
    {
        private IContainer components;
        private bool _freeze = false;
        private bool _running = false;
        private double _freezeTime = 0.0;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem secondsToolStripMenuItem;
        private ToolStripMenuItem mMSSToolStripMenuItem;
        private ToolStripMenuItem hHMMSSToolStripMenuItem;
        private DateTime _startTime;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public TimeAxis()
		{ 
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            Left = 157;
            Top  = 275;
            _chartRect.X = 10;
            _chartRect.Y = 10;
            _chartRect.Width = this.Width - 20;
            _chartRect.Height = this.Height - 5;
        }

        public void Start()
        {
            _running = true;
            _startTime = DateTime.Now;
        }

        public void Stop()
        {
            _running = false;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeAxis));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.secondsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mMSSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hHMMSSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.AccessibleDescription = null;
            this.contextMenuStrip.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.BackgroundImage = null;
            this.contextMenuStrip.Font = null;
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.secondsToolStripMenuItem,
            this.mMSSToolStripMenuItem,
            this.hHMMSSToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenu";
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.AccessibleDescription = null;
            this.propertiesToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.propertiesToolStripMenuItem, "propertiesToolStripMenuItem");
            this.propertiesToolStripMenuItem.BackgroundImage = null;
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.AccessibleDescription = null;
            this.toolStripMenuItem1.AccessibleName = null;
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            // 
            // secondsToolStripMenuItem
            // 
            this.secondsToolStripMenuItem.AccessibleDescription = null;
            this.secondsToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.secondsToolStripMenuItem, "secondsToolStripMenuItem");
            this.secondsToolStripMenuItem.BackgroundImage = null;
            this.secondsToolStripMenuItem.Name = "secondsToolStripMenuItem";
            this.secondsToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.secondsToolStripMenuItem.Click += new System.EventHandler(this.secondsToolStripMenuItem_Click);
            // 
            // mMSSToolStripMenuItem
            // 
            this.mMSSToolStripMenuItem.AccessibleDescription = null;
            this.mMSSToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.mMSSToolStripMenuItem, "mMSSToolStripMenuItem");
            this.mMSSToolStripMenuItem.BackgroundImage = null;
            this.mMSSToolStripMenuItem.Name = "mMSSToolStripMenuItem";
            this.mMSSToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.mMSSToolStripMenuItem.Click += new System.EventHandler(this.mMSSToolStripMenuItem_Click);
            // 
            // hHMMSSToolStripMenuItem
            // 
            this.hHMMSSToolStripMenuItem.AccessibleDescription = null;
            this.hHMMSSToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.hHMMSSToolStripMenuItem, "hHMMSSToolStripMenuItem");
            this.hHMMSSToolStripMenuItem.BackgroundImage = null;
            this.hHMMSSToolStripMenuItem.Name = "hHMMSSToolStripMenuItem";
            this.hHMMSSToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.hHMMSSToolStripMenuItem.Click += new System.EventHandler(this.hHMMSSToolStripMenuItem_Click);
            // 
            // TimeAxis
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = null;
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Name = "TimeAxis";
            this.DoubleClick += new System.EventHandler(this.PoWaveChartTimeAxis_DoubleClick);
            this.Resize += new System.EventHandler(this.PoWaveChartTimeAxis_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        public bool Freeze
        {
            set 
            { 
                _freeze = value;
                _freezeTime = _curTime;
                
                if(!_freeze)
                    Invalidate();
            }
            get { return _freeze; }
        }
        /// <summary>
        /// Call this to update the time axis by time overrun.
        /// </summary>
		public void Overrun(double scroll)
		{
			_curTime += _timeSlot * scroll;            
			Invalidate();
		}

        /// <summary>
        /// Call this to reset the time axis
        /// </summary>
		public void Reset()
		{
			_curTime = 0.0;
            _startTime = DateTime.Now;
			Invalidate();
		}

        /// <summary>
        /// Sets or gest the parent wave chart control.
        /// </summary>
        public WaveChartCtrl WaveChart
        {
            set 
            { 
                _waveChart = value;
                Text = _waveChart.Text;
            }

            get { return _waveChart; }
        }

        /// <summary>
        ///Sets or gets the axis unit devision.
        /// </summary>
        [SRCategory("XAxis"),
        SRDescription("XAxisDivision")]
		public uint AxisDivision
		{
			get { return _axisDivision; }
			set 
            { 
                _axisDivision = value;
                Invalidate();
            }
		}

        /// <summary>
        /// Sets or gets the axis time slot in seconds.
        /// </summary>
        [SRCategory("XAxis"),
        SRDescription("XTimeSlot")]
		public uint TimeSlot
		{
            get { return _timeSlot; }
            set { _timeSlot = value; }
		}

        /// <summary>
        /// Sets or gets the axis text color. 
        /// </summary>
        [SRCategory("XAxis"),
        SRDescription("XTextColor")]
		public Color TextColor
		{
			get { return _textColor; }
			set 
            { 
                _textColor = value;
                Invalidate();
            }
		}

        /// <summary>
        /// Sets or gest the axis line color.
        /// </summary>
        [SRCategory("XAxis"), SRDescription("XLineColor")]
		public Color LineColor
		{
			get { return _lineColor; }
			set 
            { 
                _lineColor = value;
                Invalidate();
            }
		}
        
        /// <summary>
        /// Sets or gets the axis degree text color.
        /// </summary>
        [SRCategory("Axis"), SRDescription("XDegreeTextColor")]
		public Color DegreeTextColor
		{
			get { return _degreeTxtColor; }
			set 
            { 
                _degreeTxtColor = value;
                Invalidate();
            }
		}

        /// <summary>
        /// Sets or gest the axis unit precision.
        /// </summary>
        /// <remarks>
        /// 20.45  => Precision = 2
        /// 20.450 => Precision = 3
        /// </remarks>
        [SRCategory("XAxis"), SRDescription("XPrecision")]
		public uint Precision
		{
			get { return _axisPrecision; }
			set 
            { 
                _axisPrecision = value;
                Invalidate();
            }
		}

        /// <summary>
        /// Sets or gets the axis unit representation.
        /// </summary>
        /// <remarks>
        /// TimeAxisRep.Seconds => (s)
        /// TimeAxisRep.MinutesSeconds => ( MM:SS )
        /// TimeAxisRep.HourMinutesSeconds => ( HH:MM:SS )
        /// </remarks>
        [SRCategory("XAxis"), SRDescription("XRepresentation")]
		public TimeAxisRep Representation
		{
            get { return _timeAxisRep; }
            set 
            { 
                _timeAxisRep = value;
                UpdateContextMenu();
                Invalidate();
            }
		}

        /// <summary>
        /// Sets or gets the axis text font.
        /// </summary>
        [SRCategory("XAxis"), SRDescription("XFont")]
		public new Font Font
		{
			get { return _font; }
			set 
            { 
                _font = value;
                Invalidate();
            }
		}

        /// <summary>
        /// Sets or gets the axis text label.
        /// </summary>
        [SRCategory("XAxis"), SRDescription("XAxisText")]
		public string AxisText
		{
			get { return _axisText; }
            set 
            { 
                _axisText = value;
                Invalidate();
            }
		}

        public double AxisTimeBegin
        {
            get { return _curTime; }
        }

        /// <summary>
        /// Sets or gets the chart rectagle.
        /// </summary>
        public Rectangle ChartRectangle
        {
            set{ _chartRect = value; }
        }	

		private string GetFormatedTime( double time )
		{
			string timeText = "";

            switch( _timeAxisRep )
			{
				case TimeAxisRep.Seconds:
				{	
					string format   = "f";
                    format          += _axisPrecision.ToString();
                    timeText        = time.ToString( format );
				}
				break;
				case TimeAxisRep.MinuteSeconds:
				{
                    uint minute = (uint)(time / 60);
                    uint second = (uint)(time - (minute * 60));
                    timeText = String.Format("{0}:{1}", minute.ToString("d2"), second.ToString("d2"));
				}
				break;
                case TimeAxisRep.HourMinuteSeconds:
				{
                    
//                    uint hour = (uint)(time / (60 * 60));
//                    uint minute = (uint)((time - (hour * 60 * 60)) / 60);
//                    uint second = (uint)(time - (hour * 60 * 60) - (minute * 60));
                    DateTime curTime = _startTime.Add(new TimeSpan((long)(time*10000000))); //100 nano sec.
                    timeText = String.Format("{0}:{1}:{2}", curTime.Hour.ToString("d2"), curTime.Minute.ToString("d2"), curTime.Second.ToString("d2"));
				}
				break;
			}
            return timeText;
		}

		private Rectangle           _chartRect;    
        private double		        _curTime            = 0.0;         
		private uint			    _axisDivision       = 5;
		private uint		        _timeSlot           = 10;
		private Color			    _textColor	        = Color.Gray;
		private Color			    _degreeTxtColor     = Color.Gray;
        private Color               _lineColor = Color.White;
        private uint                _axisPrecision = 2;
        private TimeAxisRep         _timeAxisRep = TimeAxisRep.HourMinuteSeconds;
        private Font                _font = new Font("Arial", 10);
		private string		        _axisText = "Time";
        private WaveChartCtrl       _waveChart;

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem propertiesToolStripMenuItem;

        protected override void OnPaint(PaintEventArgs e)
		{
            Graphics     g = e.Graphics;            
            
			SolidBrush	bkBrush = new SolidBrush(BackColor);			
			Pen			LinePen = new Pen(_lineColor);
			SizeF		size;

			//Clear the Axis
			g.FillRectangle(bkBrush,ClientRectangle);
		
			//Draw axis line
			g.DrawLine(LinePen,	new Point(_chartRect.Left,ClientRectangle.Top + 20),
								new Point(_chartRect.Right,ClientRectangle.Top + 20));
				
			//Draw first degree
			g.DrawLine(LinePen,	new Point(_chartRect.Left,ClientRectangle.Top + 10),
								new Point(_chartRect.Left,ClientRectangle.Top + 30));

			double		delta       = _chartRect.Width / (double)_axisDivision ;
            double      pos         = delta;
            double		time		= _curTime;
            
            if (_freeze)
                time = _freezeTime;
			
			string		text		= GetFormatedTime(time);
			SolidBrush  textBrush   = new SolidBrush(_textColor);
			SolidBrush  degreeBrush	= new SolidBrush(_degreeTxtColor);

			//Draw first degree text			
            g.DrawString(text, _font, degreeBrush, new Point(_chartRect.Left, ClientRectangle.Top + 30));

            time += (double)_timeSlot / (double)_axisDivision;
			
			//Draw degrees and texts
			for(int  n = 0; n < _axisDivision - 1; n++)
			{
				//Draw next degree 
                g.DrawLine(LinePen, new Point(_chartRect.Left + (int)pos, ClientRectangle.Top + 15), new Point(_chartRect.Left + (int)pos, ClientRectangle.Top + 25));

				//Draw next degree text
                text = GetFormatedTime(time);
                g.DrawString(text, _font, degreeBrush, new Point(_chartRect.Left + (int)pos, ClientRectangle.Top + 30));

				//Calc next draw pos
                pos += delta;
                time += (double)_timeSlot / _axisDivision;			
			}

			//Draw last degree
            g.DrawLine(LinePen, new Point(_chartRect.Left + (int)pos, ClientRectangle.Top + 10), new Point(_chartRect.Left + (int)pos, ClientRectangle.Top + 30));

			//Draw last degree text
            size = g.MeasureString(text, _font);
            text = GetFormatedTime(time);
            g.DrawString(text, _font, degreeBrush, new Point((int)(_chartRect.Left + pos - size.Width), ClientRectangle.Top + 30));			
			
			//Axis text
            text = _axisText;
            size = g.MeasureString(text, _font);
            g.DrawString(text, _font, textBrush, new Point((_chartRect.Width / 2) - (int)(size.Width / 2), ClientRectangle.Top + 50));
		}
  
        private void PoWaveChartTimeAxis_Resize(object sender, System.EventArgs e)
		{ 
            Invalidate(); 
        }
   
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TimeAxisPropDlg XAxisPropDlg = new TimeAxisPropDlg(_running);

            XAxisPropDlg.OnChangeProperties                 = new TimeAxisPropDlg.ChangeProperties(OnXAxisPropChanged);
            XAxisPropDlg.DialogProperties.TimeSlot          = _timeSlot;
            XAxisPropDlg.DialogProperties.Representation    = (int)_timeAxisRep;
            XAxisPropDlg.DialogProperties.Precision         = (int) _axisPrecision;
            XAxisPropDlg.DialogProperties.Name              = this.Text;
            XAxisPropDlg.DialogProperties.LineColor         = _lineColor;
            XAxisPropDlg.DialogProperties.Division          = (int)_axisDivision;
            XAxisPropDlg.DialogProperties.DegreeTxtColor    = _degreeTxtColor;
            XAxisPropDlg.DialogProperties.BackColor         = BackColor;
            XAxisPropDlg.DialogProperties.AxisTextColor     = _textColor;
            XAxisPropDlg.DialogProperties.AxisText          = _axisText;

            XAxisPropDlg.ShowDialog();
        }

        private void OnXAxisPropChanged(TimeAxisPropDlg.PoDialogProperties Properties)
        {
            TimeSlot            = Properties.TimeSlot;
            _timeAxisRep        = (TimeAxisRep)Properties.Representation;
            _axisPrecision      = (uint) Properties.Precision;
            _lineColor          = Properties.LineColor;
            _axisDivision       = (uint) Properties.Division;
            _degreeTxtColor     = Properties.DegreeTxtColor;
            BackColor           = Properties.BackColor;
            _textColor          = Properties.AxisTextColor;
            _axisText           = Properties.AxisText;

            UpdateContextMenu();
            Invalidate();
        }

        private void UpdateContextMenu()
        {
            secondsToolStripMenuItem.Checked = false;
            mMSSToolStripMenuItem.Checked = false;
            hHMMSSToolStripMenuItem.Checked = false;

            switch (_timeAxisRep)
            {
                case TimeAxisRep.Seconds:
                    secondsToolStripMenuItem.Checked = true;
                    break;
                case TimeAxisRep.MinuteSeconds:
                    mMSSToolStripMenuItem.Checked = true;
                    break;
                case TimeAxisRep.HourMinuteSeconds:
                    hHMMSSToolStripMenuItem.Checked = true;
                    break;
            }
        }

        private void PoWaveChartTimeAxis_DoubleClick(object sender, EventArgs e)
        {  
            propertiesToolStripMenuItem_Click(sender, e); 
        }

        private void secondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _timeAxisRep = TimeAxisRep.Seconds;
            secondsToolStripMenuItem.Checked = true;
            mMSSToolStripMenuItem.Checked = false;
            hHMMSSToolStripMenuItem.Checked = false;
            Invalidate();
        }

        private void mMSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _timeAxisRep = TimeAxisRep.MinuteSeconds;
            secondsToolStripMenuItem.Checked = false;
            mMSSToolStripMenuItem.Checked = true;
            hHMMSSToolStripMenuItem.Checked = false;
            Invalidate();
        }

        private void hHMMSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _timeAxisRep = TimeAxisRep.HourMinuteSeconds;
            secondsToolStripMenuItem.Checked = false;
            mMSSToolStripMenuItem.Checked = false;
            hHMMSSToolStripMenuItem.Checked = true;
            Invalidate();
        }        
	}
}

