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
#ifndef MPS_FFTLPS_H
#define MPS_FFTLPS_H

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

class FFTPs : public mps::core::ProcessStation
{
public:
    FFTPs();
    virtual ~FFTPs();

    virtual void onInitInstance();
    
    virtual void onInitialize();
    virtual void onStart();

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);
    virtual void onDeinitialize();
    virtual void onExitInstance();

    inline Pt::uint32_t nfft() const
    {
        return _nfft;
    }

    inline void setNfft(Pt::uint32_t n)
    {
        _nfft = n;
    }
    
    inline Pt::uint8_t window() const
    {
        return _window;
    }

    inline void setWindow(Pt::uint8_t w)
    {
        _window = w;
    }

private:
    enum WindowFunction
    {
        None,
        Hann,
        Hamming,
        Blackman,
        Rectangle,
        Triangle,
        Tukey25,
        Tukey50
    };

    Pt::uint32_t             _nfft;
    Pt::uint64_t             _fftHandle;
    double*                  _timeData; 
    mpsFFTComplex*           _freqData;
    const mps::core::Signal* _signal;
    Pt::uint32_t             _inSourceIndex;
    Pt::uint32_t             _sigOffsetInSource;
    Pt::uint32_t             _recSize;
    Pt::uint32_t             _curInputIndex;
    std::vector<Pt::uint8_t> _outData;
    std::vector<double>      _frequency;
    Pt::uint32_t             _outRecSize;
    Pt::uint8_t              _window;
    std::vector<double>      _windowValues;
    double                   _correctionFactor;
    Pt::uint32_t             _nfftHalfOne;
    
};

}}

#endif
