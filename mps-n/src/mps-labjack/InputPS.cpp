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
#include "InputPS.h"
#include <mps/core/Port.h>
#include <mps/core/SignalList.h>
#include <mps/core/Message.h>
#include <Pt/System/Clock.h>
#include "DriverWrapper.h"
#include <sstream>
#include <map>

using namespace mps::core;

namespace mps{
namespace labjack{

typedef std::map<long,InBoard*>::iterator BoardsIt;


InputPS::InputPS(void)
: FiFoSynchSourcePS()
, _errorState(false)
{
}


InputPS::~InputPS(void)
{
}


void InputPS::onInitialize()
{
    FiFoSynchSourcePS::onInitialize();

    const Port* port = _outputPorts->at(0);
    const Sources& sources = port->sources();
    long productIDList[127];
    long serialnumList[127];
    long localIDList[127];
    long powerList[127];
    long calMatrix[127][20];
    long numberFound = 0;
    long reserved1 = 0;
    long reserved2 = 0;

    memset(&productIDList[0],0,sizeof(long)*127);
    memset(&serialnumList[0],0,sizeof(long)*127);
    memset(&localIDList[0],0,sizeof(long)*127);
    memset(&powerList[0],0,sizeof(long)*127);
    memset(&calMatrix[0][0],0,sizeof(long)*127*20);

    _errorState = false;

    if(!DriverWrapper::loadDriver())
    {
        _errorState = true;
        mps::core::Message message( format(translate("Mp.Labjack.Err.InDriver"), "1", this->getName()), Message::Output,
                              Message::Error, Pt::System::Clock::getLocalTime());
        sendMessage(message);
        return;
    }

    long ret = DriverWrapper::ListAll(&productIDList[0], &serialnumList[0], &localIDList[0], &powerList[0], &calMatrix[0], &numberFound, &reserved1, &reserved2);    
    
    for( Pt::uint32_t srcIndex = 0; srcIndex < sources.size(); ++srcIndex)
    {
        const std::vector<Signal*>& source = sources[srcIndex];
        
        for( Pt::uint32_t sigIndex = 0; sigIndex < source.size(); ++sigIndex)
        {
            LJSignal* signal = (LJSignal*) source[sigIndex];

            if(!DriverWrapper::isBoardAvailable(serialnumList, static_cast<Pt::uint32_t>(numberFound), signal->board()))
            {
                _errorState = true;
                std::stringstream ss;
                std::string strboard;
                ss<<signal->board();
                ss>>strboard;
                mps::core::Message message( format(translate("Mp.Labjack.Err.InBoard"), strboard, this->getName()), Message::Output, 
                                      Message::Error, Pt::System::Clock::getLocalTime());
                sendMessage(message);   
                continue;
            }

            BoardsIt boardsIt = _boards.find((long)signal->board());
            
            if( boardsIt == _boards.end())
            {
                InBoard* board = new InBoard(this);
                board->addSignal(signal,srcIndex);
                std::pair<long, InBoard*> pair(signal->board(), board); 
                _boards.insert(pair);
            }
            else
            {
                boardsIt->second->addSignal(signal, srcIndex);
            }
        }
    }

    if( _errorState)
    {
        for( BoardsIt it = _boards.begin(); it != _boards.end(); ++it)
            delete it->second;

        _boards.clear();
        return;
    }

    for( BoardsIt it = _boards.begin(); it != _boards.end(); ++it)
        it->second->init();
}


void InputPS::onStart()
{
    if( _errorState == true)
        return;

    FiFoSynchSourcePS::onStart();

    for( BoardsIt it = _boards.begin(); it != _boards.end(); ++it)
        it->second->start();
}


void InputPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    for( BoardsIt it = _boards.begin(); it != _boards.end(); ++it)
        it->second->scanData(noOfRecords, sourceIdx);

    FiFoSynchSourcePS::onSourceEvent(noOfRecords, maxNoOfSamples, sourceIdx, portIdx, data);
}


void InputPS::onStop()
{
    for( BoardsIt it = _boards.begin(); it != _boards.end(); ++it)
        it->second->stop();

    FiFoSynchSourcePS::onStop();
}


void InputPS::onDeinitialize()
{
    for( BoardsIt it = _boards.begin(); it != _boards.end(); ++it)
    {
        it->second->deinit();
        delete it->second;
    }

    _boards.clear();

    FiFoSynchSourcePS::onDeinitialize();
    DriverWrapper::freeDriver();
}


}}
