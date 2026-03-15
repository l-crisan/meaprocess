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

using Mp.Visual.Oscilloscope;


namespace Mp.Runtime.Sdk
{
    internal class OscilloscopePS : ProcessStation
    {
        private OscilloscopeView   _oscilloscope;
        private Signal          _ch1Signal;
        private Signal          _ch2Signal;
        private Signal          _ch1TriggerSig;
        private Signal          _ch2TriggerSig;

        public OscilloscopePS()
        {            
        }        

        public override void OnStart()
        {
            _oscilloscope.Start();
            base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStop();
            _oscilloscope.Stop();
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            _oscilloscope.SynchMutex.WaitOne();

            List<Signal> source = GetSource(portIdx, sourceIdx);

            int recSize = GetSourceSize(portIdx, sourceIdx);
            for (int rec = 0; rec < records; ++rec)
            {
                for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; ++sigIdxInSrc)
                {
                    if (portIdx == 0)
                    {
                        Signal signal = source[sigIdxInSrc];

                        if (signal == _ch1Signal)
                        {
                            double value = ExtractValue(data, portIdx, sigIdxInSrc, sourceIdx, rec * recSize, true);
                            _oscilloscope.AddValue(0, value, rec == records - 1);
                        }
                        else if (signal == _ch2Signal)
                        {
                            double value = ExtractValue(data, portIdx, sigIdxInSrc, sourceIdx, rec * recSize, true);
                            _oscilloscope.AddValue(1, value, rec == records - 1);
                        }
                    }
                    else
                    {//trigger
                        Signal signal = source[sigIdxInSrc];

                        if (signal == _ch1TriggerSig)
                        {
                            double value = ExtractValue(data, portIdx, sigIdxInSrc, sourceIdx, rec * recSize, true);
                            _oscilloscope.AddTriggerValue(0, value, rec == records - 1);
                        }
                        else if (signal == _ch2TriggerSig)
                        {
                            double value = ExtractValue(data, portIdx, sigIdxInSrc, sourceIdx, rec * recSize, true);
                            _oscilloscope.AddTriggerValue(1, value, rec == records - 1);
                        }
                    }
                }
            }
            _oscilloscope.SynchMutex.ReleaseMutex();
        }

        protected override void OnSetupTheControls()
        {
            SignalList signalList = GetPortSignals(0);
            _ch1Signal = signalList[0];
            
            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            _oscilloscope = (OscilloscopeView)Controls[0];
            _oscilloscope.Chn1SampleRate = _ch1Signal.SampleRate;
            _oscilloscope.Ch1Name = _ch1Signal.Name;


            if (signalList.Count > 1)
            {
                _ch2Signal = signalList[1];
                _oscilloscope.Chn2SampleRate = _ch2Signal.SampleRate;
                _oscilloscope.Ch2Name = _ch2Signal.Name;
            }


            signalList = GetPortSignals(1);

            if (signalList != null)
            {
                if (signalList.Count == 1)
                {
                    _ch1TriggerSig = signalList[0];
                    _oscilloscope.Chn1TriggerSampleRate = _ch1TriggerSig.SampleRate;
                }

                if (signalList.Count == 2)
                {
                    _ch1TriggerSig = signalList[0];
                    _ch2TriggerSig = signalList[1];
                    _oscilloscope.Chn1TriggerSampleRate = _ch1TriggerSig.SampleRate;
                    _oscilloscope.Chn2TriggerSampleRate = _ch2TriggerSig.SampleRate;
                }
            }
        }
    }
}
