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

namespace Mp.Runtime.Adaption
{
    public class Message
    {
        public enum Target
        {
            Output,
            Event,
            Status,
            Modal,
            LogFile,
            Trace,
            File,
            System
        }

        public enum MessageType
        {
            Info = 0,
            Warning = 1,
            Error = 2,
            Question = 3,
            Stop = 4,
            EventMsg = 5,
            QuestionFile = 6
        }

	    public string		Text;
        public string Comment;
        public string FileName;
        public Target TargetType;
        public MessageType Type;
        public DateTime TimeStamp;
    }
}
