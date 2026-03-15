using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Collections;

namespace Mp.Drv.DataFile
{
    public class MMFMetaFileReader : MetaFileReaderBase
    {
        private XmlDocument _xmlDoc = new XmlDocument();
        private List<StorageGroup> _storageGroups = new List<StorageGroup>();
        private Hashtable _properties = new Hashtable();
        private string _metaFile;
        private List<string> _filesToRemove = new List<string>();

        public override void Read(string metaFile)
        {
            _metaFile = metaFile;

            _xmlDoc.Load(_metaFile);
            _storageGroups.Clear();
            _properties.Clear();

            XmlElement xmlProperties = _xmlDoc.DocumentElement["mp:Properties"];

            if (xmlProperties != null)
            {
                foreach (XmlElement xmlProperty in xmlProperties)
                {
                    string name = xmlProperty.Attributes["name"].Value;
                    string value = xmlProperty.InnerText;
                    _properties[name] = value;
                }
            }

            XmlElement xmlGoups = _xmlDoc.DocumentElement["mp:Groups"];

            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            int grp = 0;
            foreach (XmlElement element in xmlGoups.ChildNodes)
            {
                if (element.Name == "mp:StorageGroup")
                {
                    StorageGroup strGrp = new StorageGroup();
                    strGrp.Number = grp;
                    grp++;
                    strGrp.TargetDataFile = element["mp:dataFile"].InnerText;
                    strGrp.DataFile = element["mp:dataFile"].InnerText;
                    strGrp.SampleRate = Double.Parse(element["mp:sampleRate"].InnerText, info);
                    strGrp.SourceId = UInt32.Parse(element["mp:sourceID"].InnerText);
                    strGrp.Source = element["mp:source"].InnerText;
                                        
                    //Parse signals.
                    int offsetInRecord = 0;

                    XmlElement signals = element["mp:Signals"];
                    foreach (XmlElement xmlSignal in signals.ChildNodes)
                    {
                        Signal signal = new Signal();

                        string strid = xmlSignal.Attributes["id"].Value.Remove(0,1);

                        signal.Id = Convert.ToUInt32(strid);

                        XmlElement xmlComment = xmlSignal["mp:comment"];
                        
                        if( xmlComment != null)
                            signal.Comment = xmlComment.InnerText;

                        signal.DataType = xmlSignal["mp:dataType"].InnerText;

                        if (xmlSignal["mp:dataTypeSize"] != null)
                            signal.ObjectSize = Convert.ToInt32(xmlSignal["mp:dataTypeSize"].InnerText);

                        XmlElement min = xmlSignal["mp:physMin"];
                        if (min != null)
                            signal.Min = Convert.ToDouble(min.InnerText, info);
                            

                        XmlElement max = xmlSignal["mp:physMax"];
                        if (max != null)
                            signal.Max = Convert.ToDouble(max.InnerText, info);

                        signal.Name = xmlSignal["mp:name"].InnerText;
                        signal.Unit = xmlSignal["mp:unit"].InnerText;

                        if (xmlSignal["mp:cat"] != null)
                            signal.Cat = xmlSignal["mp:cat"].InnerText;

                        XmlElement xmlParameters = xmlSignal["mp:Properties"];

                        if (xmlParameters != null)
                        {
                            List<DictionaryEntry> sigParams = signal.Parameters;
                            foreach (XmlElement xmlSigProp in xmlParameters.ChildNodes)
                            {
                                DictionaryEntry entry = new DictionaryEntry(xmlSigProp.Attributes["name"].Value,xmlSigProp.InnerText);
                                sigParams.Add(entry);
                            }
                        }

                        XmlElement xmlScaling = xmlSignal["mp:Scaling"];

                        signal.Factor = 1;
                        signal.Offset = 0;

                        if (xmlScaling != null)
                        {
                            XmlElement xmlScalingType = xmlScaling["mp:type"];
                            
                            if (xmlScalingType.InnerText != "FACTOR_OFFSET")
                                throw new Exception("Unsupported scaling type.");

                            XmlElement xmlFactor = xmlScaling["mp:factor"];
                            signal.Factor = Convert.ToDouble(xmlFactor.InnerText, info);

                            XmlElement xmlOffset = xmlScaling["mp:offset"];
                            signal.Offset = Convert.ToDouble(xmlOffset.InnerText, info);
                        }
                        signal.OffsetInRecord = offsetInRecord;
                        offsetInRecord += GetDataSize(signal.DataType);

                        strGrp.Signals.Add(signal);
                    }

                    _storageGroups.Add(strGrp);
                }
            }
        }

        public override void CleanUp()
        {
            if (_filesToRemove.Count > 0)
            {
                foreach (string p in _filesToRemove)
                    File.Delete(p);

                _filesToRemove.Clear();
            }
        }
        public override void Group(OutputData.ProgressDelegate progress)
        {
            CleanUp();

            string key = "";
            Hashtable groups = new Hashtable();
            List<StorageGroup> groupToPutTogether = null;

            bool needToGroup = false;
            foreach (StorageGroup group in StorageGroups)
            {
                key = ((int)group.SampleRate).ToString() + "_"+ group.GetNoOfRecords(Path.GetDirectoryName(MetaFile)).ToString();

                if (!groups.Contains(key))
                {
                    groupToPutTogether = new List<StorageGroup>();
                    groupToPutTogether.Add(group);
                    groups[key] = groupToPutTogether;
                }
                else
                {
                    groupToPutTogether = (List<StorageGroup>)groups[key];
                    groupToPutTogether.Add(group);
                    needToGroup = true;
                }
            }

            if (!needToGroup)
                return;

            //Group
            string sourcePath = Path.GetDirectoryName(MetaFile);

            //Clone the document and group
            foreach (DictionaryEntry entry in groups)
            {
                List<StorageGroup> groupsToGroup = (List<StorageGroup>)entry.Value;
                
                if (groupsToGroup.Count < 2)
                    continue;


                StorageGroup baseGroup = groupsToGroup[0];

                FileStream[] fs    = new FileStream[groupsToGroup.Count];
                BinaryReader[] brs = new BinaryReader[groupsToGroup.Count];

                //Open the binary files.
                int i = 0;
                foreach (StorageGroup group in groupsToGroup)
                {
                    fs[i] = new FileStream(Path.Combine(sourcePath, group.TargetDataFile),FileMode.Open);
                    brs[i] = new BinaryReader(fs[i]);
                    ++i;
                }
                
                //Copy the data into the new binary files                
                string tempfName = Path.GetFileNameWithoutExtension(Path.GetTempFileName());
                tempfName = Path.Combine(Path.GetTempPath(), tempfName);


                FileStream targetFile = new FileStream( tempfName, FileMode.Create);
                _filesToRemove.Add(tempfName);
                BinaryWriter bw = new BinaryWriter(targetFile);

                long samples = baseGroup.GetNoOfRecords(sourcePath);

                int recordSize = 0;
                long percent = 0;
                long oldPercent = 0;

                for(long sample = 0; sample < samples; ++sample)
                {
                    int k = 0;
                    recordSize = 0;
                    percent = sample * 100/samples;

                    foreach (StorageGroup group in groupsToGroup)
                    {
                        Signal signal = group.Signals[0];

                        if (k != 0 && signal.Type == "SIGNAL_TIME_STAMP")
                        {
                            int dataSize = GetDataSize(signal.DataType);
                            brs[k].ReadBytes(dataSize);
                            bw.Write(brs[k].ReadBytes(group.RecordSize - dataSize));
                            recordSize += (group.RecordSize - dataSize);
                        }
                        else
                        {
                            bw.Write(brs[k].ReadBytes(group.RecordSize));
                            recordSize += group.RecordSize;
                        }

                        k++;                                   

                        if( progress != null && percent != oldPercent)
                        {
                            oldPercent = percent;
                            progress(group, (int) percent); 
                        }
                    }

                    progress(groupsToGroup[groupsToGroup.Count - 1], 100); 
                }

                //Close the binary files
                targetFile.Close();
                
                foreach (FileStream f in fs)
                    f.Close();                    

                baseGroup.DataFile = tempfName;

                //Remove the unneeded groups.
                for(int j = 1; j < groupsToGroup.Count; ++j)                    
                {
                    StorageGroup group = groupsToGroup[j];
                    foreach(Signal sig in group.Signals)
                        baseGroup.Signals.Add(sig);

                    _storageGroups.Remove(group);
                }                                
            }

            //Recalc offsets in record
            foreach (StorageGroup group in this.StorageGroups)
            {
                int offsetInRecord = 0;
                foreach (Signal signal in group.Signals)
                {
                    signal.OffsetInRecord = offsetInRecord;
                    offsetInRecord += GetDataSize(signal.DataType);
                }
            }
            //Remove double timestamp signal
            foreach (StorageGroup group in this.StorageGroups)
            {
                bool timeStampSigFound = false;

                //Remove the time signals if are two ore more
                for (int j = 0; j < group.Signals.Count; ++j)
                {                    
                    Signal signal = group.Signals[j];
                    if (signal.Type == "SIGNAL_TIME_STAMP" && !timeStampSigFound)
                    {
                        timeStampSigFound = true;                        
                    }
                    else if(signal.Type == "SIGNAL_TIME_STAMP" && timeStampSigFound)
                    {
                        group.Signals.Remove(signal);
                        --j;
                    }
                }
            }
        }

        public override string MetaFile
        {
            get { return _metaFile; }
        }

        public override long TimeStamp
        {
            get { return Convert.ToInt64(_xmlDoc.DocumentElement["mp:timeStamp"].InnerText); }
        }

        public override int TimeStampOffset
        {
            get { return Convert.ToInt32(_xmlDoc.DocumentElement["mp:timeStampOffset"].InnerText); }
        }

        public override Hashtable Properties
        {
            get { return _properties; }
        }

        public override DateTime CreationDateTime
        {
            get
            {
                XmlElement creationData = _xmlDoc.DocumentElement["mp:creationDateTime"];
                string date = creationData.InnerText;                
                string [] g = date.Split('T');
                
                if (g.Length != 2)
                    throw new Exception("Unknow creation date format.");

                string[] dateComp = g[0].Split('-');
                int year = Int32.Parse(dateComp[0]);
                int month = Int32.Parse(dateComp[1]);
                int day = Int32.Parse(dateComp[2]);

                string time = g[1];
                g = time.Split(':');

                if (g.Length != 3)
                    throw new Exception("Unknow creation time format.");

                int hour = Int32.Parse(g[0]);
                int minute = Int32.Parse(g[1]);
                string[] secarr = g[2].Split('.');
                int second = Int32.Parse(secarr[0]);
                return new DateTime(year, month, day, hour, minute, second);
            }
        }

        public override string DateAndTime
        {
            get { return _xmlDoc.DocumentElement["mp:creationDateTime"].InnerText; }
        }

        public override List<StorageGroup> StorageGroups
        {
            get{ return _storageGroups; }
        }

        public override string Comment
        {
            get { return _xmlDoc.DocumentElement["mp:comment"].InnerText; }
        }

        public override string ToolVersion
        {
            get { return _xmlDoc.DocumentElement["mp:toolVersion"].InnerText; }
        }

        public override ByteOrderType ByteOrder
        {
            get
            {
                if (_xmlDoc.DocumentElement["mp:byteOrder"].InnerText == "littleEndian")
                    return ByteOrderType.LittleEndian;
                else
                    return ByteOrderType.BigEndian;
            }
        }
    }
}
