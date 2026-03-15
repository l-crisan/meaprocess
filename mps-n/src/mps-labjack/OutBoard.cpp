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
#include "OutBoard.h"
#include "DriverWrapper.h"
#include <mps/Core/Signal.h>

using namespace mps::core;

namespace mps{
namespace labjack{

OutBoard::OutBoard(void)
: _sampleTime(0)
{
}

OutBoard::~OutBoard(void)
{
}

void OutBoard::addSignal(LJOutSignal* signal)
{
    _channels.push_back(signal);
    setSerial(signal->board());
    _sampleTime = 1000/signal->rate();
}

void OutBoard::start()
{
    _running = true;
    _outThread = new Pt::System::AttachedThread( Pt::callable(*this, &OutBoard::output));
    _outThread->start();
}

void OutBoard::stop()
{
    if(_outThread != 0)
    {
        _running = false;
        _outThread->join();
        delete _outThread;
        _outThread = 0;
    }

    for(Pt::uint32_t i = 0; i < _analogData.size(); ++i)
        _analogData[i] = 0;

    for(Pt::uint32_t i = 0; i < _digitalData.size(); ++i)
        _digitalData[i] = 0;
}

void OutBoard::output()
{
    unsigned long count = 0;
    long  board   = serial();
    long  stateD  = 0;
    long  stateIO = 0;
    float out1    = 0;
    float out2    = 0;
    long  trisD   = 1;
    long  trisIO  = 1;
    long  outputD = 0;

    while(_running)
    {
        count   = 0;
        stateD  = 0;
        stateIO = 0;
        out1    = 0;
        out2    = 0;
        trisD   = 1;
        trisIO  = 1;
        outputD = 0;

        lock();

        for( Pt::uint32_t i = 0; i < _digitalChannels.size(); ++i)
        {
            long mask = _digitalData[i];

            LJOutSignal* signal = _digitalChannels[i];

            if(signal->channel() < 4)
                stateIO |= (mask << signal->channel());
            else
                stateD  |= (mask << (signal->channel() - 4));
        }

         if(_digitalChannels.size() != 0)
            DriverWrapper::DigitalIO (&board, 0, &trisD, trisIO, &stateD, &stateIO,1, &outputD );

        for(Pt::uint32_t i = 0; i < _analogChannels.size(); ++i)
        {
            LJOutSignal* signal = _analogChannels[i];

            if(signal->channel() == 0)
                out1 = _analogData[i];
            else
                out2 = _analogData[i];
        }

        if(_analogChannels.size() != 0)
            DriverWrapper::AOUpdate(&board, 0, 1, 1,  &stateD, &stateIO, 1, 0, &count, out1, out2);
        
        unlock();

        Pt::System::Thread::sleep(_sampleTime);
    }
}

void OutBoard::init()
{
    _analogScaling.clear();
    _analogChannels.clear();
    _digitalChannels.clear();

    for( Pt::uint32_t i = 0; i < _channels.size(); ++i)
    {
        LJOutSignal* chn = _channels[i];
        std::pair<double,double> scaling;
        
        switch(chn->channelType())
        {
            case 0: //Analog
                scaling.first = getFactor(chn->scaleMin(),chn->scaleMax());
                scaling.second = getOffset(chn->scaleMin(),chn->scaleMax());

                _analogScaling.push_back(scaling);
                _analogChannels.push_back(chn);
            break;

            case 1: //Digital
                _digitalChannels.push_back(chn);
            break;
        }
    }

    _analogData.resize(_analogChannels.size());
    _digitalData.resize(_digitalChannels.size());
}

double OutBoard::getFactor(double min, double max)
{
    Pt::int32_t rmax = 5;
    Pt::int32_t rmin = 0;
    return ((double)rmax - (double)rmin) /(max - min);
}

double OutBoard::getOffset(double min, double max)
{
    Pt::int32_t rmax =  5;
    Pt::int32_t rmin = 0;
    return ((double)(rmin * max) - (double)(rmax* min)) /(max - min);
}

void OutBoard::writeData(const mps::core::Signal* signal, const Pt::uint8_t* data)
{
    //Scale the value to physical
    double value = signal->scaleValue(data);

    //Analog signal
    for( Pt::uint32_t  index  = 0; index < _analogChannels.size(); ++index)
    {
        if( _analogChannels[index]->signalID() != signal->signalID())
            continue;

        _analogData[index] = static_cast<float>(value * _analogScaling[index].first + _analogScaling[index].second);
    }

    //Digital signal
    for( Pt::uint32_t  index  = 0; index < _digitalChannels.size(); ++index)
    {
        if( _digitalChannels[index]->signalID() != signal->signalID())
            continue;

        _digitalData[index] = (value != 0) ? 1 : 0;
    }
}

}}