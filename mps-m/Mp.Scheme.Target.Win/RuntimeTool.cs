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

using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Mp.Visual.Docking;

namespace Mp.Scheme.Target.Win
{
    internal class RuntimeTool : Mp.Scheme.Sdk.Tool
    {
        private bool _debug;
        private Form _mainFrame;
        public RuntimeTool(bool debug)
        {
            _debug = debug;

            base.Icon = Images.Start;
            base.Name = StringResource.RunTimeEngineMenu;
            base.Shortcut = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.F6)));
            base.ToolTip = StringResource.RunTimeEngineText;
        }

        public override void LoadResources()
        {
            base.Name = StringResource.RunTimeEngineMenu;
            base.ToolTip = StringResource.RunTimeEngineText;
        }


        public override void OnLoadDocument(Mp.Scheme.Sdk.Document doc, DockPanel dockPanel, Form mainFrame)
        {
            _mainFrame = mainFrame;
            _doc = doc;
        }

        public override void OnExecute()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            string command = AppDomain.CurrentDomain.BaseDirectory;
            command = Path.GetFullPath(command)+ "rt\\";
            psi.WorkingDirectory = command;
            command = Path.Combine(command,"Mp.Runtime.App.exe");

            psi.Arguments = "\"" + _doc.File + "\"";
            psi.FileName = command;
                        
            Process proc = Process.Start(psi);
            
            _mainFrame.Visible = false;

            while (!proc.WaitForExit(100))
                Application.DoEvents();

            _mainFrame.Visible = true;
        }

        public override bool NeedToSaveDocument
        {
            get{ return true; }
        }

        public override bool NeedToReloadDocument
        {
            get{ return true; }
        }

        private Mp.Scheme.Sdk.Document _doc;
    }
}
