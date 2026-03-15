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
using System.Xml;
using System.Globalization;

namespace Mp.Utils
{
    /// <summary>
    /// A special XML helper class.
    /// </summary>
    /// <remarks>
    /// This class is a helper for handling XML meta modeling.
    /// </remarks>
    /// <example>
    /// XML with meta information:
    /// <object type="Class" name="myObject" id="123">
    ///     <string name="myVar">a string</string>
    /// </object>
    /// The tag name can be an object or a primitive type like byte Uint32.
    /// The type attribute is the type name of the object class.
    /// The name attribute is the instance nam of the object.
    /// </example>
    public class XmlHelper
    {
        /// <summary>
        /// Return a child XmlElement by the tag name.
        /// </summary>
        /// <param name="node">
        /// The parent node.
        /// </param>
        /// <param name="name">
        /// The child name.
        /// </param>
        /// <returns>
        /// The XML element child.
        /// </returns>
        public static XmlElement GetChildByName(XmlNode node, string name)
        {
            XmlElement element;

            foreach (XmlNode child in node.ChildNodes)
            {
                element = (child as XmlElement);

                if (element == null)
                    continue;

                if (element.GetAttribute("name") == name)
                    return element;
            }

            return null;
        }

        /// <summary>
        /// Gets the xhild node by type.
        /// </summary>
        /// <param name="node">The parent node.</param>
        /// <param name="type">The child type.</param>
        /// <returns></returns>
        public static XmlElement GetChildByType(XmlNode node, string type)
        {
            XmlElement element;

            foreach (XmlNode child in node.ChildNodes)
            {
                element = (child as XmlElement);

                if (element == null)
                    continue;

                if (element.GetAttribute("type") == type)
                    return element;
            }

            return null;
        }

        /// <summary>
        /// Gets a parameter from the xml object.
        /// </summary>
        /// <param name="xmlObject">The xml object.</param>
        /// <param name="paramName">The parameter name.</param>
        /// <returns></returns>
        public static string GetParam( XmlNode xmlObject, string paramName)
        {
            XmlElement element = GetChildByName(xmlObject, paramName);
                        
            if(element != null)
                return element.InnerText;
            else
                return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlObject"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static long GetParamNumber(XmlNode xmlObject, string paramName)
        {
            string value = GetParam(xmlObject, paramName);

            if (value == "")
                return 0;

            return Convert.ToInt64(value, 10);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlObject"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static ulong GetParamNumberULong(XmlNode xmlObject, string paramName)
        {
            string value = GetParam(xmlObject, paramName);

            if (value == "")
                return 0;

            return Convert.ToUInt64(value, 10);
        }

        public static double GetParamDouble(XmlNode xmlObject, string paramName)
        {
            string value = GetParam(xmlObject, paramName);

            if (value == "")
                return 0.0;

            string strSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            value = value.Replace(".", strSep);
            return Double.Parse(value);
        }

        public static void SetParam(XmlNode xmlObject, string paramName, string paramType, string paramValue)
        {
            XmlElement param = GetChildByName(xmlObject, paramName);
            XmlAttribute    attribute;

            if (param != null)
                xmlObject.RemoveChild(param);

            param = xmlObject.OwnerDocument.CreateElement(paramType);
            attribute = xmlObject.OwnerDocument.CreateAttribute("name");
            attribute.Value = paramName;
            param.Attributes.Append(attribute);
            xmlObject.AppendChild(param);
                        
            if(paramValue != "")
                param.InnerText = paramValue;
        }

        public static void SetParamNumber(XmlNode xmlObject, string paramName, string paramType, int value)
        {
            SetParam(xmlObject, paramName, paramType, value.ToString());
        }

        public static void SetParamNumber(XmlNode xmlObject, string paramName, string paramType, long value)
        {
            SetParam(xmlObject, paramName, paramType, value.ToString());
        }

        public static void SetParamNumber(XmlNode xmlObject, string paramName, string paramType, ulong value)
        {
            SetParam(xmlObject, paramName, paramType, value.ToString());
        }

        public static void SetParamDouble(XmlNode xmlObject, string paramName, string paramType, double value)
        {             
            string strSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string text = Convert.ToString(value);
            text = text.Replace(strSep, ".");

            SetParam(xmlObject, paramName, paramType, text);
        }

        public static uint GetObjectID(XmlNode xmlObject)
        {
            XmlElement element = (xmlObject as XmlElement);
            string id = element.GetAttribute("id");
            
            if (id == "")
                return 0;

            return Convert.ToUInt32( id , 10 );
        }

        public static XmlElement CreateObject(XmlElement parent, string type, string subType, ulong id)
        {
            XmlAttribute attribute;
            XmlElement   newObject;
            
            //Create the new object.
            newObject = parent.OwnerDocument.CreateElement("object");

            //Type attribute.
            attribute = parent.OwnerDocument.CreateAttribute("type");
            attribute.Value = type;
            newObject.Attributes.Append(attribute);
            
            //Name attribute.
            if( subType != "" )
            {
                attribute       = parent.OwnerDocument.CreateAttribute("subType");
                attribute.Value = subType;
                newObject.Attributes.Append(attribute);
            }

            //ID attribute.
            attribute = parent.OwnerDocument.CreateAttribute("id");
            attribute.Value = id.ToString();
            newObject.Attributes.Append(attribute);

            //Append to the parent object.
            parent.InsertBefore(newObject, null);
            return newObject;
        }
 
        public static XmlElement CreateElement(XmlElement parent, string type, string name, string value)
        {
            XmlElement   newElement;
            XmlAttribute attribute;

            //Create the new object.
            newElement = parent.OwnerDocument.CreateElement(type);

            //Type attribute.
            attribute = parent.OwnerDocument.CreateAttribute("name");
            attribute.Value = name;
            newElement.Attributes.Append(attribute);

            //Add the element to the parent.
            newElement.InnerText = value;
            parent.InsertBefore(newElement, null);

            return newElement;
        }
    }
}
