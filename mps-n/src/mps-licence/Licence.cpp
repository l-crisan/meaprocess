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
#include <mps/licence/Licence.h>
#include <Pt/Types.h>
#include <Windows.h>
#include <string>
#include <sstream>
#include <limits>
#include <stdlib.h>
#include <time.h> 
#include  <iostream>
#include  <iomanip>
#include "Wrapper.h"

extern "C"
{

const static unsigned int g_key[4] = { 0x0678347EE, 0x6D4D2FD0, 0x267977CC, 0xFADBB35F };
static unsigned int g_authkey[12] = 
{ 
  0x994AB095, 
  0xE5CD0E1E,
  0x5EC02AFF,
  0xFF4C805B,
  0x53B5EB4B,
  0x037E61F5,
  0xB91E34DD,
  0xBFA2E192,
  0x1773DEDD,
  0xADD18851,
  0x70D84E3B,
  0x52707003
};

enum ProtuctID
{
    MeaProcessRealtime = 1,
    MeaProcessAnalyser = 2,
    MeaProcessVisual   = 4,
    MeaProcess30DayTry   = 128,
};

struct LicenceInfo
{
    int products;
    unsigned int lockSerial;
};

static int getProducts()
{
    return MeaProcessRealtime;

    unsigned int error = 0;
    int products = MeaProcessRealtime;
    
    error = mps::licence::Wrapper::SglSearchLock(products);
    
    if(error == 0)
        return products;

    return MeaProcess30DayTry;
}

static int checkTryLicenceAvailable(Pt::uint32_t app)
{
    
    HKEY resultKey;
    std::stringstream  ss;
    
    ss<<"Identities\\{B2B857D7-9294-4EC6-A4F9-C51B66D8173"<<app <<"}";

    LONG ret =  RegOpenKeyEx(HKEY_CURRENT_USER, ss.str().c_str(), 0, KEY_ALL_ACCESS, &resultKey);	

    if( ret == ERROR_SUCCESS)
    {
        RegCloseKey(resultKey);
        return 1;
    }

    return 0;
}

static void initTryLicence(Pt::uint32_t app)
{
    HKEY resultKey;
    std::stringstream  ss;
    
    ss<<"Identities\\{B2B857D7-9294-4EC6-A4F9-C51B66D8173"<<app <<"}";
    std::string subKey =   ss.str();
    LONG ret = RegCreateKey(HKEY_CURRENT_USER,  subKey.c_str(), &resultKey);

    if( ret != ERROR_SUCCESS)
        return;

    SYSTEMTIME systemTime;
    unsigned char buffer[sizeof(SYSTEMTIME)];

    GetSystemTime(&systemTime);
    memcpy(buffer, &systemTime, sizeof(SYSTEMTIME));		

    std::string value = "";
    for( int i = 0; i < sizeof(SYSTEMTIME); ++i)
    {
        char bbb[10];
        sprintf(bbb, "%02X", buffer[i]);
        value += bbb;
    }
        

    RegSetValue( HKEY_CURRENT_USER, subKey.c_str(), REG_SZ, value.c_str(), 0);

    RegCloseKey(resultKey);
}

static Pt::int64_t deltaSysTime(const SYSTEMTIME st1, const SYSTEMTIME st2)
{
    union timeunion {
        FILETIME fileTime;
        ULARGE_INTEGER ul;
    } ;
    
    timeunion ft1;
    timeunion ft2;

    SystemTimeToFileTime(&st1, &ft1.fileTime);
    SystemTimeToFileTime(&st2, &ft2.fileTime);

    return ft2.ul.QuadPart - ft1.ul.QuadPart;
}

static int isTryLicenceExpired(Pt::uint32_t app)
{
    HKEY resultKey;
    std::stringstream  ss;
    
    ss<<"Identities\\{B2B857D7-9294-4EC6-A4F9-C51B66D8173"<<app <<"}";
    std::string subKey = ss.str();

    LONG ret =  RegOpenKeyEx(HKEY_CURRENT_USER, subKey.c_str(), 0, KEY_ALL_ACCESS, &resultKey);	
    
    if( ret != ERROR_SUCCESS)
        return 1;
    
    char buffer[255];
    unsigned char buffer2[sizeof(SYSTEMTIME)];
    DWORD size = 255;

    ret = RegGetValue(HKEY_CURRENT_USER, subKey.c_str(), NULL,  RRF_RT_REG_SZ, NULL, buffer, &size);

    RegCloseKey(resultKey);

    int j = 0;

    for(DWORD i = 0; i < (sizeof(SYSTEMTIME) *2); i+= 2)
    {
        std::stringstream sm;
        sm <<buffer[i];
        sm <<buffer[i+1];
        sm<<std::hex;
        int value;
        
        sm>>value;

        buffer2[j] = (unsigned char) value;
        ++j;
    }


    int s = sizeof(SYSTEMTIME);

    SYSTEMTIME startTime;
    SYSTEMTIME currentTime;
    GetSystemTime(&currentTime);

    memcpy(&startTime, buffer2, sizeof(SYSTEMTIME));

    double deltaSeonds = deltaSysTime(startTime, currentTime)/ 10000000.0;

    return  deltaSeonds >= 30*24*60*60;
}


MPS_LICENCE_API Pt::uint64_t mps_getLicenceHandle()
{
    if (!mps::licence::Wrapper::loadDriver())
        return 0;

    unsigned int sglCodedData[2];
    unsigned int appCodedData[2];
    unsigned int random[2];

    LicenceInfo* info = new LicenceInfo;
    
    ULONG error = mps::licence::Wrapper::SglAuthent(g_authkey);
    
    if( error != 0 )
    {
        delete info;
        return 0;	
    }
    
    info->products = getProducts();

    if( info->products == 0)
    {
        delete info;
        return 0;
    }
    
    if( info->products == MeaProcess30DayTry)
        return (Pt::uint64_t)info;

    error = mps::licence::Wrapper::SglReadSerialNumber(info->products, &info->lockSerial);

    if( error != 0 )
    {
        delete info;
        return 0;	
    }

    srand ((unsigned int) time(NULL));
    random[0] = rand() << 16 | rand();
    random[1] = rand() << 16 | rand();

    sglCodedData[0] = random[0];
    sglCodedData[1] = random[1];

    error = mps::licence::Wrapper::SglCryptLock(info->products, 0, 0, 1, sglCodedData);
    
    if( error != 0 )
    {
        delete info;
        return 0;	
    }

    mps::licence::Wrapper::SglTeaEncipher(random, appCodedData, g_key);
    
    if( (sglCodedData[0] != appCodedData[0]) || (sglCodedData[1] != appCodedData[1]))
    {
        delete info;
        return 0;
    }
    
    return 	(Pt::uint64_t)	info;
}

MPS_LICENCE_API unsigned int mps_getLicencedApps(Pt::uint64_t licence, Pt::uint32_t app)
{
    LicenceInfo* info = (LicenceInfo*) licence;

    if( (info->products & app) != 0)
        return info->products;

    if( (info->products & MeaProcess30DayTry) != 0)
    {
        if(!checkTryLicenceAvailable(app))
        {
            initTryLicence(app);
            return info->products;
        }
        else
        {
            if(!isTryLicenceExpired(app))
                return info->products;
        }
    }

    return 0;
}

MPS_LICENCE_API unsigned int mps_getLockSerial(Pt::uint64_t licence)
{
    LicenceInfo* info = (LicenceInfo*) licence;
    return info->lockSerial;
}


MPS_LICENCE_API void mps_freeLicence(Pt::uint64_t licence)
{
    LicenceInfo* info = (LicenceInfo*) licence;
    delete info;
}
 
}
