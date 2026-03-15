using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mp.Conv.Data
{
    internal class CCBLOCK
    {
        public CCBLOCK()
        {
            MDFHelper.StringToByte("CC", id);
        }

        private byte[] id = new byte[2];
        private ushort size = 62;
        public ushort valueRangeFlag;
        public double min;
        public double max;
        public byte[] unit = new byte[20];
        private ushort convType = 0;
        private ushort sizeConvData = 2;
        public double factor;
        public double offset;

        public void Write(BinaryWriter bw)
        {
            bw.Write(id);
            bw.Write(size);
            bw.Write(valueRangeFlag);
            bw.Write(min);
            bw.Write(max);
            bw.Write(unit);
            bw.Write(convType);
            bw.Write(sizeConvData);
            bw.Write(offset);
            bw.Write(factor);
        }
    }
}
