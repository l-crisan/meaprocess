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
#include <mps/api/mps.h>
#include <mps/core/RuntimeEngine.h>
#include <mps/core/ObjectFactory.h>
#include <mps/builder/XmlBuilder.h>
#include <mps/modld/ModuleLoader.h>
#include <Pt/TextStream.h>
#include <Pt/Utf8Codec.h>
#include <Pt/System/Logger.h>
#include <Pt/System/LogLevel.h>

extern "C"
{

MPS_API Pt::uint64_t mpsLoadFromXML(const char* xmlData, mpsMessage& msg)
{
    try
    {
        mps::core::Message message;
        mps::core::RuntimeEngine* engine = mps::builder::XmlBuilder::loadXml(xmlData, message, mps::core::RuntimeEngine::objectFactory());

        if( engine == 0)
        {
            strcpy(msg.text, message.text().c_str());
            strcpy(msg.comment, message.comment().c_str());
            msg.type = message.type();
            msg.timeStamp = message.timeStamp();
            msg.target = message.target();			
        }

        return (Pt::uint64_t) engine;
    }
    catch(const std::exception& ex)
    {
        std::cout<<ex.what()<<std::endl;
        return 0;
    }
    
    return 0;
}

MPS_API void  mpsSetExecDirectory(Pt::uint64_t handle, const char* path)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    engine->setWorkingDirectory(path);
}

MPS_API void  mpsSetProperty(Pt::uint64_t handle, const char* name, const char* value)
{
        mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
        engine->setPropertyValue(name, value);
}

MPS_API void  mpsGetProperty(Pt::uint64_t handle, const char* name, char** value)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    
    std::string str  = engine->getRawPropertyValue(name);
    strcpy(*value, str.c_str());
}

MPS_API Pt::uint8_t  mpsLoadModule(const char* file)
{
    mps::core::ObjectFactory* factory = mps::modld::ModuleLoader::loadModule(file);
    
    if( factory == 0)
        return 0;
    
    mps::core::RuntimeEngine::registerFactory(factory);

    return 1;
}

MPS_API void  mpsUnloadAllModules()
{
    mps::modld::ModuleLoader::unloadAllModules();
}

MPS_API void  mpsInitialize(Pt::uint64_t handle)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    engine->initialize();
}

MPS_API	void  mpsStart(Pt::uint64_t handle)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    engine->start();
}

MPS_API void  mpsStop(Pt::uint64_t handle)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    engine->stop();
}

MPS_API void  mpsDeinitilize(Pt::uint64_t handle)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    engine->deinitialize();
}

MPS_API	void  mpsAddDataListener(Pt::uint64_t handle, Pt::uint32_t objId, Pt::uint32_t psID, mpsOnData callBack)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    engine->addDataOutListener(objId, psID, callBack);
}

MPS_API void  mpsAddDataSource(Pt::uint64_t handle, Pt::uint32_t objId, Pt::uint32_t psID, mpsOnGetSignalData callBack)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    engine->addDataSource(objId,psID, callBack);
}

MPS_API void  mpsSetMessageListener(Pt::uint64_t handle, Pt::uint64_t objId, mpsOnMessage callBack)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    engine->setMessageListener(objId, callBack);
}

MPS_API void  mpsReleaseRuntime(Pt::uint64_t handle)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    mps::builder::XmlBuilder::destroy(engine);
}

MPS_API void  mpsSetLanguage(Pt::uint64_t handle, const char* code)
{
    mps::core::RuntimeEngine* engine = (mps::core::RuntimeEngine*) handle;
    engine->setLanguage(code);
}

MPS_API void  mpsSetLogFile(const char* path, Pt::int32_t level)
{
    Pt::System::LogTarget&  target = Pt::System::LogTarget::get("");
    target.setChannel(path);
    target.setLogLevel((Pt::System::LogLevel) level);
}

}
