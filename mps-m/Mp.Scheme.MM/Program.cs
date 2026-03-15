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
using System.IO;
using System.Reflection;
using Mp.Scheme.Sdk;

namespace Mp.Scheme.MM
{
    static class Program
    {

        static void RegisterRuntime(string rFile)
        {
            try
            {
                ModuleManagerDlg mw = new ModuleManagerDlg();
                mw.Hide();

                ModuleManager moduleManager = new ModuleManager();
                Assembly asm = Assembly.GetAssembly(mw.GetType());
                string file = Path.GetDirectoryName(asm.Location);
                string meaProcessModulePath = Path.Combine(file, "module.set");

                using (FileStream fs = new FileStream(meaProcessModulePath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs);
                    moduleManager.LoadAllRegModules(sr.ReadToEnd(), mw.Handle);
                    fs.Close();
                }

                Sdk.Module module = ModuleLoader.GetInstance(rFile);

                if (module == null)
                    return;                

                //Check exists
                foreach (Sdk.Module rtModule in moduleManager.AllRuntimeEngines)
                {
                    if (rtModule.Identifier == module.Identifier)
                        return;                    
                }

                //Register
                moduleManager.RuntimeEngines.Add(module);

                moduleManager.SaveAllRegModules(meaProcessModulePath);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        static void UnregisterRuntime(string rFile)
        {
            try
            {
                ModuleManagerDlg mw = new ModuleManagerDlg();
                mw.Hide();

                ModuleManager moduleManager = new ModuleManager();
                Assembly asm = Assembly.GetAssembly(mw.GetType());
                string file = Path.GetDirectoryName(asm.Location);
                string meaProcessModulePath = Path.Combine(file, "module.set");

                using (FileStream fs = new FileStream(meaProcessModulePath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs);
                    moduleManager.LoadAllRegModules(sr.ReadToEnd(), mw.Handle);
                    fs.Close();
                }

                Sdk.Module module = ModuleLoader.GetInstance(rFile);

                if (module == null)
                    return;

                //Check exists => remove
                for( int i = 0; i  < moduleManager.AllRuntimeEngines.Count; ++i)
                {
                    Sdk.Module rtModule = moduleManager.AllRuntimeEngines[i];

                    if (rtModule.Identifier == module.Identifier)
                    {
                        rtModule.Modules.Remove(rtModule);
                        break;
                    }                   
                }

                moduleManager.SaveAllRegModules(meaProcessModulePath);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }


        static void RegisterModule(string moduleFile)
        {
            try
            {
                ModuleManagerDlg mw = new ModuleManagerDlg();
                mw.Hide();

                ModuleManager moduleManager = new ModuleManager();
                Assembly asm = Assembly.GetAssembly(mw.GetType());
                string file = Path.GetDirectoryName(asm.Location);
                string meaProcessModulePath = Path.Combine(file, "module.set");

                using (FileStream fs = new FileStream(meaProcessModulePath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs);
                    moduleManager.LoadAllRegModules(sr.ReadToEnd(), mw.Handle);
                    fs.Close();
                }                

                Sdk.Module module = ModuleLoader.GetInstance(moduleFile);
                
                if( module == null)
                    return;

                //Check exists
                foreach(Sdk.Module rtModule in moduleManager.AllRuntimeEngines)
                {
                    foreach(Sdk.Module m in rtModule.Modules)
                    {
                        if(m.Identifier == module.Identifier)
                            return;
                    }
                }

                //Register
                foreach(Sdk.Module rtModule in moduleManager.AllRuntimeEngines)
                {
                    if(rtModule.SupportWindows && module.SupportWindows)
                        rtModule.Modules.Add(module);
                }            
                
                moduleManager.SaveAllRegModules(meaProcessModulePath);
            }
            catch(Exception ex)
            {
                Console.Write(ex);
            }
        }

        static void UnregisterModule(string moduleFile)
        {
            try
            {
                ModuleManagerDlg mw = new ModuleManagerDlg();
                mw.Hide();

                ModuleManager moduleManager = new ModuleManager();
                Assembly asm = Assembly.GetAssembly(mw.GetType());
                string file = Path.GetDirectoryName(asm.Location);
                string meaProcessModulePath = Path.Combine(file, "module.set");

                using (FileStream fs = new FileStream(meaProcessModulePath, FileMode.Open, FileAccess.Read))
                {
                    StreamReader sr = new StreamReader(fs);
                    moduleManager.LoadAllRegModules(sr.ReadToEnd(), mw.Handle);
                    fs.Close();
                }

                Sdk.Module module = ModuleLoader.GetInstance(moduleFile);

                if (module == null)
                    return;

                //Check exists => remove
                foreach (Sdk.Module rtModule in moduleManager.AllRuntimeEngines)
                {
                    for( int i = 0; i < rtModule.Modules.Count; ++i)                    
                    {
                        Sdk.Module m = rtModule.Modules[i] as Sdk.Module;

                        if (m.Identifier == module.Identifier)
                        {
                            rtModule.Modules.Remove(m);
                            break;
                        }        
                    }
                }   

                moduleManager.SaveAllRegModules(meaProcessModulePath);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
        }

        [STAThread]
        static void Main(string[] args)
        {
            if(args.Length == 2)
            {
                switch(args[0].ToLower())
                {
                    case "regr":
                        RegisterRuntime(args[1]);
                        break;

                    case "unregr":
                        UnregisterRuntime(args[1]);
                        break;

                    case "regm":
                        RegisterModule(args[1]);
                    break;
                    
                    case "unregm":
                        UnregisterModule(args[1]);
                    break;                    
                }

                return;
            }

            if (args.Length  == 1)
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(args[0]);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(args[0]);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ModuleManagerDlg());
        }
    }
}
