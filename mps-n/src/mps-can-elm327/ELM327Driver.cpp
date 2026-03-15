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
#include "ELM327Driver.h"
#include <ios>
#include <Pt/System/Thread.h>

namespace mps{
namespace can{
namespace elm327{

ELM327Driver::ELM327Driver()
: CANDriver("mps-can-elm327")
, _logger("mps::drv::can::ELM327Driver")
{    

}

ELM327Driver::~ELM327Driver()
{
}

bool ELM327Driver::setAcceptanceMask( Pt::uint32_t mask, Pt::uint32_t code)
{
    std::string maskstr = no2hex(mask);
    std::string str;

    if(_extendedID)
    {
        for(Pt::uint32_t i = maskstr.size(); i < 8; ++i )	
            str += "0";
    }
    else
    {
        for(Pt::uint32_t i = maskstr.size(); i < 3; ++i )	
            str += "0";
    }

    str += maskstr;
    std::string cmd = "AT CF "+ str;
    
    if(!writeCmd(cmd))
        return false;

    std::string codestr = no2hex(code);
    str = "";

    if(_extendedID)
    {
        for(Pt::uint32_t i = codestr.size(); i < 8; ++i )	
            str += "0";
    }
    else
    {
        for(Pt::uint32_t i = codestr.size(); i < 3; ++i )	
            str += "0";
    }
    str += codestr;
    cmd = "AT CM "+ str;
    
    if(!writeCmd(cmd))
        return false;

    return true;
}

bool ELM327Driver::writeCmd(const std::string& cmd)
{
    std::string ncmd = cmd + "\r";
    std::string res ="";

    Pt::uint32_t size = _device.write(ncmd.c_str(),ncmd.size());	
    
    if( size != ncmd.size())
        return false;
    
    _device.setTimeout(10000);
    
    try
    {
        _device.read(_buffer,1024);
    }
    catch(const std::exception& e)
    {
        logger_log_error(_logger,"writeCmd() failed.  " << cmd<<" "<<e.what());
        return false;
    }

    _buffer[size] = '\0';
    res = _buffer;

    Pt::uint32_t f = res.find("OK");
    if(f == std::string::npos)
    {
        logger_log_error(_logger,"writeCmd() failed.  " << cmd);
        return false;
    }

    return true;
}

bool ELM327Driver::wait(Pt::uint32_t timeout)
{
    readDirect(timeout);

    if( !_receiveQueue.empty())
        return true;

    return false;
}

void ELM327Driver::wake()
{
    _device.cancel();
}

bool ELM327Driver::open(const std::string& device, Pt::uint8_t deviceNo, Pt::uint8_t port, Pt::uint32_t bitRate, bool extendedId)
{
    try
    {
        port++;
        logger_log_info(_logger,"enter open()");
        _lastID = 0;
        _extendedID = extendedId;
        reset();
    
        _device.open(device, std::ios::in| std::ios::out);
        
        _device.setBaudRate(Pt::System::SerialDevice::BaudRate38400);
        _device.setCharSize(8);
        _device.setStopBits(Pt::System::SerialDevice::OneStopBit);
        _device.setParity(Pt::System::SerialDevice::ParityNone);
        _device.setTimeout(100);        
        
        _device.setSignal(Pt::System::SerialDevice::SET_RTS);
        Pt::System::Thread::sleep(20);
        _device.setSignal(Pt::System::SerialDevice::CLR_RTS);

        std::string cmd = "AT E0"; // Echo off

        if(!writeCmd(cmd))
            return false;
        
        cmd = "AT SP 6"; //  //Protocol CAN 11Bit 500000 baud => default

        if( bitRate == 250000 && _extendedID)
            cmd = "AT SP 9"; //Protocol CAN 29Bit 250000 baud

        if( bitRate == 500000 && _extendedID)
            cmd = "AT SP 7"; //Protocol CAN 29Bit 500000 baud

        if( bitRate == 250000 && !_extendedID)
            cmd = "AT SP 8"; //Protocol CAN 11Bit 250000 baud

        if( bitRate == 500000 && !_extendedID)
            cmd = "AT SP 6"; //Protocol CAN 11Bit 500000 baud
        
        if(!writeCmd(cmd))
            return false; 

        cmd = "AT CAF0"; // CAN Automatic Formating off
        if(!writeCmd(cmd))
            return false;

        cmd = "AT D1";  //Show DLC on receive
        if(!writeCmd(cmd))
            return false;

        cmd = "AT H1";  ///Show header on receive
        if(!writeCmd(cmd))
            return false;
    }
    catch(const Pt::System::IOError& e)
    {
        std::cerr<<e.what();
        return false;
    }

    return true;
}

bool ELM327Driver::extendedID() const
{
    return _extendedID;
}

void ELM327Driver::send(const mps::drv::can::Message& messageData)
{	
    std::stringstream ss;
    std::string cmd;

    if((messageData.data()[0] & 0xF0) == 48)
    {//Flowcontrol frame check
        if( messageData.data()[1] != 0)
        {
            std::stringstream msg;
            msg<<"The FlowControlFrame with parameter BS=";
            msg<<(int)messageData.data()[1] <<" and STmin="<<(int) messageData.data()[2];
            msg<<" is not supported by the ELM327 driver. Use BS=0x04 STmin=0x64";
            throw mps::drv::can::CANException(msg.str(), mps::drv::can::CANException::SendError);
        }
        return; //Don't send it. ELM327 do this for us.
    }

    if( _lastID != messageData.identifier())
    {//Set the can identifier
        std::string hexid = no2hex(messageData.identifier());
        std::string newid;
        Pt::uint32_t count = 3;

        if(_extendedID)
            count =6;

        for( Pt::uint32_t i = hexid.size(); i < count; ++i)
            newid += "0";

        newid += hexid;
        ss<<"AT SH "<<newid;

        cmd = ss.str();
        
        if(!writeCmd(cmd))
            throw mps::drv::can::CANException("Send error", mps::drv::can::CANException::SendError);

        _lastID = messageData.identifier();
    }

    const Pt::uint8_t* pdata =  messageData.data();

    std::stringstream ds;

    for( Pt::uint32_t i = 0; i < messageData.dlc() -1; ++i)
        ds<<byte2hex(pdata[i]);

    cmd = ds.str() +"\r";
    _device.write(cmd.c_str(), cmd.size());
    //readDirect();
}

std::string ELM327Driver::byte2hex(Pt::uint8_t b)
{
    std::string str;
    std::stringstream hexconv;
    hexconv<<std::hex<<std::uppercase<<(Pt::uint32_t) b;
    hexconv>>str;

    if( str.size() == 1)
        return std::string("0") + str;

    return str;
}

Pt::uint32_t ELM327Driver::hex2no(std::string str)
{
    Pt::uint32_t no;
    std::stringstream hexconv;
    hexconv<<str<<std::hex<<std::uppercase;
    hexconv>>no;
    return no;
}

std::string ELM327Driver::no2hex(Pt::uint32_t no)
{
    std::string str;

    std::stringstream hexconv;
    hexconv<<std::hex<<no;
    hexconv>>str;
    return str;
}

void ELM327Driver::readDirect(Pt::uint32_t timeout)
{
    std::stringstream ss;
    mps::drv::can::Message	message;	
    char	line[255];
    char	token[20];
    std::string data = "";
    
    _device.setTimeout(timeout);
    
    while( true)
    {
        Pt::uint32_t count = 0;
        try
        {
            count = _device.read(_buffer, 1024);
        }
        catch(const std::exception& ex)
        {
            ex.what();
            break;
        }
       
        _buffer[count] = '\0';
        data += _buffer;
    }
    
    if( data.size() < 5 || data.find("NO DATA") != std::string::npos)
        return;

    ss << data;

    while(ss.getline(line,255,'\r'))
    {
        std::string strline = line;

        if( strline.size() < 2)
            break;

        std::stringstream lineStream;
        lineStream << line;

        //ID
        lineStream.getline(token,20,' ');
        message.setIdentifier(hex2no(token));

        //DLC
        lineStream.getline(token,20,' ');
        message.setDlc(hex2no(token));

        //Bytes
        for( Pt::uint32_t i = 0; i < 8; ++i)
        {
            lineStream.getline(token, 20, ' ');
            message.data()[i] = (Pt::uint8_t) hex2no(token);
        }

        _receiveQueue.push(message);
    }

    if(_receiveQueue.size() >= 1024)
    {
        while(_receiveQueue.size() == 1024)
            _receiveQueue.pop();
    }
}

bool ELM327Driver::receive(mps::drv::can::Message& messageData)
{
    readDirect(1000);

    if( _receiveQueue.empty())
        return false;

    messageData  = _receiveQueue.front();
    _receiveQueue.pop();

    return true;
}

void ELM327Driver::reset()
{
    while (!_receiveQueue.empty()) 
        _receiveQueue.pop(); 
}

void ELM327Driver::close()
{    
    _device.close();
}

std::string ELM327Driver::driverInfo() const
{
    return "ELM327";
}

}}}
