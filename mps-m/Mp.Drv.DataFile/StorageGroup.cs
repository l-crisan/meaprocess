using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Mp.Drv.DataFile
{
    public class StorageGroup
    {
        private string _targetDataFile;
        private string  _dataFile;
        private double  _sampleRate;
        private uint    _sourceId;
        private string  _source;
        private int     _number;
        private object  _tag;
        public List<Signal> _signals = new List<Signal>();


        public string TargetDataFile
        {
            get { return _targetDataFile; }
            set { _targetDataFile = value; }                 
        }

        public string DataFile
        {
            get { return _dataFile; }
            set { _dataFile = value; }
        }

        public double SampleRate
        {
            get { return _sampleRate; }
            set { _sampleRate = value; }
        }

        public uint SourceId
        {
            get { return _sourceId; }
            set { _sourceId = value; }
        }

        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }
        
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public long GetNoOfRecords(string basePath)
        {
            string file = Path.Combine(basePath, DataFile);
            FileInfo finfo = new FileInfo(file);
            return finfo.Length / RecordSize;
        }

        public int RecordSize
        {
            get
            {
                int recordSize = 0;
                foreach (Signal signal in Signals)
                {
                    switch (signal.DataType)
                    {
                        case "BYTE":
                        case "USINT":
                        case "SINT":
                        case "BOOL":
                            recordSize += 1;
                            break;

                        case "WORD":
                        case "UINT":
                        case "INT":
                            recordSize += 2;
                            break;

                        case "DWORD":
                        case "UDINT":
                        case "DINT":
                        case "REAL":
                            recordSize += 4;
                            break;

                        case "ULINT":
                        case "LWORD":
                        case "LINT":
                        case "LREAL":
                            recordSize += 8;
                            break;
                    }
                }
                return recordSize;
            }
        }

        public List<Signal> Signals
        {
            get { return _signals; }
        }
    }
}
