using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mp.Conv.Data
{
    internal class DGBLOCK
    {
        public DGBLOCK()
        {
            MDFHelper.StringToByte("DG",id);
        }

        private byte[] id = new byte[2];
        private ushort size = 28;
        private uint nextDataGroupBlock;
        public DGBLOCK nextBlock;
        private uint firstChannelGroupBlock;
        private uint triggerBlock;
        private uint dataBlock;
        public ushort noOfChannelGroups;
        private ushort noOfRecordID = 0;
        private uint reserved;
        public CGBLOCK channelGroup = new CGBLOCK();

        public void WriteDataGroupOffset(BinaryWriter bw)
        {
            uint curPos = (uint)bw.BaseStream.Position;
            bw.Seek((int)dataBlock, SeekOrigin.Begin);
            bw.Write(curPos);
            bw.Seek((int)curPos, SeekOrigin.Begin);
        }

        public void Write(BinaryWriter bw)
        {
            uint curPos;
            bw.Write(id);
            bw.Write(size);

            nextDataGroupBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            firstChannelGroupBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            triggerBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            dataBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);
            
            bw.Write(noOfChannelGroups);
            bw.Write(noOfRecordID);
            bw.Write(reserved);

            if (nextBlock != null)
            {
                curPos = (uint)bw.BaseStream.Position;
                bw.Seek((int)nextDataGroupBlock, SeekOrigin.Begin);
                bw.Write(curPos);
                bw.Seek((int)curPos, SeekOrigin.Begin);
                nextBlock.Write(bw);
            }

            curPos = (uint)bw.BaseStream.Position;
            bw.Seek((int)firstChannelGroupBlock, SeekOrigin.Begin);
            bw.Write(curPos);
            bw.Seek((int)curPos, SeekOrigin.Begin);

            channelGroup.Write(bw);
        }
    }
}
