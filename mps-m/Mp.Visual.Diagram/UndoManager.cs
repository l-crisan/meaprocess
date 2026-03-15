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
    /// <summary>
    /// Undo stack.
    /// </summary>
    public class UndoManager
    {
        private List<IAction> _undoStack = new List<IAction>();
        private int _pos = -1;
        private int _stackSize;

        /// <summary>
        /// Create a new undo manager
        /// </summary>
        /// <param name="stackSize">The actions stack size.</param>
        public UndoManager(int stackSize)
        {
            _stackSize = stackSize;
        }

        /// <summary>
        /// Clear the stack.
        /// </summary>
        public void Clear()
        {
            _pos = -1;
            _undoStack.Clear();
        }

        /// <summary>
        /// Insert a action to the undo stack.
        /// </summary>
        /// <param name="action">The action.</param>
        public void Add(IAction action)
        {
            if ((_pos != _undoStack.Count - 1) && (_pos != -1))
                _undoStack.RemoveRange(_pos,_undoStack.Count - _pos);

            if (_undoStack.Count == _stackSize)
                _undoStack.RemoveAt(0);

            _undoStack.Add(action);
            _pos = _undoStack.Count - 1;
        }

        /// <summary>
        /// Undo the last action
        /// </summary>
        public void Undo()
        {            
            if (!CanUndo)
                return;

            _undoStack[_pos].Undo();
            --_pos;
        }

        /// <summary>
        /// Redo the last action
        /// </summary>
        public void Redo()
        {
            if (!CanRedo)
                return;

            _pos++;
            _undoStack[_pos].Redo();            
        }

        /// <summary>
        /// Return true if a undo action is availabele.
        /// </summary>
        public bool CanUndo
        {
            get 
            { 
                return (_pos != -1); 
            }
        }

        /// <summary>
        /// Return true if a redo action is available.
        /// </summary>
        public bool CanRedo
        {
            get { return (_pos + 1 < _undoStack.Count) && _undoStack.Count != 0; }
        }
    }
}
