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
using System.Collections;
using System.Reflection;
using System.Drawing;
using System.Xml;
using Mp.Utils;

namespace Mp.Scheme.Sdk
{  
    /// <summary>
    /// Provides the capability to group modules, process stations and tools.
    /// </summary>
    public class Module
    {
        private List<ProcessStation> _stations = new List<ProcessStation>();
        private List<Tool> _tools = new List<Tool>();
        private string _type;
        private string _parentType;
        private string _identifier;
        private string _path;
        private string _runtimeEngineFileExt;        
        private int _licKey = 0;
        private ArrayList _modules = new ArrayList();
        private bool _supportWindows = false;
        private bool _supportLinux = false;
        private bool _supportMCM = false;
        private bool _hasGUI = false;
        private double _maxSignalRate = 200000;
        private bool _supportPlaySound = false;

        /// <summary>
        /// Gets or sets the module type identifier.
        /// </summary>
        public string Type
        {
            set { _type = value; }
            get { return _type; }
        }

        public virtual string FileExtDescription
        {
            get;
            set;
        }

        public int LicKey
        {
            set{ _licKey = value;}
            get { return _licKey; }
        }

        public bool SupportPlaySound
        {
            set { _supportPlaySound = value; }
            get { return _supportPlaySound; }
        }

        public double MaxSignalRate
        {
            get { return _maxSignalRate; }
            set { _maxSignalRate = value; }
        }

        /// <summary>
        /// Gets or sets the parent module type indetifier.
        /// </summary>
        public string ParentType
        {
            set { _parentType = value; }
            get { return _parentType; }
        }

        /// <summary>
        /// Gets or sets the module identifier.
        /// </summary>
        public string Identifier
        {
            set { _identifier = value; }
            get { return _identifier; }
        }

        /// <summary>
        /// Gets or sets the module file path.
        /// </summary>
        public string Path
        {
            set { _path = value; }
            get { return _path; }
        }

        /// <summary>
        /// Gets or sets the runtime engine file extension.
        /// </summary>
        public string RuntimeEngineFileExt
        {
            set { _runtimeEngineFileExt = value; }
            get { return _runtimeEngineFileExt; }
        }

        /// <summary>
        /// Gets or sets the sub modules.
        /// </summary>
        public ArrayList Modules
        {
            set { _modules = value; }
            get { return _modules; }
        }

        public bool SupportMCM
        {
            get{return _supportMCM;}
            set{ _supportMCM = value;}
        }

        public bool SupportLinux
        {
            get { return _supportLinux; }
            set{ _supportLinux = value;}
        }

        public bool SupportWindows
        {
            get { return _supportWindows; }
            set { _supportWindows = value; }
        }


        public bool HasGUI
        {
            get { return _hasGUI; }
            set { _hasGUI = value; }
        }


        /// <summary>
        /// Default constructor.
        /// </summary>
        public Module()
        { }

        /// <summary>
        /// Register a process station.
        /// </summary>
        /// <param name="station">The process station to register.</param>
        protected void RegStation(ProcessStation station)
        { _stations.Add(station); }

        /// <summary>
        /// Gets the list of stations in the module.
        /// </summary>
        public List<ProcessStation> Stations
        { get { return _stations; } }

        /// <summary>
        /// Gets the module version.
        /// </summary>
        public string ModuleVersion
        {
            get
            {
                AssemblyFileVersionAttribute fileVersion = (AssemblyFileVersionAttribute)GetModuleAttribute( "System.Reflection.AssemblyFileVersionAttribute");
                if (fileVersion == null)
                    return "";

                return fileVersion.Version;
            }
        }

        /// <summary>
        /// Gets the module name.
        /// </summary>
        public string ModuleName
        {
            get
            {
                
                AssemblyTitleAttribute title = (AssemblyTitleAttribute)GetModuleAttribute("System.Reflection.AssemblyTitleAttribute");
                if (title == null)
                    return "";

                return title.Title;
            }
        }

        /// <summary>
        /// Gets the module manufacturer.
        /// </summary>
        public string ModuleManufacturer
        {
            get
            {
                AssemblyCompanyAttribute company = (AssemblyCompanyAttribute)GetModuleAttribute("System.Reflection.AssemblyCompanyAttribute");
                if (company == null)
                    return "";

                return company.Company;
            }
        }

        /// <summary>
        /// Gets the module description.
        /// </summary>
        public string ModuleDescription
        {
            get
            {
                AssemblyDescriptionAttribute description = (AssemblyDescriptionAttribute)GetModuleAttribute("System.Reflection.AssemblyDescriptionAttribute");

                if (description == null)
                    return "";

                return description.Description;
            }
        }

        /// <summary>
        /// Gets the module file.
        /// </summary>
        public string ModuleFile        
        {
            get
            {
                return Path;
            }
        }

        private object GetModuleAttribute(string attributeFullName)
        {
            Assembly assembly = Assembly.LoadFrom(Path);

            object[] Attributes = assembly.GetCustomAttributes(true);
            string str;
            foreach (object attribute in Attributes)
            {
                str = attribute.GetType().FullName;
                if (attribute.GetType().FullName == attributeFullName)
                    return attribute;
            }
            return null;
        }

        /// <summary>
        /// Register a sub module.
        /// </summary>
        /// <param name="file">The sb module file.</param>
        /// <returns>The sub module.</returns>
        public Module RegModule(string file)
        {
            Module module = ModuleLoader.GetInstance(file);
            
            if (module == null)
                return null;

            Modules.Add(module);
            return module;
        }

        /// <summary>
        /// Create a process station by the given type.
        /// </summary>
        /// <param name="doc">The document in which to create the parocess station.</param>
        /// <param name="type">The type identifier of the process station.</param>
        /// <param name="xmlRep">The xml representation of the process station.</param>
        /// <returns>The new process station.</returns>
        public ProcessStation CreateStation(Document doc, Visual.Diagram.DiagramCtrl diagram, string type, XmlElement xmlRep)
        {
            string subType = "";

            XmlAttribute attr = xmlRep.Attributes["subType"];

            if (attr != null)
                subType = attr.Value;

            foreach (ProcessStation station in _stations)
            {
                if (station.Type == type && subType == station.SubType)
                {
                    ProcessStation st = (ProcessStation)Activator.CreateInstance(station.GetType(), null);
                    st.Init(doc, diagram, xmlRep, new Point(10, 10));
                    return st;
                }
            }
            return null;
        }

        /// <summary>
        /// Create a process station by the given type.
        /// </summary>
        /// <param name="doc">The document in which to create the parocess station.</param>
        /// <param name="type">The type identifier of the process station.</param>
        /// <param name="xmlRep">The xml representation of the process station.</param>
        /// <returns>The new process station.</returns>
        public ProcessStation CreateStation(Document doc, string type, XmlElement xmlRep)
        {
            string subType = "";

            XmlAttribute attr = xmlRep.Attributes["subType"];

            if( attr != null)
                subType = attr.Value;

            foreach (ProcessStation station in _stations)
            {
                if (station.Type == type && subType == station.SubType)
                {
                    ProcessStation st = (ProcessStation ) Activator.CreateInstance(station.GetType(), null);
                    uint diagramID = (uint) XmlHelper.GetParamNumber(xmlRep,"diagram");

                    if (diagramID == 0)
                    {
                        st.Init(doc, doc.Diagrams[0].Diagram, xmlRep, new Point(10, 10));
                        return st;
                    }
                    else
                    {
                        foreach (DiagramWindow diagram in doc.Diagrams)
                        {
                            if (diagram.Diagram.ID == diagramID)
                            {
                                st.Init(doc, diagram.Diagram, xmlRep, new Point(10, 10));
                                return st;
                            }
                        }
                    }
                }
            }
            return null;
        }
        
        /// <summary>
        /// Return the prototype process station.
        /// </summary>
        /// <param name="name">The process station name.</param>
        /// <returns>The prototype process station.</returns>
        public ProcessStation GetStationByClassType(string classType)
        {
            foreach (ProcessStation station in _stations)
            {
                if (station.GetType().ToString() == classType)
                    return station;
            }
            return null;
        }

        /// <summary>
        /// Remove a sub module.
        /// </summary>
        /// <param name="module">The module to remove.</param>
        public void RemoveModule(Module module)
        { 
            Modules.Remove(module); 
        }

        /// <summary>
        /// Gets the list of tools available in this module.
        /// </summary>
        public List<Tool> Tools
        { get { return _tools; } }

    }
}