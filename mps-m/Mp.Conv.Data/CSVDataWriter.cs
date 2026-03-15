using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Mp.Drv.DataFile;

namespace Mp.Conv.Data
{
    internal class CSVDataWriter : OutputData
    {
        private string _precision = "{0:0.###}";

        public CSVDataWriter(MetaFileReaderBase reader)
            : base(reader)
        {
        }

        public int Precision
        {
            set 
            {
                _precision = "{0:0.";

                for(int i = 0; i < value; ++i)
                    _precision += "#";

                _precision += "}";           
            }
        }

        public override void Write(string outputFolderOrZipFile, bool zipFiles)
        {
            _filesToZip.Clear();
            _filesToDelete.Clear();

            string csvFileFolder = GetTargetFolder(outputFolderOrZipFile,zipFiles);
            
            if (!Directory.Exists(csvFileFolder))
                Directory.CreateDirectory(csvFileFolder);

            string baseSourcePath = Path.GetDirectoryName(_reader.MetaFile);

            string sep = ";";
            foreach (StorageGroup grp in _reader.StorageGroups)
            {
                string dirName =  Path.GetDirectoryName(grp.DataFile);

                string dataFileName = grp.DataFile;

                if (dirName == "" || dirName == null)
                    dataFileName = Path.Combine(baseSourcePath, grp.DataFile);

                using (FileStream dataFile = new FileStream(dataFileName, FileMode.Open))
                {
                    BinaryReader br = new BinaryReader(dataFile);

                    string strSscFile = Path.Combine(csvFileFolder, Path.GetFileNameWithoutExtension(grp.TargetDataFile) + ".csv");

                    if (_reader.StorageGroups.Count == 1)
                        strSscFile = strSscFile.Replace(".g1", "");

                    using (FileStream csvFStream = new FileStream(strSscFile, FileMode.Create))
                    {
                        _filesToZip.Add(strSscFile);
                        _filesToDelete.Add(strSscFile);
                        StreamWriter wr = new StreamWriter(csvFStream, Encoding.UTF8);

                        //Write meta data.
                        wr.WriteLine(_reader.DateAndTime);
                        wr.WriteLine(_reader.Comment);
                        
                        foreach (DictionaryEntry entry in _reader.Properties)
                            wr.WriteLine((string)entry.Key + sep + (string)entry.Value);

                        wr.WriteLine();

                        //Write signal header.
                        foreach (Signal sig in grp.Signals)
                            wr.Write(sig.Name + "(" + sig.Unit + ")" + sep);

                        wr.WriteLine();

                        foreach (Signal sig in grp.Signals)
                            wr.Write(sig.Comment + sep);

                        wr.WriteLine();

                        double data = 0.0;

                        long position = 0;
                        long records = dataFile.Length / grp.RecordSize;
                        int lastPercent = -1;

                        //Write data
                        try
                        {
                            while (true)
                            {
                                foreach (Signal sig in grp.Signals)
                                {
                                    switch (sig.DataType)
                                    {
                                        case "LREAL":
                                            data = sig.Factor * br.ReadDouble() + sig.Offset;
                                            break;

                                        case "REAL":
                                            data = sig.Factor * br.ReadSingle() + sig.Offset;
                                            break;

                                        case "USINT":
                                        case "BYTE":
                                            data = sig.Factor * br.ReadByte() + sig.Offset;
                                            break;
                                        case "SINT":
                                            data = sig.Factor * br.ReadSByte() + sig.Offset;
                                            break;
                                        case "UINT":
                                        case "WORD":
                                            data = sig.Factor * br.ReadUInt16() + sig.Offset;
                                            break;
                                        case "INT":
                                            data = sig.Factor * br.ReadInt16() + sig.Offset;
                                            break;
                                        case "UDINT":
                                        case "DWORD":
                                            data = sig.Factor * br.ReadUInt32() + sig.Offset;
                                            break;
                                        case "DINT":
                                            data = sig.Factor * br.ReadInt32() + sig.Offset;
                                            break;
                                        case "ULINT":
                                        case "LWORD":
                                            data = sig.Factor * br.ReadUInt64() + sig.Offset;
                                            break;
                                        case "LINT":
                                            data = sig.Factor * br.ReadInt64() + sig.Offset;
                                            break;
                                        case "BOOL":
                                            data = (double)br.ReadByte();
                                            break;

                                    }
                                    wr.Write(String.Format(_precision, data) + sep);
                                }

                                position++;

                                int percent = (int)((position * 100) / records);

                                if (lastPercent != percent)
                                    WriteProgress(grp, percent);

                                lastPercent = percent;

                                wr.WriteLine();
                            }
                        }
                        catch (System.IO.EndOfStreamException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        finally
                        {
                            br.Close();
                            wr.Close();
                            dataFile.Close();
                            csvFStream.Close();
                        }
                    }
                }
                WriteProgress(grp, 100);
            }

            if (zipFiles)
                ZipFiles(outputFolderOrZipFile, csvFileFolder);
        }
    }
}
