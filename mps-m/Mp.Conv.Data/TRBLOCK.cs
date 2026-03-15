using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Conv.Data
{
    internal class TRBLOCK
    {
        public TRBLOCK()
        {
            MDFHelper.StringToByte("TR", id);
        }

        private byte[] id = new byte[2];
        private ushort size = 0;
        private uint triggerComment;
        ushort noOfTriggers;
    }
}
