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
using System.Xml;

namespace Mp.Runtime.Sdk
{
    public abstract class VisualBlockPS : ProcessStation
    {
        private Dictionary<uint, Control> _sig2CtrlMap = new Dictionary<uint, Control>();
        private Visual.Base.VisualBlock _view;

        protected abstract void OnSetControlValue(Control control, double value);
        protected abstract Control OnCreateControl(Signal signal);
        

        public VisualBlockPS()
        {

        }

        public Visual.Base.VisualBlock BlockView
        {
            set{ _view = value;}
            get{ return _view;}
        }

        private void RegisterSignalToControl(uint id, Control ctrl)
        {
            _sig2CtrlMap.Add(id, ctrl);
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

                    if (_sig2CtrlMap.ContainsKey(signal.SignalID))
                        OnSetControlValue(_sig2CtrlMap[signal.SignalID],value);
                }
            }
        }


        protected override void OnSetupTheControls()
        {
            SignalList signalList = GetPortSignals(0);
            BlockView = (Mp.Visual.Base.VisualBlock)base.Controls[0];

            int columns = BlockView.ColumnCount;
            int rows = (int)Math.Ceiling(signalList.Count / (double)columns);
            BlockView.RowCount = rows;

            int col = 0;
            int row = 0;

            foreach (Signal signal in signalList)
            {
                Control ctrl = OnCreateControl(signal);     

                BlockView.AddControl(ctrl);

                RegisterSignalToControl(signal.SignalID, ctrl);

                if (col == columns)
                {
                    col = 0;
                    row++;
                }
            }

            BlockView.UpdateProperties();
        }
    }
}
