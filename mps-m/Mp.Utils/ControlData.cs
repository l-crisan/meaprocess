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


namespace Mp.Utils
{
    /// <summary>
    /// This encapsulate the visual process station control data.
    /// </summary>
    /// <remarks>
    /// Each control attached to a process station must have a instance
    /// of this object atached to the Tag parameter.
    /// This object is used be the serialization API of the process station.
    /// </remarks>
    [Serializable]
    public class ControlData
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ControlData()
        { }

        /// <summary>
        /// Construct and object with the given property filter.
        /// </summary>
        /// <param name="propFilter"></param>
        public ControlData(List<string> propFilter)
        { PropertyFilter = propFilter; }
        

        /// <summary>
        /// The control persistence propertie filter.
        /// </summary>
        public List<string> PropertyFilter = new List<string>();

        /// <summary>
        /// The station identifier on which the control is attached.
        /// </summary>
        public uint StationId;

        /// <summary>
        /// The signal id associated with this control.
        /// </summary>
        public uint SignalId;

        /// <summary>
        /// The signal list identifier which the control handle.
        /// </summary>
        public uint SignalListId;

        /// <summary>
        /// The process station type of the control.
        /// </summary>
        public string ProcessStationLibrary;

        /// <summary>
        /// The process station type of the control.
        /// </summary>
        public string ProcessStationType;

        /// <summary>
        /// The control state can be saved in this string.
        /// </summary>
        public string ControlState;
    }
}
