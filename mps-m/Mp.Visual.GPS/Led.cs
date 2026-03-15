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
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Mp.Visual.GPS
{
  /// <summary>
  /// Summary description for UserControl1.
  /// </summary>
  [DesignerCategory("Code")]
  public class Led : System.Windows.Forms.Control { 

    private Timer tick;
    private Bitmap _backBuffer;


    public Led():base() {
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      SetStyle(ControlStyles.DoubleBuffer, true);
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      SetStyle(ControlStyles.UserPaint, true);
      SetStyle(ControlStyles.ResizeRedraw, true);

      BackColor = Color.Transparent;

      Width  = 17;
      Height = 17;

      tick = new Timer();
      tick.Enabled = false;
      tick.Tick += new System.EventHandler(this._Tick);
      _backBuffer = new Bitmap(this.Width, this.Height);
    }
    
    #region new properties
    private bool _Active = true;
    [Category("Behavior"),
    DefaultValue(true)]
    public bool Active {
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

    private bool _Flash = false;
    [Category("Behavior"),
    DefaultValue(false)]
    public bool Flash {
      get { return _Flash; }
      set { 
        _Flash = value && (flashIntervals.Length>0); 
        tickIndex = 0;
        tick.Interval = flashIntervals[tickIndex];
        tick.Enabled = _Flash;
        Active = true;
      }
    }

    private string _FlashIntervals="250";
    public int [] flashIntervals = new int[1] {250};
    [Category("Appearance"),
    DefaultValue("250")]
    public string FlashIntervals {
      get { return _FlashIntervals; }
      set { 
        _FlashIntervals = value; 
        string [] fi = _FlashIntervals.Split(new char[] {',','/','|',' ','\n'});
        flashIntervals = new int[fi.Length];
        for (int i=0; i<fi.Length; i++)
          try {
            flashIntervals[i] = int.Parse(fi[i]);
          } catch {
            flashIntervals[i] = 25;
          }
      }
    }

    private string _FlashColors=string.Empty; 
    public Color [] flashColors;
    [Category("Appearance"),
    DefaultValue("")]
    public string FlashColors {
      get { return _FlashColors; }
      set { 
        _FlashColors = value; 
        if (_FlashColors==string.Empty) {
          flashColors=null;
        } else {
          string [] fc = _FlashColors.Split(new char[] {',','/','|',' ','\n'});
          flashColors = new Color[fc.Length];
          for (int i=0; i<fc.Length; i++)
            try {
              flashColors[i] = (fc[i]!="")?Color.FromName(fc[i]):Color.Empty;
            } catch {
              flashColors[i] = Color.Empty;
            }
        }
      }
    }

    #endregion

    #region helper color functions
    public static Color FadeColor(Color c1, Color c2, int i1, int i2) {
      int r=(i1*c1.R+i2*c2.R)/(i1+i2); 
      int g=(i1*c1.G+i2*c2.G)/(i1+i2); 
      int b=(i1*c1.B+i2*c2.B)/(i1+i2); 

      return Color.FromArgb(r,g,b);
    }

    public static Color FadeColor(Color c1, Color c2) {
      return FadeColor(c1,c2,1,1);
    }
    #endregion

    public new event PaintEventHandler Paint;

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if(this.Width != 0 && this.Height != 0)
            _backBuffer = new Bitmap(this.Width, this.Height);
    }
    protected override void OnPaint(PaintEventArgs e) {
        try
        {
            if (null != Paint) Paint(this, e);
            else
            {
                base.OnPaint(e);
                Graphics g = Graphics.FromImage(_backBuffer);

                //        e.Graphics.Clear(BackColor);
                if (Enabled)
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    if (Active)
                    {
                        Rectangle r = new Rectangle(1, 1, Width - 3, Height - 3);
                        LinearGradientBrush br1 = new LinearGradientBrush(r, ColorOn, Color.WhiteSmoke, 45);
                        g.FillEllipse(br1, 1, 1, Width - 3, Height - 3);
                        //e.Graphics.DrawArc(new Pen(FadeColor(ColorOn, Color.White, 1, 2), 2), 3, 3, Width - 7, Height - 7, -90.0F, -90.0F);
                        g.DrawEllipse(new Pen(FadeColor(ColorOn, Color.White), 1), 1, 1, Width - 3, Height - 3);
                        g.DrawEllipse(new Pen(Color.Black), 1, 1, Width - 3, Height - 3);
                    }
                    else
                    {
                        Rectangle r = new Rectangle(1, 1, Width - 3, Height - 3);
                        LinearGradientBrush br1 = new LinearGradientBrush(r, ColorOff, Color.Black, 45);
                        g.FillEllipse(br1, 1, 1, Width - 3, Height - 3);
                        //e.Graphics.DrawArc(new Pen(FadeColor(ColorOff, Color.Black, 2, 1), 2), 3, 3, Width - 7, Height - 7, 0.0F, 90.0F);
                        g.DrawEllipse(new Pen(FadeColor(ColorOff, Color.Black), 1), 1, 1, Width - 3, Height - 3);
                        g.DrawEllipse(new Pen(Color.Black), 1, 1, Width - 3, Height - 3);
                    }
                }
                else
                {
                    g.DrawEllipse(new Pen(System.Drawing.SystemColors.ControlDark, 1), 1, 1, Width - 3, Height - 3);
                }

                e.Graphics.DrawImage(_backBuffer, new Point(0, 0));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public int tickIndex;
    private void _Tick(object sender, System.EventArgs e) {
      tickIndex=(++tickIndex)%(flashIntervals.Length);
      tick.Interval=flashIntervals[tickIndex];
      try {
        if ((flashColors==null)||(flashColors.Length<tickIndex)||(flashColors[tickIndex]==Color.Empty))
          Active = !Active;
        else {
          ColorOn = flashColors[tickIndex];
          Active=true;
        }
      } catch {
        Active = !Active;
      }
    }

  }
}
