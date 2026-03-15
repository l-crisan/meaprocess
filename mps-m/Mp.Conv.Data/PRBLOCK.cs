using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Conv.Data
{
    internal class PRBLOCK
    {
        public PRBLOCK()
        {
            MDFHelper.StringToByte("PR", id);
        }

        private byte[] id = new byte[2];
        private ushort size;
        private byte[] data;
    }
}
