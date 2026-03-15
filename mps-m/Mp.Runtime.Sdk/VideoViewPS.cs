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
using Mp.Visual.Video;

namespace Mp.Runtime.Sdk
{
    internal class VideoViewPS : ProcessStation
    {
        private VideoViewCtrl _view;
        private Signal _videoSignal;
        private int _objSize;

        public VideoViewPS()
        {

        }

        public override void OnStart()
        {
            _view.Start();
            base.OnStart();
        }

        public override void OnStop()
        {
            _view.Stop();
            base.OnStart();
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            Signal signal;

            List<Signal> source = GetSource(0, sourceIdx);
            int srcSize = GetSourceSize(0, sourceIdx);
            int lastRecord = srcSize * (records - 1);

            for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; sigIdxInSrc++)
            {
                signal = source[sigIdxInSrc];

                if (signal != _videoSignal)
                    continue;

                byte[] _videoObj = ExtractObject(data, 0, sigIdxInSrc, sourceIdx, lastRecord, _objSize);
                _view.UpdateStream(_videoObj);
            }
        }

        protected override void OnSetupTheControls()
        {
            _view = (VideoViewCtrl)Controls[0];

            _videoSignal = GetPortSignals(0)[0];

            Hashtable paramDef = new Hashtable();
            string parameters = XmlHelper.GetParam(_videoSignal.XmlRep, "parameters");
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


                _view.VideoWidth = (int)Convert.ToInt32(paramDef["width"]);
                _view.VideoHeight = (int)Convert.ToInt32(paramDef["height"]);
                _view.ImagePixelFormat = (VideoViewCtrl.HwPixelFormat)(int)Convert.ToInt32(paramDef["pixelFormat"]);
                _objSize = (int)XmlHelper.GetParamNumber(_videoSignal.XmlRep, "objSize");
            }
        }
    }
}
