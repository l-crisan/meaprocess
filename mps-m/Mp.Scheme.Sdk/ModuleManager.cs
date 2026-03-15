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
using System.Xml;

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// Implements the module managment.
    /// </summary>
    /// <remarks>
    /// A module manager manage all registerd runtime engines with all its sub-modules.
    /// </remarks>
    public class ModuleManager
    {
        private List<Module> _runtineEngines = new List<Module>();

        private void LoadModule(Module runtime, Module module)
        {
            for (int i = 0; i < module.Stations.Count; i++)
                runtime.Stations.Add(module.Stations[i]);

            runtime.Modules.Add(module);
        }


        /// <summary>
        /// Default constructor.
        /// </summary>
        public ModuleManager()
        { }

        /// <summary>
        /// Return the runtime engine module by identifier.
        /// </summary>
        /// <param name="identifier">The runtime engine identifier</param>
        /// <returns>The runtime engine module.</returns>
        public Module GetEngineByName(string identifier)
        {
            foreach (Module engine in _runtineEngines)
            {
                if (identifier == engine.Identifier)
                    return engine;
            }

            return null;
        }

        /// <summary>
        /// Return the runtime engine by type identifier.
        /// </summary>
        /// <param name="type">The type identifier of the requested runtime engine.</param>
        /// <returns>The runtime engine module.</returns>
        public Module GetEngineByType(string type)
        {
            foreach (Module engine in _runtineEngines)
            {
                if (type == engine.Type)
                    return engine;
            }

            return null;
        }

        /// <summary>
        /// Load a runtime engine from module file (dll).
        /// </summary>
        /// <param name="file">The module file.</param>
        /// <returns>The runtime engine module.</returns>
        public Module LoadRuntimeEngine(string file)
        {
            Module runtime = ModuleLoader.GetInstance(file);

            if (runtime == null)
                return null;

            if (runtime.ParentType == "" || runtime.ParentType == null)
            {            
                _runtineEngines.Add(runtime);
                return runtime;
            }

            return null;
        }


        /// <summary>
        /// Load a sub module in to the specified runtime engine.
        /// </summary>
        /// <param name="runtimeEngine">The runtime engine module in which to load the sub module.</param>
        /// <param name="file">The file of the sub module.</param>
        /// <returns>The loaded sub module.</returns>
        public Module LoadModule(Module runtimeEngine, string file)
        {
            Module module = ModuleLoader.GetInstance(file);

            if (module == null)
                return null;

            if(!runtimeEngine.Type.Contains(module.ParentType))
                if(module.ParentType != "General")
                    return null;

            LoadModule(runtimeEngine, module);

            return module;
        }



        /// <summary>
        /// Load all registered modules from the config file.
        /// </summary>
        /// <param name="runtimes">The string that contain the runtime engines.</param>
        public void LoadAllRegModules(string runtimes, IntPtr hwnd)
        {
            XmlDocument xmldoc = new XmlDocument();

            try
            {
                xmldoc.LoadXml(runtimes);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            //Runtimes
            foreach (XmlNode xmlRuntimeNode in xmldoc.DocumentElement.ChildNodes)
            {
                XmlElement xmlRuntime = xmlRuntimeNode as XmlElement;

                if( xmlRuntime == null)
                    continue;

                Module runtime = ModuleLoader.GetInstance(xmlRuntime.Attributes["file"].Value);
                
                if (runtime == null)
                    continue;
                          
                _runtineEngines.Add(runtime);

                //Modules
                foreach(XmlNode xmlModuleNode in xmlRuntime.ChildNodes)
                {
                    XmlElement xmlModule = xmlModuleNode as XmlElement;

                    if (xmlModule == null)
                        continue;

                    try
                    {//MeaProcess modules 
                        Module module = ModuleLoader.GetInstance(xmlModule.Attributes["file"].Value);

                        if (module != null)
                            LoadModule(runtime, module);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }            
        }

        /// <summary>
        /// Remove a runtime engine.
        /// </summary>
        /// <param name="module">The runtime engine to remove.</param>
        public void RemoveModule(Module module)
        {
            for (int i = 0; i < _runtineEngines.Count; ++i)
            {
                Module runtime = _runtineEngines[i];

                if (runtime == module)
                {
                    _runtineEngines.Remove(module);
                    return;
                }

                for (int j = 0; j < runtime.Modules.Count; ++j)
                {
                    Module m = (Module) runtime.Modules[j];

                    if (m == module)
                    {
                        runtime.Modules.Remove(module);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Save all registered modules into a string.
        /// </summary>
        public void SaveAllRegModules(string moduelSetFile)
        {            
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml("<?xml version=\"1.0\"?><Runtimes></Runtimes>");            

            foreach (Module runtime in _runtineEngines)
            {
                XmlElement xmlRuntime =  xmlDoc.CreateElement("Runtime");

                xmlDoc.DocumentElement.AppendChild(xmlRuntime);
                XmlAttribute fileNameAttribute =  xmlDoc.CreateAttribute("file");
                fileNameAttribute.Value = runtime.Path;
                xmlRuntime.Attributes.Append(fileNameAttribute);
                              
                foreach (Module module in runtime.Modules)
                {                    
                    XmlElement xmlModule =  xmlDoc.CreateElement("Module");
                    xmlRuntime.AppendChild(xmlModule);
                    fileNameAttribute =  xmlDoc.CreateAttribute("file");
                    fileNameAttribute.Value = module.Path;
                    xmlModule.Attributes.Append(fileNameAttribute);
                }
            }

            xmlDoc.Save(moduelSetFile);
        }

        /// <summary>
        /// Gets a list of the registered runtime engines.
        /// </summary>
        public List<Module> RuntimeEngines
        { 
            get 
            {
                List<Module> modules = new List<Module>();

                foreach (Mp.Scheme.Sdk.Module rt in _runtineEngines)
                    modules.Add(rt);

                return modules; 
            } 
        }

        public List<Module> AllRuntimeEngines
        {
            get
            {
                return _runtineEngines;
            }
        }
    }
}