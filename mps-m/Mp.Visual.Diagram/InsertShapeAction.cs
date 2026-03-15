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

namespace Mp.Visual.Diagram
{
    internal class InsertShapeAction : IAction
    {
        private Shape _shape;
        
        public InsertShapeAction(Shape shape)
        {
            _shape = shape;
        }

        public void Undo()
        {
            _shape.OnRemove();
            _shape.Site.Shapes.Remove(_shape);
            _shape.Site.Invalidate();
        }

        public void Redo()
        {
            _shape.Site.Shapes.Add(_shape);
            _shape.OnRestore();
            _shape.Site.Invalidate();
        }
    }
}
