using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO; 

namespace Mp.Drv.CANdb
{
    public class DBInfo : DBObject
    {
        private string _name;
        private string _file;
        private string _version;
        private  bool   _isForward;
        private List<Message> _messages = new List<Message>();

        public DBInfo()
        {
        }

        public List<Message> Messages
        {
            get { return _messages; }
        }

        public string Name
        {
            get 
            { 
                return _name; 
            }
        }

        public bool IsForward
        {
            get{ return _isForward;}
        }

        public string File
        {
            get 
            { 
                return _file; 
            }

            set
            {
                _file = value;
                _name = Path.GetFileName(value);
            }
        }

        public void ParseVersion(string line, string orgLine, int linePos)
        {
            if (line.Length < 8)
            {
                _isForward = false;
                return;
            }

            _version = line.Substring(8, line.Length - 9);

            _version = _version.TrimStart('"');
            if (_version.Length < 5)
            {
                _isForward = false;
                return;
            }

            if (_version[3] == 'F')
                _isForward = true;
            else
                _isForward = false;
        }

        public void parseAttribute(string line,int linePos)
        {

        }

        public void parseAttributeDefaultValue(string line, int linePos)
        {

        }

        public void parseAttributeDefinition(string line, int linePos)
        {
        }

    }
}
