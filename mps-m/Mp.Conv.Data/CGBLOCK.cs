using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mp.Conv.Data
{
    internal class CGBLOCK
    {
        public CGBLOCK()
        {
            MDFHelper.StringToByte("CG", id);
        }

        private byte[] id = new byte[2];
        private ushort size = 26;
        private uint nextChannelGroupBlock;
        private uint firstChannelBlock;
        public CNBLOCK channelBlock;
        private uint groupCommentTextBlock;
        private ushort recordID;
        public ushort noOfChannels;
        public ushort sizeOfDataRecord;
        public uint noOfRecords;

        public void Write(BinaryWriter bw)
        {
            uint curPos = 0;

            bw.Write(id);
            bw.Write(size);
            
            nextChannelGroupBlock = (uint) bw.BaseStream.Position;
            bw.Write((uint)0);
            
            firstChannelBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            groupCommentTextBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            bw.Write(recordID);
            bw.Write(noOfChannels);
            bw.Write(sizeOfDataRecord);
            bw.Write(noOfRecords);

            if (channelBlock != null)
            {
                curPos = (uint)bw.BaseStream.Position;
                bw.Seek((int) firstChannelBlock, SeekOrigin.Begin);
                bw.Write(curPos);
                bw.Seek((int)curPos, SeekOrigin.Begin);

                channelBlock.Write(bw);
            }
        }
    }
}
