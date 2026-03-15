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
#include <Pt/System/Clock.h>
#include <Pt/System/FileInfo.h>
#include <Pt/DateTime.h>
#include <Pt/Timespan.h>
#include <Pt/Byteorder.h>
#include <Pt/TextStream.h>
#include <Pt/Utf8Codec.h>
#include <Pt/System/Process.h>
#include "DataStoragePS.h"
#include <mps/core/SourceDescription.h>
#include <mps/core/SignalList.h>
#include <mps/core/Port.h>
#include <mps/core/Message.h>
#include <mps/core/SignalScaling.h>
#include <mps/core/FactorOffsetSignalScaling.h>
#include <mps/core/RuntimeEngine.h>
#include <string>
#include <sstream>
using namespace std;

namespace mps{
namespace storage{

std::string toString(Pt::uint32_t i)
{
    std::string s;
    std::stringstream ss;
    ss<<i;
    ss>>s;
    return s;
}

DataStoragePS::DataStoragePS(void)
: _fileName("")
, _fileKey("")
, _storageGroups()
, _errorState(false)
, _writeTimeSignal(false)
, _overWriteExistingFile(false)
, _triggerPort(0)
, _meaComment("")
, _options()
, _metaFileFormat(MeaProcess)
, _startTime()
, _startTimeUTC()
, _startTimeOffset(0)
, _properties()
, _command("")
, _cmdParam("")
, _executeCmdProcess(0)
{
    registerProperty( "fileName", *this, &DataStoragePS::getFileName, &DataStoragePS::setFileName );
    registerProperty( "writeTimeSignal", *this, &DataStoragePS::getWriteTimeSignal, &DataStoragePS::setWriteTimeSignal );
    registerProperty( "fileOptions", *this, &DataStoragePS::getFileOptions, &DataStoragePS::setFileOptions );
    registerProperty( "meaComment", *this, &DataStoragePS::comment, &DataStoragePS::setMeaComment);
    registerProperty( "metaFileFormat",*this, &DataStoragePS::metaFileFormat, &DataStoragePS::setMetaFileFormat);
    registerProperty( "property",*this, &DataStoragePS::prop, &DataStoragePS::addProperty);
    registerProperty( "command",*this, &DataStoragePS::command, &DataStoragePS::setCommand);
    registerProperty( "cmdParam",*this, &DataStoragePS::cmdParam, &DataStoragePS::setCmdParam);
}

DataStoragePS::~DataStoragePS(void)
{ }

std::string DataStoragePS::baseFileName(std::string fileName)
{
    std::string::size_type extensionPointPos = fileName.rfind('.');

    if (extensionPointPos != std::string::npos)
    {
        return fileName.substr(0, extensionPointPos);
    }
    else
    {
        return fileName;
    }
}

std::string DataStoragePS::directoryFromFileName(std::string fileName)
{
    Pt::System::Path path(fileName.c_str());
    return path.dirName().narrow();
}


void DataStoragePS::onInitInstance()
{
    ProcessStation::onInitInstance();		

    _triggerPort  =  (mps::core::TriggerPort*)  _inputPorts->at(1);

    _triggerPort->onEventTrigger += Pt::slot(*this, &DataStoragePS::onEventTrigger);
    _triggerPort->onStartTrigger += Pt::slot(*this, &DataStoragePS::onStartTrigger);
    _triggerPort->onStopTrigger += Pt::slot(*this, &DataStoragePS::onStopTrigger);
}

void DataStoragePS::store( bool store )
{
    for( Pt::uint32_t grp = 0; grp < _storageGroups.size(); ++grp)
        _storageGroups[grp]->setStore( store );
}

void DataStoragePS::onInitialize()
{
    _errorState = false;	

    _fileName = replaceProperties(_fileKey.c_str());
    _fileName = baseFileName(_fileName);

    const mps::core::Port*     inPort  =  _inputPorts->at(0);	
    const mps::core::Sources&  sources = inPort->sources();
        
    //Create the storage groups.
    for( Pt::uint32_t src = 0; src < sources.size(); ++src)
    {
        const std::vector<mps::core::Signal*>& source = sources[src];
        
        StorageGroup* storageGroup = new StorageGroup();
        const mps::core::Signal* signal = source[0];
        
        storageGroup->setSignals(source);
        storageGroup->setWriteTimeSignal(_writeTimeSignal);
        storageGroup->setSampleRate(signal->sampleRate());
        storageGroup->setPhysSourceId(signal->sourceNumber());
        storageGroup->setFileName(_fileName.c_str());

        std::stringstream ss;
        ss<<(src +1);

        std::string ext = "";
        
        if(sources.size() > 1)
            ext = ".g" + ss.str();

        if( _metaFileFormat == MeaProcess)
            ext += ".mpd";
        else
            ext += ".tdx";

        storageGroup->setFileExt(ext.c_str());

        storageGroup->setEventTrigger( _triggerPort->triggerType() == mps::core::TriggerPort::EventTrigger, 
                                       _triggerPort->preEventTime(), _triggerPort->postEventTime());

        _storageGroups.push_back( storageGroup );
    }

    store( _triggerPort->triggerType() == mps::core::TriggerPort::StopTrigger || 
           _triggerPort->triggerType() == mps::core::TriggerPort::NoTrigger );
}

void DataStoragePS::onDeinitialize()
{

    if(_executeCmdProcess  != 0)
    {
        delete _executeCmdProcess;
        _executeCmdProcess = 0;
    }

    StorageGroup* group;

    for( Pt::uint32_t grp = 0; grp <  _storageGroups.size(); ++grp)
    {
        group = _storageGroups[grp];
        delete group;
    }

    _storageGroups.clear();
}

void DataStoragePS::onExitInstance()
{

    ProcessStation::onExitInstance();
}

void DataStoragePS::zipFiles()
{
}

void DataStoragePS::onUpdateDataValue( Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data )
{
    if( _errorState )
        return;
    
    if( port->portNumber() == 0)
    {
        //TODO: write to file into a separate thread
        if( !_storageGroups[sourceIdx]->writeRecords(noOfRecords,data) )
        {
            _errorState = true;

            mps::core::Message message( format(translate("Mp.Storage.Err.File"),_storageGroups[sourceIdx]->file()), mps::core::Message::Output, mps::core::Message::Error,
                             Pt::System::Clock::getLocalTime());

            sendMessage(message);

            sendMsgSaveingTaskStopped();
        }
    }
}

void DataStoragePS::createDirectoryRecursive(std::string directoryPath)
{
    enum { BufferSize = 255};

    std::stringstream ss;
    char sep = Pt::System::Path::dirsep()[0];

    char buffer[BufferSize];

    ss<<directoryPath;
    
    std::vector<std::string> pathArray;

    while( ss.getline(buffer, BufferSize, sep) )
        pathArray.push_back(buffer);

    std::string path = "/";
    
    for( Pt::uint32_t i = 0; i < pathArray.size(); ++i )
    {
        path += pathArray[i];
        path += (char)Pt::System::Path::dirsep()[0];
        
        Pt::System::Path dir(path.c_str());

        if (Pt::System::FileInfo::exists(dir))
            continue;

        Pt::System::FileInfo::createDirectory(dir);
    }
}


void DataStoragePS::onStart()
{
    bool retVal = true;
    
    //Build the file name
    _orgFile  = getRawPropertyValueFromKey(_fileKey.c_str());
    _fileName = replaceProperties(_fileKey.c_str());
    _fileName = baseFileName(_fileName);
            
    std::string metaFile = _fileName;

    switch( _metaFileFormat )
    {
        case MeaProcess:
            metaFile += ".mmf";
        break;
        
        case TDM:
            metaFile += ".tdm";
        break;

        case DAT:
            metaFile += "*.dat";
        break;
    }
    
    //Overwrite if exists?
    if(_options == AskUserForOverwrite && Pt::System::FileInfo::exists(Pt::System::Path(metaFile.c_str())))
    {
        mps::core::Message message( format(translate("Mp.Storage.Ask.Overwrite"), metaFile), mps::core::Message::Modal, mps::core::Message::QuestionFile,
                         Pt::System::Clock::getLocalTime());

        if(sendMessage(message) == mps::core::MessageResult::Yes)
        {
            if( message.fileName() != "")
            {
                metaFile = message.fileName();
                setPropertyValue(propertyName(_fileKey.c_str()).c_str(), metaFile.c_str());
                _fileName = baseFileName(message.fileName());
            }
        }
        else
        {
            retVal = false;
        }
    }

    //Create folder if not exists
    try
    {
        std::string dirName = directoryFromFileName(_fileName);

        if(!Pt::System::FileInfo::exists(Pt::System::Path(dirName.c_str())))
            createDirectoryRecursive(dirName);
    }
    catch(const std::exception& ex)
    {
        std::cerr<<ex.what();
        retVal = false;
    }
    
    //Start the measurement
    _triggerPort->start();
    StorageGroup* storageGrp;

    for( Pt::uint32_t index = 0; index < _storageGroups.size(); ++index)
    {
        storageGrp = _storageGroups[index];
        storageGrp->setFileName(_fileName.c_str());
    }

    if( retVal )
    {
        std::string fnoStr = getTimeStampString();

        for(Pt::uint32_t grp = 0; grp < _storageGroups.size(); ++grp)
        {
            storageGrp = _storageGroups[grp];

            //If flag create new file and the file exists = > create a new file with new file no.
            if( Pt::System::FileInfo::exists(Pt::System::Path(metaFile.c_str())) && (_options == CreateNewFile))
                storageGrp->setFileNo(fnoStr.c_str());

            if( !storageGrp->start(true) )
            {
                retVal = false;
                break;
            }
        }
    }

    _errorState = false;

    if( !retVal )
    {
        _errorState	= true;
        mps::core::Message message( format(translate("Mp.Storage.Err.File"), metaFile), mps::core::Message::Output, mps::core::Message::Error,
                         Pt::System::Clock::getLocalTime());

        sendMessage(message);

        sendMsgSaveingTaskStopped();
    }

    _startTime = Pt::System::Clock::getLocalTime();
    _startTimeUTC = Pt::System::Clock::getSystemTime();
    _startTimeOffset =  (_startTime - _startTimeUTC).hours();
}

bool DataStoragePS::createMetaFile()
{
    std::string fileName = _fileName;
    std::string fileExt;

    switch( _metaFileFormat)
    { 
        case TDM:
            fileExt = ".tdm";
        break;

        case MeaProcess:
            fileExt = ".mmf";
        break;

        case DAT:
            fileExt = ".dat";
        break;
    }

    if( _storageGroups.size() == 0)
        return false;

    if(Pt::System::FileInfo::exists(Pt::System::Path((fileName + fileExt).c_str())) && _options == CreateNewFile)
        fileName = _fileName + _storageGroups[0]->fileNo() + fileExt;
    else
        fileName = _fileName + fileExt;

    switch(_metaFileFormat)
    {
        case MeaProcess:
            return createMpMetaFile(fileName.c_str());
        
        case TDM:
            return createTDMMetaFile(fileName.c_str());
        
        case DAT:

        break;
    }
    return true;
}

void DataStoragePS::sendMsgSaveingTaskStopped()
{
    mps::core::Message message(format(translate("Mp.Storage.Err.SaveStop"),getName()), mps::core::Message::Output,  mps::core::Message::Info, 
                     Pt::System::Clock::getLocalTime());

    sendMessage( message );
}

void DataStoragePS::onStop()
{	
    for(Pt::uint32_t grp = 0; grp < _storageGroups.size(); ++grp)
        _storageGroups[grp]->stop();

    if(!_errorState)
    {
        createMetaFile();
        executeCommand();
        
        if(_orgFile != "")
            setPropertyValue(propertyName(_fileKey.c_str()).c_str(), _orgFile.c_str());				
    }
}

void DataStoragePS::executeCommand()
{
    if( command() == "")
        return;

    std::string cmd = command();

    if( isProperty(cmd.c_str()))
        cmd = getPropertyValueFromKey(cmd.c_str());

    std::string upperCmd = cmd;  

    for (Pt::uint32_t i = 0; i < upperCmd.length(); i++)
        upperCmd[i] = std::toupper(upperCmd[i]);

    if(upperCmd == "MP.DATACONVERTER.EXE")
        cmd = workingDirectory()+ cmd;

    std::string cmdArgs = replaceProperties(cmdParam());
    Pt::System::ProcessInfo pi(Pt::System::Path(cmd.c_str()));
    pi.addArg(cmdArgs);

    _executeCmdProcess = new Pt::System::Process(pi);
    _executeCmdProcess->start();
    _executeCmdProcess->wait();
    delete _executeCmdProcess;
    _executeCmdProcess = 0;
}

std::string DataStoragePS::getByteOrderString()
{
    return  Pt::isLittleEndian() ? "littleEndian" : "bigEndian";
}

std::string DataStoragePS::dateTimeAsString(const Pt::DateTime& dateTime)
{
    std::stringstream ss;
    ss << dateTime.year()<<"-"<<dateTime.month()<<"-"<<dateTime.day()<<"T"<<dateTime.hour()<<":"<<dateTime.minute()<<":"<<dateTime.second();
    return ss.str();
}

bool DataStoragePS::createTDMMetaFile(const char* file)
{
    std::fstream  fs;
    mps::core::Message   message;
    StorageGroup* storageGrp;
    string    text;
    Pt::uint32_t      usi = 2;
    Pt::uint32_t      inc = 0;
    std::map<std::string,Pt::uint32_t> usiMap;
    std::map<std::string,Pt::uint32_t>::iterator it;

    if(!openMetaFileStream(fs, file))
        return false;
    
    Pt::TextStream tx(fs, new Pt::Utf8Codec());

    tx<< "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>";
    tx<<"<usi:tdm version=\"1.0\" xmlns:usi=\"http://www.ni.com/Schemas/USI/1_0\">";
    
    tx<<"<usi:documentation>";
        tx<<"<usi:exporter>MeaProcess</usi:exporter>";
        tx<<"<usi:exporterVersion>1.2</usi:exporterVersion>";
    tx<<"</usi:documentation>";

    tx<<"<usi:model modelName=\"National Instruments USI generated meta file\" modelVersion=\"1.0\">";
        tx<<"<usi:include  nsUri=\"http://www.ni.com/DataModels/USI/TDM/1_0\"/>";
    tx<<"</usi:model>";
    tx.flush();

    tx<<"<usi:include>";
    
    std::string byteOrder = getByteOrderString();
    
    for(Pt::uint32_t grp = 0; grp < _storageGroups.size(); ++grp)
    {
        storageGrp = _storageGroups[grp];
        Pt::System::FileInfo file(Pt::System::Path(storageGrp->file().c_str() ));	

        tx<<"<file byteOrder=\""<<byteOrder.c_str()<<"\" url=\"" <<file.path().fileName()<<"\">";

            Pt::uint32_t offset = 0;
            tx.flush();
            if(_writeTimeSignal)
            {
                usiMap[std::string("inct") + toString(grp+1)] = inc;
                tx<<"<block_bm id=\"inc"<<inc<<"\"  byteOffset=\"0\" blockSize=\"";
                tx<< storageGrp->recordSize()+ sizeof(double)<<"\" blockOffset=\""<< offset<<"\" length=\""<<storageGrp->getNoOfSamples()<<"\" valueType=\"";
                tx<< "eFloat64Usi\" />";
                offset = 8;
            }
            inc++;
            tx.flush();

            for(Pt::uint32_t sig =0 ; sig < storageGrp->signals().size(); ++sig)
            {
                mps::core::Signal* signal = storageGrp->signals()[sig];
                std::string strType = getTDMSignalType(signal);
                usiMap[std::string("inc") + toString(signal->signalID())] = inc;
                tx<<"<block_bm id=\"inc"<<inc<<"\"  byteOffset=\"0\" blockSize=\"";
                
                if(_writeTimeSignal)
                    tx<< storageGrp->recordSize() + sizeof(double);
                else
                    tx<< storageGrp->recordSize();

                tx<<"\" blockOffset=\""<< offset<<"\" length=\""<<storageGrp->getNoOfSamples()<<"\" valueType=\"";
                tx<< strType.c_str()<<"\" />";
                offset += signal->valueSize();
                inc++;
            }

        tx<<"</file>";
        tx.flush();
    }

    tx<<"</usi:include>";

    tx<<"<usi:data>";
        tx<<"<tdm_root id=\"usi1\">";
            tx<<"<name>"<<getName().c_str()<<"</name>";
            
            if( _properties.size() != 0)
            {
                tx<<"<instance_attributes>";
                
                for(Pt::uint32_t a = 0; a < _properties.size(); ++a)
                {
                    std::string prop = _properties[a];
                    prop.erase(0,2);
                    prop.erase(prop.length() - 1,1);
                    tx<<"<string_attribute name=\""<<prop.c_str()<<"\" >";
                    tx<<"<s>"<<getPropertyValueFromKey(_properties[a].c_str()).c_str()<<"</s>";
                    tx<<"</string_attribute>";
                }

                tx<<"</instance_attributes>";
            }

            tx<<"<channelgroups>#xpointer(";
            
                for(Pt::uint32_t grp = 0; grp < _storageGroups.size(); ++grp)
                {
                    usiMap[std::string("grp")+toString(grp+1)] =  usi;
                    tx<<"id(\"usi"<<usi<<"\") ";
                    usi++;
                }
                
            tx<<")</channelgroups>";

            tx<<"<description>"<<getMeaComment().c_str()<<"</description>";
            tx<<"<title>"<<getName().c_str()<<"</title>";			
            tx<<"<author>MeaProcess</author>";
            std::string str = dateTimeAsString(_startTime);
            tx<<"<datetime>"<<str.c_str()<<"</datetime>";
        tx<<"</tdm_root>";
        tx.flush();
        for(Pt::uint32_t grp = 0; grp < _storageGroups.size(); ++grp)
        {
            storageGrp = _storageGroups[grp];
            
            it = usiMap.find(std::string("grp")+toString(grp+1));

            //CHANNEL GROUP
            tx<<"<tdm_channelgroup id=\"usi"<<it->second<<"\">";			
            const mps::core::SourceDescription* sourceDsrc = getSourceDescription(storageGrp->physSourceId());
            tx<<"<name>"<<sourceDsrc->name().c_str()<<"_"<< storageGrp->sampleRate()<<"</name>";
            tx<<"<root>#xpointer(id(\"usi1\"))</root>";
            tx<<"<instance_attributes>";
            tx<<"<string_attribute name=\"SamplingRate\">";
            tx<<"<s>"<< storageGrp->sampleRate()<<" Hz</s>"; 
            tx<<"</string_attribute>";
            tx<<"</instance_attributes>";
            tx<<"<channels>#xpointer(";
            
            if(_writeTimeSignal)
            {
                usiMap[std::string("t")+toString(grp+1)] = usi;
                tx<<"id(\"usi"<< usi<<"\") ";
                usi++;
            }

            //Channels link
            for(Pt::uint32_t sig = 0; sig < storageGrp->signals().size(); ++sig)
            {
                const mps::core::Signal* signal = storageGrp->signals()[sig];
                usiMap[std::string("_")+toString(signal->signalID())] = usi;
                tx<<"id(\"usi"<< usi<<"\") ";
                usi++;
            }
            tx<<") </channels>";
            
            //Submatrix link
            tx<<"<submatrices>#xpointer(";
            
            if(_writeTimeSignal)
            {
                usiMap[std::string("sbmt")+ toString(grp+1)] = usi;
                tx<<"id(\"usi"<< usi<<"\") ";
                usi++;
            }

            for(Pt::uint32_t sig = 0; sig < storageGrp->signals().size(); ++sig)
            {
                const mps::core::Signal* signal = storageGrp->signals()[sig];
                usiMap[std::string("sbm")+ toString(signal->signalID())] = usi;
                tx<<"id(\"usi"<< usi<<"\") ";
                usi++;
            }
            tx<<") </submatrices>";
            tx<<"</tdm_channelgroup>";
            //END CHANNEL GROUP
            tx.flush();
            if(_writeTimeSignal)
            {
                it = usiMap.find(std::string("t")+ toString(grp+1));
                //CHANNEL
                tx<<"<tdm_channel id=\"usi"<<it->second<<"\">";
                tx<<"<name>Time"<<grp+1<<"</name>";
                tx<<"<description>Time signal</description>";
                tx<<"<unit_string>s</unit_string>";
                tx<<"<datatype>DT_DOUBLE</datatype>";
                tx<<"<minimum>0</minimum>";
                tx<<"<maximum>10000000</maximum>";
                it = usiMap.find(std::string("grp")+ toString(grp+1));
                tx<<"<group>#xpointer(id(\"usi"<<it->second<<"\"))</group>";
                usiMap[std::string("lcolt")+ toString(grp+1)] = usi;
                tx<<"<local_columns>#xpointer(id(\"usi"<<usi<<"\"))</local_columns>";
                usi++;
                tx<<"<instance_attributes></instance_attributes>";
                tx<<"</tdm_channel>";
                //ENDCHANNEL

                //SUBMATRIX
                it = usiMap.find(std::string("sbmt")+toString(grp+1));
                tx<<"<submatrix id=\"usi"<<it->second<<"\">";
                tx<<"<name>Time"<<grp+1<<"</name>";				
                tx<<"<number_of_rows>"<<storageGrp->getNoOfSamples()<<"</number_of_rows>";
                it = usiMap.find(std::string("grp")+ toString(grp+1));
                tx<<"<measurement>#xpointer(id(\"usi" <<it->second<<"\"))</measurement>";
                it = usiMap.find(std::string("lcolt")+ toString(grp+1));
                tx<<"<local_columns>#xpointer(id(\"usi"<<it->second<<"\"))</local_columns>";
                tx<<"</submatrix>";
                //END SUBMATRIX

                //LOCALCOLUMN
                tx<<"<localcolumn id=\"usi"<<it->second<<"\">";
                tx<<"<name>Time"<<grp+1<<"</name>";	
                tx<<"<global_flags>15</global_flags>";
                tx<<"<independed>0</independed>";
                tx<<"<minimum>"<<0<<"</minimum>";
                tx<<"<maximum>"<<1000000000<<"</maximum>";
                tx<<"<sequence_representation>explicit</sequence_representation>";
                it = usiMap.find(std::string("t")+ toString(grp+1));
                tx<<"<measurement_quantity>#xpointer( id(\"usi"<<it->second<<"\"))</measurement_quantity>";
                it = usiMap.find(std::string("sbmt")+ toString(grp+1));
                tx<<"<submatrix>#xpointer(id(\"usi"<<it->second<<"\"))</submatrix>";
                usiMap[std::string("valt")+toString(grp+1)] = usi;
                tx<<"<values>#xpointer(id(\"usi"<<usi<<"\")) </values>";
                tx<<"</localcolumn>";
                //END LOCALCOLUMN

                //VALUES
                tx<<"<double_sequence id = \"usi" <<usi<<"\">";
                usi++;
                it = usiMap.find(std::string("inct")+ toString(grp+1));
                tx<<"<values external=\"inc"<<it->second<<"\" />";
                tx<<"</double_sequence>";
            }

            tx.flush();
            for(Pt::uint32_t sig = 0; sig < storageGrp->signals().size(); ++sig)
            {			
                const mps::core::Signal* signal = storageGrp->signals()[sig];

                //CHANNEL
                it = usiMap.find(std::string("_") + toString(signal->signalID()));
                tx<<"<tdm_channel id=\"usi"<<it->second<<"\">";
                tx<<"<name>"<<signal->name().c_str()<<"</name>";
                tx<<"<description>"<<signal->comment().c_str()<<"</description>";
                tx<<"<unit_string>"<<signal->unit().c_str()<<"</unit_string>";
                tx<<"<datatype>"<<getTDMChannelType(signal).c_str()<<"</datatype>";
                tx<<"<minimum>"<<signal->physMin()<<"</minimum>";
                tx<<"<maximum>"<<signal->physMax()<<"</maximum>";
                usiMap[std::string("lcol")+toString(signal->signalID())] = usi;
                
                tx<<"<instance_attributes>";
                
                const mps::core::Parameters& params = signal->getParameters();

                for(Pt::uint32_t a = 0; a < params.size(); ++a)
                {
                    const std::pair<std::string,std::string>& param = params[a];
                    tx<<"<string_attribute name=\""<<param.first.c_str()<<"\" >";
                    tx<<"<s>"<<param.second.c_str()<<"</s>";
                    tx<<"</string_attribute>";
                }

                tx<<"</instance_attributes>";

                tx<<"<local_columns>#xpointer(id(\"usi"<<usi<<"\"))</local_columns>";
                usi++;				
                it = usiMap.find(std::string("grp") + toString(grp +1));
                tx<<"<group>#xpointer(id(\"usi"<<it->second<<"\"))</group>";
                tx<<"</tdm_channel>";

                //ENDCHANNEL

                //SUBMATRIX
                it = usiMap.find(std::string("sbm")+toString(signal->signalID()));
                tx<<"<submatrix id=\"usi"<<it->second<<"\">";
                tx<<"<name>"<<signal->name().c_str()<<"</name>";
                tx<<"<number_of_rows>"<<storageGrp->getNoOfSamples()<<"</number_of_rows>";

                it = usiMap.find(std::string("grp")+toString(grp +1));
                tx<<"<measurement>#xpointer(id(\"usi" <<it->second<<"\"))</measurement>";
                
                it = usiMap.find(std::string("lcol")+toString(signal->signalID()));

                tx<<"<local_columns>#xpointer(id(\"usi"<<it->second<<"\"))</local_columns>";
                usi++;
                tx<<"</submatrix>";
                //END SUBMATRIX

                //LOCALCOLUMN
                it = usiMap.find(std::string("lcol")+toString(signal->signalID()));
                tx<<"<localcolumn id=\"usi"<<it->second<<"\">";
                tx<<"<name>"<<signal->name().c_str()<<"</name>";
                tx<<"<global_flags>15</global_flags>";
                tx<<"<independed>0</independed>";
                tx<<"<minimum>"<<signal->physMin()<<"</minimum>";
                tx<<"<maximum>"<<signal->physMax()<<"</maximum>";
                

                mps::core::FactorOffsetSignalScaling* scaling = (mps::core::FactorOffsetSignalScaling*) signal->scaling();
                if( scaling != 0)
                {
                    tx<<"<sequence_representation>raw_linear</sequence_representation>";
                    tx<<"<generation_parameters>"<< scaling->getOffset()<<" "<<scaling->getFactor()<<"</generation_parameters>";
                }
                else
                {
                    tx<<"<sequence_representation>explicit</sequence_representation>";
                }

                it = usiMap.find(std::string("_")+toString(signal->signalID()));
                tx<<"<measurement_quantity>#xpointer( id(\"usi"<<it->second<<"\"))</measurement_quantity>";
                it = usiMap.find(std::string("sbm")+toString(signal->signalID()));
                tx<<"<submatrix>#xpointer(id(\"usi"<<it->second<<"\"))</submatrix>";
                
                tx<<"<values>#xpointer(id(\"usi"<<usi<<"\")) </values>";
                
                tx<<"</localcolumn>";
                //END LOCALCOLUMN

                //VALUES
                tx<<"<"<<getTDMChannelValueType(signal).c_str()<<" id = \"usi" <<usi<<"\">";
                usi++;
                it = usiMap.find(std::string("inc") + toString(signal->signalID()));
                tx<<"<values external=\"inc"<<it->second<<"\" />";
                tx<<"</"<<getTDMChannelValueType(signal).c_str()<<">";	
                tx.flush();
            }
        }
        

    tx<<"</usi:data>";

    tx<<"</usi:tdm>";
    tx.flush();
    fs.close();
    return true;
}

std::string DataStoragePS::getTDMChannelValueType(const mps::core::Signal* signal) const
{
    switch(signal->valueDataType())
    {
        case mps::core::SignalDataType::VT_bool:
            return "byte_sequence";
            
        case mps::core::SignalDataType::VT_real64:
            return "double_sequence";
            
        case mps::core::SignalDataType::VT_real32:
            return "float_sequence";
            
        case mps::core::SignalDataType::VT_uint8_t:
            return "byte_sequence";
        
        case mps::core::SignalDataType::VT_int8_t:
            return "byte_sequence";
            
        case mps::core::SignalDataType::VT_uint16_t:
            //return "short_sequence"; //bug in DIAdem
            return "long_sequence";

        case mps::core::SignalDataType::VT_int16_t:
            return "short_sequence";			
            
        case mps::core::SignalDataType::VT_uint32_t:
            return "long_sequence";

        case mps::core::SignalDataType::VT_int32_t:
            return "long_sequence";
            
        case mps::core::SignalDataType::VT_uint64_t:
            return "longlong_sequence";
            
        case mps::core::SignalDataType::VT_int64_t:
            return "longlong_sequence";
    }

    return "";
}

std::string DataStoragePS::getTDMChannelType(const mps::core::Signal* signal) const
{
    switch(signal->valueDataType())
    {
        case mps::core::SignalDataType::VT_bool:
            return "DT_BYTE";
            
        case mps::core::SignalDataType::VT_real64:
            return "DT_DOUBLE";
            
        case mps::core::SignalDataType::VT_real32:
            return "DT_FLOAT";
            
        case mps::core::SignalDataType::VT_uint8_t:
            return "DT_BYTE";
        
        case mps::core::SignalDataType::VT_int8_t:
            return "DR_BYTE";
            
        case mps::core::SignalDataType::VT_uint16_t:
            return "DT_SHORT";

        case mps::core::SignalDataType::VT_int16_t:
            return "DT_SHORT";
            
        case mps::core::SignalDataType::VT_uint32_t:
            return "DT_LONG";

        case mps::core::SignalDataType::VT_int32_t:
            return "DT_LONG";
            
        case mps::core::SignalDataType::VT_uint64_t:
            return "DT_LONGLONG";
            
        case mps::core::SignalDataType::VT_int64_t:
            return "DT_LONGLONG";
    }

    return "";
}

std::string DataStoragePS::getTDMSignalType(const mps::core::Signal* signal) const
{
    switch(signal->valueDataType())
    {
        case mps::core::SignalDataType::VT_bool:
            return "eUInt8Usi";
            
        case mps::core::SignalDataType::VT_real64:
            return "eFloat64Usi";
            
        case mps::core::SignalDataType::VT_real32:
            return "eFloat32Usi";
            
        case mps::core::SignalDataType::VT_uint8_t:
            return "eUInt8Usi";
        
        case mps::core::SignalDataType::VT_int8_t:
            return "eUInt8Usi";
            
        case mps::core::SignalDataType::VT_uint16_t:
            return "eUInt16Usi";

        case mps::core::SignalDataType::VT_int16_t:
            return "eInt16Usi";
            
        case mps::core::SignalDataType::VT_uint32_t:
            return "eUInt32Usi";

        case mps::core::SignalDataType::VT_int32_t:
            return "eInt32Usi";
            
        case mps::core::SignalDataType::VT_uint64_t:
            return "eUInt64Usi";
            
        case mps::core::SignalDataType::VT_int64_t:
            return "eInt64Usi";
    }
    return "";
}

bool DataStoragePS::openMetaFileStream(std::fstream& fs, const char* file)
{
    mps::core::Message	message;

    fs.open(file,ios::out|ios::trunc);

    if(!fs)
    {
        _errorState = true;
        mps::core::Message  message( format(translate("Mp.Storage.Err.File"),file), mps::core::Message::Output, mps::core::Message::Error,
                          Pt::System::Clock::getLocalTime());

        sendMessage(message);	

        return false;
    }
    return true;
}

std::string DataStoragePS::getTimeStampString()
{
    Pt::Timespan span = Pt::System::Clock::getSystemTicks();
    std::stringstream ss;
    ss<<"_"<<span.toMSecs();
    return ss.str();
}

bool DataStoragePS::createMpMetaFile(const char* file)
{
    std::fstream			fs;
    mps::core::Message					message;
    StorageGroup*	    	storageGrp;
    mps::core::Signal*					signal;
    string					text;
    Pt::uint32_t		id = 1;

    if(!openMetaFileStream(fs, file))
        return false;

    std::stringstream ss;
    
    ss<<(int)runtime()->majorVersion()<<"."<<(int)runtime()->minorVersion();
    std::string strVersion = ss.str();

    Pt::TextStream tx(fs, new Pt::Utf8Codec());
    Pt::DateTime	dateTime = Pt::System::Clock::getLocalTime();
    tx<< "<?xml version=\"1.0\" ?>\r\n<mp:MeaProcess version = \"";
    tx<< strVersion.c_str() <<"\" id= \"_" << id++ << "\" xmlns:mp=\"http://www.atesion.com/Schemas/MP/1_3\">";
    std::string str = dateTimeAsString(_startTime);
    tx<<"<mp:creationDateTime>"<<str.c_str()<<"</mp:creationDateTime>";
    tx<<"<mp:timeStamp>"<<msecsSinceEpoch(_startTimeUTC)<<"</mp:timeStamp>";
    tx<<"<mp:timeStampOffset>"<<_startTimeOffset<<"</mp:timeStampOffset>";
    tx<<"<mp:tool>MeaProcess- Realtime</mp:tool>";
    tx<<"<mp:toolVersion>"<<strVersion.c_str()<<"</mp:toolVersion>";
    tx<<"<mp:byteOrder>"<< getByteOrderString().c_str()<<"</mp:byteOrder>";
    tx<< "<mp:comment>";
    tx<< getMeaComment().c_str();
    tx<< "</mp:comment>";

    tx<< "<mp:Properties>";
    for(Pt::uint32_t i = 0; i < _properties.size(); ++i)
    {
        std::string prop = _properties[i];
        prop.erase(0,2);
        prop.erase(prop.length() - 1,1);
        tx<<"<mp:property name=\""<<prop.c_str()<<"\" >";
        tx<<getPropertyValueFromKey(_properties[i].c_str()).c_str();
        tx<<"</mp:property>";
    }
    tx<< "</mp:Properties>";

    tx<<"<mp:Groups id=\"_"<<id++<<"\">";

    for(Pt::uint32_t grp = 0; grp < _storageGroups.size(); ++grp)
    {
        storageGrp = _storageGroups[grp];
        tx<< "<mp:StorageGroup id=\"_"<<id++<<"\">\n";
        const mps::core::SourceDescription* sourceDsrc = getSourceDescription(storageGrp->physSourceId());
        
        tx<< "<mp:dataFile>";			
        
        Pt::System::FileInfo file(Pt::System::Path(storageGrp->file().c_str()));
        tx<< file.path().fileName().c_str();
        tx<< "</mp:dataFile>\r\n";	
        tx<< "<mp:sampleRate>";
        tx<< storageGrp->sampleRate();
        tx<< "</mp:sampleRate>\r\n";
        tx<<"<mp:source>"<<sourceDsrc->name().c_str()<<"</mp:source>";
        tx<< "<mp:sourceID>";
        tx<< storageGrp->physSourceId();
        tx<< "</mp:sourceID>\r\n";
        tx<< "<mp:Signals id=\"_"<<id++<< "\">\r\n";
        
        if(_writeTimeSignal)
        {
            tx<< "<mp:Signal id=\"_"<<id++<<"\">\r\n";
            tx<< "<mp:type>SIGNAL_TIME_STAMP</mp:type>\r\n";
            tx<< "<mp:name>";
            tx<< "Time";
            tx<< "</mp:name>\r\n";
            tx<< "<mp:unit>";
            tx<< "s";
            tx<< "</mp:unit>\r\n";
            tx<< "<mp:comment></mp:comment>\r\n";
            tx<< "<mp:physMin>0</mp:physMin>\r\n";
            tx<< "<mp:physMax>"<<std::numeric_limits<Pt::uint32_t>::max()<<"</mp:physMax>\r\n";
            tx<< "<mp:dataType>LREAL</mp:dataType>\r\n";
            tx<< "<mp:cat>Mp.Sig.Time</mp:cat>\r\n";
            tx<< "</mp:Signal>\r\n";
        }
        tx.flush();
        for(Pt::uint32_t  sig = 0; sig < storageGrp->signals().size(); sig++)
        {
            signal = storageGrp->signals().at(sig);
            tx<< "<mp:Signal id=\"_"<<id++<<"\">\r\n";
            tx<< "<mp:name>"<<signal->name().c_str()<<"</mp:name>\r\n";
            
            tx<< "<mp:unit>";
            tx<< signal->unit().c_str();
            tx<< "</mp:unit>\r\n";
            
            tx<< "<mp:comment>";
            tx<< signal->comment().c_str();
            tx<< "</mp:comment>\r\n";

            tx<< "<mp:physMin>";
            tx<< signal->physMin();
            tx<< "</mp:physMin>\r\n";

            tx<< "<mp:physMax>";
            tx<< signal->physMax();
            tx<< "</mp:physMax>\r\n";
            tx<< "<mp:dataType>";
            tx<< signal->valueDataTypeAsString().c_str();
            tx<< "</mp:dataType>\n";
            tx<< "<mp:dataTypeSize>";
            tx<< signal->valueSize();
            tx<<"</mp:dataTypeSize>\n";
            tx<<"<mp:cat>"<<signal->cat().c_str()<<"</mp:cat>\r\n";

            const mps::core::Parameters& params = signal->getParameters();

            if( params.size() != 0)
            {
                tx<< "<mp:Properties>";

                for(Pt::uint32_t u = 0; u < params.size(); ++u)
                {
                    const std::pair<std::string,std::string>& param = params[u];

                    tx<<"<mp:property name=\""<<param.first.c_str()<<"\" >";
                    tx<<param.second.c_str();
                    tx<<"</mp:property>";
                }
                tx<< "</mp:Properties>";
            }

            if( signal->scaling() != 0 )
                tx<< signal->scaling()->getScalingObjectAsXMLString(id++).c_str();
            
            //Write parameters
            
            tx<< "</mp:Signal>\n";
            tx.flush();
        }
        tx<< "</mp:Signals>\n";
        tx<< "</mp:StorageGroup>\n";
    }	
    tx<<"</mp:Groups>";
    tx<< "</mp:MeaProcess>\n";
    tx.flush();
    fs.close();
    return true;
}

void DataStoragePS::onStartTrigger()
{ 
    store(true); 
}

void DataStoragePS::onStopTrigger()
{ 
    store(false); 
}

void DataStoragePS::onEventTrigger()
{ store(true); }
    
const std::string& DataStoragePS::getFileName() const
{ return _fileKey; }

void DataStoragePS::setFileName( const std::string& name )
{ _fileKey = name; }

bool DataStoragePS::getWriteTimeSignal() const
{ return _writeTimeSignal; }

void DataStoragePS::setWriteTimeSignal( bool write )
{ 
    _writeTimeSignal = write; 
}

Pt::uint8_t DataStoragePS::getFileOptions() const
{
    return (Pt::uint8_t) _options;
}

Pt::uint8_t DataStoragePS::metaFileFormat() const
{
    return (Pt::uint8_t) _metaFileFormat;
}

void DataStoragePS::setMetaFileFormat(Pt::uint8_t format)
{
    _metaFileFormat = (MetaFileFormat) format;
}

void DataStoragePS::setFileOptions(Pt::uint8_t options)
{
    _options = (StorageFileOptions) options;
}

std::string DataStoragePS::getMeaComment() const
{ 
    if( isProperty(_meaComment.c_str()))
        return getPropertyValueFromKey(_meaComment.c_str());

    return _meaComment; 
}

void DataStoragePS::setMeaComment( const std::string& comment )
{ 
    _meaComment = comment; 
}

void DataStoragePS::addProperty(const std::string& prop)
{
    _properties.push_back(prop);
}

const std::string& DataStoragePS::prop() const
{
    return _command; 
}

const std::string& DataStoragePS::command() const
{
    return _command;
}

void DataStoragePS::setCommand(const std::string& c)
{
    _command = c;
}

const std::string& DataStoragePS::cmdParam() const
{
    return _cmdParam;
}

void DataStoragePS::setCmdParam(const std::string& c)
{
    _cmdParam = c;
}

}}

