using System;
using System.Collections.Generic;
using System.Text;
using Mp.Scheme.Sdk;

namespace Mp.Mod.CAN
{
    public class CANModule : Mp.Scheme.Sdk.Module
    {
        public CANModule()
        {
            ProcessStation station;

            base.Identifier = "CAN Module";
            base.Type = "Module";
            base.ParentType = "General";
            this.SupportWindows = true;
            this.SupportLinux = true;

            if (!Licence.IsRuntimeAvailable(1))
                return;

            //CAN signal in/out
            station = new CANInputPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new CANOutputPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            //Event out
            station = new CANEventPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            //CAN Read/Write
            station = new CANLoggerPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new CANWritePS();
            station.RuntimeEngine = base.Type;
            RegStation(station);
        }
    }
}
