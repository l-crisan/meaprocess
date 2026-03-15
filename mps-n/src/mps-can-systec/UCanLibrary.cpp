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
#include "UCanLibrary.h"

namespace mps{
namespace can{
namespace systec{

tfpUcanInitHwConnectControlEx UCanLibrary::_initHwConnectControlEx;
tfpUcanDeinitHwConnectControl UCanLibrary::_deinitHwConnectControl;
tfpUcanGetVersionEx           UCanLibrary::_getVersionEx;
tfpUcanInitHardwareEx         UCanLibrary::_initHardwareEx;
tfpUcanInitCanEx2             UCanLibrary::_initCanEx2;
tfpUcanSetTxTimeout           UCanLibrary::_setTxTimeout;
tfpUcanDeinitCanEx            UCanLibrary::_deinitCanEx;
tfpUcanDeinitHardware         UCanLibrary::_deinitHardware;
tfpUcanReadCanMsgEx           UCanLibrary::_readCanMsgEx;
tfpUcanWriteCanMsgEx          UCanLibrary::_writeCanMsgEx;
tfpUcanGetHardwareInfoEx2     UCanLibrary::_getHardwareInfoEx2;
tfpUcanGetMsgPending          UCanLibrary::_getMsgPending;
tfpUcanGetCanErrorCounter     UCanLibrary::_getCanErrorCounter;
tfpUcanGetMsgCountInfoEx      UCanLibrary::_getMsgCountInfoEx;
tfpUcanResetCanEx             UCanLibrary::_resetCanEx;
tfpUcanGetStatusEx            UCanLibrary::_getStatusEx;
tfpUcanDefineCyclicCanMsg     UCanLibrary::_defineCyclicCanMsg;
tfpUcanEnableCyclicCanMsg     UCanLibrary::_enableCyclicCanMsg;
bool                          UCanLibrary::_isOpen;
Pt::System::Library           UCanLibrary::_library;


}}}