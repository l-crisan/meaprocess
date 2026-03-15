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
#include <mps/core/SignalList.h>
#include <mps/core/ObjectVector.h>
#include <mps/core/RuntimeEngine.h>
#include <mps/core/Signal.h>
#include "ObjectFactoryManager.h"

namespace mps{
namespace core{

SignalList::SignalList(void)
: _dataSize(0)
{
   registerProperty( "signalRef", *this, &SignalList::signalRef, &SignalList::addSignalRef );    
}

SignalList::~SignalList(void)
{
}

Pt::uint32_t SignalList::signalRef() const
{
    return 0;
}

void SignalList::addSignalRef( Pt::uint32_t ref )
{
    _signalRefIDList.push_back( ref );
}

void SignalList::onExitInstance()
{
    if( _signalRefIDList.size() != 0 )
        return; //a ref signal list.

    //Delete the signals.
    Signal*	signal;

    for( Pt::uint32_t index = 0; index < size(); index++)
    {
        signal = this->at(index);
        signal->onExitInstance();
        delete signal;
    }

    clear();
}

void SignalList::onInitInstance()
{
    Signal*	signal;
    Pt::uint32_t	index; 
    
    ObjectFactoryManager& factoryMng = ObjectFactoryManager::instance();

    //If is a reference signal list instance the signals.
    for( index = 0; index < _signalRefIDList.size(); ++index )
    {
        signal = (Signal* ) factoryMng.getObjectByID( _signalRefIDList[index] );
        this->push_back( signal );
    }

    if( _signalRefIDList.size() == 0)
    {
        for( index = 0; index < size(); index++ )
        {
            signal = this->at( index );
            signal->onInitInstance();
        }
    }

    _dataSize = 0;
    for( index = 0; index < size(); index++ )
    {
        signal = this->at( index );
        _sigDataOffsetArr.push_back( _dataSize );
        _dataSize += signal->valueSize();
    }
}

Pt::uint32_t SignalList::getSignalIndex(const Signal* signal) const
{
    for(Pt::uint32_t  index = 0; index < size(); index++ )
    {
        if(this->at( index ) == signal)
            return index;
    }

    return 0;
}

}}
