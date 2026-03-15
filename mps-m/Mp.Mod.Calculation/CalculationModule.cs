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
using Mp.Scheme.Sdk;

namespace Mp.Mod.Calculation
{
    public class CalculationModule : Mp.Scheme.Sdk.Module
    {
        public CalculationModule()
        {
            ProcessStation station;

            base.Identifier = "Calculation module";
            base.Type = "Module";
            base.ParentType = "General";

            station = new ScalingPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new SignalDelayPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new SignalConvertPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new MovingMeanPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);
            
            station = new MeanNPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new ElectricityPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new SampleRatePS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new MultiplexerPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new DemultiplexerPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new BitExtractionPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new BitGroupingPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

        }
    }
}
