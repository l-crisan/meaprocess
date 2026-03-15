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

namespace Mpal.Compiler
{
    internal class CaseDim2Item : IComparer<CaseDim2Item>
    {
        private Dimension _dim;
        private CaseItem _item;
        private long _from;

        public CaseDim2Item()
        {
        }

        public Dimension Dim
        {
            get { return _dim; }
            set { _dim = value; }
        }

        public CaseItem Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public long From
        {
            get { return _from; }
            set { _from = value; }
        }

        public int Compare(CaseDim2Item a, CaseDim2Item b)
        {
            if (a.From == b.From)
                return 0;

            if (a.From < b.From)
                return -1;

            return 1;
        }

    }
}
