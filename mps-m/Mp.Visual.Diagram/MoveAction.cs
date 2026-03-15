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
using System.Collections.Generic;
using System.Drawing;

namespace Mp.Visual.Diagram
{
    public class MoveAction : IAction
    {
        private class ConnectionDescrp
        {
            public Connection Connection;
            public List<Point> Points;
        }

        private List<Point> _shapeOldPos= new List<Point>();        
        private Point _startPos;
        private Point _endPos;
        private List<Shape> _shapes = new List<Shape>();
        private List<ConnectionDescrp> _connections = new List<ConnectionDescrp>();

        public Point StartPos
        {
            set { _startPos = value; }
        }

        public Point EndPos
        {
            set { _endPos = value; }
        }

        public MoveAction(List<Entity> entities)
        {

            foreach (Entity entity in entities)
            {
                if (entity is Shape)
                {
                    Shape shape = (entity as Shape);
                    _shapes.Add(shape);
                    _shapeOldPos.Add(new Point(shape.X, shape.Y));
                }

                if (entity is Connection)
                {
                    Connection con = (Connection)entity;
                    ConnectionDescrp conDescrp = new ConnectionDescrp();
                    conDescrp.Connection = con;

                    _connections.Add(conDescrp);
                    conDescrp.Points = new List<Point>(con.Points.Count);
                    CopyPointList(con.Points, conDescrp.Points);
                }
            }
        }

        private void CopyPointList(List<Point> from, List<Point> to)
        {
            to.Clear();
            foreach (Point p in from)
                to.Add(p);
        }

        public void Undo()
        {
            int i = 0;

            foreach (Shape shape in _shapes)
            {
                shape.X = _shapeOldPos[i].X;
                shape.Y = _shapeOldPos[i].Y;
                shape.Invalidate();
                ++i;
            }            
            
            foreach (ConnectionDescrp conDescrp in _connections)
            {
                CopyPointList(conDescrp.Points, conDescrp.Connection.Points);
                conDescrp.Connection.Invalidate();
            }
        }

        public void Redo()
        {
            Point moveDelta = new Point(_endPos.X - _startPos.X, _endPos.Y - _startPos.Y);
            
            foreach (Shape shape in _shapes)
                shape.Move(moveDelta);

            foreach (ConnectionDescrp conDescrp in _connections)
                conDescrp.Connection.Move(moveDelta);
        }
    }
}
