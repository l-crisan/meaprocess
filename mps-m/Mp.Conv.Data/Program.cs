using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Mp.Drv.DataFile;

namespace Mp.Conv.Data
{
    class Program
    {
        static int Main(string[] args)
        {
            System.Console.WriteLine("MeaProcess-Data Converter (Version 1.3)");
            System.Console.WriteLine("Copyright 2010 (C) Atesion GmbH, All rights reserved.");            
            System.Console.WriteLine();

            if (args.Length < 3)
            {
                PrintUsage();
                return -1;
            }

            string sourceFile = "";
            string targetFile = "";
            try
            {
                if (args[0] == "/CSV" || args[0] == "/CSVZIP")
                {
                    if (args.Length != 4)
                    {
                        PrintUsage();
                        return -1;
                    }
                    sourceFile = args[2].TrimStart('"');
                    sourceFile = sourceFile.TrimEnd('"');

                    targetFile = args[3].TrimStart('"');
                    targetFile = targetFile.TrimEnd('"');
                }
                else
                {
                    sourceFile = args[1].TrimStart('"');
                    sourceFile = sourceFile.TrimEnd('"');

                    targetFile = args[2].TrimStart('"');
                    targetFile = targetFile.TrimEnd('"');
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Invalide arguments.");
            }


            if (!File.Exists(sourceFile))
            {
                Console.WriteLine("Error: The file '" + sourceFile + "' not found.");
                return -1;
            }

            try
            {
                MetaFileReaderBase reader = null;
                if( args[0] != "/MMF")
                    reader = new MMFMetaFileReader();
                else
                    reader = new TDMMetaFileReader();

                reader.Read(sourceFile);
                reader.Group(new OutputData.ProgressDelegate(PrintProgress));

                switch (args[0])
                {
                    case "/MMF":
                        {
                            MMFDataWriter writer = new MMFDataWriter((TDMMetaFileReader)reader);
                            writer.OnProgress += new OutputData.ProgressDelegate(PrintProgress);
                            writer.Write(targetFile, false);
                        }
                        break;

                    case "/CSV":
                        {
                            CSVDataWriter writer = new CSVDataWriter(reader);
                            int precision = 6;
                            try
                            {
                                precision = Convert.ToUInt16(args[1]);
                            }
                            catch (Exception ex)
                            {
                                precision = 6;
                            }
                            writer.Precision = precision;
                            writer.OnProgress += new OutputData.ProgressDelegate(PrintProgress);
                            writer.Write(targetFile, false);
                        }
                        break;

                    case "/TDM":
                        {
                            TDMDataWriter writer = new TDMDataWriter(reader);
                            writer.OnProgress += new OutputData.ProgressDelegate(PrintProgress);
                            writer.Write(targetFile, false);
                        }
                        break;

                    case "/CSVZIP":
                        {
                            int precision = 6;
                            try
                            {
                                precision = Convert.ToUInt16(args[1]);
                            }
                            catch (Exception ex)
                            {
                                precision = 6;
                            }
                            CSVDataWriter writer = new CSVDataWriter(reader);
                            writer.Precision = precision;
                            writer.OnProgress += new OutputData.ProgressDelegate(PrintProgress);
                            writer.Write(targetFile, true);
                        }
                        break;

                      case "/TDMZIP":
                        {
                            TDMDataWriter writer = new TDMDataWriter(reader);
                            writer.OnProgress += new OutputData.ProgressDelegate(PrintProgress);
                            writer.Write(targetFile, true);
                        }
                        break;
                    case "/MDF":
                        {
                            MDFDataWriter writer = new MDFDataWriter(reader);
                            writer.OnProgress += new OutputData.ProgressDelegate(PrintProgress);
                            writer.Write(targetFile, false);

                        }
                        break;
                    case "/MDFZIP":
                        {
                            MDFDataWriter writer = new MDFDataWriter(reader);
                            writer.OnProgress += new OutputData.ProgressDelegate(PrintProgress);
                            writer.Write(targetFile, true);
                        }
                        break;

                }

                reader.CleanUp();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1;
            }

            return 0;
        }

        private static void PrintUsage()
        {
            System.Console.WriteLine("Usage: Mp.DataConverter.exe flag <input-file> <output>");
            System.Console.WriteLine("");
            System.Console.WriteLine("flag           : /CSV precision    => for CSV file as output");
            System.Console.WriteLine("                 /MDF              => for MDF file as output");
            System.Console.WriteLine("                 /TDM              => for TDM file as output");
            System.Console.WriteLine("                 /CSVZIP precision => for zipped CSV file as output");
            System.Console.WriteLine("                 /MDFZIP           => for zipped MDF file as output");
            System.Console.WriteLine("                 /TDMZIP           => for zipped TDM file as output");
            System.Console.WriteLine("                 /MMF              => for MMF file as output");
            System.Console.WriteLine("                                      only if input file is TDM");
            System.Console.WriteLine("<input-file>   : The MeaProcess meta data file.");
            System.Console.WriteLine("<output>       : The output folder or output file");
            System.Console.WriteLine("");
            System.Console.WriteLine("Example: Mp.DataConverter.exe /CSV 3 \"c:\\test.mmf\" \"c:\\MyFolder\\\"");
        }

        static void PrintProgress(StorageGroup grp, int percent)
        {
            if( grp != null)
                Console.Write("Group: " + (grp.Number + 1).ToString() + " Position: " + percent.ToString() + " %                   \r");
            else
                Console.Write("Zipping files.. " +  " Position: " + percent.ToString() + " %                   \r");
        }
    }
}
