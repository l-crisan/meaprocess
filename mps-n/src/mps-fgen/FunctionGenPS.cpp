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
#include "FunctionGenPS.h"
#include <time.h>
#include <cmath>
#include <Pt/Math.h>
#include <mps/core/SignalList.h>
#include <mps/core/Port.h>
#include "FuncGenSignal.h"
#include <cstdlib>

namespace mps{
namespace fgen{

FunctionGenPS::FunctionGenPS(void)
{ 
}

FunctionGenPS::~FunctionGenPS(void)
{ 
}

void FunctionGenPS::onInitialize()
{
    SynchSourcePS::onInitialize();

    std::srand( (unsigned)time( 0 ) );
}

void FunctionGenPS::onSourceEvent(Pt::uint32_t& noOfRecords, Pt::uint32_t maxNoOfRecords, Pt::uint32_t sourceIdx, Pt::uint32_t portIdx, Pt::uint8_t* data)
{
    const mps::core::Sources& sources = _outputPorts->at(portIdx)->sources();
    const std::vector<mps::core::Signal*>& source = sources[sourceIdx];

    double	realData;
    Pt::uint32_t srcSigOffset = 0;
    for( Pt::uint32_t record =0 ; record < noOfRecords; record++)
    {
        for( Pt::uint32_t sigIdx = 0; sigIdx < source.size(); sigIdx++)
        {
            const mps::core::Signal* signal = source[sigIdx];
            
            realData = getValueForSignal( (FuncGenSignal*) signal );

            memcpy(&data[srcSigOffset],&realData, sizeof(double));

            srcSigOffset += signal->valueSize();
        }
    }
}

double FunctionGenPS::getValueForSignal( FuncGenSignal* signal )
{
    double value = 0.0;
    double diff = 0;
    int stop = 100;
    double newValue = 0;

    switch(signal->functionType())
    {
        case FuncGenSignal::Sine:
        {
            double	delta;
            double	offset;

            value  = std::sin(signal->counter());
            delta  = (signal->physMax() -  signal->physMin()) /2;
            value  *= delta;
            offset = (delta + (signal->physMin()));
            value  += offset;
                    
            signal->incCounter();
            
            if(signal->counter() > ((Pt::piDouble<double>()) + signal->displacementOfPhase()))
                signal->subCounter( (Pt::piDouble<double>()) );
        }
        break;

        case FuncGenSignal::SinePlus:
        {
            double	delta;
            double	offset;

            value  = std::sin(signal->counter());
            delta  = (signal->physMax() -  signal->physMin()) /2;
            value  *= delta;
            offset = (delta + (signal->physMin()));
            value  += offset;
                    
            if (value < offset)
                value = 2*offset - value;

            signal->incCounter();
            
            if(signal->counter() > ((Pt::piDouble<double>()) + signal->displacementOfPhase()))
                signal->subCounter( (Pt::piDouble<double>()) );
        }
        break;

        case FuncGenSignal::SineMinus:
        {
            double	delta;
            double	offset;

            value  = std::sin(signal->counter());
            delta  = (signal->physMax() -  signal->physMin()) /2;
            value  *= delta;
            offset = (delta + (signal->physMin()));
            value  += offset;
                    
            if (value > offset)
                value = 2*offset - value;

            signal->incCounter();
            
            if(signal->counter() > ((Pt::piDouble<double>()) + signal->displacementOfPhase()))
                signal->subCounter( (Pt::piDouble<double>()) );
        }
        break;
        
        case FuncGenSignal::ExpPlus:
        {
            value = std::exp(signal->counter());
            double delta  = (signal->physMax() -  signal->physMin()) /2;
            value *= delta;
            double offset = (delta + signal->physMin());
            value += offset;
            signal->incCounter();

            if(signal->counter() >= 0)
                signal->subCounter(5);
        }
        break;

        case FuncGenSignal::ExpMinus:
        {
            value = std::exp(signal->counter());
            double delta  = (signal->physMax() -  signal->physMin()) /2;
            value *= delta;
            double offset = (delta + signal->physMin());
            value += offset;
            value = 2 * offset - value;

            signal->incCounter();

            if(signal->counter() >= 0)
                signal->subCounter(5); 
        }
        break;

        case FuncGenSignal::Sinc:
        case FuncGenSignal::SincMinus:
        {
            double	delta;
            double	offset;
            double xval = Pt::pi<double>()* signal->counter();
            
            value  = std::sin(xval);

            double diff = 0.000000000001;

            if (std::abs(0.0 - value) < diff)
                value = 1;
            else
                value /= xval;

            delta  = (signal->physMax() -  signal->physMin()) /2.0;
            value  *= delta;
            offset = (delta + (signal->physMin()));
            value  += offset;
            
            if(FuncGenSignal::SincMinus == signal->functionType())
                value = 2 * offset - value;

            signal->incCounter();

            if(signal->counter() >= signal->sincPeriodeHalf())
                signal->subCounter(signal->sincPeriode());
        }
        break;

        case FuncGenSignal::RampUp:
        {
            value = signal->counter();

            if(value <= signal->physMax())
            {
                signal->incCounter();
            }
            else
            {
                value = signal->physMin() ;
                signal->setCounter( signal->physMin() );
            }
        }
        break;

        case FuncGenSignal::HalfRoundPlus:
        case FuncGenSignal::HalfRoundMinus:
        {
            if (signal->counter() < Pt::pi<double>())
                value = sqrt(pow(Pt::pi<double>(),2) - pow(Pt::pi<double>() - signal->counter(),2));
            else if (signal->counter() >= Pt::pi<double>() /2)
                value = sqrt(pow(Pt::pi<double>(),2) - pow(signal->counter() - Pt::pi<double>(), 2));

            const double delta = (signal->physMax() - signal->physMin()) / 2.0;
            value *= delta/Pt::pi<double>();
            const double offset = (delta +signal->physMin());
            value += offset;
            signal->incCounter();

            if( signal->functionType() == FuncGenSignal::HalfRoundMinus)
                 value = 2*offset - value;

            if (signal->counter() >= (Pt::piDouble<double>()))
                signal->subCounter(Pt::piDouble<double>());
        }
        break;

        case FuncGenSignal::RampDown:
        {
            value = signal->counter();

            if(value > signal->physMin())
            {
                signal->incCounter();
            }
            else
            {
                value = signal->physMax() ;
                signal->setCounter( signal->physMax() );
            }
        }
        break;

        case FuncGenSignal::RectanglePlus:
        {
            const int counter = (int) signal->counter() % signal->totalPointsInPeriode();

            if (counter >= 0)
            {
                if (counter > signal->onPoints())
                    value = signal->physMin();
                else
                    value = signal->physMax();
            }
            else
            {
                if (counter < signal->onPoints())
                    value = signal->physMin();
                else
                    value = signal->physMax();
            }

            signal->incCounter();
        }
        break;

        case FuncGenSignal::RectangleMinus:
        {
            const int counter = (int) signal->counter() % signal->totalPointsInPeriode();

            if (counter >= 0)
            {
                if (counter > signal->onPoints())
                    value = signal->physMax();
                else
                    value = signal->physMin();
            }
            else
            {
                if (counter < signal->onPoints())
                    value = signal->physMax();
                else
                    value = signal->physMin();
            }

            signal->incCounter();
        }
        break;

        case FuncGenSignal::Noise:
            value = (((double)rand() / ((double)RAND_MAX + 1)) * (signal->physMax() - signal->physMin()) + signal->physMin());		
        break;

    case FuncGenSignal::Random:
    {
      value = signal->counter();

      do
      {
          diff = (((double)rand() / ((double)RAND_MAX + 1) - 0.5) * (signal->physMax() - signal->physMin()))/signal->parameter();
          newValue = value + diff;
          stop--;
      }
      while ((newValue > signal->physMax() || newValue < signal->physMin()) && stop != 0);

      if (stop == 0)
      {
          stop = 100;

          if (newValue > signal->physMax())
              newValue = signal->physMax();

          if (newValue < signal->physMin())
              newValue = signal->physMin();
      }

      signal->setCounter(newValue);
      value = newValue;
    }
    break;

        case FuncGenSignal::Constant:
            value = signal->parameter();
        break;
    }
    
    return value;
}

void FunctionGenPS::onStart()
{
    FuncGenSignal* signal;
    const mps::core::SignalList*   signalList;

    for( Pt::uint32_t portIdx = 0; portIdx < _outputPorts->size(); portIdx++)
    {
        signalList = _outputPorts->at(portIdx)->signalList();

        for( Pt::uint32_t sigInx = 0; sigInx < signalList->size(); sigInx++)
        {
            signal = (FuncGenSignal*) signalList->at(sigInx);
            signal->reset();
        }
    }

    SynchSourcePS::onStart();
}

}}

