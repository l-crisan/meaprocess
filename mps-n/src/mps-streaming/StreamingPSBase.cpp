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
#include "StreamingPSBase.h"
#include <Pt/System/SerialDevice.h>
#include <sstream>
#include <iostream> 
#include <sstream>
#include <memory>
 
namespace mps{
namespace streaming{


int toInt(const char* noStr)
{
    int no;
    std::stringstream ss;
    ss<<noStr;
    ss>>no;
    return no;
}

StreamingPSBase::StreamingPSBase()
: _server(0)
,_listenThread(0) 
{
}

StreamingPSBase::~StreamingPSBase()
{
}

void StreamingPSBase::removeDevice(Pt::System::IODevice* device)
{
    for(Pt::uint32_t i = 0; i < devices().size(); ++i)
    {
        if( devices()[i] == device)
        {
            devices().erase( devices().begin() + i);
            break;
        }
    }
    
    delete device;

    if( _server != 0)
        _server->beginAccept();
}

void StreamingPSBase::createConnection(const std::string& connection)
{
    std::stringstream ss;
    char buffer[255];
    char ch;

    _connection = connection;
    ss<<_connection;
    ss.getline(buffer,255,'/');

    std::string strConn = buffer;
    ss>>ch;
    ss.getline(buffer,255);

    if(strConn == "stcp:")
    {
        createTCPConnectionServer(_connection, buffer);
    }
    else if(strConn == "tcp:")
    {
        createTCPConnectionSocket(_connection, buffer);
    }
    else if(strConn == "serial:")
    {
        createSerialConnection(_connection, buffer);
    }
}


void StreamingPSBase::createSerialConnection(const std::string& conName, const char* parameter)
{
    try
    {
        std::stringstream ss;

        char buffer[255];

        ss<<parameter;
        ss.getline(buffer,255,'?');

        std::string portStr = buffer;
        ss.getline(buffer,255, '=');
        ss.getline(buffer,255, '&');
        Pt::uint32_t baudrate  = toInt(buffer);


        Pt::System::SerialDevice::BaudRate brate = (Pt::System::SerialDevice::BaudRate)baudrate;

        Pt::System::SerialDevice* device = new Pt::System::SerialDevice(portStr, std::ios::in| std::ios::out);
        device->setBaudRate(brate);	
        device->setCharSize(8);
        device->setStopBits(Pt::System::SerialDevice::OneStopBit);
        device->setParity(Pt::System::SerialDevice::ParityNone);
        device->setTimeout(500);

       Pt::System::MutexLock lock(_mutex);

        _devices.push_back(device);
  }
  catch(const std::exception& ex)
  {
        std::cerr<<ex.what()<<std::endl;
    sendErrorMsgOnCreateConn();
  }
}

void StreamingPSBase::createTCPConnectionSocket(const std::string& conName, const char* parameter)
{
    try
    {
        unsigned short port = 0;
        std::stringstream ss;
        char buffer[255];

        ss<<parameter;

        ss.getline(buffer,255,':');
        std::string strIp = buffer;
        
        ss.getline(buffer,255);

        port =toInt(buffer);

        std::unique_ptr<Pt::Net::TcpSocket> socketPtr(new Pt::Net::TcpSocket());

        socketPtr->setTimeout(500);

        socketPtr->connect(Pt::Net::Endpoint(strIp, port));
    
        Pt::System::MutexLock lock(_mutex);

        _devices.push_back(socketPtr.release());
 
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
        sendErrorMsgOnCreateConn();
    }
}

void StreamingPSBase::destroyConnection()
{
    if(_listenThread != 0)
    {
        _serverLoop.exit();
        _listenThread->join();
        delete _listenThread;
        _listenThread = 0;
        delete _server;
        _server = 0;
    }

    for( Pt::uint32_t i = 0; i <_devices.size(); ++i)
        delete _devices[i];

    _devices.clear();
}

void StreamingPSBase::createTCPConnectionServer(const std::string& conName, const char* parameter)
{
    try
    {
        char buffer[255];
        std::stringstream ss;
        
        ss<<parameter;

        ss.getline(buffer,255,':');

        std::string strIp = buffer;
        
        ss.getline(buffer,255);

        unsigned short port = (unsigned short)toInt(buffer);

        std::unique_ptr<Pt::Net::TcpServer> serverPtr(new Pt::Net::TcpServer(Pt::Net::Endpoint(strIp,port)));
        serverPtr->connectionPending() += Pt::slot(*this, &StreamingPSBase::onAccept);
        serverPtr->setActive(_serverLoop);		
        _server = serverPtr.release();
        _listenThread = new Pt::System::AttachedThread(Pt::callable(*this, &StreamingPSBase::listen));    
        _listenThread->start();	
        Pt::System::Thread::sleep(200);
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what()<<std::endl;
        sendErrorMsgOnCreateConn();
        return;
    }
}

void StreamingPSBase::onAddNewDevice(Pt::System::IODevice* device)
{
}

void StreamingPSBase::listen()
{
    _server->beginAccept();
    _serverLoop.run();
}

void StreamingPSBase::onAccept(Pt::Net::TcpServer& server)
{
    Pt::Net::TcpSocket* socket = new Pt::Net::TcpSocket();
    socket->setTimeout(500);
    socket->accept(server);

    _mutex.lock();
    _devices.push_back(socket);
    _mutex.unlock();

    onAddNewDevice(socket);
    _server->beginAccept();
}

}}
