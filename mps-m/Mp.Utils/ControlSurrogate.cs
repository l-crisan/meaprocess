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
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Mp.Utils
{
    ///<summary>
    /// A generel serialization surrogat for all non serializable classes.
    /// </summary>
    /// <remarks>
    /// Use this class to serialialize non serializable classes.
    /// </remarks>
    public class ControlSurrogate : ISerializationSurrogate
    {
        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        /// <remarks>
        /// Use this constructor to construct a surrogat for deserialization.
        /// </remarks>      
        public ControlSurrogate()
        { }

        /// <summary>
        /// Serialization constructor.
        /// </summary>
        /// <remarks>
        /// Use this constructor to create a serialization surrogat.
        /// </remarks>
        /// <param name="propertyFilter">
        /// An array with property filter strings.
        /// </param>
        public ControlSurrogate( List<string> propertyFilter )
        { _propertyFilter = propertyFilter; }

        /// <summary>
        /// Call this to serialize an object to base 64 string.
        /// </summary>
        /// <param name="obj">
        /// The object to serialize.
        /// </param>
        /// <param name="propertyFilter">
        /// The properties to serialize as string.
        /// </param>
        /// <returns>
        /// The base 64 string.
        /// </returns>
        public static string SerializeToString( object obj, List<string> propertyFilter )
        {
            MemoryStream      stream          = new MemoryStream();
            SurrogateSelector surogatSelector = new SurrogateSelector();
            ControlSurrogate  surogat         = new ControlSurrogate(propertyFilter);
            BinaryFormatter   formater        = new BinaryFormatter();

            surogatSelector.AddSurrogate(obj.GetType(), new StreamingContext(StreamingContextStates.All), surogat);
            formater.SurrogateSelector = surogatSelector;
            formater.Serialize(stream, obj);
            stream.Flush();
            stream.Seek(0, 0);

            byte[] buffer = new byte[stream.Length];

            stream.Read(buffer, 0, (int)stream.Length);
            stream.Close();
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        ///  Deserialize an object from base 64 string,
        /// </summary>
        /// <param name="objectAsString">
        ///  The base 64 string witch contain the serialized object.
        /// </param>
        /// <param name="type">
        /// The type of the object.
        /// </param>
        /// <returns>
        /// The serialized object.
        /// </returns>
        public static object DeserializeFromString( string objectAsString, Type type, Hashtable typeMapping )
        {
            try
            {
                BinaryFormatter formater = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                SurrogateSelector surogatSelector = new SurrogateSelector();
                ControlSurrogate surogat = new ControlSurrogate();
                byte[] buffer = Convert.FromBase64String(objectAsString);

                stream.Write(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Seek(0, 0);

                surogatSelector.AddSurrogate(type, new StreamingContext(StreamingContextStates.All), surogat);

                formater.SurrogateSelector = surogatSelector;
                formater.Binder = new SerBinder(typeMapping);

                object obj = formater.Deserialize(stream);

                stream.Close();

                return obj;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Return a type identifier from string type.
        /// </summary>
        /// <remarks>
        /// This function use Assembly.GetType of the all loaded assemblys.
        /// </remarks>
        /// <param name="typeString">
        /// The type as string.
        /// </param>
        /// <returns>
        /// The type identifier.
        /// </returns>
        public static Type GetLoadedTypeByName( string typeString )
        {
            string[] array = typeString.Split('.');
            string dllName = "";

            List<Assembly> assemblies = new List<Assembly>();

            for (int i = 0; i < array.Length - 1; ++i)
                dllName += array[i] + ".";

            dllName += "dll";

            try
            {
                assemblies.Add(Assembly.LoadFrom(dllName));
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            Assembly[] asms = AppDomain.CurrentDomain.GetAssemblies();

            foreach( Assembly a in asms)
                assemblies.Add(a);

            Type type = null;
            
            foreach (Assembly assemby in assemblies)
            {                
                type = assemby.GetType(typeString);

                if (type != null)
                    return type;
            }

            return type;
        }

        /// <summary>
        /// <see cref="System.Runtime.Serialization.ISerializationSurrogate"/>
        /// </summary>        
        public void GetObjectData( object obj, SerializationInfo info, StreamingContext context )
        {
            if (_propertyFilter == null)
                throw new Exception("Property filter expected");

            PropertyDescriptorCollection    properties      = TypeDescriptor.GetProperties(obj);
            

            info.SetType(obj.GetType());

            PropertyDescriptor property;
            foreach (string propName in _propertyFilter)
            {
                property = properties[propName];
                object valueObj = property.GetValue(obj);

                if (valueObj != null)
                    info.AddValue(property.Name, property.GetValue(obj), property.PropertyType);
            }
        }

        /// <summary>
        /// <see cref="System.Runtime.Serialization.ISerializationSurrogate"/>
        /// </summary>                
        public object SetObjectData( object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector )
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
          
            obj = Activator.CreateInstance( obj.GetType() );

            SerializationInfoEnumerator en = info.GetEnumerator();

            while(en.MoveNext())
            {
                try
                {
                    PropertyDescriptor property = properties[en.Name];
                    property.SetValue(obj, en.Value);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            return obj;
        }
      
        private List<string> _propertyFilter;
    }
}
