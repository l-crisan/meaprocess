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
#include "OBD2SourcePS.h"
#include <mps/diatp/OBD2Request.h>
#include <mps/diatp/TPKLineHandler.h>
#include "OBD2Signal.h"
#include <string>
#include <mps/core/Message.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <mps/can/drv/Driver.h>
#include <mps/can/drv/Factory.h>
#include <sstream>
#include <Pt/System/Clock.h>
#include <Pt/Byteorder.h>
#include <cmath>

using namespace mps::can::drv;
using namespace mps::diatp;

namespace mps{
namespace obd2{

OBD2SourcePS::OBD2SourcePS()
: _running(false)
, _driverType("")
, _port(0)
, _addressMode(0)
, _canRate(500000)
, _serRate(0)
, _runThread(0)
, _canDriver(0)
, _canHandler(0)
, _serDevice(0)
, _tpHandler(0)
, _errorState(false)
, _vehicleInfos(0)
, _noOfEcus(0)
, _dtcSrcIdx(0)
, _dtcs(0)
, _dtcSignal(0)
, _dtcDDCSrcIdx(0)
, _dtcsDDC()
, _dtcDDCSignal(0)	
, _dtcMutex()
, _normalFixedFunctional(0x33, 0xF1, TPDUAddress::NormalFixedFunctional)
, _device("")
, _deviceNo(0)
{
    registerProperty( "driver", *this, &OBD2SourcePS::driver, &OBD2SourcePS::setDriver );
    registerProperty( "device", *this, &OBD2SourcePS::device, &OBD2SourcePS::setDevice );
    registerProperty( "deviceNo", *this, &OBD2SourcePS::deviceNo, &OBD2SourcePS::setDeviceNo );
    registerProperty( "port", *this, &OBD2SourcePS::port, &OBD2SourcePS::setPort );
    registerProperty( "addressMode", *this, &OBD2SourcePS::addressMode, &OBD2SourcePS::setAddressMode );
    registerProperty( "canRate", *this, &OBD2SourcePS::canRate, &OBD2SourcePS::setCanRate );
    registerProperty( "serRate", *this, &OBD2SourcePS::serRate, &OBD2SourcePS::setSerRate );
}

OBD2SourcePS::~OBD2SourcePS()
{
}

void OBD2SourcePS::onInitInstance()
{
    SynchSourcePS::onInitInstance();
}


mps::can::drv::Driver* OBD2SourcePS::createCANDriver()
{
    return Factory::createDriver( driver());
}

void OBD2SourcePS::onInitialize()
{
    SynchSourcePS::onInitialize();

    _errorState = false;

    if(driver() != "mps-kline-rs232")
    {//over can
        bool extendedID = addressMode() == Normal29Bit;

        _canDriver = createCANDriver();

        if(!_canDriver->open(_device,  deviceNo(), port(), canRate(), extendedID))
        {
            _errorState = true;
            mps::core::Message message( format(translate("Mp.OBD2.Err.Driver"),this->getName()),
                             mps::core::Message::Output, mps::core::Message::Error, Pt::System::Clock::getLocalTime());
            sendMessage(message);
            return;
        }

        if( !extendedID)
        {
            _canDriver->setAcceptanceMask(0xFF0, 0x7E0); //mask, code
        }
        else
        {
            //TODO: correct mask and code for extended id
            _canDriver->setAcceptanceMask(0xFF0 ,0x7E0); 
        }

        _canHandler = new mps::diatp::CANHandler(*_canDriver);
        _tpHandler = new TPCANHandler(*_canHandler);
        _tpHandler->start();
    }
    else
    {//over K-Line
        try
        {
            _serDevice = new Pt::System::SerialDevice();
            std::stringstream ss;

            _serDevice->open(device(), std::ios::in|std::ios::out);
        
            _serDevice->setFlowControl(Pt::System::SerialDevice::FlowControlSoft);
            _serDevice->setBaudRate((Pt::System::SerialDevice::BaudRate)serRate());
            _serDevice->setCharSize(8);
            _serDevice->setStopBits(Pt::System::SerialDevice::OneStopBit);
            _serDevice->setParity(Pt::System::SerialDevice::ParityNone);
            TPKLineHandler* tpHandler = new TPKLineHandler(*_serDevice);
            
            _tpHandler = tpHandler; 
            _tpHandler->start();
            tpHandler->startCommunication(_normalFixedFunctional);
        }
        catch(const std::exception& ex)
        {
            std::cerr<<ex.what()<<std::endl;
            _errorState = true;

            mps::core::Message message( format(translate("Mp.OBD2.Err.Driver"),this->getName()),
                             mps::core::Message::Output, mps::core::Message::Error, Pt::System::Clock::getLocalTime());

            sendMessage(message);
            return;
        }
    }

    readVehicleInfos();
    _tpHandler->reset(true);
    checkSupportedPids();
    _tpHandler->reset(true);

    const mps::core::Port*    port  = _outputPorts->at(0);
    const mps::core::Sources&sources = port->sources();
    _dtcSrcIdx = 0;
    _dtcSignal = 0;
    _dtcDDCSignal = 0;

    for(Pt::uint32_t src = 0; src < sources.size(); ++src)
    {
        const std::vector<mps::core::Signal*>& source = sources[src];

        for( Pt::uint32_t s = 0; s < source.size(); ++s)
        {
            const OBD2Signal* signal = (const OBD2Signal*) source[0];
            if( signal->sid() == 0x03)
            {
                _dtcSignal = signal;
                _dtcSrcIdx = src;
            }

            if( signal->sid() == 0x07)
            {
                _dtcDDCSrcIdx = src;
                _dtcDDCSignal = signal;
            }
        }

        std::vector<Pt::uint8_t> record;
        const Pt::uint32_t recordSize = port->sourceDataSize(src);
        record.resize(recordSize);
        _dataRecords.push_back(record);
    }
    
    
    if(driver()== "mps-kline-rs232")
    {
        TPKLineHandler* tpHandler = (TPKLineHandler*) _tpHandler;
        tpHandler->stopCommunication(_normalFixedFunctional);
    }

    _tpHandler->stop();
}

void OBD2SourcePS::addObject(Object* object, const std::string& type, const std::string& subType)
{
    if( type == "Mp.OBD2.VehicleInfos")
        _vehicleInfos = (mps::core::ObjectVector<OBD2VehicleInfo*>*) object;
    else
        SynchSourcePS::addObject(object, type, subType);
}

void OBD2SourcePS::readVehicleInfos()
{
    OBD2Request request(*_tpHandler, getAddress());

    Pt::uint8_t requestData[2];

    //Read the no of ecus.
    requestData[0] = 0x01;
    requestData[1] = 0x01;

    request.setData(requestData,2);
    
    request.request();

    _noOfEcus = 0;
    while(true)
    {
         std::vector<Pt::uint8_t> responseData = request.readNextResponse();
    
        if( responseData.size() == 0 )
            break;

        _noOfEcus++;
    }

    if( _noOfEcus == 0)
        _noOfEcus = 5;

    for( Pt::uint32_t i = 0; i < _vehicleInfos->size(); ++i)
    {
        OBD2VehicleInfo* vi = _vehicleInfos->at(i);
        
        requestData[0] = vi->sid();
        requestData[1] = vi->pid();
        
        request.setData(requestData,2);
    
        switch( vi->pid())
        {
            case 0x08:
                //TODO:
            break;
            case 0x0A:
            {//ECU names
                std::string ecus;

                request.request();

                while(true)
                {
                     std::vector<Pt::uint8_t> responseData = request.readNextResponse();
                
                    if( responseData.size() == 0 )
                        break;

                    const TPDUAddress& respAddress = request.responseAddress();
                    TPDUAddress targetAddress = respAddress.getComplementAddress();
                    
                    std::stringstream ss;
                    
                    ss<<std::hex<<std::uppercase;

                    switch(targetAddress.adrressType())
                    {
                        case TPDUAddress::NormalFixedPhysical:
                            ss<<targetAddress.targetAdr();
                            ecus += "(";
                            ecus += ss.str() + ") ";
                        break;

                        case TPDUAddress::Normal11Bit:
                            ss<<targetAddress.identifier();
                            ecus += "(";
                            ecus += ss.str() + ") ";
                        break;
                        default:
                        break;
                    }

                    for(Pt::uint32_t i = 0; i < responseData.size(); ++i)
                    {
                        if( responseData[i] == 0)
                            responseData[i] = ' ';
                    }
                    ecus += getStringFromResponse(responseData);
                    ecus += ";";
                }

                ecus.erase(ecus.end());
                setPropertyValue(propertyName(vi->getProperty().c_str()).c_str(), ecus.c_str());
            }
            break;
            
            default:
            {//ASCII 
                const std::vector<Pt::uint8_t>& responseData = request.getResponse();

                if( responseData.size() != 0)
                {
                    const std::string responseString = getStringFromResponse(responseData);
                    setPropertyValue(propertyName(vi->getProperty().c_str()).c_str() ,responseString.c_str());
                }
            }
            break;
        }
    }
}

std::string OBD2SourcePS::getStringFromResponse(const std::vector<Pt::uint8_t>& responseData)
{
    std::string resString;
    char* data  = new char[responseData.size()];
    memcpy(data, &responseData[1], responseData.size() - 1);
    data[responseData.size() - 1] = '\0';
    resString = data;
    delete data;
    return resString;
}

void OBD2SourcePS::checkSupportedPids()
{
    OBD2Request request(*_tpHandler, getAddress());

    Pt::uint8_t requestData[2];
    std::vector<Pt::uint8_t> supportedPids;

    for( Pt::uint8_t pid = 0; pid < 0xE1; pid += 0x20)
    {
        requestData[0] = 0x01;
        requestData[1] = pid;

        request.setData(requestData,2);
        const std::vector<Pt::uint8_t>& responseData = request.getResponse();

        if(responseData.size() == 0)
            break;

        for(Pt::uint32_t i = 1; i <  responseData.size(); ++i)
            supportedPids.push_back(responseData[i]);
    }

    const mps::core::Port*			 port  = _outputPorts->at(0);
    const mps::core::Sources&		 sources = port->sources();
    std::vector<mps::core::Signal*> source = sources[0];
    
    for(Pt::uint32_t src = 0; src < sources.size(); ++src)
    {
        const std::vector<mps::core::Signal*>& source = sources[src];

        for( Pt::uint32_t i = 0; i  < source.size(); ++i)
        {
            OBD2Signal* signal = (OBD2Signal*) source[i];

            if(signal->sid() != 0x01 || signal->sid() != 0x02)
            {
                signal->setValid(true);
                continue;
            }

            Pt::uint32_t byteNo = static_cast<Pt::uint32_t>( std::ceil(signal->pid() /8.0) - 1 );

            if(byteNo >= supportedPids.size() )
            {
                signal->setValid(true);
                continue;
            }

            Pt::uint8_t byte = supportedPids[byteNo];
            
            Pt::uint8_t mask = 0x80;
            const Pt::uint8_t maskShift = (signal->pid() % 8) - 1;
            
            mask >>= maskShift;

            bool valid = ((mask & byte) != 0);
            
            if(!valid)
            {
                mps::core::Message message( format(translate("Mp.OBD2.Err.Sig"),signal->name()), mps::core::Message::Output,
                                 mps::core::Message::Warning, Pt::System::Clock::getLocalTime());

                sendMessage(message);
            }

            signal->setValid(valid);
        }
    }
}

void OBD2SourcePS::onExitInstance()
{
    SynchSourcePS::onExitInstance();
}

void OBD2SourcePS::onDeinitialize()
{
    SynchSourcePS::onDeinitialize();

    if(_errorState)
        return;

    if( _tpHandler  != 0)
    {        
        delete _tpHandler;
        _tpHandler = 0;
    }

    if( _canHandler != 0)
    {
        delete _canHandler;
        _canHandler = 0;
    }

    if(_canDriver != 0)
    {
        _canDriver->close();
        delete _canDriver;
        _canDriver = 0;
    }

    if( _serDevice != 0)
    {
        _serDevice->close();
        delete _serDevice;
        _serDevice = 0;
    }
}

void OBD2SourcePS::onStart()
{
    if(_errorState)
        return;

    SynchSourcePS::onStart();


    _running = true;
    _dtcs.clear();
     _dtcsDDC.clear();

    for( Pt::uint32_t i = 0; i < _dataRecords.size(); ++i)
    {
        std::vector<Pt::uint8_t>& dataRecord = _dataRecords[i];
        memset(&dataRecord[0], 0, dataRecord.size());	
    }

    _tpHandler->start();
    _tpHandler->reset(true);		

    if(driver() == "mps-kline-rs232") //KLine
    {
        TPKLineHandler* tpHandler =(TPKLineHandler*) _tpHandler;
        tpHandler->stopCommunication(_normalFixedFunctional);
    }

    _runThread = new Pt::System::AttachedThread( Pt::callable(*this, &OBD2SourcePS::run));	
    _runThread->start();
}

std::vector<OBD2Signal*> OBD2SourcePS::getSignalsFromRequest(Pt::uint32_t sid, Pt::uint32_t pid, Pt::int8_t sensor)
{
    const mps::core::Port*			 port  = _outputPorts->at(0);
    const mps::core::Sources&		 sources = port->sources();
    std::vector<OBD2Signal*> pidSignals;

    for( Pt::uint32_t src = 0; src < sources.size(); ++src)
    {
        const std::vector<mps::core::Signal*>& source = sources[src];
        
        for( Pt::uint32_t sig = 0; sig < source.size(); ++sig)
        {
            OBD2Signal* signal = (OBD2Signal*) source[sig];
            if(sid == 0x05)
            {//Oxygen sensor
                if( signal->sid() == sid && signal->pid() == pid && signal->sensor() == sensor)
                    pidSignals.push_back(signal);
            }
            else
            {
                if( signal->sid() == sid && signal->pid() == pid)
                    pidSignals.push_back(signal);
            }
        }
    }
    
    return pidSignals;
}

bool OBD2SourcePS::isSignalInSequence(Pt::uint8_t sid, Pt::uint8_t pid, Pt::int8_t sensor, const std::vector<std::vector<Pt::uint8_t> >& sequense, const std::vector<Pt::uint8_t>& currentRequest)
{
    if( sensor == -1)
    {
        for(Pt::uint32_t i = 1; i < currentRequest.size(); ++i)
        {
            if( currentRequest[0] != sid)
                continue;

            if( currentRequest[i] == pid)
                return true;
        }
    }
    else
    {
        if(currentRequest.size() > 0)
        {
            if( currentRequest[0] == sid && 
                currentRequest[1] == pid && 
                currentRequest[2] == sensor)
                return true;
        }
    }

    for( Pt::uint32_t i = 0; i < sequense.size(); ++i)
    {
        const std::vector<Pt::uint8_t>& request = sequense[i];
        
        if(request[0] != sid)
            continue;

        if( sensor == -1)
        {
            for(Pt::uint32_t j = 1; j < request.size(); ++j)
            {
                if( request[j] == pid)
                    return true;
            }
        }
        else
        {
            if( request[1] == pid && request[2] == sensor)
                return true;
        }
    }
    return false;
}

void OBD2SourcePS::run()
{
    OBD2Request			 request(*_tpHandler, getAddress());
    const mps::core::Port*			 port  = _outputPorts->at(0);
    const mps::core::Sources&		 sources = port->sources();
    const Pt::uint32_t         pidsPerRequest = 4;
    
    std::vector<std::vector<Pt::uint8_t> > requestSequense;	
    std::vector<Pt::uint8_t> requestData;
    

    //Setup the request sequence for measurement data 
    for( Pt::uint32_t src = 0; src < sources.size(); ++src)
    {
        const std::vector<mps::core::Signal*>& source = sources[src];

        for(Pt::uint32_t i = 0; i < source.size(); ++i)
        {	
            const OBD2Signal* signal = (OBD2Signal*) source[i];

            if(!signal->valid())
                continue;//Not available
                    
            if(isSignalInSequence(signal->sid(), signal->pid(), signal->sensor(), requestSequense, requestData))
                continue; //We have allready this signal in sequense

            if( signal->sid() == 0x01 && signal->pid() == 0x01)
            {//MIL special handling
                if(requestData.size() != 0)
                {
                    requestSequense.push_back(requestData);
                    requestData.clear();
                }

                requestData.push_back(signal->sid());
                requestData.push_back(signal->pid());
                requestSequense.push_back(requestData);
                requestData.clear();
            }
            else if( signal->sid() == 0x03 || signal->sid() == 0x07)
            {//DTC

                if(requestData.size() != 0)
                {
                    requestSequense.push_back(requestData);
                    requestData.clear();
                }

                requestData.push_back(signal->sid());
                requestSequense.push_back(requestData);
                requestData.clear();
            }
            else if ( signal->sid() == 0x05)
            {//Oxygen sensor
                if(requestData.size() != 0)
                {
                    requestSequense.push_back(requestData);
                    requestData.clear();
                }
                requestData.push_back(signal->sid());

                requestData.push_back(signal->pid());
                requestData.push_back(signal->sensor());
                requestSequense.push_back(requestData);
                requestData.clear();
            }
            else if( signal->sid() == 0x02)
            {//Freeze frame
                if(requestData.size() != 0)
                {
                    requestSequense.push_back(requestData);
                    requestData.clear();
                }

                requestData.push_back(signal->sid());
                requestData.push_back(signal->pid());
                requestData.push_back(0x00); //Frame 00
                requestSequense.push_back(requestData);
                requestData.clear();
            }
            else
            {
                if(requestData.size() == 0)
                    requestData.push_back(signal->sid());

                requestData.push_back(signal->pid());

                if( requestData.size() == pidsPerRequest || requestData.size() >= (port->signalList()->size() + 1))
                {
                    requestSequense.push_back(requestData);
                    requestData.clear();
                }
            }
        }
    }

    while(_running)
    {
        for(Pt::uint32_t i = 0; (i < requestSequense.size() && _running); ++i)
        {			
            const std::vector<Pt::uint8_t>&	currentRequestData = requestSequense[i];

            request.setData(&currentRequestData[0], currentRequestData.size());
                        
            if(currentRequestData.size()  > 1 )
            {
                if( currentRequestData[0] == 0x01 && currentRequestData[1] == 0x01)
                {//MIL request special handling
                    Pt::uint8_t milStatus = 0;

                    //Send the request
                    request.request();

                    //Colect the responses
                    std::vector<OBD2Signal*> signals = getSignalsFromRequest(currentRequestData[0],currentRequestData[1]);
                    const OBD2Signal* signal = signals[0];
                    
                    for( Pt::uint32_t ecu = 0; ecu < _noOfEcus; ++ecu)
                    {
                        const std::vector<Pt::uint8_t>& responseData = request.readNextResponse();

                        if( responseData.size() == 0)
                            break;

                        const Pt::uint8_t mil = responseData[signal->byteOffset()];

                        if((mil & 0x80) != 0)
                            milStatus = 1;
                    }
                    
                    const Pt::uint32_t sigIdx = signalIndex(signal);
                    const Pt::uint32_t sourceIdx = port->sourceIndex(sigIdx);
                    const Pt::uint32_t offset = port->signalOffsetInSource(sigIdx);
                    std::vector<Pt::uint8_t>& dataRecord = _dataRecords[sourceIdx];
                    memcpy(&dataRecord[offset], &milStatus, signal->valueSize());
                }
                else
                {//Measurement, freez frame data, oxygen sensor test data

                    //Collect the data from response
                    Pt::uint32_t responseCount = currentRequestData.size() - 1;
                    
                    if(currentRequestData[0] == 0x02 || currentRequestData[0] == 0x05)
                        responseCount = 1;

                    //Send the request
                    request.request();

                    const std::vector<Pt::uint8_t>& responseData = request.readNextResponse();

                    if( responseData.size() == 0)
                        continue;						

                    Pt::uint32_t sid = currentRequestData[0];
                    Pt::int8_t sensor = -1;
                    
                    if( sid == 0x05)
                        sensor = currentRequestData[2];

                    Pt::uint32_t pidOffset = 0;

                    for( Pt::uint32_t j = 0; j < responseCount; ++j)
                    {
                        if( pidOffset >= responseData.size())
                            break;

                        Pt::uint32_t pid = responseData[pidOffset];

                        std::vector<OBD2Signal*> signals = getSignalsFromRequest(sid, pid, sensor);
                        Pt::uint32_t dataSize = 0;

                        for( Pt::uint32_t s = 0; s < signals.size(); ++s)
                        {
                            const OBD2Signal* signal = signals[s];
                            const Pt::uint32_t sigIdx = signalIndex(signal);
                            const Pt::uint32_t sourceIdx = port->sourceIndex(sigIdx);
                            std::vector<Pt::uint8_t>& dataRecord = _dataRecords[sourceIdx];

                            Pt::uint16_t data16;
                            Pt::uint16_t data32;
                            Pt::uint8_t* pdata = 0;

                            Pt::uint32_t readOffset = pidOffset + signal->byteOffset();
                            
                            switch(signal->valueSize())
                            {
                                case 2:
                                    data16 = (*(Pt::uint16_t*)&responseData[readOffset]);
                                    data16 = Pt::beToHost(data16);
                                    pdata = (Pt::uint8_t*)&data16;
                                break;

                                case 4:
                                    data32 = (*(Pt::uint32_t*)&responseData[readOffset]);
                                    data32 = Pt::beToHost(data32);
                                    pdata = (Pt::uint8_t*)&data32;
                                break;

                                default:
                                    pdata = (Pt::uint8_t*)&responseData[readOffset];
                                break;
                            }
                            
                            const Pt::uint32_t offset = port->signalOffsetInSource(sigIdx);
                            memcpy(&dataRecord[offset], pdata, signal->valueSize());
                            
                            dataSize = signal->totalDataSize();
                        }
                        pidOffset += (dataSize + 1);
                    }
                }
            }
            else
            {
                if( currentRequestData[0] == 0x03  || currentRequestData[0] == 0x07)
                {//DTC request special handling
                    //Request the number of DTC

                    Pt::uint32_t currentDTCs = 0;
                    
                    _dtcMutex.lock();
                    if(  currentRequestData[0] == 0x03)
                        currentDTCs = _dtcs.size();
                    else if (currentRequestData[0] == 0x07)
                        currentDTCs = _dtcsDDC.size();

                    _dtcMutex.unlock();

                    if(currentDTCs != 0)
                        continue;

                    std::vector<Pt::uint16_t> dtcs;
                    OBD2Request dtcCountRequest(*_tpHandler, getAddress());
                    Pt::uint8_t reqData[2];
                    reqData[0] = 0x01;
                    reqData[1] = 0x01;
                    dtcCountRequest.setData(reqData,2);
                    dtcCountRequest.request();
                    
                    Pt::uint32_t dtcCount = 0;

                    for(Pt::uint32_t ecu = 0; ecu < _noOfEcus; ++ecu)
                    {
                        const std::vector<Pt::uint8_t>& respData = request.readNextResponse();
                        
                        if(respData.size() == 0)
                            break;

                        dtcCount += (respData[1] & 0x7F);
                    }

                    //Send the request
                    if( dtcCount > 0)
                        request.request();

                    for(Pt::uint32_t dtc = 0; dtc < dtcCount; )
                    {
                        const std::vector<Pt::uint8_t>& respData = request.readNextResponse();
                        
                        if(respData.size() == 0)
                            break;
                        
                        for( Pt::uint32_t i = 1; i < respData.size(); i += 2)
                        {
                            //Pt::uint16_t* pdct = (Pt::uint16_t*) respData[i];
                            //dtcs.push_back(Pt::beToHost(*pdct));
                            dtc++;
                        }
                    }

                    _dtcMutex.lock();
                    
                    if(currentRequestData[0] == 0x03 )
                    {
                        for(Pt::uint32_t i = 0; i < dtcs.size(); ++i)
                            _dtcs.push_back(dtcs[i]);
                    }
                    else if( currentRequestData[0] == 0x07)
                    {
                        for(Pt::uint32_t i = 0; i < dtcs.size(); ++i)
                            _dtcsDDC.push_back(dtcs[i]);
                    }
                    _dtcMutex.unlock();
                }
            }
        }

        Pt::System::Thread::sleep(10);
    }
}

void OBD2SourcePS::onStop()
{
    if( _errorState)
        return;

    if(driver() == "mps-kline-rs232") //KLine
    {
        TPKLineHandler* tpHandler =(TPKLineHandler*) _tpHandler;
        tpHandler->stopCommunication(_normalFixedFunctional);
    }

    _tpHandler->stop();

    _running = false;

    if( _runThread != 0)
    {
        _runThread->join();
        delete _runThread;
        _runThread = 0;
    }
    SynchSourcePS::onStop();
}

void OBD2SourcePS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    if(_errorState)
        return;

    std::vector<Pt::uint8_t>& dataRecods = _dataRecords[sourceIdx];

    if( sourceIdx == _dtcSrcIdx && _dtcSignal != 0)
    {
        const mps::core::Port* port = _outputPorts->at(0);
        const Pt::uint32_t sigIdx = signalIndex(_dtcSignal);
        const Pt::uint32_t offset = port->signalOffsetInSource(sigIdx);

        _dtcMutex.lock();

        if(_dtcs.size() == 0)
        {
            Pt::uint16_t noDTC = 0x0000;//0x0143 = >P0143;
            memcpy(&dataRecods[offset], &noDTC, 2);
        }
        else
        {
            memcpy(&dataRecods[offset], &_dtcs[0], 2);
            _dtcs.erase(_dtcs.begin());
        }
        _dtcMutex.unlock();
    }

    if( sourceIdx == _dtcDDCSrcIdx && _dtcDDCSignal != 0)
    {
        const mps::core::Port* port = _outputPorts->at(0);
        const Pt::uint32_t sigIdx = signalIndex(_dtcDDCSignal);
        Pt::uint32_t offset = port->signalOffsetInSource(sigIdx);

        _dtcMutex.lock();

        if(_dtcs.size() == 0)
        {
            Pt::uint16_t noDTC = 0;
            memcpy(&dataRecods[offset], &noDTC, 2);
        }
        else
        {
            memcpy(&dataRecods[offset], &_dtcsDDC[0], 2);
            _dtcsDDC.erase(_dtcsDDC.begin());
        }
        _dtcMutex.unlock();
    }
    
    for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
        memcpy(&data[rec * dataRecods.size()], &dataRecods[0], dataRecods.size());
}

const TPDUAddress& OBD2SourcePS::getAddress()
{
    static TPDUAddress address(0xF1, 0x33, TPDUAddress::NormalFixedFunctional);

    switch(addressMode())
    {
        case Normal11Bit:
            address = TPDUAddress(0x7DF, false);
        break;

        case Normal29Bit:
        case Normal:
            address = _normalFixedFunctional;
        break;
    }

    return address;
}

}}
