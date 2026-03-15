using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using Mp.Conv.Data;

namespace Mp.Conv.Gui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(args[0]);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(args[0]);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            DataConverterDlg dlg = new DataConverterDlg();
            dlg.ShowInTaskbar = true;
            Application.Run(dlg);
        }
    }
}
