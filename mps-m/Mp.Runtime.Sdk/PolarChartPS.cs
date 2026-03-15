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
using Mp.Visual.PolarChart;

namespace Mp.Runtime.Sdk
{
    internal class PolarChartPS : ProcessStation
    {
        private Mp.Visual.PolarChart.PolarChart _view;
        private double[] _data;
        private bool[] _updateMask;
        private System.Windows.Forms.Timer _updateTimer = new System.Windows.Forms.Timer();
        private Mutex _mutex = new Mutex();
        private Signal _radiusSignal;
        private Signal _angleSignal;

        public PolarChartPS()
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
                _view.SetValue(_data[0], _data[1]);
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
                if (!b)
                    return false;
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
                if (signal == _radiusSignal)
                {
                    _data[0] = ExtractValue(data, portIdx, sigIdxInSrc, sourceIdx, lastRecord, true);
                    _updateMask[0] = true;
                }

                if( signal == _angleSignal)
                {
                    _data[1] = ExtractValue(data, portIdx, sigIdxInSrc, sourceIdx, lastRecord, true);
                    _updateMask[1] = true;
                }
            }

            _mutex.ReleaseMutex();
        }

        protected override void OnSetupTheControls()
        {
            _view = (PolarChart)Controls[0];

            SignalList sigList = GetPortSignals(0);
            _data = new double[2];
            _updateMask = new bool[2];

            _radiusSignal = sigList[0];
            _angleSignal = sigList[1];
            _view.RadiusMinimum = _radiusSignal.Minimum;
            _view.RadiusMaximum = _radiusSignal.Maximum;
        }
    }
}
