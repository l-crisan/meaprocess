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
#include "ClassingPS.h"
#include <mps/core/SignalList.h>
#include <mps/core/Port.h>
#include <Pt/System/Clock.h>
#include <mps/core/Sources.h>

namespace mps{
namespace statistics{

using namespace std;

ClassingPS::ClassingPS()
: _classes(0)
{
}

ClassingPS::~ClassingPS()
{
}

void ClassingPS::onInitInstance()
{
    FiFoSynchSourcePS::onInitInstance();

    _triggerPort  =  (mps::core::TriggerPort*)  _inputPorts->at(1);
    
    _triggerPort->onStartTrigger += Pt::slot( *this, &ClassingPS::onStartTrigger);
    _triggerPort->onStopTrigger += Pt::slot( *this, &ClassingPS::onStopTrigger);
    _triggerPort->onEventTrigger += Pt::slot( *this, &ClassingPS::onEventTrigger);

    mps::core::Port* dataOutPort = _outputPorts->at(0);

    for( Pt::uint32_t i = 0; i < _classes->size(); ++i)
    {
        Classing* cls = _classes->at(i);
        Pt::uint32_t sigid = cls->signal();
        cls->setOutputSignal(getSignalByID(cls->outSignal(), dataOutPort->signalList()));
        Sig2ClassingIt it = _signal2Classing.find(sigid);

        //Out signal 2 classing
        std::pair<Pt::uint32_t, Classing*> outPair(cls->outSignal(), cls);
       _outSignal2Classing.insert(outPair);

        if( it == _signal2Classing.end())
        {
            std::vector<Classing*> classes;
            classes.push_back(cls);
            

            std::pair<Pt::uint32_t, std::vector<Classing*> > pair(sigid,classes);
            _signal2Classing.insert(pair);
        }
        else
        {
            it->second.push_back(cls);
        }
    }

    _resetPort = _inputPorts->at(2);
    _resetSrcIdex = -1;
    _resetSigIdx = -1;

    if(_resetPort->signalList()  != 0)
    {
        _resetSigIdx = (int)_resetPort->signalIndexInSource(0);
        _resetSrcIdex = (int)_resetPort->sourceIndex(0);
    }
}

mps::core::Signal* ClassingPS::getSignalByID(Pt::uint32_t id, const mps::core::SignalList* sigList)
{
    for(Pt::uint32_t i = 0; i < sigList->size(); ++i)
    {
        mps::core::Signal* signal = sigList->at(i);
        if( signal->signalID() == id)
            return signal;
    }

    return 0;
}
    
void ClassingPS::onInitialize()
{
    FiFoSynchSourcePS::onInitialize();
    
    for( Pt::uint32_t i = 0; i < _classes->size(); ++i)
    {
        Classing* cls = _classes->at(i);
        cls->init();
    }
}

void ClassingPS::onStart()
{
    _triggerPort->start();
    _lastResetValue = 0;

    for( Pt::uint32_t i = 0; i < _classes->size(); ++i)
    {
        Classing* cls = _classes->at(i);
        cls->calc( _triggerPort->triggerType() == mps::core::TriggerPort::StopTrigger || _triggerPort->triggerType() == mps::core::TriggerPort::NoTrigger);
        cls->start();
    }

    FiFoSynchSourcePS::onStart();
}

void ClassingPS::store( bool on )
{
    for( Pt::uint32_t i = 0; i < _classes->size(); ++i)
    {
        Classing* cls = _classes->at(i);
        cls->calc(on);
    }
}

void ClassingPS::onStartTrigger()
{ 
    store(true); 
}

void ClassingPS::onStopTrigger()
{ 
    store(false); 
}

void ClassingPS::onEventTrigger()
{ 
    store(true); 
}


void ClassingPS::onStop()
{
    for( Pt::uint32_t i = 0; i < _classes->size(); ++i)
    {
        Classing* cls = _classes->at(i);
        cls->stop();
    }

    FiFoSynchSourcePS::onStart();
}

void ClassingPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    if( portIdx == 0)
    {
        const mps::core::Port* port = _outputPorts->at(portIdx);
        const mps::core::Sources& sources = port->sources();
        const std::vector<mps::core::Signal*>& source  = sources[sourceIdx];

        for(Pt::uint32_t sig = 0; sig < source.size(); ++sig)
        {
            const mps::core::Signal* signal = source[sig];
            OutSig2ClassingIt it = _outSignal2Classing.find(signal->signalID());
            
            if( it == _outSignal2Classing.end() )
                continue;

            Classing* cls =  it->second;
            Pt::uint32_t classingData = cls->getNextData();
            putValue(cls->outputSignal() ,0, (Pt::uint8_t*) &classingData);
        }
    }

    FiFoSynchSourcePS::onSourceEvent(noOfRecords, maxNoOfSamples, sourceIdx, portIdx, data);
}

void ClassingPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source  = sources[sourceIdx];

    if(port->portNumber() == 0)
    {
        const Pt::uint32_t recSize = port->sourceDataSize(sourceIdx);

        for(Pt::uint32_t sig = 0; sig < source.size(); ++sig)
        {
            const mps::core::Signal* signal = source[sig];
            Sig2ClassingIt it = _signal2Classing.find(signal->signalID());
            
            if( it == _signal2Classing.end() )
                continue;

            for(Pt::uint32_t rec = 0; rec <  noOfRecords; ++rec)
            {
                const Pt::uint32_t offseInRec = port->signalOffsetInSource(sourceIdx, sig); 
                const double value = signal->scaleValue(&data[recSize*rec + offseInRec]);

                for(Pt::uint32_t c = 0; c < it->second.size(); ++c)
                {
                    Classing* cls =  it->second[c];
                    cls->classify(value);
                }
            }
        }
     }

    if(_resetPort == port && static_cast<Pt::uint32_t>(_resetSrcIdex) == sourceIdx)
    {
       const mps::core::Signal* signal = source[_resetSigIdx];
       const Pt::uint32_t recSize = port->sourceDataSize(sourceIdx);

       for(Pt::uint32_t rec = 0; rec <  noOfRecords; ++rec)
       {
           const double value = signal->scaleValue(&data[recSize*rec + _resetSigIdx]);

           if( value != 0 && _lastResetValue == 0)
           {
               for(Pt::uint32_t i = 0; i < _classes->size(); ++i)
                   _classes->at(i)->reset();
           }

           _lastResetValue = value;
       }
    }
}

void ClassingPS::onDeinitialize()
{
    for( Pt::uint32_t i = 0; i < _classes->size(); ++i)
    {
        Classing* cls = _classes->at(i);
        cls->close();
    }

    FiFoSynchSourcePS::onDeinitialize();
}

void ClassingPS::onExitInstance()
{
    FiFoSynchSourcePS::onExitInstance();

    if( _classes != 0)
        delete _classes;
}

void ClassingPS::addObject(Object* obj, const std::string& type, const std::string& subType)
{
    if( type == "Mp.Stat.Classings")
    {
        _classes = (mps::core::ObjectVector<Classing*>*) obj;
    }
    else
    {
        FiFoSynchSourcePS::addObject(obj, type, subType);
    }
}

}}
