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

namespace Mpal.Model
{
    public class Unit
    {
        private string      _name = "Unit";
        private ulong       _version;     
        private string      _description = "";
        private Hashtable   _functions = new Hashtable();
        private Hashtable   _types = new Hashtable();
        private uint[]      _userVersion = new uint[3];        

        public Unit()
        {
        }

        public void Strip()
        {
            foreach (DictionaryEntry entry in this.Functions)
            {
                Function func = entry.Value as Function;
                foreach (Parameter param in func.Parameters)
                    param.Name = " ";
            }
        }

        public static void WriteString(string str, BinaryWriter bw)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            bw.Write(encoding.GetBytes(str));
            bw.Write((byte)0);
        }

        public static string ReadString(BinaryReader br)
        {
            string str = "";
            char ch = br.ReadChar();
            while (ch != '\0')
            {
                str += ch;
                ch = br.ReadChar();
            }

            return str;
        }


        public void Serialise(Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            //General unit info.
            bw.Write(Version);
            WriteString(Name, bw);

            bw.Write(UserVersion[0]);
            bw.Write(UserVersion[1]);
            bw.Write(UserVersion[2]);

            WriteString(Description, bw);

            //Unit types.
            bw.Write((uint)Types.Count);
            foreach (DictionaryEntry entry in Types)
            {
                Parameter param = (Parameter)entry.Value;
                param.Serialise(stream, this);
            }

            //Functions.
            bw.Write((uint)Functions.Count);

            foreach (DictionaryEntry entry in Functions)
            {
                //General function info.
                Function func = (Function)entry.Value;
                func.Serialise(stream);
            }

            bw.Flush();
        }

        public void Deserialise(Stream stream)
        {
            ClearAll();
            BinaryReader br = new BinaryReader(stream);

            ulong version = br.ReadUInt64();
            string name = ReadString(br);


            Name = name;
            Version = version;

            UserVersion[0] = br.ReadUInt32();
            UserVersion[1] = br.ReadUInt32();
            UserVersion[1] = br.ReadUInt32();
            Description = ReadString(br);

            if( version < 3)
                ReadString(br);

            LoadTypes(br);

            if (version < 3)
                LoadConstants(br);

            LoadFunctions(br);
            LoadDefaultValues();
        }

        private void LoadConstants(BinaryReader br)
        {
            uint count = br.ReadUInt32();

            for (uint i = 0; i < count; ++i)
            {
                //Type
                DataType type = (DataType)br.ReadByte();
                //Name
                string name = ReadString(br);
                //UID
                ulong uid = br.ReadUInt64();
                //Value
                object value = LoadDefaultValue(type, br);
            }
        }

        private object LoadDefaultValue(DataType dataType, BinaryReader br)
        {
            switch (dataType)
            {
                case DataType.ARRAY:
                    return null;

                case DataType.BOOL:
                    return (br.ReadByte() == 1);

                case DataType.BYTE:
                    return br.ReadByte();

                case DataType.DINT:
                    return br.ReadInt32();

                case DataType.DWORD:
                    return br.ReadUInt32();

                case DataType.INT:
                    return br.ReadInt16();

                case DataType.LINT:
                    return br.ReadInt64();

                case DataType.LREAL:
                    return br.ReadDouble();

                case DataType.LWORD:
                    return br.ReadUInt64();

                case DataType.REAL:
                    return br.ReadSingle();

                case DataType.SINT:
                    return br.ReadSByte();

                case DataType.STRING:
                    return ReadString(br);

                case DataType.STRUCT:
                    return null;

                case DataType.UDINT:
                    return br.ReadUInt32();

                case DataType.UDT:
                    return null;

                case DataType.UINT:
                    return br.ReadUInt16();

                case DataType.ULINT:
                    return br.ReadUInt64();

                case DataType.USINT:
                    return br.ReadByte();

                case DataType.WORD:
                    return br.ReadUInt16();

                case DataType.WSTRING:
                    return null;
            }

            return null;
        }
        private void LoadDefaultValues()
        {
            foreach (DictionaryEntry entry in Types)
            {
                Parameter param = (Parameter)entry.Value;
                param.LoadParamDefaultValue(this);
            }

            foreach (DictionaryEntry entry in Functions)
            {
                Function function = (Function)entry.Value;

                foreach (Parameter param in function.Parameters)
                    param.LoadParamDefaultValue(this);
            }
        }

        private void LoadTypes(BinaryReader br)
        {
            uint noOfTypes = br.ReadUInt32();

            for (uint i = 0; i < noOfTypes; ++i)
            {
                Parameter param = new Parameter();
                param.Deserialise(br.BaseStream, this);
                Types.Add(param.Name, param);
            }
        }

        private void LoadFunctions(BinaryReader br)
        {
            uint noOfFunc = br.ReadUInt32();

            for (uint i = 0; i < noOfFunc; ++i)
            {
                //General function info.
                Function func = new Function(this);
                func.Deserialise(br.BaseStream);
                //Add the function to the unit.
                Functions.Add(func.Name, func);
            }
        }

        public void Clear()
        {
            _functions.Clear();
            _types.Clear();
        }

        public void ClearAll()
        {
            _functions.Clear();
            _types.Clear();
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public ulong Version
        {
            get { return _version; }
            set { _version = value; }
        }

        public string Description
        {
            set { _description = value;}
            get { return _description;}
        }

        public uint[] UserVersion
        {
            get { return _userVersion; }
        }

        public Function SearchFunction(string funcName)
        {                        
            if (!this.Functions.ContainsKey(funcName))
                return null;

            return (Function)Functions[funcName];
        }

        public Function GetFunctionByLine(int line)
        {
            foreach (DictionaryEntry entry in _functions)
            {
                Function func = (Function) entry.Value;
                if (line >= func.LineBegin && line <= func.LineEnd)
                    return func;
            }
            return null;
        }

        public Parameter SearchUDT(string udtName)
        {
            if (Types.ContainsKey(udtName))
                return (Parameter)Types[udtName];

            return null;
        }

        public Parameter GetUDTBaseType(string udtName)
        {

            if (Types.ContainsKey(udtName))
            {
                Parameter param = (Parameter)Types[udtName];

                if (param.ParamDataType == DataType.UDT)
                    return GetUDTBaseType(param.TypeId);
                else
                    return param;
            }
            return null;
        }

        public Function Program
        {
            get 
            {
                foreach (DictionaryEntry entry in _functions)
                {
                    Function f = (Function)entry.Value;
                    if (f.FuncType == Function.Type.PG)
                        return f;
                }

                return null;
            }
        }

        public Hashtable Functions
        {
            get { return _functions; }
        }

        public Hashtable Types
        {
            get { return _types; }
        }

    }
}
