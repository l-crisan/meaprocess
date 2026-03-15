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
using System.Collections;
using Mp.Utils;

namespace Mp.Runtime.Sdk
{
    internal class HistogramViewPS : ProcessStation
    {
        private Visual.XYChart.Chart _chart;
        private double[] _xData;
        private double[] _yData;
        private Signal _signal;
        private int _seqPos;
    
        public HistogramViewPS()
        {
        }

        public override void OnStart()
        {
            _chart.Reset();
            _seqPos = 0;
            base.OnStart();
        }

        public override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            
            List<Signal> source = GetSource(0, sourceIdx);

            int recSize = GetSourceSize(0, sourceIdx);
            _chart.SynchMutex.WaitOne();

            for (int rec = 0; rec < records; ++rec)
            {
                double value;
                Signal signal;

                for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; ++sigIdxInSrc)
                {
                    signal = source[sigIdxInSrc];                    
                    if (signal == _signal)
                    {
                        value = ExtractValue(data,0,  sigIdxInSrc, sourceIdx, rec * recSize, true);

                        if (value == uint.MaxValue)
                        {
                            _seqPos = 0;
                        }
                        else
                        {
                            _yData[_seqPos] = value;                                                        
                            _seqPos++;

                            if (_seqPos == _xData.Length)
                            {
                                double ymax = 0;

                                for (int i = 0; i < _xData.Length; ++i)
                                    ymax += _yData[i];

                                if (ymax == 0)
                                    ymax = 1;

                                _chart.Clear();

                                for (int i = 0; i < _xData.Length; ++i)
                                    _chart.SetValue(_xData[i], _yData[i]*100/ymax);

                                _seqPos = 0;
                            }
                        }
                    }
                }
            }

            _chart.SynchMutex.ReleaseMutex();
        }

        protected override void OnSetupTheControls()
        {
            SignalList signalList = GetPortSignals(0);
            _signal = signalList[0];
            _chart = (Visual.XYChart.Chart)Controls[0];
            _chart.ShowControlPanel = false;

            Hashtable  paramDef = new Hashtable();
            string parameters = XmlHelper.GetParam(_signal.XmlRep, "parameters");
            if (parameters != "")
            {
                string[] pars = parameters.Split(';');
                foreach (string p in pars)
                {
                    if (p == "")
                        continue;

                    string[] pv = p.Split('=');

                    string name = pv[0].TrimEnd(' ');
                    name = name.TrimStart(' ');

                    string value = pv[1].TrimEnd(' ');
                    value = value.TrimStart(' ');
                    paramDef.Add(name, value);

                }
            }
            else
            {
                Mp.Runtime.Adaption.Message msg = new Mp.Runtime.Adaption.Message();
                msg.Text = String.Format(StringResource.HistogramSignalErr, _chart.Title);
                msg.Type = Mp.Runtime.Adaption.Message.MessageType.Error;
                msg.TargetType = Mp.Runtime.Adaption.Message.Target.Output;
                msg.TimeStamp = DateTime.Now;
                RuntimeEngine.Instance().OnNewMessage(msg);

                return;
            }
            int points = Convert.ToInt32(paramDef["classes"]);
            _chart.NoOfPoints = points;
            _chart.XMinimum = Convert.ToDouble(paramDef["lower"]);
            _chart.XMaximum = Convert.ToDouble(paramDef["upper"]);
            _chart.YMinimum = 0;
            _chart.YMaximum = 100;

            if (_signal.Unit != "")
                _chart.XText = _signal.Name + "(" + _signal.Unit + ")";
            else
                _chart.XText = _signal.Name;


            _xData = new double[points];
            _yData = new double[points];
            double deltax = _chart.XMaximum - _chart.XMinimum;
            double inc = deltax / _xData.Length;
            double pos = _chart.XMinimum;
            for (int i = 0; i < _xData.Length; ++i)
            {
                _xData[i] = pos + inc/2.0;
                
                pos += inc;
            }

            _chart.EnableRefCurveEdit = false;
            _chart.YText = "%";
            _chart.Reset();
        }


    }
}
