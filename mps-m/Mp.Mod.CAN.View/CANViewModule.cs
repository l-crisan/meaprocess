using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Collections;
using Mp.Scheme.Sdk;

namespace Mp.Mod.CAN.View
{
    public class CANViewModule : Mp.Scheme.Sdk.Module
    {

        public CANViewModule()
        {
            ProcessStation station;

            base.Identifier = "CAN View Module";
            base.Type = "Module";
            base.ParentType = "WINDOWS";
            base.SupportWindows = true;

            if (!Licence.IsRuntimeAvailable(1))
                return;

            station = new CANViewPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);
        }
    }
}
