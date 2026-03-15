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
#ifndef MPAL_VM_INSTRUCTIONTABLE_H
#define MPAL_VM_INSTRUCTIONTABLE_H

#include <Pt/Singleton.h>

#include "InstructionCode.h"

#include "AddInst.h"
#include "SubInst.h"
#include "MulInst.h"
#include "DivInst.h"
#include "GrInst.h"
#include "LsInst.h"
#include "GeInst.h"
#include "LeInst.h"
#include "EqInst.h"
#include "NeInst.h"
#include "MoveInst.h"
#include "PushTempInst.h"
#include "PopTempInst.h"
#include "PushRefInst.h"
#include "PopRefInst.h"
#include "AddOffsetInst.h"
#include "ConvertInst.h"
#include "ModInst.h"
#include "OrInst.h"
#include "AndInst.h"
#include "XorInst.h"
#include "PowInst.h"
#include "NotInst.h"
#include "NegInst.h"
#include "JmpTRUEInst.h"
#include "JmpFALSEInst.h"
#include "JmpInst.h"
#include "IncInst.h"
#include "DecInst.h"
#include "ReturnInst.h"
#include "RoundInst.h"
#include "TruncInst.h"
#include "AbsInst.h"
#include "SqrInst.h"
#include "SqrtInst.h"
#include "ExpInst.h"
#include "ExpdInst.h"
#include "LnInst.h"
#include "LogInst.h"
#include "AcosInst.h"
#include "AsinInst.h"
#include "AtanInst.h"
#include "CosInst.h"
#include "SinInst.h"
#include "TanInst.h"
#include "RolInst.h"
#include "RorInst.h"
#include "ShlInst.h"
#include "ShrInst.h"
#include "SelInst.h"
#include "MinInst.h"
#include "MaxInst.h"
#include "LimitInst.h"
#include "MuxInst.h"

namespace mpal{
namespace vm{

class InstructionTable : public Pt::Singleton<InstructionTable>
{
    friend class Pt::Singleton<InstructionTable>;

    public:
        Instruction* createInstruction(InstructionCode code) const;

    protected: 

        InstructionTable();

        virtual ~InstructionTable();

    protected:
        typedef std::pair<InstructionCode, Instruction*> Item;

        std::map<InstructionCode, Instruction*> _instTable;
};

}}


#endif

