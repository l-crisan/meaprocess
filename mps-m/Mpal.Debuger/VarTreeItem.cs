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
using System.Globalization;
using System.Text;
using System.Xml;
using Mpal.Model;
using System.Drawing;

namespace Mpal.Debugger
{
    internal class VarTreeItem
    {
        private Parameter _param;
        private string _value = "";
        
        public VarTreeItem(Parameter param)
        {
            _param = param;
            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            switch (_param.ParamDataType)
            {
                case DataType.ARRAY:
                case DataType.STRUCT:
                case DataType.UDT:
                case DataType.FB:
                    _value = "";
                    break;

                default:
                    if (_param.DefaultValue != null)
                        _value = Convert.ToString(_param.DefaultValue, info);
                    else
                        _value = "0";
                    break;
            }
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
                        return Mpal.Debugger.Resource.Struct.ToBitmap();
                    default:
                        return Mpal.Debugger.Resource.Variable.ToBitmap();
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

        public string Value
        {
            set 
            {
               if (!ValidateDefaultValue(value))
                    return;

                if (_param.ParamDataType == DataType.ENUM)
                {
                    for(int i =0 ; i < _param.EnumList.Count; ++i)
                    {
                        if (value == _param.EnumList[i])
                            _value = i.ToString();
                    }                    
                }
                else
                {
                    _value = value;
                }
            }
            
            get 
            {
                if (_param.ParamDataType == DataType.ENUM)
                {
                    int index = Convert.ToInt32(_value);
                    return _param.EnumList[index];
                }
                else
                {
                    return _value;
                }
            }
        }

        public string GetRowValue()
        {
            return _value;
        }

        public void SetRowValue(string value)
        {
            _value = value;
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

                    case DataType.ENUM:
                    {
                        if (!_param.EnumList.Contains(value))
                            return false;
                    }
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
    }
}
