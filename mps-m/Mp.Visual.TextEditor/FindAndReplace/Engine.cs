//Copyright (C) 2010-2016 ATESiON GmbH. All rights reserved.
using System;
using System.Collections.Generic;
using ICSharpCode.TextEditor.Document;
using ICSharpCode.TextEditor;
using System.IO;

namespace Mp.Visual.TextEditor.FindAndReplace
{

  public class Engine
  {
    public enum FindInto
    {
      CurrentDocument,
      AllOpenDocuments,
      Project,
    }

    private Iterator _iterator;
    private ICSharpCode.TextEditor.TextEditorControl _currentDocument;
    private int _resultOffset;
    private int _resultLength;
    private List<Command> _patternProgram = null;
    private int _curMatchEndOffset = -1;
    private bool _matchCase = false;
    private bool _matchWholeWord = false;
    private List<string> _fileList = new List<string>();
    private FindInto _findIn = FindInto.CurrentDocument;
    public delegate void FindResult(string msg);
    public event FindResult FindResultEvent;
    public event FindResult EnsureVisibleEvent;
    public List<ICSharpCode.TextEditor.TextEditorControl> _openDocuments = new List<TextEditorControl>();

    public Engine()
    {

    }

    public FindInto FindIn
    {
      get { return _findIn; }
      set { _findIn = value; }
    }

    public List<ICSharpCode.TextEditor.TextEditorControl> OpenDocuments
    {
      get
      {
        return _openDocuments;
      }

      set { _openDocuments = value; }
    }

    public List<string> FileList
    {
      get
      {
        return _fileList;
      }
      set 
      { 
        _fileList = value; 
      }
    }

    public ICSharpCode.TextEditor.TextEditorControl CurrentDocument
    {
      set
      {
        _currentDocument = value;

        if (_currentDocument == null)
          return;

        _iterator = new Iterator(_currentDocument.Document.TextBufferStrategy);
        _iterator.Reset();
      }
    }

    public void Reset()
    {
      if (_currentDocument != null)
        _iterator.Reset();
    }

    public bool Selected
    {
      get { return _resultOffset != -1; }
    }

    public void SelectSearchResult()
    {
      if (_currentDocument == null)
        return;

      TextLocation loc = _currentDocument.Document.OffsetToPosition(_resultOffset);
      _currentDocument.ActiveTextAreaControl.Caret.Line = loc.Line;
      _currentDocument.ActiveTextAreaControl.Caret.Column = loc.Column + _resultLength;

      DefaultSelection defSel = new DefaultSelection(_currentDocument.Document, loc, new TextLocation(loc.Column + _resultLength, loc.Line));
      
      _currentDocument.ActiveTextAreaControl.SelectionManager.SetSelection(defSel, false);
    }

    public void Replace(string with)
    {
        if (_currentDocument == null)
          return;

        _currentDocument.Document.Replace(_resultOffset, _resultLength, with);
        TextLocation loc = _currentDocument.Document.OffsetToPosition(_resultOffset);

        DefaultSelection defSel = new DefaultSelection(_currentDocument.Document, new TextLocation(), new TextLocation());
        _currentDocument.ActiveTextAreaControl.SelectionManager.SetSelection(defSel, false);

        _currentDocument.ActiveTextAreaControl.Caret.Line = loc.Line;
        _currentDocument.ActiveTextAreaControl.Caret.Column = loc.Column + with.Length;
        _resultOffset = -1;
    }


    private bool Next(Iterator iterator)
    {     
      _resultOffset = FindNext(iterator);
      _resultLength = _curMatchEndOffset - _resultOffset;

      return _resultOffset != -1;
    }

    public bool FindNext(string what, bool matchCase, bool matchWholeWord)
    {
      _matchCase = matchCase;
      _matchWholeWord = matchWholeWord;
      SetPattern(what, !matchCase);

      switch (_findIn)
      {
        case FindInto.CurrentDocument:
        {

          if (_currentDocument == null)
            return false;

          return Next(_iterator);
        }

        case FindInto.AllOpenDocuments:
          {
            if (_currentDocument == null || _openDocuments.Count == 0)
              return false;

            int index = _openDocuments.IndexOf(_currentDocument);

            if (index < 0)
              return false;
            bool result = Next(_iterator);

            if (!result)
            {
              index++;

              if (index >= _openDocuments.Count)
                index = 0;

              _currentDocument = _openDocuments[index];
              EnsureVisibleEvent(_currentDocument.FileName);
              _iterator = new Iterator(_currentDocument.Document.TextBufferStrategy);
              _iterator.Reset();

              result =  Next(_iterator);
            }

            if( result )
              SelectSearchResult();

            return result;
          }
          

        case FindInto.Project:
          break;
      }
      return false;
    }


    public void ReplaceAll(string what, string with,  bool matchCase, bool matchWholeWord)
    {
      _matchCase = matchCase;
      _matchWholeWord = matchWholeWord;
      SetPattern(what, true);
      int count = 0;

      if (FindResultEvent == null)
        return;

      switch (_findIn)
      {
        case FindInto.CurrentDocument:
          if( _currentDocument != null)
            count = ReplaceAll(_iterator, with);
         break;

        case FindInto.AllOpenDocuments:
        {
            foreach (TextEditorControl ctrl in _openDocuments)
            {
              _currentDocument = ctrl;
              EnsureVisibleEvent(ctrl.FileName);
              Iterator iterator = new Iterator(ctrl.Document.TextBufferStrategy);
              count += ReplaceAll(iterator, with);
            }
        }
        break;

        case FindInto.Project:          
          break;
      }

      FindResultEvent(String.Format(StringResource.ReplaceMsg, count));
    }

    public void FindAll(string what, bool matchCase, bool matchWholeWord)
    {
      _matchCase = matchCase;
      _matchWholeWord = matchWholeWord;
      SetPattern(what, true);

      if (FindResultEvent == null)
        return;

      switch (_findIn)
      {
        case FindInto.CurrentDocument:
          FindCurrentDoc();
          break;

        case FindInto.AllOpenDocuments:
          FindOpenDocs();
          break;

        case FindInto.Project:
          FindProject();
          break;
      }
    }

    private void FindOpenDocs()
    {
      int count = 0;
      foreach (TextEditorControl ctrl in _openDocuments)
      {
        _iterator = new Iterator(ctrl.Document.TextBufferStrategy);
        _iterator.Reset();
        count += FindAll(_iterator, ctrl.FileName);
      }

      FindResultEvent(String.Format(StringResource.FindTxt, count));

      if (_currentDocument != null)
      {
        _iterator = new Iterator(_currentDocument.Document.TextBufferStrategy);
        _iterator.Reset();
      }
    }

    public void Find(string what)
    {
      switch (_findIn)
      {
        case FindInto.CurrentDocument:
          {
            if (_currentDocument == null)
              return;

            SetPattern(what, true);

            _resultOffset = FindNext(_iterator);
            _resultLength = _curMatchEndOffset - _resultOffset;

            if (_resultOffset == -1)
            {
              _iterator.Reset();
              return;
            }


            SelectSearchResult();
          }
          break;

        case FindInto.AllOpenDocuments:
          {
            if (_openDocuments.Count == 0)
              return;

          }
          break;
      }
    }

    private void FindCurrentDoc()
    {

      if (_currentDocument == null)
        return;

      int count = FindAll(_iterator, _currentDocument.FileName);
      FindResultEvent(String.Format(StringResource.FindTxt, count));
    }

    private void FindProject()
    {
      int count = 0;
      foreach (string file in _fileList)
      {
        try
        {
          StringBufferStrategy strategy = new StringBufferStrategy(File.ReadAllText(file));
          Iterator it = new Iterator(strategy);
          count += FindAll(it, file);
        }
        catch (Exception)
        {

        }
      }

      FindResultEvent(String.Format(StringResource.FindTxt, count));
    }

    private int ReplaceAll(Iterator it, string with)
    {
      int count = 0;
      it.Reset();
      _resultOffset = FindNext(it);
      _resultLength = _curMatchEndOffset - _resultOffset;

      while (_resultOffset != -1)
      {
        count++;

        Replace(with);

        _resultOffset = FindNext(it);
        _resultLength = _curMatchEndOffset - _resultOffset;
      }
      return count;
    }

    private int FindAll(Iterator it, string file)
    {
      int count = 0;
      it.Reset();
      int pos = FindNext(it);

      while (pos != -1)
      {
        count++;
        string info = "";
        try
        {

          int begin = pos;
          int lenght = pos + 200 > it.Strategy.Length ? it.Strategy.Length - 1 - begin : 200;

          info = it.Strategy.GetText(begin, lenght);
          info = info.Replace('\n', ' ');
          info = info.Replace('\r', ' ');
        }
        catch (Exception)
        { }

        FindResultEvent(file + "; (" + pos.ToString() + "): " + info);
        pos = FindNext(it);
      }
      return count;
    }

    public bool SetPattern(string pattern, bool ignoreCase)
    {
      if (ignoreCase)
        pattern = pattern.ToUpper();

      _patternProgram = new List<Command>();
      for (int i = 0; i < pattern.Length; ++i)
      {
        Command newCommand = new Command();
        switch (pattern[i])
        {
          case '#':
            newCommand.CommandType = Command.CmdType.AnyDigit;
            break;
          case '*':
            newCommand.CommandType = Command.CmdType.AnyZeroOrMore;
            break;
          case '?':
            newCommand.CommandType = Command.CmdType.AnySingle;
            break;
          case '[':
            int index = pattern.IndexOf(']', i);
            if (index > 0)
            {
              newCommand.CommandType = Command.CmdType.AnyInList;
              string list = pattern.Substring(i + 1, index - i - 1);
              if (list[0] == '!')
              {
                newCommand.CommandType = Command.CmdType.NoneInList;
                list = list.Substring(1);
              }
              newCommand.CharList = ignoreCase ? list.ToUpper() : list;
              i = index;
            }
            else
            {
              goto default;
            }
            break;
          default:
            newCommand.CommandType = Command.CmdType.Match;
            newCommand.SingleChar = ignoreCase ? Char.ToUpper(pattern[i]) : pattern[i];
            break;
        }
        _patternProgram.Add(newCommand);
      }

      return true;
    }

    private bool Match(ITextBufferStrategy document, int offset, bool ignoreCase, int programStart)
    {
      int curOffset = offset;
      _curMatchEndOffset = -1;

      for (int pc = programStart; pc < _patternProgram.Count; ++pc)
      {
        if (curOffset >= document.Length)
        {
          return false;
        }

        char ch = ignoreCase ? Char.ToUpper(document.GetCharAt(curOffset)) : document.GetCharAt(curOffset);
        Command cmd = (Command)_patternProgram[pc];

        switch (cmd.CommandType)
        {
          case Command.CmdType.Match:
            if (ch != cmd.SingleChar)
            {
              return false;
            }
            break;
          case Command.CmdType.AnyZeroOrMore:
            if (ch == '\n')
            {
              return false;
            }
            return Match(document, curOffset, ignoreCase, pc + 1) ||
                   Match(document, curOffset + 1, ignoreCase, pc);
          case Command.CmdType.AnySingle:
            break;
          case Command.CmdType.AnyDigit:
            if (!Char.IsDigit(ch) && ch != '#')
            {
              return false;
            }
            break;
          case Command.CmdType.AnyInList:
            if (cmd.CharList.IndexOf(ch) < 0)
            {
              return false;
            }
            break;
          case Command.CmdType.NoneInList:
            if (cmd.CharList.IndexOf(ch) >= 0)
            {
              return false;
            }
            break;
        }
        ++curOffset;
      }
      _curMatchEndOffset = curOffset;
      return true;
    }

    private bool WholeWord(ITextBufferStrategy document, int offset, int length)
    {
      return (offset - 1 < 0 || Char.IsWhiteSpace(document.GetCharAt(offset - 1))) &&
          (offset + length + 1 >= document.Length || Char.IsWhiteSpace(document.GetCharAt(offset + length)));
    }

    private int FindNext(Iterator textIterator)
    {
      while (textIterator.MoveAhead(1))
      {
        int position = textIterator.Position;
        if (Match(textIterator.Strategy, position, !_matchCase, 0))
        {
          if (!_matchWholeWord || WholeWord(textIterator.Strategy, position, _curMatchEndOffset - position))
          {
            textIterator.MoveAhead(_curMatchEndOffset - position - 1);
            return position;
          }
        }
      }
      return -1;
    }
  }
}
