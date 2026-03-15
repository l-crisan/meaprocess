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

namespace Mp.Visual.Digital
{
	public partial class RoundButton : UserControl
	{
		#region Enumeratives
		/// <summary>
		/// Button styles
		/// </summary>
		public enum ButtonStyle
		{
			Circular = 0,
		}

		public enum ButtonType
        {
            PressButton,
            ToggleButton,
        }

		/// <summary>
		/// Button states
		/// </summary>
		public enum ButtonState
		{
			Normal = 0,
			Pressed,
		}
		#endregion
		
		#region Properties variables
		private ButtonStyle					buttonStyle = ButtonStyle.Circular;
		private ButtonState					buttonState = ButtonState.Normal;
		private Color						buttonColor = Color.LightGray;
		private	RoundButtonRenderer			renderer = null;
		private string						label = "Push";
        private ButtonType _type = new ButtonType();
        private string _name = "";
		#endregion
		
		#region Class variables
		private RectangleF			drawRect;
		protected float				drawRatio = 1.0F;
		protected RoundButtonRenderer	defaultRenderer = null;
		#endregion
		
		#region Constructor
		public RoundButton()
		{
			// Initialization
			InitializeComponent();
					
			// Set the styles for drawing
			SetStyle(ControlStyles.AllPaintingInWmPaint |
				ControlStyles.ResizeRedraw |
				ControlStyles.DoubleBuffer |
				ControlStyles.SupportsTransparentBackColor,
				true);

			// Transparent background
			this.BackColor = Color.Transparent;
						
			// Create the default renderer
			this.defaultRenderer = new RoundButtonRenderer();
			this.defaultRenderer.Button = this;			
			this.renderer = null;
			
			// Calculate the initial dimensions
			this.CalculateDimensions();
		}
		#endregion
		
		#region Properties


        [SRCategory("Button"), SRDescription("ButtonStyle")]
		public ButtonStyle Style
		{
			set 
			{ 
				this.buttonStyle = value; 
				this.Invalidate();
			}
			get { return this.buttonStyle; }
		}
		
		[SRCategory("Button"), SRDescription("ButtonColor")]
		public Color ButtonColor
		{
			get { return buttonColor; }
			set
			{
				buttonColor = value;
				Invalidate();
			}
		}

        [SRCategory("Button"), SRDescription("ButtonLabel"), Browsable(true)
		]
		public string Label
		{
			get { return this.label; }
			set
			{
				this.label = value;
				Invalidate();
			}
		}

        [SRCategory("Button"), SRDescription("ButtonState")]
		public ButtonState State
		{
			set 
			{ 
				this.buttonState = value; 
				this.Invalidate();
			}
			get { return this.buttonState; }
		}
		
		[Browsable(false)]
		public RoundButtonRenderer Renderer
		{
			get { return this.renderer; }
			set
			{
				this.renderer = value;
				if ( this.renderer != null )
					renderer.Button = this;
				Invalidate();
			}
		}
		
        [SRCategory("Button"), SRDescription("ButtonType")]        
        public ButtonType Type
        {
            get { return _type; }
            set { _type = value; }

        }
		#endregion
		
		#region Public methods
		public float GetDrawRatio()
		{
			return this.drawRatio;
		}
		#endregion

		#region Events delegates
		
		/// <summary>
		/// Font change event
		/// </summary>
		/// <param name="e"></param>
		[System.ComponentModel.EditorBrowsableAttribute()]
		protected override void OnFontChanged(EventArgs e)
		{
			// Calculate dimensions
			CalculateDimensions();
			
			// Redraw the control
			this.Invalidate();
		}
		
		/// <summary>
		/// Size change event
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSizeChanged ( EventArgs e )
		{
			base.OnSizeChanged( e );
			
			// Calculate dimensions
			CalculateDimensions();
			
			// Redraw the control
			this.Invalidate();
		}
		
		/// <summary>
		/// Paint event
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint ( PaintEventArgs e )
		{
			// Control rectangle
			RectangleF _rc = new RectangleF(0, 0, this.Width, this.Height );
			
			// Set the drawing mode
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			
			// Draw with default renderer ?
			if ( this.renderer == null )
			{
				this.defaultRenderer.DrawBackground( e.Graphics, _rc );
				this.defaultRenderer.DrawBody( e.Graphics, drawRect );
				this.defaultRenderer.DrawText( e.Graphics, drawRect );
				return;
			}

			this.renderer.DrawBackground( e.Graphics, _rc );
			this.renderer.DrawBody( e.Graphics, drawRect );
			this.renderer.DrawText( e.Graphics, drawRect );
		}
		
		/// <summary>
		/// Mouse down event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnMouseDown(object sender, MouseEventArgs e)
		{
			// Change the state
            if (_type == ButtonType.PressButton)
			    this.State = ButtonState.Pressed;

			this.Invalidate();
			
			// Call the delagates
			RoundButtonEventArgs ev = new RoundButtonEventArgs();
			ev.State = this.State;
			this.OnButtonChangeState ( ev );
		}
		
		/// <summary>
		/// Mouse up event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnMuoseUp(object sender, MouseEventArgs e)
		{
			// Change the state            
            
            if (_type == ButtonType.ToggleButton && this.State == ButtonState.Normal)
                this.State = ButtonState.Pressed;
            else
                this.State = ButtonState.Normal;

			this.Invalidate();
			
			// Call the delagates
			RoundButtonEventArgs ev = new RoundButtonEventArgs();
            
            
            ev.State = this.State;            

			this.OnButtonChangeState ( ev );
		}

		#endregion

		#region Virtual functions	
		/// <summary>
		/// Calculate the dimensions of the drawing rectangles
		/// </summary>
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
		}
		#endregion
		
		#region Fire events
		/// <summary>
		/// Event for the state changed
		/// </summary>
		public event ButtonChangeState ButtonChangeState;
		
		/// <summary>
		/// Method for call the delagetes
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnButtonChangeState( RoundButtonEventArgs e )
	    {
	        if( this.ButtonChangeState != null )
	            this.ButtonChangeState( this, e );
	    }		
		#endregion
	}

	#region Classes for event and event delagates args
	
	#region Event args class
	/// <summary>
	/// Class for events delegates
	/// </summary>
	public class RoundButtonEventArgs : EventArgs
	{
		private RoundButton.ButtonState state;
			
		public RoundButtonEventArgs()
		{			
		}
	
		public RoundButton.ButtonState State
		{
			get { return this.state; }
			set { this.state = value; }
		}
	}
	#endregion
	
	#region Delegates
	public delegate void ButtonChangeState ( object sender, RoundButtonEventArgs e );
	#endregion
	
	#endregion
}
