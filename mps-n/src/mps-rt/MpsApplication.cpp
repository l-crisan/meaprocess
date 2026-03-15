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
#include "MpsApplication.h"
#include "System.h"
#include <Pt/System/Thread.h>
#include <Pt/System/Directory.h>
#include <Pt/System/Clock.h>
#include <Pt/System/FileDevice.h>
#include <Pt/System/FileInfo.h>
#include <Pt/System/Uri.h>
#include <Pt/System/Logger.h>
#include <Pt/Http/Server.h>
#include <Pt/Settings.h>
#include <Pt/System/Application.h>
#include <Pt/System/Mutex.h>
#include <Pt/Remoting/ServiceDefinition.h>
#include <cctype>
#include <algorithm>
#include <fstream>
#include <mps/core/Port.h>
#include <mps/modld/ModuleLoader.h>
#include <mps/builder/XmlBuilder.h>

namespace mps{
namespace rt{

static inline Pt::DateTime fromMSecsSinceEpoch(Pt::int64_t t)
{
    static const Pt::DateTime dt(1970, 1, 1);
    Pt::Timespan ts(t*1000);
    return dt + ts;
}

MpsApplication* MpsApplication::_me = 0;

MpsApplication::MpsApplication()
: _logger("MeaProcess")
, _netDevice(0)
, _serialDevice()
, _listenOnNet(false)
, _listenOnSerDev(false)
, _inputBuffer()
, _runtime(0)
, _status(Setup)
, _messages()
, _schemeID("")
, _schemeTimeStamp(0)
, _path("")
, _runtimeType("")
, _langCode("en-US")
, _schemeFile("")
, _systemHandle(0)
, _scriptingInputPort(0)
, _scriptingOutputPort(0)
, _mapAnyServlet(_httpService)
, _serviceDef()
, _rpcService(_serviceDef)
, _mapUrlServlet("/MeaProcess",_rpcService)
{
    _me = this;
    _inputBuffer.resize(1024);
    _systemHandle = System::initSystem();
}

MpsApplication::~MpsApplication()
{
    System::deinitSystem(_systemHandle);
}

#if defined(WIN32)
void __stdcall MpsApplication::OnScriptingOutputData(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portNo, Pt::uint32_t dataSize,  const Pt::uint8_t* data )
#else
void MpsApplication::OnScriptingOutputData(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portNo, Pt::uint32_t dataSize,  const Pt::uint8_t* data )
#endif
{
    if(status != 0)
        return;

    if(_me != 0)
        _me->OnPsScriptingOutputData(noOfRecords, sourceIdx, portNo, dataSize, data);
}

#if defined(WIN32)
void __stdcall MpsApplication::OnScriptingInputData(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t sigInx, Pt::uint32_t dataSize, Pt::uint8_t* data)
#else
void MpsApplication::OnScriptingInputData(Pt::uint32_t objID, Pt::uint32_t status, Pt::uint32_t sigInx, Pt::uint32_t dataSize, Pt::uint8_t* data)
#endif
{
    if(status != 0)
        return;

    if(_me != 0)
        _me->OnPsScriptingInputData(sigInx, dataSize, data);
}


Pt::System::LogLevel MpsApplication::toLogLevel(std::string level)
{
    std::transform(level.begin(), level.end(), level.begin(), tolower);

    if( level == "none")
        return Pt::System::None;

    if( level == "fatal")
        return Pt::System::Fatal;

    if( level == "error")
        return Pt::System::Error;

    if( level == "warn")
        return Pt::System::Warn;

    if( level == "info")
        return Pt::System::Info;

    if( level == "debug")
        return Pt::System::Debug;

    if( level == "trace")
        return Pt::System::Trace;

    return Pt::System::Error;
}

bool MpsApplication::xmlRpcLoadRuntime(const std::string& schemeID, const std::string& timeStamp,const std::string& xmlScheme)
{
    std::stringstream ss;
    Pt::int64_t i64TimeStamp = 0;

    ss<<timeStamp;
    ss>>i64TimeStamp;

    loadRuntime(schemeID, i64TimeStamp, xmlScheme);
    
    if(_runtime == 0)
        return false;

    initialize();
    return true;
}

std::string MpsApplication::xmlRpcGetSignalList(std::string connection)
{
    return _runtime->getSignalList(connection);
}

bool MpsApplication::xmlRpcStart()
{
    start();
    return true;
}

bool MpsApplication::xmlRpcStop()
{
    stop();
    return true;
}

bool MpsApplication::xmlRpcReinitialize()
{	
    deinitialize();
    initialize();
    return true;
}

Pt::int32_t MpsApplication::xmlRpcGetNoOfInputSignals()
{
    if(_scriptingInputPort == 0)
        return 0;

    return (Pt::int32_t) _scriptingInputPort->signalList()->size();
}

Pt::int32_t MpsApplication::xmlRpcGetNoOfOutputSignals()
{
    if(_scriptingOutputPort == 0)
        return 0;

    return (Pt::int32_t) _scriptingOutputPort->signalList()->size();
}

std::string MpsApplication::xmlRpcGetMessages()
{
    std::stringstream ss;
    for( Pt::uint32_t i = 0; i < _messages.size(); ++i)
    {
        const mps::core::Message& msg = _messages[i];
        ss<<getMessageType(msg.type())<<"\t";

        Pt::DateTime dt = msg.formatedTimeStamp();	
        ss<<dt.toIsoString()<<"\t";
        ss<<msg.text()<<"\n";
    }

    return ss.str();
}

double MpsApplication::xmlRpcGetSignalMin(Pt::int32_t index, bool input)
{
    mps::core::Signal* signal = getScriptingSignal(index, input);
    
    if( signal == 0)
        return 0;

    return signal->physMin();
}

double MpsApplication::xmlRpcGetSignalMax(Pt::int32_t index, bool input)
{
    mps::core::Signal* signal = getScriptingSignal(index, input);
    
    if( signal == 0)
        return 0;

    return signal->physMax();
}

mps::core::Signal* MpsApplication::getScriptingSignal(Pt::int32_t index, bool input)
{
    if(input)
    {
        if( _scriptingInputPort == 0)
            return 0;

        if(static_cast<Pt::uint32_t>(index) >= _scriptingInputPort->signalList()->size())
            return 0;

        return _scriptingInputPort->signalList()->at(index);
    }
    else
    {
        if( _scriptingOutputPort == 0)
            return 0;

        if(static_cast<Pt::uint32_t>(index) >= _scriptingOutputPort->signalList()->size())
            return 0;

        return _scriptingOutputPort->signalList()->at(index);
    }
}

std::string MpsApplication::xmlRpcGetSignalComment(Pt::int32_t index, bool input)
{
    mps::core::Signal* signal = getScriptingSignal(index, input);
    
    if( signal == 0)
        return "";

    return signal->comment();
}

std::string MpsApplication::xmlRpcGetSignalName(Pt::int32_t index, bool input)
{
    mps::core::Signal* signal = getScriptingSignal(index, input);
    
    if( signal == 0)
        return "";

    return signal->name();
}

double MpsApplication::xmlRpcGetSignalValue(Pt::int32_t index)
{
    if(_scriptingOutputPort == 0)
        return 0;

    mps::core::Signal* signal = getScriptingSignal(index, false);

    if( signal == 0)
        return 0;

    Pt::uint32_t indexInList = _scriptingOutputPort->signalList()->getSignalIndex(signal);

    return _scriptingOutputData[indexInList];
}

bool MpsApplication::xmlRpcSetSignalValue(Pt::int32_t index, double value)
{
    if( _scriptingInputPort == 0)
        return false;

    mps::core::Signal* signal = getScriptingSignal(index, true);

    if( signal == 0)
        return 0;

    Pt::uint32_t indexInList = _scriptingInputPort->signalList()->getSignalIndex(signal);

    _scriptingInputData[indexInList] = value;
    return true;
}

std::string MpsApplication::xmlRpcGetSignalUnit(Pt::int32_t index, bool input)
{
    mps::core::Signal* signal = getScriptingSignal(index, input);
    
    if( signal == 0)
        return "";

    return signal->unit();
}

void MpsApplication::init(const MpsSettings& settings, const std::string& path)
{	
    _path = path;
    _state = StateBeginCommand;
    _cmdSize = 0;
    _schemeFile = settings.schemeFileLocation();	
    _runtimeType = settings.runtime();
    _listenOnNet = settings.listenOnNetwork();
    _sysConfig = settings.systemConfiguration();
    _listenOnSerDev = settings.listenOnSerialDevice();
    _listenOnHttp = settings.listenOnHttp();
    _httpPort = settings.httpPort();
    
    std::string httpDir = _path + "http";

    _httpService.setWorkingDir(httpDir.c_str());
    
    _logger.target().setLogLevel(toLogLevel(settings.logLevel()));
    _logger.target().setChannel(settings.logChannel());
    
    //Load runtime modules.
    loadModules();

    //Listen on Http
    if(_listenOnHttp)
    {
        try
        {	
            //Register XmlRpc methods to the service
            _serviceDef.registerProcedure("GetStatus", *this, &MpsApplication::xmlRpcGetStatus);
            _serviceDef.registerProcedure("GetSchema", *this, &MpsApplication::xmlRpcGetSchemaId);			
            _serviceDef.registerProcedure("LoadScheme", *this, &MpsApplication::xmlRpcLoadRuntime);
            _serviceDef.registerProcedure("Stop", *this, &MpsApplication::xmlRpcStop);
            _serviceDef.registerProcedure("Start", *this, &MpsApplication::xmlRpcStart);
            _serviceDef.registerProcedure("Reinitialize", *this, &MpsApplication::xmlRpcReinitialize);
            _serviceDef.registerProcedure("GetMessages", *this, &MpsApplication::xmlRpcGetMessages);
            _serviceDef.registerProcedure("GetNoOfInputSignals",*this, &MpsApplication::xmlRpcGetNoOfInputSignals);
            _serviceDef.registerProcedure("GetNoOfOutputSignals",*this, &MpsApplication::xmlRpcGetNoOfOutputSignals);
            _serviceDef.registerProcedure("GetSignalName",*this,  &MpsApplication::xmlRpcGetSignalName);
            _serviceDef.registerProcedure("GetSignalComment",*this,  &MpsApplication::xmlRpcGetSignalComment);
            _serviceDef.registerProcedure("GetSignalUnit",*this,  &MpsApplication::xmlRpcGetSignalUnit);
            _serviceDef.registerProcedure("GetSignalValue",*this, &MpsApplication::xmlRpcGetSignalValue);
            _serviceDef.registerProcedure("SetSignalValue",*this, &MpsApplication::xmlRpcSetSignalValue);
            _serviceDef.registerProcedure("GetSignalMax",*this, &MpsApplication::xmlRpcGetSignalMax);
            _serviceDef.registerProcedure("GetSignalMin",*this, &MpsApplication::xmlRpcGetSignalMin);
            _serviceDef.registerProcedure("GetSignalList",*this, &MpsApplication::xmlRpcGetSignalList);

            //Create an HTTP server
            Pt::Http::Server* server = new Pt::Http::Server(this->loop(), Pt::Net::Endpoint::ip4Any(_httpPort));
            
            //Register the XML-RPC service
            server->addServlet(_mapUrlServlet);
            server->addServlet(_mapAnyServlet);
            _httpServers.push_back(server);
        }
        catch(const std::exception& ex)
        {     
            PT_LOGGER_LOG_WARN(_logger, "Listen HTTP on port  " << _httpPort << " failed (" << ex.what() << ").");
            std::cout<<"Warning : Listen HTTP on port " <<_httpPort<< " failed."<<std::endl;
        }
    }

    //Listen on network
    if(_listenOnNet)
    {		
        Pt::Net::TcpServer* server = new Pt::Net::TcpServer();				
        try
        {				
            server->connectionPending() += Pt::slot(*this, &MpsApplication::onClientConnected);
            server->setActive(this->loop());
            server->listen(Pt::Net::Endpoint::ip4Any(settings.port()));
            server->beginAccept();
            _servers.push_back(server);
        }
        catch(const std::exception& ex)
        {
            delete server;
            PT_LOGGER_LOG_WARN(_logger, "Listen on TCP port: " << settings.port() << " failed (" << ex.what() << ").");
            std::cout<<"Warning : Listen on TCP port: " << settings.port()<< " failed."<<std::endl;
        }
    }
    
    //Listen on serial device.
    if( _listenOnSerDev )
    {	
        _serialDevice.open(settings.serDev(), std::ios::in | std::ios::out);
        _serialDevice.setBaudRate((unsigned) settings.serBaudrate());
        _serialDevice.setCharSize(8);
        _serialDevice.setStopBits(Pt::System::SerialDevice::OneStopBit);
        _serialDevice.setParity(Pt::System::SerialDevice::ParityNone);
        _serialDevice.setFlowControl(Pt::System::SerialDevice::FlowControlSoft);
        _serialDevice.inputReady() += Pt::slot(*this, &MpsApplication::onInput);
        _serialDevice.setActive(this->loop());
        
        _serialDevice.beginRead((char*) &_inputBuffer[0], _inputBuffer.size());	
    }	

    if(_servers.size() == 0 && !_listenOnSerDev && _httpServers.size() == 0)
    {
        PT_LOGGER_LOG_ERROR(_logger, "Couldn't listen on network adapters.");
        throw std::runtime_error("Couldn't listen on network adapters.");
    }

    //If a scheme exist load and start it.
    if(Pt::System::FileInfo::exists(Pt::System::Path(_schemeFile.c_str())))
    {
        std::string xml;
        std::string schemeID;
        Pt::int64_t timeStamp;	
        std::fstream f;
            
        try
        {
            f.open(_schemeFile.c_str(), std::ios::in | std::ios::binary);
            Pt::uint32_t size = 0;
            //Scheme id size
            f.read((char*)&size, sizeof(size));

            //Scheme id
            std::vector<char> buffer(size+1);
            f.read(&buffer[0], size);
            buffer[size] = 0;
            schemeID = (char*) &buffer[0];

            //Time stamp
            f.read((char*) &timeStamp, sizeof(timeStamp));
            
            //Xml data size
            f.read((char*)&size, sizeof(size));
            
            //Xml data
            buffer.resize(size+1);
            f.read(&buffer[0], size);
            buffer[size] = 0;
            f.close();
                                
            loadRuntime(schemeID, timeStamp , &buffer[0], false);
            setLanguage("en-US");
            initialize();
            start();
        }
        catch(...)
        {
            PT_LOGGER_LOG_ERROR(_logger, "Loading scheme failed.");
        }
    }			
}

int MpsApplication::xmlRpcGetStatus()
{
    return (int)_status;
}


std::string MpsApplication::xmlRpcGetSchemaId()
{
    std::stringstream ss;

    Pt::DateTime dt = fromMSecsSinceEpoch((Pt::int64_t) _schemeTimeStamp);

    ss<<"["<<dt.toIsoString()<<"] " <<_schemeID;
    return ss.str();

}

void MpsApplication::loadModules()
{
    try
    {
        const std::string file = _path + "module.set";
        std::fstream stream(file.c_str());
        char buffer[255];

        while(stream.getline(buffer,255))
        {
            std::string moduleName = "";

            if( buffer[0] == '.')
                moduleName = _path + std::string((char*)&buffer[2]);
            else
                moduleName = buffer;
            
            mps::core::ObjectFactory* factory = mps::modld::ModuleLoader::loadModule(moduleName.c_str());

            if(factory == 0)
            {
                std::cout<<"Error: " "The module " << moduleName << " couldn't be loaded."<<std::endl;
                PT_LOGGER_LOG_ERROR(_logger, "The module " << moduleName << " couldn't be loaded.");
            }
            else
            {
                mps::core::RuntimeEngine::registerFactory(factory);
            }
        }
    }
    catch(const std::exception& ex)
    {
        std::cout<<"Error: " << ex.what()<<std::endl;
        PT_LOGGER_LOG_ERROR(_logger, ex.what());
    }
}

void MpsApplication::setProperty(const std::string& name, const std::string& value)
{
    if( _runtime == 0)
        return;

    _runtime->setPropertyValue(name.c_str(), value.c_str());
}

void MpsApplication::getProperty(const std::string& name, Pt::System::IODevice* ioDevice)
{
    if( _runtime == 0)
        return;

    std::string value = _runtime->getRawPropertyValue(name.c_str());	
    try
    {
        ioDevice->write(value.c_str(), value.size() +1 ); 
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }
}

void MpsApplication::loadResource()
{
    const std::string fname = _path + "mps-rt" + "." + _langCode + ".mres";

    _trans.clear();

    try
    {
        std::ifstream s(fname.c_str());
        char buffer[500];

        if(!s.good())
            return;

        while(s.getline(buffer, 500))
            _trans.addToTranslationMapFromStr(buffer);
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }
}

void MpsApplication::onClientConnected(Pt::Net::TcpServer& server)
{
    if(_netDevice != 0)
        delete _netDevice;

    
    Pt::Net::TcpSocket* client = new Pt::Net::TcpSocket();
    client->accept(server);	
    _netDevice = client;
    _netDevice->inputReady() += Pt::slot(*this, &MpsApplication::onInput);
    _netDevice->setActive(this->loop());
    _cmdSize = 0;
    _netDevice->beginRead((char*) &_inputBuffer[0], _inputBuffer.size());	
}

std::string MpsApplication::readString(std::stringstream& stream)
{
    char ch;
    
    std::stringstream ss;
    try
    {
        stream.read(&ch,1);	

        while(ch != 0)
        {
            ss << ch;
            stream.read(&ch,1);	
        }
    }
    catch(const std::exception& ex)
    {
        std::cerr<<"MpsApplication::readString() "<<ex.what()<<std::endl;
    }

    return ss.str();
}

void MpsApplication::processCommand(Pt::uint8_t cmd, std::vector<Pt::uint8_t>& cmdData, Pt::System::IODevice& device)
{

    switch(cmd)
    {
        case CmdLoadRuntime:
        {
            std::stringstream ss;
            ss.write((char*)&cmdData[0], cmdData.size());
            std::string scheme = readString(ss);
            Pt::int64_t schemeTime = 0;
            ss.read((char*)& schemeTime, sizeof(Pt::int64_t));
            std::string runtime = readString(ss);
            loadRuntime(scheme, schemeTime, runtime, true, &device);
        }
        break;

        case CmdInitialize:
            initialize(&device);
        break;

        case CmdDeinitialize:
            deinitialize(&device);
        break;
        
        case CmdStart:
            start(&device);
        break;
        
        case CmdStop:
            stop(&device);
        break;

        case CmdCloseRuntime:
            closeRuntime();
        break;
        
        case CmdTerminate:
            terminate();
        break;

        case CmdSetProperty:
        {
            std::stringstream ss;
            ss.write((char*)&cmdData[0], cmdData.size());

            std::string name = readString(ss);
            std::string value = readString(ss);
            setProperty(name, value);
        }
        break;
        
        case CmdGetProperty:
        {
            std::stringstream ss;
            ss.write((char*)&cmdData[0], cmdData.size());

            std::string name = readString(ss);
            getProperty(name, &device);
        }
        break;
        
        case CmdReadLogFile:
            readLogFile(&device);
        break;

        case CmdGetStatus:
            getStatus(&device);
        break;
        
        case CmdSetLanguage:
        {
            std::stringstream ss;
            ss.write((char*)&cmdData[0], cmdData.size());

            std::string lang = readString(ss);
            setLanguage(lang, &device);
        }
        break;

        case CmdGetSysConf:
            getSysConfig(&device);
        break;

        default:
        break;
    }
}

void MpsApplication::processData(Pt::uint8_t dataByte, Pt::System::IODevice& device)
{
    switch(_state)
    {
        case StateBeginCommand:
            _cmd = dataByte;
            _state = StateGetCmdSize;
        break;

        case StateGetCmdSize:
            _cmdSizeBuffer.push_back(dataByte);

            if(_cmdSizeBuffer.size() == 4)
            {
                _cmdSize = *((Pt::uint32_t*) &_cmdSizeBuffer[0]);

                if(_cmdSize == 0)
                {
                    processCommand(_cmd, _cmdDataBuffer, device);
                    _cmdDataBuffer.clear();
                    _cmdSizeBuffer.clear();
                    _state = StateBeginCommand;
                }
                else
                {
                    _state = StateCommand;
                }
            }
        break;

        case StateCommand:
        {
            _cmdDataBuffer.push_back(dataByte);
            if( _cmdDataBuffer.size() == _cmdSize)
            {
                processCommand(_cmd, _cmdDataBuffer, device);
                _cmdDataBuffer.clear();
                _cmdSizeBuffer.clear();
                _cmdSize = 0;
                _state = StateBeginCommand;
            }
        }
        break;
    }
}

void MpsApplication::onInput(Pt::System::IODevice& device)
{
    try
    {
        const Pt::uint32_t dataSize = (Pt::uint32_t) device.endRead();

        if( device.isEof())
        {
            if( &device == _netDevice)
            {
                delete _netDevice;
                _netDevice = 0;
                _servers[0]->beginAccept();
            }		
            return;
        }
            
        if(dataSize == 0)
        {		
            device.beginRead((char*) &_inputBuffer[0], _inputBuffer.size());
            return;
        }			

        for( Pt::uint32_t i = 0; i < dataSize; ++i)
            processData(_inputBuffer[i], device);
    
        device.beginRead((char*) &_inputBuffer[0], _inputBuffer.size());
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;	
        if( &device == _netDevice)
        {
            delete _netDevice;
            _servers[0]->beginAccept();
            _netDevice = 0;
        }
    }
}

void MpsApplication::getSysConfig(Pt::System::IODevice* ioDevice)
{	
    if(ioDevice ==  0)
        return;

    std::stringstream ss;

    for( Pt::uint32_t i = 0; i < _sysConfig.size(); ++i)
    {
        ss<<_sysConfig[i];
        ss<<',';
    }
    std::string config = ss.str();
    
    std::vector<Pt::uint8_t> responce(config.size() + 1);
    
    if(config.size() > 0)
        memcpy(&responce[0],&config[0], config.size());

    responce[responce.size() - 1] = 0;

    try
    {
        ioDevice->write((char*) &responce[0], responce.size());
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }
}

void MpsApplication::setLanguage(const std::string& lang, Pt::System::IODevice* device)
{	
    if(_runtime != 0)
        _runtime->setLanguage(lang.c_str());
    
    if( _langCode != lang)
    {
        _langCode = lang;
        loadResource();
    }
    
    notifyOK(device);
}

void MpsApplication::readLogFile(Pt::System::IODevice* device)
{	
    try
    {
        Pt::System::Uri url(_logger.target().channelUrl());
        std::string file = url.path();

        if(file == "")
        {		
            std::vector<char> buffer(sizeof(Pt::uint32_t));
            Pt::uint32_t size = 0;
            memcpy(&buffer[0],&size, 4);			
            device->write(&buffer[0],buffer.size());
            return;
        }
        
        Pt::System::FileInfo finfo(file.c_str()); 
        Pt::uint32_t size  = static_cast<Pt::uint32_t>(finfo.size());		

        Pt::System::FileDevice fd;
        fd.open(Pt::System::Path(file.c_str()), std::ios::in);
        
        std::vector<char> buffer(size+sizeof(Pt::uint32_t));
        memcpy(&buffer[0],&size,4);

        if( size != 0)
            fd.read(&buffer[4], size);

        device->write(&buffer[0], size+sizeof(Pt::uint32_t));
    }
    catch(...)
    {
        try
        {
            std::vector<char> buffer(4);
            Pt::uint32_t size = 0;
            memcpy(&buffer[0],&size, 4);			
            device->write(&buffer[0],buffer.size());
        }
        catch(const std::exception& ex)
        {
            std::cerr<<ex.what()<<std::endl;
        }
    }
}

void MpsApplication::close()
{	
    closeRuntime();


    if(_listenOnNet)
    {
        _listenOnNet = false;

        for(Pt::uint32_t i = 0; i < _servers.size(); ++i)
        {
            Pt::Net::TcpServer* server = _servers[i];
            delete server;
        }

        _servers.clear();		
    }

    if( _listenOnHttp)
    {
        _listenOnHttp = false;
        for(Pt::uint32_t i = 0; i < _httpServers.size(); ++i)
        {
            Pt::Http::Server* server = _httpServers[i];
            delete server;
        }

        _httpServers.clear();
    }

    if( _listenOnSerDev)
    {
         _listenOnSerDev = false;
         _serialDevice.close();
    }
    
    if(_netDevice != 0)
    {
        delete _netDevice;
        _netDevice = 0;
    }			
}

void MpsApplication::sendMessage(const mps::core::Message& message, Pt::System::IODevice* ioDevice)
{
    if(ioDevice != 0)
    {
        try
        {
            std::vector<Pt::uint8_t> data;

            Pt::uint32_t size = 1 + 1 + sizeof(Pt::int64_t) + (Pt::uint32_t)message.text().size() + 1;
    
            data.resize(size);
            data[0] = (Pt::uint8_t) message.type();
            data[1] = (Pt::uint8_t) message.target();
            Pt::int64_t time = message.timeStamp();

            memcpy(&data[2], &time, sizeof(Pt::int64_t));
            memcpy(&data[10], message.text().c_str(), message.text().size() + 1);
            ioDevice->write((char*) &data[0], data.size());
        }
        catch(const std::exception& ex)
        {
            std::cerr<<ex.what()<<std::endl;
        }
    }
}

void MpsApplication::loadRuntime(const std::string& schemeID, Pt::int64_t timeStamp, const std::string& xml, bool saveToFile, Pt::System::IODevice* ioDevice)
{
    if(_runtime != 0)
        closeRuntime();

    mps::core::Message message;
    _runtime = mps::builder::XmlBuilder::loadXml( xml.c_str(), message, mps::core::RuntimeEngine::objectFactory());
    
    if(_runtime == 0)
    {
        if(ioDevice != 0)
        {
            std::vector<Pt::uint8_t> data;
            data.resize(5);
            data[0] = (Pt::uint8_t) NotifyMessage;
            Pt::uint32_t count = 1;
            memcpy(&data[1], &count, sizeof(Pt::uint32_t));
            ioDevice->write((char*) &data[0], data.size());
            sendMessage(message, ioDevice);
        }
        PT_LOGGER_LOG_INFO(_logger, getMessageType(message.type()) << " : " << message.text());
        return;
    }

    
    if( _runtimeType != _runtime->getType() )
    {
        message.setType(mps::core::Message::Error);
        message.setText(_trans.translate("WrongSchemeErr"));
        message.setTimeStamp(Pt::System::Clock::getLocalTime());
        message.setTarget(mps::core::Message::Output);

        if(ioDevice != 0)
        {
            std::vector<Pt::uint8_t> data;
            data.resize(5);
            data[0] = (Pt::uint8_t) NotifyMessage;
            Pt::uint32_t count = 1;
            memcpy(&data[1], &count, sizeof(Pt::uint32_t));
            ioDevice->write((char*) &data[0], data.size());
            sendMessage(message, ioDevice);
        }

        PT_LOGGER_LOG_INFO(_logger, getMessageType(message.type()) << " : " << message.text());
        
        mps::builder::XmlBuilder::destroy(_runtime);		
        _runtime = 0;
        
        return;
    }

    _runtime->setMessageListener(this);
    _runtime->setWorkingDirectory(_path.c_str());
    
    _schemeID = schemeID;
    _schemeTimeStamp = timeStamp;
    _status = Loaded;

    Pt::uint32_t inputPsID = _runtime->getScriptInputPSID();
    Pt::uint32_t outputPsID = _runtime->getScriptOutputPSID();

    if( inputPsID != 0)
    {
        _scriptingInputPort = _runtime->getScriptInputPSPort();
        _scriptingInputData.clear();
        _scriptingInputData.resize(_scriptingInputPort->signalList()->size(),0.0);
        _runtime->addDataSource(0, inputPsID, &MpsApplication::OnScriptingInputData);
    }

    if(outputPsID != 0)
    {
        _scriptingOutputPort = _runtime->getScriptOutputPSPort();
        _scriptingOutputData.clear();
        _scriptingOutputData.resize(_scriptingOutputPort->signalList()->size(),0.0);
        _runtime->addDataOutListener(0, outputPsID, &MpsApplication::OnScriptingOutputData);
    }

    if( _schemeFile != "" && saveToFile)
    {
        std::fstream f;
        
        try
        {
            f.open(_schemeFile.c_str(), std::ios::binary| std::ios::out| std::ios::trunc);
            //Scheme id size
            Pt::uint32_t size = (Pt::uint32_t)_schemeID.size();
            f.write((char*)&size, sizeof(size));

            //Scheme id
            f.write((char*) _schemeID.c_str(), _schemeID.size());

            //Scheme time stamp
            f.write((char*) &_schemeTimeStamp, sizeof(_schemeTimeStamp));

            //Xml data size
            size = (Pt::uint32_t) xml.size() ;
            f.write((char*)&size, sizeof(size));
            
            //Xml data
            f.write(xml.c_str(), xml.size());
            f.close();
        }
        catch(const std::exception& ex)
        {			
            PT_LOGGER_LOG_ERROR(_logger, ex.what());
        }
    }
    notifyOK(ioDevice);
}

void MpsApplication::initialize(Pt::System::IODevice* ioDevice)
{
    if(_runtime == 0)
        return;

    _messages.clear();

    _runtime->initialize();

    _status = Initialised;


    if(_messages.size() == 0)
    {		
        notifyOK(ioDevice);		
        setReadyState();
    }
    else
    {	
        checkErrorState();

        if(ioDevice != 0)
        {
            std::vector<Pt::uint8_t> data;
            data.resize(5);
            data[0] = (Pt::uint8_t) NotifyMessage;
            Pt::uint32_t count = (Pt::uint32_t) _messages.size();
            memcpy(&data[1], &count, sizeof(Pt::uint32_t));
            ioDevice->write((char*) &data[0], data.size());
        
            for(Pt::uint32_t i = 0; i < _messages.size(); ++i)
                sendMessage(_messages[i], ioDevice);
        }

        _messages.clear();		
    }	
}

void MpsApplication::OnPsScriptingOutputData(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portNo, Pt::uint32_t dataSize,  const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = _scriptingOutputPort->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
    Pt::uint32_t recordSize = _scriptingOutputPort->sourceDataSize(sourceIdx);

    for(Pt::uint32_t sigIdx = 0; sigIdx < source.size(); ++sigIdx)
    {
        const mps::core::Signal* signal = source[sigIdx];
        const Pt::uint32_t indexInList = _scriptingOutputPort->signalList()->getSignalIndex(signal);
        const Pt::uint32_t offset = _scriptingOutputPort->signalOffsetInSource(sourceIdx, sigIdx);
        const Pt::uint8_t* pData = &data[(recordSize * (noOfRecords -1)) + offset];
        _scriptingOutputData[indexInList] =  signal->scaleValue(pData);
    }
}

void MpsApplication::OnPsScriptingInputData(Pt::uint32_t sigInx, Pt::uint32_t dataSize, Pt::uint8_t* data)
{
    assert(dataSize == 8);

    memcpy(data, &_scriptingInputData[sigInx], dataSize);
}

void MpsApplication::deinitialize(Pt::System::IODevice* ioDevice)
{
    if(_runtime == 0)
        return;

    _runtime->deinitialize();
    _status = Loaded;
    setReadyState();

    notifyOK(ioDevice);

}

void MpsApplication::checkErrorState()
{
    for( Pt::uint32_t i = 0; i < _messages.size(); ++i)
    {
        if(_messages[i].type() == mps::core::Message::Error)
        {
            setErrorState();
            return;
        }
    }

    setReadyState();
}

void MpsApplication::setErrorState()
{
    System::setRun(false);
    System::setReady(false);
    System::setError(true);
}

void MpsApplication::setReadyState()
{
    System::setRun(false);
    System::setReady(true);
    System::setError(false);
}

void MpsApplication::setRunState()
{
    System::setRun(true);
    System::setReady(false);
    System::setError(false);
}

void MpsApplication::start(Pt::System::IODevice* ioDevice)
{
    if(_runtime == 0)
        return;

    _messages.clear();

    for( Pt::uint32_t i = 0; i < _scriptingInputData.size(); ++i)
        _scriptingInputData[i] = 0;

    
    for( Pt::uint32_t i = 0; i < _scriptingOutputData.size(); ++i)
        _scriptingOutputData[i] = 0;


    setRunState();

    _runtime->start();
    _status = Running;


    if(_messages.size() != 0)
    {
        checkErrorState();

        if( ioDevice != 0)
        {
            std::vector<Pt::uint8_t> data;
            data.resize(5);
            data[0] = (Pt::uint8_t) NotifyMessage;
            Pt::uint32_t count = (Pt::uint32_t) _messages.size();
            memcpy(&data[1], &count, sizeof(Pt::uint32_t));
            ioDevice->write((char*) &data[0], data.size());
        
            for(Pt::uint32_t i = 0; i < _messages.size(); ++i)
                sendMessage(_messages[i], ioDevice);

            _messages.clear();
        }

        return;
    }

    notifyOK(ioDevice);

}

void MpsApplication::getStatus(Pt::System::IODevice* ioDevice)
{
    Pt::uint8_t stat = (Pt::uint8_t) _status;
    std::vector<Pt::uint8_t> data;

    data.resize(1 + sizeof(Pt::int64_t) + _schemeID.size() + 1);
    data[0] = stat;
    memcpy(&data[1],  &_schemeTimeStamp, sizeof(Pt::int64_t));

    memcpy(&data[9], _schemeID.c_str(), _schemeID.size() +1);

    ioDevice->write((char*) &data[0], data.size());	

}

void MpsApplication::stop(Pt::System::IODevice* ioDevice)
{
    if(_runtime == 0)
        return;

    _runtime->stop();
    _status = Initialised;
    
    setReadyState();

    notifyOK(ioDevice);
}

void MpsApplication::closeRuntime()
{
    if(_runtime == 0)
        return;

    _runtime->stop();
    _runtime->deinitialize();	
        
    mps::builder::XmlBuilder::destroy(_runtime);

    _runtime = 0;
    _status = Setup;
    setErrorState();
}

mps::core::MessageResult::MsgResult MpsApplication::onMessage(const mps::core::Message& msg)
{
    _messages.push_back(msg);

    Pt::DateTime dt = msg.formatedTimeStamp();	
    
    switch(msg.type())
    {
        case mps::core::Message::Error:
            PT_LOGGER_LOG_ERROR(_logger, msg.text());
        break;
        
        case mps::core::Message::Warning:
            PT_LOGGER_LOG_WARN(_logger, msg.text());
        break;

        case mps::core::Message::Stop:
            PT_LOGGER_LOG_INFO(_logger, "Runtime stopped");
        break;

        default:		
            PT_LOGGER_LOG_INFO(_logger, msg.text());
        break;				
    }
    
    if(msg.type() == mps::core::Message::EventMsg)
        std::cout<<"[ "<< dt.toIsoString() << " ] Event: "<< msg.text()<<std::endl;

    if(msg.type() == mps::core::Message::Stop)
        _status = Initialised;

    return mps::core::MessageResult::Yes;
}

std::string MpsApplication::getMessageType(mps::core::Message::MessageType type)
{
    std::string msgType = "INFO";

    switch(type)
    {
        case mps::core::Message::Info:
            msgType = "INFO";
        break;
        case mps::core::Message::Warning:
            msgType = "WARNING";
        break;
        case mps::core::Message::Error:
            msgType = "ERROR";
        break;
        case mps::core::Message::Question:
            msgType = "QUESTION";
        break;
        case mps::core::Message::EventMsg:
            msgType = "EVENT";
        break;
        case mps::core::Message::Stop:
            msgType = "RUNTIME STOPPED";
        default:
        break;
    }

    return msgType;
}

void MpsApplication::terminate()
{
    Pt::System::Application::exit();
}

void MpsApplication::notifyOK(Pt::System::IODevice* ioDevice)
{
    if(ioDevice == 0)
        return;

    try
    {
        Pt::uint8_t ok = (Pt::uint8_t) NotifyOK;
        ioDevice->write((char*) &ok,1);
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
    }
}
}}
