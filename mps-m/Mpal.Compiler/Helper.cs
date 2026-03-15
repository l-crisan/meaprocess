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

using System.Text;
using Mpal.Model;

using Antlr.Runtime;
using Antlr.Runtime.Tree;

namespace Mpal.Compiler
{
    public class Helper
    {
        private struct ConvertionItem
        {
            public ConvertionItem(string from, string to, InstructionCode code)
            {
                FromTypeId = from;
                ToTypeId = to;
                InstCode = code;
            }

            public string FromTypeId;
            public string ToTypeId;
            public InstructionCode InstCode;
        }

        private struct GreaterTypeItem
        {
            public GreaterTypeItem(string typeId1, string typeId2, int greater)
            {
                TypeId1 = typeId1;
                TypeId2 = typeId2;
                Greater = greater;
            }

            public string TypeId1;
            public string TypeId2;
            public int Greater;
        }

        private static List<ConvertionItem> _conversionMatrix = new List<ConvertionItem>();
        private static List<GreaterTypeItem> _typeComp = new List<GreaterTypeItem>();

        static Helper()
        {            
            //Conversion table implicit
            //-------------------------------------------------------------------------------------            
            //BYTE conversion.

            _conversionMatrix.Add(new ConvertionItem("BYTE", "WORD", InstructionCode.BYTE2WORD));
            _conversionMatrix.Add(new ConvertionItem("BYTE", "DWORD", InstructionCode.BYTE2DWORD));
            _conversionMatrix.Add(new ConvertionItem("BYTE", "LWORD", InstructionCode.BYTE2LWORD));

            //WORD conversion
            _conversionMatrix.Add(new ConvertionItem("WORD", "DWORD", InstructionCode.WORD2DWORD));
            _conversionMatrix.Add(new ConvertionItem("WORD", "LWORD", InstructionCode.WORD2LWORD));

            //DWORD conversion
            _conversionMatrix.Add(new ConvertionItem("DWORD", "LWORD", InstructionCode.DWORD2LWORD));

            //INT
            _conversionMatrix.Add(new ConvertionItem("INT", "DINT", InstructionCode.INT2DINT));
            _conversionMatrix.Add(new ConvertionItem("INT", "LINT", InstructionCode.INT2LINT));
            _conversionMatrix.Add(new ConvertionItem("INT", "REAL", InstructionCode.INT2REAL));
            _conversionMatrix.Add(new ConvertionItem("INT", "LREAL", InstructionCode.INT2LREAL));


            //SINT conversion
            _conversionMatrix.Add(new ConvertionItem("SINT", "INT", InstructionCode.SINT2INT));
            _conversionMatrix.Add(new ConvertionItem("SINT", "DINT", InstructionCode.SINT2DINT));
            _conversionMatrix.Add(new ConvertionItem("SINT", "LINT", InstructionCode.SINT2LINT));
            _conversionMatrix.Add(new ConvertionItem("SINT", "REAL", InstructionCode.SINT2REAL));
            _conversionMatrix.Add(new ConvertionItem("SINT", "LREAL", InstructionCode.SINT2LREAL));

            //DINT conversion
            _conversionMatrix.Add(new ConvertionItem("DINT", "LINT", InstructionCode.DINT2LINT));
            _conversionMatrix.Add(new ConvertionItem("DINT", "REAL", InstructionCode.DINT2REAL));
            _conversionMatrix.Add(new ConvertionItem("DINT", "LREAL", InstructionCode.DINT2LREAL));

            //LINT conversion
            _conversionMatrix.Add(new ConvertionItem("LINT", "REAL", InstructionCode.DINT2REAL));
            _conversionMatrix.Add(new ConvertionItem("LINT", "LREAL", InstructionCode.DINT2LREAL));

            //USINT
            _conversionMatrix.Add(new ConvertionItem("USINT", "UINT", InstructionCode.USINT2UINT));
            _conversionMatrix.Add(new ConvertionItem("USINT", "UDINT", InstructionCode.USINT2UDINT));
            _conversionMatrix.Add(new ConvertionItem("USINT", "ULINT", InstructionCode.USINT2ULINT));
            _conversionMatrix.Add(new ConvertionItem("USINT", "REAL", InstructionCode.USINT2REAL));
            _conversionMatrix.Add(new ConvertionItem("USINT", "LREAL", InstructionCode.USINT2LREAL));

            //UINT
            _conversionMatrix.Add(new ConvertionItem("UINT", "UDINT", InstructionCode.UINT2UDINT));
            _conversionMatrix.Add(new ConvertionItem("UINT", "ULINT", InstructionCode.UINT2ULINT));
            _conversionMatrix.Add(new ConvertionItem("UINT", "REAL", InstructionCode.UINT2REAL));
            _conversionMatrix.Add(new ConvertionItem("UINT", "LREAL", InstructionCode.UINT2LREAL));

            //UDINT
            _conversionMatrix.Add(new ConvertionItem("UDINT", "ULINT", InstructionCode.UDINT2ULINT));
            _conversionMatrix.Add(new ConvertionItem("UDINT", "REAL", InstructionCode.UDINT2REAL));
            _conversionMatrix.Add(new ConvertionItem("UDINT", "LREAL", InstructionCode.UDINT2LREAL));


            //ULINT
            _conversionMatrix.Add(new ConvertionItem("ULINT", "REAL", InstructionCode.ULINT2REAL));
            _conversionMatrix.Add(new ConvertionItem("ULINT", "LREAL", InstructionCode.ULINT2LREAL));

            //REAL conversion
            _conversionMatrix.Add(new ConvertionItem("REAL", "LREAL", InstructionCode.REAL2LREAL));

            //Type comparation table
            //-------------------------------------------------------------------------------------            
            _typeComp.Add(new GreaterTypeItem("BOOL", "BOOL", 0));

            _typeComp.Add(new GreaterTypeItem("BYTE", "BYTE", 0));
            _typeComp.Add(new GreaterTypeItem("BYTE", "WORD", 1));
            _typeComp.Add(new GreaterTypeItem("BYTE", "DWORD", 1));
            _typeComp.Add(new GreaterTypeItem("BYTE", "LWORD", 1));

            _typeComp.Add(new GreaterTypeItem("WORD", "BYTE", 0));
            _typeComp.Add(new GreaterTypeItem("WORD", "WORD", 0));
            _typeComp.Add(new GreaterTypeItem("WORD", "DWORD", 1));
            _typeComp.Add(new GreaterTypeItem("WORD", "LWORD", 1));

            _typeComp.Add(new GreaterTypeItem("DWORD", "BYTE", 0));
            _typeComp.Add(new GreaterTypeItem("DWORD", "WORD", 0));
            _typeComp.Add(new GreaterTypeItem("DWORD", "DWORD", 0));
            _typeComp.Add(new GreaterTypeItem("DWORD", "LWORD", 1));

            _typeComp.Add(new GreaterTypeItem("LWORD", "BYTE", 0));
            _typeComp.Add(new GreaterTypeItem("LWORD", "WORD", 0));
            _typeComp.Add(new GreaterTypeItem("LWORD", "DWORD", 0));
            _typeComp.Add(new GreaterTypeItem("LWORD", "LWORD", 0));

            _typeComp.Add(new GreaterTypeItem("SINT","SINT",0));
            _typeComp.Add(new GreaterTypeItem("SINT","INT",1));
            _typeComp.Add(new GreaterTypeItem("SINT","DINT",1));
            _typeComp.Add(new GreaterTypeItem("SINT","LINT",1));
            _typeComp.Add(new GreaterTypeItem("SINT","USINT",0));
            _typeComp.Add(new GreaterTypeItem("SINT","UINT", 1));
            _typeComp.Add(new GreaterTypeItem("SINT","UDINT",1));
            _typeComp.Add(new GreaterTypeItem("SINT","ULINT", 1));
            _typeComp.Add(new GreaterTypeItem("SINT","REAL", 1));
            _typeComp.Add(new GreaterTypeItem("SINT","LREAL", 1));

            _typeComp.Add(new GreaterTypeItem("INT", "SINT",0));
            _typeComp.Add(new GreaterTypeItem("INT", "INT", 0));
            _typeComp.Add(new GreaterTypeItem("INT", "DINT",1));
            _typeComp.Add(new GreaterTypeItem("INT", "LINT",1));
            _typeComp.Add(new GreaterTypeItem("INT", "USINT",0));
            _typeComp.Add(new GreaterTypeItem("INT", "UINT", 0));
            _typeComp.Add(new GreaterTypeItem("INT", "UDINT",1));
            _typeComp.Add(new GreaterTypeItem("INT", "ULINT",1));
            _typeComp.Add(new GreaterTypeItem("INT", "REAL",1));
            _typeComp.Add(new GreaterTypeItem("INT", "LREAL",1));

            _typeComp.Add(new GreaterTypeItem("DINT", "SINT",0));
            _typeComp.Add(new GreaterTypeItem("DINT", "INT",0));
            _typeComp.Add(new GreaterTypeItem("DINT", "DINT",0));
            _typeComp.Add(new GreaterTypeItem("DINT", "LINT",1));
            _typeComp.Add(new GreaterTypeItem("DINT", "USINT",0));
            _typeComp.Add(new GreaterTypeItem("DINT", "UINT",0));
            _typeComp.Add(new GreaterTypeItem("DINT", "UDINT",0));
            _typeComp.Add(new GreaterTypeItem("DINT", "ULINT",1));
            _typeComp.Add(new GreaterTypeItem("DINT", "REAL", 1));
            _typeComp.Add(new GreaterTypeItem("DINT", "LREAL",1));


            _typeComp.Add(new GreaterTypeItem("LINT", "SINT", 0));
            _typeComp.Add(new GreaterTypeItem("LINT", "INT", 0));
            _typeComp.Add(new GreaterTypeItem("LINT", "DINT", 0));
            _typeComp.Add(new GreaterTypeItem("LINT", "LINT", 0));
            _typeComp.Add(new GreaterTypeItem("LINT", "USINT", 0));
            _typeComp.Add(new GreaterTypeItem("LINT", "UINT", 0));
            _typeComp.Add(new GreaterTypeItem("LINT", "UDINT", 0));
            _typeComp.Add(new GreaterTypeItem("LINT", "ULINT", 0));
            _typeComp.Add(new GreaterTypeItem("LINT", "REAL", 1));
            _typeComp.Add(new GreaterTypeItem("LINT", "LREAL", 1));

            _typeComp.Add(new GreaterTypeItem("USINT", "SINT", 0));
            _typeComp.Add(new GreaterTypeItem("USINT", "INT", 1));
            _typeComp.Add(new GreaterTypeItem("USINT", "DINT", 1));
            _typeComp.Add(new GreaterTypeItem("USINT", "LINT", 1));
            _typeComp.Add(new GreaterTypeItem("USINT", "USINT", 0));
            _typeComp.Add(new GreaterTypeItem("USINT", "UINT", 1));
            _typeComp.Add(new GreaterTypeItem("USINT", "UDINT", 1));
            _typeComp.Add(new GreaterTypeItem("USINT", "ULINT", 1));
            _typeComp.Add(new GreaterTypeItem("USINT", "REAL", 1));
            _typeComp.Add(new GreaterTypeItem("USINT", "LREAL", 1));

            _typeComp.Add(new GreaterTypeItem("UINT", "SINT", 0));
            _typeComp.Add(new GreaterTypeItem("UINT", "INT", 0));
            _typeComp.Add(new GreaterTypeItem("UINT", "DINT", 1));
            _typeComp.Add(new GreaterTypeItem("UINT", "LINT", 1));
            _typeComp.Add(new GreaterTypeItem("UINT", "USINT", 0));
            _typeComp.Add(new GreaterTypeItem("UINT", "UINT", 0));
            _typeComp.Add(new GreaterTypeItem("UINT", "UDINT", 1));
            _typeComp.Add(new GreaterTypeItem("UINT", "ULINT", 1));
            _typeComp.Add(new GreaterTypeItem("UINT", "REAL", 1));
            _typeComp.Add(new GreaterTypeItem("UINT", "LREAL", 1));

            _typeComp.Add(new GreaterTypeItem("UDINT", "SINT", 0));
            _typeComp.Add(new GreaterTypeItem("UDINT", "INT", 0));
            _typeComp.Add(new GreaterTypeItem("UDINT", "DINT", 0));
            _typeComp.Add(new GreaterTypeItem("UDINT", "LINT", 1));
            _typeComp.Add(new GreaterTypeItem("UDINT", "USINT", 0));
            _typeComp.Add(new GreaterTypeItem("UDINT", "UINT", 0));
            _typeComp.Add(new GreaterTypeItem("UDINT", "UDINT", 0));
            _typeComp.Add(new GreaterTypeItem("UDINT", "ULINT", 1));
            _typeComp.Add(new GreaterTypeItem("UDINT", "REAL", 1));
            _typeComp.Add(new GreaterTypeItem("UDINT", "LREAL", 1));

            _typeComp.Add(new GreaterTypeItem("ULINT", "SINT", 0));
            _typeComp.Add(new GreaterTypeItem("ULINT", "INT", 0));
            _typeComp.Add(new GreaterTypeItem("ULINT", "DINT", 0));
            _typeComp.Add(new GreaterTypeItem("ULINT", "LINT", 0));
            _typeComp.Add(new GreaterTypeItem("ULINT", "USINT", 0));
            _typeComp.Add(new GreaterTypeItem("ULINT", "UINT", 0));
            _typeComp.Add(new GreaterTypeItem("ULINT", "UDINT", 0));
            _typeComp.Add(new GreaterTypeItem("ULINT", "ULINT", 0));
            _typeComp.Add(new GreaterTypeItem("ULINT", "REAL", 1));
            _typeComp.Add(new GreaterTypeItem("ULINT", "LREAL", 1));

            _typeComp.Add(new GreaterTypeItem("REAL", "SINT", 0));
            _typeComp.Add(new GreaterTypeItem("REAL", "INT", 0));
            _typeComp.Add(new GreaterTypeItem("REAL", "DINT", 0));
            _typeComp.Add(new GreaterTypeItem("REAL", "LINT", 0));
            _typeComp.Add(new GreaterTypeItem("REAL", "USINT", 0));
            _typeComp.Add(new GreaterTypeItem("REAL", "UINT", 0));
            _typeComp.Add(new GreaterTypeItem("REAL", "UDINT", 0));
            _typeComp.Add(new GreaterTypeItem("REAL", "ULINT", 0));
            _typeComp.Add(new GreaterTypeItem("REAL", "REAL", 0));
            _typeComp.Add(new GreaterTypeItem("REAL", "LREAL", 1));

            _typeComp.Add(new GreaterTypeItem("LREAL", "SINT", 0));
            _typeComp.Add(new GreaterTypeItem("LREAL", "INT", 0));
            _typeComp.Add(new GreaterTypeItem("LREAL", "DINT", 0));
            _typeComp.Add(new GreaterTypeItem("LREAL", "LINT", 0));
            _typeComp.Add(new GreaterTypeItem("LREAL", "USINT", 0));
            _typeComp.Add(new GreaterTypeItem("LREAL", "UINT", 0));
            _typeComp.Add(new GreaterTypeItem("LREAL", "UDINT", 0));
            _typeComp.Add(new GreaterTypeItem("LREAL", "ULINT", 0));
            _typeComp.Add(new GreaterTypeItem("LREAL", "REAL", 0));
            _typeComp.Add(new GreaterTypeItem("LREAL", "LREAL", 0));

        }

        public static string GetOperationResultType(Operand op, InstructionCategory cat, ref uint size)
        {
            switch(cat)
            {
                case InstructionCategory.Eq:
                case InstructionCategory.Ge:
                case InstructionCategory.Gr:
                case InstructionCategory.Le:
                case InstructionCategory.Ls:
                case InstructionCategory.Ne:
                {
                    size = 1;
                    return "BOOL";
                }
                case InstructionCategory.Neg:
                {
                    size = op.Size;
                    string str = op.TypeId.TrimStart('U');
                    return str;
                }
            }

            size = op.Size;
            return op.TypeId;
        }


        public static InstructionCode GetExpConvertInstructionCode(string from, string to)
        {
            string lookingFor = from + "2" + to;

            for(int i = 0; i < Convert.ToInt32( InstructionCode.End); ++i)
            {
                InstructionCode code = (InstructionCode)i;
                string strCode = code.ToString();
                
                if (lookingFor == strCode)
                    return code;
            }

            return InstructionCode.NOP;
        }

        public static InstructionCode GetImplConvertInstructionCode(string from, string to)
        {
            foreach (ConvertionItem item in _conversionMatrix)
            {
                if (item.FromTypeId == from && item.ToTypeId == to)
                    return item.InstCode;
            }

            return InstructionCode.NOP;
        }

        public static uint SizeOf(string typeId)
        {
            switch (typeId)
            {
                case "SINT":
                case "BYTE":
                case "USINT":
                    return 1;

                case "INT":
                case "WORD":
                case "UINT":
                    return 2;

                case "DINT":
                case "DWORD":
                case "UDINT":
                    return 4;

                case "LINT":
                case "LWORD":
                case "ULINT":
                    return 8;
                
                case "REAL":
                    return sizeof(float);
                
                case "LREAL":
                    return sizeof(double);
            }

            throw new Exception("Unknow type id");
        }

        public static bool IsIntegerType( string typeId)
        {
            switch (typeId)
            {
                case "SINT":
                case "INT":
                case "DINT":
                case "LINT":
                case "USINT":
                case "UINT":
                case "UDINT":
                case "ULINT":
                case "ENUMERATION":
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsNumericType(string typeId)
        {
            switch (typeId)
            {
                case "SINT":
                case "INT":
                case "DINT":
                case "LINT":
                case "USINT":
                case "UINT":
                case "UDINT":
                case "ULINT":
                case "REAL":
                case "LREAL":
                case "ENUMERATION":
                    return true;

                default:
                    return false;
            }
        }
        
        public static bool IsBitStringType(string typeId)
        {
            switch (typeId)
            {
                case "BOOL":
                case "BYTE":
                case "WORD":
                case "DWORD":
                case "LWORD":
                    return true;

                default:
                    return false;
            }
        }

        public static Operand GetGreaterOperandByType(List<Operand> ops)
        {
            Operand lastOp = ops[0];

            for (int i = 1; i < ops.Count; ++i)
                lastOp = GetGreaterType(lastOp, ops[i]);

            return lastOp;
        }

        public static Operand GetGreaterType(Operand op1, Operand op2)
        {
            foreach (GreaterTypeItem item in _typeComp)
            {
                if (op1.TypeId == item.TypeId1 && op2.TypeId == item.TypeId2)
                {
                    if (item.Greater == 0)
                        return op1;
                    else
                        return op2;
                }
            }

            return null;
        }

        public static InstructionCode GetInstructionCode(string typeId, InstructionCategory cat)
        {
            string strCode = cat.ToString() + typeId;

            for (int i = 0; i < Convert.ToInt32(InstructionCode.End); ++i)
            {
                InstructionCode code = (InstructionCode)i;

                if (strCode == code.ToString())
                    return code;
            }

            return InstructionCode.NOP;
        }

        public static bool IsAddOffset(InstructionCode code)
        {
            switch (code)
            {
                case  InstructionCode.AddOffsetSINT:
                case InstructionCode.AddOffsetINT:
                case InstructionCode.AddOffsetDINT:
                case InstructionCode.AddOffsetLINT:
                case InstructionCode.AddOffsetUSINT:
                case InstructionCode.AddOffsetUINT:
                case InstructionCode.AddOffsetUDINT:
                case InstructionCode.AddOffsetULINT:
                    return true;
            }

            return false;
        }
        
        private static Operand XorConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);
            switch (op.TypeId)
            {
                case "BOOL":
                    value = Convert.ToBoolean(op1.ConstVal) ^ Convert.ToBoolean(op2.ConstVal);
                    break;

                case "BYTE":
                    value = Convert.ToByte(op1.ConstVal) ^ Convert.ToByte(op2.ConstVal);
                    break;
                case "WORD":
                    value = Convert.ToUInt16(op1.ConstVal) ^ Convert.ToUInt16(op2.ConstVal);
                    break;
                case "DWORD":
                    value = Convert.ToUInt32(op1.ConstVal) ^ Convert.ToUInt32(op2.ConstVal);
                    break;

                case "LWORD":
                    value = Convert.ToUInt64(op1.ConstVal) ^ Convert.ToUInt64(op2.ConstVal);
                    break;
                default:
                    return null;
            }

            return new Operand(value, op.Uid, op.Size, op.TypeId);
        }

        private static Operand AndConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);
            switch (op.TypeId)
            {
                case "BOOL":
                    value = Convert.ToBoolean(op1.ConstVal) & Convert.ToBoolean(op2.ConstVal);
                    break;

                case "BYTE":
                    value = Convert.ToByte(op1.ConstVal) & Convert.ToByte(op2.ConstVal);
                    break;
                case "WORD":
                    value = Convert.ToUInt16(op1.ConstVal) & Convert.ToUInt16(op2.ConstVal);
                    break;
                case "DWORD":
                    value = Convert.ToUInt32(op1.ConstVal) & Convert.ToUInt32(op2.ConstVal);
                    break;

                case "LWORD":
                    value = Convert.ToUInt64(op1.ConstVal) & Convert.ToUInt64(op2.ConstVal);
                    break;
                default:
                    return null;
            }
            return new Operand(value, op.Uid, op.Size, op.TypeId);
        }

        private static Operand OrConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op  = GetGreaterType(op1, op2);
            
            if( op == null)
                return null;

            switch (op.TypeId)
            {
                case "BOOL":
                    value = Convert.ToBoolean(op1.ConstVal) | Convert.ToBoolean(op2.ConstVal);
                    break;

                case "BYTE":
                    value = Convert.ToByte(op1.ConstVal) | Convert.ToByte(op2.ConstVal);
                    break;
                case "WORD":
                    value = Convert.ToUInt16(op1.ConstVal) | Convert.ToUInt16(op2.ConstVal);
                    break;
                case "DWORD":
                    value = Convert.ToUInt32(op1.ConstVal) | Convert.ToUInt32(op2.ConstVal);
                    break;

                case "LWORD":
                    value = Convert.ToUInt64(op1.ConstVal) | Convert.ToUInt64(op2.ConstVal);
                    break;
                default:
                    return null;
            }

            return new Operand(value, op.Uid, op.Size, op.TypeId);
        }

        private static Operand ModConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            try
            {                
                if (op == null)
                    return null;

                switch (op.TypeId)
                {
                    case "SINT":
                        value = Convert.ToSByte(op1.ConstVal) %  Convert.ToSByte(op2.ConstVal);
                        break;

                    case "INT":
                        value = Convert.ToInt16(op1.ConstVal) % Convert.ToInt16(op2.ConstVal);
                        break;
                    case "DINT":
                        value = Convert.ToInt32(op1.ConstVal) % Convert.ToInt32(op2.ConstVal);
                        break;
                    case "LINT":
                        value = Convert.ToInt64(op1.ConstVal) %  Convert.ToInt64(op2.ConstVal);
                        break;

                    case "USINT":
                        value = Convert.ToByte(op1.ConstVal) % Convert.ToByte(op2.ConstVal);
                        break;

                    case "UINT":
                        value = Convert.ToUInt16(op1.ConstVal) % Convert.ToUInt16(op2.ConstVal);
                        break;

                    case "UDINT":
                        value = Convert.ToUInt32(op1.ConstVal) % Convert.ToUInt32(op2.ConstVal);
                        break;

                    case "ULINT":
                        value = Convert.ToUInt64(op1.ConstVal) % Convert.ToUInt64(op2.ConstVal);
                        break;

                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }

            return new Operand(value, op.Uid, op.Size, op.TypeId);
        }

        private static Operand DivConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            try
            {
                if (op == null)
                    return null;

                switch (op.TypeId)
                {
                    case "SINT":
                        value = Convert.ToSByte(op1.ConstVal) / Convert.ToByte(op2.ConstVal);
                    break;

                    case "INT":
                        value = Convert.ToInt16(op1.ConstVal) / Convert.ToInt16(op2.ConstVal);
                        break;
                    case "DINT":
                        value = Convert.ToInt32(op1.ConstVal) / Convert.ToInt32(op2.ConstVal);
                        break;
                    case "LINT":
                        value = Convert.ToInt64(op1.ConstVal) / Convert.ToInt64(op2.ConstVal);
                        break;

                    case "USINT":
                        value = Convert.ToByte(op1.ConstVal) / Convert.ToByte(op2.ConstVal);
                        break;

                    case "UINT":
                        value = Convert.ToUInt16(op1.ConstVal) / Convert.ToUInt16(op2.ConstVal);
                        break;

                    case "UDINT":
                        value = Convert.ToUInt32(op1.ConstVal) / Convert.ToUInt32(op2.ConstVal);
                        break;

                    case "ULINT":
                        value = Convert.ToUInt64(op1.ConstVal) / Convert.ToUInt64(op2.ConstVal);
                        break;
                    case "REAL":
                    {
                        if (Convert.ToSingle(op2.ConstVal) == 0.0)
                            return null;

                        value = Convert.ToSingle(op1.ConstVal) / Convert.ToSingle(op2.ConstVal);
                    }
                    break;

                    case "LREAL":
                    {
                        if (Convert.ToDouble(op2.ConstVal) == 0.0)
                            return null;

                        value = Convert.ToDouble(op1.ConstVal) / Convert.ToDouble(op2.ConstVal);
                    }
                    break;

                    default:
                        return null;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }

            return new Operand(value, op.Uid, op.Size, op.TypeId);
        }

        public static Operand EvalConstOperation(Operand op, InstructionCategory cat)
        {
            switch (cat)
            {
                case InstructionCategory.Neg:
                    return NegConstant(op);
                case InstructionCategory.Not:
                    return NotConstant(op);
            }

            return null;
        }

        private static Operand NotConstant(Operand op)
        {
            switch (op.TypeId)
            {
                case "BOOL":
                    return new Operand(!((bool)op.ConstVal),0, 1,"BOOL");
                case "BYTE":
                    return new Operand(~(Convert.ToByte(op.ConstVal)),0,1,"BYTE");
                case "WORD":
                    return new Operand(~(Convert.ToUInt16(op.ConstVal)),0,2,"WORD");
                case "DWORD":
                    return new Operand(~(Convert.ToUInt32(op.ConstVal)),0,4,"DWORD");
                case "LWORD":
                    return new Operand(~(Convert.ToUInt64(op.ConstVal)), 0, 8, "LWORD");
            }

            return null;
        }

        private static Operand NegConstant(Operand op)
        {
            object value;
            switch (op.TypeId)
            {
                case "SINT":
                    value = -(Convert.ToByte(op.ConstVal));
                    break;

                case "INT":
                    value = -(Convert.ToInt16(op.ConstVal));
                    break;
                case "DINT":
                    value = -(Convert.ToInt32(op.ConstVal));
                    break;
                case "LINT":
                    value = -(Convert.ToInt64(op.ConstVal));
                    break;

                case "USINT":
                    value = -(Convert.ToByte(op.ConstVal));
                    break;

                case "UINT":
                    value = -(Convert.ToUInt16(op.ConstVal));
                    break;

                case "UDINT":
                    value = -(Convert.ToUInt32(op.ConstVal));
                    break;

                case "ULINT":
                    value = -(Convert.ToInt64(op.ConstVal));
                    break;
                case "REAL":
                    value = -(Convert.ToSingle(op.ConstVal));
                    break;

                case "LREAL":
                    value = -(Convert.ToDouble(op.ConstVal));
                    break;
                default:
                    return null;
            }

            string typeId = op.TypeId.TrimStart('U');
            return new Operand(value, 0, op.Size, typeId);
        }

        public static Operand EvalConstantOperation(Operand op1, Operand op2, InstructionCategory category)
        {
            try
            {
                switch (category)
                {
                    case InstructionCategory.Add:
                        return AddConstant(op1, op2);
                    case InstructionCategory.Sub:
                        return SubConstant(op1, op2);
                    case InstructionCategory.And:
                        return AndConstant(op1, op2);
                    case InstructionCategory.Div:
                        return DivConstant(op1, op2);
                    case InstructionCategory.Eq:
                        return EqConstant(op1, op2);
                    case InstructionCategory.Ge:
                        return GeConstant(op1, op2);
                    case InstructionCategory.Gr:
                        return GrConstant(op1, op2);
                    case InstructionCategory.Le:
                        return LeConstant(op1, op2);
                    case InstructionCategory.Ls:
                        return LsConstant(op1, op2);
                    case InstructionCategory.Mod:
                        return ModConstant(op1, op2);
                    case InstructionCategory.Mul:
                        return MulConstant(op1, op2);
                    case InstructionCategory.Ne:
                        return NeConstant(op1, op2);
                    case InstructionCategory.Or:
                        return OrConstant(op1, op2);
                    case InstructionCategory.Pow:
                        return PowConstant(op1, op2);
                    case InstructionCategory.Xor:
                        return XorConstant(op1, op2);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

            return null;
        }

        public static bool IsConstValueZero(Operand op)
        {
            switch (op.TypeId)
            {
                case "SINT":
                    return ((Convert.ToByte(op.ConstVal)) == 0);

                case "INT":
                    return ((Convert.ToInt16(op.ConstVal)) == 0);

                case "DINT":
                    return ((Convert.ToInt32(op.ConstVal)) == 0);

                case "LINT":
                    return ((Convert.ToInt64(op.ConstVal)) == 0);

                case "USINT":
                    return ((Convert.ToByte(op.ConstVal)) == 0);

                case "UINT":
                    return ((Convert.ToUInt16(op.ConstVal)) == 0);

                case "UDINT":
                    return ((Convert.ToUInt32(op.ConstVal)) == 0);

                case "ULINT":
                    return ((Convert.ToUInt64(op.ConstVal)) == 0);

                case "REAL":
                    return ((Convert.ToSingle(op.ConstVal)) == 0.0f);

                case "LREAL":
                    return ((Convert.ToDouble(op.ConstVal)) == 0.0);

                default:
                    return true;
            }
        }

        private static Operand LsConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            if (op == null)
                return null;

            switch (op.TypeId)
            {
                case "SINT":
                    value = Convert.ToByte(op1.ConstVal) < Convert.ToByte(op2.ConstVal);
                    break;

                case "INT":
                    value = Convert.ToInt16(op1.ConstVal) < Convert.ToInt16(op2.ConstVal);
                    break;
                case "DINT":
                    value = Convert.ToInt32(op1.ConstVal) < Convert.ToInt32(op2.ConstVal);
                    break;
                case "LINT":
                    value = Convert.ToInt64(op1.ConstVal) < Convert.ToInt64(op2.ConstVal);
                    break;

                case "USINT":
                    value = Convert.ToByte(op1.ConstVal) < Convert.ToByte(op2.ConstVal);
                    break;

                case "UINT":
                    value = Convert.ToUInt16(op1.ConstVal) < Convert.ToUInt16(op2.ConstVal);
                    break;

                case "UDINT":
                    value = Convert.ToUInt32(op1.ConstVal) < Convert.ToUInt32(op2.ConstVal);
                    break;

                case "ULINT":
                    value = Convert.ToUInt64(op1.ConstVal )< Convert.ToUInt64(op2.ConstVal);
                    break;
                case "REAL":
                    value = Convert.ToSingle(op1.ConstVal )< Convert.ToSingle(op2.ConstVal);
                    break;

                case "LREAL":
                    value = Convert.ToDouble(op1.ConstVal) < Convert.ToDouble(op2.ConstVal);
                    break;
                default:
                    return null;
            }

            return new Operand(value, 0, 1, "BOOL");
        }

        private static Operand LeConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            if (op == null)
                return null;

            switch (op.TypeId)
            {
                case "SINT":
                    value = Convert.ToByte(op1.ConstVal) <= Convert.ToByte(op2.ConstVal);
                    break;

                case "INT":
                    value = Convert.ToInt16(op1.ConstVal) <= Convert.ToInt16(op2.ConstVal);
                    break;
                case "DINT":
                    value = Convert.ToInt32(op1.ConstVal) <= Convert.ToInt32(op2.ConstVal);
                    break;
                case "LINT":
                    value = Convert.ToInt64(op1.ConstVal) <= Convert.ToInt64(op2.ConstVal);
                    break;

                case "USINT":
                    value = Convert.ToByte(op1.ConstVal) <= Convert.ToByte(op2.ConstVal);
                    break;

                case "UINT":
                    value = Convert.ToUInt16(op1.ConstVal) <= Convert.ToUInt16(op2.ConstVal);
                    break;

                case "UDINT":
                    value = Convert.ToUInt32(op1.ConstVal) <= Convert.ToUInt32(op2.ConstVal);
                    break;

                case "ULINT":
                    value = Convert.ToUInt64(op1.ConstVal) <= Convert.ToUInt64(op2.ConstVal);
                    break;
                case "REAL":
                    value = Convert.ToSingle(op1.ConstVal) <= Convert.ToSingle(op2.ConstVal);
                    break;

                case "LREAL":
                    value = Convert.ToDouble(op1.ConstVal) <= Convert.ToDouble(op2.ConstVal);
                    break;
                default:
                    return null;
            }

            return new Operand(value, 0, 1, "BOOL");
        }

        private static Operand GrConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            if (op == null)
                return null;

            switch (op.TypeId)
            {
                case "SINT":
                    value = Convert.ToByte(op1.ConstVal) > Convert.ToByte(op2.ConstVal);
                    break;

                case "INT":
                    value = Convert.ToInt16(op1.ConstVal) > Convert.ToInt16(op2.ConstVal);
                    break;
                case "DINT":
                    value = Convert.ToInt32(op1.ConstVal) > Convert.ToInt32(op2.ConstVal);
                    break;
                case "LINT":
                    value = Convert.ToInt64(op1.ConstVal) > Convert.ToInt64(op2.ConstVal);
                    break;

                case "USINT":
                    value = Convert.ToByte(op1.ConstVal) > Convert.ToByte(op2.ConstVal);
                    break;

                case "UINT":
                    value = Convert.ToUInt16(op1.ConstVal) > Convert.ToUInt16(op2.ConstVal);
                    break;

                case "UDINT":
                    value = Convert.ToUInt32(op1.ConstVal) > Convert.ToUInt32(op2.ConstVal);
                    break;

                case "ULINT":
                    value = Convert.ToUInt64(op1.ConstVal) > Convert.ToUInt64(op2.ConstVal);
                    break;
                case "REAL":
                    value = Convert.ToSingle(op1.ConstVal) > Convert.ToSingle(op2.ConstVal);
                    break;

                case "LREAL":
                    value = Convert.ToDouble(op1.ConstVal) > Convert.ToDouble(op2.ConstVal);
                    break;
                default:
                    return null;
            }

            return new Operand(value, 0, 1, "BOOL");
        }

        private static Operand GeConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            if (op == null)
                return null;

            switch (op.TypeId)
            {
                case "SINT":
                    value = Convert.ToByte(op1.ConstVal) >= Convert.ToByte(op2.ConstVal);
                    break;

                case "INT":
                    value = Convert.ToInt16(op1.ConstVal) >= Convert.ToInt16(op2.ConstVal);
                    break;
                case "DINT":
                    value = Convert.ToInt32(op1.ConstVal) >= Convert.ToInt32(op2.ConstVal);
                    break;
                case "LINT":
                    value = Convert.ToInt64(op1.ConstVal) >= Convert.ToInt64(op2.ConstVal);
                    break;

                case "USINT":
                    value = Convert.ToByte(op1.ConstVal) >= Convert.ToByte(op2.ConstVal);
                    break;

                case "UINT":
                    value = Convert.ToUInt16(op1.ConstVal) >= Convert.ToUInt16(op2.ConstVal);
                    break;

                case "UDINT":
                    value = Convert.ToUInt32(op1.ConstVal) >= Convert.ToUInt32(op2.ConstVal);
                    break;

                case "ULINT":
                    value = Convert.ToUInt64(op1.ConstVal) >= Convert.ToUInt64(op2.ConstVal);
                    break;
                case "REAL":
                    value = Convert.ToSingle(op1.ConstVal) >= Convert.ToSingle(op2.ConstVal);
                    break;

                case "LREAL":
                    value = Convert.ToDouble(op1.ConstVal) >= Convert.ToDouble(op2.ConstVal);
                    break;
                default:
                    return null;
            }

            return new Operand(value, 0, 1, "BOOL");
        }

        private static Operand EqConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            if (op == null)
                return null;

            switch (op.TypeId)
            {
                case "SINT":
                    value = Convert.ToByte(op1.ConstVal) == Convert.ToByte(op2.ConstVal);
                    break;

                case "INT":
                    value = Convert.ToInt16(op1.ConstVal) == Convert.ToInt16(op2.ConstVal);
                    break;
                case "DINT":
                    value = Convert.ToInt32(op1.ConstVal) == Convert.ToInt32(op2.ConstVal);
                    break;
                case "LINT":
                    value = Convert.ToInt64(op1.ConstVal) == Convert.ToInt64(op2.ConstVal);
                    break;

                case "USINT":
                    value = Convert.ToByte(op1.ConstVal) == Convert.ToByte(op2.ConstVal);
                    break;

                case "UINT":
                    value = Convert.ToUInt16(op1.ConstVal) == Convert.ToUInt16(op2.ConstVal);
                    break;

                case "UDINT":
                    value = Convert.ToUInt32(op1.ConstVal) == Convert.ToUInt32(op2.ConstVal);
                    break;

                case "ULINT":
                    value = Convert.ToUInt64(op1.ConstVal) == Convert.ToUInt64(op2.ConstVal);
                    break;
                case "REAL":
                    value = Convert.ToSingle(op1.ConstVal) == Convert.ToSingle(op2.ConstVal);
                    break;

                case "LREAL":
                    value = Convert.ToDouble(op1.ConstVal) == Convert.ToDouble(op2.ConstVal);
                    break;
                case "BOOL":
                    value = Convert.ToBoolean(op1.ConstVal) == Convert.ToBoolean(op2.ConstVal);
                    break;
                default:
                    return null;
            }

            return new Operand(value, 0, 1,"BOOL");
        }

        private static Operand NeConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            if (op == null)
                return null;

            switch (op.TypeId)
            {
                case "SINT":
                    value = Convert.ToByte(op1.ConstVal) != Convert.ToByte(op2.ConstVal);
                    break;

                case "INT":
                    value = Convert.ToInt16(op1.ConstVal) != Convert.ToInt16(op2.ConstVal);
                    break;
                case "DINT":
                    value = Convert.ToInt32(op1.ConstVal) != Convert.ToInt32(op2.ConstVal);
                    break;
                case "LINT":
                    value = Convert.ToInt64(op1.ConstVal) != Convert.ToInt64(op2.ConstVal);
                    break;

                case "USINT":
                    value = Convert.ToByte(op1.ConstVal) != Convert.ToByte(op2.ConstVal);
                    break;

                case "UINT":
                    value = Convert.ToUInt16(op1.ConstVal) != Convert.ToUInt16(op2.ConstVal);
                    break;

                case "UDINT":
                    value = Convert.ToUInt32(op1.ConstVal )!= Convert.ToUInt32(op2.ConstVal);
                    break;

                case "ULINT":
                    value = Convert.ToUInt64(op1.ConstVal) != Convert.ToUInt64(op2.ConstVal);
                    break;
                case "REAL":
                    value = Convert.ToSingle(op1.ConstVal) != Convert.ToSingle(op2.ConstVal);
                    break;

                case "LREAL":
                    value = Convert.ToDouble(op1.ConstVal) != Convert.ToDouble(op2.ConstVal);
                    break;

                case "BOOL":
                    value = (bool)op1.ConstVal != (bool)op2.ConstVal;
                    break;
                default:
                    return null;
            }

            return new Operand(value, 0, 1, "BOOL");
        }

        private static Operand PowConstant(Operand op1, Operand op2)
        {
            object value;
            try
            {
                switch (op1.TypeId)
                {
                    case "SINT":
                        value = Convert.ToByte(Math.Pow(Convert.ToByte(op1.ConstVal), Convert.ToDouble(op2.ConstVal)));
                        break;

                    case "INT":
                        value = Convert.ToInt16(Math.Pow(Convert.ToInt16(op1.ConstVal), Convert.ToDouble(op2.ConstVal)));
                        break;
                    case "DINT":
                        value = Convert.ToInt32(Math.Pow(Convert.ToInt32(op1.ConstVal), Convert.ToDouble(op2.ConstVal)));
                        break;
                    case "LINT":
                        value = Convert.ToInt64(Math.Pow(Convert.ToInt64(op1.ConstVal), Convert.ToDouble(op2.ConstVal)));
                        break;

                    case "USINT":
                        value = Convert.ToByte(Math.Pow(Convert.ToByte(op1.ConstVal), Convert.ToDouble(op2.ConstVal)));
                        break;

                    case "UINT":
                        value = Convert.ToUInt16(Math.Pow(Convert.ToUInt16(op1.ConstVal), Convert.ToDouble(op2.ConstVal)));
                        break;

                    case "UDINT":
                        value = Convert.ToUInt32(Math.Pow(Convert.ToUInt32(op1.ConstVal), Convert.ToDouble(op2.ConstVal)));
                        break;

                    case "ULINT":
                        value = Convert.ToUInt64(Math.Pow(Convert.ToUInt64(op1.ConstVal), Convert.ToDouble(op2.ConstVal)));
                        break;
                    case "REAL":
                        value = Convert.ToSingle(Math.Pow(Convert.ToSingle(op1.ConstVal), Convert.ToDouble(op2.ConstVal)));
                        break;

                    case "LREAL":
                        value = Math.Pow(Convert.ToDouble(op1.ConstVal), Convert.ToDouble(op2.ConstVal));
                        break;
                    default:
                        return null;
                }

            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }

            return new Operand(value, 0, op1.Size, op1.TypeId);
        }

        private static Operand MulConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            if (op == null)
                return null;

            switch (op.TypeId)
            {
                case "SINT":
                    value = Convert.ToSByte(op1.ConstVal) * Convert.ToSByte(op2.ConstVal);
                    break;

                case "INT":
                    value = Convert.ToInt16(op1.ConstVal) * Convert.ToInt16(op2.ConstVal);
                    break;
                case "DINT":
                    value = Convert.ToInt32(op1.ConstVal) * Convert.ToInt32(op2.ConstVal);
                    break;
                case "LINT":
                    value = Convert.ToInt64(op1.ConstVal) *  Convert.ToInt64(op2.ConstVal);
                    break;

                case "USINT":
                    value = Convert.ToByte(op1.ConstVal) * Convert.ToByte(op2.ConstVal);
                    break;

                case "UINT":
                    value = Convert.ToUInt16(op1.ConstVal) * Convert.ToUInt16(op2.ConstVal);
                    break;

                case "UDINT":
                    value = Convert.ToUInt32(op1.ConstVal) * Convert.ToUInt64(op2.ConstVal);
                    break;

                case "ULINT":
                    value = Convert.ToUInt64(op1.ConstVal) * Convert.ToUInt64(op2.ConstVal);
                    break;
                case "REAL":
                    value = Convert.ToSingle(op1.ConstVal) * Convert.ToSingle(op2.ConstVal);
                    break;

                case "LREAL":
                        value = Convert.ToDouble(op1.ConstVal) * Convert.ToDouble(op2.ConstVal);
                    break;
                default:
                    return null;
            }

            return new Operand(value, op.Uid, op.Size, op.TypeId);
        }

        private static Operand AddConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            if (op == null)
                return null;

            switch (op.TypeId)
            {
                case "SINT":
                    value = Convert.ToSByte(op1.ConstVal) +Convert.ToSByte(op2.ConstVal);
                break;

                case "INT":
                value = Convert.ToInt16(op1.ConstVal) + Convert.ToInt16(op2.ConstVal);
                break;

                case "DINT":
                    value = Convert.ToInt32(op1.ConstVal) + Convert.ToInt32(op2.ConstVal);
                break;

                case "LINT":
                    value = Convert.ToInt64(op1.ConstVal) + Convert.ToInt64(op2.ConstVal);
                break;

                case "USINT":
                    value = Convert.ToByte(op1.ConstVal) + Convert.ToByte(op2.ConstVal);
                break;

                case "UINT":
                    value = Convert.ToUInt16(op1.ConstVal) + Convert.ToUInt16(op2.ConstVal);
                break;

                case "UDINT":
                    value = Convert.ToUInt32(op1.ConstVal) + Convert.ToUInt32(op2.ConstVal);
                break;

                case "ULINT":
                    value = Convert.ToUInt64(op1.ConstVal) + Convert.ToUInt64(op2.ConstVal);
                break;

                case "REAL":
                    value = Convert.ToSingle(op1.ConstVal) + Convert.ToSingle(op2.ConstVal);
                break;

                case "LREAL":
                    value = Convert.ToDouble(op1.ConstVal) + Convert.ToDouble(op2.ConstVal);
                break;

                default:
                    return null;
            }

            return new Operand(value, op.Uid, op.Size, op.TypeId);
        }

        private static Operand SubConstant(Operand op1, Operand op2)
        {
            object value;
            Operand op = GetGreaterType(op1, op2);

            if (op == null)
                return null;

            switch (op.TypeId)
            {
                case "SINT":
                    value = Convert.ToByte(op1.ConstVal) - Convert.ToByte(op2.ConstVal);
                break;

                case "INT":
                    value = Convert.ToInt16(op1.ConstVal) - Convert.ToInt16(op2.ConstVal);
                break;

                case "DINT":
                    value = Convert.ToInt32(op1.ConstVal)- Convert.ToInt32(op2.ConstVal);
                break;

                case "LINT":
                    value = Convert.ToInt64(op1.ConstVal) - Convert.ToInt64(op2.ConstVal);
                break;

                case "USINT":
                    value = Convert.ToByte(op1.ConstVal) - Convert.ToByte(op2.ConstVal);
                break;

                case "UINT":
                    value = Convert.ToUInt16(op1.ConstVal) - Convert.ToUInt16(op2.ConstVal);
                break;

                case "UDINT":
                    value = Convert.ToUInt32(op1.ConstVal) - Convert.ToUInt32(op2.ConstVal);
                break;

                case "ULINT":
                    value = Convert.ToUInt64(op1.ConstVal) - Convert.ToUInt64(op2.ConstVal);
                break;
                
                case "REAL":
                    value = Convert.ToSingle(op1.ConstVal) - Convert.ToSingle(op2.ConstVal);
                break;

                case "LREAL":
                value = Convert.ToDouble(op1.ConstVal) - Convert.ToDouble(op2.ConstVal);
                break;
                
                default:
                    return null;
            }

            return new Operand(value, op.Uid, op1.Size, op1.TypeId);
        }

        public static void ConvertConst(Operand constOp, Operand to)
        {
            if (constOp.TypeId == "VOID" || to.TypeId == "VOID")
                return;

            try
            {
                if (to.TypeId == "INT")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    short value = Convert.ToInt16(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "BOOL")
                {
                    /*
                    bool value = Convert.ToBoolean(constOp.ConstVal);
                    constOp.ConstVal = value;*/
                    return;

                }
                else if (to.TypeId == "BYTE")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    byte value = Convert.ToByte(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "WORD")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    ushort value = Convert.ToUInt16(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "DWORD")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    uint value = Convert.ToUInt32(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "SINT")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    sbyte value = Convert.ToSByte(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "DINT")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    int value = Convert.ToInt32(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "LINT")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    long value = Convert.ToInt64(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "USINT")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    byte value = Convert.ToByte(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "UINT")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    ushort value = Convert.ToUInt16(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "UDINT")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    uint value = Convert.ToUInt32(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "ULINT")
                {
                    if (constOp.TypeId == "LREAL" || constOp.TypeId == "REAL")
                        return;

                    ulong value = Convert.ToUInt64(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "REAL")
                {
                    float value = Convert.ToSingle(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else if (to.TypeId == "LREAL")
                {
                    double value = Convert.ToDouble(constOp.ConstVal);
                    constOp.ConstVal = value;
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return;
            }

            constOp.Uid = to.Uid;
            constOp.TypeId = to.TypeId;
            constOp.Size = to.Size;
        }

        public static ulong GetUID(ITree trn)
        {
            ulong uid = (((ulong)trn.Line) << 32);
            uid |= (ulong)((uint)(trn.CharPositionInLine + 1));
            return uid;
        }

       
        public static string GetPosString(ITree trn)
        {
            int pos = trn.CharPositionInLine;
            return "(" + (trn.Line).ToString() + "," + pos.ToString() + ")";
        }

    }
}