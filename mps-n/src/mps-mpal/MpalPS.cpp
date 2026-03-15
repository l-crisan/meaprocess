/*
 * Copyright (C) 2015 by Laurentiu-Gheorghe Crisan
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * As a special exception, you may use this file as part of a free
 * software library without restriction. Specifically, if other files
 * instantiate templates or use macros or inline functions from this
 * file, or you compile this file and link it with other files to
 * produce an executable, this file does not by itself cause the
 * resulting executable to be covered by the GNU General Public
 * License. This exception does not however invalidate any other
 * reasons why the executable file might be covered by the GNU Library
 * General Public License.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
 */
#include "MpalPS.h"
#include <Pt/Any.h>
#include <Pt/System/Clock.h>
#include <mps/core/Sources.h>
#include <mps/core/SignalList.h>
#include <mps/core/Message.h>
#include <mps/core/FactorOffsetSignalScaling.h>
#include <sstream>

namespace mps{
namespace mpal{
    
int toInt(const std::string& str)
{
    int i;
    std::stringstream ss;
    ss << str;
    ss >> i;
    return i;
}

MpalPS::MpalPS()
: _sig2VarMap()
, _inSig2VarMap()
, _outSig2VarMap()
, _sigProp2Var()
, _programData()
, _programInfo(0)
, _vm(1024*10)
, _debugger(0)
, _triggerSignalID(0)
, _outputData()
, _varDefValues()
, _runInDebugger(false)
, _debuggerIP("")
, _debuggerPort(0)
, _memSize(10)
, _errorState(false)
{
    registerProperty("sigVarMap", *this, &MpalPS::varSigMapping, &MpalPS::addVarSigMapping);
    registerProperty("sigDefValue", *this, &MpalPS::varSigMapping, &MpalPS::addSigDefValue);
    registerProperty("program", *this, &MpalPS::program, &MpalPS::setProgram);
    registerProperty("triggerSigID",*this, &MpalPS::triggerSigID, &MpalPS::setTriggerSigID);
    registerProperty("sigPropVarMap", *this, &MpalPS::sigPropVarMapping, &MpalPS::addSigPropVarMapping);
    registerProperty("runInDebugger", *this, &MpalPS::runInDebugger, &MpalPS::setRunInDebugger);
    registerProperty("debuggerIP", *this, &MpalPS::debuggerIP, &MpalPS::setDebuggerIP);
    registerProperty("debuggerPort", *this, &MpalPS::debuggerPort, &MpalPS::setDebuggerPort);
    registerProperty("vmMemSize", *this, &MpalPS::vmMemSize, &MpalPS::setVmMemSize);
}

MpalPS::~MpalPS()
{
}

void MpalPS::setProgram(const std::string& pr)
{
    base64Decode(pr, _programData);	
}

void MpalPS::addSigDefValue(const std::string& mapping)
{
    std::stringstream ss(mapping);

    char buffer[500];
    
    std::vector<std::string> mappingArray;

    while( ss.getline(buffer, 500, '/'))
        mappingArray.push_back(std::string(buffer));

    VariableDefValue varDefVal;

    varDefVal.strValue = mappingArray[0];

    Pt::uint32_t index = 0;

    for( Pt::uint32_t i = 1; i < mappingArray.size() -1; ++i)
    {
        std::stringstream s(mappingArray[i]);
        s.getline(buffer,500,'#');

        index = toInt(buffer);
        varDefVal.path.push_back((Pt::uint32_t)index);
    }

    _varDefValues.push_back(varDefVal);
}

void MpalPS::addSigPropVarMapping(const std::string& mapping)
{
    std::stringstream ss(mapping);

    char buffer[500];
    
    std::vector<std::string> mappingArray;

    while( ss.getline(buffer, 500, '/'))
        mappingArray.push_back(std::string(buffer));

    std::stringstream sl(mappingArray[0]);

    sl.getline(buffer,500,'~');
    std::string sigIdStr = buffer;
    
    sl.getline(buffer,500,'~');
    std::string propTypeStr = buffer;

    Pt::uint32_t sigId;
    
    sigId = toInt(sigIdStr);
    int propType = toInt(propTypeStr);
    
    VariableInfo variableInfo;
    Pt::uint32_t index = 0;
    
    variableInfo.propType = propType;
    variableInfo.sigId = sigId;

    for( Pt::uint32_t i = 1; i < mappingArray.size(); ++i)
    {
        if( i == (mappingArray.size() - 1) )
        {
            variableInfo.scaled = (mappingArray[i] == "s");
            continue;
        }

        std::stringstream s(mappingArray[i]);
        s.getline(buffer,500,'#');

        index = toInt(buffer);
        variableInfo.path.push_back((Pt::uint32_t)index);
    }

    _sigProp2Var.push_back(variableInfo);
}

void MpalPS::addVarSigMapping(const std::string& mapping)
{
    std::stringstream ss(mapping);

    char buffer[500];
    
    std::vector<std::string> mappingArray;

    while( ss.getline(buffer, 500, '/'))
        mappingArray.push_back(std::string(buffer));

    Pt::uint32_t sigId;
    sigId = toInt(mappingArray[0]);

    VariableInfo variableInfo;
    Pt::uint32_t index = 0;
    variableInfo.propType = -1;

    for( Pt::uint32_t i = 1; i < mappingArray.size(); ++i)
    {
        if( i == (mappingArray.size() - 1) )
        {
            variableInfo.scaled = (mappingArray[i] == "s");
            continue;
        }

        std::stringstream s(mappingArray[i]);
        s.getline(buffer,500,'#');

        index = toInt(buffer);
        variableInfo.path.push_back((Pt::uint32_t)index);
    }

    Sig2VarMapIt it = _sig2VarMap.find(sigId);
    if(it == _sig2VarMap.end())
    {
        std::vector<VariableInfo> varInfos;
        varInfos.push_back(variableInfo);
        _sig2VarMap[sigId] = varInfos;
    }
    else
    {
        it->second.push_back(variableInfo);
    }
}

void MpalPS::onInitInstance()
{
    ProcessStation::onInitInstance();

    _vm.setVmMemory(_memSize * 1024);
    _errorState = false;

    //Load the program info.
    std::stringstream ss;
    ss.write((const char*)&_programData[0], _programData.size());

    if( _runInDebugger != 0)
    {
        setSynchronTimer(false);

        _debugger = new ::mpal::vm::Debugger(_vm);
        try
        {
            _debugger->setup(_debuggerIP, _debuggerPort);
            _programInfo = &_debugger->load(ss);
        }
        catch(const std::exception& ex)
        {
            _debugger->close();
            delete _debugger;
            _debugger = 0;

            _programInfo = &_vm.load(ss);
            _runInDebugger = 0;
            _errorState = true;
            std::cerr << ex.what() << std::endl;
        }
    }
    else
    {
        _programInfo = &_vm.load(ss);	
    }
    
    //Setup the execution trigger.    
    const mps::core::Signal* triggerSig  = getSignalByID(_triggerSignalID);	
    const mps::core::Port* triggerPort   = getPortBySignal(triggerSig);
    _executionRate                       = triggerSig->sampleRate();
    _triggerSource                       = triggerPort->sourceIndex(triggerPort->signalList()->getSignalIndex(triggerSig));
    _triggerPort                         = triggerPort->portNumber();

    //Setup the in/out signal to variable map.
    Sig2VarMapIt it = _sig2VarMap.begin();

    double maxRate = 0;
    Pt::uint32_t sigIndex = 0;
    std::vector<double> signalRates;
    std::vector<Pt::uint32_t> itemsSizes;

    for( ; it != _sig2VarMap.end(); ++it)
    {
        const mps::core::Signal* signal = getSignalByID(it->first);
        
        const mps::core::Port* curPort = getPortBySignal(signal, true);
        bool input = true;

        if( curPort == 0 )
        {
            curPort = getPortBySignal(signal, false);
            input = false;
        }

        for(Pt::uint32_t i = 0; i < it->second.size(); ++i)
        {
            it->second[i].signal = signal;
            it->second[i].port = curPort;
            const Pt::uint32_t sigIndexInList = curPort->signalList()->getSignalIndex(signal);
            it->second[i].offset = curPort->signalOffsetInSource(sigIndexInList);
            it->second[i].sourceIndex = curPort->sourceIndex(sigIndexInList);
            
            if( input)
            {
                maxRate = std::max(maxRate, signal->sampleRate());

                std::map<Pt::uint32_t,Pt::uint32_t>::iterator sig2IdxIt = _sig2IndexMap.find(signal->signalID());

                if( sig2IdxIt == _sig2IndexMap.end())
                {
                    std::pair<Pt::uint32_t,Pt::uint32_t> pair(signal->signalID(), sigIndex);
                    _sig2IndexMap.insert(pair),
                    signalRates.push_back(signal->sampleRate());
                    itemsSizes.push_back(signal->valueSize());
                    sigIndex++;
                }
                Sig2VarMapIt inIt = _inSig2VarMap.find(it->first);

                if( inIt == _inSig2VarMap.end())
                {                
                    std::vector<VariableInfo> inVars;
                    inVars.push_back(it->second[i]);
                    _inSig2VarMap[it->first] = inVars;
                }
                else
                {
                    inIt->second.push_back(it->second[i]);
                }
            }
            else
            {
                _outSig2VarMap[it->first] = it->second[i];
            }
        }
    }
    
    _inputBuffer.init(static_cast<Pt::uint32_t>(maxRate+5), itemsSizes, signalRates, _executionRate);

    for( Pt::uint32_t i = 0; i < _sigProp2Var.size(); ++i)
    {
        VariableInfo& varInfo = _sigProp2Var[i];

        const mps::core::Signal* signal = getSignalByID(varInfo.sigId);
        
        varInfo.signal = signal;
        
        const mps::core::Port* curPort = getPortBySignal(signal, true);	

        varInfo.port = curPort;		
        Pt::uint32_t sigIndex = curPort->signalList()->getSignalIndex(signal);
        varInfo.offset = curPort->signalOffsetInSource(sigIndex);
        varInfo.sourceIndex = curPort->sourceIndex(sigIndex);
    }
        

    if( _outputPorts->size() != 0 )
    {
        const mps::core::Port* port = _outputPorts->at(0);

        const mps::core::Sources&  sources = port->sources();
        for( Pt::uint32_t i = 0; i < sources.size(); ++i)
        {
            const std::vector<mps::core::Signal*>& source = sources[i];
            const mps::core::Signal* signal  = source[0];
            _outputRecordSize.push_back(port->sourceDataSize(i));
            std::vector<Pt::uint8_t> outData;
            outData.resize(static_cast<Pt::uint32_t>(_outputRecordSize[i]*signal->sampleRate()));
            _outputData.push_back(outData);
        }
    }
}


Pt::uint8_t* MpalPS::getValueFromString(const std::string& strValue, ::mpal::vm::Variable* var)
{
    std::stringstream ss;

    ss<<strValue;

    switch(var->type())
    {
        case ::mpal::vm::BOOL:
        {
            Pt::uint8_t* value = new Pt::uint8_t;

            if( strValue == "TRUE")
                *value = 1;
            else
                *value = 0;
            
            return value;
        }
        break;
        case ::mpal::vm::SINT:
        {
            Pt::int8_t* value = new Pt::int8_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::INT:
        {
            Pt::int16_t* value = new Pt::int16_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::DINT:
        case ::mpal::vm::ENUM:
        {
            Pt::int32_t* value = new Pt::int32_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::LINT:
        {
            Pt::int64_t* value = new Pt::int64_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::USINT:
        {
            Pt::uint8_t* value = new Pt::uint8_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::UINT:
        {
            Pt::uint16_t* value = new Pt::uint16_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::UDINT:
        {
            Pt::uint32_t* value = new Pt::uint32_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::ULINT:
        {
            Pt::uint64_t* value = new Pt::uint64_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::REAL:
        {
            float* value = new float;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::LREAL:
        {
            double* value = new double;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::BYTE:
        {
            Pt::uint8_t* value = new Pt::uint8_t;
            ss>>*value;
            return value;
        }
        break;
        case ::mpal::vm::WORD:
        {
            Pt::uint16_t* value = new Pt::uint16_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::DWORD:
        {
            Pt::uint32_t* value = new Pt::uint32_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case ::mpal::vm::LWORD:
        {
            Pt::uint64_t* value = new Pt::uint64_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        default:
            return 0;
        break;
    }

    return 0;
}

const mps::core::Signal* MpalPS::getSignalByID(Pt::uint32_t id)
{
    for( Pt::uint32_t i = 0; i < _inputPorts->size(); ++i)
    {
        const mps::core::Port* port = _inputPorts->at(i);
        
        for( Pt::uint32_t sigIdx = 0; sigIdx < port->signalList()->size(); ++sigIdx)
        {
            const mps::core::Signal* signal = port->signalList()->at(sigIdx);

            if( signal->signalID() == id)
                return signal;
        }
    }

    for( Pt::uint32_t i = 0; i < _outputPorts->size(); ++i)
    {
        const mps::core::Port* port = _outputPorts->at(i);
        
        for( Pt::uint32_t sigIdx = 0; sigIdx < port->signalList()->size(); ++sigIdx)
        {
            const mps::core::Signal* signal = port->signalList()->at(sigIdx);

            if( signal->signalID() == id)
                return signal;
        }
    }

    return 0;
}

const mps::core::Port* MpalPS::getPortBySignal(const mps::core::Signal* inSignal, bool input)
{
    Pt::uint32_t size = 0;

    if(input)
        size = _inputPorts->size();
    else
        size = _outputPorts->size();

    const mps::core::Port* port = 0;

    for( Pt::uint32_t portIdx = 0; portIdx < size; ++portIdx)
    {
        if( input)
            port = _inputPorts->at(portIdx);
        else
            port = _outputPorts->at(portIdx);

        const mps::core::SignalList* sigList = port->signalList();

        for( Pt::uint32_t sigIdx = 0; sigIdx < sigList->size(); ++sigIdx)
        {
            const mps::core::Signal* signal = sigList->at(sigIdx);

            if( signal->signalID() == inSignal->signalID())
                return port;
        }
    }

    return 0;
}

void MpalPS::onInitialize()
{
    ProcessStation::onInitialize();
    
    if(_errorState)
    {
        
        std::stringstream ss;
        std::string strport;
        ss<<_debuggerPort;
        ss>>strport;
        mps::core::Message message( format(translate("Mp.Mpal.Err.ListenOnPort"),_debuggerIP, strport), mps::core::Message::Output, mps::core::Message::Warning,
                         Pt::System::Clock::getLocalTime());

        sendMessage( message );
        _errorState = false;
    }

    //Input variables default values.
    for( Pt::uint32_t i = 0; i < _varDefValues.size(); ++i)
    {
        VariableDefValue& varDefVal = _varDefValues[i];
        ::mpal::vm::Variable* var = _programInfo->getVariable(varDefVal.path);
        varDefVal.size = var->size();
        
        std::string value;

        if( isProperty(varDefVal.strValue.c_str()))
            value = getPropertyValueFromKey(varDefVal.strValue.c_str());
        else
            value = varDefVal.strValue;

        varDefVal.valueBuffer = getValueFromString(value, var);
    }
}

void MpalPS::onStart()
{
    _programInfo->init();
    
    //Copy user var section default values.
    for( Pt::uint32_t i = 0; i < _varDefValues.size(); ++i)
    {
        VariableDefValue& varDefVal = _varDefValues[i];
        void* varMem = _programInfo->variableValue(varDefVal.path);
        memcpy(varMem, varDefVal.valueBuffer, varDefVal.size);
    }

    
    for(Pt::uint32_t i = 0; i < _sigProp2Var.size(); ++i)
    {
        VariableInfo& varInfo = _sigProp2Var[i];
        ::mpal::vm::Variable* var = _programInfo->getVariable(varInfo.path);
        Pt::uint8_t* varMem = (Pt::uint8_t*) _programInfo->variableValue(varInfo.path);
        const mps::core::Signal* signal = varInfo.signal;
        double defValue = 0;

        switch(varInfo.propType)
        {
            case SampleRate:
                defValue = signal->sampleRate();
            break;
            case Minimum:
                defValue = signal->physMin();
            break;
            case Maximum:
                defValue = signal->physMax();
            break;
            case Factor:
            {
                const mps::core::FactorOffsetSignalScaling* scaling = (const mps::core::FactorOffsetSignalScaling*) signal->scaling();
                if( scaling != 0)
                    defValue = scaling->getFactor();
            }
            break;
            case Offset:
            {
                const mps::core::FactorOffsetSignalScaling* scaling = (const  mps::core::FactorOffsetSignalScaling*) signal->scaling();
                if( scaling != 0)
                    defValue = scaling->getOffset();
            }
            break;
            default:
            break;
        }

        castAndCopyData(varMem, (Pt::uint8_t*) &defValue, var->type(), mps::core::SignalDataType::VT_real64);
    }

    _inputBuffer.reset();

    ProcessStation::onStart();
}

void MpalPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const std::vector<mps::core::Signal*>& signals = port->sources().at(sourceIdx);

    for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
    {
        for( Pt::uint32_t sig =0; sig < signals.size(); ++sig)
        {
            const mps::core::Signal* signal = signals[sig];
            const Pt::uint32_t recordSize = port->sourceDataSize(sourceIdx);
            std::map<Pt::uint32_t,Pt::uint32_t>::iterator it = _sig2IndexMap.find(signal->signalID());
             
            if(it == _sig2IndexMap.end())
                 continue;
            
            const Pt::uint32_t offset = port->signalOffsetInSource(sourceIdx,sig);
            const Pt::uint8_t* ptrdata = (data + (rec * recordSize) + offset);
            _inputBuffer.insert(ptrdata, it->second);
        }
    }

    execute(port->portNumber(), sourceIdx, noOfRecords);
}


void MpalPS::castAndCopyData(Pt::uint8_t* to, const Pt::uint8_t* from, ::mpal::vm::DataType toType, mps::core::SignalDataType::Type fromType)
{	
    switch(fromType)
    {
        case mps::core::SignalDataType::VT_bool:
            castFrom<Pt::uint8_t>(to,from, toType);
        break;

        case mps::core::SignalDataType::VT_real64:
            castFrom<double>(to, from, toType);
        break;

        case mps::core::SignalDataType::VT_real32:
            castFrom<float>(to, from, toType);
        break;

        case mps::core::SignalDataType::VT_uint8_t:
            castFrom<Pt::uint8_t>(to, from, toType);
        break;

        case mps::core::SignalDataType::VT_int8_t:
            castFrom<Pt::int8_t>(to, from, toType);
        break;

        case mps::core::SignalDataType::VT_uint16_t:
            castFrom<Pt::uint16_t>(to, from, toType);
        break;

        case mps::core::SignalDataType::VT_int16_t:
            castFrom<Pt::int16_t>(to, from, toType);
        break;

        case mps::core::SignalDataType::VT_uint32_t:
            castFrom<Pt::uint32_t>(to, from, toType);
        break;

        case mps::core::SignalDataType::VT_int32_t:
            castFrom<Pt::uint32_t>(to, from, toType);
        break;

        case mps::core::SignalDataType::VT_uint64_t:
            castFrom<Pt::uint64_t>(to, from, toType);
        break;

        case mps::core::SignalDataType::VT_int64_t:
            castFrom<Pt::int64_t>(to, from, toType);
        break;
        default:
        break;
    }
}

void MpalPS::execute(Pt::uint32_t port, Pt::uint32_t sourceIdx, Pt::uint32_t noOfRecords)
{
    if( _inputBuffer.elementSize() != 0)
    {
        const Pt::uint8_t* data1;
        Pt::uint32_t data1count = 0;

        const Pt::uint8_t* data2;
        Pt::uint32_t data2count = 0;

        _inputBuffer.get(&data1, data1count, &data2, data2count);

        if( data1count != 0)
        {
            initAndExecute(data1, data1count);

            mps::core::Port* port = _outputPorts->at(0);
            for( Pt::uint32_t i = 0; i < _outputData.size(); ++i)
                port->onUpdateDataValue(data1count, i, &_outputData[i][0]);
        }

        if( data2count != 0)
        {
            initAndExecute(data2,data2count);
            mps::core::Port* port = _outputPorts->at(0);

            for( Pt::uint32_t i = 0; i < _outputData.size(); ++i)
                port->onUpdateDataValue(data2count, i, &_outputData[i][0]);
        }
    }
    else
    {
        if( _triggerPort == port && _triggerSource == sourceIdx)
        {
            initAndExecute(0, noOfRecords);

            mps::core::Port* port = _outputPorts->at(0);
            for( Pt::uint32_t i = 0; i < _outputData.size(); ++i)
                port->onUpdateDataValue(noOfRecords, i, &_outputData[i][0]);
        }
    }
}

void MpalPS::initAndExecute(const Pt::uint8_t* data1, Pt::uint32_t count)
{
    for( Pt::uint32_t rec = 0; rec < count; ++rec)
    {
        if(data1 != 0)
        {//Copy the data for the input variables
            std::map<Pt::uint32_t, Pt::uint32_t>::iterator it = _sig2IndexMap.begin(); 

            for(; it != _sig2IndexMap.end(); ++it)
            {
                Sig2VarMapIt sig2VarIt = _inSig2VarMap.find(it->first);

                const Pt::uint8_t* curData = data1 + (rec * _inputBuffer.elementSize()) + _inputBuffer.itemOffset(it->second);

                for(Pt::uint32_t i = 0; i < sig2VarIt->second.size(); ++i)
                {
                    VariableInfo& varInfo = sig2VarIt->second[i];
                    void* varData = _programInfo->variableValue(varInfo.path);
                    ::mpal::vm::Variable* var = _programInfo->getVariable(varInfo.path);

                    if( varInfo.scaled)
                    {
                        double value = varInfo.signal->scaleValue(curData);
                        castAndCopyData((Pt::uint8_t*)varData ,(Pt::uint8_t*) &value, var->type(), mps::core::SignalDataType::VT_real64);
                    }
                    else
                    {
                        castAndCopyData((Pt::uint8_t*)varData ,(Pt::uint8_t*) &curData, var->type(), (mps::core::SignalDataType::Type) varInfo.signal->valueDataType());
                    }
                }
            }
        }

        //Record complete => Execute the program.
        try
        {
            if( _runInDebugger != 0)
                _debugger->start();
            else
                _vm.execute(*_programInfo);
        }
        catch(const std::exception& e)
        {
            mps::core::Message message(e.what(),  mps::core::Message::Output, mps::core::Message::Error, Pt::System::Clock::getLocalTime());
            sendMessage( message );
            return;
        }

        //Copy the data from the output variables.
        if( _outputPorts->size() == 0)
            return;

        std::map<Pt::uint32_t,VariableInfo>::iterator itOut = _outSig2VarMap.begin();
        
        for( ; itOut != _outSig2VarMap.end(); ++itOut)
        {
            VariableInfo& varInfo = itOut->second;
            void* varData = _programInfo->variableValue(varInfo.path);
            memcpy(&(_outputData[varInfo.sourceIndex][varInfo.offset + _outputRecordSize[varInfo.sourceIndex] * rec]), varData, varInfo.signal->valueSize());
        }
    }
}

void MpalPS::onDeinitialize()
{
    for( Pt::uint32_t i = 0; i < _varDefValues.size(); ++i)
    {
        VariableDefValue& varDefVal = _varDefValues[i];

        if(varDefVal.valueBuffer != 0 )
        {
            delete varDefVal.valueBuffer;
            varDefVal.valueBuffer = 0;
        }
    }

    ProcessStation::onDeinitialize();
}

void MpalPS::onExitInstance()
{
    if( _debugger != 0)
    {
        _debugger->close();
        delete _debugger;
    }

    ProcessStation::onExitInstance();
}

const std::string& MpalPS::program() const
{ 
    return _debuggerIP;
}

Pt::uint32_t MpalPS::triggerSigID() const
{
    return _triggerSignalID;
}
        
void MpalPS::setTriggerSigID(Pt::uint32_t sigID)
{
    _triggerSignalID = sigID;
}

void MpalPS::setRunInDebugger(Pt::uint8_t r)
{
    _runInDebugger = r;
}

Pt::uint8_t MpalPS::runInDebugger() const
{
    return _runInDebugger;
}

void MpalPS::setDebuggerIP(const std::string& ip)
{
    _debuggerIP = ip;
}

const std::string& MpalPS::debuggerIP() const
{
    return _debuggerIP;
}

void MpalPS::setDebuggerPort(Pt::uint32_t port)
{
    _debuggerPort = port;
}

Pt::uint32_t MpalPS::debuggerPort() const
{
    return _debuggerPort;
}

Pt::uint32_t MpalPS::vmMemSize() const
{
    return _memSize;
}

void MpalPS::setVmMemSize(Pt::uint32_t m)
{
    _memSize = m;
}

const std::string& MpalPS::varSigMapping() const
{ 
    return _debuggerIP; 
}
    
const std::string& MpalPS::sigPropVarMapping() const
{ 
    return _debuggerIP; 
}

}}
