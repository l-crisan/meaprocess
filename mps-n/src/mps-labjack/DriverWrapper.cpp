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
#include "DriverWrapper.h"
#include <sstream>
#include <iostream>

namespace mps{
namespace labjack{

Pt::System::Library DriverWrapper::_library;

TListAll        DriverWrapper::_ListAll =  0;
TAISample       DriverWrapper::_AISample =  0;
TDigitalIO      DriverWrapper::_DigitalIO =  0;
TCounter        DriverWrapper::_Counter =  0;
TAIStreamStart  DriverWrapper::_AIStreamStart = 0;
TAIStreamRead   DriverWrapper::_AIStreamRead = 0;
TAIStreamClear  DriverWrapper::_AIStreamClear = 0;
TAOUpdate       DriverWrapper::_AOUpdate = 0;

bool DriverWrapper::_loaded = false;

DriverWrapper::DriverWrapper(void)
{
}

DriverWrapper::~DriverWrapper(void)
{
}

bool DriverWrapper::loadDriver()
{
    if(_loaded)
        return true;

    try
    {
        _library.open(Pt::System::Path("ljackuw.dll"));

        _ListAll        = (TListAll) _library.resolve("ListAll");
        _AISample       = (TAISample) _library.resolve("AISample");
        _DigitalIO      = (TDigitalIO) _library.resolve("DigitalIO");
        _Counter        = (TCounter) _library.resolve("Counter");
        _AIStreamStart  = (TAIStreamStart) _library.resolve("AIStreamStart");
        _AIStreamRead   = (TAIStreamRead) _library.resolve("AIStreamRead");
        _AIStreamClear  = (TAIStreamClear)  _library.resolve("AIStreamClear");
        _AOUpdate       = (TAOUpdate)  _library.resolve("AOUpdate");
        _loaded = true;
    }
    catch(const std::exception& e)
    {
        std::cerr<<e.what()<<std::endl;
        return false;
    }

    return true;
}

void DriverWrapper::freeDriver()
{
    _loaded = false;
    _library.close();
}

long DriverWrapper::ListAll(long *productIDList,long *serialnumList,long *localIDList,long *powerList,long (*calMatrix)[20],long *numberFound,long *reserved1,long *reserved2)
{
    return _ListAll(productIDList, serialnumList, localIDList, powerList, calMatrix, numberFound, reserved1, reserved2);
}

long DriverWrapper::AISample(long *idnum, long demo, long *stateIO, long updateIO, long ledOn, long numChannels, long *channels, long *gains, long disableCal, long *overVoltage, float *voltages)
{
    return _AISample(idnum, demo, stateIO, updateIO, ledOn, numChannels, channels, gains, disableCal, overVoltage, voltages);
}

long DriverWrapper::DigitalIO(long *idnum, long demo, long *trisD, long trisIO, long *stateD, long *stateIO, long updateDigital, long *outputD)
{
    return _DigitalIO(idnum, demo, trisD, trisIO, stateD, stateIO, updateDigital, outputD);
}

long DriverWrapper::Counter( long *idnum, long demo, long *stateD, long *stateIO, long resetCounter, long enableSTB, unsigned long *count )
{
    return _Counter( idnum, demo, stateD, stateIO, resetCounter, enableSTB, count);
}

long DriverWrapper::AIStreamStart( long *idnum, long demo, long stateIOin, long updateIO, long ledOn, long numChannels, long *channels, long *gains, float *scanRate, long disableCal, long reserved1, long readCount )
{
    return _AIStreamStart( idnum, demo, stateIOin, updateIO, ledOn, numChannels, channels, gains, scanRate, disableCal, reserved1, readCount );
}

long DriverWrapper::AIStreamRead ( long localID, long numScans, long timeout, float (*voltages)[4], long *stateIOout, long *reserved, long *ljScanBacklog, long *overVoltage )
{
    return _AIStreamRead( localID, numScans, timeout, voltages, stateIOout, reserved, ljScanBacklog, overVoltage );
}

long DriverWrapper::AIStreamClear ( long localID )
{
    return _AIStreamClear(localID);
}

long DriverWrapper::AOUpdate( long *idnum, long demo, long trisD, long trisIO, long *stateD, long *stateIO, long updateDigital, long resetCounter, unsigned long *count, float analogOut0, float analogOut1)
{
    return _AOUpdate(idnum, demo, trisD, trisIO, stateD, stateIO, updateDigital, resetCounter, count, analogOut0, analogOut1);
}

}}