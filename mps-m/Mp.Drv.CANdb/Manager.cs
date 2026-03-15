using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

namespace Mp.Drv.CANdb
{
    public class Manager
    {
        private List<DBInfo> _dbInfos = new List<DBInfo>();
        private int _openedFiles = 0;
        private List<ValueType> _valueTypes = new List<ValueType>();

        private class ValueType
        {
            public ValueType()
            {
            }

            public uint ID;
            public string Signal;
            public Signal.ValueType DataType;
        }

        public Manager()
        {
        }

        public Index AddDbFromFile(string file)
        {
            String line;
            DBInfo dbInfo = new DBInfo();
            Message msgInfo = null;
            int linePos = 1;

            dbInfo.File = file;
            _valueTypes.Clear();

            //read file
            try
            {                
                using (StreamReader fs = new StreamReader(file, Encoding.Default, true))
                {                    
                    while ((line = fs.ReadLine()) != null)
                    {
                        string orgLine = line;

                        line = line.TrimStart(' ');
                        string[] items = line.Split(' ');

                        if (items.Length < 1)
                        {
                            linePos++;
                            continue;
                        }

                        if (items[0] == "VERSION")
                        {
                            dbInfo.ParseVersion(line, orgLine, linePos);
                        }
                        else if (items[0] == "BO_")
                        {
                            msgInfo = new Message();
                            msgInfo.ParseMessage(dbInfo, line, orgLine, linePos);
                            dbInfo.Messages.Add(msgInfo);
                            
                        }
                        else if (items[0] == "SG_")
                        {
                            Signal sigInfo = new Signal();

                            if (msgInfo == null)
                                throw new Exception( linePos.ToString()  + " : " + StringResource.SigWithoutMsgErr + "(" + orgLine + ")");

                            sigInfo.ParseSignal(dbInfo, msgInfo, line, orgLine, linePos);
                        }
                        else if(line.StartsWith("SIG_VALTYPE_"))
                        {
                            //SIG_VALTYPE_ 18 New_Signal_2 : 2;

                            string[] data = line.Split(' ');

                            if (data.Length != 5)
                                continue;

                            ValueType vt = new ValueType();
                            try
                            {
                                vt.ID = Convert.ToUInt32(data[1]);
                                vt.Signal = data[2];

                                data[4] = data[4].TrimEnd(';');
                               switch(Convert.ToUInt32(data[4]))
                               {
                                   case 2:
                                       vt.DataType = Signal.ValueType.Double;
                                   break;
                                   
                                   case 1:
                                       vt.DataType = Signal.ValueType.Float;
                                   break;

                                   default:
                                   throw new Exception(linePos.ToString()  + " : " +StringResource.WrongValType + "(" + orgLine + ")");
                               }

                               _valueTypes.Add(vt);

                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                throw new Exception(linePos.ToString() + " : " + StringResource.WrongValType + "(" + orgLine +")");
                            }
                            
                        }

                        linePos++;
                    }
                    fs.Close();
                }

                foreach (ValueType vt in _valueTypes)
                {
                    Message msg = GetMessage(vt.ID, dbInfo);
                    
                    if (msg == null)
                        continue;

                    Signal sig = GetSignal(msg, vt.Signal);
                    
                    if (sig == null)
                        continue;

                    sig.DataType = vt.DataType;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
                throw new Exception(String.Format(StringResource.FileOpenErr,file));
            }


            _dbInfos.Add(dbInfo);
            int retVal = _openedFiles;
            _openedFiles++;
            return new Index(retVal);
        }

        private Message GetMessage(ulong id, DBInfo info)
        {
            foreach (Message msg in info.Messages)
            {
                if (msg.ID == id)
                    return msg;
            }
            return null;
        }

        private Signal GetSignal(Message msg, string sigName)
        {
            foreach (Signal sig in msg.Signals)
            {
                if (sig.Name == sigName)
                    return sig;
            }
            return null;
        }

        public DBObject GetDBObject(Index index)
        {
            if (index.DB != -1 && index.MSG == -1 && index.Sig == -1)
            {
                return GetDB(index.DB);
            }
            else if (index.DB != -1 && index.MSG != -1 && index.Sig == -1)
            {
                return GetMessage(index.DB, index.MSG);
            }
            else if (index.DB != -1 && index.MSG != -1 && index.Sig != -1)
            {
                return GetSignal(index.DB, index.MSG, index.Sig);
            }

            return null;
        }
        
        public DBInfo GetDB(int index)
        {
            return _dbInfos[index];
        }

        public Message GetMessage(int dbIndex, int msgIndex)
        {
            return _dbInfos[dbIndex].Messages[msgIndex];
        }

        public Signal GetSignal(int dbIndex, int msgIndex, int sigIndex)
        {
            return GetMessage(dbIndex, msgIndex).Signals[sigIndex];
        }

        public int DBCount
        {
            get { return _dbInfos.Count; }
        }

    }
}
