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
#ifndef MPS_RT_MPS_APPICATION_H
#define MPS_RT_MPS_APPICATION_H

#include <Pt/System/Application.h>
#include <Pt/Types.h>
#include <Pt/Settings.h>
#include <Pt/Net/TcpServer.h>
#include <Pt/Net/TcpSocket.h>
#include <Pt/System/Logger.h>
#include <Pt/System/SerialDevice.h>
#include <Pt/Remoting/ServiceDefinition.h>
#include <Pt/Http/Server.h>
#include <Pt/Http/Servlet.h>
#include <Pt/XmlRpc/HttpService.h>
#include <mps/core/RuntimeEngine.h>
#include <mps/core/Message.h>
#include <mps/core/MessageListener.h>
#include <mps/core/Translator.h>
#include "HTTPService.h"
#include "MpsSettings.h"
#include <string>

namespace mps{
namespace rt{

class MpsApplication : public Pt::System::Application, protected mps::core::MessageListener 
{
public:
    MpsApplication();
    ~MpsApplication();

    void init(const MpsSettings& settings, const std::string& path);
    void close();

private:
    void loadRuntime(const std::string& scheme, Pt::int64_t tiemStamp, const std::string& xml, bool saveToFile = true, Pt::System::IODevice* ioDevice = 0);
    void initialize(Pt::System::IODevice* ioDevice = 0);
    void deinitialize(Pt::System::IODevice* ioDevice = 0);
    void start(Pt::System::IODevice* ioDevice = 0);
    void stop(Pt::System::IODevice* ioDevice = 0);
    void closeRuntime();
    void terminate();
    void onClientConnected(Pt::Net::TcpServer& server);
    void onInput(Pt::System::IODevice& device);
    void setProperty(const std::string& name, const std::string& value); 
    void getProperty(const std::string& name, Pt::System::IODevice* ioDevice = 0);
    void readLogFile(Pt::System::IODevice* ioDevice = 0);
    void getSysConfig(Pt::System::IODevice* ioDevice);
    void getStatus(Pt::System::IODevice* ioDevice = 0);
    void setLanguage(const std::string& lang, Pt::System::IODevice* ioDevice = 0);
    void notifyOK(Pt::System::IODevice* ioDevice);
    void sendMessage(const mps::core::Message& message, Pt::System::IODevice* ioDevice);
    void loadModules();
    void loadResource();
    virtual mps::core::MessageResult::MsgResult onMessage(const mps::core::Message& msg);
    void processData(Pt::uint8_t dataByte, Pt::System::IODevice& device);
    void processCommand(Pt::uint8_t cmd, std::vector<Pt::uint8_t>& cmdData,  Pt::System::IODevice& device);
    
    bool xmlRpcLoadRuntime(const std::string& schemeID, const std::string& timeStamp,const std::string& xmlScheme);
    bool xmlRpcStart();
    bool xmlRpcStop();
    bool xmlRpcReinitialize();
    int xmlRpcGetStatus();

    std::string xmlRpcGetMessages();
    std::string xmlRpcGetSchemaId();
    Pt::int32_t xmlRpcGetNoOfInputSignals();
    Pt::int32_t xmlRpcGetNoOfOutputSignals();
    std::string xmlRpcGetSignalName(Pt::int32_t index, bool input);
    std::string xmlRpcGetSignalComment(Pt::int32_t index, bool input);
    std::string xmlRpcGetSignalUnit(Pt::int32_t index, bool input);
    mps::core::Signal* getScriptingSignal(Pt::int32_t index, bool input);
    double xmlRpcGetSignalValue(Pt::int32_t index);
    bool xmlRpcSetSignalValue(Pt::int32_t index, double value);
    double  xmlRpcGetSignalMin(Pt::int32_t index, bool input);
    double  xmlRpcGetSignalMax(Pt::int32_t index, bool input);
    std::string xmlRpcGetSignalList(std::string connection);

    //Scripting stuff
private:
    void OnPsScriptingInputData(Pt::uint32_t sigInx, Pt::uint32_t dataSize, Pt::uint8_t* data);
    void OnPsScriptingOutputData(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portNo, Pt::uint32_t dataSize,  const Pt::uint8_t* data);

#if defined(WIN32)
    static void __stdcall OnScriptingOutputData(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portNo, Pt::uint32_t dataSize,  const Pt::uint8_t* data );
    static void __stdcall OnScriptingInputData(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t sigInx, Pt::uint32_t dataSize, Pt::uint8_t* data);
#else
    static void  OnScriptingOutputData(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portNo, Pt::uint32_t dataSize,  const Pt::uint8_t* data );
    static void  OnScriptingInputData(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t sigInx, Pt::uint32_t dataSize, Pt::uint8_t* data);
#endif

    void checkErrorState();
    void setErrorState();
    void setReadyState();
    void setRunState();

private:
    enum Status
    {
        Setup = 0,    ///< Runtime is setup
        Loaded,       ///< Runtime is loaded
        Initialised,  ///< Runtime is initialized
        Running       ///< Runtime is running
    };

    enum State
    {
        StateBeginCommand,
        StateGetCmdSize,
        StateCommand,
    };

    enum Command
    {
        CmdLoadRuntime = 1,
        CmdInitialize,
        CmdDeinitialize,
        CmdStart,
        CmdStop,
        CmdCloseRuntime,
        CmdTerminate,
        CmdGetStatus,
        CmdGetProperty,
        CmdSetProperty,
        CmdReadLogFile,
        CmdSetLanguage,
        CmdGetSysConf= 15,
        NotifyMessage=100,
        NotifyOK,
    };

    static std::string getMessageType(mps::core::Message::MessageType type);
    static Pt::System::LogLevel toLogLevel(std::string level);
    static std::string readString(std::stringstream& stream);

private:
    Pt::System::Logger _logger;
    std::vector<Pt::Net::TcpServer*> _servers;
    Pt::System::IODevice* _netDevice;
    Pt::System::SerialDevice _serialDevice;
    bool _listenOnNet;
    bool _listenOnSerDev;
    bool _listenOnHttp;
    Pt::uint32_t _httpPort;
    std::vector<Pt::uint8_t> _inputBuffer;
    mps::core::RuntimeEngine* _runtime;
    Status _status;
    std::vector<mps::core::Message> _messages;
    std::string _schemeID;
    Pt::int64_t _schemeTimeStamp;
    std::string _path;
    std::string _runtimeType;
    std::string _langCode;
    std::string _schemeFile;
    mps::core::Translator _trans;
    Pt::uint64_t _systemHandle;
    std::vector<std::string> _sysConfig;
    
    //XmlRpc staff
    std::vector<Pt::Http::Server*> _httpServers;
    
    //Socket/Serial com staff
    std::vector<Pt::uint8_t> _cmdSizeBuffer;
    std::vector<Pt::uint8_t> _cmdDataBuffer;
    Pt::uint8_t _cmd;
    Pt::uint32_t _cmdSize;
    State _state;
    static MpsApplication* _me;
    const mps::core::Port* _scriptingInputPort;
    const mps::core::Port* _scriptingOutputPort;
    std::vector<double> _scriptingInputData;
    std::vector<double> _scriptingOutputData;
    HTTPService _httpService;
    Pt::Http::MapAny    _mapAnyServlet;			
    Pt::Remoting::ServiceDefinition _serviceDef;
    Pt::XmlRpc::HttpService _rpcService;
    Pt::Http::MapUrl		_mapUrlServlet;		
};

}}

#endif
