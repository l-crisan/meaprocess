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
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Mp.Visual.WaveChart;
using Mp.Utils;

namespace Mp.Runtime.Sdk
{
    internal class WaveChartPS : ProcessStation
    {
        public WaveChartPS()
        {
        }

        public override void OnStart()
        {
            _waveChart.Start();
            base.OnStart();
            
        }

        public override void OnStop()
        {
            base.OnStop();
            _waveChart.Stop();
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            List<Signal> source = GetSource(0, sourceIdx);

            double value;
            Signal signal;

            int recSize = GetSourceSize(0, sourceIdx);
            _waveChart.SynchMutex.WaitOne();

            for (int rec = 0; rec < records; ++rec)
            {
                for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; ++sigIdxInSrc)
                {
                    signal = source[sigIdxInSrc];                    
                    value = ExtractValue(data, 0, sigIdxInSrc, sourceIdx, rec * recSize, true);                    
                    _waveChart.SetValue(signal.SignalIndex, value);
                }
            }
            _waveChart.SynchMutex.ReleaseMutex();
        }

        protected override void OnSetupTheControls()
        {
            SigLegend legend = null;
            TimeAxis timeAxis = null;
            YAxis yAxis = null;

            foreach( Control ctrl in Controls )
            {
                if (ctrl as WaveChartCtrl != null)
                    _waveChart = (WaveChartCtrl)ctrl;

                if (ctrl as SigLegend != null) 
                    legend = (SigLegend)ctrl;

                if (ctrl as TimeAxis != null)
                    timeAxis = (TimeAxis)ctrl;

                if (ctrl as YAxis != null)
                    yAxis = (YAxis) ctrl;
            }

            _waveChart.Legend = legend;
            _waveChart.TimeAxis = timeAxis;
            _waveChart.YAxis  = yAxis;

            Mp.Visual.WaveChart.Signal chartSignal;
            SignalList signalList = GetPortSignals(0);
            foreach (Signal signal in signalList)
            {
                chartSignal = new Mp.Visual.WaveChart.Signal();
                
                chartSignal.Samplerate       = signal.SampleRate;
                chartSignal.InitialMinimum   = signal.Minimum;
                chartSignal.InitialMaximum   = signal.Maximum;
                chartSignal.Comment          = signal.Comment;
                chartSignal.Unit             = signal.Unit;
                chartSignal.Name             = signal.Name;

                _waveChart.Add( chartSignal );
            }

            _waveChart.InitDone();

            RestoreLegendState();
        }

        private void RestoreLegendState()
        {
            ControlData ctrlData = (ControlData) _waveChart.Legend.Tag;

            if (ctrlData.ControlState == null)
                return;

            if (ctrlData.ControlState == "")
                return;

            string [] strSignals =  ctrlData.ControlState.Split('\n');

            if (strSignals.Length == 0)
                return;

            _waveChart.MaxSampleRate = Convert.ToInt32(strSignals[0]);
            Mp.Visual.WaveChart.Signal signal;

            for (int i = 1; i < strSignals.Length; ++i)
            {
                string strSignal = strSignals[i];

                if (strSignal == "")
                    continue;

                byte[] sigData = Convert.FromBase64String(strSignal);
                MemoryStream stream = new MemoryStream(sigData);
                BinaryFormatter formater = new BinaryFormatter();
                signal = (Mp.Visual.WaveChart.Signal)formater.Deserialize(stream);
                SynchronizeLegendSignal(signal);
            }

            _waveChart.Legend.UpdateLegend();
        }
        
        private void SynchronizeLegendSignal(Mp.Visual.WaveChart.Signal sig)
        {
            foreach (Mp.Visual.WaveChart.Signal legendSignal in _waveChart.Legend.Signals)
            {
                if( legendSignal.Name == sig.Name) 
                {
                    legendSignal.LineColor = sig.LineColor;
                    legendSignal.LineWidth = sig.LineWidth;
                    legendSignal.PointColor = sig.PointColor;
                    legendSignal.PointSize = sig.PointSize;
                    legendSignal.PointsVisible = sig.PointsVisible;
                    legendSignal.Visible = sig.Visible;
                    legendSignal.YAxisDivision = sig.YAxisDivision;
                    legendSignal.YAxisPrecision = sig.YAxisPrecision;
                    legendSignal.Maximum = sig.Maximum;
                    legendSignal.Minimum = sig.Minimum;
                    return;
                }
            }
        }

        public override void OnSaveControlsStates()
        {
            //Save the wave chart legend signal settings in the legend control data.
            ControlData ctrlData = (ControlData)_waveChart.Legend.Tag;
            ctrlData.ControlState = _waveChart.MaxSampleRate.ToString() + "\n";

            _waveChart.Clear();
            
            foreach (Mp.Visual.WaveChart.Signal sig in _waveChart.Legend.Signals)
            {
                MemoryStream st = new MemoryStream();
                BinaryFormatter formater = new BinaryFormatter();

                formater.Serialize(st, sig);

                st.Flush();
                st.Seek(0, 0);

                byte[] buffer = new byte[st.Length];

                st.Read(buffer, 0, (int)st.Length);
                st.Close();

                ctrlData.ControlState += Convert.ToBase64String(buffer);
                ctrlData.ControlState += "\n";
            }
        }

        public WaveChartCtrl _waveChart;
    }
}
