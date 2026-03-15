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
using System.Xml;
using Mp.Visual.GPS;

namespace Mp.Runtime.Sdk
{
    internal class GPSTrackViewPS : ProcessStation
    {
        private TrackViewCtrl _view;
        private Hashtable _signalMapping = new Hashtable();
        
        private enum Channel
        {
            Latitude = 0,
            Longitude = 1,
            Altitude = 2,
            Status = 3,
            Speed = 4,
            Angle = 5,
            Day = 6,
            Month = 7,
            Year = 8,
            Hour = 9,
            Minute = 10,
            Second = 11
        }

        public GPSTrackViewPS()
        {
        }

        public override void OnStart()
        {
            base.OnStart();
            _view.Clear();
        }

        protected override void OnUpdateDataValue(byte[] data, int sourceIdx, int portIdx, int records)
        {
            List<Signal> source = GetSource(0, sourceIdx);

            int recSize = GetSourceSize(0, sourceIdx);
            double latitude = 0;
            double longitude = 0;
            double altitude = 0;
            double status = 0;
            double speed = 0;
            double angle = 0;
            byte day = 0;
            byte month = 0;
            ushort year = 0;
            byte hour = 0;
            byte minute = 0;
            byte second = 0;


            for (int rec = 0; rec < records; ++rec)
            {
                for (int sigIdxInSrc = 0; sigIdxInSrc < source.Count; ++sigIdxInSrc)
                {
                    Signal signal = source[sigIdxInSrc];

                    if (!_signalMapping.ContainsKey(signal.SignalID))
                        continue;

                    Channel channel = (Channel)_signalMapping[signal.SignalID];

                    double value = ExtractValue(data, 0, sigIdxInSrc, sourceIdx, rec * recSize, true);


                    switch (channel)
                    {
                        case Channel.Latitude:
                            latitude = value;
                        break;
                        case Channel.Longitude:
                            longitude = value;
                        break;
                        case Channel.Altitude:
                            altitude = value;
                        break;
                        case Channel.Angle:
                            angle = value;
                        break;
                        case Channel.Status:
                            status = value;
                        break;
                        
                        case Channel.Speed:
                            speed = value;
                        break;
                        
                        case Channel.Day:
                            day = (byte)value;
                        break;
                        
                        case Channel.Month:
                            month = (byte)value;
                        break;
                        
                        case Channel.Year:
                            year = (ushort)value;
                            if (year < 1000)
                                year += 2000;
                        break;
                        
                        case Channel.Hour:
                            hour = (byte)value;
                        break;
                        
                        case Channel.Minute:
                            minute = (byte)value;
                        break;

                        case Channel.Second:
                            second = (byte)value;
                        break;
                    }
                }
                
                _view.Status = status;
                _view.Altitude = altitude;
                _view.Speed = speed;
                _view.Hour = hour;
                _view.Minute = minute;
                _view.Second = second;
                _view.Day = day;
                _view.Month = month;
                _view.Year = year;

                _view.AddPoint(longitude, latitude, angle);
  
            }
        }

        protected override void OnSetupTheControls()
        {
            _view = (TrackViewCtrl)Controls[0];

            foreach (XmlElement xmlElement in XmlRep.ChildNodes)
            {
                if (!xmlElement.HasAttribute("name"))
                    continue;

                if(xmlElement.GetAttribute("name") != "sigMaping")
                    continue;

                string[] mapping = xmlElement.InnerText.Split('/');

                uint sigID = Convert.ToUInt32(mapping[0]);
                int index = Convert.ToInt32(mapping[1]);
                _signalMapping[sigID] = index;
            }
        }
    }
}
