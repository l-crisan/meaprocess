/***************************************************************************
 *   Copyright (C) 2010 by Atesion GmbH                                    *
 *                                                                         *
 *   All rights reserved.												   *
 **************************************************************************/
#ifndef MPS_CANDRV_PEAKCANDRVWRAPPER_H
#define MPS_CANDRV_PEAKCANDRVWRAPPER_H

#include <windows.h>
#include <PCANBasic.h>
#include <Pt/System/Library.h>

namespace mps{
namespace candrv{

typedef TPCANStatus (__stdcall *TCAN_Initialize)( TPCANHandle Channel, TPCANBaudrate Btr0Btr1,  TPCANType HwType, DWORD IOPort, WORD Interrupt);
typedef TPCANStatus (__stdcall *TCAN_Uninitialize)(TPCANHandle Channel);
typedef TPCANStatus (__stdcall *TCAN_Read)(TPCANHandle Channel, TPCANMsg* MessageBuffer, TPCANTimestamp* TimestampBuffer);
typedef TPCANStatus (__stdcall *TCAN_Write)( TPCANHandle Channel, TPCANMsg* MessageBuffer);
typedef TPCANStatus (__stdcall *TCAN_SetValue)(TPCANHandle Channel, TPCANParameter Parameter, void* Buffer, DWORD BufferLength);
typedef TPCANStatus (__stdcall *TCAN_Reset)(TPCANHandle Channel);

class PeakCANDrvWrapper
{
	public:
		static bool loadDriver()
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

		static TPCANStatus CAN_Initialize( TPCANHandle Channel, TPCANBaudrate Btr0Btr1, TPCANType HwType, DWORD IOPort, WORD Interrupt)
		{
			return _CAN_Initialize(Channel, Btr0Btr1, HwType, IOPort, Interrupt);
		}
	
        static TPCANStatus CAN_Uninitialize(TPCANHandle Channel)
        {
            return _CAN_Uninitialize(Channel);
        }

        static TPCANStatus CAN_Read(TPCANHandle Channel, TPCANMsg* MessageBuffer, TPCANTimestamp* TimestampBuffer)
        {
            return _CAN_Read(Channel, MessageBuffer, TimestampBuffer);
        }

        static TPCANStatus CAN_Write(TPCANHandle Channel, TPCANMsg* MessageBuffer)
        {
            return _CAN_Write(Channel, MessageBuffer);
        }

        static TPCANStatus CAN_SetValue(TPCANHandle Channel, TPCANParameter Parameter, void* Buffer, DWORD BufferLength)
        {
            return _CAN_SetValue( Channel, Parameter, Buffer, BufferLength);
        }

        static TPCANStatus CAN_Reset(TPCANHandle Channel)
        {
            return _CAN_Reset(Channel);
        }

	private:
		static TCAN_Initialize _CAN_Initialize;
        static TCAN_Uninitialize _CAN_Uninitialize;
        static TCAN_Read _CAN_Read;
        static TCAN_Write _CAN_Write;
        static TCAN_SetValue _CAN_SetValue;
        static TCAN_Reset _CAN_Reset;
		static Pt::System::Library _library;
		static bool _isopen;
};

}}

#endif
