using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using Mp.Drv.DataFile;

namespace Mp.Conv.Data
{
    internal class TDMDataWriter : OutputData
    {     
        public TDMDataWriter(MetaFileReaderBase reader)
            :base(reader)
        {
        }

        public override void Write(string fileOrFolder, bool zipFiles)
        {
            string tdmFileFolder = GetTargetFolder(fileOrFolder, zipFiles);

            if (!Directory.Exists(tdmFileFolder))
                Directory.CreateDirectory(tdmFileFolder);

            _filesToZip.Clear();
            _filesToDelete.Clear();
            string tdmFile = tdmFileFolder + Path.GetFileNameWithoutExtension(_reader.MetaFile) + ".tdm";
            string baseSourcePath = Path.GetDirectoryName(_reader.MetaFile);

            _filesToZip.Add(tdmFile);
            _filesToDelete.Add(tdmFile);
            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

	        int usi = 2;
	        int inc = 0;
            Hashtable usiMap = new Hashtable();

            using (FileStream fs = new FileStream(tdmFile, FileMode.Create))
            {
                StreamWriter sr = new StreamWriter(fs, Encoding.UTF8);

                sr.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>");
                sr.WriteLine("<usi:tdm version=\"1.0\" xmlns:usi=\"http://www.ni.com/Schemas/USI/1_0\">");

                sr.WriteLine("<usi:documentation>");
                sr.WriteLine("<usi:exporter>MeaProcess</usi:exporter>");
                sr.WriteLine("<usi:exporterVersion>" + _reader.ToolVersion + "</usi:exporterVersion>");
                sr.WriteLine("</usi:documentation>");

                sr.WriteLine("<usi:model modelName=\"National Instruments USI generated meta file\" modelVersion=\"1.0\">");
                sr.WriteLine("<usi:include  nsUri=\"http://www.ni.com/DataModels/USI/TDM/1_0\"/>");
                sr.WriteLine("</usi:model>");

                sr.WriteLine("<usi:include>");

                foreach (StorageGroup group in _reader.StorageGroups)
                {
                    if (_reader.ByteOrder == MMFMetaFileReader.ByteOrderType.LittleEndian)
                        sr.WriteLine("<file byteOrder=\"" + "littleEndian" + "\" url=\"" + group.TargetDataFile + "\">");
                    else
                        sr.WriteLine("<file byteOrder=\"" + "bigEndian" + "\" url=\"" + group.TargetDataFile + "\">");

                    foreach (Signal sig in group.Signals)
                    {
                        usiMap.Add("inc" + sig.Id.ToString(), inc);
                        sr.Write("<block_bm id=\"inc" + inc.ToString() + "\"  byteOffset=\"0\" blockSize=\"");
                        sr.Write(group.RecordSize.ToString() + "\" blockOffset=\"" + sig.OffsetInRecord.ToString() + "\" length=\"" + group.GetNoOfRecords(baseSourcePath).ToString() + "\" valueType=\"" +
                                 GetDataType(sig.DataType) + "\" />");
                        sr.WriteLine();
                        inc++;
                    }

                    sr.WriteLine("</file>");
                    sr.Flush();
                }
                sr.WriteLine("</usi:include>");

                sr.WriteLine("<usi:data>");
                sr.WriteLine("<tdm_root id=\"usi1\">");
                sr.WriteLine("<name>MeaProcess</name>");

                if (_reader.Properties.Count != 0)
                {
                    sr.WriteLine("<instance_attributes>");

                    foreach (DictionaryEntry entry in _reader.Properties)
                    {
                        sr.WriteLine("<string_attribute name=\"" + ((string)entry.Key) + "\" >");
                        sr.WriteLine("<s>" + ((string)entry.Value) + "</s>");
                        sr.WriteLine("</string_attribute>");
                    }
                    sr.WriteLine("</instance_attributes>");
                }

                sr.Write("<channelgroups>#xpointer(");

                for (int grpNo = 0; grpNo < _reader.StorageGroups.Count; ++grpNo)
                {
                    usiMap.Add("grp" + (grpNo + 1).ToString(), usi);
                    sr.Write("id(\"usi" + usi.ToString() + "\") ");
                    usi++;
                }
                sr.WriteLine(")</channelgroups>");

                sr.WriteLine("<description>" + _reader.Comment + "</description>");
                sr.WriteLine("<title>MeaProcess data file</title>");
                sr.WriteLine("<author>MeaProcess</author>");
                sr.WriteLine("<datetime>" + _reader.DateAndTime + "</datetime>");
                sr.WriteLine("</tdm_root>");
                sr.Flush();

                int grp = 0;
                foreach (StorageGroup group in _reader.StorageGroups)
                {

                    int curUsi = (int)usiMap["grp" + (grp + 1).ToString()];
                    sr.WriteLine("<tdm_channelgroup id=\"usi" + curUsi.ToString() + "\">");
                    sr.WriteLine("<name>Group" + grp.ToString() + "</name>");
                    sr.WriteLine("<root>#xpointer(id(\"usi1\"))</root>");
                    sr.WriteLine("<instance_attributes>");

                    sr.WriteLine("<string_attribute name=\"SamplingRate\">");
                    sr.Write("<s>" + group.SampleRate.ToString() + " Hz</s>"); 
                    sr.WriteLine("</string_attribute>");

                    sr.WriteLine("</instance_attributes>");
                    sr.Write("<channels>#xpointer(");

                    foreach (Signal signal in group.Signals)
                    {
                        usiMap.Add("_" + signal.Id.ToString(), usi);
                        sr.Write("id(\"usi" + usi.ToString() + "\") ");
                        usi++;
                    }

                    sr.WriteLine(") </channels>");

                    sr.Write("<submatrices>#xpointer(");

                    foreach (Signal signal in group.Signals)
                    {
                        usiMap.Add("sbm" + signal.Id.ToString(), usi);
                        sr.Write("id(\"usi" + usi.ToString() + "\") ");
                        usi++;
                    }
                    sr.WriteLine(") </submatrices>");
                    sr.WriteLine("</tdm_channelgroup>");

                    foreach (Signal signal in group.Signals)
                    {
                        curUsi = (int)usiMap["_" + (signal.Id).ToString()];

                        //CHANNEL
                        sr.WriteLine("<tdm_channel id=\"usi" + curUsi.ToString() + "\">");
                        sr.WriteLine("<name>" + signal.Name + "</name>");
                        sr.WriteLine("<description>" + signal.Comment + "</description>");
                        sr.WriteLine("<unit_string>" + signal.Unit + "</unit_string>");
                        sr.WriteLine("<datatype>" + GetSignalDataType(signal.DataType) + "</datatype>");
                        sr.WriteLine("<minimum>" + signal.Min.ToString() + "</minimum>");
                        sr.WriteLine("<maximum>" + signal.Max.ToString() + "</maximum>");

                        curUsi = (int)usiMap["grp" + (grp + 1).ToString()];

                        sr.WriteLine("<group>#xpointer(id(\"usi" + curUsi.ToString() + "\"))</group>");

                        usiMap.Add("lcol" + (signal.Id).ToString(), usi);
                        sr.WriteLine("<local_columns>#xpointer(id(\"usi" + usi.ToString() + "\"))</local_columns>");
                        usi++;

                        sr.WriteLine("<instance_attributes>");
				
                        List<DictionaryEntry> parms = signal.Parameters;

				        foreach(DictionaryEntry entry in parms)
				        {
                            sr.WriteLine("<string_attribute name=\""+entry.Key.ToString()+ "\" >");
                            sr.WriteLine("<s>"+ entry.Value.ToString()+ "</s>");
					        sr.WriteLine("</string_attribute>");
				        }
                            
                        sr.WriteLine("</instance_attributes>");
                        sr.WriteLine("</tdm_channel>");
                        //ENDCHANNEL

                        //SUBMATRIX
                        curUsi = (int)usiMap["sbm" + signal.Id.ToString()];

                        sr.WriteLine("<submatrix id=\"usi" + curUsi.ToString() + "\">");
                        sr.WriteLine("<name>" + signal.Name + "</name>");
                        sr.WriteLine("<number_of_rows>" + group.GetNoOfRecords(baseSourcePath).ToString() + "</number_of_rows>");

                        curUsi = (int)usiMap["grp" + (grp + 1).ToString()];

                        sr.WriteLine("<measurement>#xpointer(id(\"usi" + curUsi.ToString() + "\"))</measurement>");

                        curUsi = (int)usiMap["lcol" + signal.Id.ToString()];

                        sr.WriteLine("<local_columns>#xpointer(id(\"usi" + curUsi.ToString() + "\"))</local_columns>");
                        sr.WriteLine("</submatrix>");
                        //END SUBMATRIX                    

                        //LOCALCOLUMN
                        curUsi = (int)usiMap["lcol" + signal.Id.ToString()];

                        sr.WriteLine("<localcolumn id=\"usi" + curUsi.ToString() + "\">");
                        sr.WriteLine("<name>" + signal.Name + "</name>");
                        sr.WriteLine("<global_flags>15</global_flags>");
                        sr.WriteLine("<independed>0</independed>");
                        sr.WriteLine("<minimum>" + Convert.ToString(signal.Min, info) + "</minimum>");
                        sr.WriteLine("<maximum>" + Convert.ToString(signal.Max, info) + "</maximum>");

                        if (signal.Factor != 1 || signal.Offset != 0)
                        {
                            sr.WriteLine("<sequence_representation>raw_linear</sequence_representation>");
                            sr.Write("<generation_parameters>");
                            sr.Write(Convert.ToString(signal.Offset, info));
                            sr.Write(" ");
                            sr.Write(Convert.ToString(signal.Factor, info));
                            sr.WriteLine("</generation_parameters>");
                        }
                        else
                        {
                            sr.WriteLine("<sequence_representation>explicit</sequence_representation>");
                        }

                        curUsi = (int)usiMap["_" + signal.Id.ToString()];

                        sr.WriteLine("<measurement_quantity>#xpointer( id(\"usi" + curUsi.ToString() + "\"))</measurement_quantity>");

                        curUsi = (int)usiMap["sbm" + signal.Id.ToString()];

                        sr.WriteLine("<submatrix>#xpointer(id(\"usi" + curUsi.ToString() + "\"))</submatrix>");

                        sr.WriteLine("<values>#xpointer(id(\"usi" + usi.ToString() + "\")) </values>");

                        sr.WriteLine("</localcolumn>");
                        //END LOCALCOLUMN

                        //VALUES
                        sr.WriteLine("<" + GetTDMChannelValueType(signal.DataType) + " id = \"usi" + usi.ToString() + "\">");
                        usi++;
                        curUsi = (int)usiMap["inc" + signal.Id.ToString()];
                        sr.WriteLine("<values external=\"inc" + curUsi.ToString() + "\" />");
                        sr.WriteLine("</" + GetTDMChannelValueType(signal.DataType) + ">");
                        sr.Flush();
                    }

                    grp++;
                }

                sr.WriteLine("</usi:data>");
                sr.WriteLine("</usi:tdm>");
                sr.Flush();
                fs.Close();
            }

            //Copy data files.
            foreach (StorageGroup group in _reader.StorageGroups)
            {
                string dirName = Path.GetDirectoryName(group.DataFile);

                string sourceFile = group.DataFile;

                if (dirName == "" || dirName == null)
                    sourceFile = Path.Combine(Path.GetDirectoryName(_reader.MetaFile), group.DataFile);

                string destFile = Path.Combine(tdmFileFolder, group.TargetDataFile);
                
                if( sourceFile.ToUpper() != destFile.ToUpper())
                {
                    File.Copy(sourceFile, destFile, true);
                    _filesToDelete.Add(destFile);
                }

                _filesToZip.Add(destFile);
            }

            if (zipFiles)
                ZipFiles(fileOrFolder, tdmFileFolder);
        }
        
        private string GetTDMChannelValueType(string dt)
        {
            switch(dt)
	        {
		        case "LREAL":
			        return "double_sequence";
        			
		        case "REAL":
			        return "float_sequence";
        			
		        case "BYTE":
                case "SINT":
                case "USINT":
                case "BOOL":
			        return "byte_sequence";
        		       			
		        case "UINT":
                case "WORD":
                case "INT":
                    return "long_sequence"; //"short_sequence" bug in DIAdem
        			
		        case "DWORD":
                case "DINT":
                case "UDINT":
			        return "long_sequence";

		        case "LWORD":
                case "LINT":
                case "ULINT":
			        return "longlong_sequence";
            }

            return "";
        }

        private string GetSignalDataType(string dt)
        {
            switch (dt)
            {
                case "BOOL":
                    return "DT_BYTE";

                case "LREAL":
                    return "DT_DOUBLE";

                case "REAL":
                    return "DT_FLOAT";

                case "BYTE":
                case "USINT":
                    return "DT_BYTE";

                case "SINT":
                    return "DT_BYTE";

                case "WORD":
                case "UINT":
                    return "DT_SHORT";

                case "INT":
                    return "DT_SHORT";

                case "DWORD":
                case "UDINT":
                    return "DT_LONG";

                case "DINT":
                    return "DT_LONG";

                case "ULINT":
                case "LWORD":
                    return "DT_LONGLONG";

                case "LINT":
                    return "DT_LONGLONG";
            }
            return "";
        }

        private string GetDataType(string dt)
        {
	        switch(dt)
	        {
		        case "BOOL":
			        return "eUInt8Usi";
        			
		        case "LREAL":
			        return "eFloat64Usi";
        			
		        case "REAL":
			        return "eFloat32Usi";
        			
		        case "BYTE":
                case "USINT":
			        return "eUInt8Usi";
        		
		        case "SINT":
			        return "eUInt8Usi";
        			
		        case "WORD":
                case "UINT":
			        return "eUInt16Usi";

		        case "INT":
			        return "eInt16Usi";
        			
		        case "DWORD":
                case "UDINT":
			        return "eUInt32Usi";

		        case "DINT":
			        return "eInt32Usi";
        			
		        case "ULINT":
                case "LWORD":
			        return "eUInt64Usi";
        			
		        case "LINT":
			        return "eInt64Usi";
    	    }
	        return "";
        }

    }
}
