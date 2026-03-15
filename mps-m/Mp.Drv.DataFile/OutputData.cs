using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

namespace Mp.Drv.DataFile
{
    public class OutputData
    {
        public delegate void ProgressDelegate(StorageGroup group, int persent);

        public event ProgressDelegate OnProgress;
        protected MetaFileReaderBase _reader;
        protected List<string> _filesToZip = new List<string>();
        protected List<string> _filesToDelete = new List<string>();

        public OutputData(MetaFileReaderBase reader)
        {
            _reader = reader;
        }

        public virtual void Write(string targetFileOrFolder, bool zipFiles)
        {

        }

        protected string GetTargetFolder(string targetFileOrFolder, bool zipFiles)
        {
            string folder = targetFileOrFolder;

            if (zipFiles)
            {
                if (!Path.HasExtension(targetFileOrFolder))
                    throw new Exception(StringResource.FileNameErr);

                folder = Path.GetDirectoryName(targetFileOrFolder);
            }
            else
            {
                if (Path.HasExtension(targetFileOrFolder))
                    throw new Exception(StringResource.FileNameErr);

                folder += "\\";
            }

            return folder;
        }

        protected void WriteProgress(StorageGroup grp, int percent)
        {
            if (OnProgress != null && percent <= 100)
                OnProgress(grp, percent);
        }

        protected void ZipFiles(string targetFileOrFolder, string basePath)
        {
            ZipFile zfile = ZipFile.Create(targetFileOrFolder);
            zfile.NameTransform = new ZipNameTransform(basePath);
                      
            int percent = 0;

            int pos = 1;
            WriteProgress(null, 0);
            foreach (string file in _filesToZip)
            {
                zfile.BeginUpdate();
                zfile.Add(file);
                zfile.CommitUpdate();
                percent = (pos * 100) / _filesToZip.Count;
                WriteProgress(null, percent);
                pos++;
            }
            zfile.Close();

            foreach (string file in _filesToDelete)
                File.Delete(file);
        }
    }
}
