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
#include "AudioObjectFactory.h"

#include <Pt/System/Plugin.h>
#include <mps/core/ObjectVector.h>
#include "InPS.h"
#include "OutPS.h"
#include "AudioSignal.h"
#include "OutChannel.h"
#include "mps-audio.h"

namespace mps{
namespace audio{

AudioObjectFactory::AudioObjectFactory()
{
}

AudioObjectFactory::~AudioObjectFactory()
{
}

std::string AudioObjectFactory::resourceID() const
{
    return "mps-audio";
}

mps::core::Object* AudioObjectFactory::createObject( const Pt::String& type, const Pt::String& sybtype, Pt::uint32_t id )
{
    if(type == L"Mp.PS.Audio.In")
    {
        return new InPS();
    }
    else if( type == L"Mp.PS.Audio.Out")
    {
        return new OutPS();		
    }
    else if( sybtype == L"Mp.Sig.Audio" )
    {
        return new AudioSignal(id);
    }
    else if( type == L"Mp.Audio.Channels")
    {
        return new mps::core::ObjectVector<OutChannel*>();		
    }
    else if( type == L"Mp.Audio.Channel")
    {
        return new OutChannel();		
    }
    return 0;
}

extern "C"
{
    MPSAUDIO_API mps::core::ObjectFactory* mpsGetFactory()
    {
        static AudioObjectFactory factory;
        return &factory;
    }
}

}}