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
using System.Text;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Diagnostics;

namespace Mp.Visual.XYChart
{
    public partial class Chart : UserControl
    {
        private bool _repaint = true;
        private string _xText = "X Signal";
        private string _yText = "Y Signal";
        private double _xmin = -10;
        private double _xmax = 10.0;
        private double _ymin = -10.0;
        private double _ymax = 10.0;
        private double _xlogMin = -10;
        private double _xlogMax = 10.0;
        private double _ylogMin = -10.0;
        private double _ylogMax = 10.0;
        private Color  _lineColor = Color.YellowGreen;
        private Color  _axisColor = Color.AliceBlue;
        private Color  _pointColor = Color.Violet;
        private int    _ydiv = 10;
        private int    _xdiv = 10;
        private int    _xprec = 2;
        private int    _yprec = 2;
        private Rectangle _drawRect;
        private int _leftOffset = 0;
        private Color _gridColor = Color.Gray;
        private int _gridXDev = 10;
        private int _gridYDev = 10;
        private DashStyle _gridStyle = DashStyle.Dot;
        private Bitmap _backBuffer;
        private System.Threading.Mutex _mutex = new System.Threading.Mutex();
        private List<PointF> _points = new List<PointF>();
        private Timer _update = new Timer();
        private bool _lineVisible = true;
        private bool _pointVisible = true;
        private string _text = "X/Y Chart";
        private int _top = 30;
        private bool _beginDrag = false;
        private bool _draging = false;
        private double _orgXMax = 0;
        private double _orgXMin = 0;
        private double _orgYMax = 0;
        private double _orgYMin = 0;
        private int _pointsToPlot = 10000;
        private bool _lineVisibleNoEvent = false;
        private bool _pointVisibleNoEvent = false;
        protected Rectangle _selectionRect = new Rectangle();
        private CurveList _refCurves = new CurveList();
        private bool _reset = false;
        private bool _editable = true;
        private bool _xLog = false;
        private bool _yLog = false;
        private int _pointsToPlotHalfOne;

        
     
        public Chart()
        {            
            InitializeComponent();
            ShowControlPanel = false;

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            if (controlPanel.Visible)
            {
                _backBuffer = new Bitmap(this.Width - controlPanel.Width, this.Height);
            }
            else
            {
                _backBuffer = new Bitmap(this.Width, this.Height);
            }

            showLineToolStripMenuItem.Checked = true;
            showPointsToolStripMenuItem.Checked = true;

            _update.Interval = 50;
            _update.Tick += new EventHandler(OnUpdate_Tick);
            RepaintAll();
            _update.Start();
        }

        private void DrawSelectionRect(Graphics g)
        {
            if (!_draging)
                return;

            _mutex.WaitOne();

            Point[] pol = new Point[4];
            pol[0] = new Point(_selectionRect.X, _selectionRect.Y);
            pol[1] = new Point(_selectionRect.X + _selectionRect.Width, _selectionRect.Y);
            pol[2] = new Point(_selectionRect.X + _selectionRect.Width, _selectionRect.Y + _selectionRect.Height);
            pol[3] = new Point(_selectionRect.X, _selectionRect.Y + _selectionRect.Height);
            Pen pen = new Pen(Color.LightGray, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            g.DrawPolygon(pen, pol);

            _mutex.ReleaseMutex();
        }

        private void OnUpdate_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        [SRCategory("View"), SRDescription("ShowControlPanel")]
        public bool ShowControlPanel
        {
            set
            {
                _mutex.WaitOne();
                controlPanel.Visible = value;
                UpdateBackBuffer();   
                RepaintAll();
                
                _mutex.ReleaseMutex();
            }
            get { return controlPanel.Visible; }
        }

        [SRCategory("View"), SRDescription("Editable")]
        public bool Editable
        {
            set{ _editable = value; }
            get { return _editable; }
        }


        public bool EnableRefCurveEdit
        {
            set { editReferenceCurvesToolStripMenuItem.Enabled = value;}
            get { return editReferenceCurvesToolStripMenuItem.Enabled; }
        }

        public void Start()
        {
            _mutex.WaitOne();
            _orgXMax = _xmax;
            _orgXMin = _xmin;
            _orgYMax = _ymax;
            _orgYMin = _ymin;
            _mutex.ReleaseMutex(); 
            Reset();
        }

        public void Clear()
        {
            _points.Clear();
            RepaintAll();
        }

        public void Reset()
        {
            if (_reset)
                return;

            _mutex.WaitOne();

            _points.Clear();
            RepaintAll();
            _mutex.ReleaseMutex();
            _reset = true;
        }

        [SRCategory("View"), SRDescription("Title")]
        public string Title
        {
            set 
            {
                _mutex.WaitOne();
                _text = value;
                RepaintAll();
                
                _mutex.ReleaseMutex();
            }
            get
            {  return _text;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (this.Width < 1 || this.Height < 1)
                return;

            _mutex.WaitOne();
            UpdateBackBuffer();
            RepaintAll();
            _mutex.ReleaseMutex();
        }

        private void UpdateBackBuffer()
        {
            if (controlPanel.Visible)
            {
                int width = this.Width - controlPanel.Width;

                if (width > 0)
                {
                    _backBuffer = new Bitmap(width, this.Height);
                }
                else
                {
                    _backBuffer = new Bitmap(this.Width, this.Height);
                }
            }
            else
            {
                _backBuffer = new Bitmap(this.Width, this.Height);
            }
        }


        public int MaxPoints
        {
            set 
            {
                _pointsToPlot = value;
                _pointsToPlotHalfOne = _pointsToPlot / 2 + 1;
            }

            get { return _pointsToPlot; }
        }

        [SRCategory("View"), SRDescription("NoOfPoints")]
        public int NoOfPoints
        {
            set
            {
                _pointsToPlot = value;
                _pointsToPlotHalfOne = _pointsToPlot / 2 + 1;
            }
            get { return _pointsToPlot; }
        }

        private void ScaleByRect(Rectangle rect)
        {
            double xMin = PixelXToValue(rect.X);
            double xMax = PixelXToValue(rect.X + rect.Width);
            double yMax = PixelYToValue(rect.Y);
            double yMin = PixelYToValue(rect.Y + rect.Height);

            double difference = 0.00001;

            if ((Math.Abs(xMax - xMin) > difference) && (Math.Abs(yMax - yMin) > difference))
            {
                _xmin = xMin;
                _xmax = xMax;
                _ymin = yMin;
                _ymax = yMax;

                RepaintAll();
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            _beginDrag = false;
            _draging = false;
            base.OnDoubleClick(e);
        }

        private double PixelXToValue(int x)
        {
            double min = _xmin;
            double max = _xmax;

            if (_xLog)
            {
                min = 0;
                max = 0;

                if(CompareDoubleNotEq(_xmin,0))
                    min = Math.Log10(Math.Abs(_xmin)) * Math.Sign(_xmin);

                if (CompareDoubleNotEq(_xmax, 0))
                    max = Math.Log10(Math.Abs(_xmax)) * Math.Sign(_xmax);
            }

            double factor = (max - min) / (double)(_drawRect.Width);
            double offset = ((double)(min * _drawRect.Right) - (double)(max * _drawRect.Left)) / (_drawRect.Width);

            if( _xLog)
                return Math.Pow(10, x * factor + offset);

            return (double)(x * factor + offset);
        }

        private double PixelYToValue(int y)
        {
            double factor = (_ymin-_ymax) / (double)(_drawRect.Height);

            double offset = ((_ymax * _drawRect.Bottom) - (_ymin * _drawRect.Top)) / (double)_drawRect.Height;

            return (double)(y * factor + offset);
        }

        public void RepaintAll()
        {
            if (!_repaint)
                return;

            Graphics g = Graphics.FromImage(_backBuffer);
            
            DrawYAxis(g);
            DrawXAxis(g);
            DrawGrid(g);
            DrawRefCurves(g);

            SizeF size = g.MeasureString(_text, this.Font);
            Point topCenter = new Point(_drawRect.Left + _drawRect.Width / 2 - (int)size.Width / 2, 15);
            g.DrawString(_text, this.Font, new SolidBrush(_axisColor), topCenter);

            if (_points.Count > 0)
            {
                PointF last = _points[0];
                DrawXY(last.X, last.Y, last.X, last.Y, _lineColor, _pointVisible);
            }

            for (int i = 1; i < _points.Count; ++i)
            {
                PointF last = _points[i - 1];
                PointF current = _points[i];
                DrawXY(last.X, last.Y, current.X, current.Y, _lineColor, _pointVisible);
            }
        }

        private void DrawRefCurves(Graphics g)
        {            
            foreach(Curve curve in _refCurves)
            {
                if (curve.X == null)
                {
                    int i = (int)(_xmin * curve.YRate);

                    double pos = _xmin;
                    double inc = 1 / curve.YRate;

                    for (; i < curve.Y.Length - 1; ++i)
                    {
                        if( i >= 0)
                            DrawXY(pos, curve.Y[i], pos + inc, curve.Y[i + 1], curve.LineColor, false);

                        pos += inc;
                        if (pos > _xmax)
                            break;
                    }
                }
                else
                {
                    int lenght = Math.Min(curve.X.Length, curve.Y.Length) - 1;

                    for (int i = 0; i < lenght; ++i)
                        DrawXY(curve.X[i], curve.Y[i], curve.X[i + 1], curve.Y[i + 1], curve.LineColor, false);
                }
            }
        }

        [SRCategory("View"), SRDescription("ReferenceCurves"),
        System.ComponentModel.Browsable(true),
        System.ComponentModel.Editor(typeof(RefCurveUIEditor), typeof(UITypeEditor))]
        public CurveList ReferenceCurves
        {
            get { return _refCurves; }
            set { _refCurves = value;}
        }

        [SRCategory("View"), SRDescription("DrawLine")]
        public bool DrawLine
        {
            get { return _lineVisible; }
            set 
            {
                _mutex.WaitOne();
                _lineVisibleNoEvent = true;
                _lineVisible = value;
                showLineToolStripMenuItem.Checked = _lineVisible;
                onShowLine.Checked = _lineVisible;
                RepaintAll();
                _lineVisibleNoEvent = false;
                _mutex.ReleaseMutex();

            }
        }
        
        [SRCategory("View"), SRDescription("XLog")]
        public bool XLogarithmic
        {
            get { return _xLog; }
            set
            {
                _xLog = value;
            }
        }

        [SRCategory("View"), SRDescription("YLog")]
        public bool YLogarithmic
        {
            get { return _yLog; }
            set
            {
                _yLog = value;
            }
        }

        [SRCategory("View"), SRDescription("DrawPoint")]
        public bool DrawPoint
        {
            get { return _pointVisible; }
            set 
            {
                _mutex.WaitOne();
                _pointVisibleNoEvent = true;
                _pointVisible = value;
                showPointsToolStripMenuItem.Checked = _pointVisible;
                onShowPoints.Checked = _pointVisible;
                RepaintAll();
                _pointVisibleNoEvent = false;
                _mutex.ReleaseMutex();
            }
        }


        public System.Threading.Mutex SynchMutex
        {
            get { return _mutex; }
        }

        public void SetValue(double x, double y)
        {
            if (_points.Count > 0)
            {
                if (_points[_points.Count - 1].X == x && _points[_points.Count - 1].Y == y)
                    return;

                _reset = false;
                DrawXY(_points[_points.Count - 1].X, _points[_points.Count - 1].Y, x, y,_lineColor, _pointVisible);
                _points.Add(new PointF((float)x, (float)y));
            }
            else
            {
                _reset = false;

                if (_pointVisible)
                    DrawXYPoint(x, y);

                _points.Add(new PointF((float)x, (float)y));
            }

            if( _points.Count == NoOfPoints)
                _points.Clear();
        }

        private void DrawXY(double oldX, double oldY, double x, double y, Color lineColor, bool points)
        {
            int oldXPix = ValueXToPixel(oldX);
            int oldYPix = ValueYToPixel(oldY);
            int xPix = ValueXToPixel(x);
            int yPix = ValueYToPixel(y);

            if (oldXPix == xPix && oldYPix == yPix)
                return;

            Graphics g = Graphics.FromImage(_backBuffer);
            Region oldClip = g.Clip;

            g.Clip = new Region(new Rectangle(_drawRect.X - 1, _drawRect.Y - 1, _drawRect.Width + 2, _drawRect.Height + 2));

            if (points)
            {
                Pen dPen = new Pen(_pointColor);
                Point dPoint = new Point(xPix, yPix);
                dPoint.X -= (int)(5 / 2);
                dPoint.Y -= (int)(5 / 2);
                try
                {
                    g.DrawEllipse(dPen, dPoint.X, dPoint.Y, 5, 5);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            try
            {
                if (_lineVisible)
                    g.DrawLine(new Pen(lineColor, 1), new Point(oldXPix, oldYPix), new Point(xPix, yPix));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            g.Clip = oldClip;
        }

        private void DrawXYPoint(double x, double y)
        {
            int xPix = ValueXToPixel(x);
            int yPix = ValueYToPixel(y);

            Graphics g = Graphics.FromImage(_backBuffer);
            Region oldClip = g.Clip;

            g.Clip = new Region(new Rectangle(_drawRect.X - 1, _drawRect.Y - 1, _drawRect.Width + 2, _drawRect.Height + 2));
            Pen dPen = new Pen(_pointColor);
            Point dPoint = new Point(xPix, yPix);
            
            dPoint.X -= (int)(5 / 2);
            dPoint.Y -= (int)(5 / 2);
            
            try
            {
                g.DrawEllipse(dPen, dPoint.X, dPoint.Y, 5, 5);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            g.Clip = oldClip;
        }


        private bool CompareDoubleNotEq(double a, double b)
        {
            double difference = 0.000000001;
            return (Math.Abs(a-b) > difference);
        }

        private int ValueXToPixel(double x)
        {
            double min = _xmin;
            double max = _xmax; 

            if (_xLog)
            {
                if (x < 1)
                    return 0;

                min =_xlogMin;                
                max = _xlogMax;
                x = Math.Log10(x);
            }

            double factor = (_drawRect.Width) / (max - min);
            double offset = ((double)(_drawRect.Left * max) - (double)(_drawRect.Right * min)) / (max - min);

            return (int)( x * factor + offset);
        } 

        private int ValueYToPixel(double y)
        {
            double verticalFactor = 0;
            double difference = 0.00001;

            double min = _ymin;
            double max = _ymax;

            if (_yLog)
            {
                if (y < 1)
                    return 0;

                min = _ylogMin;
                max = _ylogMax;
                y = Math.Log10(y);
            }

            if (Math.Abs(max - min) > difference)
                verticalFactor = (double)_drawRect.Height / (max - min);

            return (int)(_drawRect.Bottom - (int)((y - min) * verticalFactor));
        }

        public double YLogMin
        {
            get { return _ylogMin; }
            set { _ylogMin = value; }
        }

        public double YLogMax
        {
            get { return _ylogMax; }
            set { _ylogMax = value; }
        }

        public double XLogMin
        {
            get{ return _xlogMin;}
            set{ _xlogMin = value;}
        }

        public double XLogMax
        {
            get { return _xlogMax; }
            set { _xlogMax = value; }
        }

        [SRCategoryAttribute("XAxis"), SRDescriptionAttribute("XMinimum")]
        public double XMinimum
        {
            set 
            {
                _mutex.WaitOne();
                _xmin = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
            get
            { 
                return _xmin;
            }
        }

        [SRCategoryAttribute("XAxis"), SRDescriptionAttribute("XMaximum")]        
        public double XMaximum
        {
            set
            {
                _mutex.WaitOne();
                _xmax = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
            get{ return _xmax;}
        }

        [SRCategoryAttribute("YAxis"), SRDescriptionAttribute("YMinimum")]        
        public double YMinimum
        {
            set 
            {
                _mutex.WaitOne();
                _ymin = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }

            get { return _ymin; }
        }

        [SRCategoryAttribute("YAxis"), SRDescriptionAttribute("YMaximum")]        
        public double YMaximum
        {
            set 
            {
                _mutex.WaitOne();
                _ymax = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
            get { return _ymax; }
        }


        public string XText
        {
            get 
            { 
                return _xText; 
            }
            set 
            {
                _mutex.WaitOne();
                _xText = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
        }

        public string YText
        {
            get { return _yText; }
            set 
            {
                _mutex.WaitOne();
                _yText = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
        }

        [SRCategoryAttribute("Colors"), SRDescriptionAttribute("LineColor")]
        public Color LineColor
        {
            set
            {
                _mutex.WaitOne();
                _lineColor = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
            get
            { return _lineColor;
            }
        }
        [SRCategoryAttribute("Colors"), SRDescriptionAttribute("AxisColor")]
        public Color AxisColor
        {
            set
            {
                _mutex.WaitOne();
                _axisColor = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
            get
            {
                return _axisColor;
            }
        }
        [SRCategoryAttribute("Colors"), SRDescriptionAttribute("PointColor")]
        public Color PointColor
        {
            set
            {
                _mutex.WaitOne();
                _pointColor = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }

            get{ return _pointColor;}
        }

        [SRCategoryAttribute("YAxis"), SRDescriptionAttribute("YAxisDivision")]
        public int YAxisDivision
        {
            get
            {
                return _ydiv;
            }
            set
            {
                _mutex.WaitOne();
                _ydiv = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
        }

        [SRCategoryAttribute("XAxis"), SRDescriptionAttribute("XAxisDivision")]
        public int XAxisDivision
        {
            get { return _xdiv; }
            set
            {
                _mutex.WaitOne();
                _xdiv = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
        }

        [SRCategoryAttribute("YAxis"), SRDescriptionAttribute("YAxisDivision")]
        public int YAxisDevision
        {
            get
            { return _ydiv;
            }
            set
            {
                _mutex.WaitOne();
                _ydiv = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
        }

        [SRCategoryAttribute("XAxis"), SRDescriptionAttribute("XAxisDivision")]
        public int XAxisDevision
        {
            get{ return _xdiv;}
            set
            {
                _mutex.WaitOne();
                _xdiv = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
        }

        [SRCategoryAttribute("XAxis"), SRDescriptionAttribute("XAxisPrecision")]
        public int XAxisPrecision
        {
            get{ return _xprec;}
            set
            {
                _mutex.WaitOne();
                _xprec = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
        }

        [SRCategoryAttribute("YAxis"), SRDescriptionAttribute("YAxisPrecision")]
        public int YAxisPrecision
        {
            get{ return _yprec;}
            set
            {
                _mutex.WaitOne();
                _yprec = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Sets or gets the grid line Style.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridLineStyle")]
        public DashStyle GridLineStyle
        {
            set
            {
                _mutex.WaitOne();
                _gridStyle = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }

            get { return _gridStyle; }
        }

        /// <summary>
        /// Sets or gets the grid line Style.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridXDivision")]
        public int GridXDevision
        {
            set
            {
                _gridXDev = value;
            }

            get { return _gridXDev; }
        }

        /// <summary>
        /// Sets or gets the grid line Style.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridYDivision")]
        public int GridYDevision
        {
            set
            {
                _mutex.WaitOne();
                _gridYDev = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }

            get { return _gridYDev; }
        }

        /// <summary>
        /// Sets or gets the grid line Style.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridXDivision")]
        public int GridXDivision
        {
            set
            {
                _gridXDev = value;
            }

            get { return _gridXDev; }
        }

        /// <summary>
        /// Sets or gets the grid line Style.
        /// </summary>
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridYDivision")]
        public int GridYDivision
        {
            set
            {
                _mutex.WaitOne();
                _gridYDev = value;
                RepaintAll();
                _mutex.ReleaseMutex();
            }

            get { return _gridYDev; }
        }
        [SRCategoryAttribute("Grid"), SRDescriptionAttribute("GridColor")]
        public Color GridColor
        {
            set
            {
                _mutex.WaitOne();
                _gridColor = value;
                RepaintAll();

                _mutex.ReleaseMutex();
            }

            get { return _gridColor; }
        }
        

        private string GetFormatedScaleText(double value, int precision)
		{
            return value.ToString("f" + precision.ToString());
		}

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            _mutex.WaitOne();
            double xscale = (XMaximum - XMinimum) / XAxisDevision;
            double yscale = (YMaximum - YMinimum) / YAxisDevision;            

            if (e.Delta < 0)
            {
                _xmax += xscale;
                _xmin -= xscale;
                _ymax += yscale;
                _ymin -= yscale;
            }
            else
            {
                _xmax -= xscale;
                _xmin += xscale;
                _ymax -= yscale;
                _ymin += yscale;
            }
            
            RepaintAll();

            _mutex.ReleaseMutex();
        }
        private void DrawYAxis(Graphics g)
        {
            g.Clear(this.BackColor);

            Pen dPen;
            int offset = 5;
            int maxTextExt = 0;
            SizeF size;
            string strText;
            int textExtend = 0;
            SolidBrush textBrush = new SolidBrush(_axisColor);
            StringFormat drawFormat = new StringFormat();
            Matrix matrixRot270;
            Matrix oldMatrix;

            int height = this.Height - 5;

            drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            PointF pt;

            //Draw the reference curves
            foreach (Curve curve in _refCurves)
            {
                string curveName ;
                if( curve.Name == "")
                    curveName = curve.YName;
                else
                    curveName = curve.Name;

                size = g.MeasureString(curveName, this.Font);
                pt = new Point(offset, _top + height - 100);
                
                oldMatrix = g.Transform;
                matrixRot270 = new Matrix();
                matrixRot270.RotateAt(270, pt);                
                g.Transform = matrixRot270;
                g.DrawString(curveName, this.Font, new SolidBrush(curve.LineColor), pt);
                g.Transform = oldMatrix;
                g.DrawLine(new Pen(curve.LineColor), new Point((int)(offset + size.Height / 2), _top + height - 90), new Point((int)(offset + size.Height / 2), _top + height - 50));
                g.FillRectangle( new SolidBrush(curve.LineColor), new Rectangle((int)(offset + size.Height / 2) - 3,_top + height - 70,7,7));
                
                offset += (int) (size.Height);
            }

            //Select the pen
            dPen = new Pen(_axisColor);

            //Y label
            size = g.MeasureString(_yText, this.Font);
            textExtend = (int)size.Height;

            pt = new Point((int)(offset), (int)(((height - 60 + _top) / 2) + (size.Width / 2)));

            oldMatrix = g.Transform;
            matrixRot270 = new Matrix();
            matrixRot270.RotateAt(270, pt);
            g.Transform = matrixRot270;

            g.DrawString(_yText, this.Font, textBrush, pt);
            g.Transform = oldMatrix;

            //Draw text degrees
            strText = GetFormatedScaleText(_ymin, _yprec);

            size = g.MeasureString(strText, this.Font);

            if (maxTextExt < size.Width)
                maxTextExt = (int) size.Width;

            double degreePos = _top;
            double degreeInc = 0.0;
            if (_yLog)
            {
                degreeInc = (height - 75 - _top) / (double)_ydiv;

                offset += (int)(size.Height + 2);

                double posInc = (Math.Log10(_ymax) - Math.Log10(_ymin)) / _ydiv;
                double degree = Math.Log10(_ymax);

                //Degree text
                for (int n = 0; n <= _ydiv; n++)
                {
                    if (n == _ydiv)
                        strText = GetFormatedScaleText(_ymin, _yprec);
                    else
                        strText = GetFormatedScaleText(Math.Pow(10,degree), _yprec);

                    size = g.MeasureString(strText, this.Font);
                    textExtend = (int)size.Width;

                    if (maxTextExt < textExtend)
                        maxTextExt = textExtend;

                    degreePos += degreeInc;
                    degree -= posInc;
                }

                degreePos = _top;
                degree =  Math.Log10(_ymax);

                for (int n = 0; n <= _ydiv; n++)
                {
                    if (n == _ydiv)
                        strText = GetFormatedScaleText(_ymin, _yprec);
                    else
                        strText = GetFormatedScaleText(Math.Pow(10, degree), _yprec);

                    size = g.MeasureString(strText, this.Font);
                    g.DrawString(strText, this.Font, textBrush, new Point((offset + maxTextExt) - (int)size.Width, (int)(degreePos)));

                    degreePos += degreeInc;
                    degree -= posInc;
                }
            }
            else
            {
                degreeInc = (height - 75 - _top) / (double)_ydiv;

                offset += (int)(size.Height + 2);

                double posInc = (_ymax - _ymin) / _ydiv;
                double degree = _ymax;

                //Degree text
                for (int n = 0; n <= _ydiv; n++)
                {
                    if (n == _ydiv)
                        strText = GetFormatedScaleText(_ymin, _yprec);
                    else
                        strText = GetFormatedScaleText(degree, _yprec);

                    size = g.MeasureString(strText, this.Font);
                    textExtend = (int)size.Width;

                    if (maxTextExt < textExtend)
                        maxTextExt = textExtend;

                    degreePos += degreeInc;
                    degree -= posInc;
                }

                degreePos = _top;
                degree = _ymax;

                for (int n = 0; n <= _ydiv; n++)
                {
                    if (n == _ydiv)
                        strText = GetFormatedScaleText(_ymin, _yprec);
                    else
                        strText = GetFormatedScaleText(degree, _yprec);

                    size = g.MeasureString(strText, this.Font);
                    g.DrawString(strText, this.Font, textBrush, new Point((offset + maxTextExt) - (int)size.Width, (int)(degreePos)));

                    degreePos += degreeInc;
                    degree -= posInc;
                }
            }

            offset += maxTextExt + 11;
            
            //Degree lines
            degreePos = 15 + _top;            

            for (int n = 0; n <= _ydiv; n++)
            {
                if (n == 0)
                {
                    g.DrawLine(dPen, new Point(offset + 10, (int)(degreePos)),
                        new Point(offset - 10, (int)(degreePos)));
                }
                else if (n == _ydiv)
                {
                    g.DrawLine(dPen, new Point(offset + 10,height - 60),
                        new Point(offset - 10, height - 60));
                }
                else
                {
                    g.DrawLine(dPen, new Point(offset + 5, (int)(degreePos)),
                        new Point(offset - 5, (int)(degreePos)));

                }
                degreePos += degreeInc;
            }            

            //Draw axis
            g.DrawLine(dPen, new Point(offset, 15 + _top), new Point(offset,  height - 60));
            _leftOffset = offset;
        }

        private void DrawGrid(Graphics g)
        {

            double xPixelInc = (_drawRect.Width / (double)_gridXDev);
            double nPos = _drawRect.Left;
            Pen dPen = new Pen(_gridColor);
            dPen.Width = 1;
            dPen.DashStyle = _gridStyle;

            for (int x = 0; x < _gridXDev; x++)
            {
                g.DrawLine(dPen, new Point((int)nPos, (int)_drawRect.Top), new Point((int)nPos, (int)_drawRect.Bottom));
                nPos += xPixelInc;
            }

            g.DrawLine(dPen, new Point((int)(_drawRect.Right), (int)_drawRect.Top), new Point((int)(_drawRect.Right), (int)_drawRect.Bottom));

            double yPixeInc = (_drawRect.Height / (double)_gridYDev);
            nPos = _drawRect.Top;

            for (int y = 0; y < _gridYDev; y++)
            {
                g.DrawLine(dPen, new Point((int)_drawRect.Left, (int)(nPos)), new Point((int)_drawRect.Right, (int)(nPos)));
                nPos += yPixeInc;
            }
            g.DrawLine(dPen, new Point((int)(_drawRect.Left), (int)(_drawRect.Bottom)), new Point((int)_drawRect.Right, (int)(_drawRect.Bottom)));

        }

        private void DrawXAxis(Graphics g)
        {
            SolidBrush textBrush = new SolidBrush(_axisColor);
            int offset = 0;

            int width;
            if( controlPanel.Visible)
                width = this.Width - 5 - controlPanel.Width;
            else
                width = this.Width - 5;

            int height = this.Height - 5;
            //Axis text
            SizeF size = g.MeasureString(_xText, this.Font);
            offset = height - (int)size.Height - 5;
            int xTextWidth = (int) size.Width;

            //Max value text extend
            string maxstr = GetFormatedScaleText(_xmax, _xprec);

            size = g.MeasureString(maxstr, this.Font);

            g.DrawString(_xText, this.Font, textBrush, new Point((int)((width + _leftOffset - (int)size.Width) / 2) - xTextWidth / 2, offset+2));
          
            _leftOffset += 10;

            //Draw axis degree
            double posInc = (width - _leftOffset - size.Width) / (double)_xdiv;
            double degreePos = _leftOffset;

            double degreeInc = 0;
            double degree = _xmin;        

            if( _xLog)
            {
                double delta = Math.Log10(Math.Abs(_xmax)) - Math.Log10(Math.Abs(_xmin));

                degree = Math.Log10(_xmin);        
                degreeInc = delta / (double)_xdiv;
                offset -= 20;
                
                for (int n = 0; n <= _xdiv; ++n)
                {
                    string text = GetFormatedScaleText(Math.Pow(10,degree), _xprec);
                    g.DrawString(text, this.Font, textBrush, new Point((int)degreePos, offset));
                   
                    degree += degreeInc;
                    degreePos += posInc;
                }
            }
            else
            {
                degreeInc = (double)(_xmax - _xmin) / (double)_xdiv;

                offset -= 20;

                for (int n = 0; n <= _xdiv; ++n)
                {
                    string text = GetFormatedScaleText(degree, _xprec);
                    g.DrawString(text, this.Font, textBrush, new Point((int)degreePos, offset));
                    degree += degreeInc;
                    degreePos += posInc;
                }
            }

            //Draw axis line            
            offset = height - 50;
            int drawRectBottom = offset - 10;

            Pen linePen = new Pen(_axisColor);
            g.DrawLine(linePen, new Point(_leftOffset, offset), new Point(width - (int)size.Width, offset));

            degreePos = _leftOffset;

            //Draw degree
            for (int n = 0; n <= _xdiv; ++n)
            {
                //Draw next degree text
                string text = GetFormatedScaleText(degree, _xprec);

                if (n == 0)
                {
                    g.DrawLine(linePen, new Point((int)degreePos, offset + 10), new Point((int)degreePos, offset - 10));
                }
                else if (n == _xdiv)
                {
                    g.DrawLine(linePen, new Point(width - (int)size.Width, offset + 10), new Point(width - (int)size.Width, offset - 10));
                }
                else
                {
                    g.DrawLine(linePen, new Point((int)degreePos, offset + 5), new Point((int)degreePos, offset - 5));
                }

                degreePos += posInc;
            }                

            _drawRect = new Rectangle(_leftOffset, 15 + _top, width - _leftOffset - (int)size.Width, drawRectBottom - 15 - _top);
        }

        public bool EnableRepaint
        {
            set{ _repaint = value;}
            get { return _repaint; }
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XYChartDlg dlg = new XYChartDlg(this);
            dlg.ShowDialog();
        }

        private void Chart_DoubleClick(object sender, EventArgs e)
        {
            if (!_editable)
                return;

            XYChartDlg dlg = new XYChartDlg(this);
            dlg.ShowDialog();
        }

        private void Chart_BackColorChanged(object sender, EventArgs e)
        {
            _mutex.WaitOne();
            RepaintAll();
            _mutex.ReleaseMutex();
        }

        private void scaleToInitialSaluesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mutex.WaitOne();
            _xmax = _orgXMax;
            _xmin = _orgXMin;
            _ymin = _orgYMin;
            _ymax = _orgYMax;
            RepaintAll();
            
            _mutex.ReleaseMutex();
        }

        private void oKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void showLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_lineVisibleNoEvent)
                return;

            _mutex.WaitOne();

            _lineVisibleNoEvent = true;
            _lineVisible = !_lineVisible;
            showLineToolStripMenuItem.Checked = _lineVisible;
            onShowLine.Checked = _lineVisible;
            RepaintAll();
            
            _lineVisibleNoEvent = false;
            _mutex.ReleaseMutex();
        }

        private void showPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_pointVisibleNoEvent)
                return;

            _mutex.WaitOne();
            _pointVisibleNoEvent = true;
            _pointVisible = !_pointVisible;
            showPointsToolStripMenuItem.Checked = _pointVisible;
            onShowPoints.Checked = _pointVisible;
            RepaintAll();
            

            _pointVisibleNoEvent = false;
            _mutex.ReleaseMutex();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            _mutex.WaitOne();
            base.OnPaint(e);
            e.Graphics.DrawImage(_backBuffer, new Point(0,0));
            _mutex.ReleaseMutex();
            DrawSelectionRect(e.Graphics);            
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Point mousePos = new Point(e.X, e.Y);
            _selectionRect.X = mousePos.X;
            _selectionRect.Y = mousePos.Y;
            _beginDrag = true;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_beginDrag && _editable)
            {
                _beginDrag = false;
                _draging = true;
            }

            if (_draging)
            {
                Point mousePos = new Point(e.X, e.Y);
                _selectionRect.Width = mousePos.X - _selectionRect.X;
                _selectionRect.Height = mousePos.Y - _selectionRect.Y;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_draging)
            {
                _mutex.WaitOne();
                Point mousePos = new Point(e.X, e.Y);

                if (mousePos.X > _selectionRect.X)
                {
                    _selectionRect.Width = mousePos.X - _selectionRect.X;
                }
                else
                {
                    int x = _selectionRect.X;
                    _selectionRect.X = mousePos.X;
                    _selectionRect.Width = x - mousePos.X;
                }

                if (mousePos.Y > _selectionRect.Y)
                {
                    _selectionRect.Height = mousePos.Y - _selectionRect.Y;
                }
                else
                {
                    int y = _selectionRect.Y;
                    _selectionRect.Y = mousePos.Y;
                    _selectionRect.Height = y - mousePos.Y;
                }

                ScaleByRect(_selectionRect);
                _mutex.ReleaseMutex();
            }

            _draging = false;
            _beginDrag = false;
        }

        private void onZoomP_Click(object sender, EventArgs e)
        {
            _mutex.WaitOne();
            double xscale = (XMaximum - XMinimum) / XAxisDevision;
            double yscale = (YMaximum - YMinimum) / YAxisDevision;

            _xmax -= xscale;
            _xmin += xscale;
            _ymax -= yscale;
            _ymin += yscale;
            RepaintAll();
            
            _mutex.ReleaseMutex();
        }

        private void onZoomM_Click(object sender, EventArgs e)
        {
            _mutex.WaitOne();
            double xscale = (XMaximum - XMinimum) / XAxisDevision;
            double yscale = (YMaximum - YMinimum) / YAxisDevision;

            _xmax += xscale;
            _xmin -= xscale;
            _ymax += yscale;
            _ymin -= yscale;
            RepaintAll();
            
            _mutex.ReleaseMutex();
        }

        private void onTopPan_Click(object sender, EventArgs e)
        {
            _mutex.WaitOne();
            double yscale = (YMaximum - YMinimum) / YAxisDevision;
            _ymin -= yscale;
            _ymax -= yscale;
            RepaintAll();
            
            _mutex.ReleaseMutex();
        }

        private void onDownPen_Click(object sender, EventArgs e)
        {
            _mutex.WaitOne();
            double yscale = (YMaximum - YMinimum) / YAxisDevision;
            _ymin += yscale;
            _ymax += yscale;
            RepaintAll();
            
            _mutex.ReleaseMutex();
        }

        private void onLeftPan_Click(object sender, EventArgs e)
        {
            _mutex.WaitOne();
            double xscale = (XMaximum - XMinimum) / XAxisDevision;
            _xmin += xscale;
            _xmax += xscale;
            RepaintAll();
            
            _mutex.ReleaseMutex();
        }

        private void onRightPen_Click(object sender, EventArgs e)
        {
            _mutex.WaitOne();
            double xscale = (XMaximum - XMinimum) / XAxisDevision;
            _xmin -= xscale;
            _xmax -= xscale;
            RepaintAll();
            
            _mutex.ReleaseMutex();
        }


        private void onZoomReset_Click(object sender, EventArgs e)
        {
            _mutex.WaitOne();
            _xmax = _orgXMax;
            _xmin = _orgXMin;
            _ymin = _orgYMin;
            _ymax = _orgYMax;
            RepaintAll();
            
            _mutex.ReleaseMutex();
        }

        private void editReferenceCurvesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefCurveEditorDlg dlg = new RefCurveEditorDlg(this._refCurves);
            dlg.ShowDialog();

            _mutex.WaitOne();
            RepaintAll();
            
            _mutex.ReleaseMutex();            
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            propertiesToolStripMenuItem.Enabled = _editable;
            scaleToInitialSaluesToolStripMenuItem.Enabled = _editable;
            clearToolStripMenuItem.Enabled = _editable;
        }
    }
}
