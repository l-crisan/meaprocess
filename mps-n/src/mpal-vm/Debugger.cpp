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
#include <mpal/vm/Debugger.h>
#include <mpal/vm/VirtualMachine.h>
#include <Pt/System/IOStream.h>

#include <vector>
#include <sstream>

namespace mpal{
namespace vm{

Debugger::Debugger(VirtualMachine& vm)
: _executionThread(0)
, _serverListenerThread(0)
, _vm(vm)
, _programInfo(0)
, _server()
, _client(0)
, _ip("")
, _port(0)
, _runInLoop(false)
, _closeOnTerminate(false)
, _running(false)
, _terminateCondition()
, _inputBuffer()
, _loop()
{
    _inputBuffer.resize(1);
}

Debugger::~Debugger()
{
    close();
}

void Debugger::setup(const std::string& ip, Pt::uint32_t port)
{
    close();

    _ip = ip;
    _port = port;

    _server.listen(Pt::Net::Endpoint(_ip, (unsigned short)_port));
    _server.connectionPending() += Pt::slot(*this, &Debugger::onClientConnected);

    _vm.onLine += Pt::slot( *this, &Debugger::onLineUpdate);
    _vm.onBreakPoint += Pt::slot( *this, &Debugger::onBreakPoint);
    _vm.onTerminate += Pt::slot( *this, &Debugger::onTerminate);  
    _vm.onMessage += Pt::slot( *this, &Debugger::onMessage);  
    _server.setActive(_loop);
    _running = true;
    _serverListenerThread = new Pt::System::AttachedThread( Pt::callable(*this, &Debugger::listenThread) );
    _serverListenerThread->start(); 
}

mpal::vm::ProgramInfo& Debugger::load(std::istream& ist)
{
    _programInfo = &_vm.load(ist);
    return *_programInfo;
}

void Debugger::start()
{  
    execute();
}

void Debugger::onClientConnected(Pt::Net::TcpServer& server)
{
    if(!_running)
        return;

    if(_client != 0)
    {
        delete _client;
        _server.beginAccept();
    }

    _inputBuffer.resize(1);

    _client = new Pt::Net::TcpSocket();
    _client->accept(server);	

    _client->inputReady() += Pt::slot(*this, &Debugger::onInput);

    _client->setActive(_loop);
    _client->beginRead((char*) &_inputBuffer[0], _inputBuffer.size());
}

void Debugger::waitEnd()
{
    Pt::System::Mutex mutex;
    Pt::System::MutexLock lock(mutex);
    _terminateCondition.wait(mutex);
}

void Debugger::onInput(Pt::System::IODevice& device)
{
    Pt::uint32_t n = 0;

    {
        Pt::System::MutexLock lock(_readWriteMutex);

        n = device.endRead();
    
        if(device.isEof())
        {
            delete _client;
            _client = 0;
            _server.beginAccept();
            return;
        }
    }

    if(n == 0)
    {
        try
        {
            Pt::System::MutexLock lock(_readWriteMutex);
            device.beginRead((char*) &_inputBuffer[0], _inputBuffer.size());
        }
        catch(const std::exception& ex)
        {
            delete _client;
            _client = 0;
            _server.beginAccept();
            std::cerr<<ex.what()<<std::endl;
        }

        return;
    }
    
    const Pt::uint8_t cmd = _inputBuffer[0];
    
    switch(cmd)
    {
        case InitInputStack:
        {
            std::string vars = readString(_client);

            initInputStack(vars);
        }
        break;
            
        case ClearBreakPoints:
            clearBreakPoints();
        break;

        case InsertBreakPoint:
        {
            Pt::int32_t  line;
            _client->read((char*)&line, sizeof(Pt::int32_t));
            std::string function = readString(_client);
            insertBreakPoint(line, function);
        }
        break;
            
        case RemoveBreakPoint:
        {
            Pt::int32_t  line;
            _client->read((char*)&line, sizeof(Pt::int32_t));
            std::string function = readString(_client);
            removeBreakPoint(line, function);
        }
        break;

        case StartDebugger:
            startDebugger();
        break;

        case StepOver:
            stepOver();
        break;

        case StepInto:
            stepInto();
        break;

        case GetCallStack:
            getCallStack();
        break;
            
        case ContinueExecution:
            continueExecution();
        break;

        case Terminate:
            terminate();
        break;

        case ReadMemoryByRef:
        {
            Pt::int32_t offset;
            Pt::int32_t size;

            _client->read((char*)&offset, sizeof(Pt::int32_t));
            _client->read((char*)&size, sizeof(Pt::int32_t));
            std::string func = readString(_client);
            readMemoryByRef(offset, size, func);
        }
        break;
            
        case ReadMemoryFromInstance:
        {
            Pt::int32_t offset;
            Pt::int32_t size;

            _client->read((char*)&offset, sizeof(Pt::int32_t));
            _client->read((char*)&size, sizeof(Pt::int32_t));
            std::string func = readString(_client);
            readMemoryFromInstance(offset, size, func);
        }
        break;

        case ReadMemoryByOffset:
        {
            Pt::int32_t offset;
            Pt::int32_t size;

            _client->read((char*)&offset, sizeof(Pt::int32_t));
            _client->read((char*)&size, sizeof(Pt::int32_t));
            std::string func = readString(_client);
            readMemoryByOffset(offset, size, func);
        }
        break;

        case SetVmMemSize:
        {
            Pt::int32_t size;
            _client->read((char*)&size, sizeof(Pt::int32_t));
            setVmMemSize(size);
        }
        break;
    }

    try
    {
        Pt::System::MutexLock lock(_readWriteMutex);
        device.cancel();
        device.beginRead((char*) &_inputBuffer[0], _inputBuffer.size());
    }
    catch(const std::exception& ex)
    {
        delete _client;
        _client = 0;
        _server.beginAccept();
        std::cerr<<ex.what()<<std::endl;
    }

}

std::string Debugger::readString(Pt::Net::TcpSocket* socket)
{
    char ch;
    
    std::string str;
    socket->read(&ch,1);

    while(ch != 0)
    {
        str += ch;
        socket->read(&ch,1);
    }

    return str;
}

void Debugger::listenThread()
{
    _server.beginAccept();
    _loop.run();
}

void Debugger::stop()
{
    if(_executionThread != 0)
    {
        terminate();
        _executionThread->join();
        delete _executionThread;
        _executionThread = 0;
    }
}

void Debugger::close()
{    
    stop();

    if(_client != 0)
    {
        delete _client;
        _client = 0;
        _server.beginAccept();
    }

    _running = false;

    if(_serverListenerThread != 0)
    {
        _server.detach();
        _server.close();
        _loop.exit();
        _serverListenerThread->join();
        delete _serverListenerThread;
        _serverListenerThread = 0;
    }
}

Pt::uint8_t* Debugger::getValueFromString(const std::string& strValue, mpal::vm::Variable* var)
{
    std::stringstream ss;

    ss<<strValue;

    switch(var->type())
    {
        case mpal::vm::BOOL:
        {
            Pt::uint8_t* value = new Pt::uint8_t;

            if( strValue == "TRUE")
                *value = 1;
            else
                *value = 0;
            
            return value;
        }
        break;
        case mpal::vm::SINT:
        {
            Pt::int8_t* value = new Pt::int8_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::INT:
        {
            Pt::int16_t* value = new Pt::int16_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::DINT:
        case mpal::vm::ENUM:
        {
            Pt::int32_t* value = new Pt::int32_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::LINT:
        {
            Pt::int64_t* value = new Pt::int64_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::USINT:
        {
            Pt::uint8_t* value = new Pt::uint8_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::UINT:
        {
            Pt::uint16_t* value = new Pt::uint16_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::UDINT:
        {
            Pt::uint32_t* value = new Pt::uint32_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::ULINT:
        {
            Pt::uint64_t* value = new Pt::uint64_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::REAL:
        {
            float* value = new float;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::LREAL:
        {
            double* value = new double;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::BYTE:
        {
            Pt::uint8_t* value = new Pt::uint8_t;
            ss>>*value;
            return value;
        }
        break;
        case mpal::vm::WORD:
        {
            Pt::uint16_t* value = new Pt::uint16_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::DWORD:
        {
            Pt::uint32_t* value = new Pt::uint32_t;
            ss>>*value;
            return (Pt::uint8_t*) value;
        }
        break;
        case mpal::vm::LWORD:
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

void Debugger::setVmMemSize(int size)
{
    _vm.setVmMemory(1024*size);
    notifyOK();
}

void Debugger::readMemoryByRef(int offset, int size, const std::string& func)
{
    std::vector<Pt::uint8_t> adrByte = _vm.readMemoryByOffset(offset, sizeof(void*), func);
    size_t address;
    memcpy(&address, &adrByte[0], sizeof(address));

    if( address == 0 )
    {
        std::vector<Pt::uint8_t> responce(size + 1);
        responce[0] =  (Pt::uint8_t)Responce;
        send(responce);
    }
    else
    {
        std::vector<Pt::uint8_t> data =  _vm.readMemory(address, size, func);
        std::vector<Pt::uint8_t> responce(data.size() + 1);
        responce[0] = (Pt::uint8_t)Responce;
        memcpy(&responce[1], &data[0], data.size());
        send(responce);
    }
}

void Debugger::readMemoryFromInstance(int offset, int size, const std::string& func)
{
    std::vector<Pt::uint8_t> adrByte = _vm.readMemoryByOffset(0, sizeof(void*), func);
    size_t address;
    memcpy(&address, &adrByte[0], sizeof(address));
    
    if( address == 0)
    {
        std::vector<Pt::uint8_t> responce(size + 1);
        responce[0] = (Pt::uint8_t)Responce;
        send(responce);
    }
    else
    {
        address += offset;
        std::vector<Pt::uint8_t> data =  _vm.readMemory(address, size, func);
        std::vector<Pt::uint8_t> responce(data.size() + 1);
        responce[0] = (Pt::uint8_t)Responce;
        memcpy(&responce[1], &data[0], data.size());
        send(responce);
    }
}

void Debugger::readMemoryByOffset(int offset, int size, const std::string& func)
{
    std::vector<Pt::uint8_t> data =  _vm.readMemoryByOffset(offset, size, func);    
    std::vector<Pt::uint8_t> responce(data.size() + 1);
    responce[0] = (Pt::uint8_t)Responce;
    memcpy(&responce[1], &data[0], data.size());
    send(responce);
}

void Debugger::initInputStack(const std::string& vars)
{
    _programInfo->init();

    if(vars == "")
    {
        notifyOK();
        return;
    }

    std::stringstream ss(vars);

    char buffer[500];
    
    std::vector<std::string> variables;

    while( ss.getline(buffer, 500, '\n'))
        variables.push_back(std::string(buffer));

    for( Pt::uint32_t i = 0; i < variables.size(); ++i)
    {
        std::string variable = variables[i];
        std::stringstream s(variable);
        std::vector<Pt::uint32_t> path;
        int pos = 0;
        std::string valueStr;

        while(s.getline(buffer,500,'/'))
        {
            if( pos == 0)
            {
                valueStr = buffer;
            }
            else
            {
                Pt::uint32_t index = 0;

                std::string item = buffer;
                std::stringstream sn(item);
                sn.getline(buffer, 500, '#');

                std::stringstream ss1;
                ss1 << buffer;
                ss1 >> index;	
                path.push_back(index);
            }
            ++pos;
        }

        Variable* var = _programInfo->getVariable(path);
        Pt::uint8_t* value = getValueFromString(valueStr,var);
        void* varMem = _programInfo->variableValue(path);
        memcpy(varMem, value, var->size());
        delete value;
    }

    notifyOK();
}

void Debugger::insertBreakPoint(int line, const std::string& unit)
{
    std::string strunit = unit;
    BreakPoint bp(strunit,line);
   _vm.insertBreakPoint(bp);
   notifyOK();
}

void Debugger::notifyOK()
{
    try
    {
        std::vector<Pt::uint8_t> ok(1);
        ok[0] = (Pt::uint8_t) NotifyOK;
        send(ok);
    }
    catch(std::exception ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }
}

void Debugger::removeBreakPoint(int line, const std::string& unit)
{
    BreakPoint bp(unit, line);
   _vm.removeBreakPoint(bp);
   notifyOK();
}

void Debugger::clearBreakPoints()
{
  _vm.clearBreakPoints();
    notifyOK();
}

void Debugger::startDebugger()
{
    _executionThread = new Pt::System::AttachedThread( Pt::callable(*this, &Debugger::execute) );     
    _executionThread->start();
    notifyOK();
}

void Debugger::stepOver()
{
    _vm.stepOver();
}

void Debugger::stepInto()
{
    _vm.stepInto();
}

void Debugger::continueExecution()
{
    _vm.continueExecution();
}

void Debugger::terminate()
{
    _vm.terminate();
}

void Debugger::getCallStack()
{
    std::string callStack = _vm.getCallStack();
    std::vector<Pt::uint8_t> responce(callStack.size() + 2);
    responce[0]  = (Pt::uint8_t) Responce;
    
    if(callStack.size() != 0)
        memcpy(&responce[1], &callStack[0], callStack.size());

    responce[responce.size() - 1] = 0;
    send(responce);
}

void Debugger::execute()
{
    try
    {
        _vm.execute(*_programInfo, true, _runInLoop);
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }
}

void Debugger::onBreakPoint(BreakPoint breakPoint)
{    
    int line = breakPoint.line();
    std::string unit = breakPoint.funcName();
    std::vector<Pt::uint8_t> data(1 + sizeof(int) + unit.size() + 1);
    data[0] = (Pt::uint8_t) NotifyOnline;
    memcpy(&data[1], &line, sizeof(int));
    memcpy(&data[5], &unit[0], unit.size());
    data[data.size() - 1] = 0;
    send(data);
}

void Debugger::onLineUpdate(int line, std::string unit)
{
    std::vector<Pt::uint8_t> data(1 + sizeof(int) + unit.size() + 1);
    data[0] = (Pt::uint8_t) NotifyOnline;
    memcpy(&data[1], &line, sizeof(int));
    
    if(unit.size() != 0)
        memcpy(&data[5], &unit[0], unit.size());

    data[data.size() - 1] = 0;
    send(data);
}

void Debugger::onMessage(std::string msg)
{
    std::vector<Pt::uint8_t> data(1 + msg.size() + 1);
    data[0] = (Pt::uint8_t) NotifyOnMessage;
    memcpy(&data[1], &msg[0], msg.size());
    data[data.size() - 1] = 0;
    send(data);
}

void Debugger::send(const std::vector<Pt::uint8_t>& data)
{
    Pt::System::MutexLock lock(_readWriteMutex);

    if( _client == 0 )
        return;

    try
    {
        _client->cancel();
        _client->write((char*) &data[0], data.size());
        _client->beginRead((char*) &_inputBuffer[0], _inputBuffer.size());
    }
    catch(const std::exception& ex)
    {
        delete _client;
        _client = 0;
        _server.beginAccept();
        std::cerr << ex.what()<<std::endl;
    }
}

void Debugger::onTerminate()
{
    std::vector<Pt::uint8_t> data(1);
    data[0] = (Pt::uint8_t) NotifyOnTerminate;
    send(data);

    _terminateCondition.signal();
}

}}

