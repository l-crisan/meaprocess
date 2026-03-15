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
#include <mps/core/RuntimeEngine.h>

#include <Pt/System/SystemError.h>
#include <Pt/System/Logger.h>
#include <Pt/System/Clock.h>
#include <Pt/System/Library.h>
#include <Pt/Slot.h>
#include <Pt/Convert.h>
#include "Property.h"
#include "SystemOutPS.h"
#include "SystemInPS.h"
#include "ObjectFactoryManager.h"
#include <mps/core/SynchSourcePS.h>
#include <mps/core/Port.h>
#include <fstream>
#include <sstream>
#include <algorithm>

namespace mps{
namespace core{

Pt::System::Logger RuntimeEngine::_logger("mps::core::RuntimeEngine");

void RuntimeEngine::registerFactory(ObjectFactory* factory)
{
    ObjectFactoryManager& objMng = ObjectFactoryManager::instance();
    objMng.addObjectFactory(factory);
}

ObjectFactory& RuntimeEngine::objectFactory()
{
    return ObjectFactoryManager::instance();
}

RuntimeEngine::RuntimeEngine()
: _processStationList(0)
, _signalListList(0)
, _sourceDescriptions(0)
, _properties(0)
, _timer(0)
, _name("")
, _type("")
, _msgListenerObjId(0)
, _msgListenerCallBack(0)
, _sourcePs()
, _workPs()
, _receptorPs()
, _propertyMap()
, _systemSourceDescription(0)
, _running(false)
, _workDir("")
, _msgListener(0)
, _init(false)
, _synchronTimer(true)
, _timerResolution(10000000)
{	
     _systemSourceDescription.setName("MeaProcess");
     _systemSourceDescription.setSourceKey(0);

    registerProperty( "name", *this, &RuntimeEngine::getName, &RuntimeEngine::setName );
    registerProperty( "type", *this, &RuntimeEngine::getType, &RuntimeEngine::setType );
    registerProperty( "timerResolution", *this, &RuntimeEngine::timerResolution, &RuntimeEngine::setTimerResolution );
}

RuntimeEngine::~RuntimeEngine(void)
{
}

const std::string& RuntimeEngine::translate(const char* key)
{
    return _translator.translate(key);
}

void RuntimeEngine::onInitInstance()
{
    SignalList*     signalList;
    Pt::uint32_t    index;
    
    try
    {
        Pt::System::Path libPath("mps-timer");
        _timerLibrary.open(libPath);
        _createTimer = (CreateTimer*) _timerLibrary.resolve("createTimer");
        _freeTimer = (FreeTimer*) _timerLibrary.resolve("freeTimer");
        _timer = _createTimer(_timerResolution);		
        _timer->stopped += Pt::slot(*this, &RuntimeEngine::stopEngine); 
    }
    catch(const std::exception& ex)
    {
        Message message;
        std::cout<<"Load mps-timer failed. "<<ex.what()<<std::endl;
        message.setText("Load mps-timer failed.");
        message.setTarget(Message::System);
        message.setType(Message::Error);
        message.setTimeStamp(Pt::System::Clock::getLocalTime());
        sendMessage(message);
        _running = false;
        return;
    }

    for( index = 0; index < _signalListList->size(); index++ )
    {
        signalList = _signalListList->at(index);
        signalList->onInitInstance();
    }

    _propertyMap.clear();

    if( _properties != 0)
    {
        for( index = 0; index < _properties->size(); ++index)
        {
            Property* prop = _properties->at(index);
            _propertyMap[prop->name()] = prop;
        }
    }

    ProcessStation* processStation;
    SynchSourcePS*  timerListenerPS;
    std::vector<SynchSourcePS*> timerListener;

    for( index = 0; index < _processStationList->size(); index++ )
    {
        processStation = _processStationList->at(index);
        processStation->setRuntime(this);
        
        switch( processStation->psType() )
        {
            case ProcessStation::SourcePS:
            case ProcessStation::SystemIn:
                _sourcePs.push_back(processStation);
            break;

            case ProcessStation::WorkPS:
                _workPs.push_back(processStation);
            break;

            case ProcessStation::ReceptorPS:
            case ProcessStation::SystemOut:
                _receptorPs.push_back(processStation);
            break;
        }

        if( processStation->isSynchronizedPS() )
        {
            timerListenerPS = (SynchSourcePS*)processStation;

            if( processStation->psType() == ProcessStation::SourcePS)
            {
                _timer->addTimerListener(timerListenerPS);
                processStation->onInitInstance();
            }
            else
            {
                timerListener.push_back(timerListenerPS);
            }
        }
        else
        {
            processStation->onInitInstance();
        }
    }


    for(Pt::uint32_t i = 0; i < timerListener.size(); ++i)
    {
        _timer->addTimerListener(timerListener[i]);
        timerListener[i]->onInitInstance();
    }

    //Initialize the system properties.
    if( _properties != 0)
    {
        for( index = 0; index < _properties->size(); ++index)
        {
            Property* prop = _properties->at(index);
            
            if(prop->type() == "START COUNTER") 
                prop->setValue("0");
        }
    }
}

Pt::uint64_t RuntimeEngine::timerResolution() const
{ 
    return _timerResolution; 
}

void RuntimeEngine::setTimerResolution(Pt::uint64_t ns)
{
    _timerResolution = ns;
}

Pt::uint64_t RuntimeEngine::currentTime() const
{
    return _timer->getCurrentTimeStamp();
}


Pt::uint64_t RuntimeEngine::startTime() const
{
    return _timer->getStartTime();
}

void RuntimeEngine::initialize()
{
    if(_init)
        return;

    Pt::uint32_t index = 0;

    for( index = 0; index < _receptorPs.size(); ++index)
        _receptorPs[index]->onInitialize();

    for( index = 0; index < _workPs.size(); ++index)
        _workPs[index]->onInitialize();

    for( index = 0; index < _sourcePs.size(); ++index)
        _sourcePs[index]->onInitialize();

    _init = true;
}

void RuntimeEngine::start()
{
    if(_running)
        return;

    if( _properties  != 0)
    { //Update the system properties.

        for( Pt::uint32_t index = 0; index < _properties->size(); ++index)
        {
            Property* prop = _properties->at(index);
            
            if(prop->type() == "START COUNTER") 
            {
                std::string value = prop->value();
                int countValue = 0;

                if(value == "")
                {
                    std::stringstream strss;
                    strss << countValue;
                    prop->setValue(strss.str());
                }
            }
        }
    }
    Pt::uint32_t index = 0;	

    for( index = 0; index < _receptorPs.size(); ++index)
        _receptorPs[index]->onStart();

    for( index = 0; index < _workPs.size(); ++index)
        _workPs[index]->onStart();

    for( index = 0; index < _sourcePs.size(); ++index)
        _sourcePs[index]->onStart();

    _timer->start(_synchronTimer);
    _running = true;
}

void RuntimeEngine::stop(bool internalStop, Pt::uint32_t delayMs)
{
    if(!_running)
        return;

    _timer->stop(internalStop, delayMs);
}

void RuntimeEngine::stopEngine()
{
    Pt::uint32_t index;

    for( index = 0; index < _sourcePs.size(); ++index)
        _sourcePs[index]->onStop();

    for( index = 0; index < _workPs.size(); ++index)
        _workPs[index]->onStop();

    for( index = 0; index < _receptorPs.size(); ++index)
        _receptorPs[index]->onStop();

    if( _properties  != 0)
    { //Update the system properties.

        for( Pt::uint32_t index = 0; index < _properties->size(); ++index)
        {
            Property* prop = _properties->at(index);
            
            if(prop->type() == "START COUNTER") 
            {
                std::string value = prop->value();
                int countValue = 0;

                if(value != "")
                {
                    std::stringstream ss;
                    ss<< value;
                    ss>>countValue;
                }
                countValue++;  
                std::stringstream strss;
                strss << countValue;
                prop->setValue(strss.str());
            }
        }
    }
    _running = false;
    Message message;
    message.setTarget(Message::System);
    message.setType(Message::Stop);
    message.setTimeStamp(Pt::System::Clock::getLocalTime());
    sendMessage(message);
}

void RuntimeEngine::deinitialize()
{
    if(!_init)
        return;

    if(_running)
        this->stop();
        
    Pt::uint32_t index;

    for( index = 0; index < _sourcePs.size(); ++index)
        _sourcePs[index]->onDeinitialize();

    for( index = 0; index < _workPs.size(); ++index)
        _workPs[index]->onDeinitialize();

    for( index = 0; index < _receptorPs.size(); ++index)
        _receptorPs[index]->onDeinitialize();

    _init = false;
}

MessageResult::MsgResult RuntimeEngine::sendMessage(Message& message)
{
    if(_msgListenerCallBack != 0)
    {
        mpsMessage msg;
        memset(&msg, 0, sizeof(msg));
    
        memcpy(msg.comment,  message.comment().c_str(), std::min(message.comment().size() + 1, (size_t) 199));
        memcpy(msg.text, message.text().c_str(), std::min(message.text().size() + 1, (size_t)199));
        memcpy(msg.fileName, message.fileName().c_str(), std::min( message.fileName().size() + 1, (size_t)199));

        msg.target = message.target();
        msg.type = message.type();
        msg.timeStamp = message.timeStamp();
        msg.errorCode = message.errorCode();
        MessageResult::MsgResult result = (MessageResult::MsgResult) _msgListenerCallBack(_msgListenerObjId, msg);
        message.setFileName(msg.fileName);
        return result;
    }
    else
    {
        if( _msgListener != 0)
            return (MessageResult::MsgResult) _msgListener->onMessage(message);
    }

    return MessageResult::No;
}

void RuntimeEngine::onExitInstance()
{
    ProcessStation* processStation;
    SignalList*     signalList;
    Pt::uint32_t    index;

    for(index = 0; index < _processStationList->size(); index++)
    {
        processStation = _processStationList->at(index);
        processStation->onExitInstance();
        delete processStation;
    }
    
    for(index = 0; index < _signalListList->size(); index++)
    {
        signalList = _signalListList->at(index);
        signalList->onExitInstance();
        delete  signalList;
    }
        
    if( _sourceDescriptions != 0)
    {
        for(index = 0; index < _sourceDescriptions->size(); ++index)
        {
            SourceDescription* descrp = _sourceDescriptions->at(index);
            delete descrp;
        }
    }

    if( _properties != 0)
    {
        for(index = 0; index < _properties->size(); ++index)
        {
            Property* prop = _properties->at(index);
            delete prop;
        }

        _properties->clear();
        delete _properties;
    }

    _sourcePs.clear();
    _workPs.clear();
    _receptorPs.clear();
    
    _signalListList->clear();
    
    _processStationList->clear();
    
    if( _sourceDescriptions != 0)
    {
        _sourceDescriptions->clear();
        delete _sourceDescriptions;
    }

    delete _signalListList;
    delete _processStationList;

    if( _timer != 0)
        _freeTimer(_timer);

    _timerLibrary.close();
}

void RuntimeEngine::setLanguage(const char* languageCode)
{
    std::string code = languageCode;

    if( code != _translator.languageCode())
    {
        _translator.clear();
        _translator.setLanguageCode(languageCode);
        loadResource();
    }
}

void RuntimeEngine::addTranslationString(const char* transString )
{
    _translator.addToTranslationMapFromStr(transString);
}

void RuntimeEngine::loadResource()
{
    ObjectFactoryManager& objMng = ObjectFactoryManager::instance();

    for( Pt::uint32_t i = 0; i < objMng.objectFactories().size(); ++i)
    {
        const ObjectFactory* factory = objMng.objectFactories()[i];
            
        std::string fname = _workDir + factory->resourceID() + "." + _translator.languageCode()+ ".mres";

        try
        {
            std::ifstream s(fname.c_str());
            char buffer[500];

            if(!s.good())
                continue;

            while(s.getline(buffer, 500))
                addTranslationString(buffer);
        }
        catch(const std::exception& ex)
        {
            std::cerr<<ex.what()<<std::endl;
        }		
    }
}

const std::string& RuntimeEngine::languageCode() const
{
    return _translator.languageCode();
}

void RuntimeEngine::setName(const std::string& name)
{
    _name = name;
}

const std::string& RuntimeEngine::getName() const
{
    return _name;
}

void RuntimeEngine::addObject( Object* object, const std::string& type, const std::string& sybType )
{	
    if( type == "Mp.PS.List" )
        _processStationList = (ObjectVector<ProcessStation*>*) object;
    else if ( type == "Mp.Signals" )
        _signalListList = (ObjectVector<SignalList*>*) object;	
    else if( type == "Mp.Sources")
        _sourceDescriptions = (SourceDescriptionList*) object;
    else if( type == "Mp.Properties")
        _properties = (PropertyList*) object;
}

void RuntimeEngine::setType(const std::string& type)
{
    _type = type;
}

const std::string& RuntimeEngine::getType() const
{
    return _type;
}

bool RuntimeEngine::addDataOutListener( Pt::uint32_t objID, Pt::uint32_t dataOutPsId, mpsOnData callback)
{
    ObjectFactoryManager& objMng = ObjectFactoryManager::instance();
    SystemOutPS* systemOutPS = (SystemOutPS*) objMng.getObjectByID( dataOutPsId );
    
    if( systemOutPS == 0 )
        return false;

    systemOutPS->addDataListener(objID, callback );	
    
    return true;
}

bool RuntimeEngine::addDataSource( Pt::uint32_t objId, Pt::uint32_t psID, mpsOnGetSignalData callBack)
{
    ObjectFactoryManager& objMng = ObjectFactoryManager::instance();
    SystemInPS* systemInPS = (SystemInPS*) objMng.getObjectByID( psID );
    
    if( systemInPS == 0 )
        return false;

    systemInPS->setDataSource( objId, callBack);	
    
    return true;
}


void RuntimeEngine::setPropertyValue(const char* propName, const char* value)
{
    std::map<std::string,Property*>::iterator it = _propertyMap.find(propName);

    if( it == _propertyMap.end())
        return;

    _propertyMap[propName]->setValue(value);
}

void RuntimeEngine::setPropertyValue(const char* propName, double value)
{
    std::map<std::string,Property*>::iterator it = _propertyMap.find(propName);

    if( it == _propertyMap.end())
        return;

    _propertyMap[propName]->setNumericValue(value);
}

std::string RuntimeEngine::replaceProperties(std::string args) const
{
    return replaceProperties(args, "");
}

 std::string RuntimeEngine::replaceProperties(std::string args, const std::string& baseProp) const
{
    bool propBegin = false;
    int lbreakClose = 0;
    std::pair<Pt::uint32_t, Pt::uint32_t> position;
    std::string prop = "";

    for(Pt::uint32_t i = 0; i < args.size(); ++i)
    {
        if (args[i] == '$' && i < (args.size() - 1))
        {
            if (args[i + 1] == '(')
            {
                position.first = i;
                propBegin = true;
                ++i;
                lbreakClose++;
                continue;
            }
        }

        if (propBegin)
            prop += args[i];

        if (args[i] == '(' && propBegin)
            lbreakClose++;

        if (args[i] == ')' && propBegin)
        {
            lbreakClose--;
            if (lbreakClose == 0)
            {
                position.second = i;
                std::string propValue = "";

                if( baseProp != prop)
                    propValue = getPropertyValueFromKey((std::string("$(") + prop).c_str());

                args.replace(position.first,(position.second - position.first) +1, propValue);
                i = position.first + (Pt::uint32_t) propValue.size() -1;
                prop = "";
                propBegin = false;
            }
        }
    }

    if (propBegin)
    {
        position.second = (Pt::uint32_t) args.size() - 1;
        
        std::string propValue = "";

        if( baseProp != prop)
            propValue = getPropertyValueFromKey((std::string("$(") + prop).c_str());
        
        args.replace(position.first,(position.second - position.first) +1, propValue);
    }

    return args;
}

std::string RuntimeEngine::propertyName(const char* propKey) const
{
    std::string strPropKey = propKey;

    strPropKey.erase(0,2);
    strPropKey.erase(strPropKey.length()- 1, 1);
    return strPropKey;
}

bool RuntimeEngine::isProperty(const char* propKey) const
{
    std::string strPropKey = propKey;

    if( strPropKey.length() < 3)
        return false;

    return strPropKey[0] == '$' && strPropKey[1] == '(';
}

std::string RuntimeEngine::getPropertyValueFromKey(const char* propKey) const
{
    return getPropertyValue( propertyName(propKey).c_str());
}

std::string RuntimeEngine::getRawPropertyValue(const char* propName) const
{
    std::map<std::string, Property*>::const_iterator it =  _propertyMap.find(propName);

    if( it == _propertyMap.end())
        return "";

    return it->second->value();
}

std::string RuntimeEngine::getPropertyValue(const char* propName) const
{
    std::map<std::string, Property*>::const_iterator it =  _propertyMap.find(propName);

    if( it == _propertyMap.end())
        return "";

    return replaceProperties(it->second->value(), propName);
}

double RuntimeEngine::getPropertyNumericValue(const char* propName) const
{
    std::map<std::string,Property*>::const_iterator it = _propertyMap.find(propName);

    if( it == _propertyMap.end())
        return 0;

    return it->second->numericValue();
}

const SourceDescription* RuntimeEngine::getSourceDescription(Pt::uint32_t id) const
{
    if( id == 0)
        return &_systemSourceDescription;

    ObjectFactoryManager& objMng = ObjectFactoryManager::instance();
    return (SourceDescription*) objMng.getObjectByID(id);	
}


const Port* RuntimeEngine::getScriptInputPSPort() const
{
    for(Pt::uint32_t i = 0; i < _sourcePs.size(); ++i)
    {
        const ProcessStation* ps = _sourcePs[i];

        if( ps->psType() == ProcessStation::SystemIn)
        {
            SystemInPS* inPs = (SystemInPS*) ps;

            if( inPs->subType() == "Mp.Runtime.Win.SystemInputPS")
            {
                const ObjectVector<Port*>& ports = ps->outputPorts();
                return ports.at(0);
            }
        }
    }

    return 0;
}

const Port* RuntimeEngine::getScriptOutputPSPort() const
{
    for(Pt::uint32_t i = 0; i < _receptorPs.size(); ++i)
    {
        const ProcessStation* ps = _receptorPs[i];

        if( ps->psType() == ProcessStation::SystemOut)
        {
            SystemOutPS* outPs = (SystemOutPS*) ps;

            if( outPs->subType() == "Mp.Runtime.Win.SystemOutputPS")
            {
                const ObjectVector<Port*>& ports = ps->inputPorts();
                return ports.at(0);
            }
        }
    }

    return 0;
}

Pt::uint32_t RuntimeEngine::getScriptInputPSID() const
{
    for(Pt::uint32_t i = 0; i < _sourcePs.size(); ++i)
    {
        const ProcessStation* ps = _sourcePs[i];

        if( ps->psType() == ProcessStation::SystemIn)
        {
            SystemInPS* inPs = (SystemInPS*) ps;

            if( inPs->subType() == "Mp.Runtime.Win.SystemInputPS")
                return inPs->psID();
        }
    }

    return 0;
}

Pt::uint32_t RuntimeEngine::getScriptOutputPSID() const
{
    for(Pt::uint32_t i = 0; i < _receptorPs.size(); ++i)
    {
        const ProcessStation* ps = _receptorPs[i];

        if( ps->psType() == ProcessStation::SystemOut)
        {
            SystemOutPS* outPs = (SystemOutPS*) ps;

            if( outPs->subType() == "Mp.Runtime.Win.SystemOutputPS")
                return outPs->psID();
        }
    }

    return 0;
}

const std::string& RuntimeEngine::getSignalList(const std::string& connection) const
{
    static std::string staticList("");

    for(Pt::uint32_t i = 0; i < _receptorPs.size(); ++i)
    {
        const ProcessStation* ps = _receptorPs[i];
        const std::string& list = ps->signalList(connection);
        
        if( list != "")
            return list;
    }


    for(Pt::uint32_t i = 0; i < _sourcePs.size(); ++i)
    {
        const ProcessStation* ps = _sourcePs[i];
        const std::string& list = ps->signalList(connection);
        
        if( list != "")
            return list;
    }

    return staticList;
}

}}
