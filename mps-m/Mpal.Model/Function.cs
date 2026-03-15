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
    public class Function
    {
        public enum Type
        {
            FC,
            FB,
            PG
        }

        private string _name;
        private Type _type;
        private Unit _unit;
        private List<Parameter>   _parameters = new List<Parameter>();
        private List<Instruction> _instructions = new List<Instruction>();
        private uint              _stackSize = 0;
        private uint              _fbSize = 0;
        private ITree             _astNode;
        private int               _lineBegin;
        private int               _lineEnd;
        private Hashtable         _uid2ParamMapping = new Hashtable();
        private Hashtable         _uid2VarMapping = new Hashtable();
        private Hashtable          _operandCache = new Hashtable();

        public Function(Unit unit)
        {
            _unit = unit;
        }

        public Function(string name, Type type, Unit unit)
        {
            _unit = unit;
            _name = name;
            _type = type;
        }

        public bool IsStackParam(Parameter param)
        {
            Parameter.Access access = param.ParamAccess;

            switch (this.FuncType)
            {
                case Function.Type.PG:
                    return access == Parameter.Access.Input ||
                           access == Parameter.Access.VarTemp ||
                           access == Parameter.Access.VarConst ||
                           access == Parameter.Access.VarTempConst;

                case Function.Type.FC:
                    return access == Parameter.Access.Input ||
                           access == Parameter.Access.VarTemp ||
                           access == Parameter.Access.VarConst ||
                           access == Parameter.Access.Var ||
                           access == Parameter.Access.VarTempConst;

                case Function.Type.FB:
                    return
                           access == Parameter.Access.VarTemp ||
                           access == Parameter.Access.VarTempConst;
            }

            throw new Exception("Unknow function type : " + this.FuncType.ToString());
        }

        public Hashtable OperandCache
        {
            get { return _operandCache; }
        }

        public int LineBegin
        {
            get { return _lineBegin; }
            set { _lineBegin = value; }
        }

        public int LineEnd
        {
            get { return _lineEnd; }
            set { _lineEnd = value; }
        }

        public Hashtable Uid2Param
        {
            get { return _uid2ParamMapping; }
        }

        public Hashtable Uid2Var
        {
            get { return _uid2VarMapping; }
        }

        public ITree ASTNode
        {
            get { return _astNode; }
            set { _astNode = value; }
        }

        public string Name
        {
            get{ return _name;}
            set { _name = value; }
        }

        public Type FuncType
        {
            get { return _type; }
            set { _type = value; }
        }

        public Unit Unit
        {
            get { return _unit; }
        }

        public void Serialise(Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            Unit.WriteString(Name, bw);
            bw.Write((byte)FuncType);
            bw.Write(StackSize);
            bw.Write(FuncBlockSize);
            bw.Write(LineBegin);
            bw.Write(LineEnd);

            //Interface
            bw.Write((uint)Parameters.Count);
            foreach (Parameter param in Parameters)
                param.Serialise(stream, _unit);

            //Instructions
            bw.Write((uint)Instructions.Count);

            foreach (Instruction inst in Instructions)
                inst.Serialise(stream);

            bw.Flush();
        }

        public void Deserialise(Stream stream)
        {
            //General function info.
            BinaryReader br = new BinaryReader(stream);
            this.Name = Unit.ReadString(br);
            this.FuncType  = (Function.Type)br.ReadByte();

            StackSize = br.ReadUInt32();
            FuncBlockSize = br.ReadUInt32();

            if (_unit.Version != 1)
            {
                LineBegin = br.ReadInt32();
                LineEnd = br.ReadInt32();
            }
            
            //Load interface.
            uint noOfParam = br.ReadUInt32();
            for (uint j = 0; j < noOfParam; ++j)
            {
                Parameter param = new Parameter();
                param.Deserialise(br.BaseStream, _unit);
                param.Index = (int) j;
                Parameters.Add(param);
            }

            //Load instructions.
            LoadInstructions(br);
        }

        private void LoadInstructions(BinaryReader br)
        {
            uint count = br.ReadUInt32();

            for (uint i = 0; i < count; ++i)
            {
                Instruction inst = new Instruction();
                inst.Deserialise(br.BaseStream, _unit);
                this.Instructions.Add(inst);
            }
        }

        public uint FuncBlockSize
        {
            get
            {
                return _fbSize;
            }

            set
            {
                _fbSize = value;
            }
        }

        public bool CanSetBreakPoint(int line)
        {
            foreach (Instruction inst in _instructions)
            {
                int curLine = (int)(inst.UID >> 32);
                
                if (curLine == line)
                    return true;                
            }

            if (line == _lineEnd)
                return true;

            return false;
        }

        public uint CalcFBSize(Unit unit)
        {
            _fbSize = 0;
            foreach (Parameter param in _parameters)
            {
                if (param.ParamAccess == Parameter.Access.VarTemp ||
                    param.ParamAccess == Parameter.Access.VarTempConst)
                    continue;

                _fbSize += param.CalcSize(unit, null);
            }

            return _fbSize;
        }

        public uint StackSize
        {
            get
            {
                return _stackSize;
            }

            set
            {
                _stackSize = value;
            }

        }

        public bool AddVariable(Parameter varParam)        
        {
            foreach (Parameter param in _parameters)
                if (param.Name == varParam.Name)
                    return false;

            _parameters.Add(varParam);
            return true;
        }

        public Parameter GetParameter(string name)
        {
            foreach (Parameter param in _parameters)
            {
                if (param.Name == name)
                    return param;
            }

            return null;
        }

        public List<Parameter> Parameters
        {
            get { return _parameters; }
        }

        public List<Instruction> Instructions
        {
            get { return _instructions; }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
