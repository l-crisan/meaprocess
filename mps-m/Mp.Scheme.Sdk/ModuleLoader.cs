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
using System.Reflection;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// Load modules from a config file or direct from a module file.
    /// </summary>    
    public class ModuleLoader
    {
        private static string ExpandFile(string file)
        {
            if (file.Contains(".\\"))
            {
                string str = file.TrimStart('.');
                str = str.TrimStart('\\');
                return  AppDomain.CurrentDomain.BaseDirectory + str;
            }

            return file;
        }
        
        /// <summary>
        /// Load a module from a module file (dll).
        /// </summary>
        /// <param name="file">The module file.</param>
        /// <returns>The loaded module.</returns>
        public static Module GetInstance(string file)
        {
            Assembly  assembly;
            Type[]    types;
            Module    module;

            string expFile = ExpandFile(file);

            try
            {
                assembly = Assembly.LoadFrom(expFile);
                types = assembly.GetTypes();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
                        

            foreach (Type type in types)
            {                
                if(type.BaseType == null)
                    continue;
                 
                if (type.BaseType.FullName == "Mp.Scheme.Sdk.Module")
                {
                    module = (Activator.CreateInstance(type) as Mp.Scheme.Sdk.Module);
                    module.Path = expFile;
                    return module;
                }

                if(type.BaseType.BaseType == null)
                    continue;

                if (type.BaseType.BaseType.FullName == "Mp.Scheme.Sdk.Module")
                {
                    module = (Activator.CreateInstance(type) as Mp.Scheme.Sdk.Module);
                    module.Path = expFile;
                    return module;
                }                
            }

            return null;
        }
    }
}
