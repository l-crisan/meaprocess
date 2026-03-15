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
using System.Threading;
using System.Globalization;
using System.IO;


namespace Mp.Visual.GPS
{
    public partial class TrackViewCtrl : UserControl
    {
        private float _zoom = 2;
        private float _rotationAngle = 0.0f;
        private bool _rotateMap = false;
        private bool _showPoint = false;
        private bool _showLine = true;
        private bool _centerMap = true;
        private PointF _shift = new PointF(0,0);
        private Bitmap _backBuffer;
        private bool _beginDrag = false;
        private bool _dragging = false;
        private Point _beginDragPos = new Point();
        private System.Windows.Forms.Timer _updateTimer = new System.Windows.Forms.Timer();

        private double _xfactor;
        private double _xoffset;
        private double _yfactor;
        private double _yoffset;
        private bool _firstSample = true;

        private const double ErdRadiusKm = 6378.137;
        private List<PointF> _points = new List<PointF>();        
        private Color _trackColor = Color.Blue;
        private Color _pointColor = Color.Blue;
        private Rectangle _drawRect = new Rectangle();
        private Matrix _toViewMatrix = new Matrix();
        private Matrix _toViewRotateMatrix = new Matrix();

        private Matrix _fromViewMatrix = new Matrix();
        private Matrix _fromViewRotationMatrix = new Matrix();
        
        private double _lastLo;
        private double _lastLa;
        private Mutex _mutex = new Mutex();
        private double _speed;
        private double _altitude;
        private double _status;
        private double _distance;
        private byte _day = 1;
        private byte _month = 1;
        private ushort _year = 2009;
        private byte _hour = 12;
        private byte _minute = 0;
        private byte _second = 0;
        private bool _addPosIfStatus = true;
        private NumberFormatInfo _noInfo;
        
        private class City
        {
            public City(double la, double lo, string name, Type cityType)
            {
                Longitude = lo;
                Latitude = la;
                Name = name;
                CityType = cityType;
            }

            public enum Type
            {
                Town,
                Village,
                Lake
            }

            public double Longitude;
            public double Latitude;
            public string Name;
            public Type CityType;
        }

        private List<City> _cities = new List<City>();

        public TrackViewCtrl()
        {
            InitializeComponent();
            _noInfo = Thread.CurrentThread.CurrentUICulture.NumberFormat;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            led.ColorOn = Color.YellowGreen;
            led.ColorOff = Color.Green;

            _drawRect = new Rectangle(0, 0, this.Width, this.Height);

            _backBuffer = new Bitmap(_drawRect.Width, _drawRect.Height);

            LoadCities();
             _points.Add(GeoToMercator(8.40376, 49.00808));
            UpdateMatrix();
            RepaintAll();

            _updateTimer.Interval = 100;
            _updateTimer.Tick += new EventHandler(OnUpdateTimer);
            _updateTimer.Start();
        }

        [SRDescription("AddPosOnlyStatus")]
        public bool AddPosOnlyStatus
        {
            get { return _addPosIfStatus; }
            set { _addPosIfStatus = value; }
        }

        private void LoadCities()
        {
            try
            {

                NumberFormatInfo info = new NumberFormatInfo();
                info.NumberDecimalSeparator = ".";

                string path = this.GetType().Assembly.Location;
                path = Path.GetDirectoryName(path);
                using(StreamReader sr = new StreamReader(Path.Combine(path,"cities.txt")))
                {
                    while (!sr.EndOfStream)
                    {
                        string city = sr.ReadLine();
                        string[] array = city.Split(';');
                        
                        City.Type cityType = City.Type.Town;

                        if (array[2] == "town")
                            cityType = City.Type.Town;
                        else if (array[2] == "village")
                            cityType = City.Type.Village;

                        _cities.Add(new City(Convert.ToDouble(array[0], info), Convert.ToDouble(array[1], info), array[2], cityType));
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void OnUpdateTimer(object sender, EventArgs e)
        {
            speed.Text = ((int)_speed).ToString()+" km/h";
            altitude.Text = ((int)_altitude).ToString() + " m";
            distance.Text = String.Format(_noInfo, "{0:0.00} km",_distance);
            led.Active = _status != 0;
            
            try
            {
                DateTime dt = new DateTime(_year, _month, _day, _hour, _minute, _second);
                date.Text = dt.Date.ToShortDateString();
                time.Text = dt.ToLongTimeString();
                localTime.Text = dt.ToLocalTime().ToLongTimeString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (_points.Count > 0)
            {
                double lon = 0;
                double lat = 0;
                MercatorToGeo(_points[_points.Count - 1],out lon, out lat);
                longitude.Text = String.Format(_noInfo, "{0:0.000000} °", lon);
                latitude.Text = String.Format(_noInfo, "{0:0.000000} °", lat);
            }

            Invalidate();
        }

        public void Clear()
        {
            _points.Clear();
            _zoom = 600000f;
            _shift = new PointF(0, 0);
            _rotationAngle = 0.0f;
            speed.Text = "0.0";
            altitude.Text = "0";
            distance.Text = "0.0";
            _distance = 0.0;
            _firstSample = true;

            UpdateMatrix(); 
            RepaintAll();
        }

        public byte Day
        {
            get { return _day; }
            set { _day = value; }
        }

        public byte Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public ushort Year
        {
            get { return _year; }
            set { _year = value; }
        }

        public byte Hour
        {
            get { return _hour; }
            set { _hour = value; }
        }

        public byte Minute
        {
            get { return _minute; }
            set { _minute = value; }
        }

        public byte Second
        {
            get { return _second; }
            set { _second = value; }
        }
        
        public double Speed
        {
            set
            {
                _speed = value * 1.852f;               
            }
            get{ return _speed;}
        }

        public double Altitude
        {
            get { return _altitude; }
            set 
            { 
                _altitude = value;
            }
        }

        public double Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public Color TrackColor
        {
            set { _trackColor = value; }
            get { return _trackColor; } 
        }


        private void RepaintAll()
        {
            _mutex.WaitOne();
            Graphics g = Graphics.FromImage(_backBuffer);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.Clear(BackColor);
            DrawGrid(g);            
            DrawCities(g);

            PointF[] points = _points.ToArray();

            if (points.Length == 0)
            {
                _mutex.ReleaseMutex();
                return;
            }

            PointF last = points[0];

            foreach (PointF p in points)
            {
                DrawPoint(last,p);
                last = p;
            }

            Point center = MercatorToView(last);
            DrawCursor(g, center.X, center.Y);
            DrawScale(g);
            _mutex.ReleaseMutex();
        }

        private void DrawScale(Graphics g)
        {
            g.DrawLine(new Pen(Color.White,5), new Point(_drawRect.Left + 9, _drawRect.Bottom - 25), new Point(_drawRect.Left + 60, _drawRect.Bottom - 25));
            g.DrawLine(new Pen(Color.Black, 2), new Point(_drawRect.Left + 9, _drawRect.Bottom - 25), new Point(_drawRect.Left + 60, _drawRect.Bottom - 25));
            
            g.DrawLine(new Pen(Color.White, 5), new Point(_drawRect.Left + 10, _drawRect.Bottom - 32), new Point(_drawRect.Left + 10, _drawRect.Bottom - 18));
            g.DrawLine(new Pen(Color.Black, 2), new Point(_drawRect.Left + 10, _drawRect.Bottom - 30), new Point(_drawRect.Left + 10, _drawRect.Bottom - 20));
            g.DrawLine(new Pen(Color.White, 5), new Point(_drawRect.Left + 61, _drawRect.Bottom - 32), new Point(_drawRect.Left + 61, _drawRect.Bottom - 18));
            g.DrawLine(new Pen(Color.Black, 2), new Point(_drawRect.Left + 61, _drawRect.Bottom - 30), new Point(_drawRect.Left + 61, _drawRect.Bottom - 20));

            PointF m1 = ViewToMercator(new Point(0, 0));
            PointF m2 = ViewToMercator(new Point(50, 50));
            double lo1, la1, lo2, la2;
            MercatorToGeo(m1, out lo1, out la1);
            MercatorToGeo(m2, out lo2, out la2);
            double distance = DistanceToKm(lo1, la1, lo2, la2);
            string str; 

            if (distance > 1)
                str = ((int)distance).ToString() + "km";
            else
                str = ((int)(distance * 1000)).ToString() + "m";

            SizeF s = g.MeasureString(str, this.Font);
            Point center = new Point(_drawRect.Left + 25, _drawRect.Bottom - 27);
            center.X -= (int)s.Width / 2;
            center.Y -= (int)(s.Height + 5);
            g.DrawString(str, this.Font, new SolidBrush(Color.Black), center);
        }

        private void DrawCities(Graphics g)
        {
            foreach (City city in _cities)
            {
                PointF m = GeoToMercator(city.Longitude, city.Latitude);
                Point view = MercatorToView(m);
                
                if (view.X < _drawRect.Left || view.X > _drawRect.Right ||
                    view.Y < _drawRect.Top || view.Y > _drawRect.Bottom)
                    continue;

                g.FillEllipse(new SolidBrush(Color.Black),new Rectangle(view.X -2, view.Y - 2, 4, 4));
                g.DrawEllipse(new Pen(Color.Black), new Rectangle(view.X - 3, view.Y - 3, 6, 6));
                DrawHighlightText(g, new Point(view.X,view.Y), city.Name);
            }
        }

        private void DrawHighlightText(Graphics g, Point center, string text)
        {
            SizeF s = g.MeasureString(text, this.Font);
            center.X -= (int) s.Width / 2;
            center.Y -= (int)(s.Height + 5);
            Point p = center;

            p.X -= 1;
            p.Y -= 1;
            g.DrawString(text, this.Font, new SolidBrush(Color.White), p);

            p = center;
            p.X += 1;
            p.Y += 1;
            g.DrawString(text, this.Font, new SolidBrush(Color.White), p);

            p = center;
            p.X -= 1;
            p.Y += 1;
            g.DrawString(text, this.Font, new SolidBrush(Color.White), p);

            p = center;
            p.X += 1;
            p.Y -= 1;
            g.DrawString(text, this.Font, new SolidBrush(Color.White), p);

            p = center;
            p.Y -= 1;
            g.DrawString(text, this.Font, new SolidBrush(Color.White), p);

            p = center;
            p.X -= 1;
            g.DrawString(text, this.Font, new SolidBrush(Color.White), p);


            p = center;
            p.Y += 1;
            g.DrawString(text, this.Font, new SolidBrush(Color.White), p);

            p = center;
            p.X += 1;
            g.DrawString(text, this.Font, new SolidBrush(Color.White), p);

            g.DrawString(text, this.Font, new SolidBrush(Color.Black), center);

        }

        private void DrawGrid(Graphics g)
        {
            g.DrawLine(new Pen(Color.Gray), new Point(0, _drawRect.Height / 3), new Point(_drawRect.Width, _drawRect.Height / 3));
            g.DrawLine(new Pen(Color.Gray), new Point(0, 2*_drawRect.Height / 3), new Point(_drawRect.Width,2* _drawRect.Height / 3));
            g.DrawLine(new Pen(Color.Gray), new Point(_drawRect.Width/3, 0), new Point(_drawRect.Width/3, _drawRect.Height));
            g.DrawLine(new Pen(Color.Gray), new Point(2*_drawRect.Width / 3, 0), new Point(2*_drawRect.Width / 3, _drawRect.Height));

            double lon = 0;
            double lat = 0;
            PointF m;
            string str;

            m = ViewToMercator(new Point(_drawRect.Width / 3, _drawRect.Height / 3));            
            MercatorToGeo(m, out lon, out lat);

            str = String.Format(_noInfo, "{0:0.000000}°", lat);        
            g.DrawString(str, this.Font, new SolidBrush(Color.Black), new PointF(10,_drawRect.Height / 3));
            g.DrawString(str, this.Font, new SolidBrush(Color.Black), new PointF(_drawRect.Right - 65, _drawRect.Height / 3));

            str = String.Format(_noInfo, "{0:0.000000}°", lon);
            g.DrawString(str, this.Font, new SolidBrush(Color.Black), new PointF(_drawRect.Width / 3, 0));
            g.DrawString(str, this.Font, new SolidBrush(Color.Black), new PointF(_drawRect.Width / 3, _drawRect.Bottom - 20));

            m = ViewToMercator(new Point(2*_drawRect.Width / 3, 2*_drawRect.Height / 3));
            MercatorToGeo(m, out lon, out lat);

            str = String.Format(_noInfo, "{0:0.000000}°", lat);
            g.DrawString(str, this.Font, new SolidBrush(Color.Black), new PointF(10, 2*_drawRect.Height / 3));
            g.DrawString(str, this.Font, new SolidBrush(Color.Black), new PointF(_drawRect.Right - 65, 2*_drawRect.Height / 3));

            str = String.Format(_noInfo, "{0:0.000000}°", lon);
            g.DrawString(str, this.Font, new SolidBrush(Color.Black), new PointF(2*_drawRect.Width / 3, 0));
            g.DrawString(str, this.Font, new SolidBrush(Color.Black), new PointF(2*_drawRect.Width / 3, _drawRect.Bottom - 20));
          
        }


        private void DrawPoint(PointF last, PointF p)
        {
            Point p1 = MercatorToView(last);
            Point p2 = MercatorToView(p);

            if (p1.X < _drawRect.Left && p2.X < _drawRect.Left ||
                p1.X > _drawRect.Right && p2.X > _drawRect.Right ||
                p1.Y < _drawRect.Top && p2.Y < _drawRect.Top ||
                p1.Y > _drawRect.Bottom && p2.Y > _drawRect.Bottom)
                return;

            
            Graphics g = Graphics.FromImage(_backBuffer);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clip = new Region(_drawRect);

            if(_showPoint)
                g.DrawEllipse(new Pen(_pointColor), new Rectangle(p2.X- 3, p2.Y - 3, 6, 6));

            if (_showLine)
            {
                if (p1.X == p2.X && p1.Y == p2.Y)
                    return;

                Pen pen = new Pen(_trackColor, 3);
                pen.EndCap = LineCap.Round;
                g.DrawLine(pen, p1, p2);
            }
        }

        public void SetCenter(double longitude, double latitude)
        {
            SetCenter(GeoToMercator(longitude, latitude));
        }

        private void SetCenter(PointF m)
        {
            _shift.X = 0;
            _shift.Y = 0;
            float oldAngle = _rotationAngle;
            _rotationAngle = 0;            
            UpdateMatrix();
            Point point = MercatorToView(m);
            
            _rotationAngle = oldAngle;

            _shift.X = _drawRect.Left + _drawRect.Width / 2.0f - point.X;
            _shift.Y = _drawRect.Top + _drawRect.Height/2.0f - point.Y;
            UpdateMatrix();
        }

        public void AddPoint(double longitude, double latitude, double angle)
        {
            if (_addPosIfStatus && _status == 0 )
                return;

            if (_lastLo == longitude && _lastLa == latitude)
                return;

            if (!_firstSample)
                _distance += DistanceToKm(_lastLo, _lastLa, longitude, latitude);
            else
                _firstSample = false; 
            
            _lastLa = latitude;
            _lastLo = longitude;
             
            PointF m = GeoToMercator(longitude, latitude);
            if (_points.Count > 0)
            {
                PointF last = _points[_points.Count -1];
                if (last.X == m.X && last.Y == m.Y)
                    return;
                else
                    last = new PointF();
            }            

            if (_rotationAngle != angle)
            {
                _mutex.WaitOne();
                _rotationAngle = (float)angle;
                UpdateMatrix();
                _mutex.ReleaseMutex();
            }

            if (_centerMap)
            {
                _mutex.WaitOne();
                _points.Add(m);
                _mutex.ReleaseMutex();
                SetCenter(m);
                RepaintAll();
            }
            else
            {
                _mutex.WaitOne();

                if (_points.Count > 0)
                    DrawPoint(_points[_points.Count - 1], m);     
                else
                    DrawPoint(m, m);

                _points.Add(m);
                _mutex.ReleaseMutex();
            }                             
        }

        private void DrawCursor(Graphics g, int x, int y)
        {
            g.DrawEllipse(new Pen(Color.Black), new Rectangle(x-15, y-15, 30, 30));
            Point[] points = new Point[3];
            points[0] = new Point(x-15,y);
            points[1] = new Point(x, y -15);
            points[2] = new Point(x+15,y);
            Matrix m = new Matrix();

            m.RotateAt(_rotationAngle, new PointF(x, y));
            m.TransformPoints(points);
            g.FillPolygon(new SolidBrush(Color.Red), points);
            g.DrawPolygon(new Pen(Color.Black), points);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);


            if (this.Width == 0 || this.Height == 0)
                return;

             _drawRect = new Rectangle(0, 0, this.Width, this.Height);

            _backBuffer = new Bitmap(_drawRect.Width, _drawRect.Height);

            //Range -180..180
            _xfactor = (_drawRect.Width) / (360);
            _xoffset = ((double)(-180 * _drawRect.Right) - (double)(_drawRect.Left * 180)) / (-360);

            //Range -PI..PI
            _yfactor = _drawRect.Height / (2 * Math.PI);
            _yoffset = _drawRect.Top;

            UpdateMatrix();
            RepaintAll();
        }

        private PointF ViewToMercator(Point p)
        {
            PointF[] points = new PointF[1];

            points[0] = p;

            if (_rotateMap)
                _fromViewRotationMatrix.TransformPoints(points);

            _fromViewMatrix.TransformPoints(points);

            PointF m = new PointF();

            m.X = (float)(((double)points[0].X - _xoffset) / _xfactor);

            double my = (((double)points[0].Y - _yoffset) / _yfactor);

            m.Y = (float)(Math.PI - my);
            
            return m; 
        }

        private Point MercatorToView(PointF m)
        {
            float x = (float)(m.X * _xfactor + _xoffset);

            float my = (float)(Math.PI - m.Y);
            float y = (float)(my * _yfactor + _yoffset);

            
            PointF[] points = new PointF[1];
            points[0] = new PointF(x,y);

            _toViewMatrix.TransformPoints(points);
            
            if(_rotateMap)
                _toViewRotateMatrix.TransformPoints(points);            

            return new Point((int)points[0].X,(int)points[0].Y);
        }

        private PointF GeoToMercator(double longitude, double latitude)
        {
            PointF m = new PointF();
            m.X = (float)longitude;
            m.Y = (float)(Math.Log(Math.Tan(0.25 * Math.PI + DegreesToRadians(latitude) * 0.5)));

            return m;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _mutex.WaitOne();
            e.Graphics.DrawImage(_backBuffer, new Point(0, 0));
            _mutex.ReleaseMutex();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            
            if (e.Delta < 0)
                ZoomOut();                
            else
                ZoomIn();

        }

        private void UpdateMatrix()
        {
            _toViewMatrix = new Matrix();
            _toViewMatrix.Scale(_zoom, _zoom, MatrixOrder.Append);
            //_toViewMatrix.RotateAt(_rotationAngle, new PointF(_drawRect.Width / 2, _drawRect.Height / 2));
            _toViewMatrix.Translate(_shift.X, _shift.Y, MatrixOrder.Append);
            
            _toViewRotateMatrix = new Matrix();
            _toViewRotateMatrix.RotateAt(_rotationAngle, new PointF(_drawRect.Width / 2, _drawRect.Height / 2));
            _fromViewMatrix = new Matrix();
            //_fromViewMatrix.Rotate(-_rotationAngle, MatrixOrder.Append);
            _fromViewMatrix.Translate(-_shift.X, -_shift.Y, MatrixOrder.Append);
            _fromViewMatrix.Scale(1.0f/_zoom, 1.0f/_zoom, MatrixOrder.Append);
                        
            _fromViewRotationMatrix = new Matrix();
            _fromViewRotationMatrix.RotateAt(_rotationAngle,new PointF(_drawRect.Width/2,_drawRect.Height/2));            
        }


        private double DistanceToKm(double lon1, double lat1, double lon2, double lat2)
        {
            double theta = lon1 - lon2;

            double dist = Math.Sin(DegreesToRadians(lat1)) * Math.Sin(DegreesToRadians(lat2)) + Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) * Math.Cos(DegreesToRadians(theta));
            dist = Math.Acos(dist);
            dist = RadiansToDegrees(dist);
            dist *= (60 * 1.1515);
            return dist * 1.609344;
        }

        private double DegreesToRadians(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double RadiansToDegrees(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private void MercatorToGeo(PointF m, out double longitude, out double latitude)
        {
            longitude = m.X;
            latitude = RadiansToDegrees(2*Math.Atan(Math.Exp(m.Y)) - Math.PI * 0.5);
        }

        private void onZoomM_Click(object sender, EventArgs e)
        {
            ZoomOut();
        }

        private void onZoomP_Click(object sender, EventArgs e)
        {
            ZoomIn();
        }

        private void ZoomOut()
        {
            if (_zoom > 1 && _zoom <= 10)
                _zoom -= 1.0f;

            if (_zoom > 10 && _zoom <= 1000)
                _zoom -= 10.0f;

            if (_zoom > 1000 && _zoom <= 10000)
                _zoom -= 1000.0f;

            if (_zoom > 10000 && _zoom <= 50000)
                _zoom -= 5000.0f;

            if (_zoom >= 50000)
                _zoom -= 10000.0f;

            if (_zoom <= 1)
                _zoom = 2f;

            if (_centerMap && _points.Count > 0)
                SetCenter(_points[_points.Count - 1]);
            else
                UpdateMatrix();
            RepaintAll();
        }


        private void ZoomIn()
        {
            
            if (_zoom <= 10 && _zoom >= 1)
                _zoom += 1.0f;

            if (_zoom > 10 && _zoom <= 1000)
                _zoom += 10.0f;

            if (_zoom > 1000 && _zoom <= 10000)
                _zoom += 1000.0f;

            if (_zoom > 10000 && _zoom <= 50000f)
                _zoom += 5000.0f;

            if (_zoom > 50000 && _zoom <= 600000f)
                _zoom += 10000.0f;

            if (_zoom > 600000f)
                _zoom = 600000f;

            if (_centerMap && _points.Count > 0)
                SetCenter(_points[_points.Count - 1]);
            else
                UpdateMatrix();
            RepaintAll();
        }
        private void onShowPoints_CheckedChanged(object sender, EventArgs e)
        {
            _showPoint = !_showPoint;
            RepaintAll();
        }

        private void onShowLine_CheckedChanged(object sender, EventArgs e)
        {
            _showLine = !_showLine;
            RepaintAll();
        }

        private void onCenterMap_CheckedChanged(object sender, EventArgs e)
        {
            _centerMap = !_centerMap;
            RepaintAll();
        }

        private void onTopPan_Click(object sender, EventArgs e)
        {
            _shift.Y += _drawRect.Height / 10; ; 
            UpdateMatrix();
            RepaintAll();
        }

        private void onDownPen_Click(object sender, EventArgs e)
        {
            _shift.Y -= _drawRect.Height / 10; ;
            UpdateMatrix();
            RepaintAll();
        }

        private void onLeftPan_Click(object sender, EventArgs e)
        {
            PointF[] points = new PointF[1];
            float size = _drawRect.Width / 10.0f;
            _shift.X -= size ;
            _rotationAngle = 0;
            UpdateMatrix();
            RepaintAll();
        }

        private void onRightPen_Click(object sender, EventArgs e)
        {
            _shift.X += _drawRect.Width / 10; ;
            float oldAngle = _rotationAngle;
            _rotationAngle = 0;
            UpdateMatrix();
            _rotationAngle = oldAngle;
            UpdateMatrix();
            RepaintAll();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            //_beginDrag = true;
            _beginDragPos = new Point(e.X, e.Y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (_beginDrag)
            {
                _dragging = true;
                _beginDrag = false;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _beginDrag = false;

            if (_dragging)
            {                
                //Adjust the map
                Point deltaP = new Point();

                deltaP.X = e.X - _beginDragPos.X;
                deltaP.Y = e.Y - _beginDragPos.Y;
                _shift.X -= deltaP.X;
                _shift.Y -= deltaP.Y;

                UpdateMatrix();
                RepaintAll();
            }

            _dragging = false;
        }

        private void onRotateL_Click(object sender, EventArgs e)
        {
            _rotationAngle -= 5;
            UpdateMatrix();
            RepaintAll();
        }

        private void onRotateR_Click(object sender, EventArgs e)
        {
            _rotationAngle += 5;
            UpdateMatrix();
            RepaintAll();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _rotateMap = true;
            RepaintAll();
        }

        private void TrackViewCtrl_DoubleClick(object sender, EventArgs e)
        {
            //PropertyDlg dlg = new PropertyDlg(this);
            //dlg.ShowDialog();
        }

        private void TrackViewCtrl_BackColorChanged(object sender, EventArgs e)
        {
            RepaintAll();
        }

        private void showPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showPoint = !_showPoint;
            showPointsToolStripMenuItem.Checked = _showPoint;
            RepaintAll();
        }
    }
}
