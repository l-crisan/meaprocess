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
#include "InstructionTable.h"

namespace mpal{
namespace vm{

InstructionTable::InstructionTable()
{ 
    
    //Add
    _instTable.insert( Item(AddSINT, new AddInst<Pt::int8_t>()) );
    _instTable.insert( Item(AddINT, new AddInst<Pt::int16_t>()) );
    _instTable.insert( Item(AddDINT, new AddInst<Pt::int32_t>()) );
    _instTable.insert( Item(AddLINT, new AddInst<Pt::int64_t>()) );
    _instTable.insert( Item(AddUSINT, new AddInst<Pt::uint8_t>()) );
    _instTable.insert( Item(AddUINT, new AddInst<Pt::uint16_t>()) );
    _instTable.insert( Item(AddUDINT, new AddInst<Pt::uint32_t>()) );
    _instTable.insert( Item(AddULINT, new AddInst<Pt::uint64_t>() ));
    _instTable.insert( Item(AddREAL, new AddInst<float>()) );
    _instTable.insert( Item(AddLREAL, new AddInst<double>()) );

    //Sub
    _instTable.insert( Item(SubSINT, new SubInst<Pt::int8_t>()) );
    _instTable.insert( Item(SubINT, new SubInst<Pt::int16_t>()) );
    _instTable.insert( Item(SubDINT, new SubInst<Pt::int32_t>()) );
    _instTable.insert( Item(SubLINT, new SubInst<Pt::int64_t>()) );
    _instTable.insert( Item(SubUSINT, new SubInst<Pt::uint8_t>()) );
    _instTable.insert( Item(SubUINT, new SubInst<Pt::uint16_t>()) );
    _instTable.insert( Item(SubUDINT, new SubInst<Pt::uint32_t>()) );
    _instTable.insert( Item(SubULINT, new SubInst<Pt::uint64_t>() ));
    _instTable.insert( Item(SubREAL, new SubInst<float>()) );
    _instTable.insert( Item(SubLREAL, new SubInst<double>()) );


    //Mul
    _instTable.insert( Item(MulSINT, new MulInst<Pt::int8_t>()) );
    _instTable.insert( Item(MulINT, new MulInst<Pt::int16_t>() ));
    _instTable.insert( Item(MulDINT, new MulInst<Pt::int32_t>() ));
    _instTable.insert( Item(MulLINT, new MulInst<Pt::int64_t>() ));
    _instTable.insert( Item(MulUSINT, new MulInst<Pt::uint8_t>() ));
    _instTable.insert( Item(MulUINT, new MulInst<Pt::uint16_t>() ));
    _instTable.insert( Item(MulUDINT, new MulInst<Pt::uint32_t>()) );
    _instTable.insert( Item(MulULINT, new MulInst<Pt::uint64_t>()) );
    _instTable.insert( Item(MulREAL, new MulInst<float>() ));
    _instTable.insert( Item(MulLREAL, new MulInst<double>()) );

    //Div
    _instTable.insert( Item(DivSINT, new DivInst<Pt::int8_t>()) );
    _instTable.insert( Item(DivINT, new DivInst<Pt::int16_t>() ));
    _instTable.insert( Item(DivDINT, new DivInst<Pt::int32_t>()) );
    _instTable.insert( Item(DivLINT, new DivInst<Pt::int64_t>()) );
    _instTable.insert( Item(DivUSINT, new DivInst<Pt::uint8_t>()) );
    _instTable.insert( Item(DivUINT, new DivInst<Pt::uint16_t>()) );
    _instTable.insert( Item(DivUDINT, new DivInst<Pt::uint32_t>()) );
    _instTable.insert( Item(DivULINT, new DivInst<Pt::uint64_t>()) );
    _instTable.insert( Item(DivREAL, new DivInst<float>()) );
    _instTable.insert( Item(DivLREAL, new DivInst<double>()) );

    //Greater
    _instTable.insert( Item(GrSINT, new GrInst<Pt::int8_t>()) );
    _instTable.insert( Item(GrINT, new GrInst<Pt::int16_t>()) );
    _instTable.insert( Item(GrDINT, new GrInst<Pt::int32_t>()) );
    _instTable.insert( Item(GrLINT, new GrInst<Pt::int64_t>()) );
    _instTable.insert( Item(GrUSINT, new GrInst<Pt::uint8_t>()) );
    _instTable.insert( Item(GrUINT, new GrInst<Pt::uint16_t>() ));
    _instTable.insert( Item(GrUDINT, new GrInst<Pt::uint32_t>() ));
    _instTable.insert( Item(GrULINT, new GrInst<Pt::uint64_t>()) );
    _instTable.insert( Item(GrREAL, new GrInst<float>()) );
    _instTable.insert( Item(GrLREAL, new GrInst<double>()) );

    //Less
    _instTable.insert( Item(LsSINT, new LsInst<Pt::int8_t>()) );
    _instTable.insert( Item(LsINT, new LsInst<Pt::int16_t>()) );
    _instTable.insert( Item(LsDINT, new LsInst<Pt::int32_t>()) );
    _instTable.insert( Item(LsLINT, new LsInst<Pt::int64_t>()) );
    _instTable.insert( Item(LsUSINT, new LsInst<Pt::uint8_t>()) );
    _instTable.insert( Item(LsUINT, new LsInst<Pt::uint16_t>()) );
    _instTable.insert( Item(LsUDINT, new LsInst<Pt::uint32_t>()) );
    _instTable.insert( Item(LsULINT, new LsInst<Pt::uint64_t>()) );
    _instTable.insert( Item(LsREAL, new LsInst<float>()) );
    _instTable.insert( Item(LsLREAL, new LsInst<double>()) );


    //Greater equal
    _instTable.insert( Item(GeSINT, new GeInst<Pt::int8_t>()) );
    _instTable.insert( Item(GeINT, new GeInst<Pt::int16_t>()) );
    _instTable.insert( Item(GeDINT, new GeInst<Pt::int32_t>()) );
    _instTable.insert( Item(GeLINT, new GeInst<Pt::int64_t>()) );
    _instTable.insert( Item(GeUSINT, new GeInst<Pt::uint8_t>()) );
    _instTable.insert( Item(GeUINT, new GeInst<Pt::uint16_t>()) );
    _instTable.insert( Item(GeUDINT, new GeInst<Pt::uint32_t>()) );
    _instTable.insert( Item(GeULINT, new GeInst<Pt::uint64_t>()) );
    _instTable.insert( Item(GeREAL, new GeInst<float>()) );
    _instTable.insert( Item(GeLREAL, new GeInst<double>()) );

    //Less equal
    _instTable.insert( Item(LeSINT, new LeInst<Pt::int8_t>()) );
    _instTable.insert( Item(LeINT, new LeInst<Pt::int16_t>()) );
    _instTable.insert( Item(LeDINT, new LeInst<Pt::int32_t>()) );
    _instTable.insert( Item(LeLINT, new LeInst<Pt::int64_t>()) );
    _instTable.insert( Item(LeUSINT, new LeInst<Pt::uint8_t>()) );
    _instTable.insert( Item(LeUINT, new LeInst<Pt::uint16_t>()) );
    _instTable.insert( Item(LeUDINT, new LeInst<Pt::uint32_t>()) );
    _instTable.insert( Item(LeULINT, new LeInst<Pt::uint64_t>()) );
    _instTable.insert( Item(LeREAL, new LeInst<float>()) );
    _instTable.insert( Item(LeLREAL, new LeInst<double>()) );

    //Equal
    _instTable.insert( Item(EqSINT, new EqInst<Pt::int8_t>()) );
    _instTable.insert( Item(EqINT, new EqInst<Pt::int16_t>()) );
    _instTable.insert( Item(EqDINT, new EqInst<Pt::int32_t>()) );
    _instTable.insert( Item(EqLINT, new EqInst<Pt::int64_t>()) );
    _instTable.insert( Item(EqUSINT, new EqInst<Pt::uint8_t>()) );
    _instTable.insert( Item(EqUINT, new EqInst<Pt::uint16_t>()) );
    _instTable.insert( Item(EqUDINT, new EqInst<Pt::uint32_t>()) );
    _instTable.insert( Item(EqULINT, new EqInst<Pt::uint64_t>()) );
    _instTable.insert( Item(EqREAL, new EqInst<float>()) );
    _instTable.insert( Item(EqLREAL, new EqInst<double>()) );

    //Not Equal
    _instTable.insert( Item(NeSINT, new NeInst<Pt::int8_t>()) );
    _instTable.insert( Item(NeINT, new NeInst<Pt::int16_t>()) );
    _instTable.insert( Item(NeDINT, new NeInst<Pt::int32_t>()) );
    _instTable.insert( Item(NeLINT, new NeInst<Pt::int64_t>()) );
    _instTable.insert( Item(NeUSINT, new NeInst<Pt::uint8_t>()) );
    _instTable.insert( Item(NeUINT, new NeInst<Pt::uint16_t>()) );
    _instTable.insert( Item(NeUDINT, new NeInst<Pt::uint32_t>()) );
    _instTable.insert( Item(NeULINT, new NeInst<Pt::uint64_t>()) );
    _instTable.insert( Item(NeREAL, new NeInst<float>()) );
    _instTable.insert( Item(NeLREAL, new NeInst<double>()) );

    //Move
    _instTable.insert( Item(Move, new MoveInst()) );

    //Stack handling
    _instTable.insert( Item(PushTempOp, new PushTempInst()) );
    _instTable.insert( Item(PopTempOp, new PopTempInst()) );
    _instTable.insert( Item(PushRef, new PushRefInst()) );
    _instTable.insert( Item(PopRef, new PopRefInst()) );

    _instTable.insert( Item(AddOffsetSINT, new AddOffsetInst<Pt::int8_t>()) );
    _instTable.insert( Item(AddOffsetINT, new AddOffsetInst<Pt::int16_t>()) );
    _instTable.insert( Item(AddOffsetDINT, new AddOffsetInst<Pt::int32_t>()) );
    _instTable.insert( Item(AddOffsetLINT, new AddOffsetInst<Pt::int64_t>()) );
    _instTable.insert( Item(AddOffsetUSINT, new AddOffsetInst<Pt::uint8_t>()) );
    _instTable.insert( Item(AddOffsetUINT, new AddOffsetInst<Pt::uint16_t>()) );
    _instTable.insert( Item(AddOffsetUDINT, new AddOffsetInst<Pt::uint32_t>()) );
    _instTable.insert( Item(AddOffsetULINT, new AddOffsetInst<Pt::uint64_t>()) );
            
    //Convert
    _instTable.insert( Item(BOOL2SINT, new ConvertInst<Pt::uint8_t,Pt::int8_t>()) );
    _instTable.insert( Item(BOOL2INT, new ConvertInst<Pt::uint8_t,Pt::int16_t>()) );
    _instTable.insert( Item(BOOL2DINT, new ConvertInst<Pt::uint8_t,Pt::int32_t>()) );
    _instTable.insert( Item(BOOL2LINT, new ConvertInst<Pt::uint8_t,Pt::int64_t>()) );
    _instTable.insert( Item(BOOL2USINT, new ConvertInst<Pt::uint8_t,Pt::uint8_t>()) );
    _instTable.insert( Item(BOOL2UINT, new ConvertInst<Pt::uint8_t,Pt::uint16_t>()) );
    _instTable.insert( Item(BOOL2UDINT, new ConvertInst<Pt::uint8_t,Pt::uint32_t>()) );
    _instTable.insert( Item(BOOL2ULINT, new ConvertInst<Pt::uint8_t,Pt::uint64_t>()) );
    _instTable.insert( Item(BOOL2REAL, new ConvertInst<Pt::uint8_t,float>()) );
    _instTable.insert( Item(BOOL2LREAL, new ConvertInst<Pt::uint8_t,double>()) );
    _instTable.insert( Item(BOOL2BYTE, new ConvertInst<Pt::uint8_t,Pt::uint8_t>()) );
    _instTable.insert( Item(BOOL2WORD, new ConvertInst<Pt::uint8_t,Pt::uint16_t>()) );
    _instTable.insert(Item(BOOL2DWORD, new ConvertInst<Pt::uint8_t, Pt::uint32_t>()));
    _instTable.insert( Item(BOOL2LWORD, new ConvertInst<Pt::uint8_t,Pt::uint64_t>()) );

    _instTable.insert( Item(SINT2BOOL, new ConvertInst<Pt::int8_t,Pt::uint8_t>()) );
    _instTable.insert( Item(SINT2INT, new ConvertInst<Pt::int8_t,Pt::int16_t>()) );
    _instTable.insert( Item(SINT2DINT, new ConvertInst<Pt::int8_t,Pt::int32_t>()) );
    _instTable.insert( Item(SINT2LINT, new ConvertInst<Pt::int8_t,Pt::int64_t>()) );
    _instTable.insert( Item(SINT2USINT, new ConvertInst<Pt::int8_t,Pt::uint8_t>()) );
    _instTable.insert( Item(SINT2UINT, new ConvertInst<Pt::int8_t,Pt::uint16_t>()) );
    _instTable.insert( Item(SINT2UDINT, new ConvertInst<Pt::int8_t,Pt::uint32_t>()) );
    _instTable.insert( Item(SINT2ULINT, new ConvertInst<Pt::int8_t,Pt::uint64_t>()) );
    _instTable.insert( Item(SINT2REAL, new ConvertInst<Pt::int8_t,float>()) );
    _instTable.insert( Item(SINT2LREAL, new ConvertInst<Pt::int8_t,double>()) );
    _instTable.insert( Item(SINT2BYTE, new ConvertInst<Pt::int8_t,Pt::uint8_t>()) );
    _instTable.insert( Item(SINT2WORD, new ConvertInst<Pt::int8_t,Pt::uint16_t>()) );
    _instTable.insert( Item(SINT2DWORD, new ConvertInst<Pt::int8_t,Pt::uint32_t>()) );
    _instTable.insert( Item(SINT2LWORD, new ConvertInst<Pt::int8_t,Pt::uint64_t>()) );


    _instTable.insert( Item(INT2BOOL, new ConvertInst<Pt::int16_t,Pt::uint8_t>()) );
    _instTable.insert( Item(INT2SINT, new ConvertInst<Pt::int16_t,Pt::int8_t>()) );
    _instTable.insert( Item(INT2DINT, new ConvertInst<Pt::int16_t,Pt::int32_t>()) );
    _instTable.insert( Item(INT2LINT, new ConvertInst<Pt::int16_t,Pt::int64_t>()) );
    _instTable.insert( Item(INT2USINT, new ConvertInst<Pt::int16_t,Pt::uint8_t>()) );
    _instTable.insert( Item(INT2UINT, new ConvertInst<Pt::int16_t,Pt::uint16_t>()) );
    _instTable.insert( Item(INT2UDINT, new ConvertInst<Pt::int16_t,Pt::uint32_t>()) );
    _instTable.insert( Item(INT2ULINT, new ConvertInst<Pt::int16_t,Pt::uint64_t>()) );
    _instTable.insert( Item(INT2REAL, new ConvertInst<Pt::int16_t,float>()) );
    _instTable.insert( Item(INT2LREAL, new ConvertInst<Pt::int16_t,double>()) );
    _instTable.insert( Item(INT2BYTE, new ConvertInst<Pt::int16_t,Pt::uint8_t>()) );
    _instTable.insert( Item(INT2WORD, new ConvertInst<Pt::int16_t,Pt::uint16_t>()) );
    _instTable.insert( Item(INT2DWORD, new ConvertInst<Pt::int16_t,Pt::uint32_t>()) );
    _instTable.insert( Item(INT2LWORD, new ConvertInst<Pt::int16_t,Pt::uint64_t>()) );
            

    _instTable.insert( Item(DINT2BOOL, new ConvertInst<Pt::int32_t,Pt::uint8_t>()) );
    _instTable.insert( Item(DINT2SINT, new ConvertInst<Pt::int32_t,Pt::int8_t>()) );
    _instTable.insert( Item(DINT2INT, new ConvertInst<Pt::int32_t,Pt::int16_t>()) );
    _instTable.insert( Item(DINT2LINT, new ConvertInst<Pt::int32_t,Pt::int64_t>()) );
    _instTable.insert( Item(DINT2USINT, new ConvertInst<Pt::int32_t,Pt::uint8_t>()) );
    _instTable.insert( Item(DINT2UINT, new ConvertInst<Pt::int32_t,Pt::uint16_t>()) );
    _instTable.insert( Item(DINT2UDINT, new ConvertInst<Pt::int32_t,Pt::uint32_t>()) );
    _instTable.insert( Item(DINT2ULINT, new ConvertInst<Pt::int32_t,Pt::uint64_t>()) );
    _instTable.insert( Item(DINT2REAL, new ConvertInst<Pt::int32_t,float>()) );
    _instTable.insert( Item(DINT2LREAL, new ConvertInst<Pt::int32_t,double>()) );
    _instTable.insert( Item(DINT2BYTE, new ConvertInst<Pt::int32_t,Pt::uint8_t>()) );
    _instTable.insert( Item(DINT2WORD, new ConvertInst<Pt::int32_t,Pt::uint16_t>()) );
    _instTable.insert( Item(DINT2DWORD, new ConvertInst<Pt::int32_t,Pt::uint32_t>()) );
    _instTable.insert( Item(DINT2LWORD, new ConvertInst<Pt::int32_t,Pt::uint64_t>()) );


    _instTable.insert( Item(LINT2BOOL, new ConvertInst<Pt::int64_t,Pt::uint8_t>()) );
    _instTable.insert( Item(LINT2SINT, new ConvertInst<Pt::int64_t,Pt::int8_t>()) );
    _instTable.insert( Item(LINT2INT, new ConvertInst<Pt::int64_t,Pt::int16_t>()) );
    _instTable.insert( Item(LINT2DINT, new ConvertInst<Pt::int64_t,Pt::int32_t>()) );
    _instTable.insert( Item(LINT2USINT, new ConvertInst<Pt::int64_t,Pt::uint8_t>()) );
    _instTable.insert( Item(LINT2UINT, new ConvertInst<Pt::int64_t,Pt::uint16_t>()) );
    _instTable.insert( Item(LINT2UDINT, new ConvertInst<Pt::int64_t,Pt::uint32_t>()) );
    _instTable.insert( Item(LINT2ULINT, new ConvertInst<Pt::int64_t,Pt::uint64_t>()) );
    _instTable.insert( Item(LINT2REAL, new ConvertInst<Pt::int64_t,float>()) );
    _instTable.insert( Item(LINT2LREAL, new ConvertInst<Pt::int64_t,double>()) );
    _instTable.insert( Item(LINT2BYTE, new ConvertInst<Pt::int64_t,Pt::uint8_t>()) );
    _instTable.insert( Item(LINT2WORD, new ConvertInst<Pt::int64_t,Pt::uint16_t>()) );
    _instTable.insert( Item(LINT2DWORD, new ConvertInst<Pt::int64_t,Pt::uint32_t>()) );
    _instTable.insert( Item(LINT2LWORD, new ConvertInst<Pt::int64_t,Pt::uint64_t>()) );


    _instTable.insert( Item(USINT2BOOL, new ConvertInst<Pt::uint8_t,Pt::uint8_t>()) );
    _instTable.insert( Item(USINT2SINT, new ConvertInst<Pt::uint8_t,Pt::int8_t>()) );
    _instTable.insert( Item(USINT2INT, new ConvertInst<Pt::uint8_t,Pt::int16_t>()) );
    _instTable.insert( Item(USINT2DINT, new ConvertInst<Pt::uint8_t,Pt::int32_t>()) );
    _instTable.insert( Item(USINT2LINT, new ConvertInst<Pt::uint8_t,Pt::int64_t>()) );
    _instTable.insert( Item(USINT2UINT, new ConvertInst<Pt::uint8_t,Pt::uint16_t>()) );
    _instTable.insert( Item(USINT2UDINT, new ConvertInst<Pt::uint8_t,Pt::uint32_t>()) );
    _instTable.insert( Item(USINT2ULINT, new ConvertInst<Pt::uint8_t,Pt::uint64_t>()) );
    _instTable.insert( Item(USINT2REAL, new ConvertInst<Pt::uint8_t,float>()) );
    _instTable.insert( Item(USINT2LREAL, new ConvertInst<Pt::uint8_t,double>()) );
    _instTable.insert( Item(USINT2BYTE, new ConvertInst<Pt::uint8_t,Pt::uint8_t>()) );
    _instTable.insert( Item(USINT2WORD, new ConvertInst<Pt::uint8_t,Pt::uint16_t>()) );
    _instTable.insert( Item(USINT2DWORD, new ConvertInst<Pt::uint8_t,Pt::uint32_t>() ));
    _instTable.insert( Item(USINT2LWORD, new ConvertInst<Pt::uint8_t,Pt::uint64_t>()) );

    _instTable.insert( Item(UINT2BOOL, new ConvertInst<Pt::uint16_t,Pt::uint8_t>()) );
    _instTable.insert( Item(UINT2SINT, new ConvertInst<Pt::uint16_t,Pt::int8_t>()) );
    _instTable.insert( Item(UINT2INT, new ConvertInst<Pt::uint16_t,Pt::int16_t>()) );
    _instTable.insert( Item(UINT2DINT, new ConvertInst<Pt::uint16_t,Pt::int32_t>()) );
    _instTable.insert( Item(UINT2LINT, new ConvertInst<Pt::uint16_t,Pt::int64_t>()) );
    _instTable.insert( Item(UINT2USINT, new ConvertInst<Pt::uint16_t,Pt::uint8_t>()) );
    _instTable.insert( Item(UINT2UDINT, new ConvertInst<Pt::uint16_t,Pt::uint32_t>()) );
    _instTable.insert( Item(UINT2ULINT, new ConvertInst<Pt::uint16_t,Pt::uint64_t>()) );
    _instTable.insert( Item(UINT2REAL, new ConvertInst<Pt::uint16_t,float>() ));
    _instTable.insert( Item(UINT2LREAL, new ConvertInst<Pt::uint16_t,double>()) );
    _instTable.insert( Item(UINT2BYTE, new ConvertInst<Pt::uint16_t,Pt::uint8_t>()) );
    _instTable.insert( Item(UINT2WORD, new ConvertInst<Pt::uint16_t,Pt::uint16_t>()) );
    _instTable.insert( Item(UINT2DWORD, new ConvertInst<Pt::uint16_t,Pt::uint32_t>()) );
    _instTable.insert( Item(UINT2LWORD, new ConvertInst<Pt::uint16_t,Pt::uint64_t>()) );

    _instTable.insert( Item(UDINT2BOOL, new ConvertInst<Pt::uint32_t,Pt::uint8_t>()) );
    _instTable.insert( Item(UDINT2SINT, new ConvertInst<Pt::uint32_t,Pt::int8_t>()) );
    _instTable.insert( Item(UDINT2INT, new ConvertInst<Pt::uint32_t,Pt::int16_t>()) );
    _instTable.insert( Item(UDINT2DINT, new ConvertInst<Pt::uint32_t,Pt::int32_t>()) );
    _instTable.insert( Item(UDINT2LINT, new ConvertInst<Pt::uint32_t,Pt::int64_t>()) );
    _instTable.insert( Item(UDINT2USINT, new ConvertInst<Pt::uint32_t,Pt::uint8_t>()) );
    _instTable.insert( Item(UDINT2ULINT, new ConvertInst<Pt::uint32_t,Pt::uint64_t>()) );
    _instTable.insert( Item(UDINT2UINT, new ConvertInst<Pt::uint32_t,Pt::uint16_t>()) );
    _instTable.insert( Item(UDINT2REAL, new ConvertInst<Pt::uint32_t,float>()) );
    _instTable.insert( Item(UDINT2LREAL, new ConvertInst<Pt::uint32_t,double>()) );
    _instTable.insert( Item(UDINT2BYTE, new ConvertInst<Pt::uint32_t,Pt::uint8_t>()) );
    _instTable.insert( Item(UDINT2WORD, new ConvertInst<Pt::uint32_t,Pt::uint16_t>()) );
    _instTable.insert( Item(UDINT2DWORD, new ConvertInst<Pt::uint32_t,Pt::uint32_t>()) );
    _instTable.insert( Item(UDINT2LWORD, new ConvertInst<Pt::uint32_t,Pt::uint64_t>()) );

    _instTable.insert( Item(ULINT2BOOL, new ConvertInst<Pt::uint64_t,Pt::uint8_t>()) );
    _instTable.insert( Item(ULINT2SINT, new ConvertInst<Pt::uint64_t,Pt::int8_t>()) );
    _instTable.insert( Item(ULINT2INT, new ConvertInst<Pt::uint64_t,Pt::int16_t>()) );
    _instTable.insert( Item(ULINT2DINT, new ConvertInst<Pt::uint64_t,Pt::int32_t>()) );
    _instTable.insert( Item(ULINT2LINT, new ConvertInst<Pt::uint64_t,Pt::int64_t>()) );
    _instTable.insert( Item(ULINT2USINT, new ConvertInst<Pt::uint64_t,Pt::uint8_t>()) );
    _instTable.insert( Item(ULINT2UDINT, new ConvertInst<Pt::uint64_t,Pt::uint32_t>()) );
    _instTable.insert( Item(ULINT2UINT, new ConvertInst<Pt::uint64_t,Pt::uint16_t>()) );
    _instTable.insert( Item(ULINT2REAL, new ConvertInst<Pt::uint64_t,float>()) );
    _instTable.insert( Item(ULINT2LREAL, new ConvertInst<Pt::uint64_t,double>()) );
    _instTable.insert( Item(ULINT2BYTE, new ConvertInst<Pt::uint64_t,Pt::uint8_t>()) );
    _instTable.insert( Item(ULINT2WORD, new ConvertInst<Pt::uint64_t,Pt::uint16_t>()) );
    _instTable.insert( Item(ULINT2DWORD, new ConvertInst<Pt::uint64_t,Pt::uint32_t>()) );
    _instTable.insert( Item(ULINT2LWORD, new ConvertInst<Pt::uint64_t,Pt::uint64_t>()) );

    _instTable.insert( Item(REAL2BOOL, new ConvertInst<float,Pt::uint8_t>()) );
    _instTable.insert( Item(REAL2SINT, new ConvertInst<float,Pt::int8_t>()) );
    _instTable.insert( Item(REAL2INT, new ConvertInst<float,Pt::int16_t>()) );
    _instTable.insert( Item(REAL2DINT, new ConvertInst<float,Pt::int32_t>()) );
    _instTable.insert( Item(REAL2LINT, new ConvertInst<float,Pt::int64_t>()) );
    _instTable.insert( Item(REAL2USINT, new ConvertInst<float,Pt::uint8_t>()) );
    _instTable.insert( Item(REAL2UDINT, new ConvertInst<float,Pt::uint32_t>()) );
    _instTable.insert( Item(REAL2UINT, new ConvertInst<float,Pt::uint16_t>()) );
    _instTable.insert( Item(REAL2ULINT, new ConvertInst<float,Pt::uint64_t>()) );
    _instTable.insert( Item(REAL2LREAL, new ConvertInst<float,double>()) );
    _instTable.insert( Item(REAL2BYTE, new ConvertInst<float,Pt::uint8_t>()) );
    _instTable.insert( Item(REAL2WORD, new ConvertInst<float,Pt::uint16_t>()) );
    _instTable.insert( Item(REAL2DWORD, new ConvertInst<float,Pt::uint32_t>()) );
    _instTable.insert( Item(REAL2LWORD, new ConvertInst<float,Pt::uint64_t>()) );

    _instTable.insert( Item(LREAL2BOOL, new ConvertInst<double,Pt::uint8_t>()) );
    _instTable.insert( Item(LREAL2SINT, new ConvertInst<double,Pt::int8_t>()) );
    _instTable.insert( Item(LREAL2INT, new ConvertInst<double,Pt::int16_t>()) );
    _instTable.insert( Item(LREAL2DINT, new ConvertInst<double,Pt::int32_t>()) );
    _instTable.insert( Item(LREAL2LINT, new ConvertInst<double,Pt::int64_t>()) );
    _instTable.insert( Item(LREAL2USINT, new ConvertInst<double,Pt::uint8_t>()) );
    _instTable.insert( Item(LREAL2UDINT, new ConvertInst<double,Pt::uint32_t>()) );
    _instTable.insert( Item(LREAL2UINT, new ConvertInst<double,Pt::uint16_t>()) );
    _instTable.insert( Item(LREAL2ULINT, new ConvertInst<double,Pt::uint64_t>()) );
    _instTable.insert( Item(LREAL2REAL, new ConvertInst<double,float>()) );
    _instTable.insert( Item(LREAL2BYTE, new ConvertInst<double,Pt::uint8_t>()) );
    _instTable.insert( Item(LREAL2WORD, new ConvertInst<double,Pt::uint16_t>()) );
    _instTable.insert( Item(LREAL2DWORD, new ConvertInst<double,Pt::uint32_t>()) );
    _instTable.insert( Item(LREAL2LWORD, new ConvertInst<double,Pt::uint64_t>()) );

    _instTable.insert( Item(BYTE2BOOL, new ConvertInst<Pt::uint8_t,Pt::uint8_t>()) );
    _instTable.insert( Item(BYTE2SINT, new ConvertInst<Pt::uint8_t,Pt::int8_t>()) );
    _instTable.insert( Item(BYTE2INT, new ConvertInst<Pt::uint8_t,Pt::int16_t>()) );
    _instTable.insert( Item(BYTE2DINT, new ConvertInst<Pt::uint8_t,Pt::int32_t>()) );
    _instTable.insert( Item(BYTE2LINT, new ConvertInst<Pt::uint8_t,Pt::int64_t>()) );
    _instTable.insert( Item(BYTE2USINT, new ConvertInst<Pt::uint8_t,Pt::uint8_t>()) );
    _instTable.insert( Item(BYTE2UDINT, new ConvertInst<Pt::uint8_t,Pt::uint32_t>()) );
    _instTable.insert( Item(BYTE2UINT, new ConvertInst<Pt::uint8_t,Pt::uint16_t>()) );
    _instTable.insert( Item(BYTE2ULINT, new ConvertInst<Pt::uint8_t,Pt::uint64_t>()) );
    _instTable.insert( Item(BYTE2REAL, new ConvertInst<Pt::uint8_t,float>()) );
    _instTable.insert( Item(BYTE2LREAL, new ConvertInst<Pt::uint8_t,double>()) );
    _instTable.insert( Item(BYTE2WORD, new ConvertInst<Pt::uint8_t,Pt::uint16_t>()) );
    _instTable.insert( Item(BYTE2DWORD, new ConvertInst<Pt::uint8_t,Pt::uint32_t>()) );
    _instTable.insert( Item(BYTE2LWORD, new ConvertInst<Pt::uint8_t,Pt::uint64_t>()) );

    _instTable.insert( Item(WORD2BOOL, new ConvertInst<Pt::uint16_t,Pt::uint8_t>()) );
    _instTable.insert( Item(WORD2SINT, new ConvertInst<Pt::uint16_t,Pt::int8_t>()) );
    _instTable.insert( Item(WORD2INT, new ConvertInst<Pt::uint16_t,Pt::int16_t>()) );
    _instTable.insert( Item(WORD2DINT, new ConvertInst<Pt::uint16_t,Pt::int32_t>()) );
    _instTable.insert( Item(WORD2LINT, new ConvertInst<Pt::uint16_t,Pt::int64_t>()) );
    _instTable.insert( Item(WORD2USINT, new ConvertInst<Pt::uint16_t,Pt::uint8_t>()) );
    _instTable.insert( Item(WORD2UDINT, new ConvertInst<Pt::uint16_t,Pt::uint32_t>()) );
    _instTable.insert( Item(WORD2UINT, new ConvertInst<Pt::uint16_t,Pt::uint16_t>()) );
    _instTable.insert( Item(WORD2ULINT, new ConvertInst<Pt::uint16_t,Pt::uint64_t>()) );
    _instTable.insert( Item(WORD2REAL, new ConvertInst<Pt::uint16_t,float>()) );
    _instTable.insert( Item(WORD2LREAL, new ConvertInst<Pt::uint16_t,double>()) );
    _instTable.insert( Item(WORD2BYTE, new ConvertInst<Pt::uint16_t,Pt::uint8_t>()) );
    _instTable.insert( Item(WORD2DWORD, new ConvertInst<Pt::uint16_t,Pt::uint32_t>()) );
    _instTable.insert( Item(WORD2LWORD, new ConvertInst<Pt::uint16_t,Pt::uint64_t>()) );


    _instTable.insert( Item(DWORD2BOOL, new ConvertInst<Pt::uint32_t,Pt::uint8_t>()) );
    _instTable.insert( Item(DWORD2SINT, new ConvertInst<Pt::uint32_t,Pt::int8_t>()) );
    _instTable.insert( Item(DWORD2INT, new ConvertInst<Pt::uint32_t,Pt::int16_t>()) );
    _instTable.insert( Item(DWORD2DINT, new ConvertInst<Pt::uint32_t,Pt::int32_t>()) );
    _instTable.insert( Item(DWORD2LINT, new ConvertInst<Pt::uint32_t,Pt::int64_t>()) );
    _instTable.insert( Item(DWORD2USINT, new ConvertInst<Pt::uint32_t,Pt::uint8_t>()) );
    _instTable.insert( Item(DWORD2UDINT, new ConvertInst<Pt::uint32_t,Pt::uint32_t>()) );
    _instTable.insert( Item(DWORD2UINT, new ConvertInst<Pt::uint32_t,Pt::uint16_t>()) );
    _instTable.insert( Item(DWORD2ULINT, new ConvertInst<Pt::uint32_t,Pt::uint64_t>()) );
    _instTable.insert( Item(DWORD2REAL, new ConvertInst<Pt::uint32_t,float>()) );
    _instTable.insert( Item(DWORD2LREAL, new ConvertInst<Pt::uint32_t,double>()) );
    _instTable.insert( Item(DWORD2BYTE, new ConvertInst<Pt::uint32_t,Pt::uint8_t>()) );
    _instTable.insert( Item(DWORD2WORD, new ConvertInst<Pt::uint32_t,Pt::uint16_t>()) );
    _instTable.insert( Item(DWORD2LWORD, new ConvertInst<Pt::uint32_t,Pt::uint64_t>()) );


    _instTable.insert( Item(LWORD2BOOL, new ConvertInst<Pt::uint64_t,Pt::uint8_t>()) );
    _instTable.insert( Item(LWORD2SINT, new ConvertInst<Pt::uint64_t,Pt::int8_t>()) );
    _instTable.insert( Item(LWORD2INT, new ConvertInst<Pt::uint64_t,Pt::int16_t>()) );
    _instTable.insert( Item(LWORD2DINT, new ConvertInst<Pt::uint64_t,Pt::int32_t>()) );
    _instTable.insert( Item(LWORD2LINT, new ConvertInst<Pt::uint64_t,Pt::int64_t>()) );
    _instTable.insert( Item(LWORD2USINT, new ConvertInst<Pt::uint64_t,Pt::uint8_t>()) );
    _instTable.insert( Item(LWORD2UDINT, new ConvertInst<Pt::uint64_t,Pt::uint32_t>()) );
    _instTable.insert( Item(LWORD2UINT, new ConvertInst<Pt::uint64_t,Pt::uint16_t>()) );
    _instTable.insert( Item(LWORD2ULINT, new ConvertInst<Pt::uint64_t,Pt::uint64_t>()) );
    _instTable.insert( Item(LWORD2REAL, new ConvertInst<Pt::uint64_t,float>()) );
    _instTable.insert( Item(LWORD2LREAL, new ConvertInst<Pt::uint64_t,double>()) );
    _instTable.insert( Item(LWORD2BYTE, new ConvertInst<Pt::uint64_t,Pt::uint8_t>()) );
    _instTable.insert( Item(LWORD2WORD, new ConvertInst<Pt::uint64_t,Pt::uint16_t>()) );
    _instTable.insert( Item(LWORD2DWORD, new ConvertInst<Pt::uint64_t,Pt::uint32_t>()) );

    //Modulo
    _instTable.insert( Item(ModSINT, new ModInst<Pt::int8_t>()) );
    _instTable.insert( Item(ModINT, new ModInst<Pt::int16_t>()) );
    _instTable.insert( Item(ModDINT, new ModInst<Pt::int32_t>()) );
    _instTable.insert( Item(ModLINT, new ModInst<Pt::int64_t>()) );
    _instTable.insert( Item(ModUSINT, new ModInst<Pt::uint8_t>()) );
    _instTable.insert( Item(ModUINT, new ModInst<Pt::uint16_t>()) );
    _instTable.insert( Item(ModUDINT, new ModInst<Pt::uint32_t>()) );
    _instTable.insert( Item(ModULINT, new ModInst<Pt::uint64_t>()) );
    
    //Or 
    _instTable.insert( Item(OrBOOL, new OrInst<Pt::uint8_t>()) );
    _instTable.insert( Item(OrBYTE, new OrInst<Pt::uint8_t>()) );
    _instTable.insert( Item(OrWORD, new OrInst<Pt::uint16_t>()) );
    _instTable.insert( Item(OrDWORD, new OrInst<Pt::uint32_t>()) );
    _instTable.insert( Item(OrLWORD, new OrInst<Pt::uint64_t>()) );
    
    //And
    _instTable.insert( Item(AndBOOL, new AndInst<Pt::uint8_t>()) );
    _instTable.insert( Item(AndBYTE, new AndInst<Pt::uint8_t>()) );
    _instTable.insert( Item(AndWORD, new AndInst<Pt::uint16_t>()) );
    _instTable.insert( Item(AndDWORD, new AndInst<Pt::uint32_t>()) );
    _instTable.insert( Item(AndLWORD, new AndInst<Pt::uint64_t>()) );

    //Xor
    _instTable.insert( Item(XorBOOL, new XorInst<Pt::uint8_t>()) );
    _instTable.insert( Item(XorBYTE, new XorInst<Pt::uint8_t>()) );
    _instTable.insert( Item(XorWORD, new XorInst<Pt::uint16_t>()) );
    _instTable.insert( Item(XorDWORD, new XorInst<Pt::uint32_t>()) );
    _instTable.insert( Item(XorLWORD, new XorInst<Pt::uint64_t>()) );

    //Pow
    _instTable.insert( Item(PowSINT, new PowInst<Pt::int8_t>()) );
    _instTable.insert( Item(PowINT, new PowInst<Pt::int16_t>()) );
    _instTable.insert( Item(PowDINT, new PowInst<Pt::int32_t>()) );
    _instTable.insert( Item(PowLINT, new PowInst<Pt::int64_t>()) );
    _instTable.insert( Item(PowUSINT, new PowInst<Pt::uint8_t>()) );
    _instTable.insert( Item(PowUINT, new PowInst<Pt::uint16_t>()) );
    _instTable.insert( Item(PowUDINT, new PowInst<Pt::uint32_t>()) );
    _instTable.insert( Item(PowULINT, new PowInst<Pt::uint64_t>()) );
    _instTable.insert( Item(PowREAL, new PowInst<float>()) );
    _instTable.insert( Item(PowLREAL, new PowInst<double>() ));

    //Not inst.
    _instTable.insert( Item(NotBOOL, new NotInst()));

    //Neg
    _instTable.insert( Item(NegSINT, new NegInst<Pt::int8_t,Pt::int8_t>()) );
    _instTable.insert( Item(NegINT, new NegInst<Pt::int16_t,Pt::int16_t>()) );
    _instTable.insert( Item(NegDINT, new NegInst<Pt::int32_t,Pt::int32_t>()) );
    _instTable.insert( Item(NegLINT, new NegInst<Pt::int64_t,Pt::int64_t>()) );
    _instTable.insert( Item(NegUSINT, new NegInst<Pt::uint8_t,Pt::int8_t>()) );
    _instTable.insert( Item(NegUINT, new NegInst<Pt::uint16_t,Pt::int16_t>()) );
    _instTable.insert( Item(NegUDINT, new NegInst<Pt::uint32_t,Pt::int32_t>()) );
    _instTable.insert( Item(NegULINT, new NegInst<Pt::uint64_t,Pt::int64_t>()) );
    _instTable.insert( Item(NegREAL, new NegInst<float,float>()) );
    _instTable.insert( Item(NegLREAL, new NegInst<double,double>() ));

    //Jump
    _instTable.insert( Item(JmpTRUE, new JmpTRUEInst()));
    _instTable.insert( Item(JmpFALSE, new JmpFALSEInst()));
    _instTable.insert( Item(Jmp, new JmpInst()));
    _instTable.insert( Item(Return, new ReturnInst()));

    //Inc
    _instTable.insert( Item(IncSINT, new IncInst<Pt::int8_t>()) );
    _instTable.insert( Item(IncINT, new IncInst<Pt::int16_t>()) );
    _instTable.insert( Item(IncDINT, new IncInst<Pt::int32_t>()) );
    _instTable.insert( Item(IncLINT, new IncInst<Pt::int64_t>()) );
    _instTable.insert( Item(IncUSINT, new IncInst<Pt::uint8_t>()) );
    _instTable.insert( Item(IncUINT, new IncInst<Pt::uint16_t>()) );
    _instTable.insert( Item(IncUDINT, new IncInst<Pt::uint32_t>()) );
    _instTable.insert( Item(IncULINT, new IncInst<Pt::uint64_t>() ));
    _instTable.insert( Item(IncREAL, new IncInst<float>()) );
    _instTable.insert( Item(IncLREAL, new IncInst<double>()) );

    //Dec
    _instTable.insert( Item(DecSINT, new DecInst<Pt::int8_t>()) );
    _instTable.insert( Item(DecINT, new DecInst<Pt::int16_t>()) );
    _instTable.insert( Item(DecDINT, new DecInst<Pt::int32_t>()) );
    _instTable.insert( Item(DecLINT, new DecInst<Pt::int64_t>()) );
    _instTable.insert( Item(DecUSINT, new DecInst<Pt::uint8_t>()) );
    _instTable.insert( Item(DecUINT, new DecInst<Pt::uint16_t>()) );
    _instTable.insert( Item(DecUDINT, new DecInst<Pt::uint32_t>()) );
    _instTable.insert( Item(DecULINT, new DecInst<Pt::uint64_t>() ));
    _instTable.insert( Item(DecREAL, new DecInst<float>()) );
    _instTable.insert( Item(DecLREAL, new DecInst<double>()) );

    //Floating point operations
    _instTable.insert( Item(Round, new RoundInst()) );
    _instTable.insert( Item(Trunc, new TruncInst()) );

    //Abs
    _instTable.insert( Item(AbsSINT, new AbsInst<Pt::int8_t>()) );
    _instTable.insert( Item(AbsINT, new AbsInst<Pt::int16_t>()) );
    _instTable.insert( Item(AbsDINT, new AbsInst<Pt::int32_t>()) );
    _instTable.insert( Item(AbsLINT, new AbsInst<Pt::int64_t>()) );
    _instTable.insert( Item(AbsREAL, new AbsInst<float>()) );
    _instTable.insert( Item(AbsLREAL, new AbsInst<double>()) );

    //Sqr
    _instTable.insert( Item(SqrSINT, new SqrInst<Pt::int8_t>()) );
    _instTable.insert( Item(SqrINT, new SqrInst<Pt::int16_t>()) );
    _instTable.insert( Item(SqrDINT, new SqrInst<Pt::int32_t>()) );
    _instTable.insert( Item(SqrLINT, new SqrInst<Pt::int64_t>()) );
    _instTable.insert( Item(SqrUSINT, new SqrInst<Pt::uint8_t>()) );
    _instTable.insert( Item(SqrUINT, new SqrInst<Pt::uint16_t>()) );
    _instTable.insert( Item(SqrUDINT, new SqrInst<Pt::uint32_t>()) );
    _instTable.insert( Item(SqrULINT, new SqrInst<Pt::uint64_t>() ));
    _instTable.insert( Item(SqrREAL, new SqrInst<float>()) );
    _instTable.insert( Item(SqrLREAL, new SqrInst<double>()) );

    //Sqrt
    _instTable.insert( Item(SqrtSINT, new SqrtInst<Pt::int8_t>()) );
    _instTable.insert( Item(SqrtINT, new SqrtInst<Pt::int16_t>()) );
    _instTable.insert( Item(SqrtDINT, new SqrtInst<Pt::int32_t>()) );
    _instTable.insert( Item(SqrtLINT, new SqrtInst<Pt::int64_t>()) );
    _instTable.insert( Item(SqrtUSINT, new SqrtInst<Pt::uint8_t>()) );
    _instTable.insert( Item(SqrtUINT, new SqrtInst<Pt::uint16_t>()) );
    _instTable.insert( Item(SqrtUDINT, new SqrtInst<Pt::uint32_t>()) );
    _instTable.insert( Item(SqrtULINT, new SqrtInst<Pt::uint64_t>() ));
    _instTable.insert( Item(SqrtREAL, new SqrtInst<float>()) );
    _instTable.insert( Item(SqrtLREAL, new SqrtInst<double>()) );

    //Exp
    _instTable.insert( Item(ExpSINT, new ExpInst<Pt::int8_t>()) );
    _instTable.insert( Item(ExpINT, new ExpInst<Pt::int16_t>()) );
    _instTable.insert( Item(ExpDINT, new ExpInst<Pt::int32_t>()) );
    _instTable.insert( Item(ExpLINT, new ExpInst<Pt::int64_t>()) );
    _instTable.insert( Item(ExpUSINT, new ExpInst<Pt::uint8_t>()) );
    _instTable.insert( Item(ExpUINT, new ExpInst<Pt::uint16_t>()) );
    _instTable.insert( Item(ExpUDINT, new ExpInst<Pt::uint32_t>()) );
    _instTable.insert( Item(ExpULINT, new ExpInst<Pt::uint64_t>() ));
    _instTable.insert( Item(ExpREAL, new ExpInst<float>()) );
    _instTable.insert( Item(ExpLREAL, new ExpInst<double>()) );

    //Expd
    _instTable.insert( Item(ExpdSINT, new ExpdInst<Pt::int8_t>()) );
    _instTable.insert( Item(ExpdINT, new ExpdInst<Pt::int16_t>()) );
    _instTable.insert( Item(ExpdDINT, new ExpdInst<Pt::int32_t>()) );
    _instTable.insert( Item(ExpdLINT, new ExpdInst<Pt::int64_t>()) );
    _instTable.insert( Item(ExpdUSINT, new ExpdInst<Pt::uint8_t>()) );
    _instTable.insert( Item(ExpdUINT, new ExpdInst<Pt::uint16_t>()) );
    _instTable.insert( Item(ExpdUDINT, new ExpdInst<Pt::uint32_t>()) );
    _instTable.insert( Item(ExpdULINT, new ExpdInst<Pt::uint64_t>() ));
    _instTable.insert( Item(ExpdREAL, new ExpdInst<float>()) );
    _instTable.insert( Item(ExpdLREAL, new ExpdInst<double>()) );

    //Ln
    _instTable.insert( Item(LnSINT, new LnInst<Pt::int8_t>()) );
    _instTable.insert( Item(LnINT, new LnInst<Pt::int16_t>()) );
    _instTable.insert( Item(LnDINT, new LnInst<Pt::int32_t>()) );
    _instTable.insert( Item(LnLINT, new LnInst<Pt::int64_t>()) );
    _instTable.insert( Item(LnUSINT, new LnInst<Pt::uint8_t>()) );
    _instTable.insert( Item(LnUINT, new LnInst<Pt::uint16_t>()) );
    _instTable.insert( Item(LnUDINT, new LnInst<Pt::uint32_t>()) );
    _instTable.insert( Item(LnULINT, new LnInst<Pt::uint64_t>() ));
    _instTable.insert( Item(LnREAL, new LnInst<float>()) );
    _instTable.insert( Item(LnLREAL, new LnInst<double>()) );

    //Log
    _instTable.insert( Item(LogSINT, new LogInst<Pt::int8_t>()) );
    _instTable.insert( Item(LogINT, new LogInst<Pt::int16_t>()) );
    _instTable.insert( Item(LogDINT, new LogInst<Pt::int32_t>()) );
    _instTable.insert( Item(LogLINT, new LogInst<Pt::int64_t>()) );
    _instTable.insert( Item(LogUSINT, new LogInst<Pt::uint8_t>()) );
    _instTable.insert( Item(LogUINT, new LogInst<Pt::uint16_t>()) );
    _instTable.insert( Item(LogUDINT, new LogInst<Pt::uint32_t>()) );
    _instTable.insert( Item(LogULINT, new LogInst<Pt::uint64_t>() ));
    _instTable.insert( Item(LogREAL, new LogInst<float>()) );
    _instTable.insert( Item(LogLREAL, new LogInst<double>()) );

    //Acos
    _instTable.insert( Item(AcosSINT, new AcosInst<Pt::int8_t>()) );
    _instTable.insert( Item(AcosINT, new AcosInst<Pt::int16_t>()) );
    _instTable.insert( Item(AcosDINT, new AcosInst<Pt::int32_t>()) );
    _instTable.insert( Item(AcosLINT, new AcosInst<Pt::int64_t>()) );
    _instTable.insert( Item(AcosUSINT, new AcosInst<Pt::uint8_t>()) );
    _instTable.insert( Item(AcosUINT, new AcosInst<Pt::uint16_t>()) );
    _instTable.insert( Item(AcosUDINT, new AcosInst<Pt::uint32_t>()) );
    _instTable.insert( Item(AcosULINT, new AcosInst<Pt::uint64_t>() ));
    _instTable.insert( Item(AcosREAL, new AcosInst<float>()) );
    _instTable.insert( Item(AcosLREAL, new AcosInst<double>()) );

    //Asin
    _instTable.insert( Item(AsinSINT, new AsinInst<Pt::int8_t>()) );
    _instTable.insert( Item(AsinINT, new AsinInst<Pt::int16_t>()) );
    _instTable.insert( Item(AsinDINT, new AsinInst<Pt::int32_t>()) );
    _instTable.insert( Item(AsinLINT, new AsinInst<Pt::int64_t>()) );
    _instTable.insert( Item(AsinUSINT, new AsinInst<Pt::uint8_t>()) );
    _instTable.insert( Item(AsinUINT, new AsinInst<Pt::uint16_t>()) );
    _instTable.insert( Item(AsinUDINT, new AsinInst<Pt::uint32_t>()) );
    _instTable.insert( Item(AsinULINT, new AsinInst<Pt::uint64_t>() ));
    _instTable.insert( Item(AsinREAL, new AsinInst<float>()) );
    _instTable.insert( Item(AsinLREAL, new AsinInst<double>()) );

    //Atan
    _instTable.insert( Item(AtanSINT, new AtanInst<Pt::int8_t>()) );
    _instTable.insert( Item(AtanINT, new AtanInst<Pt::int16_t>()) );
    _instTable.insert( Item(AtanDINT, new AtanInst<Pt::int32_t>()) );
    _instTable.insert( Item(AtanLINT, new AtanInst<Pt::int64_t>()) );
    _instTable.insert( Item(AtanUSINT, new AtanInst<Pt::uint8_t>()) );
    _instTable.insert( Item(AtanUINT, new AtanInst<Pt::uint16_t>()) );
    _instTable.insert( Item(AtanUDINT, new AtanInst<Pt::uint32_t>()) );
    _instTable.insert( Item(AtanULINT, new AtanInst<Pt::uint64_t>() ));
    _instTable.insert( Item(AtanREAL, new AtanInst<float>()) );
    _instTable.insert( Item(AtanLREAL, new AtanInst<double>()) );

    //Cos
    _instTable.insert( Item(CosSINT, new CosInst<Pt::int8_t>()) );
    _instTable.insert( Item(CosINT, new CosInst<Pt::int16_t>()) );
    _instTable.insert( Item(CosDINT, new CosInst<Pt::int32_t>()) );
    _instTable.insert( Item(CosLINT, new CosInst<Pt::int64_t>()) );
    _instTable.insert( Item(CosUSINT, new CosInst<Pt::uint8_t>()) );
    _instTable.insert( Item(CosUINT, new CosInst<Pt::uint16_t>()) );
    _instTable.insert( Item(CosUDINT, new CosInst<Pt::uint32_t>()) );
    _instTable.insert( Item(CosULINT, new CosInst<Pt::uint64_t>() ));
    _instTable.insert( Item(CosREAL, new CosInst<float>()) );
    _instTable.insert( Item(CosLREAL, new CosInst<double>()) );

    //Sin
    _instTable.insert( Item(SinSINT, new SinInst<Pt::int8_t>()) );
    _instTable.insert( Item(SinINT, new SinInst<Pt::int16_t>()) );
    _instTable.insert( Item(SinDINT, new SinInst<Pt::int32_t>()) );
    _instTable.insert( Item(SinLINT, new SinInst<Pt::int64_t>()) );
    _instTable.insert( Item(SinUSINT, new SinInst<Pt::uint8_t>()) );
    _instTable.insert( Item(SinUINT, new SinInst<Pt::uint16_t>()) );
    _instTable.insert( Item(SinUDINT, new SinInst<Pt::uint32_t>()) );
    _instTable.insert( Item(SinULINT, new SinInst<Pt::uint64_t>() ));
    _instTable.insert( Item(SinREAL, new SinInst<float>()) );
    _instTable.insert( Item(SinLREAL, new SinInst<double>()) );

    //Tan
    _instTable.insert( Item(TanSINT, new TanInst<Pt::int8_t>()) );
    _instTable.insert( Item(TanINT, new TanInst<Pt::int16_t>()) );
    _instTable.insert( Item(TanDINT, new TanInst<Pt::int32_t>()) );
    _instTable.insert( Item(TanLINT, new TanInst<Pt::int64_t>()) );
    _instTable.insert( Item(TanUSINT, new TanInst<Pt::uint8_t>()) );
    _instTable.insert( Item(TanUINT, new TanInst<Pt::uint16_t>()) );
    _instTable.insert( Item(TanUDINT, new TanInst<Pt::uint32_t>()) );
    _instTable.insert( Item(TanULINT, new TanInst<Pt::uint64_t>() ));
    _instTable.insert( Item(TanREAL, new TanInst<float>()) );
    _instTable.insert( Item(TanLREAL, new TanInst<double>()) );

    //Rotate left
    _instTable.insert( Item(RolBYTE, new RolInst<Pt::uint8_t>()) );
    _instTable.insert( Item(RolWORD, new RolInst<Pt::uint16_t>()) );
    _instTable.insert( Item(RolDWORD, new RolInst<Pt::uint32_t>()) );
    _instTable.insert( Item(RolLWORD, new RolInst<Pt::uint64_t>()) );

    //Rotate right
    _instTable.insert( Item(RorBYTE, new RorInst<Pt::uint8_t>()) );
    _instTable.insert( Item(RorWORD, new RorInst<Pt::uint16_t>()) );
    _instTable.insert( Item(RorDWORD, new RorInst<Pt::uint32_t>()) );
    _instTable.insert( Item(RorLWORD, new RorInst<Pt::uint64_t>()) );

        //Shift left
    _instTable.insert( Item(ShlBYTE, new ShlInst<Pt::uint8_t>()) );
    _instTable.insert( Item(ShlWORD, new ShlInst<Pt::uint16_t>()) );
    _instTable.insert( Item(ShlDWORD, new ShlInst<Pt::uint32_t>()) );
    _instTable.insert( Item(ShlLWORD, new ShlInst<Pt::uint64_t>()) );

        //Shift right
    _instTable.insert( Item(ShrBYTE, new ShrInst<Pt::uint8_t>()) );
    _instTable.insert( Item(ShrWORD, new ShrInst<Pt::uint16_t>()) );
    _instTable.insert( Item(ShrDWORD, new ShrInst<Pt::uint32_t>()) );
    _instTable.insert( Item(ShrLWORD, new ShrInst<Pt::uint64_t>()) );

    //Sel
    _instTable.insert( Item(Sel, new SelInst()) );

    //Min
    _instTable.insert( Item(MinSINT, new MinInst<Pt::int8_t>()) );
    _instTable.insert( Item(MinINT, new MinInst<Pt::int16_t>()) );
    _instTable.insert( Item(MinDINT, new MinInst<Pt::int32_t>()) );
    _instTable.insert( Item(MinLINT, new MinInst<Pt::int64_t>()) );
    _instTable.insert( Item(MinUSINT, new MinInst<Pt::uint8_t>()) );
    _instTable.insert( Item(MinUINT, new MinInst<Pt::uint16_t>()) );
    _instTable.insert( Item(MinUDINT, new MinInst<Pt::uint32_t>()) );
    _instTable.insert( Item(MinULINT, new MinInst<Pt::uint64_t>() ));
    _instTable.insert( Item(MinREAL, new MinInst<float>()) );
    _instTable.insert( Item(MinLREAL, new MinInst<double>()) );

    //Max
    _instTable.insert( Item(MaxSINT, new MaxInst<Pt::int8_t>()) );
    _instTable.insert( Item(MaxINT, new MaxInst<Pt::int16_t>()) );
    _instTable.insert( Item(MaxDINT, new MaxInst<Pt::int32_t>()) );
    _instTable.insert( Item(MaxLINT, new MaxInst<Pt::int64_t>()) );
    _instTable.insert( Item(MaxUSINT, new MaxInst<Pt::uint8_t>()) );
    _instTable.insert( Item(MaxUINT, new MaxInst<Pt::uint16_t>()) );
    _instTable.insert( Item(MaxUDINT, new MaxInst<Pt::uint32_t>()) );
    _instTable.insert( Item(MaxULINT, new MaxInst<Pt::uint64_t>() ));
    _instTable.insert( Item(MaxREAL, new MaxInst<float>()) );
    _instTable.insert( Item(MaxLREAL, new MaxInst<double>()) );

    //Limit
    _instTable.insert( Item(LimitSINT, new LimitInst<Pt::int8_t>()) );
    _instTable.insert( Item(LimitINT, new LimitInst<Pt::int16_t>()) );
    _instTable.insert( Item(LimitDINT, new LimitInst<Pt::int32_t>()) );
    _instTable.insert( Item(LimitLINT, new LimitInst<Pt::int64_t>()) );
    _instTable.insert( Item(LimitUSINT, new LimitInst<Pt::uint8_t>()) );
    _instTable.insert( Item(LimitUINT, new LimitInst<Pt::uint16_t>()) );
    _instTable.insert( Item(LimitUDINT, new LimitInst<Pt::uint32_t>()) );
    _instTable.insert( Item(LimitULINT, new LimitInst<Pt::uint64_t>() ));
    _instTable.insert( Item(LimitREAL, new LimitInst<float>()) );
    _instTable.insert( Item(LimitLREAL, new LimitInst<double>()) );

    //Mux
    _instTable.insert( Item(MuxSINT, new MuxInst<Pt::int8_t>()) );
    _instTable.insert( Item(MuxINT, new MuxInst<Pt::int16_t>()) );
    _instTable.insert( Item(MuxDINT, new MuxInst<Pt::int32_t>()) );
    _instTable.insert( Item(MuxLINT, new MuxInst<Pt::int64_t>()) );
    _instTable.insert( Item(MuxUSINT, new MuxInst<Pt::uint8_t>()) );
    _instTable.insert( Item(MuxUINT, new MuxInst<Pt::uint16_t>()) );
    _instTable.insert( Item(MuxUDINT, new MuxInst<Pt::uint32_t>()) );
    _instTable.insert( Item(MuxULINT, new MuxInst<Pt::uint64_t>() ));
}

InstructionTable::~InstructionTable()
{
    std::map<InstructionCode, Instruction*>::iterator it = _instTable.begin();
            
    for(; it != _instTable.end(); ++it)
        delete it->second;
}

Instruction* InstructionTable::createInstruction(InstructionCode code) const
{
    std::map<InstructionCode, Instruction*>::const_iterator it = _instTable.find(code);
            
    if( it == _instTable.end())
        return 0;
            
    return it->second->clone();
}

}}

