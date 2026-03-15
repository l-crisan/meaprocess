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
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Mp.Visual.Digital;
using System.Threading;

namespace Mp.Runtime.Sdk
{
    internal class SignalListViewPS : ProcessStation
    {
        private Visual.Digital.ListView _numericView;
        private double[] _data;
        private bool[] _updateMask;
        private System.Windows.Forms.Timer _updateTimer = new System.Windows.Forms.Timer();
        private Mutex _mutex = new Mutex();

        public SignalListViewPS()
        {
            _updateTimer.Interval = 100;
            _updateTimer.Tick += new EventHandler(OnUpdate);
        }

        public override void OnStart()
        {
            _updateTimer.Start();
            base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStop();
            _updateTimer.Stop();
        }

        private void OnUpdate(object sender, EventArgs e)
        {
            _mutex.WaitOne();

            if (IsRecordFull())
            {
                _numericView.AddValue(_data);
                ClearRecord();
            }

            _mutex.ReleaseMutex();            
        }


        private void ClearRecord()
        {
            for (int i = 0; i < _updateMask.Length; ++i)
                _updateMask[i] = false;
        }

        private bool IsRecordFull()
        {
            foreach (bool b in _updateMask)
            {
                if (b == false)
                {
                    return false;
                }
            }

            return true;
        }
        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            _mutex.WaitOne();

            Signal signal;

            List<Signal> source = GetSource(portIdx, sourceIdx);
            int srcSize = GetSourceSize(portIdx, sourceIdx);
            int lastRecord = srcSize * (records - 1);

            for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; sigIdxInSrc++)
            {
                signal = source[sigIdxInSrc];
                _data[signal.SignalIndex] = ExtractValue(data, portIdx, sigIdxInSrc, sourceIdx, lastRecord, true);
                _updateMask[signal.SignalIndex] = true;
            }

            _mutex.ReleaseMutex();
        }

        protected override void OnSetupTheControls()
        {
            _numericView = (Visual.Digital.ListView)Controls[0];

            Visual.Digital.ListView.Signal numericViewSignal;
            SignalList sigList = GetPortSignals(0);
            _data = new double[sigList.Count];
            _updateMask = new bool[sigList.Count];

            foreach (Signal signal in sigList)
            {
                numericViewSignal = new Visual.Digital.ListView.Signal();

                numericViewSignal.Unit = signal.Unit;
                numericViewSignal.Name = signal.Name;

                _numericView.AddSignal(numericViewSignal);
            }

            _numericView.InitDone();
        }
    }
}
