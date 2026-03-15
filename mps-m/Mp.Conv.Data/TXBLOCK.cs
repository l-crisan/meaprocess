using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Conv.Data
{
    internal class TXBLOCK
    {
        public TXBLOCK()
        {
            MDFHelper.StringToByte("TX", id);
        }

        public string Text
        {
            get { return text; }

            set
            {
                text = value;
                data = new byte[text.Length];
                size = (ushort)text.Length;
                MDFHelper.StringToByte(text, data);
            }
        }

        private byte[] id = new byte[2];
        private ushort size;
        private byte[] data;
        private string text;
    }
}
