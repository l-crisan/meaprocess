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

namespace Mp.Mod.Controller
{
    public partial class FuzzificationViewCtrl : UserControl
    {
        private int _leftOffset = 10;
        private int _xdiv = 5;
        private int _top = 10;
        private int _ydiv = 2;
        private Rectangle _drawRect;
        private Bitmap _backBuffer;
        private string _sigName;
        private double _min;
        private double _max;
        List<FuzzificationItem> _items = new List<FuzzificationItem>();

        public FuzzificationViewCtrl()
        {
            InitializeComponent();
            _backBuffer = new Bitmap(this.Width, this.Height);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _backBuffer = new Bitmap(this.Width, this.Height);
            DrawAll(_sigName, _min, _max);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(_backBuffer, new Point(0, 0));            
        }

        public void UpdateView(List<FuzzificationItem> items, string sigName, double min, double max)
        {
            _sigName = sigName;
            _min = min;
            _max = max;
            _items = items;
            DrawAll(sigName, min, max);
            Invalidate();
        }

        private void DrawAll(string sigName, double min, double max)
        {
            Graphics g = Graphics.FromImage(_backBuffer);
            g.SmoothingMode = SmoothingMode.HighQuality;

            DrawYAxis(g);
            DrawXAxis(g, sigName, min, max);

            int count = 0;
            foreach (FuzzificationItem item in _items)
            {
                Point p1 = new Point();
                Point p2 = new Point();
                Point p3 = new Point();
                Point p4 = new Point();

                if (count == 0)
                {
                    int x = ValueXToPixel(item.P1x);
                    int y = ValueYToPixel(100);
                    p1 = new Point(x, y);

                    x = ValueXToPixel(item.P2x);
                    y = ValueYToPixel(100);
                    p2 = new Point(x, y);

                    x = ValueXToPixel(item.P3x);
                    y = ValueYToPixel(100);
                    p3 = new Point(x, y);

                    x = ValueXToPixel(item.P4x);
                    y = ValueYToPixel(0);
                    p4 = new Point(x, y);
              
                }
                else if (count == _items.Count - 1)
                {
                    int x = ValueXToPixel(item.P1x);
                    int y = ValueYToPixel(0);
                    p1 = new Point(x, y);

                    x = ValueXToPixel(item.P2x);
                    y = ValueYToPixel(100);
                    p2 = new Point(x, y);

                    x = ValueXToPixel(item.P3x);
                    y = ValueYToPixel(100);
                    p3 = new Point(x, y);

                    x = ValueXToPixel(item.P4x);
                    y = ValueYToPixel(100);
                    p4 = new Point(x, y);
                }
                else
                {
                    int x = ValueXToPixel(item.P1x);
                    int y = ValueYToPixel(0);
                    p1 = new Point(x, y);

                    x = ValueXToPixel(item.P2x);
                    y = ValueYToPixel(100);
                    p2 = new Point(x, y);

                    x = ValueXToPixel(item.P3x);
                    y = ValueYToPixel(100);
                    p3 = new Point(x, y);

                    x = ValueXToPixel(item.P4x);
                    y = ValueYToPixel(0);
                    p4 = new Point(x, y);

                }

                if (item.Selected)
                {
                    g.DrawLine(new Pen(Color.Green, 2), p1, p2);
                    g.DrawLine(new Pen(Color.Green, 2), p2, p3);
                    g.DrawLine(new Pen(Color.Green, 2), p3, p4);
                }
                else
                {
                    g.DrawLine(new Pen(Color.Blue, 1), p1, p2);
                    g.DrawLine(new Pen(Color.Blue, 1), p2, p3);
                    g.DrawLine(new Pen(Color.Blue, 1), p3, p4);
                }
                count++;
            }
        }

        private string GetFormatedScaleText(double value, int precision)
        {
            return value.ToString("f" + precision.ToString());
        }

        private void DrawXAxis(Graphics g, string sigName, double min, double max)
        {
            SolidBrush textBrush = new SolidBrush(Color.Black);
            int offset = 0;

            int width = this.Width - 5;

            int height = this.Height - 5;
            //Axis text
            SizeF size = g.MeasureString(sigName, this.Font);
            offset = height - (int)size.Height;
            int xTextWidth = (int)size.Width;

            //Max value text extend
            string maxstr = GetFormatedScaleText(max, 2);

            size = g.MeasureString(maxstr, this.Font);

            g.DrawString(sigName, this.Font, textBrush, new Point((int)((width + _leftOffset - (int)size.Width) / 2) - xTextWidth / 2, offset + 2));

            //_leftOffset += 10;

            //Draw axis degree
            double posInc = (width - _leftOffset - size.Width) / (double)_xdiv;
            double degreePos = _leftOffset;

            double degreeInc = 0;
            double degree = min;

            degreeInc = (double)(max - min) / (double)_xdiv;

            offset -= 20;

            for (int n = 0; n <= _xdiv; ++n)
            {
                string text = GetFormatedScaleText(degree, 2);
                g.DrawString(text, this.Font, textBrush, new Point((int)degreePos, offset));
                degree += degreeInc;
                degreePos += posInc;
            }
            

            //Draw axis line            
            offset = height - 50;
            int drawRectBottom = offset;

            Pen linePen = new Pen(Color.Black);
            g.DrawLine(linePen, new Point(_leftOffset, offset), new Point(width - (int)size.Width, offset));

            degreePos = _leftOffset;

            //Draw degree
            for (int n = 0; n <= _xdiv; ++n)
            {
                //Draw next degree text
                string text = GetFormatedScaleText(degree, 2);

                if (n == 0)
                {
                    g.DrawLine(linePen, new Point((int)degreePos, offset+10), new Point((int)degreePos, offset));
                }
                else if (n == _xdiv)
                {
                    g.DrawLine(linePen, new Point(width - (int)size.Width, offset+10), new Point(width - (int)size.Width, offset));
                }
                else
                {
                    g.DrawLine(linePen, new Point((int)degreePos, offset+5), new Point((int)degreePos, offset));
                }

                degreePos += posInc;
            }

            _drawRect = new Rectangle(_leftOffset, 15 + _top, width - _leftOffset - (int)size.Width, drawRectBottom - 15 - _top);
        }

        private int ValueXToPixel(double x)
        {
            double min = _min;
            double max = _max;

            double factor = (_drawRect.Width) / (max - min);
            double offset = ((double)(_drawRect.Left * max) - (double)(_drawRect.Right * min)) / (max - min);

            return (int)(x * factor + offset);
        }

        private int ValueYToPixel(double y)
        {
            double verticalFactor = 0;
            double difference = 0.00001;

            double min = 0;
            double max = 100;

            if (Math.Abs(max - min) > difference)
                verticalFactor = (double)_drawRect.Height / (max - min);

            return (int)(_drawRect.Bottom - (int)((y - min) * verticalFactor));
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
            SolidBrush textBrush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();
            Matrix matrixRot270;
            Matrix oldMatrix;

            int height = this.Height - 5;

            drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;

            PointF pt;
            //Select the pen
            dPen = new Pen(Color.Black);

            //Y label
            size = g.MeasureString("(%)", this.Font);
            textExtend = (int)size.Height;

            pt = new Point((int)(offset), (int)(((height - 60 + _top) / 2) + (size.Width / 2)));

            oldMatrix = g.Transform;
            matrixRot270 = new Matrix();
            matrixRot270.RotateAt(270, pt);
            g.Transform = matrixRot270;

            g.DrawString("(%)", this.Font, textBrush, pt);
            g.Transform = oldMatrix;

            //Draw text degrees
            strText = GetFormatedScaleText(0, 0);

            size = g.MeasureString(strText, this.Font);

            if (maxTextExt < size.Width)
                maxTextExt = (int)size.Width;

            double degreePos = _top;
            double degreeInc = 0.0;
            degreeInc = (height - 65 - _top) / (double)_ydiv;

            offset += (int)(size.Height + 2);

            double posInc = (100 - 0) / _ydiv;
            double degree = 0;

            //Degree text
            for (int n = 0; n <= _ydiv; n++)
            {
                if (n == _ydiv)
                    strText = GetFormatedScaleText(0, 0);
                else
                    strText = GetFormatedScaleText(degree,0);

                size = g.MeasureString(strText, this.Font);
                textExtend = (int)size.Width;

                if (maxTextExt < textExtend)
                    maxTextExt = textExtend;

                degreePos += degreeInc;
                degree -= posInc;
            }

            degreePos = _top;
            degree = 100;

            for (int n = 0; n <= _ydiv; n++)
            {
                if (n == _ydiv)
                    strText = GetFormatedScaleText(0, 0);
                else
                    strText = GetFormatedScaleText(degree,0);

                size = g.MeasureString(strText, this.Font);
                g.DrawString(strText, this.Font, textBrush, new Point((offset + maxTextExt) - (int)size.Width, (int)(degreePos)));

                degreePos += degreeInc;
                degree -= posInc;
            }

            offset += maxTextExt + 11;

            //Degree lines
            degreePos = 15 + _top;

            for (int n = 0; n <= _ydiv; n++)
            {
                if (n == 0)
                {
                    g.DrawLine(dPen, new Point(offset, (int)(degreePos)),
                        new Point(offset - 10, (int)(degreePos)));
                }
                else if (n == _ydiv)
                {
                    g.DrawLine(dPen, new Point(offset, height - 50),
                        new Point(offset - 10, height - 50));
                }
                else
                {
                    g.DrawLine(dPen, new Point(offset, (int)(degreePos)),
                        new Point(offset - 5, (int)(degreePos)));

                }
                degreePos += degreeInc;
            }

            //Draw axis
            g.DrawLine(dPen, new Point(offset, 15 + _top), new Point(offset, height - 50));
            _leftOffset = offset;
        }

    }
}
