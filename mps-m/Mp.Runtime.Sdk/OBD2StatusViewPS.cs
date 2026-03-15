using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;

using Mp.Visual.OBD2;

namespace Mp.Runtime.Sdk
{
    internal class OBD2StatusViewPS : ProcessStation
    {
        private OBD2StatusView _view;
        private Timer _updateTimer = new Timer();
        private List<double> _data = new List<double>();

        public OBD2StatusViewPS()
        {        
            _updateTimer.Tick += new EventHandler(OnReadData);
            _updateTimer.Interval = 1000;
            
        }

        private void OnReadData(object sender, EventArgs e)
        {
            SignalList signalList = GetPortSignals(0);

            for (int i = 0; i < signalList.Count; ++i)
            {
                Signal signal = signalList[i];
                _view.SetValue(signal.SignalID, _data[i]);
            }
        }

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
                _data[signal.SignalIndex] = value;
            }
        }

        protected override void OnSetupTheControls()
        {
            _view = (OBD2StatusView)Controls[0];

            SignalList signalList = GetPortSignals(0);
            foreach (Signal sig in signalList)
                _data.Add(0.0);

            
            for (int i = 0; i < XmlRep.ChildNodes.Count; ++i)
            {
                XmlElement xmlElement = (XmlElement)XmlRep.ChildNodes[i];

                if (!xmlElement.HasAttribute("name"))
                    continue;

                string name = xmlElement.GetAttribute("name");

                if (name != "signalTypeMap")
                    continue;

                string[] array = xmlElement.InnerText.Split(new char[] { '/' });
                uint sigId = Convert.ToUInt32(array[0]);

                foreach (Signal signal in signalList)
                {
                    if (sigId == signal.SignalID)
                    {
                        _view.AddSignal(signal.Name, sigId, (OBD2StatusView.StatusType)Convert.ToInt32(array[1]));
                        break;
                    }
                }                
            }
            _view.InitDone();
        }
    }
}
