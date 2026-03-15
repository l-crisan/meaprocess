using System;
using System.Collections.Generic;
using System.Text;
using Mp.Scheme.Sdk;

namespace Mp.Mod.OBD2
{
    public class OBD2Module : Mp.Scheme.Sdk.Module
    {
        public OBD2Module()
        {
            ProcessStation station;

            base.Identifier = "OBD2 Module";
            base.Type = "Module";
            base.ParentType = "General";
            this.SupportWindows = true;
            this.SupportLinux = true;

            if (!Licence.IsRuntimeAvailable(1))
                return;

            station = new OBD2InputPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);
        }
    }
}
