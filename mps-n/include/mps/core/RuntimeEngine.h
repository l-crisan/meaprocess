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
#ifndef MPS_CORE_RUNTIMEENGINE_H
#define MPS_CORE_RUNTIMEENGINE_H

#include <Pt/System/Logger.h>
#include <Pt/System/Library.h>
#include <Pt/Singleton.h>
#include <Pt/Signal.h>
#include <mps/timer/ITimer.h>
#include <mps/core/ProcessStation.h>
#include <mps/core/SignalList.h>
#include <mps/core/ObjectFactory.h>
#include <mps/core/SourceDescription.h>
#include <mps/core/Translator.h>
#include <mps/core/MessageListener.h>
#include <mps/core/ApiDef.h>
#include <vector>

namespace mps{
namespace core{

class Property;

typedef mps::timer::ITimer* (CreateTimer)(Pt::uint64_t resolutionMSec);
typedef void (FreeTimer)(mps::timer::ITimer* t);

/**@brief This class implements the runtime engine functionality.

This class load a xml scheme file, instantiate the process station, 
connect it and destribute the signal list to the ports.
This class use an object factory manager for dynamicaly creation 
of scheme objects like process station and ports. This object factory 
manager manage one default or more object factories registerd by 
the application.*/
class MPS_CORE_API RuntimeEngine : public Object, public Pt::Connectable
{
    friend class DefaultObjectFactory;
public:
    
    /**@brief Destructor*/
    virtual ~RuntimeEngine();

    /**@brief Sets the language code and the translation string.
    * 
    *  The language code is a string defined by the application.
    *  The translation string consists of "key#value\n" paars.
    *  @param languageCode The language code.
    *  @param languageString The language translation string.
    */
    void setLanguage( const char* languageCode);	

    /**@brief Gets the language code.
    *  @return   The language code.*/    	
    const std::string& languageCode() const;

    /**@brief translate a key into a text.
    *  @param key The key.
    *  @return The text.*/
    const std::string& translate(const char* key);

    /** @brief Override this to setup the runtime engine.*/
    virtual void onInitInstance();

    /** @brief Override this to shutdown the runtime engine.*/
    virtual void onExitInstance();

    /**@brief Initialize the process stations.*/
    void initialize();

    /**@brief Start the process stations.*/
    void start();

    /**@brief Stopt the process stations.*/
    void stop(bool internalStop = false, Pt::uint32_t delayMs = 0);

    /**@brief Deinitialize the process stations.*/
    void deinitialize();
    
    /**@brief Sets the message listener*/
    inline void setMessageListener(Pt::uint64_t objId, mpsOnMessage callBack)
    {
        _msgListenerObjId = objId;
        _msgListenerCallBack = callBack;
    }

    /**@brief Sets the message listener*/
    inline void setMessageListener(MessageListener* listener)
    {
        _msgListener = listener;
    }
    
    /** @brief Gets the runtime engine identifier.
    * @return The runtime engine identifier */
    const std::string& getName() const;
    
    /** @brief Gets the runtime engine type identifier.
    *   @return The runtime engine type identifier */
    const std::string& getType() const;

    /** @brief For internal use to instance the child objects of the runtime engine.
    *
    *   This is called by the reflection API of the runtime engine.
    *   @param object The object to add.
    *   @param type The type identifier of the object.
    *   @param name The object instance name.*/
    void addObject(Object* object, const std::string& type, const std::string& subType );
    
    /**@brief Attache a listener to a system out process station.
    *  @param dataOutPsId The system out process station identifier.
    *  @param listener The data output listener.
    *  @return True if successful.*/
    bool addDataOutListener( Pt::uint32_t objId, Pt::uint32_t psID, mpsOnData callback);

    /**@brief Attache data input source
    *  @param dataOutPsId The system out process station identifier.
    *  @param source The data source.
    *  @return True if successful.*/
    bool addDataSource( Pt::uint32_t objId, Pt::uint32_t psID, mpsOnGetSignalData callBack);

    /**@brief Send a message to the listener.
    * @param message The message to send.*/
    MessageResult::MsgResult sendMessage(Message& message);

    /**@brief Sets a runtime property value.
    *
    * @param propName  The property name.
    * @param value The property value.
    */
    void setPropertyValue(const char* propName, const char* value);

    /**@brief Sets a runtime property value.
    *
    * @param propName  The property name.
    * @param value The property value.
    */
    void setPropertyValue(const char* propName, double value);

    /**@brief Gets a runtime property value.
    *
    * @param propName  The property name.
    * @return The property value.
    */
    std::string getPropertyValue(const char* propName) const;
    
    /**@brief Gets a runtime property value.
    *
    * @param propName  The property name.
    * @return The property value.
    */
    double getPropertyNumericValue(const char* propName) const;
    
    /**@brief Gets the raw runtime property value.
    *
    * @param propName  The property name.
    * @return The raw property value.
    */
    std::string getRawPropertyValue(const char* propName) const;

    /**@brief Check if the string is a property
    *
    * @param propKey  The string to check
    * @return True if is a property
    */
    bool isProperty(const char* propKey) const;

    /**@brief Replace the properties in the string with its values.
    *
    * @param args  The string to replace.
    * @return The new replaced string.
    */
    std::string replaceProperties(std::string args) const;

    /**@brief Return the property name from property key.
    *
    * @param propKey  The property key.
    * @return The property name.
    */
    std::string propertyName(const char* propKey) const;

    /**@brief Return the property value from property key.
    *
    * @param propKey  The property key.
    * @return The property value.
    */
    std::string getPropertyValueFromKey(const char* propKey) const;

    /**@brief Gets the data source description by the source id.
    *
    * @param id The source identifier.
    * @return The source description.
    */
    const SourceDescription* getSourceDescription(Pt::uint32_t id) const;

    /**@brief Gets the internal timer resolution in millseconds
    *
    * @return The timer resolution in nano seconds
    */
    Pt::uint64_t timerResolution() const;

    /** @brief Gets the current time stamp.
    * 
    * @return The current time stamp in ns.*/
    Pt::uint64_t currentTime() const;

    /** @brief Gets the start time stamp.
    * 
    * @return The start time stamp in ns.*/
    Pt::uint64_t startTime() const;

    /**@brief Sets the runtime working directory
    *
    * @param workDir The working directory
    */
    inline void setWorkingDirectory(const char* workDir)
    {
        _workDir = workDir;
    }
 
    /**@brief Gets the runtime working directory
    *
    * @return The working directory
    */
    inline const std::string& workingDirectory() const
    {
        return _workDir;
    }

    /**@brief Gets the runtime version
    *
    * @return The runtime version string formated as "V 1.4.0"
    */
    inline static std::string version()
    {
        std::stringstream ss;
        ss <<(int)majorVersion()<<"."<<(int)minorVersion()<<"."<<(int)buildVersion()<<" (BETA)";
        return ss.str();
    }

    /**@brief Gets the major version number.
    *
    * @return The major version number.
    */
    inline static Pt::uint8_t majorVersion()
    {
        return 2;
    }

    /**@brief Gets the minor version number.
    *
    * @return The minor version number.
    */
    inline static Pt::uint8_t minorVersion()
    {
        return 0;
    }
    
    /**@brief Gets the build version number.
    *
    * @return The build version number.
    */
    inline static Pt::uint8_t buildVersion()
    {
        return 0;
    }

    /**@brief Define if the system timer event are synchron generated.
    *
    * @param[in] synchron The timer mode.
    */
    inline void setSynchronTimer(bool synchron)
    {
        _synchronTimer = synchron;
    }

    /**@brief Returns the scripting input Process Station port.
    *
    * @return The portt.
    */
    const Port* getScriptInputPSPort() const;

    /**@brief Return the scripting output Process Station port.
    *
    * @return The port.
    */
    const Port* getScriptOutputPSPort() const;

    /**@brief Gets the Scripting Input Process Station ID.
    *
    * @return The Process Sattion ID.
    */
    Pt::uint32_t getScriptInputPSID() const;

    
    /**@brief Gets the Scripting Output Process Station ID.
    *
    * @return The Process Sattion ID.
    */
    Pt::uint32_t getScriptOutputPSID() const;

    const std::string& getSignalList(const std::string& connection) const;

    static void registerFactory(ObjectFactory* factory);
    static ObjectFactory& objectFactory();

protected:
    /**@brief Default constructor*/
    RuntimeEngine();

private:
    std::string replaceProperties(std::string args, const std::string& baseProp) const;

    void stopEngine();

    void setName(const std::string& name);

    void setType(const std::string& type);
    
    void setTimerResolution(Pt::uint64_t ns);	

    void loadResource();
    void addTranslationString(const char* transString );

    typedef ObjectVector<SourceDescription*>  SourceDescriptionList;
    typedef ObjectVector<Property*>			 PropertyList;	

    ObjectVector<ProcessStation*>*		 _processStationList;
    ObjectVector<SignalList*>*			 _signalListList;
    SourceDescriptionList*					 _sourceDescriptions;
    PropertyList*							 _properties;
    mps::timer::ITimer*						 _timer;
    Translator								 _translator;	
    std::string								 _name;
    std::string								 _type;
    Pt::uint64_t							 _msgListenerObjId;
    mpsOnMessage							 _msgListenerCallBack;
    std::vector<ProcessStation*>			 _sourcePs;
    std::vector<ProcessStation*>			 _workPs;
    std::vector<ProcessStation*>			 _receptorPs;
    std::map<std::string,Property*>			 _propertyMap;
    SourceDescription						 _systemSourceDescription;
    static 	Pt::System::Logger				 _logger;
    bool									 _running;
    std::string								 _workDir;
    MessageListener*						 _msgListener;
    bool									 _init;
    Pt::System::Library						 _timerLibrary;
    CreateTimer*							 _createTimer;
    FreeTimer*								 _freeTimer;
    bool									 _synchronTimer;
    Pt::uint64_t							 _timerResolution;	
};

}}

#endif
