using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mp.Conv.Data
{
    internal class HDBLOCK
    {
        public HDBLOCK()
        {
            MDFHelper.StringToByte("HD",id);
            MDFHelper.StringToByte("MeaProcess",author);
            MDFHelper.StringToByte("Local PC Reference Time", timeSource);
        }

        private byte[] id = new byte[2];
        private ushort size = 208;
        private uint linkDataGroupBlock = 0;
        private uint linkMeaFileComment = 0;
        private uint linkProgramBlock = 0;
        public ushort noOfDataGroups;
        public byte[] startData = new byte[10];
        public byte[] startTime = new byte[8];
        public byte[] author = new byte[32];
        public byte[] department = new byte[32];
        public byte[] project = new byte[32];
        public byte[] subject = new byte[32];
        public ulong  timestamp;
        public short utcTimeOffset;
        private ushort timeQuality = 0;
        private byte[] timeSource = new byte[32];
        public DGBLOCK dgBlock;

        public void Write(BinaryWriter br)
        {
            br.Write(id);
            br.Write(size);
            
            linkDataGroupBlock = (uint) br.BaseStream.Position;
            br.Write((uint)0);
            
            linkMeaFileComment = (uint)br.BaseStream.Position;
            br.Write((uint)0);

            linkProgramBlock = (uint)br.BaseStream.Position;
            br.Write((uint)0);

            br.Write(noOfDataGroups);
            br.Write(startData);
            br.Write(startTime);
            br.Write(author);
            br.Write(department);
            br.Write(project);
            br.Write(subject);
            br.Write(timestamp);
            br.Write(utcTimeOffset);
            br.Write(timeQuality);
            br.Write(timeSource);

            if (dgBlock != null)
            {
                uint curPos = (uint)br.BaseStream.Position;
                br.Seek((int) linkDataGroupBlock, SeekOrigin.Begin);
                br.Write(curPos);
                br.Seek((int) curPos, SeekOrigin.Begin);
                dgBlock.Write(br);
            }                                 
        }
    }
}
