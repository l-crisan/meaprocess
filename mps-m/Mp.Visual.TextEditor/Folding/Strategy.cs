using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using ICSharpCode.TextEditor.Document;

namespace Mp.Visual.TextEditor.Folding
{ 
    public class Strategy : IFoldingStrategy
    {
        private Dictionary<string, string> _foldingData = new Dictionary<string, string>();
        private char[] _wordDelimiter  = new char[] { ' ', '\t', ';', '\r', '\n' }; 

        public Strategy(string path)
        {
            LoadFolding(path);
        }

        public char[] WordDelimiter
        {
            get
            {
                return _wordDelimiter;
            }

            set
            {
                _wordDelimiter = value;
            }
        }

        private void LoadFolding(string path)
        {
            try
            {             
                XmlSerializer ser = new XmlSerializer(typeof(List<Item>));

                using (TextReader reader = new StreamReader(path))
                {
                    List<Item> data = (List < Item >)ser.Deserialize(reader);

                    foreach (Item item in data)
                        _foldingData.Add(item.From, item.To);

                    reader.Close();
                }
            }
            catch (Exception)
            { }
        }


        public List<FoldMarker> GenerateFoldMarkers(IDocument document, string fileName, object parseInformation)
        {
            List<FoldMarker>       list = new List<FoldMarker>();
            List<StackItem> stack = new List<StackItem>();            

            for( int i = 0; i < document.TotalNumberOfLines; i++ )
            {
                string text = document.GetText(document.GetLineSegment(i));
                text = text.TrimStart(_wordDelimiter);
                text = text.Split(_wordDelimiter)[0];

                if( _foldingData.ContainsKey(text) )
                    stack.Add(new StackItem(_foldingData[text], i ));

                if (stack.Count != 0 && stack[stack.Count - 1].EndText == text)
                {
                    int startIdx = stack[stack.Count - 1].StartLine;
                    stack.RemoveAt(stack.Count - 1);
                    
                    list.Add(new FoldMarker(document, startIdx, document.GetLineSegment(startIdx).Length, i, 7));                    
                }
            }

            return list;
        }
    }
}
