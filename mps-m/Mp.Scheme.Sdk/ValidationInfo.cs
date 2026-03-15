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

namespace Mp.Scheme.Sdk
{
    /// <summary>
    /// This class implemets a validation result item.
    /// </summary>
    public class ValidationInfo
    {
        /// <summary>
        /// The validation info type.
        /// </summary>
        public enum InfoType
        {
            /// <summary>
            /// The item has only a message.
            /// </summary>
            Valid,

            /// <summary>
            /// The item has only an information message.
            /// </summary>
            Info,

            /// <summary>
            /// The item has a warning message.
            /// </summary>
            Warning,

            /// <summary>
            /// The item has an error message.
            /// </summary>
            Error,
        }
        /// <summary>
        /// Constructs a new ValidationInfo object.
        /// </summary>
        /// <param name="msg">The message of validation item.</param>
        /// <param name="infoType">The info type.</param>
        public ValidationInfo( string msg, InfoType infoType)
        {
            _message = msg;
            _infoType = infoType;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Gets the type of the validation info.
        /// </summary>
        public InfoType Type
        {
            get { return _infoType; }
        }

        private string _message;
        private InfoType _infoType;
    }
}
