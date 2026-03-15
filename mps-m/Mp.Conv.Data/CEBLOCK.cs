using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Conv.Data
{
    internal class CEBLOCK
    {
        public CEBLOCK()
        {
            MDFHelper.StringToByte("CE", id);
        }

        private byte[] id = new byte[2];
        private ushort size;
        ushort extensionType = 2;
    }
}
