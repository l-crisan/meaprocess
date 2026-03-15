using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Drv.CAN
{
    public class DeviceItem
    {
        public DeviceItem(string name, string id)
        {
            Name = name;
            ID = id;
        }

        public string Name;
        public string ID;

        public override string ToString()
        {
            return Name;
        }
    }
}
