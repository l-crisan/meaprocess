using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using Mp.Utils;
using Mp.Visual.OBD2;

namespace Mp.Runtime.Sdk
{
    internal class OBD2DTCViewPS : ProcessStation
    {
        private OBD2DTCView _view;

        public OBD2DTCViewPS()
        {

        }
        public override void OnStart()
        {
            _view.Clear();
            base.OnStart();
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            double value;
            Signal signal;

            List<Signal> source = GetSource(0, sourceIdx);
            int srcSize = GetSourceSize(0, sourceIdx);
            int lastRecord = srcSize * (records - 1);

            for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; sigIdxInSrc++)
            {
                signal = source[sigIdxInSrc];
                value = ExtractValue(data, 0, sigIdxInSrc, sourceIdx, lastRecord, true);
                _view.AddValue(value, signal.Name);
            }
        }

        protected override void OnSetupTheControls()
        {
            _view = (OBD2DTCView)Controls[0];
        }
    }
}
