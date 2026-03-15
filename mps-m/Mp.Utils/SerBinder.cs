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
using System.Collections;
using System.Runtime.Serialization;
using System.Reflection;

namespace Mp.Utils
{
    /// <summary>
    /// A general deserialization binder for the given 
    /// type to the same type into a loaded assembly.    
    /// </summary>
    /// <remarks>
    /// The deserialization formater is looking only in the application directory for
    /// assemblies. Loaded assemblies are not used to find the deserialization 
    /// object type. Use this binder to bypass this problem.
    /// <code>
    ///  BinaryFormatter formater = new BinaryFormatter();
    ///  formater.Binder  = new SerBinder();
    ///  ...
    ///  object obj = formater.Deserialize( stream );
    ///  ...
    /// </code>
    /// </remarks>
    public class SerBinder : SerializationBinder
    {
        private Hashtable _typeMapping;

         public SerBinder(Hashtable typeMapping)
         {
             _typeMapping = typeMapping;
         }

         private string GetNewTypeName(string oldTypeName)
         {
             if (_typeMapping.Contains(oldTypeName))
                 return GetNewTypeName((string)_typeMapping[oldTypeName]);

             return oldTypeName;
         }

         public override Type BindToType( string assemblyName, string typeName ) 
         {
            Assembly[]  assemblies = AppDomain.CurrentDomain.GetAssemblies();

            string newTypeName = GetNewTypeName(typeName);

            foreach( Assembly assembly in assemblies ) 
            {
                Type type = assembly.GetType(newTypeName);                
                if (type  != null)
                    return type;
            }

            Type lastChanche = Type.GetType(newTypeName);
            return lastChanche;

        }

    }

}
