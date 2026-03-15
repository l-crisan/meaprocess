using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Mp.Runtime.Scripting
{
    [Guid("74DCF11F-0C1E-4843-8D15-861CA58B1A41")]
    public interface IMeaProcess
    {
        bool OpenScheme(string scheme, int port);
        void Close();
        void Start();
        void Stop();
        void Reinitialize();

        bool Visible
        {
            get;
            set;
        }

        string Messages
        {
            get;
        }

        int InputSignals
        {
            get;
        }

        int OutputSignals
        {
            get;
        }
        
        ISignal GetSignal(int index, bool input);
    }
}
