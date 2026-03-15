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
#include "EventPS.h"
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Sources.h>
#include <mps/core/Port.h>
#include <mps/core/Message.h>
#include <Pt/System/Clock.h>
#include <Pt/System/Process.h>
#include <Pt/System/FileInfo.h>
#include <ctype.h> 
#include "SystemOutput.h"
#include <algorithm>

namespace mps{
namespace eventps{

EventPS::EventPS(void)
{
}

EventPS::~EventPS()
{
	for( Pt::uint32_t i = 0; i < _events->size(); ++i)
	{
		delete _events->at(i);
	}

	_events->clear();
	delete _events;
}

void EventPS::onInitInstance()
{
	ProcessStation::onInitInstance();

	for( Pt::uint32_t i = 0; i < _events->size(); ++i)
	{
		RtEvent* ev = _events->at(i);
		Sig2EventIt it = _sig2Event.find(ev->signal());
		
		if( it == _sig2Event.end())
		{
			std::vector<RtEvent*> events;
			events.push_back(ev);
			std::pair<Pt::uint32_t, std::vector<RtEvent*> > pair(ev->signal(), events);
			_sig2Event.insert(pair);
		}
		else
		{
			it->second.push_back(ev);
		}
	}	
}

void EventPS::onExitInstance()
{
	ProcessStation::onExitInstance();
}

void EventPS::onInitialize()
{
	ProcessStation::onInitialize();

	const mps::core::Port* port = _inputPorts->at(0);

	const mps::core::Sources& sources = port->sources();
	
	_lastData.resize(sources.size());
	_firstSample.resize(sources.size());

	for( Pt::uint32_t src = 0; src < sources.size(); ++src)
	{
		const std::vector<mps::core::Signal*>& source = sources[src];
		_lastData[src].resize(source.size());
		_firstSample[src].resize(source.size());
	}
}

void EventPS::onStart()
{
	ProcessStation::onStart();
	resetLastData();
}

void EventPS::resetLastData()
{
	for( Pt::uint32_t i = 0; i < _lastData.size(); ++i)
	{
		std::vector<double>& data = _lastData[i];
	
		for( Pt::uint32_t j = 0; j < data.size(); ++j)
		{
			_firstSample[i][j] = true;
			data[j] = 0.0;
		}
	}
}

void EventPS::onStop()
{
	ProcessStation::onStop();
	for(Pt::uint32_t i = 0; i < _runningCmds.size(); ++i)
	{
		try
		{
			if(_runningCmds[i]->state() != Pt::System::Process::Finished)
				_runningCmds[i]->kill();
		}
		catch(const std::exception& ex)
		{
			std::cerr<<ex.what()<<std::endl;
		}

		delete _runningCmds[i];
	}
	_runningCmds.clear();
	
}

void EventPS::onDeinitialize()
{
	ProcessStation::onDeinitialize();
}

void EventPS::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
	const mps::core::Sources& sources = port->sources();
	const std::vector<mps::core::Signal*>& source = sources[sourceIdx];
	
	mps::core::Signal* signal = source[0];

	Pt::uint32_t recSize = port->sourceDataSize(sourceIdx);

	Pt::uint32_t offset = 0;
	for( Pt::uint32_t i = 0; i < source.size(); ++i)
	{
		signal = source[i];
		Sig2EventIt it = _sig2Event.find(signal->signalID());

		if( it != _sig2Event.end())
		{
			for( Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
			{
				const Pt::uint8_t* datap = &data[rec* recSize + offset];
				double value = signal->scaleValue(datap);
				
				if( _firstSample[sourceIdx][i])
				{
					fireEvents(it->second, value);
					_firstSample[sourceIdx][i] = false;
				}
				else
				{
					if(_lastData[sourceIdx][i] != value)
						fireEvents(it->second, value);
				}

		  	_lastData[sourceIdx][i] = value;
			}
		}

		offset += signal->valueSize();
	}
}

bool EventPS::fireEvents(std::vector<RtEvent*>& events, double value)
{
	bool fired = false;

	for(Pt::uint32_t i = 0; i < events.size(); ++i)
	{
		RtEvent* ev = events[i];
		
		switch(ev->operation())
		{
			case RtEvent::NotEq:				
				if( value == ev->limit())
					continue;
			break;
			
			case RtEvent::Eq:
				if( value != ev->limit())
					continue;
			break;

			case RtEvent::Ls:
				if( value >= ev->limit())
					continue;
			break;

			case RtEvent::Le:
				if( value > ev->limit())
					continue;
			break;

			case RtEvent::Gr:
				if( value <= ev->limit())
					continue;
			break;

			case RtEvent::Ge:
				if( value < ev->limit())
					continue;
			break;
		}

		fired = true;
		std::string msg = ev->message();

		//Massage event
		if( msg != "")
		{
			if( isProperty( ev->message().c_str()))
			msg = getPropertyValueFromKey(ev->message().c_str());

      if(ev->outputTarget() == 0 || ev->outputTarget() == 1)
      {
			  mps::core::Message message;
			  message.setText(msg);

			  if(ev->outputTarget() == 0)
				  message.setTarget(mps::core::Message::Event);
			  else
				  message.setTarget(mps::core::Message::Modal);

			  switch(ev->priority())
			  {
				  case 0:
					  message.setType(mps::core::Message::Info);
				  break;
				  case 1:
					  message.setType(mps::core::Message::Warning);
				  break;
				  case 2:
					  message.setType(mps::core::Message::Error);
				  break;
			  }
			
			  message.setTimeStamp(Pt::System::Clock::getLocalTime());
			  sendMessage(message);
    		//Unamed event
	    	if( msg == "" && ev->audioData().size() == 0 && ev->command() == "")
		    {
			    mps::core::Message message( translate("Mp.Event.Txt.UnnamedEvent"), mps::core::Message::Event, mps::core::Message::EventMsg,
			                  Pt::System::Clock::getLocalTime());

    			sendMessage(message);
	    		return fired;
		    }
        }
        else if( ev->outputTarget() == 2)
        {
            writeSystemEvent(ev->priority(), msg.c_str());
        }
      }

		//Audio event
		if( ev->audioData().size() != 0)
			playSound(&ev->audioData()[0]);

		//Command event
		std::string cmd = ev->command();
			
		if( cmd != "")
		{
			if( isProperty(cmd.c_str()))
			cmd = getPropertyValueFromKey(cmd.c_str());

			std::string upperCmd = cmd;                
			std::transform(upperCmd.begin(), upperCmd.end(), upperCmd.begin(), toupper);

			if(upperCmd == "MP.DATACONVERTER.EXE")
				cmd = workingDirectory()+ cmd;

			std::string cmdParam = replaceProperties(ev->commandParam());

			std::string exec = cmd;
						
			if( cmdParam != "")
			{
				exec += " ";
				exec += cmdParam;
			}
            Pt::System::ProcessInfo pi(Pt::System::Path(exec.c_str()));
			Pt::System::Process* proc = new Pt::System::Process(pi);

			try
			{
				proc->start();
				_runningCmds.push_back(proc);
			}
			catch(std::exception ex)
			{
				std::cerr<<ex.what()<<std::endl;
			}
			
		}
	}

	return fired;
}
	
void EventPS::addObject(Object* object, const std::string& type, const std::string& name )
{
	if( type == "Mp.Event.RtEvents")
		_events = (mps::core::ObjectVector<RtEvent*>*) object;
	else
		ProcessStation::addObject(object, type, name);
}

}}


