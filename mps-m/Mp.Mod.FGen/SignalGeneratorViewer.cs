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
using System.Xml;


namespace Mp.Mod.FGen
{
        internal partial class SignalGeneratorViewer : UserControl
        {
            public class SignalData
            {
                public SignalData()
                { }

                public double Min = -10;
                public double Max = 10;
                public double Periode = 10000;
                public double GenRate = 10;
                public double Phase;
                public double OnPeriode;
                public double Parameter;
                public FunctionGenPS.FunctionType Type;
            }

            private SignalData  _data       = new SignalData();
            private SolidBrush  _brush      = new SolidBrush(Color.GreenYellow);
            private Random      _random     = new Random(288);
            private bool        _showPoints = false;
            private Pen         _linePen = new Pen(Color.White);
            private Pen         _pointPen = new Pen(Color.Violet);
            private Bitmap      _backBuffer;

            public SignalGeneratorViewer()
            {
                InitializeComponent();
                _backBuffer = new Bitmap(this.Width, this.Height);
            }

            public SignalData Data
            {
                set
                {
                    _data = value;
                    DrawContent();
                    Invalidate();
                }
                get
                {
                    return _data;
                }
            }

            protected override void OnResize(EventArgs e)
            {
                base.OnResize(e);
                
                if( this.Width != 0 && this.Height != 0)
                    _backBuffer = new Bitmap(this.Width, this.Height);

                DrawContent();
                Invalidate();                
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.DrawImage(_backBuffer, new Point(0,0));
            }

            private void DrawContent()
            {
                if (_data.Periode == 0)
                    return;

                if (_data.Max < _data.Min)
                    return;

                if (_data.Max == _data.Min)
                    return;

                Graphics g = Graphics.FromImage(_backBuffer);
                g.Clear(Color.Black);

                Rectangle drawRect = new Rectangle(40, 5, ClientSize.Width - 50, ClientSize.Height - 20);

                //Axis
                //X
                g.DrawLine(new Pen(Color.Yellow), new Point(drawRect.Left, drawRect.Top), new Point(drawRect.Left, drawRect.Bottom));

                //Y
                Pen p = new Pen(Color.LightGreen);
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                g.DrawLine( p, new Point(drawRect.Left, drawRect.Top + drawRect.Height / 2), new Point(drawRect.Right, drawRect.Top + drawRect.Height / 2));
                g.DrawLine( p, new Point(drawRect.Left, drawRect.Top), new Point(drawRect.Right, drawRect.Top));
                g.DrawLine( p, new Point(drawRect.Left, drawRect.Bottom), new Point(drawRect.Right, drawRect.Bottom));

                //Degree
                g.DrawLine(new Pen(Color.Yellow), new Point(drawRect.Left - 5, drawRect.Top), new Point(drawRect.Left + 5, drawRect.Top));
                g.DrawLine(new Pen(Color.Yellow), new Point(drawRect.Left - 5, drawRect.Bottom), new Point(drawRect.Left + 5, drawRect.Bottom));
                g.DrawLine(new Pen(Color.Yellow), new Point(drawRect.Right, drawRect.Top + (drawRect.Height / 2) - 5), new Point(drawRect.Right, drawRect.Top + (drawRect.Height / 2) + 5));

                //Min/Max text 
                g.DrawString(_data.Max.ToString(), this.Font, _brush, new Point(3, 3));
                g.DrawString(_data.Min.ToString(), this.Font, _brush, new Point(3, ClientSize.Height - 15));
                double periode = _data.Periode;

                if(_data.Type == FunctionGenPS.FunctionType.RampUp ||
                    _data.Type == FunctionGenPS.FunctionType.RampDown)
                {
                    DrawTriangle(g, drawRect);
                    periode = CalcPeriode();
                }
                else
                {
                    DrawFunction(g, drawRect);
                }

                //Periode
                string str = String.Format( "{0}",periode)+ " (ms)";
                SizeF s = g.MeasureString(str, this.Font);
                g.DrawString(str, this.Font, _brush, new Point(ClientSize.Width - (int)s.Width, (ClientSize.Height / 2) + 5));
            }

            public double CalcPeriode()
            {
                if (_data.Periode == 0)
                    return 0;

                //Generation rate in ms
                double genRate  = 1000 / _data.GenRate;
                
                //Round the periode to multiplier from genRate
                double s = (_data.Periode % genRate);

                double periode = _data.Periode;
                
                if( s != 0)
                    periode = _data.Periode + (genRate - s);

                //Check if we should add a genRate to periode.
                if ( ((periode / genRate) %2) == 0)
                    return periode + genRate;

                return periode;
            }

            public  double CalcPhase()
            {
                double periode = CalcPeriode();
                
                if( periode == 0)
                    return _data.Phase;

                double phase = _data.Phase;

                if (_data.Phase == 0)
                    return 0;

                if (Math.Abs(_data.Phase) < (1000 / _data.GenRate))
                {
                    if( _data.Phase > 0)
                        return 1000 / _data.GenRate;
                    else
                        return -(1000 / _data.GenRate);
                }

                if (Math.Abs(_data.Phase) > (periode / 2))
                {
                    if (_data.Phase > periode / 2)
                        phase = -(periode / 2 - (_data.Phase % (periode / 2)));
                    else
                        phase = periode/2 + (_data.Phase % (periode / 2));
                }

                return phase;
            }

            private void DrawTriangle(Graphics g, Rectangle drawRect)
            {
                double periode = CalcPeriode()/1000.0;

                Point lastPoint = new Point(); 
                Point curPoint = new Point();
                bool  first = true;
                
                //Calcutlate the number of points pro periode.
                double noOfPoints = (periode * _data.GenRate);

                if (noOfPoints < 3)
                    return;
                
                //Calculate the pixel slope to convert y into pixel.
                double pixSlope = drawRect.Height / (_data.Max - _data.Min);
                
                //X increment.
                double xPix = drawRect.Left;
                double xPixInc = drawRect.Width / noOfPoints;
                double xValueInc = periode / noOfPoints;
                double slope = 0;
                double yValueInc = 0;
                double yValue = 0;

                if (_data.Type == FunctionGenPS.FunctionType.RampUp)
                {
                    //Calculate the slope.
                    //double slope = ((_data.Max - _data.Min) / 2) / ((periode / 2000.0) - xValueInc / 2);
                     slope = ((_data.Max - _data.Min)) / (periode - xValueInc);

                    //Y increment.
                    yValueInc = xValueInc * slope;

                    //Calculate the phase displacement.
                    double phase = CalcPhase()/1000.0;
                
                    //Calculate start y value.

                    // y = x*slope + b
                    // x = (-phase), b = (((_data.Max - _data.Min) / 2) + _data.Min)

                    yValue = (slope * (-phase)) + (((_data.Max - _data.Min) / 2.0) + _data.Min);

                    if (yValue < _data.Min)
                        yValue = _data.Max - Math.Abs(_data.Min - yValue);

                    if (yValue > _data.Max)
                        yValue = _data.Min + Math.Abs(_data.Max - yValue);
                }
                else
                {
                    //Calculate the slope.
                    //double slope = ((_data.Max - _data.Min) / 2) / ((periode / 2000.0) - xValueInc / 2);
                    slope = -((_data.Max - _data.Min)) / (periode/2 - xValueInc);

                    //Y increment.
                    yValueInc = xValueInc * slope;

                    //Calculate the phase displacement.
                    double phase = CalcPhase() / 1000.0;

                    //Calculate start y value.

                    // y = x*slope + b
                    // x = (-phase), b = (((_data.Max - _data.Min) / 2) + _data.Min)

                    yValue = (slope * (-phase)) + _data.Max;// (((_data.Max - _data.Min) / 2.0) + _data.Min);

                    if (yValue < _data.Min)
                        yValue = _data.Max - Math.Abs(_data.Min - yValue);

                    if (yValue > _data.Max)
                        yValue = _data.Min + Math.Abs(_data.Max - yValue);
                }

                //Calculate x,y through incrementation.
                while (xPix < (drawRect.Right + xPixInc))
                {                    
                    int yPix = (int)(drawRect.Bottom - ((yValue - _data.Min) * pixSlope));
                    curPoint = new Point((int)xPix, yPix);

                    if (first)
                    {
                        lastPoint = curPoint;
                        first = false;
                    }

                    Region r = g.Clip;
                    
                    g.Clip = new Region(drawRect);
                    if (lastPoint.X != curPoint.X || lastPoint.Y != curPoint.Y)
                    {
                        if (_showPoints)
                            g.DrawEllipse(_pointPen, (int)curPoint.X - 3, curPoint.Y - 3, 6, 6);

                        Point interPoint = new Point(lastPoint.X + (curPoint.X - lastPoint.X), lastPoint.Y);
                        g.DrawLine(_linePen, lastPoint, interPoint);
                        g.DrawLine(_linePen, interPoint, curPoint);
                    }
                    g.Clip = r;

                    xPix   += xPixInc;
                    yValue += yValueInc;
                    if (_data.Type == FunctionGenPS.FunctionType.RampUp)
                    {
                        if (Math.Round(yValue, 3) > _data.Max)
                            yValue = _data.Min;
                    }
                    else
                    {
                        if (Math.Round(yValue, 3) < _data.Min)
                            yValue = _data.Max;                        
                    }

                    lastPoint = curPoint;
                }
            }

            private void DrawFunction(Graphics g, Rectangle drawRect)
            {
                int     yPix;
                bool    first         = true;
                double  xValueCounter = 0;
                double  xValueInc     = 0;
                double  yValue        = 0;
                Point   lastPoint     = new Point(0, 0);
                Point   curPoint      = new Point(0, 0);
                double  noOfPoints    = (double)((_data.Periode / 1000.0) * _data.GenRate);
                double  onPoints      = (_data.OnPeriode / 1000.0) * _data.GenRate;
                double  pixSlope      = drawRect.Height / (_data.Max - _data.Min);
                double  xPix          = drawRect.Left;
                double  xPixInc       = drawRect.Width / noOfPoints;
                double  phaseInPoints = (_data.Phase / 1000.0 * _data.GenRate);
                int     phaseInPixel  = (int) ((drawRect.Width * phaseInPoints) / noOfPoints);
                int     onInPixel = (int) ((drawRect.Width * onPoints) / (noOfPoints));
                int     offInPixel = drawRect.Width - onInPixel;
                

                if(_data.Type == FunctionGenPS.FunctionType.Sine||
                   _data.Type == FunctionGenPS.FunctionType.SinePlus ||
                   _data.Type == FunctionGenPS.FunctionType.SineMinus)
                {
                    if (_data.Periode == 0)
                        return;

                    xValueInc = (2 * Math.PI) / (_data.GenRate * (_data.Periode / 1000.0));
                    xValueCounter = _data.Phase * (2 * Math.PI / 360.0); //Transform to rad.
                }

                if(_data.Type == FunctionGenPS.FunctionType.ExpMinus ||
                   _data.Type == FunctionGenPS.FunctionType.ExpPlus)
                {
                    if (_data.Periode == 0)
                        return;

                    xValueInc = 5 / (_data.GenRate * (_data.Periode / 1000.0));

                    xValueCounter = -5; 
                }

                if (_data.Type == FunctionGenPS.FunctionType.HalfRoundPlus ||
                   _data.Type == FunctionGenPS.FunctionType.HalfRoundMinus)
                {
                    if (_data.Periode == 0)
                        return;

                    xValueInc = (2*Math.PI) / (_data.GenRate * (_data.Periode / 2000.0));
                    xValueCounter = _data.Phase * (2*Math.PI / 360.0); //Transform to rad.
                }
                
                if (_data.Type == FunctionGenPS.FunctionType.Sinc ||
                    _data.Type == FunctionGenPS.FunctionType.SincMinus)
                {
                    if (_data.Periode == 0)
                        return;

                    double gradPer = 0;

                    if(_data.Parameter != 0)
                        gradPer = _data.Parameter * Math.PI;
                    else
                        gradPer = 5 * Math.PI;

                    xValueInc = gradPer/ (_data.GenRate * (_data.Periode / 1000.0));
                    xValueCounter = -gradPer/2 + _data.Phase * (gradPer / 360.0); //Transform to rad.
                }


                int pointCounter = (int) -phaseInPoints;

                while( ((int)xPix) <= drawRect.Right)
                {
                    switch (_data.Type)
                    {
                        case FunctionGenPS.FunctionType.Sine:
                        {
                            yValue = Math.Sin(xValueCounter);
                            double delta = (_data.Max - _data.Min) / 2;
                            yValue *= delta;
                            double offset = (delta + _data.Min);
                            yValue += offset;
                            xValueCounter += xValueInc;
                        }
                        break;

                        case FunctionGenPS.FunctionType.ExpPlus:
                        {
                            yValue = Math.Exp(xValueCounter);
                            double delta = (_data.Max - _data.Min) / 2;
                            yValue *= delta;
                            double offset = (delta + _data.Min);
                            yValue += offset;
                            xValueCounter += xValueInc;
                        }
                        break;

                        case FunctionGenPS.FunctionType.ExpMinus:
                        {
                            yValue = Math.Exp(xValueCounter);
                            double delta = (_data.Max - _data.Min) / 2;
                            yValue *= delta;
                            double offset = (delta + _data.Min);
                            yValue += offset;
                            xValueCounter += xValueInc;
                            yValue = 2 * offset - yValue;
                        }
                        break;

                        case FunctionGenPS.FunctionType.HalfRoundPlus:
                        {
                            if (xValueCounter < Math.PI)
                                yValue = Math.Sqrt(Math.Pow(Math.PI,2) - Math.Pow(Math.PI - xValueCounter,2));
                            else if (xValueCounter >= Math.PI/2)
                                yValue = Math.Sqrt(Math.Pow(Math.PI, 2) - Math.Pow(xValueCounter - Math.PI, 2));

                            double delta = (_data.Max - _data.Min) / 2;
                            yValue *= delta/Math.PI;
                            double offset = (delta + _data.Min);
                            yValue += offset;
                            xValueCounter += xValueInc;

                            if (xValueCounter >= (2*Math.PI))
                                xValueCounter = 0;
                        }
                        break;

                        case FunctionGenPS.FunctionType.HalfRoundMinus:
                        {
                            if (xValueCounter < Math.PI)
                                yValue = Math.Sqrt(Math.Pow(Math.PI, 2) - Math.Pow(Math.PI - xValueCounter, 2));
                            else if (xValueCounter >= Math.PI / 2)
                                yValue = Math.Sqrt(Math.Pow(Math.PI, 2) - Math.Pow(xValueCounter - Math.PI, 2));

                            double delta = (_data.Max - _data.Min) / 2;
                            yValue *= delta / Math.PI;
                            double offset = (delta + _data.Min);
                            yValue += offset;
                            xValueCounter += xValueInc;
                             yValue = 2 * offset - yValue;

                            if (xValueCounter >= (2 * Math.PI))
                                xValueCounter = 0;
                        }
                        break;
                        case FunctionGenPS.FunctionType.SinePlus:
                        {
                            yValue = Math.Sin(xValueCounter);
                            double delta = (_data.Max - _data.Min) / 2;
                            yValue *= delta;
                            double offset = (delta + _data.Min);
                            yValue += offset;

                            if (yValue < offset)
                                yValue = 2*offset - yValue;

                            xValueCounter += xValueInc;
                        }
                        break;
                        case FunctionGenPS.FunctionType.SineMinus:
                        {
                            yValue = Math.Sin(xValueCounter);
                            double delta = (_data.Max - _data.Min) / 2;
                            yValue *= delta;
                            double offset = (delta + _data.Min);
                            yValue += offset;

                            if (yValue > offset)
                                yValue = 2*offset - yValue;

                            xValueCounter += xValueInc;
                        }
                        break;
                        case FunctionGenPS.FunctionType.Sinc:
                        {
                            double xValue = Math.PI*xValueCounter;

                            yValue = Math.Sin(xValue);
                            double diff = 0.000000000001;
                            
                            if (Math.Abs(0 - xValue) < diff)
                                yValue = 1;
                            else
                                yValue /= xValue;

                            double delta = (_data.Max - _data.Min)/ 2;
                            yValue *= delta;

                            double offset = (delta + _data.Min);
                            yValue += offset;
                      
                            xValueCounter += xValueInc;
                        }
                        break;
                        case FunctionGenPS.FunctionType.SincMinus:
                        {
                            double xValue = Math.PI * xValueCounter;

                            yValue = Math.Sin(xValue);
                            double diff = 0.000000000001;

                            if (Math.Abs(0 - xValue) < diff)
                                yValue = 1;
                            else
                                yValue /= xValue;

                            double delta = (_data.Max - _data.Min) / 2;
                            yValue *= delta;

                            double offset = (delta + _data.Min);
                            yValue += offset;
                            yValue = 2 * offset - yValue;
                            xValueCounter += xValueInc;
                        }
                        break;
                        case FunctionGenPS.FunctionType.Noise:
                        {
                            yValue = _random.NextDouble() * (_data.Max - _data.Min) + _data.Min;
                        }
                        break;
                        case FunctionGenPS.FunctionType.Random:
                        {
                            double newValue = 0; 
                            int stop = 100;

                            do
                            {
                                if (_data.Parameter == 0)
                                    _data.Parameter = 10;

                                newValue = yValue + ((_random.NextDouble() - 0.5) * (_data.Max - _data.Min)) /_data.Parameter;
                                stop--;
                            }
                            while ((newValue > _data.Max || newValue < _data.Min) && stop != 0);

                            if (stop == 0)
                            {
                                if (newValue > _data.Max)
                                    newValue = _data.Max;

                                if (newValue < _data.Min)
                                    newValue = _data.Min;
                            }

                            yValue = newValue;
                        }
                        break;

                        case FunctionGenPS.FunctionType.Constant:
                            if (_data.Parameter >= _data.Min && _data.Parameter <= _data.Max)
                                yValue = _data.Parameter;
                            else
                                yValue = _data.Min;
                        break;
                        case FunctionGenPS.FunctionType.RectanglePlus:                        
                        {
                            try
                            {
                                int counter = pointCounter % (int) noOfPoints;
                                if (counter >= 0)
                                {
                                    if (counter > onPoints)
                                        yValue = _data.Min;
                                    else
                                        yValue = _data.Max;
                                }
                                else
                                {
                                    if (counter < onPoints)
                                        yValue = _data.Min;
                                    else
                                        yValue = _data.Max;
                                }
                            }
                            catch(Exception ex)
                            {
                            Console.Write(ex);
                            }
                        }
                        break;
                        case FunctionGenPS.FunctionType.RectangleMinus:
                        {
                            int counter = pointCounter % (int)noOfPoints;
                            if (counter >= 0)
                            {
                                if (counter > onPoints)
                                    yValue = _data.Max;
                                else
                                    yValue = _data.Min;
                            }
                            else
                            {
                                if (counter < onPoints)
                                    yValue = _data.Max;
                                else
                                    yValue = _data.Min;
                            }                         
                        }
                        break;
                    }

                    yPix = (int)(drawRect.Bottom - ((yValue - _data.Min) * pixSlope));
                    curPoint = new Point((int)xPix, yPix);

                    if (first)
                    {
                        lastPoint = curPoint;
                        first = false;
                    }

                    if (xPix >= drawRect.Left)
                    {
                        if (lastPoint.X != curPoint.X || lastPoint.Y != curPoint.Y)
                        {
                            if (_showPoints)
                            {
                                g.DrawEllipse(_pointPen, (int)lastPoint.X - 3, lastPoint.Y - 3, 6, 6);
                                g.DrawEllipse(_pointPen, (int)curPoint.X - 3, curPoint.Y - 3, 6, 6);
                            }

                            Point interPoint = new Point(lastPoint.X + (curPoint.X - lastPoint.X), lastPoint.Y);
                            g.DrawLine(_linePen, lastPoint, interPoint);
                            g.DrawLine(_linePen, interPoint, curPoint);
                        }
                    }
                    xPix += xPixInc;

                    lastPoint = curPoint;        
                    pointCounter++;
                }
            }


            private void showPointsToolStripMenuItem_Click(object sender, EventArgs e)
            {
                _showPoints = !_showPoints;
                DrawContent();
                Invalidate();
            }
        }
}