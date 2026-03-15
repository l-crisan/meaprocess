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
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Actions;

namespace Mpal.Editor
{
    internal class Folding : IFoldingStrategy
    {
        private void SearchAndAdd(string start, string end, List<FoldMarker> list, IDocument document)
        {
            int startIdx = -1;

            for (int i = 0; i < document.TotalNumberOfLines; i++)
            {
                string text = document.GetText(document.GetLineSegment(i));
                text = text.TrimStart(new char[]{' ','\t'});

                if (text.StartsWith(start))
                    startIdx = i;

                if (text.StartsWith(end) && startIdx != -1)
                {
                    list.Add(new FoldMarker(document, startIdx, document.GetLineSegment(startIdx).Length, i, 7));
                    startIdx = -1;
                }

            }
        }

        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation)
        {
            List<FoldMarker> list = new List<FoldMarker>();


            SearchAndAdd("/*", "*/", list, document);
            SearchAndAdd("(*", "*)", list, document);
            SearchAndAdd("VAR_INPUT", "END_VAR", list, document);
            SearchAndAdd("VAR_IN_OUT", "END_VAR", list, document);
            SearchAndAdd("VAR_OUTPUT", "END_VAR", list, document);
            SearchAndAdd("VAR", "END_VAR", list, document);
            SearchAndAdd("VAR_TEMP", "END_VAR", list, document);
            SearchAndAdd("PROGRAM", "END_PROGRAM", list, document);
            SearchAndAdd("FUNCTION", "END_FUNCTION", list, document);
            SearchAndAdd("FUNCTION_BLOCK", "END_FUNCTION_BLOCK", list, document);
            SearchAndAdd("TYPE", "END_TYPE", list, document);                       
            return list;
        }
    }
}
