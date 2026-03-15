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
using System.Globalization;

using Antlr.Runtime;
using Antlr.Runtime.Tree;

using Mpal.Model;

namespace Mpal.Compiler
{
    internal class IfaceCompiler
    {
        private CompilerOptions _options;
        private bool _error = false;

        public event Message OnMessage;
        
        public IfaceCompiler()
        {
        }

        public bool Compile(Unit unit, CompilerOptions options)
        {
            _options = options;
            _error = false;

            //Load the the type definitions.
            foreach (DictionaryEntry entry in unit.Types)
            {
                Parameter param = (Parameter)entry.Value;
                LoadType(param, unit.Name, unit);
            }
            //Calculate the variable offsets

            foreach (DictionaryEntry entry in unit.Functions)
                CalcVariableOffset((Function)entry.Value, unit);
            
            //Load the functions.
            foreach (DictionaryEntry entry in unit.Functions)
                LoadVariables((Function)entry.Value, unit);            

            return !_error;
        }

        private void CalcVariableOffset(Function function, Unit unit)
        {
            uint offset = 0;
            uint fbOffset = 0;

            if (function.FuncType == Function.Type.FB)
                offset = _options.VmAddressSize; //First is a reference of the object

            foreach (Parameter param in function.Parameters)
            {
                if (param.ParamDataType == DataType.UDT ||  param.ParamDataType == DataType.FB)
                    CalcUDTSize(param, unit);

                param.CalcSize(unit, null);

                if (function.IsStackParam(param))
                {
                    param.Offset = offset;   
                    offset += param.Size;
                }
                else
                {
                    if (function.FuncType == Function.Type.FB)
                    {
                        param.Offset = fbOffset;
                        fbOffset += param.Size;
                    }
                    else
                    {
                        param.Offset = offset;   
                        offset += _options.VmAddressSize;
                    }
                }
                param.CalcOffsets();
            }

            function.FuncBlockSize = fbOffset;
            function.StackSize = offset;
        }

        private void LoadVariables(Function function, Unit unit)
        {
            //Load the constant section first
            foreach (Parameter param in function.Parameters)
            {                
                if ((param.ParamAccess == Parameter.Access.VarConst || param.ParamAccess == Parameter.Access.VarTempConst))
                {
                    LoadParameter(function, param, param.ASTNode, unit);

                    if (param.DefaultValue == null && param.ParamDataType != DataType.STRUCT)
                        OutputMessage(GetPosString(param.ASTNode) + String.Format(Messages.G1009, param.Name));

                    if (param.ParamDataType == DataType.STRUCT)
                        CheckedStructMemberOfDefaultValue(param);

                    if (param.ParamDataType == DataType.FB)
                        OutputMessage(GetPosString(param.ASTNode) + String.Format(Messages.G1013, param.Name));
                }
            }

            //Load the other sections
            foreach (Parameter param in function.Parameters)
            {
                if ((param.ParamAccess == Parameter.Access.VarConst || param.ParamAccess == Parameter.Access.VarTempConst))
                    continue;

                LoadParameter(function, param, param.ASTNode, unit);                
            }
        }


        private void CheckedStructMemberOfDefaultValue(Parameter param)
        {
            foreach (Parameter memberParam in param.Structure)
            {
                if (memberParam.ParamDataType == DataType.STRUCT)
                {
                    CheckedStructMemberOfDefaultValue(memberParam);                   
                }
                else
                {
                    if (memberParam.DefaultValue == null)
                        OutputMessage(GetPosString(memberParam.ASTNode.Parent.GetChild(0)) + String.Format(Messages.G1009, memberParam.Name));
                }
            }
        }

        private void LoadType(Parameter param, string unitName, Unit unit)
        {            
            ITree trnTypeDef = param.ASTNode;
            ITree trnTypeType = trnTypeDef.GetChild(1);
            LoadParameter(null, param, trnTypeType, unit);
        }

        private object GetDefaultValueFromTypeDef(Function func, Unit unit, ITree trnVarType, DataType dataType, int astType)
        {
            if (trnVarType.ChildCount == 0)
                return null;

            ITree trnDefVal = trnVarType.GetChild(0);

            if (trnDefVal.Type == Mpal.Parser.Parser.SHARP)
            {
                if (trnDefVal.GetChild(0).Type != astType)
                    OutputMessage(GetPosString(trnDefVal) + Messages.G1004);

                trnDefVal = trnDefVal.GetChild(1);
            }
            else if (trnDefVal.Type == Mpal.Parser.Parser.IDENTIFIER)
            {
                if (func != null)
                {
                    foreach (Parameter param in func.Parameters)
                    {
                        if (param.ParamAccess == Parameter.Access.VarConst || param.ParamAccess == Parameter.Access.VarTempConst)
                        {
                            if (param.Name == trnDefVal.Text)
                                return GetDefaultValue(param.ASTNode.GetChild(0), dataType);
                        }
                    }

                    OutputMessage(GetPosString(trnDefVal) + String.Format(Messages.G1002, trnDefVal.Text));
                    return null;
                }
            }
            return GetDefaultValue(trnDefVal, dataType);
        }


        private void LoadStructParam(Function function, Parameter param, ITree trnVarType, Unit unit)
        {
            uint offset = 0;
            foreach (Parameter memberParam in param.Structure)
            {
                memberParam.CalcSize(unit, param);
                memberParam.CalcOffsets();
                LoadParameter(function, memberParam, memberParam.ASTNode, unit);
                memberParam.Offset = offset;
                offset += memberParam.Size;
            }
        }

        private void LoadParameter(Function function, Parameter param, ITree trnType, Unit unit)
        {            
            switch (param.ParamDataType)
            {
                case DataType.ARRAY:
                    LoadArrayParam(function, param, trnType, unit);
                    param.DefaultValue = LoadArrayDefaultValue(param, trnType, unit);
                break;
                
                case DataType.STRUCT:
                    LoadStructParam(function, param, trnType, unit);
                    param.DefaultValue = LoadStructFBDefaultValue(param, trnType, null, unit);
                break;
                
                case DataType.UDT:
                case DataType.FB:
                    LoadUDTParam(param, trnType, unit);
                    param.DefaultValue = LoadUDTParamDefaultValue(param, trnType, trnType, false, unit);
                break;                

                default:
                {
                    object obj = GetDefaultValueFromTypeDef(function, unit, trnType, param.ParamDataType, trnType.Type);

                    if (obj != null)
                        param.DefaultValue = obj;
                }
                break;
            }
        }

        private void CalcUDTSize(Parameter param, Unit unit)
        {
            Parameter udtParam = unit.SearchUDT(param.ASTNode.Text);

            if (udtParam == null)
            {  //Search Function Block.
                Function func = unit.SearchFunction(param.ASTNode.Text);
                
                if (func == null)
                    return;

                param.Size = func.CalcFBSize(unit);
                if( param.Size == 0)
                {//Empty FB can not be instaciated.

                    OutputMessage(GetPosString(param.ASTNode) + String.Format(Messages.G1014, param.ASTNode.Text));
                    return;
                }
            }
            else
            {
                param.Size = udtParam.CalcSize(unit, null);
            }

            
        }

        private void LoadUDTParam(Parameter param, ITree trnType, Unit unit)
        {
            Parameter udtParam = unit.SearchUDT(trnType.Text);

            if (udtParam == null)
            {  //Search Function Block.
                Function func = unit.SearchFunction(trnType.Text);

                if (func == null)
                {
                    OutputMessage(GetPosString(trnType) + String.Format(Messages.G1005, trnType.Text));
                    return;
                }

                if (func.FuncType != Function.Type.FB)
                {
                    OutputMessage(GetPosString(trnType) + Messages.C1047);
                    return;
                }

                param.ParamDataType = DataType.FB;
                param.TypeId = func.Name;
                param.TypeName = func.Name;
            }
            else
            {
                param.TypeId    = udtParam.TypeId;
                param.TypeName  = udtParam.Name;
            }
        }
        
        private object LoadUDTParamDefaultValue(Parameter param, ITree trnType, ITree trnValue, bool valueForced, Unit unit)
        {
            Parameter udtParam = unit.SearchUDT(trnType.Text);

            if (udtParam == null)
            {//May be is a function block.

                Function func = unit.SearchFunction(trnType.Text);

                if (func == null)
                    return null;

                if( func.FuncType != Function.Type.FB)
                    return null;

                return LoadStructFBDefaultValue(null, trnValue, func, unit);
            }

            switch (udtParam.ParamDataType)
            {
                case DataType.STRUCT:
                    return LoadStructFBDefaultValue(udtParam, trnValue, null, unit);

                case DataType.ARRAY:
                    return LoadArrayDefaultValue(udtParam, trnValue, unit);

                case DataType.ENUM:
                    return LoadENUMDefaultValue(udtParam, trnValue);                

                case DataType.UDT:
                case DataType.FB:
                    return LoadUDTParamDefaultValue(udtParam, udtParam.ASTNode.GetChild(1), trnValue, valueForced, unit);

                default:
                    if (valueForced)
                    {
                        return GetDefaultValue(trnValue, udtParam.ParamDataType);
                    }
                    else
                    {
                        if (trnValue.ChildCount == 0)
                            return null;
                        return GetDefaultValue(trnValue.GetChild(0), udtParam.ParamDataType);
                    }
            }
        }

        private object LoadENUMDefaultValue(Parameter udtParam, ITree trnValue)
        {
            if (trnValue.ChildCount == 0)
                return null;

            ITree defValueChild = trnValue.GetChild(0);
            for (int i = 0; i < udtParam.EnumList.Count; ++i)
            {
                if (udtParam.EnumList[i] == defValueChild.Text)
                    return i;
            }

            OutputMessage(GetPosString(defValueChild) + String.Format(Messages.G1005, defValueChild.Text));
            return null;
        }

        private object LoadArrayDefaultValue(Parameter param, ITree trnType, Unit unit)
        {
            ITree trnDefValue = trnType.Parent.GetChild(trnType.Parent.ChildCount - 1);

            if (trnDefValue.Type != Mpal.Parser.Parser.LBRACKED)
                return null;

            ArrayList defValue = new ArrayList();

            LoadArrayDimensionDefValue(0, trnDefValue, param, defValue, unit);
            return defValue;
        }

        private void LoadArrayDimensionDefValue(int dim, ITree trn, Parameter parameter, ArrayList values, Unit unit)
        {
            if (trn.Type != Mpal.Parser.Parser.LBRACKED)
            {
                OutputMessage(GetPosString(trn) + String.Format(Messages.G1008, parameter.Name));
                return;
            }                

            long countElement = Math.Abs(parameter.Dimensions[dim].To - parameter.Dimensions[dim].From) + 1;
            if (trn.ChildCount > countElement)
            {
                OutputMessage(GetPosString(trn) + String.Format(Messages.G1008, parameter.Name));
                return;
            }

            dim++;
            // We are on a leaf? peek the value into the array.
            if (dim == parameter.Dimensions.Count)
            {
                DataType arrayType = parameter.Structure[0].ParamDataType;

                object defValue = null;
                Dimension endDim = parameter.Dimensions[parameter.Dimensions.Count - 1];
                int elements = (int)((endDim.To - endDim.From) + 1);

                for (int i = 0; i < elements; ++i)
                {
                    if (i >= trn.ChildCount)
                    {
                        values.Add(null);
                        continue;
                    }

                    ITree trnValue = trn.GetChild(i);                    

                    switch(arrayType)
                    {
                        case DataType.STRUCT:
                        {
                            ArrayList elemetsInitArray = new ArrayList();
                            Parameter typeParam = parameter.Structure[0];
                            LoadMemberDefaultValue(trnValue, typeParam, elemetsInitArray, null, unit);                            
                            defValue = elemetsInitArray;
                        }
                        break;

                        case DataType.FB:
                        {
                            Parameter typeParam = parameter.Structure[0];
                            Function func = unit.SearchFunction(typeParam.TypeName);
                            defValue = new ArrayList();
                            
                            if (trnValue != null)
                                LoadMemberDefaultValue(trnValue, typeParam, (ArrayList) defValue, func, unit);
                        }
                        break;

                        case DataType.UDT:
                            if (trnValue != null)                            
                                defValue = LoadUDTParamDefaultValue(parameter.Structure[0], trnValue, trnValue, false, unit);
                        break;

                        default:
                            if (trnValue != null)
                                defValue = GetDefaultValue(trnValue, parameter.Structure[0].ParamDataType);
                            else
                                defValue = 0;
                        break;
                    }
                    values.Add(defValue);
                }

                return;
            }

            //Call recursivly for the other dimmensions.
            for (int i = 0; i < trn.ChildCount; ++i)
                LoadArrayDimensionDefValue(dim, trn.GetChild(i), parameter, values, unit);
        }


        private void LoadArrayParam(Function curFunc, Parameter param, ITree trnType, Unit unit)
        {
            try
            {
                Parameter arrayOfParam = param.Structure[0];
                if (arrayOfParam.ParamDataType == DataType.UDT)
                {
                    Parameter udtParam = unit.SearchUDT(arrayOfParam.TypeName);
                    if (udtParam == null)
                    {
                        Function func = unit.SearchFunction(arrayOfParam.TypeName);
                        if( func == null)
                        {
                            string msg = String.Format(Messages.G1005, arrayOfParam.TypeName);
                            OutputMessage(GetPosString(trnType.GetChild(1)) + msg);
                            return;
                        }
                    }
                }

                LoadParameter(curFunc, arrayOfParam, arrayOfParam.ASTNode, unit);
            }
            catch (Exception ex)
            {//TODO: error message
                Console.WriteLine(ex);
                OutputMessage(GetPosString(trnType.GetChild(1)) + "Recursivly type definition.");
                return;
            }
        }

     
        private object LoadStructFBDefaultValue(Parameter param, ITree trnType, Function func, Unit unit)
        {
            ITree trnDefValue = trnType.Parent.GetChild(trnType.Parent.ChildCount - 1);

            if (trnDefValue.Type != Mpal.Parser.Parser.LRBRACKED)
                return null;

            ArrayList defValue = new ArrayList();
            LoadMemberDefaultValue(trnDefValue, param, defValue, func, unit);

            return defValue;
        }


        private void LoadMemberDefaultValue(ITree trnDefValueList, Parameter typeParam, ArrayList defValues, Function func, Unit unit)
        {
            for (int i = 0; i < trnDefValueList.ChildCount; ++i)
            {
                ITree trnElemInit = trnDefValueList.GetChild(i);
                ITree trnName = trnElemInit.GetChild(0);
                ITree trnValue = trnElemInit.GetChild(1);

                Parameter memberParam = null;

                if (func == null)
                    memberParam = typeParam.FindMember(trnName.Text, unit);
                else
                    memberParam = func.GetParameter(trnName.Text);

                if (memberParam == null)
                {
                    OutputMessage(GetPosString(trnElemInit.GetChild(0)) + String.Format(Messages.G1005, trnElemInit.GetChild(0).Text));
                    return;
                }

                if (func != null)
                {
                    if(memberParam.ParamAccess == Parameter.Access.Output)
                    {
                        OutputMessage(GetPosString(trnElemInit.GetChild(0)) + String.Format(Messages.G1010, memberParam.Name, func.Name));
                        return;
                    }

                    if (memberParam.ParamAccess == Parameter.Access.VarTemp ||
                        memberParam.ParamAccess == Parameter.Access.VarTempConst)
                    {
                        OutputMessage(GetPosString(trnElemInit.GetChild(0)) + String.Format(Messages.G1011, memberParam.Name, func.Name));
                        return;
                    }                        
                }

                DictionaryEntry entry = new DictionaryEntry();
                entry.Key = memberParam.Index;

                switch(memberParam.ParamDataType)
                {
                    case DataType.STRUCT:
                    {
                        ArrayList list = new ArrayList();                        
                        LoadMemberDefaultValue(trnElemInit.GetChild(1), memberParam, list, null, unit);
                        entry.Value = list;
                        defValues.Add(entry);
                    }
                    break;
                    
                    case DataType.ARRAY:
                        entry.Value = LoadArrayDefaultValue(memberParam, trnValue, unit);
                    break;
                    
                    case DataType.UDT:
                    case DataType.FB:
                        entry.Value = LoadUDTParamDefaultValue(memberParam, memberParam.ASTNode, trnValue, true, unit);
                    break;

                    default:
                        entry.Value = GetDefaultValue(trnValue, memberParam.ParamDataType);
                    break;
                }
                defValues.Add(entry);
            }
        }

        private static string GetPosString(ITree trn)
        {
            return " (" + trn.Line.ToString() + "," + trn.CharPositionInLine.ToString() + ")";
        }

        private object GetDefaultValue(ITree trnValue, DataType dataType)
        {
            NumberFormatInfo info = new NumberFormatInfo();
            info.NumberDecimalSeparator = ".";

            string text = trnValue.Text;
            object defValue = null;

            switch (trnValue.Type)
            {//Where is the value.
                case Mpal.Parser.Parser.NEG:
                case Mpal.Parser.Parser.PLUS:
                    text = trnValue.Text + trnValue.GetChild(0).Text;
                break;

                case Mpal.Parser.Parser.BINARY_INTEGER:
                case Mpal.Parser.Parser.OCTAL_INTEGER:
                case Mpal.Parser.Parser.HEX_INTEGER:
                {
                    string[] array = trnValue.Text.Split('#');
                    ulong no = Convert.ToUInt64(array[1], Convert.ToInt32(array[0]));
                    text = Convert.ToString(no);
                }
                break;

                case Mpal.Parser.Parser.INTEGER:
                case Mpal.Parser.Parser.REAL_CONSTANT:
                case Mpal.Parser.Parser.TRUE:
                case Mpal.Parser.Parser.FALSE:
                    text = trnValue.Text;
                break;                

            }
            
            switch (dataType)
            {
                case DataType.BOOL:
                    if (text == "TRUE" || text == "1")
                    {
                        defValue = true;
                    }
                    else if (text == "FALSE" || text == "0")
                    {
                        defValue = false;
                    }
                    else
                    {
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.SINT:
                    try
                    {
                        defValue = Convert.ToSByte(text);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.INT:
                    try
                    {
                        defValue = Convert.ToInt16(text);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.DINT:
                    try
                    {
                        defValue = Convert.ToInt32(text);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.LINT:
                    try
                    {
                        defValue = Convert.ToInt64(text);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.USINT:
                case DataType.BYTE:
                    try
                    {
                        defValue = Convert.ToByte(text);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.UINT:
                case DataType.WORD:
                    try
                    {
                        defValue = Convert.ToUInt16(text);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.UDINT:
                case DataType.DWORD:
                    try
                    {
                        defValue = Convert.ToUInt32(text);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.ULINT:
                case DataType.LWORD:
                    try
                    {
                        defValue = Convert.ToUInt64(text);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.REAL:
                    try
                    {
                        defValue = Convert.ToSingle(text, info);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
                case DataType.LREAL:
                    try
                    {
                        defValue = Convert.ToDouble(text, info);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        OutputMessage(GetPosString(trnValue) + String.Format(Messages.G1002, text));
                    }
                    break;
            }

            return defValue;
        }

        private void OutputMessage(string msg)
        {
            _error = true;

            if (OnMessage != null)
                OnMessage(msg);
        }
    }
}
