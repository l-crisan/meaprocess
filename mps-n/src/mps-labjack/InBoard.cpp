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
#include "InBoard.h"
#include <Pt/System/Thread.h>
#include <mps/core/FiFoSynchSourcePS.h>
#include "DriverWrapper.h"
#include <algorithm>

using namespace mps::core;

namespace mps{
namespace labjack{

InBoard::InBoard(FiFoSynchSourcePS* ps)
: _ps(ps)
, _scanThread(0)
, _running(false)
, _scanModeStream(false)
, _counterSignal(0)
{
}

InBoard::~InBoard(void)
{
}

void InBoard::init()
{
    std::map<Pt::uint32_t, std::vector<const LJSignal*>>::iterator src2sigIt;
    src2sigIt = _src2signals.begin();

    for( ; src2sigIt != _src2signals.end(); ++src2sigIt )
    {
        Pt::uint32_t recordSize = 0;
        std::vector<const LJSignal*>& signals = src2sigIt->second;
        
        for( Pt::uint32_t sig = 0; sig < signals.size(); ++sig)
        {
            const LJSignal* signal = signals[sig];
            std::pair<Pt::uint32_t,Pt::uint32_t> srcOffset(src2sigIt->first, recordSize);
            std::pair<Pt::uint32_t, std::pair<Pt::uint32_t,Pt::uint32_t>> pair(signal->signalID(),srcOffset);
            _sigId2SrcOffset.insert(pair);
            recordSize += signal->valueSize();
        }
        std::vector<Pt::uint8_t> recordData(recordSize);
        std::pair<Pt::uint32_t,std::vector<Pt::uint8_t>> pair(src2sigIt->first,recordData);
        _data.insert(pair);
    }    

    //Prepare analog scan sequency
    for(Pt::uint32_t i = 0; i < _analogSignals.size(); ++i)
    {
        const LJSignal* signal = _analogSignals[i];
        
        if( signal->channelMode() == LJSignal::SingleEnded) 
        {//Single ended
            _channels.push_back(signal->channel());
            _gains.push_back(0);
        }
        else
        {//Differential
            _channels.push_back(8 + signal->channel());
            _gains.push_back(signal->gain());
        }
    }    

    if(_channels.size() == 3 || _channels.size() == 7)
    {
        _channels.push_back(getFreeChannel(_channels, _analogSignals[0]->channelMode() ==  LJSignal::SingleEnded));
        _gains.push_back(_gains[_gains.size() -1]);
    }

}

void InBoard::start()
{
    _boardID = serial();

    if( _counterSignal != 0)
    { //Reset the counter.
        long stateD = 0;
        long stateIO = 0;
        unsigned long counterValue = 0;
        DriverWrapper::Counter(&_boardID, 0, &stateD, &stateIO, 1, 0, &counterValue);
    }

    if(_scanModeStream && _analogSignals.size() != 0)
    {
        float scanRate = static_cast<float>(_analogSignals[0]->sampleRate());
        long ret = DriverWrapper::AIStreamStart( &_boardID, 0, 0, 0, 1, _channels.size(), &_channels[0], &_gains[0], &scanRate, 0, 0, 0);
        ret++;
    }

    _running = true;
    _scanThread = new Pt::System::AttachedThread(Pt::callable(*this, &InBoard::scan));
    _scanThread->start();
}

void InBoard::scan()
{
    std::map<Pt::uint32_t, std::vector<Pt::uint8_t>>::iterator dataIt;
    std::map<Pt::uint32_t, std::pair<Pt::uint32_t,Pt::uint32_t>>::iterator sigId2SrcOffIt;
    long   overVoltage = 0;
    bool   singleEnded = false;
    long   stateIO[4096];
    long   reserved = 0;
    long   ljScanBacklog = 0;
    Pt::uint32_t records = 0;

    if(_scanModeStream)
        records = static_cast<Pt::uint32_t>(_analogSignals[0]->sampleRate()/20);				

    while( _running )
    {		
        if( _scanModeStream)
        {
            memset(&stateIO[0], 0, sizeof(long) * 4096);
            memset(&_streamData[0][0], 0, sizeof(float) * 4096 * 4);
            
            long result = DriverWrapper::AIStreamRead( _boardID, records, 10, &_streamData[0], &stateIO[0], &reserved, &ljScanBacklog, &overVoltage );
            
            if( result == 0)
            {		
                _ps->lock();
                for(Pt::uint32_t rec = 0; rec < records ; ++rec)
                {
                    for( Pt::uint32_t sig = 0; sig < _analogSignals.size(); ++sig)
                    {
                        const LJSignal* signal = _analogSignals[sig];
                
                        sigId2SrcOffIt = _sigId2SrcOffset.find(signal->signalID());
                    
                        dataIt = _data.find(sigId2SrcOffIt->second.first);			
                        _ps->putValue(signal,0,(Pt::uint8_t*)&_streamData[rec][sig]);
                    }
                }
                _ps->unlock();
            }			
        }
        else
        {
            lock();

            if( _analogSignals.size() != 0)
                scanAnalog();

            if( _digitalSignals.size() != 0)
                scanDigital();		

            if( _counterSignal != 0)
                scanCounter();

            unlock();

            Pt::System::Thread::sleep(10);
        }		
        
    }
}

void InBoard::scanCounter()
{
    long stateD = 0;
    long stateIO = 0;
    unsigned long counterValue = 0;

    long ret = DriverWrapper::Counter(&_boardID, 0, &stateD, &stateIO, 0, 0, &counterValue);

    if( ret == 0)
    {
        Pt::System::MutexLock lock(_mutex);

        std::map<Pt::uint32_t, std::pair<Pt::uint32_t,Pt::uint32_t>>::iterator sigId2SrcOffIt;
        std::map<Pt::uint32_t, std::vector<Pt::uint8_t>>::iterator dataIt;

        sigId2SrcOffIt = _sigId2SrcOffset.find(_counterSignal->signalID());

        dataIt = _data.find(sigId2SrcOffIt->second.first);
        memcpy(&dataIt->second[sigId2SrcOffIt->second.second], &counterValue, sizeof(Pt::uint32_t));
    }
}

void InBoard::scanDigital()
{
    long trisD   = 0;
    long trisIO  = 0;
    long stateD  = 0;
    long stateIO = 0;
    long outputD = 0;

    long result = DriverWrapper::DigitalIO(&_boardID, 0, &trisD, trisIO, &stateD, &stateIO, 0, &outputD);

    _mutex.lock();
    std::map<Pt::uint32_t, std::vector<Pt::uint8_t>>::iterator dataIt;
    std::map<Pt::uint32_t, std::pair<Pt::uint32_t,Pt::uint32_t>>::iterator sigId2SrcOffIt;

    for(Pt::uint32_t i = 0; i < _digitalSignals.size() ; ++i)
    {
        const LJSignal* signal = _digitalSignals[i];
        Pt::uint8_t signalData = 0;

        if(signal->channel() < 4)
        {
            long mask = 1;
            mask = mask << signal->channel();
            signalData = static_cast<Pt::uint8_t>((stateIO & mask) >>signal->channel());
        }
        else
        {
            long mask = 1;
            mask = mask << (signal->channel() - 4);
            signalData = static_cast<Pt::uint8_t>((outputD & mask) >> (signal->channel() - 4));
        }

        sigId2SrcOffIt = _sigId2SrcOffset.find(signal->signalID());
        
        dataIt = _data.find(sigId2SrcOffIt->second.first);
        memcpy(&dataIt->second[sigId2SrcOffIt->second.second], &signalData, sizeof(Pt::uint8_t));                    
    }
    _mutex.unlock();

}

void InBoard::scanAnalog()
{  
    std::map<Pt::uint32_t, std::vector<Pt::uint8_t>>::iterator dataIt;
    std::map<Pt::uint32_t, std::pair<Pt::uint32_t,Pt::uint32_t>>::iterator sigId2SrcOffIt;
    std::vector<long>   channels;
    std::vector<long>   overVoltage(4,0);
    std::vector<long>   gains;
    std::vector<float> dataReaded;
    long chnCount = std::min(size_t(4),_channels.size());
    long stateIO[4] = {0 ,0 ,0, 0 };

    dataReaded.resize(4,0);

    long result = DriverWrapper::AISample( &_boardID, 0, &stateIO[0], 0, 1, chnCount,  &_channels[0], 
                                           &_gains[0], 0, &overVoltage[0], &dataReaded[0] );

    if( result == 0)
    {
        Pt::System::MutexLock lock(_mutex);

        for(Pt::uint32_t i = 0; i < std::min(_analogSignals.size(),size_t(4)) ; ++i)
        {
            const LJSignal* signal = _analogSignals[i];
        
            sigId2SrcOffIt = _sigId2SrcOffset.find(signal->signalID());
            
            dataIt = _data.find(sigId2SrcOffIt->second.first);
            memcpy(&dataIt->second[sigId2SrcOffIt->second.second], &dataReaded[i], sizeof(float));
        }
    }

    if( _channels.size() > 4)
    {
        memset(&dataReaded[0], 0, sizeof(float) * 4);

        result = DriverWrapper::AISample( &_boardID, 0, &stateIO[0], 0, 1, _channels.size() - 4,  &_channels[4], 
                                          &_gains[0], 0, &overVoltage[0], &dataReaded[0] );

        if( result == 0)
        {
            Pt::System::MutexLock lock(_mutex);

            for(Pt::uint32_t i = 4; i < _analogSignals.size() ; ++i)
            {
                const LJSignal* signal = _analogSignals[i];
            
                sigId2SrcOffIt = _sigId2SrcOffset.find(signal->signalID());
                
                dataIt = _data.find(sigId2SrcOffIt->second.first);
                memcpy(&dataIt->second[sigId2SrcOffIt->second.second], &dataReaded[i-4], sizeof(float));                    
            }
        }
    }
}

long InBoard::getFreeChannel(std::vector<long>& channels, bool singleEnded)
{
    if( singleEnded)
    {
        for(long i = 0; i < 8; ++i)
        {
            bool found = false;
            for(Pt::uint32_t j = 0; j < channels.size(); ++j)
            {
                if(channels[j] == i)
                {
                    found = true;
                    break;
                }
            }

            if(!found)
                return i;
        }
    }
    else
    {
        for(long i = 8; i < 12; ++i)
        {
            bool found = false;
            for(Pt::uint32_t j = 0; j < channels.size(); ++j)
            {
                if(channels[j] == i)
                {
                    found = true;
                    break;
                }
            }

            if(!found)
                return i;
        }
    }
    return 0;
}

void InBoard::stop()
{
    if( _scanThread != 0)
    {
        if(_scanModeStream)
        {
            long err = DriverWrapper::AIStreamClear(_boardID);
            err++;
        }

        _running = false;
        _scanThread->join();
        delete _scanThread;
        _scanThread = 0;
    }
}

void InBoard::deinit()
{
    _src2signals.clear();
    _data.clear();
    _counterSignal = 0;
}

void InBoard::addSignal(const LJSignal* signal, Pt::uint32_t srcIdx)
{
    std::map<Pt::uint32_t, std::vector<const LJSignal*>>::iterator src2sigIt;        
    src2sigIt = _src2signals.find(srcIdx);
    setSerial(signal->board());

    _scanModeStream = signal->scanMode() == LJSignal::Stream;

    if( src2sigIt == _src2signals.end())
    {
        std::vector<const LJSignal*> signals;
        signals.push_back(signal);
        std::pair<Pt::uint32_t,std::vector<const LJSignal*>> pair(srcIdx, signals);
        _src2signals.insert(pair);
    }
    else
    {
        src2sigIt->second.push_back(signal);
    }

    switch(signal->channelType()) 
    {
        case LJSignal::Analog:
            _analogSignals.push_back(signal);
        break;

        case LJSignal::Digital:
            _digitalSignals.push_back(signal);
        break;

        case LJSignal::Counter:
            _counterSignal = signal;
        break;
    }
}

void InBoard::scanData(Pt::uint32_t records, Pt::uint32_t sourceIdx)
{
    if(!_scanModeStream)
    {
        Pt::System::MutexLock lock(_mutex);
        
        std::map<Pt::uint32_t, std::vector<Pt::uint8_t>>::iterator dataIt;
        dataIt = _data.find(sourceIdx);

        if(dataIt == _data.end())
            return;
        
        for( Pt::uint32_t i = 0; i < records; ++i)
            _ps->putRecords(sourceIdx, 0, 1, &dataIt->second[0]);		
    }
}

}}