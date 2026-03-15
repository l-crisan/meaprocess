using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mp.Conv.Data
{
    internal class IDBLOCK
    {
        public IDBLOCK()
        {
            MDFHelper.StringToByte("MDF     ", fileID);
            MDFHelper.StringToByte("3.20", formatID);
            MDFHelper.StringToByte("MeaPro", programID);
        }

        private byte[] fileID = new byte[8];
        private byte[] formatID = new byte[8];
        private byte[] programID = new byte[8];
        public  ushort byteOrder = 0;
        private ushort floatingType = 0;
        private ushort version = 320;
        private ushort reserved1 = 0;
        private byte[] reserved2 = new byte[2];
        private byte[] reserved3 = new byte[30];

        public HDBLOCK hdBlock = new HDBLOCK();

        public void Write(BinaryWriter br)
        {
            br.Write(fileID);
            br.Write(formatID);
            br.Write(programID);
            br.Write(byteOrder);
            br.Write(floatingType);
            br.Write(version);
            br.Write(reserved1);
            br.Write(reserved2);
            br.Write(reserved3);
            hdBlock.Write(br);
        }
    }
}
