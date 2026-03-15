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
#ifndef MPS_CORE_PROCESSSTATION_H
#define MPS_CORE_PROCESSSTATION_H

#include <Pt/Types.h>
#include <string>
#include <mps/core/Api.h>
#include <mps/core/Object.h>
#include <mps/core/ObjectVector.h>
#include <mps/core/Message.h>
#include <map>

namespace mps{
namespace core{

class Port;
class Translator;
class SignalList;
class RuntimeEngine;
class SourceDescription;


 /** @brief The base class for each process stations.*/
class MPS_CORE_API ProcessStation : public Object
{
public:
    /**@brief A enum for the process station kinds*/
    enum PSType
    {
        SourcePS,    ///< Is a source process station. 
        WorkPS,      ///< Is a work process station.
        ReceptorPS,  ///< Is a receptor process station.
        SystemIn,    ///< Is a system input process station.
        SystemOut    ///< IS a systen output process station.
    };

    /** @brief Destructor.*/
    virtual ~ProcessStation(void);

    /** @brief Override this to setup the process station.
    *   
    *   The properties of the process station are allready loaded.*/
    virtual void onInitInstance();

    /** @brief Override this to shutdown the process station.*/
    virtual void onExitInstance();

    /** @brief Override this to process the data working.
    *
    *   This methode is called when an input port has become new data.
    *   @param noOfRecords The number of records in data.
    *   @param sourceIdx The source index.
    *   @param port The data incoming port.
    *   @param data The incoming data.*/
    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Port* port, const Pt::uint8_t* data);

    /** @brief Check if the current process station is a synchronized process station.
    *
    *   A synchronized process station is a source process station derivated from
    *   SynchSourcePS. This process station propagate it's data synchron to a
    *   given system tact.
    *   @return True if the process station is synchronized.*/
    virtual bool isSynchronizedPS() const;

    /** @brief Return the identifier of the process station
    *   @return The name of the process station.*/
    const std::string& getName() const;

    /** @brief Set the identifier of the process station
    *   @param name The name of the process station.*/
    void setName( const std::string& name );

    /** @brief Translate a key 
    *   @param key The key to translate.*/
    const std::string& translate( const char* key );

    /** @brief Return the current language.
    *   @return The current language.*/
    const std::string& languageCode() const;

    /** @brief Override this to initialize your process station.
    *
    *   Is called by the framework when the initialize signal is
    *   send throw the control port.*/
    virtual void onInitialize();

    /** @brief Override this to start your process station.
    *
    *   Is called by the framework when the start signal is
    *   send throw the control port.*/
    virtual void onStart();
    
    /** @brief Override this to stop your process station.
    *
    *   Is called by the framework when the stop signal is
    *   send throw the control port.*/
    virtual void onStop();

    /** @brief Override this to deinitialize your process station.
    *
    *   Is called by the framework when the deinittialize signal is
    *   send throw the control port.*/
    virtual void onDeinitialize();

    /** @brief Sets the runtime engine.
    * @param runtime The runtime engine.
    */
    void setRuntime(RuntimeEngine* runtime);

    inline RuntimeEngine* runtime() const
    {
        return _runtime;
    }

    /** @brief Returns the process station type
    * @return The process station type.*/
    virtual PSType psType() const;

    /** @brief Gets the a poperty value.
    *
    * @param propName The property name.
    * @return The property value.
    */
    std::string getPropertyValue(const char* propName) const;

    /** @brief Sets the a poperty value.
    *
    * @param propName The property name.
    * @param propValue The property value.
    */
    void setPropertyValue(const char* propName, const char* propValue);

    /** @brief Sets the a poperty value.
    *
    * @param propName The property name.
    * @param propValue The property value.
    */
    void setPropertyValue(const char* propName, double value);

    /**@brief Gets a runtime property value.
    *
    * @param propName  The property name.
    * @return The property value.
    */
    double getPropertyNumericValue(const char* propName) const;

    /** @brief Gets the a poperty value.
    *
    * @param propKey The property key.
    * @return The property value.
    */
    std::string getPropertyValueFromKey(const char* propKey) const;
    
    /** @brief Gets the a poperty value.
    *
    * @param propKey The property key.
    * @return The property value.
    */
    std::string getRawPropertyValueFromKey(const char* propKey) const;

    /** @brief Return true if the text is a property key
    *
    * @param propKey The property key.
    * @return True if is a property key.
    */
    bool isProperty(const char* propKey) const;
    
    /** @brief Return the property name from key
    *
    * @param propKey The property key.
    * @return The property name.
    */

    /**@brief Return the property name from the property key.
    *
    * @param[in] propKey The property key.
    * @return The property key.
    */
    std::string propertyName(const char* propKey) const;

    void addObject(Object* object, const std::string& type, const std::string& subType);

    /** @brief Decode a base64 string
    *
    * @param encodedString The base64 string
    * @param ret The decoded data
    */
    static void base64Decode(std::string const& encodedString, std::vector<Pt::uint8_t>& ret);

    /** @brief Format a string
    *
    * @param format The string format
    * @param par1 The format parameter
    */
    static std::string format(const std::string& format, const std::string& par1);

    /** @brief Format a string
    *
    * @param format The string format
    * @param par1 The format parameter
    * @param par2 The format parameter
    */
    static std::string format(const std::string& format, const std::string& par1, const std::string& par2);

    /**@brief Gets the input ports.
    *
    * @return The input ports.
    */
    inline const ObjectVector<Port*>& inputPorts() const
    {
        return *_inputPorts;
    }

    /**@brief Gets the output ports.
    *
    * @return The output ports.
    */
    inline const ObjectVector<Port*>& outputPorts() const
    {
        return *_outputPorts;
    }

    
    /** @brief Send a message.
    *
    *  @param message The message to send.
    */	
    MessageResult::MsgResult sendMessage(Message& message );

    const std::string& signalList(const std::string& con) const;

protected:
    /** @brief Default constructor.*/
    ProcessStation(void);


    virtual const std::string& onGetSignalList(const std::string& connection) const;

    
    /** @brief Stops the execution of the runtime engine.*/	
    void stopRuntimeEngine(Pt::uint32_t delayMs);

    /** @brief Gets the source description.
    * 
    * @param id The source id
    * @return The source description
    */	
    const SourceDescription* getSourceDescription(Pt::uint32_t id) const;

    /** @brief Gets the time resolution.
    * 
    * @return The timer resolution in millseconds.*/
    Pt::uint32_t timerResolution() const;


    /** @brief Gets the current time stamp.
    * 
    * @return The current time stamp in ns.*/
    Pt::uint64_t currentTime() const;


    /** @brief Gets the start time stamp.
    * 
    * @return The start time stamp in ns.*/
    Pt::uint64_t startTime() const;

    /**@brief Replace the properties in the string with its values.
    *
    * @param args  The string to replace.
    * @return The new replaced string.
    */
    std::string replaceProperties(std::string args) const;

    /** @brief Input port array.*/
    ObjectVector<Port*>*	 _inputPorts;

    /** @brief Output port array.*/
    ObjectVector<Port*>*	 _outputPorts;

    /**@brief Gets the runtime working directory.
    *
    * @return The working directory.
    */
    const std::string& workingDirectory() const;	

    void setSynchronTimer(bool synchron);

private:
    static bool isBase64(unsigned char c);
    std::string		 _name;
    Translator*		 _tanslator;
    RuntimeEngine*	 _runtime;
    std::map<std::string,std::string>* _propertyMap;
};

}}

#endif
