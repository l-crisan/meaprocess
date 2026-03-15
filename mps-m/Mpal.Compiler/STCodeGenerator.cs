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
using System.Globalization;

using System.Text;

using Antlr.Runtime;
using Antlr.Runtime.Tree;

using Mpal.Model;

namespace Mpal.Compiler
{
    internal class STCodeGenerator
    {
        private Unit        _unit;
        private Function    _function;
        private Operand     _loopEndLabel = null;
        private Operand     _loopStartLabel = null;
        private CompilerOptions _options;
        private CodeOutput  _codeOutput;
        
        public STCodeGenerator(CompilerOptions options)
        {
            _options = options;
        }

        public event Message OnMessage;

        public void Generate(Unit unit, Function function)
        {
            _unit = unit;
            _function = function;
            _codeOutput = new CodeOutput(_function, _options);

            //Cache all function member
            ITree fistStatment = GetFirstStatment(function.ASTNode);

            foreach (Parameter param in _function.Parameters)
                EmitCreateFuncMemberOp(fistStatment, param);

            EmitInstructions(function.ASTNode);

            //Remove cached function member            
            Instruction lastInst = _function.Instructions[_function.Instructions.Count - 1];
            foreach (DictionaryEntry entry in _function.OperandCache)
            {
                Operand op = (Operand) entry.Value;
                op.Uid = lastInst.UID;
                _codeOutput.EmitRemoveTempOperandInst(op);
            }
            _function.OperandCache.Clear();
        }


        private void EmitInstructions(ITree trnItem)
        {
            for (int i = 1; i < trnItem.ChildCount; ++i)
            {
                ITree trnStatment = trnItem.GetChild(i);
                
                if( !IsDeclaration(trnStatment))
                    EvalStatment(trnStatment);
            }
        }
        
        private ITree GetFirstStatment(ITree funcNode)
        {
            for (int i = 1; i < funcNode.ChildCount; ++i)
            {
                ITree trnStatment = funcNode.GetChild(i);

                if (!IsDeclaration(trnStatment))
                    return trnStatment;
            }

            return null;
        }

        private bool IsDeclaration(ITree trn)
        {
            switch (trn.Type)
            {
                case Mpal.Parser.Parser.VAR_INPUT:
                case Mpal.Parser.Parser.VAR_OUTPUT:
                case Mpal.Parser.Parser.VAR_IN_OUT:
                case Mpal.Parser.Parser.VAR:
                case Mpal.Parser.Parser.VAR_TEMP:
                case Mpal.Parser.Parser.TYPE:
                case Mpal.Parser.Parser.END_FUNCTION:
                case Mpal.Parser.Parser.END_PROGRAM:
                case Mpal.Parser.Parser.END_FUNCTION_BLOCK:
                    return true;                
            }

            return false;
        }

        private Parameter SearchEnumeration(string item, out int value)
        {
            value = 0;
            foreach ( DictionaryEntry entry in _unit.Types)
            {
                Parameter param = (Parameter) entry.Value;

                if (param.ParamDataType == DataType.ENUM)
                {
                    for (int i = 0; i < param.EnumList.Count; ++i)
                    {
                        if (param.EnumList[i] == item)
                        {
                            value = i;
                            return param;
                        }
                    }
                }
            }

            return null;
        }

        private Parameter SearchParameter(string ident)
        {
            foreach (Parameter param in _function.Parameters)
            {
                if (param.Name == ident)
                    return param;
            }

            return null;
        }

        private Parameter GetFunctionParam(ITree parentNode)
        {
            if (parentNode.ChildCount == 0)
            {
                return _function.GetParameter(parentNode.Text);
            }
            else
            {
                if (parentNode.Type == Parser.Parser.DOT)
                {
                    ITree left = parentNode.GetChild(0);
                    ITree right = parentNode.GetChild(1);
                    Parameter leftParam = GetFunctionParam(left);

                    if (leftParam.ParamDataType == DataType.UDT)
                        leftParam = _unit.SearchUDT(leftParam.TypeName);

                    foreach (Parameter param in leftParam.Structure)
                        if (param.Name == right.Text)
                            return param;
                }
                else if( parentNode.Type == Parser.Parser.LBRACKED)
                {

                    ITree leftNode = parentNode.GetChild(parentNode.ChildCount - 1);
                    Parameter param = GetFunctionParam(leftNode);
                    return param.Structure[0];
                }
            }

            return null;
        }

        private Function SearchFunction(ITree fnode, Unit unit, out Operand obj)
        {
            obj = null;

            if (fnode.Type == Parser.Parser.DOT)
            {
                try
                {
                    Parameter fbParam = GetFunctionParam(fnode);
                    obj = EvalMemSelectExpression(fnode);
                    return unit.SearchFunction(fbParam.TypeName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return null;
                }
            }            
            else if( fnode.Type == Parser.Parser.LBRACKED)
            {
                ITree arrayOfNode = fnode.GetChild(fnode.ChildCount - 1);

                Parameter arrayParam = GetFunctionParam(arrayOfNode);
                if (arrayParam.Structure.Count == 0)
                    return null;

                Function func = unit.SearchFunction(arrayParam.Structure[0].TypeName);
                obj = EvalArraySelectExpression(fnode);
                return func;
            }
            else
            {
                obj = null;

                Function func = unit.SearchFunction(fnode.Text);

                if (func != null)
                {
                    if (func.FuncType == Function.Type.PG)
                        return null;

                    return func;
                }
                obj = EvalIdentExpression(fnode);

                Parameter fbParam = _function.GetParameter(fnode.Text);

                if (fbParam == null)
                    return null;

                if (fbParam.ParamDataType == DataType.FB)
                    return (Function)_unit.Functions[fbParam.TypeName];

                return null;
            }
        }
    
        private Operand EvalRealConstant(ITree trn)
        {
            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";
            try
            {
                return new Operand(Convert.ToDouble(trn.Text, info), Helper.GetUID(trn), sizeof(double), DataType.LREAL.ToString());
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                OutputMessage(Helper.GetPosString(trn) + Messages.C1006);
                return null;
            }
        }

        private Operand EvalIntegerConstant(ITree trn)
        {
            try
            {
                if (trn.Text.Contains("#"))
                {
                    string[] array = trn.Text.Split('#');
                    long no = Convert.ToInt64(array[1], Convert.ToInt32(array[0]));
                    return new Operand(no, Helper.GetUID(trn), sizeof(long), DataType.DINT.ToString());
                }
                else
                {
                    return new Operand(Convert.ToInt64(trn.Text), Helper.GetUID(trn), sizeof(long), DataType.DINT.ToString());
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                OutputMessage(Helper.GetPosString(trn) + Messages.C1005);
                return null;
            }
        }

        private Operand EvalIdentExpression(ITree trn)
        {
            Parameter param = SearchParameter(trn.Text);

            if (param == null)
            {
                int value = 0;

                param = SearchEnumeration(trn.Text, out value);
                if(param == null)
                {
                    OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1002, trn.Text));
                    return null;
                }

                return new Operand(value, Helper.GetUID(trn), 4, "ENUMERATION");
            }
            
            _function.Uid2Param[Helper.GetUID(trn)] = new StructVarUid(0, Helper.GetUID(trn), param.Name);
            _function.Uid2Var[Helper.GetUID(trn)] = param;
            
            if (_function.OperandCache.Contains(param.Name))
            {
                Operand op = (Operand) _function.OperandCache[param.Name];
                Operand rsOp = new Operand(op);
                
                rsOp.Uid = Helper.GetUID(trn);
                return rsOp;
            }

            return EmitCreateFuncMemberOp(trn, param);
        }

        private Operand EmitCreateFuncMemberOp(ITree trn, Parameter param)
        {
            string typeId = param.TypeId;

            if (param.ParamDataType == DataType.UDT)
            {
                Parameter udtParam = _unit.SearchUDT(param.TypeName);
                typeId = udtParam.ParamDataType == DataType.ENUM ? "ENUMERATION" : param.TypeId;
            }

            ulong uid = Helper.GetUID(trn);
            Operand retVal = null; 

            bool isConstant = param.ParamAccess == Parameter.Access.VarConst || param.ParamAccess == Parameter.Access.VarTempConst;

            if (isConstant && (Helper.IsNumericType(typeId) || Helper.IsBitStringType(typeId)))
            {
                retVal = new Operand(param.DefaultValue, uid, param.Size, typeId);
                _function.OperandCache.Add(param.Name, retVal);
                retVal.Cached = true;
                return retVal;
            }

            if (_function.FuncType != Function.Type.FB)
            {
                if (_function.IsStackParam(param))
                {
                    retVal = new Operand(param.Offset, uid, Operand.Type.Direct, param.Size, typeId);
                    retVal.IsConstant = isConstant;
                    retVal.Cached = true;
                    _function.OperandCache.Add(param.Name, retVal);
                    return retVal;
                }
                else
                {
                    retVal = new Operand(param.Offset, uid, Operand.Type.Reference, param.Size, typeId);
                    retVal.IsConstant = isConstant;
                    retVal.Cached = true;
                    _function.OperandCache.Add(param.Name, retVal);
                    return retVal;
                }
            }
            else
            {//Function block
                if (_function.IsStackParam(param))
                {
                    retVal = new Operand(param.Offset, uid, Operand.Type.Direct, param.Size, typeId);
                    retVal.IsConstant = isConstant;
                    retVal.Cached = true;
                    _function.OperandCache.Add(param.Name, retVal);
                    return retVal;
                }
                else
                {
                    if (param.Offset == 0)
                    {
                        retVal = new Operand(0, uid, Operand.Type.Reference, param.Size, typeId);
                        retVal.IsConstant = isConstant;
                        retVal.Cached = true;
                        _function.OperandCache.Add(param.Name, retVal);
                        return retVal;
                    }
                    else 
                    {
                        Operand obj = new Operand(0, uid, Operand.Type.Reference, 4, typeId);

                        retVal = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trn), param.Size, typeId);

                        _codeOutput.EmitInstruction( InstructionCode.AddOffsetDINT, Helper.GetUID(trn), retVal, obj,
                                                     new Operand(param.Offset, Helper.GetUID(trn), 4, "DINT"));

                        retVal.IsConstant = isConstant;
                        retVal.Cached = true;
                        _function.OperandCache.Add(param.Name, retVal);
                        return retVal;
                    }
                }
            }
        }

        private Operand EmitConvertInstruction(Operand from, Operand to, bool noMessage)
        {
            if (from.TypeId == to.TypeId)
                return from;

            InstructionCode code = Helper.GetImplConvertInstructionCode(from.TypeId, to.TypeId);

            if (code == InstructionCode.NOP)
            {
                if (!noMessage)
                    OutputMessage(GetPosInfo(to) + String.Format(Messages.C1001, from.TypeId, to.TypeId));                    

                return null;
            }

            //Create a temporary operand.
            Operand result = _codeOutput.EmitCreateTempOperandInst(to.Uid, to.Size, to.TypeId);
           
            //Create the convert instruction
            _codeOutput.EmitInstruction(code, from.Uid, result, from, null);

            return result;
        }

        private Operand CreateConvertedOperand(Operand from, Operand to)
        {
            Operand convertedOp = from;

            if (from.OpType == Operand.Type.Immediate)
                Helper.ConvertConst(from, to);

            if (from.TypeId != to.TypeId)
            {
                InstructionCode code = Helper.GetImplConvertInstructionCode(from.TypeId, to.TypeId);

                if (code == InstructionCode.NOP)
                {
                    OutputMessage(GetPosInfo(from) + String.Format(Messages.C1001, from.TypeId, to.TypeId));
                    return null;
                }

                convertedOp = _codeOutput.EmitCreateTempOperandInst(from.Uid, to.Size, to.TypeId);
            }

            return convertedOp;
        }

        private Operand EvalBinaryExpression(ITree trn, InstructionCategory category, Operand resultOp)
        {
            Instruction createResultTempInst = null;
            Operand sizeOp = new Operand(0, 0, 4, "UDINT");
            
            if (resultOp == null)
            {
                resultOp = new Operand(_codeOutput.CreateTempOpID(), Helper.GetUID(trn), Operand.Type.Temporary, 0, "");
                createResultTempInst = _codeOutput.EmitInstruction(InstructionCode.PushTempOp, Helper.GetUID(trn), resultOp, sizeOp, null);
            }
            
            //Eval the operands.            
            Operand op1 = EvalExpression(trn.GetChild(0), null);
            Operand op2 = EvalExpression(trn.GetChild(1), null);
            
            //Check the operands 
            if (op1 == null || op2 == null)
                return null;

            if (op1.OpType == Operand.Type.Immediate && op2.OpType == Operand.Type.Immediate)
            {                
                Operand result = Helper.EvalConstantOperation(op1, op2, category);                

                if( result == null)
                {
                    OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1004,op1.TypeId, op2.TypeId ));
                    return null;
                }

                if (createResultTempInst != null)
                    _function.Instructions.Remove(createResultTempInst);

                return result;
            }

            //Convert implicitly the operands if neccessery
            if (op1.OpType == Operand.Type.Immediate)
                Helper.ConvertConst(op1, op2);

            if (op2.OpType == Operand.Type.Immediate)
                Helper.ConvertConst(op2, op1);

            Operand opConv1 = op1;
            Operand opConv2 = op2;

            uint resOpSize = 0;

            string resultOpType = Helper.GetOperationResultType(opConv1, category, ref resOpSize);

            if (createResultTempInst != null || resultOpType == "BOOL")
            {
                Operand greatherOp = Helper.GetGreaterType(op1, op2);

                if (greatherOp == op1)
                    opConv2 = EmitConvertInstruction(op2, op1, false);

                if (greatherOp == op2)
                    opConv1 = EmitConvertInstruction(op1, op2, false);

                if (opConv1 == null || opConv2 == null)
                    return null;
            }
            else
            {
                opConv2 = EmitConvertInstruction(op2, resultOp, false);
                opConv1 = EmitConvertInstruction(op1, resultOp, false);
            }
            
            //Initialize the result operand => back patching
            resultOpType = Helper.GetOperationResultType(opConv1, category, ref resOpSize);

            if (createResultTempInst != null)
            {
                resultOp.Size = resOpSize;
                resultOp.TypeId = resultOpType;
            }
            else
            {
                if (resultOpType != resultOp.TypeId)
                {
                    OutputMessage(Helper.GetUID(trn) + String.Format(Messages.C1001, resultOp.TypeId, resultOpType));
                    return null;
                }
            }

            sizeOp.ConstVal = resOpSize;
            
            //Create the add instruction.
            InstructionCode instCode = Helper.GetInstructionCode(opConv1.TypeId, category);

            if (instCode == InstructionCode.NOP)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1004, op1.TypeId, op2.TypeId));
                return null;
            }

            //Check devision by zero.
            if (category == InstructionCategory.Div || category == InstructionCategory.Mod)
            {
                if (opConv2.OpType == Operand.Type.Immediate)
                {
                    if (Helper.IsConstValueZero(opConv2))
                    {
                        OutputMessage(Helper.GetPosString(trn.GetChild(1)) + Messages.C1051);
                        return null;
                    }
                }
            }

            _codeOutput.EmitInstruction(instCode, Helper.GetUID(trn), resultOp, opConv1, opConv2);

            if (opConv2 != op2)
                _codeOutput.EmitRemoveTempOperandInst(opConv2);

            if (opConv1 != op1)
                _codeOutput.EmitRemoveTempOperandInst(opConv1);

            _codeOutput.EmitRemoveTempOperandInst(op2);
            _codeOutput.EmitRemoveTempOperandInst(op1);
            return resultOp;
        }

        private Operand EvalSignExpression(ITree trn, Operand resultOp)
        {
            if (trn.Type == Mpal.Parser.Parser.PLUS)
            {
                if (trn.ChildCount == 2)
                    return EvalBinaryExpression(trn, InstructionCategory.Add, resultOp);
                else
                    return EvalUnaryExpression(trn, InstructionCategory.Plus);
            }
            else if (trn.Type == Mpal.Parser.Parser.NEG)
            {
                if (trn.ChildCount == 2)
                    return EvalBinaryExpression(trn, InstructionCategory.Sub, resultOp);
                else
                    return EvalUnaryExpression(trn, InstructionCategory.Neg);
            }

            return null;
        }
        private Operand EvalExpression(ITree trn)
        {
            return EvalExpression(trn, null);
        }

        private Operand EvalExpression(ITree trn, Operand resultOp)
        {
            switch (trn.Type)
            {
                case Mpal.Parser.Parser.IDENTIFIER:
                    return EvalIdentExpression(trn);

                case Mpal.Parser.Parser.PLUS:
                case Mpal.Parser.Parser.NEG:
                    return EvalSignExpression(trn, resultOp);

                case Mpal.Parser.Parser.MUL:
                    return EvalBinaryExpression(trn, InstructionCategory.Mul, resultOp);
                
                case Mpal.Parser.Parser.DIV:
                    return EvalBinaryExpression(trn, InstructionCategory.Div, resultOp);
                
                case Mpal.Parser.Parser.REAL_CONSTANT:
                    return EvalRealConstant(trn);
                
                case Mpal.Parser.Parser.INTEGER:
                case Mpal.Parser.Parser.HEX_INTEGER:
                case Mpal.Parser.Parser.BINARY_INTEGER:
                case Mpal.Parser.Parser.OCTAL_INTEGER:
                    return EvalIntegerConstant(trn);               

                case Mpal.Parser.Parser.GR:
                    return EvalBinaryExpression(trn, InstructionCategory.Gr, resultOp);

                case Mpal.Parser.Parser.GEQ:
                    return EvalBinaryExpression(trn, InstructionCategory.Ge, resultOp);

                case Mpal.Parser.Parser.LS:
                    return EvalBinaryExpression(trn, InstructionCategory.Ls, resultOp);

                case Mpal.Parser.Parser.LEQ:
                    return EvalBinaryExpression(trn, InstructionCategory.Le, resultOp);

                case Mpal.Parser.Parser.NEQ:
                    return EvalBinaryExpression(trn, InstructionCategory.Ne, resultOp);

                case Mpal.Parser.Parser.EQU:
                    return EvalBinaryExpression(trn, InstructionCategory.Eq, resultOp);

                case Mpal.Parser.Parser.MOD:
                    return EvalBinaryExpression(trn, InstructionCategory.Mod, resultOp);

                case Mpal.Parser.Parser.AND:
                    return EvalBinaryExpression(trn, InstructionCategory.And, resultOp);

                case Mpal.Parser.Parser.OR:
                    return EvalBinaryExpression(trn, InstructionCategory.Or, resultOp);

                case Mpal.Parser.Parser.XOR:
                    return EvalBinaryExpression(trn, InstructionCategory.Xor, resultOp);

                case Mpal.Parser.Parser.POW:
                    {
                        if (!_options.SupportMathLIB)
                        {
                            OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1052, trn.GetChild(0).Text));
                        }
                        return EvalBinaryExpression(trn, InstructionCategory.Pow, resultOp);
                    }
                
                case Mpal.Parser.Parser.NOT:
                    return EvalUnaryExpression(trn, InstructionCategory.Not);
                
                case Mpal.Parser.Parser.TRUE:
                    return new Operand(true, Helper.GetUID(trn), 1, "BOOL");
                
                case Mpal.Parser.Parser.FALSE:
                    return new Operand(false, Helper.GetUID(trn), 1, "BOOL");

                case Mpal.Parser.Parser.DOT:
                    return EvalMemSelectExpression(trn);

                case Mpal.Parser.Parser.LBRACKED:
                    return EvalArraySelectExpression(trn);

                case Mpal.Parser.Parser.LRBRACKED:
                    return EvalFunctionStmt(trn);
                
                case Mpal.Parser.Parser.SHARP:
                    return EvalSharp(trn);
            }

            throw new Exception("Unknow Expresion type.");
        }

        private Operand EvalSharp(ITree trn)
        {
            ITree trnType = trn.GetChild(0);

            Operand op = EvalExpression(trn.GetChild(1));
            
            if (op == null)
                return null;

            Helper.ConvertConst(op, new Operand(0, 0, Helper.SizeOf(trnType.Text), trnType.Text));

            if (op.TypeId != trnType.Text)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1001, op.TypeId, trnType.Text));
                return null;
            }
            return op;
        }

        private void EvalIncDecFunction(ITree trn, InstructionCategory cat)
        {
            if (!CheckStdFuncCall(trn))
                return;

            if (trn.GetChild(1).Type != Mpal.Parser.Parser.IDENTIFIER)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(1)) + Messages.C1016);
                return;
            }

            Parameter param = SearchParameter(trn.GetChild(1).Text);

            if (param == null)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(1)) + String.Format(Messages.C1002, trn.GetChild(1).Text));
                return;
            }

            if ( param.ParamAccess == Parameter.Access.VarConst ||
                 param.ParamAccess == Parameter.Access.VarTempConst)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(1)) + Messages.C1015);
                return;
            }

            InstructionCode code = InstructionCode.NOP;

            if (param.ParamDataType != DataType.UDT)
            {
                code = Helper.GetInstructionCode(param.TypeId, cat);
            }
            else
            {
                Parameter udtParam = _unit.GetUDTBaseType(param.TypeId);
                code = Helper.GetInstructionCode(udtParam.ParamDataType.ToString(), cat);
            }

            if (code == InstructionCode.NOP)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(0)) + String.Format(Messages.C1004_1, trn.GetChild(0).Text));
                return;
            }

            Operand incOp = EvalExpression(trn.GetChild(1));

            if (incOp == null)
                return;

            if (incOp.IsConstant)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(1)) + Messages.C1015);
                return;
            }

            _codeOutput.EmitInstruction(code, Helper.GetUID(trn.GetChild(0)), incOp, null, null);
        }
        
        private Operand EvalRoundTruncFunction(ITree trn, InstructionCode code)
        {
            if (!CheckStdFuncCall(trn))
                return null;

            //Create the temporary result operand
            Operand resOp = _codeOutput.EmitCreateTempOperandInst(0, 4, "DINT");

            Operand srcOp = EvalExpression(trn.GetChild(1));

            if (srcOp == null)
                return null;

            if (srcOp.TypeId != "LREAL" && srcOp.TypeId != "REAL")
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(1)) + Messages.C1027);
                return null;
            }

            Operand convSrcOp = null;

            if( _options.SupportLREAL)
                convSrcOp = EmitConvertInstruction(srcOp, new Operand(0,0,8,"LREAL"));
            else
                convSrcOp = EmitConvertInstruction(srcOp, new Operand(0, 0, 8, "REAL"));

            _codeOutput.EmitInstruction(code, Helper.GetUID(trn.GetChild(0)), resOp, convSrcOp, null);

            if (convSrcOp != srcOp)
                _codeOutput.EmitRemoveTempOperandInst(convSrcOp);

            _codeOutput.EmitRemoveTempOperandInst(srcOp);

            return resOp;
        }

        private Operand EvalMathFunction(ITree trn, InstructionCategory cat)
        {
            if (!_options.SupportMathLIB)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1052, trn.GetChild(0).Text));
                return null;
            }

            Operand resOp = new Operand(_codeOutput.CreateTempOpID(), 0, Operand.Type.Temporary, 0, "");
            Operand sizeOp = new Operand(0, 0, 4, "UDINT");

            _codeOutput.EmitInstruction(InstructionCode.PushTempOp, Helper.GetUID(trn), resOp, sizeOp, null);

            Operand srcOp = EvalExpression(trn.GetChild(1));

            if (srcOp == null)
                return null;

            if (cat == InstructionCategory.Abs)
            {
                resOp.TypeId = srcOp.TypeId;
                resOp.Size = srcOp.Size;
                sizeOp.ConstVal = srcOp.Size;
            }
            else
            {
                if (_options.SupportLREAL)
                {
                    resOp.TypeId = "LREAL";
                    resOp.Size = 8;
                    sizeOp.ConstVal = 8;
                }
                else
                {
                    resOp.TypeId = "REAL";
                    resOp.Size = 4;
                    sizeOp.ConstVal = 4;
                }
            }
            
            if(!Helper.IsNumericType(srcOp.TypeId))
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(1)) + String.Format(Messages.C1028, trn.GetChild(0).Text));
                return null;
            }

            InstructionCode inst = Helper.GetInstructionCode(srcOp.TypeId, cat);
            _codeOutput.EmitInstruction(inst, Helper.GetUID(trn), resOp, srcOp, null);

            _codeOutput.EmitRemoveTempOperandInst(srcOp);
            return resOp;
        }

        private Operand EvalBitStringFunction(ITree trn, InstructionCategory cat)
        {
            if (trn.ChildCount != 3)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(1)) + String.Format(Messages.C1030, trn.GetChild(0).Text));
                return null;
            }

            if (trn.GetChild(1).Type != Mpal.Parser.Parser.ASSIGN)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(1)) + String.Format(Messages.C1031, trn.GetChild(0).Text));
                return null;
            }

            if (trn.GetChild(2).Type != Mpal.Parser.Parser.ASSIGN)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(2)) + String.Format(Messages.C1031, trn.GetChild(0).Text));
                return null;
            }
            
            ITree trn1 = trn.GetChild(1).GetChild(0);
            ITree trn2 = trn.GetChild(2).GetChild(0);

            if (trn1.Text != "IN" && trn2.Text != "IN")
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(2)) + String.Format(Messages.C1032, trn.GetChild(0).Text));
                return null;
            }
            
            if( trn1.Text != "N" && trn2.Text != "N")
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(2)) + String.Format(Messages.C1033, trn.GetChild(0).Text));
                return null;
            }

            ITree trnIn = null;
            ITree trnN = null;

            if (trn1.Text == "IN")
                trnIn = trn.GetChild(1);
            else
                trnIn = trn.GetChild(2);

            if( trn2.Text == "N")
                trnN = trn.GetChild(2);
            else
                trnN = trn.GetChild(1);


            Operand resOp = new Operand(_codeOutput.CreateTempOpID(), Helper.GetUID(trn.GetChild(0)), Operand.Type.Temporary, 0, "");
            Operand sizeOp = new Operand(0, 0, 4, "UDINT");

            _codeOutput.EmitInstruction(InstructionCode.PushTempOp, Helper.GetUID(trn), resOp, sizeOp, null);


            Operand srcOp = EvalExpression(trnIn.GetChild(1));
            
            if (srcOp == null)
                return null;

            if (!Helper.IsBitStringType(srcOp.TypeId))
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(1)) + String.Format(Messages.C1029, trn.GetChild(0).Text));
                return null;
            }

            resOp.TypeId    = srcOp.TypeId;
            resOp.Size      = srcOp.Size;
            sizeOp.ConstVal = srcOp.Size;

            Operand countOp = EvalExpression(trnN.GetChild(1));
            
            if (countOp == null)
                return null;

            Operand convCountOp = EmitConvertInstruction(countOp, new Operand(0, 0, 2, "INT"));
            
            if (convCountOp == null)
                return null;

            InstructionCode code = Helper.GetInstructionCode(srcOp.TypeId, cat);
            
            if (code == InstructionCode.NOP)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(0)) + String.Format(Messages.C1034, trn.GetChild(0).Text, srcOp.TypeId));
                return null;
            }

            _codeOutput.EmitInstruction(code, Helper.GetUID(trn.GetChild(0)), resOp, srcOp, convCountOp);

            if (convCountOp != countOp)
                _codeOutput.EmitRemoveTempOperandInst(convCountOp);

            _codeOutput.EmitRemoveTempOperandInst(countOp);
            _codeOutput.EmitRemoveTempOperandInst(srcOp);

            return resOp;
        }
        
        private Operand EvalLimitFunction(ITree trn)
        {
            //Check the parameter.
            if (trn.ChildCount != 4)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1030, trn.GetChild(0).Text));
                return null;
            }

            Hashtable paraAssignMap = new Hashtable();

            bool error = false;
            for (int i = 1; i < trn.ChildCount; ++i)
            {
                ITree trnParaAss = trn.GetChild(i);

                if (trnParaAss.Type != Mpal.Parser.Parser.ASSIGN)
                {
                    OutputMessage(Helper.GetPosString(trnParaAss) + String.Format(Messages.C1031, trn.GetChild(0).Text));
                    error = true;
                    continue;
                }

                if (trnParaAss.GetChild(0).Text != "IN" &&
                    trnParaAss.GetChild(0).Text != "MN" &&
                    trnParaAss.GetChild(0).Text != "MX")
                {
                    error = true;
                    OutputMessage(Helper.GetPosString(trnParaAss.GetChild(0)) + String.Format(Messages.C1037, trnParaAss.GetChild(0).Text, trn.GetChild(0).Text));
                    continue;
                }

                paraAssignMap.Add(trnParaAss.GetChild(0).Text, trnParaAss.GetChild(1));
            }

            if (error)
                return null;

            if (paraAssignMap.Count != 3)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1030, trn.GetChild(0).Text));
                return null;
            }
            
            //Create the temporary result operand
            Operand resultOp = new Operand(_codeOutput.CreateTempOpID(), Helper.GetUID(trn), Operand.Type.Temporary, 0, "");
            Operand sizeOp = new Operand(0, 0, 4, "UDINT");
            _codeOutput.EmitInstruction(InstructionCode.PushTempOp, Helper.GetUID(trn), resultOp, sizeOp, null);

            //Eval the param assignment.
            Operand inParamOp = EvalExpression((ITree)paraAssignMap["IN"]);
            Operand mnParamOp = EvalExpression((ITree)paraAssignMap["MN"]);
            Operand mxParamOp = EvalExpression((ITree)paraAssignMap["MX"]);

            if (inParamOp == null || mnParamOp == null || mxParamOp == null)
                return null;

            List<Operand> opParams = new List< Operand>();
            opParams.Add(inParamOp);
            opParams.Add(mnParamOp);
            opParams.Add(mxParamOp);

            Operand greatherOp = Helper.GetGreaterOperandByType(opParams);

            List<Operand> convOps = new List<Operand>();

            for (int i = 0; i < opParams.Count; ++i)
            {
                Operand op = EmitConvertInstruction(opParams[i], greatherOp);

                if (op == null)
                    return null;

                convOps.Add(op);
            }

            //Back patch the result op.
            resultOp.Size = greatherOp.Size;
            resultOp.TypeId = greatherOp.TypeId;
            sizeOp.ConstVal = greatherOp.Size;

            _codeOutput.EmitInstruction(InstructionCode.PushRef, Helper.GetUID(trn.GetChild(0)), null, convOps[1], null);
            _codeOutput.EmitInstruction(InstructionCode.PushRef, Helper.GetUID(trn.GetChild(0)), null, convOps[2], null);

            InstructionCode code = Helper.GetInstructionCode(greatherOp.TypeId, InstructionCategory.Limit);
            if (code == InstructionCode.NOP)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1004_1, greatherOp.TypeId));
                return null;
            }

            _codeOutput.EmitInstruction(code, Helper.GetUID(trn.GetChild(0)), resultOp, convOps[0], null);

            //Clear the stack.             
            for(int i = convOps.Count - 1; i > -1; --i)
            {
                Operand op = convOps[i];

                if (!opParams.Contains(op))
                    _codeOutput.EmitRemoveTempOperandInst(op);
            }

            for (int i = opParams.Count - 1; i > -1; --i)
            {
                Operand op = opParams[i];
                _codeOutput.EmitRemoveTempOperandInst(op);
            }

            return resultOp;
        }

        private Operand EvalSelFunction(ITree trn)
        {
            //Check the parameter.
            if (trn.ChildCount != 4)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1030, trn.GetChild(0).Text));
                return null;
            }

            Hashtable paraAssignMap = new Hashtable();

            bool error = false;
            for (int i = 1; i < trn.ChildCount; ++i)
            {
                ITree trnParaAss = trn.GetChild(i);

                if (trnParaAss.Type != Mpal.Parser.Parser.ASSIGN)
                {
                    OutputMessage(Helper.GetPosString(trnParaAss) + String.Format(Messages.C1031, trn.GetChild(0).Text));
                    error = true;
                    continue;
                }
                ITree trnParamName = trnParaAss.GetChild(0);

                if (trnParamName.Text != "G" &&
                    trnParamName.Text != "IN0" &&
                    trnParamName.Text != "IN1")
                {
                    error = true;
                    OutputMessage(Helper.GetPosString(trnParaAss.GetChild(0)) + String.Format(Messages.C1037, trnParaAss.GetChild(0).Text, trn.GetChild(0).Text));
                    continue;
                }

                paraAssignMap.Add(trnParamName.Text, trnParaAss.GetChild(1));
            }

            if (error)
                return null;

            if (paraAssignMap.Count != 3)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1030, trn.GetChild(0).Text));
                return null;
            }

            //Create the temporary result operand
            Operand resultOp = new Operand(_codeOutput.CreateTempOpID(), Helper.GetUID(trn), Operand.Type.Temporary, 0, "");
            Operand sizeOp = new Operand(0, 0, 4, "UDINT");
            _codeOutput.EmitInstruction(InstructionCode.PushTempOp, Helper.GetUID(trn), resultOp, sizeOp, null);

            //Eval the param assignment.
            Operand gParamOp = EvalExpression((ITree)paraAssignMap["G"]);
            Operand in0ParamOp = EvalExpression((ITree)paraAssignMap["IN0"]);
            Operand in1ParamOp = EvalExpression((ITree)paraAssignMap["IN1"]);

            if (gParamOp == null || in0ParamOp == null || in1ParamOp == null)
                return null;
            
            if (gParamOp.TypeId != "BOOL")
            {
                OutputMessage(Helper.GetPosString((ITree)paraAssignMap["G"]) + Messages.C1043);
                return null;            
            }

            if (in0ParamOp.OpType == Operand.Type.Immediate)
                Helper.ConvertConst(in0ParamOp, in1ParamOp);

            if (in1ParamOp.OpType == Operand.Type.Immediate)
                Helper.ConvertConst(in1ParamOp, in0ParamOp);

            Operand op1 = in0ParamOp;
            Operand op2 = in1ParamOp;
            bool    op1Created = false;
            bool    op2Created = false;

            if (op1.TypeId != op2.TypeId)
            {
                op1Created  = true;
                op1 = EmitConvertInstruction(in0ParamOp, in1ParamOp, true);
                op2 = in1ParamOp;

                if (op1 == null)
                {
                    op2Created  = true;
                    op1Created  = false;
                    op1 = in0ParamOp;
                    op2 = EmitConvertInstruction(in1ParamOp, in0ParamOp, false);
                }

                if (op2 == null)
                    return null;
            }

            //Back patch result op
            resultOp.Size = op1.Size;
            resultOp.TypeId = op1.TypeId;
            sizeOp.ConstVal = op1.Size;

            _codeOutput.EmitInstruction(InstructionCode.PushRef, Helper.GetUID(trn.GetChild(0)), null, op1, null);
            _codeOutput.EmitInstruction(InstructionCode.PushRef, Helper.GetUID(trn.GetChild(0)), null, op2, null);

            _codeOutput.EmitInstruction(InstructionCode.Sel, Helper.GetUID(trn.GetChild(0)), resultOp, gParamOp, new Operand(resultOp.Size, 0, 4, "UDINT"));

            //Clear the stack.
            if( op2Created)
                _codeOutput.EmitRemoveTempOperandInst(op2);

            if( op1Created)
                _codeOutput.EmitRemoveTempOperandInst(op1);

            _codeOutput.EmitRemoveTempOperandInst(in1ParamOp);
            _codeOutput.EmitRemoveTempOperandInst(in0ParamOp);
            _codeOutput.EmitRemoveTempOperandInst(gParamOp);

            return resultOp;
        }

        private Operand EvalMinMaxFunction(ITree trn, InstructionCategory cat)
        {
            ITree trnParaAss = null;
            bool error = false;

            List<Operand> parameters = new List<Operand>();
            if (trn.ChildCount < 3)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(0)) + String.Format(Messages.C1030, trn.GetChild(0).Text));
                return null;
            }

            for (int i = 1; i < trn.ChildCount; ++i)
            {
                trnParaAss = trn.GetChild(i);

                Operand op;
                
                if (trnParaAss.Type != Mpal.Parser.Parser.ASSIGN)
                    op = EvalExpression(trnParaAss);
                else
                    op = EvalExpression(trnParaAss.GetChild(1));

                if (op == null)
                {
                    error = true;
                    continue;                    
                }

                if( !Helper.IsNumericType(op.TypeId))
                {
                    OutputMessage(Helper.GetPosString(trnParaAss) + Messages.C1044);
                    error = true;
                }

                parameters.Add(op);
            }

            if (error)
                return null;

            //Create the temporary result operand
            Operand resultOp = new Operand(_codeOutput.CreateTempOpID(), Helper.GetUID(trn), Operand.Type.Temporary, 0, "");
            Operand sizeOp = new Operand(0, 0, 4, "UDINT");
            _codeOutput.EmitInstruction(InstructionCode.PushTempOp, Helper.GetUID(trn), resultOp, sizeOp, null);

            //Convert the operands
            Operand gratherOp = Helper.GetGreaterOperandByType(parameters);
            List<Operand> convOps = new List<Operand>();

            for (int i = 0; i < parameters.Count; ++i)
            {
                Operand op = EmitConvertInstruction(parameters[i], gratherOp);
                
                if (op == null)
                    return null;
                
                convOps.Add(op);
            }

            //Back patch the result op.
            resultOp.Size   = gratherOp.Size;
            resultOp.TypeId = gratherOp.TypeId;
            sizeOp.Size     = gratherOp.Size;            

            foreach (Operand op in convOps)
                _codeOutput.EmitInstruction(InstructionCode.PushRef, 0, null, op, null);

            InstructionCode code = Helper.GetInstructionCode(gratherOp.TypeId, cat);
            _codeOutput.EmitInstruction(code, Helper.GetUID(trn.GetChild(0)), resultOp, new Operand(convOps.Count, 0, 4, "UDINT"), null);

            //Clear the Stack
            for(int i = convOps.Count - 1; i > -1; --i)
            {
                Operand op = convOps[i];

                if (!parameters.Contains(op))
                    _codeOutput.EmitRemoveTempOperandInst(op);
            }

            for(int i = parameters.Count - 1; i > -1; --i)
            {
                Operand op = parameters[i];
                _codeOutput.EmitRemoveTempOperandInst(op);
            }

            return resultOp;
        }

        private Operand EvalMuxFunction(ITree trn)
        {
            bool error = false;
            Operand opK = null;

            //Create the result op.
            Operand resultOp = new Operand(_codeOutput.CreateTempOpID(), Helper.GetUID(trn), Operand.Type.Temporary, 0, "");
            Operand sizeOp = new Operand(0, 0, 4, "UDINT");
            _codeOutput.EmitInstruction(InstructionCode.PushTempOp, Helper.GetUID(trn), resultOp, sizeOp, null);

            List<Operand> opParams = new List<Operand>();

            for (int i = 1; i < trn.ChildCount; ++i)
            {
                ITree trnParam = trn.GetChild(i);

                if (trnParam.Type == Mpal.Parser.Parser.ASSIGN)
                {
                    OutputMessage(Helper.GetPosString(trnParam) + String.Format(Messages.C1031, trn.GetChild(0).Text));
                    error = true;
                    continue;
                }

                if (i == 1)
                {
                    opK = EvalExpression(trnParam);
                    
                    if (opK == null)                    
                        error = true;

                    opParams.Add(opK);
                }
                else
                {
                    Operand op = EvalExpression(trnParam);
                    
                    if( op == null)
                        error = true;

                    opParams.Add(op);
                }
            }

            if (error)
                return null;

            if (opK == null)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(0)) + String.Format(Messages.C1037, "K", trn.GetChild(0).Text));
                return null;
            }
            
            if (opParams.Count  < 2)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(0)) + String.Format(Messages.C1030, trn.GetChild(0).Text));
                return null;
            }

            for (int i = 1; i < opParams.Count; ++i)
            {
                if (opParams[i - 1] == opK || opParams[i] == opK)
                    continue;

                if (opParams[i - 1].TypeId != opParams[i].TypeId)
                {
                    OutputMessage(Helper.GetPosString(trn.GetChild(0)) + Messages.C1045);
                    return null;
                }
            }

            if( !Helper.IsIntegerType(opK.TypeId))
            {
                OutputMessage(GetPosInfo(opK) + Messages.C1046);
                return null;
            }
            //Back patch the result op.
            resultOp.Size = opParams[1].Size;
            resultOp.TypeId = opParams[1].TypeId;
            sizeOp.ConstVal = opParams[1].Size;             
            
            for(int i = opParams.Count - 1; i > -1; --i)
            {
                Operand op = opParams[i];
                
                if (op == opK)
                    continue;

                _codeOutput.EmitInstruction(InstructionCode.PushRef, 0, null, op, null);
            }

            _codeOutput.EmitInstruction(InstructionCode.PushRef, 0, null, opK, null);

            InstructionCode code = Helper.GetInstructionCode(opK.TypeId, InstructionCategory.Mux);
            _codeOutput.EmitInstruction(code, Helper.GetUID(trn.GetChild(0)), resultOp, new Operand(opParams[1].Size, 0, 4, "UDINT"), new Operand(opParams.Count, 0, 4, "UDINT"));

            //Clear the stack
            for (int i = opParams.Count - 1; i > -1; --i)
            {
                Operand op = opParams[i];
                _codeOutput.EmitRemoveTempOperandInst(op);
            }

            return resultOp;
        }

        private Operand EvalFunctionStmt(ITree trn)
        {
            switch (trn.GetChild(0).Text)            
            {
                case "MUX":
                    return EvalMuxFunction(trn);

                case "LIMIT":
                    return EvalLimitFunction(trn);

                case "SEL":
                    return EvalSelFunction(trn);

                case "MAX":
                    return EvalMinMaxFunction(trn, InstructionCategory.Max);
                
                case "MIN":
                    return EvalMinMaxFunction(trn, InstructionCategory.Min);

                case "INC":
                    EvalIncDecFunction(trn, InstructionCategory.Inc);
                    return new Operand(0, Helper.GetUID(trn), 0, "VOID");
                
                case "DEC":
                    EvalIncDecFunction(trn, InstructionCategory.Dec);
                    return new Operand(0, Helper.GetUID(trn), 0, "VOID");
                
                case "SHR":
                    return EvalBitStringFunction(trn, InstructionCategory.Shr);

                case "SHL":
                    return EvalBitStringFunction(trn, InstructionCategory.Shl);

                case "ROR":
                    return EvalBitStringFunction(trn, InstructionCategory.Ror);

                case "ROL":
                    return EvalBitStringFunction(trn, InstructionCategory.Rol);

                case "TAN":
                    return EvalMathFunction(trn, InstructionCategory.Tan);
    
                case "SIN":
                    return EvalMathFunction(trn, InstructionCategory.Sin);
    
                case "COS":
                    return EvalMathFunction(trn, InstructionCategory.Cos);

                case "ATAN":
                    return EvalMathFunction(trn, InstructionCategory.Atan);
    
                case "ASIN":
                    return EvalMathFunction(trn, InstructionCategory.Asin);

                case "ACOS":
                    return EvalMathFunction(trn, InstructionCategory.Acos);

                case "LOG":
                    return EvalMathFunction(trn, InstructionCategory.Log);

                case "LN":
                    return EvalMathFunction(trn, InstructionCategory.Ln);

                case "EXP":
                    return EvalMathFunction(trn, InstructionCategory.Exp);

                case "EXPD":
                    return EvalMathFunction(trn, InstructionCategory.Expd);

                case "SQRT":
                    return EvalMathFunction(trn, InstructionCategory.Sqrt);
                
                case "SQR":
                    return EvalMathFunction(trn, InstructionCategory.Sqr);

                case "ABS":
                    return EvalMathFunction(trn,InstructionCategory.Abs);

                case "ROUND":
                    return EvalRoundTruncFunction(trn,InstructionCode.Round);
                
                case "TRUNC":
                    return EvalRoundTruncFunction(trn, InstructionCode.Trunc);
                
                case "BOOL_TO_SINT":
                    return EvalConvertFunction(trn, "BOOL", "SINT");

                case "BOOL_TO_INT":
                    return EvalConvertFunction(trn, "BOOL", "INT");

                case "BOOL_TO_DINT":
                    return EvalConvertFunction(trn, "BOOL", "DINT");
                
                case "BOOL_TO_LINT":
                    return EvalConvertFunction(trn, "BOOL", "LINT");

                case "BOOL_TO_USINT":
                    return EvalConvertFunction(trn, "BOOL", "USINT");

                case "BOOL_TO_UINT":
                    return EvalConvertFunction(trn, "BOOL", "UINT");

                case "BOOL_TO_UDINT":
                    return EvalConvertFunction(trn, "BOOL", "UDINT");

                case "BOOL_TO_ULINT":
                    return EvalConvertFunction(trn, "BOOL", "ULINT");
                
                case "BOOL_TO_REAL":
                    return EvalConvertFunction(trn, "BOOL", "REAL");
                
                case "BOOL_TO_LREAL":
                    return EvalConvertFunction(trn, "BOOL", "LREAL");
                
                case "BOOL_TO_BYTE":
                    return EvalConvertFunction(trn, "BOOL", "BYTE");
                
                case "BOOL_TO_WORD":
                    return EvalConvertFunction(trn, "BOOL", "WORD");
                
                case "BOOL_TO_DWORD":
                    return EvalConvertFunction(trn, "BOOL", "DWORD");
                
                case "BOOL_TO_LWORD":
                    return EvalConvertFunction(trn, "BOOL", "LWORD");
                
                case "BOOL_TO_STRING":
                    return EvalConvertFunction(trn, "BOOL", "STRING");
                
                case "BOOL_TO_WSTRING":
                    return EvalConvertFunction(trn, "BOOL", "WSTRING");

                case "SINT_TO_BOOL":
                    return EvalConvertFunction(trn, "SINT", "BOOL");

                case "SINT_TO_INT":
                    return EvalConvertFunction(trn, "SINT", "BOOL");

                case "SINT_TO_DINT":
                    return EvalConvertFunction(trn, "SINT", "DINT");

                case "SINT_TO_LINT":
                    return EvalConvertFunction(trn, "SINT", "LINT");

                case "SINT_TO_USINT":
                    return EvalConvertFunction(trn, "SINT", "USINT");

                case "SINT_TO_UINT":
                    return EvalConvertFunction(trn, "SINT", "UINT");

                case "SINT_TO_UDINT":
                    return EvalConvertFunction(trn, "SINT", "UDINT");

                case "SINT_TO_ULINT":
                    return EvalConvertFunction(trn, "SINT", "ULINT");

                case "SINT_TO_REAL":
                    return EvalConvertFunction(trn, "SINT", "REAL");

                case "SINT_TO_LREAL":
                    return EvalConvertFunction(trn, "SINT", "LREAL");

                case "SINT_TO_STRING":
                    return EvalConvertFunction(trn, "SINT", "STRING");
                
                case "SINT_TO_BYTE":
                    return EvalConvertFunction(trn, "SINT", "BYTE");

                case "SINT_TO_WORD":
                    return EvalConvertFunction(trn, "SINT", "WORD");
                
                case "SINT_TO_DWORD":
                    return EvalConvertFunction(trn, "SINT", "DWORD");
                
                case "SINT_TO_LWORD":
                    return EvalConvertFunction(trn, "SINT", "LWORD");

                case "SINT_TO_WSTRING":
                    return EvalConvertFunction(trn, "SINT", "WSTRING");

                case "INT_TO_BOOL":
                    return EvalConvertFunction(trn, "INT", "BOOL");

                case "INT_TO_SINT":
                    return EvalConvertFunction(trn, "INT", "SINT");

                case "INT_TO_DINT":
                    return EvalConvertFunction(trn, "INT", "DINT");

                case "INT_TO_LINT":
                    return EvalConvertFunction(trn, "INT", "LINT");

                case "INT_TO_USINT":
                    return EvalConvertFunction(trn, "INT", "USINT");

                case "INT_TO_UINT":
                    return EvalConvertFunction(trn, "INT", "UINT");

                case "INT_TO_UDINT":
                    return EvalConvertFunction(trn, "INT", "UDINT");

                case "INT_TO_ULINT":
                    return EvalConvertFunction(trn, "INT", "ULINT");

                case "INT_TO_REAL":
                    return EvalConvertFunction(trn, "INT", "REAL");

                case "INT_TO_LREAL":
                    return EvalConvertFunction(trn, "INT", "LREAL");

                case "INT_TO_STRING":
                    return EvalConvertFunction(trn, "INT", "STRING");

                case "INT_TO_BYTE":
                    return EvalConvertFunction(trn, "INT", "BYTE");

                case "INT_TO_WORD":
                    return EvalConvertFunction(trn, "INT", "WORD");

                case "INT_TO_DWORD":
                    return EvalConvertFunction(trn, "INT", "DWORD");

                case "INT_TO_LWORD":
                    return EvalConvertFunction(trn, "INT", "LWORD");

                case "INT_TO_WSTRING":
                    return EvalConvertFunction(trn, "INT", "WSTRING");

                case "DINT_TO_BOOL":
                    return EvalConvertFunction(trn, "DINT", "BOOL");

                case "DINT_TO_LINT":
                    return EvalConvertFunction(trn, "DINT", "LINT");

                case "DINT_TO_USINT":
                    return EvalConvertFunction(trn, "DINT", "USINT");

                case "DINT_TO_UINT":
                    return EvalConvertFunction(trn, "DINT", "UINT");

                case "DINT_TO_UDINT":
                    return EvalConvertFunction(trn, "DINT", "UDINT");

                case "DINT_TO_ULINT":
                    return EvalConvertFunction(trn, "DINT", "ULINT");

                case "DINT_TO_REAL":
                    return EvalConvertFunction(trn, "DINT", "REAL");

                case "DINT_TO_LREAL":
                    return EvalConvertFunction(trn, "DINT", "LREAL");

                case "DINT_TO_BYTE":
                    return EvalConvertFunction(trn, "DINT", "BYTE");

                case "DINT_TO_WORD":
                    return EvalConvertFunction(trn, "DINT", "WORD");

                case "DINT_TO_DWORD":
                    return EvalConvertFunction(trn, "DINT", "DWORD");

                case "DINT_TO_LWORD":
                    return EvalConvertFunction(trn, "DINT", "LWORD");

                case "DINT_TO_WSTRING":
                    return EvalConvertFunction(trn, "DINT", "WSTRING");

                case "DINT_TO_SINT":
                    return EvalConvertFunction(trn, "DINT", "SINT");

                case "DINT_TO_INT":
                    return EvalConvertFunction(trn, "DINT", "INT");

                case "DINT_TO_STRING":
                    return EvalConvertFunction(trn, "DINT", "STRING");

                case "LINT_TO_BOOL":
                    return EvalConvertFunction(trn, "LINT", "BOOL");

                case "LINT_TO_SINT":
                    return EvalConvertFunction(trn, "LINT", "SINT");

                case "LINT_TO_INT":
                    return EvalConvertFunction(trn, "LINT", "INT");

                case "LINT_TO_DINT":
                    return EvalConvertFunction(trn, "LINT", "DINT");

                case "LINT_TO_USINT":
                    return EvalConvertFunction(trn, "LINT", "USINT");

                case "LINT_TO_UINT":
                    return EvalConvertFunction(trn, "LINT", "UINT");

                case "LINT_TO_ULINT":
                    return EvalConvertFunction(trn, "LINT", "ULINT");

                case "LINT_TO_REAL":
                    return EvalConvertFunction(trn, "LINT", "REAL");

                case "LINT_TO_LREAL":
                    return EvalConvertFunction(trn, "LINT", "LREAL");

                case "LINT_TO_STRING":
                    return EvalConvertFunction(trn, "LINT", "STRING");

                case "LINT_TO_BYTE":
                    return EvalConvertFunction(trn, "LINT", "BYTE");

                case "LINT_TO_WORD":
                    return EvalConvertFunction(trn, "LINT", "WORD");

                case "LINT_TO_DWORD":
                    return EvalConvertFunction(trn, "LINT", "DWORD");

                case "LINT_TO_LWORD":
                    return EvalConvertFunction(trn, "LINT", "LWORD");

                case "LINT_TO_WSTRING":
                    return EvalConvertFunction(trn, "LINT", "WSTRING");

                case "LINT_TO_UDINT":
                    return EvalConvertFunction(trn, "LINT", "UDINT");

                case "USINT_TO_BOOL":
                    return EvalConvertFunction(trn, "USINT", "BOOL");

                case "USINT_TO_SINT":
                    return EvalConvertFunction(trn, "USINT", "SINT");

                case "USINT_TO_INT":
                    return EvalConvertFunction(trn, "USINT", "INT");
                
                case "USINT_TO_DINT":
                    return EvalConvertFunction(trn, "USINT", "DINT");
                
                case "USINT_TO_LINT":
                    return EvalConvertFunction(trn, "USINT", "LINT");
                
                case "USINT_TO_UINT":
                    return EvalConvertFunction(trn, "USINT", "UINT");
                
                case "USINT_TO_UDINT":
                    return EvalConvertFunction(trn, "USINT", "UDINT");
                
                case "USINT_TO_ULINT":
                    return EvalConvertFunction(trn, "USINT", "ULINT");
                
                case "USINT_TO_REAL":
                    return EvalConvertFunction(trn, "USINT", "REAL");
                
                case "USINT_TO_LREAL":
                    return EvalConvertFunction(trn, "USINT", "LREAL");
                
                case "USINT_TO_STRING":
                    return EvalConvertFunction(trn, "USINT", "STRING");
                
                case "USINT_TO_BYTE":
                    return EvalConvertFunction(trn, "USINT", "BYTE");
                
                case "USINT_TO_WORD":
                    return EvalConvertFunction(trn, "USINT", "WORD");
                
                case "USINT_TO_DWORD":
                    return EvalConvertFunction(trn, "USINT", "DWORD");
                
                case "USINT_TO_LWORD":
                    return EvalConvertFunction(trn, "USINT", "LWORD");
                
                case "USINT_TO_WSTRING":
                    return EvalConvertFunction(trn, "USINT", "WSTRING");
                
                case "UINT_TO_BOOL":
                    return EvalConvertFunction(trn, "UINT", "BOOL");
                
                case "UINT_TO_SINT":
                    return EvalConvertFunction(trn, "UINT", "SINT");
                
                case "UINT_TO_INT":
                    return EvalConvertFunction(trn, "UINT", "INT");
                
                case "UINT_TO_DINT":
                    return EvalConvertFunction(trn, "UINT", "DINT");
                
                case "UINT_TO_LINT":
                    return EvalConvertFunction(trn, "UINT", "LINT");
                
                case "UINT_TO_USINT":
                    return EvalConvertFunction(trn, "UINT", "USINT");
                
                case "UINT_TO_UDINT":
                    return EvalConvertFunction(trn, "UINT", "UDINT");
                
                case "UINT_TO_ULINT":
                    return EvalConvertFunction(trn, "UINT", "ULINT");
                
                case "UINT_TO_REAL":
                    return EvalConvertFunction(trn, "UINT", "REAL");
                
                case "UINT_TO_LREAL":
                    return EvalConvertFunction(trn, "UINT", "LREAL");
                
                case "UINT_TO_STRING":
                    return EvalConvertFunction(trn, "UINT", "STRING");
                
                case "UINT_TO_BYTE":
                    return EvalConvertFunction(trn, "UINT", "BYTE");
                
                case "UINT_TO_WORD":
                    return EvalConvertFunction(trn, "UINT", "WORD");
                
                case "UINT_TO_DWORD":
                    return EvalConvertFunction(trn, "UINT", "DWORD");
                
                case "UINT_TO_LWORD":
                    return EvalConvertFunction(trn, "UINT", "LWORD");

                case "UINT_TO_WSTRING":
                    return EvalConvertFunction(trn, "UINT", "WSTRING");

                case "UDINT_TO_BOOL":
                    return EvalConvertFunction(trn, "UDINT", "BOOL");
                
                case "UDINT_TO_SINT":
                    return EvalConvertFunction(trn, "UDINT", "SINT");
                
                case "UDINT_TO_INT":
                    return EvalConvertFunction(trn, "UDINT", "INT");
                
                case "UDINT_TO_DINT":
                    return EvalConvertFunction(trn, "UDINT", "DINT");
                
                case "UDINT_TO_LINT":
                    return EvalConvertFunction(trn, "UDINT", "LINT");
                
                case "UDINT_TO_USINT":
                    return EvalConvertFunction(trn, "UDINT", "USINT");
                
                case "UDINT_TO_UINT":
                    return EvalConvertFunction(trn, "UDINT", "UINT");
                
                case "UDINT_TO_ULINT":
                    return EvalConvertFunction(trn, "UDINT", "ULINT");
                
                case "UDINT_TO_REAL":
                    return EvalConvertFunction(trn, "UDINT", "REAL");
                
                case "UDINT_TO_LREAL":
                    return EvalConvertFunction(trn, "UDINT", "LREAL");
                
                case "UDINT_TO_STRING":
                    return EvalConvertFunction(trn, "UDINT", "STRING");
                
                case "UDINT_TO_BYTE":
                    return EvalConvertFunction(trn, "UDINT", "BYTE");
                
                case "UDINT_TO_WORD":
                    return EvalConvertFunction(trn, "UDINT", "WORD");
                
                case "UDINT_TO_DWORD":
                    return EvalConvertFunction(trn, "UDINT", "DWORD");
                
                case "UDINT_TO_LWORD":
                    return EvalConvertFunction(trn, "UDINT", "LWORD");
                
                case "UDINT_TO_WSTRING":
                    return EvalConvertFunction(trn, "UDINT", "WSTRING");

                case "ULINT_TO_BOOL":
                    return EvalConvertFunction(trn, "ULINT", "BOOL");
                
                case "ULINT_TO_SINT":
                    return EvalConvertFunction(trn, "ULINT", "SINT");
                
                case "ULINT_TO_INT":
                    return EvalConvertFunction(trn, "ULINT", "INT");
                
                case "ULINT_TO_DINT":
                    return EvalConvertFunction(trn, "ULINT", "DINT");
                
                case "ULINT_TO_LINT":
                    return EvalConvertFunction(trn, "ULINT", "LINT");
                
                case "ULINT_TO_USINT":
                    return EvalConvertFunction(trn, "ULINT", "USINT");
                
                case "ULINT_TO_UINT":
                    return EvalConvertFunction(trn, "ULINT", "UINT");
                
                case "ULINT_TO_UDINT":
                    return EvalConvertFunction(trn, "ULINT", "UDINT");
                
                case "ULINT_TO_REAL":
                    return EvalConvertFunction(trn, "ULINT", "REAL");
                
                case "ULINT_TO_LREAL":
                    return EvalConvertFunction(trn, "ULINT", "LREAL");
                
                case "ULINT_TO_STRING":
                    return EvalConvertFunction(trn, "ULINT", "STRING");
                
                case "ULINT_TO_BYTE":
                    return EvalConvertFunction(trn, "ULINT", "BYTE");
                
                case "ULINT_TO_WORD":
                    return EvalConvertFunction(trn, "ULINT", "WORD");
                
                case "ULINT_TO_DWORD":
                    return EvalConvertFunction(trn, "ULINT", "DWORD");
                
                case "ULINT_TO_LWORD":
                    return EvalConvertFunction(trn, "ULINT", "LWORD");
                
                case "ULINT_TO_WSTRING":
                    return EvalConvertFunction(trn, "ULINT", "WSTRING");
                    
                case "REAL_TO_BOOL":
                    return EvalConvertFunction(trn, "REAL", "BOOL");
                
                case "REAL_TO_SINT":
                    return EvalConvertFunction(trn, "REAL", "SINT");
                
                case "REAL_TO_INT":
                    return EvalConvertFunction(trn, "REAL", "INT");
                
                case "REAL_TO_DINT":
                    return EvalConvertFunction(trn, "REAL", "DINT");
                
                case "REAL_TO_LINT":
                    return EvalConvertFunction(trn, "REAL", "LINT");
                
                case "REAL_TO_USINT":
                    return EvalConvertFunction(trn, "REAL", "USINT");
                
                case "REAL_TO_UINT":
                    return EvalConvertFunction(trn, "REAL", "UINT");
                
                case "REAL_TO_UDINT":
                    return EvalConvertFunction(trn, "REAL", "UDINT");
                
                case "REAL_TO_ULINT":
                    return EvalConvertFunction(trn, "REAL", "ULINT");
                
                case "REAL_TO_LREAL":
                    return EvalConvertFunction(trn, "REAL", "LREAL");
                
                case "REAL_TO_STRING":
                    return EvalConvertFunction(trn, "REAL", "STRING");
                
                case "REAL_TO_BYTE":
                    return EvalConvertFunction(trn, "REAL", "BYTE");
                
                case "REAL_TO_WORD":
                    return EvalConvertFunction(trn, "REAL", "WORD");
                
                case "REAL_TO_DWORD":
                    return EvalConvertFunction(trn, "REAL", "DWORD");
                
                case "REAL_TO_LWORD":
                    return EvalConvertFunction(trn, "REAL", "LWORD");
                
                case "REAL_TO_WSTRING":
                    return EvalConvertFunction(trn, "REAL", "WSTRING");

                case "LREAL_TO_BOOL":
                    return EvalConvertFunction(trn, "LREAL", "BOOL");

                case "LREAL_TO_SINT":
                    return EvalConvertFunction(trn, "LREAL", "SINT");

                case "LREAL_TO_INT":
                    return EvalConvertFunction(trn, "LREAL", "INT");

                case "LREAL_TO_DINT":
                    return EvalConvertFunction(trn, "LREAL", "DINT");

                case "LREAL_TO_LINT":
                    return EvalConvertFunction(trn, "LREAL", "LINT");

                case "LREAL_TO_USINT":
                    return EvalConvertFunction(trn, "LREAL", "USINT");

                case "LREAL_TO_UINT":
                    return EvalConvertFunction(trn, "LREAL", "UINT");

                case "LREAL_TO_UDINT":
                    return EvalConvertFunction(trn, "LREAL", "UDINT");

                case "LREAL_TO_ULINT":
                    return EvalConvertFunction(trn, "LREAL", "ULINT");

                case "LREAL_TO_REAL":
                    return EvalConvertFunction(trn, "LREAL", "REAL");

                case "LREAL_TO_STRING":
                    return EvalConvertFunction(trn, "LREAL", "STRING");

                case "LREAL_TO_BYTE":
                    return EvalConvertFunction(trn, "LREAL", "BYTE");

                case "LREAL_TO_WORD":
                    return EvalConvertFunction(trn, "LREAL", "WORD");

                case "LREAL_TO_DWORD":
                    return EvalConvertFunction(trn, "LREAL", "DWORD");

                case "LREAL_TO_LWORD":
                    return EvalConvertFunction(trn, "LREAL", "LWORD");

                case "LREAL_TO_WSTRING":
                    return EvalConvertFunction(trn, "LREAL", "WSTRING");
                                    
                case "STRING_TO_BOOL":
                    return EvalConvertFunction(trn, "STRING", "BOOL");
                
                case "STRING_TO_SINT":
                    return EvalConvertFunction(trn, "STRING", "SINT");
                
                case "STRING_TO_INT":
                    return EvalConvertFunction(trn, "STRING", "INT");
                
                case "STRING_TO_DINT":
                    return EvalConvertFunction(trn, "STRING", "DINT");
                
                case "STRING_TO_LINT":
                    return EvalConvertFunction(trn, "STRING", "LINT");
                
                case "STRING_TO_USINT":
                    return EvalConvertFunction(trn, "STRING", "USINT");
                
                case "STRING_TO_UINT":
                    return EvalConvertFunction(trn, "STRING", "UINT");
                
                case "STRING_TO_UDINT":
                    return EvalConvertFunction(trn, "STRING", "UDINT");
                
                case "STRING_TO_ULINT":
                    return EvalConvertFunction(trn, "STRING", "ULINT");
                
                case "STRING_TO_REAL":
                    return EvalConvertFunction(trn, "STRING", "REAL");
                
                case "STRING_TO_LREAL":
                    return EvalConvertFunction(trn, "STRING", "LREAL");
                
                case "STRING_TO_BYTE":
                    return EvalConvertFunction(trn, "STRING", "BYTE");
                
                case "STRING_TO_WORD":
                    return EvalConvertFunction(trn, "STRING", "WORD");
                
                case "STRING_TO_DWORD":
                    return EvalConvertFunction(trn, "STRING", "DWORD");
                
                case "STRING_TO_LWORD":
                    return EvalConvertFunction(trn, "STRING", "LWORD");
                
                case "STRING_TO_WSTRING":
                    return EvalConvertFunction(trn, "STRING", "WSTRING");

                case "BYTE_TO_BOOL":
                    return EvalConvertFunction(trn, "BYTE", "BOOL");
                
                case "BYTE_TO_SINT":
                    return EvalConvertFunction(trn, "BYTE", "SINT");
                
                case "BYTE_TO_INT":
                    return EvalConvertFunction(trn, "BYTE", "INT");
                
                case "BYTE_TO_DINT":
                    return EvalConvertFunction(trn, "BYTE", "DINT");
                
                case "BYTE_TO_LINT":
                    return EvalConvertFunction(trn, "BYTE", "LINT");
                
                case "BYTE_TO_USINT":
                    return EvalConvertFunction(trn, "BYTE", "USINT");
                
                case "BYTE_TO_UINT":
                    return EvalConvertFunction(trn, "BYTE", "UINT");
                
                case "BYTE_TO_UDINT":
                    return EvalConvertFunction(trn, "BYTE", "UDINT");
                
                case "BYTE_TO_ULINT":
                    return EvalConvertFunction(trn, "BYTE", "ULINT");
                
                case "BYTE_TO_REAL":
                    return EvalConvertFunction(trn, "BYTE", "REAL");
                
                case "BYTE_TO_LREAL":
                    return EvalConvertFunction(trn, "BYTE", "LREAL");
                
                case "BYTE_TO_STRING":
                    return EvalConvertFunction(trn, "BYTE", "STRING");
                
                case "BYTE_TO_WORD":
                    return EvalConvertFunction(trn, "BYTE", "WORD");
                
                case "BYTE_TO_DWORD":
                    return EvalConvertFunction(trn, "BYTE", "DWORD");
                
                case "BYTE_TO_LWORD":
                    return EvalConvertFunction(trn, "BYTE", "LWORD");
                
                case "BYTE_TO_WSTRING":
                    return EvalConvertFunction(trn, "BYTE", "WSTRING");

                case "WORD_TO_BOOL":
                    return EvalConvertFunction(trn, "WORD", "BOOL");

                case "WORD_TO_SINT":
                    return EvalConvertFunction(trn, "WORD", "SINT");

                case "WORD_TO_INT":
                    return EvalConvertFunction(trn, "WORD", "INT");

                case "WORD_TO_DINT":
                    return EvalConvertFunction(trn, "WORD", "DINT");

                case "WORD_TO_LINT":
                    return EvalConvertFunction(trn, "WORD", "LINT");

                case "WORD_TO_USINT":
                    return EvalConvertFunction(trn, "WORD", "USINT");

                case "WORD_TO_UINT":
                    return EvalConvertFunction(trn, "WORD", "UINT");

                case "WORD_TO_UDINT":
                    return EvalConvertFunction(trn, "WORD", "UDINT");

                case "WORD_TO_ULINT":
                    return EvalConvertFunction(trn, "WORD", "ULINT");

                case "WORD_TO_REAL":
                    return EvalConvertFunction(trn, "WORD", "REAL");

                case "WORD_TO_LREAL":
                    return EvalConvertFunction(trn, "WORD", "LREAL");

                case "WORD_TO_STRING":
                    return EvalConvertFunction(trn, "WORD", "STRING");

                case "WORD_TO_BYTE":
                    return EvalConvertFunction(trn, "WORD", "BYTE");

                case "WORD_TO_DWORD":
                    return EvalConvertFunction(trn, "WORD", "DWORD");

                case "WORD_TO_LWORD":
                    return EvalConvertFunction(trn, "WORD", "LWORD");

                case "WORD_TO_WSTRING":
                    return EvalConvertFunction(trn, "WORD", "WSTRING");

                case "DWORD_TO_BOOL":
                    return EvalConvertFunction(trn, "DWORD", "BOOL");

                case "DWORD_TO_SINT":
                    return EvalConvertFunction(trn, "DWORD", "SINT");

                case "DWORD_TO_INT":
                    return EvalConvertFunction(trn, "DWORD", "INT");

                case "DWORD_TO_DINT":
                    return EvalConvertFunction(trn, "DWORD", "DINT");

                case "DWORD_TO_LINT":
                    return EvalConvertFunction(trn, "DWORD", "LINT");

                case "DWORD_TO_USINT":
                    return EvalConvertFunction(trn, "DWORD", "USINT");

                case "DWORD_TO_UINT":
                    return EvalConvertFunction(trn, "DWORD", "UINT");

                case "DWORD_TO_UDINT":
                    return EvalConvertFunction(trn, "DWORD", "UDINT");

                case "DWORD_TO_ULINT":
                    return EvalConvertFunction(trn, "DWORD", "ULINT");

                case "DWORD_TO_REAL":
                    return EvalConvertFunction(trn, "DWORD", "REAL");

                case "DWORD_TO_LREAL":
                    return EvalConvertFunction(trn, "DWORD", "LREAL");

                case "DWORD_TO_STRING":
                    return EvalConvertFunction(trn, "DWORD", "STRING");

                case "DWORD_TO_BYTE":
                    return EvalConvertFunction(trn, "DWORD", "BYTE");

                case "DWORD_TO_WORD":
                    return EvalConvertFunction(trn, "DWORD", "WORD");

                case "DWORD_TO_LWORD":
                    return EvalConvertFunction(trn, "DWORD", "LWORD");

                case "DWORD_TO_WSTRING":
                    return EvalConvertFunction(trn, "DWORD", "WSTRING");

                case "LWORD_TO_BOOL":
                    return EvalConvertFunction(trn, "LWORD", "BOOL");

                case "LWORD_TO_SINT":
                    return EvalConvertFunction(trn, "LWORD", "SINT");

                case "LWORD_TO_INT":
                    return EvalConvertFunction(trn, "LWORD", "INT");

                case "LWORD_TO_DINT":
                    return EvalConvertFunction(trn, "LWORD", "DINT");

                case "LWORD_TO_LINT":
                    return EvalConvertFunction(trn, "LWORD", "LINT");

                case "LWORD_TO_USINT":
                    return EvalConvertFunction(trn, "LWORD", "USINT");

                case "LWORD_TO_UINT":
                    return EvalConvertFunction(trn, "LWORD", "UINT");

                case "LWORD_TO_UDINT":
                    return EvalConvertFunction(trn, "LWORD", "UDINT");

                case "LWORD_TO_ULINT":
                    return EvalConvertFunction(trn, "LWORD", "ULINT");

                case "LWORD_TO_REAL":
                    return EvalConvertFunction(trn, "LWORD", "REAL");

                case "LWORD_TO_LREAL":
                    return EvalConvertFunction(trn, "LWORD", "LREAL");

                case "LWORD_TO_STRING":
                    return EvalConvertFunction(trn, "LWORD", "STRING");

                case "LWORD_TO_BYTE":
                    return EvalConvertFunction(trn, "LWORD", "BYTE");

                case "LWORD_TO_WORD":
                    return EvalConvertFunction(trn, "LWORD", "WORD");

                case "LWORD_TO_DWORD":
                    return EvalConvertFunction(trn, "LWORD", "DWORD");

                case "LWORD_TO_WSTRING":
                    return EvalConvertFunction(trn, "LWORD", "WSTRING");

                default:
                    return EvalCall(trn);
            }
        }

        private Operand EvalCall(ITree trn)
        {
            ITree trnFuncName = trn.GetChild(0);
            Function func = null;
            Operand obj = null;

            func = SearchFunction(trnFuncName, _unit, out obj);

            if (func == null)
            {
                OutputMessage(Helper.GetPosString(trnFuncName) + String.Format(Messages.C1002, trnFuncName.Text));
                return null;
            }

            switch (func.FuncType)
            {
                case Function.Type.FC:
                    return EvalUserDefinedFC(trn, func, _unit);

                case Function.Type.FB:
                    EvalUserDefinedFB(trn, func, _unit, obj);
                    return new Operand(0, Helper.GetUID(trn), 0, "VOID");

                default:
                    throw new Exception("Not implemented");
            }
        }
        
        private void EvalUserDefinedFB(ITree trn, Function func, Unit unit, Operand obj)
        {
            if (obj == null)
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(0)) + String.Format(Messages.C1048, trn.GetChild(0).Text));
                return;
            }

            List<Operand> inOps = new List<Operand>();
            List<DictionaryEntry> outOps = new List<DictionaryEntry>();
            Hashtable paramOps = new Hashtable();

            //Create the copy in instructions.
            if (!CopyFBParamIntoTheInstance(trn, func, obj, inOps, outOps, paramOps))
                return;
            
            //Create the call stack.
            CreateFBStack(func, paramOps, trn, obj);

            //Execute the call.
            Instruction callInst = new Instruction(InstructionCode.Call, Helper.GetUID(trn.GetChild(0)), null, null, null);
            callInst.CallFunction = func.Name;
            _function.Instructions.Add(callInst);

            //Create the copy out instructions.
            foreach (DictionaryEntry entry in outOps)
            {
                Operand op = (Operand)entry.Key;
                Parameter param = (Parameter)entry.Value;

                Operand src = (Operand) paramOps[param.Name];
                Operand convSrcOp = EmitConvertInstruction(src, op);
                
                if (convSrcOp == null)
                    continue;

                _codeOutput.EmitInstruction(InstructionCode.Move, Helper.GetUID(trn), op, src, new Operand(param.Size, 0, 4, "UDINT"));

                if (src != convSrcOp)
                    _codeOutput.EmitRemoveTempOperandInst(convSrcOp);
            }
                                    
            //Clear the stack 
            for (int i = inOps.Count - 1; i > -1; --i)
            {
                inOps[i].Uid = Helper.GetUID(trn.GetChild(0));
                _codeOutput.EmitRemoveTempOperandInst(inOps[i]);
            }

            foreach (DictionaryEntry entry in paramOps)
            {
                Operand op = (Operand)entry.Value;
                op.Uid = Helper.GetUID(trn.GetChild(0));
                _codeOutput.EmitRemoveTempOperandInst(op);
            }
        }

        private void CreateFBStack(Function func, Hashtable operands, ITree trn, Operand obj)
        {
            FCCodeOutput fcCodeOutput = new FCCodeOutput(_unit, func, _codeOutput);

            //Push the object reference of the stack
            _codeOutput.EmitInstruction(InstructionCode.PushRef, Helper.GetUID(trn), null, obj, null);

            //Create the stack and init it with default values.
            foreach (Parameter param in func.Parameters)
            {
                if (func.IsStackParam(param))
                {
                    Operand op = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trn), param.Size, param.TypeId);
                    fcCodeOutput.EmitCopyFuncParamDefValue(param, op, trn);
                }
            }
        }

        private bool CopyFBParamIntoTheInstance(ITree trn, Function func, Operand obj, List<Operand> ops, List<DictionaryEntry> outOps, Hashtable paramOps)
        {
            bool error = false;
            ITree trnFuncName = trn.GetChild(0);
            //Copy the function block parameter into the instace.
            for (int i = 1; i < trn.ChildCount; ++i)
            {
                ITree trnParamAss = trn.GetChild(i);
                if (trnParamAss.ChildCount == 0)
                {
                    OutputMessage(Helper.GetPosString(trnParamAss) + String.Format(Messages.C1038, trnParamAss.Text, trnFuncName.Text));
                    error = true;
                    continue;
                }

                Parameter param = func.GetParameter(trnParamAss.GetChild(0).Text);

                if (param.ParamAccess == Parameter.Access.Output)
                {
                    if(trnParamAss.Type != Mpal.Parser.Parser.ASSIGN2)
                    {
                        OutputMessage(Helper.GetPosString(trnParamAss) + String.Format(Messages.C1038, trnParamAss.Text, trnFuncName.Text));
                        error = true;
                        continue;
                    }
                }
                else
                {
                    if (trnParamAss.Type != Mpal.Parser.Parser.ASSIGN)
                    {
                        OutputMessage(Helper.GetPosString(trnParamAss) + String.Format(Messages.C1038, trnParamAss.Text, trnFuncName.Text));
                        error = true;
                        continue;
                    }
                }

                if (!CheckFuncParamExistance(func, trnParamAss.GetChild(0)))
                {
                    error = true;
                    continue;
                }

                Operand op = EvalExpression(trnParamAss.GetChild(1));
                if (op == null)
                {
                    error = true;
                    continue;
                }

                if (param == null)
                {
                    OutputMessage(Helper.GetPosString(trnParamAss.GetChild(0)) + String.Format(Messages.C1037, trnParamAss.GetChild(0).Text, func.Name));
                    error = true;
                    continue;
                }

                if (param.ParamAccess != Parameter.Access.Input &&
                    param.ParamAccess != Parameter.Access.Output &&
                    param.ParamAccess != Parameter.Access.InOut)
                {
                    OutputMessage(Helper.GetPosString(trnParamAss.GetChild(0)) + String.Format(Messages.C1049, param.Name, func.Name));
                    error = true;
                    continue;
                }

                ops.Add(op);                

                if (param.ParamAccess == Parameter.Access.Input ||
                    param.ParamAccess == Parameter.Access.InOut)
                {
                    Operand convOp = EmitConvertInstruction(op, new Operand(0, Helper.GetUID(trnParamAss), Operand.Type.Direct, param.Size, param.TypeId));

                    if (convOp == null)
                    {
                        error = true;
                        continue;
                    }

                    if (convOp != op)
                        ops.Add(convOp);

                    Operand instOp = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trnParamAss.GetChild(0)), param.Size, param.TypeId);
                    Operand offsetOp = new Operand(param.Offset, param.UID, 4, "UDINT");
                    _codeOutput.EmitInstruction(InstructionCode.AddOffsetUDINT, Helper.GetUID(trnParamAss.GetChild(0)), instOp, obj, offsetOp);
                    paramOps.Add(param.Name, instOp);
                    _codeOutput.EmitInstruction(InstructionCode.Move, Helper.GetUID(trnParamAss), instOp, convOp, new Operand(convOp.Size, 0, 4, "UDINT"));

                    if(param.ParamAccess == Parameter.Access.InOut)
                        outOps.Add(new DictionaryEntry(op, param));
                }
                
                if(param.ParamAccess == Parameter.Access.Output)
                {
                    if (op.OpType == Operand.Type.Immediate || op.IsConstant)
                    {
                        error = true;
                        OutputMessage(Helper.GetPosString(trnParamAss.GetChild(0)) + String.Format(Messages.C1041, param.Name, func.Name));
                        continue;
                    }

                    outOps.Add(new DictionaryEntry(op, param));
                    Operand instOp = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trnParamAss.GetChild(0)), param.Size, param.TypeId);
                    Operand offsetOp = new Operand(param.Offset, param.UID, 4, "UDINT");
                    _codeOutput.EmitInstruction(InstructionCode.AddOffsetUDINT, Helper.GetUID(trnParamAss.GetChild(0)), instOp, obj, offsetOp);
                    paramOps.Add(param.Name, instOp);
                }
            }

            return !error;
        }

        private Operand EvalUserDefinedFC(ITree trn, Function func, Unit unit)
        {
            ITree trnFuncName = trn.GetChild(0);

            //Check the parameter.
            if (!CheckFunctionParameter(trn, func))
                return null;

            //Create the function parameter initialisation operands.
            Hashtable refParam2Operand = CreateFunctionParamInitOperands(trn, func);
            
            if (refParam2Operand == null)
                return null;

            //Create the input stack frame
            if (!CreateFunctionInputStackFrame(func, refParam2Operand, trn))
                return null;

            //Execute the call => the call clear after the stack frame and restore the parent base ptr.
            Instruction callInst  = new Instruction(InstructionCode.Call, Helper.GetUID(trnFuncName), null, null, null );
            callInst.CallFunction = func.Name;
            _function.Instructions.Add(callInst);

            //Clear the function initialisation parameter
            ClearTheFunctionInitOperands(func, refParam2Operand);

            //Extract the function return value operand.
            Operand resultOp = (Operand)refParam2Operand[func.Name];
            
            if(resultOp == null)
                resultOp = new Operand(0,Helper.GetUID(trn),0,"VOID");

            return resultOp;
        }

        private void ClearTheFunctionInitOperands(Function func, Hashtable refParam2Operand)
        {
            foreach (Parameter param in func.Parameters)
            {
                if (func.Name == param.Name)
                    continue;

                Operand initOp = (Operand)refParam2Operand[param.Name];
                
                if(initOp != null)
                    _codeOutput.EmitRemoveTempOperandInst(initOp);
            }
        }

        private bool CreateFunctionInputStackFrame(Function func, Hashtable refParam2Operand, ITree trn)
        {
            bool retParam = func.GetParameter(func.Name) != null;
            FCCodeOutput fcCodeOutput = new FCCodeOutput(_unit, func, _codeOutput);

            foreach (Parameter param in func.Parameters)
            {
                Operand initOp = (Operand)refParam2Operand[param.Name];

                ITree trnParam = GetFuncParameter(trn, param.Name);
                ITree trnParamName = trnParam;

                if (trnParam == null)
                {
                    trnParamName = trn;
                    trnParam = trn;
                }
                else
                {
                    if (trnParam.ChildCount == 2)
                        trnParamName = trnParam.GetChild(0);
                }

                switch (param.ParamAccess)
                {
                    case Parameter.Access.Input:
                    case Parameter.Access.VarConst:                    
                    case Parameter.Access.Var:
                    case Parameter.Access.VarTemp:
                    case Parameter.Access.VarTempConst:
                        Operand paramOp = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trnParamName), param.Size, param.TypeId);

                        if (initOp == null)
                            fcCodeOutput.EmitCopyFuncParamDefValue(param, paramOp, trn);
                        else
                            _codeOutput.EmitInstruction(InstructionCode.Move, Helper.GetUID(trnParam), paramOp, initOp, new Operand(param.Size, 0, 4, "UDINT"));

                    break;

                    case Parameter.Access.InOut:
                    case Parameter.Access.Output:
                        
                        if (initOp == null)
                            return false;

                        _codeOutput.EmitInstruction(InstructionCode.PushRef, Helper.GetUID(trnParamName), null, initOp, null);
                    break;
                }
            }

            return true;
        }

        private Hashtable CreateFunctionParamInitOperands(ITree trn, Function func)
        {
            Hashtable refParam2Operand = new Hashtable();

            bool retParam = func.GetParameter(func.Name) != null;
            
            if (retParam && func.Parameters.Count == 2)                
            { //One parameter with return param

                //Create the parameter init operand.
                Parameter param = func.Parameters[0];

                ITree trnParam = null;
                
                if (trn.ChildCount == 2)
                {
                    trnParam = trn.GetChild(1);

                    if (trnParam.ChildCount == 2)
                        trnParam = trnParam.GetChild(1);
                }

                if (!CreateFuncParamInitOperand(trn, refParam2Operand, param, trnParam))
                    return null;


                //Create the return value operand.
                param = func.Parameters[1];
                
                trnParam = null;

                if (!CreateFuncParamInitOperand(trn, refParam2Operand, param, trnParam))
                    return null;

                return refParam2Operand;
            }

            if (!retParam && func.Parameters.Count == 1)
            { //One parameter no return param

                Parameter param = func.Parameters[0];

                ITree trnParam = null;

                if (trn.ChildCount == 2)
                {
                    trnParam = trn.GetChild(1);

                    if (trnParam.ChildCount == 2)
                        trnParam = trnParam.GetChild(1);
                }

                if (!CreateFuncParamInitOperand(trn, refParam2Operand, param, trnParam))
                    return null;

                return refParam2Operand;
            }

            bool error = false;

            foreach (Parameter param in func.Parameters)
            {//More that one parameter
                ITree trnParam = null;
                
                ITree trnAssParam = GetFuncParameter(trn, param.Name);

                if (trnAssParam != null)
                    trnParam = trnAssParam.GetChild(1);

                if (!CreateFuncParamInitOperand(trn, refParam2Operand, param, trnParam))
                    error = true;
            }

            if (error)
                return null;

            return refParam2Operand;
        }

        private bool CreateFuncParamInitOperand(ITree trn, Hashtable refParam2Operand, Parameter param, ITree trnParam)
        {
            FCCodeOutput fcCodeOutput = new FCCodeOutput(_unit, _function, _codeOutput);

            switch (param.ParamAccess)
            {
                case Parameter.Access.InOut:
                case Parameter.Access.Output:
                    if (trnParam == null)
                    {
                        Operand op = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trn.GetChild(0)), param.Size, param.TypeId);
                        refParam2Operand[param.Name] = op;
                        fcCodeOutput.EmitCopyFuncParamDefValue(param, op, trn);
                    }
                    else
                    {
                        if (!EvalFuncParameter(refParam2Operand, param, trnParam))
                            return false;

                        Operand op = (Operand) refParam2Operand[param.Name];
                        
                        if (op.OpType == Operand.Type.Immediate || op.IsConstant)
                        {
                            OutputMessage(GetPosInfo(op) +  String.Format(Messages.C1041, param.Name, trn.GetChild(0).Text));
                            return false;
                        }
                    }
                break;

                case Parameter.Access.Input:
                    if (trnParam == null)
                    {
                        refParam2Operand[param.Name] = null;
                    }
                    else
                    {
                        if (!EvalFuncParameter(refParam2Operand, param, trnParam))
                            return false;
                        
                    }
                break;

                case Parameter.Access.Var:
                case Parameter.Access.VarConst:
                case Parameter.Access.VarTempConst:
                case Parameter.Access.VarTemp:
                    refParam2Operand[param.Name] = null;
                break;
            }
            return true;
        }

        private bool EvalFuncParameter(Hashtable refParam2Operand, Parameter param, ITree trnParam)
        {
            Operand op = EvalExpression(trnParam);

            if (op == null)
                return false;

            Operand convOp = op;

            if (op.TypeId != param.TypeId)
            {
                convOp = EmitConvertInstruction(op, new Operand(0,Helper.GetUID(trnParam), param.Size, param.TypeId));
                if (convOp == null)
                    return false;
            }
            
            refParam2Operand[param.Name] = convOp;

            if (convOp != op)
                _codeOutput.EmitRemoveTempOperandInst(op);     
       
            return true;
        }

        private bool CheckFunctionParameter(ITree trn, Function func)
        {
            bool error = false;

            ITree trnFuncName = trn.GetChild(0);

            if (func.Parameters.Count == 1)
            {
                if (func.GetParameter(func.Name) != null)
                { //Only return value
                    if (trn.ChildCount != 1)
                    {
                        OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1039, func.Name));
                        return false;
                    }
                }
                else
                {//Only one parameter
                    if (trn.ChildCount > 2)
                    {
                        OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1040, func.Name));
                        return false;
                    }
                }

                return true;
            }
            
            if (func.Parameters.Count == 2)
            {
                if (func.GetParameter(func.Name) != null)
                { //One parameter 

                    if (trn.ChildCount > 2)
                    {
                        OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1040, func.Name));
                        return false;
                    }

                    ITree trnParamAss = trn.GetChild(1);

                    if (trnParamAss.ChildCount == 2)
                    {
                        if (!CheckFuncParamExistance(func, trnParamAss.GetChild(0)))
                            return false;

                        Parameter param = func.GetParameter(trnParamAss.GetChild(0).Text);

                        if (param.ParamAccess == Parameter.Access.Output && trnParamAss.Type != Mpal.Parser.Parser.ASSIGN2)
                        {
                            OutputMessage(Helper.GetPosString(trnParamAss) + String.Format(Messages.C1038, trnParamAss.Text, trnFuncName.Text));
                            return false;
                        }
                    }

                    return true;
                }
                
            }

            for (int i = 1; i < trn.ChildCount; ++i)
            { //More that one parameter.
                ITree trnParamAss = trn.GetChild(i);

                if (trnParamAss.ChildCount != 2)
                {
                    OutputMessage(Helper.GetPosString(trnParamAss) + String.Format(Messages.C1038, trnParamAss.Text , trnFuncName.Text));
                    error = true;
                    continue;
                }

                if (!CheckFuncParamExistance(func, trnParamAss.GetChild(0)))
                    error = true;

                Parameter param = func.GetParameter(trnParamAss.GetChild(0).Text);

                if (param.ParamAccess == Parameter.Access.Output && trnParamAss.Type != Mpal.Parser.Parser.ASSIGN2)
                {
                    OutputMessage(Helper.GetPosString(trnParamAss) + String.Format(Messages.C1038, trnParamAss.Text, trnFuncName.Text));
                    error = true;
                }

            }

            return !error;
        }

        private bool CheckFuncParamExistance(Function func, ITree trnParam)
        {
            Parameter param = func.GetParameter(trnParam.Text);
            if (param == null)
            {
                OutputMessage(Helper.GetPosString(trnParam) + String.Format(Messages.C1037, trnParam.Text, func.Name));
                return false;
            }

            if (param.ParamAccess != Parameter.Access.InOut &&
                param.ParamAccess != Parameter.Access.Input &&
                param.ParamAccess != Parameter.Access.Output)
            {
                OutputMessage(Helper.GetPosString(trnParam) + String.Format(Messages.C1036, trnParam.Text, func.Name));
             
                return false;
            }
            return true;
        }        
        
        private ITree GetFuncParameter(ITree trn, string name)
        {
            for (int i = 1; i < trn.ChildCount; ++i)
            {
                ITree trnParam = trn.GetChild(i);
                
                if (trnParam.ChildCount == 2)
                {
                    if (trnParam.GetChild(0).Text == name)
                        return trnParam;
                }
            }

            return null;
        }

        private bool CheckSupoortedDataType(ITree trn, string from, string to)
        {
            if( (from == "LREAL" || to == "LREAL") && !_options.SupportLREAL)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1052, trn.GetChild(0).Text));
                return false;
            }

            if (((from.Contains("LINT") || to.Contains("ULINT") || from.Contains("LWORD") || to.Contains("LWORD")) && !_options.SupportINT64))
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1052, trn.GetChild(0).Text));
                return false;
            }

            return true;
        }

        private bool CheckStdFuncCall(ITree trn)
        {
            if (trn.ChildCount > 2)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1013, trn.GetChild(0).Text));
                return false;
            }

            if (trn.GetChild(1).Type == Mpal.Parser.Parser.ASSIGN)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1014, trn.GetChild(0).Text));
                return false;
            }

            return true;
        }

        private Operand EvalConvertFunction(ITree trn, string from , string to)
        {
            if (!CheckStdFuncCall(trn))
                return null;
            
            if( !CheckSupoortedDataType(trn, from, to))
                return null;

            Operand result = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trn), Helper.SizeOf(to), to);
            Operand src = EvalExpression(trn.GetChild(1));

            if (src == null)
                return null;

            Operand newSrc = src;
            bool newSrcCreated = false;

            if (src.TypeId != from)
            {
                newSrc = EmitConvertInstruction(src, new Operand(0, 0,Helper.SizeOf(from), from), false);

                if (newSrc == null)
                    return null;

                newSrcCreated = true;
            }

            InstructionCode instCode = Helper.GetExpConvertInstructionCode(from, to);
            _codeOutput.EmitInstruction(instCode, Helper.GetUID(trn), result, newSrc, null);

            if (newSrcCreated)
                _codeOutput.EmitRemoveTempOperandInst(newSrc);

            _codeOutput.EmitRemoveTempOperandInst(src);

            return result;
        }
 
     

        private Operand EvalArraySelectExpression(ITree trn)
        {
            try
            {
                Operand resultOp = null;
                EmitSelectArrayInst(trn, out resultOp);
                return resultOp;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        private bool CollectArrayIndexOp(ITree trn, List<Operand> indexOperands,Parameter baseParam)
        {
            List<Operand> dimIndexOperands = new List<Operand>();
            bool allConstants = true;

            //Collect the index operands.
            for (int i = 0; i < trn.ChildCount - 1; ++i)
            {
                Operand dimIndexOp = EvalExpression(trn.GetChild(i));
                if (dimIndexOp == null)
                    throw new Exception("Index error");

                indexOperands.Add(dimIndexOp);

                if (dimIndexOp.OpType != Operand.Type.Immediate)
                {
                    allConstants = false;
                }
                else
                {
                    Dimension dim = baseParam.Dimensions[i];

                    long pos = Convert.ToInt64(dimIndexOp.ConstVal);

                    if (dim.From > pos || pos > dim.To)
                    {
                        ITree indexTrn = trn.GetChild(i);

                        OutputMessage(Helper.GetPosString(indexTrn) + String.Format(Messages.C1012, indexTrn.Text));
                        throw new Exception("Index out of range");
                    }
                }
            }

            return allConstants;
        }

        private Parameter EmitSelectArrayInst(ITree trn, out Operand resultOp)
        {
            Parameter baseParam = null;
            Parameter arrayOfParam = null;            
            ITree trnBase = trn.GetChild(trn.ChildCount - 1);

            //Optain the base parameter and operand.
            Operand baseOp = null;

            resultOp = null;
            Operand newRightOp = null;
            baseParam = EmitSelectMemoryInst(trnBase, out baseOp, out newRightOp);
            
            if (baseParam.ParamDataType == DataType.UDT)                
                baseParam = GetUDTTypeDefinition(baseParam.TypeName);

            if (baseParam.ParamDataType != DataType.ARRAY)
            {
                OutputMessage(Helper.GetPosString(trnBase) + String.Format(Messages.C1010, trnBase.Text));
                throw new Exception("Not an array type.");
            }
            
            arrayOfParam  = baseParam.Structure[0];

            if( trn.ChildCount - 1 > baseParam.Dimensions.Count)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1011, trnBase.Text));
                throw new Exception("To many index parameter");
            }
            
            //Create the result operand.            
            resultOp = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trn), arrayOfParam.Size, arrayOfParam.TypeId);
            Instruction createResultRefInst = _function.Instructions[_function.Instructions.Count - 1];
            
            //Collect all index operands.
            List<Operand> dimIndexOperands = new List<Operand>();
            bool allIndexsConstant = CollectArrayIndexOp(trn, dimIndexOperands, baseParam);

            //Calculate the array element size and determinate the array element typeId.
            uint size = arrayOfParam.Size;
            string typeId = arrayOfParam.TypeId;

            if (dimIndexOperands.Count != baseParam.Dimensions.Count)
            {
                for (int i = dimIndexOperands.Count; i < baseParam.Dimensions.Count; ++i)
                {
                    Dimension dim = baseParam.Dimensions[i];
                    size *= (uint)Math.Abs(dim.To - dim.From) + 1;
                }

                typeId += "\\" + dimIndexOperands.Count.ToString();
            }

            resultOp.Size = size;
            resultOp.TypeId = typeId;

            //For array indexing look at: http://de.wikipedia.org/wiki/Feld_(Datentyp)
            if (!allIndexsConstant)
            { //Not all index are constant => AddOffset operation.
                InstructionCode inst  = InstructionCode.NOP;

                //Calculate the liniar index
                Dimension endDim = baseParam.Dimensions[dimIndexOperands.Count - 1];
                Operand endIndex = dimIndexOperands[dimIndexOperands.Count - 1];

                Operand liniarIndexOP = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trn), 4, "DINT");

                Operand curIndexOP = null;

                if( dimIndexOperands.Count > 1)
                    curIndexOP = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trn), 4, "DINT");

                EmitArraySubIndexOp(trn, endDim, endIndex, liniarIndexOP);

                for (int s = 0; s < dimIndexOperands.Count - 1; ++s)
                {
                    Operand dimIndexOp = dimIndexOperands[s];
                    Dimension dim = baseParam.Dimensions[s];

                    EmitArraySubIndexOp(trn, dim, dimIndexOp, curIndexOP);

                    long dimSum = 0;

                    for (int t = s + 1; t < dimIndexOperands.Count; ++t)
                    {
                        Dimension curDim = baseParam.Dimensions[t];
                        dimSum += (curDim.To - curDim.From) + 1;
                    }

                    _codeOutput.EmitInstruction(InstructionCode.MulDINT, Helper.GetUID(trn), curIndexOP, curIndexOP,
                                    new Operand(dimSum, Helper.GetUID(trn), 4, "DINT"));

                    _codeOutput.EmitInstruction(InstructionCode.AddDINT, Helper.GetUID(trn), liniarIndexOP, liniarIndexOP, curIndexOP);
                }

                if (dimIndexOperands.Count > 1)
                    _codeOutput.EmitRemoveTempOperandInst(curIndexOP);

                _codeOutput.EmitInstruction(InstructionCode.MulDINT, Helper.GetUID(trn), liniarIndexOP, liniarIndexOP,
                                new Operand( arrayOfParam.Size, Helper.GetUID(trn), 4, "DINT"));
                

                //Base + offset instruction
                inst = Helper.GetInstructionCode(liniarIndexOP.TypeId, InstructionCategory.AddOffset);
                _codeOutput.EmitInstruction(inst, Helper.GetUID(trn), resultOp, baseOp, liniarIndexOP);

                _codeOutput.EmitRemoveTempOperandInst(liniarIndexOP);                
            }
            else
            { // All index are constant => calculate the current offset.

                //Calculate the linear index of the array
                Dimension endDim = baseParam.Dimensions[dimIndexOperands.Count - 1];
                Operand endIndex = dimIndexOperands[dimIndexOperands.Count - 1];
                
                long index = Convert.ToInt32( endIndex.ConstVal );
                long liniarIndex = index - endDim.From;

                for(int s = 0; s < dimIndexOperands.Count - 1; ++s)
                {
                    Operand dimIndexOp = dimIndexOperands[s];
                    Dimension dim = baseParam.Dimensions[s];                
                                                            
                    long curIndex = Convert.ToInt32( dimIndexOp.ConstVal ) - dim.From;
                    long dimSum = 0;

                    for (int t = s + 1; t < dimIndexOperands.Count; ++t)
                    {
                        Dimension curDim = baseParam.Dimensions[t];
                        dimSum += (curDim.To - curDim.From) + 1;
                    }

                    liniarIndex += curIndex * dimSum;
                }
                               
                //Calculate the offset linearIndex * sizeof(element)
                if (baseOp.OpType == Operand.Type.TemporaryRef || baseOp.OpType == Operand.Type.Reference)
                {
                    if (liniarIndex != 0)
                    {
                        _codeOutput.EmitInstruction(InstructionCode.AddOffsetUDINT, Helper.GetUID(trn), resultOp, baseOp,
                                        new Operand((liniarIndex * arrayOfParam.Size), 0, 4, "UDINT"));
                    }
                    else
                    {
                        string resTypeId = resultOp.TypeId;

                        resultOp = new Operand(baseOp.Offset, baseOp.Uid, baseOp.OpType, resultOp.Size, resultOp.TypeId);
                        resultOp.TypeId = resTypeId;

                        _function.Instructions.Remove(createResultRefInst);                        
                    }
                }
                else
                {
                    _function.Instructions.Remove(createResultRefInst);

                    uint resOffset = (uint)(baseOp.Offset + liniarIndex * arrayOfParam.Size);
                    resultOp = new Operand(resOffset, Helper.GetUID(trn), Operand.Type.Direct, size, typeId);
                }
            }

            for (int i = dimIndexOperands.Count - 1; i > -1; --i)
            {
                Operand op = dimIndexOperands[i];
                _codeOutput.EmitRemoveTempOperandInst(op);
            }

            if (dimIndexOperands.Count != baseParam.Dimensions.Count)
                return null;

            return arrayOfParam;
        }

        private void EmitArraySubIndexOp(ITree trn, Dimension dim, Operand indexOp, Operand liniarIndexOP)
        {
            if (dim.From == 0)
            {
                Operand convEndIndexOp = EmitConvertInstruction(indexOp, liniarIndexOP);
                _codeOutput.EmitInstruction(InstructionCode.Move, Helper.GetUID(trn), liniarIndexOP, convEndIndexOp, new Operand(convEndIndexOp.Size, (ulong)0, 4, "UDINT"));

                if (convEndIndexOp != indexOp)
                    _codeOutput.EmitRemoveTempOperandInst(convEndIndexOp);

                return;
            }

            if (indexOp.OpType == Operand.Type.Immediate)
            {
                if (Convert.ToUInt64(indexOp.ConstVal) != 0)
                {
                    Operand convEndIndexOp = EmitConvertInstruction(indexOp, liniarIndexOP);

                    _codeOutput.EmitInstruction(InstructionCode.SubDINT, Helper.GetUID(trn), liniarIndexOP, convEndIndexOp,
                                    new Operand(dim.From, Helper.GetUID(trn), 4, "DINT"));

                    if (convEndIndexOp != indexOp)
                        _codeOutput.EmitRemoveTempOperandInst(convEndIndexOp);
                }
            }
            else
            {
                Operand convEndIndexOp = EmitConvertInstruction(indexOp, liniarIndexOP);

                _codeOutput.EmitInstruction(InstructionCode.SubDINT, Helper.GetUID(trn), liniarIndexOP, convEndIndexOp,
                                new Operand(dim.From, Helper.GetUID(trn), 4, "DINT"));

                if (convEndIndexOp != indexOp)
                    _codeOutput.EmitRemoveTempOperandInst(convEndIndexOp);
            }
        }

        private Operand EvalMemSelectExpression(ITree trn)
        {            
            try
            {
                Operand op = null;
                Operand rightOp = null;
                EmitSelectMemoryInst(trn, out op, out rightOp);
                return op;
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }

        private Parameter GetUDT(string udtType)
        {
            string[] array = udtType.Split('.');

            return (Parameter)_unit.Types[udtType];
        }

        private Parameter GetUDTTypeDefinition(string udtType)
        {
            Parameter param = GetUDT(udtType);

            if (param.ParamDataType == DataType.UDT)
                return GetUDTTypeDefinition(param.TypeName);

            return param;
        }

        private Parameter EmitSelectMemoryInst(ITree trn, out Operand result, out Operand newRightOp)
        {
            Parameter leftParam = null;
            newRightOp = null;

            if (trn.ChildCount == 0)
                return  SelectMemoryLeftParam(trn, out result);

            ITree trnLeft = trn.GetChild(0);
            ITree trnRight = trn.GetChild(1);

            switch (trnLeft.Type)
            {
                case Mpal.Parser.Parser.DOT:
                {                    
                    leftParam = EmitSelectMemoryInst(trnLeft, out result, out newRightOp);
                }
                break;

                case Mpal.Parser.Parser.LBRACKED:
                    leftParam = EmitSelectArrayInst(trnLeft, out result);
                    if( leftParam == null)
                    {
                        OutputMessage(Helper.GetPosString(trnRight) + String.Format(Messages.C1017,trnRight.Text));
                        throw new Exception("Array indexing uncompled.");
                    }
                break;

                default:
                    leftParam = SelectMemoryLeftParam(trnLeft, out result);
                break;
            }

            Parameter rightParam = leftParam.FindMember(trnRight.Text, _unit);
            
            if (rightParam == null)
            {
                OutputMessage(Helper.GetPosString(trnRight) + String.Format(Messages.C1008, trnRight.Text, leftParam.Name));
                throw new Exception("Undefined member");
            }

            ulong leftUID = Helper.GetUID(trnLeft);

            if (trnLeft.Type == Mpal.Parser.Parser.DOT)
                leftUID = Helper.GetUID(trnLeft.GetChild(1));

            _function.Uid2Param[Helper.GetUID(trnRight)] = new StructVarUid(leftUID, Helper.GetUID(trnRight), rightParam.Name);

            if (rightParam.ParamAccess != Parameter.Access.Input &&
                rightParam.ParamAccess != Parameter.Access.InOut &&
                rightParam.ParamAccess != Parameter.Access.Output)
            {
                OutputMessage(Helper.GetPosString(trnRight) + String.Format(Messages.C1049, rightParam.Name, leftParam.Name));
                throw new Exception("Access denieded");
            }
            
            if (result.OpType == Operand.Type.Direct)
            {
                result.Offset += rightParam.Offset;
                result.TypeId  = rightParam.TypeId;
                result.Size    = rightParam.Size;
                result.OpType  = Operand.Type.Direct;                
            }
            else if(result.OpType == Operand.Type.TemporaryRef ||
                    result.OpType == Operand.Type.Reference)
            {//Add right to result operation and return the new result.
                Operand rightOp = new Operand(rightParam.Offset, Helper.GetUID(trnRight), 4, "UDINT");

                //AddOffset instruction.
                if (result.OpType == Operand.Type.Reference)
                {
                    if (rightOp.OpType != Operand.Type.Immediate || Convert.ToUInt32(rightOp.ConstVal) != 0)
                    {
                        newRightOp = rightOp;
                        Operand newResult = _codeOutput.EmitCreateTempReferenceInst(Helper.GetUID(trnRight), rightParam.Size, rightParam.TypeId);
                        _codeOutput.EmitInstruction(InstructionCode.AddOffsetUDINT, Helper.GetUID(trnRight), newResult, result, newRightOp);
                        _codeOutput.EmitRemoveTempOperandInst(result);
                        newResult.IsConstant = result.IsConstant;
                        result = newResult;
                    }
                }
                else
                {
                    if (newRightOp != null)
                    {
                        if (newRightOp.OpType == Operand.Type.Immediate)
                            newRightOp.ConstVal = Convert.ToUInt32(newRightOp.ConstVal) + Convert.ToUInt32(rightOp.ConstVal);
                        else
                            _codeOutput.EmitInstruction(InstructionCode.AddOffsetUDINT, Helper.GetUID(trn), result, result, rightOp);
                    }
                    else
                    {
                        if (rightOp.OpType == Operand.Type.Immediate)
                        {
                            if (Convert.ToUInt32(rightOp.ConstVal) != 0)
                            {
                                //Optimize check preview instruction

                                if (_function.Instructions.Count > 0)
                                {
                                    Instruction preViewInst = _function.Instructions[_function.Instructions.Count - 1];

                                    if (Helper.IsAddOffset(preViewInst.InstCode))
                                    {
                                        if (preViewInst.Op2.OpType == Operand.Type.Immediate && preViewInst.Result == result)
                                            preViewInst.Op2.ConstVal = Convert.ToUInt64(preViewInst.Op2.ConstVal) + Convert.ToUInt64(rightOp.ConstVal);
                                    }
                                    else
                                    {
                                        _codeOutput.EmitInstruction(InstructionCode.AddOffsetUDINT, Helper.GetUID(trn), result, result, rightOp);
                                    }
                                }
                                else
                                {
                                    _codeOutput.EmitInstruction(InstructionCode.AddOffsetUDINT, Helper.GetUID(trn), result, result, rightOp);
                                }
                            }
                        }
                        else
                        {
                            _codeOutput.EmitInstruction(InstructionCode.AddOffsetUDINT, Helper.GetUID(trn), result, result, rightOp);
                        }
                    }
                }                
                
                result.TypeId = rightParam.TypeId;
                result.Size = rightParam.Size;
                result.Uid = Helper.GetUID(trnRight);                
            }

            return rightParam;
        }

        private Parameter SelectMemoryLeftParam(ITree trn, out Operand result)
        {
            Parameter leftParam = SearchParameter(trn.Text);
            
            if (leftParam == null)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1002, trn.Text));
                throw new Exception("Undefined Symbol");
            }

            _function.Uid2Param[Helper.GetUID(trn)] = new StructVarUid(0, Helper.GetUID(trn), leftParam.Name);
            _function.Uid2Var[Helper.GetUID(trn)] = leftParam;

            Parameter udtParam = leftParam;

            if (leftParam.ParamDataType == DataType.UDT)
                udtParam = GetUDTTypeDefinition(leftParam.TypeName);

            if (udtParam.ParamDataType != DataType.ARRAY && 
                udtParam.ParamDataType != DataType.STRUCT &&
                udtParam.ParamDataType != DataType.FB)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1009, trn.Text));
                throw new Exception("Not a structured type.");
            }

            result = EvalExpression(trn);
            result.IsConstant = leftParam.ParamAccess == Parameter.Access.VarTempConst || leftParam.ParamAccess == Parameter.Access.VarConst;
            return leftParam;
        }

        private void EvalAssignStmt(ITree trn)
        {
            Operand resultOp = EvalExpression(trn.GetChild(0));
            Operand source = EvalExpression(trn.GetChild(1), resultOp);

            if (source == resultOp)
                return;

            if (resultOp == null || source == null)
                return;

            if (resultOp.OpType == Operand.Type.Immediate || resultOp.IsConstant)
            {
                OutputMessage(Helper.GetPosString(trn) + Messages.C1003);
                return;
            }

            Operand newSource = EmitConvertInstruction(source, resultOp);
            
            if (newSource == null)
                return;

            Operand sizeOp = new Operand(newSource.Size, (ulong)0, 4, "UDINT");
            Instruction inst = new Instruction(InstructionCode.Move, Helper.GetUID(trn), resultOp, newSource, sizeOp);

            _function.Instructions.Add(inst);

            if (newSource != source)
                _codeOutput.EmitRemoveTempOperandInst(newSource);

            _codeOutput.EmitRemoveTempOperandInst(source);
            _codeOutput.EmitRemoveTempOperandInst(resultOp);            
        }
  

        private Operand EvalUnaryExpression(ITree trn, InstructionCategory category)
        {
            if (category == InstructionCategory.Plus)
                return EvalExpression(trn.GetChild(0));

            //Create the temporaray result operand
            Operand resultOp = new Operand(_codeOutput.CreateTempOpID(), Helper.GetUID(trn), Operand.Type.Temporary, 0, "");
            Operand sizeOp = new Operand(0, 0, 4, "UDINT");
            Instruction createResultTempInst = _codeOutput.EmitInstruction(InstructionCode.PushTempOp, Helper.GetUID(trn), resultOp, sizeOp, null);

            Operand sourceOp = EvalExpression(trn.GetChild(0));

            if (sourceOp == null)
                return null;

            if (sourceOp.OpType == Operand.Type.Immediate)
            {
                Operand tempRes = Helper.EvalConstOperation(sourceOp, category);

                if (tempRes == null)
                    OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1004_1, sourceOp.TypeId));

                _function.Instructions.Remove(createResultTempInst);

                return tempRes;
            }

            //Back patching the result operand
            uint resOpSize = 0;            
            string typeId = Helper.GetOperationResultType(sourceOp, category, ref resOpSize);

            resultOp.TypeId = typeId;
            resultOp.Size = resOpSize;
            sizeOp.ConstVal = resOpSize;

            InstructionCode instCode = Helper.GetInstructionCode(sourceOp.TypeId, category);

            if (instCode == InstructionCode.NOP)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1004_1, sourceOp.TypeId));
                return null;
            }

            Instruction inst = new Instruction(instCode, Helper.GetUID(trn), resultOp, sourceOp, null);

            _function.Instructions.Add(inst);

            _codeOutput.EmitRemoveTempOperandInst(sourceOp);

            return resultOp;
        }

        private void EvalStatment(ITree trn)
        {
            switch (trn.Type)
            {
                case Mpal.Parser.Parser.ASSIGN:
                    EvalAssignStmt(trn);
                break;

                case Mpal.Parser.Parser.IF:
                    EvalIfStmt(trn, new Operand(0, 0, 4, "UDINT"), null, trn.ChildCount == 2);
                break;
                
                case Mpal.Parser.Parser.CASE:
                    EvalCaseStmt(trn);
                break;

                case Mpal.Parser.Parser.LRBRACKED:
                    Operand op = EvalFunctionStmt(trn);
                    if (op != null)                    
                    {
                        if (op.TypeId != "VOID")
                        {
                            OutputMessage(Helper.GetPosString(trn.GetChild(0)) + String.Format(Messages.C1042, trn.GetChild(0).Text));
                            _codeOutput.EmitRemoveTempOperandInst(op);
                        }
                    }
                break;

                case Mpal.Parser.Parser.FOR:
                    EvalForStmt(trn);
                break;

                case Mpal.Parser.Parser.RETURN:
                    EvalReturnStmt(trn);
                break;

                case Mpal.Parser.Parser.CONTINUE:
                    EvalContinueStmt(trn);                    
                break;

                case Mpal.Parser.Parser.EXIT:
                    EvalExitStmt(trn);
                break;

                case Mpal.Parser.Parser.WHILE:
                    EvalWhileStmt(trn);
                break;

                case Mpal.Parser.Parser.REPEAT:
                    EvalRepeatStmt(trn);
                break;
            }
        }

        private void EvalReturnStmt(ITree trn)
        {
            _codeOutput.EmitInstruction(InstructionCode.Return, Helper.GetUID(trn), null, null, null);
        }

        private void EvalExitStmt(ITree trn)
        {
            if (_loopEndLabel == null)
            {
                OutputMessage(Helper.GetPosString(trn) + Messages.C1024);
                return;
            }

            _codeOutput.EmitInstruction(InstructionCode.Jmp, Helper.GetUID(trn), null, _loopEndLabel, null);
        }

        private void EvalContinueStmt(ITree trn)
        {
            if (_loopStartLabel == null)
            {
                OutputMessage(Helper.GetPosString(trn) + Messages.C1025);
                return;
            }

            _codeOutput.EmitInstruction(InstructionCode.Jmp, Helper.GetUID(trn), null, _loopStartLabel, null);
        }

        private Operand EmitConvertInstruction(Operand from, Operand to)
        {
            Operand convertedOp = from;

            if (from.OpType == Operand.Type.Immediate)
                Helper.ConvertConst(from, to);

            if (from.TypeId != to.TypeId)
                convertedOp = EmitConvertInstruction(from, to, false);

            return convertedOp;
        }

        private void EvalRepeatStmt(ITree trn)
        {
            Operand doLabelOp = new Operand(0, 0, 4, "UDINT");
            Operand endLabelOp = new Operand(0, 0, 4, "UDINT");

            _codeOutput.EmitInstruction(InstructionCode.Jmp, Helper.GetUID(trn), null, doLabelOp, null);
            
            //Set the start label.
            Operand startLabelOp = new Operand(_function.Instructions.Count, 0, 4, "UDINT");

            ITree untilOp = trn.GetChild(trn.ChildCount - 1);
            Operand condOp = EvalExpression(untilOp.GetChild(0));

            if (condOp == null)
                return;

            _codeOutput.EmitInstruction(InstructionCode.JmpFALSE, Helper.GetUID(trn), null, condOp, endLabelOp);

            condOp.Uid = Helper.GetUID(trn);
            _codeOutput.EmitRemoveTempOperandInst(condOp);       

            //Set the do label.
            doLabelOp.ConstVal = _function.Instructions.Count;

            Operand oldLoopEndLabel = _loopEndLabel;
            Operand oldLoopStartLabel = _loopStartLabel;

            _loopStartLabel = startLabelOp;
            _loopEndLabel = endLabelOp;

            for (int i = 0; i < trn.ChildCount - 1; ++i)
                EvalStatment(trn.GetChild(i));

            _loopStartLabel = oldLoopStartLabel;
            _loopEndLabel = oldLoopEndLabel;

            _codeOutput.EmitInstruction(InstructionCode.Jmp, Helper.GetUID(trn.GetChild(trn.ChildCount - 1)), null, startLabelOp, null);

            //Set the end label.
            endLabelOp.ConstVal = _function.Instructions.Count;

            condOp.Uid = GetNextNodeUid(trn);
            _codeOutput.EmitRemoveTempOperandInst(condOp);
        }

        private void EvalWhileStmt(ITree trn)
        {
            Operand endLabelOp   = new Operand(0, 0, 4, "UDINT");
            
            //Set the start label
            Operand startLabelOp = new Operand(_function.Instructions.Count, 0, 4, "UDINT");

            //Eval the condition.
            Operand condOp = EvalExpression(trn.GetChild(0));

            if(condOp == null)
                return;

            if (condOp.TypeId != "BOOL")
            {
                OutputMessage(Helper.GetPosString(trn.GetChild(0)) + Messages.C1026);
                return;
            }

            //Jump to end if false.            
            _codeOutput.EmitInstruction(InstructionCode.JmpFALSE, Helper.GetUID(trn.GetChild(0)), null, condOp, endLabelOp);

            Operand oldLoopEndLabel = _loopEndLabel;
            Operand oldLoopStartLabel = _loopStartLabel;

            _loopEndLabel   = endLabelOp;
            _loopStartLabel = startLabelOp;

            //Eval the statments.
            ITree trnDo = trn.GetChild(1);

            for (int i = 0; i < trnDo.ChildCount; ++i)
                EvalStatment(trnDo.GetChild(i));

            _loopEndLabel   = oldLoopEndLabel;
            _loopStartLabel = oldLoopStartLabel;

            //Remove the cond operand.
            condOp.Uid = Helper.GetUID(trnDo.GetChild(trnDo.ChildCount - 1));
            _codeOutput.EmitRemoveTempOperandInst(condOp);

            //Jump to start.
            _codeOutput.EmitInstruction(InstructionCode.Jmp, Helper.GetUID(trnDo.GetChild(trnDo.ChildCount - 1)), null, startLabelOp, null);
            
            //Set the end label.
            endLabelOp.ConstVal = _function.Instructions.Count;
            condOp.Uid = GetNextNodeUid(trn);

            _codeOutput.EmitRemoveTempOperandInst(condOp);
        }

        private void EvalForStmt(ITree trn)
        {
            ITree trnForVarAssign = trn.GetChild(0);
            ITree trnForVar = trnForVarAssign.GetChild(0);

            //For variable
            Operand forVarOp = GetForVariableOperand(trnForVar);

            if (forVarOp == null)
                return;

            //Initialisation variable
            Operand initForVarOp = EvalExpression(trnForVarAssign.GetChild(1));

            if (initForVarOp == null)
                return;

            //Convert the initialisation variable.
            Operand initVarConverted = EmitConvertInstruction(initForVarOp, forVarOp);
            
            if( initVarConverted == null)
                return;

            //Create the initialise 'for'-variable instruction.
            _codeOutput.EmitInstruction(InstructionCode.Move, Helper.GetUID(trnForVarAssign), forVarOp, initForVarOp, 
                             new Operand(initForVarOp.Size, (ulong)0, 4,"UDINT") );

            if( initVarConverted != initForVarOp )
                _codeOutput.EmitRemoveTempOperandInst(initVarConverted);

            //The 'to' variable.
            ITree trnTo = trn.GetChild(1);
            Operand toVarOp = EvalExpression(trnTo.GetChild(0));

            if (toVarOp == null)
                return;

            if( !Helper.IsIntegerType(toVarOp.TypeId))
            {
                OutputMessage(Helper.GetPosString(trnTo) + Messages.C1023);
                return;
            }
            
            //Convert the 'to' variable
            Operand convToVarOp = CreateConvertedOperand(toVarOp, forVarOp);
            
            if( convToVarOp == null)
                return;

            //Get the 'by' variable
            ITree trnDo = trn.GetChild(2);
            ITree trnBy = null;

            Operand incOp = null;
            Operand incOpConv = null;

            if (trnDo.Type == Mpal.Parser.Parser.BY)
            {
                trnBy = trnDo;
                trnDo = trn.GetChild(3);
                incOp = EvalExpression(trnBy.GetChild(0));
                
                if (incOp == null)
                    return;

                incOpConv = CreateConvertedOperand(incOp, forVarOp);
                
                if (incOpConv == null)
                    return;
            }

            Operand endLabelOp      = new Operand(0, 0, 4, "UDINT");
            Operand continueLabelOp = new Operand(0, 0, 4, "UDINT");
            Operand cmpTempOp = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trn), 1, "BOOL");

            //Set the start label.
            Operand startLabelOp = new Operand(_function.Instructions.Count, 0, 4, "UDINT");            

            //Update the to variable.
            if( convToVarOp != toVarOp)
            {
                InstructionCode inst = Helper.GetImplConvertInstructionCode(toVarOp.TypeId, convToVarOp.TypeId);
                _codeOutput.EmitInstruction(inst, 0, convToVarOp, toVarOp, null);
            }

            //Update the step variable.
            if( incOpConv != null && incOpConv != incOp)
            {
                InstructionCode inst = Helper.GetImplConvertInstructionCode(incOp.TypeId, incOpConv.TypeId);
                _codeOutput.EmitInstruction(inst, 0, incOpConv, incOp, null);
            }

            //Create the compare instruction.
            InstructionCode code = Helper.GetInstructionCode(forVarOp.TypeId, InstructionCategory.Gr);

            _codeOutput.EmitInstruction(code, Helper.GetUID(trnTo), cmpTempOp, forVarOp, convToVarOp);

            _codeOutput.EmitInstruction(InstructionCode.JmpTRUE, Helper.GetUID(trn), null, cmpTempOp, endLabelOp);

            Operand oldLoopEndLabel = _loopEndLabel;
            Operand oldLoopStartLabel = _loopStartLabel;

            //Set the start/end label.
            _loopEndLabel = endLabelOp;          //Used by EXIT statment.
            _loopStartLabel = continueLabelOp;   //Used by CONTINUE statment

            //Create the 'do' statments.
            for (int i = 0; i < trnDo.ChildCount; ++i)
                EvalStatment(trnDo.GetChild(i));

            //Reset the start/end label.
            _loopEndLabel = oldLoopEndLabel;
            _loopStartLabel = oldLoopStartLabel;

            continueLabelOp.ConstVal = _function.Instructions.Count;

            //Create the increment statment.
            if (incOpConv == null)
            {
                InstructionCode inst = Helper.GetInstructionCode(forVarOp.TypeId, InstructionCategory.Inc);
                _codeOutput.EmitInstruction(inst, Helper.GetUID(trn), forVarOp, null, null);
            }
            else
            {
                InstructionCode inst = Helper.GetInstructionCode(forVarOp.TypeId, InstructionCategory.Add);
                _codeOutput.EmitInstruction(inst, Helper.GetUID(trn), forVarOp, forVarOp, incOpConv);
            }

            //Create the jump to the start label statment.
            _codeOutput.EmitInstruction(InstructionCode.Jmp, Helper.GetUID(trn), null, startLabelOp, null);

            //Initialise the end label.
            endLabelOp.ConstVal = _function.Instructions.Count;

            //Clear the stack.
            _codeOutput.EmitRemoveTempOperandInst(cmpTempOp);

            if (incOp != incOpConv && incOpConv != null)
                _codeOutput.EmitRemoveTempOperandInst(incOpConv);

            if( incOp != null)
                _codeOutput.EmitRemoveTempOperandInst(incOp);

            if (convToVarOp != toVarOp)
                _codeOutput.EmitRemoveTempOperandInst(convToVarOp);

            _codeOutput.EmitRemoveTempOperandInst(toVarOp);
            _codeOutput.EmitRemoveTempOperandInst(initForVarOp);
            _codeOutput.EmitRemoveTempOperandInst(forVarOp);   
        }

        private Operand GetForVariableOperand(ITree trnForVar)
        {
            Parameter param = SearchParameter(trnForVar.Text);

            if (param == null)
            {
                OutputMessage(Helper.GetPosString(trnForVar) + String.Format(Messages.C1002, trnForVar.Text));
                return null;
            }

            if (param.ParamAccess == Parameter.Access.VarConst ||
                param.ParamAccess == Parameter.Access.VarTempConst)
            {
                OutputMessage(Helper.GetPosString(trnForVar) + Messages.C1003);
                return null;
            }

            if (!Helper.IsIntegerType(param.TypeId))
            {
                OutputMessage(Helper.GetPosString(trnForVar) + String.Format(Messages.C1022, param.Name));
                return null;
            }

            _function.Uid2Param[Helper.GetUID(trnForVar)] = new StructVarUid(0, Helper.GetUID(trnForVar), param.Name);
            _function.Uid2Var[Helper.GetUID(trnForVar)] = param;

            if (_function.IsStackParam(param))
                return new Operand(param.Offset, Helper.GetUID(trnForVar), Operand.Type.Direct, param.Size, param.TypeId);
            else
                return new Operand(param.Offset, Helper.GetUID(trnForVar), Operand.Type.Reference, param.Size, param.TypeId);
        }

        private void EvalIfThen(ITree trn)
        {
            for (int i = 0; i < trn.ChildCount; ++i)
                EvalStatment(trn.GetChild(i));
        }

        private long GetCaseValueFromIdentifier(ITree trn, string selName)
        {
            Parameter param = SearchParameter(trn.Text);

            if (param == null)
            {
                param = SearchParameter(selName);

                if (param == null)
                {
                    OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1002, trn.Text));
                    throw new Exception("error");
                }
                else
                {
                    if (param.ParamDataType != DataType.UDT)
                    {
                        OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1002, trn.Text));
                        throw new Exception("error");
                    }

                    Parameter udtParam = _unit.SearchUDT(param.TypeName);

                    if (udtParam.ParamDataType != DataType.ENUM)
                    {
                        OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1002, trn.Text));
                        throw new Exception("error");
                    }

                    long pos = 0;
                    foreach (string item in udtParam.EnumList)
                    {
                        if (item == trn.Text)
                            return pos;
                        ++pos;
                    }

                    if (param.ParamDataType != DataType.ENUM)
                    {
                        OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1002, trn.Text));
                        throw new Exception("error");
                    }
                }

            }

            if (param.ParamAccess != Parameter.Access.VarConst ||
                param.ParamAccess != Parameter.Access.VarTempConst)
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1019, trn.Text));
                throw new Exception("error");
            }

            if (!Helper.IsIntegerType(param.TypeId))
            {
                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.C1020, trn.Text));
                throw new Exception("error");
            }

            return (long)param.DefaultValue;
        }

        private void CollectCaseSelectors(ITree trnSelector, List<Dimension> selectors, string caseVar)
        {
            Dimension dim = new Dimension();

            switch (trnSelector.Type)
            {
                case Mpal.Parser.Parser.COMMA:
                    CollectCaseSelectors(trnSelector.GetChild(0), selectors, caseVar);
                    CollectCaseSelectors(trnSelector.GetChild(1), selectors, caseVar);
                return;

                case Mpal.Parser.Parser.INTEGER:
                    dim.From = Convert.ToInt64(trnSelector.Text);
                    dim.To = dim.From;
                break;

                case Mpal.Parser.Parser.DOTDOT:
                    dim.From = GetCaseRangeIndex(trnSelector.GetChild(0), caseVar);
                    dim.To = GetCaseRangeIndex(trnSelector.GetChild(1), caseVar);
                break;

                case Mpal.Parser.Parser.IDENTIFIER:                    
                    try
                    {
                        dim.From = GetCaseValueFromIdentifier(trnSelector, caseVar);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        return;
                    }

                    dim.To = dim.From;

                break;
            }

            selectors.Add(dim);
        }

        private long GetCaseRangeIndex(ITree trn, string caseVar)
        {
            long index = 0;

            switch (trn.Type)
            {
                case Mpal.Parser.Parser.NEG:
                    index = -Convert.ToInt64(trn.GetChild(0).Text);
                    break;

                case Mpal.Parser.Parser.PLUS:
                    index = Convert.ToInt64(trn.GetChild(0).Text);
                    break;

                case Mpal.Parser.Parser.IDENTIFIER:
                    index = GetCaseValueFromIdentifier(trn, caseVar);
                    break;

                case Mpal.Parser.Parser.INTEGER:
                    index = Convert.ToInt64(trn.Text);
                    break;
            }
            return index;
        }

        private void EvalCaseStmt(ITree trn)
        {
            //Eval the case variable.
            Operand caseOp = EvalExpression(trn.GetChild(0));

            if (caseOp == null)
                return;
            
            if (!Helper.IsIntegerType(caseOp.TypeId))
            {
                OutputMessage(GetPosInfo(caseOp) + Messages.C1018);
                return;
            }

            //Convert the case variable to DINT            
            Operand selOp = caseOp;
            bool selOpCreated = false;

            if (caseOp.TypeId != "DINT" && caseOp.TypeId != "ENUMERATION")
            {
                selOp = EmitConvertInstruction(caseOp, new Operand(0, caseOp.Uid, 8, "DINT"), false);
                
                if (selOp == null)
                    return;

                selOpCreated = true;
            }

            //Collect the cases
            ITree trnCaseOf = trn.GetChild(1);
            List<CaseItem> cases = new List<CaseItem>();

            for (int i = 0; i < trnCaseOf.ChildCount; ++i)
            {
                ITree trnCase = trnCaseOf.GetChild(i);
                
                CaseItem item = new CaseItem();
                item.TrnStatments = trnCase;

                ITree trnSelector = trnCase.GetChild(0);

                CollectCaseSelectors(trnSelector, item.Selector, trn.GetChild(0).Text);
                cases.Add(item);
            }

            //Check the cases
            CheckCases(cases);

            //Create the jump instructions.
            Operand cmpResOp1 = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trn), 4, "DINT");
            Operand cmpResOp2 = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trn), 4, "DINT");
            Operand cmpResOp3 = _codeOutput.EmitCreateTempOperandInst(Helper.GetUID(trn), 4, "DINT");

            foreach (CaseItem item in cases)
            {
                foreach (Dimension dim in item.Selector)
                {
                    if (dim.From == dim.To)
                    {
                        _codeOutput.EmitInstruction(InstructionCode.EqDINT, Helper.GetUID(item.TrnStatments), cmpResOp3, selOp, new Operand(dim.From, 0, 4, "DINT"));
                        _codeOutput.EmitInstruction(InstructionCode.JmpTRUE, Helper.GetUID(item.TrnStatments), null, cmpResOp3, item.Label);
                    }
                    else
                    {
                        _codeOutput.EmitInstruction(InstructionCode.GeDINT, Helper.GetUID(item.TrnStatments), cmpResOp1, selOp, new Operand(dim.From, 0, 4, "DINT"));
                        _codeOutput.EmitInstruction(InstructionCode.LeDINT, Helper.GetUID(item.TrnStatments), cmpResOp2, selOp, new Operand(dim.To, 0, 4, "DINT"));
                        _codeOutput.EmitInstruction(InstructionCode.AndBOOL, Helper.GetUID(item.TrnStatments), cmpResOp3, cmpResOp1, cmpResOp2);
                        _codeOutput.EmitInstruction(InstructionCode.JmpTRUE, Helper.GetUID(item.TrnStatments), null, cmpResOp3, item.Label);
                    }
                }
            }

            //Create the case else body.
            ulong uid = 0;
            if (trn.ChildCount == 3)
            {
                ITree trnCaseElse = trn.GetChild(2);
                for( int i = 0; i < trnCaseElse.ChildCount; ++i)
                    EvalStatment(trnCaseElse.GetChild(i));

                uid = Helper.GetUID(trnCaseElse.GetChild(trnCaseElse.ChildCount - 1));
            }
            else
            {
                CaseItem lastItem = cases[cases.Count - 1];
                uid = Helper.GetUID(lastItem.TrnStatments);
            }

            //Create the jump to the end label.
            Operand endLabel = new Operand(0, 0, 4, "UDINT");
            _codeOutput.EmitInstruction(InstructionCode.Jmp, uid, null, endLabel, null);

            //Create the cases body.
            int pos = 0;
            foreach (CaseItem item in cases)
            {
                item.Label.ConstVal = _function.Instructions.Count;

                for (int i = 1; i < item.TrnStatments.ChildCount; ++i)
                    EvalStatment(item.TrnStatments.GetChild(i));

                if (pos != cases.Count - 1)
                {//The last label don't need to jump to the end.
                    int lastPos = item.TrnStatments.ChildCount - 1;
                    _codeOutput.EmitInstruction(InstructionCode.Jmp, Helper.GetUID(item.TrnStatments.GetChild(lastPos)), null, endLabel, null);
                }

                ++pos;
            }

            //Set the end label.
            endLabel.ConstVal = _function.Instructions.Count;

            //Clear the stack.
            cmpResOp3.Uid = GetNextNodeUid(trn);
            cmpResOp2.Uid = cmpResOp3.Uid;
            cmpResOp1.Uid = cmpResOp3.Uid;

            _codeOutput.EmitRemoveTempOperandInst(cmpResOp3);
            _codeOutput.EmitRemoveTempOperandInst(cmpResOp2);
            _codeOutput.EmitRemoveTempOperandInst(cmpResOp1);

            if (selOpCreated)
            {
                selOp.Uid = cmpResOp3.Uid;
                _codeOutput.EmitRemoveTempOperandInst(selOp);
            }
        }

        private void CheckCases(List<CaseItem> selectors)
        {
            List<CaseDim2Item> sl = new List<CaseDim2Item>();            
            
            foreach (CaseItem item in selectors)
            {
                foreach (Dimension dim in item.Selector)
                {
                    CaseDim2Item it = new CaseDim2Item();
                    it.Dim = dim;
                    it.Item = item;
                    it.From = dim.From;
                    sl.Add(it);
                }
            }

            sl.Sort(new CaseDim2Item());
            
            for(int i = 0; i < sl.Count; ++i)
            {
                 CaseDim2Item item = sl[i];

                for (int j = i; j < sl.Count; ++j)
                {
                    CaseDim2Item nextItem = sl[j];

                    long to = item.Dim.To;
                    long from = nextItem.Dim.From;

                    if (to >= from && item.Item != nextItem.Item)
                        OutputMessage(Helper.GetPosString(item.Item.TrnStatments) + String.Format(Messages.C1021, item.Item.TrnStatments.Line.ToString(), nextItem.Item.TrnStatments.Line.ToString()));
                }
            }
        }

        private int GetNodeIndexInParent(ITree node)
        {
            if (node.Parent == null)
                return - 1;

            for (int i = 0; i < node.Parent.ChildCount; ++i)
            {
                ITree n = node.Parent.GetChild(i);
                
                if (n == node)
                    return i;
            }

            return - 1;
        }

        private ulong GetNextNodeUid(ITree node)
        {
            int index = GetNodeIndexInParent(node);
            if (index == -1)
                return 0;

            if (index == (node.Parent.ChildCount - 1))
                return GetNextNodeUid(node.Parent);

            return Helper.GetUID(node.Parent.GetChild(index + 1));
        }

        private void EvalIfStmt(ITree trn, Operand endLabel, Operand compCondOp, bool lastStatment)
        {
            Operand condOp = compCondOp;

            if (compCondOp == null)
                condOp = EvalExpression(trn.GetChild(0));
            else
                condOp = EvalExpression(trn.GetChild(0), compCondOp);

            if (condOp == null)
                return;

            if (condOp.TypeId != "BOOL")
            {
                OutputMessage(Helper.GetPosString(trn) + Messages.C1007);
                return;
            }

            //IfThen
            Operand jmpThen = new Operand(0,0,4,"UDINT");
            _codeOutput.EmitInstruction(InstructionCode.JmpFALSE, Helper.GetUID(trn), null, condOp, jmpThen);

            ITree trnElse = null;
            
            if( trn.ChildCount > 2)
                trnElse = trn.GetChild(2);

            EvalIfThen( trn.GetChild(1));

            ITree endNode = trn.GetChild(1).GetChild( trn.GetChild(1).ChildCount - 1);
            
            if(!lastStatment) 
                _codeOutput.EmitInstruction(InstructionCode.Jmp, Helper.GetUID(endNode), null, endLabel, null);

            jmpThen.ConstVal = _function.Instructions.Count;            

            for (int i = 2; i < trn.ChildCount; ++i)
            {
                ITree trnElIf = trn.GetChild(i);

                if (trnElIf.Type == Mpal.Parser.Parser.ELSIF)
                {
                    EvalIfStmt(trnElIf, endLabel, condOp, i == (trn.ChildCount -1) );
                }
                else if (trnElIf.Type == Mpal.Parser.Parser.ELSE)
                {
                    EvalIfThen(trnElIf);
                }
            }

            endLabel.ConstVal = _function.Instructions.Count;

            condOp.Uid = GetNextNodeUid(trn);            

             if( compCondOp == null)
                _codeOutput.EmitRemoveTempOperandInst(condOp);
        }

        private string GetPosInfo(Operand op)
        {
            ulong uid = op.Uid;
            ulong line = (uid) >> 32;
            uint pos = (uint)(uid & 0x00000000ffffffff);
            string linePos = " (" + line.ToString() + "," + pos.ToString() + ")";
            return linePos;
        }

        private void OutputMessage(string msg)
        {
            if (OnMessage != null)
                OnMessage(msg);
        }
     
    }
}
