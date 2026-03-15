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

namespace Mpal.Model
{
    public class Operand
    {
        public enum Type
        {
            NoneType = 0,
            Direct,
            Immediate,
            Reference,
            Temporary,
            TemporaryRef,
        }

        private uint     _offset;
        private ulong    _uid;
        private Type     _type;
        private object   _constant;
        private uint     _size;
        private string   _typeId;
        private bool     _isConstant;
        private bool     _cached;

        public Operand()
        {
        }

        public Operand(Operand op)
        {
            _offset = op._offset;
            _uid = op._uid;
            _type = op._type;
            _constant = op._constant;
            _size = op._size;
            _typeId = op._typeId;
            _isConstant = op.IsConstant;
            _cached = op._cached;
        }

        public Operand(uint offset, ulong uid, Type type, uint size, string typeId)
        {
            _offset = offset;
            _uid = uid;
            _type = type;
            _size = size;
            _typeId = typeId;
        }

        public Operand(object constant, ulong uid, uint size, string typeId)
        {
            _constant = constant;
            _type = Type.Immediate;
            _uid = uid;
            _offset = 0;
            _typeId = typeId;
            _size = size;
        }

        public bool IsConstant
        {
            get{ return _isConstant;}
            set { _isConstant = value; }
        }

        public bool Cached
        {
            get { return _cached; }
            set { _cached = value; }
        }
        public string TypeId
        {
            get { return _typeId; }
            set { _typeId = value; }
        }

        public uint Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public uint Offset
        {
            get { return _offset; }
            set { _offset = value; }
        }

        public ulong Uid
        {
            get { return _uid; }
            set { _uid = value; }
        }

        public Type OpType
        {
            get { return _type; }
            set { _type = value; }
        }

        public object ConstVal
        {
            get{return _constant;}
            set { _constant = value; }
        }
    }
}
