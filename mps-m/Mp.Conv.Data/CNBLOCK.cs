using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mp.Conv.Data
{
    internal class CNBLOCK
    {
        public CNBLOCK()
        {
            MDFHelper.StringToByte("CN",id);
        }

        private byte[] id = new byte[2];
        private ushort size = 228;
        private uint nextChannelBlock;
        private uint convertionFormulaBlock;
        private uint sourceDependingExtensionBlock;
        private uint dependencyBlock;
        private uint commentBlock;
        public ushort channelType;
        public byte[] signalName = new byte[32];
        public byte[] signalDescription = new byte[128];
        public ushort startOffsetInBit;
        public ushort noOfBits;
        public ushort signalDataType;
        public ushort valueRangeVaildFlag;
        public double signalMin;
        public double signalMax;
        public double rateInSec;
        private uint longSignalNameBlock;
        private uint displaySignalNameBlock;
        private ushort addByteOffset;
        public CNBLOCK next;
        public CCBLOCK convertion;

        public void Write(BinaryWriter bw)
        {
            uint curPos = 0;

            bw.Write(id);
            bw.Write(size);
            
            nextChannelBlock = (uint) bw.BaseStream.Position;
            bw.Write((uint)0);
            
            convertionFormulaBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            sourceDependingExtensionBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            dependencyBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            commentBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            bw.Write(channelType);
            bw.Write(signalName);
            bw.Write(signalDescription);
            bw.Write(startOffsetInBit);
            bw.Write(noOfBits);
            bw.Write(signalDataType);
            bw.Write(valueRangeVaildFlag);
            bw.Write(signalMin);
            bw.Write(signalMax);
            bw.Write(rateInSec);

            longSignalNameBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            displaySignalNameBlock = (uint)bw.BaseStream.Position;
            bw.Write((uint)0);

            bw.Write(addByteOffset);

            if( next != null)
            {
                curPos = (uint) bw.BaseStream.Position;
                bw.Seek((int)nextChannelBlock, SeekOrigin.Begin);
                bw.Write(curPos);

                bw.Seek((int)curPos, SeekOrigin.Begin);
                next.Write(bw);
            }

            if( convertion != null)
            {
                curPos = (uint)bw.BaseStream.Position;
                bw.Seek((int)convertionFormulaBlock, SeekOrigin.Begin);
                bw.Write(curPos);

                bw.Seek((int)curPos, SeekOrigin.Begin);
                convertion.Write(bw);
            }
        }
    }
}
