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
#ifndef MPS_LABJACK_DRIVERWRAPPER_H
#define MPS_LABJACK_DRIVERWRAPPER_H

#include <Pt/System/Library.h>
#include <vector>

namespace mps{
namespace labjack{

typedef long (_stdcall *TListAll)(long *productIDList,long *serialnumList,long *localIDList,long *powerList,long (*calMatrix)[20],long *numberFound,long *reserved1,long *reserved2);
typedef long (_stdcall *TAISample)(long *idnum, long demo, long *stateIO, long updateIO, long ledOn, long numChannels, long *channels, long *gains, long disableCal, long *overVoltage, float *voltages);
typedef long (_stdcall *TDigitalIO)(long *idnum, long demo, long *trisD, long trisIO, long *stateD, long *stateIO, long updateDigital, long *outputD);
typedef long (_stdcall *TCounter)( long *idnum, long demo, long *stateD, long *stateIO, long resetCounter, long enableSTB, unsigned long *count );
typedef long (_stdcall *TAIStreamStart)( long *idnum, long demo, long stateIOin, long updateIO, long ledOn, long numChannels, long *channels, long *gains, float *scanRate, long disableCal, long reserved1, long readCount );
typedef long (_stdcall *TAIStreamRead)( long localID, long numScans, long timeout, float (*voltages)[4], long *stateIOout, long *reserved, long *ljScanBacklog, long *overVoltage );
typedef long (_stdcall *TAIStreamClear)( long localID );
typedef long (_stdcall *TAOUpdate)( long *idnum, long demo, long trisD, long trisIO, long *stateD, long *stateIO, long updateDigital, long resetCounter, unsigned long *count, float analogOut0, float analogOut1);

class DriverWrapper
{
public:

    static bool loadDriver();
    static void freeDriver();

    static long ListAll(long *productIDList,long *serialnumList,long *localIDList,long *powerList,long (*calMatrix)[20],long *numberFound,long *reserved1,long *reserved2);
    static long AISample(long *idnum, long demo, long *stateIO, long updateIO, long ledOn, long numChannels, long *channels, long *gains, long disableCal, long *overVoltage, float *voltages);
    static long DigitalIO(long *idnum, long demo, long *trisD, long trisIO, long *stateD, long *stateIO, long updateDigital, long *outputD);
    static long Counter( long *idnum, long demo, long *stateD, long *stateIO, long resetCounter, long enableSTB, unsigned long *count );
    static long AIStreamStart( long *idnum, long demo, long stateIOin, long updateIO, long ledOn, long numChannels, long *channels, long *gains, float *scanRate, long disableCal, long reserved1, long readCount );
    static long AIStreamRead ( long localID, long numScans, long timeout, float (*voltages)[4], long *stateIOout, long *reserved, long *ljScanBacklog, long *overVoltage );    
    static long AIStreamClear ( long localID );
    static long AOUpdate( long *idnum, long demo, long trisD, long trisIO, long *stateD, long *stateIO, long updateDigital, long resetCounter, unsigned long *count, float analogOut0, float analogOut1);


    static bool isBoardAvailable(const long* serialList, Pt::uint32_t serialCount, long serial)
    {
        if( serial == -1)
            return (serialCount != 0);

        for( Pt::uint32_t i = 0; i < serialCount; ++i)
        {
            if( serialList[i] == serial)
                return true;
        }

        return false;
    }

private:
    DriverWrapper(void);
    ~DriverWrapper(void);


    static Pt::System::Library _library;
    static TListAll _ListAll;
    static TAISample _AISample;
    static TDigitalIO _DigitalIO;
    static TCounter _Counter;
    static TAIStreamStart _AIStreamStart;
    static TAIStreamRead _AIStreamRead;
    static TAIStreamClear _AIStreamClear;
    static TAOUpdate _AOUpdate;
    static bool _loaded;
};


}}

#endif