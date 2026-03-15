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
using System.Drawing.Drawing2D;

namespace Mp.Visual.Digital	
{
	public class RoundButtonRenderer
	{
		#region Variables
		/// <summary>
		/// Control to render
		/// </summary>
		private RoundButton button = null;
		#endregion
		
		#region Properies
		public RoundButton Button
		{
			set { this.button = value; }
			get { return this.button; }
		}
		#endregion
		
		#region Virtual method
		/// <summary>
		/// Draw the background of the control
		/// </summary>
		/// <param name="Gr"></param>
		/// <param name="rc"></param>
		/// <returns></returns>
		public virtual bool DrawBackground( Graphics Gr, RectangleF rc )
		{
			if ( this.Button == null )
				return false;
			
			Color c = this.Button.BackColor;
			SolidBrush br = new SolidBrush ( c );
			Pen pen = new Pen ( c );
			
			Rectangle _rcTmp = new Rectangle(0, 0, this.Button.Width, this.Button.Height );
			Gr.DrawRectangle ( pen, _rcTmp );
			Gr.FillRectangle ( br, rc );
			
			br.Dispose();
			pen.Dispose();
			
			return true;
		}
		
		/// <summary>
		/// Draw the body of the control
		/// </summary>
		/// <param name="Gr"></param>
		/// <param name="rc"></param>
		/// <returns></returns>
		public virtual bool DrawBody( Graphics Gr, RectangleF rc )
		{
			if ( this.Button == null )
				return false;
			
			Color bodyColor = this.Button.ButtonColor;
			Color cDark = LBColorManager.StepColor ( bodyColor, 20 );
			
			LinearGradientBrush br1 = new LinearGradientBrush ( rc, 
			                                                   bodyColor,
			                                                   cDark,
			                                                   45 );
			Gr.FillEllipse ( br1, rc );
			
			br1.Dispose();
			
			if ( this.Button.State == RoundButton.ButtonState.Pressed )
			{			
				float drawRatio = this.Button.GetDrawRatio();
				
				RectangleF _rc = rc;
				_rc.Inflate ( -15F * drawRatio, -15F * drawRatio );
				LinearGradientBrush br2 = new LinearGradientBrush ( _rc, 
				                                                   cDark,
				                                                   bodyColor,
				                                                   45 );
				Gr.FillEllipse ( br2, _rc );
				
				br2.Dispose();
			}
			
			return true;
		}
		
		/// <summary>
		/// Draw the text of the control
		/// </summary>
		/// <param name="Gr"></param>
		/// <param name="rc"></param>
		/// <returns></returns>
		public virtual bool DrawText( Graphics Gr, RectangleF rc )
		{
			if ( this.Button == null )
				return false;
			
			float drawRatio = this.Button.GetDrawRatio();
			
			//Draw Strings
       		Font font = new Font ( this.Button.Font.FontFamily, this.Button.Font.Size * drawRatio );

	        String str = this.Button.Label;
	
	        Color bodyColor = this.Button.ButtonColor;
			Color cDark = LBColorManager.StepColor ( bodyColor, 20 );

			SizeF size = Gr.MeasureString ( str, font );
			
			SolidBrush br1 = new SolidBrush ( bodyColor );
			SolidBrush br2 = new SolidBrush ( cDark );
			
			Gr.DrawString ( str, 
			                font, 
			                br1, 
			                rc.Left + ( ( rc.Width * 0.5F ) - (float)( size.Width * 0.5F ) ) + ( 1 * drawRatio ),
			                rc.Top + ( ( rc.Height * 0.5F ) - (float)( size.Height * 0.5 ) ) + ( 1 * drawRatio ) );
			
			Gr.DrawString ( str, 
			                font, 
			                br2, 
			                rc.Left + ( ( rc.Width * 0.5F ) - (float)( size.Width * 0.5F ) ),
			                rc.Top + ( ( rc.Height * 0.5F ) - (float)( size.Height * 0.5 ) ) );
			
			br1.Dispose();
			br2.Dispose();
			font.Dispose();
			
			return false;
		}
		#endregion
	}
}
