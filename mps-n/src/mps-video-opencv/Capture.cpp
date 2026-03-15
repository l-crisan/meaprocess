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
#include "Capture.h"
#include <sstream>

namespace mps {
namespace video{
namespace opencv{

Capture::Capture(VideoSignal* signal, mps::core::FiFoSynchSourcePS* ps)
: _ps(ps)
, _thread(0)
, _frameBuffer()
, _signal(signal)
, _running(false)
, _handle(0)
{
}

Capture::~Capture()
{
}


std::string Capture::formatMessage(const char* msg)
{
    std::stringstream ss;
    ss<<msg<< "Device id "<<_signal->deviceID();
    return ss.str();
}


void Capture::init()
{
    int card = _signal->deviceID();
    
    _info = mpsopencv_detect(card);
    
    if( _info.Error != 0)
        throw std::runtime_error(formatMessage("Open video source failed."));

    if( _info.Width != _signal->width())
        throw std::runtime_error(formatMessage("Wrong width parameter."));

    if( _info.Height != _signal->height())
        throw std::runtime_error(formatMessage("Wrong height parameter."));

    _handle = mpsopencv_open(card);
    
    if( _handle == 0)
        throw std::runtime_error(formatMessage("Open video source failed."));

    mpsopencv_setFrameRate(_handle, (int) _signal->sampleRate());

    _frameBuffer.resize(_info.Width * _info.Height*3);
}


void Capture::start()
{
    if( _handle == 0)
        return;

    memset(&_frameBuffer[0], 0, _frameBuffer.size());
    _running = true;	
    _thread = new Pt::System::AttachedThread(Pt::callable(*this, &Capture::run));
    _thread->start();
}


void Capture::run()
{
    while(_running)
    {	
        int size = mpsopencv_readFrame(_handle, (char*) &_frameBuffer[0]);
        
        if( size == 0)
        {
            Pt::System::Thread::sleep(10);
            continue;
        }

        _ps->lock();
        _ps->putValue(_signal, 0, &_frameBuffer[0]);
        _ps->unlock();
    }
}


void Capture::stop()
{
    _running = false;
    if(_thread != 0)
    {
        _thread->join();
        delete _thread;
        _thread = 0;
    }
}


void Capture::deinit()
{
    if( _handle == 0)
        return;
         
    mpsopencv_close(_handle);
    _handle = 0;
}

}}}
