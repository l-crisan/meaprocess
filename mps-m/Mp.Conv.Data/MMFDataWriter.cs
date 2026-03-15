using System;
using System.Collections.Generic;
using System.Text;
using Mp.Drv.DataFile;
using System.IO;
using System.Xml;
using System.Globalization;

namespace Mp.Conv.Data
{
    internal class MMFDataWriter : OutputData
    {
        public MMFDataWriter(TDMMetaFileReader reader)
        :base(reader)
        {
        }

        public override void Write(string targetFileOrFolder, bool zipFiles)
        {
            base.Write(targetFileOrFolder, zipFiles);

            foreach (StorageGroup group in _reader.StorageGroups)
            {
                TDMMetaFileReader.ChannelData firstChnData = (TDMMetaFileReader.ChannelData) group.Signals[0].Tag;
                int position = 0;
                int lastPercent = 0; 

                using(FileStream of = new FileStream(Path.Combine(targetFileOrFolder,group.DataFile),FileMode.Create, FileAccess.Write))
                {
                    if( firstChnData.IsBlock)
                    {
                        BinaryWriter bw = new BinaryWriter(of);

                        using(FileStream inf = new FileStream(firstChnData.File,FileMode.Open,FileAccess.Read))
                        {
                            BinaryReader br = new BinaryReader(inf);

                            for (int rec = 0; rec < firstChnData.Length; ++rec)
                            {
                                for (int sig = 0; sig < group.Signals.Count; ++sig)
                                {
                                    Signal signal = group.Signals[sig];
                                    TDMMetaFileReader.ChannelData chnData = (TDMMetaFileReader.ChannelData) signal.Tag;

                                    int sigSize = MetaFileReaderBase.GetDataSize(signal.DataType);
                                    inf.Seek((chnData.ByteOffset) + (rec * group.RecordSize), SeekOrigin.Begin);
                                    byte[] buffer = new byte[sigSize];
                                    br.Read(buffer, 0, sigSize);
                                    bw.Write(buffer);
                                    
                                    position++;

                                    int percent = (int)((position * 100) / firstChnData.Length);

                                    if (lastPercent != percent)
                                        WriteProgress(group, percent);

                                    lastPercent = percent;
                                }
                            }
                        }                    
                    }
                    else
                    {
                        BinaryWriter bw = new BinaryWriter(of);
                        using(FileStream inf = new FileStream(firstChnData.File,FileMode.Open,FileAccess.Read))
                        {
                            BinaryReader br = new BinaryReader(inf);

                            for (int rec = 0; rec < firstChnData.Length; ++rec)
                            {
                                for (int sig = 0; sig < group.Signals.Count; ++sig)
                                {
                                    Signal signal = group.Signals[sig];
                                    TDMMetaFileReader.ChannelData chnData = (TDMMetaFileReader.ChannelData) signal.Tag;

                                    int sigSize = MetaFileReaderBase.GetDataSize(signal.DataType);
                                    inf.Seek((chnData.ByteOffset) + (rec * sigSize), SeekOrigin.Begin);
                                    byte[] buffer = new byte[sigSize];
                                    br.Read(buffer, 0, sigSize);
                                    bw.Write(buffer);
                                    
                                    position++;

                                    int percent = (int)((position * 100) / firstChnData.Length);

                                    if (lastPercent != percent)
                                        WriteProgress(group, percent);

                                    lastPercent = percent;
                                }

                            }
                        }                    
                    }
                }
            }

            WriteMetaFile(targetFileOrFolder);
        }

        private void WriteMetaFile(string path)
        {
            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            TDMMetaFileReader reader = (TDMMetaFileReader) _reader;

            uint maxID = 1;

            using(XmlTextWriter xmlWriter = new XmlTextWriter(Path.Combine(path,_reader.MetaFile),Encoding.UTF8))
            {
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.IndentChar = ' ';
                xmlWriter.Indentation = 4;
                xmlWriter.WriteStartDocument();

                xmlWriter.WriteStartElement("mp:MeaProcess");
                xmlWriter.WriteAttributeString("version", "1.2");
                xmlWriter.WriteAttributeString("id", "_"+ maxID.ToString());
                maxID++;
                xmlWriter.WriteAttributeString("xmlns:mp", "http://www.crisan-soft.com/Schemas/MP/1_2");
                
                xmlWriter.WriteElementString("mp:creationDateTime",reader.DateAndTime);
                xmlWriter.WriteElementString("mp:timeStamp","");
                xmlWriter.WriteElementString("mp:timeStampOffset","");
                xmlWriter.WriteElementString("mp:tool","DataConverter");
                xmlWriter.WriteElementString("mp:toolVersion",_reader.ToolVersion);
                
                if( _reader.ByteOrder == MetaFileReaderBase.ByteOrderType.LittleEndian)
                    xmlWriter.WriteElementString("mp:byteOrder", "littleEndian");
                else
                    xmlWriter.WriteElementString("mp:byteOrder", "bigEndian");
                
                xmlWriter.WriteElementString("mp:comment", _reader.Comment);

                xmlWriter.WriteElementString("mp:Properties", "");

                
                
                xmlWriter.WriteStartElement("mp:Groups");
                xmlWriter.WriteAttributeString("id", "_" + maxID.ToString());
                maxID++;

                foreach (StorageGroup group in _reader.StorageGroups)
                {
                    xmlWriter.WriteStartElement("mp:StorageGroup");
                    xmlWriter.WriteAttributeString("id", "_" + maxID.ToString());
                    maxID++;

                    xmlWriter.WriteElementString("mp:dataFile", group.DataFile);
                    xmlWriter.WriteElementString("mp:sampleRate", Convert.ToString(group.SampleRate,info));
                    xmlWriter.WriteElementString("mp:source", group.Source);
                    xmlWriter.WriteElementString("mp:sourceID", group.SourceId.ToString());

                    xmlWriter.WriteStartElement("mp:Signals");
                    xmlWriter.WriteAttributeString("id", "_" + maxID.ToString());
                    maxID++;

                    foreach (Signal signal in group.Signals)
                    {
                        xmlWriter.WriteStartElement("mp:Signal");
                        xmlWriter.WriteAttributeString("id", "_" + maxID.ToString());
                        maxID++;
                        xmlWriter.WriteElementString("mp:type", signal.Type);
                        xmlWriter.WriteElementString("mp:name", signal.Name);
                        xmlWriter.WriteElementString("mp:unit", signal.Unit);
                        xmlWriter.WriteElementString("mp:comment", signal.Comment);
                        xmlWriter.WriteElementString("mp:physMin", Convert.ToString(signal.Min,info));
                        xmlWriter.WriteElementString("mp:physMax", Convert.ToString(signal.Max,info));
                        xmlWriter.WriteElementString("mp:dataType", signal.DataType);

                        if (signal.Factor != 0)
                        {
                            xmlWriter.WriteStartElement("mp:Scaling");
                            xmlWriter.WriteAttributeString("id", "_" + maxID.ToString());
                            maxID++;

                            xmlWriter.WriteElementString("mp:type", "FACTOR_OFFSET");
                            xmlWriter.WriteElementString("mp:factor", Convert.ToString(signal.Factor,info));
                            xmlWriter.WriteElementString("mp:offset", Convert.ToString(signal.Offset, info));
                            xmlWriter.WriteEndElement();
                        }

                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();                                
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }
    }
}
