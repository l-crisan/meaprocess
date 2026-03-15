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
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Globalization;

using Mp.Visual.WaveChart;


namespace Mp.Runtime.Sdk
{
    internal class XYChartPS : ProcessStation
    {
        private Visual.XYChart.Chart   _chart;
        private Signal          _xSignal;
        private Signal          _ySignal;
        private int[]           _writeIndex = new int[2];
        private int             _readIndex;
        private double[]        _xValue;
        private double[]        _yValue;
        private double[]        _samples = new double[2];
        private bool[]          _looped = new bool[2];
        private bool _lastResetValue = false;
        
        public XYChartPS()
        {            
        }        

        public override void OnStart()
        {
            int samples = (int) Math.Max(_xSignal.SampleRate, _ySignal.SampleRate) * 2;

            _xValue = new double[samples];
            _yValue = new double[samples];

            _writeIndex[0] = 0;
            _writeIndex[1] = 0;
            _readIndex = 0;
            _looped[0] = false;
            _looped[1] = false;
            _samples[0] = 0;
            _samples[1] = 0;
            _lastResetValue = false;
            _chart.Start();
            base.OnStart();
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            List<Signal> source = GetSource(portIdx, sourceIdx);

            int recSize = GetSourceSize(portIdx, sourceIdx);

            for (int rec = 0; rec < records; ++rec)
            {
                double value;
                Signal signal;
                for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; ++sigIdxInSrc)
                {
                    //Extract the value from data stream
                    signal = source[sigIdxInSrc];
                    value = ExtractValue(data, portIdx, sigIdxInSrc, sourceIdx, rec * recSize, true);

                    if (portIdx == 0)
                    {
                        //Write the values
                        if (signal == _xSignal)
                        {//x value
                            if (_xSignal.SampleRate < _ySignal.SampleRate)
                            {
                                _samples[0] += _ySignal.SampleRate / _xSignal.SampleRate;

                                int i = 0;
                                for (; i < (int)_samples[0]; ++i)
                                    WriteXValue(value);

                                _samples[0] -= i;

                            }
                            else
                            {
                                WriteXValue(value);
                                _samples[0] = 0;
                            }
                        }

                        if (signal == _ySignal)
                        {//y value
                            if (_ySignal.SampleRate < _xSignal.SampleRate)
                            {
                                _samples[1] += _xSignal.SampleRate / _ySignal.SampleRate;

                                int i = 0;
                                for (; i < (int)_samples[1]; ++i)
                                    WriteYValue(value);

                                _samples[1] -= i;
                            }
                            else
                            {
                                WriteYValue(value);
                                _samples[1] = 0;
                            }
                        }
                    }
                    else
                    {
                        SignalList resetSignalList = GetPortSignals(1);

                        if (resetSignalList != null)
                        {
                            if (signal == resetSignalList[0])
                            {
                                if (value != 0)
                                {
                                    if (!_lastResetValue)
                                    {
                                        _lastResetValue = true;
                                        _chart.Reset();
                                    }
                                }
                                else
                                {
                                    _lastResetValue = false;
                                }
                            }
                        }
                    }
                }
            }
            UpdateChart();
        }

        private void UpdateChart()
        {
            _chart.SynchMutex.WaitOne();

            //Extract the first part
            int writeIndex = _xValue.Length + 1;

          	for(int i = 0; i < 2; ++i)
            {
                if (_looped[i])
                    writeIndex = Math.Min(writeIndex, _xValue.Length);
                else
                    writeIndex = Math.Min(writeIndex, _writeIndex[i]);	
            }

            if (writeIndex != _readIndex)
            {
                for (int i = _readIndex; i < writeIndex; ++i)
                    _chart.SetValue(_xValue[i], _yValue[i]);

                _readIndex = writeIndex;
            }               

            //Extract the second part
            if (_looped[0] && _looped[1])
            {//all looped

                //Determinate the minimum
                writeIndex = Math.Min(_writeIndex[0], _writeIndex[1]);

                if (writeIndex == 0)
                {
                    _chart.SynchMutex.ReleaseMutex();
                    return;
                }

                _looped[0] = false;
                _looped[1] = false;

                _readIndex = 0;
                for (int i = _readIndex; i < writeIndex; ++i)
                    _chart.SetValue(_xValue[i], _yValue[i]);

            }
            _readIndex = writeIndex;
            _chart.SynchMutex.ReleaseMutex();
        }

        private void WriteYValue(double value)
        {
            _yValue[_writeIndex[1]] = value;
            _writeIndex[1]++;

            if (_writeIndex[1] == _yValue.Length)
            {
                _writeIndex[1] = 0;
                _looped[1] = true;
            }

            if (_writeIndex[1] == _readIndex)
            {
                _readIndex++;
                if (_readIndex == _yValue.Length)
                    _readIndex = 0;
            }            
        }

        private void WriteXValue(double value)
        {
            _xValue[_writeIndex[0]] = value;
            _writeIndex[0]++;

            if (_writeIndex[0] == _xValue.Length)
            {
                _writeIndex[0] = 0;
                _looped[0] = true;
            }

            if (_writeIndex[0] == _readIndex)
            {
                _readIndex++;
                if (_readIndex == _xValue.Length)
                    _readIndex = 0;
            }
        }

        protected override void OnSetupTheControls()
        {
            SignalList signalList = GetPortSignals(0);
            _xSignal = signalList[0];
            _ySignal = signalList[1];
            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            _chart = (Visual.XYChart.Chart)Controls[0];

            if (_xSignal.Unit != "")
                _chart.XText = _xSignal.Name + " (" + _xSignal.Unit + ")";
            else
                _chart.XText = _xSignal.Name;

            if (_ySignal.Unit != "")
                _chart.YText = _ySignal.Name + " (" + _ySignal.Unit + ")";
            else
                _chart.YText = _ySignal.Name;         
            
            _chart.Reset();
        }


    }
}
