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
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

namespace Mp.Visual.WaveChart
{
	/// <summary>
	/// This class implements the Y-Axis for a wave chart.
	/// </summary>
	public class YAxis : System.Windows.Forms.UserControl
    {
        private IContainer components;

        private List<Signal> _signals;
        private Rectangle _chartRect;
        private Font _font = new Font("Arial", 10);
        private int _maxScrollPos;
        private System.Windows.Forms.Timer _updateLoadTimer = new System.Windows.Forms.Timer();

        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem propertiesToolStripMenuItem;
        private HScrollBar hScrollBar;

        /// <summary>
        /// Default constructor.
        /// </summary>
		public YAxis()
		{ 
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

		#region Vom Komponenten-Designer generierter Code
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(YAxis));
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // hScrollBar
            // 
            this.hScrollBar.AccessibleDescription = null;
            this.hScrollBar.AccessibleName = null;
            resources.ApplyResources(this.hScrollBar, "hScrollBar");
            this.hScrollBar.BackgroundImage = null;
            this.hScrollBar.Font = null;
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnHScroll);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.AccessibleDescription = null;
            this.contextMenuStrip.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.contextMenuStrip.BackgroundImage = null;
            this.contextMenuStrip.Font = null;
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
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
            // YAxis
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = null;
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.hScrollBar);
            this.Font = null;
            this.Name = "YAxis";
            this.Load += new System.EventHandler(this.OnLoad);
            this.DoubleClick += new System.EventHandler(this.PoWaveChartYAxis_DoubleClick);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		protected string GetFormatedScaleText( double value, Signal signal )
		{
            return value.ToString("f" + signal.YAxisPrecision.ToString());
		}

		private void DrawAxis(Graphics g)
		{
            if (_signals == null)
				return;

			uint				    nVisibleSignals = 0;
			SolidBrush			    br = new SolidBrush(BackColor);
			Signal	                signal;	
			Pen					    dPen;
			int					    nPosInc			= 40;
			int					    nPos			= 20 - hScrollBar.Value;	
			int					    nMaxTextExt		= 0;
			SizeF				    size;
			string				    strText;
			int					    textExtend;
			SolidBrush			    TextBrush = new SolidBrush(BackColor);
			StringFormat		    drawFormat = new StringFormat();
			Matrix				    matrixRot270;
			Matrix				    oldMatrix;
			

			drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;	

			//Clear the Axis
			g.FillRectangle(br,ClientRectangle);

			//Draw the signal axis
            for (int nSig = 0; nSig < _signals.Count; nSig++)
			{
                signal = _signals[nSig];
				
				if( !signal.Visible )
					continue;

                TextBrush.Color = signal.LineColor;

				nVisibleSignals++;
				//Select the pen
                dPen = new Pen(signal.LineColor);
				
				//Draw axis
                g.DrawLine(dPen, new Point(ClientRectangle.Right - nPos, _chartRect.Top),
                    new Point(ClientRectangle.Right - nPos, _chartRect.Bottom));
	
				//Draw text degrees
                strText = GetFormatedScaleText(signal.Maximum, signal);
				
				size = g.MeasureString(strText,_font);

				textExtend = (int) size.Width;
		
				if(nMaxTextExt < textExtend)
					nMaxTextExt = textExtend;

                double nDegreeInc = _chartRect.Height / (double)signal.YAxisDivision;
				double nDegreePos = 0;

                double dbPosInc = (signal.Maximum - signal.Minimum) / signal.YAxisDivision;
                double dbDgree = signal.Maximum;
		
				for( int n = 0; n < signal.YAxisDivision; n++)
				{
					//Degree text
					strText = GetFormatedScaleText( dbDgree, signal );
                    size = g.MeasureString(strText, _font);
					textExtend = (int)size.Width;
		
					if(nMaxTextExt < textExtend)
						nMaxTextExt = textExtend;

                    g.DrawString(strText, _font, TextBrush, new Point(ClientRectangle.Right - nPos - textExtend - 5, (int)(_chartRect.Top + nDegreePos)));

					//Degree line
					if(n == 0)
					{
                        g.DrawLine(dPen, new Point(ClientRectangle.Right - nPos - 10, (int)(_chartRect.Top + nDegreePos)),
                            new Point(ClientRectangle.Right - nPos + 10, (int)(_chartRect.Top + nDegreePos)));
					}
					else
					{
                        g.DrawLine(dPen, new Point(ClientRectangle.Right - nPos - 5, (int)(_chartRect.Top + nDegreePos)),
                            new Point(ClientRectangle.Right - nPos + 5, (int)(_chartRect.Top + nDegreePos)));

					}
					nDegreePos += nDegreeInc;
					dbDgree -= dbPosInc;
				}

				//Last degree
                g.DrawLine(dPen, new Point(ClientRectangle.Right - nPos - 10, (int)(_chartRect.Top + nDegreePos)),
                    new Point(ClientRectangle.Right - nPos + 10, (int)(_chartRect.Top + nDegreePos)));


				//Last degree text
                
                //Measure text min and max
                strText = GetFormatedScaleText(signal.Minimum, signal);
                string maxText = GetFormatedScaleText(signal.Maximum, signal);

                size = g.MeasureString(strText, _font);
                SizeF maxSize = g.MeasureString(maxText, _font);

                textExtend = (int)size.Width;
		
				if(nMaxTextExt < textExtend)
					nMaxTextExt = textExtend;


                g.DrawString(strText, _font, TextBrush, new Point(ClientRectangle.Right - nPos - textExtend - 5, (int)(_chartRect.Bottom - size.Height - 1)));

				//Signal name and unit
				strText = signal.Name;
				if(signal.Unit != "")
				{
					strText += " (";
					strText += signal.Unit;
					strText += ")";
				}
                
                textExtend = (int)Math.Max(size.Width, maxSize.Width);

                size = g.MeasureString(strText, _font);

                Point pt = new Point((int)(ClientRectangle.Right - nPos - size.Height - textExtend - 5), (int)(_chartRect.Top + (_chartRect.Height / 2) + (size.Width / 2)));

				oldMatrix	= g.Transform;
				matrixRot270 = new Matrix();
				matrixRot270.RotateAt(270, pt);
				g.Transform = matrixRot270;

                g.DrawString(strText, _font, TextBrush, pt);
				g.Transform = oldMatrix;

                nPos += (nPosInc + textExtend);
			}
            _maxScrollPos = hScrollBar.Value + nPos;
		}


        /// <summary>
        /// Sets the chart signals.
        /// </summary>    
        public List<Signal> Signals
        {
            set 
            { 
                _signals = value; 
            }
        }

        /// <summary>
        ///  Sets the chart rectangle
        /// </summary>
        public Rectangle ChartRectangle
        {
            set 
            { 
                _chartRect = value; 
            }
        }
        
        /// <summary>
        /// Sets the text font.
        /// </summary>
        public new Font Font
        {
            set 
            { 
                if( value != null)
                    _font = value; 
            }
        }

		protected override void OnPaint(PaintEventArgs e)
		{         
			DrawAxis(e.Graphics);
		}

		private void OnHScroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			Invalidate();
		}

		private void OnLoad(object sender, System.EventArgs e)
		{
			hScrollBar.Minimum = 0;
            hScrollBar.Maximum = _maxScrollPos;

            if (_updateLoadTimer != null)
            {
                _updateLoadTimer.Interval = 500;
                _updateLoadTimer.Tick += new EventHandler(UpdateLoad);

                _updateLoadTimer.Enabled = true;
            }
            Invalidate();        
		}
        
        private delegate void SetScrollMaximumDelegate();

        private void UpdateLoad(object sender, System.EventArgs e)
        {
            _updateLoadTimer.Enabled = false;
            _updateLoadTimer = null;
            hScrollBar.Maximum = _maxScrollPos;
        }

        private void SetScrollMaximum()
        {
            int value = hScrollBar.Value;
            hScrollBar.Value = 0;
            hScrollBar.Maximum = _maxScrollPos;
            hScrollBar.Value = Math.Min(value, _maxScrollPos);
        }

        public void UpdateYAxis()
        {
            Invalidate();
            
            try
            {                
                BeginInvoke(new SetScrollMaximumDelegate(SetScrollMaximum),null);            
            }
            catch(Exception ex)
            {
                SetScrollMaximum();
            }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            YAxisPropDlg YAxisPropDlg = new YAxisPropDlg();
            YAxisPropDlg.OnChangeProperties = new YAxisPropDlg.ChangeProperties(OnChangeProperties);
            YAxisPropDlg.DialogProperties.BackColor = BackColor;
            YAxisPropDlg.DialogProperties.ChartName = this.Text;
            YAxisPropDlg.ShowDialog();
        }

        private void OnChangeProperties(YAxisPropDlg.PoDialogProperties Properties)
        {
            BackColor = Properties.BackColor;
        }

        private void PoWaveChartYAxis_DoubleClick(object sender, EventArgs e)
        {
            propertiesToolStripMenuItem_Click(sender, e);
        }
	}
}
