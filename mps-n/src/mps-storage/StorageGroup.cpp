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
#include "StorageGroup.h"
#include <Pt/Types.h>
#include <Pt/System/FileInfo.h>

using namespace std;

namespace mps{
namespace storage{ 

StorageGroup::StorageGroup()
: _fileName("")
, _fileNo("")
, _fileExt("")
, _sampleRate(0.0)
, _timeStamp(0.0)
, _timeIncrement(0.0)
, _writeTimeSignal(false)
, _store(false)
, _recordSize(0)
, _eventTrigger(false)
, _postTriggerSamples(false)
, _storingPostTrigger(false)
{
}

StorageGroup::~StorageGroup()
{
}

bool StorageGroup::writeRecords(Pt::uint32_t noOfRecords, const Pt::uint8_t* data)
{
    if(!_fileStream.is_open())
        return false;

    if( _eventTrigger)
        return writeTrigged(noOfRecords,data); //Write triggered.

    if( !_store)
    {//No store, no event trigger => only increment the time stamp.
        _timeStamp += (_timeIncrement * noOfRecords);
        return true;
    }

    //No trigger only store =>  Write direct to file.
    return writeDirect(noOfRecords,data);
}

bool StorageGroup::writeTrigged(Pt::uint32_t noOfRecords, const Pt::uint8_t* data)
{
    if( !_storingPostTrigger)
    {
        //Insert the data to the _preTriggerBuffer.
        if( _preTriggerBuffer.totalSize() < noOfRecords )
        {   //The trigger buffer is smaler as the received data.
            //Copy the newest data in to the buffer.
            const Pt::uint8_t* toCopy = data + ( noOfRecords * _recordSize);
            toCopy -=  ( _preTriggerBuffer.totalSize() * _recordSize);

            _preTriggerBuffer.reset();

            if( _writeTimeSignal )
            {
                const std::vector<Pt::uint8_t>& timeData =  generateTimeStamp(_preTriggerBuffer.totalSize(),toCopy);
                _preTriggerBuffer.insert( &timeData[0], _preTriggerBuffer.totalSize());
            }
            else
            {
                _timeStamp += (_timeIncrement * noOfRecords);
                _preTriggerBuffer.insert( toCopy, _preTriggerBuffer.totalSize());
            }
        }
        else
        {
            //The trigger buffer is bigger as the received data.
            const Pt::uint32_t capacity = (_preTriggerBuffer.totalSize() - _preTriggerBuffer.noOfElements());
            
            if( capacity > noOfRecords)
            {//If the capacity of the buffer is bigger as the number of records => write the records to buffer.

                if( _writeTimeSignal )
                {
                    const std::vector<Pt::uint8_t>& timeData =  generateTimeStamp(noOfRecords,data);
                    _preTriggerBuffer.insert( &timeData[0], noOfRecords);
                }
                else
                {
                    _timeStamp += (_timeIncrement * noOfRecords);
                    _preTriggerBuffer.insert( data, noOfRecords);
                }
            }
            else
            {//Drop the oldes data.		
                const Pt::uint32_t noToDrop =  noOfRecords - capacity;
                _preTriggerBuffer.next(noToDrop);

                //Insert the new data
                if( _writeTimeSignal )
                {
                    const std::vector<Pt::uint8_t>& timeData =  generateTimeStamp(noOfRecords,data);
                    _preTriggerBuffer.insert( &timeData[0], noOfRecords);
                }
                else
                {
                    _timeStamp += (_timeIncrement * noOfRecords);
                    _preTriggerBuffer.insert(data, noOfRecords);
                }
            }
        }

        if( _store)
        {
            //Store the pre Trigger Data.
            Pt::uint32_t max;		
            
            const Pt::uint8_t* preData = _preTriggerBuffer.get(_preTriggerBuffer.noOfElements(), max);
            
            if( _writeTimeSignal)
            {
                if( !writeToFile( preData, max * (_recordSize+ sizeof(double)) ))
                    return false;
            }
            else
            {
                if(!writeToFile( preData, max * _recordSize))
                    return false;
            }
        
            _preTriggerBuffer.next(max);
            
            _storingPostTrigger = true;
            _postTriggerCount = 0;

            _store = false;
        }
        return true;
    }
    
    if( _storingPostTrigger && _store)
    {
        _postTriggerCount = 0;
        _store = false;
    }

    //Store the post trigger data
    if( (_postTriggerCount + noOfRecords) < _postTriggerSamples)
    {
        if( _writeTimeSignal )
        {
            const std::vector<Pt::uint8_t>& timeData =  generateTimeStamp(noOfRecords,data);
            writeToFile( &timeData[0], noOfRecords* (_recordSize+ sizeof(double)));
        }
        else
        {
            writeToFile( data, noOfRecords * _recordSize);
            _timeStamp += (_timeIncrement * noOfRecords);
        }
        _postTriggerCount += noOfRecords;
        _storingPostTrigger = true;
    }
    else
    {
        const Pt::uint32_t count = _postTriggerSamples - _postTriggerCount;

        if( _writeTimeSignal )
        {
            const std::vector<Pt::uint8_t>& timeData =  generateTimeStamp(count,data);
            
            if(!writeToFile( &timeData[0], count* (_recordSize+ sizeof(double))))
                return false;
        }
        else
        {
            _timeStamp += (_timeIncrement * noOfRecords);

            if( !writeToFile( data, count * _recordSize))
                return false;
        }
        _postTriggerCount = 0;
        _storingPostTrigger = false;
        _store = false;
    }

    return true;
}

const std::vector<Pt::uint8_t>& StorageGroup::generateTimeStamp(Pt::uint32_t noOfRecords, const Pt::uint8_t* data)
{
    const Pt::uint32_t dataSize = (noOfRecords * (_recordSize + sizeof(double)));

    if( _timeStampBuffer.size() < dataSize )
        _timeStampBuffer.resize( dataSize );

    for( Pt::uint32_t rec = 0; rec < noOfRecords; rec++)
    {
        Pt::uint8_t* record = &_timeStampBuffer[rec*(_recordSize+ sizeof(double))];

        //Copy the time stamp.
        memcpy(record,&_timeStamp,sizeof(double));
        record += sizeof(double);

        //Copy the data record.
        const Pt::uint8_t* dataRecord = &data[rec* _recordSize];
        memcpy(record, dataRecord,_recordSize);

        //Increment the time stamp.
        _timeStamp += _timeIncrement;
    }

    return _timeStampBuffer;
}

bool StorageGroup::writeToFile(const Pt::uint8_t* data, Pt::uint32_t dataSize)
{
    _fileStream.write( (const char*) data, dataSize);	

    if( (_fileStream.rdstate() & ifstream::goodbit)  != ifstream::goodbit) 
    {
        _fileStream.close();
        return false;
    }

    return true;
}

bool StorageGroup::writeDirect(Pt::uint32_t noOfRecords, const Pt::uint8_t* data)
{
    if(_writeTimeSignal)
    {//Generate the time signal.
        const std::vector<Pt::uint8_t>& timeStampData = generateTimeStamp(noOfRecords,data);

        return writeToFile( &timeStampData[0], ( sizeof(double)+ _recordSize) * noOfRecords );
    }

    //Direct write. 
    if( !writeToFile( data, _recordSize * noOfRecords ) )
        return false;

    _timeStamp +=  (_timeIncrement* noOfRecords);
    return true;
}

bool StorageGroup::start(bool overWriteExistingFile)
{
    _timeIncrement = 1 / _sampleRate;
    _timeStamp = 0.0;
    
    if( overWriteExistingFile )
        _fileStream.open( file().c_str(), ios::out|ios::trunc|ios::binary );
    else
        _fileStream.open( file().c_str(), ios::out|ios::ate|ios::binary );
    
    if( _eventTrigger )
    {
        _storingPostTrigger = false;
        _postTriggerSamples = static_cast<Pt::uint32_t>(_sampleRate * _postTime);
        _preTriggerSamples = static_cast<Pt::uint32_t>(_sampleRate * _preTime);

        if( _writeTimeSignal) 
            _preTriggerBuffer.init(_preTriggerSamples, _recordSize + sizeof(double));
        else
            _preTriggerBuffer.init(_preTriggerSamples, _recordSize);
    }

    return ( _fileStream.is_open() == true );
}

void StorageGroup::setSignals(const std::vector<mps::core::Signal*>& signals)
{ 
    _signals = signals; 
    _recordSize = 0 ;

    //Calculate the record size.
    for(Pt::uint32_t sig =0 ;sig < _signals.size(); sig++)
    {
        const mps::core::Signal* signal = _signals[sig];
        _recordSize += signal->valueSize();
    }
}

void StorageGroup::stop()
{
    if(_fileStream)
    {
        _fileStream.flush();
        _fileStream.close();
    }
}

Pt::uint64_t StorageGroup::getNoOfSamples() const
{
    Pt::System::FileInfo file(StorageGroup::file().c_str());

    if( !_writeTimeSignal)
        return file.size() / recordSize();
    else
        return file.size() / (recordSize() + sizeof(double));
}

void StorageGroup::setStore(bool store)
{ 
    _store = store; 
}
    
    
const std::vector<mps::core::Signal*>& StorageGroup::signals() const 
{ 
    return _signals; 
}

void StorageGroup::setWriteTimeSignal( bool write)
{ 
    _writeTimeSignal = write; 
}
        
void StorageGroup::setSampleRate( double rate)
{ 
    _sampleRate = rate; 
}
        
double StorageGroup::sampleRate() const
{ 
    return _sampleRate; 
}
        
void StorageGroup::setFileName( const char* fname)
{ 
    _fileName = fname; 
}
        
void StorageGroup::setFileNo( const char* no)
{ 
    _fileNo = no; 
}

std::string StorageGroup::fileNo() const
{ 
    return _fileNo;
}

void StorageGroup::setFileExt(const char* ext)
{ 
    _fileExt = ext;
}

std::string StorageGroup::file() const
{ 
    return _fileName + _fileNo + _fileExt; 
}
     
void StorageGroup::setEventTrigger(bool ev, double preTime, double postTime)
{ 
    _eventTrigger = ev; 
    _preTime = preTime;
    _postTime = postTime;
}

Pt::uint32_t StorageGroup::recordSize() const
{ 
    return _recordSize; 
}

void StorageGroup::setPhysSourceId(Pt::uint32_t id)
{ 
    _physSourceId = id; 
}

Pt::uint32_t StorageGroup::physSourceId() const
{ 
    return _physSourceId; 
}

}}
