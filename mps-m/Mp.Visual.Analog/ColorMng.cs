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

namespace Mp.Visual.Analog
{
	internal class ColorManager : Object
	{
		public static double BlendColour ( double fg, double bg, double alpha )
		{
			double result = bg + (alpha * (fg - bg));
			if (result < 0.0)
				result = 0.0;
			if (result > 255)
				result = 255;
			return result;
		}

		public static Color StepColor ( Color clr, int alpha )
		{
			if ( alpha == 100 )
				return clr;
			
			byte a = clr.A;
			byte r = clr.R;
			byte g = clr.G;
			byte b = clr.B;
			float bg = 0;
				
			int _alpha = Math.Min(alpha, 200);
			_alpha = Math.Max(alpha, 0);
			double ialpha = ((double)(_alpha - 100.0))/100.0;
		    
			if (ialpha > 100)
			{
				// blend with white
				bg = 255.0F;
				ialpha = 1.0F - ialpha;  // 0 = transparent fg; 1 = opaque fg
			}
			else
			{
				// blend with black
				bg = 0.0F;
				ialpha = 1.0F + ialpha;  // 0 = transparent fg; 1 = opaque fg
			}
		    
			r = (byte)(ColorManager.BlendColour(r, bg, ialpha));
			g = (byte)(ColorManager.BlendColour(g, bg, ialpha));
			b = (byte)(ColorManager.BlendColour(b, bg, ialpha));
	    
			return Color.FromArgb ( a, r, g, b );
		}
	};
}
