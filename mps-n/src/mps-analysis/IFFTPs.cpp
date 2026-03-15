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
#include "IFFTPs.h"
#include <mps/core/SignalList.h>
#include <Pt/Math.h>

namespace mps{
namespace analysis{

IFFTPs::IFFTPs()
{
    registerProperty( "nifft", *this, &IFFTPs::nifft, &IFFTPs::setNifft );
    registerProperty( "chn1", *this, &IFFTPs::chn1, &IFFTPs::setChn1 );
    registerProperty( "chn2", *this, &IFFTPs::chn2, &IFFTPs::setChn2 );
    registerProperty( "channelType", *this, &IFFTPs::channelType, &IFFTPs::setChannelType );
}

IFFTPs::~IFFTPs()
{
}

void IFFTPs::onInitInstance()
{
    ProcessStation::onInitInstance();

    _timeDataCount = nifft();
    _halfOne  = nifft()/2 +1;

    _fftHandle = mpsCreateFFT((int) _timeDataCount,true);
    _timeData = new double[_timeDataCount]; 
    _freqData = new mpsFFTComplex[_halfOne];
}
    
void IFFTPs::onInitialize()
{
    ProcessStation::onInitialize();
    const mps::core::Port* port = _inputPorts->at(0);

    for(Pt::uint32_t i = 0; i < port->signalList()->size(); ++i)
    {
        const mps::core::Signal*  signal  = port->signalList()->at(i);
        
        if( signal->signalID() == chn1())
            _chn1signal = signal;

        if( signal->signalID() == chn2())
            _chn2signal = signal;
    }

    Pt::uint32_t sigIdx = port->signalList()->getSignalIndex(_chn1signal);
    _inSourceIndex = port->sourceIndex(sigIdx);
    _chn1OffsetInSource = port->signalOffsetInSource(sigIdx);
    _chn2OffsetInSource = port->signalOffsetInSource(sigIdx);
    _recSize         = port->sourceDataSize(_inSourceIndex);

    _outData.resize(_timeDataCount);

    _outPort = _outputPorts->at(0);
}


void IFFTPs::onStart()
{
    ProcessStation::onStart();

    for( Pt::uint32_t i = 0; i < _timeDataCount; ++i)
        _timeData[i] = 0;

    for( Pt::uint32_t i = 0; i < _halfOne; ++i)
    {
        _freqData[i].r = 0;
        _freqData[i].i = 0;
    }
    _curInputIndex = 0;
}


void IFFTPs::setFreqData(Pt::uint32_t index, double chn1value, double chn2value)
{
    double ampl = 0;
    switch(channelType())
    {
        case Complex:
            _freqData[index].i = static_cast<float>(chn1value);
            _freqData[index].r = static_cast<float>(chn2value);
            return;
        break;
        
        case PhaseAmpl:
            ampl = chn1value * nifft()/2;
        break;

        case PhasePower:
            ampl = sqrt(chn1value) * nifft()/2;
        break;
    }

    double phase = chn2value * Pt::pi<double>() / 180.0;

    if(chn2value > 90 && chn2value <= 180) 
    {
        _freqData[index].r = static_cast<float>(-cos(Pt::pi<double>() - phase) * ampl);
        _freqData[index].i = static_cast<float>(sin(Pt::pi<double>() - phase) * ampl);
    }
    else if(chn2value > 180 && chn2value <= 270) 
    {
        _freqData[index].r = static_cast<float>(-cos(phase - Pt::pi<double>()) * ampl);
        _freqData[index].i = static_cast<float>(-sin(phase - Pt::pi<double>()) * ampl);
    }
    else if(chn2value > 270 && chn2value <= 360) 
    {
        _freqData[index].r = static_cast<float>(cos(Pt::piDouble<double>() - phase) * ampl);
        _freqData[index].i = static_cast<float>(-sin(Pt::piDouble<double>() - phase) * ampl);
    }
    else
    {
        _freqData[index].r = static_cast<float>(cos(phase) * ampl);

        _freqData[index].i = static_cast<float>(sin(phase) * ampl);
    }
}

void IFFTPs::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if( _inSourceIndex != sourceIdx)
        return;

    for(Pt::uint32_t rec = 0; rec < noOfRecords; rec++)
    {
        const double chn2value = _chn2signal->scaleValue(&data[rec* _recSize + _chn2OffsetInSource]);
        const double chn1value = _chn1signal->scaleValue(&data[rec* _recSize + _chn1OffsetInSource]);				

        _curInputIndex++;

        if( _curInputIndex == _halfOne)
        {
            _curInputIndex = 0;
            mpsIFFT(_fftHandle, _freqData, _timeData);

            for( Pt::uint32_t i = 0; i < _timeDataCount; ++i)
            {
                _outData[i] = _timeData[i] /_timeDataCount;
                _timeData[i] = 0;
            }

            _outPort->onUpdateDataValue(_timeDataCount, 0, (const Pt::uint8_t*) &_outData[0]);			
        }
        else
        {
            setFreqData(_curInputIndex, chn1value, chn2value);
        }
    }
}

void IFFTPs::onDeinitialize()
{
    ProcessStation::onDeinitialize();
}

void IFFTPs::onExitInstance()
{
    delete _timeData; 
    delete _freqData;

    mpsFreeFFT(_fftHandle);

    ProcessStation::onExitInstance();
}

}}
