using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Xml;

namespace Mp.Drv.DataFile
{
    public class Signal
    {
        private uint _id;
        private string _name;
        private string _type;
        private string _unit;
        private double _min;
        private double _max;
        private double _factor = 0;
        private double _offset;
        private string _comment;
        private int _objSize;
        private string _dataType;
        private int _offsetInRecord;
        private object _tag;
        string _cat;

        List<DictionaryEntry> _parameters = new List<DictionaryEntry>();

        public uint Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Cat
        {
            get { return _cat; }
            set { _cat = value; }
        }

        public int ObjectSize
        {
            get { return _objSize; }
            set { _objSize = value; }
        }

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

        public double Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public double Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public double Factor
        {
            get { return _factor; }
            set { _factor = value; }
        }

        public double Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public int OffsetInRecord
        {
            get { return _offsetInRecord; }
            set { _offsetInRecord = value; }
        }

        public List<DictionaryEntry> Parameters
        {
            get { return _parameters; }
        }
    }
}
