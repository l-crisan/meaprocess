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
using System.Threading;

namespace Mp.Runtime.App
{
    public static class NamedEvent
    {

        public static EventWaitHandle OpenOrCreate(string name, bool initialState, EventResetMode mode)
        {
            EventWaitHandle ewh = null;

            try
            {
                ewh = EventWaitHandle.OpenExisting(name);
            }

            catch (WaitHandleCannotBeOpenedException)
            {
                //Handle does not exist, create it.
                ewh = new EventWaitHandle(initialState, mode, name);
            }
            return ewh;
        }


        public static EventWaitHandle OpenOrWait(string name)
        {

            EventWaitHandle ewh = null;

            while (null == ewh)
            {

                try
                {
                    ewh = EventWaitHandle.OpenExisting(name);
                }

                catch (WaitHandleCannotBeOpenedException)
                {
                    Thread.Sleep(50);
                }
            }
            return ewh;
        }
    }
}
