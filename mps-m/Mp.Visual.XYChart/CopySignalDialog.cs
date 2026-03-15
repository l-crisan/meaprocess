//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
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
using System.Threading;
using System.IO;

namespace Mp.Visual.XYChart
{
    public partial class CopySignalDialog : Form
    {
        private bool _canClose = false;
        private Thread _thread;
        private int _percent = 0;
        private System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer();
//        private RefCurveEditorDlg.CurveInfo _cvi;
        public double[] SignalData;
        
        public CopySignalDialog()
        {
//            _cvi = cvi;

            _thread = new Thread(new ThreadStart(Run));

            InitializeComponent();
            _timer.Interval = 300;
            _timer.Tick += new EventHandler(OnUpdateGui);
            _timer.Start();
            _thread.Start();
        }

        private void OnUpdateGui(object sender, EventArgs e)
        {
            progressBar.Value = _percent;
            if (_canClose)
                Close();
        }
    

        private void DistributeDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !_canClose;
        }

        private void Run()
        {
            /*
            Mp.Drv.DataFile.Signal sig = _cvi.Signal;
            double data = 0.0;
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            long records = _cvi.StgInfo.GetNoOfRecords(_cvi.BasePath);
            SignalData = new double[records];

            _percent = 0;

            int recordSize = _cvi.StgInfo.RecordSize;

            using (FileStream dataFile = new FileStream(Path.Combine(_cvi.BasePath, _cvi.StgInfo.DataFile), FileMode.Open))
            {
                BinaryReader br = new BinaryReader(dataFile);

                for (long i = 0; i < records; ++i)
                {
                    br.BaseStream.Seek(i * recordSize + _cvi.Signal.OffsetInRecord, SeekOrigin.Begin);
                    switch (_cvi.Signal.DataType)
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
                    SignalData[i] = data;
                    _percent = (int)(i * 100 / records);
                }
            }
            */
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