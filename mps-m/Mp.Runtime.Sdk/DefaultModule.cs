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

namespace Mp.Runtime.Sdk
{
    /// <summary>
    /// Default MeaProcess Module.
    /// </summary>
    public class DefaultModule : Mp.Runtime.Sdk.Module
    {
        public  DefaultModule()
        {
          
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Assembly.LoadFrom(path + "\\Mp.Visual.Base.dll");
            Assembly.LoadFrom(path + "\\Mp.Visual.Digital.dll");
            Assembly.LoadFrom(path + "\\Mp.Visual.Analog.dll");
            Assembly.LoadFrom(path + "\\Mp.Visual.WaveChart.dll");                                                
            Assembly.LoadFrom(path + "\\Mp.Visual.XYChart.dll");
            Assembly.LoadFrom(path + "\\Mp.Visual.GPS.dll");
            Assembly.LoadFrom(path + "\\Mp.Visual.HTML.dll");
            Assembly.LoadFrom(path + "\\Mp.Visual.Oscilloscope.dll");
            Assembly.LoadFrom(path + "\\Mp.Visual.PolarChart.dll");
            Assembly.LoadFrom(path + "\\Mp.Visual.Video.dll");
        }
    }
}
