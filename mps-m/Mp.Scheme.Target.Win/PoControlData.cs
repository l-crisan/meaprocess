//Copyright(C)2006, Laurentiu-Gheorghe Crisan, All rights reserved.
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml;

namespace Mp.Ide.Sdk
{
    [Serializable]
    public class PoControlData
    {
        public PoControlData(ArrayList propFilter)
        { PropertyFilter = propFilter; }
        
        public PoControlData()
        { }

        public ArrayList    PropertyFilter;
        public ulong        StationId;
    }
}
