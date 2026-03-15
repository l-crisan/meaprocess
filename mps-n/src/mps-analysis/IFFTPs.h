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
#ifndef MPS_IFFTLPS_H
#define MPS_IFFTLPS_H

#include <string>
#include <map>

#include <Pt/Types.h>
#include <Pt/Any.h>

#include <mps/core/ProcessStation.h>
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include <mps/fft/mps-fft.h>

namespace mps{
namespace analysis{

class IFFTPs : public mps::core::ProcessStation
{
public:
    IFFTPs();
    virtual ~IFFTPs();

    virtual void onInitInstance();

    virtual void onInitialize();

    virtual void onStart();

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);

    virtual void onDeinitialize();

    virtual void onExitInstance();

    inline Pt::uint32_t nifft() const
    {
        return _nifft;
    }

    inline void setNifft(Pt::uint32_t n)
    {
        _nifft = n;
    }

    inline Pt::uint32_t chn1() const
    {
        return _chn1;
    }

    inline void setChn1(Pt::uint32_t c)
    {
        _chn1 = c;
    }

    inline Pt::uint32_t chn2() const
    {
        return _chn2;
    }

    inline void setChn2(Pt::uint32_t r)
    {
        _chn2 = r;
    }

    inline Pt::uint8_t channelType() const
    {
        return _chnType;
    }

    inline void setChannelType(Pt::uint8_t c)
    {
        _chnType = c;
    }

private:
    enum ChannelType
    {
        Complex,
        PhaseAmpl,
        PhasePower
    };

    void setFreqData(Pt::uint32_t index, double chn1value, double chn2value);

private:
    Pt::uint32_t             _nifft;
    Pt::uint32_t             _chn1;
    Pt::uint32_t             _chn2;
    Pt::uint8_t              _chnType;
    Pt::uint64_t             _fftHandle;
    double*                  _timeData;
    mpsFFTComplex*           _freqData;
    Pt::uint32_t             _timeDataCount;
    Pt::uint32_t             _halfOne;
    const mps::core::Signal* _chn1signal;
    const mps::core::Signal* _chn2signal;
    Pt::uint32_t             _inSourceIndex;
    Pt::uint32_t             _chn1OffsetInSource;
    Pt::uint32_t             _chn2OffsetInSource;
    Pt::uint32_t             _recSize;
    Pt::uint32_t             _curInputIndex;
    std::vector<double>      _outData;
    mps::core::Port*         _outPort;
};

}}

#endif
