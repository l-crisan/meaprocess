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

#ifndef MPAL_VM_DEBUGGER_H
#define MPAL_VM_DEBUGGER_H

#include <mpal/vm/VirtualMachine.h>
#include <Pt/System/Thread.h>
#include <Pt/System/Condition.h>
#include <Pt/Net/TcpServer.h>
#include <Pt/Net/TcpSocket.h>
#include <Pt/System/EventLoop.h>
#include <Pt/String.h>
#include <Pt/System/Condition.h>
#include <Pt/System/IOStream.h>
#include <Pt/System/MainLoop.h>
#include <string>

namespace mpal{
namespace vm{

class MPAL_VM_API Debugger : public Pt::Connectable
{
    public:
        Debugger(VirtualMachine& vm);
    
        virtual ~Debugger();
    
        void setup(const std::string& ip, Pt::uint32_t port);
    
        ProgramInfo& load(std::istream& ist);

        void start();
        void stop();
        void close();
        void waitEnd();

private:  
        enum Command
        {
            InitInputStack = 1,
            ClearBreakPoints,
            InsertBreakPoint,
            RemoveBreakPoint,
            StartDebugger,
            StepOver,
            StepInto,
            GetCallStack,
            ContinueExecution,
            Terminate,
            ReadMemoryByRef,
            ReadMemoryFromInstance,
            ReadMemoryByOffset,
            SetVmMemSize,
            Responce,
            NotifyOnline = 100,
            NotifyOnTerminate,
            NotifyOnMessage,
            NotifyOK
        };

        static Pt::uint8_t* getValueFromString(const std::string& strValue, mpal::vm::Variable* var);
        static std::string readString(Pt::Net::TcpSocket* socket);

private:
        void execute();

        //Remote command
        void insertBreakPoint(int line, const std::string& unit);
        void removeBreakPoint(int line, const std::string& unit);
        void clearBreakPoints();
        void initInputStack(const std::string& vars);
    
        void startDebugger();
        void stepOver();
        void stepInto();
        void continueExecution();
        void terminate();
        void setVmMemSize(int size);
        void getCallStack();

        //Notifications
        void onBreakPoint(BreakPoint breakPoint);
        void onLineUpdate(int line, std::string unit);
        void onTerminate();
        void onMessage(std::string msg);
        void onClientConnected(Pt::Net::TcpServer& server);
        void onCommand();
        void onInput(Pt::System::IODevice& device);

        void readMemoryByRef(int offset, int size, const std::string& func);
        void readMemoryByOffset(int offset, int size, const std::string& func);
        void readMemoryFromInstance(int offset, int size, const std::string& func);
        void listenThread();
        void notifyOK();
        void send(const std::vector<Pt::uint8_t>& data);

    private:
        Pt::System::AttachedThread*	_executionThread;       //Execution of the Mpal program.
        Pt::System::AttachedThread* _serverListenerThread;    //TcpServer listen thread.

        VirtualMachine&         _vm;
        mpal::vm::ProgramInfo*  _programInfo;
        Pt::Net::TcpServer      _server;
        Pt::Net::TcpSocket*     _client;
        std::string              _ip;
        Pt::uint32_t            _port;
        bool                    _runInLoop;
        bool                    _closeOnTerminate;
        bool                    _running;
        Pt::System::Condition       _terminateCondition;
        std::vector<Pt::uint8_t> _inputBuffer;
        Pt::System::MainLoop        _loop;
        Pt::System::Mutex           _readWriteMutex;
};

}} //namespace

#endif

