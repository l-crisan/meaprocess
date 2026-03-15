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
#ifndef MPS_AUDIO_AUDIOOUTPS_H
#define MPS_AUDIO_AUDIOOUTPS_H

#include <Pt/System/Mutex.h>
#include <map>
#include <vector>
#include <mps/core/ProcessStation.h>
#include <mps/core/CircularBuffer.h>
#include <mps/core/ObjectVector.h>
#include "OutChannel.h"

namespace mps {

class Signal;

namespace audio{

class OutDevice;

class OutPS : public mps::core::ProcessStation
{
public:
    OutPS(void);
    virtual ~OutPS(void);

    void onInitInstance();

    void onExitInstance();

    void onInitialize();

    void onStart();

    void onStop();

    void onDeinitialize();

    void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);
    
    virtual mps::core::ProcessStation::PSType psType() const
    { return mps::core::ProcessStation::ReceptorPS; }

    void addObject(Object* object, const std::string& type, const std::string& name );

private:
    typedef std::map<Pt::uint32_t,std::vector<OutDevice*> > Sig2DevMap;
    typedef  Sig2DevMap::iterator  Sig2DevIt;

    mps::core::ObjectVector<OutChannel*>* _channels;
    std::map<int, OutDevice*>            _dev2ChnMap;
    Sig2DevMap                           _sigId2Dev;
    bool                                 _errorState;
};

}}

#endif
