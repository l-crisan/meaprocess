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
    internal class SplittPointAction : IAction
    {
        private List<Point> _oldPoints = new List<Point>();
        private List<Point> _newPoints = new List<Point>();
        private Connection _con;

        public SplittPointAction(Connection con)
        {
            _con = con;

            foreach (Point p in con.Points)
                _oldPoints.Add(p);
        }

        public void Undo()
        {
            _newPoints.Clear();
            
            foreach (Point p in _con.Points)
                _newPoints.Add(p);

            _con.Points.Clear();

            foreach (Point p in _oldPoints)
                _con.Points.Add(p);

            _con.Invalidate();
        }

        public void Redo()
        {
            _con.Points.Clear();

            foreach (Point p in _newPoints)
                _con.Points.Add(p);

            _con.Invalidate();
        }
    }
}
