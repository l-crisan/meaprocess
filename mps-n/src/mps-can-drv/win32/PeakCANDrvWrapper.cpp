/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#include <windows.h>
#include "PeakCANDrvWrapper.h"

namespace mps{
namespace candrv{

static TCAN_Initialize PeakCANDrvWrapper::_CAN_Initialize = 0;
static TCAN_Uninitialize PeakCANDrvWrapper::_CAN_Uninitialize = 0;
static TCAN_Read PeakCANDrvWrapper::_CAN_Read = 0;
static TCAN_Write PeakCANDrvWrapper::_CAN_Write = 0;
static TCAN_SetValue PeakCANDrvWrapper::_CAN_SetValue = 0;
static TCAN_Reset PeakCANDrvWrapper::_CAN_Reset = 0;
static Pt::System::Library PeakCANDrvWrapper::_library;
static bool PeakCANDrvWrapper::_isopen = false;

bool PeakCANDrvWrapper::loadDriver()
{
	if(_isopen)
		return true;

	try
	{
		_library.open("PCANBasic.dll");

		_isopen = true;

		_CAN_Initialize     = (TCAN_Initialize)   _library.resolve("CAN_Initialize");
        _CAN_Uninitialize   = (TCAN_Uninitialize) _library.resolve("CAN_Uninitialize");
        _CAN_Read           = (TCAN_Read)         _library.resolve("CAN_Read");
        _CAN_Write          = (TCAN_Write)        _library.resolve("CAN_Write");
        _CAN_SetValue       = (TCAN_SetValue)     _library.resolve("CAN_SetValue");
        _CAN_Reset          = (TCAN_Reset)        _library.resolve("CAN_Reset");
	}
	catch(...)
	{
		return false;
	}

	return true;
}

static TPCANStatus PeakCANDrvWrapper::CAN_Initialize( TPCANHandle Channel, TPCANBaudrate Btr0Btr1, TPCANType HwType, DWORD IOPort, WORD Interrupt)
{
	return _CAN_Initialize(Channel, Btr0Btr1, HwType, IOPort, Interrupt);
}
	
static TPCANStatus PeakCANDrvWrapper::CAN_Uninitialize(TPCANHandle Channel)
{
    return _CAN_Uninitialize(Channel);
}

static TPCANStatus PeakCANDrvWrapper::CAN_Read(TPCANHandle Channel, TPCANMsg* MessageBuffer, TPCANTimestamp* TimestampBuffer)
{
    return _CAN_Read(Channel, MessageBuffer, TimestampBuffer);
}

static TPCANStatus PeakCANDrvWrapper::CAN_Write(TPCANHandle Channel, TPCANMsg* MessageBuffer)
{
    return _CAN_Write(Channel, MessageBuffer);
}

static TPCANStatus PeakCANDrvWrapper::CAN_SetValue(TPCANHandle Channel, TPCANParameter Parameter, void* Buffer, DWORD BufferLength)
{
    return _CAN_SetValue( Channel, Parameter, Buffer, BufferLength);
}

static TPCANStatus PeakCANDrvWrapper::CAN_Reset(TPCANHandle Channel)
{
    return _CAN_Reset(Channel);
}

}}