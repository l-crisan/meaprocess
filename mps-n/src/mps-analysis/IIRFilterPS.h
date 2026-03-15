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
#ifndef MPS_ANALYSIS_IIRFILTERPS_H
#define MPS_ANALYSIS_IIRFILTERPS_H

#include <mps/core/ProcessStation.h>
#include <mps/core/Signal.h>
#include <mps/filter/Filter.h>

namespace mps{
namespace analysis{

class IIRFilterPS : public mps::core::ProcessStation
{
public:
    IIRFilterPS();

    virtual ~IIRFilterPS();

    virtual void onInitInstance();

    virtual void onInitialize();

    virtual void onStart();

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);

    virtual void onDeinitialize();

    virtual void onExitInstance();


    inline Pt::uint8_t ftype() const
    {
        return _ftype;
    }

    inline void setFType(Pt::uint8_t ft)
    {
        _ftype = ft;
    }

    inline Pt::uint8_t order() const
    {
        return _order;
    }

    inline void setOrder(Pt::uint8_t o)
    {
        _order = o;
    }

    inline Pt::uint32_t lowerPass() const
    {
        return _lowerPass;
    }

    inline void setLowerPass(Pt::uint32_t lp)
    {
        _lowerPass = lp;
    }

    inline Pt::uint32_t upperPass() const
    {
        return _upperPass;
    }

    inline void setUpperPass(Pt::uint32_t up)
    {
        _upperPass = up;
    }

    inline Pt::uint32_t transitionBW() const
    {
        return _transitionBW;
    }

    inline void setTransitionBW(Pt::uint32_t b)
    {
        _transitionBW = b;
    }

    inline Pt::uint32_t stopBand() const
    {
        return _stopBand;
    }

    inline void setStopBand(Pt::uint32_t b)
    {
        _stopBand = b;
    }

private:
    Pt::uint8_t         _ftype;
    Pt::uint8_t         _order;
    Pt::uint32_t        _lowerPass;
    Pt::uint32_t        _upperPass;
    Pt::uint32_t        _transitionBW;
    Pt::uint32_t        _stopBand;
    mps::core::Signal*  _signal;
    Pt::uint32_t        _inSourceIndex;
    Pt::uint32_t        _sigOffsetInSource;
    Pt::uint32_t        _recSize;
    mps::filter::Filter _filter;
    std::vector<double> _outData;
    bool                _errorState;
};

}}
#endif
