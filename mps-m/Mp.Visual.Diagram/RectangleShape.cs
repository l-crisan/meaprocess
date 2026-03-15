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

namespace Mp.Visual.Diagram
{
    /// <summary>
    /// A simple rectangular shape.
    /// </summary>
    public class RectangleShape : Shape
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public RectangleShape()
        { 
                
        }

        public RectangleShape(DiagramCtrl s)
        : base(s)
        { }

        /// <summary>
        /// Tests whether the mouse hits this shape
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public override bool Hit(System.Drawing.Point p)
        {
            _drawRectangle = _rectangle;
            _drawRectangle.X -= XOffset;
            _drawRectangle.Y -= YOffset;

            return _drawRectangle.Contains(new Rectangle(p, new Size(1, 1)));
        }

            
        public virtual void RemoveConnector(Connector con)
        { _connectors.Remove(con); }

        public virtual void AddConnnector(Point p, string name)
        {
            Connector con = new Connector(p);
            con.Site = this.Site;
            con.Shape = this;
            _connectors.Add(con);
        }

        public virtual void AddConnnector(Connector con)
        {
            con.Site = this.Site;
            con.Shape = this;
            _connectors.Add(con);
        }
        /// <summary>
        /// Paints the shape on the canvas
        /// </summary>
        /// <param name="g"></param>
        public override void Paint(System.Drawing.Graphics g)
        {
            _drawRectangle = _rectangle;
            _drawRectangle.X -= XOffset;
            _drawRectangle.Y -= YOffset;

            g.FillRectangle(_shapeBrush, _drawRectangle);

            if (Hovered)
                g.DrawRectangle(new Pen(Color.Black, 1F), _drawRectangle);
            else
                g.DrawRectangle(new Pen(Color.LightGray, 1F), _drawRectangle);

            if (IsSelected && !IsMaster)
                g.DrawRectangle(new Pen(Color.DarkBlue, 1F), _drawRectangle);

            if (IsMaster)
                g.DrawRectangle(new Pen(Color.Blue, 1F), _drawRectangle);

            //g.DrawRectangle(new Pen(Color.LightGray, 1F), _drawRectangle);


            for (int k = 0; k < _connectors.Count; k++)
            {
                _connectors[k].XOffset = XOffset;
                _connectors[k].YOffset = YOffset;

                _connectors[k].Paint(g);
            }
            //well, a lot should be said here like
            //the fact that one should measure the text before drawing it,
            //resize the width and height if the text if bigger than the rectangle,
            //alignment can be set and changes the drawing as well...
            //here we keep it really simple:
            if (_text != string.Empty)
            {

                RectangleF rct = new RectangleF(_drawRectangle.X + 2, _drawRectangle.Y + 6, Width - 2, 12);
                   g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawString(_text, _font, Brushes.White, rct);
            }
        }

        /// <summary>
        /// Invalidates the shape
        /// </summary>
        public override void Invalidate()
        {
            Rectangle r = _rectangle;
            r.Offset(-5, -5);
            r.Inflate(20, 20);
            Site.Invalidate(r);
        }

        public override void Resize(int width, int height)
        {
            /*
                        base.Resize(width,height);
                        cBottom.Point = new Point((int) (rectangle.Left+rectangle.Width/2),rectangle.Bottom);	
                        cLeft.Point = new Point(rectangle.Left,(int) (rectangle.Top +rectangle.Height/2));
                        cRight.Point = new Point(rectangle.Right,(int) (rectangle.Top +rectangle.Height/2));
                        cTop.Point = new Point((int) (rectangle.Left+rectangle.Width/2),rectangle.Top);
                */
            Invalidate();
        }
        public Object Tag;

        private Font _font = new Font("Calibri ", 7F, FontStyle.Regular);
        protected Rectangle _drawRectangle;
    }
}
