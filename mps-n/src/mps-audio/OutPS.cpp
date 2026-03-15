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
#include "OutPS.h"
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Port.h>
#include <mps/core/Message.h>
#include <Pt/System/Clock.h>
#include <sstream>
#include "OutDevice.h"

namespace mps{
namespace audio{

OutPS::OutPS(void)
: _errorState(false)
, _channels(0)
{
}

OutPS::~OutPS(void)
{
    if( _channels != 0)
    {
        for( Pt::uint32_t i = 0; i < _channels->size(); ++i)
            delete _channels->at(i);

        delete _channels;
    }
}

void OutPS::addObject( Object* object, const std::string& type, const std::string& name )
{
    if( type == "Mp.Audio.Channels")
        _channels = (mps::core::ObjectVector<OutChannel*>*) object;
    else
        ProcessStation::addObject(object, type, name);
}

void OutPS::onInitInstance()
{
    ProcessStation::onInitInstance();

    Pa_Initialize();

    //Create the output devices and map the channels to this devices
    std::map<int, OutDevice*>::iterator dev2ChnIt;
    for( Pt::uint32_t i = 0; i < _channels->size(); ++i)
    {
        OutChannel* channel = _channels->at(i);

        dev2ChnIt = _dev2ChnMap.find(channel->deviceID());
        
        if( dev2ChnIt == _dev2ChnMap.end())
        {
            OutDevice* device = new OutDevice();
            device->addChannel(channel);
            std::pair<int, OutDevice*>		pair(channel->deviceID(), device);
            _dev2ChnMap.insert(pair);
        }
        else
        {
            dev2ChnIt->second->addChannel(channel);
        }
    }

    //Create the signal id to device map
    for(dev2ChnIt = _dev2ChnMap.begin() ; dev2ChnIt != _dev2ChnMap.end(); ++dev2ChnIt)
    {
        for( Pt::uint32_t i = 0; i < dev2ChnIt->second->channels().size(); ++i)
        {
            OutChannel* channel = dev2ChnIt->second->channels().at(i);
            Sig2DevIt it = _sigId2Dev.find(channel->signalID());

            if( it == _sigId2Dev.end())
            {
                std::vector<OutDevice*> devices;
                devices.push_back(dev2ChnIt->second);

                std::pair<Pt::uint32_t, std::vector<OutDevice*> > pair(channel->signalID(), devices);
                _sigId2Dev.insert(pair);
            }
            else
            {
                it->second.push_back(dev2ChnIt->second);
            }
        }
    }

}

void OutPS::onExitInstance()
{
    ProcessStation::onExitInstance();

    std::map<int, OutDevice*>::iterator dev2ChnIt;

    for( dev2ChnIt = _dev2ChnMap.begin(); dev2ChnIt != _dev2ChnMap.end(); ++dev2ChnIt)
    {
        OutDevice* device = dev2ChnIt->second;
        delete device;	
    }

    _dev2ChnMap.clear();
    Pa_Terminate();
}

void OutPS::onInitialize()
{
    ProcessStation::onInitialize();

    _errorState = false;
    const mps::core::Port* port  = _inputPorts->at(0);
    
    std::map<int, OutDevice*>::iterator dev2ChnIt;

    for( dev2ChnIt = _dev2ChnMap.begin(); dev2ChnIt != _dev2ChnMap.end(); ++dev2ChnIt)
    {
        OutDevice* device = dev2ChnIt->second;

        if( !device->init(port))
        {
            mps::core::Message message( format(translate("Mp.Audio.Err.OutputDev"), this->getName()), mps::core::Message::Output, mps::core::Message::Error,
                             Pt::System::Clock::getLocalTime());

            sendMessage(message);
            _errorState = true;
            break;
        }
    }
}

void OutPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if(_errorState)
        return;

    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];

    const mps::core::Signal* signal = source[0];

    for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec )
    {
        for( Pt::uint32_t sig = 0; sig < source.size(); ++sig )
        {
            const mps::core::Signal* signal = source[sig];

            Sig2DevIt it = _sigId2Dev.find(signal->signalID());
        
            if( it == _sigId2Dev.end())
                continue;						

            const Pt::uint8_t* dataRecord = data + (rec * port->sourceDataSize(sourceIdx));
            const Pt::uint8_t* dataValue = (dataRecord + port->signalOffsetInSource(sourceIdx, sig));
                    
            for(  Pt::uint32_t j = 0; j< it->second.size(); ++j )
            {
                if(!it->second[j]->writeData( signal, dataValue, (rec == noOfRecords - 1)))
                {
        
                    mps::core::Message message( format(translate("Mp.Audio.Err.Data"), this->getName()),  mps::core::Message::Output, mps::core::Message::Warning,
                                     Pt::System::Clock::getLocalTime());

                    sendMessage(message);
                }
            }
        }
    }
}

void OutPS::onStart()
{
    if( _errorState )
        return;

    std::map<int, OutDevice*>::iterator dev2ChnIt;

    for( dev2ChnIt = _dev2ChnMap.begin(); dev2ChnIt != _dev2ChnMap.end(); ++dev2ChnIt)
    {
        OutDevice* device = dev2ChnIt->second;
        device->start();		
    }
}

void OutPS::onStop()
{
    std::map<int, OutDevice*>::iterator dev2ChnIt;

    for( dev2ChnIt = _dev2ChnMap.begin(); dev2ChnIt != _dev2ChnMap.end(); ++dev2ChnIt)
    {
        OutDevice* device = dev2ChnIt->second;
        device->stop();
    }
}

void OutPS::onDeinitialize()
{
    if(_errorState)
        return;
    
    std::map<int, OutDevice*>::iterator dev2ChnIt;

    for( dev2ChnIt = _dev2ChnMap.begin(); dev2ChnIt != _dev2ChnMap.end(); ++dev2ChnIt)
    {
        OutDevice* device = dev2ChnIt->second;
        device->deinit();
    }
}

}}
