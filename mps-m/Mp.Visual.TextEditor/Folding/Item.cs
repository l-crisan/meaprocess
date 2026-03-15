using System;

namespace Mp.Visual.TextEditor.Folding
{
    [Serializable]
    public class Item
    {
        private string _from;
        private string _to;

        public Item()
        {
        }

        public Item(string from, string to)
        {
            _from = from;
            _to = to;
        }

        public string From
        {
            get
            {
                return _from;
            }

            set
            {
                _from = value;
            }
        }

        public string To
        {
            get
            {
                return _to;
            }

            set
            {
                _to = value;
            }
        }
    }
}
