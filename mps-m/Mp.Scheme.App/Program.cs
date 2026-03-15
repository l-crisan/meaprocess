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
using System.Windows.Forms;
using System.Threading;

namespace Mp.Scheme.App
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Properties.Settings.Default.Reload();

            if (Properties.Settings.Default.Language == 0)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            }
            else if( Properties.Settings.Default.Language == 1)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("de-DE");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("de-DE");
            }

            if (Properties.Settings.Default.ShowSplashScreen)
            {
                SplashScreen.SetBackgroundImage(Mp.Scheme.App.Resource.SplashRealtime);
                string[] version = AboutBox.AssemblyVersion.Split('.');
                string splashInfo = "Version PARIS (R3)";
                SplashScreen.SetTitleString(splashInfo);
                SplashScreen.BeginDisplay();
            }

            MainFrame mf;

            if(args.Length == 1)
                mf = new MainFrame(args[0]);
            else
                mf = new MainFrame("");

            Application.Run(mf);
        }
    }
}
