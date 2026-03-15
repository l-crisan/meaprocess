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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using Mpal.Model;

namespace Mpal.Editor
{
    internal class CodeCompletionProvider : ICompletionDataProvider
    {
        private ImageList _imageList;
        private Unit _unit;


        public CodeCompletionProvider(ImageList imageList)
        {
            _imageList = imageList;
        }

        public Unit Unit
        {
            set{ _unit = value;}
        }

        public ImageList ImageList
        {
            get
            {
                return _imageList;
            }
        }

        public string PreSelection
        {
            get
            {
                return null;
            }
        }

        public int DefaultIndex
        {
            get
            {
                return -1;
            }
        }

        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (char.IsLetterOrDigit(key) || key == '_')
                return CompletionDataProviderKeyResult.NormalKey;
            else
                return CompletionDataProviderKeyResult.InsertionKey;
        }

        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            textArea.Caret.Position = textArea.Document.OffsetToPosition(insertionOffset - 1);
            return data.InsertAction(textArea, key);
        }

        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            if (textArea.Document.TextLength == 0)
                return null;

            int line   = textArea.Caret.Line;
            int col    = textArea.Caret.Column - 1;
            int offset = textArea.Caret.Offset;

            if ( 0 == offset)
                return null;

            if (textArea.Document.TextLength == offset)
                return null;

            List<ICompletionData> resultList = new List<ICompletionData>();
            
            string preText = "";
            int from = col;
            for (from = col; from > 0; --from)
            {
                int curOffset = textArea.Document.PositionToOffset(new TextLocation(from, line));
                char curChar = textArea.Document.GetCharAt(curOffset);
                if (curChar == ' ' || curChar == '\t')
                {
                    from++;
                    break;
                }
            }

            int textOffset = textArea.Document.PositionToOffset(new TextLocation(from, line));
            preText = textArea.Document.GetText(textOffset, (col - from) + 1);
            preText = preText.TrimStart(' ');
            preText = preText.TrimStart('\t');


            char ch = textArea.Document.GetCharAt(offset - 1);
            switch (ch)
            {
                case '.':
                {
                    preText = preText.TrimEnd('.');
                    string[] keys = preText.Split('.');

                    UpdateCodeCompletationData(keys, resultList, line);
                }
                break;

                case ':':
                {
                    resultList.Add(new CodeCompletionData("SINT", " SINT", "Signed 1 byte integer", 0));
                    resultList.Add(new CodeCompletionData("USINT", " USINT", "Unsigned 1 byte integer", 0));
                    resultList.Add(new CodeCompletionData("INT", " INT", "Signed 2 byte integer", 0));
                    resultList.Add(new CodeCompletionData("UINT", " UINT", "Unsigned 2 byte integer", 0));
                    resultList.Add(new CodeCompletionData("DINT", " DINT", "Signed 4 byte integer", 0));
                    resultList.Add(new CodeCompletionData("UDINT", " UDINT", "Unsigned 4 byte integer", 0));
                    resultList.Add(new CodeCompletionData("LINT", " LINT", "Signed 8 byte integer", 0));
                    resultList.Add(new CodeCompletionData("ULINT", " ULINT", "Unsigned 8 byte integer", 0));
                    resultList.Add(new CodeCompletionData("BOOL", " BOOL", "Boolean", 0));
                    resultList.Add(new CodeCompletionData("REAL", " REAL", "4 byte real number", 0));
                    resultList.Add(new CodeCompletionData("LREAL", " LREAL", "8 byte real number", 0));
                    resultList.Add(new CodeCompletionData("BYTE", " BYTE", "8 bit data type", 0));
                    resultList.Add(new CodeCompletionData("WORD", " WORD", "16 bit data type", 0));
                    resultList.Add(new CodeCompletionData("DWORD", " DWORD", "32 bit data type", 0));
                    resultList.Add(new CodeCompletionData("LWORD", " LWORD", "64 bit data type", 0));
                    resultList.Add(new CodeCompletionData("ARRAY", " ARRAY [0..2] OF ", "Array data type", 1));
                    resultList.Add(new CodeCompletionData("STRUCT", " STRUCT \n", "Struct data type", 1));
                }
                break;
                default:
                {
                }
                break;
            }
      
            return resultList.ToArray();
        }

        private void UpdateParameterCompletation(List<ICompletionData> resultList, Parameter param, string[] keys, int level, int line)
        {
            if (keys.Length == level)
            {
                int index = 0;

                if (param.ParamDataType == DataType.STRUCT ||
                    param.ParamDataType == DataType.ARRAY ||
                    param.ParamDataType == DataType.UDT)
                    index = 1;

                DefaultCompletionData item = new DefaultCompletionData(param.Name, param.ParamDataType.ToString(),  index);
                resultList.Add(item);
                return;
            }

            if (param.Name != keys[level])
                return;

            level++;

            foreach (Parameter childParam in param.Structure)
                UpdateParameterCompletation(resultList, childParam, keys, level, line);

            if (param.ParamDataType == DataType.UDT)
            {
                if (_unit.Types.ContainsKey(param.TypeName))
                {
                    Parameter udtParam = (Parameter)_unit.Types[param.TypeName];
                    foreach (Parameter childParam in udtParam.Structure)
                        UpdateParameterCompletation(resultList, childParam, keys, level, line);

                }
            }
        }
                
        private void UpdateCodeCompletationData(string[] keys, List<ICompletionData> resultList, int line)
        {
            if (_unit == null)
                return;

            foreach (DictionaryEntry entry in _unit.Functions)
            {
                Function function = (Function)entry.Value;

                if(line >=  function.LineBegin && line < function.LineEnd)
                    foreach (Parameter param in function.Parameters)
                        UpdateParameterCompletation(resultList, param, keys, 0, line);
            }
        }
    }
}
