using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Conv.Data
{
    internal class CDBLOCK
    {
        public CDBLOCK()
        {
            MDFHelper.StringToByte("CD", id);
        }

        private byte[] id = new byte[2];
        private ushort size;
        private ushort dependencyType;
        private ushort noOfSignalsDependencies = 0;

    }
}
