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

namespace Mp.Mod.OBD2.View
{
    public class OBD2ViewModule : Mp.Scheme.Sdk.Module
    {
        private Assembly _asm;
        public OBD2ViewModule()
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Mp.Visual.OBD2.dll");
            _asm = Assembly.LoadFile(path);

            
            ProcessStation station;

            base.Identifier = "OBD2 View Module";
            base.Type = "Module";
            base.ParentType = "WINDOWS";

            if (!Licence.IsRuntimeAvailable(1))
                return;

            station = new OBD2DTCViewPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

            station = new OBD2StatusViewPS();
            station.RuntimeEngine = base.Type;
            RegStation(station);

        }
    }
}
