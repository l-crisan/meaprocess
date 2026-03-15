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

using Mp.Scheme.Sdk;

namespace Mp.Mod.GPS
{
    public class GPSModule : Mp.Scheme.Sdk.Module
    {
        public GPSModule()
        {
            ProcessStation station;

            base.Identifier = "GPS Generator Module";
            base.Type = "Module";
            base.ParentType = "General";
            this.SupportWindows = true;
            this.SupportLinux = true;

            if (!Licence.IsRuntimeAvailable(1))
                return;

            station = new GpsPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

        }
    }
}