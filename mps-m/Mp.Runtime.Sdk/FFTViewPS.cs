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

namespace Mp.Runtime.Sdk
{
    internal class FFTViewPS : ProcessStation
    {
        private Visual.XYChart.Chart _chart;
        private double[] _xData;
        private double[] _yData;
        private Signal _xSignal;
        private Signal _ySignal;
        private int _seqXPos;
        private int _seqYPos;
        private int _visiblePoints = 0;
    
        public FFTViewPS()
        {
        }

        public override void OnStart()
        {
            _chart.Reset();
            _seqXPos = 0;
            _seqYPos = 0;
            base.OnStart();
        }

        public override void OnStop()
        {
            for (int i = 0; i < _xData.Length; ++i)
            {
                _xData[i] = 0;
                _yData[i] = 0;
            }
            base.OnStop();
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            List<Signal> source = GetSource(0, sourceIdx);

            int recSize = GetSourceSize(0, sourceIdx);
            _chart.SynchMutex.WaitOne();
            try
            {
                for (int rec = 0; rec < records; ++rec)
                {
                    double value;
                    Signal signal;

                    for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; ++sigIdxInSrc)
                    {
                        signal = source[sigIdxInSrc];

                        value = ExtractValue(data, 0, sigIdxInSrc, sourceIdx, rec * recSize, true);

                        if (signal == _xSignal)
                        {
                            if (value == 0)
                            {
                                _seqXPos = 0;
                                _seqYPos = 0;
                            }

                            _xData[_seqXPos] = value;
                            _seqXPos++;
                        }

                        if (signal == _ySignal)
                        {
                            if (_seqYPos < _yData.Length)
                                _yData[_seqYPos] = value;
                            
                            _seqYPos++;
                        }

                        if (_seqXPos == _visiblePoints && _seqYPos == _visiblePoints)
                        {
                            _chart.Clear();

                            for (int i = 0; i < _xData.Length; ++i)
                                _chart.SetValue(_xData[i], _yData[i]);

                            _seqXPos = 0;
                            _seqYPos = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            _chart.SynchMutex.ReleaseMutex();
        }

        protected override void OnSetupTheControls()
        {
            SignalList signalList = GetPortSignals(0);
            _xSignal = signalList[0];
            _ySignal = signalList[1];
            _chart = (Visual.XYChart.Chart)Controls[0];
            _chart.ShowControlPanel = false;

            if (_xSignal.Unit != "")
                _chart.XText = _xSignal.Name + "(" + _xSignal.Unit + ")";
            else
                _chart.XText = _xSignal.Name;


            if (_ySignal.Unit != "")
                _chart.XText = _ySignal.Name + "(" + _ySignal.Unit + ")";
            else
                _chart.XText = _ySignal.Name;


            _visiblePoints = _chart.NoOfPoints / 2 + 1;
            if (_visiblePoints == 0)
                _visiblePoints = 1;

            _xData = new double[_visiblePoints];
            _yData = new double[_visiblePoints];

            _chart.EnableRefCurveEdit = false;
            _chart.Reset();
        }

    }
}
