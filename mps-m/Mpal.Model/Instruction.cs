/***************************************************************************
 *   Copyright (C) 2006-2008 by Laurentiu-Gheorghe Crisan                  *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;

using System.Text;

namespace Mpal.Model
{
    public class Instruction
    {
        private InstructionCode _code;
        private ulong _uid;
        private Operand _result = new Operand();
        private Operand _op1 = new Operand();
        private Operand _op2 = new Operand();
        private string _call;

        public Instruction()
        {
        }

        public Instruction(InstructionCode code, ulong uid,Operand result, Operand op1, Operand op2)
        {
            _code   = code;
            _result = result;
            _op1    = op1;
            _op2    = op2;
            _uid    = uid;
        }

        private void WriteOpConstant(object value, BinaryWriter bw)
        {
            byte[] buffer = new byte[sizeof(ulong)];
            MemoryStream stream = new MemoryStream(buffer);
            BinaryWriter tbr = new BinaryWriter(stream);

            if (value is Byte)
            {
                tbr.Write((byte)value);
            }
            else if (value is Boolean)
            {
                if ((bool)value)
                    tbr.Write((byte)1);
                else
                    tbr.Write((byte)0);
            }
            else if (value is SByte)
            {
                tbr.Write((sbyte)value);
            }
            else if (value is Int16)
            {
                tbr.Write((short)value);
            }
            else if (value is UInt16)
            {
                tbr.Write((ushort)value);
            }
            else if (value is Int32)
            {
                tbr.Write((int)value);
            }
            else if (value is UInt32)
            {
                tbr.Write((uint)value);
            }
            else if (value is Int64)
            {
                tbr.Write((long)value);
            }
            else if (value is UInt64)
            {
                tbr.Write((ulong)value);
            }
            else if (value is Single)
            {
                tbr.Write((float)value);
            }
            else if (value is Double)
            {
                tbr.Write((double)value);
            }
            else
            {
                tbr.Write((byte[])value);
            }

            tbr.Flush();

            tbr.Seek(0, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(stream);
            bw.Write(br.ReadBytes(8));
            bw.Flush();
        }

        private object ReadOpConstant(BinaryReader br)
        {
            return br.ReadBytes(sizeof(ulong));
        }

        public void Deserialise(Stream stream, Unit unit)
        {
            //Instruction code
            BinaryReader br = new BinaryReader(stream);

            InstCode = (InstructionCode)br.ReadUInt32();
            UID = br.ReadUInt64();
            if (InstCode == InstructionCode.Call)
            {
                this.CallFunction = Unit.ReadString(br);                
                return;
            }

            //Result
            this.Result.OpType = (Operand.Type) br.ReadByte();
            this.Result.Offset =  br.ReadUInt32();
            
            if (unit.Version == 2 || unit.Version == 1)
                br.ReadUInt64();

            //op1
            this.Op1.OpType = (Operand.Type) br.ReadByte();

            if (unit.Version == 2 || unit.Version == 1)
                br.ReadUInt64();

            if (Op1.OpType == Operand.Type.Immediate)
            {
                this.Op1.ConstVal = ReadOpConstant(br);
            }
            else
            {
                this.Op1.Offset = (uint) br.ReadUInt64();
            }
            
            //op2
            this.Op2.OpType = (Operand.Type)br.ReadByte();
            
            if (unit.Version == 2 || unit.Version == 1)
                br.ReadUInt64();

            if (Op2.OpType == Operand.Type.Immediate)
            {
                this.Op2.ConstVal = ReadOpConstant(br);
            }
            else
            {
                this.Op2.Offset = (uint)br.ReadUInt64();
            }

        }

        public void Serialise(Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(stream);

            //Instruction code
            bw.Write((uint)InstCode);
            bw.Write(UID);

            if (InstCode == InstructionCode.Call)
            {
                Unit.WriteString(CallFunction, bw);
                return;
            }

            //Result
            if (Result != null)
            {
                bw.Write((byte)Result.OpType);
                //bw.Write(Result.Uid);
                bw.Write((uint)Result.Offset);
            }
            else
            {
                bw.Write((byte)0);
                //bw.Write((ulong)0);
                bw.Write((uint)0);
            }

            //op1
            if (Op1 != null)
            {
                bw.Write((byte)Op1.OpType);
                //bw.Write(Op1.Uid);

                if (Op1.OpType == Operand.Type.Immediate)
                    WriteOpConstant(Op1.ConstVal, bw);
                else
                    bw.Write((ulong)Op1.Offset);
            }
            else
            {
                bw.Write((byte)0);
                //bw.Write((ulong)0);
                bw.Write((ulong)0);
            }

            //op2
            if (Op2 != null)
            {
                bw.Write((byte)Op2.OpType);
                //bw.Write(Op2.Uid);

                if (Op2.OpType == Operand.Type.Immediate)
                    WriteOpConstant(Op2.ConstVal, bw);
                else
                    bw.Write((ulong)Op2.Offset);
            }
            else
            {
                bw.Write((byte)0);
                //bw.Write((ulong)0);
                bw.Write((ulong)0);
            }
            bw.Flush();
        }

        public InstructionCode InstCode
        {
            get { return _code; }
            set { _code = value; }
        }

        public Operand Result
        {
            get { return _result; }
        }

        public Operand Op1
        {
            get { return _op1; }
        }

        public Operand Op2
        {
            get { return _op2; }
        }

        public ulong UID
        {
            get { return _uid; }
            set { _uid = value; }
        }

        public string CallFunction
        {
            set { _call = value; }
            get { return _call; }
        }
    }
}
