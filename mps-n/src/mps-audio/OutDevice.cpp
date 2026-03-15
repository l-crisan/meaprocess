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
#include "OutDevice.h"
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>

namespace mps{
namespace audio{

OutDevice::OutDevice()
: _start(false)
{
}

OutDevice::~OutDevice()
{
}

void OutDevice::addChannel(OutChannel* chn)
{
    _channels.push_back(chn);
}

bool OutDevice::init(const mps::core::Port* inPort)
{
    std::vector<double>  rates;
    double               maxSampleRate = 0.0;
    double               minSampleRate = 10000000000;
    OutChannel*          channel = _channels[0];
    int                  deviceID = channel->deviceID();
    const mps::core::Signal* signal	= signalById(inPort, channel->signalID());

    //Setup the HW-parameter	
    _sampleRate = channel->sampleRate();	

    PaStreamParameters parameters;	

    parameters.channelCount = (int) _channels.size();
    parameters.device = deviceID;		

    if( channel->dataType() == mps::core::SignalDataType::VT_int16_t)
    {
        _resolution = 16;
        parameters.sampleFormat = paInt16;
    }
    else
    {
        _resolution = 8;
        parameters.sampleFormat = paUInt8;
    }

    parameters.suggestedLatency =  Pa_GetDeviceInfo( deviceID )->defaultLowOutputLatency;
    parameters.hostApiSpecificStreamInfo = 0;

    //Calculate the sample rate scaling und value scaling
    for( Pt::uint32_t i = 0; i < _channels.size(); ++i)
    {
        OutChannel* cahnnel = _channels[i];
        const mps::core::Signal* signal = signalById(inPort, cahnnel->signalID());
        _signals.push_back(signal);

        std::pair<double,double> scale;
        scale.first = getFactor(signal->physMin(),signal->physMax());
        scale.second = getOffset(signal->physMin(),signal->physMax());
        _signalScaling.push_back(scale);
        
        rates.push_back(signal->sampleRate());	
    }	

    //Init the scaling records
    std::vector<Pt::uint32_t> itemSizes(rates.size(), _resolution /8);
    _dataRecords.init(_sampleRate*5,itemSizes,rates, _sampleRate);	
    _outputBuffer.init(_sampleRate*2, _dataRecords.elementSize());
    
    //Init HW
    return Pa_OpenStream(&_stream, 0,  &parameters, _sampleRate, _sampleRate/4, 0, &OutDevice::streamCallback, this) == paNoError;
}

void OutDevice::deinit()
{
    Pa_CloseStream(_stream);
    _dataRecords.reset();
    _signals.clear();
    _signalScaling.clear();
}

const mps::core::Signal* OutDevice::signalById(const mps::core::Port* port, Pt::uint32_t id)
{
    const mps::core::Signal* signal;

    for( Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        signal = port->signalList()->at(i);
        
        if( signal->signalID() == id)
            return signal;
    }

    return 0;
}

double OutDevice::getFactor(double min, double max)
{
    Pt::int32_t rmax;
    Pt::int32_t rmin;

    switch(_resolution)
    {
        case 8:
            rmax = 255;
            rmin = 0;
        break;

        case 16:
            rmax = 32767;
            rmin = -32768;
        break;
    }
    
    return (rmax - rmin) / (max - min);
}

double OutDevice::getOffset(double min, double max)
{
    Pt::int32_t rmax;
    Pt::int32_t rmin;

    switch(_resolution)
    {
        case 8:
            rmax = 255;
            rmin = 0;
        break;
 
        case 16:
            rmax = 32767;
            rmin = -32768;
        break;
    }
    
    return (rmin * max - rmax* min) / (max - min);
}

void OutDevice::startEvent()
{
    Pt::System::Thread::sleep(100);
    _start = true;	
    _startThread = 0;
}

void OutDevice::start()
{
    _start = false;	
    _startThread  = new Pt::System::AttachedThread(Pt::callable(*this, &OutDevice::startEvent));
    _startThread->start();
}

int OutDevice::streamCallback(const void *input, void *output, unsigned long frameCount, const PaStreamCallbackTimeInfo *timeInfo, PaStreamCallbackFlags statusFlags, void *userData)
{
    OutDevice* me = (OutDevice*) userData;
    me->outputData((Pt::uint8_t*) output, frameCount);
    return paContinue;
}

bool OutDevice::writeData(const mps::core::Signal* signal, const Pt::uint8_t* data, bool lastSample)
{
    //Scale the value to physical
    double value = signal->scaleValue(data);
    
    //Determinate the signal index in output device.
    Pt::uint32_t index = 0;

    for(index  = 0; index < _signals.size(); ++index)
    {
        if( _signals[index]->signalID() == signal->signalID())
        {			
            //Scale the physical value to output device resolution and write the value into the record.
            switch(_resolution)
            {
                case 8:
                {
                    Pt::uint8_t  scaledData;
                    scaledData = static_cast<Pt::uint8_t>(value * _signalScaling[index].first + _signalScaling[index].second);
                    
                    _dataRecords.insert(&scaledData,index);
                }
                break;
                
                case 16:
                {
                    Pt::int16_t  scaledData;
                    scaledData = static_cast<Pt::int16_t>(value * _signalScaling[index].first + _signalScaling[index].second);

                    _dataRecords.insert((Pt::uint8_t*)&scaledData, index);
                }
                break;
            }
        }
    }


    if( lastSample)
    {
        if(_start)
        {
            Pa_StartStream(_stream);
            _start = false;
        }
        else
        {
            const Pt::uint8_t* data1 = 0;
            const Pt::uint8_t* data2 = 0;
    
            Pt::uint32_t data1count = 0;	
            Pt::uint32_t data2count = 0;	

            //Copy the records into the output buffer.
            _dataRecords.get(&data1, data1count, &data2, data2count);
        
            if(data1count != 0)
                _outputBuffer.insert(data1, data1count);

            if(data2count != 0)
                _outputBuffer.insert(data2, data2count);
        }
    }

    return true;
}

void OutDevice::outputData(Pt::uint8_t* out, Pt::uint32_t frames)
{
    try
    {
        const Pt::uint8_t* data  = _outputBuffer.get(frames,frames);
        memcpy(out, data, frames);		 
        _outputBuffer.next(frames);
    }
    catch( const std::runtime_error& e)
    {
        std::cerr<<e.what();
    }
}

void OutDevice::stop()
{
    Pa_StopStream(_stream);
    _dataRecords.reset();
    _noOfSamples = 0;

    if(_startThread != 0)
        delete _startThread;

    _start = false;
}

}}