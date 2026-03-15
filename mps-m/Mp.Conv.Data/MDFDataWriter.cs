using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using Mp.Drv.DataFile;

namespace Mp.Conv.Data
{
    internal class MDFDataWriter : OutputData
    {
        public MDFDataWriter(MetaFileReaderBase reader)
            : base(reader)
        {
        }

        public override void Write(string fileOrFolder, bool zipFiles)
        {
            if (!Path.HasExtension(fileOrFolder))
                throw new Exception(StringResource.FileNameErr);

            _filesToZip.Clear();
            _filesToDelete.Clear();
            
            string mdfFile = fileOrFolder;

            if (zipFiles)
                mdfFile = Path.ChangeExtension(mdfFile, "mdf");

            string baseSourcePath = Path.GetDirectoryName(_reader.MetaFile);

            _filesToZip.Add(mdfFile);
            _filesToDelete.Add(mdfFile);

            IDBLOCK idBlock = new IDBLOCK();

            idBlock.byteOrder = 0;

            if (_reader.ByteOrder == MMFMetaFileReader.ByteOrderType.BigEndian)
                idBlock.byteOrder = 1;

            DateTime dateTime = _reader.CreationDateTime;
            CultureInfo ci = new CultureInfo("de-DE");

            string startDate = dateTime.Date.ToString(ci);
            startDate = startDate.Replace('.', ':');
            MDFHelper.StringToByte(startDate, idBlock.hdBlock.startData);

            string startTime = dateTime.ToLongTimeString();
            MDFHelper.StringToByte(startTime, idBlock.hdBlock.startTime);
            idBlock.hdBlock.timestamp = (ulong)_reader.TimeStamp * 1000000;
            idBlock.hdBlock.utcTimeOffset = (short)_reader.TimeStampOffset;

            idBlock.hdBlock.noOfDataGroups = (ushort) _reader.StorageGroups.Count;

            if (_reader.StorageGroups.Count > 0)
                idBlock.hdBlock.dgBlock = new DGBLOCK();

            DGBLOCK curGroup = null;
            
            for(int i = 0; i < _reader.StorageGroups.Count; ++i)                
            {
                StorageGroup grp = _reader.StorageGroups[i];


                if (i == 0)
                {
                    idBlock.hdBlock.dgBlock = new DGBLOCK();
                    curGroup = idBlock.hdBlock.dgBlock;
                }
                else
                {
                    curGroup.nextBlock = new DGBLOCK();
                    curGroup = curGroup.nextBlock;
                }

                grp.Tag = curGroup;
                curGroup.noOfChannelGroups = 1;
                curGroup.channelGroup.noOfRecords = (uint) grp.GetNoOfRecords(baseSourcePath);
                curGroup.channelGroup.sizeOfDataRecord = (ushort)(grp.RecordSize);
                CNBLOCK curChannelBlock = null;

                bool timeSignalAvail = false;

                for( int j = 0; j < grp.Signals.Count; ++j)                    
                {
                    Signal signal  =  grp.Signals[j];

                    if (j == 0)
                    {
                        curGroup.channelGroup.channelBlock = new CNBLOCK();
                        curChannelBlock = curGroup.channelGroup.channelBlock;
                    }
                    else
                    {
                        curChannelBlock.next = new CNBLOCK();
                        curChannelBlock = curChannelBlock.next;
                    }

                    if (signal.Type == "SIGNAL_TIME_STAMP")
                    {
                        timeSignalAvail = true;
                        curChannelBlock.channelType = 1;
                        curChannelBlock.valueRangeVaildFlag = 0;
                    }
                    else
                    {
                        curChannelBlock.channelType = 0;
                        curChannelBlock.valueRangeVaildFlag = 1;
                    }

                    MDFHelper.StringToByte(signal.Name, curChannelBlock.signalName);
                    MDFHelper.StringToByte(signal.Comment, curChannelBlock.signalDescription);
                    curChannelBlock.startOffsetInBit = (ushort) (signal.OffsetInRecord * 8);
                    curChannelBlock.noOfBits = (ushort)(MMFMetaFileReader.GetDataSize(signal.DataType) * 8);
                    curChannelBlock.signalDataType = GetSignalDataType(signal);
                    curChannelBlock.signalMin = signal.Min;
                    curChannelBlock.signalMax = signal.Max;

                    if( signal.Factor != 0)
                    {
                        curChannelBlock.convertion = new CCBLOCK();
                        if (signal.Type == "SIGNAL_TIME_STAMP")
                        {
                            curChannelBlock.convertion.valueRangeFlag = 0;
                        }
                        else
                        {
                            curChannelBlock.convertion.valueRangeFlag = 1;
                        }
                        curChannelBlock.convertion.min = signal.Min;
                        curChannelBlock.convertion.max = signal.Max;
                        
                        MDFHelper.StringToByte(signal.Unit,curChannelBlock.convertion.unit);
                        curChannelBlock.convertion.factor = signal.Factor;
                        curChannelBlock.convertion.offset = signal.Offset;
                    }
               }

                if (!timeSignalAvail)
                {//create one
                    curChannelBlock.next = new CNBLOCK();
                    curChannelBlock = curChannelBlock.next;
                    curChannelBlock.channelType = 1;
                    curChannelBlock.valueRangeVaildFlag = 0;
                    curChannelBlock.startOffsetInBit = 0;
                    curChannelBlock.noOfBits = 64;
                    curChannelBlock.signalDataType = 3;
                    curChannelBlock.rateInSec = 1.0 / grp.SampleRate;
                    MDFHelper.StringToByte("Time", curChannelBlock.signalName);                    
                    curGroup.channelGroup.noOfChannels = (ushort)(grp.Signals.Count + 1);                    
                }
                else
                {
                    curGroup.channelGroup.noOfChannels = (ushort)grp.Signals.Count;
                }
            }
            
            using (FileStream fs = new FileStream(mdfFile, FileMode.Create, FileAccess.Write))
            {
                BinaryWriter bw = new BinaryWriter(fs);
                idBlock.Write(bw);
                
                foreach (StorageGroup grp in _reader.StorageGroups)
                {
                    DGBLOCK dbBlock = (DGBLOCK)grp.Tag;
                    dbBlock.WriteDataGroupOffset(bw);

                    string dirName = Path.GetDirectoryName(grp.DataFile);

                    string dataFileName = grp.DataFile;

                    if (dirName == "" || dirName == null)
                        dataFileName = Path.Combine(baseSourcePath, grp.DataFile);

                    using (FileStream dataFile = new FileStream(dataFileName, FileMode.Open))
                    {
                        byte[] record = new byte[grp.RecordSize];
                        BinaryReader br = new BinaryReader(dataFile);
                        int lastPercent = 0;
                        long noOfRecords = grp.GetNoOfRecords(baseSourcePath);

                        for (long i = 0; i < noOfRecords; ++i)
                        {
                            br.Read(record, 0, record.Length);
                            bw.Write(record);

                            int percent = (int)((i * 100) / noOfRecords);

                            if (lastPercent != percent)
                                WriteProgress(grp, percent);

                            lastPercent = percent;
                        }
                        WriteProgress(grp, 100);
                        dataFile.Close();
                    }
                }
            }

            if (zipFiles)
                ZipFiles(fileOrFolder, Path.GetDirectoryName(mdfFile));
        }

        private ushort GetSignalDataType(Signal signal)
        {
            switch (signal.DataType)
            {
                case "LREAL":
                    return 3;

                case "REAL":
                    return 3;

                case "BYTE":
                case "USINT":
                case "UINT":
                case "WORD":
                case "BOOL":
                case "DWORD":
                case "UDINT":
                case "ULINT":
                    return 0;
                
                case "INT":
                case "SINT":
                case "DINT":
                case "LWORD":
                case "LINT":
                    return 1;
            }
            return 0;
        }

        private void WriteHDBLOCK(BinaryWriter br)
        {
            char[] id = new char[2] { 'H', 'D' };
            br.Write(id);
            
            ushort bsize = 2;
            br.Write(bsize);

        }

        private void WriteIDBLOCK(BinaryWriter br)
        {
            string text = "MDF     ";
            br.Write(text.ToCharArray());
            text = "3.20    ";
            br.Write(text.ToCharArray());
            text = "MP      ";
            br.Write(text.ToCharArray());
            ushort endian = 0;

            if (_reader.ByteOrder == MMFMetaFileReader.ByteOrderType.BigEndian)
                endian = 1;

            br.Write(endian);

            ushort floatType = 0;
            br.Write(floatType);

            ushort version = 320;
            br.Write(version);
            ushort data = 0;
            br.Write(data);
            br.Write(data);

            char[] cdata = new char[30];
            br.Write(cdata);
        }
    }
}
