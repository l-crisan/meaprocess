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
#ifndef MPS_STORAGE_DATASTORAGE_H
#define MPS_STORAGE_DATASTORAGE_H

#include <Pt/Types.h>
#include <Pt/DateTime.h>
#include <string>
#include <vector>
#include <map>

#include <mps/core/ProcessStation.h>
#include "StorageGroup.h"
#include <Pt/Connectable.h>

#include <mps/core/TriggerPort.h>
#include <mps/core/Signal.h>

namespace mps {
namespace storage{

class DataStoragePS : public mps::core::ProcessStation, public Pt::Connectable
{
public:
    DataStoragePS(void);

    virtual ~DataStoragePS(void);

    virtual void onInitInstance();

    virtual void onExitInstance();

    virtual void onUpdateDataValue( Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data );

    virtual mps::core::ProcessStation::PSType psType() const
    { return mps::core::ProcessStation::ReceptorPS; }

    virtual void onInitialize();

    virtual void onDeinitialize();

    virtual void onStart();

    virtual void onStop();

private:
    enum MetaFileFormat
    {
        MeaProcess = 0,
        TDM,
        DAT
    };

    void onStartTrigger();
    void onStopTrigger();
    void onEventTrigger();

    const std::string& getFileName() const;
    void setFileName( const std::string& name );

    bool getWriteTimeSignal() const;
    void setWriteTimeSignal( bool write );
    
    Pt::uint8_t getFileOptions() const;
    void setFileOptions(Pt::uint8_t options);

    std::string getMeaComment() const;
    const std::string& comment() const
    {
        return _meaComment;
    }

    void setMeaComment(const std::string& comment);
    
    Pt::uint8_t metaFileFormat() const;
    void setMetaFileFormat(Pt::uint8_t format);

    void addProperty(const std::string& prop);

    const std::string& prop() const;

    const std::string& command() const;

    void setCommand(const std::string& c);

    const std::string& cmdParam() const;
    void setCmdParam(const std::string& c);

    bool createMetaFile();

    bool createMpMetaFile(const char* file);
    bool createTDMMetaFile(const char* file);
    bool openMetaFileStream(std::fstream& fs, const char* file);
    void sendMsgSaveingTaskStopped();
    std::string getTDMSignalType(const mps::core::Signal* signal) const;
    std::string getTDMChannelType(const mps::core::Signal* signal) const;
    std::string getTDMChannelValueType(const mps::core::Signal* signal) const;
    std::string getTimeStampString();
    void store(bool store);
    void zipFiles();
    std::string getByteOrderString();
    std::string dateTimeAsString(const Pt::DateTime& dateTime);
    static std::string baseFileName(std::string fileName);
    static std::string directoryFromFileName(std::string fileName);
    static void createDirectoryRecursive(std::string fileName);
    void executeCommand();	

    inline static Pt::int64_t msecsSinceEpoch(Pt::DateTime t)
    {
        static const Pt::DateTime dt(1970, 1, 1); //from epoch
        return (t - dt).toMSecs();
    }

private:
    std::string                 _fileName;
    std::string                 _fileKey;
    std::vector<StorageGroup*>  _storageGroups;
    bool                        _errorState;
    bool                        _writeTimeSignal;
    bool                        _overWriteExistingFile;
    mps::core::TriggerPort*     _triggerPort;
    std::string                 _meaComment;
    StorageFileOptions          _options;
    MetaFileFormat              _metaFileFormat;
    Pt::DateTime                _startTime;
    Pt::DateTime                _startTimeUTC;
    int                         _startTimeOffset;
    std::vector<std::string>    _properties;
    std::string                 _command;
    std::string                 _cmdParam;
    Pt::System::Process*        _executeCmdProcess;
    std::string                 _orgFile;
};

}}
#endif
