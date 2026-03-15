//    MeaProcess - Meaurement and Automation framework.
//    Copyright (C) 2015  Laurentiu-Gheorghe Crisan
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;
using Mpal.Model;
using Antlr.Runtime.Tree;

namespace Mpal.Compiler
{
    internal class CodeOutput
    {
        private Function _function;
        private uint _tempOp = 1;
        private CompilerOptions _options;

        public CodeOutput(Function func, CompilerOptions options)
        {
            _options = options;
            _function = func;            
        }

        public uint CreateTempOpID()
        {
            return ++_tempOp;
        }

        private bool IsTemporaryOp(Operand op)
        {
            return op.OpType == Operand.Type.TemporaryRef || op.OpType == Operand.Type.Temporary;
        }

        public Instruction EmitInstruction(InstructionCode code, ulong uid, Operand result, Operand op1, Operand op2)
        {
            Instruction inst = new Instruction(code, uid, result, op1, op2);
            _function.Instructions.Add(inst);
            return inst;
        }

        public Operand EmitCreateTempOperandInst(ulong uid, uint size, string typeId)
        {
            Operand tempOp = new Operand(CreateTempOpID(), uid, Operand.Type.Temporary, size, typeId);
            Operand sizeOp = new Operand(size, 0, 4, "UDINT");
            EmitInstruction(InstructionCode.PushTempOp, uid, tempOp, sizeOp, null);
            return tempOp;
        }

        public Operand EmitCreateTempReferenceInst(ulong uid, uint size, string typeId)
        {
            Operand tempOp = new Operand(CreateTempOpID(), uid, Operand.Type.TemporaryRef, size, typeId);
            Operand sizeOp = new Operand(_options.VmAddressSize, 0, 4, "UDINT");
            EmitInstruction(InstructionCode.PushTempOp, uid, tempOp, sizeOp, null);
            return tempOp;
        }

        public void EmitRemoveTempOperandInst(Operand op)
        {
            if (!IsTemporaryOp(op) || op.Cached)
                return;

            EmitInstruction(InstructionCode.PopTempOp, op.Uid, op, null, null);
        }
    }
}
