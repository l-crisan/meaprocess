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
#include "OutputPS.h"
#include "OutBoard.h"
#include "LJOutSignal.h"
#include <vector>
#include <mps/Core/Signal.h>
#include <mps/Core/Port.h>
#include <mps/Core/SignalList.h>
#include <mps/Core/Message.h>
#include <Pt/System/Clock.h>
#include "DriverWrapper.h"
#include <sstream>

namespace mps{
namespace labjack{

OutputPS::OutputPS(void)
: _signals(0)
, _errorState(false)
{
}

OutputPS::~OutputPS(void)
{
    if( _signals != 0)
    {
        for(Pt::uint32_t i = 0; i < _signals->size(); ++i)
            delete _signals->at(i);

        delete _signals;
    }
}


void OutputPS::addObject(Object* object, const std::string& type, const std::string& subType)
{
    if( type == "Mp.LabJackU12.Output")
        _signals = (mps::core::ObjectVector<LJOutSignal*>*) object;
    else
        ProcessStation::addObject(object, type, subType);
}


void OutputPS::onInitInstance()
{
    ProcessStation::onInitInstance();

    std::map<Pt::int32_t,OutBoard*>::iterator boardIt;

    for(Pt::uint32_t i = 0; i < _signals->size(); ++i)
    {
        LJOutSignal* outSignal = _signals->at(i);
        boardIt = _boards.find(outSignal->board());
        
        OutBoard* board = 0;
        if(boardIt == _boards.end())
        {
            board =  new OutBoard();
            std::pair<Pt::int32_t,OutBoard*> pair(outSignal->board(), board);
            _boards.insert(pair);
        }
        else
        {
            board = boardIt->second;
        }

        board->addSignal(outSignal);
    }

    const mps::core::Port* port = _inputPorts->at(0);

    for(Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        const mps::core::Signal* signal = port->signalList()->at(i);

        for( boardIt = _boards.begin(); boardIt != _boards.end(); ++boardIt)
        {
            OutBoard* outBoard = boardIt->second;

            const std::vector<LJOutSignal*>& channels = outBoard->channels();

            for(Pt::uint32_t sigIdx = 0; sigIdx  < channels.size(); ++sigIdx)
            {
                if(signal->signalID() == channels[sigIdx]->signalID())
                {
                    Sig2BoardIt it = _sigId2Board.find(signal->signalID());

                    if( it == _sigId2Board.end())
                    {
                        std::vector<OutBoard*> boards;
                        boards.push_back(outBoard);
                        _sigId2Board[signal->signalID()] = boards;
                    }
                    else
                    {
                        it->second.push_back(outBoard);
                    }

                    break;
                }
            }
        }
    }
}

void OutputPS::onExitInstance()
{
    ProcessStation::onExitInstance();
    
    std::map<Pt::int32_t,OutBoard*>::iterator boardIt;

    for( boardIt = _boards.begin(); boardIt != _boards.end(); ++boardIt)
        delete boardIt->second;

    _boards.clear();
}

void OutputPS::onInitialize()
{
    ProcessStation::onInitialize();

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

        mps::core::Message message( format(translate("Mp.Labjack.Err.InDriver"), "1", this->getName()), mps::core::Message::Output, 
                              mps::core::Message::Error, Pt::System::Clock::getLocalTime());
        sendMessage(message);   
        return;
    }

    long ret = DriverWrapper::ListAll(&productIDList[0], &serialnumList[0], &localIDList[0], &powerList[0], &calMatrix[0], &numberFound, &reserved1, &reserved2);    
    
    _errorState =  false;
    std::map<Pt::int32_t,OutBoard*>::iterator boardIt;
    
    for( boardIt = _boards.begin(); boardIt != _boards.end(); ++boardIt)
    {
        if(!DriverWrapper::isBoardAvailable( serialnumList,static_cast<Pt::uint32_t>(numberFound), boardIt->second->serial()))
        {
            _errorState = true;

            std::stringstream ss;
            std::string strboard;
            ss<<boardIt->second->serial();
            ss>>strboard;            

            mps::core::Message message( format(translate("Mp.Labjack.Err.OutBoard"),strboard, this->getName()), mps::core::Message::Output, 
                                  mps::core::Message::Error, Pt::System::Clock::getLocalTime());
            sendMessage(message);
            continue;
        }

        boardIt->second->init();
    }
}

void OutputPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if(_errorState)
        return;

    const mps::core::Sources& sources = port->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];

    Pt::uint32_t rec = noOfRecords - 1;

    for( Pt::uint32_t sig = 0; sig < source.size(); ++sig )
    {
        const mps::core::Signal* signal = source[sig];

        Sig2BoardIt it = _sigId2Board.find(signal->signalID());
    
        if( it == _sigId2Board.end())
            continue;						

        const Pt::uint8_t* dataRecord = data + (rec * port->sourceDataSize(sourceIdx));
        const Pt::uint8_t* dataValue = (dataRecord + port->signalOffsetInSource(sourceIdx, sig));
                
        for(  Pt::uint32_t j = 0; j< it->second.size(); ++j )
            it->second[j]->writeData( signal, dataValue);
    }
}

void OutputPS::onStart()
{
    ProcessStation::onStart();

    if(_errorState)
        return;

    std::map<Pt::int32_t,OutBoard*>::iterator boardIt;

    for( boardIt = _boards.begin(); boardIt != _boards.end(); ++boardIt)
        boardIt->second->start();
}

void OutputPS::onStop()
{
    std::map<Pt::int32_t,OutBoard*>::iterator boardIt;

    for( boardIt = _boards.begin(); boardIt != _boards.end(); ++boardIt)
        boardIt->second->stop();

    ProcessStation::onStop();
}

}}