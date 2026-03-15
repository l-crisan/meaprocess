using System;
using System.Collections.Generic;
using System.Text;

namespace Mp.Drv.CAN
{
    public class DriverItem
    {
        private string _name;
        private string _lib;

        public DriverItem(string name, string lib)
        {
            _name = name;
            _lib = lib;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Lib
        {
            get { return _lib; }
        }

        public override string ToString()
        {
            return _name;
        }
        
    }
}
