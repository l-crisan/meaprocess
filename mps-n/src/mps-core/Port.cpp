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
#include <mps/core/Port.h>
#include <mps/core/RuntimeEngine.h>
#include <mps/core//Signal.h>
#include "ObjectFactoryManager.h"

namespace mps{
namespace core{

Port::Port()
: _name("")
, _connectedToPortsID()
, _connectedToPorts()
, _signalList(0)
, _inputPort(false)
, _processStation(0)
, _portNumber(0)
, _signalListID(0)
, _sources()
{
    registerProperty( "name", *this, &Port::getName, &Port::setName );	
    registerProperty( "number", *this, &Port::portNumber, &Port::setPortNumber );
    registerProperty( "refLinkToPort", *this, &Port::dummy, &Port::connectToPort);
    registerProperty( "refSignalList", *this, &Port::getSignalListID, &Port::setSignalListID );	
}

Port::~Port(void)
{ }

void Port::onInitInstance()
{
    ObjectFactoryManager& objFactoryMng = ObjectFactoryManager::instance();
    
    for( Pt::uint32_t index = 0; index < _connectedToPortsID.size(); index++ )
    {
        if( _connectedToPortsID[index] == 0 )
            continue;

        Port* port = (Port*)objFactoryMng.getObjectByID(_connectedToPortsID[index]);

        if ( port == 0)
            continue;

        _connectedToPorts.push_back(port);
     }
    
    _signalList = (const SignalList*) objFactoryMng.getObjectByID(_signalListID);

    if( _signalList == 0 )
        return;
    
    createSources();
}

void Port::onExitInstance()
{ 
    _sources.clear();
}

Signal* Port::getSignalByID(Pt::uint32_t id) const
{
    for( Pt::uint32_t i = 0; i < _signalList->size(); ++i)
    {
        Signal* sig = _signalList->at(i);

        if( sig->signalID() == id)
            return sig;
    }

    return 0;
}


void Port::setParentPS( ProcessStation* processStation )
{ 
    _processStation = processStation; 
}

void Port::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Pt::uint8_t* data)
{
    if(_inputPort)
    { //send to process station
        _processStation->onUpdateDataValue( noOfRecords, sourceIdx, this, data );
    }
    else
    {//send to connected ports.
        for (Pt::uint32_t index = 0; index < _connectedToPorts.size(); index++)
        {
            if(_connectedToPorts[index] != 0)
                _connectedToPorts[index]->onUpdateDataValue( noOfRecords,sourceIdx, data);
        }
    }
}

Pt::uint32_t Port::portNumber() const
{ 
    return _portNumber; 
}

void Port::setPortNumber( Pt::uint32_t port)
{ 
    _portNumber = port; 
}

const std::string& Port::getName() const
{ 
    return _name; 
}

void Port::setName(const std::string& name)
{ 
    _name = name; 
}

void Port::connectToPort( Pt::uint32_t portId )
{ 
    if ( portId != 0)
        _connectedToPortsID.push_back( portId ); 
}

Pt::uint32_t Port::dummy() const
{ 
    return 0; 
}

void Port::setSignalListID( Pt::uint32_t id)
{ 
    _signalListID = id; 
}

Pt::uint32_t Port::getSignalListID() const
{ 
    return _signalListID; 
}

void Port::setInputPort( bool input )
{ 
    _inputPort = input; 
}

bool Port::isInputPort() const 
{ 
    return _inputPort; 
}

bool Port::isConnected() const
{
    return (_connectedToPorts.size() != 0);
}

Pt::uint64_t Port::signalSourceID(const Signal* signal)
{
    Pt::uint32_t source = signal->sourceNumber();
    double samplerate = signal->sampleRate();

    Pt::uint64_t sourceID = source;
    sourceID  = sourceID << 32;
    sourceID |= (Pt::uint64_t) samplerate;
    return sourceID;
}

void Port::createSources()
{
    //Setup a temporary source signal multimap.
    Signal* signal = 0;	
        
    std::map<Pt::uint64_t,std::vector<Signal*> > sourceMap;
    std::map<Pt::uint64_t,std::vector<Signal*> >::iterator it;

    for( Pt::uint32_t sig = 0; sig < _signalList->size(); sig++)
    {
        signal = _signalList->at(sig);
        Pt::uint64_t id = signalSourceID(signal);

        it = sourceMap.find(id);

        if( it == sourceMap.end())
        {
            std::vector<Signal*> signals;
            signals.push_back(signal);
            std::pair<Pt::uint64_t, std::vector<Signal*> > pair(id, signals);
            sourceMap.insert(pair);
        }
        else
        {
            it->second.push_back(signal);
        }
    }

    _sources.clear();

    it = sourceMap.begin();

    for( ;it != sourceMap.end(); ++it)
        _sources.push_back(it->second);

    _sourceDataSize.resize(_sources.size());
    _signalOffsetInSrc.resize(_signalList->size());
    _signalIdxInSrc.resize(_signalList->size());	
    _sourceIndex.clear();
    _signalOffsetInSrcByIdx.resize(_sources.size());
    _sourceIndex.resize(_signalList->size());

    for( Pt::uint32_t src = 0; src < _sources.size(); ++src)
    {
        const std::vector<Signal*>& source = _sources[src];
        Pt::uint32_t sourceSize  = 0;
        
        for( Pt::uint32_t i = 0; i < source.size(); ++i)
        {
            signal = source[i];
            const Pt::uint32_t sigIndex = _signalList->getSignalIndex(signal);
            _signalOffsetInSrc[sigIndex] = sourceSize;
            _signalIdxInSrc[sigIndex] = i;
            _signalOffsetInSrcByIdx[src].push_back(sourceSize);
            sourceSize += signal->valueSize();
            _sourceIndex[sigIndex] = src;
        }
        _sourceDataSize[src] = sourceSize;
    }
}

Pt::uint32_t Port::sourceDataSize(Pt::uint32_t srcIndex) const
{
    return _sourceDataSize[srcIndex];
}

const Sources& Port::sources() const
{
    return _sources;
}

}}
