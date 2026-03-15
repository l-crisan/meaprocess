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
#ifndef MPS_LABJACK_OUTPUTPS_H
#define MPS_LABJACK_OUTPUTPS_H

#include <mps/core/ProcessStation.h>
#include <mps/core/ObjectVector.h>

namespace mps{
namespace labjack{

class OutBoard;
class LJOutSignal;

class OutputPS : public mps::core::ProcessStation
{
public:
    OutputPS(void);

    virtual ~OutputPS(void);

    virtual void onInitInstance();

    virtual void onExitInstance();

    virtual void onInitialize();

    virtual void onStart();

    virtual void onStop();
    
    virtual PSType psType() const
    { return ReceptorPS; }

    virtual void addObject(Object* object, const std::string& type, const std::string& subType);

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);

private:
    mps::core::ObjectVector<LJOutSignal*>* _signals;
    std::map<Pt::uint32_t,std::vector<OutBoard*>> _sigId2Board;
    typedef std::map<Pt::uint32_t,std::vector<OutBoard*>>::iterator Sig2BoardIt;

    std::map<Pt::int32_t,OutBoard*> _boards;
    bool _errorState;
};

}}
#endif
