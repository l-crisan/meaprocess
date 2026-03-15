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

namespace Mp.Visual.Diagram
{
    internal class MoveXYAction :IAction
    {
        private List<Shape> _shapes;
        private int _newPos;
        private List<int> _oldPos = new List<int>();
        private bool _x;

        public MoveXYAction(List<Shape> shapes, int newPos, bool x)
        {
            _x = x;
            _newPos = newPos;
            _shapes = shapes;

            foreach (Shape shape in _shapes)
            {
                 if(x)
                    _oldPos.Add(shape.X);
                 else
                    _oldPos.Add(shape.Y);
            }
        }

        public void Undo()
        {
            int i = 0;
            foreach (Shape shape in _shapes)
            {
                if(_x)
                    shape.X = _oldPos[i];
                else
                    shape.Y = _oldPos[i];

                ++i;
            }
        }

        public void Redo()
        {
            foreach (Shape shape in _shapes)
            {
                if(_x)
                    shape.X = _newPos;
                else
                    shape.Y = _newPos;
            }
        }
    }
}
