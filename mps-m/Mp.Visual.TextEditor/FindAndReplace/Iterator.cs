//Copyright (C) 2010-2016 ATESiON GmbH. All rights reserved.
using System;
using ICSharpCode.TextEditor.Document;

namespace Mp.Visual.TextEditor.FindAndReplace
{
  internal class Iterator
  {
    private enum State
    {
      Resetted,
      Iterating,
      Done,
    }

    private State _state;
    private ITextBufferStrategy _strategy;
    private int _endOffset = 0;
    private int _oldOffset = -1;
    private int _position;

    public Iterator(ITextBufferStrategy startegy)
    {
      _strategy = startegy;
      Reset();
    }

    public ITextBufferStrategy Strategy
    {
      get { return _strategy; }
    }

    public char Current
    {
      get
      {
        if (_state == State.Iterating)
          return _strategy.GetCharAt(Position);

        throw new Exception();
      }
    }

    public int Position
    {
      get { return _position; }
      set { _position = value; }
    }

    public bool MoveAhead(int numChars)
    {
      switch (_state)
      {
        case State.Resetted:
          if (_strategy.Length == 0)
          {
            _state = State.Done;
            return false;
          }
          Position = _endOffset;
          _state = State.Iterating;
          return true;
        case State.Done:
          return false;
        case State.Iterating:
          if (_oldOffset == -1 && _strategy.Length == _endOffset)
            Position--;

          if (_oldOffset != -1 && Position == _endOffset - 1 && _strategy.Length == _endOffset)
          {
            _state = State.Done;
            return false;
          }

          Position = (Position + numChars) % _strategy.Length;
          bool finish = _oldOffset != -1 && (_oldOffset > Position || _oldOffset < _endOffset) && Position >= _endOffset;

          if (_oldOffset != -1 && _oldOffset == _endOffset - 1 && _strategy.Length == _endOffset)
          {
            finish = true;
          }

          _oldOffset = Position;
          if (finish)
          {
            _state = State.Done;
            return false;
          }
          return true;
        default:
          throw new Exception();
      }
    }

    public void Reset()
    {
      _state = State.Resetted;
      Position = _endOffset;
      _oldOffset = -1;
    }

    public override string ToString()
    {
      return "Iterator";
    }
  }
}