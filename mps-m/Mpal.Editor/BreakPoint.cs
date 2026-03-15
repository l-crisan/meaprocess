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
using System.Drawing;
using System.Text;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;
using ICSharpCode.TextEditor.Gui.CompletionWindow;

namespace Mpal.Editor
{
    public class BreakPoint
    {
        private TextEditorControl _textEditor;
        private int _line;
        private bool _enabled = true;
        private string _function;

        public BreakPoint(TextEditorControl control, int line, string function)
        {
            _line = line;
            _function = function;
            _textEditor = control;
        }

        public void Draw(Graphics g)
        {

            int xpos = _textEditor.ActiveTextAreaControl.TextArea.TextView.GetDrawingXPos(_line-1, 0);

            Point pos = new Point(_textEditor.ActiveTextAreaControl.TextArea.TextView.DrawingPosition.X + xpos,
                             _textEditor.ActiveTextAreaControl.TextArea.TextView.DrawingPosition.Y
                             + (_textEditor.ActiveTextAreaControl.TextArea.Document.GetVisibleLine(_line-1)) *
                             _textEditor.ActiveTextAreaControl.TextArea.TextView.FontHeight
                             - _textEditor.ActiveTextAreaControl.TextArea.TextView.TextArea.VirtualTop.Y);

            _textEditor.ActiveTextAreaControl.TextArea.IconBarMargin.DrawBreakpoint(g, pos.Y, _enabled, true);
        }

        public int Line
        {
            set { _line = value; }
            get { return _line; }
        }

        public string Unit
        {
            set { _function = value; }
            get { return _function; }
        }

        public bool Enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }

    }
}
