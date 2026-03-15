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

using Mp.Visual.Digital;

namespace Mp.Runtime.Sdk
{
    internal class NumericViewPS : ProcessStation
    {
        private Visual.Digital.TableView _numericView;

        public NumericViewPS()
        {
        }

        protected override void  OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            double value;
            Signal signal;

            List<Signal> source = GetSource(portIdx, sourceIdx);
            int srcSize = GetSourceSize(portIdx, sourceIdx);
            int lastRecord = srcSize * (records - 1);

            for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; sigIdxInSrc++)
            {
                signal = source[sigIdxInSrc];
                value = ExtractValue(data, portIdx,sigIdxInSrc, sourceIdx, lastRecord, true);
                _numericView.SetValue(signal.SignalIndex, value);
            }
        }

        protected override void OnSetupTheControls()
        {
            _numericView = (Visual.Digital.TableView)Controls[0];

            Visual.Digital.TableView.Signal numericViewSignal;
            SignalList sigList = GetPortSignals(0);

            foreach (Signal signal in sigList)
            {
                numericViewSignal = new TableView.Signal();

                numericViewSignal.Unit = signal.Unit;
                numericViewSignal.Name = signal.Name;

                _numericView.AddSignal(numericViewSignal);
            }

            _numericView.InitDone();
        }

        public override void OnSaveControlsStates()
        {
        }
    }
}
