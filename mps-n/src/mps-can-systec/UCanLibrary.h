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
#ifndef MPS_CAN_SYSTEC_UCAN_H
#define MPS_CAN_SYSTEC_UCAN_H

#include <windows.h>
#include<TCHAR.H>
#include <Usbcan32.h>
#include <Pt/System/Library.h>

namespace mps{
namespace can{
namespace systec{

typedef BYTE (__stdcall *tfpUcanInitHwConnectControlEx)(tConnectControlFktEx fpConnectControlFktEx_p, void* pCallbackArg_p);
typedef BYTE (__stdcall *tfpUcanDeinitHwConnectControl)(void);
typedef DWORD   (__stdcall *tfpUcanGetVersionEx)           (tUcanVersionType VerType_p);
typedef BYTE (__stdcall *tfpUcanInitHardwareEx)         (tUcanHandle* pUcanHandle_p, BYTE bDeviceNr_p, tCallbackFktEx fpCallbackFktEx_p, void* pCallbackArg_p);
typedef BYTE (__stdcall *tfpUcanInitCanEx2)             (tUcanHandle UcanHandle_p, BYTE bChannel_p, tUcanInitCanParam* pInitCanParam_p);
typedef BYTE (__stdcall *tfpUcanSetTxTimeout)           (tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD dwTxTimeout_p);
typedef BYTE (__stdcall *tfpUcanDeinitCanEx)            (tUcanHandle UcanHandle_p, BYTE bChannel_p);
typedef BYTE (__stdcall *tfpUcanDeinitHardware)         (tUcanHandle UcanHandle_p);
typedef BYTE (__stdcall *tfpUcanReadCanMsgEx)           (tUcanHandle UcanHandle_p, BYTE* pbChannel_p, tCanMsgStruct* pCanMsg_p, DWORD* pdwCount_p);
typedef BYTE (__stdcall *tfpUcanWriteCanMsgEx)          (tUcanHandle UcanHandle_p, BYTE bChannel_p, tCanMsgStruct* pCanMsg_p, DWORD* pdwCount_p);
typedef BYTE (__stdcall *tfpUcanGetHardwareInfoEx2)     (tUcanHandle UcanHandle_p, tUcanHardwareInfoEx* pHwInfo_p, tUcanChannelInfo* pCanInfoCh0_p, tUcanChannelInfo* pCanInfoCh1_p);
typedef BYTE (__stdcall *tfpUcanGetMsgPending)          (tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD dwFlags_p, DWORD* pdwPendingCount_p);
typedef BYTE (__stdcall *tfpUcanGetCanErrorCounter)     (tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD* pdwTxErrorCounter_p, DWORD* pdwRxErrorCounter_p);
typedef BYTE (__stdcall *tfpUcanGetMsgCountInfoEx)      (tUcanHandle UcanHandle_p, BYTE bChannel_p, tUcanMsgCountInfo* pMsgCountInfo_p);
typedef BYTE (__stdcall *tfpUcanResetCanEx)             (tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD dwResetFlags_p);
typedef BYTE (__stdcall *tfpUcanGetStatusEx)            (tUcanHandle UcanHandle_p, BYTE bChannel_p, tStatusStruct* pStatus_p);
typedef BYTE (__stdcall *tfpUcanDefineCyclicCanMsg)     (tUcanHandle UcanHandle_p, BYTE bChannel_p, tCanMsgStruct* pCanMsgList_p, DWORD dwCount_p);
typedef BYTE (__stdcall *tfpUcanEnableCyclicCanMsg)     (tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD dwFlags_p);


class UCanLibrary
{
public:
    static bool loadDriver()
    {
        if(_isOpen)
            return true;

        try
        {
            _library.open("Usbcan32.dll");
            _isOpen = true;
            _initHwConnectControlEx  = (tfpUcanInitHwConnectControlEx) _library.resolve("UcanInitHwConnectControlEx");
            _deinitHwConnectControl  = (tfpUcanDeinitHwConnectControl) _library.resolve("UcanDeinitHwConnectControl");
            _getVersionEx            = (tfpUcanGetVersionEx)           _library.resolve("UcanGetVersionEx");
            _initHardwareEx          = (tfpUcanInitHardwareEx) _library.resolve("UcanInitHardwareEx");
            _initCanEx2              = (tfpUcanInitCanEx2)     _library.resolve( "UcanInitCanEx2");
            _setTxTimeout            = (tfpUcanSetTxTimeout)   _library.resolve("UcanSetTxTimeout");
            _deinitCanEx             = (tfpUcanDeinitCanEx)    _library.resolve("UcanDeinitCanEx");
            _deinitHardware          = (tfpUcanDeinitHardware) _library.resolve( "UcanDeinitHardware");
            _readCanMsgEx            = (tfpUcanReadCanMsgEx)   _library.resolve("UcanReadCanMsgEx");
            _writeCanMsgEx           = (tfpUcanWriteCanMsgEx)   _library.resolve("UcanWriteCanMsgEx");
            _getHardwareInfoEx2      = (tfpUcanGetHardwareInfoEx2) _library.resolve("UcanGetHardwareInfoEx2");
            _getMsgPending           = (tfpUcanGetMsgPending) _library.resolve("UcanGetMsgPending");
            _getCanErrorCounter      = (tfpUcanGetCanErrorCounter) _library.resolve( "UcanGetCanErrorCounter");
            _getMsgCountInfoEx       = (tfpUcanGetMsgCountInfoEx) _library.resolve("UcanGetMsgCountInfoEx");
            _resetCanEx              = (tfpUcanResetCanEx)_library.resolve( "UcanResetCanEx");
            _getStatusEx             = (tfpUcanGetStatusEx) _library.resolve( "UcanGetStatusEx");
            _defineCyclicCanMsg      = (tfpUcanDefineCyclicCanMsg) _library.resolve("UcanDefineCyclicCanMsg");
            _enableCyclicCanMsg      = (tfpUcanEnableCyclicCanMsg) _library.resolve("UcanEnableCyclicCanMsg");

        }
        catch(...)
        {
            return false;
        }

        return true;
    }


    static BYTE initHwConnectControlEx(tConnectControlFktEx fpConnectControlFktEx_p, void* pCallbackArg_p)
    {
        return _initHwConnectControlEx(fpConnectControlFktEx_p, pCallbackArg_p);
    }

    static BYTE deinitHwConnectControl(void)
    {
        return _deinitHwConnectControl();
    }

    static DWORD  getVersionEx(tUcanVersionType VerType_p)
    {
        return _getVersionEx(VerType_p);
    }

    static BYTE initHardwareEx(tUcanHandle* pUcanHandle_p, BYTE bDeviceNr_p, tCallbackFktEx fpCallbackFktEx_p, void* pCallbackArg_p)
    {
        return _initHardwareEx(pUcanHandle_p, bDeviceNr_p, fpCallbackFktEx_p, pCallbackArg_p);
    }

    static BYTE initCanEx2(tUcanHandle UcanHandle_p, BYTE bChannel_p, tUcanInitCanParam* pInitCanParam_p)
    {
        return _initCanEx2(UcanHandle_p, bChannel_p, pInitCanParam_p);
    }

    static BYTE setTxTimeout(tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD dwTxTimeout_p)
    {
        return _setTxTimeout(UcanHandle_p, bChannel_p, dwTxTimeout_p);
    }

    static BYTE deinitCanEx(tUcanHandle UcanHandle_p, BYTE bChannel_p)
    {
        return _deinitCanEx(UcanHandle_p, bChannel_p);
    }

    static BYTE deinitHardware(tUcanHandle UcanHandle_p)
    {

        return _deinitHardware(UcanHandle_p);
    }

    static BYTE readCanMsgEx(tUcanHandle UcanHandle_p, BYTE* pbChannel_p, tCanMsgStruct* pCanMsg_p, DWORD* pdwCount_p)
    {
        return _readCanMsgEx(UcanHandle_p, pbChannel_p, pCanMsg_p, pdwCount_p);
    }

    static BYTE writeCanMsgEx(tUcanHandle UcanHandle_p, BYTE bChannel_p, tCanMsgStruct* pCanMsg_p, DWORD* pdwCount_p)
    {
        return _writeCanMsgEx(UcanHandle_p, bChannel_p, pCanMsg_p, pdwCount_p);
    }

    static BYTE hardwareInfoEx2(tUcanHandle UcanHandle_p, tUcanHardwareInfoEx* pHwInfo_p, tUcanChannelInfo* pCanInfoCh0_p, tUcanChannelInfo* pCanInfoCh1_p)
    {
        return _getHardwareInfoEx2(UcanHandle_p, pHwInfo_p, pCanInfoCh0_p, pCanInfoCh1_p);
    }

    static BYTE getMsgPending (tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD dwFlags_p, DWORD* pdwPendingCount_p)
    {
        return _getMsgPending(UcanHandle_p, bChannel_p, dwFlags_p, pdwPendingCount_p);
    }

    static BYTE getCanErrorCounter(tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD* pdwTxErrorCounter_p, DWORD* pdwRxErrorCounter_p)
    {
        return _getCanErrorCounter(UcanHandle_p, bChannel_p, pdwTxErrorCounter_p, pdwRxErrorCounter_p);
    }

    static BYTE getMsgCountInfoEx(tUcanHandle UcanHandle_p, BYTE bChannel_p, tUcanMsgCountInfo* pMsgCountInfo_p)
    {
        return _getMsgCountInfoEx(UcanHandle_p, bChannel_p, pMsgCountInfo_p);
    }

    static BYTE resetCanEx(tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD dwResetFlags_p)
    {
        return _resetCanEx(UcanHandle_p, bChannel_p, dwResetFlags_p);
    }

    static BYTE getStatusEx(tUcanHandle UcanHandle_p, BYTE bChannel_p, tStatusStruct* pStatus_p)
    {
        return _getStatusEx(UcanHandle_p, bChannel_p, pStatus_p);
    }

    static BYTE defineCyclicCanMsg(tUcanHandle UcanHandle_p, BYTE bChannel_p, tCanMsgStruct* pCanMsgList_p, DWORD dwCount_p)
    {
        return _defineCyclicCanMsg(UcanHandle_p, bChannel_p, pCanMsgList_p, dwCount_p);
    }

    static BYTE enableCyclicCanMsg (tUcanHandle UcanHandle_p, BYTE bChannel_p, DWORD dwFlags_p)
    {
        return _enableCyclicCanMsg(UcanHandle_p, bChannel_p, dwFlags_p);
    }

private:
    static tfpUcanInitHwConnectControlEx _initHwConnectControlEx;
    static tfpUcanDeinitHwConnectControl _deinitHwConnectControl;
    static tfpUcanGetVersionEx           _getVersionEx;
    static tfpUcanInitHardwareEx         _initHardwareEx;
    static tfpUcanInitCanEx2             _initCanEx2;
    static tfpUcanSetTxTimeout           _setTxTimeout;
    static tfpUcanDeinitCanEx            _deinitCanEx;
    static tfpUcanDeinitHardware         _deinitHardware;
    static tfpUcanReadCanMsgEx           _readCanMsgEx;
    static tfpUcanWriteCanMsgEx          _writeCanMsgEx;
    static tfpUcanGetHardwareInfoEx2     _getHardwareInfoEx2;
    static tfpUcanGetMsgPending          _getMsgPending;
    static tfpUcanGetCanErrorCounter     _getCanErrorCounter;
    static tfpUcanGetMsgCountInfoEx      _getMsgCountInfoEx;
    static tfpUcanResetCanEx             _resetCanEx;
    static tfpUcanGetStatusEx            _getStatusEx;
    static tfpUcanDefineCyclicCanMsg     _defineCyclicCanMsg;
    static tfpUcanEnableCyclicCanMsg     _enableCyclicCanMsg;
    static bool                         _isOpen;
    static Pt::System::Library          _library;
};

}}}
#endif
