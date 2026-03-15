//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2010-2016  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Mp.Utils;

namespace Mp.Scheme.Target.Win
{
    public partial class DistributeDlg : Form
    {
        private bool _canClose = false;
        private string _docFile;
        private Thread _thread;
        private int _percent = 0;
        private string _zipFile;
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();

        public DistributeDlg(string docFile, string zipFile)
        {
            _docFile = docFile;
            _zipFile = zipFile;

            _thread = new Thread(new ThreadStart(Run));

            InitializeComponent();
            _timer.Interval = 300;
            _timer.Tick += new EventHandler(OnUpdateGui);
        }

        private void OnUpdateGui(object sender, EventArgs e)
        {
            progressBar.Value = _percent;
            if (_canClose)
                Close();
        }

        private static void CopyDirectory(string src, string dst)
        {
            string[] files;

            if (dst[dst.Length - 1] != Path.DirectorySeparatorChar)
                dst += Path.DirectorySeparatorChar;

            try
            {
                if (!Directory.Exists(dst))
                    Directory.CreateDirectory(dst);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            files = Directory.GetFileSystemEntries(src);

            foreach (string Element in files)
            {
                if (Directory.Exists(Element))
                    CopyDirectory(Element, dst + Path.GetFileName(Element));
                else
                    File.Copy(Element, dst + Path.GetFileName(Element), true);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = !_canClose;
            base.OnFormClosing(e);
        }


        private void Run()
        {            
            string runtimePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".\\rt");
            runtimePath = Path.GetFullPath(runtimePath);

            string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(_docFile));
            CopyDirectory(runtimePath, tempPath);
            
            string exefile = Path.GetFileName(_docFile);
            exefile = Path.GetFileNameWithoutExtension(exefile);
            exefile += ".exe";
            File.Move(Path.Combine(tempPath,"Mp.Runtime.App.exe"),Path.Combine(tempPath,exefile));
            File.Copy(_docFile, Path.Combine(tempPath, "Mp.Rtf.dll"), true);

            _percent = 0;
            ZipUtil.ZipFiles(tempPath, _zipFile, null, ref _percent);
           
            Directory.Delete(tempPath, true);
            _canClose = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _thread.Start();
            _timer.Start();
        }
    }
}
