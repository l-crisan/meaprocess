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
#ifndef MPS_AUDIOOUTDEVICE_H
#define MPS_AUDIOOUTDEVICE_H

#include <mps/core/Object.h>
#include <mps/core/Port.h>
#include <mps/core/CircularBuffer.h>
#include <mps/core/RecordBuilder.h>
#include "OutChannel.h"
#include <string>
#include <vector>
#include "portaudio.h"
#include <Pt/System/Condition.h>
namespace mps{
namespace audio{

class OutDevice
{

public:
    OutDevice();

    virtual ~OutDevice();

    void addChannel(OutChannel* channels);

    bool init(const mps::core::Port* inPort);

    void start();

    bool writeData(const mps::core::Signal* signal, const Pt::uint8_t* data, bool lastSample);

    void stop();

    void deinit();

    const std::vector<OutChannel*>& channels() const
    {
        return _channels;
    }

private: //Helper
    static int streamCallback(const void *input, void *output, unsigned long frameCount, const PaStreamCallbackTimeInfo *timeInfo, PaStreamCallbackFlags statusFlags, void *userData);

    void outputData(Pt::uint8_t* out, Pt::uint32_t frames);

    double getFactor(double min, double max);

    double getOffset(double min, double max);

    const mps::core::Signal* signalById(const mps::core::Port* port, Pt::uint32_t id);

    void startEvent();

private: //Member
    //Output device description
    Pt::uint8_t	 _resolution;
    Pt::uint32_t _sampleRate;
    
    //Signals description
    std::vector<OutChannel*>               _channels;
    std::vector<const mps::core::Signal*>  _signals;
    std::vector<std::pair<double,double> >  _signalScaling;
    
    //Record building
    mps::core::RecordBuilder    _dataRecords;
    std::vector<Pt::uint32_t>   _writeIndex;
    mps::core::CircularBuffer   _outputBuffer;
    Pt::uint32_t                _noOfSamples;
    std::vector<bool>           _updateMask;

    //HW I/O
    PaStream*                   _stream;

    Pt::System::AttachedThread* _startThread;
    bool _start;
};

}}

#endif
