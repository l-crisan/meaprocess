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
#ifndef MPS_AUDIO_AUDIOINPS_H
#define MPS_AUDIO_AUDIOINPS_H

#include <Pt/System/Thread.h>
#include <mps/core/Signal.h>
#include <mps/core/FiFoSynchSourcePS.h>
#include <map>
#include <vector>
#include <portaudio.h>

namespace mps {
namespace audio{

class InPS : public mps::core::FiFoSynchSourcePS
{
public:
    InPS();

    virtual ~InPS();
    
    void onInitInstance();

    void onExitInstance();

    void onInitialize();

    void onStart();

    void onStop();

    void onDeinitialize();

private:
    struct DeviceInfo
    {
        std::vector<mps::core::Signal*> signals;
        Pt::uint16_t        deviceID;
        Pt::uint8_t         channels;
        PaStream*           stream;
        PaStreamParameters  parameter;
        InPS*               parent;
        Pt::uint32_t        sourceIndex;
    };

    static int streamCallback(const void *input, void *output, unsigned long frameCount, const PaStreamCallbackTimeInfo *timeInfo, PaStreamCallbackFlags statusFlags, void *userData);

    void writeData(DeviceInfo* devInfo,const Pt::uint8_t* data, Pt::uint32_t noOfRecords);

    std::map<Pt::uint32_t,DeviceInfo*> _sourceIdDeviceInfo;

    bool _errorState;
};

}}


#endif
