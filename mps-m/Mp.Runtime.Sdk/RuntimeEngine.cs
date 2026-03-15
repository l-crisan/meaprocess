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
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using Mp.Runtime.Adaption;
using System.Drawing;

namespace Mp.Runtime.Sdk
{
    /// <summary>
    /// This class implements the wrapper for the runtime engine.
    /// </summary>
    /// <remarks>
    /// It's a singleton.
    /// </remarks>
    public class RuntimeEngine : Mp.Runtime.Adaption.Runtime
    {
        private static RuntimeEngine _instance;
        private static Icon _appIcon;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>
        /// Is private use Instance() to get a instance.
        /// </remarks>
        private RuntimeEngine()
        { }
        /// <summary>
        /// Destructor
        /// </summary>
        ~RuntimeEngine()
        {
            FreeHandle();
            UnloadAllModules();
        }

        public static Icon AppIcon
        {
            get { return _appIcon; }
            set { _appIcon = value; }
        }

        /// <summary>
        /// Gets a instance of the runtime engine.
        /// </summary>
        /// <returns>The runtime engine</returns>
        public static RuntimeEngine Instance()
        {
            if (_instance == null)
                _instance = new RuntimeEngine();

            return _instance;
        }

        public static XmlDocument DocToCurrentVersion(XmlDocument doc)
        {
            return doc;
        }
    }
}
