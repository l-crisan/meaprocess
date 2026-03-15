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
#include <mps/diatp/CANHandler.h>
#include <mps/diatp/TPDU.h>

namespace mps{
namespace diatp{

CANHandler::CANHandler(mps::can::drv::Driver& driver,  Pt::uint8_t blockSize, Pt::uint32_t separationTime, TimeUnit::Unit timeUnit)	
: _running(false)
, _separationTime(separationTime)
, _blockSize(blockSize)
, _timeUnit(timeUnit)
, _driver(driver)
, _messages()
, _dataAvailCondition()
, _thread(0)
, _mutex()
, _waitForIDMap()
{

}


CANHandler::~CANHandler()
{
    stop();
}


void CANHandler::restart()
{
    stop();
    start();
}


void CANHandler::start()
{
    if(_running)
        return;

    _messages.clear();
    _running = true;
    _thread = new Pt::System::AttachedThread(Pt::callable(*this, &CANHandler::run));
    _thread->start();
}


void CANHandler::reset(bool hardware)
{
    Pt::System::MutexLock lock(_mutex);
    _messages.clear();
    _driver.reset(hardware);
}


Pt::uint32_t CANHandler::separationTime() const
{
    return _separationTime;
}


Pt::uint8_t	 CANHandler::blockSize() const
{
    return _blockSize;
}


TimeUnit::Unit	CANHandler::timeUnit() const
{
    return _timeUnit;
}


void CANHandler::stop()
{
    if(_running)
    {
        _running = false;
        _driver.wake();
        _thread->join();
        delete _thread;
    }
}


void CANHandler::wake()
{
    _dataAvailCondition.signal();
    std::map<Pt::uint32_t, Pt::System::Condition*>::iterator it = _waitForIDMap.begin();

    for(; it !=  _waitForIDMap.end(); ++it)
        it->second->signal();
}


void CANHandler::run()
{
    while(_running)
    {
        if(!_driver.wait(1000000))
            continue;

        if(!_running)
            break;

        mps::can::drv::Message msg;

        while(_driver.receive(msg))
        {
            Pt::System::MutexLock lock(_mutex);

            _messages.push_back(msg);

            if(_messages.size() > 1024)
                _messages.erase(_messages.begin());

            std::map<Pt::uint32_t, Pt::System::Condition*>::iterator it = _waitForIDMap.find(msg.identifier());

            if( it != _waitForIDMap.end())
                it->second->signal();	
            
            _dataAvailCondition.signal();

            if(!_running)
                break;
        }
    }
}


void CANHandler::send(mps::can::drv::Message& msg)
{
    Pt::System::MutexLock lock(_mutex);
    _driver.send(msg);
}


bool CANHandler::waitForID(Pt::uint32_t id, size_t timeout)
{
    Pt::System::MutexLock lock(_mutex);

    for( size_t i = 0; i < _messages.size(); ++i)
    {
        if( _messages[i].identifier() == id)
            return true;
    }

    Pt::System::Condition  condition;
    
    std::pair<Pt::uint32_t, Pt::System::Condition*> pair(id, &condition);
    
    _waitForIDMap.insert(pair);	

    bool retVal = condition.wait(_mutex, timeout);	

    _waitForIDMap.erase(id);

    return retVal;
}


bool CANHandler::wait(size_t timeout)
{
    Pt::System::MutexLock lock(_mutex);

    if( _messages.size() != 0)
        return true;

    return _dataAvailCondition.wait(_mutex, timeout);
}


mps::can::drv::Driver& CANHandler::driver()
{
    return _driver;
}


bool CANHandler::readMessageByID(Pt::uint32_t id, mps::can::drv::Message& msg)
{
    Pt::System::MutexLock lock(_mutex);

    for( size_t i = 0; i < _messages.size(); ++i)
    {
        if( _messages[i].identifier() == id)
        {
            msg = _messages[i];
            _messages.erase(_messages.begin() + i);
            return true;
        }
    }
    return false;
}


bool CANHandler::readMessage(mps::can::drv::Message& msg)
{
    Pt::System::MutexLock lock(_mutex);

    if(_messages.size() == 0)
        return false;
    
    msg = _messages[0];
    _messages.erase(_messages.begin());
    return true;
}

}}
