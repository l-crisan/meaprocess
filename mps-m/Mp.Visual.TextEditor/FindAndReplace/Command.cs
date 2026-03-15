//Copyright (C) 2010-2016 ATESiON GmbH. All rights reserved.
using System;


namespace Mp.Visual.TextEditor.FindAndReplace
{
    internal class Command
    {
        public enum CmdType
        {
            Match,
            AnyZeroOrMore,
            AnySingle,
            AnyDigit,
            AnyInList,
            NoneInList
        }

        public CmdType CommandType = CmdType.Match;
        public char SingleChar = '\0';
        public string CharList = String.Empty;
    }
}
