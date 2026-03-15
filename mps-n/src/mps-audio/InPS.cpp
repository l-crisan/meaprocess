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
#include "InPS.h"
#include "AudioSignal.h"
#include <mps/drv/audio/Driver.h>
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <mps/core/Message.h>
#include <Pt/System/Clock.h>
#include <fstream>
#include <algorithm>

namespace mps{
namespace audio{

InPS::InPS()
: FiFoSynchSourcePS()
, _errorState(false)
{	
}

InPS::~InPS()
{
}

void InPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();
    
    Pa_Initialize();

    const mps::core::Port* port = _outputPorts->at(0);

    const mps::core::Sources& sources =  port->sources();
    const std::vector<mps::core::Signal*>& source = sources[0];
    const mps::core::Signal* sig = source[0];

    const mps::core::SignalList* signalList = port->signalList();
    AudioSignal* signal;
    std::map<Pt::uint32_t,DeviceInfo*>::iterator it;
    DeviceInfo* devInfo = 0;
    
    for(Pt::uint32_t index = 0; index < signalList->size(); index++)
    {
        signal = (AudioSignal*) signalList->at(index);

        it = _sourceIdDeviceInfo.find(signal->sourceNumber() );

        if( _sourceIdDeviceInfo.end() == it)
        {
            devInfo = new DeviceInfo;
            devInfo->parent = this;
            devInfo->deviceID = signal->deviceID();
            devInfo->sourceIndex = port->sourceIndex(index);
            _sourceIdDeviceInfo[signal->sourceNumber()] = devInfo;
        }
        else
        {		
            devInfo = _sourceIdDeviceInfo[signal->sourceNumber()];			
        }

        devInfo->signals.push_back(signal);
    }		
}


int InPS::streamCallback(const void *input, void *output, unsigned long frameCount, const PaStreamCallbackTimeInfo *timeInfo, PaStreamCallbackFlags statusFlags, void *userData)
{
    DeviceInfo* devInfo = (DeviceInfo*) userData;
    devInfo->parent->writeData(devInfo, (const Pt::uint8_t*) input, frameCount);

    return paContinue;
}

void InPS::onInitialize()
{
    FiFoSynchSourcePS::onInitialize();
    
    std::map<Pt::uint32_t,DeviceInfo*>::iterator it = _sourceIdDeviceInfo.begin();

    mpsaudio_DeviceInfo* devices = 0;

    int deviceCount  = mpsaudio_detect(&devices);

    for( ; it != _sourceIdDeviceInfo.end(); ++it)
    {
        DeviceInfo* devInfo = it->second;

        if(devInfo->deviceID >= deviceCount)
        {
            mps::core::Message message(format(translate("Mp.Audio.Err.InputDev"), this->getName()), mps::core::Message::Output, mps::core::Message::Error, 
                            Pt::System::Clock::getLocalTime());

            sendMessage(message);
            _errorState = true;
            return;
        }

        AudioSignal* signal = (AudioSignal*) devInfo->signals[0];
        
        devInfo->channels = 0;

        for( Pt::uint32_t i = 0; i < devInfo->signals.size(); ++i)
        {
            AudioSignal* signal = (AudioSignal*) devInfo->signals[i];
            devInfo->channels = std::max(devInfo->channels, (Pt::uint8_t) signal->channelID());
        }

        double rate = devInfo->signals[0]->sampleRate();

        devInfo->parameter.channelCount = devInfo->channels;
        devInfo->parameter.device = devInfo->deviceID;
        
        if( devInfo->signals[0]->valueDataType() == mps::core::SignalDataType::VT_int16_t)
            devInfo->parameter.sampleFormat = paInt16;
        else
            devInfo->parameter.sampleFormat = paUInt8;

        devInfo->parameter.suggestedLatency =  Pa_GetDeviceInfo( devInfo->parameter.device )->defaultLowInputLatency;;
        devInfo->parameter.hostApiSpecificStreamInfo = 0;
        
        PaErrorCode retVal = (PaErrorCode) Pa_OpenStream(&devInfo->stream, &devInfo->parameter, 0, rate, 0, paDitherOff, &InPS::streamCallback, devInfo);
                
        if( retVal != paNoError)
        {
            devInfo->stream = 0;
            mps::core::Message message(format(translate("Mp.Audio.Err.InputDev"), this->getName()), mps::core::Message::Output, mps::core::Message::Error, 
                            Pt::System::Clock::getLocalTime());

            sendMessage(message);
            _errorState = true;
            return;
        }	
    }
}

void InPS::onStart()
{
    if(_errorState) 
        return;

    FiFoSynchSourcePS::onStart();

    std::map<Pt::uint32_t,DeviceInfo*>::iterator it = _sourceIdDeviceInfo.begin();

    for( ; it != _sourceIdDeviceInfo.end(); ++it)
    {
        DeviceInfo* devInfo = it->second;

        PaError retVal = Pa_StartStream(devInfo->stream);
        if(retVal != paNoError)
        {
            mps::core::Message message(format(translate("Mp.Audio.Err.InputDev"), this->getName()), mps::core::Message::Output, mps::core::Message::Error, 
                            Pt::System::Clock::getLocalTime());

            sendMessage(message);		
        }
    }	
}

void InPS::writeData(DeviceInfo* devInfo,const Pt::uint8_t* data, Pt::uint32_t noOfRecords)
{
    if( noOfRecords == 0)
        return;
    
    putRecords( devInfo->sourceIndex, 0, noOfRecords, data );
}

void InPS::onStop()
{
    if(_errorState) 
        return;

    FiFoSynchSourcePS::onStop();

    std::map<Pt::uint32_t,DeviceInfo*>::iterator it = _sourceIdDeviceInfo.begin();

    for( ; it != _sourceIdDeviceInfo.end(); ++it)
    {
        DeviceInfo* devInfo = it->second;
        Pa_StopStream(devInfo->stream);
    }
}

void InPS::onDeinitialize()
{
    FiFoSynchSourcePS::onDeinitialize();	

    std::map<Pt::uint32_t,DeviceInfo*>::iterator it = _sourceIdDeviceInfo.begin();

    for( ; it != _sourceIdDeviceInfo.end(); ++it)
    {
        DeviceInfo* devInfo = it->second;
        
        if( devInfo->stream != 0)
            Pa_CloseStream(devInfo->stream);
    }
}

void InPS::onExitInstance()
{
    FiFoSynchSourcePS::onExitInstance();

    std::map<Pt::uint32_t,DeviceInfo*>::iterator it =  _sourceIdDeviceInfo.begin();

    for( ; it != _sourceIdDeviceInfo.end(); it++)
        delete it->second;

    _sourceIdDeviceInfo.clear();

    Pa_Terminate();
}

}}