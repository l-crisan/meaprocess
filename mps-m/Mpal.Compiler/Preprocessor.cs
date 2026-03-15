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
using System.IO;
using Mpal.Model;

namespace Mpal.Compiler
{
    internal class Preprocessor
    {
        public Preprocessor()
        {
        }

        public void ScanFile(string file, Unit unit)
        {
            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string text = sr.ReadToEnd();
            ScanText(text, unit);
        }

        public void ScanText(string text, Unit unit)
        {
            char ch;
            char nextCh;
            bool commentBegin = false;
            string descrp = "";

            for( int i = 0; i < text.Length - 1; ++i)
            {
                ch = text[i];
                nextCh = text[i+1];

                if( ch == '(' &&  nextCh == '*')
                    commentBegin = true;

                if( commentBegin )
                    descrp += ch;

                if (commentBegin && ch == '*' && nextCh == ')')
                {
                    descrp = descrp.TrimStart('(');
                    descrp = descrp.TrimStart('*');
                    descrp = descrp.TrimEnd(')');
                    descrp = descrp.TrimEnd('*');

                    unit.Description = descrp;
                    break;
                }
            }

        }
    }
}
