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
#ifndef MPS_STORAGE_READDATAPS_H
#define MPS_STORAGE_READDATAPS_H

#include <mps/core/SynchSourcePS.h>
#include <map>
#include <iostream>
#include <fstream>

namespace mps{
namespace storage{

class ReadDataPS : public mps::core::SynchSourcePS
{
public:
    ReadDataPS();

    virtual ~ReadDataPS();

    virtual void onInitialize();

    virtual void onStart();

    virtual void onDeinitialize();

protected:
    void onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfSamples, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data);

private:
    inline const std::string& rfile() const
    {
        return _rfile;
    }

    inline void setRfile(const std::string& f)
    {
        _rfile = f;
    }

    inline Pt::uint8_t loopBack() const
    {
        return _loopBack;
    }

    inline void setLoopBack(Pt::uint8_t lb)
    {
        _loopBack = lb;
    }

    void addSourceMap(const std::string& srcMap);
    
    inline const std::string& sourceMap() const
    {
        return _rfile;
    }
    
    inline Pt::uint32_t hash() const
    {
        return _hash;
    }

    inline void setHash(Pt::uint32_t h)
    {
        _hash = h;
    }

    inline Pt::uint8_t byteOrder() const
    {
        return _byteOrder;
    }

    inline void setByteOrder(Pt::uint8_t b)
    {
        _byteOrder = b;
    }

    enum Endians
    {
        BigEndian = 0,
        LittleEndian = 1,
    };

    void sendPSErrorMessage();
    Pt::uint32_t getMetaFileHash() const;
    Pt::uint32_t getSourceIndex(Pt::uint32_t sourceId);
    static std::string getFileTimeStamp(const std::string& file);
    
    static inline Pt::DateTime formatedTimeStamp(Pt::int64_t timeStamp)
    {
        static const Pt::DateTime dt(1970, 1, 1);
        Pt::Timespan ts(timeStamp*1000);
        return dt + ts;
    }

private:
    std::string _rfile;
    Pt::uint8_t _loopBack;
    std::map<Pt::uint32_t,std::string>	 _sourceMap;
    std::map<Pt::uint32_t,std::fstream*> _fileMap;
    std::vector<Pt::uint32_t>   _samplesOffset;
    bool _errorState;
    Pt::uint8_t _eofSignalData;
    std::string _metaData;
    Pt::uint8_t _byteOrder;
    Pt::uint32_t _hash;
};

}}

#endif
