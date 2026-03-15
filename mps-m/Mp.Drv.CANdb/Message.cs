using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Drv.CANdb
{
    public class Message : DBObject
    {
        private ulong   _id;
        private string  _name;
        private int     _byteCount;
        private string  _networkName;
        private List<Signal> _signals = new List<Signal>();

        public Message()
        {
        }

        public List<Signal> Signals
        {
            get { return _signals; }
        }
        public string Name
        {
            get { return _name; }
        }

        public ulong ID
        {
            get{ return _id;}
        }

        public int ByteCount
        {
            get { return _byteCount; }
        }

        public string NetworkName
        {
            get { return _networkName; }
        }

        public void ParseMessage(DBInfo dbInfo, string line, string orgLine, int linePos)
        {
          string[] msgToken = line.Split(' ');

          int index = 1;

          if(msgToken.Length < 5)
              throw new Exception(linePos.ToString() + " : "+ StringResource.MsgDefErr + "(" + orgLine + ")");

          try
          {
              _id = Convert.ToUInt64(msgToken[index]);
          }
          catch(Exception e)
          {
              Console.WriteLine(e.Message);
              throw new Exception(linePos.ToString()  + " : " + StringResource.MsgIDErr + "(" + orgLine + ")");
          }
          index++;

          _name = msgToken[index];
          if (_name[_name.Length - 1] == ':')
          {
              _name = _name.TrimEnd(':');
          }
          else
          {
              index++;
          }

          index++;

          try
          {
              _byteCount =  Convert.ToInt32(msgToken[index]);
          }
          catch(Exception e)
          {
              Console.WriteLine(e.Message);
              throw new Exception( linePos.ToString() + " : " + StringResource.ByteCoutErr + "(" + orgLine + ")");
          }

          _networkName = msgToken[4];
     }
    }
}
