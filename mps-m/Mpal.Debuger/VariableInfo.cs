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
using System.IO;
using System.Text;
using Mpal.Model;

namespace Mpal.Debugger
{
    public class VariableInfo
    {
        private string _name;
        private byte[] _data;
        private string _value;
        private DataType _dataType;
        private string _dataTypeName;
        private List<string> _enumList = new List<string>();
        private List<VariableInfo> _variables = new List<VariableInfo>();
        private ulong _uid = 0;
        private int _offset = 0;

        public VariableInfo()
        {
        }

        public int Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public ulong UID
        {
            get { return _uid; }
            set{ _uid = value; }
        }

        public List<VariableInfo> Variables
        {
            get { return _variables; }
        }

        public List<string> EnumList
        {
            get { return _enumList; }
            set { _enumList = value; }
        }

        public DataType DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public string DataTypeName
        {
            get { return _dataTypeName; }
            set { _dataTypeName = value; }
        }

        public string Name
        {
            get { return _name; }
            set{ _name = value;}
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        
        public string Value
        {
            get { return GetValue(); }
            set { _value = value; }
        }

        private string GetValue()
        {
            if (_data == null)
                return "";

            if (_data.Length == 0)
                return "";
            

            MemoryStream mm = new MemoryStream(_data);
            BinaryReader br = new BinaryReader(mm);


            switch (_dataType)
            {
                case DataType.BOOL:
                {
                    byte b = br.ReadByte();
                    if (b != 0)
                        return "TRUE";

                    return "FALSE";
                }

                case DataType.BYTE:
                    return br.ReadByte().ToString();

                case DataType.DINT:
                        return br.ReadInt32().ToString();

               
                case DataType.UDINT:
                case DataType.DWORD:
                        return br.ReadUInt32().ToString();

                case DataType.INT:
                        {
                            short s = br.ReadInt16();
                            return s.ToString();
                        }

                case DataType.LINT:
                        return br.ReadInt64().ToString();

                case DataType.LREAL:
                        return br.ReadDouble().ToString();

                case DataType.REAL:
                        return br.ReadSingle().ToString();

                case DataType.LWORD:
                        return br.ReadUInt64().ToString();

                case DataType.SINT:
                        return br.ReadSByte().ToString();

                case DataType.UINT:
                        return br.ReadUInt16().ToString();

                case DataType.ULINT:
                        return br.ReadUInt64().ToString();

                case DataType.USINT:
                        return br.ReadByte().ToString();

                case DataType.WORD:
                        return br.ReadUInt16().ToString();

                case DataType.ENUM:
                { 
                    int index = br.ReadInt32();

                    if( index < _enumList.Count && index >= 0)
                        return _enumList[index];

                    return index.ToString();
                }
            }

            return "";
        }


    }
}
