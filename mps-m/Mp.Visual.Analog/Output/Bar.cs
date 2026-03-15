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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Mp.Visual.Analog
{
    [Serializable]
    public partial class Bar : UserControl
    {
        public enum CtrlOrientation
        {
            Vertical,
            Horizontal
        };

        private Rectangle _drawRect;
        private CtrlOrientation _orientation;
        private double _min = 0;
        private double _max = 10;
        private string _sigName;
        private int _division = 10;
        private int _precision = 0;
        private int _leftOffset = 10;
        private int _top = 5;
        private double _value = 0;
        private Color _barColor = Color.Blue;
 

        public Bar()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            //this.Font = new Font("Arial", 10);

            BackColor = Color.White;            
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        private void DrawValue(Graphics g)
        {
            if (_min == _max)
                return;

            Rectangle clearRect = new Rectangle(_drawRect.X+1,_drawRect.Y+1, _drawRect.Width-1, _drawRect.Height-1);

            double value = _value;

            if (_value > _max)
                value = _max;

            if (value < _min)
                value = _min;
            
            g.FillRectangle(new SolidBrush(BackColor), clearRect);

            if (_drawRect.Width > 0 && _drawRect.Height > 0)
            {

                if (_orientation == CtrlOrientation.Vertical)
                {
                    LinearGradientBrush brush = new LinearGradientBrush(_drawRect, _barColor, Color.LightGray, LinearGradientMode.Horizontal);
                    double factor = -clearRect.Height / (_max - _min);
                    double offset = (clearRect.Bottom * _max - clearRect.Top * _min) / (_max - _min);

                    int delta = (int)(value * factor + offset);
                    Rectangle fillRect = new Rectangle(clearRect.Left, delta, clearRect.Width, clearRect.Bottom - delta);
                    g.FillRectangle(brush, fillRect);
                }
                else
                {
                    LinearGradientBrush brush = new LinearGradientBrush(_drawRect, _barColor, Color.LightGray, LinearGradientMode.Vertical);
                    double factor = clearRect.Width / (_max - _min);
                    double offset = (clearRect.Left * _max - clearRect.Right * _min) / (_max - _min);

                    int delta = (int)(value * factor + offset);
                    Rectangle fillRect = new Rectangle(clearRect.Left, clearRect.Top, delta - clearRect.Left, clearRect.Height);
                    g.FillRectangle(brush, fillRect);

                }
            }
        }

        [SRDescription("BarColor")]
        [SRCategory("View")]
        public Color BarColor
        {
            get { return _barColor; }
            set{ 
                _barColor = value;
                Invalidate();
            }
        }

        [SRDescription("Minimum")]
        [SRCategory("View")]
        public double Minimum
        {
            set 
            { 
                _min = value;
                Invalidate();
            }
            get { return _min; }
        }

        [SRDescription("Maximum")]
        [SRCategory("View")]
        public double Maximum
        {
            set 
            { 
                _max = value;
                Invalidate();
            }

            get{ return _max;}
        }

        public string SigName
        {
            get { return _sigName; }
            set 
            { 
                _sigName = value;
                Invalidate();
            }
        }

        [SRDescription("AxisDivision")]
        [SRCategory("View")]
        public int AxisDevision
        {
            get { return _division; }
            set 
            { 
                _division = value;
                Invalidate();
            }
        }

        [SRDescription("AxisDivision")]
        [SRCategory("View")]
        public int AxisDivision
        {
            get { return _division; }
            set
            {
                _division = value;
                Invalidate();
            }
        }

        [SRDescription("AxisPrecision")]
        [SRCategory("View")]
        public int AxisPrecision
        {
            get { return _precision; }
            set 
            { 
                _precision = value;
                Invalidate();
            }
        }

        [SRDescription("Orientation")]
        [SRCategory("View")]
        public CtrlOrientation Orientation
        {
            get { return _orientation; }
            set 
            { 
                _orientation = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawAxis(e.Graphics);
            DrawValue(e.Graphics);
        }

        [SRDescription("BarCtrlValue")]
        [SRCategory("View")]
        public double Value
        {           
            get{ return _value; }
            set 
            { 
                _value = value;
                Invalidate();
            }
        }

        private string GetFormatedScaleText(double value, int precision)
        {
            return value.ToString("f" + precision.ToString());
        }

        private void DrawAxis(Graphics g)
        {
            g.Clear(BackColor);

            if (_orientation == Bar.CtrlOrientation.Horizontal)
                DrawHorizontalAxis(g);
            else
                DrawVerticalAxis(g);

            g.DrawRectangle(new Pen(ForeColor), _drawRect);
        }

        private void DrawVerticalAxis(Graphics g)
        {
            Pen dPen;
            int offset = 0;
            int maxTextExt = 0;
            SizeF size = new SizeF(0,0);
            string strText;
            int textExtend = 0;
            SolidBrush textBrush = new SolidBrush(ForeColor);
            StringFormat drawFormat = new StringFormat();
            Matrix matrixRot270;
            Matrix oldMatrix;
            int topOffset = 0;

            int height = this.Height - 5;

        
                offset = 5; 
                topOffset = 15;
                drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

                PointF pt;
                //Select the pen
                dPen = new Pen(ForeColor);

                //Y label
                size = g.MeasureString(_sigName, this.Font);
                textExtend = (int)size.Height;

                pt = new Point((int)(offset), (int)(((height - 5 + _top) / 2) + (size.Width / 2)));

                oldMatrix = g.Transform;
                matrixRot270 = new Matrix();
                matrixRot270.RotateAt(270, pt);
                g.Transform = matrixRot270;

                g.DrawString(_sigName, this.Font, textBrush, pt);
                g.Transform = oldMatrix;

                //Draw text degrees
                strText = GetFormatedScaleText(_min, _precision);

                size = g.MeasureString(strText, this.Font);

                if (maxTextExt < size.Width)
                    maxTextExt = (int)size.Width;

                double degreePos = _top;
                double degreeInc = (height - 20 - _top) / (double)_division;

                offset += (int)(size.Height + 2);

                double posInc = (_max - _min) / _division;
                double degree = _min;

                //Degree text
                for (int n = 0; n <= _division; n++)
                {
                    if (n == _division)
                        strText = GetFormatedScaleText(_min, _precision);
                    else
                        strText = GetFormatedScaleText(degree, _precision);

                    size = g.MeasureString(strText, this.Font);
                    textExtend = (int)size.Width;

                    if (maxTextExt < textExtend)
                        maxTextExt = textExtend;

                    degreePos += degreeInc;
                    degree -= posInc;
                }

                degreePos = _top;
                degree = _max;

                for (int n = 0; n <= _division; n++)
                {
                    if (n == _division)
                        strText = GetFormatedScaleText(_min, _precision);
                    else
                        strText = GetFormatedScaleText(degree, _precision);

                    size = g.MeasureString(strText, this.Font);
                    g.DrawString(strText, this.Font, textBrush, new Point((offset + maxTextExt) - (int)size.Width, (int)(degreePos)));

                    degreePos += degreeInc;
                    degree -= posInc;
                }

                offset += maxTextExt + 11;

                //Degree lines
                degreePos = 15 + _top;

                for (int n = 0; n <= _division; n++)
                {
                    if (n == 0)
                    {
                        g.DrawLine(dPen, new Point(offset, (int)(degreePos)),
                            new Point(offset - 10, (int)(degreePos)));
                    }
                    else if (n == _division)
                    {
                        g.DrawLine(dPen, new Point(offset, height - 5),
                            new Point(offset - 10, height - 5));
                    }
                    else
                    {
                        g.DrawLine(dPen, new Point(offset, (int)(degreePos)),
                            new Point(offset - 5, (int)(degreePos)));

                    }
                    degreePos += degreeInc;
                }

                //Draw axis
                g.DrawLine(dPen, new Point(offset, 15 + _top), new Point(offset, height - 5));             

            Rectangle oldRect = _drawRect;
            _drawRect = new Rectangle(offset + 5, topOffset + _top, this.Width - offset - 15, height - topOffset - _top - 5);
            
            if (_drawRect.Width < 1 || _drawRect.Height < 1)
                _drawRect = oldRect;
        }

        private void DrawHorizontalAxis(Graphics g)
        {            
            SolidBrush textBrush = new SolidBrush(Color.Black);
            int offset = 0;
            int drawRectBottom = this.Height - 5;
            int width = this.Width - 5;
            int height = this.Height - 5;
            SizeF size = new SizeF(0, 0);


                //Axis text
                size = g.MeasureString(_sigName, this.Font);
                offset = height - (int)size.Height;
                int xTextWidth = (int)size.Width;

                //Max value text extend
                string maxstr = GetFormatedScaleText(_max, _precision);

                size = g.MeasureString(maxstr, this.Font);

                g.DrawString(_sigName, this.Font, textBrush, new Point((int)((width + _leftOffset - (int)size.Width) / 2) - xTextWidth / 2, offset + 2));

                //Draw axis degree
                double posInc = (width - _leftOffset - size.Width - 5) / (double)_division;
                double degreePos = _leftOffset;

                double degreeInc = (double)(_max - _min) / (double)_division;
                double degree = _min;

                offset -= 20;

                for (int n = 0; n <= _division; ++n)
                {
                    string text = GetFormatedScaleText(degree, _precision);
                    g.DrawString(text, this.Font, textBrush, new Point((int)degreePos, offset));
                    degree += degreeInc;
                    degreePos += posInc;
                }


                //Draw axis line            
                offset = height - 50;
                drawRectBottom = offset;

                Pen linePen = new Pen(Color.Black);
                g.DrawLine(linePen, new Point(_leftOffset, offset), new Point(width - (int)size.Width - 5, offset));

                degreePos = _leftOffset;

                //Draw degree
                for (int n = 0; n <= _division; ++n)
                {
                    //Draw next degree text
                    string text = GetFormatedScaleText(degree, 2);

                    if (n == 0)
                    {
                        g.DrawLine(linePen, new Point((int)degreePos, offset + 10), new Point((int)degreePos, offset));
                    }
                    else if (n == _division)
                    {
                        g.DrawLine(linePen, new Point(width - (int)size.Width - 5, offset + 10), new Point(width - (int)size.Width - 5, offset));
                    }
                    else
                    {
                        g.DrawLine(linePen, new Point((int)degreePos, offset + 5), new Point((int)degreePos, offset));
                    }

                    degreePos += posInc;
                }

            Rectangle oldRect = _drawRect;
            _drawRect = new Rectangle(_leftOffset, _top-2, width - _leftOffset - (int)size.Width - 5, drawRectBottom - _top - 5);

            if (_drawRect.Width < 1 || _drawRect.Height < 1)
                _drawRect = oldRect;

        }
    }
}
