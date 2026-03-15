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
#ifndef MPS_CORE_WRITEPROPPS_H
#define MPS_CORE_WRITEPROPPS_H

#include <Pt/Types.h>
#include <Pt/Function.h>
#include <mps/core/ProcessStation.h>
#include <mps/core/ApiDef.h>
#include <string>
#include <map>

namespace mps {
namespace core {

class WritePropertyPS : public ProcessStation
{
public:
    WritePropertyPS();
    virtual ~WritePropertyPS();

    virtual void onUpdateDataValue( Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const Port* port, const Pt::uint8_t* data );
    
    void onInitInstance();
    
    virtual PSType psType() const
    { return  ReceptorPS;}

    inline const std::string& propMap() const
    {
        return _propMap;
    }

    inline void setPropMap(const std::string& p)
    {
        _propMap = p;
    }

private:
    std::string _propMap;
    std::map<Pt::uint32_t, std::vector<std::string> > _sig2PropMap;
    typedef std::map<Pt::uint32_t, std::vector<std::string> >::iterator Sig2PropIt;
};

}}

#endif
