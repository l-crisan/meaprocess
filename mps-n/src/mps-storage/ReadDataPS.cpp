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
#include "ReadDataPS.h"
#include <stdio.h>
#include <sstream>
#include <sys/types.h>
#include <sys/stat.h>
#include <Pt/System/FileInfo.h>
#include <Pt/System/Directory.h>
#include <Pt/DateTime.h>
#include <Pt/System/Clock.h>
#include <Pt/Byteorder.h>
#include <mps/core/Port.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <mps/core/Message.h>

namespace mps{
namespace storage{

ReadDataPS::ReadDataPS()
: _errorState(false)
{
    registerProperty( "rfile", *this, &ReadDataPS::rfile, &ReadDataPS::setRfile );
    registerProperty( "loopBack", *this, &ReadDataPS::loopBack, &ReadDataPS::setLoopBack );
    registerProperty( "sourceMap", *this, &ReadDataPS::sourceMap, &ReadDataPS::addSourceMap );
    registerProperty( "hash", *this, &ReadDataPS::hash, &ReadDataPS::setHash );
    registerProperty( "byteOrder", *this, &ReadDataPS::byteOrder, &ReadDataPS::setByteOrder );
}

ReadDataPS::~ReadDataPS()
{
}

void ReadDataPS::addSourceMap(const std::string& srcMap)
{
    std::stringstream ss;
    char buffer[255];
    
    ss<< srcMap;
    ss.getline( buffer, 255, '#' );

    std::stringstream cs;

    Pt::uint32_t srcId;
    cs << buffer;
    cs >> srcId;

    ss.getline( buffer, 255, '#' );

    std::pair<Pt::uint32_t, std::string> pair(srcId,buffer);
    _sourceMap.insert(pair);
}

std::string ReadDataPS::getFileTimeStamp(const std::string& file)
{
    struct stat fs;
    stat(file.c_str(),&fs);
    Pt::DateTime fileTime = formatedTimeStamp(fs.st_mtime*1000);
    std::stringstream ss;	
    
    ss<<fileTime.day() <<"."<<fileTime.month()<<"."<<fileTime.year()<<"#";
    ss<<fileTime.hour()<<":"<<fileTime.minute()<<":"<<fileTime.second();
    return ss.str();
}

Pt::uint32_t ReadDataPS::getMetaFileHash() const
{
    Pt::System::FileInfo finfo(_rfile.c_str());
    std::vector<Pt::uint8_t> data(static_cast<Pt::uint32_t>(finfo.size()));
    FILE* fs = fopen(_rfile.c_str(),"rb");
    Pt::uint32_t  hash = 5381;

    if(fs != 0)
    {
        fread((char*) &data[0], 1, data.size(), fs);

        for( Pt::uint32_t i = 0; i < data.size(); ++i)
            hash = ((hash << 5) + hash) + data[i];

        fclose(fs);
    }

    return hash;
}

void ReadDataPS::onInitialize()
{
    SynchSourcePS::onInitialize();
    Pt::System::Path rFile(_rfile.c_str());

    if (!Pt::System::FileInfo::exists(rFile))
    {
        mps::core::Message message(format(translate("Mp.Storage.Err.DescrFile"),_rfile), mps::core::Message::Output, mps::core::Message::Error,
                        Pt::System::Clock::getLocalTime());

        sendMessage(message);
        sendPSErrorMessage();

        _errorState = true;
        return;
    }  

    if( getMetaFileHash() != _hash)
    {      
        mps::core::Message message(format(translate("Mp.Storage.Err.DescrFileDiff"),_rfile), mps::core::Message::Output, mps::core::Message::Error,
                        Pt::System::Clock::getLocalTime());

        sendMessage(message);
        sendPSErrorMessage();
        _errorState = true;
        return;
    }
    
    std::string dirName = rFile.dirName().narrow();
    std::map<Pt::uint32_t,std::string>::iterator it = _sourceMap.begin();
    
    for(; it != _sourceMap.end(); ++it)
    {
        _samplesOffset.push_back(0);

        std::string dataFile = dirName + it->second;
        Pt::System::Path dFile(dataFile.c_str());

        if (!Pt::System::FileInfo::exists(dFile))
        {
            mps::core::Message message(format(translate("Mp.Storage.Err.DataFile"), dataFile), mps::core::Message::Output, mps::core::Message::Error,
                            Pt::System::Clock::getLocalTime());

            sendMessage(message);
            sendPSErrorMessage();
            _errorState = true;
            return;
        }

        std::fstream* dataStream = new std::fstream();
        
        dataStream->open(dataFile.c_str(),std::fstream::in |std::fstream::binary);

        if(!dataStream->good())
        {
            mps::core::Message message(format(translate("Mp.Storage.Err.DataFile"),dataFile), mps::core::Message::Output, mps::core::Message::Error,
                            Pt::System::Clock::getLocalTime());

            sendMessage(message);
            sendPSErrorMessage();
            _errorState = true;
            return;
        }
        
        Pt::uint32_t sourceIndex = getSourceIndex(it->first);
        
        std::pair<Pt::uint32_t,std::fstream*> pair(sourceIndex,dataStream);
        _fileMap.insert(pair);
    }
}

void ReadDataPS::sendPSErrorMessage()
{
    mps::core::Message message(format(translate("Mp.Storage.Err.PS"),this->getName()), mps::core::Message::Output, mps::core::Message::Warning, 
                    Pt::System::Clock::getLocalTime());

    sendMessage(message);
}

Pt::uint32_t ReadDataPS::getSourceIndex(Pt::uint32_t sourceId)
{
    const mps::core::Port* port = _outputPorts->at(0);

    const mps::core::SignalList* sigList = port->signalList();

    for(Pt::uint32_t i = 0; i < sigList->size(); ++i)
    {
        mps::core::Signal* signal = sigList->at(i);
        
        if( signal->sourceNumber() == sourceId)
            return port->sourceIndex(i);
    }

    return 0;
}

void ReadDataPS::onStart()
{
    SynchSourcePS::onStart();

    if( _errorState)
        return;

    _eofSignalData = 0;

    std::map<Pt::uint32_t,std::fstream*>::iterator it = _fileMap.begin();
                    
    for( ; it != _fileMap.end(); ++it)
    {
        it->second->clear();
        it->second->seekg(0, std::ios::beg);
        it->second->seekp(0, std::ios::beg);
    }

    for(Pt::uint32_t i = 0; i < _samplesOffset.size(); ++i)
        _samplesOffset[i] = 0;
}

void ReadDataPS::onDeinitialize()
{
    SynchSourcePS::onDeinitialize();

    std::map<Pt::uint32_t,std::fstream*>::iterator it = _fileMap.begin();

    for( ; it != _fileMap.end(); ++it)
    {
        it->second->clear();
        it->second->close();
        delete it->second;
    }

    _fileMap.clear();
}

void ReadDataPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    if(_errorState)
    {
        noOfRecords = 0;
        return;
    }
    
    //TODO: Read the file into a separate thread.
    const mps::core::Port* port = _outputPorts->at(portIdx);

    if(portIdx == 0)
    {
        _eofSignalData = 0;

        std::map<Pt::uint32_t,std::fstream*>::iterator it = _fileMap.find(sourceIdx);
        std::fstream* stream = it->second;
        
        const Pt::uint32_t recordSize = port->sourceDataSize(sourceIdx);
        
        Pt::uint32_t requeredRecords = noOfRecords;

        if( (Pt::isLittleEndian() && _byteOrder == LittleEndian) ||
            (Pt::isBigEndian() && _byteOrder == BigEndian))
        {			
            stream->read((char*)&data[0], (std::streamsize)(recordSize*noOfRecords) + _samplesOffset[sourceIdx] );
            _samplesOffset[sourceIdx] = 0;
            noOfRecords = static_cast<Pt::uint32_t>(stream->gcount()/recordSize);	
        }
        else
        {
            //TODO: Read each record iterate the signal values and swab the bytes.
        }

        if(stream->eof())
        {
            if(_loopBack)
            {
                stream->clear();
                stream->seekg(0, std::ios::beg);
                stream->seekp(0, std::ios::beg);
            }
            _eofSignalData = 1;

            _samplesOffset[sourceIdx] = (requeredRecords - noOfRecords) * recordSize;
        }
    }
    else if( portIdx == 1)
    {
        for(Pt::uint32_t i = 0; i < noOfRecords; ++i)
            data[i] = _eofSignalData;		
    }
}

}}
