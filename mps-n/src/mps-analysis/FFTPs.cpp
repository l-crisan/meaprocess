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
#include "FFTPs.h"
#include <mps/core/SignalList.h>
#include <Pt/Math.h>
#include "FFTSignal.h"
#include <cmath>

namespace mps{
namespace analysis{

FFTPs::FFTPs()
: _correctionFactor(1)
{
    registerProperty( "nfft", *this, &FFTPs::nfft, &FFTPs::setNfft );
    registerProperty( "window", *this, &FFTPs::window, &FFTPs::setWindow );
}

FFTPs::~FFTPs()
{
}

void FFTPs::onInitInstance()
{
    ProcessStation::onInitInstance();

    _fftHandle = mpsCreateFFT((int) nfft());
    _timeData = new double[nfft()]; 
    _freqData = new mpsFFTComplex[nfft()/2 + 1];

}

void FFTPs::onInitialize()
{
    mps::core::ProcessStation::onInitialize();

    const mps::core::Port* port = _inputPorts->at(0);
    _signal = port->signalList()->at(0);
    _inSourceIndex = port->sourceIndex(0);
    _sigOffsetInSource = port->signalOffsetInSource(0);
    _recSize = port->sourceDataSize(_inSourceIndex);

    const mps::core::Port* outPort = _outputPorts->at(0);	

    double rate = _signal->sampleRate()/2;
    double rateInc = rate/(nfft()/2);
    
    double ratePos = 0;
    _nfftHalfOne = nfft()/2+1;

    for(Pt::uint32_t i = 0; i < _nfftHalfOne; ++i)
    {
        _frequency.push_back(ratePos);
        ratePos += rateInc;
    }

    const mps::core::Sources& sources = outPort->sources();
    _outRecSize = outPort->sourceDataSize(0);
    _outData.resize(sizeof(double)* sources[0].size() * _nfftHalfOne);


    for(Pt::uint32_t n = 0; n < nfft(); ++n)
    {
        switch(window())
        {			
            case None:
                _windowValues.push_back(1);
            break;
            case Hann:
            {
                const double wvalue = 0.5 * (1-cos(Pt::piDouble<double>()*n/(nfft() -1)));

                _windowValues.push_back(wvalue);

                _correctionFactor = 2;
            }
            break;
            case Hamming:		
            {
                const double wvalue = 0.54 - 0.46*cos(Pt::piDouble<double>()*n/(nfft() -1));
    
                _windowValues.push_back(wvalue);
                _correctionFactor = 1/0.54;
            }
            break;
            case Blackman:
            {
                const double a0 = (1 - 0.16)/2.0;
                const double a1 = 0.5;
                const double a2 = 0.16/2.0;
                const double wvalue = a0 - a1*cos((Pt::piDouble<double>()*n)/(nfft() -1)) + a2*cos((4*Pt::pi<double>()*n)/(nfft() -1));
                _windowValues.push_back(wvalue);
                _correctionFactor = 1/0.42;
            }
            break;

            case Rectangle:
            {
                Pt::uint32_t limit = (nfft()/4);

                if(n > limit && n < 3*limit)
                    _windowValues.push_back(1);
                else
                    _windowValues.push_back(0);

                _correctionFactor = 2.0;
            }
            break;

            case Triangle:
            {
                const double a0 = 2.0/nfft();

                const double wvalue =a0*(nfft()/2.0 - std::abs(n-(nfft() -1)/2.0));
                _windowValues.push_back(wvalue);
                _correctionFactor = 2.0;
            }
            break;
            case Tukey25:
            {
                const double a0 = (nfft() - 1)/2.0;

                if( n <= a0)
                {
                    _windowValues.push_back(1.0);
                }
                else
                {
                    const double wvalue =1/2.0*( 1 + cos( Pt::piDouble<double>() *( ( n - 0.25*a0)/(2*(1-0.25)*a0))));
                    _windowValues.push_back(wvalue);
                }
                _correctionFactor = 2;
            }
            break;
            case Tukey50:
            {
                const double a0 = (nfft() - 1)/2.0;

                if( n <= a0)
                {
                    _windowValues.push_back(1.0);
                }
                else
                {
                    const double wvalue =1/2.0*( 1 + cos( Pt::pi<double>() *( ( n - 0.5*a0)/(2*(1-0.5)*a0))));
                    _windowValues.push_back(wvalue);
                }
                _correctionFactor = 2;
            }
            break;
        }
    }

}

void FFTPs::onStart()
{	
    _curInputIndex = 0;
    for( Pt::uint32_t i = 0; i < nfft(); ++i)
        _timeData[i] = 0;

    for( Pt::uint32_t i = 0; i < _nfftHalfOne; ++i)
    {
        _freqData[i].r = 0;
        _freqData[i].i = 0;
    }

    memset(&_outData[0],0, _outData.size());

    ProcessStation::onStart();
}

void FFTPs::onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data)
{
    if(_inSourceIndex != sourceIdx)
        return;

    for(Pt::uint32_t rec = 0; rec < noOfRecords; ++rec)
    {
        const Pt::uint8_t* pdata = &data[rec * _recSize + _sigOffsetInSource];
        const double value = _signal->scaleValue(pdata);

        if( _curInputIndex < nfft())
        {
            _timeData[_curInputIndex] = static_cast<float>(value * _windowValues[_curInputIndex]);
            _curInputIndex++;
            continue;
        }
        
        _curInputIndex = 0;

        mpsFFT(_fftHandle, _timeData, _freqData); //Make the FFT

        //Remark the current value
        _timeData[_curInputIndex] = static_cast<float>(value);
        _curInputIndex++;

        //Copy the FFT data to output buffer
        mps::core::Port* outPort = _outputPorts->at(0);

        const mps::core::Sources& sources = outPort->sources();
        const std::vector<mps::core::Signal*>& source =  sources[0];

        for(Pt::uint32_t rec = 0; rec < _nfftHalfOne; ++rec)
        {
            Pt::uint32_t recPos = rec;
            double r = 0.0;
            double i = 0.0;
            double am = 0.0;

            r = _freqData[rec].r;
            i = _freqData[rec].i;
            am = (sqrt(pow(r,2.0) + pow(i,2.0))*_correctionFactor) / (nfft()/2); //Calculate the amplitude


            for(Pt::uint32_t sig  = 0; sig < source.size(); ++sig)
            {
                FFTSignal* signal = (FFTSignal*) source[sig];
                Pt::uint32_t offsetInRec = outPort->signalOffsetInSource(0, sig);
                
                switch(signal->sigType())
                {
                    case FFTSignal::Frequency:
                    {
                        if(rec <(nfft()/2 + 1))
                        {
                            double fr = _frequency[rec];
                            memcpy(&_outData[recPos *_outRecSize + offsetInRec], &fr, sizeof(double));
                        }
                        else
                        {
                            double fr = -1;
                            memcpy(&_outData[recPos *_outRecSize + offsetInRec], &fr, sizeof(double));
                        }
                    }
                    break;
                    case FFTSignal::FourieSpectrum:
                    {
                        memcpy(&_outData[recPos *_outRecSize + offsetInRec], &am, sizeof(double));
                    }
                    break;
                    case FFTSignal::PowerSpectrum:
                    {
                        const double ps = pow(am,2);

                        memcpy(&_outData[recPos *_outRecSize + offsetInRec], &ps, sizeof(double));
                    }
                    break;

                    case FFTSignal::Phase:
                    {
                        double phase = 0;

                        if( r != 0)
                            phase = std::atan(std::abs(i/r));
                        
                        if(r <= 0 && i >= 0)
                            phase = Pt::pi<double>() - phase;
                        else if( r <= 0 && i < 0)
                            phase = phase + Pt::pi<double>();
                        else if( r >= 0 && i < 0)
                            phase = Pt::piDouble<double>() - phase;

                        phase *= 180.0/Pt::pi<double>() ;
                        memcpy(&_outData[recPos *_outRecSize + offsetInRec], &phase, sizeof(double));
                    }
                    break;
                    case FFTSignal::RealPart:
                    {
                        const double rr = _freqData[rec].r;
                        
                        memcpy(&_outData[recPos *_outRecSize + offsetInRec], &rr, sizeof(double));
                    }
                    break;
                    case FFTSignal::ImagPart:
                    {
                        const double ii = _freqData[rec].i;
                        memcpy(&_outData[recPos *_outRecSize + offsetInRec], &ii, sizeof(double));
                    }
                    break;
                }
            }	
        }
        
        //Output the FFT data
        outPort->onUpdateDataValue(_nfftHalfOne,0, &_outData[0]);
    }
}

void FFTPs::onDeinitialize()
{
    ProcessStation::onDeinitialize();
}

void FFTPs::onExitInstance()
{
    delete _timeData; 
    delete _freqData;

    mpsFreeFFT(_fftHandle);

    ProcessStation::onExitInstance();
}

}}
