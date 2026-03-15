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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Mp.Visual.Analog
{
  /// <summary>
  /// A termometer control
  /// </summary>
  [ToolboxBitmap(typeof(ResourceLocator), "Manometers.Resources.ThermometerIcon.bmp")]
  public class Manometer : ManometerBase
  {
    #region -- Members --

    private RectangleF shadowRect, backgroundRect, numberRect, bar1Rect, bar2Rect, bar3Rect;
    private RectangleF arrow1Rect, arrow2Rect;
    private int numberSpacing = defaultSpacing;
    private Color backColor = Color.Peru;
    private int borderWidth;
    private Color colorArrow;
    private Color barColor = Color.Black;
    private bool clockWise = true;
    private int decimals;
    private int barsBetweenNumbers = defaultBarsBetweenNumbers;
    private Brush textureBrush;

    //Constants
    private const int defaultWidth = 100;
    private const int defaultHeight = 100;
    private const int defaultFontSize = 11;
    private const int defaultMax = 10;
    private const int defaultMin = -10;
    private const int defaultLightingAngle = 90;
    private const int defaultBorderWidth = 6;
    private const int defaultDecimals = 0;
    private const int defaultSpacing = 30;
    private const int barOuterMargin = 12;
    private const int barInnerMargin = 4;
    private const int defaultBarHeight = 5;
    private const int defaultBarWidth = 3;
    private const int defaultBarsBetweenNumbers = 5;
    private const int defaultInnerShadowWidth = 2;
    private const int defaultOuterShadowWidth = 2;
    private const int numberMargin = barOuterMargin + barInnerMargin + defaultBorderWidth + defaultBarHeight;

    #endregion

    #region -- Properties --

    /// <summary>
    /// Gets or sets the decimals used for the numbers
    /// </summary>
    /// <value>The decimals.</value>
    [Browsable(true)]
    [SRDescription("Decimals")]
    [Category("Appearance")]
    [DefaultValue(defaultDecimals)]
    public int Decimals
    {
      get { return decimals; }
      set { decimals = value; Invalidate(); }
    }

    /// <summary>
    /// Gets or sets the space between numbers in degrees.
    /// </summary>
    /// <value>The number spacing.</value>
    [Browsable(true)]
    [DefaultValue(defaultSpacing)]
    [SRDescription("NumberSpacing")]
    [Category("Layout")]
    [Localizable(true)]
    public int NumberSpacing
    {
      get { return numberSpacing; }
      set
      {
        if (numberSpacing <= 0)
          Debug.Assert(false, "Number interval is less than 0");
        else
        {
          numberSpacing = value; Invalidate();
        }
      }
    }

    /// <summary>
    /// Gets or sets the background color for the control, this property is not relevant for this control.
    /// </summary>
    /// <value></value>
    /// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
    /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
    [Browsable(true)]
    [SRDescription("BackColor")]
    [Category("Appearance")]
    [DefaultValue(typeof(Color), "Peru")]
    public new Color BackColor
    {
      get { return backColor; }
      set { backColor = value; Invalidate(); }
    }

    /// <summary>
    /// Gets or sets the width of the border.
    /// </summary>
    /// <value>The width of the border.</value>
    [Browsable(true)]
    [SRDescription("BorderWidth")]
    [Category("Appearance")]
    [DefaultValue(defaultBorderWidth)]
    public int BorderWidth
    {
      get { return borderWidth; }
      set
      {
        if ((value < 0) || value > 10)
        {
          Debug.Assert(false, "Value must be between 0 and 10");
          value = defaultBorderWidth;
        }
        borderWidth = value;
        Invalidate();
      }
    }

    /// <summary>
    /// Gets or sets the color of the arrow.
    /// </summary>
    /// <value>The color of the arrow.</value>
    [Browsable(true)]
    [SRDescription("ArrowColor")]
    [Category("Appearance")]
    [DefaultValue(typeof(Color), "Black")]
    [Localizable(true)]
    public Color ArrowColor
    {
      get { return colorArrow; }
      set { colorArrow = value; Invalidate(); }
    }

    /// <summary>
    /// Set to true if the layout should be clockwise.
    /// </summary>
    /// <value>true if Clockwise</value>
    [Browsable(true)]
    [SRDescription("ClockWise")]
    [Category("Layout")]
    [DefaultValue(true)]
    [Localizable(true)]
    public bool ClockWise
    {
      get { return clockWise; }
      set { clockWise = value; Invalidate(); }
    }

    /// <summary>
    /// Number of bars between the numbers.
    /// </summary>
    /// <value>true if Clockwise</value>
    [Browsable(true)]
    [SRDescription("BarNumber")]
    [Category("Layout")]
    [DefaultValue(defaultBarsBetweenNumbers)]
    public int BarsBetweenNumbers
    {
      get { return barsBetweenNumbers; }
      set
      {
        if (value >= 1 && value <= NumberSpacing)
          barsBetweenNumbers = value;
        Invalidate();
      }
    }

    #endregion

    #region -- Constructor --
    /// <summary>
    /// Initializes a new instance of the <see cref="Manometer"/> class.
    /// </summary>
    /// <remarks></remarks>
    public Manometer()
    {
      //Styles
      SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
      SetStyle(ControlStyles.AllPaintingInWmPaint, true);
      SetStyle(ControlStyles.UserPaint, true);
      SetStyle(ControlStyles.ResizeRedraw, true);
      SetStyle(ControlStyles.ContainerControl, false);
      SetStyle(ControlStyles.SupportsTransparentBackColor, true);
      InitializeComponent();
      textureBrush = new TextureBrush(Resources.Reflection);
    }

    private void InitializeComponent()
    {
      Name = "Termometer";
      Max = defaultMax;
      Min = defaultMin;
      ClockWise = true;
      BarsBetweenNumbers = defaultBarsBetweenNumbers;
      ArrowColor = Color.Black;
      BorderWidth = defaultBorderWidth;
      Decimals = defaultDecimals;
      NumberSpacing = defaultSpacing;
      StoreMax = true;
      StoredMax = defaultMin;
      Width = defaultWidth;
      Height = defaultHeight;
      TextUnit = "°C";
      Font = new Font("Calibri", defaultFontSize, GraphicsUnit.Point);
      CalcRectangles();
      Resize += new EventHandler(Termometer_Resize);
      base.BackColor = Color.Transparent;
    }

    #endregion

    #region -- EventHandlers --

    private void Termometer_Resize(object sender, EventArgs e)
    {
      CalcRectangles();
    }

    #endregion

    #region -- Protected Overrides --

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
    protected override void OnPaint(PaintEventArgs e)
    {
      // Set smoothingmode to AntiAlias
      e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
      // Shadow
      PaintShadow(e.Graphics);
      // Background
      PaintBackground(e.Graphics);
      // Border
      PaintBorder(e.Graphics);
      // Inner shadow
      PaintInnerShadow(e.Graphics);
      // Bars
      PaintBars(e.Graphics);
      // Numbers
      PaintNumbers(e.Graphics);
      // Paint the text(s)
      PaintText(e.Graphics);
      // Paint the Arrows
      PaintArrows(e.Graphics);
      // Reflex
      PaintReflex(e.Graphics);
      // Reset smoothingmode
      e.Graphics.SmoothingMode = SmoothingMode.Default;
    }

    #endregion

    #region -- Protected Methods --

    #region PaintShadow
    /// <summary>
    /// Paints the outer shadow.
    /// </summary>
    /// <param name="g">The graphics object</param>
    protected virtual void PaintShadow(Graphics g)
    {
      if (shadowRect.IsEmpty) return; //break if nothing to draw
      using (Pen p = new Pen(Color.FromArgb(60, Color.Black), defaultOuterShadowWidth))
      {
        g.DrawEllipse(p, shadowRect);
      }
    }
    #endregion

    #region PaintBackground
    /// <summary>
    /// Paints the background.
    /// </summary>
    /// <param name="g">The graphics object</param>
    protected virtual void PaintBackground(Graphics g)
    {
      if (backgroundRect.IsEmpty) return; //break if nothing to draw
      using (Brush b = new LinearGradientBrush(backgroundRect,
          Color.FromArgb(240, 240, 240), backColor, defaultLightingAngle)) //From gray to BackColor
      {
        g.FillEllipse(b, backgroundRect);
      }
    }
    #endregion

    #region PaintBorder
    /// <summary>
    /// Paints the border.
    /// </summary>
    /// <param name="g">The graphics object</param>
    protected virtual void PaintBorder(Graphics g)
    {
      // First draw a image to reflect
      RectangleF r = backgroundRect;
      r.Inflate(-BorderWidth / 2, -BorderWidth / 2);
      if (r.IsEmpty) return; //break if nothing to draw
      using (Pen texturePen = new Pen(textureBrush, BorderWidth))
      {
        g.DrawEllipse(texturePen, r);
      }

      // Gradient overlay
      using (Brush b = new LinearGradientBrush(backgroundRect, Color.White,
          Color.FromArgb(200, Color.White), defaultLightingAngle))
      {
        using (Pen p = new Pen(b, BorderWidth))
        {
          g.DrawArc(p, r, defaultLightingAngle - 90, -180); // Upper half of ellipse
        }
      }
    }
    #endregion

    #region PaintInnerShadow
    /// <summary>
    /// Paints the inner shadow.
    /// </summary>
    /// <param name="g">The graphics object</param>
    protected virtual void PaintInnerShadow(Graphics g)
    {
      RectangleF r = backgroundRect;
      // Adjust for pen and border width
      r.Inflate(-(BorderWidth + defaultInnerShadowWidth / 2),
          -(BorderWidth + defaultInnerShadowWidth / 2));
      if (r.IsEmpty) return; // Break if nothing to draw
      Brush b = new LinearGradientBrush(backgroundRect,
          Color.FromArgb(60, Color.Black),
          Color.FromArgb(30, Color.White), defaultLightingAngle);
      using (Pen p = new Pen(b, defaultInnerShadowWidth))
      {
        g.DrawEllipse(p, r);
      }
      b.Dispose();
    }
    #endregion

    #region PaintNumbers
    /// <summary>
    /// Paints the numbers.
    /// </summary>
    /// <param name="g">The graphics object</param>
    protected virtual void PaintNumbers(Graphics g)
    {
      double tmpAngle = StartAngle;
      for (double d = Min; d <= Max; d += Interval)
      {
        String text = Math.Round(d, Decimals).ToString();
        PointF p = CalcTextPosition(tmpAngle, MeasureText(g, text, Font, (int)numberRect.Width));
        if (ClockWise)
          tmpAngle -= NumberSpacing;
        else
          tmpAngle += NumberSpacing;
        using (Brush b = new SolidBrush(ForeColor))
        {
          g.DrawString(text, Font, b, p);
        }
      }
    }
    #endregion

    #region PaintBars
    /// <summary>
    /// Paints the bars.
    /// </summary>
    /// <param name="g">The graphics object</param>
    protected virtual void PaintBars(Graphics g)
    {
      double tmpAngle = StartAngle;
      for (double d = Min; d <= Max; d += Interval)
      {
        PaintBar(g, bar2Rect, bar3Rect, tmpAngle, defaultBarWidth, barColor);
        if (ClockWise)
          tmpAngle -= NumberSpacing;
        else
          tmpAngle += NumberSpacing;
      }
      if (ClockWise)
      {
        for (double d = tmpAngle + NumberSpacing; d <= StartAngle; d += numberSpacing / BarsBetweenNumbers)
          PaintBar(g, bar1Rect, bar2Rect, d, .5f, barColor);
      }
      else
      {
        for (double d = StartAngle; d <= tmpAngle - NumberSpacing; d += numberSpacing / BarsBetweenNumbers)
          PaintBar(g, bar1Rect, bar2Rect, d, .5f, barColor);
      }
    }
    #endregion

    #region PaintArrows
    /// <summary>
    /// Paints the arrows.
    /// </summary>
    /// <param name="g">The graphics object</param>
    protected virtual void PaintArrows(Graphics g)
    {
      //If the Max value is displayed using the red arrow
      if (StoreMax)
        DrawArrow(g, StoredMax, Color.FromArgb(100, Color.Red));
      //Arrow
      DrawArrow(g, Value, ArrowColor);
    }
    #endregion

    #region PaintText
    /// <summary>
    /// Paint the text properties TextUnit and TextDescription
    /// </summary>
    /// <param name="g">The graphics object</param>
    protected virtual void PaintText(Graphics g)
    {
      PointF center = new PointF(numberRect.Width / 2 + numberRect.X, numberRect.Height / 2 + numberRect.Y);
      if (TextUnit.Length > 0)
      {
        using (Font font = new Font(Font.FontFamily, Font.Size + 4, FontStyle.Bold))
        {
          SizeF size = MeasureText(g, TextUnit, font, (int) numberRect.Width);
          PointF p = new PointF(center.X - size.Width/2, center.Y - 20);
          g.DrawString(TextUnit, font, new SolidBrush(ForeColor), p);
        }
      } 
      if (TextDescription.Length > 0)
      {
        SizeF size = MeasureText(g, TextDescription, Font, (int)numberRect.Width);
        PointF p = new PointF(center.X - size.Width / 2, center.Y + 15);
        g.DrawString(TextDescription, Font, new SolidBrush(ForeColor), p);
      }
    }

    #endregion

    #region PaintReflex
    /// <summary>
    /// Paint the reflex on top
    /// </summary>
    /// <param name="g">The graphics object</param>
    protected virtual void PaintReflex(Graphics g)
    {
      if (backgroundRect.IsEmpty) return; //break if nothing to draw
      using (Brush b = new LinearGradientBrush(backgroundRect, Color.Transparent, Color.FromArgb(100, Color.White), defaultLightingAngle))
      {
        GraphicsPath path = new GraphicsPath();
        RectangleF r = backgroundRect;
        r.Inflate(-borderWidth, -borderWidth);
        if (r.IsEmpty) return; //break if noting to draw
        path.AddArc(r, 0, -180);
        r.Height /= 2;
        r.Offset(0, r.Height);
        r.Height /= 8;
        path.AddArc(r, 180, -180);
        g.FillPath(b, path);
        path.Dispose();
      }
    }
    #endregion

    #endregion Protected Methods End

    #region -- Private Methods --

    #region DrawArrow
    /// <summary>
    /// Draws the arrow from 3 points
    /// </summary>
    /// <param name="g">The graphics object</param>
    /// <param name="v">The value between Min and Max</param>
    /// <param name="c">The arrow color</param>
    private void DrawArrow(Graphics g, double v, Color c)
    {
      PointF p1, p2, p3;
      //Make v relative to Min
      v -= Min;
      double angleValue = (v / Interval) * NumberSpacing;
      if (ClockWise)
      {
        p1 = PointInEllipse(arrow1Rect, StartAngle - angleValue);
        p2 = PointInEllipse(arrow2Rect, StartAngle - angleValue - 170);
        p3 = PointInEllipse(arrow2Rect, StartAngle - angleValue - 190);
      }
      else
      {
        p1 = PointInEllipse(arrow1Rect, StartAngle + angleValue);
        p2 = PointInEllipse(arrow2Rect, StartAngle + angleValue - 170);
        p3 = PointInEllipse(arrow2Rect, StartAngle + angleValue - 190);
      }
      GraphicsPath path = new GraphicsPath();
      path.AddLine(p1, p2);
      path.AddLine(p2, p3);
      //Fill the arrow
      using (Brush b = new SolidBrush(c))
      {
        g.FillPath(b, path);
      }
      path.Dispose();
    }
    #endregion

    #region PaintBar
    /// <summary>
    /// Paint a single bar
    /// </summary>
    /// <param name="g">The graphics object</param>
    /// <param name="outerRect">The outer rectangle</param>
    /// <param name="innerRect">The inner rectangle</param>
    /// <param name="a">The angle from the </param>
    /// <param name="width">The width of the pen</param>
    /// <param name="c">The color of the bar</param>
    private static void PaintBar(Graphics g, RectangleF outerRect, RectangleF innerRect, double a, float width, Color c)
    {
      using (Pen pen = new Pen(c, width))
      {
        PointF p1 = PointInEllipse(innerRect, a);
        PointF p2 = PointInEllipse(outerRect, a);
        g.DrawLine(pen, p1, p2);
      }
    }
    #endregion

    #region MeasureText
    /// <summary>
    /// Measures the text size
    /// </summary>
    /// <param name="g">The graphics object</param>
    /// <param name="text">The text to size up</param>
    /// <param name="f">The font</param>
    /// <param name="maxWidth">Max width of the text</param>
    /// <returns>The size of the text</returns>
    private static SizeF MeasureText(Graphics g, string text, Font f, int maxWidth)
    {
      //Get the size of the text
      StringFormat sf = new StringFormat(StringFormat.GenericTypographic);
      sf.FormatFlags |= StringFormatFlags.NoWrap | StringFormatFlags.NoClip | StringFormatFlags.FitBlackBox;
      sf.Trimming = StringTrimming.None;
      SizeF size = g.MeasureString(text, f, maxWidth, sf);
      return size;
    }
    #endregion

    #region CalcTextPosition
    /// <summary>
    /// Calcs the position af the text based on the angle in the ellipse
    /// </summary>
    /// <param name="a">The angle</param>
    /// <param name="size">The size of the text to place</param>
    /// <returns>Calculated position as PointF</returns>
    private PointF CalcTextPosition(double a, SizeF size)
    {
      PointF p = PointInEllipse(numberRect, a);
      p.X -= (float)((size.Width / 2) * (1 + Math.Cos(Convert.ToRadians(a))));
      p.Y -= (float)((size.Height / 2) * (1 - Math.Sin(Convert.ToRadians(a))));
      return p;
    }
    #endregion

    #region PointInEllipse
    /// <summary>
    /// Return a point in an ellipse.
    /// </summary>
    /// <param name="rect">The rectectangle around the ellipse</param>
    /// <param name="angle">The angle.</param>
    /// <returns>PointF in the specified ellipse</returns>
    private static PointF PointInEllipse(RectangleF rect, double angle)
    {
      double r1 = rect.Width / 2;
      double r2 = rect.Height / 2;
      double x = (float)(r1 * Math.Cos(Convert.ToRadians(angle))) + r1 + rect.X;
      double y = -(float)(r2 * Math.Sin(Convert.ToRadians(angle))) + r2 + rect.Y;
      return new PointF((float)x, (float)y);
    }
    #endregion

    #region CalcRectangles
    /// <summary>
    /// Calc most rectangles used in the design
    /// Called on the Resize event.
    /// </summary>
    private void CalcRectangles()
    {
      //ShadowRectangle
      shadowRect = ClientRectangle;
      shadowRect.Inflate(-1, -1);
      //Reducing width and height of shadow to avoid clipping
      shadowRect.Width -= 1;
      shadowRect.Height -= 1;
      //Background Rectangle
      backgroundRect = shadowRect;
      backgroundRect.Inflate(.5f, .5f);
      backgroundRect.Offset(-1, -1);
      numberRect = backgroundRect;
      numberRect.Inflate(-(numberMargin + Font.Size), -(numberMargin + Font.Size));
      //The rectangle for the bars
      bar1Rect = backgroundRect;
      bar1Rect.Inflate(-(borderWidth + barOuterMargin), -(borderWidth + barOuterMargin));
      bar2Rect = numberRect;
      bar2Rect.Inflate(barInnerMargin + defaultBarHeight, barInnerMargin + defaultBarHeight);
      bar3Rect = numberRect;
      bar3Rect.Inflate(barInnerMargin, barInnerMargin);
      //Arrow Rectangles
      arrow1Rect = numberRect;
      int infl = barInnerMargin + defaultBarHeight * 2;
      arrow1Rect.Inflate(infl, infl);
      arrow2Rect = numberRect;
      arrow2Rect.Inflate(-numberRect.Width / 6, -numberRect.Width / 6);
    }
    #endregion

    #endregion

    #region * Internal Class Convert *
    /// <summary>
    /// Sealed class Convert
    /// </summary>
    internal static class Convert
    {
      /// <summary>
      /// Convert degrees to radians.
      /// </summary>
      /// <returns>Radians</returns>
      public static double ToRadians(double degrees)
      {
        double radians = (Math.PI / 180) * degrees;
        return (radians);
      }

      /// <summary>
      /// Convert radians to degrees
      /// </summary>
      /// <returns>Degrees</returns>
      public static double ToDegrees(double radians)
      {
        double degrees = (radians * 180) / Math.PI;
        return degrees;
      }
    }
    #endregion
  }
}
// Locating resources
internal class ResourceLocator { }