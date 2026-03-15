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
using Antlr.Runtime.Tree;

namespace Mpal.Compiler
{
    internal class FCCodeOutput
    {
        private Unit _unit;
        private Function _func;
        private CodeOutput _codeOutput;

        public FCCodeOutput(Unit unit, Function func, CodeOutput codeOutput)
        {
            _unit = unit;
            _func = func;
            _codeOutput = codeOutput;
        }

        public void EmitCopyFuncParamDefValue(Parameter param, Operand paramBaseOp, ITree trn)
        {
            switch (param.ParamDataType)
            {
                case DataType.ARRAY:
                    EmitCopyFuncArrayDefValues(param, paramBaseOp, trn);
                break;

                case DataType.STRUCT:
                    EmitCopyFuncStructDefValues(param, paramBaseOp, trn);
                break;

                case DataType.UDT:
                    Parameter udtParam = _unit.SearchUDT(param.TypeName);

                    if (param.DefaultValue != null)
                        EmitCopyFuncParamMemberDefValue(udtParam, paramBaseOp, param.DefaultValue, trn);
                    else
                        EmitCopyFuncParamDefValue(udtParam, paramBaseOp, trn);
                break;

                case DataType.FB:
                    EmitCopyFuncFBDefValues(param, paramBaseOp, trn);
                break;

                default:
                    EmitCopyDefValueInst(param, paramBaseOp, param.DefaultValue, trn);
                break;
            }
        }

        private bool HasDefaultValues(Parameter param)
        {
            switch (param.ParamDataType)
            {
                case DataType.STRUCT:
                    foreach (Parameter memParam in param.Structure)
                    {
                        if (HasDefaultValues(memParam))
                            return true;
                    }
                    break;
                case DataType.ARRAY:
                    {
                        Parameter arrayType = param.Structure[0];
                        if (HasDefaultValues(arrayType))
                            return true;
                    }
                    break;
                case DataType.UDT:
                    {
                        Parameter udtParam = _unit.SearchUDT(param.TypeName);
                        if (HasDefaultValues(udtParam))
                            return true;
                    }
                    break;
                case DataType.FB:
                    {
                        Function func = _unit.SearchFunction(param.TypeName);
                        foreach (Parameter varParam in func.Parameters)
                        {
                            if (HasDefaultValues(varParam))
                                return true;
                        }
                    }
                    break;
            }

            return param.DefaultValue != null;
        }

        private void EmitInitFCParamDefValue(Operand paramBaseOp, ITree trn, Parameter stParam, object value)
        {
            if (paramBaseOp.OpType == Operand.Type.TemporaryRef || paramBaseOp.OpType == Operand.Type.Reference ||
                paramBaseOp.OpType == Operand.Type.Temporary)
            {
                if (stParam.Offset != 0)
                {
                    Operand stParamOp = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trn), stParam.Size, stParam.TypeId);

                    _codeOutput.EmitInstruction(InstructionCode.AddOffsetDINT, Helper.GetUID(trn), stParamOp, paramBaseOp,
                                    new Operand(stParam.Offset, Helper.GetUID(trn), 4, "DINT"));

                    EmitCopyFuncParamMemberDefValue(stParam, stParamOp, value, trn);
                    _codeOutput.EmitRemoveTempOperandInst(stParamOp);
                }
                else
                {
                    EmitCopyFuncParamMemberDefValue(stParam, paramBaseOp, value, trn);
                }
            }
            else
            {
                Operand stParamOp = new Operand(paramBaseOp.Offset + stParam.Offset, paramBaseOp.Uid, paramBaseOp.OpType, stParam.Size, stParam.TypeId);
                EmitCopyFuncParamMemberDefValue(stParam, stParamOp, value, trn);
            }
        }

        private void EmitCopyFuncArrayDefValues(Parameter param, Operand paramBaseOp, ITree trn)
        {
            Parameter arrayType = param.Structure[0];

            if (param.DefaultValue != null)
            {
                ArrayList values = (ArrayList)param.DefaultValue;

                for (uint index = 0; index < values.Count; ++index)
                {
                    object obj = values[(int)index];

                    uint offset = (index * arrayType.Size);

                    if (paramBaseOp.OpType == Operand.Type.Reference || paramBaseOp.OpType == Operand.Type.TemporaryRef ||
                        paramBaseOp.OpType == Operand.Type.Temporary)
                    {

                        if (offset != 0)
                        {
                            Operand arrayOp = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trn), param.Size, param.TypeId);

                            _codeOutput.EmitInstruction(InstructionCode.AddOffsetDINT, Helper.GetUID(trn), arrayOp, paramBaseOp,
                                            new Operand(offset, Helper.GetUID(trn), 4, "DINT"));

                            if (obj != null)
                                EmitCopyFuncParamMemberDefValue(arrayType, arrayOp, obj, trn);
                            else
                                EmitCopyFuncParamDefValue(arrayType, arrayOp, trn);

                            _codeOutput.EmitRemoveTempOperandInst(arrayOp);
                        }
                        else
                        {
                            if (obj != null)
                                EmitCopyFuncParamMemberDefValue(arrayType, paramBaseOp, obj, trn);
                            else
                                EmitCopyFuncParamDefValue(arrayType, paramBaseOp, trn);
                        }
                    }
                    else
                    {
                        Operand arrayOp = new Operand(paramBaseOp.Offset + offset, paramBaseOp.Uid, Operand.Type.Direct, param.Size, param.TypeId);

                        if (obj != null)
                            EmitCopyFuncParamMemberDefValue(arrayType, arrayOp, obj, trn);
                        else
                            EmitCopyFuncParamDefValue(arrayType, arrayOp, trn);
                    }
                }
            }
            else if (HasDefaultValues(arrayType))
            {//Element initialisation

                for (uint i = 0; i < param.Size; i += arrayType.Size)
                {
                    if (i != 0)
                    {
                        //calc element offset 
                        Operand elemOp = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trn), arrayType.Size, arrayType.TypeId);

                        _codeOutput.EmitInstruction(InstructionCode.AddOffsetDINT, Helper.GetUID(trn), elemOp, paramBaseOp,
                                        new Operand(i, Helper.GetUID(trn), 4, "DINT"));

                        EmitCopyFuncParamDefValue(arrayType, elemOp, trn);
                        _codeOutput.EmitRemoveTempOperandInst(elemOp);
                    }
                    else
                    {
                        EmitCopyFuncParamDefValue(arrayType, paramBaseOp, trn);
                    }
                }
            }
        }

        private void EmitInitFCParamDefValue(Operand paramBaseOp, ITree trn, Parameter param)
        {
            if (!HasDefaultValues(param))
                return;

            if (paramBaseOp.OpType == Operand.Type.TemporaryRef || paramBaseOp.OpType == Operand.Type.Reference ||
                paramBaseOp.OpType == Operand.Type.Temporary)
            {
                if (param.Offset != 0)
                {
                    Operand stParamOp = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trn), param.Size, param.TypeId);

                    _codeOutput.EmitInstruction(InstructionCode.AddOffsetDINT, Helper.GetUID(trn), stParamOp, paramBaseOp,
                                      new Operand(param.Offset, Helper.GetUID(trn), 4, "DINT"));

                    EmitCopyFuncParamDefValue(param, stParamOp, trn);
                    _codeOutput.EmitRemoveTempOperandInst(stParamOp);
                }
                else
                {
                    EmitCopyFuncParamDefValue(param, paramBaseOp, trn);
                }
            }
            else
            {
                Operand stParamOp = new Operand(paramBaseOp.Offset + param.Offset, paramBaseOp.Uid, paramBaseOp.OpType, param.Size, param.TypeId);
                EmitCopyFuncParamDefValue(param, stParamOp, trn);
            }
        }

        private void EmitCopyFuncFBDefValues(Parameter param, Operand paramBaseOp, ITree trn)
        {
            Function func = _unit.SearchFunction(param.TypeName);

            if (param.DefaultValue != null)
            {
                ArrayList list = (ArrayList)param.DefaultValue;

                foreach ( Parameter stParam in func.Parameters)
                {
                    object value = FindKeyInList(list, stParam.Index);
                    
                    if( value != null)
                        EmitInitFCParamDefValue(paramBaseOp, trn, stParam, value);
                    else
                        EmitInitFCParamDefValue(paramBaseOp, trn, stParam);
                }
            }
            else
            {
                foreach (Parameter fparam in func.Parameters)
                    EmitInitFCParamDefValue(paramBaseOp, trn, fparam);
            }
        }

        private void EmitCopyDefValueInst(Parameter param, Operand op, object value, ITree trn)
        {
            Operand valueOp = null;

            if (value != null)
                valueOp = new Operand(value, param.UID, param.Size, param.TypeId);
            else
                valueOp = new Operand(0, param.UID, param.Size, param.TypeId);

            _codeOutput.EmitInstruction(InstructionCode.Move, Helper.GetUID(trn), op, valueOp, new Operand(param.Size, 0, 4, "UDINT"));
        }

        private void EmitCopyFuncStructDefValues(Parameter param, Operand paramBaseOp, ITree trn)
        {
            if (param.DefaultValue != null)
            {//Copy struct default value
                ArrayList list = (ArrayList)param.DefaultValue;
                foreach(Parameter stParam in param.Structure)
                {
                    object value = FindKeyInList(list, stParam.Index);

                    if( value != null)
                        EmitInitFCParamDefValue(paramBaseOp, trn, stParam, value);
                    else
                        EmitInitFCParamDefValue(paramBaseOp, trn, stParam);
                }
            }
            else
            {//Copy member default value
                foreach (Parameter stParam in param.Structure)
                    EmitInitFCParamDefValue(paramBaseOp, trn, stParam);
            }
        }


        private object FindKeyInList(ArrayList list, int key)
        {
            foreach (DictionaryEntry entry in list)
            {
                int curKey = (int)entry.Key;
                if (curKey == key)
                    return entry.Value;
            }

            return null;
        }

        private void EmitCopyFuncArrayDefValues(Parameter param, Operand paramBaseOp, ITree trn, object value)
        {
            Parameter arrayType = param.Structure[0];

            ArrayList values = (ArrayList)value;

            for (uint index = 0; index < values.Count; ++index)
            {
                object obj = values[(int)index];
                uint offset = (index * arrayType.Size);

                if (paramBaseOp.OpType == Operand.Type.Reference || paramBaseOp.OpType == Operand.Type.TemporaryRef ||
                    paramBaseOp.OpType == Operand.Type.Temporary)
                {
                    if (offset != 0)
                    {
                        Operand arrayOp = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trn), param.Size, param.TypeId);

                        _codeOutput.EmitInstruction(InstructionCode.AddOffsetDINT, Helper.GetUID(trn), arrayOp, paramBaseOp,
                                        new Operand(offset, Helper.GetUID(trn), 4, "DINT"));

                        EmitCopyFuncParamMemberDefValue(arrayType, arrayOp, obj, trn);
                        _codeOutput.EmitRemoveTempOperandInst(arrayOp);
                    }
                    else
                    {
                        EmitCopyFuncParamMemberDefValue(arrayType, paramBaseOp, obj, trn);
                    }
                }
                else
                {
                    Operand arrayOp = new Operand(paramBaseOp.Offset + offset, paramBaseOp.Uid, Operand.Type.Direct, param.Size, param.TypeId);
                    EmitCopyFuncParamMemberDefValue(arrayType, arrayOp, obj, trn);
                }
            }
        }


        private void EmitCopyFuncParamMemberDefValue(Parameter param, Operand op, object value, ITree trn)
        {
            switch (param.ParamDataType)
            {
                case DataType.ARRAY:

                    if (param.DefaultValue == null)
                        return;

                    EmitCopyFuncArrayDefValues(param, op, trn, value);
                    break;

                case DataType.STRUCT:
                    ArrayList list = (ArrayList)value;

                    foreach (Parameter stParam  in param.Structure)
                    {
                        object paramValue = FindKeyInList(list, stParam.Index);
                        if( paramValue != null)
                            EmitInitFCParamDefValue(op, trn, stParam, paramValue);
                        else
                            EmitInitFCParamDefValue(op, trn, stParam);
                    }
                    break;

                case DataType.UDT:
                    Parameter udtParam = _unit.SearchUDT(param.TypeName);
                    EmitCopyFuncParamMemberDefValue(udtParam, op, value, trn);
                    break;

                default:
                    EmitCopyDefValueInst(param, op, value, trn);
                    break;
            }
        }
    }
}
