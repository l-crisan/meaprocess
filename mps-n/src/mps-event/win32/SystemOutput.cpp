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
 #include "SystemOutput.h"
#include <Windows.h>
#include <Winbase.h> 

namespace mps{			
namespace eventps{
	
void playSound(Pt::uint8_t* wavBuffer)
{
	sndPlaySound((LPCTSTR)wavBuffer, SND_MEMORY|SND_ASYNC);
}

void writeSystemEvent(int type, const char* text)
{
    HANDLE handle  = OpenEventLogA( NULL, "MeaProcess");
    
    if( handle == 0)
        return;

    WORD eType = 0;

    switch(type)
    {
        case 0://Info
            eType = EVENTLOG_INFORMATION_TYPE;
        break;

        case 1://Warning
            eType = EVENTLOG_WARNING_TYPE;
        break;

        case 2: //Error
            eType = EVENTLOG_ERROR_TYPE;
        break;
    }

    const char* errorMessage[1];
    *errorMessage = text;
    ReportEvent(handle , eType, 0, 0, NULL, 1, 0, errorMessage, NULL);    
    CloseEventLog(handle);
}

}}