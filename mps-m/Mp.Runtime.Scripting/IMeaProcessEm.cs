using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Mp.Runtime.Scripting
{
    [Guid("BFA36FF1-7FC6-4962-B228-0B7D3881ED39")]
    public interface IMeaProcessEm
    {
        bool OpenScheme(string scheme);
        bool Start();
        bool Stop();
        bool Reinitialize();

        string ServerIP
        {
            get;
            set;
        }

        int Port
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
