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
#ifndef MPS_CALCULATION_ELECTRICITYPS_H
#define MPS_CALCULATION_ELECTRICITYPS_H

#include <Pt/Types.h>
#include <Pt/Any.h>
#include <mps/core/FiFoSynchSourcePS.h>
#include <mps/core/Signal.h>
#include <mps/core/Port.h>
#include <mps/core/RecordBuilder.h>
#include <map>
#include <vector>
#include <string>
#include "MovingMeanSignal.h"

namespace mps{
namespace calculation{

class ElectricityPS : public mps::core::FiFoSynchSourcePS
{
public:
    ElectricityPS();
    virtual ~ElectricityPS();

    virtual void onInitInstance();

    virtual void onStart(); 

    virtual void onUpdateDataValue(Pt::uint32_t noOfRecords, Pt::uint32_t sourceIdx, const mps::core::Port* port, const Pt::uint8_t* data);

    inline Pt::uint32_t  voltageSignal() const
    {
        return _voltageSignal;
    }

    inline void setVoltageSignal(Pt::uint32_t  v)
    {
        _voltageSignal = v;
    }

    inline Pt::uint32_t  currentSignal() const
    {
        return _currentSignal;
    }

    inline void setCurrentSignal(Pt::uint32_t  c)
    {
        _currentSignal = c;
    }

    virtual void onReadData();

private:
    const mps::core::Signal* getInputSignal(Pt::uint32_t sigId);

    void processRecords(const Pt::uint8_t* data, Pt::uint32_t count);

private:
    //Input
    Pt::uint32_t _voltageSignal;
    Pt::uint32_t _currentSignal;
    const mps::core::Signal* _vSignal;
    const mps::core::Signal* _iSignal;
    Pt::uint32_t _vSignalOffset;
    Pt::uint32_t _iSignalOffset;
    Pt::uint32_t _vSourceSize;
    Pt::uint32_t _iSourceSize;
    Pt::uint32_t _vSource;
    Pt::uint32_t _iSource;

    //Output
    mps::core::RecordBuilder _record;
    double _outputRate;

    //Voltage
    double _effectiveVoltage;
    double _voltageInt;
    Pt::uint32_t _voltageCounter;
    double _lastVoltage;
    bool _firstVoltagePriode;
    double _voltagePeriode;

    //Current
    double _effectiveCurrent;
    double _currentInt;
    Pt::uint32_t _currentCounter;
    double _lastCurrent;
    double _currentPeriode;
    bool _firstCurrentPriode;

    //Power
    double _activePower;
    double _currentPower;
    double _apparentPower;
    double _idelPower;
    double _powerInt;

    //Phase
    double _phi;

    std::vector<double> _outDataRecord;
};

}}

#endif
