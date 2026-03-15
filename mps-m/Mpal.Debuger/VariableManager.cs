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
using System.Globalization;
using Mpal.Model;


namespace Mpal.Debugger
{
    public class VariableManager
    {
        private List<VariableInfo> _variables = new List<VariableInfo>();
        private Unit _unit = null;
        private Mpal.Debugger.Debugger _debugger;
        private Hashtable _uid2varMap = new Hashtable();
        private string _lastFuncName = "";

        public VariableManager(Mpal.Debugger.Debugger debugger, Unit unit)
        {
            _unit = unit;
            _debugger = debugger;
        }

        public VariableInfo GetValueByUID(ulong uid)
        {
            if (!_uid2varMap.Contains(uid))
                return null;

            return (VariableInfo)_uid2varMap[uid];
        }

        public void UpdateVariables(string fname)
        {
            if (!_unit.Functions.Contains(fname))
                return;

            Function func = (Function)_unit.Functions[fname];

            byte[] stackData = null;
            byte[] instanceData = null;

            switch (func.FuncType)
            {
                case Function.Type.PG:
                case Function.Type.FC:
                {
                    stackData = _debugger.ReadStackData(func);
                }
                break;
                case Function.Type.FB:
                {
                    stackData = _debugger.ReadStackData(func);
                    instanceData = _debugger.ReadInstanceData(func);
                }
                break;
            }

            if (_lastFuncName == fname)
                UpdateVariableInfo(func, stackData, instanceData);
            else
                CreateVariableInfo(func, stackData, instanceData);

            _lastFuncName = fname;
        }

        private void UpdateVariableInfo(Function func, byte[] stackData, byte[] instanceData)
        {
            try
            {
                for (int i = 0; i < _variables.Count; ++i)
                {
                    Parameter param = func.Parameters[i];
                    VariableInfo varInfo = _variables[i];
                    if (func.IsStackParam(param))
                    {
                        if (stackData == null)
                            return;

                        varInfo.Data = new byte[param.Size];
                        Array.Copy(stackData,param.Offset, varInfo.Data,0, param.Size);                        
                    }
                    else 
                    {
                        if (func.FuncType == Function.Type.FB)
                        {
                            if (instanceData == null)
                                return;

                            varInfo.Data = new byte[param.Size];
                            Array.Copy(instanceData, param.Offset, varInfo.Data, 0, param.Size);
                        }
                        else
                        {
                            varInfo.Data = _debugger.ReadMemoryByRef(param, func.Name);
                        }
                    }
                    
                    UpdateChildVariables(varInfo);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void CreateVariableInfo(Function func, byte[] stackData, byte[] instanceData)
        {
            _variables.Clear();
            _uid2varMap.Clear();
            
            foreach (Parameter param in func.Parameters)
            {
                VariableInfo varInfo = new VariableInfo();

                if (func.IsStackParam(param))
                {
                    if (stackData == null)
                        return;

                    varInfo.Data = new byte[param.Size];
                    Array.Copy(stackData, param.Offset, varInfo.Data, 0, param.Size);
                }
                else
                {
                    if (func.FuncType == Function.Type.FB)
                    {
                        if (instanceData == null)
                            return;

                        varInfo.Data = new byte[param.Size];
                        Array.Copy(instanceData, param.Offset, varInfo.Data, 0, param.Size);
                    }
                    else
                    {
                        varInfo.Data = _debugger.ReadMemoryByRef(param, func.Name);
                    }
                }

                varInfo.DataType = param.ParamDataType;
                varInfo.DataTypeName = GetDataTypeName(param);
                varInfo.EnumList = param.EnumList;
                varInfo.Name = param.Name;        
                varInfo.UID = param.UID;
                _uid2varMap.Add(param.UID, varInfo);

                CreateChildVariables(varInfo, param);
                _variables.Add(varInfo);
            }
            
            foreach (DictionaryEntry entry in func.Uid2Param)
            {
                ulong uid = (ulong)entry.Key;

                if (_uid2varMap.Contains(uid))
                    continue;

                StructVarUid uidMapping = (StructVarUid)entry.Value;
                try
                {
                    _uid2varMap[uid] = GetVariable(uidMapping, func.Uid2Param, func.Uid2Var);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }


        private VariableInfo GetVariable(StructVarUid uidMapping, Hashtable uid2Param, Hashtable uid2Var)
        {
            VariableInfo varInfo;
            Parameter param;

            if (uidMapping.UidLeft != 0)
            {
                StructVarUid varItem = (StructVarUid)uid2Param[uidMapping.UidLeft];

                if (varItem == null)
                {
                    param = (Parameter)uid2Var[uidMapping.UidLeft];
                    if( param == null)
                        throw new Exception("error");

                    varInfo = (VariableInfo)_uid2varMap[param.UID];
                }
                else
                {
                    varInfo = GetVariable(varItem, uid2Param, uid2Var);
                }

                if (varInfo == null)
                    throw new Exception("error");

                if (varInfo.DataType == DataType.UDT && varInfo.Variables.Count == 1)
                    varInfo = varInfo.Variables[0];

                foreach (VariableInfo var in varInfo.Variables)
                {
                    if (uidMapping.RightName == var.Name)
                        return var;
                }
            }

            param = (Parameter)uid2Var[uidMapping.UidRight];
            if( param == null)
                throw new Exception("error");

            VariableInfo vi =  (VariableInfo)_uid2varMap[param.UID];
            return vi;
        }

        private string GetDataTypeName(Parameter param)
        {
            if (param.ParamDataType != DataType.UDT || param.ParamDataType != DataType.FB)
                return param.ParamDataType.ToString();

            if (param.ParamDataType == DataType.FB)
            {
                Function fbFunction = (Function)_unit.Functions[param.TypeName];
                return fbFunction.Name;
            }

            if (param.ParamDataType == DataType.UDT)
            {
                Parameter udtParam = (Parameter)_unit.Types[param.TypeName];
                return udtParam.Name;
            }

            return "";            
        }

        public List<VariableInfo> Variables
        {
            get { return _variables; }
        }

        private void UpdateChildVariables(VariableInfo parentVar)
        {    
            foreach (VariableInfo varInfo in parentVar.Variables)
            {
                int j = 0;
                for (int i = varInfo.Offset; i < (varInfo.Data.Length + varInfo.Offset); ++i)
                {
                    varInfo.Data[j] = parentVar.Data[i];
                    ++j;
                }

                UpdateChildVariables(varInfo);
            }
        }

        private void CreateChildVariables(VariableInfo parentVar, Parameter parentPar)
        {
            switch (parentVar.DataType)
            {
                case DataType.STRUCT:
                {
                    int offset = 0;
                    foreach (Parameter param in parentPar.Structure)
                    {
                        VariableInfo varInfo = new VariableInfo();
                        
                        varInfo.Data = new byte[param.Size];
                        varInfo.DataType = param.ParamDataType;
                        varInfo.DataTypeName = GetDataTypeName(param);
                        varInfo.Name = param.Name;
                        varInfo.EnumList = param.EnumList;
                        varInfo.UID = param.UID;
                        varInfo.Offset = offset;
                        
                        int j = 0;
                        for (int i = offset; i < (param.Size + offset); ++i)
                        {
                            varInfo.Data[j] = parentVar.Data[i];                         
                            ++j;
                        }
                       

                        offset += (int) param.Size;
                        parentVar.Variables.Add(varInfo);
                        CreateChildVariables(varInfo, param);
                    }
                }
                break;
                case DataType.FB:
                {
                    Function fbFunction = (Function)_unit.Functions[parentPar.TypeName];

                    int offset = 0;
                    foreach (Parameter param in fbFunction.Parameters)
                    {
                        if (param.ParamAccess == Parameter.Access.VarTemp ||
                            param.ParamAccess == Parameter.Access.VarTempConst)
                            continue; //Not in instance is of the stack

                        VariableInfo varInfo = new VariableInfo();
                        varInfo.Data = new byte[param.Size];
                        varInfo.DataType = param.ParamDataType;
                        varInfo.DataTypeName = GetDataTypeName(param);
                        varInfo.Name = param.Name;
                        varInfo.EnumList = param.EnumList;
                        varInfo.UID = param.UID;
                        varInfo.Offset = offset;

                        int j = 0;
                        for (int i = offset; i < (param.Size + offset); ++i)
                        {
                            varInfo.Data[j] = parentVar.Data[i];
                            ++j;
                        }
                        
                        offset += (int)param.Size;
                        parentVar.Variables.Add(varInfo);
                        CreateChildVariables(varInfo, param);
                    }
                }
                break;

                case DataType.UDT:
                {
                    Parameter udtParam = (Parameter)_unit.Types[parentPar.TypeName];
                    
                    VariableInfo varInfo = new VariableInfo();
                    varInfo.Data = new byte[udtParam.Size];
                    varInfo.DataType = udtParam.ParamDataType;
                    varInfo.DataTypeName = GetDataTypeName(udtParam);
                    varInfo.Name = udtParam.Name;
                    varInfo.EnumList = udtParam.EnumList;
                    varInfo.UID = parentPar.UID;
                    varInfo.Offset = (int) udtParam.Offset;

                    int j = 0;
                    for (int i = (int)udtParam.Offset; i < (udtParam.Size + udtParam.Offset); ++i)
                    {
                        varInfo.Data[j] = parentVar.Data[i];
                        ++j;
                    }
                     
                    
                    parentVar.Variables.Add(varInfo);
                    CreateChildVariables(varInfo, udtParam);
                }
                break;

                case DataType.ARRAY:
                {
                    Parameter paramArr = parentPar;
                    Parameter paramArrOf = paramArr.Structure[0];
                    Dimension dim = paramArr.Dimensions[0];

                    for (long index = dim.From; index <= dim.To; ++index)
                    {
                        Parameter partParam;

                        if (paramArr.Dimensions.Count == 1)
                        {
                            partParam = new Parameter(paramArrOf);
                            partParam.Name = "[" + index.ToString() + "]";
                        }
                        else
                        {
                            partParam = new Parameter("[" + index.ToString() + "]", 0, DataType.ARRAY, Parameter.Access.None, "");
                            partParam.Structure.Add(paramArrOf);

                            for (int j = 1; j < paramArr.Dimensions.Count; ++j)
                                partParam.Dimensions.Add(dim);
                        }

                        VariableInfo varInfo = new VariableInfo();                        
                        varInfo.DataType = partParam.ParamDataType;
                        varInfo.Name = partParam.Name;
                        varInfo.EnumList = partParam.EnumList;
                        varInfo.DataTypeName = GetDataTypeName(paramArrOf);
                        varInfo.UID = parentPar.UID;
                        
                        uint size = 1;

                        for (int j = 1; j < paramArr.Dimensions.Count; ++j)
                        {
                            Dimension curDim  = paramArr.Dimensions[j];                       
                            size *= (uint) ((curDim.To - curDim.From) + 1);
                        }

                        size *= paramArrOf.Size;

                        int offset = (int)((index - dim.From) * size);
                        varInfo.Data = new byte[size];
                        varInfo.Offset = offset;

                        int p = 0;
                        for (int pos = offset; pos < (offset + size); ++pos)
                        {
                            varInfo.Data[p] = parentVar.Data[pos];
                            ++p;
                        }
                        
                        parentVar.Variables.Add(varInfo);
                        CreateChildVariables(varInfo, partParam);
                    }                    
                }
                break;
            }
        }
    }
}
