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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Mp.Visual.Analog
{
    [Serializable]
	public partial class Knob : UserControl
	{
		#region Enumerators
		public enum KnobStyle
		{
			Circular = 0,
		}
		#endregion
		
		#region Properties variables
		private float			minValue = 0.0F;
		private float			maxValue = 1.0F;
		private float			stepValue = 0.1F;
		private float			currValue = 0.0F;
		private KnobStyle		style = KnobStyle.Circular;
		private KnobRenderer	renderer = null;
		private Color			scaleColor = Color.DarkGray;
		private Color			knobColor = Color.LightGray ;
		private Color			indicatorColor = Color.Black;
		private float			indicatorOffset = 10F;
		#endregion
		
		#region Class variables
		private RectangleF		drawRect;
		private RectangleF		rectScale;
		private RectangleF		rectKnob;
		private float			drawRatio;
		private KnobRenderer	defaultRenderer = null;
		private bool			isKnobRotating = false;
		private PointF			knobCenter;
		private PointF			knobIndicatorPos;
		#endregion		
		
		#region Constructor
		public Knob()
		{
			InitializeComponent();
			
			// Set the styles for drawing
			SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.ResizeRedraw |
				ControlStyles.DoubleBuffer |
				ControlStyles.SupportsTransparentBackColor,
				true);

			// Transparent background
			this.BackColor = Color.Transparent;
			
			this.defaultRenderer = new KnobRenderer();
			this.defaultRenderer.Knob = this;
			
			this.CalculateDimensions();
		}
		#endregion
		
		#region Properties
		
        [SRCategory("Knob"), SRDescription("KnobMinValue")]
		public float MinValue
		{
			set 
			{                
				this.minValue = value;
				this.Invalidate();
			}
			get { return this.minValue; }
		}

        [SRCategory("Knob"), SRDescription("KnobMaxValue")]
		public float MaxValue
		{
			set 
			{ 
				this.maxValue = value;
				this.Invalidate();
			}
			get { return this.maxValue; }
		}


        [SRCategory("Knob"), SRDescription("KnobStepValue")]
		public float StepValue
		{
			set 
			{ 
				this.stepValue = value;
				this.Invalidate();
			}
			get { return this.stepValue; }
		}

        [SRCategory("Knob"), SRDescription("KnobCurValue")]
		public float Value
		{
			set 
			{ 
				this.currValue = value;
				this.knobIndicatorPos = this.GetPositionFromValue ( this.currValue );
				this.Invalidate();
					
				LBKnobEventArgs e = new LBKnobEventArgs();
				e.Value = this.currValue;
				this.OnKnobChangeValue( e );
			}
			get { return this.currValue; }
		}

        [SRCategory("Knob"), SRDescription("KnobStyle")]
		public KnobStyle Style
		{
			set 
			{ 
				this.style = value;
				this.Invalidate();
			}
			get { return this.style; }
		}


        [SRCategory("Knob"), SRDescription("KnobColor")]
		public Color KnobColor
		{
			set 
			{ 
				this.knobColor = value;
				this.Invalidate();
			}
			get { return this.knobColor; }
		}

        [SRCategory("Knob"), SRDescription("KnobScaleColor")]
		public Color ScaleColor
		{
			set 
			{ 
				this.scaleColor = value;
				this.Invalidate();
			}
			get { return this.scaleColor; }
		}

        [SRCategory("Knob"), SRDescription("KnobIndicatorColor")]
		public Color IndicatorColor
		{
			set 
			{ 
				this.indicatorColor = value;
				this.Invalidate();
			}
			get { return this.indicatorColor; }
		}

        [SRCategory("Knob"), SRDescription("KnobIndicatorBorderOffset")]
		public float IndicatorOffset
		{
			set 
			{ 
				this.indicatorOffset = value;
				this.CalculateDimensions();
				this.Invalidate();
			}
			get { return this.indicatorOffset; }
		}
		
		[Browsable(false)]
		public KnobRenderer Renderer
		{
			get { return this.renderer; }
			set
			{
				this.renderer = value;
				if ( this.renderer != null )
					renderer.Knob = this;
				Invalidate();
			}
		}
		
		[Browsable(false)]
		public PointF KnobCenter
		{
			get { return this.knobCenter; }
		}
		#endregion

		#region Events delegates
		
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			bool blResult = true;

			/// <summary>
			/// Specified WM_KEYDOWN enumeration value.
			/// </summary>
			const int WM_KEYDOWN = 0x0100;

			/// <summary>
			/// Specified WM_SYSKEYDOWN enumeration value.
			/// </summary>
			const int WM_SYSKEYDOWN = 0x0104;

			float val = this.Value;
			
			if ((msg.Msg == WM_KEYDOWN) || (msg.Msg == WM_SYSKEYDOWN))
			{
				switch(keyData)
				{
					case Keys.Up:
						val += this.StepValue;
						if ( val <= this.MaxValue )
							this.Value = val;
						break;

					case Keys.Down:
						val -= this.StepValue;
						if ( val >= this.MinValue )
							this.Value = val;
						break;
						
					case Keys.PageUp:
						if ( val <  this.MaxValue )
						{
							val += ( this.StepValue * 10 );
							this.Value = val;
						}
						break;
						
					case Keys.PageDown:
						if ( val > this.MinValue )
						{
							val -= ( this.StepValue * 10 );
							this.Value = val;
						}
						break;

					case Keys.Home:
						this.Value = this.MinValue;
						break;
						
					case Keys.End:
						this.Value = this.MaxValue;
						break;

					default:
						blResult = base.ProcessCmdKey(ref msg,keyData);
						break;
				}
			}

			return blResult;
		}
		
		[System.ComponentModel.EditorBrowsableAttribute()]
		protected override void OnClick(EventArgs e)
		{
			this.Focus();
			this.Invalidate();
			base.OnClick(e);
		}
		
		void OnMouseUp(object sender, MouseEventArgs e)
		{
			this.isKnobRotating = false;
			
			if ( this.rectKnob.Contains ( e.Location ) == false )
				return;

            try
            {
                float val = this.GetValueFromPosition(e.Location);
                if (val != this.Value)
                {
                    this.Value = val;
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
		}
		
		void OnMouseDown(object sender, MouseEventArgs e)
		{
			if ( this.rectKnob.Contains ( e.Location ) == false )
				return;
			
			this.isKnobRotating = true;
			
			this.Focus();
		}
		
		void OnMouseMove(object sender, MouseEventArgs e)
		{
			if ( this.isKnobRotating == false )
				return;
            try
            {
                float val = this.GetValueFromPosition(e.Location);
                if (val != this.Value)
                {
                    this.Value = val;
                    this.Invalidate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
		}
		
		void OnKeyDown(object sender, KeyEventArgs e)
		{
			float val = this.Value;

			switch ( e.KeyCode )
			{
				case Keys.Up:
					val = this.Value + this.StepValue;
					break;

				case Keys.Down:
					val = this.Value - this.StepValue;
					break;
			}

			if ( val < this.MinValue )
				val = this.MinValue;
			   
			if ( val > this.MaxValue )
				val = this.MaxValue;
			
			this.Value = val;
		}

		
		[System.ComponentModel.EditorBrowsableAttribute()]
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			
			this.CalculateDimensions();
			
			this.Invalidate();
		}
		
		[System.ComponentModel.EditorBrowsableAttribute()]
		protected override void OnPaint(PaintEventArgs e)
		{
            RectangleF kr = this.rectScale;
            kr.Height -= 2;
            kr.Width -= 2;

			RectangleF _rc = new RectangleF(0, 0, this.Width, this.Height );
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			
			if ( this.Renderer == null )
			{
				this.defaultRenderer.DrawBackground( e.Graphics, _rc );
                this.defaultRenderer.DrawScale(e.Graphics, kr);
				this.defaultRenderer.DrawKnob( e.Graphics, this.rectKnob );
				this.defaultRenderer.DrawKnobIndicator( e.Graphics, this.rectKnob, this.knobIndicatorPos );
				return;
			}
		
			this.Renderer.DrawBackground( e.Graphics, _rc );
            this.Renderer.DrawScale(e.Graphics, kr);
			this.Renderer.DrawKnob( e.Graphics, this.rectKnob );
            this.Renderer.DrawKnobIndicator(e.Graphics, this.rectKnob, this.knobIndicatorPos);
		}
		#endregion
		
		#region Virtual functions		
		protected virtual void CalculateDimensions()
		{
			// Rectangle
			float x, y, w, h;
			x = 0;
			y = 0;
			w = this.Size.Width;
			h = this.Size.Height;
			
			// Calculate ratio
			drawRatio = (Math.Min(w,h)) / 200;
			if ( drawRatio == 0.0 )
				drawRatio = 1;
		
			// Draw rectangle
			drawRect.X = x;
			drawRect.Y = y;
			drawRect.Width = w - 2;
			drawRect.Height = h - 2;
		
			if ( w < h )
				drawRect.Height = w;
			else if ( w > h )
				drawRect.Width = h;
			
			if ( drawRect.Width < 10 )
				drawRect.Width = 10;
			if ( drawRect.Height < 10 )
				drawRect.Height = 10;
			
			this.rectScale = this.drawRect;
			this.rectKnob = this.drawRect;
			this.rectKnob.Inflate ( -20 * this.drawRatio, -20 * this.drawRatio );
			
			this.knobCenter.X = this.rectKnob.Left + ( this.rectKnob.Width * 0.5F );
			this.knobCenter.Y = this.rectKnob.Top + ( this.rectKnob.Height * 0.5F );	
			
			this.knobIndicatorPos = this.GetPositionFromValue ( this.Value );
		}
		
		public virtual float GetValueFromPosition ( PointF position )
		{
			float degree = 0.0F;
			float v = 0.0F;

            
			PointF center = this.KnobCenter;
			
			if ( position.X <= center.X )
			{
                if ((center.X - position.X) == 0)
                    throw new Exception();

				degree  = (center.Y - position.Y ) /  (center.X - position.X );
				degree = (float)Math.Atan(degree);
				degree = (float)((degree) * (180F / Math.PI) + 45F);
                v = (degree * (this.MaxValue - this.MinValue) / 270F) + this.MinValue;
			}
			else
			{
				if ( position.X > center.X )
				{
                    if ((position.X - center.X) == 0)
                        throw new Exception();

					degree  = (position.Y - center.Y ) /  (position.X - center.X );
					degree = (float)Math.Atan(degree);
					degree = (float)(225F + (degree) * (180F / Math.PI));
                    v = (degree * (this.MaxValue - this.MinValue) / 270F) + this.MinValue;
				}
			}
		
			if ( v > this.MaxValue )
				v = this.MaxValue;
		
			if (v < this.MinValue )
				v = this.MinValue;
		
			return v;					
		}
		
		public virtual PointF GetPositionFromValue ( float val )
		{
			PointF pos = new PointF( 0.0F, 0.0F );
			
				// Elimina la divisione per 0
			if ( ( this.MaxValue - this.MinValue ) == 0 )	
				return pos;
				
            float degree = 270f * (val - this.MinValue) / (this.MaxValue - this.MinValue);
			degree = (degree + 135F) * (float)Math.PI / 180F;
		
			pos.X = (int)(Math.Cos(degree) * ((this.rectKnob.Width * 0.5F)- this.indicatorOffset ) + this.rectKnob.X + ( this.rectKnob.Width * 0.5F));
            pos.Y = (int)(Math.Sin(degree) * ((this.rectKnob.Width * 0.5F) - this.indicatorOffset) + this.rectKnob.Y + (this.rectKnob.Height * 0.5F));
		
			return pos;
		}
		#endregion

		#region Fire events
		public event KnobChangeValue KnobChangeValue;
		protected virtual void OnKnobChangeValue( LBKnobEventArgs e )
	    {
	        if( this.KnobChangeValue != null )
	            this.KnobChangeValue( this, e );
	    }		
		#endregion
	}

	#region Classes for event and event delagates args
	
	#region Event args class
	/// <summary>
	/// Class for events delegates
	/// </summary>
	public class LBKnobEventArgs : EventArgs
	{
		private float val;
			
		public LBKnobEventArgs()
		{			
		}
	
		public float Value
		{
			get { return this.val; }
			set { this.val = value; }
		}
	}
	#endregion
	
	#region Delegates
	public delegate void KnobChangeValue ( object sender, LBKnobEventArgs e );
	#endregion
	
	#endregion
}
