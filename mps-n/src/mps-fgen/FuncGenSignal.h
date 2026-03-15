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
#ifndef MPS_FGEN_FUNCGENSIGNAL_H
#define MPS_FGEN_FUNCGENSIGNAL_H

#include <Pt/Api.h>
#include <mps/core/Signal.h>


namespace mps{
namespace fgen{

class FuncGenSignal : public mps::core::Signal
{
public:
    
    enum FunctionType
    {
        Sine = 0,
        RampUp,
        RectanglePlus,
        Noise,
        Constant,
        Sinc,
        SinePlus,
        SineMinus,
        RampDown,
        HalfRoundPlus,
        HalfRoundMinus,
        RectangleMinus,
        ExpPlus,
        ExpMinus,
        SincMinus,
        Random
    };

    FuncGenSignal(Pt::uint32_t id);

    virtual ~FuncGenSignal(void);
    
    virtual void onInitInstance();

    double periode() const;

    void setPeriode(double periode);
    
    Pt::uint8_t functionType() const;

    void setFunctionType(Pt::uint8_t type);
    
    double displacementOfPhase() const;

    void setDisplacementOfPhase(double phase);

    double counter() const;
    
    void setCounter( double value );

    void incCounter();

    void subCounter( double value);

    double onPeriode() const;

    void setOnPeriode(double periode);

    void setParameter(double value);

    double parameter() const;

    void reset();

    int totalPointsInPeriode() const;

    int onPoints() const;

    inline double sincPeriodeHalf() const
    {
        return _periodeHalf;
    }

    inline double sincPeriode() const
    {
        return _sincPeriode;
    }

private:
    double       _sigCounter;
    double       _sigCounterInit;
    double       _periode;
    FunctionType _functionType;
    double       _displacementOfPhase;
    double       _displacementOfPhaseRad;
    double       _valueInc;
    double       _onPeriode;
    double       _parameter;
    int          _totalPoints;
    int          _onPoints; 
    double       _periodeHalf;
    double       _sincPeriode;
};
}}
#endif
