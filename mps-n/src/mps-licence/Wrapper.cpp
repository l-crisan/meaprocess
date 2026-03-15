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
#include "Wrapper.h"

#include <Pt/System/Library.h>

namespace mps{
namespace licence{


SglSearchLock_t  Wrapper::_SglSearchLock = 0;
SglAuthentA_t Wrapper::_SglAuthentA = 0;
SglAuthentB_t Wrapper::_SglAuthentB = 0;
SglReadSerialNumber_t  Wrapper::_SglReadSerialNumber = 0;
SglCryptLock_t Wrapper::_SglCryptLock = 0;
bool                Wrapper::_isopen = false;
Pt::System::Library Wrapper::_library;

bool Wrapper::loadDriver()
{
    if(_isopen)
        return true;

    try
    {
        if( sizeof(void*) == 8)
            _library.open(Pt::System::Path("SGLW64.dll"));
        else
            _library.open(Pt::System::Path("SglW32.dll"));

        _isopen = true;

        _SglSearchLock = (SglSearchLock_t)   _library.resolve("SglSearchLock");
        _SglAuthentA = (SglAuthentA_t) _library.resolve("SglAuthentA");
        _SglAuthentB  = (SglAuthentB_t)_library.resolve("SglAuthentB");
        _SglReadSerialNumber = (SglReadSerialNumber_t)         _library.resolve("SglReadSerialNumber");
        _SglCryptLock = (SglCryptLock_t)        _library.resolve("SglCryptLock");
    }
    catch(...)
    {
        return false;
    }

    return true;
}


unsigned int Wrapper::SglSearchLock(unsigned int products)
{
    return _SglSearchLock(products);
}


unsigned int Wrapper::SglAuthent(unsigned int* AuthentCode)
{
    if (sizeof(void*) == 8)
    {
        unsigned int RandNum[2];
        unsigned int AppRandNum[2];
        unsigned int LibRandNum[2];
        unsigned int AuthentCodeLocal[8];
        unsigned int RetCode;
        unsigned int i;

        for (i = 0; i < 8; i++) {
            AuthentCodeLocal[i] = AuthentCode[i];
            AuthentCode[i] = (rand() << 16) | rand();
        }

        srand(AuthentCode[0]);
        RandNum[0] = (rand() << 16) | rand();
        RandNum[1] = (rand() << 16) | rand();

        AppRandNum[0] = RandNum[0];
        AppRandNum[1] = RandNum[1];

        RetCode = _SglAuthentA(
            AuthentCodeLocal,
            AppRandNum,
            LibRandNum);

        for (i = 0; i < 8; i++) {
            AuthentCode[i] = AuthentCodeLocal[i];
        }

        if (RetCode != 0)
            return 6;

        SglTeaEncipher(RandNum, RandNum, &AuthentCode[8]);

        if ((RandNum[0] != AppRandNum[0]) ||
            (RandNum[1] != AppRandNum[1])) {

            return 6;

        }

        SglTeaEncipher(LibRandNum, LibRandNum, &AuthentCode[8]);

        return  _SglAuthentB(LibRandNum);
    }
    else
    {
        unsigned int RandNum[2];
        unsigned int AppRandNum[2];
        unsigned int LibRandNum[2];
        unsigned int AuthentCodeLocal[8];
        unsigned int RetCode;
        unsigned int i;


        for (i = 0; i<8; i++) {
            AuthentCodeLocal[i] = AuthentCode[i];
            AuthentCode[i] = (rand() << 16) | rand();
        }

        srand(AuthentCode[0]);
        RandNum[0] = (rand() << 16) | rand();
        RandNum[1] = (rand() << 16) | rand();

        AppRandNum[0] = RandNum[0];
        AppRandNum[1] = RandNum[1];

        RetCode = _SglAuthentA(
            AuthentCodeLocal,
            AppRandNum,
            LibRandNum);

        for (i = 0; i<8; i++) {
            AuthentCode[i] = AuthentCodeLocal[i];
        }

        if (RetCode != 0)
            return 6;

        SglTeaEncipher(RandNum, RandNum, &AuthentCode[8]);

        if ((RandNum[0] != AppRandNum[0]) ||
            (RandNum[1] != AppRandNum[1])) {

            return 6;

        }

        SglTeaEncipher(LibRandNum, LibRandNum, &AuthentCode[8]);

        return  _SglAuthentB(LibRandNum);
    }

    return 6;
}


unsigned int Wrapper::SglReadSerialNumber(unsigned int a, unsigned int* b)
{
    return _SglReadSerialNumber(a, b);
}


unsigned int Wrapper::SglCryptLock(unsigned  int ProductId, unsigned  int KeyNum, unsigned  int CryptMode, unsigned  int BlockCnt, unsigned  int *Data)
{
    return _SglCryptLock(ProductId, KeyNum, CryptMode, BlockCnt, Data);
}

void Wrapper::SglTeaEncipher(const unsigned int *const   InData, unsigned int  *const         OutData, const unsigned int * const  Key) 
{
    if (sizeof(void*) == 8)
    {

        register unsigned int y = InData[0];
        register unsigned int z = InData[1];
        register unsigned int sum = 0;
        register unsigned int delta = 0x9E3779B9;
        register unsigned int a = Key[0];
        register unsigned int b = Key[1];
        register unsigned int c = Key[2];
        register unsigned int d = Key[3];
        register unsigned int n = 32;


        while (n-->0) {
            sum += delta;
            y += (z << 4) + a ^ z + sum ^ (z >> 5) + b;
            z += (y << 4) + c ^ y + sum ^ (y >> 5) + d;
        }

        OutData[0] = y;
        OutData[1] = z;
    }
    else
    {
        register unsigned int y = InData[0];
        register unsigned int z = InData[1];
        register unsigned int sum = 0;
        register unsigned int delta = 0x9E3779B9;
        register unsigned int a = Key[0];
        register unsigned int b = Key[1];
        register unsigned int c = Key[2];
        register unsigned int d = Key[3];
        register unsigned int n = 32;


        while (n-- > 0) {
            sum += delta;
            y += (z << 4) + a ^ z + sum ^ (z >> 5) + b;
            z += (y << 4) + c ^ y + sum ^ (y >> 5) + d;
        }

        OutData[0] = y;
        OutData[1] = z;
    }


}

}}
