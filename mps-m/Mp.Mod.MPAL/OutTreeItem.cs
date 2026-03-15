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
using System.Drawing;
using Mpal.Model;

namespace Mp.Mod.MPAL
{
    internal class MpalOutTreeItem
    {
        private string _signal = "";
        private decimal _factor = 1;
        private decimal _offset = 0;
        private uint   _sampleRate;
        private decimal _min;
        private decimal _max;
        private string _unit;
        private string _comment;
        private Parameter _param;
        private uint _sigID = 0;
        private string _path;

        public MpalOutTreeItem(Parameter param)
        {
            _param = param;
        }

        public string Path
        {
            set { _path = value; }
            get { return _path; }
        }

        public uint SignalID
        {
            set 
            {
                if (!CanAssign())
                    return;

                _sigID = value; 
            }
            get { return _sigID; }
        }

        public Bitmap Icon
        {
            get
            {
                switch (_param.ParamDataType)
                {
                    case DataType.ARRAY:
                    case DataType.STRUCT:
                    case DataType.UDT:
                    case DataType.FB:
                        return Images.Struct.ToBitmap();
                    default:
                        return Images.Property.ToBitmap();
                }
            }
        }

        public Parameter Param
        {
            get { return _param; }
        }

        public string Variable
        {
            get { return _param.Name; }
        }

        public string Type
        {
            get { return _param.ParamDataType.ToString(); }

        }

        private bool CanAssign()
        {
            if (_param.ParamDataType == DataType.UDT ||
                _param.ParamDataType == DataType.STRUCT ||
                _param.ParamDataType == DataType.UDT)
                return false;
            return true;
        }

        public string Signal
        {
            get { return _signal; }
            set 
            {
                if (!CanAssign())
                    return;

                _signal = value; 
            }
        }

        public decimal Factor
        {
            get { return _factor; }
            set 
            {
                if (!CanAssign())
                    return;

                _factor = value; 
            }
        }

        public decimal Offset
        {
            get { return _offset; }
            set 
            {
                if (!CanAssign())
                    return;

                _offset = value; 
            }
        }

        public uint SampleRate
        {
            get { return _sampleRate; }
            set 
            {
                if (!CanAssign())
                    return;

                _sampleRate = value; 
            }
        }

        public decimal Min
        {
            get { return _min; }
            set 
            {
                if (!CanAssign())
                    return;

                _min = value; 
            }
        }

        public decimal Max
        {
            get { return _max; }
            set 
            {
                if (!CanAssign())
                    return;

                _max = value; 
            }
        }

        public string Unit
        {
            get { return _unit; }
            set 
            {
                if (!CanAssign())
                    return;

                _unit = value; 
            }
        }

        public string Comment
        {
            get { return _comment; }
            set 
            {
                if (!CanAssign())
                    return;

                _comment = value; 
            }
        }
    }
}
