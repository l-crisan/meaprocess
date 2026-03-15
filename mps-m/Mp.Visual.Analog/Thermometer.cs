using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Mp.Visual.Analog
{
    /// <summary>
    /// A thermometer control.
    /// </summary>
    public partial class ThermometerCtrl : UserControl
    {
        #region Private Member
        private double _rangeMin = -20.0;
        private double _rangeMax = 120;
        private double _value = 0;
        private bool   _drawTics = true;
        private int    _smallTicFreq = 5;
        private int    _largeTicFreq = 20;
        private Unit   _display = Unit.Celsius;
        #endregion    
        
        #region Properties

        /// <summary>
        /// Constricuts a Thermometer control.
        /// </summary>
        public ThermometerCtrl()            
        {
            base.ForeColor = Color.FromArgb(255, 0, 0);
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
        }

        /// <summary>
        /// Gets or sets the display mode.
        /// </summary>
        [SRDescription("Display")]
        [SRCategory("Thermometer")]
        public Unit Display
        {
            get { return _display; }

            set
            {
                _display = value;
                Invalidate();
            }
        }
        /// <summary>
        /// Gets or sets the small tic frequency.
        /// </summary>
        [SRDescription("SmallTicFreq")]
        [SRCategory("Thermometer")]
        public int SmallTicFreq
        {
            get { return _smallTicFreq; }
            set
            {
                _smallTicFreq = value;
                Refresh();
            }
        }

        /// <summary>
        /// Gets or sets the large tic frequency.
        /// </summary>
        [SRDescription("LargeTicFreq")]
        [SRCategory("Thermometer")]
        public int LargeTicFreq
        {
            get { return _largeTicFreq; }
            set
            {
                _largeTicFreq = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets otr sets the range minimum.
        /// </summary>
        [System.ComponentModel.Browsable(true),
        SRCategory("Thermometer"),
        SRDescription("RangeMin")]
        public double RangeMin
        {
            get 
            {
                return F2C(_rangeMin);
            }
            set 
            {
                double oldValue = _rangeMin;

                _rangeMin = C2F(value);

                if (_rangeMin >= _rangeMax)
                    _rangeMin = oldValue;

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the range maximum.
        /// </summary>
        [System.ComponentModel.Browsable(true),
        SRCategory("Thermometer"),
        SRDescription("RangeMax")]
        public double RangeMax
        {
            get 
            {
                return F2C(_rangeMax);
            }

            set 
            {
                double oldValue = _rangeMax;

                _rangeMax = C2F(value);

                if (_rangeMax <= _rangeMin)
                    _rangeMax = oldValue;
                
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the current control value in Fahreinheit.
        /// </summary>
        /// <remarks>
        /// To set ot get the value in Celsius use F2C/C2F methods.
        /// </remarks>
        [System.ComponentModel.Browsable(true),
        SRCategory("Thermometer"),
        SRDescription("Value")]
        public double Value
        {
            get 
            {
                return F2C(_value);
            }

            set 
            {
                double dPos;

                dPos = C2F(value);

                if (dPos > _rangeMax)
                    dPos = _rangeMax;
                if (dPos < _rangeMin)
                    dPos = _rangeMin;

                _value = dPos;
                Invalidate();
            }
        }

        #endregion

        #region Public

        public enum Unit
        {
            Celsius,
            Fahrenheit
        }

        /// <summary>
        /// Transform a Fahreinheit value in Celsius value.
        /// </summary>
        /// <param name="dF"></param>
        /// <returns></returns>
        public static double F2C(double dF)
        {
            return ((dF - 32) / 1.8);
        }

        /// <summary>
        /// Transform a Celsius value in Fahreinheit.
        /// </summary>
        /// <param name="dC"></param>
        /// <returns></returns>
        public static double C2F(double dC)
        {
            return dC * 1.8 + 32;
        }
        #endregion

        #region Private Helper
        private void FillCylinder(Graphics pGfx, RectangleF rCtrl, Brush pFillBrush, Color cOutlineColor)
        {
            RectangleF rTopPlane = new RectangleF(rCtrl.X, rCtrl.Y - 5, rCtrl.Width, 5);
            RectangleF rBottomPlane = new RectangleF(rCtrl.X, rCtrl.Bottom - 5, rCtrl.Width, 5);

            // Outline pen
            Pen penOutline = new Pen(cOutlineColor);
            // Draw body
            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddArc(rTopPlane, 0.0f, 180.0f);
            gfxPath.AddArc(rBottomPlane, 180.0f, -180.0f);
            gfxPath.CloseFigure();

            // Fill body
            pGfx.FillPath(pFillBrush, gfxPath);
            // Outline body
            pGfx.DrawPath(penOutline, gfxPath);
            // Draw top plane
            gfxPath.Reset();

            gfxPath.AddEllipse(rTopPlane);

            // Fill top plane 
            pGfx.FillPath(pFillBrush, gfxPath);
            // Outline top plane 
            pGfx.DrawPath(penOutline, gfxPath);
        }

        #endregion

        protected override void  OnPaint(PaintEventArgs e)
        {            
            //Double buffering.
            Graphics gfx = e.Graphics;
            
            // Erase background
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Create GDI+ objects
            Color clrFore, clrBack, clrScale, clrOutline;
            clrFore = base.ForeColor;
            clrBack = this.BackColor;
            clrScale = Color.FromArgb(0, 0, 0);
            clrOutline = Color.FromArgb(64, 0, 0);
            Pen fgPen = new Pen(clrFore);
            Pen scalePen = new Pen(clrScale);
            Pen outlinePen = new Pen(clrOutline);
            SolidBrush blackBrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
            SolidBrush fillBrush = new SolidBrush(clrBack);
            Font fntText = this.Font;
            StringFormat strfmtText = new StringFormat();
            strfmtText.Alignment = StringAlignment.Center;
            strfmtText.LineAlignment = StringAlignment.Center;

            // Everything is drawn from center
            float topOffset = 20.0f;
            Point ptCenter = new Point(Width / 2, Height / 2);
            float fTmpWidth = (float)(Width / 10.0);

            // Get bulb dimensions
            RectangleF rBulb = new RectangleF((float)ptCenter.X - (float)fTmpWidth,
                                              (float)this.Height - topOffset- (float)((fTmpWidth * 2) + 25),
                                              (float)fTmpWidth * 2, (float)fTmpWidth * 2);

            // Draw the bulb   
            Color toColor = Color.FromArgb(150, 100, 100);
            LinearGradientBrush brKnob = new LinearGradientBrush(rBulb, clrFore, toColor, LinearGradientMode.Horizontal);
            gfx.FillEllipse(brKnob, rBulb);
            gfx.DrawEllipse(outlinePen, rBulb);

            // Get cylinder coordinates
            RectangleF rCylinder = new RectangleF((float)ptCenter.X - (float)fTmpWidth / 2,
                                                  (float)(topOffset +( _drawTics ? 25 : 10)), // 25 pixels on top for F/C
                                                  (float)fTmpWidth,
                                                  (float)rBulb.Top - topOffset - (_drawTics ? 20 : 5)); // 5 pixel overlap over bulb

            // Make sure we have positive values to work with
            float fRange = (float)(_rangeMax - _rangeMin);
            float fVal = (float)_value;
            
            fVal -= (float)_rangeMin;

            // Draw the cylinder
            FillCylinder(gfx, rCylinder, fillBrush, clrOutline);

            if (fVal > 0)
            {
                float fPixFull = (fVal * rCylinder.Height) / fRange;
                RectangleF rFull = new RectangleF((float)rCylinder.Left, (float)rCylinder.Bottom - fPixFull, (float)rCylinder.Width, (float)fPixFull);
                FillCylinder(gfx, rFull, brKnob, clrOutline);
            }

            // Outline top (empty) plane
            RectangleF rEmptyTopPlane = new RectangleF(rCylinder.X, rCylinder.Y - 5, rCylinder.Width, 5);
            gfx.DrawEllipse(outlinePen, rEmptyTopPlane);

            if (_drawTics)
            {
                Font fntMark = new Font("Arial", 7, FontStyle.Regular);
                StringFormat strfmtMark = new StringFormat();
                strfmtMark.Alignment = StringAlignment.Far;
                strfmtMark.LineAlignment = StringAlignment.Center;
                string strDegree = "";
                Point ptStart, ptEnd;
                PointF ptText;

                // The range and the values are in fahrenheit
                float fPixPerDegree = rCylinder.Height / fRange;
                float fMarkFreq = fPixPerDegree * _largeTicFreq;
                long lMarkVal = (long)_rangeMax;

                //Measure the degree text.
                string str = String.Format("{0:0}", _rangeMax);
                SizeF maxSize = gfx.MeasureString(str, fntMark);
                str = String.Format("{0:0}", _rangeMin);
                SizeF minSize = gfx.MeasureString(str, fntMark);

                 //Calculate the offset.
                int offset = (int) Math.Max(maxSize.Width, minSize.Width) + 10;

                // Draw large marks and text
                for (float y = rCylinder.Top; y <= rCylinder.Bottom; y += fMarkFreq)
                {
                    ptStart = new Point((int)rCylinder.Right + 3, (int)y);
                    ptEnd = new Point((int)rCylinder.Right + 10, (int)y);
                    gfx.DrawLine(scalePen, ptStart, ptEnd);
                    strDegree = lMarkVal.ToString();

                    ptText = new PointF((float)rCylinder.Right + offset, (float)y);

                    gfx.DrawString(strDegree, fntMark, blackBrush, ptText.X, ptText.Y, strfmtMark);
                    lMarkVal -= _largeTicFreq;
                }

                // Draw small marks
                fMarkFreq = fPixPerDegree * _smallTicFreq;
                for (float y = rCylinder.Top; y <= rCylinder.Bottom; y += fMarkFreq)
                {
                    ptStart = new Point((int)rCylinder.Right + 3, (int)y);
                    ptEnd = new Point((int)rCylinder.Right + 8, (int)y);
                    gfx.DrawLine(scalePen, ptStart, ptEnd);
                }

                // The range and the values are stored in fahrenheit but we must draw celsius too
                fRange = (float)(F2C(_rangeMax) - F2C(_rangeMin));
                fPixPerDegree = rCylinder.Height / fRange;
                fMarkFreq = fPixPerDegree * _largeTicFreq;
                lMarkVal = (long)F2C(_rangeMin);
               
                // Draw large marks and text
                for (float y = rCylinder.Bottom; y >= rCylinder.Top; y -= fMarkFreq)
                {
                    int iCy = (int)y;

                    if (iCy <= rCylinder.Top)
                        break;

                    ptStart = new Point((int)rCylinder.Left - 10, (int)iCy);
                    ptEnd = new Point((int)rCylinder.Left - 3, (int)iCy);
                    gfx.DrawLine(scalePen, ptStart, ptEnd);
                    strDegree = lMarkVal.ToString();

                    gfx.DrawString(strDegree, fntMark, blackBrush, new PointF((float)rCylinder.Left - 15,
                                  (float)iCy), strfmtMark);

                    lMarkVal += _largeTicFreq;
                }
                
                // Draw small marks every _smallTicFreq degrees.
                fMarkFreq = fPixPerDegree * _smallTicFreq;

                for (float y = rCylinder.Bottom; y >= rCylinder.Top; y -= fMarkFreq)
                {
                    int iCy = (int)y;

                    if (iCy <= rCylinder.Top)
                        break;

                    ptStart = new Point((int)rCylinder.Left - 8, (int)iCy);
                    ptEnd = new Point((int)rCylinder.Left - 3, (int)iCy);
                    gfx.DrawLine(scalePen, ptStart, ptEnd);
                }
                
                // Draw F/C.
                RectangleF rText = new RectangleF((float)ptCenter.X + 20, topOffset, (float)20, (float)20);

                gfx.DrawString("°F", fntText, blackBrush, rText, strfmtText);

                rText = new RectangleF((float)ptCenter.X - 40, topOffset, (float)20, (float)20);
                gfx.DrawString("°C", fntText, blackBrush, rText, strfmtText);
            }

            // Draw the text.
            RectangleF rrText = new RectangleF(0.0f, (float)5.0f,(float)this.Width, 15.0f);

            gfx.DrawString(this.Text, fntText, blackBrush, rrText, strfmtText);

            // Draw the value.
            rrText = new RectangleF((float)0.0f, (float)rBulb.Bottom + 5,
                                           (float)this.Width, (float)(this.Height - (rBulb.Bottom + 5)));
            string strValue = "";

            if (_display == Unit.Fahrenheit)
                strValue = String.Format("{0:0.0} °F", _value);
            else
                strValue = String.Format("{0:0.0} °C", F2C(_value));

            gfx.DrawString(strValue, fntText, blackBrush, rrText, strfmtText);            
        }
    }
}