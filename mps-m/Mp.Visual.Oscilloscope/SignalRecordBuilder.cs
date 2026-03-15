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

namespace Mp.Visual.Oscilloscope
{
    internal delegate void NewValueDelegate(double[] values);
  
    internal class SignalRecordBuilder
    {
        private int[]     _writeIndex;
        private int       _readIndex;
        private double[,] _values;
        private double[]  _samples;
        private bool[]    _looped;
        private double[]  _rates;
        private int _totalSamples = 0;
        public  event NewValueDelegate OnNewValue;
        
        public SignalRecordBuilder(int channels)
        {
            _writeIndex = new int[channels];            
            _samples = new double[channels];
            _looped = new bool[channels];                        
        }

        public void Start(double[] rates)
        {
            _rates = new double[rates.Length];
            
            for( int i = 0; i< rates.Length; ++i)
                _rates[i] = rates[i];

            Reset();
        }

        public void Reset()
        {
            if (_rates == null)
                return;

            _totalSamples = 0;

            for (int i = 0; i < _writeIndex.Length; ++i)
            {
                _totalSamples = (int)Math.Max(_totalSamples, _rates[i]);
                _writeIndex[i] = 0;
                _looped[i] = false;
                _samples[i] = 0;
            }

            _readIndex = 0;
            _values = new double[_totalSamples, _writeIndex.Length];
        }

        private bool IsLooped()
        {
            for (int i = 0; i < _looped.Length; ++i)
            {
                if (!_looped[i])
                    return false;
            }
            return true;
        }

        private void UpdateValues()
        {
            //Extract the first part
            int writeIndex = _totalSamples + 1;

            for (int i = 0; i < _looped.Length; ++i)
            {
                if (_looped[i])
                    writeIndex = Math.Min(writeIndex, _totalSamples);
                else
                    writeIndex = Math.Min(writeIndex, _writeIndex[i]);
            }

            if (writeIndex != _readIndex)
            {
                if (OnNewValue != null)
                {
                    for (int i = _readIndex; i < writeIndex; ++i)
                    {
                        double[] record = new double[_writeIndex.Length];

                        for (int j = 0; j < _writeIndex.Length; ++j)
                            record[j] = _values[i, j];

                        OnNewValue(record);
                    }
                }

                _readIndex = writeIndex;
            }

            //Extract the second part
            if (IsLooped())
            {//all looped
                //Determinate the minimum
                writeIndex = _writeIndex[0];

                for(int i = 0; i < _writeIndex.Length; ++i)
                    writeIndex = Math.Min(writeIndex, _writeIndex[1]);

                if (writeIndex == 0)
                    return;

                for( int i = 0; i < _looped.Length; ++i)
                    _looped[i] = false;

                _readIndex = 0;
                if (OnNewValue != null)
                {
                    for (int i = _readIndex; i < writeIndex; ++i)
                    {
                        double[] record = new double[_writeIndex.Length];

                        for (int j = 0; j < _writeIndex.Length; ++j)
                            record[j] = _values[i, j];

                        OnNewValue(record);
                    }
                }

            }
            _readIndex = writeIndex;
        }

        private void WriteValue(int index, double value)
        {
            _values[_writeIndex[index],index] = value;
            _writeIndex[index]++;

            if (_writeIndex[index] == _totalSamples)
            {
                _writeIndex[index] = 0;
                _looped[index] = true;
            }

            if (_writeIndex[index] == _readIndex)
            {
                _readIndex++;
                if (_readIndex == _totalSamples)
                    _readIndex = 0;
            }
        }     

        public void AddValue(int chnIdx, double value)
        {
            _samples[chnIdx] += _rates[0] / _rates[chnIdx];

            int i = 0;
            for (; i < (int)_samples[chnIdx]; ++i)
                WriteValue(chnIdx, value);

            _samples[chnIdx] -= i;

            UpdateValues();
        }
    }
}
