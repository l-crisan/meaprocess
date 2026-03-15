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
#ifndef MPS_LICENCE_WRAPPER_H
#define MPS_LICENCE_WRAPPER_H

#include <Pt/Types.h>
#include <Pt/System/Library.h>

namespace mps{
namespace licence{

typedef unsigned int (__stdcall *SglSearchLock_t)(unsigned int products);
typedef unsigned int(__stdcall *SglAuthentB_t)(unsigned int* auth);
typedef unsigned int(__stdcall *SglAuthentA_t)(unsigned int *AuthentCode, unsigned int *AppRandNum, unsigned int *LibRandNum);
typedef unsigned int (__stdcall *SglReadSerialNumber_t)(unsigned int a, unsigned int* b);
typedef unsigned int (__stdcall *SglCryptLock_t)(unsigned  int ProductId, unsigned  int KeyNum, unsigned  int CryptMode, unsigned  int BlockCnt, unsigned  int *Data);


class Wrapper
{
public:
    static bool loadDriver();

    static unsigned int SglSearchLock(unsigned int products);
    static unsigned int SglAuthent(unsigned int* auth);
    static unsigned int SglReadSerialNumber(unsigned int a, unsigned int* b);
    static unsigned int SglCryptLock(unsigned  int ProductId, unsigned  int KeyNum, unsigned  int CryptMode, unsigned  int BlockCnt, unsigned  int *Data);
    static void SglTeaEncipher(const unsigned int *const   InData, unsigned int *const OutData, const unsigned int * const  Key);


private:
    static SglSearchLock_t  _SglSearchLock;
    static SglAuthentA_t _SglAuthentA;
    static SglAuthentB_t _SglAuthentB;
    static SglReadSerialNumber_t  _SglReadSerialNumber;
    static SglCryptLock_t _SglCryptLock;
    static bool _isopen;
    static Pt::System::Library _library;
};

}}

#endif
