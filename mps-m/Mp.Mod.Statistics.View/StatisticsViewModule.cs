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
using System.Reflection;
using Mp.Scheme.Sdk;

namespace Mp.Mod.Statistics.View
{
    public class StatisticsViewModule : Mp.Scheme.Sdk.Module
    {        
        public StatisticsViewModule()
        {
            ProcessStation station;

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Mp.Visual.Diagram.dll");
            Assembly.LoadFile(path);

            path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Mp.Visual.XYChart.dll");
            Assembly.LoadFile(path);

            base.Identifier = "Statistics View (Module)";
            base.Type = "Module";
            base.ParentType = "WINDOWS";

            station = new HistogramViewPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

        }
    }
}
