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
using System.Threading;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace Mp.Runtime.App
{
    static class MeaProcess
    {
        [STAThread]
        static void Main(string[] args)
        {
            Properties.Settings.Default.Reload();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainFrame mf;
            MeaProcessRPC rpc = null;
            
            switch(args.Length)
            {
                case 1: //only the file
                    mf = new MainFrame(args[0]);
                break;

                case 2: // file + ip:port
                {
                    mf = new MainFrame(args[0]);
                    string[] arr = args[1].Split(':');
                    rpc = new MeaProcessRPC(mf, arr[0], Convert.ToInt32(arr[1]));
                }
                break;

                default:
                {
                    string defaultFile = AppDomain.CurrentDomain.BaseDirectory + "Mp.Rtf.dll";

                    if (File.Exists(defaultFile))
                        mf = new MainFrame(defaultFile);
                    else
                        mf = new MainFrame("");
                }
                break;
            }
            
            Application.Run(mf);
            
            if( rpc != null)
                rpc.StopServer();

            Properties.Settings.Default.Save();
        }
    }
}