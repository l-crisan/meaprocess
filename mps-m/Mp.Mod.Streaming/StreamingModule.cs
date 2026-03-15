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
using Mp.Scheme.Sdk;

namespace Mp.Mod.Streaming
{
    public class StreamingModule : Mp.Scheme.Sdk.Module
    {
        public StreamingModule()
        {
            ProcessStation station;

            base.Identifier = "Streaming module";
            base.Type = "Module";
            base.ParentType = "General";


            station = new InputPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new OutputPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);
         }
    }
}
