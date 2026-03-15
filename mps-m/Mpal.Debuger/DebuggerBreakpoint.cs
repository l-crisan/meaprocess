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

namespace Mpal.Debugger
{
    public class DebuggerBreakpoint
    {
        private int _line;
        private string _unit;

        public DebuggerBreakpoint(int line, string unit)
        {
            _unit = unit;
            _line = line;
        }
        public int Line
        {
            get { return _line; }
            set { _line = value; }
        }

        public string Unit
        {
            get { return _unit; }
            set { _unit = value; }
        }

    }
}
