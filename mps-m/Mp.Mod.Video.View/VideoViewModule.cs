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
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;
using Mp.Scheme.Sdk;

namespace Mp.Mod.VideoView
{
    public class VideoViewModule : Mp.Scheme.Sdk.Module
    {
        private Assembly _viewCtrlAsm;

        public VideoViewModule()
        {
            ProcessStation station;

            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Mp.Visual.Video.dll");
            _viewCtrlAsm = Assembly.LoadFile(path);


            base.Identifier = "Video view module";
            base.Type = "Module";
            base.ParentType = "WINDOWS";


            station = new VideoViewPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);
        }
    }
}
