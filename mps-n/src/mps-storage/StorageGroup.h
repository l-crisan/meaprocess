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
#ifndef MPS_STORAGE_STORAGEGROUP_H
#define MPS_STORAGE_STORAGEGROUP_H

#include <vector>
#include <map>
#include <string>
#include <fstream>

#include <mps/core/CircularBuffer.h>
#include <mps/core/Signal.h>
#include <mps/core/SignalList.h>
#include <Pt/Types.h>

namespace mps {
namespace storage{

enum StorageFileOptions
{
    Overwrite = 0,
    CreateNewFile,
    AskUserForOverwrite,
};

class StorageGroup
{
    public:
        StorageGroup();
        virtual ~StorageGroup();

        bool start(bool overWriteExistingFile);

        bool writeRecords(Pt::uint32_t noOfRecords, const Pt::uint8_t* data);

        void stop();
        
        void setStore(bool store);
        
        void setSignals(const std::vector<mps::core::Signal*>& signals);
        
        const std::vector<mps::core::Signal*>& signals() const ;

        void setWriteTimeSignal( bool write);
        
        void setSampleRate( double rate);
        
        double sampleRate() const;
        
        void setFileName( const char* fname);
        
        void setFileNo( const char* no);

        std::string fileNo() const;

        void setFileExt(const char* ext);

        std::string file() const;
     
        void setEventTrigger(bool ev, double preTime, double postTime);

        Pt::uint32_t recordSize() const;

        void setPhysSourceId(Pt::uint32_t id);
        Pt::uint32_t physSourceId() const;

        Pt::uint64_t getNoOfSamples() const;
        
private:
        bool writeDirect(Pt::uint32_t noOfRecords, const Pt::uint8_t* data);

        bool writeTrigged(Pt::uint32_t noOfRecords, const Pt::uint8_t* data);		

        const std::vector<Pt::uint8_t>& generateTimeStamp(Pt::uint32_t noOfRecords, const Pt::uint8_t* data);

        bool writeToFile(const Pt::uint8_t* data, Pt::uint32_t dataSize);

private:
        std::vector<mps::core::Signal*> _signals;
        std::string         _fileName;
        std::string         _fileNo;
        std::string         _fileExt;
        double              _sampleRate;
        double              _timeStamp;
        double              _timeIncrement;
        bool                _writeTimeSignal;
        bool                _store;
        Pt::uint32_t        _recordSize;
        double              _preTime;
        double              _postTime;
        std::vector<Pt::uint8_t> _timeStampBuffer;

        //Event Trigger Data
        bool                        _eventTrigger;
        mps::core::CircularBuffer   _preTriggerBuffer;
        Pt::uint32_t                _postTriggerSamples;
        Pt::uint32_t                _preTriggerSamples;
        Pt::uint32_t                _postTriggerCount;
        bool                        _storingPostTrigger;
        std::fstream                _fileStream;
        Pt::uint32_t                _physSourceId;
};

}}

#endif
