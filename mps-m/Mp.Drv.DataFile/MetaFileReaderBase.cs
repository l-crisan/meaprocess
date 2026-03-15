using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Mp.Drv.DataFile
{
    public class MetaFileReaderBase
    {
        private string _metaFile;

        public MetaFileReaderBase()
        {
        }

        public virtual void Read(string metaFile)
        {
            _metaFile = metaFile;
        }

        public virtual void Group(OutputData.ProgressDelegate progress)
        {

        }

        public virtual void CleanUp()
        {
        }

        public static int GetDataSize(string dataType)
        {
            switch (dataType)
            {
                case "LREAL":
                    return 8;

                case "REAL":
                    return 4;

                case "BYTE":
                case "SINT":
                case "USINT":
                case "BOOL":
                    return 1;

                case "UINT":
                case "WORD":
                case "INT":
                    return 2;

                case "DWORD":
                case "DINT":
                case "UDINT":
                    return 4;

                case "LWORD":
                case "LINT":
                case "ULINT":
                    return 8;
            }
            return 0;
        }

        public virtual string MetaFile
        {
            get { return _metaFile; }
        }

        public virtual long TimeStamp
        {
            get { return 0; }
        }

        public virtual int TimeStampOffset
        {
            get { return 0; }
        }

        public virtual Hashtable Properties
        {
            get { return new Hashtable(); }
        }

        public virtual DateTime CreationDateTime
        {
            get
            {
                return new DateTime();
            }
        }

        public virtual string DateAndTime
        {
            get { return ""; }
        }

        public virtual List<StorageGroup> StorageGroups
        {
            get { return new List<StorageGroup>(); }
        }

        public virtual string Comment
        {
            get { return ""; }
        }

        public enum ByteOrderType
        {
            LittleEndian,
            BigEndian
        }

        public virtual string ToolVersion
        {
            get { return ""; }
        }

        public virtual ByteOrderType ByteOrder
        {
            get
            {
                return ByteOrderType.LittleEndian;
            }
        }
    }
}
