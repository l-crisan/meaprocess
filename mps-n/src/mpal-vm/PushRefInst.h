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
#ifndef MPAL_VM_PUSHREFINST_H
#define MPAL_VM_PUSHREFINST_H

#include "Instruction.h"

namespace mpal{
namespace vm{

class PushRefInst : public Instruction
{
    public:
        PushRefInst()
        { }

        ~PushRefInst()
        { }

        virtual Instruction* clone()
        {
            return new PushRefInst();
        }

        virtual int execute(VMContext& context)
        {
            const Pt::uint32_t currentSize = (Pt::uint32_t) ((context.stackPtr + sizeof(void*)) - context.baseAdr);
            
            if( currentSize >= context.stackSize)
            {
                std::stringstream ss;
                ss<<" Function '"<<context.currentFunction;
                ss<<"' "<< LineInfo::lineInfo(uid());
                ss<<": error R1006: Out of memory, stack over flow";
                throw std::logic_error(ss.str());
            }

            void* toPush =  _op1->calcAddress(context.basePtr);
            memcpy(context.stackPtr, &toPush, sizeof(void*));

            context.stackPtr += sizeof(void*);

            return -1;
        }
};

}}

#endif

