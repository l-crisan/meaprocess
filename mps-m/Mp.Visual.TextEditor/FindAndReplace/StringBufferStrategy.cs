//Copyright (C) 2010-2016 ATESiON GmbH. All rights reserved.
using System.Text;
using ICSharpCode.TextEditor.Document;

namespace Mp.Visual.TextEditor.FindAndReplace
{
  internal class StringBufferStrategy : ITextBufferStrategy
  {
    private StringBuilder _buffer = new StringBuilder();

    public StringBufferStrategy()
    {
    }

    public StringBufferStrategy(string text)
    {
      SetContent(text);
    }


    public int Length
    {
      get { return (int)_buffer.Length; }
    }


    public void Insert(int offset, string text)
    {
      _buffer.Insert(offset, text);
    }


    public void Remove(int offset, int length)
    {
      _buffer.Remove(offset, length);
    }


    public void Replace(int offset, int length, string text)
    {
      _buffer.Remove(offset, length);
      _buffer.Insert(offset, text);
    }

    public string GetText(int offset, int length)
    {
      return _buffer.ToString(offset, length);
    }


    public char GetCharAt(int offset)
    {
      return _buffer.ToString(offset, 1)[0];
    }


    public void SetContent(string text)
    {
      _buffer = new StringBuilder(text);
    }
  }
}
