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
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Mp.Visual.Diagram
{
    public class Connection : Entity
    {
        protected Connector from;
        protected Connector to;
        private ToolTip _toolTipCtrl = new ToolTip();
        public List<Point> Points = new List<Point>();
        private Pen _hoveredPen = new Pen(Color.Green, 3F);
        private Pen _normalPen = new Pen(Color.Gray, 3F);
        private Pen _selectedPen = new Pen(Color.DarkBlue, 5F);
        private Pen _outPen = new Pen(Color.Black, 5F);

        public Connector From
        {
            get { return from; }
            set
            {
                Points.Clear();
                from = value;
                from.IsInput = true;
                from.Connection = this;
                Points.Add(from.Position);
            }
        }

        public Connector To
        {
            get { return to; }
            set
            {
                to = value;
                to.IsInput = false;
                to.Connection = this;
                Points.Add(to.Position);
            }
        }
        /// <summary>
        /// Default ctor
        /// </summary>
        public Connection()
        {
            Init();
        }

        private void Init()
        {
            _hoveredPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            _normalPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            _selectedPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            _outPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            _toolTipCtrl = new ToolTip();
            _toolTipCtrl.InitialDelay = 1000;
        }

        /// <summary>
        /// Constructs a connection between the two given points
        /// </summary>
        /// <param name="from">the starting point of the connection</param>
        /// <param name="to">the end-point of the connection</param>
        public Connection(Point from, Point to)
        {
            Init(); 
            Points.Clear();

            this.from = new Connector(from);
            this.from.IsInput = true;
            this.from.Connection = this;
            this.to = new Connector(to);
            this.to.IsInput = false;
            this.To.Connection = this;
            Points.Add(from);
            Points.Add(to);
        }

        protected override void OnShowToolTip(string [] text)
        {
            if (text == null)
                return;

            _toolTipCtrl.ToolTipTitle = text[0];

            Point pos = this.Site.PointToClient(Cursor.Position);
            pos.X += 10;
            pos.Y += 10;
            _toolTipCtrl.Show(text[1], Site, pos, 3000);                
        }

        protected override void OnHideToolTip()
        {
            _toolTipCtrl.Hide(Site);
        }

        public int AddSplitPoint(Point p)
        {
            int n = 1;
            Point fromP;
            Point toP;
            Point newP = p;

            newP.X += XOffset;
            newP.Y += YOffset;

            for (n = 1; n < Points.Count; n++)
            {
                fromP = (Point)Points[n - 1];
                toP = (Point)Points[n];

                if (CheckLine(fromP, toP, newP))
                {
                    Points.Insert(n, newP);
                    Invalidate();
                    return n;
                }
            }
            Invalidate();
            return -1;
        }

        public int MoveSplitPoint(Point point, int pointIndex)
        {
            Point newPoint = point;
            newPoint.X += XOffset;
            newPoint.Y += YOffset;
                
            Points[pointIndex] = newPoint;
                
            Point p1;
            Point p2;
            for (int n = 1; n < Points.Count; n++)
            {
                p1 = (Point)Points[n - 1];
                p2 = (Point)Points[n];

                if (((p1.X > p2.X - 3) && (p1.X < p2.X + 3)) &&
                        ((p1.Y > p2.Y - 3) && (p1.Y < p2.Y + 3)))
                {
                    Points.RemoveAt(n);

                    if (Points.Count > 2)
                        return n - 1;
                    else
                        return -1;
                }
            }
                 
            Invalidate();
            return pointIndex;
        }

        /// <summary>
        /// Paints the connection on the canvas
        /// </summary>
        /// <param name="g"></param>
        public override void Paint(System.Drawing.Graphics g)
        {
            int n = 1;

            if (From != null)
            {
                From.XOffset = XOffset;
                From.YOffset = YOffset;
            }

            if (To != null)
            {
                To.XOffset = XOffset;
                To.YOffset = YOffset;
            }

            if (Points.Count > 0)
            {
                Points[0] = from.Position;
                Points[Points.Count - 1] = to.Position;
            }
                
            Point[] pointsToDraw = new Point[Points.Count];
            for (n = 0; n < Points.Count; n++)
            {
                pointsToDraw[n] = Points[n];
                pointsToDraw[n].X -= XOffset;
                pointsToDraw[n].Y -= YOffset;

            }

            if (IsSelected)
                g.DrawLines(_selectedPen, pointsToDraw);
            /*
            else
                g.DrawLines(_outPen, pointsToDraw);   */

            if (Hovered)
                g.DrawLines(_hoveredPen,pointsToDraw);            
            else
                g.DrawLines(_normalPen, pointsToDraw);

        }
        /// <summary>
        /// Invalidates the connection
        /// </summary>
        public override void Invalidate()
        {
            Rectangle f = new Rectangle(from.Position, new Size(10, 10));
            Rectangle t = new Rectangle(to.Position, new Size(10, 10));
            //			site.Invalidate(Rectangle.Union(f,t));
            Site.Invalidate();

        }

        /// <summary>
        /// Tests if the mouse hits this connection
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Hit(Point p)
        {
            Point fromP;
            Point toP;

            for (int n = 1; n < Points.Count; n++)
            {
                fromP = (Point)Points[n - 1];
                fromP.X -= XOffset;
                fromP.Y -= YOffset;

                toP = (Point)Points[n];
                toP.X -= XOffset;
                toP.Y -= YOffset;

                if (CheckLine(fromP, toP, p))
                    return true;
            }

            return false;
        }

        public int HitSplitPoint(Point p)
        {
            Point point;
            for (int n = 0; n < Points.Count; n++)
            {
                point = (Point)Points[n];

                point.X -= XOffset;
                point.Y -= YOffset;

                if (((p.X > point.X - 5) && (p.X < point.X + 5)) &&
                        ((p.Y > point.Y - 5) && (p.Y < point.Y + 5)))
                {
                    return n;
                }
            }

            return -1;
        }


        protected bool CheckLine(Point p1, Point p2, Point p)
        {
            Point s;
            RectangleF r1, r2;
            float o, u;

            // p1 must be the leftmost point.
            if (p1.X > p2.X)
            {
                s = p2;
                p2 = p1;
                p1 = s;
            }

            r1 = new RectangleF(p1.X, p1.Y, 0, 0);
            r2 = new RectangleF(p2.X, p2.Y, 0, 0);
            r1.Inflate(3, 3);
            r2.Inflate(3, 3);
            //this is like a topological neighborhood
            //the connection is shifted left and right
            //and the point under consideration has to be in between.						
            if (RectangleF.Union(r1, r2).Contains(p))
            {
                if (r2.Bottom == r1.Bottom)
                    return true;

                if (r2.Top == r1.Top)
                    return true;

                if (p1.Y < p2.Y) //SWNE
                {
                    o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
                    u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
                    return ((p.X > o) && (p.X < u));
                }
                else //NWSE
                {
                    o = r1.Left + (((r2.Left - r1.Left) * (p.Y - r1.Top)) / (r2.Top - r1.Top));
                    u = r1.Right + (((r2.Right - r1.Right) * (p.Y - r1.Bottom)) / (r2.Bottom - r1.Bottom));
                    return ((p.X > o) && (p.X < u));
                }
            }
            return false;
        }
        /// <summary>
        /// Moves the connection with the given shift
        /// </summary>
        /// <param name="p"></param>
        public override void Move(Point p)
        {
            Point pt;
            for (int i = 0; i < Points.Count; i++ )
            {
                pt = (Point) Points[i];
                pt.X += p.X;
                pt.Y += p.Y;
                Points[i] = pt;
            }
        }

        public override bool CanMove(Point pos)
        {
            return true;
        }
    }
}