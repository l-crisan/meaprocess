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
using System.Reflection;

namespace Mp.Utils
{
    public class AssemblyVersionInfo
    {
        public static string GetModuleVersion(string file)
        {
            AssemblyFileVersionAttribute fileVersion = (AssemblyFileVersionAttribute)GetModuleAttribute(file, "System.Reflection.AssemblyFileVersionAttribute");
            if (fileVersion == null)
                return "";

            return fileVersion.Version;
        }

        /// <summary>
        /// Return the module name.
        /// </summary>
        /// <returns>The module name.</returns>
        public static string GetModuleName(string file)
        {
            AssemblyProductAttribute product = (AssemblyProductAttribute)GetModuleAttribute(file, "System.Reflection.AssemblyProductAttribute");
            if (product == null)
                return "";

            return product.Product;
        }

        /// <summary>
        /// Return the module manufacturer.
        /// </summary>
        /// <returns>The module manufacturer.</returns>
        public static string GetModuleManufacturer(string file)
        {
            AssemblyCompanyAttribute company = (AssemblyCompanyAttribute)GetModuleAttribute(file, "System.Reflection.AssemblyCompanyAttribute");
            if (company == null)
                return "";

            return company.Company;
        }

        /// <summary>
        /// Return the module description.
        /// </summary>
        /// <returns>The module description.</returns>
        public static string GetModuleDescription(string file)
        {
            AssemblyDescriptionAttribute description = (AssemblyDescriptionAttribute)GetModuleAttribute(file, "System.Reflection.AssemblyDescriptionAttribute");

            if (description == null)
                return "";

            return description.Description;
        }

        /// <summary>
        /// Return the requested module attribute.
        /// </summary>
        /// <param name="attributeFullName">The identifier of the attribute.</param>
        /// <returns>The module attribute.</returns>
        protected static object GetModuleAttribute(string file, string attributeFullName)
        {
            Assembly assembly = Assembly.LoadFrom(file);

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
    }
}
