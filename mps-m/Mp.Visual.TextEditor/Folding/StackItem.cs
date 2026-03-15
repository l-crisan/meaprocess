using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Visual.TextEditor.Folding
{
    internal class StackItem
    {
        private string _endText;
        private int _startLine;

        public StackItem(string text, int startLine)
        {
            _endText = text;
            _startLine = startLine;
        }

        public string EndText
        {
            get { return _endText; }
            set { _endText = value; }
        }

        public int StartLine
        {
            get { return _startLine; }
            set { _startLine = value; }
        }
    }    
}
