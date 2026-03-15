using System;
using System.Collections.Generic;
using System.Collections;
using System.Xml;
using System.Globalization;
using System.IO;

namespace Mp.Drv.DataFile
{
    public class TDMMetaFileReader : MetaFileReaderBase
    {
        private XmlDocument _xmlDoc = new XmlDocument();
        private List<StorageGroup> _storageGroups = new List<StorageGroup>();
        private Hashtable _properties = new Hashtable();
        private Hashtable _xmlObjMapping = new Hashtable();
        private string _dateTime = "";
        private uint _maxId = 1;

        public class ChannelData
        {
            public ChannelData(string url, bool isBlock, ByteOrderType byteOrder, int byteOffset, int length, string dataType)
            {
                Url = url;
                ByteOrder = byteOrder;
                ByteOffset = byteOffset;
                Length = length;
                DataType = dataType;
                IsBlock = isBlock;
            }

            public string Url;
            public ByteOrderType ByteOrder;
            public int ByteOffset;
            public int Length;
            public string DataType;
            public bool IsBlock;
            public string File;
        }

        private Hashtable _channelData = new Hashtable();


        private void BuildIDMapping(XmlElement xmlRoot)
        {
            foreach (XmlNode xmlNode in xmlRoot.ChildNodes)
            {
                XmlElement xmlElement = xmlNode as XmlElement;

                if (xmlElement == null)
                    continue;

                if (!xmlElement.HasAttribute("id"))
                {
                    BuildIDMapping(xmlElement);
                    continue;
                }

                string id = xmlElement.GetAttribute("id");
                _xmlObjMapping[id] = xmlElement;
                
            }
        }

        public override void Read(string metaFile)
        {            
            _xmlDoc.Load(metaFile);
            _storageGroups.Clear();
            _properties.Clear();
            _channelData.Clear();
            _maxId = 1;

            string metaFileName = Path.GetFileName(metaFile);            
            string inputPath = Path.GetDirectoryName(metaFile);

            string ext = Path.GetExtension(metaFile);
            metaFileName = metaFileName.Replace(ext,".mmf");

            base.Read(metaFileName);

            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            BuildIDMapping(_xmlDoc.DocumentElement);

            XmlElement xmlInclude = _xmlDoc.DocumentElement["usi:include"];

            foreach (XmlElement xmlFile in xmlInclude.ChildNodes)
            {
                if (xmlFile.Name == "file")
                {
                    string fileUrl = xmlFile.GetAttribute("url");
                    string byteOrderStr = xmlFile.GetAttribute("byteOrder");
                    ByteOrderType byteOrder = ByteOrderType.BigEndian;

                    if( byteOrderStr == "littleEndian")
                        byteOrder = ByteOrderType.LittleEndian;

                    foreach (XmlElement xmlBlock in xmlFile.ChildNodes)
                    {
                        int byteOffset = Convert.ToInt32(xmlBlock.GetAttribute("byteOffset"));
                        string id = xmlBlock.GetAttribute("id");
                        int length = Convert.ToInt32(xmlBlock.GetAttribute("length"));
                        string valueType = "";
                        bool isBlock = true;

                        if(xmlBlock.Name == "block")
                            isBlock = false;
                            
                        switch (xmlBlock.GetAttribute("valueType"))
                        {
                            case "eUInt8Usi":
                                valueType = "USINT";
                            break;
                            case "eFloat64Usi":
                                valueType = "LREAL";
                            break;
                            case "eFloat32Usi":
                                valueType = "REAL";
                            break;
                            case "eUInt16Usi":
                                valueType = "UINT";
                            break;

                            case "eUInt32Usi":
                                valueType = "UDINT";
                            break;
                            case "eInt16Usi":
                                valueType = "INT";
                            break;
                            case "eInt32Usi":
                                valueType = "DINT";
                            break;
                            case "eUInt64Usi":
                                valueType = "ULINT";
                            break; 
                            case "eInt64Usi":
                                valueType = "LINT";
                            break; 
                        }

                        _channelData[id] = new ChannelData(fileUrl, isBlock, byteOrder, byteOffset, length, valueType);
                    }
                }
            }

            XmlElement xmlTDMRoot = _xmlDoc.DocumentElement["usi:data"]["tdm_root"];
            
            _dateTime = xmlTDMRoot["datetime"].InnerText;
            string groups = xmlTDMRoot["channelgroups"].InnerText;
            string[] groupArray = GetXPathIDs(groups);

            int no = 0;
            foreach (string groupStr in groupArray)
            {
                if (groupStr == "")
                    continue;

                XmlElement xmlGroup = (XmlElement)_xmlObjMapping[groupStr];
                StorageGroup group = new StorageGroup();
                group.Number = no;
                no++;
                group.SourceId = _maxId;
                _maxId++;
                group.SampleRate = GetGroupRate(xmlGroup);
                string[] channels = GetXPathIDs(xmlGroup["channels"].InnerText);                

                int offsetInRecord = 0;

                foreach (string channleStr in channels)
                {
                    if (channleStr == "")
                        continue;

                    XmlElement xmlChannel = (XmlElement)_xmlObjMapping[channleStr];

                    Signal signal = new Signal();

                    signal.Comment = xmlChannel["description"].InnerText;                    
                    signal.Id = _maxId;
                    _maxId++;
                    signal.Min = Convert.ToDouble(xmlChannel["minimum"].InnerText, info);
                    signal.Max = Convert.ToDouble(xmlChannel["maximum"].InnerText, info);
                    signal.Name = xmlChannel["name"].InnerText;
                    signal.OffsetInRecord = offsetInRecord;
                    signal.Type = "SIGNAL_GENERAL";
                    signal.Unit = xmlChannel["unit_string"].InnerText;
                    offsetInRecord += GetDataSize(signal.DataType);

                    string[] locCols = GetXPathIDs(xmlChannel["local_columns"].InnerText);

                    XmlElement xmlChnLocalCol = (XmlElement)_xmlObjMapping[locCols[0]];
                    
                    string[] values = GetXPathIDs(xmlChnLocalCol["values"].InnerText);
                    XmlElement valuePointer = (XmlElement)_xmlObjMapping[values[0]];
                    XmlElement xmlValue = (XmlElement)valuePointer.ChildNodes[0];
                    
                    if (!xmlValue.HasAttribute("external"))
                        throw new Exception("Only external data can be converted to MMF");

                    string valueID = xmlValue.GetAttribute("external");
                    ChannelData chnData = (ChannelData) _channelData[valueID];

                    signal.DataType = chnData.DataType;
                    chnData.File = Path.Combine(inputPath, chnData.Url);

                    signal.Tag = chnData;
                    
                    if (xmlChnLocalCol["sequence_representation"].InnerText == "raw_linear")
                    {
                        string scaling = xmlChnLocalCol["generation_parameters"].InnerText;
                        string[] fo = scaling.Split(' ');

                        signal.Factor = Convert.ToDouble(fo[1], info);
                        signal.Offset = Convert.ToDouble(fo[0], info);
                    }
                    group.Signals.Add(signal);
                }

                string mext = Path.GetExtension(metaFileName);
                string groupFileName = metaFileName.Replace(mext, "") + (no).ToString() + ".mpd";
                group.TargetDataFile = Path.GetFileName(groupFileName);
                group.DataFile = Path.GetFileName(groupFileName);
                _storageGroups.Add(group);
            }
         }

        private static void CopyData(string metaFileName, string inputPath, int no, StorageGroup group, List<ChannelData> channelDataList)
        {

            ChannelData firstChannel = (ChannelData)channelDataList[0];

            if (firstChannel.IsBlock)
            {
                int records = firstChannel.Length;
                using (FileStream ifs = new FileStream(Path.Combine(inputPath, firstChannel.Url), FileMode.Open, FileAccess.Read))
                {
                    BinaryReader br = new BinaryReader(ifs);
                    using (FileStream ofs = new FileStream(Path.Combine(inputPath, firstChannel.Url), FileMode.Open, FileAccess.Read))
                    for (int i = 0; i < records; ++i)
                    {
                        foreach (ChannelData chnData in channelDataList)
                        {

                        }
                    }
                }
            }
            else
            {

            }
        }
    
        private string GetChannelDataType(string dtype)
        {
            switch (dtype)
            {
                case "DT_DOUBLE":
                    return "LREAL";
                case "DT_BYTE":
                    return "SINT";
                case "DT_FLOAT":
                    return "REAL";
                case "DT_SHORT":
                    return "INT";
                case "DT_LONG":
                    return "DINT";
                case "DT_LONGLONG":
                    return "LINT";
            }
            return  "UINT";
        }
        private static string[] GetXPathIDs(string xpath)
        {
            xpath = xpath.Replace("#xpointer", "");
            xpath = xpath.Replace("id", "");
            xpath = xpath.Replace("(", "");
            xpath = xpath.Replace(")", "");
            xpath = xpath.Replace("\"", "");
            string[] array = xpath.Split(' ');
            return array;
        }

        private double GetGroupRate(XmlElement xmlGroup)
        {
            try
            {
                XmlElement atrbs = xmlGroup["instance_attributes"];
                NumberFormatInfo info = new NumberFormatInfo();
                info.NumberDecimalSeparator = ".";

                foreach (XmlElement xmlChild in atrbs.ChildNodes)
                {
                    if (xmlChild.GetAttribute("name") == "SamplingRate")
                    {
                        string[] arr = xmlChild.InnerText.Split(' ');

                        if (arr.Length > 1)
                        {
                            if (arr[1].ToUpper() == "HZ")
                                return Convert.ToDouble(arr[0], info);
                            else if (arr[1].ToUpper() == "KHZ")
                                return Convert.ToDouble(arr[0], info) * 1000;
                        }
                        else
                            return Convert.ToDouble(arr[0], info);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 1;
        }

        public override long TimeStamp
        {
            get { return 0; }
        }

        public override int TimeStampOffset
        {
            get { return 0; }
        }

        public override Hashtable Properties
        {
            get { return _properties; }
        }

        public override DateTime CreationDateTime
        {
            get
            {
                return new DateTime();
            }
        }

        public override string DateAndTime
        {
            get { return _dateTime; }
        }

        public override List<StorageGroup> StorageGroups
        {
            get { return _storageGroups; }
        }

        public override string Comment
        {
            get { return ""; }
        }

        public override string ToolVersion
        {
            get { return _xmlDoc.DocumentElement["usi:documentation"]["usi:exporter"].InnerText; }
        }

        public override ByteOrderType ByteOrder
        {
            get
            {
              return ByteOrderType.LittleEndian;
            }
        }
    }
}
