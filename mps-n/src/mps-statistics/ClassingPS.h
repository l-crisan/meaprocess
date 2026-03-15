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
#ifndef MPS_STATISTICS_IIRFILTERPS_H
#define MPS_STATISTICS_IIRFILTERPS_H

#include <map>
#include <mps/core/FiFoSynchSourcePS.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include "Classing.h"
#include <vector>
#include <mps/core/TriggerPort.h>
#include <Pt/Connectable.h>

namespace mps{
namespace statistics{

class ClassingPS : public mps::core::FiFoSynchSourcePS, public Pt::Connectable
{
public:
    ClassingPS();
    virtual ~ClassingPS();

    virtual void onInitInstance();

    virtual void onInitialize();

    virtual void onStart();

    virtual void onStop();

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);

    virtual void onDeinitialize();

    virtual void onExitInstance();

    virtual void addObject(Object* obj, const std::string& type, const std::string& subType);

protected:
    virtual void onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data);

private:

    void store( bool on );

    void onStartTrigger();

    void onStopTrigger();

    void onEventTrigger();

    mps::core::Signal* getSignalByID(Pt::uint32_t id, const mps::core::SignalList* sigList);

private:
    mps::core::ObjectVector<Classing*>* _classes;
    typedef std::map<Pt::uint32_t,std::vector<Classing*> >::iterator Sig2ClassingIt;
    std::map<Pt::uint32_t,std::vector<Classing*> > _signal2Classing;
    std::map<Pt::uint32_t,Classing*> _outSignal2Classing;
    typedef std::map<Pt::uint32_t,Classing*>::iterator OutSig2ClassingIt;
    //Trigger
    mps::core::TriggerPort* _triggerPort;
    mps::core::Port*        _resetPort;
    int       _resetSrcIdex;
    int      _resetSigIdx;
    double _lastResetValue;
};

}}
#endif
