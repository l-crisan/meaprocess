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

namespace Mp.Visual.PolarChart
{
    public partial class PolarChart : UserControl
    {
        private int _margine = 5;
        private AngleType _angleType;
        private Color _axisColor = Color.Black;
        private int _angleDivision = 8;
        private int _radiusDivision = 3;
        private double _rmin = -10;
        private double _rmax = 10;
        private int _rPrecision = 1;
        private double _angleValue = 0;
        private double _radiusValue = 0;
        private Color _indicatorColor = Color.Blue;
        private int _chartRadius;
        private Point _center;
        private string _titel = "Polar";

        public enum AngleType
        {
            Radiant, //2PI
            Degree   //360 
        }

        public PolarChart()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            
        }

        [SRCategory("View"),
        SRDescription("IndicatorColor")]
        public Color IndicatorColor
        {
            get { return _indicatorColor; }
            set { _indicatorColor = value; }

        }

        [SRCategory("View"),
        SRDescription("RadiusPrecision")]
        public int RadiusPrecision
        {
            set { _rPrecision = value; }
            get{ return _rPrecision;}
        }

        [SRCategory("View"),
        SRDescription("AxisColor")]
        public Color AxisColor
        {
            get { return _axisColor; }
            set 
            { 
                _axisColor = value;
                Invalidate();
            }
        }

        [SRCategory("View"),
        SRDescription("Titel")]
        public string Titel
        {
            get { return _titel; }
            set
            {
                _titel = value;
                Invalidate();
            }
        }

        public double RadiusMinimum
        {
            get { return _rmin; }
            set { _rmin = value; }
        }

        public double RadiusMaximum
        {
            get { return _rmax; }
            set { _rmax = value; }
        }

        [SRCategory("View"),
        SRDescription("AngleDivision")]
        public int AngleDivision
        {
            get { return _angleDivision; }
            set 
            { 
                _angleDivision = value;
                if (_angleDivision == 0)
                    _angleDivision = 1;
                Invalidate();
            }
        }

        [SRCategory("View"),
        SRDescription("RadiusDivision")]
        public int RadiusDivision
        {
            get { return _radiusDivision; }
            set
            {
                _radiusDivision = value;
                if (_radiusDivision == 0)
                    _radiusDivision = 1;
                Invalidate();
            }
        }

        [SRCategory("View"),
        SRDescription("AngleType")]
        public AngleType Angle
        {
            get { return _angleType; }
            set 
            { 
                _angleType = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawAxis(e.Graphics);
            DrawValue(e.Graphics);
        }

        private double DegreeToRadiant(double degree)
        {
            return degree * Math.PI / 360;
        }

        public void SetValue( double radius, double angle)
        {
            if (_angleType == AngleType.Degree)
            {
                angle = Modulo(angle, 360);
                angle = (angle * Math.PI * 2) / 360;
            }
            else
            {
                angle = Modulo(angle, Math.PI * 2);
            }

            _angleValue = angle;

            if (radius > _rmax)
                _radiusValue = _rmax;
            else if( radius < _rmin)
                _radiusValue = _rmin;
            else
                _radiusValue = radius;
            Invalidate();
        }

        private double Modulo(double value, double modulo)
        {
            while (value > modulo)
                value -= modulo;

            return value;
        }

        private int ValueToPixel(double value)
        {
            if (_rmax == _rmin)
                return _center.X;

            double factor = _chartRadius / (_rmax - _rmin);
            double offset = (-_chartRadius * _rmin) / (_rmax - _rmin);

            return (int)(value * factor + offset);
        }

        private void DrawValue(Graphics g)
        {
            Point point = new Point();

            int radius = ValueToPixel(_radiusValue);
                        

            if (_angleValue <= Math.PI / 2.0)
            {
                int x = (int)(Math.Cos(_angleValue) * radius);
                int y = (int)(Math.Sin(_angleValue) * radius);
                point = new Point(_center.X + x, _center.Y - y);
            }
            else if (_angleValue > (Math.PI / 2.0) && _angleValue <= Math.PI)
            {
                int x = (int)(Math.Cos(Math.PI - _angleValue) * radius);
                int y = (int)(Math.Sin(Math.PI - _angleValue) * radius);

                point = new Point(_center.X - x, _center.Y - y);

            }
            else if (_angleValue > Math.PI && _angleValue <= (Math.PI * 3 / 2.0))
            {
                int x = (int)(Math.Cos(_angleValue - Math.PI) * radius);
                int y = (int)(Math.Sin(_angleValue - Math.PI) * radius);
                point = new Point(_center.X - x, _center.Y + y);

            }
            else if (_angleValue > (Math.PI * 3 / 2.0))
            {
                int x = (int)(Math.Cos(2 * Math.PI - _angleValue) * radius);
                int y = (int)(Math.Sin(2 * Math.PI - _angleValue) * radius);
                point = new Point(_center.X + x, _center.Y + y);
            }

            g.DrawLine(new Pen(_indicatorColor, 2), _center, point);
        }

        private void DrawAxis(Graphics g)
        {
            _center = new Point(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);

            SizeF size = new SizeF();

            Font titelFont = new System.Drawing.Font(this.Font, FontStyle.Bold);

            size = g.MeasureString(_titel, titelFont);

            g.DrawString(_titel, titelFont, new SolidBrush(ForeColor), new PointF(_center.X - size.Width / 2, 3+ size.Height / 2));
            double v = 360;
            _center.Y += (int) size.Height;

            if (_angleType == AngleType.Radiant)
                v = Math.PI * 2;

            size = g.MeasureString(v.ToString("f2"), this.Font);

            int top = (int)size.Height + _margine;
            int bottom = this.ClientRectangle.Height - (int)size.Height - _margine;
            int left = (int)size.Width + _margine;
            int right = (int)this.ClientRectangle.Width - (int)size.Width - _margine;

            _chartRadius = (right - left) / 2;

            //Draw angle division
            double angleInc = (Math.PI*2)/_angleDivision;
            double angle = 0;
            double degreeAngle = 0;
            double degreeInc = 360.0 / _angleDivision; ;

            for (int i = 0; i < _angleDivision; ++i)
            {
                Point point = new Point();

                if (angle <= Math.PI / 2.0)
                {
                    int x = (int)(Math.Cos(angle) * _chartRadius);
                    int y = (int)(Math.Sin(angle) * _chartRadius);
                    point = new Point(_center.X + x, _center.Y - y);
                    string degreeText = "";
                    if (_angleType == AngleType.Degree)
                        degreeText = degreeAngle.ToString("f1") + "°";
                    else
                        degreeText = angle.ToString("f2");

                    g.DrawString(degreeText, this.Font, new SolidBrush(ForeColor), new PointF(point.X, point.Y - size.Height));
                }
                else if (angle > (Math.PI / 2.0) && angle <= Math.PI)
                {
                    int x = (int)(Math.Cos(Math.PI - angle) * _chartRadius);
                    int y = (int)(Math.Sin(Math.PI - angle) * _chartRadius);

                    point = new Point(_center.X - x, _center.Y - y);

                    string degreeText = "";
                    if (_angleType == AngleType.Degree)
                        degreeText = degreeAngle.ToString("f1") + "°";
                    else
                        degreeText = angle.ToString("f2");

                    g.DrawString(degreeText, this.Font, new SolidBrush(ForeColor), new PointF(point.X - size.Width + 2, point.Y - size.Height));

                }
                else if (angle > Math.PI && angle <= (Math.PI * 3 / 2.0))
                {
                    int x = (int)(Math.Cos(angle - Math.PI) * _chartRadius);
                    int y = (int)(Math.Sin(angle - Math.PI) * _chartRadius);
                    point = new Point(_center.X - x, _center.Y + y);
                    
                    string degreeText = "";
                    if (_angleType == AngleType.Degree)
                        degreeText = degreeAngle.ToString("f1") + "°";
                    else
                        degreeText = angle.ToString("f2");


                    g.DrawString(degreeText, this.Font, new SolidBrush(ForeColor), new PointF(point.X - size.Width, point.Y));
                }
                else if (angle > (Math.PI * 3 / 2.0))
                {
                    int x = (int)(Math.Cos(2 * Math.PI - angle) * _chartRadius);
                    int y = (int)(Math.Sin(2 * Math.PI - angle) * _chartRadius);
                    point = new Point(_center.X + x, _center.Y + y);

                    string degreeText = "";
                    if (_angleType == AngleType.Degree)
                        degreeText = degreeAngle.ToString("f1") + "°";
                    else
                        degreeText = angle.ToString("f2");

                    g.DrawString(degreeText, this.Font, new SolidBrush(ForeColor), new PointF(point.X, point.Y));
                }

                g.DrawLine(new Pen(_axisColor), _center, point);

                angle += angleInc;
                degreeAngle += degreeInc;
            }

            //Draw the radius devision
            double radiusInc = (double)_chartRadius / (double)_radiusDivision;
            double curRadius = radiusInc;
            double rValue = _rmin;
            double rValueInc = (_rmax - _rmin) / _radiusDivision;
            string strValue = "";
            for (int i = 0; i < _radiusDivision; ++i)
            {
                Rectangle rect = new Rectangle((int)(_center.X - curRadius), (int)(_center.Y - curRadius), (int)(2 * curRadius), (int)(2 * curRadius));

                g.DrawEllipse(new Pen(_axisColor), rect);


                strValue = rValue.ToString("f" + _rPrecision.ToString());
                g.DrawString(strValue, this.Font, new SolidBrush(ForeColor), new PointF((float)(_center.X + (curRadius - radiusInc)), _center.Y + size.Height / 2));                

                curRadius += radiusInc;
                rValue += rValueInc;
            }

            strValue = rValue.ToString("f" + _rPrecision.ToString());
            g.DrawString(strValue, this.Font, new SolidBrush(ForeColor), new PointF((float)(_center.X + (curRadius - radiusInc)), _center.Y + size.Height / 2));                

        }
    }
}
