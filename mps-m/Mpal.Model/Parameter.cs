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
using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace Mpal.Model
{   
    public class Parameter
    {
        public enum Access
        {
	        Input = 1,
	        Output,
	        InOut,
	        Var,
            VarConst,
            VarTemp,
            VarTempConst,
            None
        }

        private string          _name;
        private string          _typeName = "";
        private DataType        _dataType;
        private Access          _access;
        private object          _defValue;
        private uint            _offset;
        private ulong           _uid;
        private string          _typeId = "";
        private int             _index;
        private uint            _size;
        private List<Parameter> _structure  = new List<Parameter>();
        private List<Dimension> _dimensions = new List<Dimension>();
        private ITree           _astNode    = null;
        private byte[]          _buffer;
        private List<string> _enumList = new List<string>();

        public Parameter()
        {
        }

        public Parameter(Parameter param)
        {
            _name = param._name;
            _typeName =  param._typeId;
            _dataType = param._dataType;
            _access = param._access;
            _defValue = param._defValue;
            _offset = param._offset;
            _uid = param._uid;
            _typeId = param._typeId;
            _index = param._index;
            _size = param._size;
            _astNode    = param._astNode;

            foreach (Parameter p in param._structure)
                _structure.Add(new Parameter(p));

            foreach (Dimension d in param._dimensions)
                _dimensions.Add(d);            
        }

        public Parameter(string name, ulong uid, Access access, string typeId, string typeName)
        {
            _name = name;
            _dataType = DataType.UDT;
            _access = access;
            _uid = uid;
            _typeId = typeId;
            _typeName = typeName;
        }

        public Parameter(string name, ulong uid, DataType dataType, Access access, string typeId)
        {
            _name = name;
            _dataType = dataType;
            _access = access;
            _uid = uid;
            _typeId = typeId;
        }
        
        public void Deserialise(Stream stream, Unit unit)
        {
            BinaryReader br = new BinaryReader(stream);

            _name = Unit.ReadString(br);
            ulong uid = br.ReadUInt64();
            _access  = (Parameter.Access)br.ReadByte();
            _typeId = Unit.ReadString(br);
            _size = br.ReadUInt32();
            _typeName = Unit.ReadString(br);
            uint offset = br.ReadUInt32();
            uint fbOffset = br.ReadUInt32();
            ParamDataType = (DataType)br.ReadByte();            

            switch (ParamDataType)
            {
                case DataType.STRUCT:
                    uint noOfParam = br.ReadUInt32();

                    for (int i = 0; i < noOfParam; ++i)
                    {
                        Parameter param = new Parameter();
                        param.Deserialise(stream, unit);
                        Structure.Add(param);
                    }
                    break;

                case DataType.ARRAY:
                    {
                        Parameter param = new Parameter();
                        param.Deserialise(stream, unit);
                        Structure.Add(param);
                        uint noOfDim = br.ReadUInt32();

                        for (int i = 0; i < noOfDim; ++i)
                        {
                            Dimension dim = new Dimension(br.ReadInt64(), br.ReadInt64());
                            Dimensions.Add(dim);
                        }
                    }
                    break;

                case DataType.ENUM:
                    {
                        uint noOfEnum = br.ReadUInt32();
                        for (int i = 0; i < noOfEnum; ++i)
                        {
                            string str = Unit.ReadString(br);
                            EnumList.Add(str);
                        }
                    }
                    break;
            }

            uint lenght = br.ReadUInt32();

            if (lenght != 0)
                DefaultValueBuffer = br.ReadBytes((int)lenght);
        }

        public void LoadParamDefaultValue(Unit unit)
        {
            if (ParamDataType == DataType.STRUCT)
            {
                foreach (Parameter memberParam in Structure)
                    memberParam.LoadParamDefaultValue(unit);
            }

            if (DefaultValueBuffer == null)
                return;

            MemoryStream mm = new MemoryStream(DefaultValueBuffer);
            BinaryReader br = new BinaryReader(mm);

            switch (ParamDataType)
            {
                case DataType.STRUCT:
                    DefaultValue = LoadStructDefaultValue(br, null, unit);
                    break;

                case DataType.ARRAY:
                    DefaultValue = LoadArrayDefaultValue(br, unit);
                    break;

                case DataType.UDT:
                case DataType.FB:
                    DefaultValue = LoadUDTDefaultValue(br, unit);
                    break;

                default:
                    DefaultValue = LoadDefaultValue(br);
                    break;
            }
        }

        private object LoadStructDefaultValue(BinaryReader br, Function f, Unit unit)
        {
            uint count = br.ReadUInt32();

            if (count == 0)
                return null;

            ArrayList resList = new ArrayList();
            DictionaryEntry entry;

            for (int i = 0; i < count; ++i)
            {
                int index = br.ReadInt32();
                Parameter curParam = null;
                if (f == null)
                    curParam = Structure[index];
                else
                    curParam = f.Parameters[index];

                entry = new DictionaryEntry();
                entry.Key = index;

                switch (curParam.ParamDataType)
                {
                    case DataType.STRUCT:
                        entry.Value = curParam.LoadStructDefaultValue(br, null, unit);
                        break;

                    case DataType.ARRAY:
                        entry.Value = curParam.LoadArrayDefaultValue(br, unit);
                        break;

                    case DataType.UDT:
                    case DataType.FB:
                        entry.Value = curParam.LoadUDTDefaultValue(br, unit);
                        break;

                    default:
                        entry.Value = curParam.LoadDefaultValue(br);
                        break;
                }

                resList.Add(entry);
            }

            return resList;
        }

        private object LoadArrayDefaultValue(BinaryReader br, Unit unit)
        {
            uint count = br.ReadUInt32();

            if (count == 0)
                return null;

            ArrayList values = new ArrayList();

            object obj;

            Parameter arrayTypeParam = Structure[0];

            for (int i = 0; i < count; ++i)
            {
                switch (arrayTypeParam.ParamDataType)
                {
                    case DataType.UDT:
                    case DataType.FB:
                        obj = arrayTypeParam.LoadUDTDefaultValue(br, unit);
                        break;

                    case DataType.STRUCT:
                        obj = arrayTypeParam.LoadStructDefaultValue(br, null, unit);
                        break;

                    default:
                        obj = arrayTypeParam.LoadDefaultValue(br);
                        break;
                }

                values.Add(obj);
            }

            return values;
        }

        private object LoadUDTDefaultValue(BinaryReader br, Unit unit)
        {
            Parameter udtParam = unit.SearchUDT(TypeName);
            if (udtParam == null)
            {
                Function f = unit.SearchFunction(TypeName);
                return LoadStructDefaultValue(br, f, unit);
            }

            switch (udtParam.ParamDataType)
            {
                case DataType.ARRAY:
                    return udtParam.LoadArrayDefaultValue(br, unit);

                case DataType.STRUCT:
                    return udtParam.LoadStructDefaultValue(br, null, unit);

                case DataType.UDT:
                    return udtParam.LoadUDTDefaultValue(br, unit);

                default:
                    return udtParam.LoadDefaultValue(br);
            }
        }
        private object LoadDefaultValue(BinaryReader br)
        {
            switch (this.ParamDataType)
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
                    return Unit.ReadString(br);

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

        public Parameter FindMember(string member, Unit module)
        {
            switch(ParamDataType)
            {
                case DataType.UDT:
                if (module.Types.ContainsKey(this.TypeName))
                {
                    Parameter parameter = (Parameter)module.Types[this.TypeName];
                    return parameter.FindMember(member, module);
                }
                break;
                
                case DataType.FB:
                {
                    if (!module.Functions.ContainsKey(this.TypeName))
                        return null;

                    Function func = (Function)module.Functions[this.TypeName];
                    return func.GetParameter(member);
                }

                default:
                    for (int i = 0; i < _structure.Count; ++i)
                    {
                        if (_structure[i].Name == member)
                            return _structure[i];
                    }
                break;
            }

            return null;
        }

        public ITree ASTNode
        {
            get { return _astNode; }
            set { _astNode = value; }
        }

        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }

        public string TypeId
        {
            get{ return _typeId;}
            set { _typeId = value; }
        }

        public uint Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        public ulong UID
        {
            get { return _uid; }
            set { _uid = value; }
        }

        private uint CalcFBSizeInternal(Unit module)
        {
            if (!module.Functions.ContainsKey(this.TypeName))
                return 0;

            Function func = (Function)module.Functions[this.TypeName];
            return func.CalcFBSize(module);
        }

        public void CalcOffsets()
        {
            switch (_dataType)
            {
                case DataType.ARRAY:
                {
                    Parameter arrayOf = _structure[0];
                    switch(arrayOf.ParamDataType)
                    {
                        case DataType.STRUCT:
                        {
                            uint offset = 0;
                            foreach (Parameter structParam in arrayOf.Structure)
                            {
                                structParam.Offset = offset;
                                offset += structParam.Size;
                            }
                        }
                        break;
                    }             
                }
                break;
            }
        }

        public uint CalcSize(Unit unit, Parameter parent)
        {
            if (this == parent)
                throw new Exception("Recursivly type definition");

            switch(_dataType)
            {
                case DataType.UDT:
                    Parameter udtParam = SearchUDT(unit);

                    if (udtParam != null)
                        _size = udtParam.CalcSize(unit, this);
                    else
                        _size = CalcFBSizeInternal(unit);                        
                break;

                case DataType.FB:
                    _size = CalcFBSizeInternal(unit);
                break;

                case DataType.STRUCT:
                    _size = 0;

                    foreach (Parameter stParam in _structure)
                        _size += stParam.CalcSize(unit, parent);
                break;
                
                case DataType.BOOL:
                case DataType.BYTE:
                case DataType.SINT:
                case DataType.USINT:
                    _size = 1;
                break;

                case DataType.DINT:
                case DataType.DWORD:
                case DataType.REAL:
                case DataType.UDINT:
                case DataType.ENUM:
                    _size= 4;
                break;

                case DataType.INT:
                case DataType.UINT:
                case DataType.WORD:
                    _size = 2;
                break;
                
                case DataType.LINT:
                case DataType.LREAL:
                case DataType.LWORD:
                case DataType.ULINT:                
                    _size= 8;
                break;
                
                case DataType.STRING:
                case DataType.WSTRING:
                    throw new Exception("Not implemented");
                
                case DataType.ARRAY:
                {
                    Parameter param = _structure[0];
                    param.CalcSize(unit, this);

                    _size = 1;

                    foreach (Dimension dim in _dimensions)
                        _size *= (uint)((dim.To - dim.From) + 1);

                    _size *= param.Size;
                 }
                break;
            }

            return _size;
        }


        private void WriteStructDefaultValue(BinaryWriter bw, object defValue, Function f, Unit unit)
        {
            if (defValue == null)
                return;

            if (!(defValue is ArrayList))
                return;

            ArrayList defValueArray = (ArrayList)defValue;
            WriteMemberDefaultValue(defValueArray, bw, f, unit);
        }

        private void WriteArrayDefaultValue(BinaryWriter bw, object value, Unit unit)
        {
            if (value == null)
                return;

            ArrayList values = (ArrayList)value;

            bw.Write((uint)(values.Count));

            Parameter arrayType = Structure[0];

            switch (arrayType.ParamDataType)
            {
                case DataType.STRUCT:
                    foreach (object obj in values)
                    {
                        if( obj != null)
                            arrayType.WriteStructDefaultValue(bw, obj, null, unit);
                        else
                            arrayType.WriteStructDefaultValue(bw, new ArrayList(), null, unit);
                    }
                    break;

                case DataType.UDT:
                case DataType.FB:
                    foreach (object obj in values)
                        arrayType.WriteUDTDefaultValue(bw, obj, unit, true);
                    break;

                default:
                    foreach (object obj in values)
                        WriteDefaultValue(obj, arrayType.ParamDataType, bw);
                    break;
            }
        }

        private void WriteUDTDefaultValue(BinaryWriter bw, object value, Unit unit, bool generateNull)
        {
            if (value == null && !generateNull)
                return;

            Parameter udtParam = unit.SearchUDT(TypeName);

            if (udtParam == null)
            {
                Function f = unit.SearchFunction(TypeName);
                WriteStructDefaultValue(bw, value, f, unit);
                return;
            }

            switch (udtParam.ParamDataType)
            {
                case DataType.ARRAY:
                    udtParam.WriteArrayDefaultValue(bw, value, unit);
                    break;

                case DataType.STRUCT:
                    if(value != null)
                        udtParam.WriteStructDefaultValue(bw, value, null, unit);
                    else
                        udtParam.WriteStructDefaultValue(bw, new ArrayList(), null, unit);
                break;

                case DataType.UDT:
                    udtParam.WriteUDTDefaultValue(bw, value, unit, generateNull);
                    break;

                default:
                    WriteDefaultValue(value, udtParam.ParamDataType, bw);
                break;
            }
        }
        private void WriteMemberDefaultValue(ArrayList list, BinaryWriter bw, Function f, Unit unit)
        {
            bw.Write((uint)list.Count);
            Parameter curParam = null;

            foreach (DictionaryEntry entry in list)
            {
                int paramIndex = (int)entry.Key;
                bw.Write(paramIndex);

                if (f == null)
                    curParam = Structure[paramIndex];
                else
                    curParam = f.Parameters[paramIndex];

                switch (curParam.ParamDataType)
                {
                    case DataType.ARRAY:
                        curParam.WriteArrayDefaultValue(bw, entry.Value, unit);
                        break;

                    case DataType.STRUCT:
                        curParam.WriteStructDefaultValue(bw, entry.Value, null, unit);
                        break;

                    case DataType.UDT:
                        curParam.WriteUDTDefaultValue(bw, entry.Value, unit, false);
                        break;

                    case DataType.FB:
                        curParam.WriteUDTDefaultValue(bw, entry.Value, unit, false);
                        break;

                    default:
                        WriteDefaultValue(entry.Value, curParam.ParamDataType, bw);
                        break;
                }
            }
        }

        private void WriteDefaultValue(object obj, DataType type, BinaryWriter bw)
        {
            switch (type)
            {
                case DataType.ENUM:
                    if (obj == null)
                        bw.Write((int)0);
                    else
                        bw.Write((int)obj);
                    break;

                case DataType.BOOL:
                    if (obj == null)
                    {
                        bw.Write((byte)0);
                    }
                    else
                    {
                        if ((bool)obj)
                            bw.Write((byte)1);
                        else
                            bw.Write((byte)0);
                    }
                    break;

                case DataType.BYTE:
                    if (obj == null)
                        bw.Write((byte)0);
                    else
                        bw.Write((byte)obj);
                    break;

                case DataType.DINT:
                    if (obj == null)
                        bw.Write((int)0);
                    else
                        bw.Write((int)obj);
                    break;

                case DataType.DWORD:
                    if (obj == null)
                        bw.Write((uint)0);
                    else
                        bw.Write((uint)obj);
                    break;

                case DataType.INT:
                    if (obj == null)
                        bw.Write((short)0);
                    else
                        bw.Write((short)obj);
                    break;

                case DataType.LINT:
                    if (obj == null)
                        bw.Write((long)0);
                    else
                        bw.Write((long)obj);
                    break;

                case DataType.LREAL:
                    if (obj == null)
                        bw.Write((double)0);
                    else
                        bw.Write((double)obj);
                    break;

                case DataType.LWORD:
                    if (obj == null)
                        bw.Write((ulong)0);
                    else
                        bw.Write((ulong)obj);
                    break;

                case DataType.REAL:
                    if (obj == null)
                        bw.Write((float)0);
                    else
                        bw.Write((float)obj);
                    break;
                case DataType.SINT:
                    if (obj == null)
                        bw.Write((sbyte)0);
                    else
                        bw.Write((sbyte)obj);
                    break;

                case DataType.STRING:
                    if (obj != null)
                        Unit.WriteString((string)obj, bw);
                    else
                        bw.Write((byte)0);
                    break;

                case DataType.UDINT:
                    if (obj == null)
                        bw.Write((uint)0);
                    else
                        bw.Write((uint)obj);
                    break;

                case DataType.UINT:
                    if (obj == null)
                        bw.Write((ushort)0);
                    else
                        bw.Write((ushort)obj);
                    break;

                case DataType.ULINT:
                    if (obj == null)
                        bw.Write((ulong)0);
                    else
                        bw.Write((ulong)obj);
                    break;

                case DataType.USINT:
                    if (obj == null)
                        bw.Write((byte)0);
                    else
                        bw.Write((byte)obj);
                    break;
                case DataType.WORD:
                    if (obj == null)
                        bw.Write((ushort)0);
                    else
                        bw.Write((UInt16)obj);
                    break;
                case DataType.WSTRING:
                    break;
            }
        }
        public void Serialise(Stream stream, Unit unit)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            Unit.WriteString(Name, bw);
            bw.Write(UID);
            bw.Write((byte)ParamAccess);
            Unit.WriteString(TypeId, bw);
            bw.Write(Size);
            Unit.WriteString(TypeName, bw);
            bw.Write((uint)Offset);
            bw.Write((uint)0);
            bw.Write((byte)ParamDataType);

            MemoryStream mm = new MemoryStream();
            BinaryWriter mmbr = new BinaryWriter(mm);

            switch (ParamDataType)
            {
                case DataType.STRUCT:
                    bw.Write((uint)Structure.Count);

                    foreach (Parameter childParam in Structure)
                        childParam.Serialise(stream, unit);

                    WriteStructDefaultValue(mmbr, DefaultValue, null, unit);
                    break;

                case DataType.ARRAY:
                    //The array type.
                    Structure[0].Serialise(stream, unit);

                    //The dimensions.
                    bw.Write((uint)Dimensions.Count);
                    foreach (Dimension dim in Dimensions)
                    {
                        bw.Write(dim.From);
                        bw.Write(dim.To);
                    }

                    //The defualt value.
                    WriteArrayDefaultValue(mmbr, DefaultValue, unit);
                    break;

                case DataType.UDT:
                case DataType.FB:
                    WriteUDTDefaultValue(mmbr, DefaultValue, unit, false);
                    break;

                case DataType.ENUM:

                    bw.Write((uint)EnumList.Count);

                    foreach (string en in EnumList)
                        Unit.WriteString(en, bw);

                    WriteDefaultValue(DefaultValue, ParamDataType, mmbr);
                    break;

                default:
                    WriteDefaultValue(DefaultValue, ParamDataType, mmbr);
                    break;
            }

            bw.Write((uint)mm.Length);

            if (mm.Length != 0)
            {
                mmbr.Flush();
                mmbr.Seek(0, SeekOrigin.Begin);
                bw.Write(mm.GetBuffer(), 0, (int)mm.Length);
            }
            bw.Flush();
        }

        private Parameter SearchUDT(Unit unit)
        {
            Parameter udtParam = null;

            if (unit.Types.ContainsKey(this.TypeName))
            {
                udtParam = (Parameter)unit.Types[this.TypeName];
                return udtParam;
            }

            return udtParam;
        }

        public uint Size
        {
            get {     return _size;}

            set { _size = value; }
        }
        
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public DataType ParamDataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        public Access ParamAccess
        {
            get { return _access; }
            set { _access = value; }
        }

        public List<string> EnumList
        {
            get { return _enumList; }
        }

        public object DefaultValue
        {
            get { return _defValue; }
            set { _defValue = value; }
        }

        public byte[] DefaultValueBuffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        public bool AddMember(Parameter memberParam)
        {
            foreach(Parameter param in _structure)
            {
                if (param.Name == memberParam.Name)
                    return false;
            }

            _structure.Add(memberParam);
            return true;
        }

        public List<Parameter> Structure
        {
            get { return _structure; }
            set { _structure = value; } 
        }

        public List<Dimension> Dimensions
        {
            get { return _dimensions; }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}
