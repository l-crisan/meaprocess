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
using System.Reflection;
using Mp.Scheme.Sdk;
using Mp.Scheme.Sdk.Ui;

namespace Mp.Scheme.Target.Win
{
        /// <summary>
        ///  The windows runtime engine module representation in the scheme.
        /// </summary>
        internal class Runtime : Sdk.Module
        {
            Assembly _asm;

            public Runtime()
            {
                string path = System.IO.Path.GetDirectoryName(typeof(Mp.Scheme.Sdk.Module).Assembly.Location);

                _asm = Assembly.LoadFile(System.IO.Path.Combine(path,"Mp.Scheme.Sdk.Ui.dll"));

                RuntimeEngineFileExt = "*.mpw";

                base.Identifier = StringResource.WinRuntime;
                base.Type = "WINDOWS";
                base.SupportWindows = true;
                base.HasGUI = true;
                base.SupportPlaySound = true;


                //Register the process stations 
                //------------------------------------------
                ProcessStation station;

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

                //Wave Chart
                station = new WaveChartPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //XY Chart
                station = new XYChartPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Polar Chart
                station = new PoolarChartPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Oscilloscope
                station = new OscilloscopePS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Numeric View
                station = new NumericViewPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //List view
                station = new SignalViewPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Numeric Control
                station = new NumericControlPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);
                    
                //Digital Meter
                station = new DigitalMeterPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                station = new DigitalMeter7SegPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Bar
                station = new BarCtrlPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Digital meter 1
                station = new NumericControlBlockPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);


                //Digital meter 2
                station = new DigitalMeterBlockPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Digital meter 3
                station = new DigitalMeter7SegBlockPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Bar View
                station = new BarViewBlockPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                station = new MamometerBlockPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);


                station = new GaugeBlockPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                station = new TachometerBlockPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);


                station = new ThermometerBlockPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                station = new LedBlockPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Manometer
                station = new ManometerPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Gauge
                station = new GaugePS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Tachometer 
                station = new TachometerPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Thermometer
                station = new ThermometerPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Led
                station = new LedPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Input Button
                station = new InputButtonPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Radio Button
                station = new RadioButtonPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Check Box
                station = new CheckBoxPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Combo Box
                station = new ComboBoxPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Knob
                station = new KnobPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Switch
                station = new SwitchPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Slider
                station = new SliderPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Numeric Input
                station = new NumericInputPS();
                station.RuntimeEngine = base.Identifier;
                RegStation(station);

                //Register the tools
                //-----------------------------------------
                Tool tool = new PanelDesignerTool();
                Tools.Add(tool);

                tool = new DistributeTool();
                Tools.Add(tool);

                tool = new RuntimeOptTool();
                Tools.Add(tool);

                tool = new RuntimeTool(false);
                Tools.Add(tool);
            }

            public override string FileExtDescription
            {
                get
                {
                    return "Windows";
                }
            }
       }
}