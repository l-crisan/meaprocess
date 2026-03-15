using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Mp.Runtime.Scripting
{
    [Guid("AE1311E9-8BA8-40D1-8B85-D2DAB45E2C86")]
    public interface ISignal
    {
        string Name
        {
            get;
        }

        string Unit
        {
            get;
        }

        double Value
        {
            get;
            set;
        }

        double Minimum
        {
            get;
        }

        double Maximum
        {
            get;
        }
    }
}
