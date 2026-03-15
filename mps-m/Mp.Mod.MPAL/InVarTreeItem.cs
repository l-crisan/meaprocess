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
using System.Globalization;
using System.Xml;
using Mpal.Model;
using System.Drawing;
using Mp.Utils;

namespace Mp.Mod.MPAL
{
    internal class MpalInVarTreeItem
    {
        private Parameter _param;
        private XmlElement _xmlSignal;
        private bool _scaled = true;
        private string _defValue = "";
        private bool _defValueChanged = false;
        private bool _isProperty;
        private PropertyType _propType;
        
        public MpalInVarTreeItem(Parameter param)
        {
            _param = param;
        }
        
        public bool Scaled
        {
            set { _scaled = value; }
            get { return _scaled; }
        }

        public Parameter Param
        {
            get { return _param; }
        }

        public Bitmap Icon
        {
            get
            {
               switch (_param.ParamDataType)
               {
                   case DataType.ARRAY:
                   case DataType.STRUCT:
                   case DataType.UDT:
                   case DataType.FB:
                       return Images.Struct.ToBitmap();
                   default:
                       return Images.Property.ToBitmap();
               }
            }
        }

        public string Name
        {
            get { return _param.Name; }
        }

        public string Type
        {
            get { return _param.ParamDataType.ToString(); }
        }

        public string Signal
        {
            get 
            {
                if (_xmlSignal != null && !_isProperty)
                    return XmlHelper.GetParam(_xmlSignal, "name");

                if (_xmlSignal != null && _isProperty)
                {
                    return "$(" + XmlHelper.GetParam(_xmlSignal, "name") + "," + _propType.ToString() +")";
                }

                return ""; 
            }
        }

        public bool DefValueChanged
        {
            get{ return _defValueChanged; }
        }

        public PropertyType PropType
        {
            set
            { 
                _propType = value;
                _isProperty = true;
            }
            get { return _propType; }
        }

        public bool IsProperty
        {
            set { _isProperty = value; }
            get { return _isProperty; }
        }

        public string DefValue
        {
            set 
            {
                if (value.Length > 1)
                {
                    if (value[0] != '$' || value[1] != '(')
                        if (!ValidateDefaultValue(value))
                            return;
                }
                else
                {
                    if (!ValidateDefaultValue(value))
                        return;
                }

                _defValueChanged = true;
                _defValue = value; 
            }
            get { return _defValue;}
        }

        private bool ValidateDefaultValue(string value)
        {
            if (value == "")
                return false;

            NumberFormatInfo info = new NumberFormatInfo();
                info.NumberDecimalSeparator = ".";

            try
            {
                switch (_param.ParamDataType)
                {
                    case DataType.ARRAY:
                    case DataType.STRUCT:
                    case DataType.UDT:
                    case DataType.FB:
                        return false;

                    case DataType.BOOL:
                        return (value == "TRUE" || value == "FALSE");

                    case DataType.SINT:
                        Convert.ToSByte(value);
                        break;

                    case DataType.INT:
                        Convert.ToInt16(value);
                        break;

                    case DataType.DINT:
                        Convert.ToInt32(value);
                        break;

                    case DataType.LINT:
                        Convert.ToInt64(value);
                        break;

                    case DataType.USINT:
                    case DataType.BYTE:
                        Convert.ToByte(value);
                        break;

                    case DataType.UINT:
                    case DataType.WORD:
                        Convert.ToUInt16(value);
                        break;

                    case DataType.UDINT:
                    case DataType.DWORD:
                        Convert.ToUInt32(value);
                        break;

                    case DataType.ULINT:
                    case DataType.LWORD:
                        Convert.ToUInt64(value);
                        break;

                    case DataType.REAL:
                        Convert.ToSingle(value, info);
                        break;

                    case DataType.LREAL:
                        Convert.ToDouble(value, info);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }

        public void SetDefValue(string value)
        {
            if(_param.ParamDataType == DataType.BOOL)
                _defValue = value.ToUpper();
            else
                _defValue = value;
        }

        public XmlElement XmlSignal
        {
            set 
            {
                if (_param.ParamDataType == DataType.STRUCT ||
                    _param.ParamDataType == DataType.ARRAY ||
                    _param.ParamDataType == DataType.UDT)
                return;

                _xmlSignal = value; 
            
            }
            get { return _xmlSignal; }
        }
    }
}
