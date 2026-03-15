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
using System.Collections;
using System.Globalization;
using System.Text;
using System.Xml;
using System.IO;
using Mpal.Model;
using System.Drawing;
using Mpal.Debugger;
namespace Mpal.Editor
{
    internal class VarTreeItem
    {
        private VariableInfo _varInfo;

        public VarTreeItem(VariableInfo varInfo)
        {
            _varInfo = varInfo;
        }

        public VariableInfo VarInfo
        {
            get { return _varInfo; }
        }

        public Bitmap Icon
        {
            get
            {
                switch (_varInfo.DataType)
                {
                    case DataType.ARRAY:
                        return Mpal.Editor.Resource.Array.ToBitmap();
                    case DataType.STRUCT:
                    case DataType.UDT:
                    case DataType.FB:
                        return Mpal.Editor.Resource.Struct.ToBitmap();
                    default:
                        return Mpal.Editor.Resource.Variable.ToBitmap();
                }
            }
        }

        public string Name
        {
            get { return _varInfo.Name; }
        }

        public string Type
        {
            get { return _varInfo.DataTypeName; }
        }

    
        public string Value
        {
            set 
            { }

            get 
            {
                return _varInfo.Value;
            }
        }        
    }
}
