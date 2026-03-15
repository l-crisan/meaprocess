
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
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Mp.Visual.Digital
{
  /// <summary>
  /// Summary description for UserControl1.
  /// </summary>
  [DesignerCategory("Code")]
  [Serializable]
  public class Led : System.Windows.Forms.UserControl { 

    private Bitmap _backBuffer;


    public Led():base() 
    {
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      SetStyle(ControlStyles.DoubleBuffer, true);
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      SetStyle(ControlStyles.UserPaint, true);
      SetStyle(ControlStyles.ResizeRedraw, true);

      BackColor = Color.Transparent;

      Width  = 17;
      Height = 17;
      _backBuffer = new Bitmap(this.Width, this.Width);
    }
    
    #region new properties
    private bool _Active = true;

    [Category("Behavior"),
    DefaultValue(true)]
    public bool Active 
    {
      get { return _Active; }
      set { 
        _Active = value; 
        Invalidate();
      }
    }

    private Color _ColorOn = Color.Red;
    [Category("Appearance")]
    public Color ColorOn {
      get { return _ColorOn; }
      set { 
        _ColorOn = value; 
        Invalidate();
      }
    }

    private Color _ColorOff = SystemColors.Control;
    [Category("Appearance")]
    public Color ColorOff {
      get { return _ColorOff; }
      set { 
        _ColorOff = value; 
        Invalidate();
      }
    }

    #endregion

    
    public static Color FadeColor(Color c1, Color c2, int i1, int i2)
    {
      int r=(i1*c1.R+i2*c2.R)/(i1+i2); 
      int g=(i1*c1.G+i2*c2.G)/(i1+i2); 
      int b=(i1*c1.B+i2*c2.B)/(i1+i2); 

      return Color.FromArgb(r,g,b);
    }

    public static Color FadeColor(Color c1, Color c2)
    {
      return FadeColor(c1,c2,1,1);
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
            
        int width =  Math.Min(this.Width, this.Height);
        width = Math.Max(width, 2);

       _backBuffer = new Bitmap(width, width);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        try
        {
            base.OnPaint(e);
            Graphics g = Graphics.FromImage(_backBuffer);

            int radius = _backBuffer.Width - 1;

            if (Enabled)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;

                if (Active)
                {
                    Rectangle r = new Rectangle(0, 0, radius, radius);
                    LinearGradientBrush br1 = new LinearGradientBrush(r, ColorOn, Color.WhiteSmoke, 45);
                    g.FillEllipse(br1, 0, 0, radius, radius);
                    g.DrawEllipse(new Pen(FadeColor(ColorOn, Color.White), 1), 0, 0, radius, radius);
                    g.DrawEllipse(new Pen(Color.Black), 0, 0, radius, radius);
                }
                else
                {
                    Rectangle r = new Rectangle(0, 0, radius, radius);
                    LinearGradientBrush br1 = new LinearGradientBrush(r, ColorOff, Color.Black, 45);
                    g.FillEllipse(br1, 0, 0, radius, radius);
                    g.DrawEllipse(new Pen(FadeColor(ColorOff, Color.Black), 1), 0, 0, radius, radius);
                    g.DrawEllipse(new Pen(Color.Black), 0, 0, radius, radius);
                }
            }
            else
            {
                g.DrawEllipse(new Pen(SystemColors.ControlDark, 1), 0, 0, radius, radius);
            }

            e.Graphics.DrawImage(_backBuffer, new Point(0, 0));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
  }
}
