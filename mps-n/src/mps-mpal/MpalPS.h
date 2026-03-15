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
#ifndef MPS_MPAL_MPALPS_H
#define MPS_MPAL_MPALPS_H

#include <mps/core/ProcessStation.h>
#include <string>
#include <map>
#include <Pt/Types.h>
#include <Pt/Any.h>
#include <mpal/vm/VirtualMachine.h>
#include <mpal/vm/Debugger.h>
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include <mps/core/RecordBuilder.h>

namespace mps{
namespace mpal{

class MpalPS : public mps::core::ProcessStation
{
public:
    MpalPS();
    virtual ~MpalPS();

    virtual void onInitInstance();
    
    virtual void onInitialize();
    virtual void onStart();

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);
    virtual void onDeinitialize();
    virtual void onExitInstance();

private:
    void addVarSigMapping(const std::string& mapping);
    const std::string& varSigMapping() const;
    void addSigPropVarMapping(const std::string& mapping);
    
    const std::string& sigPropVarMapping() const;
    void addSigDefValue(const std::string& mapping);

    void setProgram(const std::string& pr);
    const std::string& program() const;

    Pt::uint32_t triggerSigID() const;		
    void setTriggerSigID(Pt::uint32_t sigID);

    void setRunInDebugger(Pt::uint8_t r);
    Pt::uint8_t runInDebugger() const;

    void setDebuggerIP(const std::string& ip);
    const std::string& debuggerIP() const;

    void setDebuggerPort(Pt::uint32_t port);
    Pt::uint32_t debuggerPort() const;

    Pt::uint32_t vmMemSize() const;
    void setVmMemSize(Pt::uint32_t m);
    
    template<typename T> 
    static void castFrom(Pt::uint8_t* to, const Pt::uint8_t* from, ::mpal::vm::DataType toType)
    {
        switch(toType)
        {
            case ::mpal::vm::BOOL:
                *to = (Pt::uint8_t)(*((T*)from));
            break;
            case ::mpal::vm::SINT:
            {
                Pt::int8_t* val = (Pt::int8_t*) to;
                *val = (Pt::int8_t)(*((T*)from));
            }
            break;
            case ::mpal::vm::INT:
            {
                Pt::int16_t* val = (Pt::int16_t*) to;
                *val =(Pt::int16_t)(*((T*)from));
            }
            break;

            case ::mpal::vm::DINT:
            case ::mpal::vm::ENUM:
            {
                Pt::int32_t* val = (Pt::int32_t*)to;
                *val = (Pt::int32_t)(*((T*)from));
            }
            break;

            case ::mpal::vm::LINT:
            {
                Pt::int64_t* val = (Pt::int64_t*)to;
                *val = (Pt::int64_t)(*((T*)from));
            }
            break;
            case ::mpal::vm::USINT:
            case ::mpal::vm::BYTE:
                *to = (Pt::uint8_t)(*((T*)from));
            break;
            case ::mpal::vm::UINT:
            case ::mpal::vm::WORD:
            {
                Pt::uint16_t* val = (Pt::uint16_t*)to;
                *val = (Pt::uint16_t)(*((T*)from));
            }
            break;
            case ::mpal::vm::UDINT:
            case ::mpal::vm::DWORD:
            {
                Pt::uint32_t* val = (Pt::uint32_t*)to;
                *val = (Pt::uint32_t)(*((T*)from));
            }
            break;
            case ::mpal::vm::ULINT:
            case ::mpal::vm::LWORD:
            {
                Pt::uint64_t* val = (Pt::uint64_t*)to;
                *val = (Pt::uint64_t)(*((T*)from));
            }
            break;
            case ::mpal::vm::REAL:
            {
                float* val = (float*)to;
                *val = (float)(*((T*)from));
            }
            break;
            case ::mpal::vm::LREAL:
            {
                double* val = (double*)to;
                *val = (double)(*((T*)from));
            }
            break;
            default:
            break;
        }
    }
    
    const mps::core::Port* getPortBySignal(const mps::core::Signal* inSignal, bool input = true);

    const mps::core::Signal* getSignalByID(Pt::uint32_t id);

    static void castAndCopyData(Pt::uint8_t* to, const Pt::uint8_t* from, ::mpal::vm::DataType toType, mps::core::SignalDataType::Type fromType);

    void execute(Pt::uint32_t portIdx, Pt::uint32_t source, Pt::uint32_t noOfRecords);

    void initAndExecute(const Pt::uint8_t* data1, Pt::uint32_t rec);

    static Pt::uint8_t* getValueFromString(const std::string& strValue, ::mpal::vm::Variable* var);

private:
    enum PropertyType
    {
        SampleRate,
        Minimum,
        Maximum,
        Factor,
        Offset
    };

    struct VariableInfo
    {
        const mps::core::Port*		port;
        const mps::core::Signal*	signal;
        Pt::uint32_t			offset;
        Pt::uint32_t			sourceIndex;
        Pt::int8_t      propType;
        Pt::uint32_t    sigId;
        std::vector<Pt::uint32_t> path;
        Pt::uint32_t          indexInInputBuf;
        bool scaled;
    };

    struct VariableDefValue
    {
        std::vector<Pt::uint32_t> path;
        std::string               strValue;
        Pt::uint32_t              size;
        Pt::uint8_t*              valueBuffer;
    };

private:
    typedef std::map<Pt::uint32_t,std::vector<VariableInfo> > Sig2VarMap;
    std::map<Pt::uint32_t,Pt::uint32_t> _sig2IndexMap;

    Sig2VarMap _sig2VarMap;
    typedef Sig2VarMap::iterator Sig2VarMapIt;
    Sig2VarMap _inSig2VarMap;
    std::map<Pt::uint32_t,VariableInfo> _outSig2VarMap;
    std::vector<VariableInfo> _sigProp2Var;

    std::vector<Pt::uint8_t>		_programData;
    ::mpal::vm::ProgramInfo*			_programInfo;
    ::mpal::vm::VirtualMachine        _vm;
    ::mpal::vm::Debugger*             _debugger;
    Pt::uint32_t					_triggerSignalID;
    double							_executionRate;
    std::vector<std::vector<Pt::uint8_t> >		_outputData;
    std::vector<VariableDefValue>   _varDefValues;
    Pt::uint8_t                     _runInDebugger;
    std::string                     _debuggerIP;
    Pt::uint32_t                    _debuggerPort;
    Pt::uint32_t                    _memSize;
    bool							_errorState;
    std::vector<Pt::uint32_t>				_outputRecordSize;	
    mps::core::RecordBuilder		_inputBuffer;  
    Pt::uint32_t							_triggerSource;
    Pt::uint32_t							_triggerPort;	
};

}}

#endif
