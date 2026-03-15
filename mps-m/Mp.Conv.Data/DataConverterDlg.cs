using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Mp.Drv.DataFile;

namespace Mp.Conv.Data
{
    public partial class DataConverterDlg : Form
    {
        private MetaFileReaderBase _reader = null;
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
        private bool _running = false;
        private string _error = "";
        private bool _targetIsFolder = false;

        public DataConverterDlg()
        {
            InitializeComponent();
            _timer.Interval = 500;
            _timer.Tick += new EventHandler(OnUpdate);
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            if (!_running)
            {
                _timer.Stop();
                close.Enabled = true;
                convert.Enabled = true;

                if(_error != "")
                    MessageBox.Show(_error, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);

                _error = "";
            }
        }

        private void srcBt_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "*.mmf|*.mmf|*.tdm|*.tdm";
            dlg.CheckFileExists = true;

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            source.Text = dlg.FileName;
            string fext = Path.GetExtension(source.Text).ToUpper();
            if (fext == ".tdm".ToUpper())
            {
                mmf.Checked = true;
                mmf.Enabled = true;
                csv.Enabled = false;
                csvzip.Enabled = false;
                tdm.Enabled = false;
                tdmzip.Enabled = false;
                mdf.Enabled = false;
                mdfzip.Enabled = false;
            }
            else
            {
                if (mmf.Checked)
                    csv.Checked = true;

                mmf.Enabled = false;
                csv.Enabled = true;
                csvzip.Enabled = true;
                tdm.Enabled = true;
                tdmzip.Enabled = true;
                mdf.Enabled = true;
                mdfzip.Enabled = true;
            }
            
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void targetBt_Click(object sender, EventArgs e)
        {
            if (mmf.Checked || csv.Checked || tdm.Checked)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog() == DialogResult.Cancel)
                    return;

                target.Text = dlg.SelectedPath;
                _targetIsFolder = true;
            }            
            else
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.CheckPathExists = true;
                dlg.CheckFileExists = false;

                if( mdf.Checked)
                    dlg.Filter = "*.mdf|*.mdf";
                else
                    dlg.Filter = "*.zip|*.zip";

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                target.Text = dlg.FileName;
                _targetIsFolder = false; ;
            }

            if (source.Text != "" && target.Text != "")
                convert.Enabled = true;
            else
                convert.Enabled = false;
        }

        private void source_TextChanged(object sender, EventArgs e)
        {
            if (source.Text != "" && target.Text != "")
                convert.Enabled = true;
            else
                convert.Enabled = false;
        }

        private void target_TextChanged(object sender, EventArgs e)
        {
            if (source.Text != "" && target.Text != "")
                convert.Enabled = true;
            else
                convert.Enabled = false;
        }


        private void Run()
        {
            try
            {

                _reader.Group(new OutputData.ProgressDelegate(OnProgress));

                if (mmf.Checked)
                {
                    MMFDataWriter writer = new MMFDataWriter((TDMMetaFileReader)_reader);
                    writer.OnProgress += new OutputData.ProgressDelegate(OnProgress);
                    writer.Write(target.Text, false);
                }
                
                if (csv.Checked)
                {
                    CSVDataWriter writer = new CSVDataWriter(_reader);
                    writer.Precision = Convert.ToInt32(precision.Text);
                    writer.OnProgress += new OutputData.ProgressDelegate(OnProgress);
                    writer.Write(target.Text, false);
                }

                if (csvzip.Checked)
                {
                    CSVDataWriter writer = new CSVDataWriter(_reader);
                    writer.OnProgress += new OutputData.ProgressDelegate(OnProgress);
                    writer.Write(target.Text, true);
                }

                if (tdm.Checked)
                {
                    TDMDataWriter writer = new TDMDataWriter(_reader);
                    writer.OnProgress += new OutputData.ProgressDelegate(OnProgress);
                    writer.Write(target.Text, false);
                }

                if (tdmzip.Checked)
                {
                    TDMDataWriter writer = new TDMDataWriter(_reader);
                    writer.OnProgress += new OutputData.ProgressDelegate(OnProgress);
                    writer.Write(target.Text, true);
                }

                if (mdf.Checked)
                {
                    MDFDataWriter writer = new MDFDataWriter(_reader);
                    writer.OnProgress += new OutputData.ProgressDelegate(OnProgress);
                    writer.Write(target.Text, false);
                }

                if (mdfzip.Checked)
                {
                    MDFDataWriter writer = new MDFDataWriter(_reader);
                    writer.OnProgress += new OutputData.ProgressDelegate(OnProgress);
                    writer.Write(target.Text, true);
                }

                _reader.CleanUp();
            }
            catch (Exception e)
            {
                _error = e.Message;
            }

            _running = false;
        }

        private void convert_Click(object sender, EventArgs e)
        {
           try
           {
               group.Text = "0";
               progress.Value = 0;

               Thread thread = new Thread(new ThreadStart(Run));

               if (mmf.Checked)
                   _reader = new TDMMetaFileReader();
               else
                   _reader = new MMFMetaFileReader();

               _reader.Read(source.Text);
               _running = true;
               close.Enabled = false;
               convert.Enabled = false;
               _timer.Start();
               thread.Start();               
           }
           catch(Exception ex)
           {
               MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
           }
        }
        
        private delegate void UpdateStateDelegate(StorageGroup grp, int per);

        private void UpdateState(StorageGroup grp, int per)
        {
            if (grp != null)
                group.Text = (grp.Number + 1).ToString();
            else
                group.Text = "zipping files...";

            progress.Value = per;
            percent.Text = per.ToString() + " %";
        }

        private void OnProgress(StorageGroup grp, int per)
        {
            this.Invoke(new UpdateStateDelegate(UpdateState), new object[] { grp, per });
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if(_targetIsFolder && (mdf.Checked || csvzip.Checked || tdmzip.Checked  || mdfzip.Checked))
            {
                target.Text = "";
            }
            
            if( !_targetIsFolder && ( csv.Checked || tdm.Checked || mmf.Checked))
            {
                target.Text = "";
            }

            if (source.Text != "" && target.Text != "")
                convert.Enabled = true;
            else
                convert.Enabled = false;

            precision.Enabled = csvzip.Checked || csv.Checked;
        }

        private void precision_Validating(object sender, CancelEventArgs e)
        {
            errorProvider.Clear();
            try
            {
                Convert.ToUInt16(precision.Text);
            }
            catch (Exception ex)
            {
                errorProvider.SetError(precision, ex.Message);
                e.Cancel = true;
            }
        }
    }
}
