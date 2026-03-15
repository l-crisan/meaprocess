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
using System.Collections;
using Mp.Utils;

namespace Mp.Runtime.Sdk
{
    /// <summary>
    /// This class is the base class for a signal based process station.
    /// </summary>
    /// <remarks>
    /// A signal based process station is a process station which each control is attached to a signal.
    /// Override OnUpdateControlValue to update the control value.
    /// </remarks>
    public class SignalBasedPS : ProcessStation
    {
        private Timer _updateTimer = new Timer();
        private List<byte[]> _data = new List<byte[]>();
        private Hashtable _controlToSignal = new Hashtable();
        private Hashtable _signalToControl = new Hashtable();

        /// <summary>
        /// Constructs a new signal based process station.  
        /// </summary>
        /// <param name="dataReadInterval">The data read interval in milli seconds.</param>
        protected SignalBasedPS(int dataReadInterval)
        {
            _updateTimer.Interval = dataReadInterval;
            _updateTimer.Tick += new EventHandler(OnReadData);
        }

        private void OnReadData(object sender, EventArgs e)
        {
            for (int srcIdx = 0; srcIdx < GetSourceCount(0); srcIdx++)
            {
                List<Signal> source = GetSource(0, srcIdx);
                byte[] record = _data[srcIdx];

                for (int sig = 0; sig < source.Count; sig++)
                {                                
                    double value = ExtractValue(record, 0,  sig, srcIdx, 0, true);
                    
                    Signal signal = source[sig];        
                    Control control = GetControlForSignal(signal);
                    OnUpdateControlValue(control, value, signal);        
                }
            }            
        }
        /// <summary>
        /// Called by the framework to notify the process station to update the control value.
        /// </summary>
        /// <param name="control">The control to update.</param>
        /// <param name="value">The value.</param>
        /// <param name="signal">The signal.</param>
        protected virtual void OnUpdateControlValue(Control control, double value, Signal signal)
        { }

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

        public Signal GetSignalForControl(Control ctrl)
        {
            return (Signal)_controlToSignal[ctrl];
        }

        public Control GetControlForSignal(Signal signal)
        {
            return (Control)_signalToControl[signal.SignalID];
        }

        protected override void OnSetupTheControls()
        {
            //Setup Control to Signal mapping
            foreach (SortedList<uint, List<Signal>> source in Sources)
            {
                foreach (List<Signal> signalList in source.Values)
                {
                    foreach (Signal signal in signalList)
                    {
                        foreach (Control control in Controls)
                        {
                            ControlData ctrlData = control.Tag as ControlData;

                            if (ctrlData.SignalId == signal.SignalID)
                            {
                                _controlToSignal[control] = signal;
                                _signalToControl[signal.SignalID] = control;
                            }
                        }
                    }
                }
            }
        
            //Setup the data array for sources.            
            for (int srcIdx = 0; srcIdx < GetSourceCount(0); srcIdx++)
                _data.Add(new byte[GetSourceSize(0, srcIdx)]);
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            Array.Copy(data, (records - 1) * _data[sourceIdx].Length, _data[sourceIdx], 0, _data[sourceIdx].Length);
        }
    }
}
