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
    internal class IfaceScaner
    {
        private uint _typeId = ((uint)DataType.TYPEID);
        private bool _error = false;
        public event Message OnMessage;
        private bool _showMessage = false;
        private CompilerOptions _options;

        public IfaceScaner()
        {
        }

        public bool Scan(Unit unit, ITree tree, bool showMessage, CompilerOptions options)
        {
            _showMessage = showMessage;
            _options = options;

            bool progAvailable = false;
            if( tree.IsNil)
            {
                for (int index = 0; index < tree.ChildCount; ++index)
                {
                    ITree trn = tree.GetChild(index);

                    switch (trn.Type)
                    {
                        case Mpal.Parser.Parser.TYPE:
                            ScanTypes(unit, trn);
                            break;


                        case Mpal.Parser.Parser.PROGRAM:
                            ScanFunction(unit, trn, Function.Type.PG);

                            if (progAvailable)
                                OutputMessage(Helper.GetPosString(trn) + Messages.D1003);

                            progAvailable = true;
                            break;

                        case Mpal.Parser.Parser.FUNCTION:
                            ScanFunction(unit, trn, Function.Type.FC);
                            break;

                        case Mpal.Parser.Parser.FUNCTION_BLOCK:
                            ScanFunction(unit, trn, Function.Type.FB);
                            break;
                    }
                }
            }
            else
            {
                ITree trn = tree;

                switch (trn.Type)
                {
                    case Mpal.Parser.Parser.TYPE:
                        ScanTypes(unit, trn);
                        break;


                    case Mpal.Parser.Parser.PROGRAM:
                        ScanFunction(unit, trn, Function.Type.PG);

                        if (progAvailable)
                            OutputMessage(Helper.GetPosString(trn) + Messages.D1003);

                        progAvailable = true;
                        break;

                    case Mpal.Parser.Parser.FUNCTION:
                        ScanFunction(unit, trn, Function.Type.FC);
                        break;

                    case Mpal.Parser.Parser.FUNCTION_BLOCK:
                        ScanFunction(unit, trn, Function.Type.FB);
                        break;
                }
            }
            return !_error;
        }

        private void LoadDependencies(Unit unit)
        {
        }

        private void ScanFunction(Unit unit, ITree trn, Function.Type type)
        {
            ITree trnFcName = trn.GetChild(0);
            string fcName = "";

            if (type == Function.Type.FC)
                fcName = trnFcName.GetChild(0).Text;
            else
                fcName = trnFcName.Text;

            Function function = new Function(fcName, type, unit);

            try
            {
                unit.Functions.Add(function.Name, function);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                OutputMessage(Helper.GetPosString(trn) + String.Format(Messages.D1002, function.Name));
            }

            function.LineBegin = trn.Line;
            ITree child = trn.GetChild(trn.ChildCount - 1);
            function.LineEnd = child.Line;

            function.ASTNode = trn;

            //Scan the constants
            for (int i = 0; i < trn.ChildCount; ++i)
            {
                ITree trnItem = trn.GetChild(i);

                switch (trnItem.Type)
                {
                    case Mpal.Parser.Parser.VAR:
                        if (trnItem.GetChild(0).Type == Mpal.Parser.Parser.CONSTANT)
                            ScanVariable(function, unit, trnItem, Parameter.Access.VarConst);
                        break;

                    case Mpal.Parser.Parser.VAR_TEMP:
                        if (trnItem.GetChild(0).Type == Mpal.Parser.Parser.CONSTANT)
                            ScanVariable(function, unit, trnItem, Parameter.Access.VarTempConst);
                        break;
                }

            }

            for (int i = 0; i < trn.ChildCount; ++i)
            {
                ITree trnItem = trn.GetChild(i);
                
                switch(trnItem.Type)
                {
                    case Mpal.Parser.Parser.VAR_INPUT:                        
                        ScanVariable(function, unit, trnItem, Parameter.Access.Input);
                    break;

                    case Mpal.Parser.Parser.VAR_OUTPUT:                        
                        ScanVariable(function, unit, trnItem, Parameter.Access.Output);
                    break;

                    case Mpal.Parser.Parser.VAR_IN_OUT:
                        ScanVariable(function, unit, trnItem, Parameter.Access.InOut);
                    break;

                    case Mpal.Parser.Parser.VAR:
                        if (trnItem.GetChild(0).Type != Mpal.Parser.Parser.CONSTANT)
                           ScanVariable(function, unit, trnItem, Parameter.Access.Var);
                    break;

                    case Mpal.Parser.Parser.VAR_TEMP:
                        if (trnItem.GetChild(0).Type != Mpal.Parser.Parser.CONSTANT)
                            ScanVariable(function, unit, trnItem, Parameter.Access.VarTemp);
                    break;
                }
                
            }

            //Has return value
            if (type == Function.Type.FC)
            {
                Parameter funcNameParam = function.GetParameter(trnFcName.GetChild(0).Text);
            
                if(funcNameParam != null)

                    OutputMessage(Helper.GetPosString(trnFcName.GetChild(0)) + String.Format(Messages.D1007, trnFcName.GetChild(0).Text));

                if (trnFcName.GetChild(1).Type != Mpal.Parser.Parser.VOID)
                {
                    Parameter parameter = ScanParameter(trnFcName.GetChild(0).Text, fcName, trnFcName.GetChild(1),Parameter.Access.Output, unit);
                    parameter.UID = Helper.GetUID(trnFcName.GetChild(0));
                    parameter.ASTNode = trnFcName.GetChild(1);
                    parameter.Index = function.Parameters.Count;

                    if (!function.AddVariable(parameter))
                        OutputMessage(Helper.GetPosString(trnFcName.GetChild(0)) + String.Format(Messages.D1007, trnFcName.GetChild(0).Text));
                }
            }


            //Set the parameter index in parent
            for(int index = 0; index < function.Parameters.Count; ++index)
            {
                Parameter param = function.Parameters[index];
                param.Index = index;
            }
        }

        private void ScanTypes(Unit unit, ITree trnItem)
        {
            for (int index = 0; index < trnItem.ChildCount; ++index)
            {
                ITree       trnTypeDef  = trnItem.GetChild(index);
                ITree       trnTypeName = trnTypeDef.GetChild(0);
                ITree       trnTypeType = trnTypeDef.GetChild(1);
                
                string      typeName    = trnTypeName.Text;

                Parameter param = ScanParameter(typeName, typeName, trnTypeType, Parameter.Access.Input, unit);
                param.UID = Helper.GetUID(trnTypeName);
                param.ASTNode = trnTypeDef;
                param.TypeId  = typeName;
                try
                {
                    unit.Types.Add(typeName, param);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    OutputMessage(Helper.GetPosString(trnTypeName) +  String.Format(Messages.D1001, typeName));
                }
            }
        }

        private long GetConstantValue(string cname, string fname, Unit unit)
        {
            Function func = (Function) unit.Functions[fname];
            foreach (Parameter param in func.Parameters)
            {
                if (param.ParamAccess == Parameter.Access.VarConst ||
                   param.ParamAccess == Parameter.Access.VarTempConst)
                {
                    if (param.Name == cname && param.DefaultValue != null)
                        return Convert.ToInt64(param.DefaultValue);
                }
            }

            throw new Exception("Wrong index type");
        }

        private long GetRangeIndex(ITree trn, string baseName, Unit unit)
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

                case Mpal.Parser.Parser.INTEGER:
                    index = Convert.ToInt64(trn.Text);
                    break;

                case Mpal.Parser.Parser.IDENTIFIER:
                    return GetConstantValue(trn.Text, baseName, unit);
            }

            return index;
        }

        private Parameter ScanArrayParam(string name, string baseName, ITree trnVarType, Parameter.Access access, Unit unit)
        {           
            Parameter parameter = new Parameter(name, Helper.GetUID(trnVarType), DataType.ARRAY, access, baseName+ "." +(++_typeId).ToString());
            parameter.ASTNode = trnVarType;
            
            //Loads the type of array.
            ITree trnArrayType = trnVarType.GetChild(1);
            Parameter arrayTypeParam = ScanParameter("", baseName, trnArrayType, Parameter.Access.Input, unit);
            arrayTypeParam.ASTNode = trnArrayType;

            parameter.Structure.Add(arrayTypeParam);


            List<long> dimLenght = new List<long>();
            ITree trnDimensions = trnVarType.GetChild(0);
  
            //Loads the dimensions.
            for (int i = 0; i < trnDimensions.ChildCount; ++i)
            {
                ITree trnDim = trnDimensions.GetChild(i);

                long from = 0;
                long to = 0;

                ITree trnFrom = trnDim.GetChild(0);
                ITree trnTo = trnDim.GetChild(1);

                try
                {
                    from = GetRangeIndex(trnFrom, baseName, unit);
                    to = GetRangeIndex(trnTo, baseName, unit);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    OutputMessage(Helper.GetPosString(trnVarType) + String.Format(Messages.G1007, trnVarType.Text));
                    return parameter;
                }

                if (to <= from)
                    OutputMessage(Helper.GetPosString(trnVarType) + String.Format(Messages.G1007, trnVarType.Text));

                dimLenght.Add(Math.Abs(to - from) + 1);

                parameter.Dimensions.Add(new Dimension((int)from, (int)to));
            }
            return parameter;
        }
      
        private Parameter LoadStringParam(string name, ITree trnVarType,Parameter.Access access)
        {
            uint len = 255;
            object defValue = null;
            string strDefVal = "";

            if (trnVarType.ChildCount == 1)
            {
                ITree child = trnVarType.GetChild(0);
                if (child.Type != Mpal.Parser.Parser.STRING_LITERAL)
                {
                    len = Convert.ToUInt32(child.Text);
                }
                else
                {
                    strDefVal = child.Text.Remove(0, 1);
                    strDefVal = strDefVal.Remove(strDefVal.Length - 1, 1);
                    defValue = strDefVal;
                    len = (uint)strDefVal.Length;
                }
            }
            else if (trnVarType.ChildCount == 2)
            {
                ITree trnLen = trnVarType.GetChild(0);
                ITree trnDefVal = trnVarType.GetChild(1);
                try
                {
                    len = Convert.ToUInt32(trnLen.Text);
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }

                strDefVal = trnDefVal.Text.Remove(0, 1);
                strDefVal = strDefVal.Remove(strDefVal.Length - 1, 1);
                defValue = strDefVal;
            }

            //Check lenght and default value lenght.
            if (strDefVal.Length > len)
                OutputMessage(Helper.GetPosString(trnVarType) + String.Format(Messages.D1006, trnVarType.Text));

            string typeId = DataType.STRING.ToString() + len.ToString();
            Parameter parameter = new Parameter(name, Helper.GetUID(trnVarType),DataType.STRING, access, typeId);
            parameter.Size = len;
            return parameter;
        }

        private Parameter ScanParameter(string name, string baseName, ITree trnVarType, Parameter.Access access, Unit unit)
        {
            switch (trnVarType.Type)
            {
                case Mpal.Parser.Parser.BOOL:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.BOOL, access, DataType.BOOL.ToString());

                case Mpal.Parser.Parser.SINT:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.SINT, access, DataType.SINT.ToString());                

                case Mpal.Parser.Parser.INT:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.INT, access, DataType.INT.ToString());

                case Mpal.Parser.Parser.DINT:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.DINT, access, DataType.DINT.ToString());

                case Mpal.Parser.Parser.LINT:
                    {
                        if (!_options.SupportINT64)
                        {
                            OutputMessage(Helper.GetPosString(trnVarType) + String.Format(Messages.G1015, trnVarType.Text));
                        }

                        return new Parameter(name, Helper.GetUID(trnVarType), DataType.LINT, access, DataType.LINT.ToString());
                    }

                case Mpal.Parser.Parser.USINT:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.USINT, access, DataType.USINT.ToString());

                case Mpal.Parser.Parser.UINT:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.UINT, access, DataType.UINT.ToString());                    

                case Mpal.Parser.Parser.UDINT:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.UDINT, access, DataType.UDINT.ToString());

                case Mpal.Parser.Parser.ULINT:
                    {
                        if (!_options.SupportINT64)
                        {
                            OutputMessage(Helper.GetPosString(trnVarType) + String.Format(Messages.G1015, trnVarType.Text));
                        }
                        return new Parameter(name, Helper.GetUID(trnVarType), DataType.ULINT, access, DataType.ULINT.ToString());
                    }

                case Mpal.Parser.Parser.REAL:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.REAL, access, DataType.REAL.ToString());

                case Mpal.Parser.Parser.LREAL:
                    {
                        if (!_options.SupportLREAL)
                        {
                            OutputMessage(Helper.GetPosString(trnVarType) + String.Format(Messages.G1015, trnVarType.Text));
                        }

                        return new Parameter(name, Helper.GetUID(trnVarType), DataType.LREAL, access, DataType.LREAL.ToString());
                    }

                case Mpal.Parser.Parser.BYTE:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.BYTE, access, DataType.BYTE.ToString());
                
                case Mpal.Parser.Parser.WORD:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.WORD, access, DataType.WORD.ToString());

                case Mpal.Parser.Parser.DWORD:
                    return new Parameter(name, Helper.GetUID(trnVarType), DataType.DWORD, access, DataType.DWORD.ToString());

                case Mpal.Parser.Parser.LWORD:
                    {
                        if (!_options.SupportINT64)
                        {
                            OutputMessage(Helper.GetPosString(trnVarType) + String.Format(Messages.G1015, trnVarType.Text));
                        }
                        return new Parameter(name, Helper.GetUID(trnVarType), DataType.LWORD, access, DataType.LWORD.ToString());
                    }

                case Mpal.Parser.Parser.STRING:
                    return LoadStringParam(name, trnVarType,  access);

                case Mpal.Parser.Parser.WSTRING:
                    throw new Exception("WSTRING Not implemented yet.");

                case Mpal.Parser.Parser.STRUCT:
                    return ScanStructParam(name, baseName,  trnVarType, access, unit);

                case Mpal.Parser.Parser.ARRAY:
                    return ScanArrayParam(name, baseName,  trnVarType, access, unit);

                case Mpal.Parser.Parser.LRBRACKED:
                    return ScanEnum(name, baseName, trnVarType, access, unit);

                default: //User defined type uninitialized.
                    return ScanUDT(name, baseName, trnVarType, access, unit);
            }
        }

        private Parameter ScanEnum(string name, string unitName, ITree trnVarType, Parameter.Access access, Unit unit)
        {
            Parameter parameter = new Parameter(name, Helper.GetUID(trnVarType), DataType.ENUM, access, unitName + "." + (++_typeId).ToString());

            for (int i = 0; i < trnVarType.ChildCount; ++i)
            {
                ITree itemNode = trnVarType.GetChild(i);
                if (itemNode.Type == Mpal.Parser.Parser.ASSIGN)
                {
                    for (int pos = 0; pos < parameter.EnumList.Count; ++pos)
                    {
                        if (parameter.EnumList[pos] == itemNode.GetChild(0).Text)
                        {
                            parameter.DefaultValue = pos;
                            break;
                        }
                    }

                    if (parameter.DefaultValue == null)
                    {
                        OutputMessage(Helper.GetPosString(itemNode.GetChild(0)) + String.Format(Messages.G1005, itemNode.GetChild(0).Text));
                        return parameter;
                    }
                }
                else
                {
                    //Check current enum
                    foreach (string str in parameter.EnumList)
                    {
                        if (str == itemNode.Text)
                        {
                            OutputMessage(Helper.GetPosString(itemNode) + String.Format(Messages.D1008, itemNode.Text));
                            return parameter;
                        }
                    }
                    //check the other enums in the unit
                    foreach (DictionaryEntry entry in unit.Types)
                    {
                        Parameter udtParam = (Parameter) entry.Value;
                        
                        if (udtParam.ParamDataType != DataType.ENUM)
                            continue;

                        foreach (string str in udtParam.EnumList)
                        {
                            if (str == itemNode.Text)
                            {
                                OutputMessage(Helper.GetPosString(itemNode) + String.Format(Messages.D1008, itemNode.Text));
                                return parameter;
                            }
                        }
                    }

                    
                    parameter.EnumList.Add(itemNode.Text);
                }
            }
            return parameter;
        }

        private Parameter ScanStructParam(string name, string baseName, ITree trnVarType, Parameter.Access access, Unit unit)
        {
            Parameter parameter = new Parameter(name, Helper.GetUID(trnVarType), DataType.STRUCT, access, baseName + "." + (++_typeId).ToString());
            int newIndex = 0;

            for (int i = 0; i < trnVarType.ChildCount; ++i)
            {
                ITree trn = trnVarType.GetChild(i);

                int count = trn.ChildCount - 1;
                ITree trnType = trn.GetChild(count);

                if (trnType.Type == Mpal.Parser.Parser.LBRACKED ||
                    trnType.Type == Mpal.Parser.Parser.LRBRACKED)
                {
                    count = trn.ChildCount - 2;
                    trnType = trn.GetChild(count);
                }

                for (int j = 0; j < count; ++j)
                {
                    ITree trnName = trn.GetChild(j);
                    Parameter structParam = ScanParameter(trnName.Text, baseName, trnType,Parameter.Access.Input, unit);
                    structParam.UID = Helper.GetUID(trnName);
                    structParam.ASTNode = trnType;
                    structParam.Index = newIndex;

                    if (!parameter.AddMember(structParam))
                        OutputMessage(Helper.GetPosString(trnName) + String.Format(Messages.D1005, structParam.Name, name));

                      newIndex++;
                }
            }
            
            return parameter;
        }

        private Parameter ScanUDT(string name, string unitName, ITree trnVarType, Parameter.Access access, Unit unit)
        {
            Parameter udtParam = unit.SearchUDT(trnVarType.Text);
            Parameter param;

            if (udtParam == null)
            {                
                param = new Parameter(name, Helper.GetUID(trnVarType), access, "", trnVarType.Text);
            }
            else
            {
                param = new Parameter(name, Helper.GetUID(trnVarType), access, udtParam.TypeId, udtParam.Name);
            }

            param.ASTNode = trnVarType;
            return param;
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
                        return null;
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
                        return null;
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
                        return null;
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
                        return null;
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
                        return null;
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
                        return null;
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
                        return null;
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
                        return null;
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
                        return null;
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
                        return null;
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
                        return null;
                    }
                    break;
            }

            return defValue;
        }

        private object TryGetDefValue(ITree trnVarType, DataType dataType)
        {
            ITree trnDefVal = trnVarType.GetChild(0);

            if (trnDefVal.Type == Mpal.Parser.Parser.SHARP)
                trnDefVal = trnDefVal.GetChild(1);

            return GetDefaultValue(trnDefVal, dataType);
        }

        private void ScanVariable(Function function, Unit unit, ITree trn, Parameter.Access access)
        {
            string unitName = unit.Name;
            int i = 0;

            if (access == Parameter.Access.VarConst || access == Parameter.Access.VarTempConst)
                i = 1;

            for( ; i < trn.ChildCount; ++i)
            {
                ITree trnVarDef = trn.GetChild(i);
                string lastTypeId = "";

                ITree trnType = trnVarDef.GetChild(trnVarDef.ChildCount - 1);
                int count = trnVarDef.ChildCount - 1;

                if (trnType.Type == Mpal.Parser.Parser.LBRACKED || trnType.Type == Mpal.Parser.Parser.LRBRACKED)
                {
                    count = trnVarDef.ChildCount - 2;
                    trnType = trnVarDef.GetChild(count);
                }

                for (int j = 0; j < count; ++j)
                {
                    ITree trnName = trnVarDef.GetChild(j);

                    Parameter parameter = ScanParameter(trnName.Text, function.Name, trnType, access, unit);                    
                    parameter.ASTNode = trnType;
                    parameter.UID = Helper.GetUID(trnName);
                    
                    if (access == Parameter.Access.VarConst || access == Parameter.Access.VarTempConst)
                        parameter.DefaultValue = TryGetDefValue(trnType, parameter.ParamDataType);

                    if (!function.AddVariable(parameter))
                        OutputMessage(Helper.GetPosString(trnName) + String.Format(Messages.D1004, trnName.Text));

                    if (lastTypeId != "")
                        parameter.TypeId = lastTypeId;
                    else
                        lastTypeId = parameter.TypeId;                    
                }
            }
        }

        private void OutputMessage(string msg)
        {
            _error = true;

            if (OnMessage != null && _showMessage)
                OnMessage(msg);
        }
    }
}
