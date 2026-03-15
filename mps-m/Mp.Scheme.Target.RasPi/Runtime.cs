//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2010-2016  Laurentiu-Gheorghe Crisan
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
using Mp.Scheme.Deploy;

namespace Mp.Scheme.Target.Mp.Scheme.Target.RasPi
{
    internal class RtLinuxEm : Sdk.Module
    {
        public RtLinuxEm()
        {
            RuntimeEngineFileExt = "*.mrpi";
            ProcessStation station;
            base.Identifier = "RasPi";
            base.Type = "RasPi";
            base.SupportLinux = true;
            base.HasGUI = false;

            //Splitter
            station = new SplitterPS();
            station.RuntimeEngine = base.Identifier;
            RegStation(station);

            //SubSchema
            station = new SubSchemePS();
            station.RuntimeEngine = base.Identifier;
            RegStation(station);

            //WriteProperty
            station = new WritePropPS();
            station.RuntimeEngine = base.Identifier;
            RegStation(station);

            //Read property
            station = new ReadPropPS();
            station.RuntimeEngine = base.Identifier;
            RegStation(station);

            //Stop PS
            station = new StopPS();
            station.RuntimeEngine = base.Identifier;
            RegStation(station);


            //Register the tools
            Tool tool = new DeployTool();
            Tools.Add(tool);
        }

        public override string FileExtDescription
        {
            get
            {
                return "RaspberryPi";
            }
        }
    }
}