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
#ifndef MPS_EVENT_EVENTPS_H
#define MPS_EVENT_EVENTPS_H

#include <Pt/Types.h>
#include <mps/core/ProcessStation.h>
#include <mps/core/ObjectVector.h>
#include "RtEvent.h"
#include <map>
#include <Pt/System/Process.h>

namespace mps{
namespace eventps{

class EventPS : public mps::core::ProcessStation
{
public:

    EventPS(void);

    virtual ~EventPS();

    void onInitInstance();

    void onExitInstance();

    void onInitialize();

    void onStart();

    void onStop();

    void onDeinitialize();

    void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);

    virtual PSType psType() const
    { return ReceptorPS; }

    void addObject(Object* object, const std::string& type, const std::string& name );

private:
    bool fireEvents(std::vector<RtEvent*>& events, double value);

    void resetLastData();

private:
    mps::core::ObjectVector<RtEvent*>* _events;
    typedef std::map<Pt::uint32_t,std::vector<RtEvent*> >::iterator Sig2EventIt;
    std::map<Pt::uint32_t,std::vector<RtEvent*> > _sig2Event;

    std::vector<std::vector<double> > _lastData;
    std::vector<std::vector<bool> > _firstSample;
    std::vector<Pt::System::Process*> _runningCmds;
};

}}

#endif
