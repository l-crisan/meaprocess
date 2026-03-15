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
#include <mps/drv/audio/Driver.h>
#include <portaudio.h>
#include <string.h>
#include <stdlib.h>
#include <memory.h>

extern "C"
{

static mpsaudio_DeviceInfo devices[50];

MPS_AUDIO_PORT_API Pt::int32_t mpsaudio_detect(mpsaudio_DeviceInfo** data)
{
    PaError retVal = Pa_Initialize();

    if(retVal != paNoError )
    {
        return 0;
    }

    int count = Pa_GetDeviceCount();

    if( count >= 50)
    {
        Pa_Terminate();
        return 0;
    }

    for( int i = 0; i <  count; ++i)
    {
        const PaDeviceInfo* devInfo = Pa_GetDeviceInfo(i);
        devices[i].hostApi = i;
        devices[i].maxInputChannels = devInfo->maxInputChannels;
        devices[i].maxOutputChannels = devInfo->maxOutputChannels;
        strcpy(devices[i].name, devInfo->name);
    }

    *data = &devices[0];
    Pa_Terminate();

    return count;
}

}
